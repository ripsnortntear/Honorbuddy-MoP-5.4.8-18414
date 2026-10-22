------------------------------------
-- QuestGuru English Localization --
------------------------------------

	QG_REP_GAIN = "Reputation with (.*) increased by ([%d]+).";
	QG_REP_DEC = "Reputation with (.*) decreased by ([%d]+).";
	QG_XP_GAIN = "You gain ([%d]+) experience.";
	QG_QUEST_COMPLETE = "Quest Complete!";

	-- Default filler words used in various places
	QG_UNKNOWN = "Unknown";
	QG_NONE = "None";
	QG_ACTIVE = "Active";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "Track";
	QG_UNTRACK = "Untrack";
	QG_SHARE_QUEST = "Share Quest";
	QG_ABANDON_QUEST = "Abandon Quest";
	QG_DELETE_QUEST = "Delete Quest";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Expand/Collapse all headers";
	QG_OPTIONS = "Options";
	QG_SEARCH = "Search: ";

	-- History strings
	QG_HISTORY_NONE = "No Quest History";
	QG_SHOWHISTORY = "History";
	QG_COLLEVEL = "Level";
	QG_COLTITLE = "Title";
	QG_COLSTATUS = "Status";
	QG_COLSTART = "Started by";

	-- Announcer
	QG_OPT_ANNOUNCE_HEAD = "QuestGuru Announcer Options";
	QG_OPT_ANNOUNCE_DESC = "Use these options to announce your quest progress to others.\nChanges to these settings are saved when you click Okay.";
	QG_OPT_ANNOUNCE_PROGRESS = "Announce Quest Progress";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD = "Channels:";
	QG_OPT_ANNOUNCE_CHANNEL_SAY = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC = "Make announcements in Say";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC = "Make announcements to your Party";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC = "Make announcements to your Guild";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "Whisper your announcements to someone";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO = "to:";
	QG_OPT_ANNOUNCE_ECHO = "Echo";
	QG_OPT_ANNOUNCE_ECHO_DESC = "Display announcements in your General chat window.";
	QG_OPT_ANNOUNCE_MESSAGES = "Messages:";
	QG_OPT_ANNOUNCE_HELP_DONE = "Number of items collected or monsters killed";
	QG_OPT_ANNOUNCE_HELP_NEEDED = "Total number of items or monsters needed";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT = "Number of items or monsters remaining";
	QG_OPT_ANNOUNCE_HELP_NAME = "Objective text without #/#";
	QG_OPT_ANNOUNCE_HELP_TITLE = "Name of the quest for this objective";
	QG_OPT_ANNOUNCE_HELP_UITEXT = "The text that showed up onscreen";
	QG_OPT_ANNOUNCE_HELP_COMPLETE = "Localized complete message: \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK = "The quest link of the affected quest";
	QG_OPT_ANNOUNCE_ITEM_MSG = "Announce item objective progress using message:";
	QG_OPT_ANNOUNCE_MOB_MSG = "Announce monster objective progress using message:";
	QG_OPT_ANNOUNCE_EVENT_MSG = "Announce event objective progress using message:";
	QG_OPT_ANNOUNCE_COMPLETE_MSG = "Announce quest completion using message:";
	QG_OPT_ANNOUNCE_QUESTACCEPT_MSG = "Announce when you Accept a new Quest";
	QG_OPT_ANNOUNCE_MSG_VARS = "Announce Message Variables"
  -- Announcer substitution keywords
  QG_ANNOUNCE_KEYWORD_DONE = "$done";
  QG_ANNOUNCE_KEYWORD_NEEDED = "$needed";
  QG_ANNOUNCE_KEYWORD_NUMLEFT = "$numleft";
  QG_ANNOUNCE_KEYWORD_ITEMNAME = "$name";
  QG_ANNOUNCE_KEYWORD_QUESTTITLE = "$questTitle";
  QG_ANNOUNCE_KEYWORD_UITEXT = "$uitext";
  QG_ANNOUNCE_KEYWORD_COMPLETE = "$complete";
  QG_ANNOUNCE_KEYWORD_QLINK = "$qlink";
  -- Announcer message format strings
	QG_ANNOUNCE_QUEST_TEXT = format("%s %s!",
                                	QG_ANNOUNCE_KEYWORD_QLINK,
	                                QG_ANNOUNCE_KEYWORD_COMPLETE)
	QG_ANNOUNCE_ITEM_DEFAULT = format("Collected %s %s of %s needed for %s",
                                    QG_ANNOUNCE_KEYWORD_DONE,
                                    QG_ANNOUNCE_KEYWORD_ITEMNAME,
                                    QG_ANNOUNCE_KEYWORD_NEEDED,
                                    QG_ANNOUNCE_KEYWORD_QUESTTITLE)
	QG_ANNOUNCE_MOB_DEFAULT = format("%s %s of %s needed for %s",
                                    QG_ANNOUNCE_KEYWORD_DONE,
                                    QG_ANNOUNCE_KEYWORD_ITEMNAME,
                                    QG_ANNOUNCE_KEYWORD_NEEDED,
                                    QG_ANNOUNCE_KEYWORD_QUESTTITLE)
	QG_ANNOUNCE_QUESTACCEPT_DEFAULT = format("Started quest %s",
                                    QG_ANNOUNCE_KEYWORD_QLINK)
	QG_ANNOUNCE_RESET = "Resetting Announcer settings to defaults.";

	-- Options
	QG_OPT_OPTIONS_DESC = "These options change general settings for QuestGuru.\nChanges to these settings are saved immediately.";
	QG_OPT_LEVELS_HEAD = "Show Levels";
	QG_OPT_LEVELS_QUESTLOG = "In Quest Log";
	QG_OPT_LEVELS_HISTORY = "In History Log";
	QG_OPT_LEVELS_ABANDON = "In Abandon Log";
	QG_OPT_AUTOCOMPLETE = "AutoComplete Quests";
	QG_OPT_OBJ_ICON = "Show objective item icons in log";
	QG_OPT_HEADER_QUEST_NUM = "Show number of quests in headers";
	QG_OPT_DISABLE_COMM = "Disable all addon-to-addon communications";

	QG_OPT_SOUND_HEAD = "Sound Options";
	QG_OPT_SOUND_DESC = "Use this panel to change the sounds played on quest progress.\nChanges to these settings are saved immediately.";
	QG_OPT_SOUND_ENABLE = "Enable additional sounds";
	QG_OPT_SOUND_OBJ = "Play sound on objective progress:";
	QG_OPT_SOUND_OBJ_DONE = "Play sound on objective completion:";
	QG_SOUND_QUEST_DONE = "Play sound on quest completion:";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors