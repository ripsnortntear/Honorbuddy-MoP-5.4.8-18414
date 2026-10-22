QUESTGURU_VERSION = GetAddOnMetadata("QuestGuru", "Version");

QuestGuru_Settings = {};
QuestGuru_Items = {};

QGC_Cache = {};
QGC_Active = {};

local QuestGuru_RealmName, QuestGuru_PlayerFaction;

do
	local QuestGuru_Frame = CreateFrame("Frame", "QuestGuru_Frame", UIParent);
	QuestGuru_Frame:SetScript("OnEvent",
		function (self, event, arg1, arg2)
			QuestGuru_OnEvent(self, event, arg1, arg2);
		end);

	QuestGuru_Frame:RegisterEvent("QUEST_COMPLETE");
	QuestGuru_Frame:RegisterEvent("QUEST_LOG_UPDATE");
	QuestGuru_Frame:RegisterEvent("QUEST_PROGRESS");
	QuestGuru_Frame:RegisterEvent("UI_INFO_MESSAGE");
	QuestGuru_Frame:RegisterEvent("VARIABLES_LOADED");
	QuestGuru_Frame:RegisterEvent("GOSSIP_SHOW");
	QuestGuru_Frame:RegisterEvent("QUEST_DETAIL");
	QuestGuru_Frame:RegisterEvent("QUEST_GREETING");
	QuestGuru_Frame:RegisterEvent("QUEST_PROGRESS");

	tinsert(UISpecialFrames, "QuestLogFrame");
	QuestLogFrame:SetMovable(true)
	QuestLogFrame:EnableMouse(true)
	QuestLogFrame:RegisterForDrag("LeftButton")
	QuestLogFrame:SetScript("OnDragStart", QuestLogFrame.StartMoving)
	QuestLogFrame:SetScript("OnDragStop", QuestLogFrame.StopMovingOrSizing)
end

function QuestGuru_OnEvent(self, event, arg1, arg2)
	if (event == "UI_INFO_MESSAGE") then
		QuestGuru_UIInfoMessage(arg1);
	elseif (event == "VARIABLES_LOADED") then
		QuestLogTitleText:SetText(QuestLogTitleText:GetText().." - QuestGuru v"..QUESTGURU_VERSION);
		QuestGuru_LoadVariables();
		QuestGuru_HistoryShowButton:Hide();
		if IsAddOnLoaded("QuestCompletist") then QuestGuru_HistoryShowButton:Show(); end
	elseif (event=="QUEST_COMPLETE") then
		if (QuestGuru_Settings.AutoComplete and (GetNumQuestChoices() == 0)) then
			QuestRewardCompleteButton_OnClick();
		end
	elseif (event=="QUEST_PROGRESS") then
		QuestGuru_QuestProgressShow();
		if (QuestGuru_Settings.AutoComplete and IsQuestCompletable()) then
			CompleteQuest();
		end
	elseif (event=="QUEST_LOG_UPDATE") then
		wipe(QGC_Active);
		local numEntries, numQuests = GetNumQuestLogEntries();
		for i=1, numEntries do
			local qLink = GetQuestLink(i);
			if (qLink) then
				local qi, qj, questNum = string.find(qLink, (":(%d+):"));
				if (questNum) then QGC_Active[questNum]="true"; end
			end
		end
	elseif (event=="GOSSIP_SHOW") then
		QuestGuru_GossipShow();
	elseif (event=="QUEST_GREETING") then
		QuestGuru_QuestFrameGreetingPanel_OnShow();
	elseif (event=="QUEST_DETAIL") then
		QuestGuru_QuestDetailShow();
	end
end

function QuestGuru_ShowHistory()
-- Bring up any history addon that is installed but doesn't have it's own button
  if IsAddOnLoaded("QuestCompletist") then qcQuestCompletistUI:Show(); end
end

