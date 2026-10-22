QuestGuru_CommFrame = CreateFrame("FRAME", "QuestGuru_CommFrame", UIParent);
QuestGuru_CommFrame:SetPoint("TOPLEFT");
QuestGuru_CommFrame:SetHeight(0);
QuestGuru_CommFrame:SetWidth(0);
QuestGuru_CommFrame:SetScript("OnUpdate",
	function (self, elapsed)
		QuestGuru_CommOnUpdate(elapsed);
	end);
QuestGuru_CommFrame:Show();

QuestGuru_CommGuild = {};
QuestGuru_CommParty = {};
-- [PlayerName].QGver = version of QuestGuru other player is using
--              quietTime = number of seconds since last message from player
--              [quest number].Title
--			.Status = status of player on that quest
-- 			.Objectives = objectives text
--                            .Description = description text
--                            .[Obj#].type = objective type
--			.text = objective text
--			.finished = objective finished?
--                            .[Reward#].type = reward type (choice, spell, reward)
--                            .link = reward link
--              MPmsg = storage for multi-part messages

QuestGuru_CommTimer = {};
-- .interval = number of seconds to wait
-- .currTime = number of seconds remaining
-- .func = function to call after interval
-- .count = number of times to repeat (-1 for infinite)
-- .args = options arguments to pass

QuestGuru_CommLastPartyNum = 0;
QuestGuru_CommPendingPartyUpdate = false;

function QuestGuru_CommOnUpdate(elapsed)
	local i, timer;
	
	for i, timer in ipairs(QuestGuru_CommTimer) do
		timer.currTime = timer.currTime - elapsed;
		if (timer.currTime <= 0) then
			timer.func(timer.arg1, timer.arg2, timer.arg3, timer.arg4);
			timer.currTime = timer.interval;
			timer.count = timer.count - 1;
			if (timer.count < 0) then
				timer.count = -1;
			elseif (timer.count == 0) then
				tremove(QuestGuru_CommTimer, i);
				i = i - 1;
			end
		end
	end
end

function QuestGuru_CommAddTimer(interval, count, func, arg1, arg2, arg3, arg4)
	local tempTimer = {}
	tempTimer.interval = interval;
	tempTimer.currTime = interval;
	tempTimer.count = count;
	tempTimer.func = func;
	tempTimer.arg1 = arg1;
	tempTimer.arg2 = arg2;
	tempTimer.arg3 = arg3;
	tempTimer.arg4 = arg4;
	tinsert(QuestGuru_CommTimer, tempTimer);
end

function QuestGuru_CommSendMessage(channel, code, ...)
	local i = 1;
	local recipient;
	
	if (channel == "WHISPER") then
		i = 2;
		recipient = select(1, ...);
	end
	if ((not QuestGuru_Settings.DisableComm) and not ((UnitInBattleground("player") ~= nil) and (channel == "PARTY"))) then
		ChatThrottleLib:SendAddonMessage("NORMAL", "QuGu", strjoin(string.char(2), code, ...), channel, recipient);
	end
end

function QuestGuru_CommSendPGMessage(...)
	if ((IsInGroup() and GetNumSubgroupMembers(LE_PARTY_CATEGORY_HOME) > 0) and (table.getn(QuestGuru_CommParty) > 0)) then
		QuestGuru_CommSendMessage("PARTY", ...);
	end
	if (IsInGuild()) then
		QuestGuru_CommSendMessage("GUILD", ...);
	end
end

function QuestGuru_CommSendMPMessage(channel, code, recipient, qNum, msg)
-- Sends a MultiPart message (for quest objective and description since they can exceed the per-message size limit)
-- These should always be WHISPER. Part "0" is last part, ready to process
-- MP messages use char(3) to not conflict with SendMessage

	local i=1;
	msg = qNum..string.char(3)..msg;
	while (strlen(msg) > 200) do
		local t = string.sub(msg, 1, 200);
		msg = string.sub(msg, 201);
		QuestGuru_CommSendMessage(channel, code, recipient, i, t)
		i = i + 1;
	end
	QuestGuru_CommSendMessage(channel, code, recipient, "0", msg)
end

function QuestGuru_CommProcessMsg(sender, channel, message)
	local msg = { strsplit(string.char(2), message) };
	if (msg == nil) then return; end
	local code = msg[1];
	local currSender;

	if (channel == "GUILD") then
		if (QuestGuru_CommGuild[sender] == nil) then
			QuestGuru_CommGuild[sender] = {};
		end
		currSender = QuestGuru_CommGuild[sender];
	elseif (channel == "PARTY") then
		if (QuestGuru_CommParty[sender] == nil) then
			QuestGuru_CommParty[sender] = {};
		end
		currSender = QuestGuru_CommParty[sender];
	else
		if (QuestGuru_CommGuild[sender]) then QuestGuru_CommGuild[sender].quietTime = 0; end
		if (QuestGuru_CommParty[sender]) then QuestGuru_CommParty[sender].quietTime = 0; end
		currSender = {};
	end
	currSender.quietTime = 0;

	if (code == "0000") then -- Player is sending their QG info
		-- store it
		currSender.QGver = msg[2];
QuestGuru_Echo(sender.." is using QuestGuru v"..msg[2]);
	elseif (code == "0001") then -- Player is sending QG info and requesting yours
		-- store their info and send ours
		currSender.QGver = msg[2];
QuestGuru_Echo(sender.." is using QuestGuru v"..msg[2]);
		if (sender ~= QuestGuru_Player) then QuestGuru_CommSendMessage(channel, "0000", QUESTGURU_VERSION); end
	elseif (code == "0008") then -- Pong
		-- not really necessary to do anything
	elseif (code == "0009") then -- Ping
		-- send a Pong
		QuestGuru_CommSendMessage("WHISPER", "0008", sender);
	elseif (code == "000F") then -- Player logged out
		currSender = nil;
QuestGuru_Echo(sender.." logged off");
	elseif (code == "0020") then -- Discovered NPC Name
		QuestGuru_MoreNPCNames[msg[2]] = 1;
	elseif (code == "0024") then -- Discovered Area Name
		QuestGuru_MoreAreaNames[msg[2]] = 1;
	elseif (code == "0108") then -- Player is on quest (linkNum, objStatus)
		if ((not msg[2]) or (not msg[3])) then return; end
		local qIDi, qIDj, qNum = string.find(msg[2], (":(%d+):"));
		if (qNum) then
			if (not currSender[qNum]) then currSender[qNum] = {}; end
			currSender[qNum].Status = msg[3];
		end
		QuestLog_Update();
	elseif (code == "0109") then -- Player is on quest, requesting who else is too
		if ((not msg[2]) or (not msg[3])) then return; end
		local qIDi, qIDj, qNum = string.find(msg[2], (":(%d+):"));
		if (qNum) then
			if (not currSender[qNum]) then currSender[qNum] = {}; end
			currSender[qNum].Status = msg[3];
		end
		local numEntries, numQuests = GetNumQuestLogEntries();
		local i, j;
		local qStatus = "";
		for i=1, numEntries do
			local qLink = GetQuestLink(i);
			if (qLink) then
				local qli, qlj, qlNum = string.find(qLink, (":(%d+):"));
				if (qlNum == qNum) then
					QuestGuru_SendQuestStatus(i, false);
				end
			end
		end
		QuestLog_Update();
	elseif (code == "010A") then -- Player completed quest
		currSender[msg[2]] = nil;
		QuestLog_Update();
	elseif (code == "010B") then -- Player completed quest, requesting who else has it
		-- not used at this time
	elseif (code == "010C") then -- Player abandoned quest
		currSender[msg[2]] = nil;
		QuestLog_Update();
	elseif (code == "010D") then -- Player abandoned quest, requesting who else has it
		-- not used at this time
	elseif (code == "0200") then -- qID = quest link
		QuestGuru_UpdateqID(msg[2], msg[3]);
	elseif (code == "0201") then -- Request quest link by qID
		if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]] ~= nil) then
			if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]]["_link"] ~= nil) then
				QuestGuru_CommSendMessage(channel, "0200", msg[2], QuestGuru_Quests[QuestGuru_RealmName][msg[2]]["_link"]);
			end
		end
	elseif (code == "0202") then -- Quest Title (qNum, Title)
		if (not currSender[msg[2]]) then currSender[msg[2]] = {}; end
		currSender[msg[2]].Title = msg[3];
	elseif (code == "0203") then -- Request quest title (qNum)
		if (not msg[2]) then return; end
		local qNum = msg[2];
		local numEntries, numQuests = GetNumQuestLogEntries();
		local i, j;
		local qStatus = "";
		for i=1, numEntries do
			local qLink = GetQuestLink(i);
			if (qLink) then
				local qli, qlj, qlNum = string.find(qLink, (":(%d+):"));
				if (qlNum == qNum) then
					local qTitle = GetQuestLogTitle(i);
					QuestGuru_CommSendMessage(channel, "0202", sender, qNum, qTitle);
				end
			end
		end
	elseif (code == "0204") then -- Objectives text (msg[2] = part, msg[3] = qNum..char(3)..text)
		if (tonumber(msg[2]) == 1) then currSender.MPmsg = nil; end
		currSender.MPmsg = currSender.MPmsg..msg[3];
		if (msg[2] == "0") then -- end of multi-part
			local qDesc = { strsplit(string.char(3), currSender.MPmsg) }; -- qDesc[1] = qNum, qDesc[2] = text
			if (not currSender[qDesc[1]]) then currSender[qDesc[1]] = {}; end
			currSender[qDesc[1]].Objectives = qDesc[2];
			currSender.MPmsg = nil;
		end
	elseif (code == "0205") then -- Request objectives text (qNum)
		if (not msg[2]) then return; end
		local qNum = msg[2];
		local numEntries, numQuests = GetNumQuestLogEntries();
		local i, j;
		local qStatus = "";
		for i=1, numEntries do
			local qLink = GetQuestLink(i);
			if (qLink) then
				local qli, qlj, qlNum = string.find(qLink, (":(%d+):"));
				if (qlNum == qNum) then
					QuestGuru_SendQuestObjectivesText(i, sender, qNum);
				end
			end
		end
	elseif (code == "0206") then -- Description text (msg[2] = part, msg[3] = qNum..char(3)..text)
		if (tonumber(msg[2]) < 2) then currSender.MPmsg = nil; end
		currSender.MPmsg = currSender.MPmsg..msg[3];
		if (msg[2] == "0") then -- end of multi-part
			local qDesc = { strsplit(string.char(3), currSender.MPmsg) }; -- qDesc[1] = qNum, qDesc[2] = text
			if (not currSender[qDesc[1]]) then currSender[qDesc[1]] = {}; end
			currSender[qDesc[1]].Description = qDesc[2];
		end
	elseif (code == "0207") then -- Request description text (qNum)
		if (not msg[2]) then return; end
		local qNum = msg[2];
		local numEntries, numQuests = GetNumQuestLogEntries();
		local i, j;
		local qStatus = "";
		for i=1, numEntries do
			local qLink = GetQuestLink(i);
			if (qLink) then
				local qli, qlj, qlNum = string.find(qLink, (":(%d+):"));
				if (qlNum == qNum) then
					QuestGuru_SendQuestDescription(i, sender, qNum);
				end
			end
		end
	elseif (code == "0208") then -- Quest starter info (qNum, info)
		if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]] == nil) then
			QuestGuru_Quests[QuestGuru_RealmName][msg[2]] = {};
		end
		if ((QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo == nil) or (string.find(QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo, QG_UNKNOWN))) then
			QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo = msg[3];
		end
	elseif (code == "0209") then -- Requesting quest starter info (qNum)
		if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]] ~= nil) then
			if ((QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo ~= nil) and (not string.find(QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo, QG_UNKNOWN))) then
				QuestGuru_CommSendMessage(channel, "020A", msg[2], QuestGuru_Quests[QuestGuru_RealmName][msg[2]].StartInfo);
			end
		end
	elseif (code == "020A") then -- Quest finisher info (qNum, info)
		if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]] == nil) then
			QuestGuru_Quests[QuestGuru_RealmName][msg[2]] = {};
		end
		if ((QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo == nil) or (string.find(QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo, QG_UNKNOWN))) then
			QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo = msg[3];
		end
	elseif (code == "020B") then -- Requesting quest finisher info (qNum)
		if (QuestGuru_Quests[QuestGuru_RealmName][msg[2]] ~= nil) then
			if ((QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo ~= nil) and (not string.find(QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo, QG_UNKNOWN))) then
				QuestGuru_CommSendMessage(channel, "020A", msg[2], QuestGuru_Quests[QuestGuru_RealmName][msg[2]].FinishInfo);
			end
		end
	elseif (code == "020C") then -- Quest objective (qNum, type, text)
	elseif (code == "020D") then -- Request quest objectives (qNum)
	elseif (code == "020E") then -- Quest reward (qNum, type, link)
	elseif (code == "020F") then -- Request quest rewards (qNum)
	elseif (code == "0300") then -- Item link by name
		if (QuestGuru_Items[QuestGuru_RealmName]) then
			QuestGuru_Items[QuestGuru_RealmName][msg[2]] = msg[3];
		end
	elseif (code == "0301") then -- Request item link by name
		if (QuestGuru_Items[QuestGuru_RealmName] and QuestGuru_Items[QuestGuru_RealmName][msg[2]]) then
			QuestGuru_CommSendMessage(channel, "0300", msg[2], QuestGuru_Items[QuestGuru_RealmName][msg[2]]);
		end
	end
