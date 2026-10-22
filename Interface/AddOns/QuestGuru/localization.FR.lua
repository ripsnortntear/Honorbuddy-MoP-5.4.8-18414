------------------------------------
-- QuestGuru French Localization --
------------------------------------

--	À	\195\128
--	Á	\195\129
--	Â	\195\130
--	Ä	\195\132
--	È	\195\136
--	É	\195\137
--	Ê	\195\138
--	Ë	\195\139
--	Î	\195\142
--	Ï	\195\143
--	Ô	\195\148
--	Ö	\195\150
--	Û	\195\155
--	Ü	\195\156
--	à	\195\160
--	á	\195\161
--	â	\195\162
--	ä	\195\164
--	è	\195\168
--	é	\195\169
--	ê	\195\170
--	ë	\195\171
--	î	\195\174
--	ï	\195\175
--	ô	\195\180
--	ö	\195\182
--	û	\195\187
--	ü	\195\188
--	'Œ	\39\197\146
--

if (GetLocale() == "frFR") then
	QG_REP_GAIN = "R\195\169putation aupr\195\168s de (.*) augment\195\169e de ([%d]+).";
	QG_REP_DEC = "R\195\169putation aupr\195\168s de (.*) diminu\195\169e de ([%d]+).";
	QG_XP_GAIN = "Vous gagnez ([%d]+) points d'exp\195\169rience.";
	QG_QUEST_COMPLETE = "Qu\195\170te termin\195\169e !";

	-- Default filler words used in various places
	QG_UNKNOWN = "Inconnu";
	QG_NONE = "Aucun";
	QG_ACTIVE = "Active";

	-- Right click menu strings for Quest Log and Tracker
	QG_TRACK = "Suivre";
	QG_UNTRACK = "Ne pas suivre";
	QG_SHARE_QUEST = "Partager la qu\195\170te";
	QG_ABANDON_QUEST = "Abandonner la qu\195\170te";
	QG_DELETE_QUEST = "Supprimer la qu\195\170te";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Ouvrir/fermer tout";
	QG_OPTIONS = "Options";
	QG_SEARCH = "Rech. : ";

	-- History strings
	QG_HISTORY_NONE = "Pas de qu\195\170te dans l'historique";
	QG_SHOWHISTORY = "History";
	QG_COLLEVEL = "Level";
	QG_COLTITLE = "Title";
	QG_COLSTATUS = "Status";
	QG_COLSTART = "Started by";

	-- Announcer
	QG_OPT_ANNOUNCE_HEAD = "Options d'annonce";
	QG_OPT_ANNOUNCE_DESC = "Evitez les annonces publiques si vous ne voulez pas vous faire ignorer !\nLes modifications sont enregistr\195\169es lors du clic sur OK.";
	QG_OPT_ANNOUNCE_PROGRESS = "Annoncer la progression";
	QG_OPT_ANNOUNCE_CHANNEL_HEAD = "Canaux :";
	QG_OPT_ANNOUNCE_CHANNEL_SAY = "/s";
	QG_OPT_ANNOUNCE_CHANNEL_SAY_DESC = "Annoncer \195\160 ceux qui vous entourent";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY = "/p";
	QG_OPT_ANNOUNCE_CHANNEL_PARTY_DESC = "Annoncer au groupe";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD = "/g";
	QG_OPT_ANNOUNCE_CHANNEL_GUILD_DESC = "Annoncer \195\160 votre guilde";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER = "/w";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_DESC = "Annoncer \195\160 un ami";
	QG_OPT_ANNOUNCE_CHANNEL_WHISPER_TO = "A :";
	QG_OPT_ANNOUNCE_ECHO = "Discussion";
	QG_OPT_ANNOUNCE_ECHO_DESC = "Afficher votre progression sur la fen\195\169tre de discussion (conseill\195\169).";
	QG_OPT_ANNOUNCE_MESSAGES = "Messages :";
	QG_OPT_ANNOUNCE_HELP_DONE = "Nombre d'objets collect\195\169s ou de mobs tu\195\169s";
	QG_OPT_ANNOUNCE_HELP_NEEDED = "Nombre total d'objets ou de mobs n\195\169c\195\169ssaires";
	QG_OPT_ANNOUNCE_HELP_NUMLEFT = "Nombre d'objets restants";
	QG_OPT_ANNOUNCE_HELP_NAME = "Texte d'objectif (sans #/#)";
	QG_OPT_ANNOUNCE_HELP_TITLE = "Nom de la qu\195\170te pour cet objectif";
	QG_OPT_ANNOUNCE_HELP_UITEXT = "Le texte affich\195\169 sur l'\195\169cran";
	QG_OPT_ANNOUNCE_HELP_COMPLETE = "Message \""..COMPLETE.."\"";
	QG_OPT_ANNOUNCE_HELP_QLINK = "Lien de la qu\195\170te concern\195\169e";
	QG_OPT_ANNOUNCE_ITEM_MSG = "Message pour la progression d'objectifs (objets) :";
	QG_OPT_ANNOUNCE_MOB_MSG = "Message pour la progression d'objectifs (mobs) :";
	QG_OPT_ANNOUNCE_EVENT_MSG = "Message pour la progression d'objectifs (event) :";
	QG_OPT_ANNOUNCE_COMPLETE_MSG = "Message pour la fin de qu\195\170te :";
	QG_OPT_ANNOUNCE_QUESTACCEPT_MSG = "Announce when you Accept a new Quest";
	QG_OPT_ANNOUNCE_MSG_VARS = "Announce Message Variables"
  -- Announcer substitution keywords
	--QG_ANNOUNCE_ITEM_DEFAULT = "$done $name sur $needed collect\195\169(s) pour la qu\195\170te $questTitle";
	--QG_ANNOUNCE_MOB_DEFAULT = "$done $name sur $needed n\195\169c\195\169ssaires pour la qu\195\170te $questTitle";
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
	QG_ANNOUNCE_RESET = "Restaurer les param\195\169tres par d\195\169faut.";

	-- Options
	QG_OPT_OPTIONS_DESC = "Param\195\169tres g\195\169n\195\169raux de QuestGuru. Les modifications sont prises en compte imm\195\169diatement.";
	QG_OPT_LEVELS_HEAD = "Show Levels";
	QG_OPT_LEVELS_QUESTLOG = "Dans le journal de qu\195\170te";
	QG_OPT_LEVELS_HISTORY = "Dans l'historique";
	QG_OPT_LEVELS_ABANDON = "Dans le journal d'abandon";
	QG_OPT_AUTOCOMPLETE = "Terminer automatiquement les qu\195\170tes";
	QG_OPT_OBJ_ICON = "Afficher les ic\195\180nes des objets dans le journal";
	QG_OPT_HEADER_QUEST_NUM = "Montrer le nombre de qu\195\170tes dans les ent\195\170tes";
	QG_OPT_DISABLE_COMM = "Emp\195\170cher QG de communiquer avec les autres joueurs";

	QG_OPT_SOUND_HEAD = "Sons";
	QG_OPT_SOUND_DESC = "Vous pouvez modifier les sons jou\195\169s lors de la progression des qu\195\170tes.\nLes modifications sont prises en compte imm\195\169diatement.";
	QG_OPT_SOUND_ENABLE = "Activer les sons";
	QG_OPT_SOUND_OBJ = "Pour la progression d'un objectif :";
	QG_OPT_SOUND_OBJ_DONE = "Pour la fin d'un objectif :";
	QG_SOUND_QUEST_DONE = "Pour la fin d'une qu\195\170te :";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
end
