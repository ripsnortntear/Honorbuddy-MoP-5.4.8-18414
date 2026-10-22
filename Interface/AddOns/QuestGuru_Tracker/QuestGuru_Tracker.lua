QGT_Settings = {};
QGT_PlayerAlive = false;
QGT_VariablesLoaded = false;
QGT_ShowTracker = true;
QGT_PlayerInCombat = false;

BINDING_HEADER_QGT_HEADER = "QuestGuru Tracker";
BINDING_NAME_QGT_TOGGLE_TRACKER_KB = "Toggle QuestGuru Tracker";

local lastColorPick;

do
---------------------
-- Tracker Options --
---------------------
	QGT_OptionsFrameTracker = CreateFrame("FRAME", "QGT_OptionsFrameTracker", QGT_OptionsFrame);
	QGT_OptionsFrameTracker.name = "QG Tracker";
	if (IsAddOnLoaded("QuestGuru")) then QGT_OptionsFrameTracker.parent = "QuestGuru"; end
	QGT_OptionsFrameTracker.default = function () QGT_SetTrackerDefaults(); end;
	InterfaceOptions_AddCategory(QGT_OptionsFrameTracker);
	QGT_OptionsFrameTracker:SetScript("OnShow",
		function ()
			QGT_UpdateOptionsTrackerObjFade();
		end);

	QGT_OptionsFrameTrackerTitle = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerTitle", "ARTWORK", "GameFontNormalLarge");
	QGT_OptionsFrameTrackerTitle:SetPoint("TOPLEFT", 16, -16);
	QGT_OptionsFrameTrackerTitle:SetJustifyH("LEFT");
	QGT_OptionsFrameTrackerTitle:SetJustifyV("TOP");
	QGT_OptionsFrameTrackerTitle:SetText("QuestGuru Tracker "..QG_OPTIONS);

	QGT_OptionsFrameTrackerSubText = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerSubText", "ARTWORK", "GameFontHighlightSmall");
	QGT_OptionsFrameTrackerSubText:SetPoint("TOPLEFT", "QGT_OptionsFrameTrackerTitle", "BOTTOMLEFT", 0, -8);
	QGT_OptionsFrameTrackerSubText:SetPoint("RIGHT", -32, 0);
	QGT_OptionsFrameTrackerSubText:SetJustifyH("LEFT");
	QGT_OptionsFrameTrackerSubText:SetJustifyV("TOP");
	QGT_OptionsFrameTrackerSubText:SetHeight(24);
	QGT_OptionsFrameTrackerSubText:SetText(QG_OPT_TRACKER_DESC);

	QGT_OptionsFrameTrackerBorderToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerBorderToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerBorderToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerSubText, "BOTTOMLEFT", -2, -8);
	QGT_OptionsFrameTrackerBorderToggleText:SetText(QG_OPT_TRACKER_BORDER);
	QGT_OptionsFrameTrackerBorderToggle.tooltipText = QG_OPT_TRACKER_BORDER_DESC;
	QGT_OptionsFrameTrackerBorderToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerBorderToggle:GetChecked()) then
				QGT_Settings.ShowBorder = true;
			else
				QGT_Settings.ShowBorder = false;
			end
			QGT_SetQuestWatchBorder(QGT_Settings.ShowBorder);
			QGT_SetAchievementWatchBorder(QGT_Settings.ShowBorder);
			WatchFrame_Update();
		end);

	QGT_OptionsFrameQuestItemIconsToggle = CreateFrame("CheckButton", "QGT_OptionsFrameQuestItemIconsToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameQuestItemIconsToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerBorderToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameQuestItemIconsToggleText:SetText(QG_OPT_TRACKER_ITEMICONS);
	QGT_OptionsFrameQuestItemIconsToggle.tooltipText = QG_OPT_TRACKER_ITEMICONS_DESC;
	QGT_OptionsFrameQuestItemIconsToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameQuestItemIconsToggle:GetChecked()) then
				QGT_Settings.QuestItemIcons = true;
			else
				QGT_Settings.QuestItemIcons = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerClickThroughToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerClickThroughToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerClickThroughToggle:SetPoint("TOPLEFT", QGT_OptionsFrameQuestItemIconsToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerClickThroughToggleText:SetText(QG_OPT_TRACKER_CLICKTHROUGH);
	QGT_OptionsFrameTrackerClickThroughToggle.tooltipText = QG_OPT_TRACKER_CLICKTHROUGH_DESC;
	QGT_OptionsFrameTrackerClickThroughToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerClickThroughToggle:GetChecked()) then
				QGT_Settings.ClickThrough = true;
			else
				QGT_Settings.ClickThrough = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerHeadersToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerHeadersToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerHeadersToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerSubText, "BOTTOMLEFT", 200, -8);
	QGT_OptionsFrameTrackerHeadersToggleText:SetText(QG_OPT_TRACKER_HEADERS);
	QGT_OptionsFrameTrackerHeadersToggle.tooltipText = QG_OPT_TRACKER_HEADERS_DESC;
	QGT_OptionsFrameTrackerHeadersToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerHeadersToggle:GetChecked()) then
				QGT_Settings.ShowHeaders = true;
			else
				QGT_Settings.ShowHeaders = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerShowLevelsToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerShowLevelsToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerShowLevelsToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerHeadersToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerShowLevelsToggleText:SetText(QG_OPT_TRACKER_LEVELS);
	QGT_OptionsFrameTrackerShowLevelsToggle.tooltipText = QG_OPT_TRACKER_LEVELS_DESC;
	QGT_OptionsFrameTrackerShowLevelsToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerShowLevelsToggle:GetChecked()) then
				QGT_Settings.ShowLevels = true;
			else
				QGT_Settings.ShowLevels = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerQuestTooltipsToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerQuestTooltipsToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerQuestTooltipsToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerShowLevelsToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerQuestTooltipsToggleText:SetText(QG_OPT_TRACKER_QUEST_TOOLTIPS);
	QGT_OptionsFrameTrackerQuestTooltipsToggle.tooltipText = QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC;
	QGT_OptionsFrameTrackerQuestTooltipsToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerQuestTooltipsToggle:GetChecked()) then
				QGT_Settings.ShowQuestTooltips = true;
			else
				QGT_Settings.ShowQuestTooltips = false;
			end
		end);

	QGT_OptionsFrameTrackerPartyTooltipsToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerPartyTooltipsToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerPartyTooltipsToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerQuestTooltipsToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerPartyTooltipsToggleText:SetText(QG_OPT_TRACKER_PARTY_TOOLTIPS);
	QGT_OptionsFrameTrackerPartyTooltipsToggle.tooltipText = QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC;
	QGT_OptionsFrameTrackerPartyTooltipsToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerPartyTooltipsToggle:GetChecked()) then
				QGT_Settings.ShowPartyTooltips = true;
			else
				QGT_Settings.ShowPartyTooltips = false;
			end
		end);

	QGT_OptionsFrameTrackerQuestPercentToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerQuestPercentToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerQuestPercentToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerPartyTooltipsToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerQuestPercentToggleText:SetText(QG_OPT_TRACKER_PERCENT);
	QGT_OptionsFrameTrackerQuestPercentToggle.tooltipText = QG_OPT_TRACKER_PERCENT_DESC;
	QGT_OptionsFrameTrackerQuestPercentToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerQuestPercentToggle:GetChecked()) then
				QGT_Settings.ShowQuestPercent = true;
			else
				QGT_Settings.ShowQuestPercent = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerExpandUpToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerExpandUpToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerExpandUpToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerQuestPercentToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerExpandUpToggleText:SetText(QG_OPT_TRACKER_ANCHOR_BOTTOM);
	QGT_OptionsFrameTrackerExpandUpToggle.tooltipText = QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC;
	QGT_OptionsFrameTrackerExpandUpToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerExpandUpToggle:GetChecked()) then
				QGT_Settings.Anchor = "BOTTOM";
			else
				QGT_Settings.Anchor = "TOP";
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerHideCombatToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerHideCombatToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerHideCombatToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerExpandUpToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerHideCombatToggleText:SetText(QG_OPT_TRACKER_HIDECOMBAT);
	QGT_OptionsFrameTrackerHideCombatToggle.tooltipText = QG_OPT_TRACKER_HIDECOMBAT_DESC;
	QGT_OptionsFrameTrackerHideCombatToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerHideCombatToggle:GetChecked()) then
				QGT_Settings.HideDuringCombat = true;
			else
				QGT_Settings.HideDuringCombat = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerHideBlizzTrackToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerHideBlizzTrackToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerHideBlizzTrackToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerHideCombatToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerHideBlizzTrackToggleText:SetText(QG_OPT_TRACKER_HIDEBLIZZTRACK);
	QGT_OptionsFrameTrackerHideBlizzTrackToggle.tooltipText = QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC;
	QGT_OptionsFrameTrackerHideBlizzTrackToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerHideBlizzTrackToggle:GetChecked()) then
				QGT_Settings.HideBlizzardTracker = true;
			else
				QGT_Settings.HideBlizzardTracker = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerPinToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerPinToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerPinToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerClickThroughToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerPinToggleText:SetText(QG_OPT_TRACKER_PIN);
	QGT_OptionsFrameTrackerPinToggle.tooltipText = QG_OPT_TRACKER_PIN_DESC;
	QGT_OptionsFrameTrackerPinToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerPinToggle:GetChecked()) then
				QGT_Settings.Pin = true;
			else
				QGT_Settings.Pin = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerAutoUnTrackToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerAutoUnTrackToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerAutoUnTrackToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerPinToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerAutoUnTrackToggleText:SetText(QG_OPT_TRACKER_AUTOUNTRACK);
	QGT_OptionsFrameTrackerAutoUnTrackToggle.tooltipText = QG_OPT_TRACKER_AUTOUNTRACK_DESC;
	QGT_OptionsFrameTrackerAutoUnTrackToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerAutoUnTrackToggle:GetChecked()) then
				QGT_Settings.AutoUnTrack = true;
			else
				QGT_Settings.AutoUnTrack = false;
			end
		end);

	QGT_OptionsFrameTrackerShowCompletedObjToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerShowCompletedObjToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerShowCompletedObjToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerAutoUnTrackToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerShowCompletedObjToggleText:SetText(QG_OPT_TRACKER_COMPLETE_OBJ);
	QGT_OptionsFrameTrackerShowCompletedObjToggle.tooltipText = QG_OPT_TRACKER_COMPLETE_OBJ_DESC;
	QGT_OptionsFrameTrackerShowCompletedObjToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerShowCompletedObjToggle:GetChecked()) then
				QGT_Settings.ShowCompletedObj = true;
			else
				QGT_Settings.ShowCompletedObj = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerColorizeObjToggle = CreateFrame("CheckButton", "QGT_OptionsFrameTrackerColorizeObjToggle", QGT_OptionsFrameTracker, "QGT_InterfaceOptionsCheckButtonTemplate");
	QGT_OptionsFrameTrackerColorizeObjToggle:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerShowCompletedObjToggle, "BOTTOMLEFT", 0, 2);
	QGT_OptionsFrameTrackerColorizeObjToggleText:SetText(QG_OPT_TRACKER_COLOR_OBJ);
	QGT_OptionsFrameTrackerColorizeObjToggle.tooltipText = QG_OPT_TRACKER_COLOR_OBJ_DESC;
	QGT_OptionsFrameTrackerColorizeObjToggle:SetScript("OnClick",
		function ()
			if (QGT_OptionsFrameTrackerColorizeObjToggle:GetChecked()) then
				QGT_Settings.ColorizeObj = true;
			else
				QGT_Settings.ColorizeObj = false;
			end
			WatchFrame_Update();
		end);

	QGT_OptionsFrameTrackerZeroColor = CreateFrame("Button", "QGT_OptionsFrameTrackerZeroColor", QGT_OptionsFrameTracker, "QGT_ColorPickerButtonTemplate");
	QGT_OptionsFrameTrackerZeroColor:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerColorizeObjToggle, "BOTTOMLEFT", 0, -6);
	QGT_OptionsFrameTrackerZeroColorSwatch:SetTexture(1.0, 1.0, 1.0, 1.0);
	QGT_OptionsFrameTrackerZeroColor.tooltipText = QG_OPT_TRACKER_OBJ_COLOR_0;
	QGT_OptionsFrameTrackerZeroColor:SetScript("OnClick",
		function ()
			ColorPickerFrame.hasOpacity = false;
			ColorPickerFrame.func = QGT_OptionsSaveColor;
			lastColorPick = "TrackerZero";
			ColorPickerFrame:SetColorRGB(QGT_Settings.ColorizeObjZero.r, QGT_Settings.ColorizeObjZero.g, QGT_Settings.ColorizeObjZero.b);
			ColorPickerFrame:Show();
			ColorPickerFrame:SetPoint("TOPLEFT", InterfaceOptionsFrame, "TOPRIGHT", -50, 0);
		end);
	QGT_OptionsFrameTrackerZeroLabel = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerZeroLabel", "ARTWORK", "GameFontNormalSmall");
	QGT_OptionsFrameTrackerZeroLabel:SetPoint("TOP", "QGT_OptionsFrameTrackerZeroColor", "BOTTOM", 0, 0);
	QGT_OptionsFrameTrackerZeroLabel:SetText("0%");

	temp = QGT_OptionsFrameTracker:CreateTexture("QGT_OptionsFrameTrackerGradient", "ARTWORK");
	temp:SetWidth(80);
	temp:SetHeight(12);
	temp:SetTexture(1, 1, 1);
	temp:SetPoint("TOPLEFT", "QGT_OptionsFrameTrackerZeroColor", "TOPRIGHT", 12, -2);

	QGT_OptionsFrameTrackerFullColor = CreateFrame("Button", "QGT_OptionsFrameTrackerFullColor", QGT_OptionsFrameTracker, "QGT_ColorPickerButtonTemplate");
	QGT_OptionsFrameTrackerFullColor:SetPoint("TOPLEFT", "QGT_OptionsFrameTrackerGradient", "TOPRIGHT", 12, 2);
	QGT_OptionsFrameTrackerFullColorSwatch:SetTexture(1.0, 1.0, 1.0, 1.0);
	QGT_OptionsFrameTrackerFullColor.tooltipText = QG_OPT_TRACKER_OBJ_COLOR_99;
	QGT_OptionsFrameTrackerFullColor:SetScript("OnClick",
		function ()
			ColorPickerFrame.hasOpacity = false;
			ColorPickerFrame.func = QGT_OptionsSaveColor;
			lastColorPick = "TrackerFull";
			ColorPickerFrame:SetColorRGB(QGT_Settings.ColorizeObjFull.r, QGT_Settings.ColorizeObjFull.g, QGT_Settings.ColorizeObjFull.b);
			ColorPickerFrame:Show();
			ColorPickerFrame:SetPoint("TOPLEFT", InterfaceOptionsFrame, "TOPRIGHT", -50, 0);
		end);

	QGT_OptionsFrameTrackerFullLabel = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerFullLabel", "ARTWORK", "GameFontNormalSmall");
	QGT_OptionsFrameTrackerFullLabel:SetPoint("TOP", "QGT_OptionsFrameTrackerFullColor", "BOTTOM", 0, 0);
	QGT_OptionsFrameTrackerFullLabel:SetText("99%");

	QGT_OptionsFrameTrackerCompleteColor = CreateFrame("Button", "QGT_OptionsFrameTrackerCompleteColor", QGT_OptionsFrameTracker, "QGT_ColorPickerButtonTemplate");
	QGT_OptionsFrameTrackerCompleteColor:SetPoint("TOPLEFT", QGT_OptionsFrameTrackerFullColor, "TOPRIGHT", 30, 0);
	QGT_OptionsFrameTrackerCompleteColorSwatch:SetTexture(1.0, 1.0, 1.0, 1.0);
	QGT_OptionsFrameTrackerCompleteColor.tooltipText = QG_OPT_TRACKER_OBJ_COLOR_COMPLETE;
	QGT_OptionsFrameTrackerCompleteColor:SetScript("OnClick",
		function ()
			ColorPickerFrame.hasOpacity = false;
			ColorPickerFrame.func = QGT_OptionsSaveColor;
			lastColorPick = "TrackerComplete";
			ColorPickerFrame:SetColorRGB(QGT_Settings.ColorizeObjComplete.r, QGT_Settings.ColorizeObjComplete.g, QGT_Settings.ColorizeObjComplete.b);
			ColorPickerFrame:Show();
			ColorPickerFrame:SetPoint("TOPLEFT", InterfaceOptionsFrame, "TOPRIGHT", -50, 0);
		end);

	QGT_OptionsFrameTrackerCompleteLabel = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerCompleteLabel", "ARTWORK", "GameFontNormalSmall");
	QGT_OptionsFrameTrackerCompleteLabel:SetPoint("TOP", "QGT_OptionsFrameTrackerCompleteColor", "BOTTOM", 0, 0);
	QGT_OptionsFrameTrackerCompleteLabel:SetText(COMPLETE);

	QGT_OptionsFrameTrackerBulletLabel = QGT_OptionsFrameTracker:CreateFontString("QGT_OptionsFrameTrackerBulletLabel", "ARTWORK", "GameFontNormal");
	QGT_OptionsFrameTrackerBulletLabel:SetPoint("TOPLEFT", "QGT_OptionsFrameTrackerZeroLabel", "BOTTOMLEFT", 0, -6);
	QGT_OptionsFrameTrackerBulletLabel:SetText(QG_OPT_TRACKER_BULLET);

	QGT_OptionsFrameTrackerBulletText = CreateFrame("EditBox", "QGT_OptionsFrameTrackerBulletText", QGT_OptionsFrameTracker, "InputBoxTemplate");
	QGT_OptionsFrameTrackerBulletText:SetPoint("LEFT", "QGT_OptionsFrameTrackerBulletLabel", "RIGHT", 8, 0);
	QGT_OptionsFrameTrackerBulletText:SetHeight(20);
	QGT_OptionsFrameTrackerBulletText:SetWidth(36);
	QGT_OptionsFrameTrackerBulletText:SetAutoFocus(false);
	QGT_OptionsFrameTrackerBulletText:SetScript("OnShow",
		function (self)
			QGT_OptionsFrameTrackerBulletText:SetCursorPosition(0);
		end);
	QGT_OptionsFrameTrackerBulletText:SetScript("OnTextChanged",
		function ()
			QGT_Settings.Bullet = QGT_OptionsFrameTrackerBulletText:GetText();
			WatchFrame_Update();
		end);
	QGT_OptionsFrameTrackerBulletText:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QGT_OptionsFrameTrackerAlpha = CreateFrame("Slider", "QGT_OptionsFrameTrackerAlpha", QGT_OptionsFrameTracker, "OptionsSliderTemplate");
	QGT_OptionsFrameTrackerAlpha:SetWidth(300);
	QGT_OptionsFrameTrackerAlpha:SetHeight(16);
	QGT_OptionsFrameTrackerAlpha:SetPoint("CENTER", QGT_OptionsFrameTracker, "BOTTOM", 0, 24);
	QGT_OptionsFrameTrackerAlphaText:SetText(QG_OPT_TRACKER_ALPHA);
	QGT_OptionsFrameTrackerAlphaHigh:SetText("100%");
	QGT_OptionsFrameTrackerAlphaLow:SetText("0%");
	QGT_OptionsFrameTrackerAlpha:SetMinMaxValues(0,1);
	QGT_OptionsFrameTrackerAlpha:SetValueStep(0.01);
	QGT_OptionsFrameTrackerAlpha:SetValue(0.7);
	QGT_OptionsFrameTrackerAlpha:SetScript("OnValueChanged",
		function ()
			QGT_Settings.Alpha = QGT_OptionsFrameTrackerAlpha:GetValue();
			QGT_QuestWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
			QGT_QuestWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
			QGT_AchievementWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
			QGT_AchievementWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
			QGT_OptionsFrameTrackerAlphaText:SetText(QG_OPT_TRACKER_ALPHA.." ("..abs(ceil((QGT_Settings.Alpha * 100)-0.5)).."%)");
		end);

	QGT_OptionsFrameTrackerScale = CreateFrame("Slider", "QGT_OptionsFrameTrackerScale", QGT_OptionsFrameTracker, "OptionsSliderTemplate");
	QGT_OptionsFrameTrackerScale:SetWidth(300);
	QGT_OptionsFrameTrackerScale:SetHeight(16);
	QGT_OptionsFrameTrackerScale:SetPoint("CENTER", QGT_OptionsFrameTracker, "BOTTOM", 0, 58);
	QGT_OptionsFrameTrackerScaleText:SetText(QG_OPT_TRACKER_SIZE);
	QGT_OptionsFrameTrackerScaleHigh:SetText("150%");
	QGT_OptionsFrameTrackerScaleLow:SetText("50%");
	QGT_OptionsFrameTrackerScale:SetMinMaxValues(0.5,1.5);
	QGT_OptionsFrameTrackerScale:SetValueStep(0.01);
	QGT_OptionsFrameTrackerScale:SetValue(0.9);
	QGT_OptionsFrameTrackerScale:SetScript("OnValueChanged",
		function ()
			QGT_Settings.Scale = QGT_OptionsFrameTrackerScale:GetValue();
			QGT_OptionsFrameTrackerScaleText:SetText(QG_OPT_TRACKER_SIZE.." ("..ceil((QGT_Settings.Scale * 100)-0.5).."%)");
			QGT_QuestWatchFrame:SetScale(QGT_Settings.Scale);
			QGT_AchievementWatchFrame:SetScale(QGT_Settings.Scale);
		end);

	QGT_OptionsFrameTrackerLines = CreateFrame("Slider", "QGT_OptionsFrameTrackerLines", QGT_OptionsFrameTracker, "OptionsSliderTemplate");
	QGT_OptionsFrameTrackerLines:SetWidth(300);
	QGT_OptionsFrameTrackerLines:SetHeight(16);
	QGT_OptionsFrameTrackerLines:SetPoint("CENTER", QGT_OptionsFrameTracker, "BOTTOM", 0, 92);
	QGT_OptionsFrameTrackerLinesText:SetText(QG_OPT_TRACKER_MAX_LINES);
	QGT_OptionsFrameTrackerLinesHigh:SetText("40");
	QGT_OptionsFrameTrackerLinesLow:SetText("7");
	QGT_OptionsFrameTrackerLines:SetMinMaxValues(7,40);
	QGT_OptionsFrameTrackerLines:SetValueStep(1);
	QGT_OptionsFrameTrackerLines:SetValue(30);
	QGT_OptionsFrameTrackerLines:SetScript("OnValueChanged",
		function ()
			QGT_Settings.Lines = QGT_OptionsFrameTrackerLines:GetValue();
			QGT_OptionsFrameTrackerLinesText:SetText(QG_OPT_TRACKER_MAX_LINES.." "..QGT_Settings.Lines);
			WatchFrame_Update();
		end);