end

function QuestGuru_UpdateqID(qID, link)
	if (QuestGuru_Quests[QuestGuru_RealmName][qID] == nil) then
		QuestGuru_Quests[QuestGuru_RealmName][qID] = {};
	end
	if (QuestGuru_Quests[QuestGuru_RealmName][qID]["_link"] == nil) then
		QuestGuru_Quests[QuestGuru_RealmName][qID]["_link"] = link;
	end

	local qIDi, qIDj, qNum, qLevel = string.find(link, (":(%d+):(%d+)|"));
	if (qLevel and (QuestGuru_Quests[QuestGuru_RealmName][qID].level == nil)) then
		QuestGuru_Quests[QuestGuru_RealmName][qID].level = qLevel;
	end
	if (qNum and (QuestGuru_Quests[QuestGuru_RealmName][qNum] == nil)) then
		QuestGuru_Quests[QuestGuru_RealmName][qNum] = {};
	end
	if (qNum and (QuestGuru_Quests[QuestGuru_RealmName][qNum][qID] == nil)) then
		QuestGuru_Quests[QuestGuru_RealmName][qNum][qID] = 1;
	end
end

function QuestGuru_SendCurrentQuestStatus(req, channel)
	local numEntries, numQuests = GetNumQuestLogEntries();
	local i;
	for i=1, numEntries do
		QuestGuru_SendQuestStatus(i, req, channel);
	end
