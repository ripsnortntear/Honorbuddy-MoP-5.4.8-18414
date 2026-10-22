local BLUE = "|cff0000FF";
local CYAN = "|cff00FFFF";
local GREEN = "|cff00FF00";
local MAGENTA = "|cffFF00EA";
local RED  = "|cffFF0000";
local WHITE = "|cffFFFFFF";
local YELLOW = "|cffFFFF00";
local OVERLAYS = "Interface\\AddOns\\RareSpawnOverlay\\Overlays\\"

RareSpawnOverlay.ColorData = {
   blue = {
      rgb = BLUE,
      r = 0x00,
      g = 0x00,
      b = 0xFF
   },
   cyan = {
      rgb = CYAN,
      r = 0x00,
      g = 0xFF,
      b = 0xFF
   },
   green = {
      rgb = GREEN,
      r = 0x00,
      g = 0xFF,
      b = 0x00
   },
   magenta = {
      rgb = MAGENTA,
      r = 0xFF,
      g = 0x00,
      b = 0xEA
   },
      red = {
      rgb = RED,
      r = 0xFF,
      g = 0x00,
      b = 0x00
   },
      white = {
      rgb = WHITE,
      r = 0xFF,
      g = 0xFF,
      b = 0xFF
   },
      yellow = {
      rgb = YELLOW,
      r = 0xFF,
      g = 0xFF,
      b = 0x00
   },
}

