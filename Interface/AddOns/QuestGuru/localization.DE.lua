------------------------------------
-- QuestGuru German Localization  --
------------------------------------
if (GetLocale() == "deDE") then
	QG_REP_GAIN = "Euer Ruf bei der Fraktion '(.*)' hat sich um ([%d]+) verbessert.";
	QG_REP_DEC = "Euer Ruf bei der Fraktion '(.*)' hat sich um ([%d]+) verschlechtert.";
	QG_XP_GAIN = "Ihr bekommt ([%d]+) Erfahrung.";
	QG_QUEST_COMPLETE = "Quest fertig!";

	-- Default filler words used in various places
	QG_UNKNOWN = "Unbekannt";
	QG_NONE = "Nichts";
	QG_ACTIVE = "Aktiv";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "Beobachten";
	QG_UNTRACK = "Nicht beobachten";
	QG_SHARE_QUEST = "Quest teilen";
	QG_ABANDON_QUEST = "Quest abbrechen";
	QG_DELETE_QUEST = "Quest l\195\182schen";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Alle Bereiche ein-/ausklappen";
	QG_OPTIONS = "Optionen";
	QG_SEARCH = "Suchen: ";

	-- History strings
	QG_HISTORY_NONE = "No Quest History";
	QG_SHOWHISTORY = "History";
	QG_COLLEVEL = "Level";
	QG_COLTITLE = "Title";
	QG_COLSTATUS = "Status";
	QG_COLSTART = "Started by";

	-- Announcer
	QG_OPT_ANNOUNCE_HEAD = "QuestGuru Announcer Optionen";
	QG_OPT_ANNOUNCE_DESC = "Benutzen Sie diese Optionen, um Ihren Questfortschritt anderen mitzuteilen.\n\195\132nderungen werden beim Klick auf OK gespeichert.";
	QG_OPT_ANNOUNCE_PROGRESS = "Questfortschritt mitteilen";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD = "Kan\195\164le:";
	QG_OPT_ANNOUNCE_CHANNEL_SAY = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC = "Questfortschritt in \"Say\" mitteilen";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC = "Questfortschritt der Gruppe mitteilen";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC = "Questfortschritt der Gilde mitteilen";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "Questfortschritt jemandem zufl\195\188stern";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO = "an:";
	QG_OPT_ANNOUNCE_ECHO = "Echo";
	QG_OPT_ANNOUNCE_ECHO_DESC = "Fortschritt im allgemeinen Chatfenster anzeigen.";
	QG_OPT_ANNOUNCE_MESSAGES = "Nachrichten:";
	QG_OPT_ANNOUNCE_HELP_DONE = "Anzahl gesammelter Gegenst\195\164nde oder get\195\182teter Monster";
	QG_OPT_ANNOUNCE_HELP_NEEDED = "Gesamtzahl ben\195\182tigter Gegens\195\164tnde oder Monster";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT = "Anzahl noch ben\195\182tigter Gegenst\195\164nde oder Monster";
	QG_OPT_ANNOUNCE_HELP_NAME = "Zieltext ohne #/#";
	QG_OPT_ANNOUNCE_HELP_TITLE = "Name der Quest f\195\188r dieses Ziel";
	QG_OPT_ANNOUNCE_HELP_UITEXT = "The text that showed up onscreen";
	QG_OPT_ANNOUNCE_HELP_COMPLETE = "\195\156bersetzte Fertigmeldung: \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK = "Der Questlink der betroffenen Quest";
	QG_OPT_ANNOUNCE_ITEM_MSG = "Fortschritt bei Genst\195\164nden mit dieser Nachricht mitteilen:";
	QG_OPT_ANNOUNCE_MOB_MSG = "Fortschritt bei Monstern mit dieser Nachricht mitteilen:";
	QG_OPT_ANNOUNCE_EVENT_MSG = "Fortschritt bei Ereignissen mit dieser Nachricht mitteilen:";
	QG_OPT_ANNOUNCE_COMPLETE_MSG = "Quest Fertigstellung mit dieser Nachticht mitteilen:";
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
	QG_ANNOUNCE_RESET = "Announcer Optionen werden zur\195\188ckgesetzt.";

	-- Options
	QG_OPT_OPTIONS_DESC = "Die sind die allgemeinen Einstellungen von QuestGuru.\n\195\132nderungen werden sofort gespeichert.";
	QG_OPT_LEVELS_HEAD = "Stufen anzeigen";
	QG_OPT_LEVELS_QUESTLOG = "Im Questlog";
	QG_OPT_LEVELS_HISTORY = "In der Historie";
	QG_OPT_LEVELS_ABANDON = "In der Abgebrochen-Liste";
	QG_OPT_AUTOCOMPLETE = "Quests automatisch einl\195\182sen";
	QG_OPT_OBJ_ICON = "Zeige Gegenstandicons im Questlog";
	QG_OPT_HEADER_QUEST_NUM = "Show number of quests in headers";
	QG_OPT_DISABLE_COMM = "gesamte addon-zu-addon Kommunikation abschalten";

	QG_OPT_SOUND_HEAD = "Klangoptionen";
	QG_OPT_SOUND_DESC = "Benutzen Sie diese Optionen, um die Kl\195\164nge zu \195\164ndern, die beim Questfortschritt abgespielt werden.\n\195\132nderungen werden sofort gespeichert.";
	QG_OPT_SOUND_ENABLE = "Zus\195\164tzliche Kl\195\164nge aktivieren";
	QG_OPT_SOUND_OBJ = "Klang beim Zielfortschritt abspielen:";
	QG_OPT_SOUND_OBJ_DONE = "Klang beim abschlie\195\159en eines Questziels abspielen:";
	QG_SOUND_QUEST_DONE = "Klang beim Fertigstellen einer Quest abspielen:";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
end