end

function QuestGuru_SendQuestStatusByName(name, req, channel)
	local i;
	local numEntries, numQuests = GetNumQuestLogEntries();
	
	for i=1, numEntries do
		local qTitle = GetQuestLogTitle(i);
		if (qTitle == name) then
			QuestGuru_SendQuestStatus(i, req, channel);
		end
	end
end

function QuestGuru_SendQuestStatus(index, req, channel)
	local j;
	local qStatus = "";
	if (req ~= false) then req = true; end
	
	local qLink = GetQuestLink(index);
	if (qLink) then
		local numObjectives = GetNumQuestLeaderBoards(index);
		for j=1, numObjectives do
			local text, type, finished = GetQuestLogLeaderBoard(j, index);
			if ((type == "item") or (type == "monster") or (type == "object")) then
				local x, y, name, numItems, numNeeded = string.find(text, QG_ITEM_REQ_STR);
				if (name ~= nil) then
					qStatus = qStatus..numItems..";";
				end
			else
				if (finished) then
					qStatus = qStatus.."1;";
				else
					qStatus = qStatus.."0;";
				end
			end
		end

		local code = "0108";
		if (req == true) then code = "0109"; end
		if (channel == nil) then
			QuestGuru_CommSendPGMessage(code, qLink, qStatus);
		else
			QuestGuru_CommSendMessage(channel, code, qLink, qStatus);
		end
	end