RareSpawnOverlay.SpawnData = {
   BladesEdgeMountains = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 475,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\BladesEdgeMountains.blp",
	 Information = "Hemathion, Morcrush, Speaker Mar'grom",
      },
   },
   Hellfire = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 465,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\Hellfire.blp",
	 Information = "Fulgorge, Mekthorg the Wild, Vorakem Doomspeaker",
      },
   },
   Nagrand = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 477,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\Nagrand.blp",
	 Information = "Bro'Gaz the Clanless, Goretooth, Voidhunter Yar",
      },
   },
   Netherstorm = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 479,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\Netherstorm.blp",
	 Information = "Chief Engineer Lorthander, Ever-Core the Punisher, Nuramoc ",
      },
   },
   ShadowmoonValley = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 473,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\ShadowmoonValley.blp",
	 Information = "Ambassador Jerikkar, Collidus the Warp-Watcher, Kraator",
      },
   },
   TerokkarForest = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 478,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\TerokkarForest.blp",
	 Information = "Crippler, Okrek, Doomsayer Jurim",
      },
   },
   Zangarmarsh = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 467,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Outlands\\Zangarmarsh.blp",
	 Information = "Marticar, Coilfang Emissary, Bog Lurker",
      },
   },
   BoreanTundra = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 486,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\BoreanTundra.blp",
	 Information = "Fumblub Gearwind, Old Crystalbark, Icehorn",
      },
   },
   Dragonblight = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 488,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\Dragonblight.blp",
	 Information = "Crazed Indu'le Survivor, Scarlet Highlord Daion, Tukemuth",
      },
   },
   GrizzlyHills = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 490,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\GrizzlyHills.blp",
	 Information = "Grocklar, Syreian the Bonecarver, Seething Hate, Arcturis(Spirit Beast)",
      },
   },
   HowlingFjord = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 491,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\HowlingFjord.blp",
	 Information = "Vigdis the War Maiden, Perobas the Bloodthirster, King Pin",
      },
   },
   IcecrownGlacier = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 492,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\IcecrownGlacier.blp",
	 Information = "Putridus the Ancient, High Thane Jorfus, Hildana Deathstealer",
      },
   },
   SholazarBasin = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 493,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\SholazarBasin.blp",
	 Information = "King Krush, Aotona, Loque'nahak(Spirit Beast)",
      },
   },
   TheStormPeaks = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 495,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\TheStormPeaks.blp",
	 Information = "Time-Lost Proto-Drake, Vyragosa, Dirkee, Skoll(Spirit Beast)",
      },
   },
   ZulDrak = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 496,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Northrend\\ZulDrak.blp",
	 Information = " Zul'drak Sentinel, Griegen, Terror Spinner,  Gondria(Spirit Beast)",
      },
   },
   Deepholm = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 640,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\Deepholm.blp",
	 Information = "Aeonaxx, Xariona, Golgarok, Terborus, Jadefang",
      },
   },
   Hyjal_terrain1 = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 606,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\Hyjal.blp",
	 Information = "Thartuk the Exile, Garr, Terrorpene, Blazewing,  Ankha, Ban'thalos(Spirit Beast), Magria(Spirit Beast)",
      },
   },
   Hyjal = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 606,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\Hyjal.blp",
	 Information = "Thartuk the Exile, Garr, Terrorpene, Blazewing,  Ankha, Ban'thalos(Spirit Beast), Magria(Spirit Beast)",
      },
   },
      TwilightHighlands = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 700,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\TwilightHighlands.blp",
	 Information = "Overlord Sunderfury, Julak-Doom, Sambas, Tarvus the Vile, Karoma(Spirit Beasts)",
      },
   },
      TwilightHighlands_terrain1 = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 700,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\TwilightHighlands.blp",
	 Information = "Overlord Sunderfury, Julak-Doom, Sambas, Tarvus the Vile, Karoma(Spirit Beasts)",
      },
   },
   Uldum = {
      LegendX = 10,
      LegendY = -20,
      {
	 Name = "Camel Spawns",
	 ID = 720,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\UldumCamels.blp",
	 Information = "Camel Mount Spawn Loctations",
      },
      {
	 Name = "Rares",
	 ID = 720,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\Uldum.blp",
	 Information = "Akma'hat, Cyrus the Black, Armagedillo, Madexx",
      },
   },
   VashjirDepths = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 614,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\AbyssalDepths.blp",
	 Information = "Mobus, Poseidus, Ghostcrawler, Shok'sharak",
      },
   },
   VashjirKelpForest = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 610,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\KelpTharForest.blp",
	 Information = "Lady La-La",
      },
   },
   VashjirRuins = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 615,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Cataclysm\\ShimmeringExpanse.blp",
	 Information = "Poseidus, Captain Florence, Captain Foulwind, Burgy Blackheart",
      },
   },
   DreadWastes = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 858,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\DreadWastes.blp",
	 Information = "Ai-Li Skymirror, Gar'lok Omnis Grinlok, Krol the Blade,  Karr the Darkener, Dak the Breaker, Nalash Verdantis, Ik-Ik the Nimble"
      },
   },
   IsleoftheThunderKing = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 928,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\IsleofThunder.blp",
	 Information = "Haywire Sunreaver Construct, Ku'lai the Skyclaw, Progenitus, Mumta, Goda, Al'tabim the All-Seeing, God-Hulk Ramuk, Lu-Ban, Backbreaker Uru, Molthor, Ra'sha, Na'lak"
      },
   },
   TheJadeForest = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 806,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\TheJadeForest.blp",
      },
   },
   Krasarang = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 857,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\KrasarangWilds.blp",
	 Information = "Ruun Ghostpaw, Torik-Ethis, Arness the Scale, Qu'nas, Gaarn the Toxic, Go-Kan, Cournith Waterstrider, Spriggin, Degu(Spirit Beast"
      },
   },
      Krasarang_terrain1 = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 857,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\KrasarangWilds.blp",
	 Information = "Ruun Ghostpaw, Torik-Ethis, Arness the Scale, Qu'nas, Gaarn the Toxic, Go-Kan, Cournith Waterstrider, Spriggin, Degu(Spirit Beast"
      },
   },
   KunLaiSummit = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 809,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\KunLaiSummit.blp",
	 Information = "Ahone the Wanderer, Ski'thik, Nessos the Oracle, Havak, Borginn Darkfist, Korda Torros, Zai the Outcast, Scritch, Sha of Anger, Gumi(Spirit Beast)"
      },
   },
   TimelessIsle = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 951,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\TimelessIsle.blp",
	 Information = "Evermaw, Dread Ship Vazuvius, Karkanos, Zesqua, Rattleskew, Stinkbraid, Great Turtle-Furyshell, Chelon, Monstrous Spineclaw, Bufo, Gu'chi the Swarmbringer, Zhu-Gon the Sour, Ironfur Steelhorn, Cranegnasher, Imperial Python, Emerald Gander, Golganarr, Tsavo'ka, Spelurk, Spirit of Jadefire, Rock Moss, Jakur of Ordon, Watcher Osu, Champion of the Black Flame, Leafmender, Huolon, Cinderfall, Flintlord Gairan, Archiereus of Flame, Urdur the Cauterizer, Garnia"
      },
      {
	 Name = "Text",
	 ID = 951,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\TimelessIsleText.blp",
	 Information = "Rares Text"
      },
   },
   TownlongWastes = {
      LegendX = 10,
      LegendY = -40,
      {
     Name = "Rares",
	 ID = 810,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\TownlongSteppes.blp",
	 Information = "Yul Wildpaw, Lith'ik the Stalker, Siltriss the Sharpener, Kah'tir, Norlaxx, Lon the Bull, Eshelon, The Yowler"
      },
   },
   ValeofEternalBlossoms = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 811,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\ValeofEternalBlossoms.blp",
	 Information = "Ai-Ran the Shifting Cloud, Kal'tik the Blight, Moldo One-Eye, Urgolax, Kang the Soul Thief, Yorik Sharpeye, Sahn Tidehunter, Major Nanners"
      },
   },
   ValleyoftheFourWinds = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 807,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Pandaria\\ValleyoftheFourWinds.blp",
	 Information = "Nasra Spothide, Nal'lak the Ripper, Salyin Warscout, Jonn-Dar, Sulik'shor, Blackhoof, Sele'na, Bonobos, Galleon"
      },
   },
   DarkmoonFaireIsland = {
      LegendX = 10,
      LegendY = -40,
      {
	 Name = "Rares",
	 ID = 807,
	 Color = RareSpawnOverlay.ColorData.white,
	 OverlayFilename = OVERLAYS.."Other\\Darkmoon.blp",
	 Information = "Darkmoon Isle Rares"
      },
   },
};