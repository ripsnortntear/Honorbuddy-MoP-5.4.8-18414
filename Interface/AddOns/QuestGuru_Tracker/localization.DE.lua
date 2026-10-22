------------------------------------
-- QuestGuru German Localization  --
------------------------------------
if (GetLocale() == "deDE") then
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

	-- Party Info Tooltip strings
	QG_ABANDONED = "Abgebrochen";
	QG_INCOMPLETE = "Nicht fertig";
	QG_ALT_STATUS_HEAD = "------------- Status eigener Charaktere -------------";
	QG_GUILD_STATUS_HEAD = "---------- Gildenmitglieder Status ----------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Alle Bereiche ein-/ausklappen";
	QG_OPTIONS = "Optionen";
	QG_SEARCH = "Suchen: ";
	QG_CLEAR_ABANDON = "Abgebrochen-Liste l\195\182schen";

	-- Tracker
	QG_TRACKER_SHOW = "Tracker anzeigen";
	QG_TRACKER_MINIMIZE = "Tracker minimieren";
	QG_TRACKER_OPTIONS = "Trackeroptionen";
	QG_TRACKER_TOGGLE = "Click to switch between Quest and Achievement tracking. Right-click to toggle showing both windows";
	QG_TRACKER_SORT = "Click to open a menu with quest sorting options";
	QG_TRACKER_QUESTS = "Quests";
	QG_TRACKER_ACHIEVE = "Achievements";
	QG_TRACKER_Q = "Q"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "A"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC = "Diese Optionen \195\164ndern das Aussehen des QuestGuru Trackers.\n\195\132nderungen werden sofort gespeichert.";
	QG_OPT_TRACKER_USE = "QuestGuru Tracker verwenden";
	QG_OPT_TRACKER_USE_DESC = "Deaktivieren Sie diese Option, um den Standardtracker oder andere Tracker Addons zu verwenden";
	QG_OPT_TRACKER_BORDER = "Rahmen anzeigen";
	QG_OPT_TRACKER_BORDER_DESC = "Aktivieren Sie diese Option, um einen d\195\188nnen Rahmen um den Tracker anzuzeigen";
	QG_OPT_TRACKER_CLICKTHROUGH = "Durch den Tracker klicken";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC = "Aktivieren Sie diese Option, um den Tracker nicht anklickbar zu machen";
	QG_OPT_TRACKER_HEADERS = "\195\156berschriften anzeigen";
	QG_OPT_TRACKER_HEADERS_DESC = "Aktivieren Sie diese Option, um Zonen\195\188berschriften im Tracker anzuzeigen";
	QG_OPT_TRACKER_LEVELS = "Queststufen anzeigen";
	QG_OPT_TRACKER_LEVELS_DESC = "Aktivieren Sie diese Option, um die Queststufen im Tracker anzuzeigen";
	QG_OPT_TRACKER_QUEST_TOOLTIPS = "Quest Tooltips anzeigen";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "Aktivieren Sie diese Option, um einen Tooltip mit Informationen anzuzeigen, wenn Sie mit dem Mauszeiger \195\188ber Questnamen oder Ziele fahren";
	QG_OPT_TRACKER_PARTY_TOOLTIPS = "Gruppen Tooltips anzeigen";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "Aktivieren Sie diese Option, um den Tooltip f\195\188r Gruppen-, Gilden und Zweitcharakterinformationen anzuzeigen, wenn Sie mit dem Mauszeiger \195\188ber Questnamen fahren";
	QG_OPT_TRACKER_PERCENT = "Quest Prozentwert anzeigen";
	QG_OPT_TRACKER_PERCENT_DESC = "Aktivieren Sie diese Option, um den Fortschrittsprozentwert neben dem Questnamen im Tracker anzuzeigen";
	QG_OPT_TRACKER_ANCHOR_BOTTOM = "Unten verankern";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC = "Verankert den unteren Rand des Trackers, um ihn nach oben zu vergr\195\182\195\159ern.";
	QG_OPT_TRACKER_AUTOTRACK = "Neue Q's automatisch beobachten";
	QG_OPT_TRACKER_AUTOTRACK_DESC = "Aktivieren Sie diese Option, um neue Quests automatisch zum Tracker hinzuzuf\195\188gen";
	QG_OPT_TRACKER_AUTOUNTRACK = "Fertige Q's nicht beobachten";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC = "Nach abschlie\195\159en aller Questziele Quest automtisch aus dem Tracker entfernen";
	QG_OPT_TRACKER_COMPLETE_OBJ = "Vollendete Ziele anzeigen";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC = "Deaktivieren Sie diese Option, um vollendete Questziele nicht im Tracker anzuzeigen";
	QG_OPT_TRACKER_COLOR_OBJ = "Ziele nach Fortschritt einf\195\164rben";
	QG_OPT_TRACKER_COLOR_OBJ_DESC = "Aktivieren Sie diese Option, um Questziele nach dem Fortschritt mit den folgenden Farben einzuf\195\164rben";
	QG_OPT_TRACKER_OBJ_COLOR_0 = "Farbe f\195\188r zu 0% vollendete Ziele";
	QG_OPT_TRACKER_OBJ_COLOR_99 = "Farbe f\195\188r zu 99% vollendete Ziele";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE = "Farbe f\195\188r vollendete Ziele";
	QG_OPT_TRACKER_SIZE = "Trackergr\195\182\195\159e";
	QG_OPT_TRACKER_MAX_LINES = "Max. Trackerzeilen:";
	QG_OPT_TRACKER_ALPHA = "Hintergrund-Alpha";
	QG_OPT_TRACKER_BULLET = "Bullet symbol to use for objectives:";
	QG_OPT_TRACKER_HIDECOMBAT = "Hide during combat";
	QG_OPT_TRACKER_HIDECOMBAT_DESC = "Selecting this option will automatically hide the tracker(s) during combat.";
	QG_OPT_TRACKER_HIDEBLIZZTRACK = "Hide Blizzard Tracker";
	QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC = "Select this option will hide the default Blizzard tracker. Uncheck if special functions of that tracker are needed.";
	QG_OPT_TRACKER_SORTQUESTNONE = "Sort by header/quest level";
	QG_OPT_TRACKER_SORTQUESTPROX = "Sort by proximity";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
	QG_ITEM_REQ_STR2 = "(.*):%s*%(([%d]+)%)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors (alternate format for some quests)
	QG_DATETIME = "%d/%m/%Y %H:%M:%S"; -- The format that date/time values are stored and displayed for start/finish info
end