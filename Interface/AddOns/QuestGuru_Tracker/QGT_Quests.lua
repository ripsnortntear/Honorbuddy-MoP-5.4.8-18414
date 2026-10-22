QGT_WatchHeaders = {};
QGT_WatchQuests = {}; -- Stores if objectives for individual quests are shown
QGT_WatchFlash = {};
QGT_WatchList = {}; -- List of quests that are tracked
QGT_WatchLine = {};
QGT_WatchLines = 0;
QGT_WatchUpdateTime = 0;
QGT_WATCHFRAME_NUM_ITEMS = 0;
QGT_LastAbandonQuestTitle = "";
QGT_QuestTimer = {};

QGT_Timer = {};
-- .interval = number of seconds to wait
-- .currTime = number of seconds remaining
-- .func = function to call after interval
-- .count = number of times to repeat (-1 for infinite)
-- .args = options arguments to pass
QGT_CurrTime = 0;

local QuestGuru_RealmName = GetRealmName() --Scenario
local QGT_ScenarioProgress = {} --Scenario

function QGT_SetQuestWatchBorder(enabled)
	if (enabled) then
		QGT_QuestWatchFrame:SetBackdrop({
			bgFile="Interface\\Characterframe\\UI-Party-Background",
			edgeFile="Interface\\Tooltips\\UI-Tooltip-Border",
			tile=1, tileSize=16, edgeSize=16,
			insets={left=4, right=4, top=4, bottom=4}
		});
	else
		QGT_QuestWatchFrame:SetBackdrop({
			bgFile="Interface\\Characterframe\\UI-Party-Background",
			tile=1, tileSize=16,
			insets={left=4, right=4, top=4, bottom=4}
		});
	end
	QGT_QuestWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
end

