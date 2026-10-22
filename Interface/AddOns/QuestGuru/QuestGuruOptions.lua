local lastColorPick;

QuestGuru_Sounds = {"PVPENTERQUEUE","PVPTHROUGHQUEUE","GLUECREATECHARACTERBUTTON","GLUEENTERWORLDBUTTON","QUESTCOMPLETED","INTERFACESOUND_MONEYFRAMEOPEN",
"igPlayerInvite","LEVELUP","QUESTADDED","gsCharacterCreationCreateChar","TalentScreenOpen","ReadyCheck","RaidWarning","AuctionWindowOpen","AuctionWindowClose",
"Sound\\Creature\\Peon\\PeonBuildingComplete1.wav", "Sound\\Creature\\Peon\\PeonReady1.wav", "Sound\\Creature\\Peasant\\PeasantReady1.wav", "Sound\\Interface\\igPlayerBind.wav",
"Sound\\Creature\\MillhouseManastorm\\TEMPEST_Millhouse_Ready01.wav", "Sound\\Creature\\MillhouseManastorm\\TEMPEST_Millhouse_Slay01.wav", "Sound\\Creature\\Cow\\CowDeath.wav",
"Sound\\interface\\HumanExploration.wav","Sound\\Creature\\Peasant\\PeasantWhat3.wav", "Sound\\Creature\\Peon\\PeonYes3.wav", "Sound\\Creature\\Peon\\PeonWhat4.wav",
"Interface\\AddOns\\QuestGuru\\Peasant_job_done.mp3"};

function QuestGuru_OptionsFrameSoundProgressDropDown_Initialize()
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local info = UIDropDownMenu_CreateInfo();
	info.func = QuestGuru_OptionsFrameSoundProgress_OnClick;
	info.owner = QuestGuru_OptionsFrameSoundProgressButton;
	info.checked = nil;
	info.icon = nil;

	for i,v in ipairs(QuestGuru_Sounds) do
		info.text = v;
		info.value = i;
		if (v == QuestGuru_Settings.Sounds.Progress.Sound) then
			info.checked = 1;
		else
			info.checked = nil;
		end
		UIDropDownMenu_AddButton(info, 1);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundProgressButton, QuestGuru_Settings.Sounds.Progress.Sound);
end

function QuestGuru_OptionsFrameSoundProgress_OnClick(self, button, down)
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local sound = QuestGuru_Sounds[self.value];
	if ((string.sub(sound, -4) == ".wav") or (string.sub(sound, -4) == ".mp3")) then
		PlaySoundFile(sound);
	else
		PlaySound(sound);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundProgressButton, sound);
	QuestGuru_Settings.Sounds.Progress.Sound = sound;
end

function QuestGuru_OptionsFrameSoundObjCompleteDropDown_Initialize()
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local info = UIDropDownMenu_CreateInfo();
	info.func = QuestGuru_OptionsFrameSoundObjComplete_OnClick;
	info.owner = QuestGuru_OptionsFrameSoundObjCompleteButton;
	info.checked = nil;
	info.icon = nil;

	for i,v in ipairs(QuestGuru_Sounds) do
		info.text = v;
		info.value = i;
		if (v == QuestGuru_Settings.Sounds.ObjComplete.Sound) then
			info.checked = 1;
		else
			info.checked = nil;
		end
		UIDropDownMenu_AddButton(info, 1);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundObjCompleteButton, QuestGuru_Settings.Sounds.ObjComplete.Sound);
end

function QuestGuru_OptionsFrameSoundObjComplete_OnClick(self, button, down)
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local sound = QuestGuru_Sounds[self.value];
	if ((string.sub(sound, -4) == ".wav") or (string.sub(sound, -4) == ".mp3")) then
		PlaySoundFile(sound);
	else
		PlaySound(sound);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundObjCompleteButton, sound);
	QuestGuru_Settings.Sounds.ObjComplete.Sound = sound;
end