end

function QGT_OptionsSaveColor()
	local r,g,b = ColorPickerFrame:GetColorRGB();

	if (lastColorPick == "TrackerZero") then
		QGT_Settings.ColorizeObjZero.r = r;
		QGT_Settings.ColorizeObjZero.g = g;
		QGT_Settings.ColorizeObjZero.b = b;
		QGT_UpdateOptionsTrackerObjFade();
		WatchFrame_Update();
	elseif (lastColorPick == "TrackerFull") then
		QGT_Settings.ColorizeObjFull.r = r;
		QGT_Settings.ColorizeObjFull.g = g;
		QGT_Settings.ColorizeObjFull.b = b;
		QGT_UpdateOptionsTrackerObjFade();
		WatchFrame_Update();
	elseif (lastColorPick == "TrackerComplete") then
		QGT_Settings.ColorizeObjComplete.r = r;
		QGT_Settings.ColorizeObjComplete.g = g;
		QGT_Settings.ColorizeObjComplete.b = b;
		QGT_OptionsFrameTrackerCompleteColorSwatch:SetTexture(r, g, b);
		WatchFrame_Update();
	end
end

function QGT_UpdateOptionsTrackerObjFade()
	local r, g, b;
	
	QGT_OptionsFrameTrackerGradient:SetGradient("HORIZONTAL", QGT_Settings.ColorizeObjZero.r, QGT_Settings.ColorizeObjZero.g, QGT_Settings.ColorizeObjZero.b, QGT_Settings.ColorizeObjFull.r, QGT_Settings.ColorizeObjFull.g, QGT_Settings.ColorizeObjFull.b);

	r, g, b = QGT_QuestWatchGetObjColor(0, true);
	QGT_OptionsFrameTrackerZeroColorSwatch:SetTexture(r, g, b);

	r, g, b = QGT_QuestWatchGetObjColor(0.99, true);
	QGT_OptionsFrameTrackerFullColorSwatch:SetTexture(r, g, b);

	r, g, b = QGT_QuestWatchGetObjColor(1, true);
	QGT_OptionsFrameTrackerCompleteColorSwatch:SetTexture(r, g, b);