function QuestGuru_TitleButtonCheckBox_OnClick(self, button, down)
	local index = self:GetParent():GetID();
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(index);

	if (self:GetChecked()) then
		if (isHeader) then
			QuestGuru_AddQuestWatchHeader(questLogTitleText);
		else
			if (not AddQuestWatch(index)) then
				self:SetChecked(0);
			end
		end
	else
		if (isHeader) then
			QuestGuru_RemoveQuestWatchHeader(questLogTitleText);
		else
			RemoveQuestWatch(index);
		end
	end
	WatchFrame_Update();
	QuestLog_Update();
end

function QuestGuru_AddQuestWatchHeader(questHeader)
	local numEntries, numQuests = GetNumQuestLogEntries();
	local inHeader = false;

	for questIndex=1, numEntries do
		local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
		if (isHeader) then
			if (questLogTitleText == questHeader) then
				inHeader = true;
			else
				inHeader = false;
			end
		end
		if (inHeader and (not isHeader)) then
			AddQuestWatch(questIndex);
		end
	end
end

function IsHeaderWatched(questHeader)
	local numEntries, numQuests = GetNumQuestLogEntries();
	local inHeader = false;
	local headerWatched = true;

	for questIndex=1, numEntries do
		local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
		if (isHeader) then
			if (questLogTitleText == questHeader) then
				inHeader = true;
			else
				inHeader = false;
			end
		end
		if (inHeader and (not isHeader)) then
			headerWatched = (headerWatched and IsQuestWatched(questIndex));
		end
	end
	return headerWatched;
end

function QuestGuru_RemoveQuestWatchHeader(questHeader)
	local numEntries, numQuests = GetNumQuestLogEntries();
	local inHeader = false;

	for questIndex=1, numEntries do
		local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
		if (isHeader) then
			if (questLogTitleText == questHeader) then
				inHeader = true;
			else
				inHeader = false;
			end
		end
		if (inHeader and (not isHeader)) then
			RemoveQuestWatch(questIndex);
		end
	end
end

local prevParent;
local function QuestGuru_QuestLog_HighlightQuest(questLogTitle)
	if ( prevParent and prevParent ~= questLogTitle ) then
		-- set prev quest's colors back to normal
		prevParent.level:SetTextColor(prevParent.r, prevParent.g, prevParent.b);
	end
	if ( questLogTitle ) then
		-- highlight the quest's colors
		questLogTitle.level:SetTextColor(HIGHLIGHT_FONT_COLOR.r, HIGHLIGHT_FONT_COLOR.g, HIGHLIGHT_FONT_COLOR.b);
	end
	prevParent = questLogTitle;
end

function QuestGuru_SetSelection(questIndex)
	if ( questIndex == 0 ) then
		return;
	end

	-- find and highlight the selected quest
	local buttons = QuestLogScrollFrame.buttons;
	local numButtons = #buttons;
	local min = 1;
	local max = #buttons;
	local mid;
	local titleButton, id;
	while ( min <= max ) do
		mid = bit.rshift(min + max, 1);
		titleButton = buttons[mid];
		id = titleButton:GetID();
		if ( id == questIndex ) then
			QuestGuru_QuestLog_HighlightQuest(titleButton);
			break;
		end
		if ( id > questIndex ) then
			max = mid - 1;
		else
			min = mid + 1;
		end
	end

	-- Dridzt supplied snippet to solve some obscure framelevel issues.
  local QG_qlcp = _G["QuestLogControlPanel"]
  QG_qlcp:SetFrameLevel(QG_qlcp:GetParent():GetFrameLevel()+1);

end
hooksecurefunc("QuestLog_SetSelection", QuestGuru_SetSelection);

