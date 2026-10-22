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

	-- Party Info Tooltip strings
	QG_ABANDONED = "Abandonn\195\169e";
	QG_INCOMPLETE = "Incompl\195\168te";
	QG_ALT_STATUS_HEAD = "------------- Vos autres personnages ----------";
	QG_GUILD_STATUS_HEAD = "-------- Les membres de votre Guilde ---------";

	-- User Interface Elements
	QG_EXPAND_HEADERS = "Ouvrir/fermer tout";
	QG_OPTIONS = "Options";
	QG_SEARCH = "Rech. : ";
	QG_CLEAR_ABANDON = "Effacer la liste";

	-- Tracker
	QG_TRACKER_SHOW = "Afficher le suivi";
	QG_TRACKER_MINIMIZE = "R\195\169duire";
	QG_TRACKER_OPTIONS = "Options";
	QG_TRACKER_TOGGLE = "Click to switch between Quest and Achievement tracking. Right-click to toggle showing both windows";
	QG_TRACKER_SORT = "Click to open a menu with quest sorting options";
	QG_TRACKER_QUESTS = "Quests";
	QG_TRACKER_ACHIEVE = "Achievements";
	QG_TRACKER_Q = "Q"; --Abbreviation for Quest for toggle button
	QG_TRACKER_A = "A"; --Abbreviation for Achievement for toggle button

	-- Options
	QG_OPT_TRACKER_DESC = "Faites-vous un superbe Tracker avec plein de couleurs !\nLes modifications sont appliqu\195\169es imm\195\169diatement.";
	QG_OPT_TRACKER_USE = "Suivre les qu\195\170tes avec QG";
	QG_OPT_TRACKER_USE_DESC = "D\195\169cocher cette option pour utiliser le Tracker original du jeu.";
	QG_OPT_TRACKER_BORDER = "Afficher la bordure";
	QG_OPT_TRACKER_BORDER_DESC = "Afficher ou non une bordure autour de la fen\195\170tre de suivi.";
	QG_OPT_TRACKER_CLICKTHROUGH = "Fixer la fen\195\170tre";
	QG_OPT_TRACKER_CLICKTHROUGH_DESC = "Cette option activ\195\169e, vous ne pouvez plus d\195\169placer la fen\195\170tre.";
	QG_OPT_TRACKER_HEADERS = "Afficher les ent\195\170tes";
	QG_OPT_TRACKER_HEADERS_DESC = "Affiche les ent\195\170tes des zones/cat\195\169gories des qu\195\170tes";
	QG_OPT_TRACKER_LEVELS = "Afficher les niveaux";
	QG_OPT_TRACKER_LEVELS_DESC = "Affiche les informations sur le niveau n\195\169c\195\169ssaire pour faire les qu\195\170tes.";
	QG_OPT_TRACKER_QUEST_TOOLTIPS = "Afficher les infobulles";
	QG_OPT_TRACKER_QUEST_TOOLTIPS_DESC = "Ouvre une infobulle avec plein d'infos sur la qu\195\170te survol\195\169e. Ne fonctionne pas si le Tracker est fix\195\169 !";
	QG_OPT_TRACKER_PARTY_TOOLTIPS = "Infobulles de groupe";
	QG_OPT_TRACKER_PARTY_TOOLTIPS_DESC = "Ouvre une infobulle avec plein d'infos concernant le groupe/la guilde sur la qu\195\170te survol\195\169e.\nNe fonctionne pas si le tracker est fix\195\169 !";
	QG_OPT_TRACKER_PERCENT = "Afficher les pourcentages";
	QG_OPT_TRACKER_PERCENT_DESC = "Affiche le pourcentage de r\195\169alisation des qu\195\170tes.";
	QG_OPT_TRACKER_ANCHOR_BOTTOM = "Etirer vers le haut";
	QG_OPT_TRACKER_ANCHOR_BOTTOM_DESC = "La fen\195\170tre de suivi grandit vers le haut (pratique pour la fixer en bas de l'\195\169cran)";
	QG_OPT_TRACKER_AUTOTRACK = "Suivi auto. des qu\195\170tes";
	QG_OPT_TRACKER_AUTOTRACK_DESC = "Ajoute automatiquement les nouvelles qu\195\170tes \195\160 la fen\195\170tre de suivi.";
	QG_OPT_TRACKER_AUTOUNTRACK = "Masquer les qu\195\170tes termin\195\169es";
	QG_OPT_TRACKER_AUTOUNTRACK_DESC = "D\195\169sactive automatiquement le suivi des qu\195\170tes termin\195\169es.";
	QG_OPT_TRACKER_COMPLETE_OBJ = "Objectifs termin\195\169s";
	QG_OPT_TRACKER_COMPLETE_OBJ_DESC = "D\195\169cocher la case pour masquer les objectifs termin\195\169s.";
	QG_OPT_TRACKER_COLOR_OBJ = "Couleurs";
	QG_OPT_TRACKER_COLOR_OBJ_DESC = "Utiliser les couleurs ci-dessous pour identifier l'\195\169tat des qu\195\170tes :";
	QG_OPT_TRACKER_OBJ_COLOR_0 = "Couleur quand l'objectif est atteint \195\160 0%";
	QG_OPT_TRACKER_OBJ_COLOR_99 = "Couleur quand l'objectif est atteint \195\160 99%";
	QG_OPT_TRACKER_OBJ_COLOR_COMPLETE = "Couleur quand l'objectif est termin\195\169";
	QG_OPT_TRACKER_SIZE = "Taille de la fen\195\170tre";
	QG_OPT_TRACKER_MAX_LINES = "Lignes maximum :";
	QG_OPT_TRACKER_ALPHA = "Transparence";
	QG_OPT_TRACKER_BULLET = "Symbole affich\195\169 devant les objectifs :";
	QG_OPT_TRACKER_HIDECOMBAT = "Hide during combat";
	QG_OPT_TRACKER_HIDECOMBAT_DESC = "Selecting this option will automatically hide the tracker(s) during combat.";
	QG_OPT_TRACKER_HIDEBLIZZTRACK = "Hide Blizzard Tracker";
	QG_OPT_TRACKER_HIDEBLIZZTRACK_DESC = "Select this option will hide the default Blizzard tracker. Uncheck if special functions of that tracker are needed.";
	QG_OPT_TRACKER_SORTQUESTNONE = "Sort by header/quest level";
	QG_OPT_TRACKER_SORTQUESTPROX = "Sort by proximity";

	-- Other strings that may or may not need localizing
	QG_ITEM_REQ_STR = "(.*):%s*([%d]+)%s*/%s*([%d]+)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors
	QG_ITEM_REQ_STR2 = "(.*):%s*%(([%d]+)%)"; -- The format of the line when an item is required for a quest, an invalid format can cause many errors (alternate format for some quests)
	QG_DATETIME = "%d/%m/%Y %H:%M:%S"; -- Format des dates utilise dans les infos des quetes
end
