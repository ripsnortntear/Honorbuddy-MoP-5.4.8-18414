local addonName, ns = ...

local NONE = 0
local ALLIANCE = 1
local HORDE = 2
local NEUTRAL = 3

ns.TaxiHashMap = {
	["Acherus: The Ebon Hold"] = {
		[NEUTRAL] = 315,
	},
	["Aerie Peak, The Hinterlands"] = {
		[ALLIANCE] = 43,
	},
	["Agmar's Hammer, Dragonblight"] = {
		[HORDE] = 256,
	},
	["Allerian Stronghold, Terokkar Forest"] = {
		[ALLIANCE] = 121,
	},
	["Altar of Sha'tar, Shadowmoon Valley"] = {
		[NEUTRAL] = 140,
	},
	["Amber Ledge, Borean (To Beryl)"] = {
		[NONE] = 221,
	},
	["Amber Ledge, Borean (to Coldarra)"] = {
		[NONE] = 225,
	},
	["Amber Ledge, Borean Tundra"] = {
		[NEUTRAL] = 289,
	},
	["Amberpine Lodge, Grizzly Hills"] = {
		[ALLIANCE] = 253,
	},
	["Andorhal, Western Plaguelands"] = {
		[ALLIANCE] = 650,
		[HORDE] = 649,
	},
	["Andruk, Zoram'gar"] = {
		[NONE] = 349,
	},
	["Apothecary Camp, Howling Fjord"] = {
		[HORDE] = 248,
	},
	["Area 52, Netherstorm"] = {
		[NEUTRAL] = 122,
	},
	["Argent Stand, Zul'Drak"] = {
		[NONE] = 290,
	},
	["Argent Tournament Grounds, Icecrown"] = {
		[NEUTRAL] = 340,
	},
	["Ashenvale 4.x, Azshara to Mor'shan Ramparts"] = {
		[NONE] = 346,
	},
	["Ashenvale 4.x, Mor'shan Landing"] = {
		[NONE] = 347,
	},
	["Astranaar, Ashenvale"] = {
		[ALLIANCE] = 28,
	},
	["Azure Watch, Azuremyst Isle"] = {
		[ALLIANCE] = 624,
	},
	["Bambala, Stranglethorn"] = {
		[HORDE] = 593,
	},
	["Beeble's Wreck, Isle Of Giants"] = {
		[ALLIANCE] = 1221,
	},
	["Beryl Point, Borean"] = {
		[NONE] = 222,
	},
	["Bilgewater Harbor, Azshara"] = {
		[HORDE] = 44,
	},
	["Binan Village, Kun-Lai Summit"] = {
		[NEUTRAL] = 1017,
	},
	["Blackfathom Camp, Ashenvale"] = {
		[ALLIANCE] = 338,
	},
	["Blood Watch, Bloodmyst Isle"] = {
		[ALLIANCE] = 93,
	},
	["Bloodgulch, Twilight Highlands"] = {
		[HORDE] = 659,
	},
	["Bloodhoof Village, Mulgore"] = {
		[HORDE] = 402,
	},
	["Bloodvenom Post, Felwood [DISABLED in 4.x]"] = {
		[NONE] = 48,
	},
	["Bloodwatcher Point, Badlands"] = {
		[HORDE] = 632,
	},
	["Bogpaddle, Swamp of Sorrows"] = {
		[NEUTRAL] = 599,
	},
	["Bootlegger Outpost, Tanaris"] = {
		[NEUTRAL] = 539,
	},
	["Booty Bay, Stranglethorn"] = {
		[ALLIANCE] = 19,
		[HORDE] = 18,
	},
	["Bor'gorok Outpost, Borean Tundra"] = {
		[HORDE] = 259,
	},
	["Borean Tundra, Naglevar"] = {
		[NONE] = 224,
	},
	["Borean Tundra - Quest - Dusk - End"] = {
		[NONE] = 240,
	},
	["Borean Tundra - Quest - Dusk Start"] = {
		[NONE] = 239,
	},
	["Borean Tundra - Warsong Hold Wolf End"] = {
		[NONE] = 233,
	},
	["Borean Tundra - Warsong Hold Wolf Start"] = {
		[NONE] = 232,
	},
	["Bouldercrag's Refuge, The Storm Peaks"] = {
		[NEUTRAL] = 327,
	},
	["Bozzle's Wreck, Isle Of Giants"] = {
		[HORDE] = 1222,
	},
	["Brackenwall Village, Dustwallow Marsh"] = {
		[HORDE] = 55,
	},
	["Brill, Tirisfal Glades"] = {
		[HORDE] = 460,
	},
	["CC Prologue - GT - Battle Flight - End"] = {
		[NONE] = 395,
	},
	["CC Prologue - GT - Battle Flight - Start"] = {
		[NONE] = 394,
	},
	["CC Prologue - GT - Quest - Vent Horizon - End"] = {
		[NONE] = 393,
	},
	["CC Prologue - GT - Quest - Vent Horizon - Start"] = {
		[NONE] = 392,
	},
	["Camp Ataya, Feralas"] = {
		[HORDE] = 568,
	},
	["Camp Everstill, Redridge"] = {
		[ALLIANCE] = 615,
	},
	["Camp Mojache, Feralas"] = {
		[HORDE] = 42,
	},
	["Camp Onequah, Grizzly Hills (Quest)"] = {
		[NONE] = 311,
	},
	["Camp Oneqwah, Grizzly Hills"] = {
		[HORDE] = 249,
	},
	["Camp Tunka'lo, The Storm Peaks"] = {
		[HORDE] = 324,
	},
	["Camp Winterhoof, Howling Fjord"] = {
		[HORDE] = 192,
	},
	["Cenarion Hold, Silithus"] = {
		[ALLIANCE] = 73,
		[HORDE] = 72,
	},
	["Chillwind Camp, Western Plaguelands"] = {
		[ALLIANCE] = 66,
	},
	["Chiselgrip, Burning Steppes"] = {
		[NEUTRAL] = 676,
	},
	["Cliffwalker Post, Stonetalon Mountains"] = {
		[HORDE] = 360,
	},
	["Coldarra, Keristrasza Landing"] = {
		[NONE] = 237,
	},
	["Coldarra, Keristrasza to Malygos"] = {
		[NONE] = 236,
	},
	["Coldarra Ledge, Coldarra"] = {
		[NONE] = 234,
	},
	["Conquest Hold, Grizzly Hills"] = {
		[HORDE] = 250,
	},
	["Cosmowrench, Netherstorm"] = {
		[NEUTRAL] = 150,
	},
	["Cradle of Chi-Ji, Krasarang Wilds"] = {
		[NEUTRAL] = 992,
	},
	["Crown Guard Tower, Eastern Plaguelands"] = {
		[NEUTRAL] = 87,
	},
	["Crusaders' Pinnacle, Icecrown"] = {
		[NEUTRAL] = 335,
	},
	["Crushblow, Twilight Highlands"] = {
		[HORDE] = 656,
	},
	["Dalaran"] = {
		[NEUTRAL] = 310,
	},
	["Darkbreak Cove, Vashj'ir"] = {
		[ALLIANCE] = 524,
	},
	["Darkshire, Duskwood"] = {
		[ALLIANCE] = 12,
	},
	["Darnassus, Teldrassil"] = {
		[ALLIANCE] = 457,
	},
	["Dawn's Blossom, Jade Forest"] = {
		[NEUTRAL] = 895,
	},
	["Dawnchaser Retreat, Krasarang Wilds"] = {
		[HORDE] = 990,
	},
	["Dawnrise Expedition, Tanaris"] = {
		[HORDE] = 531,
	},
	["Death's Rise, Icecrown"] = {
		[NEUTRAL] = 325,
	},
	["Desolation Hold, Southern Barrens"] = {
		[HORDE] = 391,
	},
	["Dolanaar, Teldrassil"] = {
		[ALLIANCE] = 456,
	},
	["Domination Point, Krasarang Wilds"] = {
		[HORDE] = 1195,
	},
	["Dragon's Mouth, Badlands"] = {
		[ALLIANCE] = 634,
	},
	["Dragon Soul Raid - East Sarlac"] = {
		[NONE] = 907,
	},
	["Dragon Soul Raid - Wyrmrest Temple Base"] = {
		[NONE] = 906,
	},
	["Dragonmaw Port, Twilight Highlands"] = {
		[HORDE] = 661,
	},
	["Dreadmaul Hold, Blasted Lands"] = {
		[NONE] = 604,
	},
	["Dreamer's Rest, Feralas"] = {
		[ALLIANCE] = 565,
	},
	["Dun Baldar, Alterac Valley"] = {
		[NONE] = 59,
	},
	["Dun Modr, Wetlands"] = {
		[ALLIANCE] = 553,
	},
	["Dun Niffelem, The Storm Peaks"] = {
		[NEUTRAL] = 322,
	},
	["Durotar - ET - CC Prologue - Troll Battle End"] = {
		[NONE] = 440,
	},
	["Durotar - ET - CC Prologue Spy Frog End"] = {
		[NONE] = 405,
	},
	["Durotar - ET - CC Prologue Spy Frog Start"] = {
		[NONE] = 404,
	},
	["Durotar - ET - CC Prologue Troll Recruit End"] = {
		[NONE] = 439,
	},
	["Durotar - ET - CC Prologue Troll Taxi Bat Start"] = {
		[NONE] = 438,
	},
	["Dustwind Dig, Badlands"] = {
		[ALLIANCE] = 633,
	},
	["Eastpoint Tower, Hillsbrad"] = {
		[HORDE] = 669,
	},
	["Eastvale Logging Camp, Elwynn"] = {
		[ALLIANCE] = 589,
	},
	["Eastwall Tower, Eastern Plaguelands"] = {
		[NEUTRAL] = 86,
	},
	["Eastwind Rest, Kun-Lai Summit"] = {
		[HORDE] = 1019,
	},
	["Ebon Hold - Acherus -> Death's Breach End"] = {
		[NONE] = 317,
	},
	["Ebon Hold - Acherus -> Death's Breach Start"] = {
		[NONE] = 316,
	},
	["Ebon Hold - Death's Breach -> Acherus End"] = {
		[NONE] = 319,
	},
	["Ebon Hold - Death's Breach -> Acherus Start"] = {
		[NONE] = 318,
	},
	["Ebon Watch, Zul'Drak"] = {
		[NONE] = 305,
	},
	["Emerald Sanctuary, Felwood"] = {
		[NEUTRAL] = 166,
	},
	["Emperor's Omen, Jade Forest"] = {
		[NEUTRAL] = 970,
	},
	["Ethel Rethor, Desolace"] = {
		[NEUTRAL] = 370,
	},
	["Evergrove, Blade's Edge Mountains"] = {
		[NEUTRAL] = 160,
	},
	["Everlook, Winterspring"] = {
		[ALLIANCE] = 52,
		[HORDE] = 53,
	},
	["Eversong - Duskwither Teleport"] = {
		[NONE] = 111,
	},
	["Eversong - Duskwither Teleport End"] = {
		[NONE] = 112,
	},
	["Explorers' League Digsite, Stranglethorn"] = {
		[ALLIANCE] = 591,
	},
	["Fairbreeze Village, Eversong Woods"] = {
		[HORDE] = 625,
	},
	["Falcon Watch, Hellfire Peninsula"] = {
		[HORDE] = 102,
	},
	["Falconwing Square, Eversong Woods"] = {
		[HORDE] = 631,
	},
	["Farstrider Lodge, Loch Modan"] = {
		[ALLIANCE] = 555,
	},
	["Farwatcher's Glen, Stonetalon Mountains"] = {
		[ALLIANCE] = 365,
	},
	["Feathermoon, Feralas"] = {
		[ALLIANCE] = 41,
	},
	["Filming"] = {
		[NONE] = 168,
	},
	["Firebeard's Patrol, Twilight Highlands"] = {
		[ALLIANCE] = 664,
	},
	["Fizzcrank Airstrip, Borean Tundra"] = {
		[ALLIANCE] = 246,
	},
	["Fizzle & Pozzik's Speedbarge, Thousand Needles"] = {
		[NEUTRAL] = 513,
	},
	["Flame Crest, Burning Steppes"] = {
		[HORDE] = 70,
	},
	["Flamestar Post, Burning Steppes"] = {
		[NEUTRAL] = 675,
	},
	["Flavor - Stormwind Harbor  - Start"] = {
		[NONE] = 292,
	},
	["Flavor - Stormwind Harbor - Stop"] = {
		[NONE] = 293,
	},
	["Fordragon Hold, Dragonblight"] = {
		[ALLIANCE] = 251,
	},
	["Forest Song, Ashenvale"] = {
		[ALLIANCE] = 167,
	},
	["Forsaken Forward Command, Gilneas"] = {
		[HORDE] = 646,
	},
	["Forsaken High Command, Silverpine Forest"] = {
		[HORDE] = 645,
	},
	["Forsaken Rear Guard, Silverpine Forest"] = {
		[HORDE] = 681,
	},
	["Fort Livingston, Stranglethorn"] = {
		[ALLIANCE] = 590,
	},
	["Fort Triumph, Southern Barrens"] = {
		[ALLIANCE] = 389,
	},
	["Fort Wildervar, Howling Fjord"] = {
		[ALLIANCE] = 184,
	},
	["Frosthold, The Storm Peaks"] = {
		[ALLIANCE] = 321,
	},
	["Frostwolf Keep, Alterac Valley"] = {
		[NONE] = 60,
	},
	["Furien's Post, Desolace"] = {
		[HORDE] = 366,
	},
	["Furlbrow's Pumpkin Farm, Westfall"] = {
		[ALLIANCE] = 584,
	},
	["Fuselight, Badlands"] = {
		[NEUTRAL] = 635,
	},
	["Gadgetzan, Tanaris"] = {
		[ALLIANCE] = 39,
		[HORDE] = 40,
	},
	["Galen's Fall, Arathi"] = {
		[NONE] = 601,
	},
	["Gao-Ran Battlefront, Townlong Steppes"] = {
		[NEUTRAL] = 1054,
	},
	["Garadar, Nagrand"] = {
		[HORDE] = 120,
	},
	["Gates of Sothann, Hyjal"] = {
		[NEUTRAL] = 616,
	},
	["Generic, World Target 001"] = {
		[NONE] = 36,
	},
	["Generic, World Target 002"] = {
		[NONE] = 24,
	},
	["Gol'Bolar Quarry, Dun Morogh"] = {
		[ALLIANCE] = 620,
	},
	["Goldshire, Elwynn"] = {
		[ALLIANCE] = 582,
	},
	["Grassy Cline, Valley of the Four Winds"] = {
		[NEUTRAL] = 1052,
	},
	["Greenwarden's Grove, Wetlands"] = {
		[ALLIANCE] = 552,
	},
	["Grizzly Hills, Alliance Log Ride End"] = {
		[NONE] = 268,
	},
	["Grizzly Hills, Alliance Log Ride End 01"] = {
		[NONE] = 263,
	},
	["Grizzly Hills, Alliance Log Ride Start"] = {
		[NONE] = 267,
	},
	["Grizzly Hills, Alliance Log Ride Start 01"] = {
		[NONE] = 262,
	},
	["Grizzly Hills, Horde Log Ride End"] = {
		[NONE] = 272,
	},
	["Grizzly Hills, Horde Log Ride Start"] = {
		[NONE] = 271,
	},
	["Grom'arsh Crash-Site, The Storm Peaks"] = {
		[HORDE] = 323,
	},
	["Grom'gol, Stranglethorn"] = {
		[HORDE] = 20,
	},
	["Grookin Hill, Jade Forest"] = {
		[NONE] = 894,
	},
	["Grove of Aessina, Hyjal"] = {
		[NEUTRAL] = 558,
	},
	["Grove of the Ancients, Darkshore"] = {
		[ALLIANCE] = 339,
	},
	["Gundrak, Zul'Drak"] = {
		[NONE] = 331,
	},
	["Gunstan's Dig, Tanaris"] = {
		[ALLIANCE] = 532,
	},
	["Halfhill, Valley of the Four Winds"] = {
		[NEUTRAL] = 985,
	},
	["Hammerfall, Arathi"] = {
		[HORDE] = 17,
	},
	["Hardwrench Hideaway, Stranglethorn"] = {
		[HORDE] = 592,
	},
	["Hearthglen, Western Plaguelands"] = {
		[NEUTRAL] = 672,
	},
	["Hellfire Peninsula, The Dark Portal"] = {
		[ALLIANCE] = 129,
		[HORDE] = 130,
	},
	["Hellfire Peninsula - Force Camp Beach Head"] = {
		[NONE] = 147,
	},
	["Hellfire Peninsula - Reaver's Fall"] = {
		[NONE] = 142,
	},
	["Hellscream's Watch, Ashenvale"] = {
		[HORDE] = 350,
	},
	["Highbank, Twilight Highlands"] = {
		[ALLIANCE] = 662,
	},
	["Hiri'watha Research Station, The Hinterlands"] = {
		[HORDE] = 617,
	},
	["Honeydew Village, Jade Forest"] = {
		[HORDE] = 973,
	},
	["Honor's Stand, Southern Barrens"] = {
		[ALLIANCE] = 387,
	},
	["Honor Hold, Hellfire Peninsula"] = {
		[ALLIANCE] = 100,
	},
	["Hunter's Hill, Southern Barrens"] = {
		[HORDE] = 390,
	},
	["Huojin Landing, Timeless Isle"] = {
		[HORDE] = 1294,
	},
	["Iron Summit, Searing Gorge"] = {
		[NEUTRAL] = 673,
	},
	["Ironforge, Dun Morogh"] = {
		[ALLIANCE] = 6,
	},
	["Irontree Clearing, Felwood"] = {
		[HORDE] = 597,
	},
	["Jade Temple Grounds, Jade Forest"] = {
		[NEUTRAL] = 968,
	},
	["Justin Test Node 1"] = {
		[NONE] = 586,
	},
	["Justin Test Node 2"] = {
		[NONE] = 587,
	},
	["K3, The Storm Peaks"] = {
		[NEUTRAL] = 320,
	},
	["Kamagua, Howling Fjord"] = {
		[NEUTRAL] = 295,
	},
	["Karnum's Glade, Desolace"] = {
		[NEUTRAL] = 368,
	},
	["Kharanos, Dun Morogh"] = {
		[ALLIANCE] = 619,
	},
	["Kirthaven, Twilight Highlands"] = {
		[ALLIANCE] = 666,
	},
	["Klaxxi'vess, Dread Wastes"] = {
		[NEUTRAL] = 1070,
	},
	["Kor'kron Vanguard, Dragonblight"] = {
		[HORDE] = 260,
	},
	["Kota Basecamp, Kun-Lai Summit"] = {
		[NEUTRAL] = 1023,
	},
	["Krom'gar Fortress, Stonetalon Mountains"] = {
		[HORDE] = 362,
	},
	["Lakeshire, Redridge"] = {
		[ALLIANCE] = 5,
	},
	["Legion's Rest, Vashj'ir"] = {
		[HORDE] = 525,
	},
	["Light's Breach, Zul'Drak"] = {
		[NEUTRAL] = 306,
	},
	["Light's Breach, Zul'Drak (Quest)"] = {
		[NONE] = 312,
	},
	["Light's Hope Chapel, Eastern Plaguelands"] = {
		[ALLIANCE] = 67,
		[HORDE] = 68,
	},
	["Light's Shield Tower, Eastern Plaguelands"] = {
		[NEUTRAL] = 630,
	},
	["Lion's Landing, Krasarang Wilds"] = {
		[ALLIANCE] = 1190,
	},
	["Longying Outpost, Townlong Steppes"] = {
		[NEUTRAL] = 1053,
	},
	["Lor'danel, Darkshore"] = {
		[ALLIANCE] = 26,
	},
	["Malaka'jin, Stonetalon Mountains"] = {
		[HORDE] = 363,
	},
	["Marista, Krasarang Wilds"] = {
		[NEUTRAL] = 993,
	},
	["Marshal's Stand, Un'Goro Crater"] = {
		[NEUTRAL] = 79,
	},
	["Marshtide Watch, Swamp of Sorrows"] = {
		[ALLIANCE] = 598,
	},
	["Menethil Harbor, Wetlands"] = {
		[ALLIANCE] = 7,
	},
	["Mirkfallon Post, Stonetalon Mountains"] = {
		[ALLIANCE] = 541,
	},
	["Moa'ki, Dragonblight"] = {
		[NEUTRAL] = 294,
	},
	["Mok'Nathal Village, Blade's Edge Mountains"] = {
		[HORDE] = 163,
	},
	["Moonbrook, Westfall"] = {
		[ALLIANCE] = 583,
	},
	["Moonglade"] = {
		[ALLIANCE] = 49,
		[HORDE] = 69,
	},
	["Morgan's Vigil, Burning Steppes"] = {
		[ALLIANCE] = 71,
	},
	["Mossy Pile, Un'Goro Crater"] = {
		[NEUTRAL] = 386,
	},
	["Mudsprocket, Dustwallow Marsh"] = {
		[NEUTRAL] = 179,
	},
	["Mulgore - Red Cloud Mesa: To Bloodhoof"] = {
		[NONE] = 373,
	},
	["Mulgore - Red Cloud Mesa: To Bloodhoof (End)"] = {
		[NONE] = 374,
	},
	["Nagrand - PvP - Attack Run End 1"] = {
		[NONE] = 104,
	},
	["Nagrand - PvP - Attack Run End 2"] = {
		[NONE] = 106,
	},
	["Nagrand - PvP - Attack Run End 3"] = {
		[NONE] = 108,
	},
	["Nagrand - PvP - Attack Run End 4"] = {
		[NONE] = 110,
	},
	["Nagrand - PvP - Attack Run Start 1 "] = {
		[NONE] = 103,
	},
	["Nagrand - PvP - Attack Run Start 2"] = {
		[NONE] = 105,
	},
	["Nagrand - PvP - Attack Run Start 3"] = {
		[NONE] = 107,
	},
	["Nagrand - PvP - Attack Run Start 4"] = {
		[NONE] = 109,
	},
	["Naxxramas"] = {
		[NONE] = 78,
	},
	["Nesingwary Base Camp, Sholazar Basin"] = {
		[NEUTRAL] = 309,
	},
	["Nethergarde Keep, Blasted Lands"] = {
		[ALLIANCE] = 45,
	},
	["New Agamand, Howling Fjord"] = {
		[HORDE] = 190,
	},
	["New Kargath, Badlands"] = {
		[HORDE] = 21,
	},
	["Nighthaven, Moonglade"] = {
		[ALLIANCE] = 62,
		[HORDE] = 63,
	},
	["Nijel's Point, Desolace"] = {
		[ALLIANCE] = 37,
	},
	["Nordrassil, Hyjal"] = {
		[NEUTRAL] = 559,
	},
	["Northern Rocketway, Azshara"] = {
		[HORDE] = 614,
	},
	["Northpass Tower, Eastern Plaguelands"] = {
		[NEUTRAL] = 85,
	},
	["Northwatch Expedition Base Camp, Stonetalon Mountains"] = {
		[ALLIANCE] = 364,
	},
	["Northwatch Hold, Southern Barrens"] = {
		[ALLIANCE] = 388,
	},
	["Nozzlepot's Outpost, Northern Barrens"] = {
		[HORDE] = 458,
	},
	["Oasis of Vir'sar, Uldum"] = {
		[NEUTRAL] = 653,
	},
	["Ogri'La"] = {
		[NONE] = 172,
	},
	["One Keg, Kun-Lai Summit"] = {
		[NEUTRAL] = 1022,
	},
	["Orebor Harborage, Zangarmarsh"] = {
		[ALLIANCE] = 164,
	},
	["Orgrimmar, Durotar"] = {
		[HORDE] = 23,
	},
	["Pang's Stead, Valley of the Four Winds"] = {
		[NONE] = 984,
	},
	["Paw'Don Village, Jade Forest"] = {
		[ALLIANCE] = 966,
	},
	["Pearlfin Village, Jade Forest"] = {
		[ALLIANCE] = 972,
	},
	["Plaguewood Tower, Eastern Plaguelands"] = {
		[NEUTRAL] = 84,
	},
	["Quest - \"To New Thalanaar\" - Mudsprocket -> New Thalanaar - Begin"] = {
		[NONE] = 433,
	},
	["Quest - \"To New Thalanaar\" - New Thalanaar - End"] = {
		[NONE] = 434,
	},
	["Quest - \"To New Thalanaar\" - Shadebough -> New Thalanaar - Begin"] = {
		[NONE] = 436,
	},
	["Quest - \"To The Summit\" - Camp Mojache -> Westreach Summit - Begin"] = {
		[NONE] = 414,
	},
	["Quest - \"To The Summit\" - Westreach Summit - End"] = {
		[NONE] = 413,
	},
	["Quest - Azuremyst Shaman Start"] = {
		[NONE] = 92,
	},
	["Quest - Azuremyst Shaman Target"] = {
		[NONE] = 91,
	},
	["Quest - Blade's Edge - Vision Guide - End"] = {
		[NONE] = 158,
	},
	["Quest - Blade's Edge - Vision Guide - Start"] = {
		[NONE] = 157,
	},
	["Quest - Borean Tundra - Check In With Bixie - Begin"] = {
		[NONE] = 301,
	},
	["Quest - Borean Tundra - Check In With Bixie - End"] = {
		[NONE] = 302,
	},
	["Quest - Caverns of Time (Intro Flight Path) (End)"] = {
		[NONE] = 143,
	},
	["Quest - Caverns of Time (Intro Flight Path) (Start)"] = {
		[NONE] = 144,
	},
	["Quest - Caverns of Time OH - Begin"] = {
		[NONE] = 115,
	},
	["Quest - Caverns of Time OH - End"] = {
		[NONE] = 116,
	},
	["Quest - Dragonblight - Spiritual Vision - Begin"] = {
		[NONE] = 242,
	},
	["Quest - Dragonblight - Spiritual Vision - End"] = {
		[NONE] = 243,
	},
	["Quest - Dustwallow - Alcaz Survey End"] = {
		[NONE] = 181,
	},
	["Quest - Dustwallow - Alcaz Survey Start"] = {
		[NONE] = 180,
	},
	["Quest - Elekk Path to Kessel"] = {
		[NONE] = 97,
	},
	["Quest - Elekk to Kessel Target"] = {
		[NONE] = 98,
	},
	["Quest - Hellfire, Aerial Mission (Alliance) End"] = {
		[NONE] = 138,
	},
	["Quest - Hellfire, Aerial Mission (Alliance) Start"] = {
		[NONE] = 137,
	},
	["Quest - Hellfire, Aerial Mission (Horde) End"] = {
		[NONE] = 136,
	},
	["Quest - Hellfire, Aerial Mission (Horde) Start"] = {
		[NONE] = 135,
	},
	["Quest - Hellfire Peninsula (Alliance) End"] = {
		[NONE] = 134,
	},
	["Quest - Hellfire Peninsula (Alliance Path) Start"] = {
		[NONE] = 133,
	},
	["Quest - Horde Hellfire End"] = {
		[NONE] = 132,
	},
	["Quest - Horde Hellfire Start"] = {
		[NONE] = 131,
	},
	["Quest - Howling Fjord - Flight to the Windrunner - End"] = {
		[NONE] = 187,
	},
	["Quest - Howling Fjord - Flight to the Windrunner - Start"] = {
		[NONE] = 186,
	},
	["Quest - Howling Fjord - McGoyver End"] = {
		[NONE] = 200,
	},
	["Quest - Howling Fjord - McGoyver Start"] = {
		[NONE] = 199,
	},
	["Quest - Howling Fjord - Mission: Plague This! - End"] = {
		[NONE] = 193,
	},
	["Quest - Howling Fjord - Mission: Plague This! - Start"] = {
		[NONE] = 194,
	},
	["Quest - Howling Fjord - Naglfar (Nodes)"] = {
		[NONE] = 215,
	},
	["Quest - Howling Fjord - Naglfar (Start)"] = {
		[NONE] = 214,
	},
	["Quest - Howling Fjord - Test at Sea - End"] = {
		[NONE] = 189,
	},
	["Quest - Howling Fjord - Test at Sea - Start"] = {
		[NONE] = 188,
	},
	["Quest - Howling Fjord Tauren Canoe (End)"] = {
		[NONE] = 177,
	},
	["Quest - Howling Fjord Tauren Canoe (Start)"] = {
		[NONE] = 176,
	},
	["Quest - Icecrown - North Sea Kraken Bombing - End"] = {
		[NONE] = 359,
	},
	["Quest - Icecrown - North Sea Kraken Bombing - Start"] = {
		[NONE] = 358,
	},
	["Quest - Nether Dragon Ally - End"] = {
		[NONE] = 162,
	},
	["Quest - Nether Dragon Ally - Start"] = {
		[NONE] = 161,
	},
	["Quest - Netherstorm - Manaforge Ultris (End)"] = {
		[NONE] = 153,
	},
	["Quest - Netherstorm - Manaforge Ultris (Second Pass) End"] = {
		[NONE] = 155,
	},
	["Quest - Netherstorm - Manaforge Ultris (Second Pass) Start"] = {
		[NONE] = 154,
	},
	["Quest - Netherstorm - Manaforge Ultris (Start)"] = {
		[NONE] = 152,
	},
	["Quest - Netherstorm - Stealth Flight - Begin"] = {
		[NONE] = 145,
	},
	["Quest - Netherstorm - Stealth Flight - End"] = {
		[NONE] = 146,
	},
	["Quest - Netherwing Ledge - Mine Cart Ride - South - End"] = {
		[NONE] = 170,
	},
	["Quest - Netherwing Ledge - Mine Cart Ride - South - Start"] = {
		[NONE] = 169,
	},
	["Quest - Nethrandamus End Target"] = {
		[NONE] = 114,
	},
	["Quest - Nethrandamus Start"] = {
		[NONE] = 113,
	},
	["Quest - New Agamand -> Venomspite"] = {
		[NONE] = 204,
	},
	["Quest - Splintertree  Post -> Zoramgar"] = {
		[NONE] = 344,
	},
	["Quest - Stars' Rest -> Wintergarde"] = {
		[NONE] = 203,
	},
	["Quest - Stars' Rest to Wintergarde End"] = {
		[NONE] = 261,
	},
	["Quest - Sunwell Daily - Dead Scar Bombing - End"] = {
		[NONE] = 210,
	},
	["Quest - Sunwell Daily - Dead Scar Bombing - Start"] = {
		[NONE] = 209,
	},
	["Quest - Sunwell Daily - Ship Bombing - End"] = {
		[NONE] = 212,
	},
	["Quest - Sunwell Daily - Ship Bombing - Start"] = {
		[NONE] = 211,
	},
	["Quest - Valgarde -> Westguard Keep End"] = {
		[NONE] = 288,
	},
	["Quest - Valgarde -> Westguard Keep Start"] = {
		[NONE] = 287,
	},
	["Quest - Westguard Keep to Wintergarde Keep Begin"] = {
		[NONE] = 269,
	},
	["Quest - Westguard Keep to Wintergarde Keep End"] = {
		[NONE] = 270,
	},
	["Quest - Wintergarde -> Stars' Rest (End)"] = {
		[NONE] = 286,
	},
	["Quest - Wintergarde -> Stars' Rest (Start)"] = {
		[NONE] = 285,
	},
	["Quest - Yarzill Flight Start"] = {
		[NONE] = 173,
	},
	["Quest - Yarzill Flight Start End"] = {
		[NONE] = 174,
	},
	["Quest - Zoramgar, Ashenvale"] = {
		[NONE] = 345,
	},
	["Ramkahen, Uldum"] = {
		[NEUTRAL] = 652,
	},
	["Ratchet, Northern Barrens"] = {
		[NEUTRAL] = 80,
	},
	["Raven Hill, Duskwood"] = {
		[ALLIANCE] = 622,
	},
	["Razor Hill, Durotar"] = {
		[HORDE] = 537,
	},
	["Rebel Camp, Stranglethorn Vale"] = {
		[NONE] = 195,
	},
	["Refuge Pointe, Arathi"] = {
		[ALLIANCE] = 16,
	},
	["Rensai's Watchpost, Townlong Steppes"] = {
		[NEUTRAL] = 1055,
	},
	["Revantusk Village, The Hinterlands"] = {
		[HORDE] = 76,
	},
	["River's Heart, Sholazar Basin"] = {
		[NEUTRAL] = 308,
	},
	["Ruined City Post 01, Zul'Drak"] = {
		[NONE] = 291,
	},
	["Ruins of Southshore, Hillsbrad"] = {
		[HORDE] = 667,
	},
	["Rut'theran Village, Teldrassil"] = {
		[ALLIANCE] = 27,
	},
	["Sanctuary of Malorne, Hyjal"] = {
		[NEUTRAL] = 781,
	},
	["Sanctum of the Stars, Shadowmoon Valley"] = {
		[NEUTRAL] = 159,
	},
	["Sandy Beach, Vashj'ir"] = {
		[ALLIANCE] = 607,
		[HORDE] = 608,
	},
	["Schnottz's Landing, Uldum"] = {
		[NEUTRAL] = 674,
	},
	["Sen'jin Village, Durotar"] = {
		[HORDE] = 536,
	},
	["Sentinel Basecamp, Krasarang Wilds"] = {
		[NONE] = 991,
	},
	["Sentinel Hill, Westfall"] = {
		[ALLIANCE] = 4,
	},
	["Serpent's Overlook, Jade Forest"] = {
		[NEUTRAL] = 1080,
	},
	["Serpent's Spine, Kun-Lai Summit"] = {
		[HORDE] = 1117,
	},
	["Serpent's Spine, Vale of Eternal Blossoms"] = {
		[NEUTRAL] = 1073,
	},
	["Shadebough, Feralas"] = {
		[ALLIANCE] = 31,
	},
	["Shado-Pan Fallback, Kun-Lai Summit"] = {
		[NEUTRAL] = 1024,
	},
	["Shado-Pan Garrison, Townlong Steppes"] = {
		[NEUTRAL] = 1056,
	},
	["Shadowmoon Village, Shadowmoon Valley"] = {
		[HORDE] = 123,
	},
	["Shadowprey Village, Desolace"] = {
		[HORDE] = 38,
	},
	["Shalewind Canyon, Redridge"] = {
		[ALLIANCE] = 596,
	},
	["Shatter Point, Hellfire Peninsula"] = {
		[NONE] = 149,
	},
	["Shatter Point, Hellfire Peninsula (Beach Assault)"] = {
		[NONE] = 148,
	},
	["Shattered Sun Staging Area"] = {
		[NEUTRAL] = 213,
	},
	["Shattrath, Terokkar Forest"] = {
		[NEUTRAL] = 128,
	},
	["Sholazar Basin - Quest - Reconnaisance - End"] = {
		[NONE] = 298,
	},
	["Sholazar Basin - Quest - Reconnaisance - Start"] = {
		[NONE] = 297,
	},
	["Shrine of Aviana, Hyjal"] = {
		[NEUTRAL] = 557,
	},
	["Shrine of Seven Stars, Vale of Eternal Blossoms"] = {
		[ALLIANCE] = 1057,
	},
	["Shrine of Two Moons, Vale of Eternal Blossoms"] = {
		[HORDE] = 1058,
	},
	["Silver Tide Hollow, Vashj'ir"] = {
		[NEUTRAL] = 522,
	},
	["Silvermoon City"] = {
		[NONE] = 82,
	},
	["Silverwind Refuge, Ashenvale"] = {
		[HORDE] = 356,
	},
	["Skettis"] = {
		[NONE] = 171,
	},
	["Slabchisel's Survey, Wetlands"] = {
		[ALLIANCE] = 554,
	},
	["Smuggler's Scar, Vashj'ir"] = {
		[NEUTRAL] = 521,
	},
	["Soggy's Gamble, Dread Wastes"] = {
		[NEUTRAL] = 1071,
	},
	["Southern Rocketway, Azshara"] = {
		[HORDE] = 613,
	},
	["Southpoint Gate, Hillsbrad"] = {
		[HORDE] = 668,
	},
	["Southshore, Hillsbrad"] = {
		[NONE] = 14,
	},
	["Spinebreaker Ridge, Hellfire Peninsula"] = {
		[HORDE] = 141,
	},
	["Splintertree Post, Ashenvale"] = {
		[HORDE] = 61,
	},
	["Sri-La Village, Jade Forest"] = {
		[NEUTRAL] = 969,
	},
	["Stardust Spire, Ashenvale"] = {
		[ALLIANCE] = 351,
	},
	["Stars' Rest, Dragonblight"] = {
		[ALLIANCE] = 247,
	},
	["Stonard, Swamp of Sorrows"] = {
		[HORDE] = 56,
	},
	["Stonebreaker Hold, Terokkar Forest"] = {
		[HORDE] = 127,
	},
	["Stonemaul Hold, Feralas"] = {
		[HORDE] = 569,
	},
	["Stoneplow, Valley of the Four Winds"] = {
		[NEUTRAL] = 989,
	},
	["Stormfeather Outpost, The Hinterlands"] = {
		[ALLIANCE] = 618,
	},
	["Stormwind, Elwynn"] = {
		[ALLIANCE] = 2,
	},
	["Strahnbrad, Alterac Mountains"] = {
		[HORDE] = 670,
	},
	["Stygian Bounty, Vashj'ir"] = {
		[HORDE] = 612,
	},
	["Sun Rock Retreat, Stonetalon Mountains"] = {
		[HORDE] = 29,
	},
	["Sunreaver's Command, Crystalsong Forest"] = {
		[HORDE] = 337,
	},
	["Sunveil Excursion, Blasted Lands"] = {
		[HORDE] = 603,
	},
	["Surwich, Blasted Lands"] = {
		[ALLIANCE] = 602,
	},
	["Swamprat Post, Zangarmarsh"] = {
		[HORDE] = 151,
	},
	["Sylvanaar, Blade's Edge Mountains"] = {
		[ALLIANCE] = 125,
	},
	["Talonbranch Glade, Felwood"] = {
		[ALLIANCE] = 65,
	},
	["Talrendis Point, Azshara (NEVER in 4.x)"] = {
		[NONE] = 64,
	},
	["Tarren Mill, Hillsbrad"] = {
		[HORDE] = 13,
	},
	["Taunka'le Village, Borean Tundra"] = {
		[HORDE] = 258,
	},
	["Tavern in the Mists, The Veiled Stair"] = {
		[NEUTRAL] = 1029,
	},
	["Telaar, Nagrand"] = {
		[ALLIANCE] = 119,
	},
	["Telredor, Zangarmarsh"] = {
		[ALLIANCE] = 117,
	},
	["Temple of Telhamat, Hellfire Peninsula"] = {
		[ALLIANCE] = 101,
	},
	["Temple of the White Tiger, Kun-Lai Summit"] = {
		[NEUTRAL] = 1018,
	},
	["Tenebrous Cavern, Vashj'ir"] = {
		[HORDE] = 526,
	},
	["Test - SP"] = {
		[NONE] = 196,
	},
	["Test SP1"] = {
		[NONE] = 198,
	},
	["Test SP2"] = {
		[NONE] = 197,
	},
	["Thal'darah Overlook, Stonetalon Mountains"] = {
		[ALLIANCE] = 33,
	},
	["Thargad's Camp, Desolace"] = {
		[ALLIANCE] = 367,
	},
	["The Arboretum, Jade Forest"] = {
		[NEUTRAL] = 967,
	},
	["The Argent Stand, Zul'Drak"] = {
		[NEUTRAL] = 304,
	},
	["The Argent Vanguard, Icecrown"] = {
		[NEUTRAL] = 334,
	},
	["The Briny Muck, Dread Wastes"] = {
		[NEUTRAL] = 1090,
	},
	["The Bulwark, Tirisfal"] = {
		[HORDE] = 384,
	},
	["The Crossroads, Northern Barrens"] = {
		[HORDE] = 25,
	},
	["The Exodar"] = {
		[ALLIANCE] = 94,
	},
	["The Forsaken Front, Silverpine Forest"] = {
		[HORDE] = 654,
	},
	["The Gullet, Twilight Highlands"] = {
		[HORDE] = 657,
	},
	["The Harborage, Swamp of Sorrows"] = {
		[NONE] = 600,
	},
	["The Incursion, Krasarang Wilds"] = {
		[ALLIANCE] = 988,
	},
	["The Krazzworks, Twilight Highlands"] = {
		[HORDE] = 660,
	},
	["The Lion's Redoubt, Dread Wastes"] = {
		[ALLIANCE] = 1115,
	},
	["The Menders' Stead, Western Plaguelands"] = {
		[NEUTRAL] = 651,
	},
	["The Mor'Shan Ramparts, Ashenvale"] = {
		[HORDE] = 354,
	},
	["The Sepulcher, Silverpine Forest"] = {
		[HORDE] = 10,
	},
	["The Shadow Vault, Icecrown"] = {
		[NEUTRAL] = 333,
	},
	["The Sludgewerks, Stonetalon Mountains"] = {
		[HORDE] = 540,
	},
	["The Stormspire, Netherstorm"] = {
		[NEUTRAL] = 139,
	},
	["The Sunset Brewgarden, Dread Wastes"] = {
		[NEUTRAL] = 1072,
	},
	["Thelsamar, Loch Modan"] = {
		[ALLIANCE] = 8,
	},
	["Theramore, Dustwallow Marsh"] = {
		[NONE] = 32,
	},
	["Thondroril River, Eastern Plaguelands"] = {
		[NEUTRAL] = 383,
	},
	["Thorium Point, Searing Gorge"] = {
		[ALLIANCE] = 74,
		[HORDE] = 75,
	},
	["Thrallmar, Hellfire Peninsula"] = {
		[HORDE] = 99,
	},
	["Thunder Bluff, Mulgore"] = {
		[HORDE] = 22,
	},
	["Thunder Cleft, Krasarang Wilds"] = {
		[NONE] = 987,
	},
	["Thunderlord Stronghold, Blade's Edge Mountains"] = {
		[HORDE] = 126,
	},
	["Thundermar, Twilight Highlands"] = {
		[ALLIANCE] = 665,
	},
	["Thunk's Abode, Desolace"] = {
		[NEUTRAL] = 369,
	},
	["Tian Monastery, Jade Forest"] = {
		[NEUTRAL] = 971,
	},
	["Toshley's Station, Blade's Edge Mountains"] = {
		[ALLIANCE] = 156,
	},
	["Tower of Estulan, Feralas"] = {
		[ALLIANCE] = 567,
	},
	["Tranquil Wash, Vashj'ir"] = {
		[ALLIANCE] = 523,
	},
	["Tranquillien, Ghostlands"] = {
		[HORDE] = 83,
	},
	["Transitus Shield, Coldarra"] = {
		[NEUTRAL] = 226,
	},
	["Transitus Shield, Coldarra (NOT USED)"] = {
		[NONE] = 235,
	},
	["Transport, Auberdine"] = {
		[NONE] = 51,
	},
	["Transport, Borean Tundra"] = {
		[NONE] = 178,
	},
	["Transport, Feathermoon"] = {
		[NONE] = 54,
	},
	["Transport, Howling Fjord"] = {
		[NONE] = 175,
	},
	["Transport, Menethil Harbor"] = {
		[NONE] = 50,
	},
	["Transport, Northrend 2"] = {
		[NONE] = 165,
	},
	["Transport, Orgrimmar"] = {
		[NONE] = 35,
	},
	["Transport, Southshore"] = {
		[NONE] = 46,
	},
	["Transport, Stormwind"] = {
		[NONE] = 300,
	},
	["Transport, Valiance Keep"] = {
		[NONE] = 241,
	},
	["Tushui Landing, Timeless Isle"] = {
		[ALLIANCE] = 1293,
	},
	["Ulduar, The Storm Peaks"] = {
		[NEUTRAL] = 326,
	},
	["Ulduar Raid - Interior - Insertion Point"] = {
		[NONE] = 341,
	},
	["Ulduar Raid - Iron Concourse"] = {
		[NONE] = 342,
	},
	["Undercity, Tirisfal"] = {
		[HORDE] = 11,
	},
	["Unu'pe, Borean Tundra"] = {
		[NEUTRAL] = 296,
	},
	["Valgarde Port, Howling Fjord"] = {
		[ALLIANCE] = 183,
	},
	["Valiance Keep, Borean Tundra"] = {
		[ALLIANCE] = 245,
	},
	["Valiance Landing Camp, Wintergrasp"] = {
		[ALLIANCE] = 303,
	},
	["Valormok, Azshara"] = {
		[NONE] = 683,
	},
	["Vendetta Point, Southern Barrens"] = {
		[HORDE] = 77,
	},
	["Vengeance Landing, Howling Fjord"] = {
		[HORDE] = 191,
	},
	["Venomspite, Dragonblight"] = {
		[HORDE] = 254,
	},
	["Vermillion Redoubt, Twilight Highlands"] = {
		[NEUTRAL] = 658,
	},
	["Victor's Point, Twilight Highlands"] = {
		[ALLIANCE] = 663,
	},
	["Voldrin's Hold, Vashj'ir"] = {
		[ALLIANCE] = 611,
	},
	["Warsong Camp, Wintergrasp"] = {
		[HORDE] = 332,
	},
	["Warsong Hold, Borean Tundra"] = {
		[HORDE] = 257,
	},
	["Westfall Brigade, Grizzly Hills"] = {
		[ALLIANCE] = 255,
	},
	["Westfall Brigade, Grizzly Hills (Quest)"] = {
		[NONE] = 313,
	},
	["Westguard Keep, Howling Fjord"] = {
		[ALLIANCE] = 185,
	},
	["Westreach Summit, Thousand Needles"] = {
		[HORDE] = 30,
	},
	["Westwind Rest, Kun-Lai Summit"] = {
		[ALLIANCE] = 1020,
	},
	["Whelgar's Retreat, Wetlands"] = {
		[ALLIANCE] = 551,
	},
	["Whisperwind Grove, Felwood"] = {
		[NEUTRAL] = 594,
	},
	["Wildhammer Stronghold, Shadowmoon Valley"] = {
		[ALLIANCE] = 124,
	},
	["Wildheart Point, Felwood"] = {
		[NEUTRAL] = 595,
	},
	["Windrunner's Overlook, Crystalsong Forest"] = {
		[ALLIANCE] = 336,
	},
	["Windshear Hold, Stonetalon Mountains"] = {
		[ALLIANCE] = 361,
	},
	["Winter's Blossom, Kun-Lai Summit"] = {
		[NEUTRAL] = 1025,
	},
	["Wintergarde Keep, Dragonblight"] = {
		[ALLIANCE] = 244,
	},
	["Wyrmrest Temple, Dragonblight"] = {
		[NEUTRAL] = 252,
	},
	["Wyrmrest Temple - bottom to middle, Dragonblight - Begin"] = {
		[NONE] = 284,
	},
	["Wyrmrest Temple - bottom to middle, Dragonblight - End"] = {
		[NONE] = 283,
	},
	["Wyrmrest Temple - bottom to top, Dragonblight - Begin"] = {
		[NONE] = 273,
	},
	["Wyrmrest Temple - bottom to top, Dragonblight - End"] = {
		[NONE] = 274,
	},
	["Wyrmrest Temple - middle to bottom, Dragonblight - Begin"] = {
		[NONE] = 282,
	},
	["Wyrmrest Temple - middle to bottom, Dragonblight - End"] = {
		[NONE] = 281,
	},
	["Wyrmrest Temple - middle to top, Dragonblight - Begin"] = {
		[NONE] = 280,
	},
	["Wyrmrest Temple - middle to top, Dragonblight - End"] = {
		[NONE] = 279,
	},
	["Wyrmrest Temple - top to bottom, Dragonblight - Begin"] = {
		[NONE] = 275,
	},
	["Wyrmrest Temple - top to bottom, Dragonblight - End"] = {
		[NONE] = 276,
	},
	["Wyrmrest Temple - top to middle, Dragonblight - Begin"] = {
		[NONE] = 277,
	},
	["Wyrmrest Temple - top to middle, Dragonblight - End"] = {
		[NONE] = 278,
	},
	["Zabra'jin, Zangarmarsh"] = {
		[HORDE] = 118,
	},
	["Zangarmarsh - Quest - As the Crow Flies"] = {
		[NONE] = 95,
	},
	["Zangarmarsh - Quest - As the Crow Flies - End"] = {
		[NONE] = 96,
	},
	["Zhu's Watch, Krasarang Wilds"] = {
		[NEUTRAL] = 986,
	},
	["Zim'Torga, Zul'Drak"] = {
		[NEUTRAL] = 307,
	},
	["Zim'Torga, Zul'Drak (Quest)"] = {
		[NONE] = 314,
	},
	["Zoram'gar Outpost, Ashenvale"] = {
		[HORDE] = 58,
	},
	["Zouchin Village, Kun-Lai Summit"] = {
		[NEUTRAL] = 1021,
	},
	["Zul'Aman, Ghostlands"] = {
		[NEUTRAL] = 205,
	},
	["test - JZB - Orgrim's Hammer"] = {
		[NONE] = 538,
	},
}