end

function QGT_SetTrackerDefaults()
	QGT_Settings.ShowBorder = true;
	QGT_SetQuestWatchBorder(QGT_Settings.ShowBorder);
	QGT_SetAchievementWatchBorder(QGT_Settings.ShowBorder);
	QGT_OptionsFrameTrackerBorderToggle:SetChecked(QGT_Settings.ShowBorder);
	
	QGT_Settings.Scale = 0.9;
	QGT_OptionsFrameTrackerScaleText:SetText(QG_OPT_TRACKER_SIZE.." ("..ceil((QGT_Settings.Scale * 100)-0.5).."%)");
	QGT_OptionsFrameTrackerScale:SetValue(QGT_Settings.Scale);
	QGT_QuestWatchFrame:SetScale(QGT_Settings.Scale);
	QGT_AchievementWatchFrame:SetScale(QGT_Settings.Scale);
	
	QGT_Settings.Lines = 30;
	QGT_OptionsFrameTrackerLinesText:SetText(QG_OPT_TRACKER_MAX_LINES.." "..QGT_Settings.Lines);
	QGT_OptionsFrameTrackerLines:SetValue(QGT_Settings.Lines);
	
	QGT_Settings.Minimized = false;
	
	QGT_Settings.Alpha = 0.7;
	QGT_QuestWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
	QGT_QuestWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
	QGT_AchievementWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
	QGT_AchievementWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
	QGT_OptionsFrameTrackerAlphaText:SetText(QG_OPT_TRACKER_ALPHA.." ("..abs(ceil((QGT_Settings.Alpha * 100)-0.5)).."%)");
	QGT_OptionsFrameTrackerAlpha:SetValue(QGT_Settings.Alpha);
	
	QGT_Settings.ShowHeaders = true;
	QGT_OptionsFrameTrackerHeadersToggle:SetChecked(QGT_Settings.ShowHeaders);
	
	QGT_Settings.QuestItemIcons = true;
	QGT_OptionsFrameQuestItemIconsToggle:SetChecked(QGT_Settings.QuestItemIcons);
	
	QGT_Settings.ShowLevels = true;
	QGT_OptionsFrameTrackerShowLevelsToggle:SetChecked(QGT_Settings.ShowLevels);
	
	QGT_Settings.Pin = false;
	QGT_OptionsFrameTrackerPinToggle:SetChecked(QGT_Settings.Pin);
	
	QGT_Settings.HideDuringCombat = false;
	QGT_OptionsFrameTrackerHideCombatToggle:SetChecked(QGT_Settings.HideDuringCombat);
	
	QGT_Settings.HideBlizzardTracker = true;
	QGT_OptionsFrameTrackerHideBlizzTrackToggle:SetChecked(QGT_Settings.HideBlizzardTracker);
	
	QGT_Settings.AutoUnTrack = false;
	QGT_OptionsFrameTrackerAutoUnTrackToggle:SetChecked(QGT_Settings.AutoUnTrack);
	
	QGT_Settings.ShowCompletedObj = true;
	QGT_OptionsFrameTrackerShowCompletedObjToggle:SetChecked(QGT_Settings.ShowCompletedObj);
	
	QGT_Settings.ColorizeObj = false;
	QGT_OptionsFrameTrackerColorizeObjToggle:SetChecked(QGT_Settings.ColorizeObj);
	QGT_Settings.ColorizeObjZero.r = 0.8;
	QGT_Settings.ColorizeObjZero.g = 0.2;
	QGT_Settings.ColorizeObjZero.b = 0.8;
	QGT_Settings.ColorizeObjFull.r = 0.3;
	QGT_Settings.ColorizeObjFull.g = 0.8;
	QGT_Settings.ColorizeObjFull.b = 1.0;
	QGT_Settings.ColorizeObjComplete.r = 0.1;
	QGT_Settings.ColorizeObjComplete.g = 0.9;
	QGT_Settings.ColorizeObjComplete.b = 1.0;
	
	QGT_Settings.ClickThrough = false;
	QGT_OptionsFrameTrackerClickThroughToggle:SetChecked(QGT_Settings.ClickThrough);
	
	QGT_Settings.ShowQuestTooltips = true;
	QGT_OptionsFrameTrackerQuestTooltipsToggle:SetChecked(QGT_Settings.ShowQuestTooltips);
	
	QGT_Settings.ShowPartyTooltips = true;
	QGT_OptionsFrameTrackerPartyTooltipsToggle:SetChecked(QGT_Settings.ShowPartyTooltips);
	
	QGT_Settings.ShowQuestPercent = true;
	QGT_OptionsFrameTrackerQuestPercentToggle:SetChecked(QGT_Settings.ShowQuestPercent);

	QGT_Settings.Bullet = "-";
	QGT_OptionsFrameTrackerBulletText:SetText(QGT_Settings.Bullet);
	
	QGT_Settings.Anchor = "TOP";
	QGT_OptionsFrameTrackerExpandUpToggle:SetChecked(false);
	
	QGT_Settings.QuestWatchSort = 0; -- Default to Header/Quest style, like QuestLog (1=Sorted by proximity)
	
	QGT_UpdateOptionsTrackerObjFade();
	WatchFrame_Update();