--local QG_QuestLogUpdate = QuestLog_Update;
function QG_QuestLog_Update()
--	QG_QuestLogUpdate();
	local buttons = QuestLogScrollFrame.buttons;
	local numButtons = #buttons;
	local scrollOffset = HybridScrollFrame_GetOffset(QuestLogScrollFrame);
	local questLogSelection = GetQuestLogSelection();

	local questIndex, questLogTitle, questTitleTag, questNumGroupMates, questNormalText, questCheck;
	local title, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily;
	for i=1, numButtons do
		questLogTitle = buttons[i];
		questIndex = questLogTitle:GetID();
		title, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
		questNumGroupMates = questLogTitle.groupMates;
		questNumGroupMates:SetPoint("LEFT", 6, 0);
		if (questLogTitle.level == nil) then
			questLogTitle.level = questLogTitle:CreateFontString(questLogTitle:GetName().."Level", "BACKGROUND", "GameFontNormalSmall");
			questLogTitle.tag:SetFontObject("GameFontNormalSmall");
			questLogTitle.level:SetPoint("LEFT", 28, 0);
			questLogTitle.level:SetWidth(32);
			questLogTitle.level:SetJustifyH("CENTER");
			questLogTitle.level.parentKey = "level";
			questLogTitle:HookScript("OnEnter", function(self)
					self.level:SetTextColor(HIGHLIGHT_FONT_COLOR.r, HIGHLIGHT_FONT_COLOR.g, HIGHLIGHT_FONT_COLOR.b);
				end);
			questLogTitle:HookScript("OnLeave", function(self)
					if ( self:GetID() ~= QuestLogFrame.selectedIndex ) then
						self.level:SetTextColor(self.r, self.g, self.b);
					end
				end);
		end
		if (questLogTitle.checkBox == nil) then
			questLogTitle.checkBox = CreateFrame("CheckButton", questLogTitle:GetName().."CheckBox", questLogTitle, "OptionsCheckButtonTemplate");
			questLogTitle.checkBox:SetHeight(16);
			questLogTitle.checkBox:SetWidth(16);
			questLogTitle.checkBox:SetPoint("LEFT", 16, 0);
			questLogTitle.checkBox:SetHitRectInsets(0, 0, 0, 0);
			questLogTitle.checkBox:SetScript("OnClick", QuestGuru_TitleButtonCheckBox_OnClick);
		end
		questLogTitle.check:Hide();

		questLevel = questLogTitle.level;
		if (isHeader) then
			questLevel:SetText("");
			questLogTitle.normalText:SetPoint("LEFT", 32, 0);
			if (IsHeaderWatched(title)) then
				questLogTitle.checkBox:SetChecked(1);
			else
				questLogTitle.checkBox:SetChecked(0);
			end
		else
			local strLvl = level
			if (isDaily) then
				strLvl = strLvl.."Y";
			end
			if (questTag ~= nil) then
				if ((questTag == "Group") and (suggestedGroup > 0)) then
					strLvl = strLvl.."G"..suggestedGroup;
				elseif (questTag == "Dungeon") then
					strLvl = strLvl.."D";
				else
					strLvl = strLvl.."+"
				end
			else
				if (suggestedGroup > 0) then
					strLvl = strLvl.."G"..suggestedGroup;
				end
			end

			if (QuestGuru_Settings.ShowLevels == nil) then
				QuestGuru_Settings.ShowLevels = {};
			end
			if (QuestGuru_Settings.ShowLevels.QuestLog) then
				questLevel:Show();
				textLeft = 52;
			else
				questLevel:Hide();
				textLeft = 32;
			end
			questLevel:SetText(strLvl);
			questLogTitle.normalText:SetPoint("LEFT", textLeft, 0);
			local color = GetQuestDifficultyColor(level);
			questLevel:SetTextColor(color.r, color.g, color.b);
			if ( questLogSelection == questIndex ) then
				QuestGuru_QuestLog_HighlightQuest(questLogTitle);
			end
			if (IsQuestWatched(questIndex)) then
				questLogTitle.checkBox:SetChecked(1);
			else
				questLogTitle.checkBox:SetChecked(0);
			end
		end
		QuestLogTitleButton_Resize(questLogTitle);
	end
end
hooksecurefunc("QuestLog_Update", QG_QuestLog_Update);

QuestLogScrollFrame.update = function () QuestLog_Update(); end;

