do
	local temp, i;

	QuestGuru_AnnounceFrame = CreateFrame("FRAME", "QuestGuru_AnnounceFrame", QuestGuru_OptionsFrame);
	QuestGuru_AnnounceFrame.name = "QG Announcer";
	QuestGuru_AnnounceFrame.parent = "QuestGuru";
	QuestGuru_AnnounceFrame.default = function () QuestGuru_SetAnnounceDefaults(); end;
	QuestGuru_AnnounceFrame.okay = function () QuestGuru_SaveAnnounceSettings(); end;
	QuestGuru_AnnounceFrame.cancel = function () QuestGuru_CancelAnnounceChanges(); end;
	InterfaceOptions_AddCategory(QuestGuru_AnnounceFrame);

	QuestGuru_OptionsFrameAnnounceTitle = QuestGuru_AnnounceFrame:CreateFontString("QuestGuru_OptionsFrameAnnounceTitle", "ARTWORK", "GameFontNormalLarge");
	QuestGuru_OptionsFrameAnnounceTitle:SetPoint("TOPLEFT", 16, -16);
	QuestGuru_OptionsFrameAnnounceTitle:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameAnnounceTitle:SetJustifyV("TOP");
	QuestGuru_OptionsFrameAnnounceTitle:SetText(QG_OPT_ANNOUNCE_HEAD);

	QuestGuru_OptionsFrameAnnounceSubText = QuestGuru_AnnounceFrame:CreateFontString("QuestGuru_OptionsFrameAnnounceSubText", "ARTWORK", "GameFontHighlightSmall");
	QuestGuru_OptionsFrameAnnounceSubText:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameAnnounceTitle", "BOTTOMLEFT", 0, -8);
	QuestGuru_OptionsFrameAnnounceSubText:SetPoint("RIGHT", -32, 0);
	QuestGuru_OptionsFrameAnnounceSubText:SetJustifyH("LEFT");
	QuestGuru_OptionsFrameAnnounceSubText:SetJustifyV("TOP");
	QuestGuru_OptionsFrameAnnounceSubText:SetHeight(24);
	QuestGuru_OptionsFrameAnnounceSubText:SetText(QG_OPT_ANNOUNCE_DESC);

	QuestGuru_AnnounceFrameAnnounceToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameAnnounceToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameAnnounceToggle:SetPoint("TOPLEFT", "QuestGuru_OptionsFrameAnnounceSubText", "BOTTOMLEFT", -2, -8);
	QuestGuru_AnnounceFrameAnnounceToggleText:SetText(QG_OPT_ANNOUNCE_PROGRESS);

	temp = QuestGuru_AnnounceFrame:CreateFontString("QuestGuru_AnnounceFrameChannelsLabel", "ARTWORK", "GameFontNormal");
	temp:SetText(QG_OPT_ANNOUNCE_CHANNEL_HEAD);
	temp:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameAnnounceToggle", "BOTTOMLEFT", 0, -6);
	
	QuestGuru_AnnounceFrameChannelSayToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameChannelSayToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameChannelSayToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameChannelsLabel", "BOTTOMLEFT", 0, -2);
	QuestGuru_AnnounceFrameChannelSayToggleText:SetText(QG_OPT_ANNOUNCE_CHANNEL_SAY);
	QuestGuru_AnnounceFrameChannelSayToggle.tooltipText = QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC;
	
	QuestGuru_AnnounceFrameChannelPartyToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameChannelPartyToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameChannelPartyToggle:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelSayToggle", "RIGHT", 24, 0);
	QuestGuru_AnnounceFrameChannelPartyToggleText:SetText(QG_OPT_ANNOUNCE_CHANNEL_PARTY);
	QuestGuru_AnnounceFrameChannelPartyToggle.tooltipText = QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC;
	
	QuestGuru_AnnounceFrameChannelGuildToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameChannelGuildToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameChannelGuildToggle:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelPartyToggle", "RIGHT", 24, 0);
	QuestGuru_AnnounceFrameChannelGuildToggleText:SetText(QG_OPT_ANNOUNCE_CHANNEL_GUILD);
	QuestGuru_AnnounceFrameChannelGuildToggle.tooltipText = QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC;
	
	QuestGuru_AnnounceFrameChannelWhisperToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameChannelWhisperToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameChannelWhisperToggle:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelGuildToggle", "RIGHT", 24, 0);
	QuestGuru_AnnounceFrameChannelWhisperToggleText:SetText(QG_OPT_ANNOUNCE_CHANNEL_WHISPER);
	QuestGuru_AnnounceFrameChannelWhisperToggle.tooltipText = QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC;

	temp = QuestGuru_AnnounceFrame:CreateFontString("QuestGuru_AnnounceFrameChannelWhisperToText", "ARTWORK", "GameFontNormal");
	temp:SetText(QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO);
	temp:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelWhisperToggle", "RIGHT", 24, 0);
	
	QuestGuru_AnnounceFrameChannelWhisperTo = CreateFrame("EditBox", "QuestGuru_AnnounceFrameChannelWhisperTo", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameChannelWhisperTo:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelWhisperToText", "RIGHT", 8, 0);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetHeight(20);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetWidth(72);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetAutoFocus(false);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameChannelWhisperTo:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QuestGuru_AnnounceFrameChannelEchoToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameChannelEchoToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameChannelEchoToggle:SetPoint("LEFT", "QuestGuru_AnnounceFrameChannelWhisperTo", "RIGHT", 24, 2);
	QuestGuru_AnnounceFrameChannelEchoToggleText:SetText(QG_OPT_ANNOUNCE_ECHO);
	QuestGuru_AnnounceFrameChannelEchoToggle.tooltipText = QG_OPT_ANNOUNCE_ECHO_DESC;

	temp = QuestGuru_AnnounceFrame:CreateFontString("QuestGuru_AnnounceFrameMessageLabel", "ARTWORK", "GameFontNormal");
	temp:SetText(QG_OPT_ANNOUNCE_MESSAGES);
	temp:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameChannelSayToggle", "BOTTOMLEFT", 0, -8);
	
	QuestGuru_AnnounceFrameMessageHelpButton = CreateFrame("BUTTON", "QuestGuru_AnnounceFrameMessageHelpButton", QuestGuru_AnnounceFrame, "UIPanelButtonTemplate");
	QuestGuru_AnnounceFrameMessageHelpButton:SetHeight(24);
	QuestGuru_AnnounceFrameMessageHelpButton:SetWidth(24);
	QuestGuru_AnnounceFrameMessageHelpButton:SetText("?");
	QuestGuru_AnnounceFrameMessageHelpButton:SetPoint("TOP", "QuestGuru_AnnounceFrameChannelSayToggle", "BOTTOM", 0, -24);
	QuestGuru_AnnounceFrameMessageHelpButton:SetPoint("RIGHT", -8, 0);
	QuestGuru_AnnounceFrameMessageHelpButton:SetScript("OnEnter",
		function (self)
			GameTooltip:SetOwner(self, "ANCHOR_RIGHT");
			GameTooltip:ClearLines();
			GameTooltip:AddLine(QG_OPT_ANNOUNCE_MSG_VARS);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_DONE,QG_OPT_ANNOUNCE_HELP_DONE);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_NEEDED,QG_OPT_ANNOUNCE_HELP_NEEDED);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_NUMLEFT,QG_OPT_ANNOUNCE_HELP_NUMLEFT);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_ITEMNAME,QG_OPT_ANNOUNCE_HELP_NAME);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_QUESTTITLE,QG_OPT_ANNOUNCE_HELP_TITLE);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_UITEXT,QG_OPT_ANNOUNCE_HELP_UITEXT);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_COMPLETE,QG_OPT_ANNOUNCE_HELP_COMPLETE);
			GameTooltip:AddDoubleLine(QG_ANNOUNCE_KEYWORD_QLINK,QG_OPT_ANNOUNCE_HELP_QLINK);
			GameTooltip:Show();
		end);
	QuestGuru_AnnounceFrameMessageHelpButton:SetScript("OnLeave",
		function ()
			GameTooltip:Hide();
		end);

	QuestGuru_AnnounceFrameMessageItemToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameMessageItemToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameMessageItemToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageLabel", "BOTTOMLEFT", 0, -2);
	QuestGuru_AnnounceFrameMessageItemToggleText:SetText(QG_OPT_ANNOUNCE_ITEM_MSG);
	
	QuestGuru_AnnounceFrameMessageItem = CreateFrame("EditBox", "QuestGuru_AnnounceFrameMessageItem", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameMessageItem:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageItemToggle", "BOTTOMLEFT", 2, 0);
	QuestGuru_AnnounceFrameMessageItem:SetHeight(20);
	QuestGuru_AnnounceFrameMessageItem:SetWidth(352);
	QuestGuru_AnnounceFrameMessageItem:SetAutoFocus(false);
	QuestGuru_AnnounceFrameMessageItem:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameMessageItem:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameMessageItem:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QuestGuru_AnnounceFrameMessageMonsterToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameMessageMonsterToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameMessageMonsterToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageItem", "BOTTOMLEFT", -2, -6);
	QuestGuru_AnnounceFrameMessageMonsterToggleText:SetText(QG_OPT_ANNOUNCE_MOB_MSG);
	
	QuestGuru_AnnounceFrameMessageMonster = CreateFrame("EditBox", "QuestGuru_AnnounceFrameMessageMonster", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameMessageMonster:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageMonsterToggle", "BOTTOMLEFT", 2, 0);
	QuestGuru_AnnounceFrameMessageMonster:SetHeight(20);
	QuestGuru_AnnounceFrameMessageMonster:SetWidth(352);
	QuestGuru_AnnounceFrameMessageMonster:SetAutoFocus(false);
	QuestGuru_AnnounceFrameMessageMonster:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameMessageMonster:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameMessageMonster:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QuestGuru_AnnounceFrameMessageEventToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameMessageEventToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameMessageEventToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageMonster", "BOTTOMLEFT", -2, -6);
	QuestGuru_AnnounceFrameMessageEventToggleText:SetText(QG_OPT_ANNOUNCE_EVENT_MSG);
	
	QuestGuru_AnnounceFrameMessageEvent = CreateFrame("EditBox", "QuestGuru_AnnounceFrameMessageEvent", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameMessageEvent:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageEventToggle", "BOTTOMLEFT", 2, 0);
	QuestGuru_AnnounceFrameMessageEvent:SetHeight(20);
	QuestGuru_AnnounceFrameMessageEvent:SetWidth(352);
	QuestGuru_AnnounceFrameMessageEvent:SetAutoFocus(false);
	QuestGuru_AnnounceFrameMessageEvent:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameMessageEvent:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameMessageEvent:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QuestGuru_AnnounceFrameMessageQuestToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameMessageQuestToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameMessageQuestToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageEvent", "BOTTOMLEFT", -2, -6);
	QuestGuru_AnnounceFrameMessageQuestToggleText:SetText(QG_OPT_ANNOUNCE_COMPLETE_MSG);
	
	QuestGuru_AnnounceFrameMessageQuest = CreateFrame("EditBox", "QuestGuru_AnnounceFrameMessageQuest", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameMessageQuest:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageQuestToggle", "BOTTOMLEFT", 2, 0);
	QuestGuru_AnnounceFrameMessageQuest:SetHeight(20);
	QuestGuru_AnnounceFrameMessageQuest:SetWidth(352);
	QuestGuru_AnnounceFrameMessageQuest:SetAutoFocus(false);
	QuestGuru_AnnounceFrameMessageQuest:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameMessageQuest:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameMessageQuest:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);

	QuestGuru_AnnounceFrameMessageQuestAcceptToggle = CreateFrame("CheckButton", "QuestGuru_AnnounceFrameMessageQuestAcceptToggle", QuestGuru_AnnounceFrame, "QuestGuru_InterfaceOptionsCheckButtonTemplate");
	QuestGuru_AnnounceFrameMessageQuestAcceptToggle:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageQuest", "BOTTOMLEFT", -2, -6);
	QuestGuru_AnnounceFrameMessageQuestAcceptToggleText:SetText(QG_OPT_ANNOUNCE_QUESTACCEPT_MSG);
	
	QuestGuru_AnnounceFrameMessageQuestAccept = CreateFrame("EditBox", "QuestGuru_AnnounceFrameMessageQuestAccept", QuestGuru_AnnounceFrame, "InputBoxTemplate");
	QuestGuru_AnnounceFrameMessageQuestAccept:SetPoint("TOPLEFT", "QuestGuru_AnnounceFrameMessageQuestAcceptToggle", "BOTTOMLEFT", 2, 0);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetHeight(20);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetWidth(352);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetAutoFocus(false);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetScript("OnShow",
		function ()
			QuestGuru_AnnounceFrameMessageQuestAccept:SetCursorPosition(1);
		end);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetScript("OnEscapePressed",
		function (self)
			self:ClearFocus();
		end);
end

function QuestGuru_QuestAnnounceLoadSettings()
	if (QuestGuru_Settings.Announce == nil) then
		QuestGuru_Settings.Announce={};
	end
	
	if (QuestGuru_Settings.Announce.Enabled ~= true) then
		QuestGuru_Settings.Announce.Enabled = false;
	end
	QuestGuru_AnnounceFrameAnnounceToggle:SetChecked(QuestGuru_Settings.Announce.Enabled);

	if (QuestGuru_Settings.Announce.item == nil) then
		QuestGuru_Settings.Announce.item = {};
	end
	if (QuestGuru_Settings.Announce.item.Enabled ~= false) then
	    QuestGuru_Settings.Announce.item.Enabled = true;
	end
	QuestGuru_AnnounceFrameMessageItemToggle:SetChecked(QuestGuru_Settings.Announce.item.Enabled);
	if (QuestGuru_Settings.Announce.item.Text == nil) then
		QuestGuru_Settings.Announce.item.Text = QG_ANNOUNCE_ITEM_DEFAULT;
	end
	QuestGuru_AnnounceFrameMessageItem:SetText(QuestGuru_Settings.Announce.item.Text);

	if (QuestGuru_Settings.Announce.monster == nil) then
	    QuestGuru_Settings.Announce.monster = {};
	end
	if (QuestGuru_Settings.Announce.monster.Enabled ~= false) then
		QuestGuru_Settings.Announce.monster.Enabled = true;
	end
	QuestGuru_AnnounceFrameMessageMonsterToggle:SetChecked(QuestGuru_Settings.Announce.monster.Enabled);
	if (QuestGuru_Settings.Announce.monster.Text == nil) then
		QuestGuru_Settings.Announce.monster.Text = QG_ANNOUNCE_MOB_DEFAULT;
	end
	QuestGuru_AnnounceFrameMessageMonster:SetText(QuestGuru_Settings.Announce.monster.Text);

	if (QuestGuru_Settings.Announce.QuestAccept == nil) then
		QuestGuru_Settings.Announce.QuestAccept = {};
	end
	if (QuestGuru_Settings.Announce.QuestAccept.Enabled ~= false) then
		QuestGuru_Settings.Announce.QuestAccept.Enabled = true;
	end
	QuestGuru_AnnounceFrameMessageQuestAcceptToggle:SetChecked(QuestGuru_Settings.Announce.QuestAccept.Enabled);
	if (QuestGuru_Settings.Announce.QuestAccept.Text == nil) then
		QuestGuru_Settings.Announce.QuestAccept.Text = QG_ANNOUNCE_QUESTACCEPT_DEFAULT;
	end
	QuestGuru_AnnounceFrameMessageQuestAccept:SetText(QuestGuru_Settings.Announce.QuestAccept.Text);

	if (QuestGuru_Settings.Announce.event == nil) then
		QuestGuru_Settings.Announce.event = {};
	end
	if (QuestGuru_Settings.Announce.event.Enabled ~= false) then
		QuestGuru_Settings.Announce.event.Enabled = true;
	end
	QuestGuru_AnnounceFrameMessageEventToggle:SetChecked(QuestGuru_Settings.Announce.event.Enabled);
	if (QuestGuru_Settings.Announce.event.Text == nil) then
		QuestGuru_Settings.Announce.event.Text = QG_ANNOUNCE_KEYWORD_UITEXT;
	end
	QuestGuru_AnnounceFrameMessageEvent:SetText(QuestGuru_Settings.Announce.event.Text);

	if (QuestGuru_Settings.Announce.Quest == nil) then
		QuestGuru_Settings.Announce.Quest = {};
	end
	if (QuestGuru_Settings.Announce.Quest.Enabled ~= false) then
		QuestGuru_Settings.Announce.Quest.Enabled = true;
	end
	QuestGuru_AnnounceFrameMessageQuestToggle:SetChecked(QuestGuru_Settings.Announce.Quest.Enabled);
	if (QuestGuru_Settings.Announce.Quest.Text == nil) then
		QuestGuru_Settings.Announce.Quest.Text = QG_ANNOUNCE_QUEST_TEXT;
	end
	QuestGuru_AnnounceFrameMessageQuest:SetText(QuestGuru_Settings.Announce.Quest.Text);
	
	if (QuestGuru_Settings.Announce.say ~= true) then
		QuestGuru_Settings.Announce.say = false;
	end
	QuestGuru_AnnounceFrameChannelSayToggle:SetChecked(QuestGuru_Settings.Announce.say);
	if (QuestGuru_Settings.Announce.party ~= false) then
		QuestGuru_Settings.Announce.party = true;
	end
	QuestGuru_AnnounceFrameChannelPartyToggle:SetChecked(QuestGuru_Settings.Announce.party);
	if (QuestGuru_Settings.Announce.guild ~= true) then
		QuestGuru_Settings.Announce.guild = false;
	end
	QuestGuru_AnnounceFrameChannelGuildToggle:SetChecked(QuestGuru_Settings.Announce.guild);
	if (QuestGuru_Settings.Announce.whisper == nil) then
		QuestGuru_Settings.Announce.whisper = {};
	end
	if (QuestGuru_Settings.Announce.whisper.Enabled ~= true) then
		QuestGuru_Settings.Announce.whisper.Enabled = false;
	end
	QuestGuru_AnnounceFrameChannelWhisperToggle:SetChecked(QuestGuru_Settings.Announce.whisper.Enabled);
	if (QuestGuru_Settings.Announce.whisper.To == nil) then
		QuestGuru_Settings.Announce.whisper.To = "";
	end
	QuestGuru_AnnounceFrameChannelWhisperTo:SetText(QuestGuru_Settings.Announce.whisper.To);
	if (QuestGuru_Settings.Announce.Echo ~= true) then
		QuestGuru_Settings.Announce.Echo = false;
	end
	QuestGuru_AnnounceFrameChannelEchoToggle:SetChecked(QuestGuru_Settings.Announce.Echo);
end

function QuestGuru_SetAnnounceDefaults()
	QuestGuru_Echo(QG_ANNOUNCE_RESET);
	QuestGuru_Settings.Announce.Enabled = false;
	QuestGuru_AnnounceFrameAnnounceToggle:SetChecked(QuestGuru_Settings.Announce.Enabled);
	QuestGuru_Settings.Announce.item = {};
	QuestGuru_Settings.Announce.item.Enabled = true;
	QuestGuru_AnnounceFrameMessageItemToggle:SetChecked(QuestGuru_Settings.Announce.item.Enabled);
	QuestGuru_Settings.Announce.item.Text = QG_ANNOUNCE_ITEM_DEFAULT;
	QuestGuru_AnnounceFrameMessageItem:SetText(QuestGuru_Settings.Announce.item.Text);
	QuestGuru_Settings.Announce.monster = {};
	QuestGuru_Settings.Announce.monster.Enabled = true;
	QuestGuru_AnnounceFrameMessageMonsterToggle:SetChecked(QuestGuru_Settings.Announce.monster.Enabled);
	QuestGuru_Settings.Announce.monster.Text = QG_ANNOUNCE_MOB_DEFAULT;
	QuestGuru_AnnounceFrameMessageMonster:SetText(QuestGuru_Settings.Announce.monster.Text);
	QuestGuru_Settings.Announce.QuestAccept = {};
	QuestGuru_Settings.Announce.QuestAccept.Enabled = true;
	QuestGuru_AnnounceFrameMessageQuestAcceptToggle:SetChecked(QuestGuru_Settings.Announce.QuestAccept.Enabled);
	QuestGuru_Settings.Announce.QuestAccept.Text = QG_ANNOUNCE_QUESTACCEPT_DEFAULT;
	QuestGuru_AnnounceFrameMessageQuestAccept:SetText(QuestGuru_Settings.Announce.QuestAccept.Text);
	QuestGuru_Settings.Announce.event = {};
	QuestGuru_Settings.Announce.event.Enabled = true;
	QuestGuru_AnnounceFrameMessageEventToggle:SetChecked(QuestGuru_Settings.Announce.event.Enabled);
	QuestGuru_Settings.Announce.event.Text = QG_ANNOUNCE_KEYWORD_UITEXT;
	QuestGuru_AnnounceFrameMessageEvent:SetText(QuestGuru_Settings.Announce.event.Text);
	QuestGuru_Settings.Announce.Quest = {};
	QuestGuru_Settings.Announce.Quest.Enabled = true;
	QuestGuru_AnnounceFrameMessageQuestToggle:SetChecked(QuestGuru_Settings.Announce.Quest.Enabled);
	QuestGuru_Settings.Announce.Quest.Text = QG_ANNOUNCE_QUEST_TEXT;
	QuestGuru_AnnounceFrameMessageQuest:SetText(QuestGuru_Settings.Announce.Quest.Text);

	QuestGuru_Settings.Announce.say = false;
	QuestGuru_AnnounceFrameChannelSayToggle:SetChecked(QuestGuru_Settings.Announce.say);
	QuestGuru_Settings.Announce.party = true;
	QuestGuru_AnnounceFrameChannelPartyToggle:SetChecked(QuestGuru_Settings.Announce.party);
	QuestGuru_Settings.Announce.guild = false;
	QuestGuru_AnnounceFrameChannelGuildToggle:SetChecked(QuestGuru_Settings.Announce.guild);
	QuestGuru_Settings.Announce.whisper = {};
	QuestGuru_Settings.Announce.whisper.Enabled = false;
	QuestGuru_AnnounceFrameChannelWhisperToggle:SetChecked(QuestGuru_Settings.Announce.whisper.Enabled);
	QuestGuru_Settings.Announce.whisper.To = "";
	QuestGuru_AnnounceFrameChannelWhisperTo:SetText(QuestGuru_Settings.Announce.whisper.To);
	QuestGuru_Settings.Announce.Echo = false;
	QuestGuru_AnnounceFrameChannelEchoToggle:SetChecked(QuestGuru_Settings.Announce.Echo);
end

function QuestGuru_SaveAnnounceSettings()
	if (QuestGuru_AnnounceFrameAnnounceToggle:GetChecked()) then
		QuestGuru_Settings.Announce.Enabled = true;
	else
		QuestGuru_Settings.Announce.Enabled = false;
	end
	if (QuestGuru_AnnounceFrameChannelSayToggle:GetChecked()) then
		QuestGuru_Settings.Announce.say = true;
	else
		QuestGuru_Settings.Announce.say = false;
	end
	if (QuestGuru_AnnounceFrameChannelPartyToggle:GetChecked()) then
		QuestGuru_Settings.Announce.party = true;
	else
		QuestGuru_Settings.Announce.party = false;
	end
	if (QuestGuru_AnnounceFrameChannelGuildToggle:GetChecked()) then
		QuestGuru_Settings.Announce.guild = true;
	else
		QuestGuru_Settings.Announce.guild = false;	
	end
	if (QuestGuru_AnnounceFrameChannelWhisperToggle:GetChecked()) then
		QuestGuru_Settings.Announce.whisper.Enabled = true;
	else
		QuestGuru_Settings.Announce.whisper.Enabled = false;
	end
	QuestGuru_Settings.Announce.whisper.To = QuestGuru_AnnounceFrameChannelWhisperTo:GetText();
	if (QuestGuru_AnnounceFrameChannelEchoToggle:GetChecked()) then
		QuestGuru_Settings.Announce.Echo = true;
	else
		QuestGuru_Settings.Announce.Echo = false;
	end
	if (QuestGuru_AnnounceFrameMessageItemToggle:GetChecked()) then
		QuestGuru_Settings.Announce.item.Enabled = true;
	else
		QuestGuru_Settings.Announce.item.Enabled = false;
	end
	QuestGuru_Settings.Announce.item.Text = QuestGuru_AnnounceFrameMessageItem:GetText();
	if (QuestGuru_AnnounceFrameMessageMonsterToggle:GetChecked()) then
		QuestGuru_Settings.Announce.monster.Enabled = true;
	else
		QuestGuru_Settings.Announce.monster.Enabled = false;
	end
	QuestGuru_Settings.Announce.monster.Text = QuestGuru_AnnounceFrameMessageMonster:GetText();
	if (QuestGuru_AnnounceFrameMessageQuestAcceptToggle:GetChecked()) then
		QuestGuru_Settings.Announce.QuestAccept.Enabled = true;
	else
		QuestGuru_Settings.Announce.QuestAccept.Enabled = false;
	end
	QuestGuru_Settings.Announce.QuestAccept.Text = QuestGuru_AnnounceFrameMessageQuestAccept:GetText();
	if (QuestGuru_AnnounceFrameMessageEventToggle:GetChecked()) then
		QuestGuru_Settings.Announce.event.Enabled = true;
	else
		QuestGuru_Settings.Announce.event.Enabled = false;
	end
	QuestGuru_Settings.Announce.event.Text = QuestGuru_AnnounceFrameMessageEvent:GetText();
	if (QuestGuru_AnnounceFrameMessageQuestToggle:GetChecked()) then
		QuestGuru_Settings.Announce.Quest.Enabled = true;
	else
		QuestGuru_Settings.Announce.Quest.Enabled = false;
	end
	QuestGuru_Settings.Announce.Quest.Text = QuestGuru_AnnounceFrameMessageQuest:GetText();
end

function QuestGuru_CancelAnnounceChanges()
	QuestGuru_AnnounceFrameAnnounceToggle:SetChecked(QuestGuru_Settings.Announce.Enabled);
	QuestGuru_AnnounceFrameChannelSayToggle:SetChecked(QuestGuru_Settings.Announce.say);
	QuestGuru_AnnounceFrameChannelPartyToggle:SetChecked(QuestGuru_Settings.Announce.party);
	QuestGuru_AnnounceFrameChannelGuildToggle:GetChecked(QuestGuru_Settings.Announce.guild);
	QuestGuru_AnnounceFrameChannelWhisperToggle:GetChecked(QuestGuru_Settings.Announce.whisper.Enabled);
	QuestGuru_AnnounceFrameChannelWhisperTo:SetText(QuestGuru_Settings.Announce.whisper.To);
	QuestGuru_AnnounceFrameChannelEchoToggle:SetChecked(QuestGuru_Settings.Announce.Echo);
	QuestGuru_AnnounceFrameMessageItemToggle:SetChecked(QuestGuru_Settings.Announce.item.Enabled);
	QuestGuru_AnnounceFrameMessageItem:SetText(QuestGuru_Settings.Announce.item.Text);
	QuestGuru_AnnounceFrameMessageMonsterToggle:SetChecked(QuestGuru_Settings.Announce.monster.Enabled);
	QuestGuru_AnnounceFrameMessageMonster:SetText(QuestGuru_Settings.Announce.monster.Text);
	QuestGuru_AnnounceFrameMessageQuestAcceptToggle:SetChecked(QuestGuru_Settings.Announce.QuestAccept.Enabled);
	QuestGuru_AnnounceFrameMessageQuestAccept:SetText(QuestGuru_Settings.Announce.QuestAccept.Text);
	QuestGuru_AnnounceFrameMessageEventToggle:SetChecked(QuestGuru_Settings.Announce.event.Enabled);
	QuestGuru_AnnounceFrameMessageEvent:SetText(QuestGuru_Settings.Announce.event.Text);
	QuestGuru_AnnounceFrameMessageQuestToggle:SetChecked(QuestGuru_Settings.Announce.Quest.Enabled);
	QuestGuru_AnnounceFrameMessageQuest:SetText(QuestGuru_Settings.Announce.Quest.Text);
end

function QuestGuru_DoAnnounce(announceText, numDone, numNeeded, itemName, questTitle, arg1, questID)
	if (not QuestGuru_Settings.Announce.Enabled) then return; end

	if (announceText ~= nil) then
		if (numDone == nil) then numDone = 0; end
		if (numNeeded == nil) then numNeeded = 0; end
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_DONE,numDone);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_NEEDED,numNeeded);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_NUMLEFT,numNeeded-numDone);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_ITEMNAME,itemName);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_QUESTTITLE,questTitle);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_UITEXT,arg1);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_COMPLETE,COMPLETE);
		announceText = string.gsub(announceText,QG_ANNOUNCE_KEYWORD_QLINK,GetQuestLink(questID));
	    
		if (QuestGuru_Settings.Announce.say) then
			SendChatMessage(announceText, "SAY");
		end
		if (QuestGuru_Settings.Announce.party and (IsInGroup() and GetNumSubgroupMembers(LE_PARTY_CATEGORY_HOME) > 0 and IsInRaid() == false)) then
			SendChatMessage(announceText, "PARTY");
		end
		if ((QuestGuru_Settings.Announce.guild) and IsInGuild()) then
			SendChatMessage(announceText, "GUILD");
		end
		if (QuestGuru_Settings.Announce.whisper.Enabled) then
			SendChatMessage(announceText, "WHISPER", nil, QuestGuru_Settings.Announce.whisper.To);
		end
		if (QuestGuru_Settings.Announce.Echo) then
			QuestGuru_Echo(announceText, true);
		end
	end
end

function QuestGuru_DoAnnounceByTitle(questTitle, announceText, numDone, numNeeded, itemName, arg1)
	local i;
	local numEntries, numQuests = GetNumQuestLogEntries();

	for i=1, numEntries do
		local qTitle, _, _, _, isHeader = GetQuestLogTitle(i);
		if not isHeader and (qTitle == questTitle) then
			QuestGuru_DoAnnounce(announceText, numDone, numNeeded, itemName, questTitle, arg1, i);
		end
	end
end