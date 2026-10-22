QGT_WatchAchievements = {};
QGT_WatchAchievementLines = 0;
QGT_AchievementTimer = {};

--QuestGuru_Echo("At set AWU false initial QGT_AchievementWatch_Update()", false)
QGT_AWUpdating = false;

WATCHFRAME_MAXACHIEVEMENTS = 9999;

function QGT_SetAchievementWatchBorder(enabled)
	if (enabled) then
		QGT_AchievementWatchFrame:SetBackdrop({
			bgFile="Interface\\Characterframe\\UI-Party-Background",
			edgeFile="Interface\\Tooltips\\UI-Tooltip-Border",
			tile=1, tileSize=16, edgeSize=16,
			insets={left=4, right=4, top=4, bottom=4}
		});
	else
		QGT_AchievementWatchFrame:SetBackdrop({
			bgFile="Interface\\Characterframe\\UI-Party-Background",
			tile=1, tileSize=16,
			insets={left=4, right=4, top=4, bottom=4}
		});
	end
	QGT_AchievementWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
end

do
	local temp, i;

	QGT_AchievementWatchFrame = CreateFrame("FRAME", "QGT_AchievementWatchFrame", UIParent);
	QGT_AchievementWatchFrame:Hide();
	QGT_AchievementWatchFrame:EnableMouse(1);
	QGT_AchievementWatchFrame:EnableMouseWheel(1);
	QGT_AchievementWatchFrame:SetMovable(1);
	QGT_AchievementWatchFrame:SetResizable(0);
	QGT_AchievementWatchFrame:SetToplevel(1);
	QGT_AchievementWatchFrame:SetFrameStrata("LOW");
	QGT_AchievementWatchFrame:SetClampedToScreen(true);
	QGT_AchievementWatchFrame:SetWidth(256);
	QGT_AchievementWatchFrame:SetHeight(20);
	QGT_AchievementWatchFrame:SetPoint("TOPRIGHT", MinimapCluster, "BOTTOMRIGHT", 0, 10);
	QGT_AchievementWatchFrame:SetHitRectInsets(-16, 0, 0, 0);

	QGT_SetAchievementWatchBorder(true);

	QGT_AchievementWatchFrame:RegisterForDrag("LeftButton");
	QGT_AchievementWatchFrame:SetScript("OnDragStart",
		function ()
			if (not QGT_Settings.Pin) then
				QGT_AchievementWatchFrame:StartMoving();
				QGT_AchievementWatchFrame.isMoving = true;
			end
		end);
 	QGT_AchievementWatchFrame:SetScript("OnDragStop",
	 	function ()
			QGT_AchievementWatchFrame:StopMovingOrSizing();
			QGT_AchievementWatchFrame.isMoving = false;
			QGT_Settings.AchievementWatch.Left = QGT_AchievementWatchFrame:GetLeft();
			QGT_Settings.AchievementWatch.Top = QGT_AchievementWatchFrame:GetTop();
			QGT_Settings.AchievementWatch.Bottom = QGT_AchievementWatchFrame:GetBottom();
			QGT_AchievementWatchFrameSlider:ClearAllPoints();
			if (((QGT_Settings.AchievementWatch.Left + 256) * QGT_Settings.Scale) > (UIParent:GetWidth() - 16)) then
				QGT_AchievementWatchFrameSlider:SetPoint("TOPLEFT", -14, -16);
			else    
				QGT_AchievementWatchFrameSlider:SetPoint("TOPRIGHT", 14, -16);
			end
		end);
	QGT_AchievementWatchFrame:SetScript("OnEnter",
		function (self)
			QGT_AchievementWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowAchievementTrackerSlider(self, true);
		end);
	QGT_AchievementWatchFrame:SetScript("OnLeave",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, false);
		end);
	QGT_AchievementWatchFrame:SetScript("OnMouseWheel",
		function (self, delta)
			local min, max = QGT_AchievementWatchFrameSlider:GetMinMaxValues();
			local currVal = QGT_AchievementWatchFrameSlider:GetValue();

			currVal = currVal - delta;
			if (currVal < min) then currVal = min; end
			if (currVal > max) then currVal = max; end
			QGT_AchievementWatchFrameSlider:SetValue(currVal);
		end);
	QGT_AchievementWatchFrame:SetScript("OnShow",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, false);
		end);
	QGT_AchievementWatchFrame:RegisterEvent("ACHIEVEMENT_EARNED");
	QGT_AchievementWatchFrame:RegisterEvent("VARIABLES_LOADED");
	QGT_AchievementWatchFrame:RegisterEvent("TRACKED_ACHIEVEMENT_UPDATE");
	QGT_AchievementWatchFrame:SetScript("OnEvent",
		function (self, event, achievementID, ...)
			if (event == "ACHIEVEMENT_EARNED") then
				RemoveTrackedAchievement(achievementID);
			elseif (event == "TRACKED_ACHIEVEMENT_UPDATE") then
				local criteriaID, elapsed, duration = ...;
 
				if ( not elapsed or not duration ) then
					-- Don't do anything
				elseif ( elapsed >= duration ) then
					QGT_AchievementTimer[achievementID] = nil;
				else
					QGT_AchievementTimer[achievementID] = {};
					QGT_AchievementTimer[achievementID].startTime = GetTime() - elapsed;
					QGT_AchievementTimer[achievementID].duration = duration;
				end
			elseif (event == "VARIABLES_LOADED") then
				QGT_SetAchievements();
			end
		end);
	QGT_AchievementWatchFrame:SetScript("OnUpdate",
		function (self, elapsed)
			QGT_AchievementWatch_OnUpdate(elapsed);
		end);

	temp = QGT_AchievementWatchFrame:CreateTexture("QGT_AchievementWatchFrameBackground", "ARTWORK");
	temp:SetHeight(18);
	temp:SetPoint("TOPRIGHT", -4, -4);
	temp:SetPoint("TOPLEFT", 4, -4);
	temp:SetTexture(1, 1, 1);
	temp:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, 1);

	temp = QGT_AchievementWatchFrame:CreateFontString("QGT_AchievementWatchName", "ARTWORK", "GameFontNormal");
	temp:SetPoint("TOPLEFT", 8, -6);
	temp:SetJustifyH("LEFT");
	temp:SetWidth(0);
	temp:SetHeight(12);
	temp:SetText("QuestGuru "..QG_TRACKER_ACHIEVE);

	temp = QGT_AchievementWatchFrame:CreateFontString("QGT_AchievementWatchNum", "ARTWORK", "GameFontNormal");
	temp:SetPoint("LEFT", QGT_AchievementWatchName, "RIGHT", 12, 0);
	temp:SetJustifyH("CENTER");
	temp:SetWidth(0);
	temp:SetHeight(12);
	temp:SetText("");

	QGT_AchievementWatchFrameOptions = CreateFrame("BUTTON", "QGT_AchievementWatchFrameOptions", QGT_AchievementWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_AchievementWatchFrameOptions:SetText("O");
	QGT_AchievementWatchFrameOptions:SetPoint("TOPRIGHT", -5, -4);
	QGT_AchievementWatchFrameOptions:SetScript("OnClick",
		function ()
			InterfaceOptionsFrame_OpenToCategory("QG Tracker");
		end);
	QGT_AchievementWatchFrameOptions:SetScript("OnEnter",
		function (self)
			QGT_AchievementWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowAchievementTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			GameTooltip:SetText(QG_TRACKER_OPTIONS, nil,nil, nil, nil, 1);
		end);
	QGT_AchievementWatchFrameOptions:SetScript("OnLeave",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_AchievementWatchFrameMinimize = CreateFrame("BUTTON", "QGT_AchievementWatchFrameMinimize", QGT_AchievementWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_AchievementWatchFrameMinimize:SetText("-");
	QGT_AchievementWatchFrameMinimize:SetPoint("RIGHT", QGT_AchievementWatchFrameOptions, "LEFT", 0, 0);
	QGT_AchievementWatchFrameMinimize:RegisterForClicks("LeftButtonUp", "RightButtonUp");
	QGT_AchievementWatchFrameMinimize:SetScript("OnClick",
		function (self, button, down)
			if (button == "LeftButton") then
				if (QGT_Settings.AchievementWatch.AutoMinimize) then
					QGT_ShowAchievementTrackerSlider(self, false);
					QGT_Settings.AchievementWatch.Minimized = true;
				else
					QGT_ShowAchievementTrackerSlider(self, QGT_Settings.AchievementWatch.Minimized);
					QGT_Settings.AchievementWatch.Minimized = not QGT_Settings.AchievementWatch.Minimized;
				end
				QGT_Settings.AchievementWatch.AutoMinimize = false;
				WatchFrame_Update();
				GameTooltip:Hide();
			elseif (button == "RightButton") then
				QGT_Settings.AchievementWatch.AutoMinimize = not QGT_Settings.AchievementWatch.AutoMinimize;
				WatchFrame_Update();
			end
		end);
	QGT_AchievementWatchFrameMinimize:SetScript("OnEnter",
		function (self)
		    QGT_AchievementWatchFrameSlider:SetScript("OnMouseUp", nil);
		    QGT_ShowAchievementTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			if (QGT_Settings.AchievementWatch.Minimized) then
				GameTooltip:SetText(QG_TRACKER_SHOW, nil,nil, nil, nil, 1);
			else
				GameTooltip:SetText(QG_TRACKER_MINIMIZE, nil,nil, nil, nil, 1);
			end
		end);
	QGT_AchievementWatchFrameMinimize:SetScript("OnLeave",
		function ()
			QGT_ShowAchievementTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_AchievementWatchFrameToggle = CreateFrame("BUTTON", "QGT_AchievementWatchFrameToggle", QGT_AchievementWatchFrame, "QGT_QuestWatchMiniButtonTemplate");
	QGT_AchievementWatchFrameToggle:SetText(QG_TRACKER_Q);
	QGT_AchievementWatchFrameToggle:SetPoint("RIGHT", QGT_AchievementWatchFrameMinimize, "LEFT", 0, 0);
	QGT_AchievementWatchFrameToggle:RegisterForClicks("LeftButtonUp", "RightButtonUp");
	QGT_AchievementWatchFrameToggle:SetScript("OnClick",
		function (self, button, down)
			if (button == "LeftButton") then
				QGT_Settings.LastTracker = "Q";
				QGT_Settings.BothTrackers = false;
				QGT_Settings.QuestWatch.Top = QGT_Settings.AchievementWatch.Top;
				QGT_Settings.QuestWatch.Bottom = QGT_Settings.AchievementWatch.Bottom;
				QGT_Settings.QuestWatch.Left = QGT_Settings.AchievementWatch.Left;
				QGT_Settings.QuestWatch.Minimized = QGT_Settings.AchievementWatch.Minimized;
				QGT_Settings.QuestWatch.AutoMinimize = QGT_Settings.AchievementWatch.AutoMinimize;
				WatchFrame_Update();
				GameTooltip:Hide();
			elseif (button == "RightButton") then
				QGT_Settings.LastTracker = "A";
				QGT_Settings.BothTrackers = not QGT_Settings.BothTrackers;
				WatchFrame_Update();
			end
		end);
	QGT_AchievementWatchFrameToggle:SetScript("OnEnter",
		function (self)
			QGT_AchievementWatchFrameSlider:SetScript("OnMouseUp", nil);
			QGT_ShowAchievementTrackerSlider(self, true);
			GameTooltip:SetOwner(self, "ANCHOR_LEFT");
			GameTooltip:SetText(QG_TRACKER_TOGGLE, nil,nil, nil, nil, 1);
		end);
	QGT_AchievementWatchFrameToggle:SetScript("OnLeave",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, false);
			GameTooltip:Hide();
		end);

	QGT_AchievementWatchFrameSlider = CreateFrame("Slider", "QGT_AchievementWatchFrameSlider", QGT_AchievementWatchFrame, "OptionsSliderTemplate");
	QGT_AchievementWatchFrameSlider:SetWidth(16);
	QGT_AchievementWatchFrameSlider:SetHeight(200);
	QGT_AchievementWatchFrameSliderText:SetText("");
	QGT_AchievementWatchFrameSliderHigh:SetText("");
	QGT_AchievementWatchFrameSliderLow:SetText("");
	QGT_AchievementWatchFrameSlider:SetOrientation("VERTICAL");
	QGT_AchievementWatchFrameSlider:SetPoint("TOPLEFT", QGT_AchievementWatchFrame, "TOPLEFT", -14, -16);
	QGT_AchievementWatchFrameSlider:SetMinMaxValues(0,0);
	QGT_AchievementWatchFrameSlider:SetValueStep(1);
	QGT_AchievementWatchFrameSlider:SetValue(0);
	QGT_AchievementWatchFrameSlider:Hide();
	QGT_AchievementWatchFrameSlider:SetScript("OnEnter",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, true);
		end);
	QGT_AchievementWatchFrameSlider:SetScript("OnLeave",
		function (self)
			QGT_ShowAchievementTrackerSlider(self, false);
		end);
	QGT_AchievementWatchFrameSlider:SetScript("OnValueChanged",
		function ()
			WatchFrame_Update();
		end);
	
	for i=1, 40 do
		temp = CreateFrame("Button", "QGT_AchievementWatchLine"..i, QGT_AchievementWatchFrame, "QGT_AchievementWatchButtonTemplate");
		temp.statusBar = _G["QGT_AchievementWatchLine"..i.. "StatusBar"]
		temp:SetPoint("TOPLEFT", QGT_AchievementWatchFrame, 8, -22);
		temp:SetHeight(13);
		temp:SetWidth(240);
		temp:RegisterForClicks("LeftButtonUp");
		temp:SetScript("OnEnter",
			function (self)
				_G[self:GetName().."Highlight"]:SetAlpha(0.5);
				_G[self:GetName().."Highlight"]:Show();
				if (self.achievementID) then
					GameTooltip:SetOwner(self);
					GameTooltip:SetHyperlink(GetAchievementLink(self.achievementID));
					GameTooltip:Show()
					local tW = GameTooltip:GetWidth();
					if (QGT_Settings.AchievementWatch.Left < tW) then
						GameTooltip:SetOwner(self, "ANCHOR_RIGHT");
						GameTooltip:SetHyperlink(GetAchievementLink(self.achievementID));
						GameTooltip:Show()
					else
						GameTooltip:SetOwner(self, "ANCHOR_LEFT");
						GameTooltip:SetHyperlink(GetAchievementLink(self.achievementID));
						GameTooltip:Show()
					end
				end
				QGT_ShowAchievementTrackerSlider(self, true);
			end);
		temp:SetScript("OnLeave",
			function (self)
				_G[self:GetName().."Highlight"]:Hide();
				GameTooltip:Hide();
				QGT_ShowAchievementTrackerSlider(self, false);
			end);
		temp:SetScript("OnClick",
			function (self, button, down)
				if (self.achievementID) then
					if (IsShiftKeyDown()) then
						local activeWindow = ChatEdit_GetActiveWindow();
						if (activeWindow) then
							activeWindow:Insert(GetAchievementLink(self.achievementID));
						else
							RemoveTrackedAchievement(self.achievementID);
						end
					else
						AchievementFrame_LoadUI();
						AchievementFrame:Show();
						AchievementFrame_SelectAchievement(self.achievementID);
					end
				end
			end);
	end

	QGT_AchievementWatchFrameTooltip = CreateFrame("GameTooltip", "QGT_AchievementWatchFrameTooltip", QGT_AchievementWatchFrame, "GameTooltipTemplate");
	QGT_AchievementWatchFrameTooltip:Hide();
	QGT_AchievementWatchFrameTooltip:SetFrameStrata("TOOLTIP");
end

function QGT_ShowAchievementTrackerSlider(self, flag)
	if (QGT_Settings and QGT_Settings.AchievementWatch and (QGT_Settings.AchievementWatch.AutoMinimize==true) and not IsMouseButtonDown("LeftButton")) then
		QGT_Settings.AchievementWatch.Minimized = not flag;
	end

	local watchLines = 30;
	if (QGT_Settings and QGT_Settings.Lines) then
		watchLines = QGT_Settings.Lines;
	end
	if ((flag == true) and (QGT_WatchAchievementLines > watchLines) and not QGT_Settings.AchievementWatch.Minimized) then
		QGT_AchievementWatchFrameSlider:Show();
	else
		if (IsMouseButtonDown("LeftButton")) then
			self:SetScript("OnMouseUp",
				function (self)
					QGT_ShowAchievementTrackerSlider(self, false);
					self:SetScript("OnMouseUp", nil);
				end);
		else
			QGT_AchievementWatchFrameSlider:Hide();
		end
	end
end

function QGT_SetAchievementTrackerMouse(enableFlag)
	QGT_AchievementWatchFrame:EnableMouse(enableFlag);
	for i=1, 40 do
		_G["QGT_AchievementWatchLine"..i]:EnableMouse(enableFlag);
	end
end

-- QuestWatch functions
function QGT_AchievementWatch_Update()
--QuestGuru_Echo("Entered QGT_AchievementWatch_Update()", false)
	local achieveCount=0;
	local i, j;
	
--QuestGuru_Echo("At AWU test Before QGT_AchievementWatch_Update()", false)
	if (QGT_AWUpdating == true) then return; end
--QuestGuru_Echo("At AWU test After QGT_AchievementWatch_Update()", false)
	QGT_AWUpdating = true;

--QuestGuru_Echo("At 1 QGT_AchievementWatch_Update()", false)
	
	table.sort(QGT_WatchAchievements, function(a, b) return (a < b); end);
	for i, j in pairs(QGT_WatchAchievements) do
		achieveCount = achieveCount + 1;
	end

	QGT_AchievementWatchNum:SetText(achieveCount);

--QuestGuru_Echo(format("QGT_Settings.AchievementWatch.AutoMinimize: %s", tostring(QGT_Settings.AchievementWatch.AutoMinimize)), true)

	if (QGT_Settings.AchievementWatch.AutoMinimize) then
		QGT_AchievementWatchFrameMinimize:SetText("*");
	else
		QGT_AchievementWatchFrameMinimize:SetText("-");
	end

	local numEntries, numQuests = GetNumQuestLogEntries();
	local questCount=0;
	
	for i=1, numEntries do
		if (IsQuestWatched(i)) then questCount = questCount + 1; end
	end

	if (questCount == 0) then
		QGT_AchievementWatchFrameToggle:Hide();
	else
		QGT_AchievementWatchFrameToggle:Show();
		if (QGT_Settings.BothTrackers) then
			QGT_AchievementWatchFrameToggle:SetText("*");
		else
			QGT_AchievementWatchFrameToggle:SetText(QG_TRACKER_Q);
		end
	end
	
	if (achieveCount == 0 and questCount == 0) then
		QGT_AchievementWatchFrame:Hide();
--QuestGuru_Echo("At set AWU false A=0 Q=0 QGT_AchievementWatch_Update()", false)
		QGT_AWUpdating = false;
		return;
	elseif (achieveCount == 0 and questCount > 0) then
		if (QGT_Settings.BothTrackers) then
			QGT_AchievementWatchFrame:Hide();
		else
			QGT_AchievementWatchFrameToggle:Click();
		end
--QuestGuru_Echo("At set AWU false A=0 Q>0 QGT_AchievementWatch_Update()", false)
		QGT_AWUpdating = false;
		return;
	else
		QGT_AchievementWatchFrame:Show();
	end

	if (QGT_Settings.AchievementWatch.Left == nil) then
		QGT_Settings.AchievementWatch.Left = MinimapCluster:GetLeft();
	end
	if (QGT_Settings.AchievementWatch.Top == nil) then
		QGT_Settings.AchievementWatch.Top = MinimapCluster:GetBottom()+10;
	end
	if (QGT_Settings.AchievementWatch.Bottom == nil) then
		QGT_Settings.AchievementWatch.Bottom = 60;
	end

	if (QGT_Settings.ClickThrough) then
		QGT_SetAchievementTrackerMouse(0);
	else
		QGT_SetAchievementTrackerMouse(1);
	end
	
	if (QGT_Settings.Pin) then
		QGT_AchievementWatchFrame:SetMovable(0);
	else
		QGT_AchievementWatchFrame:SetMovable(1);
	end

--QuestGuru_Echo("At 2 QGT_AchievementWatch_Update()", false)

	local _;
	local self = QGT_AchievementWatchFrame;

	local numTrackedAchievements = achieveCount;
 
	local maxWidth = 0;
	local heightUsed = 0;
	local heightNeeded = 0;
 
	local achievementID;
	local achievementName, completed, description, icon;
 
	local line;
	local currLine = 0;
	local achievementTitle;
	local previousLine;
	local nextXOffset = 0;
	local linkButton;
 
	local numCriteria, criteriaDisplayed;
	local criteriaString, criteriaType, criteriaCompleted, quantity, totalQuantity, name, flags, assetID, quantityString, criteriaID, achievementCategory;
	
	local achieveLine = {};
	achieveLine[currLine] = {};
	
	for i = 1, 40 do
    --QuestGuru_Echo(format("/qgt %s", tostring(SlashText)), true)
    --QGT_Chat has been removed.
    --QGT_Chat("i was nil", i, nil)
		local aLine = _G["QGT_AchievementWatchLine"..i];
    --QGT_Chat("i was nil with aLine", i, aLine)
		--local aLineIcon = _G["QGT_AchievementWatchLine"..i.."Icon"];
    --QGT_Chat("i was nil with aLineIcon", i, aLineIcon)
		aLine.text:SetText("");
		aLine.text:SetTextColor(1.0, 1.0, 1.0);
		aLine.statusBar:Hide();
		aLine.achievementID = nil;
		--aLineIcon:SetTexture("");
	end

	if (QGT_Settings.AchievementWatch.Minimized) then
		if (QGT_Settings.AchievementWatch.AutoMinimize) then
			QGT_AchievementWatchFrameMinimize:SetText("*");
		else
			QGT_AchievementWatchFrameMinimize:SetText("+");
		end
		QGT_AchievementWatchFrame:SetHeight(26);
	
		for i = 1, 40 do
      --QGT_Chat("i was nil", i, nil)
			local aLine = _G["QGT_AchievementWatchLine"..i];
			--local aLineIcon = _G["QGT_AchievementWatchLine"..i.."Icon"];
			--aLineIcon:Hide();
			aLine:Hide();
		end

		if (QGT_Settings.AchievementWatch.Left == nil) then
			QGT_Settings.AchievementWatch.Left = MinimapCluster:GetLeft();
		end
		if (QGT_Settings.AchievementWatch.Top == nil) then
			QGT_Settings.AchievementWatch.Top = MinimapCluster:GetBottom()+10;
		end
		if (QGT_Settings.AchievementWatch.Bottom == nil) then
			QGT_Settings.AchievementWatch.Bottom = 60;
		end
		if (not QGT_AchievementWatchFrame.isMoving) then
			QGT_AchievementWatchFrame:ClearAllPoints();
			if (QGT_Settings.Anchor == "TOP") then
				QGT_AchievementWatchFrame:SetPoint("TOPLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.AchievementWatch.Left, QGT_Settings.AchievementWatch.Top);
			elseif (QGT_Settings.Anchor == "BOTTOM") then
				QGT_AchievementWatchFrame:SetPoint("BOTTOMLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.AchievementWatch.Left, QGT_Settings.AchievementWatch.Bottom);
			end
		end
--QuestGuru_Echo("At set AWU false end of if QGT_Settings.AchievementWatch.Minimized QGT_AchievementWatch_Update()", false)
		QGT_AWUpdating = false;
		return;
	end

--QuestGuru_Echo(format("At 2.5 QGT_AchievementWatch_Update(), currLine=%s", tostring(currLine) ), false)
-- currLine should be zero here

	for k, v in pairs(QGT_WatchAchievements) do
		achievementID = k;
		achievementCategory = GetAchievementCategory(achievementID);
		if ( (not displayOnlyArena) or achievementCategory == WATCHFRAME_ACHIEVEMENT_ARENA_CATEGORY ) then
			_, achievementName, _, completed, _, _, _, description, _, icon = GetAchievementInfo(achievementID);
			if (achievementName == nil) then
				RemoveTrackedAchievement(achievementID);
			else
				currLine = currLine + 1;
				achieveLine[currLine] = {};
				achieveLine[currLine].Icon = icon;
				achieveLine[currLine].text = achievementName;
				achieveLine[currLine].achievementID = achievementID;
				if ( completed ) then
					achieveLine[currLine].TextColor = { r=NORMAL_FONT_COLOR.r, g=NORMAL_FONT_COLOR.g, b=NORMAL_FONT_COLOR.b };
				else	
					achieveLine[currLine].TextColor = { r=0.75, g=0.61, b=0 };
				end
				numCriteria = GetAchievementNumCriteria(achievementID);
				if ( numCriteria > 0 ) then
					criteriaDisplayed = 0;
					for j = 1, numCriteria do
						criteriaString, criteriaType, criteriaCompleted, quantity, totalQuantity, name, flags, assetID, quantityString, criteriaID = GetAchievementCriteriaInfo(achievementID, j);
						if ( not criteriaCompleted ) then
              if ( bit.band(flags, EVALUATION_TREE_FLAG_PROGRESS_BAR) == EVALUATION_TREE_FLAG_PROGRESS_BAR ) then
--MoP					if ( bit.band(flags, ACHIEVEMENT_CRITERIA_PROGRESS_BAR) == ACHIEVEMENT_CRITERIA_PROGRESS_BAR ) then
								currLine = currLine + 1;
								achieveLine[currLine] = {};
								achieveLine[currLine].totalQuantity = totalQuantity;
								achieveLine[currLine].quantity = quantity;
								achieveLine[currLine].quantityString = quantityString;
							else
								currLine = currLine + 1;
								achieveLine[currLine] = {};
								achieveLine[currLine].text = " - " .. criteriaString;
								achieveLine[currLine].TextColor = { r=0.8, g=0.8, b=0.8 };
							end
						end
					end
				else
					currLine = currLine + 1;
					achieveLine[currLine] = {};
					achieveLine[currLine].text = " - "..description;
					achieveLine[currLine].TextColor = { r=0.8, g=0.8, b=0.8 };
				end
			end
		end
	end
	
	local watchLines = 30;
	if (QGT_Settings and QGT_Settings and QGT_Settings.Lines) then
		watchLines = QGT_Settings.Lines;
	end
	if (currLine > watchLines) then
		QGT_AchievementWatchFrameSlider:SetMinMaxValues(0, currLine - watchLines);
	else
		QGT_AchievementWatchFrameSlider:SetMinMaxValues(0, 0);
	end
	
	QGT_WatchAchievementLines = currLine;

	--local sliderVal = QGT_AchievementWatchFrameSlider:GetValue();
	--Urgh!  Blizz is returning fractional values despite SetValueStep(1) setting.
	local sliderMin, sliderMax = QGT_AchievementWatchFrameSlider:GetMinMaxValues();
	local sliderVal = math.min(math.floor(QGT_AchievementWatchFrameSlider:GetValue()+0.5), sliderMax);
	local iconWidth, yOffset;
	local titles = 0;
	local statusBars = 0;
	local yOffset = 0;
	local xOffset = 0;

--QuestGuru_Echo(format("At 3 QGT_AchievementWatch_Update(), currLine=%s", tostring(currLine) ), false)

	for i= 1, currLine do
		j = i - sliderVal;
		if (j > watchLines) then
			j = j - 1;
			break;
		end
		if (i > sliderVal) then

			local aLine = _G["QGT_AchievementWatchLine"..j];
			local aLineText = _G["QGT_AchievementWatchLine"..j.."NormalText"];
			--local aLineIcon = _G["QGT_AchievementWatchLine"..j.."Icon"];
			local aLineTimer = _G["QGT_AchievementWatchLine"..j.."Timer"];
			
			if (achieveLine[i].Icon) then
				--aLineIcon:SetTexture(achieveLine[i].Icon);
				--aLineIcon:Show();
				iconWidth = 16;
				xOffset = 16;
				if (j > 1) then yOffset = yOffset + 6; end
			else
				iconWidth = 0;
				xOffset = 0;
			end
			aLine.achievementID = achieveLine[i].achievementID;
			if (achieveLine[i].totalQuantity) then --statusBar
				aLine:SetText("");
				aLine.statusBar:Show();
				aLine.statusBar:GetStatusBarTexture():SetVertexColor(0,0.6,0,1);
				aLine.statusBar:SetMinMaxValues(0, achieveLine[i].totalQuantity);
				aLine.statusBar:SetValue(achieveLine[i].quantity);
				aLine.statusBar.text:SetText(achieveLine[i].quantityString);
				yOffset = yOffset + 4;
				iconWidth = -16;
			else
				aLine:SetText(achieveLine[i].text);
				aLineText:SetTextColor(achieveLine[i].TextColor.r,achieveLine[i].TextColor.g,achieveLine[i].TextColor.b);
			end
			if (QGT_AchievementTimer[achieveLine[i].achievementID]) then
				aLine:SetScript("OnUpdate",
					function (self)
						if (QGT_AchievementTimer[achieveLine[i].achievementID] == nil) then
							self:SetScript("OnUpdate", nil);
							return;
						end
						local timeLeft = math.floor(QGT_AchievementTimer[self.achievementID].startTime + QGT_AchievementTimer[self.achievementID].duration - GetTime());
						local aLineTimer = _G[self:GetName().."Timer"];
						if (timeLeft <= 0) then timeLeft = 0; end
						aLineTimer:SetText(SecondsToTime(timeLeft));
						local tempWidth = aLineTimer:GetWidth() + 8;
						_G[self:GetName().."NormalText"]:SetWidth(240 - tempWidth - 16);
					end);
			else
				aLineTimer:SetText("");
				aLine:SetScript("OnUpdate", nil);
			end
			aLine:SetWidth(240 - iconWidth);
			aLineText:SetWidth(240 - iconWidth);
			aLine:SetPoint("TOPLEFT", QGT_AchievementWatchFrame, 8 + xOffset, (-yOffset - 22 - ((j-1) * 13)));
			aLine:Show();
		end
	end

--QuestGuru_Echo(format("At 4 QGT_AchievementWatch_Update(), j=%s", tostring(j) ), false)
	
	for i = j + 1, 40 do
		local aLine = _G["QGT_AchievementWatchLine"..i];
		--local aLineIcon = _G["QGT_AchievementWatchLine"..i.."Icon"];
		--aLineIcon:Hide();
		aLine:Hide();
	end

--QuestGuru_Echo("At 5 QGT_AchievementWatch_Update()", false)

	QGT_AchievementWatchFrame:Show();
	local tempHeight = 30 + (j * 13) + yOffset;
	QGT_AchievementWatchFrame:SetHeight(tempHeight);
	QGT_AchievementWatchFrameSlider:SetHeight(tempHeight - 16);
	QGT_AchievementWatchFrame:SetWidth(256);
	if (not QGT_AchievementWatchFrame.isMoving) then
		QGT_AchievementWatchFrame:ClearAllPoints();
		if (QGT_Settings.Anchor == "TOP") then
			QGT_AchievementWatchFrame:SetPoint("TOPLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.AchievementWatch.Left, QGT_Settings.AchievementWatch.Top);
			QGT_Settings.AchievementWatch.Bottom = QGT_AchievementWatchFrame:GetBottom();
		elseif (QGT_Settings.Anchor == "BOTTOM") then
			QGT_AchievementWatchFrame:SetPoint("BOTTOMLEFT", "UIParent", "BOTTOMLEFT", QGT_Settings.AchievementWatch.Left, QGT_Settings.AchievementWatch.Bottom);
			QGT_Settings.AchievementWatch.Top = QGT_AchievementWatchFrame:GetTop();
		end
	end
	QGT_AchievementWatchFrameSlider:ClearAllPoints();
	if (((QGT_Settings.AchievementWatch.Left + 256) * QGT_Settings.Scale) > (UIParent:GetWidth() - 16)) then
		QGT_AchievementWatchFrameSlider:SetPoint("TOPLEFT", -14, -16);
	else
		QGT_AchievementWatchFrameSlider:SetPoint("TOPRIGHT", 14, -16);
	end
--QuestGuru_Echo("At set AWU false end QGT_AchievementWatch_Update()", false)
	QGT_AWUpdating = false;
end

function AchievementButton_ToggleTracking(id)
	if ( IsTrackedAchievement(id) ) then
		RemoveTrackedAchievement(id);
		AchievementFrameAchievements_ForceUpdate();
		WatchFrame_Update();
		return;
	end
 
	local _, _, _, completed = GetAchievementInfo(id)
	if ( completed ) then
		UIErrorsFrame:AddMessage(ERR_ACHIEVEMENT_WATCH_COMPLETED, 1.0, 0.1, 0.1, 1.0);
		return;
	end
 
	AddTrackedAchievement(id);
	AchievementFrameAchievements_ForceUpdate();
	WatchFrame_Update();
 
	return true;
end

function QGT_RemoveTrackedAchievement(id)
	QGT_WatchAchievements[id] = nil;
	table.sort(QGT_WatchAchievements, function(a, b) return (a < b); end);

	WatchFrame_Update();
end
hooksecurefunc("RemoveTrackedAchievement", QGT_RemoveTrackedAchievement);

function QGT_AddTrackedAchievement(id)
	QGT_WatchAchievements[id] = true;
	table.sort(QGT_WatchAchievements, function(a, b) return (a < b); end);

	WatchFrame_Update();
end
hooksecurefunc("AddTrackedAchievement", QGT_AddTrackedAchievement);

--function IsTrackedAchievement(id)
--	if (QGT_WatchAchievements[id] == true) then
--		return true;
--	else
--		return false;
--	end
--end

function QGT_SetAchievements()
	local k, v;
	
	QGT_WatchAchievements = {};
	local trackedAchivements = {GetTrackedAchievements()};

	for k, v in pairs(trackedAchivements) do
		QGT_AddTrackedAchievement(v);
	end
end

function QGT_AchievementWatch_OnUpdate(elapsed)
-- Do nothing for now
end