function QuestGuru_ShowObjectives(template, parentFrame, acceptButton, material)
	local i, lastObjective;
	local numObjectives = GetNumQuestLeaderBoards();
	
	for i=1, MAX_OBJECTIVES do
		_G["QuestGuru_QuestLogObjectiveItem"..i]:Hide();
		_G["QuestGuru_QuestLogObjectiveItem"..i].timerEnabled = false;
	end

	if (QuestGuru_Settings.ShowObjItemIcons) then
		local currObjItem = 0;
		for i=1, numObjectives, 1 do
			local text, type, finished = GetQuestLogLeaderBoard(i);
			local count;
			if (type == "item") then
				local questObj = _G["QuestInfoObjective"..i];
				if ( not text or strlen(text) == 0 ) then
					text = type;
				end
				if ( finished ) then
					questObj:SetTextColor(0.2, 0.2, 0.2);
					text = text.." ("..COMPLETE..")";
				else
					questObj:SetTextColor(0, 0, 0);
				end
				questObj:SetText(text);
				questObj:Show();
				questObj:SetAlpha(0);
				lastObjective = questObj;

				currObjItem = currObjItem + 1;
				local questItem = _G["QuestGuru_QuestLogObjectiveItem"..currObjItem];
				questItem.type = "required";
				local x, y, name, numItems, numNeeded = string.find(text, QG_ITEM_REQ_STR);
				local iiName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, texture = GetItemInfo(name);

				if ((not itemLink) and QuestGuru_Items[QuestGuru_RealmName][name]) then
					iiName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, texture = GetItemInfo(QuestGuru_Items[QuestGuru_RealmName][name]);
				end
				if (not itemLink) then
					texture = "Interface\\Icons\\INV_Misc_QuestionMark";
					count = 0;
					SetItemButtonTextureVertexColor(questItem, 0.4, 0.4, 0.4);
					QuestGuru_CommSendPGMessage("0301", name);
				else
					if (texture == nil) then
						GameTooltip:SetHyperlink(itemLink);
						GameTooltip:Show();
						GameTooltip:Hide();
						questItem.timerEnabled = true;
					end
					count = GetItemCount(itemLink);
					SetItemButtonTextureVertexColor(questItem, 0.8, 0.8, 0.8);
				end
				questItem:SetID(currObjItem)
				-- For the tooltip
				questItem.rewardType = "item"
				questItem.link = itemLink;
				_G["QuestGuru_QuestLogObjectiveItem"..currObjItem.."Name"]:SetText(name);
				_G["QuestGuru_QuestLogObjectiveItem"..currObjItem.."Count"]:SetText(count.." /"..numNeeded);
				SetItemButtonTexture(questItem, texture);
				SetItemButtonNameFrameVertexColor(questItem, 1, 1, 1);
				if ( currObjItem  > 1 ) then
					if ( mod(currObjItem ,2) == 1 ) then
						questItem:SetPoint("TOPLEFT", "QuestGuru_QuestLogObjectiveItem"..(currObjItem - 2), "BOTTOMLEFT", 0, -2);
						lastObjective = questItem;
					else
						questItem:SetPoint("TOPLEFT", "QuestGuru_QuestLogObjectiveItem"..(currObjItem - 1), "TOPRIGHT", 1, 0);
						lastObjective = _G["QuestGuru_QuestLogObjectiveItem"..(currObjItem)-1];
					end
				else
					if (i > 1) then
						questItem:SetPoint("TOPLEFT", "QuestInfoObjective"..(i - 1), "BOTTOMLEFT", 0, -5);
					else
						questItem:SetPoint("TOPLEFT");
					end
					lastObjective = questItem;
				end
				questItem:Show();
			end
		end
		if ( lastObjective ) then
			QuestInfoObjectivesFrame:SetHeight(QuestInfoObjectivesFrame:GetTop() - lastObjective:GetTop() + lastObjective:GetHeight());
			QuestInfoDescriptionHeader:SetPoint("TOPLEFT", QuestInfoObjectivesFrame, "BOTTOMLEFT");
		end
	end
end
hooksecurefunc("QuestInfo_Display", QuestGuru_ShowObjectives);

