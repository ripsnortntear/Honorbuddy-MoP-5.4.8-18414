-------------------------------------------------------------------------------
-- QuestGuru Chinese Localization(zhCN) By Honooon, http://t.qq.com/edosnite
-------------------------------------------------------------------------------
if (GetLocale() == "zhCN") then
	-- Default filler words used in various places
	QG_UNKNOWN = "未知";
	QG_NONE    = "没有";
	QG_ACTIVE  = "进行";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK         = "加入追踪";
	QG_UNTRACK       = "取消追踪";
	QG_SHARE_QUEST   = "分享任务";
	QG_ABANDON_QUEST = "放弃任务";
	QG_DELETE_QUEST  = "删除任务";

	-- Party Info Tooltip strings
	QG_ABANDONED         = "放弃"
	QG_INCOMPLETE        = "未完";
	QG_ALT_STATUS_HEAD   = "------------- 分身角色任务状况 -------------";
	QG_GUILD_STATUS_HEAD = "------------- 公会成员任务状况 -------------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "收起/展开所有标题";
	QG_OPTIONS        = "选项";
	QG_SEARCH         = "搜寻: ";
	QG_CLEAR_ABANDON  = "清除放弃任务列表";

	-- Tracker
	QG_TRACKER_SHOW     = "显示追踪视窗";
	QG_TRACKER_MINIMIZE = "缩小追踪视窗";
	QG_TRACKER_OPTIONS  = "追踪视窗设定";	
	QG_TRACKER_TOGGLE = "点击切换追踪任务或追踪成就. 右键-点击 另外开启窗口显示这个追踪项目";
	QG_TRACKER_SORT = "点击打开任务排序选项菜单";
	QG_TRACKER_QUESTS = "任务";
	QG_TRACKER_ACHIEVE = "成就";
	QG_TRACKER_Q = "Q"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "A"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC                = "这些选项可以改变追踪视窗的设定.";
	QG_OPT_TRACKER_BORDER              = "显示追踪视窗边框";
	QG_OPT_TRACKER_BORDER_DESC         = "勾选这项会在追踪视窗外围显示边框";
	QG_OPT_TRACKER_ITEMICONS           = "显示任务物品图标";
	QG_OPT_TRACKER_ITEMICONS_DESC      = "点击这个设置是否显示任务物品的图标显示";
	QG_OPT_TRACKER_CLICKTHROUGH        = "点选可穿越追踪视窗";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC   = "勾选这项滑鼠的点选可以穿越追踪视窗";
	QG_OPT_TRACKER_HEADERS             = "显示任务分类标题";
	QG_OPT_TRACKER_HEADERS_DESC        = "勾选这项在任务追踪视窗会显示任务分类的标题";
	QG_OPT_TRACKER_LEVELS              = "显示任务等级";
	QG_OPT_TRACKER_LEVELS_DESC         = "勾选这项在任务追踪视窗会显示任务等级";
	QG_OPT_TRACKER_QUEST_TOOLTIPS      = "显示任务提示";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "勾选这项让你的滑鼠游标移到任务上时可以看到提示";
	QG_OPT_TRACKER_PARTY_TOOLTIPS      = "显示队伍提示";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "勾选这项会让你的滑鼠游标移到任务上时会显示 队伍/分身/公会 的任务状况";
	QG_OPT_TRACKER_PERCENT             = "显示任务完成比例";
	QG_OPT_TRACKER_PERCENT_DESC        = "这个选项可以显示任务的完成比例";
	QG_OPT_TRACKER_ANCHOR_BOTTOM       = "固定底部";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC  = "勾选这个项目追踪视窗会以底部为准进行缩放";
	QG_OPT_TRACKER_PIN                 = "锁定追踪视窗";
	QG_OPT_TRACKER_PIN_DESC            = "勾选这个项目会锁定追踪视窗";
	QG_OPT_TRACKER_AUTOUNTRACK         = "完成任务自动解除追踪";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC    = "勾选这个项目可以自动将已经完成的任务从追踪视窗中移除";
	QG_OPT_TRACKER_COMPLETE_OBJ        = "显示已完成的任务项目";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC   = "取消勾选这项让已经完成的任务项目从追踪视窗中消失";
	QG_OPT_TRACKER_COLOR_OBJ           = "用颜色来标示任务完成比例";
	QG_OPT_TRACKER_COLOR_OBJ_DESC      = "勾选这个项目可以让你用颜色来标示任务的完成比例";
	QG_OPT_TRACKER_OBJ_COLOR_0         = "设定任务项目完成比例  0% 时的颜色";
	QG_OPT_TRACKER_OBJ_COLOR_99        = "设定任务项目完成比例 99% 时的颜色";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE  = "设定任务项目完成的颜色";
	QG_OPT_TRACKER_SIZE                = "追踪视窗尺寸";
	QG_OPT_TRACKER_MAX_LINES           = "追踪视窗行数:";
	QG_OPT_TRACKER_ALPHA               = "视窗背景透明度";
	QG_OPT_TRACKER_BULLET              = "任务目标前置符号:";
	QG_OPT_TRACKER_HIDECOMBAT = "战斗状态时隐藏";
	QG_OPT_TRACKER_HIDECOMBAT_DESC = "勾选这个项目可以让追踪窗口在战斗状态时自动隐藏.";
	QG_OPT_TRACKER_HIDEBLIZZTRACK = "隐藏暴雪系统自带任务追踪窗口";
	QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC = "勾选这个项目可以隐藏系统自带的任务追踪窗口. 如果某些特殊动作需要的话请取消勾选这个项目.";
	QG_OPT_TRACKER_SORTQUESTNONE = "按标题/任务等级排序";
	QG_OPT_TRACKER_SORTQUESTPROX = "Sort by proximity";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*)：%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
	QG_ITEM_REQ_STR2 = "(.*)：%s*%(([%d]+)%)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors (alternate format for some quests)
	QG_DATETIME = "%m/%d/%Y %H:%M:%S"; -- The format that date/time values are stored and displayed for start/finish info
end