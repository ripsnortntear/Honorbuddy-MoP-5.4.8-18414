-- $Id: Atlas_WorldEvents-deDE.lua 68 2014-07-06 08:34:50Z arith $
--[[

	Atlas World Events, a World of Warcraft map browser for world events
	Copyright 2011 ~ 2013 Arith Hsu

	This file is a plugin of Atlas.

	Atlas World Events is free software; you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation; either version 2 of the License, or
	(at your option) any later version.

	Atlas World Events is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Atlas; if not, write to the Free Software
	Foundation, Inc., 51 Franklin St, Fifth Floor, Boston, MA  02110-1301  USA

--]]

local AceLocale = LibStub:GetLibrary("AceLocale-3.0");
local AL = AceLocale:NewLocale("Atlas_WorldEvents", "deDE", false);

if AL then


	-- ----------------------------------------------------------------------
	-- Common
	-- ----------------------------------------------------------------------
	AL["World Events Maps"] = "Weltereigniskarten" -- Needs review
	AL["Lower"] = "Untere" -- Needs review
	AL["Major Cities"] = "Hauptstädte" -- Needs review
	AL["Red: "] = "Rot:" -- Needs review
	AL["Blue: "] = "Blau:" -- Needs review
	AL["Orange: "] = "Orange:" -- Needs review
	AL["Purple: "] = "Lila:" -- Needs review
	AL["Quest: "] = "Quest:" -- Needs review
	AL["Start"] = "Start" -- Needs review
	AL["End"] = "Ende" -- Needs review
	AL["Horde Questline"] = "Horde-Questreihe" -- Needs review
	AL["Alliance Questline"] = "Allianz-Questreihe" -- Needs review
	AL["Portals"] = "Portale" -- Needs review
	AL["Daily Quest"] = "Tagesquest";	-- Needs review
	AL["Quest Destination"] = "Quest-Ziel";	-- Needs review
	AL["Daily Group"] = "Täglich Gruppe";

	-- ----------------------------------------------------------------------
	-- World Events
	-- ----------------------------------------------------------------------
	AL["Lunar Festival"] = "Mondfest" -- Needs review
	AL["Lunar_Date"] = "20. Januar - 3. Februar" -- Needs review

	AL["Midsummer Fire Festival"] = "Sonnenwendfest" -- Needs review
	AL["Midsummer_Date"] = "21. Juni - 5. Juli" -- Needs review

	AL["Hallow's End"] = "Schlotternächte" -- Needs review
	AL["Hallow_Date"] = "18. Oktober - 1. November" -- Needs review
	
	AL["Harvest Festival"] = "Erntedankfest";	
	AL["Harvest_Date"] = "2. September ~ 9. September";	-- update dates for year 2014

	AL["Brewfest"] = "Braufest";
	AL["Brewfest_Date"] = "20. September - 6. Oktober"; 	-- update dates for year 2014

	AL["Love is in the Air"] = "Liebe liegt in der Luft";
	AL["Love_Date"] = "10. Februar - 24. Februar";		-- update dates for year 2014

	-- Darkmoon Faire
	AL["The first Sunday each month, last for a week"] = "Beginn: erster Sonntag jeden Monats, Dauer: eine Woche" -- Needs review

	AL["Feast of Winter Veil"] = "Winterhauchfest";
	AL["Winter_Date"] = "16. Dezember - 2. Januar";	-- update dates for year 2014

	-- ----------------------------------------------------------------------
	-- Lunar Festival
	-- ----------------------------------------------------------------------
	-- Azeroth
	AL["Elder Hammershout"] = "Urahne Hammerklang" -- Needs review
	AL["Elder Stormbrow"] = "Urahne Sturmbraue" -- Needs review
	AL["Elder Skychaser"] = "Urahnin Himmelsjäger" -- Needs review
	AL["Elder Winterhoof"] = "Urahne Winterhuf" -- Needs review
	AL["Elder Starglade"] = "Urahne Sternenau" -- Needs review
	AL["Elder Bellowrage"] = "Urahne Heulwut" -- Needs review
	AL["Elder Starsong"] = "Urahnin Sternensang" -- Needs review
	AL["Elder Rumblerock"] = "Urahne Rumpelfels" -- Needs review
	AL["Elder Dawnstrider"] = "Urahnin Morgenwandler" -- Needs review
	AL["Elder Ironband"] = "Urahne Eisenband" -- Needs review
	AL["Elder Morndeep"] = "Urahne Schwermut" -- Needs review
	AL["Elder Stonefort"] = "Urahne Steinwehr" -- Needs review
	AL["Elder Goldwell"] = "Urahne Goldbrunn" -- Needs review
	AL["Elder Bronzebeard"] = "Urahne Bronzebart" -- Needs review
	AL["Elder Silvervein"] = "Urahne Silberader" -- Needs review
	AL["Elder Highpeak"] = "Urahne Höhengipfel" -- Needs review
	AL["Elder Snowcrown"] = "Urahne Schneekrone" -- Needs review
	AL["Elder Windrun"] = "Urahne Windflucht" -- Needs review
	AL["Elder Farwhisper"] = "Urahne Fernwisper" -- Needs review
	AL["Elder Meadowrun"] = "Urahnin Auenflucht" -- Needs review
	AL["Elder Moonstrike"] = "Urahnin Mondstreich" -- Needs review
	AL["Elder Graveborn"] = "Urahne Grufthauch" -- Needs review
	AL["Elder Darkcore"] = "Urahne Dunkelherz" -- Needs review
	AL["Elder Obsidian"] = "Urahne Obsidian" -- Needs review
	-- Kalimdor
	AL["Valadar Starsong"] = "Valadar Sternensang" -- Needs review
	AL["Fariel Starsong"] = "Fariel Sternensang" -- Needs review
	AL["Omen"] = "Omen" -- Needs review
	AL["Elder Bladeleaf"] = "Urahne Messerblatt" -- Needs review
	AL["Elder Bladeswift"] = "Urahne Klingentanz" -- Needs review
	AL["Elder Starweave"] = "Urahnin Sternenweber" -- Needs review
	AL["Elder Nightwind"] = "Urahne Nachtwind" -- Needs review
	AL["Elder Brightspear"] = "Urahnin Prunkspeer" -- Needs review
	AL["Elder Stonespire"] = "Urahne Steinkeil" -- Needs review
	AL["Elder Skygleam"] = "Urahne Himmelsglanz" -- Needs review
	AL["Elder Darkhorn"] = "Urahnin Dunkelhorn" -- Needs review
	AL["Runetotem the Elder"] = "Urahnin Runentotem" -- Needs review
	AL["Elder Windtotem"] = "Urahnin Windtotem" -- Needs review
	AL["Elder Moonwarden"] = "Urahne Mondwacht" -- Needs review
	AL["Elder High Mountain"] = "Urahne Hohenberg" -- Needs review
	AL["Elder Bloodhoof"] = "Urahne Bluthuf" -- Needs review
	AL["Elder Ezra Wheathoof"] = "Urahne Weizhuf" -- Needs review
	AL["Elder Skyseer"] = "Urahne Himmelsdeuter" -- Needs review
	AL["Elder Morningdew"] = "Urahnin Morgentau" -- Needs review
	AL["Elder Dreamseer"] = "Urahne Traumdeuter" -- Needs review
	AL["Elder Wildmane"] = "Urahnin Wildmähne" -- Needs review
	AL["Elder Ragetotem"] = "Urahnin Rachtotem" -- Needs review
	AL["Elder Thunderhorn"] = "Urahne Donnerhorn" -- Needs review
	AL["Elder Bladesing"] = "Urahnin Klingensang" -- Needs review
	AL["Elder Primestone"] = "Urahne Urstein" -- Needs review
	AL["Grimtotem Elder"] = "Ältester der Grimmtotem" -- Needs review
	AL["Elder Mistwalker"] = "Urahnin Nebelgänger" -- Needs review
	AL["Elder Splitrock"] = "Urahne Splitterfels" -- Needs review
	AL["Elder Riversong"] = "Urahnin Flussweise" -- Needs review
	-- Northrend
	AL["Elder Sardis"] = "Urahne Sardis" -- Needs review
	AL["Elder Pamuya"] = "Urahne Pamuya" -- Needs review
	AL["Elder Northal"] = "Urahne Northal" -- Needs review
	AL["Elder Igasho"] = "Urahne Igasho" -- Needs review
	AL["Elder Arp"] = "Urahne Arp" -- Needs review
	AL["Elder Sandrene"] = "Urahne Sandrene" -- Needs review
	AL["Elder Wanikaya"] = "Urahne Wanikaya" -- Needs review
	AL["Elder Bluewolf"] = "Urahne Blauwolf" -- Needs review
	AL["Elder Nurgen"] = "Urahne Nurgen" -- Needs review
	AL["Elder Morthie"] = "Urahne Morthie" -- Needs review
	AL["Elder Skywarden"] = "Urahne Himmelswächter" -- Needs review
	AL["Elder Thoim"] = "Urahne Thoim" -- Needs review
	AL["Elder Graymane"] = "Urahne Graumähne" -- Needs review
	AL["Elder Fargal"] = "Urahne Fargal" -- Needs review
	AL["Elder Stonebeard"] = "Urahne Steinbart" -- Needs review
	AL["Elder Yurauk"] = "Urahne Yurauk" -- Needs review
	AL["Elder Muraco"] = "Urahne Muraco" -- Needs review
	AL["Elder Ohanzee"] = "Urahne Ohanzee" -- Needs review
	AL["Elder Tauros"] = "Urahne Tauros" -- Needs review
	AL["Elder Beldak"] = "Urahne Beldak" -- Needs review
	AL["Elder Whurain"] = "Urahne Whurain" -- Needs review
	AL["Elder Lunaro"] = "Urahne Lunaro" -- Needs review
	AL["Elder Jarten"] = "Urahne Jarten" -- Needs review
	AL["Elder Chogan'gada"] = "Urahne Chogan'gada" -- Needs review
	AL["Elder Kilias"] = "Urahne Kilias" -- Needs review

	-- ----------------------------------------------------------------------
	-- Hallow's End
	-- ----------------------------------------------------------------------
	AL["Trick: "] = "Streich:" -- Needs review
	AL["Masked Orphan Matron"] = "Maskierte Waisenmatrone" -- Needs review
	AL["Orc Commoner"] = "Orcbürger" -- Needs review
	AL["Fire Training"] = "Feuerübung" -- Needs review
	AL["\"Let the Fires Come!\""] = "\"Lasset die Feuer kommen!\"" -- Needs review
	AL["Stop the Fires!"] = "Löscht das Feuer!" -- Needs review
	AL["Hallow's End Treats for Spoops!"] = "Schlotternachtssüßigkeiten für Spuks!" -- Needs review
	AL["Spoops"] = "Spuks" -- Needs review
	AL["Orgrimmar Nougat"] = "Nugat aus Orgrimmar" -- Needs review
	AL["Darkspear Gumdrop"] = "Kaugummikugel der Dunkelspeere" -- Needs review
	AL["Thunder Bluff Marzipan"] = "Marzipan aus Donnerfels" -- Needs review
	AL["Undercity Mint"] = "Pfefferminzbonbon aus Unterstadt" -- Needs review
	AL["Innkeeper Gryshka"] = "Gastwirtin Gryshka" -- Needs review
	AL["Kali Remik"] = "Kali Remik" -- Needs review
	AL["Innkeeper Pala"] = "Gastwirtin Pala" -- Needs review
	AL["Innkeeper Norman"] = "Gastwirt Norman" -- Needs review
	AL["Costumed Orphan Matron"] = "Kostümierte Waisenmatrone" -- Needs review
	AL["Human Commoner"] = "Menschenbürger" -- Needs review
	AL["Fire Brigade Practice"] = "Übung bei der Brandwache" -- Needs review
	AL["Hallow's End Treats for Jesper!"] = "Schlotternachtssüßigkeiten für Jesper!" -- Needs review
	AL["Jesper"] = "Jesper" -- Needs review
	AL["Stormwind Nougat"] = "Nugat aus Sturmwind" -- Needs review
	AL["Gnomeregan Gumdrop"] = "Kaugummikugel aus Gnomeregan" -- Needs review
	AL["Ironforge Mint"] = "Pfefferminzbonbon aus Eisenschmiede" -- Needs review
	AL["Darnassus Marzipan"] = "Marzipan aus Darnassus" -- Needs review
	AL["Innkeeper Allison"] = "Gastwirtin Allison" -- Needs review
	AL["Talvash del Kissel"] = "Talvash del Kissel" -- Needs review
	AL["Innkeeper Firebrew"] = "Gastwirt Feuerbräu" -- Needs review
	AL["Innkeeper Saelienne"] = "Gastwirtin Saelienne" -- Needs review

	-- ----------------------------------------------------------------------
	-- Harvest Festival
	-- ----------------------------------------------------------------------
	-- Quest
	AL["Honoring a Hero"] = "Die Ehrung eines Helden";
	
	-- NPC
	AL["Javnir Nashak"] = "Javnir Nashak";
	
	AL["Wagner Hammerstrike"] = "Wagner Hammerschlag";
	
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire - NPC
	AL["Selina Dourman <Darkmoon Faire Information>"] = "Selina Sauermann <Information des Dunkelmond-Jahrmarkts>" -- Needs review
	AL["Zina Sharpworth <Game Tokens>"] = "Zina Schnittwert <Spieljetons>" -- Needs review
	AL["Mola <Whack-a-Mole>"] = "Mola <Hau-den-Gnoll>" -- Needs review
	AL["Maxima Blastenheimer <Darkmoon Faire Cannoneer>"] = "Maxima Blastenheimer <Kanonierin des Dunkelmond-Jahrmarkts>" -- Needs review
	AL["Rinling"] = "Rinling" -- Needs review
	AL["Sazz Coincatcher <Game Tokens>"] = "Sazz Münzfänger <Spieljetons>" -- Needs review
	AL["Gelvas Grimegate <Souvenir & Toy Prizes>"] = "Gelvas Rußgatter <Souvenir- & Spielzeuggewinne>" -- Needs review
	AL["Aimee <Pie, Pastry & Cakes>"] = "Aimee <Kuchen, Gebäck & Torten>" -- Needs review
	AL["Finlay Coolshot <Tonk Challenge>"] = "Finlay Schwarzpulver <Dampfpanzer-Herausforderung>" -- Needs review
	AL["Stamp Thunderhorn <Food Vendor>"] = "Stampf Donnerhorn <Lebensmittelverkäufer des Dunkelmond-Jahrmarkts>" -- Needs review
	AL["Professor Thaddeus Paleo <Darkmoon Cards>"] = "Professor Thaddeus Paleo <Karten des Dunkelmond-Jahrmarkts>" -- Needs review
	AL["Chronos <He Who Never Forgets!>"] = "Chronos <Der, der niemals vergisst!>" -- Needs review
	AL["Sylannia <Drink Vendor>"] = "Sylannia <Getränkeverkäufer des Dunkelmond-Jahrmarkts>" -- Needs review
	AL["Baruma <Replica Armor Prizes>"] = "Baruma <Rüstungsnachbildungengewinne>" -- Needs review
	AL["Barum <Replica Armor Prizes>"] = "Barum <Rüstungsnachbildungengewinne>" -- Needs review
	AL["Daenrand Dawncrest <Heirloom Prizes>"] = "Daenrand Lichtkranz <Erbstückgewinne>" -- Needs review
	AL["Kerri Hicks <The Strongest Woman Alive!>"] = "Kerri Hicks <Die stärkste Frau der Welt!>" -- Needs review
	AL["Lhara <Pet & Mount Prizes>"] = "Lhara <Haus- & Reittiergewinne>" -- Needs review
	AL["Boomie Sparks <Fireworks>"] = "Boomie Funks <Feuerwerk>" -- Needs review
	AL["Trixi Sharpworth <Game Tokens>"] = "Trixi Schnittwert <Spieljetons>" -- Needs review
	AL["Korgol Crushskull <The Pit Master>"] = "Korgol Schädelquetscher <Der Grubenmeister>" -- Needs review
	AL["Sayge"] = "Sayge" -- Needs review
	AL["Jessica Rogers <Ring Toss>"] = "Jessica Rogers <Ringwurf>" -- Needs review
	AL["Rona Greenteeth"] = "Rona Grünzahn" -- Needs review
	AL["Tatia Brine <Fishing Supplies>"] = "Tatia Brine <Angelbedarf>" -- Needs review
	AL["Teleportologist Fozlebub <Gone Fishin'>"] = "Teleportologe Fosselbab <Macht blau>" -- Needs review
	AL["Steven Stagnaro <Fishing Trainer>"] = "Steven Stagnaro <Angellehrer>" -- Needs review

	AL["Entrance (Transported)"] = "Eingang" -- Needs review
	AL["Petting Zoo"] = "Streichelzoo" -- Needs review
	AL["L90ETC Concert"] = "L90ETC Konzert" -- Needs review

	-- Darkmoon Faire - Quest
	AL["Target: Turtle"] = "Werft den Ring" -- Needs review
	AL["Tonk Commander"] = "Panzergeneral" -- Needs review
	AL["He Shoots, He Scores!"] = "Er schießt, er trifft!" -- Needs review
	AL["The Humanoid Cannonball"] = "Die menschliche Kanonenkugel" -- Needs review
	AL["It's Hammer Time"] = "Hau-den-Gnoll" -- Needs review
	AL["Test Your Strength"] = "Beweist Eure Stärke" -- Needs review
	AL["Putting the Crunch in the Frog"] = "Wie der Frosch knusprig wird" -- Needs review
	AL["Putting Trash to Good Use"] = "Auch Abfall kann nützlich sein" -- Needs review
	AL["Putting the Carnies Back Together Again"] = "Kratzer, Beulen, blaue Flecke" -- Needs review
	AL["Keeping the Faire Sparkling"] = "Jahrmarkt der Eitelkeiten" -- Needs review

	-- ----------------------------------------------------------------------
	-- Brewfest
	-- ----------------------------------------------------------------------
	-- Queest
	-- Brewfest - NPC
	-- ----------------------------------------------------------------------
	-- Love is in the Air
	-- ----------------------------------------------------------------------
	-- Quests
	-- NPC
	-- Mics.

	-- ----------------------------------------------------------------------
	-- Location Description
	-- ----------------------------------------------------------------------
	AL["(Outside the city gate)"] = "(Außerhalb des Stadttors)"
	AL["(Outside the gate)"] = "(Außerhalb des Tors)"
	AL["(On the roof of the bank)"] = "(Auf dem Dach der Bank)" -- Needs review
	AL["(On the ground)"] = "(Auf dem Boden)" -- Needs review
	AL["(The roof of Scholomance, outside the dungeon)"] = "(Das Dach von Scholomance, außerhalb der Instanz)" -- Needs review
	AL["Southeast coast of the Lake Elune'ara"] = "südöstliches Ufer des Sees von Elune'ara" -- Needs review
	AL["(Next to the flight master)"] = "(Neben dem Flugmeister)" -- Needs review
	AL["(Inside the fortress)"] = "Der Urahne Blauwolf befindet sich innerhalb der Festung von Tausendwinter." -- Needs review

	AL["Quest: Elune's Blessing"] = "Quest: Elunes Segen" -- Needs review
end
