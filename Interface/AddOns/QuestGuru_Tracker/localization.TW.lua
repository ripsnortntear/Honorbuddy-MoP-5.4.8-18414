-------------------------------------------------------------------------------
-- QuestGuru Chinese Localization(zhTW) By Silver Fox, fixed/updated By Player Lin -- 
-------------------------------------------------------------------------------
if (GetLocale() == "zhTW") then
	-- Default filler words used in various places
	QG_UNKNOWN = "未知";
	QG_NONE    = "沒有";
	QG_ACTIVE  = "進行";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK         = "加入追蹤";
	QG_UNTRACK       = "取消追蹤";
	QG_SHARE_QUEST   = "分享任務";
	QG_ABANDON_QUEST = "放棄任務";
	QG_DELETE_QUEST  = "刪除任務";

	-- Party Info Tooltip strings
	QG_ABANDONED         = "放棄"
	QG_INCOMPLETE        = "未完";
	QG_ALT_STATUS_HEAD   = "------------- 分身角色任務狀況 -------------";
	QG_GUILD_STATUS_HEAD = "------------- 公會成員任務狀況 -------------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "收起/展開所有標題";
	QG_OPTIONS        = "選項";
	QG_SEARCH         = "搜尋: ";
	QG_CLEAR_ABANDON  = "清除放棄任務列表";

	-- Tracker
	QG_TRACKER_SHOW     = "顯示追蹤視窗";
	QG_TRACKER_MINIMIZE = "縮小追蹤視窗";
	QG_TRACKER_OPTIONS  = "追蹤視窗設定";
	QG_TRACKER_TOGGLE = "點擊轉換任務和成就追蹤. 右鍵點擊切換顯示視窗";
	QG_TRACKER_SORT = "Click to open a menu with quest sorting options";
	QG_TRACKER_QUESTS = "任務";
	QG_TRACKER_ACHIEVE = "成就";
	QG_TRACKER_Q = "Q"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "A"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC                = "這些選項可以改變追蹤視窗的設定.";
	QG_OPT_TRACKER_BORDER              = "顯示追蹤視窗邊框";
	QG_OPT_TRACKER_BORDER_DESC         = "勾選這個項目會在追蹤視窗外圍顯示邊框";
	QG_OPT_TRACKER_ITEMICONS           = "顯示任務物品圖示";
	QG_OPT_TRACKER_ITEMICONS_DESC      = "溝選這項顯示特殊任務物品圖示";
	QG_OPT_TRACKER_CLICKTHROUGH        = "點選可穿越追蹤視窗";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC   = "勾選這項滑鼠的點選可以穿越追蹤視窗";
	QG_OPT_TRACKER_HEADERS             = "顯示任務分類標題";
	QG_OPT_TRACKER_HEADERS_DESC        = "勾選這個項目在任務追蹤視窗會顯示任務分類的標題";
	QG_OPT_TRACKER_LEVELS              = "顯示任務等級";
	QG_OPT_TRACKER_LEVELS_DESC         = "勾選這個項目在任務追蹤視窗會顯示任務等級";
	QG_OPT_TRACKER_QUEST_TOOLTIPS      = "顯示任務提示";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "勾選這個項目讓你的滑鼠游標移到任務上時可以看到提示";
	QG_OPT_TRACKER_PARTY_TOOLTIPS      = "顯示隊伍提示";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "勾選這個項目會讓你的滑鼠游標移到任務上時會顯示 隊伍/分身/公會 的任務狀況";
	QG_OPT_TRACKER_PERCENT             = "顯示任務完成比例";
	QG_OPT_TRACKER_PERCENT_DESC        = "這個選項可以顯示任務的完成比例";
	QG_OPT_TRACKER_ANCHOR_BOTTOM       = "固定底部";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC  = "勾選這個項目追蹤視窗會以底部為準進行縮放";
	QG_OPT_TRACKER_PIN                 = "鎖定追蹤視窗";
	QG_OPT_TRACKER_PIN_DESC            = "勾選這個項目將鎖定追蹤視窗所以無法自由移動.";
	QG_OPT_TRACKER_AUTOUNTRACK         = "完成任務自動解除追蹤";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC    = "勾選這個項目可以自動將已經完成的任務從追蹤視窗中移除";
	QG_OPT_TRACKER_COMPLETE_OBJ        = "顯示已完成的任務項目";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC   = "取消勾選這項讓已經完成的任務項目從追蹤視窗中消失";
	QG_OPT_TRACKER_COLOR_OBJ           = "用顏色來標示任務完成比例";
	QG_OPT_TRACKER_COLOR_OBJ_DESC      = "勾選這個項目可以讓你用顏色來標示任務的完成比例";
	QG_OPT_TRACKER_OBJ_COLOR_0         = "設定任務項目完成比例  0% 時的顏色";
	QG_OPT_TRACKER_OBJ_COLOR_99        = "設定任務項目完成比例 99% 時的顏色";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE  = "設定任務項目完成的顏色";
	QG_OPT_TRACKER_SIZE                = "追蹤視窗尺寸";
	QG_OPT_TRACKER_MAX_LINES           = "追蹤視窗行數:";
	QG_OPT_TRACKER_ALPHA               = "視窗背景透明度";
	QG_OPT_TRACKER_BULLET = "任務目標前置符號:";
	QG_OPT_TRACKER_HIDECOMBAT = "Hide during combat";
	QG_OPT_TRACKER_HIDECOMBAT_DESC = "Selecting this option will automatically hide the tracker(s) during combat.";
	QG_OPT_TRACKER_HIDEBLIZZTRACK = "Hide Blizzard Tracker";
	QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC = "Select this option will hide the default Blizzard tracker. Uncheck if special functions of that tracker are needed.";
	QG_OPT_TRACKER_SORTQUESTNONE = "Sort by header/quest level";
	QG_OPT_TRACKER_SORTQUESTPROX = "Sort by proximity";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
	QG_ITEM_REQ_STR2 = "(.*):%s*%(([%d]+)%)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors (alternate format for some quests)
	QG_DATETIME     = "%m/%d/%Y %H:%M:%S"; -- The format that date/time values are stored and displayed for start/finish info
end