function QuestGuru_UIInfoMessage(arg1)
  QuestGuru_Echo(string.format("QuestGuru_UIInfoMessage invoked, arg1=%s", tostring(arg1) ), false)
	local questID=0;
	local sound, announceText;
	local numEntries, numQuests = GetNumQuestLogEntries();
	local i, ii, jj, itemName, numDone, numNeeded;
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily;

	if (arg1 == nil) then return; end

	for questID=1, numEntries, 1 do
		questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questID);
		local numObjectives = GetNumQuestLeaderBoards(questID);
		local currQuest = false;
		local questComplete = true;
		sound = nil;
		announceText = nil;

		if (numObjectives ~= nil) then
			for i=1, numObjectives, 1 do
				local text, type, finished = GetQuestLogLeaderBoard(i,questID);
        if (text == nil) then 
          QuestGuru_Echo(string.format("Error: numEntries=%s, numQuests=%s", 
            tostring(numEntries), tostring(numQuests) ), false)
          QuestGuru_Echo(string.format("Error: Variable text is nil. arg1=%s, questID=%s, i=%s, numObjectives=%s, questLogTitleText=%s", 
            tostring(arg1), tostring(questID), tostring(i), tostring(numObjectives), tostring(questLogTitleText) ), false)
          QuestGuru_Echo(string.format("Error: text=%s, type=%s, finished=%s", 
            tostring(text), tostring(type), tostring(finished) ), false)
        else
  				local qi, qj, qitemName, qnumDone, qnumNeeded = string.find(text, QG_ITEM_REQ_STR);
	  			if ((type == "item") or (type == "monster") or (type == "object")) then
		  			if (type == "object") then type = "item"; end
			  		ii, jj, itemName, numDone, numNeeded = string.find(arg1, QG_ITEM_REQ_STR);
				  	if ((itemName == qitemName) and (numNeeded == qnumNeeded)) then
					  	if (QuestGuru_Settings.Sounds.Progress.Enabled) then sound = QuestGuru_Settings.Sounds.Progress.Sound; end
						  if (QuestGuru_Settings.Announce.item.Enabled and (type == "item")) then announceText = QuestGuru_Settings.Announce.item.Text; end
   						if (QuestGuru_Settings.Announce.monster.Enabled and (type == "monster")) then announceText = QuestGuru_Settings.Announce.monster.Text; end
		  				if (numDone == numNeeded) then
			  				if (QuestGuru_Settings.Sounds.ObjComplete.Enabled) then sound = QuestGuru_Settings.Sounds.ObjComplete.Sound; end
				  			if (announceText) then announceText = announceText.." ("..QG_ANNOUNCE_KEYWORD_COMPLETE..")"; end
					  		finished = true;
						  end
						  currQuest = true;
					  end
				  elseif (type == "event") then
					  itemName = text;
  					numDone=1;
	  				numNeeded=1;
		  			if (arg1 == text.." ("..COMPLETE..")") then
			  			if (QuestGuru_Settings.Sounds.ObjComplete.Enabled) then sound = QuestGuru_Settings.Sounds.ObjComplete.Sound; end
				  		if (QuestGuru_Settings.Announce.event.Enabled) then announceText = QuestGuru_Settings.Announce.event.Text; end
					  	currQuest = true;
						  finished = true;
  					end
	  			end
		  		if (not finished) then questComplete = false; end
        end
			end
		end
		if (currQuest) then
			if (questComplete) then
				if (QuestGuru_Settings.Sounds.QuestComplete.Enabled) then sound = QuestGuru_Settings.Sounds.QuestComplete.Sound; end
				if (QuestGuru_Settings.Announce.Quest.Enabled) then announceText = QuestGuru_Settings.Announce.Quest.Text; end
				UIErrorsFrame:AddMessage(QG_QUEST_COMPLETE, 1, 1, 0, 1, 5);
			end
			QuestGuru_PlayExtraSound(sound);
			QuestGuru_DoAnnounce(announceText, numDone, numNeeded, itemName, questLogTitleText, arg1, questID);
			QuestGuru_CommAddTimer(1, 1, QuestGuru_SendQuestStatus, questID, false);
		end
	end
