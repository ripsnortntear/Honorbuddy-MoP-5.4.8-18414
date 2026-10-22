-- $Id: Atlas-deDE.lua 2112 2013-09-15 19:12:14Z Dynaletik $
--[[

	Atlas, a World of Warcraft instance map browser
	Copyright 2005 ~ 2010 - Dan Gilbert <dan.b.gilbert@gmail.com>
	Copyright 2010 - Lothaer <lothayer@gmail.com>, Atlas Team
	Copyright 2011 ~ 2013 - Arith Hsu, Atlas Team <atlas.addon@gmail.com>

	This file is part of Atlas.

	Atlas is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Atlas is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Atlas; if not, write to the Free Software
	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas", "deDE", false);
-- Localize file must set above to false

-- Deutsche Lokalisierung (German, deDE)
-- Dynaletik (ICQ: 176-289-585)
-- Nihlo (ICQ: 260-869-930)
-- Letztes Update: 15.09.2013

if ( GetLocale() == "deDE" ) then
-- Define the leading strings to be ignored while sorting
-- Ex: The Stockade
AtlasSortIgnore = {
	"der (.+)",
	"die (.+)",
	"das (.+)"
}

-- Syntax: ["real_zone_name"] = "localized map zone name"
AtlasZoneSubstitutions = {
	["Ahn'Qiraj"] = "Tempel von Ahn'Qiraj";
	["Der Tempel von Atal'Hakkar"] = "Versunkener Tempel";
--	["Throne of Tides"] = "The Abyssal Maw: Throne of the Tides";
};
end


if AL then
--************************************************
-- UI terms and common strings
--************************************************
	AL["ATLAS_TITLE"] = "Atlas";

	AL["BINDING_HEADER_ATLAS_TITLE"] = "Atlas Tastaturbelegungen";
	AL["BINDING_NAME_ATLAS_TOGGLE"] = "Atlas an/aus";
	AL["BINDING_NAME_ATLAS_OPTIONS"] = "Optionen an/aus";
	AL["BINDING_NAME_ATLAS_AUTOSEL"] = "Automatische Auswahl";

	AL["ATLAS_SLASH"] = "/atlas";
	AL["ATLAS_SLASH_OPTIONS"] = "Optionen";

	AL["ATLAS_STRING_LOCATION"] = "Region";
	AL["ATLAS_STRING_LEVELRANGE"] = "Stufe";
	AL["ATLAS_STRING_RECLEVELRANGE"] = "Empf. Stufe";
	AL["ATLAS_STRING_PLAYERLIMIT"] = "Max. Spielerzahl";
	AL["ATLAS_STRING_SELECT_CAT"] = "Kategorie wählen";
	AL["ATLAS_STRING_SELECT_MAP"] = "Karte wählen";
	AL["ATLAS_STRING_SEARCH"] = "Suchen";
	AL["ATLAS_STRING_CLEAR"] = "Leeren";
	AL["ATLAS_STRING_MINLEVEL"] = "Minimale Stufe";

	AL["ATLAS_OPTIONS_BUTTON"] = "Optionen";
	AL["ATLAS_OPTIONS_SHOWBUT"] = "Minimap-Schalter anzeigen";
	AL["ATLAS_OPTIONS_SHOWBUT_TIP"] = "Atlas Minimap-Schalter an der Minimap anzeigen.";
	AL["ATLAS_OPTIONS_AUTOSEL"] = "Automatische Karten-Auswahl";
	AL["ATLAS_OPTIONS_AUTOSEL_TIP"] = "Instanzkarte automatisch auswählen. Atlas wählt je nach aktueller Position die beste Instanzkarte aus.";
	AL["ATLAS_OPTIONS_BUTPOS"] = "Schalterposition";
	AL["ATLAS_OPTIONS_LOCK"] = "Atlasfenster fixieren";
	AL["ATLAS_OPTIONS_LOCK_TIP"] = "Atlasfenster fixieren / freigeben.";
	AL["ATLAS_OPTIONS_TRANS"] = "Transparenz";
	AL["ATLAS_OPTIONS_RCLICK"] = "Rechte Maustaste für Weltkarte drücken";
	AL["ATLAS_OPTIONS_RCLICK_TIP"] = "Aktiviert das Rechtsklicken im Atlasfenster, um die WoW Weltkarte anzuzeigen.";
	AL["ATLAS_OPTIONS_RESETPOS"] = "Position zurücksetzen";
	AL["ATLAS_OPTIONS_ACRONYMS"] = "Abkürzungen anzeigen";
	AL["ATLAS_OPTIONS_ACRONYMS_TIP"] = "Zeigt die Instanz-Abkürzungen in den Kartendetails an.";
	AL["ATLAS_OPTIONS_SCALE"] = "Skalierung des Atlas Fensters";
	AL["ATLAS_OPTIONS_BOSS_DESC"] = "Bossbeschreibungen anzeigen, wenn verfügbar";
	AL["ATLAS_OPTIONS_BOSS_DESC_TIP"] = "Beim Überfahren der Bossnummer mit dem Mauszeiger wird, wenn verfügbar, eine Bossbeschreibung angezeigt.";
	AL["ATLAS_OPTIONS_BOSS_DESC_SCALE"] = "Skalierung der Bossbeschreibungen auf der Karte";
	AL["ATLAS_OPTIONS_BUTRAD"] = "Schalterradius";
	AL["ATLAS_OPTIONS_CLAMPED"] = "Fenster im Bildschirm festhalten";
	AL["ATLAS_OPTIONS_CLAMPED_TIP"] = "Atlasfenster im Bildschirm festhalten. Deaktivieren, um das Atlasfenster über den Spielfensterrand hinaus bewegen zu können.";
	AL["ATLAS_OPTIONS_CTRL"] = "Steuerung drücken, um Tooltips anzuzeigen";
	AL["ATLAS_OPTIONS_CTRL_TIP"] = "Aktivieren, um die Kartendetails beim Drücken der Strg-Taste und Überfahren eines Eintrages anzuzeigen. Nützlich, falls der dargestellte Text länger als das Fenster groß ist.";
	AL["ATLAS_OPTIONS_DONTSHOWAGAIN"] = "Diese Information nicht erneut anzeigen.";
	AL["ATLAS_OPTIONS_CHECKMODULE"] = "Über fehlende Module / Plugins benachrichtigen.";
	AL["ATLAS_OPTIONS_CHECKMODULE_TIP"] = "Aktivieren Sie diese Option, um nach dem Starten von WoW zu prüfen, ob Module / Plugins fehlen.";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN"] = "Instanzlisten in Stufenfarben anzeigen";
	AL["ATLAS_OPTIONS_COLORINGDROPDOWN_TIP"] = "Zeigt die Instanzlisten je nach minimaler Stufe und Spielerstufe mit unterschiedlichen Farben zur Indikation des Schwierigkeitsgrades an. ";

	AL["ATLAS_BUTTON_CLOSE"] = "Schließen";
	AL["ATLAS_BUTTON_TOOLTIP_TITLE"] = "Atlas";
	AL["ATLAS_BUTTON_TOOLTIP_HINT"] = "Linke Maustaste drücken, um Atlas zu öffnen.\nMittlere Maustaste drücken, um Atlas Optionen anzuzeigen.\nRechte Maustaste gedrückt halten, um diesen Schalter zu verschieben.";
	AL["ATLAS_LDB_HINT"] = "Linke Maustaste drücken, um Atlas zu öffnen.\nRechte Maustaste drücken, um die Atlas Optionen anzuzeigen.";
	AL["ATLAS_MINIMAPLDB_HINT"] = "Linke Maustaste drücken, um Atlas zu öffnen.\nRechte Maustaste drücken, um die Atlas Optionen anzuzeigen.\nLinke Maustaste gedrückt halten, um diesen Schalter zu verschieben.";

	AL["ATLAS_OPTIONS_CATDD"] = "Sortierung der Karten nach:";
	AL["ATLAS_DDL_CONTINENT"] = "Kontinent";
	AL["ATLAS_DDL_CONTINENT_EASTERN"] = "Instanzen der Östlichen Königreiche";
	AL["ATLAS_DDL_CONTINENT_KALIMDOR"] = "Instanzen von Kalimdor";
	AL["ATLAS_DDL_CONTINENT_OUTLAND"] = "Instanzen der Scherbenwelt";
	AL["ATLAS_DDL_CONTINENT_NORTHREND"] = "Instanzen von Nordend";
	AL["ATLAS_DDL_CONTINENT_DEEPHOLM"] = "Instanzen in Tiefenheim";
	AL["ATLAS_DDL_CONTINENT_PANDARIA"] = "Instanzen in Pandaria";
	AL["ATLAS_DDL_LEVEL"] = "Stufe";
	AL["ATLAS_DDL_LEVEL_UNDER45"] = "Instanzen unter Stufe 45";
	AL["ATLAS_DDL_LEVEL_45TO60"] = "Instanzen Stufe 45-60";
	AL["ATLAS_DDL_LEVEL_60TO70"] = "Instanzen Stufe 60-70";
	AL["ATLAS_DDL_LEVEL_70TO80"] = "Instanzen Stufe 70-80";
	AL["ATLAS_DDL_LEVEL_80TO85"] = "Instanzen Stufe 80-85";
	AL["ATLAS_DDL_LEVEL_85TO90"] = "Instanzen Stufe 85-90";
	AL["ATLAS_DDL_LEVEL_90PLUS"] = "Instanzen Stufe 90+";
	AL["ATLAS_DDL_PARTYSIZE"] = "Gruppengröße";
	AL["ATLAS_DDL_PARTYSIZE_5_AE"] = "Instanzen für 5 Spieler 1/3";
	AL["ATLAS_DDL_PARTYSIZE_5_FS"] = "Instanzen für 5 Spieler 2/3";
	AL["ATLAS_DDL_PARTYSIZE_5_TZ"] = "Instanzen für 5 Spieler 3/3";
	AL["ATLAS_DDL_PARTYSIZE_10_AN"] = "Instanzen für 10 Spieler 1/2";
	AL["ATLAS_DDL_PARTYSIZE_10_OZ"] = "Instanzen für 10 Spieler 2/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40AH"] = "Instanzen für 20-40 Spieler 1/2";
	AL["ATLAS_DDL_PARTYSIZE_20TO40IZ"] = "Instanzen für 20-40 Spieler 2/2";
	AL["ATLAS_DDL_EXPANSION"] = "Erweiterung";
	AL["ATLAS_DDL_EXPANSION_OLD_AO"] = "Instanzen der alten Welt 1/2";
	AL["ATLAS_DDL_EXPANSION_OLD_PZ"] = "Instanzen der alten Welt 2/2";
	AL["ATLAS_DDL_EXPANSION_BC"] = "Burning Crusade Instanzen";
	AL["ATLAS_DDL_EXPANSION_WOTLK"] = "Wrath of the Lich King Instanzen";
	AL["ATLAS_DDL_EXPANSION_CATA"] = "Cataclysm Instanzen";
	AL["ATLAS_DDL_EXPANSION_MOP"] = "Mists of Pandaria Instanzen";
	AL["ATLAS_DDL_TYPE"] = "Typ";
	AL["ATLAS_DDL_TYPE_INSTANCE_AB"] = "Instanzen 1/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_CF"] = "Instanzen 2/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_GM"] = "Instanzen 3/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_NS"] = "Instanzen 4/5";
	AL["ATLAS_DDL_TYPE_INSTANCE_TZ"] = "Instanzen 5/5";
	AL["ATLAS_DDL_TYPE_ENTRANCE"] = "Eingänge";

	AL["ATLAS_INSTANCE_BUTTON"] = "Instanz";
	AL["ATLAS_ENTRANCE_BUTTON"] = "Eingang";
	AL["ATLAS_SEARCH_UNAVAIL"] = "Suche nicht verfügbar";

	AL["ATLAS_DEP_MSG1"] = "Atlas hat veraltete Module entdeckt.";
	AL["ATLAS_DEP_MSG2"] = "Daher wurden diese Module deaktiviert.";
	AL["ATLAS_DEP_MSG3"] = "Entfernen Sie diese aus Ihrem Verzeichnis AddOns.";
	AL["ATLAS_DEP_OK"] = "OK";

	AL["ATLAS_INFO"] = "Atlas Information";
	AL["ATLAS_INFO_12200"] = "Wichtiger Hinweis:\n\nDa die Addondatei stets größer wird, wurde ein Teil \nder Instanzkarten in getrennte Module verschoben.\n\nBeim Download des Addons von den bekannten Webseiten \nerhält man daher nun lediglich das Haupt-Addon mit den Kernfunktionen \nund den Instanzkarten von Cataclysm.\n\nWer alle alten Instanzkarten und alle Atlas Plugins benötigt, \nmuss diese seperat herunterladen.\n\nMehr Infos dazu gibt es im Forum:\nhttp://www.atlasmod.com/phpBB3/viewtopic.php?t=1522";
	AL["ATLAS_INFO_12201"] = "Bitte beachten Sie, dass ein neues Plugin |cff6666ffAtlas Scenarios|cffffffff mit den Karten der neuen Szenarien erstellt wurde. \n\nBesuchen Sie für weitere Details unsere Webseite und vergessen Sie nicht,\ndas Plugin separat zu installieren.\n|cff6666ffhttp://www.atlasmod.com/|cffffffff";

	AL["ATLAS_MISSING_MODULE"] = "Atlas hat fehlende Module / Plugins erkannt: ";