function QuestGuru_OptionsFrameSoundQuestCompleteDropDown_Initialize()
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local info = UIDropDownMenu_CreateInfo();
	info.func = QuestGuru_OptionsFrameSoundQuestComplete_OnClick;
	info.owner = QuestGuru_OptionsFrameSoundQuestCompleteButton;
	info.checked = nil;
	info.icon = nil;

	for i,v in ipairs(QuestGuru_Sounds) do
		info.text = v;
		info.value = i;
		if (v == QuestGuru_Settings.Sounds.QuestComplete.Sound) then
			info.checked = 1;
		else
			info.checked = nil;
		end
	    UIDropDownMenu_AddButton(info, 1);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundQuestCompleteButton, QuestGuru_Settings.Sounds.QuestComplete.Sound);
end

function QuestGuru_OptionsFrameSoundQuestComplete_OnClick(self, button, down)
	if (QuestGuru_Settings.Sounds == nil) then return; end
	local sound = QuestGuru_Sounds[self.value];
	if ((string.sub(sound, -4) == ".wav") or (string.sub(sound, -4) == ".mp3")) then
		PlaySoundFile(sound);
	else
		PlaySound(sound);
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundQuestCompleteButton, sound);
	QuestGuru_Settings.Sounds.QuestComplete.Sound = sound;
end

do
	local temp, i;