end

function QuestGuru_PlayExtraSound(sound)
	if (sound == nil) then return; end
	
	if (QuestGuru_Settings.Sounds.Enabled) then
		if ((string.sub(sound, -4) == ".wav") or (string.sub(sound, -4) == ".mp3")) then
			PlaySoundFile(sound);
		else
			PlaySound(sound);
		end
	end
end

function QuestGuru_QuestProgressShow()
	local questTitle = GetTitleText();
	
	QuestGuru_UpdateProgressFrame(questTitle, GetGossipActiveQuests());

	local i;
	for i=1, GetNumQuestItems() do
		local name, texture, numItems, quality, isUsable = GetQuestItemInfo("required", i);
		local link = GetQuestItemLink("required", i);
		if (name and link) then
			QuestGuru_Items[QuestGuru_RealmName][name] = link;
			_G["QuestProgressItem"..i].link = link;
			_G["QuestProgressItem"..i]:SetScript("OnEnter", function (self)
				GameTooltip:SetOwner(self, "ANCHOR_RIGHT");
				GameTooltip:SetHyperlink(self.link);
			end);
		end
	end
end

function QuestGuru_UpdateProgressFrame(questTitle, ...)
	local i;

	for i=1, select("#", ...), 3 do
	    if (select(i, ...) == questTitle) then
	        QuestProgressTitleText:SetText("["..select(i+1, ...).."] "..questTitle);
		end
	end
end

function QuestGuru_LoadVariables()
		QuestGuru_Player = UnitName("player");
		QuestGuru_PlayerFaction = UnitFactionGroup("player");
		QuestGuru_RealmName = GetRealmName();

		if (QuestGuru_Items == nil) then
			QuestGuru_Items = {};
		end
		if (QuestGuru_Items[QuestGuru_RealmName] == nil) then
			QuestGuru_Items[QuestGuru_RealmName] = {};
		end
		
		if (QuestGuru_Settings == nil) then
			QuestGuru_Settings = {};
		end
		QuestGuru_QuestAnnounceLoadSettings();
		QuestGuru_OptionsSoundsLoadSettings();
		
		if (QuestGuru_Settings.AutoComplete ~= true) then
			QuestGuru_Settings.AutoComplete = false;
		end
		QuestGuru_OptionsFrameAutoCompleteToggle:SetChecked(QuestGuru_Settings.AutoComplete);
		
		if (QuestGuru_Settings.ShowObjItemIcons ~= true) then
			QuestGuru_Settings.ShowObjItemIcons = false;
		end
		QuestGuru_OptionsFrameShowObjItemIconsToggle:SetChecked(QuestGuru_Settings.ShowObjItemIcons);
		
		if (QuestGuru_Settings.DisableComm ~= true) then
		    QuestGuru_Settings.DisableComm = false;
		end
		QuestGuru_OptionsFrameDisableCommToggle:SetChecked(QuestGuru_Settings.DisableComm);

		if (QuestGuru_Settings.HeaderQuests ~= false) then
		    QuestGuru_Settings.HeaderQuests = true;
		end
		QuestGuru_OptionsFrameShowHeaderQuestsToggle:SetChecked(QuestGuru_Settings.HeaderQuests);

		if (QuestGuru_Settings.ShowLevels == nil) then
			QuestGuru_Settings.ShowLevels = {};
		end
		if (QuestGuru_Settings.ShowLevels.QuestLog ~= false) then
			QuestGuru_Settings.ShowLevels.QuestLog = true;
		end
		QuestGuru_OptionsFrameShowLevelsQuestLogToggle:SetChecked(QuestGuru_Settings.ShowLevels.QuestLog);
end

function QuestGuru_Echo(msg, always)
	local info = ChatTypeInfo["SYSTEM"];
	if msg == nil then msg = "<nil>"; end