--************************************************
-- Zone Names, Acronyms, and Common Strings
--************************************************

	--Common strings
	AL["East"] = "Osten";
	AL["North"] = "Norden";
	AL["South"] = "Süden";
	AL["West"] = "Westen";

	--World Events, Festival
	AL["Brewfest"] = "Braufest";
	AL["Hallow's End"] = "Schlotternächte";
	AL["Love is in the Air"] = "Liebe liegt in der Luft";
	AL["Lunar Festival"] = "Mondfest";
	AL["Midsummer Festival"] = "Sonnenwendfest";

	--Misc strings
	AL["Colon"] = ": ";
	AL["Adult"] = "Erwachsen";
	AL["AKA"] = "AKA";
	AL["Arcane Container"] = "Arkaner Behälter";
	AL["Arms Warrior"] = "Offensiv Krieger";
	AL["Attunement Required"] = "Zugangsquest erforderlich";
	AL["Back"] = "Hinten";
	AL["Basement"] = "Keller";
	AL["Blacksmithing Plans"] = "Schmiedekunstpläne";
	AL["Chase Begins"] = "Jagd beginnt";
	AL["Chase Ends"] = "Jagd endet";
	AL["Child"] = "Kind";
	AL["Click to open Dungeon Journal window."] = "Zum Öffnen des Dungeonkompendiums klicken.";
	AL["Connection"] = "Verbindung";
	AL["Elevator"] = "Aufzug";
	AL["End"] = "Ende";
	AL["Engineer"] = "Ingenieur";
	AL["Entrance"] = "Eingang";
	AL["Event"] = "Ereignis";
	AL["Exalted"] = "Ehrfürchtig";
	AL["Exit"] = "Ausgang";
	AL["Fourth Stop"] = "Vierter Halt";
	AL["Front"] = "Vorne";
	AL["Ghost"] = "Geist";
	AL["Graveyard"] = "Friedhof";
	AL["Heroic"] = "Heroisch";
	AL["Holy Paladin"] = "Heilig Paladin";
	AL["Holy Priest"] = "Heilig Priesterin";
	AL["Hunter"] = "Jäger";
	AL["Imp"] = "Wichtel";
	AL["Key"] = "Schlüssel";
	AL["Lower"] = "Unten";
	AL["Mage"] = "Magier";
	AL["Meeting Stone"] = "Versammlungsstein";
	AL["Middle"] = "Mitte";
	AL["Monk"] = "Mönch";
	AL["Moonwell"] = "Mondbrunnen";
	AL["Optional"] = "Optional";
	AL["Orange"] = "Orange";
	AL["Outside"] = "Außerhalb";
	AL["Paladin"] = "Paladin";
	AL["Portal"] = "Portal";
	AL["Priest"] = "Priester";
	AL["Protection Warrior"] = "Defensiv Krieger";
	AL["Purple"] = "Lila";
	AL["Random"] = "Zufällig";
	AL["Rare"] = "Selten";
	AL["Reputation"] = "Ruf";
	AL["Repair"] = "Reparieren";
	AL["Retribution Paladin"] = "Vergeltungs Paladin";
	AL["Rewards"] = "Belohnungen";
	AL["Rogue"] = "Schurke";
	AL["Second Stop"] = "Zweiter Halt";
	AL["Shadow Priest"] = "Schatten Priesterin";
	AL["Shaman"] = "Schamane";
	AL["Spawn Point"] = "Spawnpunkt";
	AL["Start"] = "Anfang";
	AL["Summon"] = "Beschwörbar";
	AL["Teleporter"] = "Teleporter";
	AL["Teleporter destination"] = "Teleportziel";
	AL["Third Stop"] = "Dritter Halt";
	AL["Top"] = "Spitze";
	AL["Tunnel"] = "Tunnel";
	AL["Underwater"] = "Unter Wasser";
	AL["Upper"] = "Oben";
	AL["Varies"] = "Variiert";
	AL["Wanders"] = "Wandert";
	AL["Warlock"] = "Hexenmeister";
	AL["Warrior"] = "Krieger";
	AL["Wave 5"] = "Welle 5";
	AL["Wave 6"] = "Welle 6";
	AL["Wave 10"] = "Welle 10";
	AL["Wave 12"] = "Welle 12";
	AL["Wave 18"] = "Welle 18";
	AL["MapsNotFound"] = "Für die gewählte Instanz wurde \nkeine anzuzeigende Karte gefunden. \n\nBitte stellen Sie sicher, dass die erforderlichen \nAtlas-Kartenmodule installiert sind.";

	--Classic Acronyms
	AL["AQ"] = "AQ"; -- Ahn'Qiraj
	AL["AQ20"] = "AQ20"; -- Ruins of Ahn'Qiraj
	AL["AQ40"] = "AQ40"; -- Temple of Ahn'Qiraj
	AL["BFD"] = "BFT"; -- Blackfathom Deeps
	AL["BRD"] = "BRT"; -- Blackrock Depths
	AL["BRM"] = "BRM"; -- Blackrock Mountain
	AL["BWL"] = "BWL"; -- Blackwing Lair
	AL["DM"] = "DM"; -- Dire Maul
	AL["Gnome"] = "Gnome"; -- Gnomeregan
	AL["Halls"] = "Hallen"; -- Scarlet Halls
	AL["LBRS"] = "LBRS"; -- Lower Blackrock Spire
	AL["Mara"] = "Mara"; -- Maraudon
	AL["MC"] = "MC"; -- Molten Core
	AL["RFC"] = "RF"; -- Ragefire Chasm
	AL["RFD"] = "Hügel"; -- Razorfen Downs
	AL["RFK"] = "Kral"; -- Razorfen Kraul
	AL["Scholo"] = "Scholo"; -- Scholomance
	AL["SFK"] = "BSF"; -- Shadowfang Keep
	AL["SM"] = "Kloster"; -- Scarlet Monastery
	AL["ST"] = "Tempel"; -- Sunken Temple
	AL["Strat"] = "Strat"; -- Stratholme
	AL["Stocks"] = "Verlies"; -- The Stockade
	AL["UBRS"] = "UBRS"; -- Upper Blackrock Spire
	AL["Ulda"] = "Ulda"; -- Uldaman
	AL["VC"] = "DM"; -- The Deadmines
	AL["WC"] = "HdW"; -- Wailing Caverns
	AL["ZF"] = "ZF"; -- Zul'Farrak

	--BC Acronyms
	AL["AC"] = "Krypta"; -- Auchenai Crypts
	AL["Arca"] = "Arka"; -- The Arcatraz
	AL["Auch"] = "Auch"; -- Auchindoun
	AL["BF"] = "BK"; -- The Blood Furnace
	AL["BT"] = "BT"; -- Black Temple
	AL["Bota"] = "Bota"; -- The Botanica
	AL["CoT"] = "HdZ"; -- Caverns of Time
	AL["CoT1"] = "Durnholde, HdZ1"; -- Old Hillsbrad Foothills
	AL["CoT2"] = "Morast, HdZ2"; -- The Black Morass
	AL["CoT3"] = "Hyjal, HdZ3"; -- Hyjal Summit
	AL["CR"] = "EK"; -- Coilfang Reservoir
	AL["GL"] = "Gruul"; -- Gruul's Lair
	AL["HC"] = "HZ"; -- Hellfire Citadel
	AL["Kara"] = "Kara"; -- Karazhan
	AL["MaT"] = "TdM"; -- Magisters' Terrace
	AL["Mag"] = "Maggi"; -- Magtheridon's Lair
	AL["Mech"] = "Mecha"; -- The Mechanar
	AL["MT"] = "Gruft"; -- Mana-Tombs
	AL["Ramp"] = "BW"; -- Hellfire Ramparts
	AL["SSC"] = "SSC, HdS"; -- Serpentshrine Cavern
	AL["Seth"] = "SH"; -- Sethekk Halls
	AL["SH"] = "ZH"; -- The Shattered Halls
	AL["SL"] = "Laby"; -- Shadow Labyrinth
	AL["SP"] = "SU"; -- The Slave Pens
	AL["SuP"] = "Sunwell"; -- Sunwell Plateau
	AL["SV"] = "DK"; -- The Steamvault
	AL["TK"] = "FdS"; -- Tempest Keep
	AL["UB"] = "TS"; -- The Underbog

	--WotLK Acronyms
	AL["AK, Kahet"] = "AK, Kahet"; -- Ahn'kahet
	AL["AN, Nerub"] = "AN, Azjol"; -- Azjol-Nerub
	AL["Champ"] = "PDC"; -- Trial of the Champion
	AL["CoT-Strat"] = "HdZ4"; -- Culling of Stratholme
	AL["Crus"] = "PDK"; -- Trial of the Crusader
	AL["DTK"] = "Feste"; -- Drak'Tharon Keep
	AL["FoS"] = "Schmiede, SS";
	AL["FH1"] = "FH1"; -- The Forge of Souls
	AL["Gun"] = "Gun"; -- Gundrak
	AL["HoL"] = "HdB"; -- Halls of Lightning
	AL["HoR"] = "HdR";
	AL["FH3"] = "FH3"; -- Halls of Reflection
	AL["HoS"] = "HdS"; -- Halls of Stone
	AL["IC"] = "ICC, Zita"; -- Icecrown Citadel
	AL["Nax"] = "Naxx"; -- Naxxramas
	AL["Nex, Nexus"] = "Nex"; -- The Nexus
	AL["Ocu"] = "Ocu"; -- The Oculus
	AL["Ony"] = "Ony"; -- Onyxia's Lair
	AL["OS"] = "Obsi"; -- The Obsidian Sanctum
	AL["PoS"] = "Grube";
	AL["FH2"] = "FH2"; -- Pit of Saron
	AL["RS"] = "RS"; -- The Ruby Sanctum
	AL["TEoE"] = "Maly"; -- The Eye of Eternity
	AL["UK, Keep"] = "Burg"; -- Utgarde Keep
	AL["Uldu"] = "Uldu"; -- Ulduar
	AL["UP, Pinn"] = "Turm"; -- Utgarde Pinnacle
	AL["VH"] = "VF, Vio"; -- The Violet Hold
	AL["VoA"] = "Archa"; -- Vault of Archavon

	--Zones not included in LibBabble-Zone
	AL["Crusaders' Coliseum"] = "Kolloseum der Kreuzfahrer";

	--Cataclysm Acronyms
	AL["BH"] = "BF"; --Baradin Hold
	AL["BoT"] = "BdZ"; --Bastion of Twilight
	AL["BRC"] = "BRH"; --Blackrock Caverns
	AL["BWD"] = "BWD"; --Blackwing Descent
	AL["CoT-DS"] = "HdZ-DS"; --Caverns of Time: Dragon Soul
	AL["CoT-ET"] = "HdZ-EZ"; --Caverns of Time: End Time
	AL["CoT-HoT"] = "HdZ-SdZ"; --Caverns of Time: Hour of Twilight
	AL["CoT-WoE"] = "HdZ-BdE"; --Caverns of Time: Well of Eternity
	AL["FL"] = "FL"; --Firelands
	AL["GB"] = "GB"; --Grim Batol
	AL["HoO"] = "HdU"; --Halls of Origination
	AL["LCoT"] = "VSdT"; --Lost City of the Tol'vir
	AL["TSC"] = "DSK"; --The Stonecore
	AL["TWT"] = "TdVW"; --Throne of the Four Winds
	AL["ToTT"] = "TdG"; --Throne of the Tides
	AL["VP"] = "VG"; --The Vortex Pinnacle
	AL["ZA"] = "ZA"; -- Zul'Aman
	AL["ZG"] = "ZG"; --Zul'Gurub

	--MoP Acronyms
	AL["GSS"] = "TdUS, Tor"; --Gate of the Setting Sun
	AL["HoF"] = "HdF"; --Heart of Fear
	AL["MP"] = "MP, Palast"; --Mogu'shan Palace
	AL["MV"] = "MS, Kammer"; --Mogu'shan Vaults
	AL["SPM"] = "SPK"; --Shado-Pan Monastery
	AL["SNT"] = "BNT, Niuzao"; --Siege of Niuzao Temple
	AL["SB"] = "BS, Brauerei"; --Stormstout Brewery
	AL["SoO"] = "SuO, OG"; --Siege of Orgrimmar
	AL["TJS"] = "TdJ, Jade"; --Temple of the Jade Serpent
	AL["TES"] = "TdEF, Terrasse"; --Terrace of Endless Spring
	AL["ToT"] = "TdD"; --Throne of Thunder

	--Map sections
	AL["MapA"] = " [A]";
	AL["MapB"] = " [B]";
	AL["MapC"] = " [C]";
	AL["MapD"] = " [D]";
	AL["MapE"] = " [E]";
	AL["MapF"] = " [F]";