ns.Taxi = {
	[2] = {
		[4] = {faction = ALLIANCE, distance = 2181.5300169227, cost = 110},
		[5] = {faction = ALLIANCE, distance = 3379.280695547, cost = 210},
		[6] = {faction = ALLIANCE, distance = 6502.3542981528, cost = 50},
		[12] = {faction = ALLIANCE, distance = 2935.9293659289, cost = 110},
		[19] = {faction = ALLIANCE, distance = 5950.9847464427, cost = 630},
		[45] = {faction = ALLIANCE, distance = 5320.5941372896, cost = 830},
		[71] = {faction = ALLIANCE, distance = 4482.7931201143, cost = 830},
		[74] = {faction = ALLIANCE, distance = 4102.0082315349, cost = 730},
		[195] = {faction = ALLIANCE, distance = 2765.0017871755, cost = 630},
		[582] = {faction = ALLIANCE, distance = 938.47844793106, cost = 0},
		[584] = {faction = ALLIANCE, distance = 1472.469672438, cost = 110},
	},
	[4] = {
		[2] = {faction = ALLIANCE, distance = 2561.5044471081, cost = 110},
		[5] = {faction = ALLIANCE, distance = 4009.3554851339, cost = 210},
		[12] = {faction = ALLIANCE, distance = 2731.6720084516, cost = 330},
		[19] = {faction = ALLIANCE, distance = 5406.8066371906, cost = 1260},
		[195] = {faction = ALLIANCE, distance = 1928.4223177099, cost = 630},
		[583] = {faction = ALLIANCE, distance = 716.79306204037, cost = 110},
		[584] = {faction = ALLIANCE, distance = 949.1944887811, cost = 110},
		[622] = {faction = ALLIANCE, distance = 897.08995971325, cost = 220},
	},
	[5] = {
		[2] = {faction = ALLIANCE, distance = 3363.2982454059, cost = 210},
		[4] = {faction = ALLIANCE, distance = 3886.1599925202, cost = 110},
		[12] = {faction = ALLIANCE, distance = 1838.3961593571, cost = 330},
		[71] = {faction = ALLIANCE, distance = 1673.3184076811, cost = 830},
		[589] = {faction = ALLIANCE, distance = 1146.4814343119, cost = 110},
		[600] = {faction = ALLIANCE, distance = 1401.1793588343, cost = 630},
		[615] = {faction = ALLIANCE, distance = 615.74742904364, cost = 330},
	},
	[6] = {
		[2] = {faction = ALLIANCE, distance = 6153.0401653498, cost = 1660},
		[7] = {faction = ALLIANCE, distance = 3519.1843219164, cost = 330},
		[8] = {faction = ALLIANCE, distance = 3069.5278685942, cost = 110},
		[14] = {faction = ALLIANCE, distance = 6495.910492517, cost = 330},
		[16] = {faction = ALLIANCE, distance = 6153.0118028856, cost = 530},
		[43] = {faction = ALLIANCE, distance = 7214.8131105186, cost = 730},
		[59] = {faction = ALLIANCE, distance = 9553.3067156152, cost = 1020},
		[66] = {faction = ALLIANCE, distance = 7854.7879942223, cost = 1020},
		[67] = {faction = ALLIANCE, distance = 10642.4170605, cost = 1020},
		[74] = {faction = ALLIANCE, distance = 2360.7482580774, cost = 830},
		[213] = {faction = ALLIANCE, distance = 24806.947511539, cost = 2775},
		[606] = {faction = ALLIANCE, distance = 5617.4024460127, cost = 10000},
		[619] = {faction = ALLIANCE, distance = 1358.1850637393, cost = 0},
		[620] = {faction = ALLIANCE, distance = 1672.7267239911, cost = 110},
		[658] = {faction = ALLIANCE, distance = 6913.4068503886, cost = 10000},
	},
	[7] = {
		[6] = {faction = ALLIANCE, distance = 2696.2058510682, cost = 330},
		[8] = {faction = ALLIANCE, distance = 4948.3457126977, cost = 110},
		[14] = {faction = ALLIANCE, distance = 3224.4350432739, cost = 330},
		[16] = {faction = ALLIANCE, distance = 3409.2078159232, cost = 530},
		[551] = {faction = ALLIANCE, distance = 1388.3136335125, cost = 330},
	},
	[8] = {
		[6] = {faction = ALLIANCE, distance = 3313.9411874315, cost = 110},
		[7] = {faction = ALLIANCE, distance = 4624.1055293776, cost = 330},
		[16] = {faction = ALLIANCE, distance = 4926.0702016534, cost = 530},
		[74] = {faction = ALLIANCE, distance = 2452.3775785875, cost = 830},
		[554] = {faction = ALLIANCE, distance = 1772.9906437087, cost = 330},
		[555] = {faction = ALLIANCE, distance = 1377.003300699, cost = 110},
		[620] = {faction = ALLIANCE, distance = 1609.0605135408, cost = 110},
		[633] = {faction = ALLIANCE, distance = 1371.0520121392, cost = 830},
		[662] = {faction = ALLIANCE, distance = 5409.787816163, cost = 30000},
	},
	[10] = {
		[11] = {faction = HORDE, distance = 3657.729915839, cost = 110},
		[13] = {faction = HORDE, distance = 2891.8759968713, cost = 330},
		[645] = {faction = HORDE, distance = 1595.730357548, cost = 110},
		[646] = {faction = HORDE, distance = 1632.2918020099, cost = 220},
		[654] = {faction = HORDE, distance = 739.58095587017, cost = 220},
		[681] = {faction = HORDE, distance = 1510.4047181435, cost = 110},
	},
	[11] = {
		[10] = {faction = HORDE, distance = 3010.6470771518, cost = 110},
		[13] = {faction = HORDE, distance = 4271.7161267285, cost = 330},
		[17] = {faction = HORDE, distance = 7837.1939718439, cost = 860},
		[21] = {faction = HORDE, distance = 14383.159717332, cost = 630},
		[60] = {faction = HORDE, distance = 481.02543173565, cost = 1020},
		[68] = {faction = HORDE, distance = 7712.248087269, cost = 1750},
		[76] = {faction = HORDE, distance = 8262.8570178005, cost = 730},
		[383] = {faction = HORDE, distance = 4725.2153478125, cost = 1020},
		[384] = {faction = HORDE, distance = 2687.2686313579, cost = 730},
		[460] = {faction = HORDE, distance = 1852.5565237674, cost = 0},
		[601] = {faction = HORDE, distance = 5806.5008147768, cost = 550},
		[608] = {faction = HORDE, distance = 10404.631617695, cost = 10000},
		[645] = {faction = HORDE, distance = 1997.6687640473, cost = 110},
	},
	[12] = {
		[2] = {faction = ALLIANCE, distance = 2645.7271943776, cost = 330},
		[4] = {faction = ALLIANCE, distance = 2607.4369382123, cost = 110},
		[5] = {faction = ALLIANCE, distance = 1794.4307245, cost = 210},
		[19] = {faction = ALLIANCE, distance = 4901.2424215166, cost = 1260},
		[45] = {faction = ALLIANCE, distance = 2946.6120900665, cost = 830},
		[195] = {faction = ALLIANCE, distance = 1422.8581020359, cost = 630},
		[582] = {faction = ALLIANCE, distance = 2045.3769216573, cost = 110},
		[622] = {faction = ALLIANCE, distance = 1860.2165677482, cost = 220},
	},
	[13] = {
		[10] = {faction = HORDE, distance = 3222.3971394559, cost = 110},
		[11] = {faction = HORDE, distance = 4252.6595714988, cost = 330},
		[17] = {faction = HORDE, distance = 3565.4778451153, cost = 530},
		[76] = {faction = HORDE, distance = 4869.2543195825, cost = 730},
		[383] = {faction = HORDE, distance = 3220.5218702599, cost = 730},
		[384] = {faction = HORDE, distance = 2124.4371449737, cost = 730},
		[601] = {faction = HORDE, distance = 1534.7846880483, cost = 220},
		[617] = {faction = HORDE, distance = 3152.1983501366, cost = 730},
		[667] = {faction = HORDE, distance = 892.1313597255, cost = 220},
		[669] = {faction = HORDE, distance = 726.62523643705, cost = 220},
		[670] = {faction = HORDE, distance = 827.82660765337, cost = 220},
	},
	[14] = {
		[6] = {faction = NONE, distance = 5982.4752876425, cost = 660},
		[7] = {faction = NONE, distance = 3286.2694365743, cost = 330},
		[16] = {faction = NONE, distance = 2226.7038343535, cost = 530},
		[43] = {faction = NONE, distance = 2123.3555314102, cost = 730},
		[66] = {faction = NONE, distance = 2401.6242679189, cost = 830},
	},
	[16] = {
		[6] = {faction = ALLIANCE, distance = 6491.3207078219, cost = 660},
		[7] = {faction = ALLIANCE, distance = 3795.1148567538, cost = 330},
		[8] = {faction = ALLIANCE, distance = 5138.9699168275, cost = 110},
		[14] = {faction = ALLIANCE, distance = 2602.5621781244, cost = 330},
		[43] = {faction = ALLIANCE, distance = 2138.7197554484, cost = 730},
		[553] = {faction = ALLIANCE, distance = 1743.7710585111, cost = 530},
		[618] = {faction = ALLIANCE, distance = 2525.8045122139, cost = 730},
		[658] = {faction = ALLIANCE, distance = 4381.1031548474, cost = 10000},
	},
	[17] = {
		[11] = {faction = HORDE, distance = 7597.799808626, cost = 2920},
		[13] = {faction = HORDE, distance = 3413.6753072809, cost = 330},
		[21] = {faction = HORDE, distance = 7492.5506172703, cost = 630},
		[76] = {faction = HORDE, distance = 2655.671294272, cost = 730},
		[601] = {faction = HORDE, distance = 2006.9364665318, cost = 220},
		[617] = {faction = HORDE, distance = 1578.9055946299, cost = 730},
		[658] = {faction = HORDE, distance = 5034.6088931375, cost = 10000},
		[661] = {faction = HORDE, distance = 7543.5699816093, cost = 30000},
	},
	[18] = {
		[20] = {faction = HORDE, distance = 2261.2375970965, cost = 630},
		[21] = {faction = HORDE, distance = 9301.3679743043, cost = 630},
		[56] = {faction = HORDE, distance = 7159.2515872838, cost = 630},
		[592] = {faction = HORDE, distance = 1222.2725692423, cost = 630},
		[603] = {faction = HORDE, distance = 5094.9455910535, cost = 1030},
	},
	[19] = {
		[2] = {faction = ALLIANCE, distance = 5986.8926374598, cost = 630},
		[4] = {faction = ALLIANCE, distance = 4549.1057050006, cost = 530},
		[12] = {faction = ALLIANCE, distance = 4989.9285711893, cost = 960},
		[195] = {faction = ALLIANCE, distance = 3543.1695668375, cost = 630},
		[591] = {faction = ALLIANCE, distance = 1457.1367285565, cost = 630},
		[602] = {faction = ALLIANCE, distance = 4321.7270870862, cost = 1030},
	},
	[20] = {
		[18] = {faction = HORDE, distance = 2329.0785676457, cost = 630},
		[21] = {faction = HORDE, distance = 7289.2889213854, cost = 630},
		[56] = {faction = HORDE, distance = 5231.0249632729, cost = 630},
		[70] = {faction = HORDE, distance = 5951.3112950089, cost = 830},
		[592] = {faction = HORDE, distance = 1177.2702115483, cost = 630},
		[593] = {faction = HORDE, distance = 1693.5726033933, cost = 630},
	},
	[21] = {
		[11] = {faction = HORDE, distance = 14201.039132636, cost = 630},
		[17] = {faction = HORDE, distance = 7591.3686208011, cost = 630},
		[18] = {faction = HORDE, distance = 9036.4758850365, cost = 630},
		[20] = {faction = HORDE, distance = 7329.7563098937, cost = 530},
		[56] = {faction = HORDE, distance = 6634.8744279346, cost = 2480},
		[70] = {faction = HORDE, distance = 2171.8253536916, cost = 830},
		[75] = {faction = HORDE, distance = 1880.9485790598, cost = 830},
		[632] = {faction = HORDE, distance = 1342.1445463146, cost = 730},
		[635] = {faction = HORDE, distance = 1700.0683649347, cost = 830},
		[658] = {faction = HORDE, distance = 5702.9691914851, cost = 10000},
		[661] = {faction = HORDE, distance = 7022.8628818583, cost = 30000},
	},
	[22] = {
		[23] = {faction = HORDE, distance = 6473.2710320923, cost = 50},
		[25] = {faction = HORDE, distance = 3071.7477981845, cost = 110},
		[29] = {faction = HORDE, distance = 3808.1767150866, cost = 860},
		[30] = {faction = HORDE, distance = 6352.2380300753, cost = 430},
		[38] = {faction = HORDE, distance = 4782.976893757, cost = 530},
		[40] = {faction = HORDE, distance = 8803.3725547775, cost = 730},
		[42] = {faction = HORDE, distance = 6779.9953345357, cost = 850},
		[44] = {faction = HORDE, distance = 9525.7935723826, cost = 830},
		[55] = {faction = HORDE, distance = 4402.9832764621, cost = 740},
		[58] = {faction = HORDE, distance = 7436.8868433353, cost = 1390},
		[77] = {faction = HORDE, distance = 2542.5013277292, cost = 110},
		[363] = {faction = HORDE, distance = 1619.4342485281, cost = 530},
		[369] = {faction = HORDE, distance = 1972.9945451129, cost = 530},
		[390] = {faction = HORDE, distance = 1956.1880426234, cost = 330},
		[402] = {faction = HORDE, distance = 1513.747503655, cost = 0},
	},
	[23] = {
		[22] = {faction = HORDE, distance = 6569.5739010155, cost = 220},
		[25] = {faction = HORDE, distance = 3353.2619559457, cost = 110},
		[40] = {faction = HORDE, distance = 10076.812211087, cost = 730},
		[44] = {faction = HORDE, distance = 3395.0030362479, cost = 830},
		[48] = {faction = HORDE, distance = 7663.7062760535, cost = 2190},
		[53] = {faction = HORDE, distance = 7537.3520270167, cost = 1020},
		[55] = {faction = HORDE, distance = 6384.6313184987, cost = 740},
		[61] = {faction = HORDE, distance = 2792.6238014847, cost = 530},
		[80] = {faction = HORDE, distance = 3349.0924030208, cost = 110},
		[198] = {faction = HORDE, distance = 692.61870286408, cost = 1},
		[354] = {faction = HORDE, distance = 2968.3918880955, cost = 530},
		[458] = {faction = HORDE, distance = 1656.9355154666, cost = 220},
		[537] = {faction = HORDE, distance = 1642.5048644384, cost = 0},
		[613] = {faction = HORDE, distance = 2777.0231708786, cost = 830},
		[616] = {faction = HORDE, distance = 3489.5092833034, cost = 30000},
		[683] = {faction = HORDE, distance = 2455.5494867188, cost = 830},
	},
	[25] = {
		[22] = {faction = HORDE, distance = 3216.3119450698, cost = 110},
		[23] = {faction = HORDE, distance = 3608.9252079751, cost = 110},
		[29] = {faction = HORDE, distance = 4531.9597325499, cost = 210},
		[30] = {faction = HORDE, distance = 5653.8960715223, cost = 430},
		[40] = {faction = HORDE, distance = 8588.5719809758, cost = 1510},
		[42] = {faction = HORDE, distance = 6287.1426159291, cost = 850},
		[44] = {faction = HORDE, distance = 6861.5564382786, cost = 830},
		[48] = {faction = HORDE, distance = 7624.5157763729, cost = 930},
		[55] = {faction = HORDE, distance = 3910.1305578556, cost = 740},
		[58] = {faction = HORDE, distance = 6462.7851647908, cost = 1190},
		[61] = {faction = HORDE, distance = 3291.681644066, cost = 860},
		[77] = {faction = HORDE, distance = 2049.6486091226, cost = 110},
		[80] = {faction = HORDE, distance = 1554.7915470821, cost = 110},
		[354] = {faction = HORDE, distance = 1876.5249896273, cost = 330},
		[363] = {faction = HORDE, distance = 3479.47970021, cost = 530},
		[390] = {faction = HORDE, distance = 1461.2257875524, cost = 330},
		[458] = {faction = HORDE, distance = 2017.5317816599, cost = 110},
	},
	[26] = {
		[27] = {faction = ALLIANCE, distance = 1811.8798187165, cost = 0},
		[28] = {faction = ALLIANCE, distance = 5292.4476958718, cost = 660},
		[32] = {faction = ALLIANCE, distance = 14265.635033289, cost = 1400},
		[33] = {faction = ALLIANCE, distance = 7649.5007427822, cost = 660},
		[37] = {faction = ALLIANCE, distance = 9314.5659734108, cost = 1520},
		[41] = {faction = ALLIANCE, distance = 14936.247089732, cost = 730},
		[49] = {faction = ALLIANCE, distance = 2769.0877781849, cost = 830},
		[64] = {faction = ALLIANCE, distance = 9269.7836142735, cost = 2120},
		[65] = {faction = ALLIANCE, distance = 2856.0162791854, cost = 730},
		[339] = {faction = ALLIANCE, distance = 2771.5936753048, cost = 330},
	},
	[27] = {
		[26] = {faction = ALLIANCE, distance = 1812.5817028991, cost = 0},
		[94] = {faction = ALLIANCE, distance = 25769.334976931, cost = 110},
		[457] = {faction = ALLIANCE, distance = 3347.299674103, cost = 0},
	},
	[28] = {
		[26] = {faction = ALLIANCE, distance = 5388.3965731718, cost = 660},
		[32] = {faction = ALLIANCE, distance = 8973.187337417, cost = 740},
		[33] = {faction = ALLIANCE, distance = 4071.1692398632, cost = 860},
		[64] = {faction = ALLIANCE, distance = 4474.7810379155, cost = 730},
		[80] = {faction = ALLIANCE, distance = 5821.36569015, cost = 110},
		[166] = {faction = ALLIANCE, distance = 2471.1424709686, cost = 830},
		[167] = {faction = ALLIANCE, distance = 4042.2006005777, cost = 330},
		[338] = {faction = ALLIANCE, distance = 1568.9711929773, cost = 330},
		[339] = {faction = ALLIANCE, distance = 2620.4276911663, cost = 330},
		[351] = {faction = ALLIANCE, distance = 1221.2581966723, cost = 330},
	},
	[29] = {
		[22] = {faction = HORDE, distance = 3420.7282660471, cost = 860},
		[25] = {faction = HORDE, distance = 4503.4455130942, cost = 210},
		[38] = {faction = HORDE, distance = 4340.3264057545, cost = 530},
		[58] = {faction = HORDE, distance = 3892.0059346815, cost = 330},
		[350] = {faction = HORDE, distance = 3288.0695346784, cost = 330},
		[360] = {faction = HORDE, distance = 1453.8496828768, cost = 330},
		[362] = {faction = HORDE, distance = 1265.6711622377, cost = 330},
		[363] = {faction = HORDE, distance = 2029.2407831826, cost = 330},
		[366] = {faction = HORDE, distance = 3061.8298193841, cost = 530},
		[368] = {faction = HORDE, distance = 3257.8840333545, cost = 530},
		[540] = {faction = HORDE, distance = 1210.671760046, cost = 330},
	},
	[30] = {
		[22] = {faction = HORDE, distance = 6480.4391850906, cost = 430},
		[25] = {faction = HORDE, distance = 5579.0548242761, cost = 430},
		[40] = {faction = HORDE, distance = 4599.4710433851, cost = 730},
		[42] = {faction = HORDE, distance = 1488.2633077341, cost = 730},
		[55] = {faction = HORDE, distance = 2923.8886737249, cost = 630},
		[77] = {faction = HORDE, distance = 3648.9088084398, cost = 430},
		[179] = {faction = HORDE, distance = 2464.9006666265, cost = 630},
		[391] = {faction = HORDE, distance = 2981.018099931, cost = 430},
		[513] = {faction = HORDE, distance = 3537.4250963509, cost = 430},
	},
	[31] = {
		[32] = {faction = ALLIANCE, distance = 5535.055628149, cost = 1260},
		[39] = {faction = ALLIANCE, distance = 6483.2327625821, cost = 730},
		[41] = {faction = ALLIANCE, distance = 2512.3272058287, cost = 630},
		[179] = {faction = ALLIANCE, distance = 3972.1948745023, cost = 630},
		[513] = {faction = ALLIANCE, distance = 5333.139031574, cost = 730},
		[567] = {faction = ALLIANCE, distance = 1797.3097836483, cost = 330},
	},
	[32] = {
		[26] = {faction = NONE, distance = 14765.637222284, cost = 1100},
		[28] = {faction = NONE, distance = 9377.2406491121, cost = 440},
		[31] = {faction = NONE, distance = 6064.1325706151, cost = 430},
		[37] = {faction = NONE, distance = 9224.3313578091, cost = 2220},
		[39] = {faction = NONE, distance = 4596.3203006136, cost = 730},
		[64] = {faction = NONE, distance = 7043.612637923, cost = 730},
		[80] = {faction = NONE, distance = 3438.7390351382, cost = 110},
		[179] = {faction = NONE, distance = 1907.8139992639, cost = 630},
		[388] = {faction = NONE, distance = 2391.1499036529, cost = 630},
	},
	[33] = {
		[26] = {faction = ALLIANCE, distance = 7612.9593538044, cost = 330},
		[28] = {faction = ALLIANCE, distance = 4142.3070491767, cost = 860},
		[37] = {faction = ALLIANCE, distance = 3269.7873089622, cost = 530},
		[338] = {faction = ALLIANCE, distance = 3806.2600827876, cost = 330},
		[339] = {faction = ALLIANCE, distance = 5067.0873945364, cost = 330},
		[351] = {faction = ALLIANCE, distance = 2979.8639839808, cost = 530},
		[361] = {faction = ALLIANCE, distance = 2036.2017586595, cost = 660},
		[364] = {faction = ALLIANCE, distance = 3064.0009367139, cost = 330},
		[365] = {faction = ALLIANCE, distance = 1870.2943666919, cost = 330},
		[367] = {faction = ALLIANCE, distance = 4273.5386626533, cost = 530},
		[368] = {faction = ALLIANCE, distance = 3645.1923303076, cost = 530},
		[541] = {faction = ALLIANCE, distance = 1198.571175504, cost = 330},
	},
	[36] = {
		[24] = {faction = NONE, distance = 742.38403429992, cost = 0},
	},
	[37] = {
		[26] = {faction = ALLIANCE, distance = 9612.2722342701, cost = 1520},
		[32] = {faction = ALLIANCE, distance = 9087.4459133082, cost = 2220},
		[33] = {faction = ALLIANCE, distance = 3048.6907319193, cost = 330},
		[41] = {faction = ALLIANCE, distance = 5683.57556592, cost = 990},
		[351] = {faction = ALLIANCE, distance = 3061.4325959025, cost = 530},
		[361] = {faction = ALLIANCE, distance = 2222.2580691131, cost = 530},
		[364] = {faction = ALLIANCE, distance = 2444.5892150426, cost = 530},
		[365] = {faction = ALLIANCE, distance = 1558.3264275045, cost = 530},
		[367] = {faction = ALLIANCE, distance = 2423.7790176158, cost = 330},
		[368] = {faction = ALLIANCE, distance = 1477.6255454932, cost = 330},
		[369] = {faction = ALLIANCE, distance = 909.24671195054, cost = 330},
		[370] = {faction = ALLIANCE, distance = 1424.3217309251, cost = 330},
	},
	[38] = {
		[22] = {faction = HORDE, distance = 4713.7128870905, cost = 1190},
		[29] = {faction = HORDE, distance = 5231.8548702487, cost = 860},
		[42] = {faction = HORDE, distance = 5944.6184594285, cost = 730},
		[363] = {faction = HORDE, distance = 5186.1941018945, cost = 530},
		[366] = {faction = HORDE, distance = 2329.6878288124, cost = 330},
		[368] = {faction = HORDE, distance = 2500.1665528172, cost = 330},
		[370] = {faction = HORDE, distance = 2597.5026434017, cost = 660},
		[568] = {faction = HORDE, distance = 2110.4245197219, cost = 330},
	},
	[39] = {
		[31] = {faction = ALLIANCE, distance = 6432.4528151621, cost = 630},
		[32] = {faction = ALLIANCE, distance = 4567.3645507261, cost = 630},
		[73] = {faction = ALLIANCE, distance = 5602.6316762253, cost = 1560},
		[79] = {faction = ALLIANCE, distance = 2874.8601664537, cost = 730},
		[80] = {faction = ALLIANCE, distance = 7436.3953877338, cost = 110},
		[386] = {faction = ALLIANCE, distance = 3134.9573753582, cost = 730},
		[513] = {faction = ALLIANCE, distance = 1463.7370550324, cost = 430},
		[532] = {faction = ALLIANCE, distance = 2656.0500897456, cost = 430},
		[539] = {faction = ALLIANCE, distance = 1585.7163400208, cost = 430},
	},
	[40] = {
		[22] = {faction = HORDE, distance = 9288.0841875007, cost = 730},
		[23] = {faction = HORDE, distance = 10346.45244751, cost = 220},
		[25] = {faction = HORDE, distance = 9080.3515280497, cost = 730},
		[30] = {faction = HORDE, distance = 5020.2756500048, cost = 430},
		[42] = {faction = HORDE, distance = 6283.8068723808, cost = 630},
		[55] = {faction = HORDE, distance = 5537.5605372247, cost = 1490},
		[72] = {faction = HORDE, distance = 5550.7990071397, cost = 1560},
		[79] = {faction = HORDE, distance = 3003.3680588336, cost = 730},
		[80] = {faction = HORDE, distance = 7227.0435235256, cost = 110},
		[386] = {faction = HORDE, distance = 3028.6806657856, cost = 730},
		[513] = {faction = HORDE, distance = 1491.7114423885, cost = 430},
		[531] = {faction = HORDE, distance = 2911.0778133006, cost = 430},
		[539] = {faction = HORDE, distance = 1676.0376214738, cost = 430},
	},
	[41] = {
		[26] = {faction = ALLIANCE, distance = 14903.195291151, cost = 730},
		[31] = {faction = ALLIANCE, distance = 2703.5985868553, cost = 660},
		[37] = {faction = ALLIANCE, distance = 5844.432790478, cost = 730},
		[73] = {faction = ALLIANCE, distance = 3554.688688656, cost = 1030},
		[365] = {faction = ALLIANCE, distance = 6130.1379390453, cost = 730},
		[367] = {faction = ALLIANCE, distance = 3661.4863248633, cost = 660},
		[565] = {faction = ALLIANCE, distance = 1640.9980399474, cost = 330},
		[567] = {faction = ALLIANCE, distance = 906.88269230112, cost = 330},
	},
	[42] = {
		[22] = {faction = HORDE, distance = 7734.9764300213, cost = 860},
		[25] = {faction = HORDE, distance = 6802.9418665173, cost = 850},
		[30] = {faction = HORDE, distance = 1254.5372449307, cost = 430},
		[38] = {faction = HORDE, distance = 5941.1098703943, cost = 1290},
		[40] = {faction = HORDE, distance = 5854.0082883158, cost = 1160},
		[72] = {faction = HORDE, distance = 3935.7324176547, cost = 1030},
		[366] = {faction = HORDE, distance = 7726.2944487835, cost = 1030},
		[391] = {faction = HORDE, distance = 3114.3195782419, cost = 630},
		[569] = {faction = HORDE, distance = 1954.8255434451, cost = 330},
	},
	[43] = {
		[6] = {faction = ALLIANCE, distance = 7791.7920226748, cost = 730},
		[14] = {faction = ALLIANCE, distance = 2072.6630231621, cost = 330},
		[16] = {faction = ALLIANCE, distance = 2428.2613299298, cost = 530},
		[66] = {faction = ALLIANCE, distance = 1717.2318065114, cost = 1020},
		[67] = {faction = ALLIANCE, distance = 4988.4755570712, cost = 1460},
		[618] = {faction = ALLIANCE, distance = 2363.9111492002, cost = 730},
		[650] = {faction = ALLIANCE, distance = 2107.1076163098, cost = 1020},
	},
	[44] = {
		[22] = {faction = HORDE, distance = 9137.3229308593, cost = 830},
		[23] = {faction = HORDE, distance = 2934.64729455, cost = 830},
		[25] = {faction = HORDE, distance = 6287.9092504956, cost = 940},
		[48] = {faction = HORDE, distance = 8586.9235018048, cost = 930},
		[53] = {faction = HORDE, distance = 4474.4451412114, cost = 1020},
		[61] = {faction = HORDE, distance = 4460.1790543204, cost = 530},
		[613] = {faction = HORDE, distance = 946.58059219772, cost = 830},
		[614] = {faction = HORDE, distance = 1555.3258079879, cost = 830},
		[683] = {faction = HORDE, distance = 2471.9460671669, cost = 830},
	},
	[45] = {
		[2] = {faction = ALLIANCE, distance = 5425.9494235543, cost = 660},
		[12] = {faction = ALLIANCE, distance = 2780.2222291767, cost = 330},
		[71] = {faction = ALLIANCE, distance = 4483.3247494519, cost = 2280},
		[598] = {faction = ALLIANCE, distance = 1153.9904033146, cost = 630},
		[602] = {faction = ALLIANCE, distance = 2767.0033597746, cost = 1030},
	},
	[46] = {
		[7] = {faction = NONE, distance = 6296.9818808176, cost = 0},
	},
	[48] = {
		[23] = {faction = NONE, distance = 7667.4958473025, cost = 1890},
		[25] = {faction = NONE, distance = 7315.4579404126, cost = 930},
		[44] = {faction = NONE, distance = 8848.8412278822, cost = 830},
		[53] = {faction = NONE, distance = 5702.445588587, cost = 1020},
		[58] = {faction = NONE, distance = 4079.9042051512, cost = 330},
		[69] = {faction = NONE, distance = 4992.7734995372, cost = 830},
		[166] = {faction = NONE, distance = 2191.5774461009, cost = 830},
		[597] = {faction = NONE, distance = 2774.8050292941, cost = 730},
	},
	[49] = {
		[26] = {faction = ALLIANCE, distance = 2539.2757934189, cost = 830},
		[52] = {faction = ALLIANCE, distance = 3565.3904355188, cost = 1020},
		[65] = {faction = ALLIANCE, distance = 1934.3983225271, cost = 730},
		[559] = {faction = ALLIANCE, distance = 4207.0925227939, cost = 10000},
	},
	[50] = {
		[51] = {faction = NONE, distance = 13979.349908902, cost = 0},
	},
	[52] = {
		[49] = {faction = ALLIANCE, distance = 3361.8964023726, cost = 830},
		[64] = {faction = ALLIANCE, distance = 5033.7813421704, cost = 730},
		[65] = {faction = ALLIANCE, distance = 3308.8769138887, cost = 730},
		[559] = {faction = ALLIANCE, distance = 3880.5334134602, cost = 10000},
	},
	[53] = {
		[23] = {faction = HORDE, distance = 7374.4451544028, cost = 1660},
		[44] = {faction = HORDE, distance = 4439.7978598528, cost = 830},
		[48] = {faction = HORDE, distance = 5947.7298746077, cost = 930},
		[69] = {faction = HORDE, distance = 3916.8585782791, cost = 830},
		[559] = {faction = HORDE, distance = 3790.3224666817, cost = 10000},
		[597] = {faction = HORDE, distance = 3664.2023672297, cost = 730},
	},
	[54] = {
		[24] = {faction = NONE, distance = 4487.5097666607, cost = 0},
	},
	[55] = {
		[22] = {faction = HORDE, distance = 5342.6044405093, cost = 220},
		[23] = {faction = HORDE, distance = 5806.0350541562, cost = 220},
		[25] = {faction = HORDE, distance = 4045.240226249, cost = 220},
		[30] = {faction = HORDE, distance = 3332.2423658158, cost = 630},
		[40] = {faction = HORDE, distance = 4841.1868135017, cost = 1490},
		[77] = {faction = HORDE, distance = 1765.5211225981, cost = 110},
		[80] = {faction = HORDE, distance = 2686.626130172, cost = 110},
		[179] = {faction = HORDE, distance = 1875.9022512454, cost = 630},
		[391] = {faction = HORDE, distance = 1663.481624078, cost = 110},
	},
	[56] = {
		[18] = {faction = HORDE, distance = 6910.1990469852, cost = 630},
		[20] = {faction = HORDE, distance = 5104.5097012494, cost = 630},
		[21] = {faction = HORDE, distance = 6858.2467500356, cost = 630},
		[70] = {faction = HORDE, distance = 4614.9140907886, cost = 1650},
		[599] = {faction = HORDE, distance = 1333.9009942834, cost = 630},
		[604] = {faction = HORDE, distance = 792.29215155018, cost = 1030},
	},
	[58] = {
		[22] = {faction = HORDE, distance = 6739.7232620483, cost = 1390},
		[25] = {faction = HORDE, distance = 7174.7610504858, cost = 110},
		[29] = {faction = HORDE, distance = 3791.6293474467, cost = 210},
		[48] = {faction = HORDE, distance = 4169.7169217995, cost = 830},
		[61] = {faction = HORDE, distance = 5266.1220351589, cost = 530},
		[166] = {faction = HORDE, distance = 4463.1137165094, cost = 830},
		[350] = {faction = HORDE, distance = 2436.5400336654, cost = 330},
		[360] = {faction = HORDE, distance = 2221.4116133319, cost = 530},
	},
	[59] = {
		[6] = {faction = NONE, distance = 9552.9859124668, cost = 0},
	},
	[60] = {
		[11] = {faction = NONE, distance = 481.02544071219, cost = 0},
	},
	[61] = {
		[23] = {faction = HORDE, distance = 3015.7260864917, cost = 530},
		[25] = {faction = HORDE, distance = 3393.1753731295, cost = 860},
		[44] = {faction = HORDE, distance = 4692.6697214868, cost = 1660},
		[58] = {faction = HORDE, distance = 4354.7526744162, cost = 660},
		[166] = {faction = HORDE, distance = 2631.9529085537, cost = 830},
		[350] = {faction = HORDE, distance = 2553.2299429273, cost = 330},
		[354] = {faction = HORDE, distance = 1480.0352112848, cost = 530},
		[356] = {faction = HORDE, distance = 1834.193219609, cost = 330},
		[683] = {faction = HORDE, distance = 2123.6400509289, cost = 830},
	},
	[62] = {
		[27] = {faction = ALLIANCE, distance = 4541.2507031279, cost = 0},
	},
	[63] = {
		[22] = {faction = HORDE, distance = 12099.219473157, cost = 0},
	},
	[64] = {
		[26] = {faction = NONE, distance = 9173.332910198, cost = 2220},
		[28] = {faction = NONE, distance = 4536.0466905321, cost = 330},
		[32] = {faction = NONE, distance = 7238.6623979671, cost = 740},
		[52] = {faction = NONE, distance = 5405.3798932791, cost = 1030},
		[65] = {faction = NONE, distance = 8152.031930497, cost = 1990},
		[80] = {faction = NONE, distance = 4086.8407507001, cost = 110},
		[167] = {faction = NONE, distance = 771.92537733661, cost = 330},
	},
	[65] = {
		[26] = {faction = ALLIANCE, distance = 3469.612829279, cost = 730},
		[49] = {faction = ALLIANCE, distance = 2124.8013688067, cost = 1020},
		[52] = {faction = ALLIANCE, distance = 3270.784052305, cost = 1020},
		[64] = {faction = ALLIANCE, distance = 7904.8134156349, cost = 1890},
		[166] = {faction = ALLIANCE, distance = 3842.0800427964, cost = 830},
		[339] = {faction = ALLIANCE, distance = 3613.4872550461, cost = 730},
		[594] = {faction = ALLIANCE, distance = 1527.7696302918, cost = 830},
	},
	[66] = {
		[6] = {faction = ALLIANCE, distance = 7785.4276757991, cost = 1020},
		[14] = {faction = ALLIANCE, distance = 2387.006447195, cost = 330},
		[43] = {faction = ALLIANCE, distance = 1959.0245803773, cost = 730},
		[67] = {faction = ALLIANCE, distance = 4451.41870885, cost = 1020},
		[383] = {faction = ALLIANCE, distance = 1747.6725322986, cost = 730},
		[650] = {faction = ALLIANCE, distance = 784.0597748669, cost = 730},
		[651] = {faction = ALLIANCE, distance = 1237.6157446529, cost = 730},
	},
	[67] = {
		[6] = {faction = ALLIANCE, distance = 11191.076976366, cost = 1020},
		[43] = {faction = ALLIANCE, distance = 5043.2299911759, cost = 730},
		[66] = {faction = ALLIANCE, distance = 4394.7061337085, cost = 1020},
		[86] = {faction = ALLIANCE, distance = 669.85028390214, cost = 730},
		[205] = {faction = ALLIANCE, distance = 7729.6244849517, cost = 1020},
		[213] = {faction = ALLIANCE, distance = 14814.138731475, cost = 1750},
		[315] = {faction = ALLIANCE, distance = 2088.1122018409, cost = 730},
		[383] = {faction = ALLIANCE, distance = 2834.6528841213, cost = 1020},
		[618] = {faction = ALLIANCE, distance = 2863.1308017025, cost = 730},
		[630] = {faction = ALLIANCE, distance = 1023.1679185773, cost = 730},
		[635] = {faction = ALLIANCE, distance = 9696.7664473587, cost = 1020},
	},
	[68] = {
		[11] = {faction = HORDE, distance = 7635.4986893794, cost = 2040},
		[76] = {faction = HORDE, distance = 4309.6072320968, cost = 730},
		[83] = {faction = HORDE, distance = 8180.3054932154, cost = 1020},
		[86] = {faction = HORDE, distance = 669.27492407261, cost = 730},
		[205] = {faction = HORDE, distance = 7519.0901794309, cost = 1020},
		[315] = {faction = HORDE, distance = 1930.1328796224, cost = 730},
		[383] = {faction = HORDE, distance = 2809.4240310245, cost = 1020},
		[630] = {faction = HORDE, distance = 1023.1679185773, cost = 730},
		[635] = {faction = HORDE, distance = 9696.7664473587, cost = 1020},
	},
	[69] = {
		[48] = {faction = HORDE, distance = 4748.0735089369, cost = 930},
		[53] = {faction = HORDE, distance = 4272.8822732716, cost = 1020},
		[559] = {faction = HORDE, distance = 4048.9901055934, cost = 10000},
		[597] = {faction = HORDE, distance = 2198.3327852645, cost = 830},
	},
	[70] = {
		[20] = {faction = HORDE, distance = 6212.8952000618, cost = 630},
		[21] = {faction = HORDE, distance = 2447.0452203717, cost = 830},
		[56] = {faction = HORDE, distance = 4463.049074243, cost = 1650},
		[75] = {faction = HORDE, distance = 2000.64647675, cost = 830},
		[599] = {faction = HORDE, distance = 3217.5662381197, cost = 1020},
		[676] = {faction = HORDE, distance = 484.93296893591, cost = 830},
	},
	[71] = {
		[2] = {faction = ALLIANCE, distance = 4727.2682415593, cost = 830},
		[5] = {faction = ALLIANCE, distance = 1854.8565748158, cost = 210},
		[45] = {faction = ALLIANCE, distance = 4350.1041783289, cost = 2280},
		[74] = {faction = ALLIANCE, distance = 2866.8334977931, cost = 830},
		[599] = {faction = ALLIANCE, distance = 2313.7935999907, cost = 1020},
		[634] = {faction = ALLIANCE, distance = 1652.9322123613, cost = 830},
		[676] = {faction = ALLIANCE, distance = 1279.0144437902, cost = 830},
	},
	[72] = {
		[40] = {faction = HORDE, distance = 5260.6739931233, cost = 1560},
		[42] = {faction = HORDE, distance = 3947.5521538507, cost = 730},
		[79] = {faction = HORDE, distance = 3131.6362629377, cost = 1160},
		[386] = {faction = HORDE, distance = 2314.0275639775, cost = 830},
		[569] = {faction = HORDE, distance = 3212.1018234001, cost = 630},
		[653] = {faction = HORDE, distance = 1900.719687642, cost = 10000},
	},
	[73] = {
		[39] = {faction = ALLIANCE, distance = 5060.5471804044, cost = 1560},
		[41] = {faction = ALLIANCE, distance = 3634.9927901539, cost = 730},
		[79] = {faction = ALLIANCE, distance = 2978.5869614415, cost = 1160},
		[386] = {faction = ALLIANCE, distance = 2160.9782624813, cost = 830},
		[653] = {faction = ALLIANCE, distance = 1923.0236197211, cost = 10000},
	},
	[74] = {
		[2] = {faction = ALLIANCE, distance = 3792.2919072724, cost = 830},
		[6] = {faction = ALLIANCE, distance = 2716.2431754522, cost = 830},
		[8] = {faction = ALLIANCE, distance = 2643.0706008096, cost = 110},
		[71] = {faction = ALLIANCE, distance = 2629.4908450804, cost = 830},
		[673] = {faction = ALLIANCE, distance = 719.55667890867, cost = 730},
	},
	[75] = {
		[21] = {faction = HORDE, distance = 1982.5578609658, cost = 630},
		[70] = {faction = HORDE, distance = 1816.6872270677, cost = 830},
		[673] = {faction = HORDE, distance = 714.50157066192, cost = 730},
	},
	[76] = {
		[11] = {faction = HORDE, distance = 8126.1562612966, cost = 730},
		[13] = {faction = HORDE, distance = 4686.8784862134, cost = 730},
		[17] = {faction = HORDE, distance = 2818.5883647006, cost = 530},
		[68] = {faction = HORDE, distance = 4041.8268831159, cost = 1030},
		[617] = {faction = HORDE, distance = 2267.6357062302, cost = 530},
	},
	[77] = {
		[22] = {faction = HORDE, distance = 3577.0833179112, cost = 110},
		[25] = {faction = HORDE, distance = 2279.7191036509, cost = 110},
		[30] = {faction = HORDE, distance = 3842.6114824426, cost = 430},
		[55] = {faction = HORDE, distance = 1860.481948733, cost = 630},
		[390] = {faction = HORDE, distance = 1659.0629558531, cost = 110},
		[391] = {faction = HORDE, distance = 1313.4543191025, cost = 110},
	},
	[78] = {
		[24] = {faction = NONE, distance = 1373.0475566436, cost = 0},
	},
	[79] = {
		[39] = {faction = ALLIANCE, distance = 2717.2708283339, cost = 730},
		[40] = {faction = HORDE, distance = 2810.7366461423, cost = 730},
		[72] = {faction = HORDE, distance = 3637.6036671126, cost = 1160},
		[73] = {faction = ALLIANCE, distance = 3583.1596266256, cost = 1160},
		[386] = {faction = NEUTRAL, distance = 1115.4853257585, cost = 330},
	},
	[80] = {
		[23] = {faction = HORDE, distance = 3119.4089239842, cost = 110},
		[25] = {faction = HORDE, distance = 2068.0426297123, cost = 110},
		[28] = {faction = ALLIANCE, distance = 5938.5016139739, cost = 330},
		[32] = {faction = NONE, distance = 3151.821647267, cost = 630},
		[39] = {faction = ALLIANCE, distance = 7369.1484298106, cost = 730},
		[40] = {faction = HORDE, distance = 7267.4372900048, cost = 730},
		[55] = {faction = HORDE, distance = 3035.5389154779, cost = 630},
		[64] = {faction = NONE, distance = 3896.6064086903, cost = 730},
		[387] = {faction = ALLIANCE, distance = 2684.6880789205, cost = 630},
		[388] = {faction = ALLIANCE, distance = 1604.6310116321, cost = 630},
	},
	[81] = {
		[36] = {faction = NONE, distance = 824.56679465643, cost = 0},
	},
	[82] = {
		[83] = {faction = NONE, distance = 2006.7052765866, cost = 330},
		[213] = {faction = NONE, distance = 5565.5731047761, cost = 530},
		[625] = {faction = NONE, distance = 936.59025905087, cost = 0},
		[631] = {faction = NONE, distance = 678.18071736534, cost = 0},
	},
	[83] = {
		[68] = {faction = HORDE, distance = 8284.1819120218, cost = 1020},
		[82] = {faction = HORDE, distance = 2215.8012040555, cost = 110},
		[205] = {faction = HORDE, distance = 1548.1156897095, cost = 530},
		[625] = {faction = HORDE, distance = 1573.7208039626, cost = 110},
	},
	[84] = {
		[85] = {faction = NEUTRAL, distance = 1672.3194840638, cost = 730},
		[86] = {faction = NEUTRAL, distance = 1992.091356694, cost = 730},
		[87] = {faction = NEUTRAL, distance = 1578.1411080887, cost = 730},
		[630] = {faction = NEUTRAL, distance = 1795.2033972354, cost = 730},
		[672] = {faction = NEUTRAL, distance = 1846.0564429676, cost = 730},
	},
	[85] = {
		[84] = {faction = NEUTRAL, distance = 1472.0489251411, cost = 730},
		[86] = {faction = NEUTRAL, distance = 886.12634769206, cost = 730},
		[87] = {faction = NEUTRAL, distance = 1541.3619268415, cost = 730},
	},
	[86] = {
		[67] = {faction = ALLIANCE, distance = 887.78247619147, cost = 1020},
		[68] = {faction = HORDE, distance = 896.71536077032, cost = 730},
		[84] = {faction = NEUTRAL, distance = 1937.7646539558, cost = 730},
		[85] = {faction = NEUTRAL, distance = 920.19126240353, cost = 730},
		[630] = {faction = NEUTRAL, distance = 479.44056858462, cost = 730},
	},
	[87] = {
		[84] = {faction = NEUTRAL, distance = 1544.216844244, cost = 730},
		[85] = {faction = NEUTRAL, distance = 1826.0152326144, cost = 730},
		[383] = {faction = NEUTRAL, distance = 1150.9433522376, cost = 730},
		[630] = {faction = NEUTRAL, distance = 1027.1001896793, cost = 730},
	},
	[92] = {
		[91] = {faction = NONE, distance = 880.81146250813, cost = 0},
	},
	[93] = {
		[94] = {faction = ALLIANCE, distance = 3016.4498446406, cost = 110},
	},
	[94] = {
		[27] = {faction = ALLIANCE, distance = 25896.715396787, cost = 110},
		[93] = {faction = ALLIANCE, distance = 2663.5430446203, cost = 110},
		[624] = {faction = ALLIANCE, distance = 1170.3344316988, cost = 0},
	},
	[95] = {
		[96] = {faction = NONE, distance = 4683.6439615244, cost = 0},
	},
	[97] = {
		[98] = {faction = NONE, distance = 1486.9489827211, cost = 0},
	},
	[99] = {
		[102] = {faction = HORDE, distance = 2015.0209372627, cost = 1020},
		[127] = {faction = HORDE, distance = 3874.0059653034, cost = 1020},
		[128] = {faction = HORDE, distance = 3674.8426944284, cost = 1020},
		[130] = {faction = HORDE, distance = 2134.0023707455, cost = 0},
		[141] = {faction = HORDE, distance = 1965.0553769775, cost = 1020},
	},
	[100] = {
		[101] = {faction = ALLIANCE, distance = 2288.027652041, cost = 1020},
		[121] = {faction = ALLIANCE, distance = 3568.9116777479, cost = 1020},
		[128] = {faction = ALLIANCE, distance = 3598.7862186071, cost = 1020},
		[129] = {faction = ALLIANCE, distance = 1933.4211409335, cost = 0},
		[149] = {faction = ALLIANCE, distance = 1692.9022983996, cost = 0},
	},
	[101] = {
		[100] = {faction = ALLIANCE, distance = 2621.9447233017, cost = 1020},
		[117] = {faction = ALLIANCE, distance = 2431.8581235746, cost = 1020},
	},
	[102] = {
		[99] = {faction = HORDE, distance = 2213.6375926662, cost = 1020},
		[118] = {faction = HORDE, distance = 4519.3932538673, cost = 1020},
		[120] = {faction = HORDE, distance = 3984.9586589221, cost = 1020},
		[128] = {faction = HORDE, distance = 2146.9517429961, cost = 1020},
		[151] = {faction = HORDE, distance = 2082.7765568089, cost = 1020},
	},
	[103] = {
		[104] = {faction = NONE, distance = 1893.5088258139, cost = 0},
	},
	[105] = {
		[106] = {faction = NONE, distance = 1985.0602515804, cost = 0},
	},
	[107] = {
		[108] = {faction = NONE, distance = 1747.8788350614, cost = 0},
	},
	[109] = {
		[110] = {faction = NONE, distance = 1748.499788008, cost = 0},
	},
	[111] = {
		[112] = {faction = NONE, distance = 118.27718263517, cost = 0},
	},
	[113] = {
		[114] = {faction = NONE, distance = 3319.6557425169, cost = 0},
	},
	[115] = {
		[116] = {faction = NONE, distance = 2243.234280538, cost = 0},
	},
	[117] = {
		[101] = {faction = ALLIANCE, distance = 2450.249406388, cost = 1020},
		[119] = {faction = ALLIANCE, distance = 3753.6090093166, cost = 1020},
		[125] = {faction = ALLIANCE, distance = 2745.371484033, cost = 1020},
		[128] = {faction = ALLIANCE, distance = 2929.2890276885, cost = 1020},
		[156] = {faction = ALLIANCE, distance = 2070.9359971276, cost = 1020},
		[164] = {faction = ALLIANCE, distance = 1881.8928090314, cost = 1020},
	},
	[118] = {
		[102] = {faction = HORDE, distance = 4433.9718867919, cost = 1020},
		[120] = {faction = HORDE, distance = 2444.5031981948, cost = 1020},
		[126] = {faction = HORDE, distance = 3383.0910004128, cost = 1020},
		[128] = {faction = HORDE, distance = 4554.5874948234, cost = 1020},
		[151] = {faction = HORDE, distance = 3360.6878834123, cost = 1020},
	},
	[119] = {
		[117] = {faction = ALLIANCE, distance = 3803.0194844018, cost = 1020},
		[121] = {faction = ALLIANCE, distance = 3653.660697094, cost = 1020},
		[128] = {faction = ALLIANCE, distance = 2632.6188465278, cost = 1020},
	},
	[120] = {
		[102] = {faction = HORDE, distance = 3795.2271797183, cost = 1020},
		[118] = {faction = HORDE, distance = 2012.6684097838, cost = 1020},
		[128] = {faction = HORDE, distance = 2312.2000954704, cost = 1020},
	},
	[121] = {
		[100] = {faction = ALLIANCE, distance = 2888.6798778067, cost = 1020},
		[119] = {faction = ALLIANCE, distance = 4496.2208490696, cost = 1020},
		[124] = {faction = ALLIANCE, distance = 2376.373466273, cost = 1020},
		[128] = {faction = ALLIANCE, distance = 2240.0500403702, cost = 1020},
	},
	[122] = {
		[125] = {faction = ALLIANCE, distance = 3825.7122595379, cost = 1020},
		[126] = {faction = HORDE, distance = 3270.0950457006, cost = 1020},
		[139] = {faction = NEUTRAL, distance = 1442.2672293668, cost = 1020},
		[150] = {faction = NEUTRAL, distance = 1981.6404031857, cost = 1020},
		[156] = {faction = ALLIANCE, distance = 2809.5659022696, cost = 1020},
		[160] = {faction = NEUTRAL, distance = 2428.62130012, cost = 710},
		[163] = {faction = HORDE, distance = 1944.4802085785, cost = 1020},
	},
	[123] = {
		[127] = {faction = HORDE, distance = 2209.3900797614, cost = 1020},
		[140] = {faction = HORDE, distance = 2533.0991254604, cost = 875},
		[159] = {faction = HORDE, distance = 1950.1699856752, cost = 875},
	},
	[124] = {
		[121] = {faction = ALLIANCE, distance = 3031.9946633149, cost = 1020},
		[140] = {faction = ALLIANCE, distance = 2506.528572223, cost = 875},
		[159] = {faction = ALLIANCE, distance = 1267.346334246, cost = 875},
	},
	[125] = {
		[117] = {faction = ALLIANCE, distance = 2459.6327897839, cost = 1020},
		[122] = {faction = ALLIANCE, distance = 3572.2067067586, cost = 1020},
		[139] = {faction = ALLIANCE, distance = 4649.9622431075, cost = 1020},
		[156] = {faction = ALLIANCE, distance = 1714.0567313627, cost = 510},
		[160] = {faction = ALLIANCE, distance = 1530.3905791628, cost = 510},
		[164] = {faction = ALLIANCE, distance = 2285.0561894228, cost = 1020},
	},
	[126] = {
		[118] = {faction = HORDE, distance = 4477.0091178485, cost = 1020},
		[122] = {faction = HORDE, distance = 2916.0505424183, cost = 1020},
		[139] = {faction = HORDE, distance = 4358.3177717851, cost = 2040},
		[151] = {faction = HORDE, distance = 3496.1740959313, cost = 1020},
		[160] = {faction = HORDE, distance = 766.26277593983, cost = 510},
		[163] = {faction = HORDE, distance = 1661.1930934841, cost = 1020},
	},
	[127] = {
		[99] = {faction = HORDE, distance = 3779.4937250868, cost = 1020},
		[123] = {faction = HORDE, distance = 2027.6360884943, cost = 1020},
		[128] = {faction = HORDE, distance = 1681.4380586617, cost = 1020},
	},
	[128] = {
		[99] = {faction = HORDE, distance = 3944.7348893031, cost = 1020},
		[100] = {faction = ALLIANCE, distance = 3336.9596343806, cost = 1020},
		[102] = {faction = HORDE, distance = 2319.5063537853, cost = 1020},
		[117] = {faction = ALLIANCE, distance = 2525.1679797165, cost = 1020},
		[118] = {faction = HORDE, distance = 4102.9528767387, cost = 1020},
		[119] = {faction = ALLIANCE, distance = 2647.1977724248, cost = 1020},
		[120] = {faction = HORDE, distance = 2457.7234963625, cost = 1020},
		[121] = {faction = ALLIANCE, distance = 2265.6238406291, cost = 1020},
		[127] = {faction = HORDE, distance = 2073.8892353084, cost = 1020},
		[151] = {faction = HORDE, distance = 2384.1907339074, cost = 1020},
	},
	[129] = {
		[100] = {faction = ALLIANCE, distance = 2200.8885575631, cost = 0},
		[101] = {faction = ALLIANCE, distance = 3449.5066596015, cost = 0},
		[149] = {faction = ALLIANCE, distance = 815.59305510864, cost = 0},
	},
	[130] = {
		[99] = {faction = HORDE, distance = 1793.0132312663, cost = 0},
		[102] = {faction = HORDE, distance = 3683.9429890278, cost = 0},
	},
	[131] = {
		[132] = {faction = NONE, distance = 1976.8265635718, cost = 0},
	},
	[133] = {
		[134] = {faction = NONE, distance = 2039.1075963012, cost = 0},
		[149] = {faction = NONE, distance = 1105.6916223317, cost = 0},
	},
	[135] = {
		[136] = {faction = NONE, distance = 3618.892487613, cost = 0},
	},
	[137] = {
		[138] = {faction = NONE, distance = 3162.5992353513, cost = 0},
	},
	[139] = {
		[122] = {faction = NEUTRAL, distance = 1606.6196952552, cost = 1020},
		[125] = {faction = ALLIANCE, distance = 4618.1066849667, cost = 1020},
		[126] = {faction = HORDE, distance = 4408.5008793917, cost = 1020},
		[150] = {faction = NEUTRAL, distance = 2094.3138154344, cost = 1020},
	},
	[140] = {
		[123] = {faction = HORDE, distance = 2001.3826788259, cost = 1020},
		[124] = {faction = ALLIANCE, distance = 2420.1500760083, cost = 875},
	},
	[141] = {
		[99] = {faction = HORDE, distance = 1864.6215641203, cost = 1020},
	},
	[142] = {
		[141] = {faction = NONE, distance = 1394.2779729978, cost = 1020},
	},
	[144] = {
		[143] = {faction = NONE, distance = 1461.1494849961, cost = 0},
	},
	[145] = {
		[146] = {faction = NONE, distance = 1296.7046750133, cost = 0},
	},
	[147] = {
		[148] = {faction = NONE, distance = 591.27553366458, cost = 0},
	},
	[148] = {
		[147] = {faction = NONE, distance = 801.8453207194, cost = 0},
	},
	[149] = {
		[100] = {faction = NONE, distance = 1703.8434731638, cost = 0},
		[129] = {faction = NONE, distance = 988.61031073541, cost = 0},
	},
	[150] = {
		[122] = {faction = NEUTRAL, distance = 1924.4969723495, cost = 1020},
		[139] = {faction = NEUTRAL, distance = 1832.0190995856, cost = 1020},
	},
	[151] = {
		[102] = {faction = HORDE, distance = 1890.0444545106, cost = 1020},
		[118] = {faction = HORDE, distance = 3329.6267583199, cost = 1020},
		[126] = {faction = HORDE, distance = 3199.0133952997, cost = 1020},
		[128] = {faction = HORDE, distance = 2612.3715889245, cost = 1020},
		[163] = {faction = HORDE, distance = 2115.8563213787, cost = 1020},
	},
	[152] = {
		[153] = {faction = NONE, distance = 1840.697070653, cost = 0},
	},
	[154] = {
		[155] = {faction = NONE, distance = 1517.1276327288, cost = 0},
	},
	[156] = {
		[117] = {faction = ALLIANCE, distance = 2175.7642802033, cost = 1020},
		[122] = {faction = ALLIANCE, distance = 2510.5773530089, cost = 1020},
		[125] = {faction = ALLIANCE, distance = 1787.7731782191, cost = 510},
		[160] = {faction = ALLIANCE, distance = 1598.0481976553, cost = 510},
	},
	[157] = {
		[158] = {faction = NONE, distance = 2396.4242083611, cost = 0},
	},
	[159] = {
		[123] = {faction = HORDE, distance = 1826.0528474179, cost = 875},
		[124] = {faction = ALLIANCE, distance = 1251.7190918996, cost = 875},
	},
	[160] = {
		[122] = {faction = NEUTRAL, distance = 2324.7454495448, cost = 710},
		[125] = {faction = ALLIANCE, distance = 1647.6206162827, cost = 510},
		[126] = {faction = HORDE, distance = 1090.4264535185, cost = 510},
		[156] = {faction = ALLIANCE, distance = 1305.5138937126, cost = 510},
	},
	[161] = {
		[162] = {faction = NONE, distance = 551.66678157105, cost = 0},
	},
	[163] = {
		[122] = {faction = HORDE, distance = 1686.3456330049, cost = 1020},
		[126] = {faction = HORDE, distance = 1891.8502697329, cost = 1020},
		[151] = {faction = HORDE, distance = 2195.0297536082, cost = 1020},
	},
	[164] = {
		[117] = {faction = ALLIANCE, distance = 1569.2852237628, cost = 1020},
		[125] = {faction = ALLIANCE, distance = 1932.9143873841, cost = 1020},
	},
	[165] = {
		[175] = {faction = NONE, distance = 1116.4767763893, cost = 0},
	},
	[166] = {
		[28] = {faction = ALLIANCE, distance = 2507.4137337173, cost = 530},
		[48] = {faction = NONE, distance = 2239.1295660151, cost = 830},
		[58] = {faction = HORDE, distance = 3885.5380849276, cost = 860},
		[61] = {faction = HORDE, distance = 2460.1923147098, cost = 530},
		[65] = {faction = ALLIANCE, distance = 4067.0463459266, cost = 830},
		[167] = {faction = ALLIANCE, distance = 3215.4037547043, cost = 330},
		[338] = {faction = ALLIANCE, distance = 2868.4352043375, cost = 530},
		[339] = {faction = ALLIANCE, distance = 2320.3784436221, cost = 730},
		[350] = {faction = HORDE, distance = 2084.0153534387, cost = 530},
		[594] = {faction = NEUTRAL, distance = 2928.6265171817, cost = 830},
		[595] = {faction = NEUTRAL, distance = 1270.6279624629, cost = 830},
	},
	[167] = {
		[28] = {faction = ALLIANCE, distance = 4263.2607447162, cost = 330},
		[64] = {faction = ALLIANCE, distance = 847.32961813425, cost = 730},
		[166] = {faction = ALLIANCE, distance = 3313.0602072338, cost = 830},
		[616] = {faction = ALLIANCE, distance = 2452.3005205066, cost = 30000},
	},
	[168] = {
		[2] = {faction = NONE, distance = 468.99880531976, cost = 1},
	},
	[169] = {
		[170] = {faction = NONE, distance = 774.38554974325, cost = 0},
	},
	[171] = {
		[172] = {faction = NONE, distance = 7310.2495635615, cost = 0},
	},
	[172] = {
		[171] = {faction = NONE, distance = 7265.1054659932, cost = 0},
	},
	[173] = {
		[174] = {faction = NONE, distance = 6847.5088489568, cost = 0},
	},
	[175] = {
		[14] = {faction = NONE, distance = 14010.687865862, cost = 0},
		[178] = {faction = NONE, distance = 19132.138583687, cost = 0},
	},
	[176] = {
		[177] = {faction = NONE, distance = 1319.4864134128, cost = 0},
	},
	[178] = {
		[24] = {faction = NONE, distance = 1332.5065070036, cost = 0},
	},
	[179] = {
		[30] = {faction = HORDE, distance = 3010.0006045618, cost = 430},
		[31] = {faction = ALLIANCE, distance = 4310.5357276167, cost = 430},
		[32] = {faction = NONE, distance = 1562.8607536467, cost = 630},
		[55] = {faction = HORDE, distance = 1893.1959089208, cost = 630},
		[389] = {faction = ALLIANCE, distance = 1967.2837641716, cost = 430},
		[391] = {faction = HORDE, distance = 2629.5347038486, cost = 430},
		[513] = {faction = NEUTRAL, distance = 1798.6405766613, cost = 430},
	},
	[180] = {
		[181] = {faction = NONE, distance = 3989.422384791, cost = 0},
	},
	[183] = {
		[184] = {faction = ALLIANCE, distance = 2119.718042619, cost = 4000},
		[185] = {faction = ALLIANCE, distance = 2076.3367706214, cost = 4000},
		[295] = {faction = ALLIANCE, distance = 2867.6335155977, cost = 4000},
		[310] = {faction = ALLIANCE, distance = 8444.0621348651, cost = 20000},
	},
	[184] = {
		[183] = {faction = ALLIANCE, distance = 2209.3612117081, cost = 4000},
		[185] = {faction = ALLIANCE, distance = 2408.9920785068, cost = 4000},
		[253] = {faction = ALLIANCE, distance = 2890.8664502911, cost = 6000},
		[255] = {faction = ALLIANCE, distance = 2945.8722888347, cost = 6000},
	},
	[185] = {
		[183] = {faction = ALLIANCE, distance = 2076.6326167227, cost = 4000},
		[184] = {faction = ALLIANCE, distance = 2574.3425258582, cost = 4000},
		[244] = {faction = ALLIANCE, distance = 4586.5586413861, cost = 4000},
		[253] = {faction = ALLIANCE, distance = 2318.8198180772, cost = 4000},
		[295] = {faction = ALLIANCE, distance = 1563.3893029671, cost = 4000},
	},
	[186] = {
		[187] = {faction = NONE, distance = 588.69867313886, cost = 0},
	},
	[188] = {
		[189] = {faction = NONE, distance = 4456.9145229067, cost = 0},
	},
	[190] = {
		[191] = {faction = HORDE, distance = 2393.7969959977, cost = 4000},
		[192] = {faction = HORDE, distance = 2384.1124252094, cost = 4000},
		[248] = {faction = HORDE, distance = 3102.9570360031, cost = 4000},
		[254] = {faction = HORDE, distance = 5856.0499337312, cost = 12000},
		[295] = {faction = HORDE, distance = 2291.9263455649, cost = 4000},
	},
	[191] = {
		[190] = {faction = HORDE, distance = 2611.2539367973, cost = 4000},
		[192] = {faction = HORDE, distance = 2203.9525974701, cost = 4000},
		[249] = {faction = HORDE, distance = 3133.3459255627, cost = 6000},
		[310] = {faction = HORDE, distance = 8831.3642984546, cost = 20000},
	},
	[192] = {
		[190] = {faction = HORDE, distance = 2380.9485568735, cost = 4000},
		[191] = {faction = HORDE, distance = 2211.9015224938, cost = 4000},
		[248] = {faction = HORDE, distance = 1698.6279789094, cost = 4000},
		[249] = {faction = HORDE, distance = 1740.7792596587, cost = 6000},
		[250] = {faction = HORDE, distance = 2818.720736779, cost = 6000},
	},
	[194] = {
		[193] = {faction = NONE, distance = 4759.6700740204, cost = 0},
	},
	[195] = {
		[2] = {faction = NONE, distance = 2927.7348526522, cost = 630},
		[4] = {faction = NONE, distance = 2084.9662087237, cost = 110},
		[12] = {faction = NONE, distance = 1446.7590043519, cost = 330},
		[19] = {faction = NONE, distance = 3478.3843194806, cost = 630},
		[590] = {faction = NONE, distance = 1609.430411364, cost = 630},
		[622] = {faction = NONE, distance = 1163.1904402575, cost = 220},
	},
	[196] = {
		[35] = {faction = NONE, distance = 12031.510643114, cost = 0},
	},
	[198] = {
		[197] = {faction = NONE, distance = 13251.4538651, cost = 0},
	},
	[199] = {
		[200] = {faction = NONE, distance = 3786.634934632, cost = 0},
	},
	[203] = {
		[261] = {faction = NONE, distance = 3499.7464604891, cost = 0},
	},
	[204] = {
		[254] = {faction = NONE, distance = 10673.934404795, cost = 0},
	},
	[205] = {
		[67] = {faction = ALLIANCE, distance = 7837.3351522411, cost = 1020},
		[68] = {faction = HORDE, distance = 7898.1511410437, cost = 1020},
		[83] = {faction = HORDE, distance = 1569.24708713, cost = 530},
		[213] = {faction = NEUTRAL, distance = 7578.5211846976, cost = 730},
	},
	[209] = {
		[210] = {faction = NONE, distance = 5131.6789999123, cost = 0},
	},
	[211] = {
		[212] = {faction = NONE, distance = 1206.6341103829, cost = 0},
	},
	[212] = {
		[211] = {faction = NONE, distance = 217.92367446402, cost = 0},
	},
	[213] = {
		[67] = {faction = ALLIANCE, distance = 14464.553699842, cost = 1750},
		[82] = {faction = NONE, distance = 5158.852991143, cost = 530},
		[205] = {faction = NEUTRAL, distance = 7005.7809929559, cost = 730},
	},
	[214] = {
		[215] = {faction = NONE, distance = 656.05965207952, cost = 0},
	},
	[221] = {
		[222] = {faction = NONE, distance = 700.28303057554, cost = 0},
	},
	[224] = {
		[178] = {faction = NONE, distance = 2320.5687097758, cost = 0},
	},
	[225] = {
		[226] = {faction = NONE, distance = 2762.4188117858, cost = 0},
	},
	[226] = {
		[289] = {faction = NEUTRAL, distance = 1362.0571901251, cost = 5000},
	},
	[232] = {
		[233] = {faction = NONE, distance = 2857.6688816267, cost = 0},
	},
	[234] = {
		[235] = {faction = NONE, distance = 2123.395510224, cost = 0},
	},
	[235] = {
		[225] = {faction = NONE, distance = 1107.6632495233, cost = 4000},
		[245] = {faction = NONE, distance = 2654.5737171417, cost = 4000},
		[257] = {faction = NONE, distance = 1355.0073756191, cost = 4000},
	},
	[236] = {
		[237] = {faction = NONE, distance = 2464.8276815885, cost = 0},
	},
	[239] = {
		[240] = {faction = NONE, distance = 607.53813943663, cost = 0},
	},
	[241] = {
		[36] = {faction = NONE, distance = 16561.954208524, cost = 0},
	},
	[242] = {
		[243] = {faction = NONE, distance = 3203.6031986564, cost = 0},
	},
	[244] = {
		[185] = {faction = ALLIANCE, distance = 3782.6724539431, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 3457.916848625, cost = 4000},
		[251] = {faction = ALLIANCE, distance = 2890.7776106544, cost = 4000},
		[252] = {faction = ALLIANCE, distance = 1663.071521274, cost = 4000},
		[253] = {faction = ALLIANCE, distance = 2321.5656296625, cost = 4000},
		[294] = {faction = ALLIANCE, distance = 2961.3016790662, cost = 4000},
		[305] = {faction = ALLIANCE, distance = 1919.7598144506, cost = 6000},
		[306] = {faction = ALLIANCE, distance = 2661.9560107003, cost = 6000},
		[336] = {faction = ALLIANCE, distance = 2134.9890389633, cost = 6000},
	},
	[245] = {
		[246] = {faction = ALLIANCE, distance = 2275.5270137219, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 4388.709847405, cost = 6000},
		[289] = {faction = ALLIANCE, distance = 1904.3902372569, cost = 4000},
		[296] = {faction = ALLIANCE, distance = 1914.3480938296, cost = 4000},
		[310] = {faction = ALLIANCE, distance = 7867.8683256431, cost = 20000},
	},
	[246] = {
		[245] = {faction = ALLIANCE, distance = 2119.3607146857, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 4002.6464819329, cost = 6000},
		[289] = {faction = ALLIANCE, distance = 1337.3560926847, cost = 4000},
		[296] = {faction = ALLIANCE, distance = 1938.1714956387, cost = 4000},
		[308] = {faction = ALLIANCE, distance = 1900.5286668947, cost = 6000},
		[309] = {faction = ALLIANCE, distance = 2304.2089424778, cost = 6000},
	},
	[247] = {
		[244] = {faction = ALLIANCE, distance = 3751.673524047, cost = 4000},
		[245] = {faction = ALLIANCE, distance = 3978.5052208512, cost = 6000},
		[246] = {faction = ALLIANCE, distance = 3875.7146496809, cost = 6000},
		[251] = {faction = ALLIANCE, distance = 2463.2590671862, cost = 4000},
		[252] = {faction = ALLIANCE, distance = 2690.6535918059, cost = 4000},
		[294] = {faction = ALLIANCE, distance = 2128.914723555, cost = 4000},
		[296] = {faction = ALLIANCE, distance = 3002.2541876366, cost = 6000},
		[303] = {faction = ALLIANCE, distance = 1980.7370262467, cost = 6000},
	},
	[248] = {
		[190] = {faction = HORDE, distance = 2779.4128553897, cost = 4000},
		[192] = {faction = HORDE, distance = 1789.3502797715, cost = 4000},
		[250] = {faction = HORDE, distance = 1408.9594716493, cost = 4000},
		[254] = {faction = HORDE, distance = 3537.9036011766, cost = 7000},
		[295] = {faction = HORDE, distance = 1620.6417797741, cost = 4000},
	},
	[249] = {
		[191] = {faction = HORDE, distance = 3174.5771860038, cost = 6000},
		[192] = {faction = HORDE, distance = 1457.6935111532, cost = 6000},
		[250] = {faction = HORDE, distance = 2892.6822550871, cost = 4000},
		[304] = {faction = HORDE, distance = 3005.0738205313, cost = 6000},
		[306] = {faction = HORDE, distance = 2779.8958981189, cost = 6000},
		[307] = {faction = HORDE, distance = 2782.5579695961, cost = 6000},
	},
	[250] = {
		[192] = {faction = HORDE, distance = 2549.9747971508, cost = 6000},
		[248] = {faction = HORDE, distance = 1720.6715838784, cost = 6000},
		[249] = {faction = HORDE, distance = 3052.476345472, cost = 4000},
		[254] = {faction = HORDE, distance = 2648.0299087402, cost = 6000},
		[306] = {faction = HORDE, distance = 2405.4312709769, cost = 6000},
	},
	[251] = {
		[244] = {faction = ALLIANCE, distance = 2604.8169619261, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 2221.1357858782, cost = 4000},
		[252] = {faction = ALLIANCE, distance = 1956.674628636, cost = 4000},
		[303] = {faction = ALLIANCE, distance = 1812.9677184713, cost = 6000},
		[305] = {faction = ALLIANCE, distance = 3571.3671899005, cost = 6000},
		[310] = {faction = ALLIANCE, distance = 1965.1935569752, cost = 6000},
	},
	[252] = {
		[244] = {faction = ALLIANCE, distance = 1548.2235313214, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 1999.6623563507, cost = 4000},
		[251] = {faction = ALLIANCE, distance = 1731.8627351913, cost = 4000},
		[254] = {faction = HORDE, distance = 1509.1402235346, cost = 4000},
		[256] = {faction = HORDE, distance = 2095.6109283757, cost = 4000},
		[260] = {faction = HORDE, distance = 1978.520002736, cost = 4000},
		[294] = {faction = NEUTRAL, distance = 1585.4088581445, cost = 4000},
		[305] = {faction = NONE, distance = 3170.6195736512, cost = 6000},
		[310] = {faction = NEUTRAL, distance = 2875.8708314461, cost = 6000},
	},
	[253] = {
		[184] = {faction = ALLIANCE, distance = 3507.8365502111, cost = 6000},
		[185] = {faction = ALLIANCE, distance = 2511.2854187922, cost = 6000},
		[244] = {faction = ALLIANCE, distance = 2469.2188698565, cost = 4000},
		[255] = {faction = ALLIANCE, distance = 2521.4373001533, cost = 4000},
		[306] = {faction = ALLIANCE, distance = 1993.3962237616, cost = 6000},
	},
	[254] = {
		[190] = {faction = HORDE, distance = 5601.5387787607, cost = 8000},
		[248] = {faction = HORDE, distance = 2943.149188547, cost = 7000},
		[250] = {faction = HORDE, distance = 1774.7976973394, cost = 4000},
		[252] = {faction = HORDE, distance = 1543.8873009618, cost = 4000},
		[256] = {faction = HORDE, distance = 3639.4982293375, cost = 8000},
		[260] = {faction = HORDE, distance = 3522.4073036978, cost = 8000},
		[294] = {faction = HORDE, distance = 2461.5031460438, cost = 4000},
		[305] = {faction = HORDE, distance = 2489.984722912, cost = 6000},
		[306] = {faction = HORDE, distance = 3205.2353832829, cost = 6000},
	},
	[255] = {
		[184] = {faction = ALLIANCE, distance = 2566.8262084683, cost = 6000},
		[253] = {faction = ALLIANCE, distance = 2366.7281827088, cost = 4000},
		[304] = {faction = ALLIANCE, distance = 2447.5150127411, cost = 6000},
		[307] = {faction = ALLIANCE, distance = 2187.3915482643, cost = 6000},
	},
	[256] = {
		[252] = {faction = HORDE, distance = 1525.1050708985, cost = 4000},
		[254] = {faction = HORDE, distance = 2618.7315190492, cost = 5000},
		[258] = {faction = HORDE, distance = 3414.5482704377, cost = 6000},
		[260] = {faction = HORDE, distance = 1944.9965220841, cost = 4000},
		[294] = {faction = HORDE, distance = 1911.0486607156, cost = 4000},
		[332] = {faction = HORDE, distance = 3533.3623127617, cost = 6000},
	},
	[257] = {
		[258] = {faction = HORDE, distance = 2600.9134069121, cost = 4000},
		[259] = {faction = HORDE, distance = 2075.711105148, cost = 8000},
		[289] = {faction = HORDE, distance = 1072.1664484551, cost = 4000},
		[296] = {faction = HORDE, distance = 2781.8662466097, cost = 4000},
		[310] = {faction = HORDE, distance = 8135.7316508916, cost = 20000},
	},
	[258] = {
		[256] = {faction = HORDE, distance = 2728.3442120616, cost = 6000},
		[257] = {faction = HORDE, distance = 2552.6882405436, cost = 4000},
		[259] = {faction = HORDE, distance = 2168.9048337278, cost = 4000},
		[289] = {faction = HORDE, distance = 2254.2653875352, cost = 4000},
		[296] = {faction = HORDE, distance = 899.27702600447, cost = 4000},
		[332] = {faction = HORDE, distance = 2130.6048202527, cost = 6000},
	},
	[259] = {
		[257] = {faction = HORDE, distance = 2175.3794400047, cost = 4000},
		[258] = {faction = HORDE, distance = 2327.6962970205, cost = 4000},
		[289] = {faction = HORDE, distance = 1708.1006270373, cost = 4000},
		[308] = {faction = HORDE, distance = 1704.7992619397, cost = 6000},
		[309] = {faction = HORDE, distance = 1425.546313921, cost = 6000},
	},
	[260] = {
		[252] = {faction = HORDE, distance = 2016.5077527081, cost = 4000},
		[254] = {faction = HORDE, distance = 2698.157544514, cost = 4000},
		[256] = {faction = HORDE, distance = 1579.1754546533, cost = 4000},
		[305] = {faction = HORDE, distance = 3207.264224359, cost = 6000},
		[310] = {faction = HORDE, distance = 1669.2126017812, cost = 6000},
	},
	[262] = {
		[263] = {faction = NONE, distance = 2218.6114085104, cost = 0},
	},
	[267] = {
		[268] = {faction = NONE, distance = 2247.9832317727, cost = 0},
	},
	[269] = {
		[270] = {faction = NONE, distance = 4150.0254748015, cost = 0},
	},
	[271] = {
		[272] = {faction = NONE, distance = 2975.1796067024, cost = 0},
	},
	[273] = {
		[274] = {faction = NONE, distance = 826.69393207824, cost = 0},
	},
	[275] = {
		[276] = {faction = NONE, distance = 821.53203306121, cost = 0},
	},
	[277] = {
		[278] = {faction = NONE, distance = 1107.5733731374, cost = 0},
	},
	[280] = {
		[279] = {faction = NONE, distance = 1020.0097314299, cost = 0},
	},
	[282] = {
		[281] = {faction = NONE, distance = 585.21497740044, cost = 0},
	},
	[284] = {
		[283] = {faction = NONE, distance = 689.24075447816, cost = 0},
	},
	[285] = {
		[286] = {faction = NONE, distance = 3199.8114807705, cost = 0},
	},
	[287] = {
		[288] = {faction = NONE, distance = 1974.9950577799, cost = 0},
	},
	[289] = {
		[226] = {faction = NEUTRAL, distance = 1099.8663189207, cost = 5000},
		[245] = {faction = ALLIANCE, distance = 1991.9045470126, cost = 4000},
		[246] = {faction = ALLIANCE, distance = 1047.8979868556, cost = 4000},
		[257] = {faction = HORDE, distance = 1257.8187213628, cost = 4000},
		[258] = {faction = HORDE, distance = 2143.5949126993, cost = 4000},
		[259] = {faction = HORDE, distance = 1003.5446566929, cost = 4000},
	},
	[290] = {
		[291] = {faction = NONE, distance = 658.16841683312, cost = 0},
	},
	[292] = {
		[293] = {faction = NONE, distance = 1963.5750819993, cost = 0},
	},
	[294] = {
		[244] = {faction = ALLIANCE, distance = 2559.2416982247, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 1647.6595835051, cost = 4000},
		[252] = {faction = NEUTRAL, distance = 1455.8052408939, cost = 4000},
		[254] = {faction = HORDE, distance = 1857.0718153421, cost = 4000},
		[256] = {faction = HORDE, distance = 1928.983274876, cost = 4000},
		[295] = {faction = NEUTRAL, distance = 5536.5246593967, cost = 7000},
		[296] = {faction = NEUTRAL, distance = 3963.9230089763, cost = 6000},
		[310] = {faction = NEUTRAL, distance = 3682.9752642574, cost = 6000},
	},
	[295] = {
		[183] = {faction = ALLIANCE, distance = 2421.242032607, cost = 4000},
		[185] = {faction = ALLIANCE, distance = 1092.368954646, cost = 4000},
		[190] = {faction = HORDE, distance = 1892.6155215217, cost = 4000},
		[248] = {faction = HORDE, distance = 1670.9893800566, cost = 4000},
		[294] = {faction = NEUTRAL, distance = 5861.5569388037, cost = 7000},
	},
	[296] = {
		[245] = {faction = ALLIANCE, distance = 1792.4475779898, cost = 4000},
		[246] = {faction = ALLIANCE, distance = 2380.3505351088, cost = 4000},
		[247] = {faction = ALLIANCE, distance = 2936.1687634502, cost = 6000},
		[257] = {faction = HORDE, distance = 2629.0392150599, cost = 4000},
		[258] = {faction = HORDE, distance = 651.9176648946, cost = 4000},
		[294] = {faction = NEUTRAL, distance = 3549.5591076647, cost = 6000},
	},
	[297] = {
		[298] = {faction = NONE, distance = 7471.3681213186, cost = 0},
	},
	[300] = {
		[196] = {faction = NONE, distance = 32.020190979877, cost = 0},
	},
	[301] = {
		[302] = {faction = NONE, distance = 1459.6253719008, cost = 0},
	},
	[303] = {
		[247] = {faction = ALLIANCE, distance = 2421.17934783, cost = 6000},
		[251] = {faction = ALLIANCE, distance = 1473.1729672533, cost = 6000},
		[308] = {faction = ALLIANCE, distance = 4110.1728110936, cost = 6000},
		[310] = {faction = ALLIANCE, distance = 2728.2485998237, cost = 6000},
		[325] = {faction = ALLIANCE, distance = 4986.7279912171, cost = 6000},
		[333] = {faction = ALLIANCE, distance = 4830.9536038627, cost = 6000},
		[335] = {faction = ALLIANCE, distance = 3297.4488806739, cost = 6000},
	},
	[304] = {
		[249] = {faction = HORDE, distance = 2985.7617304863, cost = 6000},
		[255] = {faction = ALLIANCE, distance = 2135.3947112185, cost = 6000},
		[305] = {faction = NONE, distance = 1578.7371116986, cost = 4000},
		[306] = {faction = NEUTRAL, distance = 722.11992968674, cost = 4000},
		[307] = {faction = NEUTRAL, distance = 1236.4980376067, cost = 4000},
	},
	[305] = {
		[244] = {faction = NONE, distance = 1840.1789351934, cost = 6000},
		[251] = {faction = NONE, distance = 3357.6799241718, cost = 6000},
		[252] = {faction = NONE, distance = 2756.0248952956, cost = 6000},
		[254] = {faction = NONE, distance = 2991.0795528282, cost = 6000},
		[260] = {faction = NONE, distance = 3264.5316197264, cost = 6000},
		[304] = {faction = NONE, distance = 1916.6390905151, cost = 4000},
		[306] = {faction = NONE, distance = 1343.7095104701, cost = 4000},
		[310] = {faction = NONE, distance = 2019.2236236481, cost = 6000},
		[320] = {faction = NONE, distance = 1209.4433537472, cost = 6000},
		[336] = {faction = NONE, distance = 1005.8439157746, cost = 6000},
		[337] = {faction = NONE, distance = 806.06953906561, cost = 6000},
	},
	[306] = {
		[244] = {faction = ALLIANCE, distance = 2511.753622708, cost = 6000},
		[249] = {faction = HORDE, distance = 3159.7747976696, cost = 6000},
		[250] = {faction = HORDE, distance = 2240.1514924628, cost = 6000},
		[253] = {faction = ALLIANCE, distance = 2527.1196345731, cost = 6000},
		[254] = {faction = HORDE, distance = 3667.3008177225, cost = 6000},
		[304] = {faction = NEUTRAL, distance = 1311.8724049684, cost = 4000},
		[305] = {faction = NONE, distance = 1193.3067891415, cost = 4000},
	},
	[307] = {
		[249] = {faction = HORDE, distance = 2486.5986226139, cost = 6000},
		[255] = {faction = ALLIANCE, distance = 1687.1393837071, cost = 6000},
		[304] = {faction = NEUTRAL, distance = 1593.5769761858, cost = 4000},
		[322] = {faction = NEUTRAL, distance = 2553.8852362927, cost = 6000},
		[324] = {faction = HORDE, distance = 2587.2143021246, cost = 6000},
		[326] = {faction = NEUTRAL, distance = 4629.9464355587, cost = 6000},
		[331] = {faction = NONE, distance = 1609.9676173521, cost = 4000},
	},
	[308] = {
		[246] = {faction = ALLIANCE, distance = 2082.3052389064, cost = 6000},
		[259] = {faction = HORDE, distance = 1860.28097392, cost = 6000},
		[303] = {faction = ALLIANCE, distance = 4511.4696439748, cost = 6000},
		[309] = {faction = NEUTRAL, distance = 1286.7261370194, cost = 4000},
		[310] = {faction = NEUTRAL, distance = 6051.4379697987, cost = 7000},
		[325] = {faction = NEUTRAL, distance = 2797.3565148305, cost = 6000},
		[332] = {faction = HORDE, distance = 2572.5636373082, cost = 6000},
	},
	[309] = {
		[246] = {faction = ALLIANCE, distance = 2333.2253430164, cost = 6000},
		[259] = {faction = HORDE, distance = 1829.9094663087, cost = 6000},
		[308] = {faction = NEUTRAL, distance = 1530.3257846113, cost = 4000},
		[325] = {faction = NEUTRAL, distance = 2726.477031693, cost = 6000},
	},
	[310] = {
		[183] = {faction = ALLIANCE, distance = 8446.4780173556, cost = 20000},
		[191] = {faction = HORDE, distance = 8828.5550120131, cost = 20000},
		[245] = {faction = ALLIANCE, distance = 7871.371895501, cost = 20000},
		[251] = {faction = ALLIANCE, distance = 3027.6445438581, cost = 6000},
		[252] = {faction = NEUTRAL, distance = 3640.6776709468, cost = 6000},
		[257] = {faction = HORDE, distance = 8137.4774052447, cost = 20000},
		[260] = {faction = HORDE, distance = 2195.4614663086, cost = 6000},
		[294] = {faction = NEUTRAL, distance = 4799.4599301926, cost = 6000},
		[303] = {faction = ALLIANCE, distance = 3459.18638474, cost = 6000},
		[305] = {faction = NONE, distance = 2437.8096996321, cost = 6000},
		[308] = {faction = NEUTRAL, distance = 6382.4504642711, cost = 7000},
		[320] = {faction = NEUTRAL, distance = 1639.7701289754, cost = 6000},
		[332] = {faction = HORDE, distance = 4855.5455965722, cost = 6000},
		[334] = {faction = NEUTRAL, distance = 967.11486931953, cost = 6000},
		[335] = {faction = NEUTRAL, distance = 1181.1470431904, cost = 6000},
		[336] = {faction = ALLIANCE, distance = 1590.1263511715, cost = 4000},
		[337] = {faction = HORDE, distance = 1717.5571514722, cost = 4000},
		[340] = {faction = NEUTRAL, distance = 3682.8386928935, cost = 6000},
		[538] = {faction = NONE, distance = 9268.7475613768, cost = 0},
	},
	[311] = {
		[312] = {faction = NONE, distance = 3657.0338481771, cost = 0},
	},
	[313] = {
		[312] = {faction = NONE, distance = 2825.1289878652, cost = 0},
	},
	[314] = {
		[312] = {faction = NONE, distance = 1675.5817804484, cost = 0},
	},
	[315] = {
		[67] = {faction = ALLIANCE, distance = 1556.2089837357, cost = 730},
		[68] = {faction = HORDE, distance = 1530.8345150499, cost = 730},
	},
	[316] = {
		[317] = {faction = NONE, distance = 332.14014404923, cost = 0},
	},
	[318] = {
		[319] = {faction = NONE, distance = 639.02251768766, cost = 0},
	},
	[320] = {
		[305] = {faction = NONE, distance = 1306.6323972429, cost = 6000},
		[310] = {faction = NEUTRAL, distance = 2148.2907878331, cost = 6000},
		[321] = {faction = ALLIANCE, distance = 1293.9009305416, cost = 4000},
		[322] = {faction = NEUTRAL, distance = 3023.076276532, cost = 4000},
		[323] = {faction = HORDE, distance = 2262.4956090243, cost = 4000},
		[324] = {faction = HORDE, distance = 2704.8763922817, cost = 4000},
		[336] = {faction = ALLIANCE, distance = 1608.3362948561, cost = 6000},
		[337] = {faction = HORDE, distance = 1126.0056023499, cost = 6000},
	},
	[321] = {
		[320] = {faction = ALLIANCE, distance = 1449.706707673, cost = 4000},
		[326] = {faction = ALLIANCE, distance = 2919.0188384762, cost = 4000},
		[327] = {faction = ALLIANCE, distance = 1946.1177095978, cost = 4000},
		[334] = {faction = ALLIANCE, distance = 995.46833694903, cost = 6000},
	},
	[322] = {
		[307] = {faction = NEUTRAL, distance = 2664.2546070382, cost = 6000},
		[320] = {faction = NEUTRAL, distance = 2637.2197676004, cost = 4000},
		[324] = {faction = HORDE, distance = 964.93300513761, cost = 4000},
		[326] = {faction = NEUTRAL, distance = 2527.6128532945, cost = 4000},
	},
	[323] = {
		[320] = {faction = HORDE, distance = 2611.4426884366, cost = 4000},
		[324] = {faction = HORDE, distance = 2899.1812347362, cost = 4000},
		[326] = {faction = HORDE, distance = 1543.3467872295, cost = 4000},
		[327] = {faction = HORDE, distance = 1104.973654682, cost = 4000},
		[334] = {faction = HORDE, distance = 2393.4270582764, cost = 6000},
	},
	[324] = {
		[307] = {faction = HORDE, distance = 2963.684430287, cost = 6000},
		[320] = {faction = HORDE, distance = 3443.9381451327, cost = 4000},
		[322] = {faction = HORDE, distance = 1356.6178586947, cost = 4000},
		[323] = {faction = HORDE, distance = 3009.5640365931, cost = 4000},
		[326] = {faction = HORDE, distance = 2206.3808218407, cost = 4000},
	},
	[325] = {
		[303] = {faction = ALLIANCE, distance = 5178.3633699449, cost = 6000},
		[308] = {faction = NEUTRAL, distance = 3514.0781935266, cost = 6000},
		[309] = {faction = NEUTRAL, distance = 3550.6299699513, cost = 6000},
		[332] = {faction = HORDE, distance = 3426.4641166862, cost = 6000},
		[333] = {faction = NEUTRAL, distance = 2814.0562499061, cost = 4000},
		[335] = {faction = NEUTRAL, distance = 5249.545534129, cost = 4000},
	},
	[326] = {
		[307] = {faction = NEUTRAL, distance = 4600.7262724214, cost = 6000},
		[321] = {faction = ALLIANCE, distance = 3091.8573516721, cost = 4000},
		[322] = {faction = NEUTRAL, distance = 3130.3373914468, cost = 4000},
		[323] = {faction = HORDE, distance = 1302.4043535359, cost = 4000},
		[324] = {faction = HORDE, distance = 2634.5236305389, cost = 4000},
		[327] = {faction = NEUTRAL, distance = 1448.2280757119, cost = 4000},
	},
	[327] = {
		[321] = {faction = ALLIANCE, distance = 2366.9652018777, cost = 4000},
		[323] = {faction = HORDE, distance = 1220.4507931755, cost = 4000},
		[326] = {faction = NEUTRAL, distance = 1331.2255384573, cost = 4000},
		[333] = {faction = NEUTRAL, distance = 3400.4162016654, cost = 6000},
		[340] = {faction = NEUTRAL, distance = 1812.90525876, cost = 6000},
	},
	[331] = {
		[307] = {faction = NONE, distance = 1673.0756618858, cost = 4000},
	},
	[332] = {
		[256] = {faction = HORDE, distance = 3020.8911839826, cost = 6000},
		[258] = {faction = HORDE, distance = 2403.9836203215, cost = 6000},
		[308] = {faction = HORDE, distance = 2310.8431008824, cost = 6000},
		[310] = {faction = HORDE, distance = 4249.9646236856, cost = 6000},
		[325] = {faction = HORDE, distance = 3378.462169835, cost = 6000},
		[333] = {faction = HORDE, distance = 4912.7886657909, cost = 6000},
		[335] = {faction = HORDE, distance = 4795.2926624271, cost = 6000},
	},
	[333] = {
		[303] = {faction = ALLIANCE, distance = 4226.4343825647, cost = 6000},
		[325] = {faction = NEUTRAL, distance = 2322.6328882999, cost = 4000},
		[327] = {faction = NEUTRAL, distance = 3655.5347030841, cost = 6000},
		[332] = {faction = HORDE, distance = 4113.0080649464, cost = 6000},
		[334] = {faction = NEUTRAL, distance = 4038.4433586541, cost = 4000},
		[335] = {faction = NEUTRAL, distance = 3627.8910035826, cost = 4000},
		[340] = {faction = NEUTRAL, distance = 2310.3147252451, cost = 4000},
	},
	[334] = {
		[310] = {faction = NEUTRAL, distance = 955.35894341931, cost = 6000},
		[321] = {faction = ALLIANCE, distance = 1210.4805710461, cost = 6000},
		[323] = {faction = HORDE, distance = 3093.7471720345, cost = 6000},
		[333] = {faction = NEUTRAL, distance = 4453.2285803173, cost = 4000},
		[335] = {faction = NEUTRAL, distance = 831.01751341919, cost = 4000},
	},
	[335] = {
		[303] = {faction = ALLIANCE, distance = 3193.7520181544, cost = 6000},
		[310] = {faction = NEUTRAL, distance = 1939.2393201859, cost = 10000},
		[325] = {faction = NEUTRAL, distance = 5024.8889753236, cost = 4000},
		[332] = {faction = HORDE, distance = 4755.8391411784, cost = 6000},
		[333] = {faction = NEUTRAL, distance = 3705.8983502628, cost = 4000},
		[334] = {faction = NEUTRAL, distance = 983.88037676657, cost = 4000},
		[340] = {faction = NEUTRAL, distance = 2906.8748920843, cost = 4000},
	},
	[336] = {
		[244] = {faction = ALLIANCE, distance = 2279.4543026359, cost = 6000},
		[305] = {faction = ALLIANCE, distance = 1382.8126169942, cost = 6000},
		[310] = {faction = ALLIANCE, distance = 1433.7600544757, cost = 4000},
		[320] = {faction = ALLIANCE, distance = 1397.2367774185, cost = 6000},
	},
	[337] = {
		[305] = {faction = HORDE, distance = 1135.0660257344, cost = 6000},
		[310] = {faction = HORDE, distance = 1649.988728596, cost = 4000},
		[320] = {faction = HORDE, distance = 1002.6616139981, cost = 6000},
	},
	[338] = {
		[28] = {faction = ALLIANCE, distance = 1798.3275615768, cost = 330},
		[33] = {faction = ALLIANCE, distance = 4105.4996932217, cost = 330},
		[166] = {faction = ALLIANCE, distance = 3302.4248213559, cost = 330},
		[339] = {faction = ALLIANCE, distance = 2042.416358, cost = 330},
	},
	[339] = {
		[26] = {faction = ALLIANCE, distance = 2767.9688820055, cost = 330},
		[28] = {faction = ALLIANCE, distance = 2520.854020567, cost = 330},
		[33] = {faction = ALLIANCE, distance = 4877.9070674774, cost = 330},
		[65] = {faction = ALLIANCE, distance = 4091.9126861683, cost = 730},
		[166] = {faction = ALLIANCE, distance = 2435.4565661301, cost = 730},
		[338] = {faction = ALLIANCE, distance = 1949.4092444506, cost = 330},
		[595] = {faction = ALLIANCE, distance = 1730.9933669601, cost = 830},
	},
	[340] = {
		[310] = {faction = NEUTRAL, distance = 4121.287224448, cost = 14000},
		[327] = {faction = NEUTRAL, distance = 1534.6790834763, cost = 6000},
		[333] = {faction = NEUTRAL, distance = 2647.8951006113, cost = 4000},
		[335] = {faction = NEUTRAL, distance = 2182.0479042621, cost = 4000},
	},
	[341] = {
		[342] = {faction = NONE, distance = 763.4757222569, cost = 0},
	},
	[344] = {
		[345] = {faction = NONE, distance = 4959.4000692425, cost = 0},
	},
	[346] = {
		[347] = {faction = NONE, distance = 3318.5731427695, cost = 0},
	},
	[349] = {
		[350] = {faction = NONE, distance = 2051.5244587198, cost = 0},
	},
	[350] = {
		[29] = {faction = HORDE, distance = 3377.7816902459, cost = 330},
		[58] = {faction = HORDE, distance = 1801.5227314889, cost = 330},
		[61] = {faction = HORDE, distance = 3227.5556458119, cost = 330},
		[166] = {faction = HORDE, distance = 2699.5451374103, cost = 530},
		[354] = {faction = HORDE, distance = 3499.956395673, cost = 660},
		[356] = {faction = HORDE, distance = 1415.9545183111, cost = 330},
		[360] = {faction = HORDE, distance = 2564.5002646224, cost = 530},
	},
	[351] = {
		[28] = {faction = ALLIANCE, distance = 1162.4430651959, cost = 330},
		[33] = {faction = ALLIANCE, distance = 2849.9110431909, cost = 530},
		[37] = {faction = ALLIANCE, distance = 2800.8600808666, cost = 530},
		[361] = {faction = ALLIANCE, distance = 1900.1180801033, cost = 530},
		[364] = {faction = ALLIANCE, distance = 1955.741064097, cost = 530},
		[387] = {faction = ALLIANCE, distance = 3065.3781547384, cost = 730},
	},
	[354] = {
		[23] = {faction = HORDE, distance = 3081.0347840538, cost = 530},
		[25] = {faction = HORDE, distance = 1913.1401618446, cost = 330},
		[61] = {faction = HORDE, distance = 1415.1566544386, cost = 530},
		[350] = {faction = HORDE, distance = 2784.7374436746, cost = 530},
		[356] = {faction = HORDE, distance = 2076.3047287066, cost = 330},
		[362] = {faction = HORDE, distance = 4357.8876287703, cost = 530},
		[458] = {faction = HORDE, distance = 1713.2286582023, cost = 110},
	},
	[356] = {
		[61] = {faction = HORDE, distance = 1994.9388347052, cost = 330},
		[350] = {faction = HORDE, distance = 1280.8972888738, cost = 330},
		[354] = {faction = HORDE, distance = 2084.0018773619, cost = 330},
		[362] = {faction = HORDE, distance = 2299.8692787644, cost = 530},
	},
	[358] = {
		[359] = {faction = NONE, distance = 4597.6874258945, cost = 0},
	},
	[360] = {
		[29] = {faction = HORDE, distance = 2087.1838134252, cost = 660},
		[58] = {faction = HORDE, distance = 2477.84953375, cost = 530},
		[350] = {faction = HORDE, distance = 3061.1395085263, cost = 530},
		[362] = {faction = HORDE, distance = 2140.5057242128, cost = 330},
		[363] = {faction = HORDE, distance = 3126.8241658519, cost = 330},
		[540] = {faction = HORDE, distance = 990.02082504122, cost = 330},
	},
	[361] = {
		[33] = {faction = ALLIANCE, distance = 1944.8122936948, cost = 330},
		[37] = {faction = ALLIANCE, distance = 1889.4960360275, cost = 530},
		[351] = {faction = ALLIANCE, distance = 1590.5573202664, cost = 530},
		[364] = {faction = ALLIANCE, distance = 1727.9056868489, cost = 330},
		[365] = {faction = ALLIANCE, distance = 2478.6726214419, cost = 660},
		[541] = {faction = ALLIANCE, distance = 901.90525808086, cost = 330},
	},
	[362] = {
		[29] = {faction = HORDE, distance = 1376.0341402252, cost = 330},
		[354] = {faction = HORDE, distance = 4110.6430874009, cost = 530},
		[356] = {faction = HORDE, distance = 2298.8455323087, cost = 530},
		[360] = {faction = HORDE, distance = 2097.3347720211, cost = 330},
		[363] = {faction = HORDE, distance = 2024.96557195, cost = 330},
		[540] = {faction = HORDE, distance = 1419.2257696888, cost = 330},
	},
	[363] = {
		[22] = {faction = HORDE, distance = 1391.4874828645, cost = 530},
		[25] = {faction = HORDE, distance = 2568.4723460742, cost = 530},
		[29] = {faction = HORDE, distance = 2188.7424665586, cost = 330},
		[38] = {faction = HORDE, distance = 4933.7218676892, cost = 530},
		[360] = {faction = HORDE, distance = 3339.6030610573, cost = 330},
		[362] = {faction = HORDE, distance = 2065.4737668156, cost = 330},
	},
	[364] = {
		[33] = {faction = ALLIANCE, distance = 3415.1525147391, cost = 330},
		[37] = {faction = ALLIANCE, distance = 1998.7255711161, cost = 530},
		[351] = {faction = ALLIANCE, distance = 2394.9531904173, cost = 530},
		[361] = {faction = ALLIANCE, distance = 1811.0923358909, cost = 330},
		[365] = {faction = ALLIANCE, distance = 3557.0519986207, cost = 1060},
		[387] = {faction = ALLIANCE, distance = 1483.1799091053, cost = 730},
	},
	[365] = {
		[33] = {faction = ALLIANCE, distance = 1572.7980048467, cost = 330},
		[37] = {faction = ALLIANCE, distance = 1881.1089051419, cost = 530},
		[41] = {faction = ALLIANCE, distance = 6599.1749625131, cost = 730},
		[361] = {faction = ALLIANCE, distance = 2161.9302278325, cost = 660},
		[364] = {faction = ALLIANCE, distance = 3457.5479469891, cost = 330},
		[541] = {faction = ALLIANCE, distance = 1324.299644677, cost = 330},
	},
	[366] = {
		[29] = {faction = HORDE, distance = 3200.863960428, cost = 530},
		[38] = {faction = HORDE, distance = 2010.156829067, cost = 330},
		[42] = {faction = HORDE, distance = 7462.6678902378, cost = 1030},
		[368] = {faction = HORDE, distance = 1110.6159423667, cost = 330},
		[370] = {faction = HORDE, distance = 267.81481458927, cost = 330},
	},
	[367] = {
		[33] = {faction = ALLIANCE, distance = 4733.2034120838, cost = 530},
		[37] = {faction = ALLIANCE, distance = 3053.8422769564, cost = 330},
		[41] = {faction = ALLIANCE, distance = 3259.7965483042, cost = 660},
		[368] = {faction = ALLIANCE, distance = 1695.3741273282, cost = 330},
		[370] = {faction = ALLIANCE, distance = 1889.6649519203, cost = 330},
		[565] = {faction = ALLIANCE, distance = 1605.4130585042, cost = 330},
	},
	[368] = {
		[29] = {faction = HORDE, distance = 2731.6883174315, cost = 530},
		[33] = {faction = ALLIANCE, distance = 3644.0343673955, cost = 530},
		[37] = {faction = ALLIANCE, distance = 1677.070964715, cost = 330},
		[38] = {faction = HORDE, distance = 2469.682040358, cost = 330},
		[366] = {faction = HORDE, distance = 1145.1309113009, cost = 330},
		[367] = {faction = ALLIANCE, distance = 1461.7525433677, cost = 330},
		[369] = {faction = NEUTRAL, distance = 833.56600566592, cost = 330},
		[370] = {faction = NEUTRAL, distance = 1184.8157643732, cost = 330},
	},
	[369] = {
		[22] = {faction = HORDE, distance = 1379.9803286074, cost = 530},
		[37] = {faction = ALLIANCE, distance = 984.74399774763, cost = 330},
		[368] = {faction = NEUTRAL, distance = 1666.2357055002, cost = 330},
		[370] = {faction = NEUTRAL, distance = 1870.1816925182, cost = 330},
	},
	[370] = {
		[37] = {faction = ALLIANCE, distance = 1991.7597921679, cost = 330},
		[38] = {faction = HORDE, distance = 2409.5011264307, cost = 330},
		[366] = {faction = HORDE, distance = 507.69014411021, cost = 330},
		[367] = {faction = ALLIANCE, distance = 1541.0912602132, cost = 330},
		[368] = {faction = NEUTRAL, distance = 1142.1192601645, cost = 330},
		[369] = {faction = NEUTRAL, distance = 1534.1792182736, cost = 330},
	},
	[373] = {
		[374] = {faction = NONE, distance = 782.87270629521, cost = 0},
	},
	[383] = {
		[11] = {faction = HORDE, distance = 4826.0746583549, cost = 1020},
		[13] = {faction = HORDE, distance = 2987.7944602542, cost = 730},
		[66] = {faction = ALLIANCE, distance = 1729.8781394959, cost = 730},
		[67] = {faction = ALLIANCE, distance = 2995.7053383208, cost = 730},
		[68] = {faction = HORDE, distance = 2987.0327394565, cost = 730},
		[87] = {faction = NEUTRAL, distance = 1338.0042297565, cost = 730},
		[384] = {faction = HORDE, distance = 2279.9448021389, cost = 730},
		[617] = {faction = HORDE, distance = 2568.001747174, cost = 1020},
		[618] = {faction = ALLIANCE, distance = 3267.1456460206, cost = 730},
		[651] = {faction = NEUTRAL, distance = 1157.1745387365, cost = 730},
	},
	[384] = {
		[11] = {faction = HORDE, distance = 2714.8414809014, cost = 730},
		[13] = {faction = HORDE, distance = 2206.4479654315, cost = 730},
		[383] = {faction = HORDE, distance = 2157.5805982091, cost = 730},
		[460] = {faction = HORDE, distance = 1436.0162809053, cost = 110},
		[649] = {faction = HORDE, distance = 1013.4766629946, cost = 730},
		[670] = {faction = HORDE, distance = 1340.387828109, cost = 220},
	},
	[386] = {
		[39] = {faction = ALLIANCE, distance = 2899.5689179231, cost = 730},
		[40] = {faction = HORDE, distance = 2946.6464291459, cost = 730},
		[72] = {faction = HORDE, distance = 2522.1183413541, cost = 830},
		[73] = {faction = ALLIANCE, distance = 2467.674300867, cost = 830},
		[79] = {faction = NEUTRAL, distance = 817.60869896024, cost = 330},
	},
	[387] = {
		[80] = {faction = ALLIANCE, distance = 2527.6520877018, cost = 630},
		[351] = {faction = ALLIANCE, distance = 3285.8899477782, cost = 730},
		[364] = {faction = ALLIANCE, distance = 1735.7542948739, cost = 730},
		[388] = {faction = ALLIANCE, distance = 2991.7543434918, cost = 330},
		[389] = {faction = ALLIANCE, distance = 3074.3515783343, cost = 330},
	},
	[388] = {
		[32] = {faction = ALLIANCE, distance = 2167.9224456684, cost = 630},
		[80] = {faction = ALLIANCE, distance = 1361.7987878155, cost = 630},
		[387] = {faction = ALLIANCE, distance = 3098.7015881661, cost = 330},
		[389] = {faction = ALLIANCE, distance = 2309.0495492603, cost = 330},
	},
	[389] = {
		[179] = {faction = ALLIANCE, distance = 1963.2564633924, cost = 430},
		[387] = {faction = ALLIANCE, distance = 3085.0229807226, cost = 330},
		[388] = {faction = ALLIANCE, distance = 2259.1521390261, cost = 330},
	},
	[390] = {
		[22] = {faction = HORDE, distance = 1978.7212475114, cost = 330},
		[25] = {faction = HORDE, distance = 1284.145417841, cost = 330},
		[77] = {faction = HORDE, distance = 1424.5264023696, cost = 110},
		[391] = {faction = HORDE, distance = 2737.9807214722, cost = 220},
	},
	[391] = {
		[30] = {faction = HORDE, distance = 3024.0178992116, cost = 430},
		[42] = {faction = HORDE, distance = 2924.039687704, cost = 630},
		[55] = {faction = HORDE, distance = 1386.2819885638, cost = 110},
		[77] = {faction = HORDE, distance = 1408.9031846245, cost = 110},
		[179] = {faction = HORDE, distance = 2260.1844904943, cost = 430},
		[390] = {faction = HORDE, distance = 2927.9510146986, cost = 110},
	},
	[392] = {
		[393] = {faction = NONE, distance = 3096.5573774098, cost = 0},
	},
	[394] = {
		[395] = {faction = NONE, distance = 1260.5026968173, cost = 0},
	},
	[402] = {
		[22] = {faction = HORDE, distance = 1492.1471488589, cost = 0},
	},
	[404] = {
		[405] = {faction = NONE, distance = 6262.8955486469, cost = 0},
	},
	[414] = {
		[413] = {faction = NONE, distance = 3655.6347264627, cost = 0},
	},
	[433] = {
		[434] = {faction = NONE, distance = 4301.1756742098, cost = 0},
	},
	[436] = {
		[434] = {faction = NONE, distance = 1568.8760839331, cost = 0},
	},
	[438] = {
		[439] = {faction = NONE, distance = 1166.821366568, cost = 0},
		[440] = {faction = NONE, distance = 349.22004008613, cost = 0},
	},
	[456] = {
		[457] = {faction = ALLIANCE, distance = 1815.542085508, cost = 0},
	},
	[457] = {
		[27] = {faction = ALLIANCE, distance = 3256.1433467705, cost = 0},
		[456] = {faction = ALLIANCE, distance = 1857.2562455372, cost = 0},
	},
	[458] = {
		[23] = {faction = HORDE, distance = 1828.5213641317, cost = 220},
		[25] = {faction = HORDE, distance = 1923.3489930615, cost = 110},
		[354] = {faction = HORDE, distance = 1716.735149021, cost = 110},
	},
	[460] = {
		[11] = {faction = HORDE, distance = 1777.4844802454, cost = 0},
		[384] = {faction = HORDE, distance = 1345.4586187362, cost = 110},
		[645] = {faction = HORDE, distance = 1339.5647650482, cost = 110},
	},
	[513] = {
		[30] = {faction = HORDE, distance = 3952.4445014581, cost = 430},
		[31] = {faction = ALLIANCE, distance = 5413.6885674855, cost = 730},
		[39] = {faction = ALLIANCE, distance = 1294.0172248919, cost = 430},
		[40] = {faction = HORDE, distance = 1166.643985595, cost = 430},
		[179] = {faction = NEUTRAL, distance = 2152.6531859154, cost = 430},
	},
	[521] = {
		[522] = {faction = NEUTRAL, distance = 2494.983969625, cost = 10000},
		[607] = {faction = ALLIANCE, distance = 984.62599105423, cost = 10000},
		[609] = {faction = HORDE, distance = 1277.687292874, cost = 10000},
	},
	[522] = {
		[521] = {faction = NEUTRAL, distance = 2137.3742551171, cost = 10000},
		[523] = {faction = ALLIANCE, distance = 1283.4930380287, cost = 10000},
		[525] = {faction = HORDE, distance = 1444.8209168052, cost = 10000},
		[607] = {faction = ALLIANCE, distance = 1697.5394443477, cost = 10000},
		[609] = {faction = HORDE, distance = 1055.095098183, cost = 10000},
	},
	[523] = {
		[522] = {faction = ALLIANCE, distance = 1295.2538480141, cost = 10000},
		[524] = {faction = ALLIANCE, distance = 2047.6657122857, cost = 10000},
		[611] = {faction = ALLIANCE, distance = 1797.8241946422, cost = 10000},
	},
	[524] = {
		[523] = {faction = ALLIANCE, distance = 1870.1834076293, cost = 10000},
		[611] = {faction = ALLIANCE, distance = 2375.307753962, cost = 10000},
	},
	[525] = {
		[522] = {faction = HORDE, distance = 1494.2048301578, cost = 10000},
		[526] = {faction = HORDE, distance = 2115.3772211831, cost = 10000},
		[612] = {faction = HORDE, distance = 840.7916469229, cost = 10000},
	},
	[526] = {
		[525] = {faction = HORDE, distance = 2309.3943601573, cost = 10000},
		[612] = {faction = HORDE, distance = 2151.9830766233, cost = 10000},
	},
	[531] = {
		[40] = {faction = HORDE, distance = 2796.6224231073, cost = 430},
		[539] = {faction = HORDE, distance = 1842.3353355787, cost = 430},
		[652] = {faction = HORDE, distance = 2174.1334938156, cost = 10000},
	},
	[532] = {
		[39] = {faction = ALLIANCE, distance = 2590.1492484686, cost = 430},
		[539] = {faction = ALLIANCE, distance = 1486.0724890139, cost = 430},
		[652] = {faction = ALLIANCE, distance = 2564.5081308612, cost = 10000},
	},
	[536] = {
		[537] = {faction = HORDE, distance = 1070.6492142149, cost = 0},
	},
	[537] = {
		[23] = {faction = HORDE, distance = 1781.880122126, cost = 0},
		[536] = {faction = HORDE, distance = 1072.9625757499, cost = 0},
	},
	[539] = {
		[39] = {faction = ALLIANCE, distance = 1769.2829457914, cost = 430},
		[40] = {faction = HORDE, distance = 1798.5259901345, cost = 430},
		[531] = {faction = HORDE, distance = 1921.7655207676, cost = 430},
		[532] = {faction = ALLIANCE, distance = 1507.3034637751, cost = 430},
	},
	[540] = {
		[29] = {faction = HORDE, distance = 1097.162988384, cost = 330},
		[360] = {faction = HORDE, distance = 904.74005923985, cost = 330},
		[362] = {faction = HORDE, distance = 1280.453384546, cost = 330},
	},
	[541] = {
		[33] = {faction = ALLIANCE, distance = 1150.5833722253, cost = 330},
		[361] = {faction = ALLIANCE, distance = 837.6305831555, cost = 330},
		[365] = {faction = ALLIANCE, distance = 1576.767363361, cost = 330},
	},
	[551] = {
		[7] = {faction = ALLIANCE, distance = 1570.1237249137, cost = 330},
		[552] = {faction = ALLIANCE, distance = 889.02605315932, cost = 330},
		[553] = {faction = ALLIANCE, distance = 852.5435724386, cost = 330},
	},
	[552] = {
		[551] = {faction = ALLIANCE, distance = 751.44671304074, cost = 330},
		[553] = {faction = ALLIANCE, distance = 816.17847064155, cost = 330},
		[554] = {faction = ALLIANCE, distance = 987.1668173862, cost = 330},
		[658] = {faction = ALLIANCE, distance = 2058.2789792362, cost = 10000},
	},
	[553] = {
		[16] = {faction = ALLIANCE, distance = 1505.0571638085, cost = 530},
		[551] = {faction = ALLIANCE, distance = 962.5416112849, cost = 330},
		[552] = {faction = ALLIANCE, distance = 1067.0171249449, cost = 330},
	},
	[554] = {
		[8] = {faction = ALLIANCE, distance = 1751.0549201258, cost = 330},
		[552] = {faction = ALLIANCE, distance = 969.92453263774, cost = 330},
	},
	[555] = {
		[8] = {faction = ALLIANCE, distance = 1373.3216013682, cost = 110},
		[635] = {faction = ALLIANCE, distance = 1415.3697582549, cost = 830},
	},
	[557] = {
		[558] = {faction = NEUTRAL, distance = 1102.0827135052, cost = 10000},
		[559] = {faction = NEUTRAL, distance = 1940.2425374225, cost = 10000},
		[616] = {faction = NEUTRAL, distance = 1924.1031233086, cost = 10000},
		[781] = {faction = NEUTRAL, distance = 934.48659980311, cost = 10000},
	},
	[558] = {
		[557] = {faction = NEUTRAL, distance = 1275.4817254819, cost = 10000},
		[559] = {faction = NEUTRAL, distance = 2427.7349988266, cost = 10000},
	},
	[559] = {
		[49] = {faction = ALLIANCE, distance = 3477.9007743749, cost = 10000},
		[52] = {faction = ALLIANCE, distance = 3823.7421021259, cost = 10000},
		[53] = {faction = HORDE, distance = 3645.2214665436, cost = 10000},
		[69] = {faction = HORDE, distance = 3972.2900394922, cost = 10000},
		[557] = {faction = NEUTRAL, distance = 1401.6067383152, cost = 10000},
		[558] = {faction = NEUTRAL, distance = 2273.0927963524, cost = 10000},
	},
	[565] = {
		[41] = {faction = ALLIANCE, distance = 1654.3834898, cost = 330},
		[367] = {faction = ALLIANCE, distance = 2020.488284916, cost = 330},
	},
	[567] = {
		[31] = {faction = ALLIANCE, distance = 1796.7158945542, cost = 330},
		[41] = {faction = ALLIANCE, distance = 998.26457160301, cost = 330},
	},
	[568] = {
		[38] = {faction = HORDE, distance = 2049.4288542227, cost = 330},
		[569] = {faction = HORDE, distance = 2312.4304302072, cost = 330},
	},
	[569] = {
		[42] = {faction = HORDE, distance = 2102.4905086156, cost = 630},
		[72] = {faction = HORDE, distance = 3199.8105971714, cost = 630},
		[568] = {faction = HORDE, distance = 1936.8554727265, cost = 630},
	},
	[582] = {
		[2] = {faction = ALLIANCE, distance = 750.74722602862, cost = 0},
		[12] = {faction = ALLIANCE, distance = 1997.4509179978, cost = 110},
		[589] = {faction = ALLIANCE, distance = 1693.8401196327, cost = 110},
	},
	[583] = {
		[4] = {faction = ALLIANCE, distance = 786.06818866016, cost = 110},
	},
	[584] = {
		[2] = {faction = ALLIANCE, distance = 1748.6556123113, cost = 110},
		[4] = {faction = ALLIANCE, distance = 804.32233692725, cost = 110},
	},
	[586] = {
		[587] = {faction = NONE, distance = 1220.8220839001, cost = 0},
	},
	[589] = {
		[5] = {faction = ALLIANCE, distance = 1221.9067746735, cost = 110},
		[582] = {faction = ALLIANCE, distance = 1547.7240269342, cost = 110},
	},
	[590] = {
		[195] = {faction = ALLIANCE, distance = 1618.0157725834, cost = 630},
		[591] = {faction = ALLIANCE, distance = 1292.6108236921, cost = 630},
	},
	[591] = {
		[19] = {faction = ALLIANCE, distance = 1569.5340065876, cost = 630},
		[590] = {faction = ALLIANCE, distance = 1335.53030194, cost = 630},
	},
	[592] = {
		[18] = {faction = HORDE, distance = 1427.1611089801, cost = 630},
		[20] = {faction = HORDE, distance = 1149.5421827528, cost = 630},
		[593] = {faction = HORDE, distance = 2193.7637812553, cost = 630},
	},
	[593] = {
		[20] = {faction = HORDE, distance = 1418.3065359156, cost = 630},
		[592] = {faction = HORDE, distance = 2129.6067761342, cost = 630},
	},
	[594] = {
		[65] = {faction = ALLIANCE, distance = 1755.3394762135, cost = 830},
		[166] = {faction = NEUTRAL, distance = 2678.4419139436, cost = 830},
		[595] = {faction = NEUTRAL, distance = 1807.3340137138, cost = 830},
		[597] = {faction = HORDE, distance = 1198.3242495563, cost = 730},
	},
	[595] = {
		[166] = {faction = NEUTRAL, distance = 1022.1420741359, cost = 830},
		[339] = {faction = ALLIANCE, distance = 1522.0805812643, cost = 830},
		[594] = {faction = NEUTRAL, distance = 2018.3196623358, cost = 830},
	},
	[596] = {
		[599] = {faction = ALLIANCE, distance = 1015.8080344937, cost = 630},
		[615] = {faction = ALLIANCE, distance = 708.60960234926, cost = 330},
	},
	[597] = {
		[48] = {faction = HORDE, distance = 2793.2004038144, cost = 730},
		[53] = {faction = HORDE, distance = 3652.4356337965, cost = 730},
		[69] = {faction = HORDE, distance = 2264.9126089653, cost = 830},
		[594] = {faction = HORDE, distance = 1202.2291326805, cost = 730},
	},
	[598] = {
		[45] = {faction = ALLIANCE, distance = 1295.6559749637, cost = 630},
		[599] = {faction = ALLIANCE, distance = 881.44716241667, cost = 630},
		[600] = {faction = ALLIANCE, distance = 1184.7678214669, cost = 630},
	},
	[599] = {
		[56] = {faction = HORDE, distance = 1245.4828361233, cost = 630},
		[70] = {faction = HORDE, distance = 3281.0130965051, cost = 1020},
		[71] = {faction = ALLIANCE, distance = 2447.8871837206, cost = 1020},
		[596] = {faction = ALLIANCE, distance = 989.84013390594, cost = 630},
		[598] = {faction = ALLIANCE, distance = 740.65460337457, cost = 630},
	},
	[600] = {
		[5] = {faction = NONE, distance = 1373.0243573008, cost = 630},
		[598] = {faction = NONE, distance = 1133.4086602743, cost = 630},
	},
	[601] = {
		[11] = {faction = NONE, distance = 5778.6989469013, cost = 550},
		[13] = {faction = NONE, distance = 1526.0393754025, cost = 220},
		[17] = {faction = NONE, distance = 2215.8023174266, cost = 220},
		[669] = {faction = NONE, distance = 982.51461086545, cost = 220},
	},
	[602] = {
		[19] = {faction = ALLIANCE, distance = 4394.6964151594, cost = 1030},
		[45] = {faction = ALLIANCE, distance = 2173.6624251486, cost = 1030},
	},
	[603] = {
		[18] = {faction = HORDE, distance = 5173.9950356618, cost = 1030},
		[604] = {faction = HORDE, distance = 1781.1358657526, cost = 1030},
	},
	[604] = {
		[56] = {faction = NONE, distance = 1289.4697775403, cost = 1030},
		[603] = {faction = NONE, distance = 1650.4728511407, cost = 1030},
	},
	[605] = {
		[606] = {faction = ALLIANCE, distance = 2098.5585536422, cost = 10000},
	},
	[606] = {
		[6] = {faction = ALLIANCE, distance = 5677.6365360357, cost = 10000},
		[605] = {faction = ALLIANCE, distance = 1979.203456056, cost = 10000},
	},
	[607] = {
		[521] = {faction = ALLIANCE, distance = 912.54718734886, cost = 10000},
		[522] = {faction = ALLIANCE, distance = 1802.4007117391, cost = 10000},
	},
	[608] = {
		[11] = {faction = HORDE, distance = 10791.212017616, cost = 10000},
		[23] = {faction = HORDE, distance = 21135.001483732, cost = 10000},
		[610] = {faction = HORDE, distance = 1348.247974319, cost = 10000},
	},
	[609] = {
		[521] = {faction = HORDE, distance = 1213.6266696146, cost = 10000},
		[522] = {faction = HORDE, distance = 1277.2734511933, cost = 10000},
	},
	[610] = {
		[608] = {faction = HORDE, distance = 1495.6102074469, cost = 10000},
	},
	[611] = {
		[523] = {faction = ALLIANCE, distance = 1350.9994772764, cost = 10000},
		[524] = {faction = ALLIANCE, distance = 2387.2220162027, cost = 10000},
	},
	[612] = {
		[525] = {faction = HORDE, distance = 978.84818234308, cost = 10000},
		[526] = {faction = HORDE, distance = 2279.1416115201, cost = 10000},
	},
	[613] = {
		[23] = {faction = HORDE, distance = 2625.5351773291, cost = 830},
		[44] = {faction = HORDE, distance = 1170.0944718364, cost = 830},
	},
	[614] = {
		[44] = {faction = HORDE, distance = 1469.0560949236, cost = 830},
	},
	[615] = {
		[5] = {faction = ALLIANCE, distance = 681.32759585948, cost = 330},
		[596] = {faction = ALLIANCE, distance = 974.84812458255, cost = 330},
	},
	[616] = {
		[23] = {faction = HORDE, distance = 3320.8501712513, cost = 30000},
		[167] = {faction = ALLIANCE, distance = 1864.316738337, cost = 30000},
		[557] = {faction = NEUTRAL, distance = 1969.400311427, cost = 10000},
		[683] = {faction = NONE, distance = 1641.1472019225, cost = 10000},
	},
	[617] = {
		[13] = {faction = HORDE, distance = 2433.316410174, cost = 730},
		[17] = {faction = HORDE, distance = 2115.4236116126, cost = 730},
		[76] = {faction = HORDE, distance = 2531.9043499479, cost = 730},
		[383] = {faction = HORDE, distance = 2420.5960946197, cost = 730},
		[649] = {faction = HORDE, distance = 2217.2748768456, cost = 730},
	},
	[618] = {
		[16] = {faction = ALLIANCE, distance = 2972.2934440087, cost = 730},
		[43] = {faction = ALLIANCE, distance = 2470.5567020294, cost = 730},
		[67] = {faction = ALLIANCE, distance = 2624.564407871, cost = 730},
		[383] = {faction = ALLIANCE, distance = 3583.8498620331, cost = 730},
	},
	[619] = {
		[6] = {faction = ALLIANCE, distance = 1344.5732065177, cost = 0},
		[620] = {faction = ALLIANCE, distance = 1402.1728312681, cost = 110},
	},
	[620] = {
		[6] = {faction = ALLIANCE, distance = 2080.0889886709, cost = 110},
		[8] = {faction = ALLIANCE, distance = 1689.1232647013, cost = 110},
		[619] = {faction = ALLIANCE, distance = 1290.3899786974, cost = 110},
	},
	[622] = {
		[4] = {faction = ALLIANCE, distance = 1252.9854601452, cost = 220},
		[12] = {faction = ALLIANCE, distance = 1855.1349675135, cost = 220},
		[195] = {faction = ALLIANCE, distance = 1187.7006041825, cost = 220},
	},
	[624] = {
		[94] = {faction = ALLIANCE, distance = 1231.3744591015, cost = 0},
	},
	[625] = {
		[82] = {faction = HORDE, distance = 861.66346505152, cost = 0},
		[83] = {faction = HORDE, distance = 1370.2559524053, cost = 330},
	},
	[630] = {
		[67] = {faction = ALLIANCE, distance = 1165.8836061624, cost = 730},
		[68] = {faction = HORDE, distance = 1165.8836061624, cost = 730},
		[84] = {faction = NEUTRAL, distance = 1781.3807345079, cost = 730},
		[86] = {faction = NEUTRAL, distance = 560.36505319948, cost = 730},
		[87] = {faction = NEUTRAL, distance = 865.21584733831, cost = 730},
	},
	[631] = {
		[82] = {faction = HORDE, distance = 528.25882476222, cost = 0},
	},
	[632] = {
		[21] = {faction = HORDE, distance = 1134.5561744548, cost = 730},
		[635] = {faction = HORDE, distance = 723.15071207085, cost = 730},
	},
	[633] = {
		[8] = {faction = ALLIANCE, distance = 1578.2633859594, cost = 830},
		[634] = {faction = ALLIANCE, distance = 1057.6442339258, cost = 730},
		[635] = {faction = ALLIANCE, distance = 858.06581001561, cost = 730},
	},
	[634] = {
		[71] = {faction = ALLIANCE, distance = 1636.9875988068, cost = 830},
		[633] = {faction = ALLIANCE, distance = 1034.7433845596, cost = 730},
		[635] = {faction = ALLIANCE, distance = 1616.0900767626, cost = 830},
	},
	[635] = {
		[21] = {faction = HORDE, distance = 1493.5569155501, cost = 830},
		[67] = {faction = ALLIANCE, distance = 10778.63087067, cost = 1020},
		[68] = {faction = HORDE, distance = 10783.284230917, cost = 1020},
		[555] = {faction = ALLIANCE, distance = 1446.3490760622, cost = 830},
		[632] = {faction = HORDE, distance = 521.27417959928, cost = 730},
		[633] = {faction = ALLIANCE, distance = 545.46010683606, cost = 730},
		[634] = {faction = ALLIANCE, distance = 1521.426946201, cost = 830},
	},
	[645] = {
		[10] = {faction = HORDE, distance = 1154.5531862046, cost = 110},
		[11] = {faction = HORDE, distance = 2220.3604709674, cost = 110},
		[460] = {faction = HORDE, distance = 1478.5921928987, cost = 110},
		[681] = {faction = HORDE, distance = 800.1293624117, cost = 110},
	},
	[646] = {
		[10] = {faction = HORDE, distance = 2014.9917273924, cost = 220},
		[654] = {faction = HORDE, distance = 1156.6423938663, cost = 220},
	},
	[649] = {
		[384] = {faction = HORDE, distance = 1086.7778562491, cost = 730},
		[617] = {faction = HORDE, distance = 2705.3017050195, cost = 1020},
		[651] = {faction = HORDE, distance = 434.31369717014, cost = 730},
	},
	[650] = {
		[43] = {faction = ALLIANCE, distance = 2398.5818208794, cost = 730},
		[66] = {faction = ALLIANCE, distance = 551.59724590497, cost = 730},
		[651] = {faction = ALLIANCE, distance = 740.30787708564, cost = 730},
	},
	[651] = {
		[66] = {faction = ALLIANCE, distance = 1294.436495303, cost = 730},
		[383] = {faction = NEUTRAL, distance = 1085.5187846037, cost = 1020},
		[649] = {faction = HORDE, distance = 784.19488992559, cost = 730},
		[650] = {faction = ALLIANCE, distance = 845.13588766288, cost = 730},
		[672] = {faction = NEUTRAL, distance = 1312.1915133521, cost = 730},
	},
	[652] = {
		[531] = {faction = HORDE, distance = 2015.9660603825, cost = 10000},
		[532] = {faction = ALLIANCE, distance = 2386.7568914317, cost = 10000},
		[653] = {faction = NEUTRAL, distance = 2496.6374910223, cost = 10000},
		[674] = {faction = NEUTRAL, distance = 2815.5475867529, cost = 10000},
	},
	[653] = {
		[72] = {faction = HORDE, distance = 2161.2153924416, cost = 10000},
		[73] = {faction = ALLIANCE, distance = 2039.5441607734, cost = 10000},
		[652] = {faction = NEUTRAL, distance = 2430.5065829027, cost = 10000},
		[674] = {faction = NEUTRAL, distance = 2555.1819787216, cost = 10000},
	},
	[654] = {
		[10] = {faction = HORDE, distance = 1045.3925060083, cost = 220},
		[646] = {faction = HORDE, distance = 909.06154990495, cost = 220},
		[668] = {faction = HORDE, distance = 1125.626876637, cost = 220},
	},
	[656] = {
		[657] = {faction = HORDE, distance = 1607.5946889261, cost = 10000},
		[659] = {faction = HORDE, distance = 1387.9822259463, cost = 10000},
		[661] = {faction = HORDE, distance = 2394.3081265565, cost = 10000},
	},
	[657] = {
		[656] = {faction = HORDE, distance = 1704.6181954874, cost = 10000},
		[658] = {faction = HORDE, distance = 920.75257985909, cost = 10000},
		[659] = {faction = HORDE, distance = 1054.2660271297, cost = 10000},
	},
	[658] = {
		[6] = {faction = ALLIANCE, distance = 6913.4066541615, cost = 10000},
		[16] = {faction = ALLIANCE, distance = 4151.4727539334, cost = 10000},
		[17] = {faction = HORDE, distance = 4927.0850180525, cost = 10000},
		[21] = {faction = HORDE, distance = 5565.0099751571, cost = 10000},
		[552] = {faction = ALLIANCE, distance = 1969.8841833968, cost = 10000},
		[657] = {faction = HORDE, distance = 799.38275690088, cost = 10000},
		[665] = {faction = ALLIANCE, distance = 1255.0574555933, cost = 10000},
	},
	[659] = {
		[656] = {faction = HORDE, distance = 1750.8809192056, cost = 10000},
		[657] = {faction = HORDE, distance = 1464.2254253735, cost = 10000},
		[660] = {faction = HORDE, distance = 1469.6275781575, cost = 10000},
		[661] = {faction = HORDE, distance = 1666.7282497149, cost = 10000},
	},
	[660] = {
		[659] = {faction = HORDE, distance = 1630.9695286314, cost = 10000},
		[661] = {faction = HORDE, distance = 1433.9271245257, cost = 10000},
	},
	[661] = {
		[17] = {faction = HORDE, distance = 7130.3573862969, cost = 30000},
		[21] = {faction = HORDE, distance = 6548.9599362241, cost = 30000},
		[656] = {faction = HORDE, distance = 1945.8245664793, cost = 10000},
		[659] = {faction = HORDE, distance = 1320.8841811679, cost = 10000},
		[660] = {faction = HORDE, distance = 1592.2135632357, cost = 10000},
	},
	[662] = {
		[8] = {faction = ALLIANCE, distance = 5113.5894567371, cost = 30000},
		[664] = {faction = ALLIANCE, distance = 1505.8494498677, cost = 10000},
		[666] = {faction = ALLIANCE, distance = 2766.4027698621, cost = 10000},
	},
	[663] = {
		[664] = {faction = ALLIANCE, distance = 1187.3683733397, cost = 10000},
		[665] = {faction = ALLIANCE, distance = 1259.0138915787, cost = 10000},
	},
	[664] = {
		[662] = {faction = ALLIANCE, distance = 1599.4309052442, cost = 10000},
		[663] = {faction = ALLIANCE, distance = 1233.6977361694, cost = 10000},
		[665] = {faction = ALLIANCE, distance = 1679.2540957798, cost = 10000},
		[666] = {faction = ALLIANCE, distance = 2188.1350330902, cost = 10000},
	},
	[665] = {
		[658] = {faction = ALLIANCE, distance = 1281.2140486318, cost = 10000},
		[663] = {faction = ALLIANCE, distance = 1564.3360132408, cost = 10000},
		[664] = {faction = ALLIANCE, distance = 1731.3533138779, cost = 10000},
		[666] = {faction = ALLIANCE, distance = 691.75302543085, cost = 10000},
	},
	[666] = {
		[662] = {faction = ALLIANCE, distance = 2955.4431405824, cost = 10000},
		[664] = {faction = ALLIANCE, distance = 2016.8948202368, cost = 10000},
		[665] = {faction = ALLIANCE, distance = 848.92159257574, cost = 10000},
	},
	[667] = {
		[13] = {faction = HORDE, distance = 836.11955567069, cost = 220},
		[668] = {faction = HORDE, distance = 1326.6383574505, cost = 220},
		[669] = {faction = HORDE, distance = 575.33834078435, cost = 220},
	},
	[668] = {
		[654] = {faction = HORDE, distance = 1170.6552357144, cost = 220},
		[667] = {faction = HORDE, distance = 1091.426604592, cost = 220},
	},
	[669] = {
		[13] = {faction = HORDE, distance = 625.74758488726, cost = 220},
		[601] = {faction = HORDE, distance = 937.45864766673, cost = 220},
		[667] = {faction = HORDE, distance = 726.31015355028, cost = 220},
	},
	[670] = {
		[13] = {faction = HORDE, distance = 953.60196545146, cost = 220},
		[384] = {faction = HORDE, distance = 1447.7494592545, cost = 220},
	},
	[672] = {
		[84] = {faction = NEUTRAL, distance = 1833.3878316579, cost = 730},
		[651] = {faction = NEUTRAL, distance = 1335.5538312049, cost = 730},
	},
	[673] = {
		[74] = {faction = ALLIANCE, distance = 739.7425690497, cost = 730},
		[75] = {faction = HORDE, distance = 709.07331574739, cost = 730},
		[675] = {faction = NEUTRAL, distance = 1402.6505531138, cost = 730},
	},
	[674] = {
		[652] = {faction = NEUTRAL, distance = 2637.4600432022, cost = 10000},
		[653] = {faction = NEUTRAL, distance = 2598.8258636253, cost = 10000},
	},
	[675] = {
		[673] = {faction = NEUTRAL, distance = 1328.7062243043, cost = 730},
		[676] = {faction = NEUTRAL, distance = 1053.8872772195, cost = 830},
	},
	[676] = {
		[70] = {faction = HORDE, distance = 479.13151704453, cost = 830},
		[71] = {faction = ALLIANCE, distance = 1150.0229086842, cost = 830},
		[675] = {faction = NEUTRAL, distance = 1012.4509937649, cost = 830},
	},
	[681] = {
		[10] = {faction = HORDE, distance = 875.65185742828, cost = 110},
		[645] = {faction = HORDE, distance = 705.18931924752, cost = 110},
	},
	[683] = {
		[23] = {faction = NONE, distance = 2250.5876789286, cost = 830},
		[44] = {faction = NONE, distance = 2569.0296705579, cost = 830},
		[61] = {faction = NONE, distance = 2365.7028316221, cost = 830},
		[616] = {faction = NONE, distance = 2148.781423465, cost = 10000},
	},
	[781] = {
		[557] = {faction = NEUTRAL, distance = 908.87486974604, cost = 10000},
	},
	[894] = {
		[895] = {faction = NONE, distance = 1881.9986695328, cost = 11000},
		[971] = {faction = NONE, distance = 1722.0578756021, cost = 11000},
		[973] = {faction = NONE, distance = 1822.101491606, cost = 11000},
		[984] = {faction = NONE, distance = 1181.6785431035, cost = 11000},
		[1080] = {faction = NONE, distance = 1816.1436396818, cost = 11000},
	},
	[895] = {
		[894] = {faction = NONE, distance = 2131.0846997458, cost = 11000},
		[966] = {faction = ALLIANCE, distance = 2315.7762150943, cost = 11000},
		[967] = {faction = NEUTRAL, distance = 816.86185404177, cost = 11000},
		[968] = {faction = NEUTRAL, distance = 1044.6187095707, cost = 11000},
		[970] = {faction = NEUTRAL, distance = 1166.062051125, cost = 11000},
		[971] = {faction = NEUTRAL, distance = 1463.7283509907, cost = 11000},
		[972] = {faction = ALLIANCE, distance = 2072.6598348032, cost = 11000},
		[973] = {faction = HORDE, distance = 2656.5852524978, cost = 11000},
		[984] = {faction = NONE, distance = 2262.413717867, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 4871.6777508085, cost = 22000},
		[1058] = {faction = HORDE, distance = 5029.9738453909, cost = 11000},
		[1080] = {faction = NEUTRAL, distance = 1141.1536822096, cost = 11000},
		[1293] = {faction = ALLIANCE, distance = 4288.2784569625, cost = 11000},
		[1294] = {faction = HORDE, distance = 4324.4110668425, cost = 11000},
	},
	[906] = {
		[907] = {faction = NONE, distance = 652.78882634069, cost = 0},
	},
	[966] = {
		[895] = {faction = ALLIANCE, distance = 2889.5341537922, cost = 11000},
		[968] = {faction = ALLIANCE, distance = 1957.0741490269, cost = 11000},
		[972] = {faction = ALLIANCE, distance = 1124.0425908933, cost = 11000},
		[986] = {faction = ALLIANCE, distance = 1454.9390688973, cost = 11000},
		[1017] = {faction = ALLIANCE, distance = 3294.3954408089, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 3934.3417594783, cost = 11000},
		[1190] = {faction = ALLIANCE, distance = 1148.1112134833, cost = 11000},
	},
	[967] = {
		[895] = {faction = NEUTRAL, distance = 783.89231743085, cost = 11000},
		[968] = {faction = NEUTRAL, distance = 898.69333891325, cost = 11000},
		[969] = {faction = NEUTRAL, distance = 1494.2126184338, cost = 11000},
	},
	[968] = {
		[895] = {faction = NEUTRAL, distance = 1350.4357419266, cost = 11000},
		[966] = {faction = ALLIANCE, distance = 1697.8721265232, cost = 11000},
		[967] = {faction = NEUTRAL, distance = 1133.9924321908, cost = 11000},
		[972] = {faction = ALLIANCE, distance = 1067.9218483661, cost = 11000},
	},
	[969] = {
		[967] = {faction = NEUTRAL, distance = 1470.7762827945, cost = 11000},
		[970] = {faction = NEUTRAL, distance = 614.11205892859, cost = 11000},
	},
	[970] = {
		[895] = {faction = NEUTRAL, distance = 1115.7679766972, cost = 11000},
		[969] = {faction = NEUTRAL, distance = 648.88385567693, cost = 11000},
		[971] = {faction = NEUTRAL, distance = 630.34510870222, cost = 11000},
	},
	[971] = {
		[894] = {faction = NONE, distance = 1963.2149637672, cost = 11000},
		[895] = {faction = NEUTRAL, distance = 1250.534476502, cost = 11000},
		[970] = {faction = NEUTRAL, distance = 637.73562145078, cost = 11000},
		[973] = {faction = HORDE, distance = 1600.3951747193, cost = 11000},
	},
	[972] = {
		[895] = {faction = ALLIANCE, distance = 2241.583918074, cost = 11000},
		[966] = {faction = ALLIANCE, distance = 1395.2358163077, cost = 11000},
		[968] = {faction = ALLIANCE, distance = 1085.407035587, cost = 11000},
		[1080] = {faction = ALLIANCE, distance = 1324.7287904229, cost = 11000},
	},
	[973] = {
		[894] = {faction = HORDE, distance = 1907.8045318789, cost = 11000},
		[895] = {faction = HORDE, distance = 2305.8658702382, cost = 11000},
		[971] = {faction = HORDE, distance = 1271.8114217704, cost = 11000},
		[1017] = {faction = HORDE, distance = 1827.3321706449, cost = 11000},
		[1058] = {faction = HORDE, distance = 3803.2108433077, cost = 11000},
	},
	[984] = {
		[894] = {faction = NONE, distance = 1052.8549948562, cost = 11000},
		[895] = {faction = NONE, distance = 1665.4949444816, cost = 11000},
		[985] = {faction = NONE, distance = 1557.7043394551, cost = 11000},
		[986] = {faction = NONE, distance = 1078.0271546422, cost = 11000},
		[987] = {faction = NONE, distance = 1928.4051049935, cost = 11000},
		[1029] = {faction = NONE, distance = 842.5786825186, cost = 11000},
		[1052] = {faction = NONE, distance = 742.20749231594, cost = 11000},
		[1057] = {faction = NONE, distance = 2609.2640329415, cost = 11000},
		[1058] = {faction = NONE, distance = 2796.3255586263, cost = 22000},
	},
	[985] = {
		[984] = {faction = NONE, distance = 1412.8389357802, cost = 11000},
		[986] = {faction = NEUTRAL, distance = 1201.3430059321, cost = 11000},
		[987] = {faction = NONE, distance = 988.944484326, cost = 11000},
		[988] = {faction = ALLIANCE, distance = 1221.6954177624, cost = 11000},
		[989] = {faction = NEUTRAL, distance = 1563.6969475223, cost = 11000},
		[991] = {faction = NONE, distance = 1987.9035097432, cost = 11000},
		[992] = {faction = NEUTRAL, distance = 2331.884052408, cost = 11000},
		[1052] = {faction = NEUTRAL, distance = 962.20414140603, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 2082.2321874528, cost = 11000},
		[1058] = {faction = HORDE, distance = 2161.2380290033, cost = 11000},
	},
	[986] = {
		[966] = {faction = ALLIANCE, distance = 1177.5598108519, cost = 11000},
		[984] = {faction = NONE, distance = 994.73005710042, cost = 11000},
		[985] = {faction = NEUTRAL, distance = 1367.5493842574, cost = 11000},
		[987] = {faction = NONE, distance = 1278.6203966353, cost = 11000},
		[1017] = {faction = NEUTRAL, distance = 2605.9372535731, cost = 11000},
	},
	[987] = {
		[984] = {faction = NONE, distance = 1802.8585720981, cost = 11000},
		[985] = {faction = NONE, distance = 955.76221742907, cost = 11000},
		[986] = {faction = NONE, distance = 1133.7637728319, cost = 11000},
		[990] = {faction = NONE, distance = 2391.6046544854, cost = 11000},
		[993] = {faction = NONE, distance = 2106.7388421926, cost = 11000},
	},
	[988] = {
		[985] = {faction = ALLIANCE, distance = 1307.5802183788, cost = 11000},
		[989] = {faction = ALLIANCE, distance = 2654.03553585, cost = 11000},
		[991] = {faction = ALLIANCE, distance = 2286.9569122976, cost = 11000},
		[992] = {faction = ALLIANCE, distance = 2285.5165249676, cost = 11000},
		[993] = {faction = ALLIANCE, distance = 1884.2594130481, cost = 11000},
		[1190] = {faction = ALLIANCE, distance = 1526.4774827476, cost = 11000},
	},
	[989] = {
		[985] = {faction = NEUTRAL, distance = 1490.6780724797, cost = 11000},
		[988] = {faction = ALLIANCE, distance = 2654.4871985273, cost = 11000},
		[990] = {faction = HORDE, distance = 1447.5556034408, cost = 11000},
		[991] = {faction = NONE, distance = 909.66213581943, cost = 11000},
		[992] = {faction = NEUTRAL, distance = 1876.5141714008, cost = 11000},
		[1070] = {faction = NEUTRAL, distance = 1903.4381784902, cost = 11000},
		[1195] = {faction = HORDE, distance = 1506.4123849684, cost = 11000},
	},
	[990] = {
		[987] = {faction = HORDE, distance = 1982.3966661756, cost = 11000},
		[989] = {faction = HORDE, distance = 1546.4662194665, cost = 11000},
		[992] = {faction = HORDE, distance = 532.8238762739, cost = 11000},
		[993] = {faction = HORDE, distance = 1496.3325884028, cost = 11000},
		[1070] = {faction = HORDE, distance = 3095.0101211949, cost = 11000},
		[1195] = {faction = HORDE, distance = 1721.8419489609, cost = 11000},
	},
	[991] = {
		[985] = {faction = NONE, distance = 2008.3638163796, cost = 11000},
		[988] = {faction = NONE, distance = 2251.8349242718, cost = 11000},
		[989] = {faction = NONE, distance = 1360.6992468517, cost = 11000},
		[992] = {faction = NONE, distance = 1122.8532675608, cost = 11000},
		[993] = {faction = NONE, distance = 2219.2871276075, cost = 11000},
		[1070] = {faction = NONE, distance = 2186.99465316, cost = 11000},
	},
	[992] = {
		[985] = {faction = NEUTRAL, distance = 2413.0893111156, cost = 11000},
		[988] = {faction = ALLIANCE, distance = 2389.479711539, cost = 11000},
		[989] = {faction = NEUTRAL, distance = 1798.4066668739, cost = 11000},
		[990] = {faction = HORDE, distance = 434.52477988838, cost = 11000},
		[991] = {faction = NONE, distance = 1060.4200381718, cost = 11000},
		[993] = {faction = NEUTRAL, distance = 1274.6428466697, cost = 11000},
	},
	[993] = {
		[987] = {faction = NONE, distance = 1824.8661067302, cost = 11000},
		[988] = {faction = ALLIANCE, distance = 1858.4791693259, cost = 11000},
		[990] = {faction = HORDE, distance = 1916.1048493459, cost = 11000},
		[991] = {faction = NONE, distance = 2574.5404597025, cost = 11000},
		[992] = {faction = NEUTRAL, distance = 1531.1029715711, cost = 11000},
	},
	[1017] = {
		[966] = {faction = ALLIANCE, distance = 3127.2354364881, cost = 11000},
		[973] = {faction = HORDE, distance = 1709.4504733365, cost = 11000},
		[986] = {faction = NEUTRAL, distance = 2552.7706980884, cost = 11000},
		[1018] = {faction = NEUTRAL, distance = 1960.8056574627, cost = 11000},
		[1019] = {faction = HORDE, distance = 1007.8333698307, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 1517.2376731029, cost = 11000},
		[1022] = {faction = NEUTRAL, distance = 1796.4531708701, cost = 11000},
		[1029] = {faction = NEUTRAL, distance = 1170.2262075502, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 2905.8681588795, cost = 11000},
		[1058] = {faction = HORDE, distance = 2506.5425670635, cost = 11000},
	},
	[1018] = {
		[1017] = {faction = NEUTRAL, distance = 2223.7732394276, cost = 11000},
		[1019] = {faction = HORDE, distance = 1539.1183756722, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 1817.4460308124, cost = 11000},
		[1021] = {faction = NEUTRAL, distance = 1184.4846774646, cost = 11000},
		[1022] = {faction = NEUTRAL, distance = 920.97075870961, cost = 11000},
	},
	[1019] = {
		[1017] = {faction = HORDE, distance = 884.57352614581, cost = 11000},
		[1018] = {faction = HORDE, distance = 1761.3997202365, cost = 11000},
		[1021] = {faction = HORDE, distance = 2628.1719950802, cost = 11000},
		[1022] = {faction = HORDE, distance = 1411.7936729913, cost = 11000},
		[1023] = {faction = HORDE, distance = 1836.7584673976, cost = 11000},
		[1024] = {faction = HORDE, distance = 1482.9233659403, cost = 11000},
		[1058] = {faction = HORDE, distance = 1814.7530579725, cost = 11000},
	},
	[1020] = {
		[1017] = {faction = ALLIANCE, distance = 1308.7313261678, cost = 11000},
		[1018] = {faction = ALLIANCE, distance = 1875.6052643616, cost = 11000},
		[1021] = {faction = ALLIANCE, distance = 2920.7891462891, cost = 11000},
		[1022] = {faction = ALLIANCE, distance = 1389.9023141832, cost = 11000},
		[1023] = {faction = ALLIANCE, distance = 1142.0690550149, cost = 11000},
		[1024] = {faction = ALLIANCE, distance = 817.12567186893, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 1933.04864206, cost = 11000},
	},
	[1021] = {
		[1018] = {faction = NEUTRAL, distance = 1298.4332915092, cost = 11000},
		[1019] = {faction = HORDE, distance = 2449.9807081965, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 2766.2345204221, cost = 11000},
		[1022] = {faction = NEUTRAL, distance = 1833.1989203733, cost = 11000},
		[1221] = {faction = ALLIANCE, distance = 1463.8655720765, cost = 11000},
		[1222] = {faction = HORDE, distance = 1512.6806785039, cost = 11000},
	},
	[1022] = {
		[1017] = {faction = NEUTRAL, distance = 1995.7727949673, cost = 11000},
		[1018] = {faction = NEUTRAL, distance = 689.53815858717, cost = 11000},
		[1019] = {faction = HORDE, distance = 1379.0079907666, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 1656.4050348957, cost = 11000},
		[1021] = {faction = NEUTRAL, distance = 1730.8870516786, cost = 11000},
		[1023] = {faction = NEUTRAL, distance = 1540.9415294163, cost = 11000},
	},
	[1023] = {
		[1019] = {faction = HORDE, distance = 1740.6467542994, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 1088.9330311883, cost = 11000},
		[1022] = {faction = NEUTRAL, distance = 1229.6293795774, cost = 11000},
		[1024] = {faction = NEUTRAL, distance = 880.36744605728, cost = 11000},
		[1025] = {faction = NEUTRAL, distance = 834.80823890352, cost = 11000},
	},
	[1024] = {
		[1019] = {faction = HORDE, distance = 1345.4749740492, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 746.07784683234, cost = 11000},
		[1023] = {faction = NEUTRAL, distance = 898.61229359931, cost = 11000},
		[1053] = {faction = NEUTRAL, distance = 1563.5107692013, cost = 11000},
		[1054] = {faction = NEUTRAL, distance = 1126.099149134, cost = 11000},
	},
	[1025] = {
		[1023] = {faction = NEUTRAL, distance = 778.36968411647, cost = 11000},
		[1053] = {faction = NEUTRAL, distance = 994.24515454267, cost = 11000},
		[1073] = {faction = NEUTRAL, distance = 2988.9325242209, cost = 11000},
	},
	[1029] = {
		[984] = {faction = NONE, distance = 626.98771265431, cost = 11000},
		[1017] = {faction = NEUTRAL, distance = 1134.8620872026, cost = 11000},
		[1052] = {faction = NEUTRAL, distance = 579.82925463049, cost = 11000},
	},
	[1052] = {
		[984] = {faction = NONE, distance = 878.2167433416, cost = 11000},
		[985] = {faction = NEUTRAL, distance = 940.50540107594, cost = 11000},
		[1029] = {faction = NEUTRAL, distance = 458.07114874476, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 1921.2192786703, cost = 11000},
		[1058] = {faction = HORDE, distance = 2054.1180663103, cost = 11000},
	},
	[1053] = {
		[1024] = {faction = NEUTRAL, distance = 1204.3704888053, cost = 11000},
		[1025] = {faction = NEUTRAL, distance = 1006.5311044296, cost = 11000},
		[1054] = {faction = NEUTRAL, distance = 1171.3115695825, cost = 11000},
		[1055] = {faction = NEUTRAL, distance = 1422.9620034471, cost = 11000},
		[1117] = {faction = HORDE, distance = 925.60753311493, cost = 11000},
	},
	[1054] = {
		[1024] = {faction = NEUTRAL, distance = 1159.1280818041, cost = 11000},
		[1053] = {faction = NEUTRAL, distance = 1348.4369974108, cost = 11000},
		[1055] = {faction = NEUTRAL, distance = 1507.6298847558, cost = 11000},
		[1072] = {faction = NEUTRAL, distance = 970.50259209194, cost = 11000},
		[1117] = {faction = HORDE, distance = 1014.6076924014, cost = 11000},
	},
	[1055] = {
		[1053] = {faction = NEUTRAL, distance = 1435.286492462, cost = 11000},
		[1054] = {faction = NEUTRAL, distance = 1301.9982361717, cost = 11000},
		[1056] = {faction = NEUTRAL, distance = 654.90773987134, cost = 11000},
	},
	[1056] = {
		[1055] = {faction = NEUTRAL, distance = 389.71333743952, cost = 11000},
	},
	[1057] = {
		[895] = {faction = ALLIANCE, distance = 4044.992023513, cost = 22000},
		[966] = {faction = ALLIANCE, distance = 3715.377535614, cost = 11000},
		[984] = {faction = ALLIANCE, distance = 2379.4970790313, cost = 11000},
		[985] = {faction = ALLIANCE, distance = 2006.6073889682, cost = 11000},
		[1017] = {faction = ALLIANCE, distance = 2706.6758809952, cost = 11000},
		[1020] = {faction = ALLIANCE, distance = 1907.8864027065, cost = 11000},
		[1052] = {faction = ALLIANCE, distance = 1864.6782901887, cost = 11000},
		[1070] = {faction = ALLIANCE, distance = 3384.0454991335, cost = 11000},
		[1072] = {faction = ALLIANCE, distance = 3350.4026613989, cost = 11000},
		[1073] = {faction = ALLIANCE, distance = 2036.4175888358, cost = 11000},
		[1115] = {faction = ALLIANCE, distance = 2454.4447069479, cost = 11000},
		[1221] = {faction = ALLIANCE, distance = 6022.664136216, cost = 11000},
	},
	[1058] = {
		[895] = {faction = HORDE, distance = 4404.5687823617, cost = 22000},
		[973] = {faction = HORDE, distance = 3999.5473171035, cost = 11000},
		[984] = {faction = HORDE, distance = 2739.0738378801, cost = 11000},
		[985] = {faction = HORDE, distance = 2203.1221053711, cost = 11000},
		[1017] = {faction = HORDE, distance = 2519.9390506666, cost = 11000},
		[1019] = {faction = HORDE, distance = 2016.5798217863, cost = 11000},
		[1052] = {faction = HORDE, distance = 2223.5514537651, cost = 11000},
		[1070] = {faction = HORDE, distance = 3219.0061118752, cost = 11000},
		[1072] = {faction = HORDE, distance = 3207.6416141571, cost = 11000},
		[1073] = {faction = HORDE, distance = 1883.8811000869, cost = 11000},
		[1117] = {faction = HORDE, distance = 2510.7035337965, cost = 11000},
		[1222] = {faction = HORDE, distance = 5778.0475526239, cost = 11000},
	},
	[1070] = {
		[989] = {faction = NEUTRAL, distance = 1447.0245853224, cost = 11000},
		[990] = {faction = HORDE, distance = 2894.5801887632, cost = 22000},
		[991] = {faction = NONE, distance = 2085.3357277524, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 3265.8430564166, cost = 11000},
		[1058] = {faction = HORDE, distance = 2892.1007323956, cost = 11000},
		[1071] = {faction = NEUTRAL, distance = 1430.9142732813, cost = 11000},
		[1072] = {faction = NEUTRAL, distance = 1152.2943402643, cost = 11000},
		[1073] = {faction = NEUTRAL, distance = 1239.5893930629, cost = 11000},
		[1090] = {faction = NEUTRAL, distance = 1252.864043937, cost = 11000},
	},
	[1071] = {
		[1070] = {faction = NEUTRAL, distance = 1829.1845226889, cost = 11000},
		[1073] = {faction = NEUTRAL, distance = 2432.9716957814, cost = 11000},
		[1090] = {faction = NEUTRAL, distance = 1309.8075053117, cost = 11000},
	},
	[1072] = {
		[1054] = {faction = NEUTRAL, distance = 1012.7809978592, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 3521.3859157986, cost = 11000},
		[1058] = {faction = HORDE, distance = 3152.5155329718, cost = 11000},
		[1070] = {faction = NEUTRAL, distance = 959.20178289271, cost = 11000},
		[1073] = {faction = NEUTRAL, distance = 1531.6749645794, cost = 11000},
	},
	[1073] = {
		[1025] = {faction = NEUTRAL, distance = 3044.8148000285, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 2105.8694307987, cost = 11000},
		[1058] = {faction = HORDE, distance = 1729.1032892518, cost = 11000},
		[1070] = {faction = NEUTRAL, distance = 1567.1052230579, cost = 11000},
		[1071] = {faction = NEUTRAL, distance = 2152.8237570628, cost = 11000},
		[1072] = {faction = NEUTRAL, distance = 1596.52107473, cost = 11000},
	},
	[1080] = {
		[894] = {faction = NONE, distance = 1812.0915749943, cost = 11000},
		[895] = {faction = NEUTRAL, distance = 1337.0410215267, cost = 11000},
		[972] = {faction = ALLIANCE, distance = 1299.1156474218, cost = 11000},
	},
	[1090] = {
		[1070] = {faction = NEUTRAL, distance = 1458.832114938, cost = 11000},
		[1071] = {faction = NEUTRAL, distance = 969.90378015958, cost = 11000},
	},
	[1115] = {
		[1057] = {faction = ALLIANCE, distance = 2437.6712150909, cost = 11000},
	},
	[1117] = {
		[1053] = {faction = HORDE, distance = 638.34877789216, cost = 11000},
		[1054] = {faction = HORDE, distance = 770.77377597387, cost = 11000},
		[1058] = {faction = HORDE, distance = 2290.4161562231, cost = 11000},
	},
	[1190] = {
		[966] = {faction = ALLIANCE, distance = 1534.8855714095, cost = 11000},
		[988] = {faction = ALLIANCE, distance = 1065.9269342832, cost = 11000},
	},
	[1195] = {
		[989] = {faction = HORDE, distance = 1516.526182373, cost = 11000},
		[990] = {faction = HORDE, distance = 1442.2867319095, cost = 11000},
	},
	[1221] = {
		[1021] = {faction = ALLIANCE, distance = 1601.9766103029, cost = 11000},
		[1057] = {faction = ALLIANCE, distance = 6111.3205156281, cost = 11000},
	},
	[1222] = {
		[1021] = {faction = HORDE, distance = 1489.7716394466, cost = 11000},
		[1058] = {faction = HORDE, distance = 5546.7637199701, cost = 11000},
	},
	[1293] = {
		[895] = {faction = ALLIANCE, distance = 4265.9477714866, cost = 11000},
	},
	[1294] = {
		[895] = {faction = HORDE, distance = 4293.8064522847, cost = 11000},
	},
}