--	if (always) then
--		DEFAULT_CHAT_FRAME:AddMessage("always " .. tostring(always), info.r, info.g, info.b, info.id);
--  else
--		DEFAULT_CHAT_FRAME:AddMessage("not always " .. tostring(always), info.r, info.g, info.b, info.id);
--	end
--	if (QuestGuru_Settings) then
--		DEFAULT_CHAT_FRAME:AddMessage("QuestGuru_Settings", info.r, info.g, info.b, info.id);
--    if (QuestGuru_Settings.DebugLevel) then
--      DEFAULT_CHAT_FRAME:AddMessage("QuestGuru_Settings.DebugLevel = " .. QuestGuru_Settings.DebugLevel, info.r, info.g, info.b, info.id);
--      DEFAULT_CHAT_FRAME:AddMessage(msg, info.r, info.g, info.b, info.id);
--	  end
--	end
	if (always or (QuestGuru_Settings and QuestGuru_Settings.DebugLevel and QuestGuru_Settings.DebugLevel > 0)) then
		DEFAULT_CHAT_FRAME:AddMessage(msg, info.r, info.g, info.b, info.id);
	end
end

--function QuestGuru_Dump(Var2Dump, Label)
--  if ( Label ~= nil ) then
--    DEFAULT_CHAT_FRAME:AddMessage("Dump " .. Label .. ":");
--  end
--  DevTools_Dump(Var2Dump)
--end -- function

function QuestGuru_QuestFrameGreetingPanel_OnShow()
	local numActiveQuests = GetNumActiveQuests();
	for i=1, numActiveQuests, 1 do
		local questTitleButton = _G["QuestTitleButton"..i];
		local questTitleButtonIcon = _G[questTitleButton:GetName() .. "QuestIcon"];
		local level = GetActiveLevel(i);
		if (level == -1) then level = "*"; end
		local title = GetActiveTitle(i);
		if ( IsActiveQuestTrivial(i) ) then
			questTitleButton:SetFormattedText(TRIVIAL_QUEST_DISPLAY, "["..level.."] "..title);
		else
			questTitleButton:SetFormattedText(NORMAL_QUEST_DISPLAY, "["..level.."] "..title);
		end
	    if (QuestGuru_IsQuestComplete(title)) then
	    	SetDesaturation(questTitleButtonIcon, nil);
	    else
	    	SetDesaturation(questTitleButtonIcon, 1);
	    end
	end

	local numAvailableQuests = GetNumAvailableQuests();
	for i=(numActiveQuests + 1), (numActiveQuests + numAvailableQuests), 1 do
		local questTitleButton = _G["QuestTitleButton"..i];
		local questTitleButtonIcon = _G[questTitleButton:GetName() .. "QuestIcon"];
		local level = GetAvailableLevel(i - numActiveQuests);
		if (level == -1) then level = "*"; end
		local title = GetAvailableTitle(i - numActiveQuests);
		if ( IsAvailableQuestTrivial(i - numActiveQuests) ) then
			questTitleButton:SetFormattedText(TRIVIAL_QUEST_DISPLAY, "["..level.."] "..title);
			questTitleButtonIcon:SetVertexColor(0.5,0.5,0.5);
		else
			questTitleButton:SetFormattedText(NORMAL_QUEST_DISPLAY, "["..level.."] "..title);
			questTitleButtonIcon:SetVertexColor(1,1,1);
		end
	end
end

function QuestGuru_QuestDetailShow()
	local questTitle = GetTitleText();
	QuestGuru_UpdateDetailFrame(questTitle, GetGossipAvailableQuests());
end

function QuestGuru_UpdateDetailFrame(questTitle, ...)
	local i;
	for i=1, select("#", ...), 5 do
		if (select(i, ...) == questTitle) then
			QuestInfoTitleHeader:SetText("["..select(i+1, ...).."] "..questTitle);
		end
	end
end

function QuestGuru_QuestProgressShow()
	local questTitle = GetTitleText();
	
	QuestGuru_UpdateProgressFrame(questTitle, GetGossipActiveQuests());

	local i;
	for i=1, GetNumQuestItems() do
		local name, texture, numItems, quality, isUsable = GetQuestItemInfo("required", i);
		local link = GetQuestItemLink("required", i);
		if (name and link) then
			QuestGuru_Items[QuestGuru_RealmName][name] = link;
		end
	end