--************************************************
-- Instance Entrance Maps
--************************************************

	--Auchindoun (Entrance)
	AL["Clarissa"] = "Clarissa";
	AL["Greatfather Aldrimus"] = "Großvater Aldrimus";
	AL["Ha'lei"] = "Ha'lei";
	AL["Horvon the Armorer <Armorsmith>"] = "Horvon der Rüstungsschmied <Rüstungsschmied>";
	AL["Ramdor the Mad"] = "Ramdor der Wahnsinnige";
	AL["Nexus-Prince Haramad"] = "Nexusprinz Haramad";
	AL["\"Slim\" <Shady Dealer>"] = "Smudo <Zwielichtiger Händler>";
	AL["\"Captain\" Kaftiz"] = "\"Kapitän\" Kaftiz";
	AL["Dealer Tariq <Shady Dealer>"] = "Händler Tariq <Zwielichtiger Händler>";
	AL["Provisioner Tsaalt"] = "Versorger Tsaalt";

	--Blackfathom Deeps (Entrance)

	--Blackrock Mountain (Entrance)
	AL["Bodley"] = "Bodley";
	AL["Lothos Riftwaker"] = "Lothos Felsspalter";
	AL["Orb of Command"] = "Befehlskugel";
	AL["Scarshield Quartermaster <Scarshield Legion>"] = "Rüstmeister der Schmetterschilde <Schmetterschildlegion>";
	AL["The Behemoth"] = "Das Ungetüm";

	--Caverns of Time (Entrance)
	AL["Steward of Time <Keepers of Time>"] = "Ordner der Zeit <Hüter der Zeit>";
	AL["Alexston Chrome <Tavern of Time>"] = "Alexston Chrom <Taverne der Zeit>";
	AL["Yarley <Armorer>"] = "Yarley <Rüstungsschmied>";
	AL["Bortega <Reagents & Poison Supplies>"] = "Bortega <Reagenzien & Gifte>";
	AL["Alurmi <Keepers of Time Quartermaster>"] = "Alurmi <Rüstmeisterin der Hüter der Zeit>";
	AL["Galgrom <Provisioner>"] = "Galgrom <Versorger>";
	AL["Zaladormu"] = "Zaladormu";
	AL["Soridormi <The Scale of Sands>"] = "Soridormi <Die Wächter der Sande>";
	AL["Arazmodu <The Scale of Sands>"] = "Arazmodu <Die Wächter der Sande>";
	AL["Andormu <Keepers of Time>"] = "Andormu <Hüter der Zeit>";
	AL["Nozari <Keepers of Time>"] = "Nozari <Hüter der Zeit>";
	AL["Anachronos <Keepers of Time>"] = "Anachronos <Hüter der Zeit>";

	--Caverns of Time: Hyjal (Entrance)
	AL["Indormi <Keeper of Ancient Gem Lore>"] = "Indormi <Bewahrerin der alten Edelsteinkunde>";
	AL["Tydormu <Keeper of Lost Artifacts>"] = "Tydormu <Bewahrer der verlorenen Artefakte>";

	--Coilfang Reservoir (Entrance)
	AL["Mortog Steamhead"] = "Mortog Dampfkopf";

	--Dire Maul (Entrance)
	AL["Dire Pool"] = "Düsterteich";
	AL["Dire Maul Arena"] = "Düsterbruch Arena";
	AL["Elder Mistwalker"] = "Urahnin Nebelgänger";

	--Gnomeregan (Entrance)
	AL["Torben Zapblast <Teleportation Specialist>"] = "Torben Zischknall <Teleportationsspezialist>";

	--Hellfire Citadel (Entrance)
	AL["Steps and path to the Blood Furnace"] = "Stufen und Pfad zum Blutkessel";
	AL["Path to the Hellfire Ramparts and Shattered Halls"] = "Pfad zum Höllenfeuerbollwerk und den zerschmetterten Hallen";
	AL["Meeting Stone of Magtheridon's Lair"] = "Versammlungsstein für Magtheridons Kammer";
	AL["Meeting Stone of Hellfire Citadel"] = "Versammlungsstein der Höllenfeuerzitadelle";

	--Icecrown Citadel (Entrance)

	--Karazhan (Entrance)
	AL["Archmage Leryda"] = "Erzmagierin Leryda";
	AL["Archmage Alturus"] = "Erzmagier Alturus";
	AL["Apprentice Darius"] = "Lehrling Darius";
	AL["Stairs to Underground Pond"] = "Treppe zum Unterirdischen Teich";
	AL["Stairs to Underground Well"] = "Treppe zum Unterirdischen Brunnen";
	AL["Charred Bone Fragment"] = "Verkohltes Knochenfragment";

	--Maraudon (Entrance)
	AL["The Nameless Prophet"] = "Der namenlose Prophet";
	AL["Cursed Centaur"] = "Verfluchter Zentaur";
	AL["Kherrah"] = "Kherrah";

	--Scarlet Monastery (Entrance)

	--The Deadmines (Entrance)

	--Sunken Temple (Entrance)
	AL["Priestess Udum'bra"] = "Priesterin Udum'bra";
	AL["Gomora the Bloodletter"] = "Gomora der Blutvergießer";
	AL["Captain Wyrmak"] = "Hauptmann Wyrmak";

	--Uldaman (Entrance)

	--Ulduar (Entrance)
	AL["Shavalius the Fancy <Flight Master>"] = "Shavalius der Adrette <Flugmeister>";
	AL["Chester Copperpot <General & Trade Supplies>"] = "Chester Kupferkessel <Gemischt- & Handwerkswaren>";
	AL["Slosh <Food & Drink>"] = "Slosh <Speis & Trank>";

	--Wailing Caverns (Entrance)