end

-- QuestWatch functions
function QGT_WatchFrame_Update()
--QuestGuru_Echo("Entered QGT_WatchFrame_Update()", false)
	if (QGT_Settings.HideBlizzardTracker) then
		WatchFrame:Hide();
	else
		WatchFrame:Show();
	end

	if (not (QGT_PlayerAlive and QGT_VariablesLoaded)) then return; end
	
	if ((not QGT_ShowTracker) or (QGT_PlayerInCombat and QGT_Settings.HideDuringCombat)) then
		QGT_QuestWatchFrame:Hide();
		QGT_AchievementWatchFrame:Hide();
		return;
	end

--QuestGuru_Echo("At 1 QGT_WatchFrame_Update()", false)

	if ((QGT_Settings.LastTracker == "Q") or QGT_Settings.BothTrackers) then
--QuestGuru_Echo("At 2QUp QGT_WatchFrame_Update()", false)
		QGT_QuestWatch_Update();
	else
--QuestGuru_Echo("At 2QHide QGT_WatchFrame_Update()", false)
		QGT_QuestWatchFrame:Hide();
	end
	if ((QGT_Settings.LastTracker == "A") or QGT_Settings.BothTrackers) then
--QuestGuru_Echo("At 2AUp QGT_WatchFrame_Update()", false)
		QGT_AchievementWatch_Update();
	else