do
	local temp, i;

  local function QGT_Set_Toggle()
    local CBS = GetCurrentBindingSet()
    if (CBS ~= 1 and CBS ~= 2) then return -1 end -- not a valid Binding Set
    if (GetBindingKey("QGT_TOGGLE_TRACKER_KB") or GetBindingAction("SHIFT-L") ~= "") then return 0 end -- already set to something 
  	SetBinding("SHIFT-L", "QGT_TOGGLE_TRACKER_KB");
    SaveBindings(CBS);
    return 1
  end --function

	QGT_QuestWatchFrame = CreateFrame("FRAME", "QGT_QuestWatchFrame", UIParent);
	QGT_QuestWatchFrame:Hide();
	QGT_QuestWatchFrame:EnableMouse(1);
	QGT_QuestWatchFrame:EnableMouseWheel(1);
	QGT_QuestWatchFrame:SetMovable(1);
	QGT_QuestWatchFrame:SetResizable(0);
	QGT_QuestWatchFrame:SetToplevel(1);
	QGT_QuestWatchFrame:SetFrameStrata("LOW");
	QGT_QuestWatchFrame:SetClampedToScreen(true);
	QGT_QuestWatchFrame:SetWidth(256);
	QGT_QuestWatchFrame:SetHeight(20);
	QGT_QuestWatchFrame:SetPoint("TOPRIGHT", MinimapCluster, "BOTTOMRIGHT", 0, 10);
	QGT_QuestWatchFrame:SetHitRectInsets(-16, 0, 0, 0);
	QGT_SetQuestWatchBorder(true);

	QGT_QuestWatchFrame:RegisterForDrag("LeftButton");
	QGT_QuestWatchFrame:SetScript("OnDragStart",
		function ()
			QGT_DragStart();
		end);
 	QGT_QuestWatchFrame:SetScript("OnDragStop",
	 	function ()
		 	QGT_DragStop();
		end);
	QGT_QuestWatchFrame:SetScript("OnEnter",
		function (self)
			QGT_QuestWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowQuestTrackerSlider(self, true);
		end);
	QGT_QuestWatchFrame:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
		end);
	QGT_QuestWatchFrame:SetScript("OnMouseWheel",
		function (self, delta)
			local min, max = QGT_QuestWatchFrameSlider:GetMinMaxValues();
			local currVal = QGT_QuestWatchFrameSlider:GetValue();

			currVal = currVal - delta;
			if (currVal < min) then currVal = min; end
			if (currVal > max) then currVal = max; end
			QGT_QuestWatchFrameSlider:SetValue(currVal);
		end);
	QGT_QuestWatchFrame:SetScript("OnShow",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
		end);
	QGT_QuestWatchFrame:RegisterEvent("QUEST_WATCH_UPDATE");
	QGT_QuestWatchFrame:RegisterEvent("VARIABLES_LOADED");
	QGT_QuestWatchFrame:RegisterEvent("UI_INFO_MESSAGE");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_ENTERING_WORLD");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_LEAVING_WORLD");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_LOGOUT");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_LOGIN");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_REGEN_DISABLED");
	QGT_QuestWatchFrame:RegisterEvent("PLAYER_REGEN_ENABLED");
	QGT_QuestWatchFrame:SetScript("OnEvent",
		function (self, event, arg1, arg2, arg3, arg4)

--QuestGuru_Echo(format("Event %s received", tostring(event) ), false)

			if (event == "QUEST_WATCH_UPDATE") then
				QGT_AddTimer(2, 1, CheckRemoveQuestWatch, arg1);
			elseif (event == "VARIABLES_LOADED") then
				QGT_VariablesLoaded = true;
				QGT_QuestWatchLoadSettings();
        if QGT_Set_Toggle() == -1 then
          QGT_AddTimer(1,3, function() QGT_Set_Toggle() end)
        end
			elseif (event == "UI_INFO_MESSAGE") then
				QGT_UIInfoMessage(arg1);
			elseif (event == "PLAYER_LEAVING_WORLD") then
				QGT_PlayerAlive = false;
			elseif (event == "PLAYER_ENTERING_WORLD") then
				QGT_PlayerAlive = true;
			elseif (event == "PLAYER_REGEN_ENABLED") then
				QGT_PlayerInCombat = false;
				WatchFrame_Update();
			elseif (event == "PLAYER_REGEN_DISABLED") then
				QGT_PlayerInCombat = true;
				WatchFrame_Update();
			end
		end);
	QGT_QuestWatchFrame:SetScript("OnUpdate",
		function (self, elapsed)
			QGT_OnUpdate(elapsed);
		end);

	temp = QGT_QuestWatchFrame:CreateTexture("QGT_QuestWatchFrameBackground", "ARTWORK");
	temp:SetHeight(18);
	temp:SetPoint("TOPRIGHT", -4, -4);
	temp:SetPoint("TOPLEFT", 4, -4);
	temp:SetTexture(1, 1, 1);
	temp:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, 1);

	temp = QGT_QuestWatchFrame:CreateFontString("QGT_QuestWatchQuestName", "ARTWORK", "GameFontNormal");
	temp:SetPoint("TOPLEFT", 8, -6);
	temp:SetJustifyH("LEFT");
	temp:SetWidth(0);
	temp:SetHeight(12);
	temp:SetText("QuestGuru "..QG_TRACKER_QUESTS);

	temp = QGT_QuestWatchFrame:CreateFontString("QGT_QuestWatchNumQuests", "ARTWORK", "GameFontNormal");
	temp:SetPoint("LEFT", QGT_QuestWatchQuestName, "RIGHT", 12, 0);
	temp:SetJustifyH("CENTER");
	temp:SetWidth(0);
	temp:SetHeight(12);
	temp:SetText("0/"..MAX_QUESTS);

	QGT_QuestWatchFrameOptions = CreateFrame("BUTTON", "QGT_QuestWatchFrameOptions", QGT_QuestWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_QuestWatchFrameOptions:SetText("O");
	QGT_QuestWatchFrameOptions:SetPoint("TOPRIGHT", -5, -4);
	QGT_QuestWatchFrameOptions:SetScript("OnClick",
		function ()
			InterfaceOptionsFrame_OpenToCategory("QG Tracker");
		end);
	QGT_QuestWatchFrameOptions:SetScript("OnEnter",
		function (self)
			QGT_QuestWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowQuestTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			GameTooltip:SetText(QG_TRACKER_OPTIONS, nil,nil, nil, nil, 1);
		end);
	QGT_QuestWatchFrameOptions:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_QuestWatchFrameMinimize = CreateFrame("BUTTON", "QGT_QuestWatchFrameMinimize", QGT_QuestWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_QuestWatchFrameMinimize:SetText("-");
	QGT_QuestWatchFrameMinimize:SetPoint("RIGHT", QGT_QuestWatchFrameOptions, "LEFT", 0, 0);
	QGT_QuestWatchFrameMinimize:RegisterForClicks("LeftButtonUp", "RightButtonUp");
	QGT_QuestWatchFrameMinimize:SetScript("OnClick",
		function (self, button, down)
			if (button == "LeftButton") then
				if (QGT_Settings.QuestWatch.AutoMinimize) then
					QGT_ShowQuestTrackerSlider(self, false);
					QGT_Settings.QuestWatch.Minimized = true;
				else
					QGT_ShowQuestTrackerSlider(self, QGT_Settings.QuestWatch.Minimized);
					QGT_Settings.QuestWatch.Minimized = not QGT_Settings.QuestWatch.Minimized;
				end
				QGT_Settings.QuestWatch.AutoMinimize = false;
				WatchFrame_Update();
				GameTooltip:Hide();
			elseif (button == "RightButton") then
				QGT_Settings.QuestWatch.AutoMinimize = not QGT_Settings.QuestWatch.AutoMinimize;
				WatchFrame_Update();
			end
		end);
	QGT_QuestWatchFrameMinimize:SetScript("OnEnter",
		function (self)
			QGT_QuestWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowQuestTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			if (QGT_Settings.QuestWatch.Minimized) then
				GameTooltip:SetText(QG_TRACKER_SHOW, nil,nil, nil, nil, 1);
			else
				GameTooltip:SetText(QG_TRACKER_MINIMIZE, nil,nil, nil, nil, 1);
			end
		end);
	QGT_QuestWatchFrameMinimize:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_QuestWatchFrameToggle = CreateFrame("BUTTON", "QGT_QuestWatchFrameToggle", QGT_QuestWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_QuestWatchFrameToggle:SetText(QG_TRACKER_A);
	QGT_QuestWatchFrameToggle:SetPoint("RIGHT", QGT_QuestWatchFrameMinimize, "LEFT", 0, 0);
	QGT_QuestWatchFrameToggle:RegisterForClicks("LeftButtonUp", "RightButtonUp");
	QGT_QuestWatchFrameToggle:SetScript("OnClick",
		function (self, button, down)
			if (button == "LeftButton") then
				QGT_Settings.LastTracker = "A";
				QGT_Settings.BothTrackers = false;
				QGT_Settings.AchievementWatch.Top = QGT_Settings.QuestWatch.Top;
				QGT_Settings.AchievementWatch.Bottom = QGT_Settings.QuestWatch.Bottom;
				QGT_Settings.AchievementWatch.Left = QGT_Settings.QuestWatch.Left;
				QGT_Settings.AchievementWatch.Minimized = QGT_Settings.QuestWatch.Minimized;
				QGT_Settings.AchievementWatch.AutoMinimize = QGT_Settings.QuestWatch.AutoMinimize;
				WatchFrame_Update();
				GameTooltip:Hide();
			elseif (button == "RightButton") then
				QGT_Settings.LastTracker = "Q";
				QGT_Settings.BothTrackers = not QGT_Settings.BothTrackers;
				WatchFrame_Update();
			end
		end);
	QGT_QuestWatchFrameToggle:SetScript("OnEnter",
		function (self)
			QGT_QuestWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowQuestTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			GameTooltip:SetText(QG_TRACKER_TOGGLE, nil,nil, nil, nil, 1);
		end);
	QGT_QuestWatchFrameToggle:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_QuestWatchFrameSort = CreateFrame("BUTTON", "QGT_QuestWatchFrameSort", QGT_QuestWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_QuestWatchFrameSort:SetText("~");
	QGT_QuestWatchFrameSort:SetPoint("RIGHT", QGT_QuestWatchFrameToggle, "LEFT", 0, 0);
	QGT_QuestWatchFrameSort:RegisterForClicks("LeftButtonUp");
	QGT_QuestWatchFrameSort:SetScript("OnClick",
		function (self, button, down)
			ToggleDropDownMenu(1, nil, QGT_QuestsTitleMenu, self:GetName(), 0, 0);
		end);
	QGT_QuestWatchFrameSort:SetScript("OnEnter",
		function (self)
			QGT_QuestWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowQuestTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			GameTooltip:SetText(QG_TRACKER_SORT, nil,nil, nil, nil, 1);
		end);
	QGT_QuestWatchFrameSort:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
			GameTooltip:Hide();
		end);
	QGT_QuestWatchFrameSort:Hide();

	QGT_QuestWatchFrameSlider = CreateFrame("Slider", "QGT_QuestWatchFrameSlider", QGT_QuestWatchFrame, "OptionsSliderTemplate");
	QGT_QuestWatchFrameSlider:SetWidth(16);
	QGT_QuestWatchFrameSlider:SetHeight(200);
	QGT_QuestWatchFrameSliderText:SetText("");
	QGT_QuestWatchFrameSliderHigh:SetText("");
	QGT_QuestWatchFrameSliderLow:SetText("");
	QGT_QuestWatchFrameSlider:SetOrientation("VERTICAL");
	QGT_QuestWatchFrameSlider:SetPoint("TOPLEFT", QGT_QuestWatchFrame, "TOPLEFT", -14, -2);
	QGT_QuestWatchFrameSlider:SetMinMaxValues(0,0);
	QGT_QuestWatchFrameSlider:SetValueStep(1);
	QGT_QuestWatchFrameSlider:SetValue(0);
	QGT_QuestWatchFrameSlider:Hide();
	QGT_QuestWatchFrameSlider:SetScript("OnEnter",
		function (self)
			QGT_ShowQuestTrackerSlider(self, true);
		end);
	QGT_QuestWatchFrameSlider:SetScript("OnLeave",
		function (self)
			QGT_ShowQuestTrackerSlider(self, false);
		end);
	QGT_QuestWatchFrameSlider:SetScript("OnValueChanged",
		function ()
			WatchFrame_Update();
		end);
	
	for i=1, 40 do
		temp = CreateFrame("Button", "QGT_QuestWatchLine"..i, QGT_QuestWatchFrame, "QGT_QuestWatchButtonTemplate");
		temp:SetPoint("TOPLEFT", QGT_QuestWatchFrame, 8, -20);
		temp:SetHeight(13);
		temp:RegisterForClicks("LeftButtonUp", "RightButtonUp");
	end

	QGT_QuestWatchFrameTooltip = CreateFrame("GameTooltip", "QGT_QuestWatchFrameTooltip", QGT_QuestWatchFrame, "GameTooltipTemplate");
	QGT_QuestWatchFrameTooltip:Hide();
	QGT_QuestWatchFrameTooltip:SetFrameStrata("TOOLTIP");
end

function CheckRemoveQuestWatch(arg1)
	if (QGT_IsQuestComplete(arg1) and QGT_Settings.AutoUnTrack) then
		RemoveQuestWatch(arg1);
	end
end

function QGT_DragStart()
	if (not QGT_Settings.Pin) then
		QGT_QuestWatchFrame:StartMoving();
		QGT_QuestWatchFrame.isMoving = true;
	end
end

function QGT_DragStop()
	QGT_QuestWatchFrame:StopMovingOrSizing();
	QGT_QuestWatchFrame.isMoving = false;
	QGT_Settings.QuestWatch.Left = QGT_QuestWatchFrame:GetLeft();
	QGT_Settings.QuestWatch.Top = QGT_QuestWatchFrame:GetTop();
	QGT_Settings.QuestWatch.Bottom = QGT_QuestWatchFrame:GetBottom();
	QGT_QuestWatchFrameSlider:ClearAllPoints();
	if (((QGT_Settings.QuestWatch.Left + 256) * QGT_Settings.Scale) > (UIParent:GetWidth() - 16)) then
		QGT_QuestWatchFrameSlider:SetPoint("TOPLEFT", -14, -16);
	else    
		QGT_QuestWatchFrameSlider:SetPoint("TOPRIGHT", 14, -16);
	end
end

function QGT_IsQuestComplete(index)
  if C_Scenario.IsInScenario() then return false; end --Scenario
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(index);
	local numObjectives = GetNumQuestLeaderBoards(index);

	return (isComplete or (numObjectives == 0));
end

function QGT_QuestWatchTitleMenu_OnLoad(self)
	local info = UIDropDownMenu_CreateInfo();
	info.func = QGT_QuestWatchTitleMenu_OnClick;
	info.arg1 = self:GetID();
	if (self.qID) then info.arg1 = self.qID; end
	info.checked = nil;
	info.icon = nil;

	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(info.arg1);
	if (questLogTitleText == nil) then return; end
	if (IsQuestWatched(info.arg1)) then
	    info.text = QG_UNTRACK.." "..questLogTitleText;
	    info.value = "untrack";
	    UIDropDownMenu_AddButton(info, 1);
	else
	    info.text = QG_TRACK.." "..questLogTitleText;
	    info.value = "track";
	    UIDropDownMenu_AddButton(info, 1);
	end

	if (not self.isHeader) then
		if (GetQuestLogPushable() and ( GetNumSubgroupMembers(LE_PARTY_CATEGORY_HOME) > 0 or GetNumGroupMembers(LE_PARTY_CATEGORY_HOME) > 0) ) then
			info.disabled = nil;
		else
			info.disabled = 1;
		end
		info.text = QG_SHARE_QUEST;
		info.value = "share";
		UIDropDownMenu_AddButton(info, 1);
		info.disabled = nil;

		info.text = QG_ABANDON_QUEST;
		info.value = "abandon";
		UIDropDownMenu_AddButton(info, 1);
	end

	info.text = "--------------------";
	info.value = "-";
	info.isTitle = 1;
	UIDropDownMenu_AddButton(info, 1);

	info = UIDropDownMenu_CreateInfo();
	info.func = QGT_QuestWatchTitleMenu_OnClick;
	info.checked = nil;
	info.icon = nil;
	info.text = CANCEL;
	info.value = "cancel";
	UIDropDownMenu_AddButton(info, 1);
end

function QGT_QuestWatchTitleMenu_OnClick(self, button, down)
	if (self.value == "abandon") then
		QGT_Abandon();
	elseif (self.value == "share") then
		QuestLogPushQuest();
	elseif (self.value == "track") then
		AddQuestWatch(self.arg1);
		QuestLog_Update();
	elseif (self.value == "untrack") then
		RemoveQuestWatch(self.arg1);
		QuestLog_Update();
	end
end

function QGT_QuestsTitleMenu_OnLoad()
  if (QGT_Settings.QuestWatchSort == nil) then QGT_Settings.QuestWatchSort = 0; end
	local info = UIDropDownMenu_CreateInfo();
	info.func = QGT_QuestsTitleMenu_OnClick;
	info.checked = (QGT_Settings.QuestWatchSort == 0);
	info.icon = nil;
	info.text = QG_OPT_TRACKER_SORTQUESTNONE;
	info.value = "questsortnone";
  UIDropDownMenu_AddButton(info, 1);
	
	info.checked = (QGT_Settings.QuestWatchSort == 1);
	info.text = QG_OPT_TRACKER_SORTQUESTPROX;
	info.value = "questsortprox";
	UIDropDownMenu_AddButton(info, 1);

	info.checked = nil;
	info.text = "--------------------";
	info.value = "-";
	info.isTitle = 1;
	UIDropDownMenu_AddButton(info, 1);

	info = UIDropDownMenu_CreateInfo();
	info.func = QGT_QuestsTitleMenu_OnClick;
	info.checked = nil;
	info.icon = nil;
	info.text = CANCEL;
	info.value = "cancel";
	UIDropDownMenu_AddButton(info, 1);
end

function QGT_QuestsTitleMenu_OnClick(self, button, down)
	if (self.value == "questsortnone") then
		QGT_Settings.QuestWatchSort = 0;
	elseif (self.value == "questsortprox") then
		QGT_Settings.QuestWatchSort = 1;
		SortQuestWatches();
	end
	WatchFrame_Update();
end

function QGT_ShowQuestTrackerSlider(self, flag)
	if (QGT_Settings and QGT_Settings.QuestWatch and (QGT_Settings.QuestWatch.AutoMinimize==true) and not IsMouseButtonDown("LeftButton")) then
		QGT_Settings.QuestWatch.Minimized = not flag;
	end

	local watchLines = 30;
	if (QGT_Settings and QGT_Settings.Lines) then
		watchLines = QGT_Settings.Lines;
	end
	if ((flag == true) and (QGT_WatchLines > watchLines) and not QGT_Settings.QuestWatch.Minimized) then
		QGT_QuestWatchFrameSlider:Show();
	else
		if (IsMouseButtonDown("LeftButton")) then
			self:SetScript("OnMouseUp",
				function (self)
					QGT_ShowQuestTrackerSlider(self, false);
					self:SetScript("OnMouseUp", nil);
				end);
		else
			QGT_QuestWatchFrameSlider:Hide();
		end
	end
end

function QGT_SetQuestTrackerMouse(enableFlag)
	QGT_QuestWatchFrame:EnableMouse(enableFlag);
	for i=1, 40 do
		_G["QGT_QuestWatchLine"..i]:EnableMouse(enableFlag);
	end
end

function QGT_QuestWatchButton_OnEnter(self, motion)
	local qID = self.qID;
	local minWidth = 224;

	self.fading = 0;
	self.flashing = 0;
	self:SetScript("OnUpdate", nil);
	_G[self:GetName().."Highlight"]:SetAlpha(0.5);
	_G[self:GetName().."Highlight"]:Show();
	if (qID == nil) then -- header
		return;
	elseif (tonumber(qID) == nil) then -- objective line
		if (qID == "item") then --Scenario
  	  local objName = string.sub(self:GetText(),3+strlen(QGT_Settings.Bullet));
	    local objText = "";
		--if (qID == "item") then --Scenario
			local itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture = GetItemInfo(objName);
			if ((not itemLink) and IsAddOnLoaded("QuestGuru") and QuestGuru_Items and QuestGuru_Items[QuestGuru_RealmName] and QuestGuru_Items[QuestGuru_RealmName][objName]) then
				itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture = GetItemInfo(QuestGuru_Items[QuestGuru_RealmName][objName]);
			end
			if (itemLink and QGT_Settings.ShowQuestTooltips) then
				GameTooltip:SetOwner(self);
				GameTooltip:SetHyperlink(itemLink);
				GameTooltip:Show();
				local tW = GameTooltip:GetWidth();
				if (QGT_Settings.QuestWatch.Left < tW) then
					GameTooltip:SetOwner(self, "ANCHOR_RIGHT");
					GameTooltip:SetHyperlink(itemLink);
					GameTooltip:Show();
				else
					GameTooltip:SetOwner(self, "ANCHOR_LEFT");
					GameTooltip:SetHyperlink(itemLink);
					GameTooltip:Show();
				end
			end
		--Scenario
		elseif (qID == "QGT_SCENARIO_HEADER") then
			local name, currentStage, numStages = C_Scenario.GetInfo();
			if C_Scenario.IsInScenario() and currentStage > 0 and currentStage <= numStages then
				GameTooltip:SetOwner(self)
				local _, currentStage, numStages = C_Scenario.GetInfo();
				local name, description = C_Scenario.GetStepInfo();
				GameTooltip:SetText(string.format(SCENARIO_STAGE_STATUS, currentStage, numStages), 1, 0.914, 0.682, 1);
				GameTooltip:AddLine(name, 1, 0.831, 0.380, 1);
				GameTooltip:AddLine(" ");
				GameTooltip:AddLine(description, 1, 1, 1, 1);
				GameTooltip:Show();
			end
		--ScenarioEnd
		end
	else -- quest line
	    if (QGT_Settings.ShowQuestTooltips) then
			QGT_QuestWatchFrameTooltip:SetOwner(self);
			local qLink = GetQuestLink(qID);
			if (qLink) then QGT_QuestWatchFrameTooltip:SetHyperlink(qLink); end
			QGT_QuestWatchFrameTooltip:Show();
			local tW = QGT_QuestWatchFrameTooltip:GetWidth();
			if (QGT_Settings.QuestWatch.Left < tW) then
				QGT_QuestWatchFrameTooltip:SetOwner(self, "ANCHOR_RIGHT");
				if (qLink) then QGT_QuestWatchFrameTooltip:SetHyperlink(qLink); end
				QGT_QuestWatchFrameTooltip:Show();
			else
				QGT_QuestWatchFrameTooltip:SetOwner(self, "ANCHOR_LEFT");
				if (qLink) then QGT_QuestWatchFrameTooltip:SetHyperlink(qLink); end
				QGT_QuestWatchFrameTooltip:Show();
			end
		end
		if (QGT_Settings.ShowPartyTooltips) then QGT_UpdatePartyInfoTooltip(self); end
	end
end

function QGT_UpdatePartyInfoTooltip(self)
  if C_Scenario.IsInScenario() then return; end --Scenario
	local numPartyMembers = GetNumGroupMembers();
	if ( numPartyMembers == 0 or self.isHeader ) then
		return;
	end
	GameTooltip_SetDefaultAnchor(GameTooltip, self);
 
	local questLogTitleText = GetQuestLogTitle(self.qID);
	GameTooltip:SetText(questLogTitleText);
 
	local partyMemberOnQuest;
	for i=1, numPartyMembers do
		if ( IsUnitOnQuest(self.qID, "party"..i) ) then
			if ( not partyMemberOnQuest ) then
				GameTooltip:AddLine(HIGHLIGHT_FONT_COLOR_CODE..PARTY_QUEST_STATUS_ON..FONT_COLOR_CODE_CLOSE);
				partyMemberOnQuest = 1;
			end
			GameTooltip:AddLine(LIGHTYELLOW_FONT_COLOR_CODE..UnitName("party"..i)..FONT_COLOR_CODE_CLOSE);
		end
	end
	if ( not partyMemberOnQuest ) then
		GameTooltip:AddLine(HIGHLIGHT_FONT_COLOR_CODE..PARTY_QUEST_STATUS_NONE..FONT_COLOR_CODE_CLOSE);
	end
	GameTooltip:Show();
end

function QGT_QuestWatchButton_OnLeave(self, motion)
	_G[self:GetName().."Highlight"]:Hide();
	QGT_ShowQuestTrackerSlider(self, false);
	QGT_QuestWatchFrameTooltip:Hide();
	GameTooltip:Hide();
end

function QGT_QuestWatchButton_OnUpdate(self, elapsed)
	local hilight = _G[self:GetName().."Highlight"];
	
	if (self.fading == 1) then
	    local a = hilight:GetAlpha();
	    a = a - elapsed;
	    if (a <= 0) then
			a = 0;
			self.fading = 0;
			self:SetScript("OnUpdate", nil);
		end
		hilight:SetAlpha(a);
	end
	if (self.flashing and self.flashing ~= 0) then
		local a = hilight:GetAlpha();
		a = a + (elapsed * self.flashing);
		if (self.flashing == -1 and a < 0) then
			a = 0;
			self.flashing = 1;
		elseif (self.flashing == 1 and a > 1) then
			a = 1;
			self.flashing = -1;
		end
		hilight:SetAlpha(a);
	end
end

function QGT_QuestWatchButton_OnClick(self, button, down)
	local qID = self.qID;

	if (button == "LeftButton") then
		if ( IsShiftKeyDown() ) then
			local activeWindow = ChatEdit_GetActiveWindow();
			if (qID == nil) then -- header line
				if (activeWindow) then
					activeWindow:Insert(self:GetText());
				end			
			--Scenario
			elseif qID == "QGT_SCENARIO_HEADER" then
				if self:GetText() ~= "" and (activeWindow) then
					activeWindow:Insert(self:GetText());
				end
		  --ScenarioEnd
			elseif (tonumber(qID) == nil) then -- objective line
				local objName = string.sub(self:GetText(), 3 + strlen(QGT_Settings.Bullet));
				local objText = "";
				if (qID == "item") then
					local itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture = GetItemInfo(objName);
					if (itemLink == nil) then
						objText = objName;
					else
						objText = itemLink;
					end
				else
					objText = objName;
				end
				if (activeWindow) then
					activeWindow:Insert(objText);
				end					
			else -- quest number
				if (activeWindow) then
					activeWindow:Insert(GetQuestLink(qID));
				end
			end
		else
			if (qID == nil) then -- header line
				local headName = self:GetText();
				if (QGT_WatchHeaders[headName] ~= false) then
					QGT_WatchHeaders[headName] = false;
				else
					QGT_WatchHeaders[headName] = true;
				end
				WatchFrame_Update();
			--Scenario
			elseif qID == "QGT_SCENARIO_HEADER" then
				if QGT_WatchHeaders[qID] ~= false then
					QGT_WatchHeaders[qID] = false;
				else
					QGT_WatchHeaders[qID] = true;
				end
				WatchFrame_Update();
			--ScenarioEnd
			elseif (tonumber(qID) == nil) then -- objective line
				local objName = string.sub(self:GetText(), 3 + strlen(QGT_Settings.Bullet));
				if (qID == "item") then
					local itemName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, itemTexture = GetItemInfo(objName);
					if (itemLink ~= nil) then
						SetItemRef(itemLink, nil, button);
					end
				end
			else -- title line
				-- Autocomplete
				local title, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(qID);
				if (isComplete and GetQuestLogIsAutoComplete(qID)) then
					QuestIsAutoComplete = true;
				else
					QuestIsAutoComplete = false;
				end
				if (QuestIsAutoComplete) then
					ShowQuestComplete(qID);
				else
					QuestLog_OpenToQuest(qID);
					QuestLogFrame:Show();
				end
			end
		end
	elseif (button == "RightButton") then
	    if (IsShiftKeyDown()) then
	        if (qID and (tonumber(qID) ~= nil)) then
				QuestLog_SetSelection(qID, button);
				QuestLog_Update();
				ToggleDropDownMenu(1, nil, QGT_QuestWatchTitleMenu, self:GetName(), 0, 0);
			end
	    else
			if (qID == nil) then -- header line
				local headName = self:GetText();
				if (QGT_WatchHeaders[headName] ~= false) then
					QGT_WatchHeaders[headName] = false;
				else
					QGT_WatchHeaders[headName] = true;
				end
				WatchFrame_Update();
			--Scenario
			elseif qID == "QGT_SCENARIO_HEADER" then
				if QGT_WatchHeaders[qID] ~= false then
					QGT_WatchHeaders[qID] = false;
				else
					QGT_WatchHeaders[qID] = true;
				end
				WatchFrame_Update();
			--ScenarioEnd
			elseif (tonumber(qID) ~= nil) then -- title line
			    -- expand/collapse quests
				local qName = GetQuestLogTitle(qID);

				if (QGT_WatchQuests[qName] ~= false) then
					QGT_WatchQuests[qName] = false;
				else
					QGT_WatchQuests[qName] = true;
				end
				WatchFrame_Update();
			end
		end
	end
	QGT_ShowQuestTrackerSlider(self, true);
end

function QGT_WatchFrameItem_OnEnter(self)
	GameTooltip_SetDefaultAnchor(GameTooltip, self);
	GameTooltip:SetQuestLogSpecialItem(self:GetID());
	QGT_ShowQuestTrackerSlider(self, true);
end

function QGT_WatchFrameItem_OnLeave(self)
	GameTooltip:Hide();
	QGT_ShowQuestTrackerSlider(self, false);
end

function QGT_UpdateQuestTimers(...)
	local numTimers = select("#", ...);
 
	QGT_QuestTimer = {};
	if ( numTimers == 0 ) then
		return;
	end

	for i = 1, numTimers do
		QGT_QuestTimer[GetQuestIndexForTimer(i)] = select(i, ...);
	end
end

-- QuestWatch functions
function QGT_QuestWatch_Update()
	local numEntries, numQuests = GetNumQuestLogEntries();
	local inScenario = C_Scenario.IsInScenario(); --Scenario
	
	QGT_UpdateQuestTimers(GetQuestTimers());
	
	local questCount=0;
	local i;
	
	for i=1, numEntries do
		if (IsQuestWatched(i)) then questCount = questCount + 1; end
	end
	
	if inScenario then --Scenario
		QGT_QuestWatchNumQuests:SetText(SCENARIOS) --Scenario
	else --Scenario
    QGT_QuestWatchNumQuests:SetText(numQuests.."/"..MAX_QUESTS);
	end --Scenario

	if (QGT_Settings.QuestWatch.AutoMinimize) then
		QGT_QuestWatchFrameMinimize:SetText("*");
	else
		QGT_QuestWatchFrameMinimize:SetText("-");
	end
	
	local achieveCount=0;
	local i, j;
	
	for i, j in pairs(QGT_WatchAchievements) do
		achieveCount = achieveCount + 1;
	end

	if (achieveCount == 0) then
		QGT_QuestWatchFrameToggle:Hide();
	else
		QGT_QuestWatchFrameToggle:Show();
		if (QGT_Settings.BothTrackers) then
			QGT_QuestWatchFrameToggle:SetText("*");
		else
			QGT_QuestWatchFrameToggle:SetText(QG_TRACKER_A);
		end
	end
	
	--if (questCount == 0 and achieveCount == 0) then --Scenario
	if (questCount == 0 and achieveCount == 0 and not inScenario ) then --Scenario
		QGT_QuestWatchFrame:Hide();
		return;
	--elseif (questCount == 0 and achieveCount > 0) then --Scenario
	elseif (questCount == 0 and achieveCount > 0 and not inScenario ) then --Scenario
		if (QGT_Settings.BothTrackers) then
			QGT_QuestWatchFrame:Hide();
		else
			QGT_QuestWatchFrameToggle:Click();
		end
		return;
	else
		QGT_QuestWatchFrame:Show();
	end

	if (QGT_Settings.QuestWatch.Left == nil) then
		QGT_Settings.QuestWatch.Left = MinimapCluster:GetLeft();
	end
	if (QGT_Settings.QuestWatch.Top == nil) then
		QGT_Settings.QuestWatch.Top = MinimapCluster:GetBottom()+10;
	end
	if (QGT_Settings.QuestWatch.Bottom == nil) then
		QGT_Settings.QuestWatch.Bottom = 60;
	end

	if (QGT_Settings.ClickThrough) then
		QGT_SetQuestTrackerMouse(0);
	else
		QGT_SetQuestTrackerMouse(1);
	end
	
	if (QGT_Settings.Pin) then
		QGT_QuestWatchFrame:SetMovable(0);
	else
		QGT_QuestWatchFrame:SetMovable(1);
	end

	local numObjectives;
	local questWatchMaxWidth = 0;
	local tempWidth;
	local watchText;
	local text, type, finished;
	local questTitle;
	local watchTextIndex = 1;
	QGT_WatchLine[watchTextIndex] = {};
	local objectivesCompleted;
	local strLvl;
	local questIndex = 0;
	local currHeader = "";
	local lastWatchHeader = "";
	local lastWatchIndex = 0;
	local i, j = 0, 0;
	local numEntries, numQuests;
	
	if (QGT_WatchFlash == nil) then QGT_WatchFlash = {}; end

	--Scenario
	--if (QGT_Settings.QuestWatchSort == 0 or QGT_Settings.QuestWatchSort == nil or QGT_Settings == nil) then
	if inScenario then
		-- Scenarios have one "quest" per stage
		numEntries = 1
	elseif (QGT_Settings.QuestWatchSort == 0 or QGT_Settings.QuestWatchSort == nil or QGT_Settings == nil) then
	--ScenarioEnd
		numEntries, numQuests = GetNumQuestLogEntries();
	elseif (QGT_Settings.QuestWatchSort == 1) then
		numEntries = GetNumQuestWatches();
	end
	if (numEntries == nil) then return; end
	
	for questIndex=1, numEntries do
		local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily, numCriteria, stageName, stageDescription --Scenario
		if not inScenario then --Scenario
		if (QGT_Settings.QuestWatchSort == 1) then 
			questIndex = GetQuestIndexForWatch(questIndex);
		end
		questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex); --Scenario
		if (questLogTitleText == nil) then questLogTitleText = QG_UNKNOWN; end
		if (isHeader) then
		    currHeader = questLogTitleText;
			if (currHeader == nil) then currHeader = QG_UNKNOWN; end
		end
		--Scenario
		--if (IsQuestWatched(questIndex) and (not isHeader)) then
		else
			questLogTitleText = "QGT_SCENARIO_HEADER"
			level = UnitLevel("player")
		end
		if (inScenario or (IsQuestWatched(questIndex) and (not isHeader))) then
			if inScenario then
				local name, currentStage, numStages = C_Scenario.GetInfo();
				if currentStage > 0 and currentStage <= numStages then
					stageName, stageDescription, numCriteria = C_Scenario.GetStepInfo();
					local inChallengeMode = C_Scenario.IsChallengeMode();
					QGT_WatchLine[watchTextIndex].Text = "";
					QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=1, b=1};
					QGT_WatchLine[watchTextIndex].Tag = "";
					QGT_WatchLine[watchTextIndex].qID = "QGT_SCENARIO_HEADER";
					QGT_WatchLine[watchTextIndex].isHeader = false;
					QGT_WatchLine[watchTextIndex].isTitle = false;
					watchTextIndex = watchTextIndex + 1;
					QGT_WatchLine[watchTextIndex] = {};
					
					QGT_WatchLine[watchTextIndex].Tag = "";
					QGT_WatchLine[watchTextIndex].qID = "QGT_SCENARIO_HEADER";
					QGT_WatchLine[watchTextIndex].isHeader = false;
					QGT_WatchLine[watchTextIndex].isTitle = false;
					-- CMTitan: Drycoded after Blizz' WatchFrame.lua - to be checked once challenge modes are available
					if inChallengeMode then
						QGT_WatchLine[watchTextIndex].Text = stageName;
						QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=0.831, b=0.380};
					else
						if currentStage == numStages then
							QGT_WatchLine[watchTextIndex].Text = SCENARIO_STAGE_FINAL;
							QGT_WatchLine[watchTextIndex].TextColor = {r=0.769, g=0.643, b=0.2};
						else
							QGT_WatchLine[watchTextIndex].Text = string.format(SCENARIO_STAGE, currentStage);
							QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=0.914, b=0.682};
						end
						watchTextIndex = watchTextIndex + 1;
						QGT_WatchLine[watchTextIndex] = {};
						
						QGT_WatchLine[watchTextIndex].Text = stageName;
						QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=0.831, b=0.380};
						QGT_WatchLine[watchTextIndex].Tag = "";
						QGT_WatchLine[watchTextIndex].qID = "QGT_SCENARIO_HEADER";
						QGT_WatchLine[watchTextIndex].isHeader = true;
						QGT_WatchLine[watchTextIndex].isTitle = false;
					end
					watchTextIndex = watchTextIndex + 1;
					QGT_WatchLine[watchTextIndex] = {};
					
					QGT_WatchLine[watchTextIndex].Text = "";
					QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=1, b=1};
					QGT_WatchLine[watchTextIndex].Tag = "";
					QGT_WatchLine[watchTextIndex].qID = "QGT_SCENARIO_HEADER";
					QGT_WatchLine[watchTextIndex].isHeader = false;
					QGT_WatchLine[watchTextIndex].isTitle = false;
					lastWatchIndex = watchTextIndex;
					watchTextIndex = watchTextIndex + 1;
					QGT_WatchLine[watchTextIndex] = {};
				end
				currHeader = "QGT_SCENARIO_HEADER";
				-- Be sure to update flashing status before overwriting progress
				QGT_UIInfoMessage();
				QGT_ScenarioProgress.stage = currentStage;
				QGT_ScenarioProgress.criteria = {}
			else
		--ScenarioEnd
			if (currHeader ~= lastWatchHeader and QGT_Settings.ShowHeaders) then
				QGT_WatchLine[watchTextIndex].Text = currHeader;
				QGT_WatchLine[watchTextIndex].TextColor = {r=1, g=1, b=1};
				QGT_WatchLine[watchTextIndex].Tag = "";
				lastWatchHeader = currHeader;
				lastWatchIndex = watchTextIndex;
				QGT_WatchLine[watchTextIndex].qID = nil;
				QGT_WatchLine[watchTextIndex].isHeader = true;
				QGT_WatchLine[watchTextIndex].isTitle = false;
				watchTextIndex = watchTextIndex + 1;
				QGT_WatchLine[watchTextIndex] = {};
			end
			end --Scenario
			
			if ( QGT_WatchHeaders[currHeader] ~= false ) then
				local watchQuestIndex --Scenario
				if not inScenario then --Scenario
				-- Set title
				strLvl = "["..level
				if (isDaily) then
				    strLvl = strLvl.."Y";
				end
				if (questTag ~= nil) then
					if ((questTag == "Group") and (suggestedGroup > 0)) then
						strLvl = strLvl.."G"..suggestedGroup;
					elseif (questTag == "Dungeon") then
						strLvl = strLvl.."D";
					elseif ((questTag == "Elite") or (questTag == "Group")) then
						strLvl = strLvl.."+"
					else
						strLvl = strLvl.."+"
					end
				end
				strLvl = strLvl.."] "
				if (QGT_Settings.ShowLevels) then
					QGT_WatchLine[watchTextIndex].Text = strLvl..questLogTitleText;
				else
					QGT_WatchLine[watchTextIndex].Text = questLogTitleText;
				end
				-- Autocomplete
				if (isComplete and GetQuestLogIsAutoComplete(questIndex)) then
					QGT_WatchLine[watchTextIndex].Text = "(?) "..QGT_WatchLine[watchTextIndex].Text;
					QGT_WatchLine[watchTextIndex].flashing = true;
				end
				QGT_WatchLine[watchTextIndex].qID = questIndex;

				QGT_WatchLine[watchTextIndex].isHeader = false;
				QGT_WatchLine[watchTextIndex].isTitle = true;

				--local watchQuestIndex = watchTextIndex; --Scenario
				watchQuestIndex = watchTextIndex; --Scenario
				watchTextIndex = watchTextIndex + 1;
				QGT_WatchLine[watchTextIndex] = {};
				--Scenario
				else
					if C_Scenario.IsChallengeMode() then
						watchQuestIndex = 2;
					else
						watchQuestIndex = 3;
					end
				end
				--ScenarioEnd
				objectivesCompleted = 0;
				local questNumNeeded = 0;
				local questNumDone = 0;
				local r, g, b;

				--Scenario
				if inScenario then
					numObjectives = numCriteria;
					if not numObjectives then numObjectives = 0; end -- Corner case at the end of scenarios
				else
					numObjectives = GetNumQuestLeaderBoards(questIndex);
				end
				--ScenarioEnd
				--numObjectives = GetNumQuestLeaderBoards(questIndex); --Scenario
				for j=1, numObjectives do
				  --Scenario
					local numDone, numNeeded, itemName
					if inScenario then
						local flags, assetID, quantityString, criteriaID
						text, type, finished, numDone, numNeeded, flags, assetID, quantityString, criteriaID = C_Scenario.GetCriteriaInfo(j);
						-- CMTitan: Taking itemName from text is highly experimental, since we lack any scenario that requires gathering items.
						-- CMTitan: It might be that assetID is more appropriate, but this is to be tested once possible.
						itemName = text;
						if assetID then
							local iiName = GetItemInfo(assetID);
							if iiName then
								itemName = iiName;
							end
						end
						QGT_ScenarioProgress.criteria[j] = {};
						QGT_ScenarioProgress.criteria[j].finished = finished;
						QGT_ScenarioProgress.criteria[j].numDone = numDone;
					else
	 					text, type, finished = GetQuestLogLeaderBoard(j, questIndex);
					end
				  --scenarioEnd
					--text, type, finished = GetQuestLogLeaderBoard(j, questIndex); --Scenario
					if (text ~= nil) then
						--Scenario
						--local qi, qj, itemName, numDone, numNeeded = string.find(text, QG_ITEM_REQ_STR);
						local qi, qj
						if not inScenario then
							qi, qj, itemName, numDone, numNeeded = string.find(text, QG_ITEM_REQ_STR);
						--ScenarioEnd
						if (itemName == nil) then
							qi, qj, itemName, numDone = string.find(text, QG_ITEM_REQ_STR2);
							numNeeded = 1;
						end
  					end --Scenario						

						if ((type == "item") and itemName and IsAddOnLoaded("QuestGuru") and QuestGuru_Items[QuestGuru_RealmName]) then
							iiName, itemLink, itemRarity, itemLevel, itemMinLevel, itemType, itemSubType, itemStackCount, itemEquipLoc, texture = GetItemInfo(itemName);
							if ((iiName and itemLink) and QuestGuru_Items[QuestGuru_RealmName][iiName] == nil) then
								QuestGuru_Items[QuestGuru_RealmName][iiName] = itemLink;
							end
						end

						if ((not finished) or QGT_Settings.ShowCompletedObj) then
							-- Set Objective text
							if (((type == "item") or (type == "monster") or (type == "object")) and (itemName ~= nil)) then
								QGT_WatchLine[watchTextIndex].Text = " "..QGT_Settings.Bullet.." "..itemName;
								if (numDone == numNeeded) then
									QGT_WatchLine[watchTextIndex].Tag = COMPLETE;
									objectivesCompleted = objectivesCompleted + 1;
								else
									QGT_WatchLine[watchTextIndex].Tag = numDone.."/"..numNeeded;
								end
								r, g, b = QGT_QuestWatchGetObjColor(numDone/numNeeded);
								questNumNeeded = questNumNeeded + numNeeded;
								questNumDone = questNumDone + numDone;
						  --Scenario
							elseif (inScenario and numNeeded and numNeeded > 1 and numDone and numDone >= 0) then
								QGT_WatchLine[watchTextIndex].Text = " "..QGT_Settings.Bullet.." "..text;
								if (numDone == numNeeded) then
									QGT_WatchLine[watchTextIndex].Tag = COMPLETE;
									objectivesCompleted = objectivesCompleted + 1;
								else
									QGT_WatchLine[watchTextIndex].Tag = numDone.."/"..numNeeded;
								end
								r, g, b = QGT_QuestWatchGetObjColor(numDone/numNeeded);
								questNumNeeded = questNumNeeded + numNeeded;
								questNumDone = questNumDone + numDone;
						  --SecnarioEnd
							else
								QGT_WatchLine[watchTextIndex].Text = " "..QGT_Settings.Bullet.." "..text;
								QGT_WatchLine[watchTextIndex].Tag = "";
								questNumNeeded = questNumNeeded + 1;
								if (finished) then
									questNumDone = questNumDone + 1;
									objectivesCompleted = objectivesCompleted + 1;
									r, g, b = QGT_QuestWatchGetObjColor(1);
								else
									r, g, b = QGT_QuestWatchGetObjColor(0);
								end
							end
							QGT_WatchLine[watchTextIndex].qID = type;
							QGT_WatchLine[watchTextIndex].isHeader = false;
							QGT_WatchLine[watchTextIndex].isTitle = false;
							-- Color the objectives
							QGT_WatchLine[watchTextIndex].TextColor = {r=r,g=g,b=b};
							if (QGT_WatchQuests[questLogTitleText] == false) then
								if (QGT_WatchFlash and QGT_WatchFlash[questIndex] and QGT_WatchFlash[questIndex][j]) then
									QGT_WatchLine[watchQuestIndex].fading = QGT_WatchFlash[questIndex][j];
									QGT_WatchFlash[questIndex][j] = nil;
								end
								watchTextIndex = watchTextIndex - 1;
							else
								if (QGT_WatchFlash and QGT_WatchFlash[questIndex] and QGT_WatchFlash[questIndex][j]) then
									QGT_WatchLine[watchTextIndex].fading = QGT_WatchFlash[questIndex][j];
									QGT_WatchFlash[questIndex][j] = nil;
								end
							end
							watchTextIndex = watchTextIndex + 1;
							QGT_WatchLine[watchTextIndex] = {};
						elseif (finished) then
							objectivesCompleted = objectivesCompleted + 1;
							if ((type == "item") or (type == "monster") or (type == "object")) then
								questNumNeeded = questNumNeeded + numNeeded;
								questNumDone = questNumDone + numDone;
							else
								questNumNeeded = questNumNeeded + 1;
								questNumDone = questNumDone + 1;
							end
						end
					end
				end --for

				-- Brighten the quest title if all the quest objectives were met
				color = GetQuestDifficultyColor(level);
				if ( objectivesCompleted == numObjectives ) then
				  --QGT_Chat("QGT_WatchLine[watchQuestIndex].Tag", watchQuestIndex, nil)
					QGT_WatchLine[watchQuestIndex].Tag = COMPLETE;
					if not inScenario then --Scenario
					QGT_WatchLine[watchQuestIndex].TextColor = {r=color.r, g=color.g, b=color.b};
					end --Scenario
				else
					if (questNumNeeded > 0) then
						local perComplete = math.floor((questNumDone * 100) / questNumNeeded);
						QGT_WatchLine[watchQuestIndex].Tag = perComplete.."%";
					else
						QGT_WatchLine[watchQuestIndex].Tag = "";
					end
					if not inScenario then --Scenario
					QGT_WatchLine[watchQuestIndex].TextColor = {r=color.r * 0.75, g=color.g * 0.75, b=color.b * 0.75};
					end --Scenario
				end
			elseif (QGT_WatchFlash[questIndex]) then
				if (QGT_WatchFlash[questIndex].fading == true) then
					QGT_WatchLine[lastWatchIndex].fading = true;
					QGT_WatchFlash[questIndex].fading = nil;
				end
			end
		end
	end

	local watchLines = 30;
	if (QGT_Settings and QGT_Settings and QGT_Settings.Lines) then
		watchLines = QGT_Settings.Lines;
	end
	
	QGT_WatchLines = watchTextIndex - 1;
	if (QGT_WatchLines > watchLines) then
		QGT_QuestWatchFrameSlider:SetMinMaxValues(0, QGT_WatchLines - watchLines);
	else
		QGT_QuestWatchFrameSlider:SetMinMaxValues(0, 0);
	end

	if (QGT_Settings.QuestWatch.Minimized) then
		if (QGT_Settings.QuestWatch.AutoMinimize) then
			QGT_QuestWatchFrameMinimize:SetText("*");
		else
			QGT_QuestWatchFrameMinimize:SetText("+");
		end
		for i=1, 40 do
			_G["QGT_QuestWatchLine"..i]:Hide();
		end
		for i=1, QGT_WATCHFRAME_NUM_ITEMS do
			_G["QGT_WatchFrameItem"..i]:Hide();
		end
		QGT_QuestWatchFrame:SetHeight(26);

		if (QGT_Settings.QuestWatch.Left == nil) then
			QGT_Settings.QuestWatch.Left = MinimapCluster:GetLeft();
		end
		if (QGT_Settings.QuestWatch.Top == nil) then
			QGT_Settings.QuestWatch.Top = MinimapCluster:GetBottom()+10;
		end
		if (QGT_Settings.QuestWatch.Bottom == nil) then
			QGT_Settings.QuestWatch.Bottom = 60;
		end
		if (not QGT_QuestWatchFrame.isMoving) then
			QGT_QuestWatchFrame:ClearAllPoints();
			if (QGT_Settings.Anchor == "TOP") then
				QGT_QuestWatchFrame:SetPoint("TOPLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.QuestWatch.Left, QGT_Settings.QuestWatch.Top);
			elseif (QGT_Settings.Anchor == "BOTTOM") then
				QGT_QuestWatchFrame:SetPoint("BOTTOMLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.QuestWatch.Left, QGT_Settings.QuestWatch.Bottom);
			end
		end
		return;
	end

	local numHeaders = 0;
	local numTitles = 0;
	local watchItemIndex = 0;
	local oldWatchItems = QGT_WATCHFRAME_NUM_ITEMS;
	local sliderVal = QGT_QuestWatchFrameSlider:GetValue();
	local lastQuestWatch = 1;
	for i= 1, QGT_WatchLines do
		j = i - sliderVal;
		if (j > watchLines) then
			j = j - 1;
			break;
		end
		if (i > sliderVal) then
			watchText = _G["QGT_QuestWatchLine"..j];
			watchTextTag = _G["QGT_QuestWatchLine"..j.."Tag"];
			watchNormalText = _G["QGT_QuestWatchLine"..j.."NormalText"];
			watchTextHighlight = _G["QGT_QuestWatchLine"..j.."Highlight"];
			watchText:SetText(QGT_WatchLine[i].Text);
			if (QGT_WatchLine[i].TextColor == nil) then
				watchNormalText:SetTextColor(1, 1, 1);
			else
				watchNormalText:SetTextColor(QGT_WatchLine[i].TextColor.r,QGT_WatchLine[i].TextColor.g,QGT_WatchLine[i].TextColor.b);
			end
			watchText.qID = QGT_WatchLine[i].qID;
			watchTextTag:SetText(QGT_WatchLine[i].Tag);
			watchTextTag:Show();

			if (QGT_WatchLine[i].isHeader == true) then
				if ( j > 1 ) then
					watchText:SetPoint("TOPLEFT", "QGT_QuestWatchLine"..(j - 1), "BOTTOMLEFT", 0, -6);
					numHeaders = numHeaders + 1;
				end
				watchText:SetNormalTexture("Interface\\Buttons\\UI-SortArrow");
				watchNormalText:SetPoint("LEFT", "QGT_QuestWatchLine"..j, "LEFT", 8, 0);
				if (QGT_WatchLine[i].Text == nil) then QGT_WatchLine[i].Text = QG_UNKNOWN; end
				--Scenario
				--if (QGT_WatchHeaders[QGT_WatchLine[i].Text] ~= false) then
				--	QGT_WatchHeaders[QGT_WatchLine[i].Text] = true;
				local headerText
				if QGT_WatchLine[i].qID == "QGT_SCENARIO_HEADER" then
					headerText = "QGT_SCENARIO_HEADER";
				else
					headerText = QGT_WatchLine[i].Text;
				end
				if (QGT_WatchHeaders[headerText] ~= false) then
					QGT_WatchHeaders[headerText] = true;
				--ScenarioEnd
					_G["QGT_QuestWatchLine"..j.."Arrow"]:SetTexCoord(0, 0.5, 0, 1);
				else
					_G["QGT_QuestWatchLine"..j.."Arrow"]:SetTexCoord(0.5, 0, 0, 0, 0.5, 1, 0, 1);
				end
			elseif (QGT_WatchLine[i].isTitle == true) then
				if ( j > 1 ) then
					watchText:SetPoint("TOPLEFT", "QGT_QuestWatchLine"..(j - 1), "BOTTOMLEFT", 0, -4);
					numTitles = numTitles + 1;
				end
				watchText:SetNormalTexture("");
				watchNormalText:SetPoint("LEFT", "QGT_QuestWatchLine"..j, "LEFT", 0, 0);

				if (QGT_QuestTimer[watchText.qID]) then
					watchTextTag:SetText(SecondsToTime(QGT_QuestTimer[watchText.qID]));
					watchText:SetScript("OnUpdate",
						function (self)
							QGT_UpdateQuestTimers(GetQuestTimers());
							if (QGT_QuestTimer[self.qID]) then
								_G[self:GetName().."Tag"]:SetText(SecondsToTime(QGT_QuestTimer[self.qID]));
								local tempWidth = _G[self:GetName().."Tag"]:GetWidth() + 8;
								_G[self:GetName().."NormalText"]:SetWidth(240 - tempWidth);
							else
								self:SetScript("OnUpdate", nil);
							end
						end);
				else
					if (not QGT_Settings.ShowQuestPercent) then watchTextTag:Hide(); end
				end

				local link, item, charges = GetQuestLogSpecialItemInfo(watchText.qID);
				if ( item and QGT_Settings.QuestItemIcons and QGT_WatchLine[i].Tag ~= COMPLETE ) then
					watchItemIndex = watchItemIndex + 1;
					itemButton = _G["QGT_WatchFrameItem"..watchItemIndex];
					if ( not itemButton ) then
						QGT_WATCHFRAME_NUM_ITEMS = watchItemIndex;
						itemButton = CreateFrame("BUTTON", "QGT_WatchFrameItem" .. watchItemIndex, QGT_QuestWatchFrame, "WatchFrameItemButtonTemplate");
						itemButton:SetScript("OnLeave", QGT_WatchFrameItem_OnLeave);
					end
					itemButton:Show();
					itemButton:ClearAllPoints();
					itemButton:SetID(watchText.qID);
					SetItemButtonTexture(itemButton, item);
					SetItemButtonCount(itemButton, charges);
					itemButton.charges = charges;
					WatchFrameItem_UpdateCooldown(itemButton);
					itemButton.rangeTimer = -1;
					if ((QGT_Settings.QuestWatch.Left * QGT_Settings.Scale) < 32) then
						if (QGT_QuestWatchFrameSlider:GetAlpha() > 0) then
							itemButton:SetPoint("TOPLEFT", watchText, "TOPRIGHT", 16, 0);
						else
							itemButton:SetPoint("TOPLEFT", watchText, "TOPRIGHT", 4, 0);
						end
					else
						if ((QGT_QuestWatchFrameSlider:GetAlpha() > 0) and (((QGT_Settings.QuestWatch.Left + 256) * QGT_Settings.Scale) > (UIParent:GetWidth() - 16))) then
							itemButton:SetPoint("TOPRIGHT", watchText, "TOPLEFT", -16, 0);
						else
							itemButton:SetPoint("TOPRIGHT", watchText, "TOPLEFT", -4, 0);
						end
					end
				end
			else
				if (j > 1) then
					watchText:SetPoint("TOPLEFT", "QGT_QuestWatchLine"..(j - 1), "BOTTOMLEFT", 0, 0);
				end
				watchText:SetNormalTexture("");
				watchNormalText:SetPoint("LEFT", "QGT_QuestWatchLine"..j, "LEFT", 0, 0);
			end
			local tempWidth = watchTextTag:GetWidth() + 8;
			watchNormalText:SetWidth(240 - tempWidth);
			if (QGT_WatchLine[i].flashing) then
				watchText.flashing = 1;
				watchText:SetScript("OnUpdate", QGT_QuestWatchButton_OnUpdate);
				watchTextHighlight:SetAlpha(0);
				watchTextHighlight:Show();
			else
				watchText.flashing = 0;
				watchTextHighlight:SetAlpha(0);
				watchText:SetScript("OnUpdate", nil);
			end
			if (QGT_WatchLine[i].fading) then
				watchText.fading = 1;
				watchText:SetScript("OnUpdate", QGT_QuestWatchButton_OnUpdate);
				watchTextHighlight:SetAlpha(1);
				watchTextHighlight:Show();
			end
			watchText:Show();
			lastQuestWatch = j;
		end
	end

	QGT_QuestWatchFrame:Show();
	local tempHeight = 16 + (numHeaders*6)+(numTitles*4)+((lastQuestWatch+1)*13);
	QGT_QuestWatchFrame:SetHeight(tempHeight);
	QGT_QuestWatchFrameSlider:SetHeight(tempHeight-16);
	if (IsAddOnLoaded("MobMap") and (mobmap_hide_questtracker_buttons == false)) then
		QGT_QuestWatchFrame:SetWidth(276);
	else
		QGT_QuestWatchFrame:SetWidth(256);
	end
	if (not QGT_QuestWatchFrame.isMoving) then
		QGT_QuestWatchFrame:ClearAllPoints();
		if (QGT_Settings.Anchor == "TOP") then
			QGT_QuestWatchFrame:SetPoint("TOPLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.QuestWatch.Left, QGT_Settings.QuestWatch.Top);
			QGT_Settings.QuestWatch.Bottom = QGT_QuestWatchFrame:GetBottom();
		elseif (QGT_Settings.Anchor == "BOTTOM") then
			QGT_QuestWatchFrame:SetPoint("BOTTOMLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.QuestWatch.Left, QGT_Settings.QuestWatch.Bottom);
			QGT_Settings.QuestWatch.Top = QGT_QuestWatchFrame:GetTop();
		end
	end
	QGT_QuestWatchFrameSlider:ClearAllPoints();
	if (((QGT_Settings.QuestWatch.Left + 256) * QGT_Settings.Scale) > (UIParent:GetWidth() - 16)) then
		QGT_QuestWatchFrameSlider:SetPoint("TOPLEFT", -14, -16);
	else
		QGT_QuestWatchFrameSlider:SetPoint("TOPRIGHT", 14, -16);
	end
	-- Hide unused watch lines
	for i=j + 1, 40 do
		_G["QGT_QuestWatchLine"..i]:Hide();
	end
	for i=watchItemIndex+1, oldWatchItems do
	    _G["QGT_WatchFrameItem"..i]:Hide();
	end


	-- CMTitan: Disable MobMap support in scenarios (mainly since I haven't got a clue what it does) --Scenario
	if not inScenario then --Scenario
	if (IsAddOnLoaded("MobMap")) then
		for i=1, 40 do
			local frame = _G["QGT_QuestWatchLine"..i];
			local button = _G["MobMapQGTWatchButton"..i];
			if (frame:IsVisible() and (mobmap_hide_questtracker_buttons == false) and (frame.qID ~= nil)) then
				local questobj=frame:GetText();
				if(questobj) then
					if (button==nil) then
						button = CreateFrame("Frame","MobMapQGTWatchButton"..i, frame, "MobMapQuestButtonFrameTemplate");
						button:ClearAllPoints();
						button:SetPoint("RIGHT","QGT_QuestWatchLine"..i,"RIGHT",24,0);
						button:SetAlpha(0.6);
						button:SetFrameStrata("LOW");
						_G[button:GetName().."Button"]:SetScript("OnEnter",
							function (self)
								QGT_ShowQuestTrackerSlider(self, true);
								GameTooltip_AddNewbieTip(self, "MobMap",1.0,1.0,1.0,MOBMAP_QUEST_BUTTON_TOOLTIP);
							end);
						_G[button:GetName().."Button"]:SetScript("OnLeave",
							function (self)
								QGT_ShowQuestTrackerSlider(self, false);
								GameTooltip:Hide();
							end);
					end
					if (string.sub(questobj,1,2+strlen(QGT_Settings.Bullet))==" "..QGT_Settings.Bullet.." ") then
						button.questobj=string.sub(questobj,3+strlen(QGT_Settings.Bullet));
						button.questtitle=nil;
						button.unknowntext=nil;
						button:Show();
					else
						local title=questobj;
						if(title~=nil) then
							local filteredtitle=string.match(title,".*%] (.*)");
							if(filteredtitle~=nil) then
								title=filteredtitle;
							end
						end
						button.questtitle=title;
						button.questobj=nil;
						button.unknowntext=nil;
						button:Show();
					end
				end
			else
				if (button) then
					button:Hide();
				end
			end
		end
	end
	end --Scenario
end

function QGT_QuestWatchGetObjColor(progress, alwaysColor)
	local r, g, b;
	if (QGT_Settings.ColorizeObj or alwaysColor) then
		if (progress == 1) then
			r = QGT_Settings.ColorizeObjComplete.r;
			g = QGT_Settings.ColorizeObjComplete.g;
			b = QGT_Settings.ColorizeObjComplete.b;
		else
			r = QGT_Settings.ColorizeObjZero.r + ((QGT_Settings.ColorizeObjFull.r - QGT_Settings.ColorizeObjZero.r) * progress);
			g = QGT_Settings.ColorizeObjZero.g + ((QGT_Settings.ColorizeObjFull.g - QGT_Settings.ColorizeObjZero.g) * progress);
			b = QGT_Settings.ColorizeObjZero.b + ((QGT_Settings.ColorizeObjFull.b - QGT_Settings.ColorizeObjZero.b) * progress);
		end
	else
		if (progress == 1) then
			r = HIGHLIGHT_FONT_COLOR.r;
			g = HIGHLIGHT_FONT_COLOR.g;
			b = HIGHLIGHT_FONT_COLOR.b;
		else
			r = GRAY_FONT_COLOR.r;
			g = GRAY_FONT_COLOR.g;
			b = GRAY_FONT_COLOR.b;
	    end
	end
	return r, g, b;
end

function QGT_AddQuestWatch(questIndex, watchTime)
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
	if (isHeader) then
		QGT_AddQuestWatchHeader(questLogTitleText);
		return false;
	end
	if (QGT_WatchList[questLogTitleText] == nil) then
		QGT_WatchList[questLogTitleText] = {};
	end
	if (QGT_WatchList[questLogTitleText].Watched) then
		return true;
	end
	QGT_WatchList[questLogTitleText].Watched = true;
	return true;
end
hooksecurefunc("AddQuestWatch", QGT_AddQuestWatch);

function QGT_AddQuestWatchHeader(questHeader)
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

function QGT_RemoveQuestWatch(questIndex, ...)
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
	if (isHeader) then
		QGT_RemoveQuestWatchHeader(questLogTitleText);
	end
	if (questLogTitleText) then
		QGT_WatchList[questLogTitleText] = nil;
		QGT_WatchQuests[questLogTitleText] = nil;
	end
	WatchFrame_Update();
end
hooksecurefunc("RemoveQuestWatch", QGT_RemoveQuestWatch);

function QGT_RemoveQuestWatchHeader(questHeader)
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

local old_IsQuestWatched = IsQuestWatched;
function IsQuestWatched(questIndex, ...)
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questIndex);
	if (QGT_WatchList[questLogTitleText] == nil) then
		return false;
	end
	return QGT_WatchList[questLogTitleText].Watched;
end

function QGT_OnUpdate(elapsed)
	QGT_CurrTime = QGT_CurrTime + elapsed;
	if (QGT_CurrTime < 1) then return; end

	local i, timer;
	
	for i, timer in ipairs(QGT_Timer) do
	    timer.currTime = timer.currTime - 1;  -- countdown, in seconds, to zero
	    if (timer.currTime == 0) then
	        timer.func(timer.arg1, timer.arg2, timer.arg3, timer.arg4);
	        timer.currTime = timer.interval;
	        timer.count = timer.count - 1;
	        if (timer.count < 0) then
	            timer.count = -1;
			elseif (timer.count == 0) then
			    tremove(QGT_Timer, i);
			    i = i - 1;
			end
		end
	end
	
	QGT_CurrTime = 0;
end

function QGT_AddTimer(interval, count, func, arg1, arg2, arg3, arg4)
--[[
 interval is the amount to time to wait before the timer function is called in seconds
 count is the number of times to call the timer function, use -1 if always is wanted
 func is the reference of the function to be called
 arg1 - arg4 are optional and will be passed to the function:  func(arg1, arg2, arg3, arg4)
]]
	local tempTimer = {}
	tempTimer.interval = interval;
	tempTimer.currTime = interval;
	tempTimer.count = count;
	tempTimer.func = func;
	tempTimer.arg1 = arg1;
	tempTimer.arg2 = arg2;
	tempTimer.arg3 = arg3;
	tempTimer.arg4 = arg4;
	tinsert(QGT_Timer, tempTimer);
end

function QGT_UIInfoMessage(arg1)
	--Scenario
	if C_Scenario.IsInScenario() and QGT_ScenarioProgress then
		local name, currentStage, numStages = C_Scenario.GetInfo();
		if currentStage > 0 and currentStage <= numStages and QGT_ScenarioProgress.stage == currentStage and QGT_ScenarioProgress.criteria then
			local stageName, stageDescription, numCriteria = C_Scenario.GetStepInfo();
			for i = 1, numCriteria do
				local criteriaString, criteriaType, criteriaCompleted, quantity, totalQuantity, flags, assetID, quantityString, criteriaID = C_Scenario.GetCriteriaInfo(i);
				if QGT_ScenarioProgress.criteria[i] and ((QGT_ScenarioProgress.criteria[i].finished ~= criteriaCompleted) or (quantity and quantity > 0 and QGT_ScenarioProgress.criteria[i].numDone ~= quantity)) then
					if (QGT_WatchFlash[1] == nil) then QGT_WatchFlash[1] = {}; end
					QGT_WatchFlash[1].fading = true;
					QGT_WatchFlash[1][i] = true;
				end
			end
		end
		return;
	end
	--ScenarioEnd
	local questID=0;
	local numEntries, numQuests = GetNumQuestLogEntries();
	local i, ii, jj, itemName, numDone, numNeeded;
	local questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily;

	if (arg1 == nil) then return; end

	for questID=1, numEntries, 1 do
		questLogTitleText, level, questTag, suggestedGroup, isHeader, isCollapsed, isComplete, isDaily = GetQuestLogTitle(questID);
		local numObjectives = GetNumQuestLeaderBoards(questID);

		if (numObjectives ~= nil) then
			for i=1, numObjectives, 1 do
				local text, type, finished = GetQuestLogLeaderBoard(i,questID);
				if (text ~= nil) then
					local qi, qj, qitemName, qnumDone, qnumNeeded = string.find(text, QG_ITEM_REQ_STR);
					if (qitemName == nil) then
						qi, qj, qitemName, qnumDone = string.find(text, QG_ITEM_REQ_STR2);
						qnumNeeded = 1;
					end
					if ((type == "item") or (type == "monster") or (type == "object")) then
						if (type == "object") then type = "item"; end
						ii, jj, itemName, numDone, numNeeded = string.find(arg1, QG_ITEM_REQ_STR);
						if ((itemName == qitemName) and (numNeeded == qnumNeeded)) then
							if (QGT_WatchFlash[questID] == nil) then QGT_WatchFlash[questID] = {}; end
							QGT_WatchFlash[questID].fading = true;
							QGT_WatchFlash[questID][i] = true;
						end
					elseif (type == "event") then
						itemName = text;
						numDone=1;
						numNeeded=1;
						if (arg1 == text.." ("..COMPLETE..")") then
							if (QGT_WatchFlash[questID] == nil) then QGT_WatchFlash[questID] = {}; end
							QGT_WatchFlash[questID].fading = true;
							QGT_WatchFlash[questID][i] = true;
						end
					end
				end
			end
		end
	end
end

function QGT_RewardComplete_Click()
	local qTitle = GetTitleText();

	if (qTitle) then
		QGT_WatchList[qTitle] = nil;
		QGT_WatchQuests[qTitle] = nil;
	end
end
hooksecurefunc("QuestRewardCompleteButton_OnClick", QGT_RewardComplete_Click);

function QGT_Abandon()
	SetAbandonQuest();
	local items = GetAbandonQuestItems();
	if ( items ) then
		StaticPopup_Hide("ABANDON_QUEST");
		StaticPopup_Show("ABANDON_QUEST_WITH_ITEMS", GetAbandonQuestName(), items);
	else
		StaticPopup_Hide("ABANDON_QUEST_WITH_ITEMS");
		StaticPopup_Show("ABANDON_QUEST", GetAbandonQuestName());
	end
end

function QGT_AbandonQuest()
	if (QGT_LastAbandonQuestTitle) then
		QGT_WatchList[QGT_LastAbandonQuestTitle] = nil;
		QGT_WatchQuests[QGT_LastAbandonQuestTitle] = nil;
	end
end
hooksecurefunc("AbandonQuest", QGT_AbandonQuest);

function QGT_SetAbandonQuest()
	QGT_LastAbandonQuestTitle = GetAbandonQuestName();
end
hooksecurefunc("SetAbandonQuest", QGT_SetAbandonQuest);