---------------------
-- General Options --
---------------------
	QuestGuru_OptionsFrameGeneral = CreateFrame("FRAME", "QuestGuru_OptionsFrameGeneral", QuestGuru_OptionsFrame);
	QuestGuru_OptionsFrameGeneral.name = "QuestGuru";
	QuestGuru_OptionsFrameGeneral.default = function () QuestGuru_SetGeneralDefaults(); end;
	InterfaceOptions_AddCategory(QuestGuru_OptionsFrameGeneral);

	QuestGuru_OptionsFrameGeneralTitle = QuestGuru_OptionsFrameGeneral:CreateFontString("QuestGuru_OptionsFrameGeneralTitle", "ARTWORK", "GameFontNormalLarge");
	QuestGuru_OptionsFrameGeneralTitle:SetPoint("TOPLEFT", 16, -16);
	QuestGuru_OptionsFrameGeneralTitle:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameGeneralTitle:SetJustifyV("TOP");
	QuestGuru_OptionsFrameGeneralTitle:SetText("QuestGuru "..QG_OPTIONS);

	QuestGuru_OptionsFrameGeneralSubText = QuestGuru_OptionsFrameGeneral:CreateFontString("QuestGuru_OptionsFrameGeneralSubText", "ARTWORK", "GameFontHighlightSmall");
	QuestGuru_OptionsFrameGeneralSubText:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameGeneralTitle", "BOTTOMLEFT", 0, -8);
	QuestGuru_OptionsFrameGeneralSubText:SetPoint("RIGHT", -32, 0);
	QuestGuru_OptionsFrameGeneralSubText:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameGeneralSubText:SetJustifyV("TOP");
	QuestGuru_OptionsFrameGeneralSubText:SetHeight(24);
	QuestGuru_OptionsFrameGeneralSubText:SetText(QG_OPT_OPTIONS_DESC);

	QuestGuru_OptionsFrameGeneralShowLevels = QuestGuru_OptionsFrameGeneral:CreateFontString("QuestGuru_OptionsFrameGeneralShowLevels", "OVERLAY", "GameFontHighlight");
	QuestGuru_OptionsFrameGeneralShowLevels:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameGeneralSubText", "BOTTOMLEFT", -2, -8);
	QuestGuru_OptionsFrameGeneralShowLevels:SetText(QG_OPT_LEVELS_HEAD);

	QuestGuru_OptionsFrameShowLevelsQuestLogToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameShowLevelsQuestLogToggle", QuestGuru_OptionsFrameGeneral, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameShowLevelsQuestLogToggle:SetPoint("TOPLEFT", QuestGuru_OptionsFrameGeneralShowLevels, "BOTTOMLEFT", 0, -2);
	QuestGuru_OptionsFrameShowLevelsQuestLogToggleText:SetText(QG_OPT_LEVELS_QUESTLOG);
	QuestGuru_OptionsFrameShowLevelsQuestLogToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameShowLevelsQuestLogToggle:GetChecked()) then
				QuestGuru_Settings.ShowLevels.QuestLog = true;
			else
				QuestGuru_Settings.ShowLevels.QuestLog = false;
			end
			QuestLog_Update();
		end);

	QuestGuru_OptionsFrameAutoCompleteToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameAutoCompleteToggle", QuestGuru_OptionsFrameGeneral, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameAutoCompleteToggle:SetPoint("TOPLEFT", QuestGuru_OptionsFrameShowLevelsQuestLogToggle, "BOTTOMLEFT", 0, -6);
	QuestGuru_OptionsFrameAutoCompleteToggleText:SetText(QG_OPT_AUTOCOMPLETE);
	QuestGuru_OptionsFrameAutoCompleteToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameAutoCompleteToggle:GetChecked()) then
				QuestGuru_Settings.AutoComplete = true;
			else
				QuestGuru_Settings.AutoComplete = false;
			end
		end);

	QuestGuru_OptionsFrameShowObjItemIconsToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameShowObjItemIconsToggle", QuestGuru_OptionsFrameGeneral, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameShowObjItemIconsToggle:SetPoint("TOPLEFT", QuestGuru_OptionsFrameAutoCompleteToggle, "BOTTOMLEFT", 0, -4);
	QuestGuru_OptionsFrameShowObjItemIconsToggleText:SetText(QG_OPT_OBJ_ICON);
	QuestGuru_OptionsFrameShowObjItemIconsToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameShowObjItemIconsToggle:GetChecked()) then
				QuestGuru_Settings.ShowObjItemIcons = true;
			else
				QuestGuru_Settings.ShowObjItemIcons = false;
			end
			QuestLog_UpdateQuestDetails();
		end);

	QuestGuru_OptionsFrameShowHeaderQuestsToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameShowHeaderQuestsToggle", QuestGuru_OptionsFrameGeneral, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameShowHeaderQuestsToggle:SetPoint("TOPLEFT", QuestGuru_OptionsFrameShowObjItemIconsToggle, "BOTTOMLEFT", 0, -4);
	QuestGuru_OptionsFrameShowHeaderQuestsToggleText:SetText(QG_OPT_HEADER_QUEST_NUM);
	QuestGuru_OptionsFrameShowHeaderQuestsToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameShowHeaderQuestsToggle:GetChecked()) then
				QuestGuru_Settings.HeaderQuests = true;
			else
				QuestGuru_Settings.HeaderQuests = false;
			end
			QuestLog_Update();
		end);

	QuestGuru_OptionsFrameDisableCommToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameDisableCommToggle", QuestGuru_OptionsFrameGeneral, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameDisableCommToggle:SetPoint("TOPLEFT", QuestGuru_OptionsFrameShowHeaderQuestsToggle, "BOTTOMLEFT", 0, -4);
	QuestGuru_OptionsFrameDisableCommToggleText:SetText(QG_OPT_DISABLE_COMM);
	QuestGuru_OptionsFrameDisableCommToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameDisableCommToggle:GetChecked()) then
				QuestGuru_Settings.DisableComm = true;
			else
				QuestGuru_Settings.DisableComm = false;
				QuestGuru_CommSendPGMessage("0001", QUESTGURU_VERSION);
				QuestGuru_SendCurrentQuestStatus(true);
			end
		end);
		