--QuestGuru_Echo("At 2AHide QGT_WatchFrame_Update()", false)
		QGT_AchievementWatchFrame:Hide();
	end
end
hooksecurefunc("WatchFrame_Update", QGT_WatchFrame_Update);

function WatchFrame_GetRemainingSpace()
	return 1000;
end

function QGT_QuestWatchLoadSettings()
	if (QGT_WatchHeaders == nil) then
	    QGT_WatchHeaders = {};
	end
	if (QGT_WatchQuests == nil) then
	    QGT_WatchQuests = {};
	end
	if (QGT_WatchAchievements == nil) then
	    QGT_WatchAchievements = {};
	end

	if (QGT_Settings == nil) then
		QGT_Settings = {};
	end
	if (QGT_Settings.QuestWatch == nil) then
		QGT_Settings.QuestWatch = {};
	end
	if (QGT_Settings.AchievementWatch == nil) then
		QGT_Settings.AchievementWatch = {};
	end
	if (QGT_Settings.ShowBorder ~= false) then
		QGT_Settings.ShowBorder = true;
	end
	QGT_OptionsFrameTrackerBorderToggle:SetChecked(QGT_Settings.ShowBorder);
	QGT_SetQuestWatchBorder(QGT_Settings.ShowBorder);
	QGT_SetAchievementWatchBorder(QGT_Settings.ShowBorder);
	if (QGT_Settings.Scale == nil) then
		QGT_Settings.Scale = 0.9;
	end
	QGT_OptionsFrameTrackerScaleText:SetText(QG_OPT_TRACKER_SIZE.." ("..ceil((QGT_Settings.Scale * 100)-0.5).."%)");
	QGT_OptionsFrameTrackerScale:SetValue(QGT_Settings.Scale);
	QGT_QuestWatchFrame:SetScale(QGT_Settings.Scale);
	QGT_AchievementWatchFrame:SetScale(QGT_Settings.Scale);
	
	if (QGT_Settings.Lines == nil) then
		QGT_Settings.Lines = 30;
	end
	QGT_OptionsFrameTrackerLinesText:SetText(QG_OPT_TRACKER_MAX_LINES.." "..QGT_Settings.Lines);
	QGT_OptionsFrameTrackerLines:SetValue(QGT_Settings.Lines);
	
	if (QGT_Settings.QuestWatch.Minimized ~= true) then
		QGT_Settings.QuestWatch.Minimized = false;
	end
	if (QGT_Settings.QuestWatch.AutoMinimize ~= true) then
		QGT_Settings.QuestWatch.AutoMinimize = false;
	end
	if (QGT_Settings.AchievementWatch.Minimized ~= true) then
		QGT_Settings.AchievementWatch.Minimized = false;
	end
	if (QGT_Settings.AchievementWatch.AutoMinimize ~= true) then
		QGT_Settings.AchievementWatch.AutoMinimize = false;
	end
	if (QGT_Settings.Anchor == nil) then
		QGT_Settings.Anchor = "TOP";
	end
	if (QGT_Settings.Anchor == "BOTTOM") then
		QGT_OptionsFrameTrackerExpandUpToggle:SetChecked(true);
	else
		QGT_OptionsFrameTrackerExpandUpToggle:SetChecked(false);
	end

	if (QGT_Settings.Alpha == nil) then
		QGT_Settings.Alpha = 0.7;
	end
	QGT_QuestWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
	QGT_QuestWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
	QGT_AchievementWatchFrame:SetBackdropColor(0,0,0,QGT_Settings.Alpha);
	QGT_AchievementWatchFrameBackground:SetGradientAlpha("VERTICAL", 0, 0, 0, 0, 0.3, 0.3, 0.3, QGT_Settings.Alpha);
	QGT_OptionsFrameTrackerAlphaText:SetText(QG_OPT_TRACKER_ALPHA.." ("..abs(ceil((QGT_Settings.Alpha * 100)-0.5)).."%)");
	QGT_OptionsFrameTrackerAlpha:SetValue(QGT_Settings.Alpha);

	if (QGT_Settings.ShowHeaders ~= false) then
		QGT_Settings.ShowHeaders = true;
	end
	QGT_OptionsFrameTrackerHeadersToggle:SetChecked(QGT_Settings.ShowHeaders);

	if (QGT_Settings.QuestItemIcons ~= false) then
		QGT_Settings.QuestItemIcons = true;
	end
	QGT_OptionsFrameQuestItemIconsToggle:SetChecked(QGT_Settings.QuestItemIcons);

	if (QGT_Settings.ShowLevels ~= false) then
		QGT_Settings.ShowLevels = true;
	end
	QGT_OptionsFrameTrackerShowLevelsToggle:SetChecked(QGT_Settings.ShowLevels);

	if (QGT_Settings.Pin ~= true) then
		QGT_Settings.Pin = false;
	end
	QGT_OptionsFrameTrackerPinToggle:SetChecked(QGT_Settings.Pin);

	if (QGT_Settings.HideDuringCombat ~= true) then
		QGT_Settings.HideDuringCombat = false;
	end
	QGT_OptionsFrameTrackerHideCombatToggle:SetChecked(QGT_Settings.HideDuringCombat);
	
	if (QGT_Settings.HideBlizzardTracker ~= false) then
		QGT_Settings.HideBlizzardTracker = true;
	end
	QGT_OptionsFrameTrackerHideBlizzTrackToggle:SetChecked(QGT_Settings.HideBlizzardTracker);

	if (QGT_Settings.AutoUnTrack ~= true) then
		QGT_Settings.AutoUnTrack = false;
	end
	QGT_OptionsFrameTrackerAutoUnTrackToggle:SetChecked(QGT_Settings.AutoUnTrack);

	if (QGT_Settings.ShowCompletedObj ~= false) then
		QGT_Settings.ShowCompletedObj = true;
	end
	QGT_OptionsFrameTrackerShowCompletedObjToggle:SetChecked(QGT_Settings.ShowCompletedObj);

	if (QGT_Settings.ColorizeObj ~= true) then
		QGT_Settings.ColorizeObj = false;
	end
	QGT_OptionsFrameTrackerColorizeObjToggle:SetChecked(QGT_Settings.ColorizeObj);
	
	if (QGT_Settings.ColorizeObjZero == nil) then
		QGT_Settings.ColorizeObjZero = {};
		QGT_Settings.ColorizeObjZero.r = 0.8;
		QGT_Settings.ColorizeObjZero.g = 0.2;
		QGT_Settings.ColorizeObjZero.b = 0.8;
	end
	if (QGT_Settings.ColorizeObjFull == nil) then
		QGT_Settings.ColorizeObjFull = {};
		QGT_Settings.ColorizeObjFull.r = 0.3;
		QGT_Settings.ColorizeObjFull.g = 0.8;
		QGT_Settings.ColorizeObjFull.b = 1.0;
	end
	if (QGT_Settings.ColorizeObjComplete == nil) then
		QGT_Settings.ColorizeObjComplete = {};
		QGT_Settings.ColorizeObjComplete.r = 0.1;
		QGT_Settings.ColorizeObjComplete.g = 0.9;
		QGT_Settings.ColorizeObjComplete.b = 1.0;
	end

	if (QGT_Settings.ClickThrough ~= true) then
		QGT_Settings.ClickThrough = false;
	end
	QGT_OptionsFrameTrackerClickThroughToggle:SetChecked(QGT_Settings.ClickThrough);

	if (QGT_Settings.ShowQuestTooltips ~= false) then
		QGT_Settings.ShowQuestTooltips = true;
	end
	QGT_OptionsFrameTrackerQuestTooltipsToggle:SetChecked(QGT_Settings.ShowQuestTooltips);

	if (QGT_Settings.ShowPartyTooltips ~= false) then
		QGT_Settings.ShowPartyTooltips = true;
	end
	QGT_OptionsFrameTrackerPartyTooltipsToggle:SetChecked(QGT_Settings.ShowPartyTooltips);

	if (QGT_Settings.ShowQuestPercent ~= false) then
		QGT_Settings.ShowQuestPercent = true;
	end
	QGT_OptionsFrameTrackerQuestPercentToggle:SetChecked(QGT_Settings.ShowQuestPercent);
	
	if (QGT_Settings.Bullet == nil) then
		QGT_Settings.Bullet = "-";
	end	
	QGT_OptionsFrameTrackerBulletText:SetText(QGT_Settings.Bullet);
	
	if (QGT_Settings.LastTracker ~= "A") then
		QGT_Settings.LastTracker = "Q";
	end
	if (QGT_Settings.BothTrackers ~= true) then
		QGT_Settings.BothTrackers = false;
	end
