------------------------------------
-- QuestGuru Russian Localization by StingerSoft--
------------------------------------
if (GetLocale() == "ruRU") then
	QG_REP_GAIN = "Отношение |3-7(.*) к вам улучшилось на ([%d]+).";
	QG_REP_DEC = "Отношение |3-7(.*) к вам ухудшилось на ([%d]+).";
	QG_XP_GAIN = "Вы получ[илиаете] ([%d]+) |4очко:очка:очков; опыта.";
	QG_QUEST_COMPLETE = "Задание выполнено!";

	-- Default filler words used in various places
	QG_UNKNOWN = "Неизвестный";
	QG_NONE = "Отсутствует";
	QG_ACTIVE = "Активно";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "Следить";
	QG_UNTRACK = "Не следить";
	QG_SHARE_QUEST = "Поделиться";
	QG_ABANDON_QUEST = "Отменить задание";
	QG_DELETE_QUEST = "Удалить задание";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Развернуть/Свернуть все загаловки";
	QG_OPTIONS = "Настройки";
	QG_SEARCH = "Поиск: ";
	
	-- History strings
	QG_HISTORY_NONE = "Нет истории заданий";
	QG_SHOWHISTORY = "История";
	QG_COLLEVEL = "Уровень";
	QG_COLTITLE = "Заглавие";
	QG_COLSTATUS = "Статус";
	QG_COLSTART = "Сортировано по";

	-- Announcer
	QG_OPT_ANNOUNCE_HEAD = "Настройки оповещений QuestGuru";
	QG_OPT_ANNOUNCE_DESC = "Настройки оповещений о ваших продвижениях по заданиям.\nИзменения настроек сохраняться после нажатия на кнопку Ok.";
	QG_OPT_ANNOUNCE_PROGRESS = "Сообщать о прогрессе задания";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD = "Каналы:";
	QG_OPT_ANNOUNCE_CHANNEL_SAY = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC = "Сказать";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC = "Сообщать в вашу группу";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC = "Сообщать в гильдию";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "Сообщить шепотом кому-нибудь";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO = "в:";
	QG_OPT_ANNOUNCE_ECHO = "Эхо";
	QG_OPT_ANNOUNCE_ECHO_DESC = "Отображать оповещения в окне вашего общего чата.";
	QG_OPT_ANNOUNCE_MESSAGES = "Сообщение:";
	QG_OPT_ANNOUNCE_HELP_DONE = "Число собранных предметов или убитых существ";
	QG_OPT_ANNOUNCE_HELP_NEEDED = "Общее число требуемых предметов или существ";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT = "Число оставшихся предметов или существ";
	QG_OPT_ANNOUNCE_HELP_NAME = "Текст задачи без #/#";
	QG_OPT_ANNOUNCE_HELP_TITLE = "Название задания для данной задачи";
	QG_OPT_ANNOUNCE_HELP_UITEXT = "Текст отображаемый вверху экрана";
	QG_OPT_ANNOUNCE_HELP_COMPLETE = "Сообщение о завершении: \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK = "Ссылка на задание";
	QG_OPT_ANNOUNCE_ITEM_MSG = "Оповещать о продвижение по задаче м предметами:";
	QG_OPT_ANNOUNCE_MOB_MSG = "Оповещать о продвижение по задаче с монстрами:";
	QG_OPT_ANNOUNCE_EVENT_MSG = "Оповещать о событии по продвижению по задаче:";
	QG_OPT_ANNOUNCE_COMPLETE_MSG = "Оповещать о выполненном задании:";
    QG_OPT_ANNOUNCE_QUESTACCEPT_MSG = "Оповещать о взятии нового задания";
	QG_OPT_ANNOUNCE_MSG_VARS = "Переменные оповещений"
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
	QG_ANNOUNCE_ITEM_DEFAULT = format("Собрано %s %s - %s, необхадимых для %s",
                                    QG_ANNOUNCE_KEYWORD_DONE,
                                    QG_ANNOUNCE_KEYWORD_ITEMNAME,
                                    QG_ANNOUNCE_KEYWORD_NEEDED,
                                    QG_ANNOUNCE_KEYWORD_QUESTTITLE)
	QG_ANNOUNCE_MOB_DEFAULT = format("%s %s - %s необхадимых для %s",
                                    QG_ANNOUNCE_KEYWORD_DONE,
                                    QG_ANNOUNCE_KEYWORD_ITEMNAME,
                                    QG_ANNOUNCE_KEYWORD_NEEDED,
                                    QG_ANNOUNCE_KEYWORD_QUESTTITLE)
	QG_ANNOUNCE_QUESTACCEPT_DEFAULT = format("Начато задание: %s",
                                    QG_ANNOUNCE_KEYWORD_QLINK)
	--QG_ANNOUNCE_ITEM_DEFAULT = "Собрано $done $name из $needed требуемых для $questTitle";
	--QG_ANNOUNCE_MOB_DEFAULT = "$done $name из $needed требуемых для $questTitle";
	--QG_ANNOUNCE_QUESTACCEPT_DEFAULT = "Взято задание $qlink";
	QG_ANNOUNCE_RESET = "Восcтоновить стандартные настройки оповещений.";

	-- Options
	QG_OPT_OPTIONS_DESC = "Основные настройки QuestGuru.\nИзменения настроек сохраняются мгновенно.";
	QG_OPT_LEVELS_HEAD = "Показ уровни";
	QG_OPT_LEVELS_QUESTLOG = "В журнале заданий";
	QG_OPT_LEVELS_HISTORY = "В журнале истории";
	QG_OPT_LEVELS_ABANDON = "В журнале отмен";
	QG_OPT_AUTOCOMPLETE = "Авто-сдавать задания";
	QG_OPT_OBJ_ICON = "Показ в журнале иконку требуемого предмета";
	QG_OPT_HEADER_QUEST_NUM = "Показ количевства заданий в загаловке";
	QG_OPT_DISABLE_COMM = "Отключить все коммуникации addon < - > addon";

	QG_OPT_SOUND_HEAD = "Настройки звуков";
	QG_OPT_SOUND_DESC = "Используйте панели для смены проигрываемого звука в ходе выполнения задания.";
	QG_OPT_SOUND_ENABLE = "Включить дополнительные звуки";
	QG_OPT_SOUND_OBJ = "Проиграть звук во время выполнения требований:";
	QG_OPT_SOUND_OBJ_DONE = "Проиграть звук после выполнения требования:";
	QG_SOUND_QUEST_DONE = "Проиграть звук после выполнения задания:";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors

end