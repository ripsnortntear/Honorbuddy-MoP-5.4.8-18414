-------------------------------------------------------------------------------
-- QuestGuru Chinese Localization(zhTW) By Silver Fox, fixed/updated By Player Lin -- 
-------------------------------------------------------------------------------
if (GetLocale() == "zhTW") then
	QG_REP_GAIN = "陣營 (.*) 的聲望增加了 ([%d]+).";
	QG_REP_DEC = "陣營 (.*) 的聲望降低了 ([%d]+).";
	QG_XP_GAIN = "你獲得 ([%d]+) 經驗值.";
	QG_QUEST_COMPLETE = "任務完成!";

	-- Default filler words used in various places
	QG_UNKNOWN = "未知";
	QG_NONE    = "沒有";
	QG_ACTIVE            = "進行";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK         = "加入追蹤";
	QG_UNTRACK       = "取消追蹤";
	QG_SHARE_QUEST   = "分享任務";
	QG_ABANDON_QUEST = "放棄任務";
	QG_DELETE_QUEST  = "刪除任務";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "收起/展開所有標題";
	QG_OPTIONS        = "選項";
	QG_SEARCH         = "搜尋: ";

	-- History strings
	QG_HISTORY_NONE    = "沒有任何歷史任務";
	QG_SHOWHISTORY = "History";
	QG_COLLEVEL = "Level";
	QG_COLTITLE = "Title";
	QG_COLSTATUS = "Status";
	QG_COLSTART = "Started by";
	
	-- Announcer
	QG_OPT_ANNOUNCE_HEAD                 = "QuestGuru Announcer 選項";
	QG_OPT_ANNOUNCE_DESC                 = "這些選項可以改變廣播的設定.";
	QG_OPT_ANNOUNCE_PROGRESS             = "廣播任務的進行狀況";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD         = "使用頻道:";
	QG_OPT_ANNOUNCE_CHANNEL_SAY          = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC     = "在 說 頻道中廣播";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY        = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC   = "在 隊伍聊天 頻道中廣播";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD        = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC   = "在 公會聊天 頻道中廣播";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER      = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "在 悄悄話 頻道中廣播";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO   = "誰:";
	QG_OPT_ANNOUNCE_ECHO                 = "對話視窗提示";
	QG_OPT_ANNOUNCE_ECHO_DESC            = "在你的對話視窗中顯示廣播內容.";
	QG_OPT_ANNOUNCE_MESSAGES             = "顯示訊息:";
	QG_OPT_ANNOUNCE_HELP_DONE            = "目前收集的物品或是需要殺死的怪物數量";
	QG_OPT_ANNOUNCE_HELP_NEEDED          = "需要收集的物品或是需要殺死的怪物數量";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT         = "尚需收集的物品或是需要殺死的怪物數量";
	QG_OPT_ANNOUNCE_HELP_NAME            = "物件名稱";
	QG_OPT_ANNOUNCE_HELP_TITLE           = "任務名稱";
	QG_OPT_ANNOUNCE_HELP_UITEXT          = "顯示在螢幕上的文字";
	QG_OPT_ANNOUNCE_HELP_COMPLETE        = "中文化的任務完成訊息: \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK           = "受到影響的任務連結";
	QG_OPT_ANNOUNCE_ITEM_MSG             = "廣播收集物品進度時使用這個訊息:";
	QG_OPT_ANNOUNCE_MOB_MSG              = "廣播殺死怪物進度時使用這個訊息:";
	QG_OPT_ANNOUNCE_EVENT_MSG            = "廣播事件完成進度時使用這個訊息:";
	QG_OPT_ANNOUNCE_COMPLETE_MSG         = "廣播任務完成時使用這個訊息:";
	QG_OPT_ANNOUNCE_QUESTACCEPT_MSG      = "廣播接受新任務";
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
	--QG_ANNOUNCE_ITEM_DEFAULT             = "$questTitle 需要收集物品－$name：$done／$needed.";
	--QG_ANNOUNCE_MOB_DEFAULT              = "$questTitle 需要殺死生物－$name：$done／$needed.";
	--QG_ANNOUNCE_QUESTACCEPT_DEFAULT      = "$qlink 任務已接受.";
	QG_ANNOUNCE_RESET                    = "恢復所有關於廣播的設定為預設值.";

	-- Options
	QG_OPT_OPTIONS_DESC    = "這些選項可以改變基本的設定.";
	QG_OPT_LEVELS_HEAD     = "顯示等級";
	QG_OPT_LEVELS_QUESTLOG = "目前任務列表中";
	QG_OPT_LEVELS_HISTORY  = "歷史任務列表中";
	QG_OPT_LEVELS_ABANDON  = "放棄任務列表中";
	QG_OPT_AUTOCOMPLETE    = "自動回覆已完成的任務";
	QG_OPT_OBJ_ICON        = "在記錄中顯示物品圖示";
	QG_OPT_HEADER_QUEST_NUM = "在區域標題顯示任務數量";
	QG_OPT_DISABLE_COMM    = "關閉所有插件與插件間的通訊";

	QG_OPT_SOUND_HEAD     = "音效選項";
	QG_OPT_SOUND_DESC     = "這些選項可以改變音效的設定.";
	QG_OPT_SOUND_ENABLE   = "開啟額外的音效功能";
	QG_OPT_SOUND_OBJ      = "任務項目進行中的音效:";
	QG_OPT_SOUND_OBJ_DONE = "任務項目已完成的音效:";
	QG_SOUND_QUEST_DONE   = "任務全部完成時的音效:";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
end