end

--Slash Commands are UpperCase, Global, of the form SLASH_<SlashCmdListMemberFunctionName><digit>
--local ADirName, A = ...                                    -- ... contains the name of the Addon Directory, which is the name of the addon and an (initially) empty table that is passed to all modules in the directory & sub-directories
--function A.SlashHandlerT(SlashText, OriginatingChatFrame)
--  QuestGuru_Echo(format("/qgt %s", tostring(SlashText)), true)
--  if SlashText == "show" then
--    QGT_QuestWatchFrame:Show();
--    QGT_AchievementWatchFrame:Show();
--  elseif SlashText == "var1" then
--    QuestGuru_Echo(format("QGT_Settings.LastTracker: %s", tostring(QGT_Settings.LastTracker)), true);
--    QuestGuru_Echo(format("QGT_Settings.BothTrackers: %s", tostring(QGT_Settings.BothTrackers)), true);
--    QuestGuru_Echo(format("QGT_Settings.AchievementWatch.AutoMinimize: %s", tostring(QGT_Settings.AchievementWatch.AutoMinimize)), true);
--  elseif SlashText == "U" then
--    QuestGuru_Echo("call: QGT_WatchFrame_Update()", true)
--		QGT_WatchFrame_Update();
--    QuestGuru_Echo("back: QGT_WatchFrame_Update()", true)
--  elseif SlashText == "UA" then
--		QGT_AchievementWatch_Update();
--  elseif SlashText == "UQ" then
--		QGT_QuestWatch_Update();
--  end
--end
--SLASH_QGT1 = "/qgt"                                        -- Slash command 1
--SlashCmdList["QGT"] = A.SlashHandlerT                      -- Register our SlashCmdListMemberFunction