--************************************************
-- Kalimdor Instances (Classic)
--************************************************

	--Blackfathom Deeps
	AL["Shrine of Gelihast"] = "Schrein von Gelihast";
	AL["Fathom Stone"] = "Tiefenstein";
	AL["Lorgalis Manuscript"] = "Manuskript von Lorgalis";
	AL["Scout Thaelrid"] = "Späher Thaelrid";
	AL["Flaming Eradicator"] = "Flammender Vernichter";
	AL["Altar of the Deeps"] = "Altar der Tiefen";
	AL["Ashelan Northwood"] = "Ashelan Nordwald";
	AL["Relwyn Shadestar"] = "Relwyn Schattenstern";
	AL["Sentinel Aluwyn"] = "Schildwache Aluwyn";
	AL["Sentinel-trainee Issara"] = "Schildwachenschülerin Issara";
	AL["Je'neu Sancrea <The Earthen Ring>"] = "Je'neu Sancrea <Der Irdene Ring>";
	AL["Zeya"] = "Zeya";

	--Dire Maul (East)
	AL["\"Ambassador\" Dagg'thol"] = "\"Botschafter\" Dagg'thol";
	AL["Furgus Warpwood"] = "Furgus Wucherborke";
	AL["Old Ironbark"] = "Eisenborke der Große";
	AL["Ironbark the Redeemed"] = "Eisenborke der Erlöste";

	--Dire Maul (North)
	AL["Druid of the Talon"] = "Druide der Kralle";
	AL["Stonemaul Ogre"] = "Oger der Steinbrecher";
	AL["Knot Thimblejack"] = "Knot Zwingschraub";

	--Dire Maul (West)
	AL["Ferra"] = "Ferra";
	AL["Estulan <The Highborne>"] = "Estulan <Die Hochgeborenen>";
	AL["Shen'dralar Watcher"] = "Behüter der Shen'dralar";
	AL["Pylons"] = "Pylonen";
	AL["Ancient Equine Spirit"] = "Uralter Pferdegeist";
	AL["Shen'dralar Ancient"] = "Uralte Shen'dralar";
	AL["Falrin Treeshaper"] = "Falrin Rankenweber";
	AL["Lorekeeper Lydros"] = "Wissenshüter Lydros";
	AL["Lorekeeper Javon"] = "Wissenshüter Javon";
	AL["Lorekeeper Kildrath"] = "Wissenshüter Kildrath";
	AL["Lorekeeper Mykos"] = "Wissenshüter Mykos";
	AL["Shen'dralar Provisioner"] = "Versorger der Shen'dralar";

	--Maraudon	
	AL["Elder Splitrock"] = "Urahne Splitterfels";
	AL["Celebras the Redeemed"] = "Celebras der Erlöste";

	--Ragefire Chasm
	AL["Commander Bagran"] = "Kommandant Bagran";
	AL["Invoker Xorenth"] = "Herbeirufer Xorenth";
	AL["Scout Cage"] = "Späherkäfig";

	--Razorfen Downs
	AL["Koristrasza"] = "Koristrasza";
	AL["Belnistrasz"] = "Belnistrasz";

	--Razorfen Kraul
	AL["Auld Stonespire"] = "Auld Steinkeil";
	AL["Spirit of Agamaggan <Ancient>"] = "Geist von Agamaggan <Uralter>";
	AL["Willix the Importer"] = "Willix der Importeur";

	--Ruins of Ahn'Qiraj
	AL["Four Kaldorei Elites"] = "Vier Elitesoldaten der Kaldorei";
	AL["Captain Qeez"] = "Hauptmann Qeez";
	AL["Captain Tuubid"] = "Hauptmann Tuubid";
	AL["Captain Drenn"] = "Hauptmann Drenn";
	AL["Captain Xurrem"] = "Hauptmann Xurrem";
	AL["Major Yeggeth"] = "Major Yeggeth";
	AL["Major Pakkon"] = "Major Pakkon";
	AL["Colonel Zerran"] = "Oberst Zerran";
	AL["Safe Room"] = "Sicherer Raum";

	--Temple of Ahn'Qiraj
	AL["Andorgos <Brood of Malygos>"] = "Andorgos <Brut Malygos'>";
	AL["Vethsera <Brood of Ysera>"] = "Vethsera <Brut Yseras>";
	AL["Kandrostrasz <Brood of Alexstrasza>"] = "Kandrostrasz <Brut Alexstraszas>";
	AL["Arygos"] = "Arygos";
	AL["Caelestrasz"] = "Caelestrasz";
	AL["Merithra of the Dream"] = "Merithra des Traums";

	--Wailing Caverns
	AL["Ebru <Disciple of Naralex>"] = "Ebru <Jüngerin von Naralex>";
	AL["Nalpak <Disciple of Naralex>"] = "Nalpak <Jünger von Naralex>";
	AL["Muyoh <Disciple of Naralex>"] = "Muyoh <Jünger von Naralex>";
	AL["Naralex"] = "Naralex";

	--Zul'Farrak
	AL["Chief Engineer Bilgewhizzle <Gadgetzan Water Co.>"] = "Chefingenieur Bilgenritzel <Gadgetzan Water Co.>";
	AL["Mazoga's Spirit"] = "Mazogas Geist";
	AL["Tran'rek"] = "Tran'rek";
	AL["Weegli Blastfuse"] = "Weegli Lunte";
	AL["Raven"] = "Die Krähe";
	AL["Elder Wildmane"] = "Urahnin Wildmähne";