end

function QuestGuru_UpdateProgressFrame(questTitle, ...)
	local i;

	for i=1, select("#", ...), 4 do
	    if (select(i, ...) == questTitle) then
	        QuestProgressTitleText:SetText("["..select(i+1, ...).."] "..questTitle);
		end
	end
end

function QuestGuru_GossipShow()
	local npc = UnitName("npc");
	QuestGuru_UpdateGossipFrame(1, true, GetGossipAvailableQuests());
	if (GetNumGossipAvailableQuests() > 0) then
		QuestGuru_UpdateGossipFrame(GetNumGossipAvailableQuests() + 2, false, GetGossipActiveQuests());
	else
		QuestGuru_UpdateGossipFrame(1, false, GetGossipActiveQuests());
	end
end

function QuestGuru_UpdateGossipFrame(currButton, availQuest, ...)
	local i;
	
	--MoP changed this from 4&5 to 5&6.
	local j = 5;
	if (availQuest) then j=6; end
	for i=1, select("#", ...), j do
	  --QuestGuru_Echo("level="..i,1)
	  local button = _G["GossipTitleButton"..currButton];
	  local buttonIcon = _G[button:GetName() .. "GossipIcon"];
	  local level = select(i+1, ...);
--QuestGuru_Echo(level, 1)
	  if (level == -1) then level = "*"; end
	    button:SetFormattedText("[%s] %s", level, select(i, ...));
	  if (QuestGuru_IsQuestComplete(select(i, ...)) or availQuest) then
	    SetDesaturation(buttonIcon, nil);
	  else
	    SetDesaturation(buttonIcon, 1);
	  end
	  currButton = currButton + 1;
	end
--QuestGuru_Echo("bottom", 1)
end

function QuestGuru_IsQuestComplete(questTitle)
	local numEntries, numQuests = GetNumQuestLogEntries();
	local i;
	
	for i=1, numEntries do
		local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(i);
		local numObjectives = GetNumQuestLeaderBoards(i);

		if (questLogTitleText == questTitle) then return (isComplete or (numObjectives == 0)); end
	end
	return false;
end

local QuestLogFrameX, QuestLogFrameY;
QuestLogFrame:HookScript("OnShow", 
  function(self,button)
    --QuestGuru_Echo("OnShow", 1)
    if (QuestLogFrameX ~= nil and QuestLogFrameY ~= nil) then
      QuestLogFrame:SetPoint("TOPLEFT", QuestLogFrame:GetParent(), "BOTTOMLEFT", QuestLogFrameX, QuestLogFrameY);
    end
  end)

QuestLogFrame:HookScript("OnHide",
  function(self, button)
    --QuestGuru_Echo("OnHide", 1)
    QuestLogFrameX = QuestLogFrame:GetLeft();
    QuestLogFrameY = QuestLogFrame:GetTop();
  end)

--Slash Commands are UpperCase, Global, of the form SLASH_<SlashCmdListMemberFunctionName><digit>
local ADirName, A = ...                                    -- ... contains the name of the Addon Directory, which is the name of the addon and an (initially) empty table that is passed to all modules in the directory & sub-directories
function A.SlashHandler(SlashText, OriginatingChatFrame)
  QuestGuru_Echo(format("/qg %s", tostring(SlashText)), true)
  if SlashText == "testui1" then
    QuestGuru_UIInfoMessage("Test UI Info Message 1/3")
  elseif SlashText == "debugon" then
    QuestGuru_Settings.DebugLevel = 1
    QuestGuru_Echo("Debug enabled", true)
  elseif SlashText == "debugoff" then
    QuestGuru_Settings.DebugLevel = 0
    QuestGuru_Echo("Debug disabled", true)
  end
end
SLASH_QG1 = "/qg"                                        -- Slash command 1
SlashCmdList["QG"] = A.SlashHandler                       -- Register our SlashCmdListMemberFunction
