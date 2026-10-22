--------------------------------------------
-- QuestGuru_Tracker English Localization --
--------------------------------------------

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

	-- Party Info Tooltip strings
	QG_ABANDONED = "Abandonded"
	QG_INCOMPLETE = "Not Complete";
	QG_ALT_STATUS_HEAD = "------------- Alts Quest Status -------------";
	QG_GUILD_STATUS_HEAD = "---------- Guild Members Status ----------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Expand/Collapse all headers";
	QG_OPTIONS = "Options";
	QG_SEARCH = "Search: ";
	QG_CLEAR_ABANDON = "Clear Abandon List";

	-- Tracker
	QG_TRACKER_SHOW = "Show Tracker";
	QG_TRACKER_MINIMIZE = "Minimize Tracker";
	QG_TRACKER_OPTIONS = "Tracker Options";
	QG_TRACKER_TOGGLE = "Click to switch between Quest and Achievement tracking. Right-click to toggle showing both windows";
	QG_TRACKER_SORT = "Click to open a menu with quest sorting options";
	QG_TRACKER_QUESTS = "Quests";
	QG_TRACKER_ACHIEVE = "Achievements";
	QG_TRACKER_Q = "Q"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "A"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC = "These options change the look of the QuestGuru Tracker.\nChanges to these settings are saved immediately.";
	QG_OPT_TRACKER_BORDER = "Show border";
	QG_OPT_TRACKER_BORDER_DESC = "Check this option to display a thin border around the tracker";
	QG_OPT_TRACKER_ITEMICONS = "Show quest item icons";
	QG_OPT_TRACKER_ITEMICONS_DESC = "Check this option to display icons for special quest items";
	QG_OPT_TRACKER_CLICKTHROUGH = "Click through tracker";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC = "Check this option to disable the mouse for the tracker, allowing you to click through it";
	QG_OPT_TRACKER_HEADERS = "Show headers";
	QG_OPT_TRACKER_HEADERS_DESC = "Checking this option displays zone headers in the tracker";
	QG_OPT_TRACKER_LEVELS = "Show quest levels";
	QG_OPT_TRACKER_LEVELS_DESC = "Checking this option shows quest level information in the tracker";
	QG_OPT_TRACKER_QUEST_TOOLTIPS = "Show quest tooltips";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "Checking this option displays a tooltip with more information when you mouse over a quest name or objective item in the tracker";
	QG_OPT_TRACKER_PARTY_TOOLTIPS = "Show party tooltips";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "Checking this option displays the party/alt/guild info tooltip when you mouse over a quest name in the tracker";
	QG_OPT_TRACKER_PERCENT = "Show quest percent";
	QG_OPT_TRACKER_PERCENT_DESC = "This option shows your quest progress percentage next to the quest name in the tracker";
	QG_OPT_TRACKER_ANCHOR_BOTTOM = "Anchor Bottom";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC = "Anchor the bottom of the tracker to make it expand upwards.";
	QG_OPT_TRACKER_PIN = "Pin Tracker Position";
	QG_OPT_TRACKER_PIN_DESC = "Check this option to lock the tracker in place so it can't be accidentally moved.";
	QG_OPT_TRACKER_AUTOUNTRACK = "Untrack complete quests";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC = "Automatically remove quests from the tracker upon completion of all objectives";
	QG_OPT_TRACKER_COMPLETE_OBJ = "Show completed objectives";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC = "Uncheck this option to hide complete objectives in the tracker";
	QG_OPT_TRACKER_COLOR_OBJ = "Colorize objective progress";
	QG_OPT_TRACKER_COLOR_OBJ_DESC = "Check this option to color objectives based on progress using the colors below";
	QG_OPT_TRACKER_OBJ_COLOR_0 = "Color for objectives that are 0% complete";
	QG_OPT_TRACKER_OBJ_COLOR_99 = "Color for objectives that are 99% complete";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE = "Color for objectives that are complete";
	QG_OPT_TRACKER_SIZE = "Tracker Size";
	QG_OPT_TRACKER_MAX_LINES = "Max. Tracker Lines:";
	QG_OPT_TRACKER_ALPHA = "Background Alpha";
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
	QG_DATETIME = "%m/%d/%Y %H:%M:%S"; -- The format that date/time values are stored and displayed for start/finish info