--****************************
-- Eastern Kingdoms Instances (Classic)
--****************************

	--Blackrock Depths
	AL["The Black Anvil"] = "Der Schwarze Amboss";
	AL["The Vault"] = "Der Tresorraum";
	AL["Watchman Doomgrip"] = "Wachmann Stahlgriff";
	AL["Elder Morndeep"] = "Urahne Schwermut";
	AL["Schematic: Field Repair Bot 74A"] = "Bauplan: Feldreparaturbot 74A";
	AL["Private Rocknot"] = "Gefreiter Rocknot";
	AL["Mistress Nagmara"] = "Herrin Nagmara";
	AL["Jalinda Sprig <Morgan's Militia>"] = "Jalinda Sprig <Morgans Miliz>";
	AL["Oralius <Morgan's Militia>"] = "Oralius <Morgans Miliz>";
	AL["Thal'trak Proudtusk <Kargath Expeditionary Force>"] = "Thal'trak Ehrenhauer <Expeditionskorps von Kargath>";
	AL["Galamav the Marksman <Kargath Expeditionary Force>"] = "Galamav der Schütze <Expeditionskorps von Kargath>";
	AL["Maxwort Uberglint"] = "Maxwort Funkelglanz";
	AL["Tinkee Steamboil"] = "Tinkee Kesseldampf";
	AL["Yuka Screwspigot <Engineering Supplies>"] = "Yuka Schraubstutz <Ingenieursbedarf>";
	AL["Abandonded Mole Machine"] = "Verlassene Maulwurfmaschine";
	AL["Kevin Dawson <Morgan's Militia>"] = "Kevin Dawson <Morgans Miliz>";
	AL["Lexlort <Kargath Expeditionary Force>"] = "Lexlort <Expeditionskorps von Kargath>";
	AL["Prospector Seymour <Morgan's Militia>"] = "Ausgrabungsleiter Seymour <Morgans Miliz>";
	AL["Razal'blade <Kargath Expeditionary Force>"] = "Razal'hieb <Expeditionskorps von Kargath>";
	AL["The Shadowforge Lock"] = "Das Schloss der Schattenschmiede";
	AL["Mayara Brightwing <Morgan's Militia>"] = "Mayara Wolkenglanz <Morgans Miliz>";
	AL["Hierophant Theodora Mulvadania <Kargath Expeditionary Force>"] = "Hierophantin Theodora Mulvadania <Expeditionskorps von Kargath>";
	AL["Lokhtos Darkbargainer <The Thorium Brotherhood>"] = "Lokhtos Düsterfeilsch <Die Thoriumbruderschaft>";
	AL["Mountaineer Orfus <Morgan's Militia>"] = "Gebirgsjäger Orfus <Morgans Miliz>";
	AL["Thunderheart <Kargath Expeditionary Force>"] = "Donnerherz <Expeditionskorps von Kargath>";
	AL["Marshal Maxwell <Morgan's Militia>"] = "Marschall Maxwell <Morgans Miliz>";
	AL["Warlord Goretooth <Kargath Expeditionary Force>"] = "Kriegsherr Bluthauer <Expeditionskorps von Kargath>";
	AL["The Black Forge"] = "Die schwarze Schmiede";
	AL["Core Fragment"] = "Kernfragment";
	AL["Shadowforge Brazier"] = "Schattenschmiedekohlenpfanne";

	--Blackrock Spire (Lower)
	AL["Urok's Tribute Pile"] = "Uroks Tributhaufen";
	AL["Acride <Scarshield Legion>"] = "Acride <Schmetterschildlegion>";
	AL["Elder Stonefort"] = "Urahne Steinwehr";
	AL["Roughshod Pike"] = "Beschlagene Pike";

	--Blackrock Spire (Upper)
	AL["Finkle Einhorn"] = "Finkle Einhorn";
	AL["Drakkisath's Brand"] = "Drakkisaths Brandzeichen";
	AL["Father Flame"] = "Vater Flamme";

	--Blackwing Lair
	AL["Orb of Domination"] = "Kugel der Herrschaft";
	AL["Master Elemental Shaper Krixix"] = "Meisterelementarformer Krixix";

	--Gnomeregan
	AL["Chomper"] = "Mümmler";
	AL["Blastmaster Emi Shortfuse"] = "Sprengmeisterin Emi Schnellzünd";
	AL["Murd Doc <S.A.F.E.>"] = "Murd Doc <S.I.C.H.E.R.>";
	AL["Tink Sprocketwhistle <Engineering Supplies>"] = "Tink Sprosspfiff <Ingenieursbedarf>";
	AL["The Sparklematic 5200"] = "Der Funkelmat 5200";
	AL["Mail Box"] = "Briefkasten";
	AL["B.E Barechus <S.A.F.E.>"] = "Bi'ay Bäräkuss <S.I.C.H.E.R.>";
	AL["Face <S.A.F.E.>"] = "Fähs <S.I.C.H.E.R.>";
	AL["Hann Ibal <S.A.F.E.>"] = "Hann Ibal <S.I.C.H.E.R.>";

	--Molten Core

	--Scarlet Halls
	AL["Commander Lindon"] = "Kommandant Lindon";
	AL["Hooded Crusader"] = "Vermummte Kreuzfahrerin";
	AL["Bucket of Meaty Dog Food"] = "Eimer mit fleischigem Hundefutter";
	AL["Reinforced Archery Target"] = "Verstärkte Zielscheibe";

	--Scarlet Monastery

	--Scholomance
	AL["Instructor Chillheart's Phylactery"] = "Ausbilderin Kaltherz' Phylakterium";
	AL["Professor Slate"] = "Professor Schiefer";
	AL["Polyformic Acid Potion"] = "Polyformgift";
	AL["Talking Skull"] = "Sprechender Schädel";
	AL["In the Shadow of the Light"] = "Im Schatten des Lichts";
	AL["Kel'Thuzad's Deep Knowledge"] = "Kel'Thuzads tiefgründiges Wissen";
	AL["Forbidden Rites and other Rituals Necromantic"] = "Verbotene Riten und andere nekromantische Rituale";
	AL["Coffer of Forgotten Souls"] = "Truhe der vergessenen Seelen";
	AL["The Dark Grimoire"] = "Der dunkle Zauberfoliant";

	--Shadowfang Keep
	AL["Apothecary Trio"] = "Apotheker-Trio";
	AL["Apothecary Hummel <Crown Chemical Co.>"] = "Apotheker Hummel <Chemiemanufaktur Krone>";
	AL["Apothecary Baxter <Crown Chemical Co.>"] = "Apotheker Baxter <Chemiemanufaktur Krone>";
	AL["Apothecary Frye <Crown Chemical Co.>"] = "Apotheker Frye <Chemiemanufaktur Krone>";
	AL["Packleader Ivar Bloodfang"] = "Rudelführer Ivar Blutfang";
	AL["Deathstalker Commander Belmont"] = "Todespirscherkommandant Belmont";
	AL["Haunted Stable Hand"] = "Geisterhafter Stallknecht";
	AL["Investigator Fezzen Brasstacks"] = "Ermittler Fezzen Kupferstapel";

	--Stratholme - Crusader's Square
	AL["Crusade Commander Eligor Dawnbringer <Brotherhood of the Light>"] = "Kreuzzugskommandant Eligor Morgenbringer <Bruderschaft des Lichts>";
	AL["Master Craftsman Wilhelm <Brotherhood of the Light>"] = "Meisterhandwerker Wilhelm <Bruderschaft des Lichts>";
	AL["Packmaster Stonebruiser <Brotherhood of the Light>"] = "Rottenkommandant Steinberster <Bruderschaft des Lichts>";
	AL["Stratholme Courier"] = "Kurier von Stratholme";
	AL["Fras Siabi's Postbox"] = "Fras Siabis Briefkasten";
	AL["King's Square Postbox"] = "Briefkasten am Königsplatz";
	AL["Festival Lane Postbox"] = "Briefkasten in der Feststraße";
	AL["Elder Farwhisper"] = "Urahne Fernwisper";
	AL["Market Row Postbox"] = "Briefkasten in der Marktgasse";

	--Stratholme - The Gauntlet
	AL["Elders' Square Postbox"] = "Briefkasten am Ältestenplatz";
	AL["Archmage Angela Dosantos <Brotherhood of the Light>"] = "Erzmagierin Angela Dosantos <Bruderschaft des Lichts>";
	AL["Crusade Commander Korfax <Brotherhood of the Light>"] = "Kreuzzugskommandant Korfax <Bruderschaft des Lichts>";

	--The Deadmines
	AL["Lieutenant Horatio Laine"] = "Leutnant Horatio Laine";
	AL["Kagtha"] = "Kagtha";
	AL["Slinky Sharpshiv"] = "Slinky Scharfklinge";
	AL["Quartermaster Lewis <Quartermaster>"] = "Rüstmeister Lewis <Rüstmeister>";
	AL["Miss Mayhem"] = "Fräulein Fiasko";
	AL["Vend-O-Tron D-Luxe"] = "Kauf-o-Mat 1-A";

	--The Stockade
	AL["Rifle Commander Coe"] = "Schützenkommandant Coe";
	AL["Warden Thelwater"] = "Aufseher Thelwasser";
	AL["Nurse Lillian"] = "Schwester Lillian";

	--The Sunken Temple
	AL["Lord Itharius"] = "Lord Itharius";
	AL["Elder Starsong"] = "Urahnin Sternensang";

	--Uldaman
	AL["Baelog's Chest"] = "Baelogs Truhe";
	AL["Kand Sandseeker <Explorer's League>"] = "Kand Sandsucher <Forscherliga>";
	AL["Lead Prospector Durdin <Explorer's League>"] = "Oberausgrabungsleiter Durdin <Forscherliga>";
	AL["Olga Runesworn <Explorer's League>"] = "Olga Runenschwur <Forscherliga>";
	AL["Aoren Sunglow <The Reliquary>"] = "Aoren Sonnenglanz <Die Archäologische Akademie>";
	AL["High Examiner Tae'thelan Bloodwatcher <The Reliquary>"] = "Oberster Prüfer Tae'thelan Blutwächter <Die Archäologische Akademie>";
	AL["Lidia Sunglow <The Reliquary>"] = "Lidia Sonnenglanz <Die Archäologische Akademie>";
	AL["Ancient Treasure"] = "Antiker Schatz";
	AL["The Discs of Norgannon"] = "Die Scheiben von Norgannon";

