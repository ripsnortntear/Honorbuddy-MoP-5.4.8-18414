------------------------------------
-- QuestGuru Russian Localization by StingerSoft--
------------------------------------
if (GetLocale() == "ruRU") then
	-- Default filler words used in various places
	QG_UNKNOWN = "Неизвестный";
	QG_NONE = "Отсутствует";
	QG_ACTIVE = "Активный";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "Следить";
	QG_UNTRACK = "Не следить";
	QG_SHARE_QUEST = "Поделиться";
	QG_ABANDON_QUEST = "Отменить задание";
	QG_DELETE_QUEST = "Удалить задание";

	-- Party Info Tooltip strings
	QG_ABANDONED = "Отменённый"
	QG_INCOMPLETE = "Не выполненный";
	QG_ALT_STATUS_HEAD = "------------- Статус задания альта -------------";
	QG_GUILD_STATUS_HEAD = "---------- Статус участников гильдии ----------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Развернуть/Свернуть все загаловки";
	QG_OPTIONS = "Настройки";
	QG_SEARCH = "Поиск: ";
	QG_CLEAR_ABANDON = "Очистить список отменений";

	-- Tracker
	QG_TRACKER_SHOW = "Показать трекер";
	QG_TRACKER_MINIMIZE = "Свернуть трекер";
	QG_TRACKER_OPTIONS = "Настройки трекера";
	QG_TRACKER_TOGGLE = "Кликните чтобы переключиться между заданиями и достижениями. Правый-клик переключает показ обоих окон";
	QG_TRACKER_SORT = "Кликните чтобы открыть меню с опциями сортировки заданий";
	QG_TRACKER_QUESTS = "Задания";
	QG_TRACKER_ACHIEVE = "Достижения";
	QG_TRACKER_Q = "З"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "Д"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC = "Данный раздел поможет вам настроить внешний вид трекера QG.";
	QG_OPT_TRACKER_BORDER = "Показ края";
	QG_OPT_TRACKER_BORDER_DESC = "Отметьте данную опцию для отображения тонкого края вокруг трекера";
	QG_OPT_TRACKER_ITEMICONS = "Показ иконку предмета";
	QG_OPT_TRACKER_ITEMICONS_DESC = "Отметьте данную опцию для отображения иконки, для предметов в задании";
	QG_OPT_TRACKER_CLICKTHROUGH = "Клики по трекеру";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC = "Отметьте данную опцию для отключения мыши в окне трекера, то есть, окно не будет реагировать на ваши клики мышью по нему.";
	QG_OPT_TRACKER_HEADERS = "Показ заголовки";
	QG_OPT_TRACKER_HEADERS_DESC = "Отметьте данную опцию для отображения зоны в заголовках, в трекере";
	QG_OPT_TRACKER_LEVELS = "Показ уровни заданий";
	QG_OPT_TRACKER_LEVELS_DESC = "Отметьте данную опцию для отображения уровня задания в трекере";
	QG_OPT_TRACKER_QUEST_TOOLTIPS = "Показ подсказки заданий";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "Отметьте данную опцию для отображения подсказки с информацией о задании, при наводе мыши на название или на предмет требования задания в трекере";
	QG_OPT_TRACKER_PARTY_TOOLTIPS = "Показ подсказки группы";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "Отметьте данную опцию для отображения подсказки с информацией группы/альта/гильдии, при наводе мыши на название задания в трекере";
	QG_OPT_TRACKER_PERCENT = "Показ процент выполнения";
	QG_OPT_TRACKER_PERCENT_DESC = "Данный опция отвечает за отображение в трекере значения, степени выполнения вашего задания в процентах";
	QG_OPT_TRACKER_ANCHOR_BOTTOM = "Нижний якорь";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC = "Нижний якорь трекера даёт возможность раскрываться вверх.";
	QG_OPT_TRACKER_PIN = "Закрепить трекеер";
	QG_OPT_TRACKER_PIN_DESC = "Отметьте данную опцию для фиксации трекера, это обезопасит от случайного перемещения.";
	QG_OPT_TRACKER_AUTOUNTRACK = "Убирать выполненные";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC = "Автоматически убирает задание из трекера после выполнения всех требований";
	QG_OPT_TRACKER_COMPLETE_OBJ = "Выполненные требования";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC = "Сняв галку с опции, тем самым вы уберёте из трекера выполненные требования с заданий";
	QG_OPT_TRACKER_COLOR_OBJ = "Окраска требований, основанных на прогрессе";
	QG_OPT_TRACKER_COLOR_OBJ_DESC = "Отметьте данную опцию для окраски требований, основанных на прогрессе, используя цвета приведённые ниже";
	QG_OPT_TRACKER_OBJ_COLOR_0 = "Окраски требований, которые находятся в 0% стадии выполнения";
	QG_OPT_TRACKER_OBJ_COLOR_99 = "Окраски требований, которые находятся в 99% стадии выполнения";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE = "Окраски требований, которые уже выполнены";
	QG_OPT_TRACKER_SIZE = "Размер трекера";
	QG_OPT_TRACKER_MAX_LINES = "Макс. строк в трекере:";
	QG_OPT_TRACKER_ALPHA = "Прозрачность фона";
	QG_OPT_TRACKER_BULLET = "Символы для подмечивания целей задания:";
	QG_OPT_TRACKER_HIDECOMBAT = "Скрывать в бою";
	QG_OPT_TRACKER_HIDECOMBAT_DESC = "Отметьте данную опцию для автоматического сокрытия трекере при начале боя.";
	QG_OPT_TRACKER_HIDEBLIZZTRACK = "Скрыть трекер Blizzard";
	QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC = "Данная настройка скроет стандартный трекер Blizzard. Отключите даннй параметр, если необходимы специальные функций трекера";
	QG_OPT_TRACKER_SORTQUESTNONE = "Сорт. по заглавию/уровню";
	QG_OPT_TRACKER_SORTQUESTPROX = "Сорт. по схожести";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
	QG_ITEM_REQ_STR2 = "(.*):%s*%(([%d]+)%)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors (alternate format for some quests)
	QG_DATETIME = "%m/%d/%Y %H:%M:%S"; -- The format that date/time values are stored and displayed for start/finish info
	end
	