---------------------
-- Sound Options --
---------------------
	QuestGuru_OptionsFrameSound = CreateFrame("FRAME", "QuestGuru_OptionsFrameSound", QuestGuru_OptionsFrame);
	QuestGuru_OptionsFrameSound.name = "QG Sounds";
	QuestGuru_OptionsFrameSound.parent = "QuestGuru";
	QuestGuru_OptionsFrameSound.default = function () QuestGuru_SetSoundDefaults(); end;
	InterfaceOptions_AddCategory(QuestGuru_OptionsFrameSound)

	QuestGuru_OptionsFrameSoundTitle = QuestGuru_OptionsFrameSound:CreateFontString("QuestGuru_OptionsFrameSoundTitle", "ARTWORK", "GameFontNormalLarge");
	QuestGuru_OptionsFrameSoundTitle:SetPoint("TOPLEFT", 16, -16);
	QuestGuru_OptionsFrameSoundTitle:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameSoundTitle:SetJustifyV("TOP");
	QuestGuru_OptionsFrameSoundTitle:SetText("QuestGuru "..QG_OPT_SOUND_HEAD);

	QuestGuru_OptionsFrameSoundSubText = QuestGuru_OptionsFrameSound:CreateFontString("QuestGuru_OptionsFrameSoundSubText", "ARTWORK", "GameFontHighlightSmall");
	QuestGuru_OptionsFrameSoundSubText:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundTitle", "BOTTOMLEFT", 0, -8);
	QuestGuru_OptionsFrameSoundSubText:SetPoint("RIGHT", -32, 0);
	QuestGuru_OptionsFrameSoundSubText:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameSoundSubText:SetJustifyV("TOP");
	QuestGuru_OptionsFrameSoundSubText:SetHeight(24);
	QuestGuru_OptionsFrameSoundSubText:SetText(QG_OPT_SOUND_DESC);

	QuestGuru_OptionsFrameSoundToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameSoundToggle", QuestGuru_OptionsFrameSound, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameSoundToggle:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundSubText", "BOTTOMLEFT", -2, -8);
	QuestGuru_OptionsFrameSoundToggleText:SetText(QG_OPT_SOUND_ENABLE);
	QuestGuru_OptionsFrameSoundToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameSoundToggle:GetChecked()) then
				QuestGuru_Settings.Sounds.Enabled = true;
			else
				QuestGuru_Settings.Sounds.Enabled = false;
			end
		end);

	QuestGuru_OptionsFrameSoundProgressToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameSoundProgressToggle", QuestGuru_OptionsFrameSound, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameSoundProgressToggle:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundToggle", "BOTTOMLEFT", 0, -12);
	QuestGuru_OptionsFrameSoundProgressToggleText:SetText(QG_OPT_SOUND_OBJ);
	QuestGuru_OptionsFrameSoundProgressToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameSoundProgressToggle:GetChecked()) then
				QuestGuru_Settings.Sounds.Progress.Enabled = true;
			else
				QuestGuru_Settings.Sounds.Progress.Enabled = false;
			end
		end);

	QuestGuru_OptionsFrameSoundProgressButton = CreateFrame("Frame", "QuestGuru_OptionsFrameSoundProgressButton", QuestGuru_OptionsFrameSound, "UIDropDownMenuTemplate");
	QuestGuru_OptionsFrameSoundProgressButton:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundProgressToggle", "BOTTOMLEFT");
	QuestGuru_OptionsFrameSoundProgressButton:SetWidth(200);
	QuestGuru_OptionsFrameSoundProgressButtonText:SetJustifyH("LEFT");
	UIDropDownMenu_SetWidth(QuestGuru_OptionsFrameSoundProgressButton, 200);
	UIDropDownMenu_Initialize(QuestGuru_OptionsFrameSoundProgressButton, QuestGuru_OptionsFrameSoundProgressDropDown_Initialize);

	QuestGuru_OptionsFrameSoundObjCompleteToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameSoundObjCompleteToggle", QuestGuru_OptionsFrameSound, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameSoundObjCompleteToggle:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundProgressToggle", "BOTTOMLEFT", 0, -48);
	QuestGuru_OptionsFrameSoundObjCompleteToggleText:SetText(QG_OPT_SOUND_OBJ_DONE);
	QuestGuru_OptionsFrameSoundObjCompleteToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameSoundObjCompleteToggle:GetChecked()) then
				QuestGuru_Settings.Sounds.ObjComplete.Enabled = true;
			else
				QuestGuru_Settings.Sounds.ObjComplete.Enabled = false;
			end
		end);

	QuestGuru_OptionsFrameSoundObjCompleteButton = CreateFrame("Frame", "QuestGuru_OptionsFrameSoundObjCompleteButton", QuestGuru_OptionsFrameSound, "UIDropDownMenuTemplate");
	QuestGuru_OptionsFrameSoundObjCompleteButton:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundObjCompleteToggle", "BOTTOMLEFT");
	QuestGuru_OptionsFrameSoundObjCompleteButton:SetWidth(200);
	QuestGuru_OptionsFrameSoundObjCompleteButtonText:SetJustifyH("LEFT");
	UIDropDownMenu_SetWidth(QuestGuru_OptionsFrameSoundObjCompleteButton, 200);
	UIDropDownMenu_Initialize(QuestGuru_OptionsFrameSoundObjCompleteButton, QuestGuru_OptionsFrameSoundObjCompleteDropDown_Initialize);

	QuestGuru_OptionsFrameSoundQuestCompleteToggle = CreateFrame("CheckButton", "QuestGuru_OptionsFrameSoundQuestCompleteToggle", QuestGuru_OptionsFrameSound, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_OptionsFrameSoundQuestCompleteToggle:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundObjCompleteToggle", "BOTTOMLEFT", 0, -48);
	QuestGuru_OptionsFrameSoundQuestCompleteToggleText:SetText(QG_SOUND_QUEST_DONE);
	QuestGuru_OptionsFrameSoundQuestCompleteToggle:SetScript("OnClick",
		function ()
			if (QuestGuru_OptionsFrameSoundQuestCompleteToggle:GetChecked()) then
				QuestGuru_Settings.Sounds.QuestComplete.Enabled = true;
			else
				QuestGuru_Settings.Sounds.QuestComplete.Enabled = false;
			end
		end);

	QuestGuru_OptionsFrameSoundQuestCompleteButton = CreateFrame("Frame", "QuestGuru_OptionsFrameSoundQuestCompleteButton", QuestGuru_OptionsFrameSound, "UIDropDownMenuTemplate");
	QuestGuru_OptionsFrameSoundQuestCompleteButton:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameSoundQuestCompleteToggle", "BOTTOMLEFT");
	QuestGuru_OptionsFrameSoundQuestCompleteButton:SetWidth(200);
	QuestGuru_OptionsFrameSoundQuestCompleteButtonText:SetJustifyH("LEFT");
	UIDropDownMenu_SetWidth(QuestGuru_OptionsFrameSoundQuestCompleteButton, 200);
	UIDropDownMenu_Initialize(QuestGuru_OptionsFrameSoundQuestCompleteButton, QuestGuru_OptionsFrameSoundQuestCompleteDropDown_Initialize);
end

function QuestGuru_OptionsSaveColor()
	local r,g,b = ColorPickerFrame:GetColorRGB();

	QuestGuru_Settings.Colorize[lastColorPick].Color.r = r;
	QuestGuru_Settings.Colorize[lastColorPick].Color.g = g;
	QuestGuru_Settings.Colorize[lastColorPick].Color.b = b;
	QuestGuru_OptionsFrameColorizePlayerNameColorSwatch:SetTexture(QuestGuru_Settings.Colorize.PlayerName.Color.r, QuestGuru_Settings.Colorize.PlayerName.Color.g, QuestGuru_Settings.Colorize.PlayerName.Color.b, 1.0);
	QuestGuru_OptionsFrameColorizeAreaNamesColorSwatch:SetTexture(QuestGuru_Settings.Colorize.AreaNames.Color.r, QuestGuru_Settings.Colorize.AreaNames.Color.g, QuestGuru_Settings.Colorize.AreaNames.Color.b, 1.0);
	QuestGuru_OptionsFrameColorizeNPCNamesColorSwatch:SetTexture(QuestGuru_Settings.Colorize.NPCNames.Color.r, QuestGuru_Settings.Colorize.NPCNames.Color.g, QuestGuru_Settings.Colorize.NPCNames.Color.b, 1.0);
end

function QuestGuru_OptionsSoundsLoadSettings()
	if (QuestGuru_Settings.Sounds == nil) then
		QuestGuru_Settings.Sounds = {};
	end
	if (QuestGuru_Settings.Sounds.Enabled ~= false) then
		QuestGuru_Settings.Sounds.Enabled = true;
	end
	QuestGuru_OptionsFrameSoundToggle:SetChecked(QuestGuru_Settings.Sounds.Enabled);
	
	if (QuestGuru_Settings.Sounds.Progress == nil) then
		QuestGuru_Settings.Sounds.Progress = {};
	end
	if (QuestGuru_Settings.Sounds.Progress.Enabled ~= false) then
		QuestGuru_Settings.Sounds.Progress.Enabled = true;
	end
	QuestGuru_OptionsFrameSoundProgressToggle:SetChecked(QuestGuru_Settings.Sounds.Progress.Enabled);
	if (QuestGuru_Settings.Sounds.Progress.Sound == nil) then
		QuestGuru_Settings.Sounds.Progress.Sound = "AuctionWindowOpen";
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundProgressButton, QuestGuru_Settings.Sounds.Progress.Sound);

	if (QuestGuru_Settings.Sounds.ObjComplete == nil) then
		QuestGuru_Settings.Sounds.ObjComplete = {};
	end
	if (QuestGuru_Settings.Sounds.ObjComplete.Enabled ~= false) then
		QuestGuru_Settings.Sounds.ObjComplete.Enabled = true;
	end
	QuestGuru_OptionsFrameSoundObjCompleteToggle:SetChecked(QuestGuru_Settings.Sounds.ObjComplete.Enabled);
	if (QuestGuru_Settings.Sounds.ObjComplete.Sound == nil) then
		QuestGuru_Settings.Sounds.ObjComplete.Sound = "AuctionWindowClose";
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundObjCompleteButton, QuestGuru_Settings.Sounds.ObjComplete.Sound);

	if (QuestGuru_Settings.Sounds.QuestComplete == nil) then
		QuestGuru_Settings.Sounds.QuestComplete = {};
	end
	if (QuestGuru_Settings.Sounds.QuestComplete.Enabled ~= false) then
		QuestGuru_Settings.Sounds.QuestComplete.Enabled = true;
	end
	QuestGuru_OptionsFrameSoundQuestCompleteToggle:SetChecked(QuestGuru_Settings.Sounds.QuestComplete.Enabled);
	if (QuestGuru_Settings.Sounds.QuestComplete.Sound == nil) then
		QuestGuru_Settings.Sounds.QuestComplete.Sound = "ReadyCheck";
	end
	UIDropDownMenu_SetText(QuestGuru_OptionsFrameSoundQuestCompleteButton, QuestGuru_Settings.Sounds.QuestComplete.Sound);