--*******************
-- Burning Crusade Instances
--*******************

	--Auch: Auchenai Crypts
	AL["Draenei Spirit"] = "Draeneigeist";
	AL["Avatar of the Martyred"] = "Avatar des Gemarterten";
	AL["D'ore"] = "D'ore";
	AL["Tormented Soulpriest"] = "Gequälte Seelenpriesterin";

	--Auch: Mana-Tombs
	AL["Artificer Morphalius"] = "Konstrukteur Morphalius";
	AL["Mamdy the \"Ologist\""] = "Mamdy der \"Ologe\"";
	AL["Shadow Lord Xiraxis"] = "Schattenlord Xiraxis";
	AL["Ambassador Pax'ivi"] = "Botschafter Pax'ivi";
	AL["Cryo-Engineer Sha'heen"] = "Kryoingenieur Sha'heen";
	AL["Ethereal Transporter Control Panel"] = "Bedienungskonsole des Astraltransporters";

	--Auch: Sethekk Halls
	AL["Isfar"] = "Isfar";
	AL["Dealer Vijaad"] = "Händler Vijaad";
	AL["Lakka"] = "Lakka";
	AL["The Saga of Terokk"] = "Die Sage von Terokk";

	--Auch: Shadow Labyrinth
	AL["Field Commander Mahfuun"] = "Feldkommandeur Mahfuun";
	AL["Spy Grik'tha"] = "Spionin Grik'tha";
	AL["The Codex of Blood"] = "Kodex des Blutes";
	AL["First Fragment Guardian"] = "Wächter des ersten Teils";
	AL["Spy To'gun"] = "Spion To'gun";

	--Black Temple (Start)
	AL["Towards Reliquary of Souls"] = "Zum Relikt der Seelen";
	AL["Towards Teron Gorefiend"] = "Zu Teron Blutschatten";
	AL["Towards Illidan Stormrage"] = "Zu Illidan Sturmgrimm";
	AL["Spirit of Olum"] = "Geist von Olum";
	AL["Spirit of Udalo"] = "Geist von Udalo";
	AL["Aluyen <Reagents>"] = "Aluyen <Reagenzien>";
	AL["Okuno <Ashtongue Deathsworn Quartermaster>"] = "Okuno <Rüstmeister der Todeshörigen>";
	AL["Seer Kanai"] = "Seher Kanai";

	--Black Temple (Basement)

	--Black Temple (Top)

	--CFR: Serpentshrine Cavern
	AL["Seer Olum"] = "Seher Olum";

	--CFR: The Slave Pens
	AL["Nahuud"] = "Nahuud";
	AL["Watcher Jhang"] = "Behüterin Jhang";
	AL["Weeder Greenthumb"] = "Jäter Gründaum";
	AL["Skar'this the Heretic"] = "Nar'biss der Ketzer";
	AL["Naturalist Bite"] = "Naturalist Biss";

	--CFR: The Steamvault
	AL["Windcaller Claw"] = "Windrufer Klaue";
	AL["Main Chambers Access Panel"] = "Zugangskonsole der Hauptkammer";
	AL["Second Fragment Guardian"] = "Wächter des zweiten Teils";

	--CFR: The Underbog
	AL["T'shu"] = "T'shu";
	AL["The Underspore"] = "Die Tiefenspore";
	AL["Earthbinder Rayge"] = "Erdbinder Rayge";

	--CoT: The Black Morass
	AL["Sa'at <Keepers of Time>"] = "Sa'at <Hüter der Zeit>";

	--CoT: Hyjal Summit
	AL["Lady Jaina Proudmoore"] = "Lady Jaina Prachtmeer";
	AL["Thrall <Warchief>"] = "Thrall <Kriegshäuptling>";
	AL["Tyrande Whisperwind <High Priestess of Elune>"] = "Tyrande Wisperwind <Hohepriesterin von Elune>";

	--CoT: Old Hillsbrad Foothills
	AL["Erozion"] = "Erozion";
	AL["Brazen"] = "Brazen";
	AL["Landing Spot"] = "Landepunkt";
	AL["Thrall"] = "Thrall";
	AL["Taretha"] = "Taretha";
	AL["Don Carlos"] = "Don Carlos";
	AL["Guerrero"] = "Guerrero";
	AL["Thomas Yance <Travelling Salesman>"] = "Thomas Yance <Fliegender Händler>";
	AL["Aged Dalaran Wizard"] = "Gealterter Hexer von Dalaran";
	AL["Jonathan Revah"] = "Jonathan Revah";
	AL["Jerry Carter"] = "Jerry Carter";
	AL["Helcular"] = "Helcular";
	AL["Farmer Kent"] = "Bauer Kent";
	AL["Sally Whitemane"] = "Sally Weißsträhne";
	AL["Renault Mograine"] = "Renault Mograine";
	AL["Little Jimmy Vishas"] = "Kleiner Jimmy Vishas";
	AL["Herod the Bully"] = "Herod der Tyrann";
	AL["Nat Pagle"] = "Nat Pagle";
	AL["Hal McAllister"] = "Hal McAllister";
	AL["Zixil <Aspiring Merchant>"] = "Zixil <Aufstrebender Händler>";
	AL["Overwatch Mark 0 <Protector>"] = "Überwacher V.0 <Beschützer>";
	AL["Southshore Inn"] = "Süderstade Gasthaus";
	AL["Captain Edward Hanes"] = "Kapitän Edward Hanes";
	AL["Captain Sanders"] = "Kapitän Sanders";
	AL["Commander Mograine"] = "Kommandant Mograine";
	AL["Isillien"] = "Isillien";
	AL["Abbendis"] = "Abbendis";
	AL["Fairbanks"] = "Schönufer";
	AL["Taelan"] = "Taelan";
	AL["Barkeep Kelly <Bartender>"] = "Barkeeper Kelly <Schankkellner>";
	AL["Frances Lin <Barmaid>"] = "Frances Lin <Bardame>";
	AL["Chef Jessen <Speciality Meat & Slop>"] = "Küchenchef Jessen <Spezialitätenfleisch & Pampe>";
	AL["Stalvan Mistmantle"] = "Stalvan Dunstmantel";
	AL["Phin Odelic <The Kirin Tor>"] = "Phin Odelic <Kirin Tor>";
	AL["Magistrate Henry Maleb"] = "Magistrat Henry Maleb";
	AL["Raleigh the True"] = "Raleigh der Getreue";
	AL["Nathanos Marris"] = "Nathanos Marris";
	AL["Bilger the Straight-laced"] = "Bilger der Strenge";
	AL["Innkeeper Monica"] = "Gastwirtin Monica";
	AL["Julie Honeywell"] = "Julie Honigbrunn";
	AL["Jay Lemieux"] = "Jay Lemieux";
	AL["Young Blanchy"] = "Kleine Graumähne";

	--Gruul's Lair

	--HFC: The Blood Furnace
	AL["Gunny"] = "Gunny";
	AL["Caza'rez"] = "Caza'rez";

	--HFC: Hellfire Ramparts
	AL["Advance Scout Chadwick"] = "Vorhutsspäher Chadwick";
	AL["Stone Guard Stok'ton"] = "Steingardist Stok'ton";
	AL["Reinforced Fel Iron Chest"] = "Verstärkte Teufelseisentruhe";

	--HFC: Magtheridon's Lair

	--HFC: The Shattered Halls
	AL["Shattered Hand Executioner"] = "Henker der Zerschmetterten Hand";
	AL["Private Jacint"] = "Gefreiter Jacint";
	AL["Rifleman Brownbeard"] = "Scharfschütze Braunbart";
	AL["Captain Alina"] = "Hauptmann Alina";
	AL["Scout Orgarr"] = "Späher Orgarr";
	AL["Korag Proudmane"] = "Korag Mähnenstolz";
	AL["Captain Boneshatter"] = "Hauptmann Knochenbrecher";
	AL["Randy Whizzlesprocket"] = "Randy Sauseritzel";
	AL["Drisella"] = "Drisella";

	--Karazhan Start
	AL["Baroness Dorothea Millstipe"] = "Baroness Dorothea Mühlenstein";
	AL["Lady Catriona Von'Indi"] = "Lady Catriona Von'Indi";
	AL["Lady Keira Berrybuck"] = "Lady Keira Beerhas";
	AL["Baron Rafe Dreuger"] = "Baron Rafe Dreuger";
	AL["Lord Robin Daris"] = "Lord Robin Daris";
	AL["Lord Crispin Ference"] = "Lord Crispin Ference";
	AL["Red Riding Hood"] = "Rotkäppchen";
	AL["Wizard of Oz"] = "Zauberer von Oz";
	AL["The Master's Terrace"] = "Die Terrasse des Meisters";
	AL["Servant Quarters"] = "Quartier der Diener";
	AL["Hastings <The Caretaker>"] = "Hastings <Der Hauswart>";
	AL["Berthold <The Doorman>"] = "Berthold <Der Pförtner>";
	AL["Calliard <The Nightman>"] = "Calliard <Der Nachtwächter>";
	AL["Koren <The Blacksmith>"] = "Koren <Der Schmied>";
	AL["Bennett <The Sergeant at Arms>"] = "Bennett <Die Schutzwache>";
	AL["Keanna's Log"] = "Keannas Aufzeichnungen";
	AL["Ebonlocke <The Noble>"] = "Schwarzhaupt <Der Adlige>";
	AL["Sebastian <The Organist>"] = "Sebastian <Der Orgelspieler>";
	AL["Barnes <The Stage Manager>"] = "Barnes <Der Inspizient>";

	--Karazhan End
	AL["Path to the Broken Stairs"] = "Weg zur Beschädigten Treppe";
	AL["Broken Stairs"] = "Beschädigte Treppe";
	AL["Ramp to Guardian's Library"] = "Rampe zur Bibliothek der Beschützer";
	AL["Mysterious Bookshelf"] = "Verdächtiges Bücherregal";
	AL["Ramp up to the Celestial Watch"] = "Rampe nach oben zur Himmelswacht";
	AL["Ramp down to the Gamesman's Hall"] = "Rampe nach unten zur Halle der Spieler";
	AL["Ramp to Medivh's Chamber"] = "Rampe zu Medivhs Kammer";
	AL["Spiral Stairs to Netherspace"] = "Wendeltreppe zum Netherraum";
	AL["Wravien <The Mage>"] = "Wravien <Der Magier>";
	AL["Gradav <The Warlock>"] = "Gradav <Der Hexenmeister>";
	AL["Kamsis <The Conjurer>"] = "Kamsis <Die Beschwörerin>";
	AL["Ythyar"] = "Ythyar";
	AL["Echo of Medivh"] = "Echo Medivhs";

	--Magisters Terrace
	AL["Exarch Larethor"] = "Exarch Larethor";
	AL["Fel Crystals"] = "Teufelskristalle";
	AL["Apoko"] = "Apoko";
	AL["Eramas Brightblaze"] = "Eramas Leuchtfeuer";
	AL["Ellrys Duskhallow"] = "Ellrys Dämmerweih";
	AL["Fizzle"] = "Zischel";
	AL["Garaxxas"] = "Garaxxas";
	AL["Sliver <Garaxxas' Pet>"] = "Splitter <Garaxxas Tier>";
	AL["Kagani Nightstrike"] = "Kagani Nachtschlag";
	AL["Warlord Salaris"] = "Kriegsherr Salaris";
	AL["Yazzai"] = "Yazzai";
	AL["Zelfan"] = "Zelfan";
	AL["Tyrith"] = "Tyrith";
	AL["Scrying Orb"] = "Seherkugel";

	--Sunwell Plateau
	AL["Madrigosa"] = "Madrigosa";

	--TK: The Arcatraz
	AL["Millhouse Manastorm"] = "Millhaus Manasturm";
	AL["Third Fragment Guardian"] = "Wächter des dritten Teils";
	AL["Udalo"] = "Udalo";

	--TK: The Botanica

	--TK: The Mechanar
	AL["Overcharged Manacell"] = "Überladene Manazelle";

	--TK: The Eye

