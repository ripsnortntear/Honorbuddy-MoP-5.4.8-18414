-------------------------------------------------------------------------------
-- QuestGuru Chinese Localization(zhCN) By Honooon, http://t.qq.com/edosnite
-------------------------------------------------------------------------------
if (GetLocale() == "zhCN") then
	QG_REP_GAIN = "你在(.*)中的声望值提高了([%d]+)点.";
	QG_REP_DEC = "你在(.*)中的声望值降低了([%d]+)点.";
	QG_XP_GAIN = "你获得了([%d]+)点经验值";
	QG_QUEST_COMPLETE = "任务完成";

	-- Default filler words used in various places
	QG_UNKNOWN = "未知";
	QG_NONE = "没有";
	QG_ACTIVE = "Active";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "加入追踪";
	QG_UNTRACK = "取消追踪";
	QG_SHARE_QUEST = "共享任务";
	QG_ABANDON_QUEST = "放弃任务";
	QG_DELETE_QUEST = "删除任务";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "展开/收起所有标题";
	QG_OPTIONS = "选项";
	QG_SEARCH = "查找: ";

	-- History strings
	QG_HISTORY_NONE = "没有历史任务";
	QG_SHOWHISTORY = "历史任务";
	QG_COLLEVEL = "等级";
	QG_COLTITLE = "标题";
	QG_COLSTATUS = "Status";
	QG_COLSTART = "任务开始";

	-- Announcer
	QG_OPT_ANNOUNCE_HEAD                 = "QuestGuru通告选项";
	QG_OPT_ANNOUNCE_DESC                 = "使用这些选项向别人通告你的任务进展,改变设定后点击确定保存.";
	QG_OPT_ANNOUNCE_PROGRESS             = "通告任务进展";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD         = "频道:";
	QG_OPT_ANNOUNCE_CHANNEL_SAY          = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC     = "在 说 频道中通告";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY        = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC   = "在 队伍 频道中通告";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD        = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC   = "在 公会 频道中通告";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER      = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "悄悄话通告";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO   = "到:";
	QG_OPT_ANNOUNCE_ECHO                 = "聊天窗口提示";
	QG_OPT_ANNOUNCE_ECHO_DESC            = "在聊天窗口显示通告内容.";
	QG_OPT_ANNOUNCE_MESSAGES             = "信息:";
	QG_OPT_ANNOUNCE_HELP_DONE            = "已经收集的物品数或杀死的怪物数";
	QG_OPT_ANNOUNCE_HELP_NEEDED          = "需要收集的物品数或杀死的怪物数";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT         = "还需收集的物品数或杀死的怪物数";
	QG_OPT_ANNOUNCE_HELP_NAME            = "物件名称";
	QG_OPT_ANNOUNCE_HELP_TITLE           = "任务名称";
	QG_OPT_ANNOUNCE_HELP_UITEXT          = "在屏幕上显示的文字";
	QG_OPT_ANNOUNCE_HELP_COMPLETE        = "本地化的任务完成信息: \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK           = "受影响的任务连接";
	QG_OPT_ANNOUNCE_ITEM_MSG             = "使用该信息通告收集物品的进展:";
	QG_OPT_ANNOUNCE_MOB_MSG              = "使用该信息通告杀死怪物的进展:";
	QG_OPT_ANNOUNCE_EVENT_MSG            = "使用该信息通告事件的进展:";
	QG_OPT_ANNOUNCE_COMPLETE_MSG         = "使用该信息通告任务完成:";
	QG_OPT_ANNOUNCE_QUESTACCEPT_MSG      = "通告新接受的任务:";
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
--	QG_ANNOUNCE_ITEM_DEFAULT             = "$questTitle $name：$done／$needed.";
--	QG_ANNOUNCE_MOB_DEFAULT              = "$questTitle $name：$done／$needed.";
--	QG_ANNOUNCE_QUESTACCEPT_DEFAULT      = "$qlink 任务已接受.";
	QG_ANNOUNCE_RESET                    = "重置所有关于通告的设定为预设值.";

	-- Options
	QG_OPT_OPTIONS_DESC = "这些选项可以改变QuestGuru的一般设定,\n改变这些设定将会立刻保存.";
	QG_OPT_LEVELS_HEAD = "显示等级";
	QG_OPT_LEVELS_QUESTLOG = "在当前任务记录";
	QG_OPT_LEVELS_HISTORY = "在历史任务记录";
	QG_OPT_LEVELS_ABANDON = "在放弃任务记录";
	QG_OPT_AUTOCOMPLETE = "自动完成任务";
	QG_OPT_OBJ_ICON = "在记录中显示物品图标";
	QG_OPT_HEADER_QUEST_NUM = "在标题显示任务数量";
	QG_OPT_DISABLE_COMM = "关闭所有插件间的通信";

	QG_OPT_SOUND_HEAD = "声音选项";
	QG_OPT_SOUND_DESC = "使用该面板改变声音设定,\n更改这些选项将会立刻保存.";
	QG_OPT_SOUND_ENABLE = "允许额外声音";
	QG_OPT_SOUND_OBJ = "任务项目进行中的音效:";
	QG_OPT_SOUND_OBJ_DONE = "任务目标完成的音效:";
	QG_SOUND_QUEST_DONE = "任务完成时的音效:";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*)：%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
end