end

function QuestGuru_SetGeneralDefaults()
	QuestGuru_Echo("Resetting General settings to defaults.");
	QuestGuru_Settings.AutoComplete = false;
	QuestGuru_OptionsFrameAutoCompleteToggle:SetChecked(QuestGuru_Settings.AutoComplete);
	QuestGuru_Settings.ShowObjItemIcons = false;
	QuestGuru_OptionsFrameShowObjItemIconsToggle:SetChecked(QuestGuru_Settings.ShowObjItemIcons);
	QuestGuru_Settings.DisableComm = false;
	QuestGuru_OptionsFrameDisableCommToggle:SetChecked(QuestGuru_Settings.DisableComm);
	QuestGuru_Settings.HeaderQuests = true;
	QuestGuru_OptionsFrameShowHeaderQuestsToggle:SetChecked(QuestGuru_Settings.HeaderQuests);
	QuestGuru_Settings.ShowLevels = {};
	QuestGuru_Settings.ShowLevels.QuestLog = true;
	QuestGuru_OptionsFrameShowLevelsQuestLogToggle:SetChecked(QuestGuru_Settings.ShowLevels.QuestLog);
end

function QuestGuru_SetSoundDefaults()
	QuestGuru_Echo("Resetting Sound settings to defaults.");
	QuestGuru_Settings.Sounds.Progress.Enabled = true;
	QuestGuru_Settings.Sounds.QuestComplete.Sound = "ReadyCheck";
	QuestGuru_Settings.Sounds.ObjComplete.Sound = "AuctionWindowClose";
	QuestGuru_Settings.Sounds.Progress.Sound = "AuctionWindowOpen";
end