--*****************
-- WotLK Instances
--*****************

	--Azjol-Nerub: Ahn'kahet: The Old Kingdom
	AL["Seer Ixit"] = "Seher Ixit";
	AL["Ahn'kahet Brazier"] = "Kohlenbecken von Ahn'kahet";

	--Azjol-Nerub: Azjol-Nerub
	AL["Reclaimer A'zak"] = "Pionier A'zak";
	AL["Watcher Gashra"] = "Aufseher Gashra";
	AL["Watcher Narjil"] = "Aufseher Narjil";
	AL["Watcher Silthik"] = "Aufseher Silthik";
	AL["Elder Nurgen"] = "Urahne Nurgen";

	--Caverns of Time: The Culling of Stratholme
	AL["The Culling of Stratholme"] = "Das Ausmerzen von Stratholme";
	AL["Scourge Invasion Points"] = "Invasionspunkte der Geißel";
	AL["Guardian of Time"] = "Wächter der Zeit";
	AL["Chromie"] = "Chromie";

	--Drak'Tharon Keep
	AL["Image of Drakuru"] = "Abbild von Drakuru";
	AL["Kurzel"] = "Kurzel";
	AL["Elder Kilias"] = "Urahne Kilias";
	AL["Drakuru's Brazier"] = "Drakuru's Kohlenpfanne";

	--The Frozen Halls: Halls of Reflection
	--3 beginning NPCs omitted, see The Forge of Souls
	AL["The Captain's Chest"] = "Die Truhe des Hauptmanns";

	--The Frozen Halls: Pit of Saron
	--6 beginning NPCs omitted, see The Forge of Souls
	AL["Martin Victus"] = "Martin Victus";
	AL["Gorkun Ironskull"] = "Gorkun Eisenschädel";
	AL["Rimefang"] = "Raufang";

	--The Frozen Halls: The Forge of Souls
	--Lady Jaina Proudmoore omitted, in Hyjal Summit
	AL["Archmage Koreln <Kirin Tor>"] = "Erzmagier Koreln <Kirin Tor>";
	AL["Archmage Elandra <Kirin Tor>"] = "Erzmagierin Elandra <Kirin Tor>";
	AL["Lady Sylvanas Windrunner <Banshee Queen>"] = "Fürstin Sylvanas Windläufer <Bansheekönigin>";
	AL["Dark Ranger Loralen"] = "Dunkelläuferin Loralen";
	AL["Dark Ranger Kalira"] = "Dunkelläuferin Kalira";

	--Gundrak
	AL["Chronicler Bah'Kini"] = "Chronistin Bah'Kini";
	AL["Tol'mar"] = "Tol'mar";
	AL["Elder Ohanzee"] = "Urahne Ohanzee";

	--Icecrown Citadel
	AL["To next map"] = "Zur nächsten Karte";
	AL["From previous map"] = "Von vorheriger Karte";
	AL["Upper Spire"] = "Obere Spitze";
	AL["Sindragosa's Lair"] = "Sindragosas Hort";
	AL["Stinky"] = "Stinki";
	AL["Precious"] = "Schatz";
	AL["Rimefang"] = "Raufang";
	AL["Spinestalker"] = "Wirbelpirscher";
	AL["Sister Svalna"] = "Schwester Svalna";

	--Naxxramas
	AL["Mr. Bigglesworth"] = "Mr. Bigglesworth";
	AL["Frostwyrm Lair"] = "Frostwyrmhöhle";
	AL["Teleporter to Middle"] = "Teleporter zur Mitte";

	--The Obsidian Sanctum
	AL["Black Dragonflight Chamber"] = "Kammer des schwarzen Drachenschwarms";

	--Onyxia's Lair

	--The Ruby Sanctum
	AL["Red Dragonflight Chamber"] = "Kammer des roten Drachenschwarms";

	--The Nexus: The Eye of Eternity

	--The Nexus: The Nexus
	AL["Warmage Kaitlyn"] = "Kriegsmagierin Kaitlyn";
	AL["Berinand's Research"] = "Berinands Forschungsergebnisse";
	AL["Elder Igasho"] = "Urahne Igasho";

	--The Nexus: The Oculus
	AL["Belgaristrasz"] = "Belgaristrasz";
	AL["Eternos"] = "Eternos";
	AL["Verdisa"] = "Verdisa";
	AL["Centrifuge Construct"] = "Zentrifugenkonstrukt";
	AL["Cache of Eregos"] = "Eregos' Lager";

	--Trial of the Champion
	AL["Marshal Jacob Alerius"] = "Marschall Jacob Alerius";
	AL["Ambrose Boltspark"] = "Ambrose Bolzenfunk";
	AL["Colosos"] = "Kolosos";
	AL["Jaelyne Evensong"] = "Jaelyne Abendlied";
	AL["Lana Stouthammer"] = "Lana Starkhammer";

	--Trial of the Crusader
	AL["Heroic: Trial of the Grand Crusader"] = "Heroisch: Prüfung des Obersten Kreuzfahrers";
	AL["Cavern Entrance"] = "Höhleneingang";

	--Ulduar General
	AL["The Siege"] = "Die Belagerung";
	AL["The Keepers"] = "Die Hüter";

	--Ulduar A
	AL["Tower of Life"] = "Turm des Lebens";
	AL["Tower of Flame"] = "Turm der Flammen";
	AL["Tower of Frost"] = "Turm des Frostes";
	AL["Tower of Storms"] = "Turm der Stürme";

	--Ulduar B
	AL["Prospector Doren"] = "Ausgrabungsleiter Doren"; 
	AL["Archivum Console"] = "Archivumkonsole";

	--Ulduar C
	AL["Sif"] = "Sif";

	--Ulduar D

	--Ulduar E

	--Ulduar: Halls of Lightning
	AL["Stormherald Eljrrin"] = "Sturmbote Eljrrin";

	--Ulduar: Halls of Stone
	AL["Kaldir Ironbane"] = "Kaldir Eisenbann";
	AL["Tribunal Chest"] = "Kiste des Tribunals";
	AL["Elder Yurauk"] = "Urahne Yurauk";
	AL["Brann Bronzebeard"] = "Brann Bronzebart";

	--Utgarde Keep: Utgarde Keep
	AL["Defender Mordun"] = "Verteidiger Mordun";
	AL["Dark Ranger Marrah"] = "Dunkelläuferin Marrah";
	AL["Elder Jarten"] = "Urahne Jarten";

	--Utgarde Keep: Utgarde Pinnacle
	AL["Brigg Smallshanks"] = "Brigg Kleinkeul";
	AL["Image of Argent Confessor Paletress"] = "Abbild von Argentumbeichtpatin Blondlocke";
	AL["Elder Chogan'gada"] = "Urahne Chogan'gada";

	--Vault of Archavon

	--The Violet Hold
	AL["Lieutenant Sinclari"] = "Leutnant Sinclari";

