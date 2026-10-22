-- $Id: Atlas_WorldEvents-enUS.lua 68 2014-07-06 08:34:50Z arith $
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
local AL = AceLocale:NewLocale("Atlas_WorldEvents", "enUS", true);

if AL then


	-- ----------------------------------------------------------------------
	-- Common
	-- ----------------------------------------------------------------------
	AL["World Events Maps"] = "World Events Maps";
	AL["Lower"] = "Lower";
	AL["Major Cities"] = "Major Cities";
	AL["Red: "] = "Red: ";
	AL["Blue: "] = "Blue: ";
	AL["Orange: "]  = "Orange: ";
	AL["Purple: "] = "Purple: ";
	AL["Quest: "] = "Quest: ";
	AL["Start"] = "Start"; -- The quest started NPC
	AL["End"] = "End"; -- The quest ended NPC
	AL["Horde Questline"] = "Horde Questline";
	AL["Alliance Questline"] = "Alliance Quest line";
	AL["Portals"] = "Portals";
	AL["Daily Quest"] = "Daily Quest";
	AL["Quest Destination"] = "Quest Destination";
	AL["Daily Group"] = "Daily Group";

	-- ----------------------------------------------------------------------
	-- World Events
	-- ----------------------------------------------------------------------
	AL["Lunar Festival"] = "Lunar Festival";
	AL["Lunar_Date"] = "Jan. 20 ~ Feb. 03"; 	-- update dates for year 2014

	AL["Midsummer Fire Festival"] = "Midsummer Fire Festival";
	AL["Midsummer_Date"] = "Jun. 21 - Jul. 5"; 	-- update dates for year 2014

	AL["Hallow's End"] = "Hallow's End";
	AL["Hallow_Date"] = "Oct. 18 ~ Nov. 01";	-- update dates for year 2014
	
	AL["Harvest Festival"] = "Harvest Festival";	
	AL["Harvest_Date"] = "Sep. 2 ~ Sep. 9";		-- update dates for year 2014

	AL["Brewfest"] = "Brewfest";
	AL["Brewfest_Date"] = "Sep. 20 ~ Oct. 6"; 	-- update dates for year 2014

	AL["Love is in the Air"] = "Love is in the Air";
	AL["Love_Date"] = "Feb. 10 - Feb. 24";		-- update dates for year 2014

	-- Darkmoon Faire
	AL["The first Sunday each month, last for a week"] = "The first Sunday each month, last for a week";
	
	AL["Feast of Winter Veil"] = "Feast of Winter Veil";
	AL["Winter_Date"] = "Dec. 16 - Jan. 02";	-- update dates for year 2014

	-- ----------------------------------------------------------------------
	-- Lunar Festival
	-- ----------------------------------------------------------------------
	-- Azeroth
	AL["Elder Hammershout"] = "Elder Hammershout";	-- 15562
	AL["Elder Stormbrow"] = "Elder Stormbrow";	-- 15565
	AL["Elder Skychaser"] = "Elder Skychaser";	-- 15577
	AL["Elder Winterhoof"] = "Elder Winterhoof";	-- 15576
	AL["Elder Starglade"] = "Elder Starglade";	-- 15596
	AL["Elder Bellowrage"] = "Elder Bellowrage";	-- 15563
	AL["Elder Starsong"] = "Elder Starsong";	-- 15593
	AL["Elder Rumblerock"] = "Elder Rumblerock";	-- 15557
	AL["Elder Dawnstrider"] = "Elder Dawnstrider";
	AL["Elder Ironband"] = "Elder Ironband";
	AL["Elder Morndeep"] = "Elder Morndeep";
	AL["Elder Stonefort"] = "Elder Stonefort";
	AL["Elder Goldwell"] = "Elder Goldwell";
	AL["Elder Bronzebeard"] = "Elder Bronzebeard";
	AL["Elder Silvervein"] = "Elder Silvervein";
	AL["Elder Highpeak"] = "Elder Highpeak";
	AL["Elder Snowcrown"] = "Elder Snowcrown";
	AL["Elder Windrun"] = "Elder Windrun";
	AL["Elder Farwhisper"] = "Elder Farwhisper";
	AL["Elder Meadowrun"] = "Elder Meadowrun";
	AL["Elder Moonstrike"] = "Elder Moonstrike";
	AL["Elder Graveborn"] = "Elder Graveborn";
	AL["Elder Darkcore"] = "Elder Darkcore";
	AL["Elder Obsidian"] = "Elder Obsidian";
	-- Kalimdor
	AL["Valadar Starsong"] = "Valadar Starsong";	-- 15864
	AL["Fariel Starsong"] = "Fariel Starsong";
	AL["Omen"] = "Omen";
	AL["Elder Bladeleaf"] = "Elder Bladeleaf";
	AL["Elder Bladeswift"] = "Elder Bladeswift";
	AL["Elder Starweave"] = "Elder Starweave";
	AL["Elder Nightwind"] = "Elder Nightwind";
	AL["Elder Brightspear"] = "Elder Brightspear";
	AL["Elder Stonespire"] = "Elder Stonespire";
	AL["Elder Skygleam"] = "Elder Skygleam";
	AL["Elder Darkhorn"] = "Elder Darkhorn";
	AL["Runetotem the Elder"] = "Runetotem the Elder";	-- 15572
	AL["Elder Windtotem"] = "Elder Windtotem";
	AL["Elder Moonwarden"] = "Elder Moonwarden";
	AL["Elder High Mountain"] = "Elder High Mountain";
	AL["Elder Bloodhoof"] = "Elder Bloodhoof";
	AL["Elder Ezra Wheathoof"] = "Elder Ezra Wheathoof";
	AL["Elder Skyseer"] = "Elder Skyseer";
	AL["Elder Morningdew"] = "Elder Morningdew";
	AL["Elder Dreamseer"] = "Elder Dreamseer";
	AL["Elder Wildmane"] = "Elder Wildmane";
	AL["Elder Ragetotem"] = "Elder Ragetotem";
	AL["Elder Thunderhorn"] = "Elder Thunderhorn";
	AL["Elder Bladesing"] = "Elder Bladesing";	-- 15599
	AL["Elder Primestone"] = "Elder Primestone";
	AL["Grimtotem Elder"] = "Grimtotem Elder";
	AL["Elder Mistwalker"] = "Elder Mistwalker";
	AL["Elder Splitrock"] = "Elder Splitrock";
	AL["Elder Riversong"] = "Elder Riversong";
	-- Northrend
	AL["Elder Sardis"] = "Elder Sardis";
	AL["Elder Pamuya"] = "Elder Pamuya";
	AL["Elder Northal"] = "Elder Northal";
	AL["Elder Igasho"] = "Elder Igasho";
	AL["Elder Arp"] = "Elder Arp";	-- 30364
	AL["Elder Sandrene"] = "Elder Sandrene";
	AL["Elder Wanikaya"] = "Elder Wanikaya";
	AL["Elder Bluewolf"] = "Elder Bluewolf";
	AL["Elder Nurgen"] = "Elder Nurgen";
	AL["Elder Morthie"] = "Elder Morthie";
	AL["Elder Skywarden"] = "Elder Skywarden";
	AL["Elder Thoim"] = "Elder Thoim";
	AL["Elder Graymane"] = "Elder Graymane";
	AL["Elder Fargal"] = "Elder Fargal";
	AL["Elder Stonebeard"] = "Elder Stonebeard";
	AL["Elder Yurauk"] = "Elder Yurauk";
	AL["Elder Muraco"] = "Elder Muraco";
	AL["Elder Ohanzee"] = "Elder Ohanzee";
	AL["Elder Tauros"] = "Elder Tauros";
	AL["Elder Beldak"] = "Elder Beldak";	-- 30357
	AL["Elder Whurain"] = "Elder Whurain";
	AL["Elder Lunaro"] = "Elder Lunaro";
	AL["Elder Jarten"] = "Elder Jarten";
	AL["Elder Chogan'gada"] = "Elder Chogan'gada";
	AL["Elder Kilias"] = "Elder Kilias";

	-- ----------------------------------------------------------------------
	-- Hallow's End
	-- ----------------------------------------------------------------------
	AL["Trick: "] = "Trick: ";
	AL["Masked Orphan Matron"] = "Masked Orphan Matron";
	AL["Orc Commoner"] = "Orc Commoner";
	AL["Fire Training"] = "Fire Training";
	AL["\"Let the Fires Come!\""] = "\"Let the Fires Come!\"";
	AL["Stop the Fires!"] = "Stop the Fires!";
	AL["Hallow's End Treats for Spoops!"] = "Hallow's End Treats for Spoops!";
	AL["Spoops"] = "Spoops";
	AL["Orgrimmar Nougat"] = "Orgrimmar Nougat";
	AL["Darkspear Gumdrop"] = "Darkspear Gumdrop";
	AL["Thunder Bluff Marzipan"] = "Thunder Bluff Marzipan";
	AL["Undercity Mint"] = "Undercity Mint";
	AL["Innkeeper Gryshka"] = "Innkeeper Gryshka";
	AL["Kali Remik"] = "Kali Remik";
	AL["Innkeeper Pala"] = "Innkeeper Pala";
	AL["Innkeeper Norman"] = "Innkeeper Norman";
	AL["Costumed Orphan Matron"] = "Costumed Orphan Matron";
	AL["Human Commoner"] = "Human Commoner";
	AL["Fire Brigade Practice"] = "Fire Brigade Practice";
	AL["Hallow's End Treats for Jesper!"] = "Hallow's End Treats for Jesper!";
	AL["Jesper"] = "Jesper";
	AL["Stormwind Nougat"] = "Stormwind Nougat";
	AL["Gnomeregan Gumdrop"] = "Gnomeregan Gumdrop";
	AL["Ironforge Mint"] = "Ironforge Mint";
	AL["Darnassus Marzipan"] = "Darnassus Marzipan";
	AL["Innkeeper Allison"] = "Innkeeper Allison";
	AL["Talvash del Kissel"] = "Talvash del Kissel";
	AL["Innkeeper Firebrew"] = "Innkeeper Firebrew";
	AL["Innkeeper Saelienne"] = "Innkeeper Saelienne";

	-- ----------------------------------------------------------------------
	-- Harvest Festival
	-- ----------------------------------------------------------------------
	-- Quest
	AL["Honoring a Hero"] = "Honoring a Hero";
	
	-- NPC
	AL["Javnir Nashak"] = "Javnir Nashak";
	
	AL["Wagner Hammerstrike"] = "Wagner Hammerstrike";
	
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire
	-- ----------------------------------------------------------------------
	-- Darkmoon Faire - NPC
	AL["Selina Dourman <Darkmoon Faire Information>"] = "Selina Dourman <Darkmoon Faire Information>"; -- 10445
	AL["Zina Sharpworth <Game Tokens>"] = "Zina Sharpworth <Game Tokens>"; -- 55266
	AL["Mola <Whack-a-Mole>"] = "Mola <Whack-a-Mole>"; -- 54601
	AL["Maxima Blastenheimer <Darkmoon Faire Cannoneer>"] = "Maxima Blastenheimer <Darkmoon Faire Cannoneer>"; -- 15303
	AL["Rinling"] = "Rinling"; -- 14841
	AL["Sazz Coincatcher <Game Tokens>"] = "Sazz Coincatcher <Game Tokens>"; -- 55264
	AL["Gelvas Grimegate <Souvenir & Toy Prizes>"] = "Gelvas Grimegate <Souvenir & Toy Prizes>"; -- 14828
	AL["Aimee <Pie, Pastry & Cakes>"] = "Aimee <Pie, Pastry & Cakes>"; -- 29548
	AL["Finlay Coolshot <Tonk Challenge>"] = "Finlay Coolshot <Tonk Challenge>"; -- 54605
	AL["Stamp Thunderhorn <Food Vendor>"] = "Stamp Thunderhorn <Food Vendor>"; -- 14845
	AL["Professor Thaddeus Paleo <Darkmoon Cards>"] = "Professor Thaddeus Paleo <Darkmoon Cards>"; -- 14847
	AL["Chronos <He Who Never Forgets!>"] = "Chronos <He Who Never Forgets!>"; -- 14833
	AL["Sylannia <Drink Vendor>"] = "Sylannia <Drink Vendor>"; -- 14844
	AL["Baruma <Replica Armor Prizes>"] = "Baruma <Replica Armor Prizes>"; -- 57983
	AL["Barum <Replica Armor Prizes>"] = "Barum <Replica Armor Prizes>"; -- 55072
	AL["Daenrand Dawncrest <Heirloom Prizes>"] = "Daenrand Dawncrest <Heirloom Prizes>"; -- 56335
	AL["Kerri Hicks <The Strongest Woman Alive!>"] = "Kerri Hicks <The Strongest Woman Alive!>"; -- 14832
	AL["Lhara <Pet & Mount Prizes>"] = "Lhara <Pet & Mount Prizes>"; -- 14846
	AL["Boomie Sparks <Fireworks>"] = "Boomie Sparks <Fireworks>"; -- 55278
	AL["Trixi Sharpworth <Game Tokens>"] = "Trixi Sharpworth <Game Tokens>"; -- 55339
	AL["Korgol Crushskull <The Pit Master>"] = "Korgol Crushskull <The Pit Master>"; -- 55402
	AL["Sayge"] = "Sayge"; -- 14822
	AL["Jessica Rogers <Ring Toss>"] = "Jessica Rogers <Ring Toss>"; -- 54485
	AL["Rona Greenteeth"] = "Rona Greenteeth"; -- 56041
	AL["Tatia Brine <Fishing Supplies>"] = "Tatia Brine <Fishing Supplies>"; -- 56069
	AL["Teleportologist Fozlebub <Gone Fishin'>"] = "Teleportologist Fozlebub <Gone Fishin'>"; -- 57850
	AL["Steven Stagnaro <Fishing Trainer>"] = "Steven Stagnaro <Fishing Trainer>"; -- 56068

	AL["Entrance (Transported)"] = "Entrance (Transported)";
	AL["Petting Zoo"] = "Petting Zoo";
	AL["L90ETC Concert"] = "L90ETC Concert";

	-- Darkmoon Faire - Quest
	AL["Target: Turtle"] = "Target: Turtle"; -- 29455
	AL["Tonk Commander"] = "Tonk Commander"; -- 29434
	AL["He Shoots, He Scores!"] = "He Shoots, He Scores!"; -- 29438
	AL["The Humanoid Cannonball"] = "The Humanoid Cannonball"; -- 29436
	AL["It's Hammer Time"] = "It's Hammer Time"; -- 29463
	AL["Test Your Strength"] = "Test Your Strength"; -- 29433
	AL["Putting the Crunch in the Frog"] = "Putting the Crunch in the Frog"; -- 29509
	AL["Putting Trash to Good Use"] = "Putting Trash to Good Use"; -- 29510
	AL["Putting the Carnies Back Together Again"] = "Putting the Carnies Back Together Again"; -- 29512
	AL["Keeping the Faire Sparkling"] = "Keeping the Faire Sparkling"; -- 29516

	-- ----------------------------------------------------------------------
	-- Brewfest
	-- ----------------------------------------------------------------------
	-- Queest
	AL["Brewfest!"] = "Brewfest!"; -- 11441 / 11446
	AL["Catch the Wild Wolpertinger!"] = "Catch the Wild Wolpertinger!"; -- 11117 / 11431
	AL["Pink Elekks On Parade"] = "Pink Elekks On Parade"; -- 11118 / 11120
	AL["Chug and Chuck!"] = "Chug and Chuck!"; -- 12022 / 12191
	AL["Now This is Ram Racing... Almost."] = "Now This is Ram Racing... Almost."; -- 11318 / 11409
	AL["There and Back Again"] = "There and Back Again"; -- 11122 / 11412
	AL["A New Supplier of Souvenirs"] = "A New Supplier of Souvenirs"; -- 29396 / 29397
	AL["Bark for the Barleybrews!"] = "Bark for the Barleybrews!"; -- 11293
	AL["Bark for the Thunderbrews!"] = "Bark for the Thunderbrews!"; -- 11294
	AL["Bark for Drohn's Distillery!"] = "Bark for Drohn's Distillery!"; -- 11407
	AL["Bark for T'chali's Voodoo Brewery!"] = "Bark for T'chali's Voodoo Brewery!"; -- 11408
	AL["Brew For Brewfest"] = "Brew For Brewfest"; -- 29394 / 29393
	AL["This One Time, When I Was Drunk..."] = "This One Time, When I Was Drunk..."; -- 12020
	AL["Brew of the Month Club"] = "Brew of the Month Club";
	-- Brewfest - NPC
	AL["Dwarf Commoner"] = "Dwarf Commoner";
	AL["Gnome Commoner"] = "Gnome Commoner";
	AL["Night Elf Commoner"] = "Night Elf Commoner";
	AL["Human Commoner"] = "Human Commoner";
	AL["Draenei Commoner"] = "Draenei Commoner";
	AL["Orc Commoner"] = "Orc Commoner";
	AL["Troll Commoner"] = "Troll Commoner";
	AL["Tauren Commoner"] = "Tauren Commoner";
	AL["Forsaken Commoner"] = "Forsaken Commoner";
	AL["Blood Elf Commoner"] = "Blood Elf Commoner";
	AL["Ipfelkofer Ironkeg"] = "Ipfelkofer Ironkeg";
	AL["Goldark Snipehunter"] = "Goldark Snipehunter";
	AL["Boxey Boltspinner"] = "Boxey Boltspinner";
	AL["Neill Ramstein"] = "Neill Ramstein";
	AL["Flynn Firebrew"] = "Flynn Firebrew";
	AL["Pol Amberstill"] = "Pol Amberstill";
	AL["Belbi Quikswitch"] = "Belbi Quikswitch";
	AL["Becan Barleybrew"] = "Becan Barleybrew";
	AL["Daran Thunderbrew"] = "Daran Thunderbrew";
	AL["Dark Iron Mole Machine Wreckage"] = "Dark Iron Mole Machine Wreckage"; -- 189989
	AL["Larkin Thunderbrew"] = "Larkin Thunderbrew";
	AL["Ray'ma"] = "Ray'ma";
	AL["Tapper Swindlekeg"] = "Tapper Swindlekeg";
	AL["Glodrak Huntsniper"] = "Glodrak Huntsniper";
	AL["Bizzle Quicklift"] = "Bizzle Quicklift";
	AL["Ram Master Ray"] = "Ram Master Ray";
	AL["Bok Dropcertain"] = "Bok Dropcertain";
	AL["Driz Tumblequick"] = "Driz Tumblequick";
	AL["Blix Fixwidget"] = "Blix Fixwidget";
	AL["Cort Gorestein"] = "Cort Gorestein";
	AL["Ja'ron"] = "Ja'ron";

	-- ----------------------------------------------------------------------
	-- Love is in the Air
	-- ----------------------------------------------------------------------
	-- Quests
	AL["Bonbon Blitz"] = "Bonbon Blitz";
	AL["A Perfect Puff of Perfume"] = "A Perfect Puff of Perfume";
	AL["A Cloudlet of Classy Cologne"] = "A Cloudlet of Classy Cologne";
	AL["A gift for a Lord of Ironforge"] = "A gift for a Lord of Ironforge";
	AL["A gift for a Lord of Stormwind"] = "A gift for a Lord of Stormwind";
	AL["Uncommon Scents"] = "Uncommon Scents";
	AL["Something Stinks"] = "Something Stinks";
	AL["Pilfering Perfume"] = "Pilfering Perfume";
	AL["Fireworks At The Gilded Rose"] = "Fireworks At The Gilded Rose";
	AL["Hot On The Trail"] = "Hot On The Trail";
	AL["A Friendly Chat..."] = "A Friendly Chat...";
	AL["Snivel's Sweetheart"] = "Snivel's Sweetheart";
	AL["Something is in the Air (and it Ain't Love)"] = "Something is in the Air (and it Ain't Love)";
	AL["Crushing the Crown"] = "Crushing the Crown";
	AL["A Gift for the Prophet"] = "A Gift for the Prophet";
	AL["A Gift for the High Priestess of Elune"] = "A Gift for the High Priestess of Elune";
	AL["A Gift for the Regent Lord of Quel'Thalas"] = "A Gift for the Regent Lord of Quel'Thalas";
	AL["A Gift for the Banshee Queen"] = "A Gift for the Banshee Queen";
	AL["A Gift for the Warchief"] = "A Gift for the Warchief";
	AL["A Gift for the High Chieftain"] = "A Gift for the High Chieftain";
	-- NPC
	AL["Kwee Q. Peddlefeet"] = "Kwee Q. Peddlefeet";
	AL["Public Relations Agent"] = "Public Relations Agent";
	AL["Junior Detective"] = "Junior Detective";
	AL["Detective Snap Snagglebolt"] = "Detective Snap Snagglebolt";
	AL["Inspector Snip Snagglebolt"] = "Inspector Snip Snagglebolt";
	AL["Marion Sutton"] = "Marion Sutton";
	AL["Faded Lovely Greeting Card"] = "Faded Lovely Greeting Card";
	AL["King Varian Wrynn"] = "King Varian Wrynn";
	AL["Muradin Bronzebeard"] = "Muradin Bronzebeard";
	AL["Prophet Velen"] = "Prophet Velen";
	AL["Tyrande Whisperwind"] = "Tyrande Whisperwind";
	AL["Lor'themar Theron"] = "Lor'themar Theron";
	AL["Baine Bloodhoof"] = "Baine Bloodhoof";
	AL["Lady Sylvanas Windrunner"] = "Lady Sylvanas Windrunner";
	AL["Roka"] = "Roka";
	-- Mics.
	AL["The Gilded Rose"] = "The Gilded Rose";
	AL["Gotri's Traveling Gear"] = "Gotri's Traveling Gear";
	
	-- ----------------------------------------------------------------------
	-- Feast of Winter Veil
	-- ----------------------------------------------------------------------
	-- Quest
	AL["Great-father Winter is Here!"] = "Great-father Winter is Here!";
	AL["Greatfather Winter is Here!"] = "Greatfather Winter is Here!";
	AL["You're a Mean One..."] = "You're a Mean One...";
	AL["A Smokywood Pastures' Thank You!"] = "A Smokywood Pastures' Thank You!";
	AL["Treats for Great-father Winter"] = "Treats for Great-father Winter";
	AL["Treats for Greatfather Winter"] = "Treats for Greatfather Winter";
	AL["The Reason for the Season"] = "The Reason for the Season";
	AL["The Feast of Winter Veil"] = "The Feast of Winter Veil";
	-- Horde NPC
	AL["Kaymard Copperpinch"] = "Kaymard Copperpinch";
	AL["Great-father Winter"] = "Great-father Winter";
	AL["Furmund"] = "Furmund";
	AL["Sagorne Creststrider"] = "Sagorne Creststrider";
	-- Alliance NPC
	AL["Wulmort Jinglepocket"] = "Wulmort Jinglepocket";
	AL["Greatfather Winter"] = "Greatfather Winter";
	AL["Goli Krumn"] = "Goli Krumn";
	AL["Historian Karnik"] = "Historian Karnik";
	-- Other
	AL["The Abominable Greench"] = "The Abominable Greench";

	-- ----------------------------------------------------------------------
	-- Location Description
	-- ----------------------------------------------------------------------
	AL["(Outside the city gate)"] = "(Outside the city gate)"; -- Elder Hammershout is outside the Stormwind City gate.
	AL["(Outside the gate)"] = "(Outside the gate)"; -- Elder Starglade is outside the gate of Zul'Gurub
	AL["(On the roof of the bank)"] = "(On the roof of the bank)";
	AL["(On the ground)"] = "(On the ground)";
	AL["(The roof of Scholomance, outside the dungeon)"] = "(The roof of Scholomance, outside the dungeon)";
	AL["Southeast coast of the Lake Elune'ara"] = "Southeast coast of the Lake Elune'ara";
	AL["(Next to the flight master)"] = "(Next to the flight master)";
	AL["(Inside the fortress)"] = "(Inside the fortress)"; -- The Elder Bluewolf is inside the Wintergrasp Fortress

	AL["Quest: Elune's Blessing"] = "Quest: Elune's Blessing";
end