end

function QuestGuru_SendQuestDescription(index, recipient, qNum)
	local questSelected = GetQuestLogSelection();

	SelectQuestLogEntry(index);
	local questDescription, questObjectives = GetQuestLogQuestText();
	QuestGuru_CommSendMPMessage("WHISPER", "0206", recipient, qNum, questDescription);
	SelectQuestLogEntry(questSelected);
end

function QuestGuru_SendQuestObjectivesText(index, recipient, qNum)
	local questSelected = GetQuestLogSelection();

	SelectQuestLogEntry(index);
	local questDescription, questObjectives = GetQuestLogQuestText();
	QuestGuru_CommSendMPMessage("WHISPER", "0204", recipient, qNum, questObjectives);
	SelectQuestLogEntry(questSelected);
end

function QuestGuru_FindLink(lastqID)
	local qID, record;
	for qID, record in pairs(QuestGuru_Quests) do
		if (record["_link"] == nil) then
			if (lastqID == nil) then
				lastqID = qID;
				QuestGuru_CommSendPGMessage("0201", qID);
				QuestGuru_CommAddTimer(60, 1, QuestGuru_FindLink, lastqID);
				return;
			elseif (lastqID == qID) then
				lastqID = nil;
			end
		end
	end
end

function QuestGuru_CommUpdateParty()
	local i, v;
	local c = 0;

	for i, v in pairs(QuestGuru_CommParty) do
		if (not UnitInParty(i)) then
			QuestGuru_CommParty[i] = nil;
		else
			c = c + 1;
		end
	end

	if ((QuestGuru_CommLastPartyNum == 0) and (IsInGroup() and GetNumSubgroupMembers(LE_PARTY_CATEGORY_HOME) > 0)) then
		QuestGuru_CommSendMessage("PARTY", "0001", QUESTGURU_VERSION);
		QuestGuru_SendCurrentQuestStatus(true, "PARTY");
	end
	QuestGuru_CommLastPartyNum = GetNumSubgroupMembers(LE_PARTY_CATEGORY_HOME);
	QuestGuru_CommPendingPartyUpdate = false;
end

function QuestGuru_CommUpdateGuild()
	local i;
	local c = 0;
	local guildNum = GetNumGuildMembers();
	local showOffline = GetGuildRosterShowOffline();
	if (QuestGuru_CommGuild == nil) then QuestGuru_CommGuild = {}; end

	SetGuildRosterShowOffline(true);
	for i=1, guildNum do
		local name, rank, rankIndex, level, class, zone, note, officernote, online, status = GetGuildRosterInfo(i);
		if (not online) then
			if (name) then QuestGuru_CommGuild[name] = nil; end
		else
			if (name and QuestGuru_CommGuild[name]) then
				c = c + 1;
			end
		end
	end
	SetGuildRosterShowOffline(showOffline);
end

function QuestGuru_CommListGuild()
	local i, v;
	
	for i, v in pairs(QuestGuru_CommGuild) do
		QuestGuru_Echo(i.." is using QuestGuru v"..v.QGver, true);
	end
end