--*********************
-- Cataclysm Instances
--*********************

	--Baradin Hold

	--Blackrock Caverns

	--Blackwing Descent

	--Caverns of Time: Dragon Soul
	AL["Dasnurimi <Geologist & Conservator>"] = "Dasnurimi <Geologin & Konservatorin>";
	AL["Lord Afrasastrasz"] = "Lord Afrasastrasz";

	--Caverns of Time: End Time
	AL["Alurmi"] = "Alurmi";
	AL["Nozdormu"] = "Nozdormu";

	--Caverns of Time: Hour of Twilight

	--Caverns of Time: Well of Eternity

	--Firelands
	AL["Lurah Wrathvine <Crystallized Firestone Collector>"] = "Lurah Zornranke <Sammlerin kristallisierten Feuersteins>";
	AL["Naresir Stormfury <Avengers of Hyjal Quartermaster>"] = "Naresir Sturmwut <Rüstmeister der Rächer des Hyjal>";

	--Grim Batol
	AL["Baleflame"] = "Unheilsflamme";
	AL["Farseer Tooranu <The Earthen Ring>"] = "Scharfseher Tooranu <Der Irdene Ring>";
	AL["Velastrasza"] = "Velastrasza";

	--Halls of Origination
	AL["Large Stone Obelisk"] = "Große Steintafel";

	--Lost City of the Tol'vir
	AL["Captain Hadan"] = "Kapitän Hadan";
	AL["Tol'vir Grave"] = "Grab der Tol'vir";

	--The Bastion of Twilight

	--The Stonecore
	AL["Earthwarden Yrsa <The Earthen Ring>"] = "Erdwächterin Yrsa <Der Irdene Ring>";

	--The Vortex Pinnacle
	AL["Itesh"] = "Itesh";
	AL["Magical Brazier"] = "Magische Kohlenpfanne";

	--Throne of the Four Winds

	--Throne of the Tides
	AL["Captain Taylor"] = "Kapitän Taylor";
	AL["Legionnaire Nazgrim"] = "Legionär Nazgrim";
	AL["Neptulon"] = "Neptulon";

	--Zul'Aman
	AL["Vol'jin"] = "Vol'jin";
	AL["Witch Doctor T'wansi"] = "Hexendoktor T'wansi";
	AL["Blood Guard Hakkuz <Darkspear Elite>"] = "Blutwache Hakkuz <Elite der Dunkelspeere>";
	AL["Voodoo Pile"] = "Voodoohaufen";
	AL["Bakkalzu"] = "Bakkalzu";
	AL["Hazlek"] = "Hazlek";
	AL["The Map of Zul'Aman"] = "Karte von Zul'Aman";
	AL["Norkani"] = "Norkani";
	AL["Kasha"] = "Kasha";
	AL["Thurg"] = "Thurg";
	AL["Gazakroth"] = "Gazakroth";
	AL["Lord Raadan"] = "Lord Raadan";
	AL["Darkheart"] = "Düsterherz";
	AL["Alyson Antille"] = "Alyson Antille";
	AL["Slither"] = "Glibber";
	AL["Fenstalker"] = "Fennpirscher";
	AL["Koragg"] = "Koragg";
	AL["Zungam"] = "Zungam";
	AL["Forest Frogs"] = "Urwaldfrösche";
	AL["Eulinda <Reagents>"] = "Eulinda <Reagenzien>";
	AL["Harald <Food Vendor>"] = "Harald <Lebensmittelhändler>";
	AL["Arinoth"] = "Arinoth";
	AL["Kaldrick"] = "Kaldrick";
	AL["Lenzo"] = "Lenzo";
	AL["Mawago"] = "Mawago";
	AL["Melasong"] = "Melasong";
	AL["Melissa"] = "Melissa";
	AL["Micah"] = "Micah";
	AL["Relissa"] = "Relissa";
	AL["Rosa"] = "Rosa";
	AL["Tyllan"] = "Tyllan";

	--Zul'Gurub
	AL["Briney Boltcutter <Blackwater Financial Interests>"] = "Briney Schraubschneider <Schwarzmeer Kapitalbeteiligungen>";
	AL["Vehini <Assault Provisions>"] = "Vehini <Angriffsvorräte>";
	AL["Overseer Blingbang"] = "Aufseher Klunkerknall";
	AL["Bloodslayer T'ara <Darkspear Veteran>"] = "Blutschlächterin T'ara <Dunkelspeerveteranin>";
	AL["Bloodslayer Vaena <Darkspear Veteran>"] = "Blutschlächterin Vaena <Dunkelspeerveteranin>";
	AL["Bloodslayer Zala <Darkspear Veteran>"] = "Blutschlächterin Zala <Dunkelspeerveteranin>";
	AL["Helpful Jungle Monkey"] = "Hilfreicher Dschungelaffe";
	AL["Venomancer Mauri <The Snake's Whisper>"] = "Giftmischerin Mauri <Das Flüstern der Schlange>";
	AL["Zanzil's Cauldron of Toxic Torment"] = "Zanzils Kessel der giftigen Grausamkeit";
	AL["Tiki Lord Mu'Loa"] = "Tikilord Mu'Loa";
	AL["Gub <Destroyer of Fish>"] = "Gub <Fischvernichter>";
	AL["Venomancer T'Kulu <The Toxic Bite>"] = "Giftmischer T'Kulu <Der Toxische Biss>";
	AL["Tor-Tun <The Slumberer>"] = "Tor-Tun <Der Schläfer>";
	AL["Kaulema the Mover"] = "Kaulema der Beweger";
	AL["Berserking Boulder Roller"] = "Wütender Felsroller";
	AL["Zanzil's Cauldron of Frostburn Formula"] = "Zanzils Kessel des frierenden Fleisches";
	AL["Mor'Lek the Dismantler"] = "Mor'Lek der Zerleger";
	AL["Witch Doctor Qu'in <Medicine Woman>"] = "Hexendoktor Qu'in <Medizinfrau>";
	AL["Zanza the Restless"] = "Zanza der Ruhelose";
	AL["Mortaxx <The Tolling Bell>"] = "Mortaxx <Das Schlagen der Stunde>";
	AL["Tiki Lord Zim'wae"] = "Tikilord Zim'wae";
	AL["Zanzil's Cauldron of Burning Blood"] = "Zanzils Kessel des brennenden Blutes";

--*********************
-- Mists of Pandaria Instances
--*********************

	--Gate of the Setting Sun
	AL["Bowmistress Li <Guard Captain>"] = "Bogenmeisterin Li <Wachoffizierin>";

	--Heart of Fear

	--Mogu'shan Palace
	AL["Sinan the Dreamer"] = "Sinan die Träumerin";

	--Mogu'shan Vaults

	--Shado-Pan Monastery
	AL["Ban Bearheart"] = "Ban Bärenherz";

	--Siege of Niuzao Temple
	AL["Shado-Master Chum Kiu"] = "Shado-Meister Chum-Kiu";

	--Siege of Orgrimmar

	--Stormstout Brewery
	AL["Auntie Stormstout"] = "Tantchen Sturmbräu";
	AL["Chen Stormstout"] = "Chen Sturmbräu";

	--Temple of the Jade Serpent
	AL["Master Windstrong"] = "Meister Windstark";
	AL["Priestess Summerpetal"] = "Priesterin Sommerblatt";

	--Terrace of Endless Spring

	--Throne of Thunder
	AL["Monara <The Last Queen>"] = "Monara <Die Letzte Königin>";
	AL["No'ku Stormsayer <Lord of Tempest>"] = "No'ku Sturmsprecher <Herr der Stürme>";
	AL["Rocky Horror"] = "Krankenstein";
	AL["Focused Eye"] = "Fokussiertes Auge";
	AL["Unblinking Eye"] = "Starrendes Auge";
	AL["Archritualist Kelada"] = "Erzritualist Kelada";
	AL["Flesh'rok the Diseased <Primordial Saurok Horror>"] = "Fleisch'rok der Verpestete <Urzeitlicher Saurokschrecken>";
	AL["Zao'cho <The Emperor's Shield>"] = "Zao'cho <Der Schild des Kaisers>";

end