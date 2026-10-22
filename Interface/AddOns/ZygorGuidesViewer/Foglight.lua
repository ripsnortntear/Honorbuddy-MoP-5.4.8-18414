local AceHook = LibStub("AceHook-3.0")

local ZGV=ZygorGuidesViewer
if not ZGV then return end

local Foglight = {}
ZGV.Foglight = Foglight

--local ver = select(4,GetBuildInfo())
--if ver>=40000 then return end --TODO: disabled in cata, needs rework or just a working db...

Foglight.Debug = ZGV.Debug

Foglight.data = {
 -- Kalimdor
  ['Ashenvale']={
    {'Interface\\WorldMap\\Ashenvale\\TheZoramStrand',262,390,0,0},
    {'Interface\\WorldMap\\Ashenvale\\LakeFalathim',184,232,112,148},
    {'Interface\\WorldMap\\Ashenvale\\ThistlefurVillage',314,241,255,78},
    {'Interface\\WorldMap\\Ashenvale\\Astranaar',251,271,255,164},
    {'Interface\\WorldMap\\Ashenvale\\TheRuinsofStardust',236,271,210,331},
    {'Interface\\WorldMap\\Ashenvale\\TheHowlingVale',325,239,473,97},
    {'Interface\\WorldMap\\Ashenvale\\RaynewoodRetreat',231,256,481,221},
    {'Interface\\WorldMap\\Ashenvale\\FallenSkyLake',287,276,529,385},
    {'Interface\\WorldMap\\Ashenvale\\NightRun',221,257,595,253},
    {'Interface\\WorldMap\\Ashenvale\\Satyrnaar',235,236,696,154},
    {'Interface\\WorldMap\\Ashenvale\\BoughShadow',166,211,836,148},
    {'Interface\\WorldMap\\Ashenvale\\WarsongLumberCamp',231,223,771,265},
    {'Interface\\WorldMap\\Ashenvale\\FelfireHill',277,333,714,317},
    {'Interface\\WorldMap\\Ashenvale\\MaelstrasPost',246,361,188,0},
    {'Interface\\WorldMap\\Ashenvale\\OrendilsRetreat',244,251,143,0},
    {'Interface\\WorldMap\\Ashenvale\\SilverwindRefuge',347,308,338,335},
    {'Interface\\WorldMap\\Ashenvale\\TheShrineofAssenia',306,283,40,275},
    {'Interface\\WorldMap\\Ashenvale\\ThunderPeak',203,310,377,121},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Aszhara']={
    {'Interface\\WorldMap\\Aszhara\\TheShatteredStrand',206,329,316,168},
    {'Interface\\WorldMap\\Aszhara\\BitterReaches',321,247,477,0},
    {'Interface\\WorldMap\\Aszhara\\TowerofEldara',306,337,684,22},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Aszhara\\RuinsofEldarath',218,237,228,229},
    {'Interface\\WorldMap\\Aszhara\\RavencrestMonument',295,267,476,401},
    {'Interface\\WorldMap\\Aszhara\\LakeMennar',210,232,245,377},
    {'',395,128,396,540},
    {'Interface\\WorldMap\\Aszhara\\BearsHead',256,224,113,141},
    {'Interface\\WorldMap\\Aszhara\\BilgewaterHarbor',587,381,395,127},
    {'Interface\\WorldMap\\Aszhara\\BlackmawHold',260,267,204,53},
    {'Interface\\WorldMap\\Aszhara\\DarnassianBaseCamp',243,262,343,3},
    {'Interface\\WorldMap\\Aszhara\\GallywixPleasurePalace',250,230,70,222},
    {'Interface\\WorldMap\\Aszhara\\OrgimmarRearGate',352,274,22,344},
    {'Interface\\WorldMap\\Aszhara\\RuinsofArkkoran',219,193,575,121},
    {'Interface\\WorldMap\\Aszhara\\StormCliffs',207,232,407,403},
    {'Interface\\WorldMap\\Aszhara\\TheSecretLab',184,213,353,396},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['AzuremystIsle']={
    {'Interface\\WorldMap\\AzuremystIsle\\AmmenFord',256,256,515,279},
    {'Interface\\WorldMap\\AzuremystIsle\\AmmenVale',475,512,527,104},
    {'Interface\\WorldMap\\AzuremystIsle\\AzureWatch',256,256,383,249},
    {'Interface\\WorldMap\\AzuremystIsle\\BristlelimbVillage',256,256,174,363},
    {'Interface\\WorldMap\\AzuremystIsle\\Emberglade',256,256,488,24},
    {'Interface\\WorldMap\\AzuremystIsle\\FairbridgeStrand',256,128,356,0},
    {'Interface\\WorldMap\\AzuremystIsle\\GreezlesCamp',256,256,507,350},
    {'Interface\\WorldMap\\AzuremystIsle\\MoongrazeWoods',256,256,449,183},
    {'Interface\\WorldMap\\AzuremystIsle\\OdesyusLanding',256,256,352,378},
    {'Interface\\WorldMap\\AzuremystIsle\\PodCluster',256,256,281,305},
    {'Interface\\WorldMap\\AzuremystIsle\\PodWreckage',128,256,462,349},
    {'Interface\\WorldMap\\AzuremystIsle\\SiltingShore',256,256,291,3},
    {'Interface\\WorldMap\\AzuremystIsle\\SilvermystIsle',256,222,23,446},
    {'Interface\\WorldMap\\AzuremystIsle\\StillpineHold',256,256,365,49},
    {'Interface\\WorldMap\\AzuremystIsle\\TheExodar',512,512,74,85},
    {'Interface\\WorldMap\\AzuremystIsle\\ValaarsBerth',256,256,176,303},
    {'Interface\\WorldMap\\AzuremystIsle\\WrathscalePoint',256,247,220,421},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['BloodmystIsle']={
    {'Interface\\WorldMap\\BloodmystIsle\\AmberwebPass',256,512,44,62},
    {'Interface\\WorldMap\\BloodmystIsle\\Axxarien',256,256,297,136},
    {'Interface\\WorldMap\\BloodmystIsle\\BlacksiltShore',512,242,177,426},
    {'Interface\\WorldMap\\BloodmystIsle\\Bladewood',256,256,367,209},
    {'Interface\\WorldMap\\BloodmystIsle\\BloodscaleIsle',239,256,763,256},
    {'Interface\\WorldMap\\BloodmystIsle\\BloodWatch',256,256,437,258},
    {'Interface\\WorldMap\\BloodmystIsle\\BristlelimbEnclave',256,256,546,410},
    {'Interface\\WorldMap\\BloodmystIsle\\KesselsCrossing',485,141,517,527},
    {'Interface\\WorldMap\\BloodmystIsle\\Middenvale',256,256,414,406},
    {'Interface\\WorldMap\\BloodmystIsle\\Mystwood',256,185,309,483},
    {'Interface\\WorldMap\\BloodmystIsle\\Nazzivian',256,256,250,404},
    {'Interface\\WorldMap\\BloodmystIsle\\RagefeatherRidge',256,256,481,117},
    {'Interface\\WorldMap\\BloodmystIsle\\RuinsofLorethAran',256,256,556,216},
    {'Interface\\WorldMap\\BloodmystIsle\\TalonStand',256,256,657,78},
    {'Interface\\WorldMap\\BloodmystIsle\\TelathionsCamp',128,128,180,216},
    {'Interface\\WorldMap\\BloodmystIsle\\TheBloodcursedReef',256,256,729,54},
    {'Interface\\WorldMap\\BloodmystIsle\\TheBloodwash',256,256,302,27},
    {'Interface\\WorldMap\\BloodmystIsle\\TheCrimsonReach',256,256,555,87},
    {'Interface\\WorldMap\\BloodmystIsle\\TheCryoCore',256,256,293,285},
    {'Interface\\WorldMap\\BloodmystIsle\\TheFoulPool',256,256,221,136},
    {'Interface\\WorldMap\\BloodmystIsle\\TheHiddenReef',256,256,205,39},
    {'Interface\\WorldMap\\BloodmystIsle\\TheLostFold',256,198,503,470},
    {'Interface\\WorldMap\\BloodmystIsle\\TheVectorCoil',512,430,43,238},
    {'Interface\\WorldMap\\BloodmystIsle\\TheWarpPiston',256,256,451,29},
    {'Interface\\WorldMap\\BloodmystIsle\\VeridianPoint',256,256,637,0},
    {'Interface\\WorldMap\\BloodmystIsle\\VindicatorsRest',256,256,232,242},
    {'Interface\\WorldMap\\BloodmystIsle\\WrathscaleLair',256,256,598,338},
    {'Interface\\WorldMap\\BloodmystIsle\\WyrmscarIsland',256,256,613,82},
  },
  ['Darkshore']={
    {'Interface\\WorldMap\\Darkshore\\RuinsofMathystra',200,263,517,28},
    {'Interface\\WorldMap\\Darkshore\\AmethAran',326,145,294,330},
    {'Interface\\WorldMap\\Darkshore\\TheMastersGlaive',303,185,277,483},
    {'Interface\\WorldMap\\Darkshore\\EyeoftheVortex',330,192,300,239},
    {'Interface\\WorldMap\\Darkshore\\Lordanel',277,281,391,54},
    {'Interface\\WorldMap\\Darkshore\\Nazjvel',244,201,207,467},
    {'Interface\\WorldMap\\Darkshore\\RuinsofAuberdine',203,194,280,182},
    {'Interface\\WorldMap\\Darkshore\\ShatterspearVale',250,241,596,16},
    {'Interface\\WorldMap\\Darkshore\\ShatterspearWarcamp',245,147,565,0},
    {'Interface\\WorldMap\\Darkshore\\WildbendRiver',314,193,280,378},
    {'Interface\\WorldMap\\Darkshore\\WitheringThicket',328,250,305,118},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Desolace']={
    {'Interface\\WorldMap\\Desolace\\ShadowbreakRavine',292,266,637,402},
    {'Interface\\WorldMap\\Desolace\\MannorocCoven',326,311,381,357},
    {'Interface\\WorldMap\\Desolace\\GelkisVillage',274,196,207,472},
    {'Interface\\WorldMap\\Desolace\\ShadowpreyVillage',222,299,142,369},
    {'Interface\\WorldMap\\Desolace\\KodoGraveyard',250,215,360,273},
    {'Interface\\WorldMap\\Desolace\\ValleyofSpears',321,275,170,196},
    {'Interface\\WorldMap\\Desolace\\ThunderAxeFortress',220,205,440,49},
    {'Interface\\WorldMap\\Desolace\\Sargeron',317,293,655,0},
    {'Interface\\WorldMap\\Desolace\\NijelsPoint',231,257,573,0},
    {'Interface\\WorldMap\\Desolace\\TethrisAran',274,145,399,0},
    {'Interface\\WorldMap\\Desolace\\CenarionWildlands',312,285,415,156},
    {'Interface\\WorldMap\\Desolace\\MagramTerritory',289,244,613,170},
    {'Interface\\WorldMap\\Desolace\\RanzjarIsle',161,141,210,0},
    {'Interface\\WorldMap\\Desolace\\ShokThokar',309,349,589,319},
    {'Interface\\WorldMap\\Desolace\\SlitherbladeShore',338,342,208,24},
    {'Interface\\WorldMap\\Desolace\\ThargadsCamp',212,186,275,376},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Durotar']={
    {'Interface\\WorldMap\\Durotar\\ValleyOfTrials',254,258,304,312},
    {'Interface\\WorldMap\\Durotar\\SenjinVillage',192,184,457,406},
    {'Interface\\WorldMap\\Durotar\\EchoIsles',330,255,429,413},
    {'Interface\\WorldMap\\Durotar\\TiragardeKeep',210,200,462,298},
    {'Interface\\WorldMap\\Durotar\\RazorHill',224,227,431,157},
    {'Interface\\WorldMap\\Durotar\\RazormaneGrounds',248,158,302,264},
    {'Interface\\WorldMap\\Durotar\\ThunderRidge',220,218,295,48},
    {'Interface\\WorldMap\\Durotar\\DrygulchRavine',236,196,415,60},
    {'Interface\\WorldMap\\Durotar\\SkullRock',208,157,438,0},
    {'Interface\\WorldMap\\Durotar\\NorthwatchFoothold',162,157,399,440},
    {'Interface\\WorldMap\\Durotar\\SouthfuryWatershed',244,222,282,174},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Durotar\\Orgrimmar',259,165,309,0},
  },
  ['Dustwallow']={
    {'Interface\\WorldMap\\Dustwallow\\TheramoreIsle',305,247,542,223},
    {'Interface\\WorldMap\\Dustwallow\\WitchHill',270,353,428,0},
    {'Interface\\WorldMap\\Dustwallow\\TheWyrmbog',436,299,359,369},
    {'Interface\\WorldMap\\Dustwallow\\AlcazIsland',206,200,656,21},
    {'Interface\\WorldMap\\Dustwallow\\BlackhoofVillage',344,183,199,0},
    {'Interface\\WorldMap\\Dustwallow\\BrackenwllVillage',384,249,133,59},
    {'Interface\\WorldMap\\Dustwallow\\DirehornPost',279,301,358,169},
    {'Interface\\WorldMap\\Dustwallow\\Mudsprocket',433,351,109,313},
    {'Interface\\WorldMap\\Dustwallow\\ShadyRestInn',317,230,137,188},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Dustwallow_terrain1']={
    {'Interface\\WorldMap\\Dustwallow_terrain1\\ALCAZISLAND',206,200,656,21},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\BLACKHOOFVILLAGE',344,183,199,0},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\BRACKENWLLVILLAGE',384,249,133,59},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\DIREHORNPOST',279,301,358,169},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\MUDSPROCKET',433,351,109,313},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\SHADYRESTINN',317,230,137,188},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\THERAMOREISLE',305,247,542,223},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\THEWYRMBOG',436,299,359,369},
    {'Interface\\WorldMap\\Dustwallow_terrain1\\WITCHHILL',270,353,428,0},
  },
  ['Felwood']={
    {'Interface\\WorldMap\\Felwood\\FelpawVillage',307,161,471,0},
    {'Interface\\WorldMap\\Felwood\\TalonbranchGlade',209,226,531,57},
    {'Interface\\WorldMap\\Felwood\\IrontreeWoods',261,273,406,55},
    {'Interface\\WorldMap\\Felwood\\JadefireRun',263,199,303,9},
    {'Interface\\WorldMap\\Felwood\\ShatterScarVale',343,250,243,107},
    {'Interface\\WorldMap\\Felwood\\BloodvenomFalls',345,192,220,231},
    {'Interface\\WorldMap\\Felwood\\Jaedenar',319,176,234,317},
    {'Interface\\WorldMap\\Felwood\\RuinsofConstellas',268,214,278,359},
    {'Interface\\WorldMap\\Felwood\\JadefireGlen',229,210,288,458},
    {'Interface\\WorldMap\\Felwood\\EmeraldSanctuary',274,212,394,382},
    {'Interface\\WorldMap\\Felwood\\DeadwoodVillage',173,163,410,505},
    {'Interface\\WorldMap\\Felwood\\MorlosAran',187,176,476,484},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Feralas']={
    {'Interface\\WorldMap\\Feralas\\TheTwinColossals',350,334,271,0},
    {'Interface\\WorldMap\\Feralas\\TheForgottenCoast',194,304,375,343},
    {'Interface\\WorldMap\\Feralas\\DireMaul',265,284,485,101},
    {'Interface\\WorldMap\\Feralas\\RuinsofIsildien',206,237,467,354},
    {'Interface\\WorldMap\\Feralas\\CampMojache',174,220,671,181},
    {'Interface\\WorldMap\\Feralas\\GordunniOutpost',192,157,663,116},
    {'Interface\\WorldMap\\Feralas\\LowerWilds',207,209,756,191},
    {'Interface\\WorldMap\\Feralas\\DarkmistRuins',172,198,568,287},
    {'Interface\\WorldMap\\Feralas\\FeathermoonStronghold',217,192,362,237},
    {'Interface\\WorldMap\\Feralas\\FeralScar',191,179,457,281},
    {'Interface\\WorldMap\\Feralas\\GrimtotemCompund',159,218,607,170},
    {'Interface\\WorldMap\\Feralas\\RuinsofFeathermoon',208,204,186,229},
    {'Interface\\WorldMap\\Feralas\\WrithingDeep',232,206,652,298},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Moonglade']={
    {'Interface\\WorldMap\\Moonglade\\LakeEluneara',431,319,219,273},
    {'Interface\\WorldMap\\Moonglade\\Nighthaven',346,244,370,135},
    {'Interface\\WorldMap\\Moonglade\\ShrineofRemulos',271,296,209,91},
    {'Interface\\WorldMap\\Moonglade\\StormrageBarrowDens',275,346,542,210},
  },
  ['Hyjal']={
    {'Interface\\WorldMap\\Hyjal\\AshenLake',282,418,6,78},
    {'Interface\\WorldMap\\Hyjal\\DarkwhisperGorge',320,471,682,128},
    {'Interface\\WorldMap\\Hyjal\\GatesOfSothann',272,334,622,320},
    {'Interface\\WorldMap\\Hyjal\\Nordrassil',537,323,392,0},
    {'Interface\\WorldMap\\Hyjal\\SethriasRoost',277,232,139,436},
    {'Interface\\WorldMap\\Hyjal\\ShrineOfGoldrinn',291,321,116,17},
    {'Interface\\WorldMap\\Hyjal\\TheRegrowth',441,319,52,253},
    {'Interface\\WorldMap\\Hyjal\\TheScorchedPlain',365,264,411,216},
    {'Interface\\WorldMap\\Hyjal\\TheThroneOfFlame',419,290,318,378},
    {'Interface\\WorldMap\\Hyjal\\DireforgeHill',270,173,303,197},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Hyjal_terrain1']={
    {'Interface\\WorldMap\\Hyjal\\AshenLake',282,418,6,78},
    {'Interface\\WorldMap\\Hyjal\\DarkwhisperGorge',320,471,682,128},
    {'Interface\\WorldMap\\Hyjal\\GatesOfSothann',272,334,622,320},
    {'Interface\\WorldMap\\Hyjal\\Nordrassil',537,323,392,0},
    {'Interface\\WorldMap\\Hyjal\\SethriasRoost',277,232,139,436},
    {'Interface\\WorldMap\\Hyjal\\ShrineOfGoldrinn',291,321,116,17},
    {'Interface\\WorldMap\\Hyjal\\TheRegrowth',441,319,52,253},
    {'Interface\\WorldMap\\Hyjal\\TheScorchedPlain',365,264,411,216},
    {'Interface\\WorldMap\\Hyjal\\TheThroneOfFlame',419,290,318,378},
    {'Interface\\WorldMap\\Hyjal\\DireforgeHill',270,173,303,197},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'Interface\\WorldMap\\Hyjal\\ArchimondesVengeance',270,300,320,5},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Mulgore']={
    {'Interface\\WorldMap\\Mulgore\\BaeldunDigsite',218,192,226,220},
    {'Interface\\WorldMap\\Mulgore\\BloodhoofVillage',302,223,319,273},
    {'Interface\\WorldMap\\Mulgore\\PalemaneRock',172,205,248,321},
    {'Interface\\WorldMap\\Mulgore\\RavagedCaravan',187,165,435,224},
    {'Interface\\WorldMap\\Mulgore\\RedCloudMesa',446,264,286,401},
    {'Interface\\WorldMap\\Mulgore\\RedRocks',186,185,514,43},
    {'Interface\\WorldMap\\Mulgore\\StonetalonPass',237,184,201,0},
    {'Interface\\WorldMap\\Mulgore\\TheGoldenPlains',186,216,448,101},
    {'Interface\\WorldMap\\Mulgore\\TheRollingPlains',260,243,527,291},
    {'Interface\\WorldMap\\Mulgore\\TheVentureCoMine',208,300,530,138},
    {'Interface\\WorldMap\\Mulgore\\ThunderBluff',373,259,208,62},
    {'Interface\\WorldMap\\Mulgore\\ThunderhornWaterWell',201,167,333,202},
    {'Interface\\WorldMap\\Mulgore\\WildmaneWaterWell',190,172,331,0},
    {'Interface\\WorldMap\\Mulgore\\WindfuryRidge',222,202,400,0},
    {'Interface\\WorldMap\\Mulgore\\WinterhoofWaterWell',174,185,449,340},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Barrens']={
    {'Interface\\WorldMap\\Barrens\\BoulderLodeMine',278,209,511,7},
    {'Interface\\WorldMap\\Barrens\\TheSludgeFen',257,249,403,6},
    {'Interface\\WorldMap\\Barrens\\DreadmistPeak',241,195,290,104},
    {'Interface\\WorldMap\\Barrens\\TheDryHills',283,270,116,57},
    {'Interface\\WorldMap\\Barrens\\TheForgottenPools',446,256,100,208},
    {'Interface\\WorldMap\\Barrens\\GroldomFarm',243,217,448,127},
    {'Interface\\WorldMap\\Barrens\\FarWatchPost',207,332,555,129},
    {'Interface\\WorldMap\\Barrens\\ThornHill',239,231,481,254},
    {'Interface\\WorldMap\\Barrens\\TheCrossroads',233,193,362,275},
    {'Interface\\WorldMap\\Barrens\\TheStagnantOasis',336,289,344,379},
    {'Interface\\WorldMap\\Barrens\\Ratchet',219,175,547,379},
    {'Interface\\WorldMap\\Barrens\\TheMerchantCoast',315,212,556,456},
    {'Interface\\WorldMap\\Barrens\\MorshanRampart',261,216,258,6},
    {'Interface\\WorldMap\\Barrens\\TheWailingCaverns',377,325,152,318},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Silithus']={
    {'Interface\\WorldMap\\Silithus\\TheCrystalVale',329,246,126,0},
    {'Interface\\WorldMap\\Silithus\\HiveAshi',405,267,345,4},
    {'Interface\\WorldMap\\Silithus\\SouthwindVillage',309,243,550,181},
    {'Interface\\WorldMap\\Silithus\\HiveZora',542,367,0,206},
    {'Interface\\WorldMap\\Silithus\\HiveRegal',489,358,380,310},
    {'Interface\\WorldMap\\Silithus\\TheScarabWall',580,213,0,455},
    {'Interface\\WorldMap\\Silithus\\CenarionHold',292,260,427,143},
    {'Interface\\WorldMap\\Silithus\\ValorsRest',315,285,614,0},
    {'Interface\\WorldMap\\Silithus\\TwilightBaseCamp',434,231,100,151},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['SouthernBarrens']={
    {'Interface\\WorldMap\\SouthernBarrens\\BaelModan',269,211,398,457},
    {'Interface\\WorldMap\\SouthernBarrens\\Battlescar',384,248,274,307},
    {'Interface\\WorldMap\\SouthernBarrens\\ForwardCommand',216,172,423,251},
    {'Interface\\WorldMap\\SouthernBarrens\\FrazzlecrazMotherload',242,195,269,436},
    {'Interface\\WorldMap\\SouthernBarrens\\HuntersHill',218,178,300,64},
    {'Interface\\WorldMap\\SouthernBarrens\\NorthwatchHold',280,279,548,147},
    {'Interface\\WorldMap\\SouthernBarrens\\RuinsofTaurajo',285,171,244,286},
    {'Interface\\WorldMap\\SouthernBarrens\\TheOvergrowth',355,226,289,117},
    {'Interface\\WorldMap\\SouthernBarrens\\VendettaPoint',254,214,267,196},
    {'Interface\\WorldMap\\SouthernBarrens\\HonorsStand',315,170,201,0},
    {'Interface\\WorldMap\\SouthernBarrens\\RazorfenKraul',214,140,273,528},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['StonetalonMountains']={
    {'Interface\\WorldMap\\StonetalonMountains\\StonetalonPeak',305,244,265,0},
    {'Interface\\WorldMap\\StonetalonMountains\\MirkfallonLake',244,247,417,143},
    {'Interface\\WorldMap\\StonetalonMountains\\TheCharredVale',277,274,199,368},
    {'Interface\\WorldMap\\StonetalonMountains\\SunRockRetreat',222,222,353,285},
    {'Interface\\WorldMap\\StonetalonMountains\\WindshearCrag',374,287,533,179},
    {'Interface\\WorldMap\\StonetalonMountains\\BoulderslideRavine',194,156,532,512},
    {'Interface\\WorldMap\\StonetalonMountains\\WebwinderPath',267,352,468,263},
    {'Interface\\WorldMap\\StonetalonMountains\\Malakajin',211,131,618,537},
    {'Interface\\WorldMap\\StonetalonMountains\\BattlescarValley',290,297,220,189},
    {'Interface\\WorldMap\\StonetalonMountains\\CliffwalkerPost',241,192,366,95},
    {'Interface\\WorldMap\\StonetalonMountains\\GreatwoodVale',322,220,602,448},
    {'Interface\\WorldMap\\StonetalonMountains\\KromgarFortress',183,196,588,341},
    {'Interface\\WorldMap\\StonetalonMountains\\RuinsofEldrethar',221,235,367,411},
    {'Interface\\WorldMap\\StonetalonMountains\\ThaldarahOverlook',210,189,252,121},
    {'Interface\\WorldMap\\StonetalonMountains\\UnearthedGrounds',265,206,654,369},
    {'Interface\\WorldMap\\StonetalonMountains\\WebwinderHollow',164,258,479,401},
    {'Interface\\WorldMap\\StonetalonMountains\\WindshearHold',176,189,516,289},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Tanaris']={
    {'Interface\\WorldMap\\Tanaris\\ThistleshrubValley',221,293,185,280},
    {'Interface\\WorldMap\\Tanaris\\SouthmoonRuins',232,211,301,349},
    {'Interface\\WorldMap\\Tanaris\\LandsEndBeach',224,216,431,452},
    {'Interface\\WorldMap\\Tanaris\\EastmoonRuins',173,163,380,341},
    {'Interface\\WorldMap\\Tanaris\\TheGapingChasm',225,187,448,364},
    {'Interface\\WorldMap\\Tanaris\\SouthbreakShore',274,186,437,289},
    {'Interface\\WorldMap\\Tanaris\\DunemaulCompound',231,177,305,257},
    {'Interface\\WorldMap\\Tanaris\\TheNoxiousLair',179,190,258,211},
    {'Interface\\WorldMap\\Tanaris\\BrokenPillar',195,163,413,211},
    {'Interface\\WorldMap\\Tanaris\\AbyssalSands',255,194,297,148},
    {'Interface\\WorldMap\\Tanaris\\CavernsofTime',213,173,507,238},
    {'Interface\\WorldMap\\Tanaris\\Gadgetzan',189,180,412,92},
    {'Interface\\WorldMap\\Tanaris\\SandsorrowWatch',214,149,293,99},
    {'Interface\\WorldMap\\Tanaris\\ZulFarrak',315,190,184,0},
    {'Interface\\WorldMap\\Tanaris\\GadgetzanBay',254,341,479,9},
    {'Interface\\WorldMap\\Tanaris\\LostRiggerCover',178,243,615,201},
    {'Interface\\WorldMap\\Tanaris\\ValleryoftheWatchers',269,190,255,431},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Teldrassil']={
    {'Interface\\WorldMap\\Teldrassil\\BanethilHollow',175,235,374,221},
    {'Interface\\WorldMap\\Teldrassil\\Darnassus',298,337,149,181},
    {'Interface\\WorldMap\\Teldrassil\\GnarlpineHold',198,181,347,355},
    {'Interface\\WorldMap\\Teldrassil\\LakeAlameth',289,202,422,310},
    {'Interface\\WorldMap\\Teldrassil\\PoolsofArlithrien',140,210,345,243},
    {'Interface\\WorldMap\\Teldrassil\\Shadowglen',241,217,481,104},
    {'Interface\\WorldMap\\Teldrassil\\StarbreezeVillage',187,196,544,217},
    {'Interface\\WorldMap\\Teldrassil\\TheOracleGlade',194,244,276,90},
    {'Interface\\WorldMap\\Teldrassil\\WellspringLake',165,249,382,83},
    {'Interface\\WorldMap\\Teldrassil\\RutheranVillage',317,220,329,448},
    {'Interface\\WorldMap\\Teldrassil\\TheCleft',144,226,432,109},
    {'Interface\\WorldMap\\Teldrassil\\GalardellValley',178,186,466,237},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['ThousandNeedles']={
    {'Interface\\WorldMap\\ThousandNeedles\\DarkcloudPinnacle',317,252,169,116},
    {'Interface\\WorldMap\\ThousandNeedles\\FreewindPost',436,271,276,186},
    {'Interface\\WorldMap\\ThousandNeedles\\Highperch',246,380,0,134},
    {'Interface\\WorldMap\\ThousandNeedles\\RazorfenDowns',361,314,298,0},
    {'Interface\\WorldMap\\ThousandNeedles\\SouthseaHoldfast',246,256,756,412},
    {'Interface\\WorldMap\\ThousandNeedles\\SplithoofHeights',431,410,571,49},
    {'Interface\\WorldMap\\ThousandNeedles\\TheGreatLift',272,232,136,0},
    {'Interface\\WorldMap\\ThousandNeedles\\TheShimmeringDeep',411,411,591,257},
    {'Interface\\WorldMap\\ThousandNeedles\\TheTwilightWithering',374,339,347,329},
    {'Interface\\WorldMap\\ThousandNeedles\\TwilightBulwark',358,418,125,241},
    {'Interface\\WorldMap\\ThousandNeedles\\WestreachSummit',280,325,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\ThousandNeedles\\RustmaulDiveSite',234,203,527,465},
  },
  ['Uldum']={
    {'Interface\\WorldMap\\Uldum\\AkhenetFields',164,185,471,277},
    {'Interface\\WorldMap\\Uldum\\CradelOfTheAncient',202,169,341,402},
    {'Interface\\WorldMap\\Uldum\\HallsOfOrigination',269,242,599,184},
    {'Interface\\WorldMap\\Uldum\\KhartutsTomb',203,215,542,0},
    {'Interface\\WorldMap\\Uldum\\LostCityOfTheTolVir',233,321,527,291},
    {'Interface\\WorldMap\\Uldum\\Marat',160,193,406,174},
    {'Interface\\WorldMap\\Uldum\\Nahom',237,194,583,162},
    {'Interface\\WorldMap\\Uldum\\Neferset',209,254,407,384},
    {'Interface\\WorldMap\\Uldum\\ObeliskOfTheMoon',400,224,110,0},
    {'Interface\\WorldMap\\Uldum\\ObeliskOfTheStars',196,170,551,121},
    {'Interface\\WorldMap\\Uldum\\ObeliskOfTheSun',269,203,340,282},
    {'Interface\\WorldMap\\Uldum\\Orsis',249,243,264,136},
    {'Interface\\WorldMap\\Uldum\\Ramkahen',228,227,411,67},
    {'Interface\\WorldMap\\Uldum\\RuinsOfAhmtul',278,173,365,0},
    {'Interface\\WorldMap\\Uldum\\RuinsOfAmmon',203,249,217,289},
    {'Interface\\WorldMap\\Uldum\\Schnottzslanding',312,289,28,221},
    {'Interface\\WorldMap\\Uldum\\TahretGrounds',150,159,545,193},
    {'Interface\\WorldMap\\Uldum\\TempleofUldum',296,209,132,127},
    {'Interface\\WorldMap\\Uldum\\TheCursedlanding',237,316,752,170},
    {'Interface\\WorldMap\\Uldum\\TheGateofUnendingCycles',161,236,647,15},
    {'Interface\\WorldMap\\Uldum\\TheTrailOfDevestation',206,204,657,349},
    {'Interface\\WorldMap\\Uldum\\VirnaalDam',151,144,479,215},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Uldum_terrain1']={
    {'Interface\\WorldMap\\Uldum_terrain1\\AkhenetFields',164,185,471,277},
    {'Interface\\WorldMap\\Uldum_terrain1\\CradelOfTheAncient',202,169,341,402},
    {'Interface\\WorldMap\\Uldum_terrain1\\HallsOfOrigination',269,242,599,184},
    {'Interface\\WorldMap\\Uldum_terrain1\\KhartutsTomb',203,215,542,0},
    {'Interface\\WorldMap\\Uldum_terrain1\\LostCityOfTheTolVir',233,321,527,291},
    {'Interface\\WorldMap\\Uldum_terrain1\\Marat',160,193,406,174},
    {'Interface\\WorldMap\\Uldum_terrain1\\Nahom',237,194,583,162},
    {'Interface\\WorldMap\\Uldum_terrain1\\Neferset',209,254,407,384},
    {'Interface\\WorldMap\\Uldum_terrain1\\ObeliskOfTheMoon',400,224,110,0},
    {'Interface\\WorldMap\\Uldum_terrain1\\ObeliskOfTheStars',196,170,551,121},
    {'Interface\\WorldMap\\Uldum_terrain1\\ObeliskOfTheSun',269,203,340,282},
    {'Interface\\WorldMap\\Uldum_terrain1\\Orsis',249,243,264,136},
    {'Interface\\WorldMap\\Uldum_terrain1\\Ramkahen',228,227,411,67},
    {'Interface\\WorldMap\\Uldum_terrain1\\RuinsOfAhmtul',278,173,365,0},
    {'Interface\\WorldMap\\Uldum_terrain1\\RuinsOfAmmon',203,249,217,289},
    {'Interface\\WorldMap\\Uldum_terrain1\\Schnottzslanding',312,289,28,221},
    {'Interface\\WorldMap\\Uldum_terrain1\\TahretGrounds',150,159,545,193},
    {'Interface\\WorldMap\\Uldum_terrain1\\TempleofUldum',296,209,132,127},
    {'Interface\\WorldMap\\Uldum_terrain1\\TheCursedlanding',237,316,752,170},
    {'Interface\\WorldMap\\Uldum_terrain1\\TheGateofUnendingCycles',161,236,647,15},
    {'Interface\\WorldMap\\Uldum_terrain1\\TheTrailOfDevestation',206,204,657,349},
    {'Interface\\WorldMap\\Uldum_terrain1\\VirnaalDam',151,144,479,215},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['UngoroCrater']={
    {'Interface\\WorldMap\\UngoroCrater\\FirePlumeRidge',321,288,356,192},
    {'Interface\\WorldMap\\UngoroCrater\\GolakkaHotSprings',309,277,145,226},
    {'Interface\\WorldMap\\UngoroCrater\\TerrorRun',316,293,162,357},
    {'Interface\\WorldMap\\UngoroCrater\\TheSlitheringScar',381,274,335,384},
    {'Interface\\WorldMap\\UngoroCrater\\TheMarshlands',263,412,573,256},
    {'Interface\\WorldMap\\UngoroCrater\\IronstonePlateau',197,222,706,201},
    {'Interface\\WorldMap\\UngoroCrater\\LakkariTarPits',432,294,305,0},
    {'Interface\\WorldMap\\UngoroCrater\\FungalRock',224,191,557,0},
    {'Interface\\WorldMap\\UngoroCrater\\MarshalsStand',204,170,462,330},
    {'Interface\\WorldMap\\UngoroCrater\\MossyPile',186,185,328,179},
    {'Interface\\WorldMap\\UngoroCrater\\TheRollingGarden',337,321,565,39},
    {'Interface\\WorldMap\\UngoroCrater\\TheScreamingReaches',332,332,157,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Winterspring']={
    {'Interface\\WorldMap\\Winterspring\\Everlook',194,229,482,195},
    {'Interface\\WorldMap\\Winterspring\\FrostfireHotSprings',376,289,93,118},
    {'Interface\\WorldMap\\Winterspring\\FrostsaberRock',332,268,304,0},
    {'Interface\\WorldMap\\Winterspring\\FrostwhisperGorge',317,183,424,474},
    {'Interface\\WorldMap\\Winterspring\\IceThistleHills',249,217,581,314},
    {'Interface\\WorldMap\\Winterspring\\LakeKeltheril',271,258,372,268},
    {'Interface\\WorldMap\\Winterspring\\Mazthoril',257,238,399,340},
    {'Interface\\WorldMap\\Winterspring\\OwlWingThicket',254,150,556,439},
    {'Interface\\WorldMap\\Winterspring\\StarfallVillage',367,340,229,33},
    {'Interface\\WorldMap\\Winterspring\\TheHiddenGrove',333,255,500,17},
    {'Interface\\WorldMap\\Winterspring\\TimbermawPost',362,252,92,302},
    {'Interface\\WorldMap\\Winterspring\\WinterfallVillage',221,209,588,181},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
 -- Eastern Kingdoms
  ['VashjirDepths']={
    {'Interface\\WorldMap\\VashjirDepths\\AbandonedReef',371,394,50,263},
    {'Interface\\WorldMap\\VashjirDepths\\AbyssalBreach',491,470,497,0},
    {'Interface\\WorldMap\\VashjirDepths\\ColdlightChasm',267,374,266,280},
    {'Interface\\WorldMap\\VashjirDepths\\DeepfinRidge',363,262,275,32},
    {'Interface\\WorldMap\\VashjirDepths\\FireplumeTrench',298,251,315,110},
    {'Interface\\WorldMap\\VashjirDepths\\KorthunsEnd',370,385,412,283},
    {'Interface\\WorldMap\\VashjirDepths\\LGhorek',306,293,162,210},
    {'Interface\\WorldMap\\VashjirDepths\\Seabrush',225,250,415,183},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Arathi']={
    {'Interface\\WorldMap\\Arathi\\CircleofWestBinding',220,287,85,24},
    {'Interface\\WorldMap\\Arathi\\NorthfoldManor',227,268,132,105},
    {'Interface\\WorldMap\\Arathi\\Bouldergor',249,278,171,123},
    {'Interface\\WorldMap\\Arathi\\StromgardeKeep',284,306,21,269},
    {'Interface\\WorldMap\\Arathi\\FaldirsCove',273,268,77,400},
    {'Interface\\WorldMap\\Arathi\\CircleofInnerBinding',228,227,201,312},
    {'Interface\\WorldMap\\Arathi\\ThandolSpan',237,252,261,416},
    {'Interface\\WorldMap\\Arathi\\BoulderfistHall',252,258,327,367},
    {'Interface\\WorldMap\\Arathi\\RefugePoint',196,270,293,145},
    {'Interface\\WorldMap\\Arathi\\WitherbarkVillage',260,220,476,359},
    {'Interface\\WorldMap\\Arathi\\GoShekFarm',306,248,430,249},
    {'Interface\\WorldMap\\Arathi\\DabyriesFarmstead',210,227,404,144},
    {'Interface\\WorldMap\\Arathi\\CircleofEastBinding',183,238,506,126},
    {'Interface\\WorldMap\\Arathi\\Hammerfall',270,271,581,118},
    {'Interface\\WorldMap\\Arathi\\CirecleofOuterBinding',215,188,332,273},
    {'Interface\\WorldMap\\Arathi\\GalensFall',212,305,0,144},
  },
  ['Badlands']={
    {'Interface\\WorldMap\\Badlands\\AgmondsEnd',342,353,230,315},
    {'Interface\\WorldMap\\Badlands\\AngorFortress',285,223,230,68},
    {'Interface\\WorldMap\\Badlands\\ApocryphansRest',252,353,0,66},
    {'Interface\\WorldMap\\Badlands\\CampCagg',339,347,0,281},
    {'Interface\\WorldMap\\Badlands\\CampKosh',236,260,504,19},
    {'Interface\\WorldMap\\Badlands\\HammertoesDigsite',209,196,411,116},
    {'Interface\\WorldMap\\Badlands\\LethlorRavine',469,613,533,55},
    {'Interface\\WorldMap\\Badlands\\TheDustbowl',214,285,144,99},
    {'Interface\\WorldMap\\Badlands\\Uldaman',266,210,336,0},
    {'Interface\\WorldMap\\Badlands\\DeathwingScar',328,313,175,178},
    {'Interface\\WorldMap\\Badlands\\CampBoff',274,448,407,220},
    {'Interface\\WorldMap\\Badlands\\ApocryphansRest',252,353,0,66},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['BlastedLands']={
    {'Interface\\WorldMap\\BlastedLands\\DreadmaulHold',272,206,258,0},
    {'Interface\\WorldMap\\BlastedLands\\NethergardeKeep',295,205,530,6},
    {'Interface\\WorldMap\\BlastedLands\\SerpentsCoil',218,183,459,97},
    {'Interface\\WorldMap\\BlastedLands\\AltarofStorms',238,195,225,110},
    {'Interface\\WorldMap\\BlastedLands\\DreadmaulPost',235,188,327,182},
    {'Interface\\WorldMap\\BlastedLands\\TheTaintedScar',308,226,144,175},
    {'Interface\\WorldMap\\BlastedLands\\RiseoftheDefiler',168,170,375,102},
    {'Interface\\WorldMap\\BlastedLands\\NethergardeSupplyCamps',195,199,436,0},
    {'Interface\\WorldMap\\BlastedLands\\Shattershore',240,270,578,91},
    {'Interface\\WorldMap\\BlastedLands\\SunveilExcursion',233,266,386,374},
    {'Interface\\WorldMap\\BlastedLands\\Surwich',199,191,333,474},
    {'Interface\\WorldMap\\BlastedLands\\TheDarkPortal',370,298,368,179},
    {'Interface\\WorldMap\\BlastedLands\\TheRedReaches',268,354,533,268},
    {'Interface\\WorldMap\\BlastedLands\\TheTaintedForest',348,357,132,311},
  },
  ['BurningSteppes']={
    {'Interface\\WorldMap\\BurningSteppes\\AltarofStorms',182,360,0,0},
    {'Interface\\WorldMap\\BurningSteppes\\BlackrockMountain',281,388,79,0},
    {'Interface\\WorldMap\\BurningSteppes\\BlackrockPass',298,410,419,258},
    {'Interface\\WorldMap\\BurningSteppes\\BlackrockStronghold',320,385,235,0},
    {'Interface\\WorldMap\\BurningSteppes\\Dracodar',362,431,0,237},
    {'Interface\\WorldMap\\BurningSteppes\\DreadmaulRock',274,263,568,151},
    {'Interface\\WorldMap\\BurningSteppes\\MorgansVigil',383,413,615,255},
    {'Interface\\WorldMap\\BurningSteppes\\PillarofAsh',274,413,253,255},
    {'Interface\\WorldMap\\BurningSteppes\\RuinsofThaurissan',324,354,421,0},
    {'Interface\\WorldMap\\BurningSteppes\\TerrorWingPath',0,0,0,0},
    {'Interface\\WorldMap\\BurningSteppes\\TerrorWingPath',350,341,646,7},
    {'Interface\\WorldMap\\BurningSteppes\\PillarofAsh',274,413,253,255},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['DeadwindPass']={
    {'Interface\\WorldMap\\DeadwindPass\\DeadmansCrossing',617,522,83,0},
    {'Interface\\WorldMap\\DeadwindPass\\TheVice',350,449,433,208},
    {'Interface\\WorldMap\\DeadwindPass\\Karazhan',513,358,92,310},
  },
  ['DunMorogh']={
    {'Interface\\WorldMap\\DunMorogh\\FrostmaneHold',437,249,50,227},
    {'Interface\\WorldMap\\DunMorogh\\GolBolarQuarry',198,251,663,288},
    {'Interface\\WorldMap\\DunMorogh\\HelmsBedLake',218,234,760,268},
    {'Interface\\WorldMap\\DunMorogh\\AmberstillRanch',249,183,595,225},
    {'Interface\\WorldMap\\DunMorogh\\Kharanos',184,188,449,220},
    {'Interface\\WorldMap\\DunMorogh\\TheGrizzledDen',211,160,374,287},
    {'Interface\\WorldMap\\DunMorogh\\ColdridgePass',225,276,360,340},
    {'Interface\\WorldMap\\DunMorogh\\Ironforge',376,347,398,0},
    {'Interface\\WorldMap\\DunMorogh\\ColdridgeValley',398,302,100,366},
    {'Interface\\WorldMap\\DunMorogh\\FrostmaneFront',226,335,469,256},
    {'Interface\\WorldMap\\DunMorogh\\Gnomeregan',409,318,0,27},
    {'Interface\\WorldMap\\DunMorogh\\IronforgeAirfield',308,335,630,0},
    {'Interface\\WorldMap\\DunMorogh\\TheShimmeringDeep',171,234,397,132},
    {'Interface\\WorldMap\\DunMorogh\\TheShimmeringDeep',171,234,397,132},
    {'Interface\\WorldMap\\DunMorogh\\IceFlowLake',236,358,263,0},
    {'Interface\\WorldMap\\DunMorogh\\TheTundridHills',174,249,579,306},
    {'Interface\\WorldMap\\DunMorogh\\NorthGateOutpost',237,366,765,43},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Duskwood']={
    {'Interface\\WorldMap\\Duskwood\\TheHushedBank',189,307,0,152},
    {'Interface\\WorldMap\\Duskwood\\AddlesStead',299,296,32,348},
    {'Interface\\WorldMap\\Duskwood\\RavenHillCemetary',323,309,91,132},
    {'Interface\\WorldMap\\Duskwood\\VulGolOgreMound',268,282,228,355},
    {'Interface\\WorldMap\\Duskwood\\TheYorgenFarmstead',233,248,401,396},
    {'Interface\\WorldMap\\Duskwood\\BrightwoodGrove',279,399,497,112},
    {'Interface\\WorldMap\\Duskwood\\TheRottingOrchard',291,263,539,368},
    {'Interface\\WorldMap\\Duskwood\\Darkshire',329,314,640,128},
    {'Interface\\WorldMap\\Duskwood\\ManorMistmantle',219,182,661,122},
    {'Interface\\WorldMap\\Duskwood\\TheDarkenedBank',931,235,71,26},
    {'Interface\\WorldMap\\Duskwood\\RacenHill',205,157,96,292},
    {'Interface\\WorldMap\\Duskwood\\TheTranquilGardensCemetary',291,244,627,344},
    {'Interface\\WorldMap\\Duskwood\\TheTwilightGrove',320,388,314,101},
    {'',0,0,0,0},
  },
  ['EasternPlaguelands']={
    {'Interface\\WorldMap\\EasternPlaguelands\\Acherus',228,273,774,102},
    {'Interface\\WorldMap\\EasternPlaguelands\\BlackwoodLake',238,231,382,151},
    {'Interface\\WorldMap\\EasternPlaguelands\\CorinsCrossing',186,213,493,289},
    {'Interface\\WorldMap\\EasternPlaguelands\\CrownGuardTower',202,191,258,351},
    {'Interface\\WorldMap\\EasternPlaguelands\\Darrowshire',248,206,211,462},
    {'Interface\\WorldMap\\EasternPlaguelands\\EastwallTower',181,176,541,184},
    {'Interface\\WorldMap\\EasternPlaguelands\\LakeMereldar',266,241,462,427},
    {'Interface\\WorldMap\\EasternPlaguelands\\LightsHopeChapel',196,220,687,271},
    {'Interface\\WorldMap\\EasternPlaguelands\\LightsShieldTower',243,162,391,271},
    {'Interface\\WorldMap\\EasternPlaguelands\\Northdale',265,232,570,61},
    {'Interface\\WorldMap\\EasternPlaguelands\\NorthpassTower',250,192,401,69},
    {'Interface\\WorldMap\\EasternPlaguelands\\Plaguewood',328,253,144,40},
    {'Interface\\WorldMap\\EasternPlaguelands\\QuelLithienLodge',277,175,351,0},
    {'Interface\\WorldMap\\EasternPlaguelands\\RuinsOfTheScarletEnclave',264,373,738,295},
    {'Interface\\WorldMap\\EasternPlaguelands\\Stratholme',310,178,118,0},
    {'Interface\\WorldMap\\EasternPlaguelands\\Terrordale',258,320,0,10},
    {'Interface\\WorldMap\\EasternPlaguelands\\TheFungalVale',274,216,183,211},
    {'Interface\\WorldMap\\EasternPlaguelands\\ThePestilentScar',182,320,383,348},
    {'Interface\\WorldMap\\EasternPlaguelands\\TheMarrisStead',202,202,133,335},
    {'Interface\\WorldMap\\EasternPlaguelands\\TheNoxiousGlade',297,299,650,55},
    {'Interface\\WorldMap\\EasternPlaguelands\\TheInfectisScar',177,266,595,263},
    {'Interface\\WorldMap\\EasternPlaguelands\\TheUndercroft',280,211,56,457},
    {'Interface\\WorldMap\\EasternPlaguelands\\ThondorilRiver',262,526,0,100},
    {'Interface\\WorldMap\\EasternPlaguelands\\Tyrshand',214,254,651,414},
    {'Interface\\WorldMap\\EasternPlaguelands\\ZulMashar',286,176,528,0},
    {'',0,0,0,0},
  },
  ['Elwynn']={
    {'Interface\\WorldMap\\Elwynn\\Goldshire',276,231,247,294},
    {'Interface\\WorldMap\\Elwynn\\FargodeepMine',269,248,240,420},
    {'Interface\\WorldMap\\Elwynn\\NorthshireValley',295,296,355,138},
    {'Interface\\WorldMap\\Elwynn\\JerodsLanding',230,206,396,430},
    {'Interface\\WorldMap\\Elwynn\\TowerofAzora',270,241,529,287},
    {'Interface\\WorldMap\\Elwynn\\BrackwellPumpkinPatch',287,216,532,424},
    {'Interface\\WorldMap\\Elwynn\\EastvaleLoggingCamp',294,243,703,292},
    {'Interface\\WorldMap\\Elwynn\\RidgepointTower',285,194,708,442},
    {'Interface\\WorldMap\\Elwynn\\CrystalLake',220,207,417,327},
    {'Interface\\WorldMap\\Elwynn\\StonecairnLake',340,272,552,186},
    {'Interface\\WorldMap\\Elwynn\\Stromwind',512,422,0,0},
    {'Interface\\WorldMap\\Elwynn\\WestbrookGarrison',269,313,116,355},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['EversongWoods']={
    {'Interface\\WorldMap\\EversongWoods\\SunstriderIsle',512,512,195,5},
    {'Interface\\WorldMap\\EversongWoods\\RuinsofSilvermoon',256,256,307,136},
    {'Interface\\WorldMap\\EversongWoods\\WestSanctum',128,256,292,319},
    {'Interface\\WorldMap\\EversongWoods\\SunsailAnchorage',256,128,231,404},
    {'Interface\\WorldMap\\EversongWoods\\NorthSanctum',256,256,361,298},
    {'Interface\\WorldMap\\EversongWoods\\EastSanctum',256,256,460,373},
    {'Interface\\WorldMap\\EversongWoods\\FarstriderRetreat',256,128,524,359},
    {'Interface\\WorldMap\\EversongWoods\\StillwhisperPond',256,256,474,314},
    {'Interface\\WorldMap\\EversongWoods\\DuskwitherGrounds',256,256,605,253},
    {'Interface\\WorldMap\\EversongWoods\\FairbreezeVilliage',256,256,386,386},
    {'Interface\\WorldMap\\EversongWoods\\TheLivingWood',128,248,511,420},
    {'Interface\\WorldMap\\EversongWoods\\TorWatha',256,353,648,315},
    {'Interface\\WorldMap\\EversongWoods\\TheScortchedGrove',256,128,255,507},
    {'Interface\\WorldMap\\EversongWoods\\SilvermoonCity',512,512,440,87},
    {'Interface\\WorldMap\\EversongWoods\\AzurebreezeCoast',256,256,669,228},
    {'Interface\\WorldMap\\EversongWoods\\ElrendarFalls',128,256,580,399},
    {'Interface\\WorldMap\\EversongWoods\\GoldenboughPass',256,128,243,469},
    {'Interface\\WorldMap\\EversongWoods\\LakeElrendar',128,197,584,471},
    {'Interface\\WorldMap\\EversongWoods\\RunestoneFalithas',256,172,378,496},
    {'Interface\\WorldMap\\EversongWoods\\RunestoneShandor',256,174,464,494},
    {'Interface\\WorldMap\\EversongWoods\\SatherilsHaven',256,256,324,384},
    {'Interface\\WorldMap\\EversongWoods\\TheGoldenStrand',128,253,183,415},
    {'Interface\\WorldMap\\EversongWoods\\ThuronsLivery',256,128,539,305},
    {'Interface\\WorldMap\\EversongWoods\\TranquilShore',256,256,215,298},
    {'Interface\\WorldMap\\EversongWoods\\Zebwatha',128,193,554,475},
    {'',0,0,0,0},
  },
  ['Ghostlands']={
    {'Interface\\WorldMap\\Ghostlands\\Tranquillien',256,512,365,2},
    {'Interface\\WorldMap\\Ghostlands\\SuncrownVillage',512,256,460,0},
    {'Interface\\WorldMap\\Ghostlands\\GoldenmistVillage',512,512,44,0},
    {'Interface\\WorldMap\\Ghostlands\\WindrunnerVillage',256,512,60,117},
    {'Interface\\WorldMap\\Ghostlands\\SanctumoftheMoon',256,256,210,126},
    {'Interface\\WorldMap\\Ghostlands\\SanctumoftheSun',256,512,448,150},
    {'Interface\\WorldMap\\Ghostlands\\DawnstarSpire',427,256,575,0},
    {'Interface\\WorldMap\\Ghostlands\\FarstriderEnclave',429,256,573,136},
    {'Interface\\WorldMap\\Ghostlands\\HowlingZiggurat',256,449,340,219},
    {'Interface\\WorldMap\\Ghostlands\\Deatholme',512,293,95,375},
    {'Interface\\WorldMap\\Ghostlands\\ZebNowa',512,431,466,237},
    {'Interface\\WorldMap\\Ghostlands\\AmaniPass',404,436,598,232},
    {'Interface\\WorldMap\\Ghostlands\\WindrunnerSpire',256,256,40,287},
    {'Interface\\WorldMap\\Ghostlands\\BleedingZiggurat',256,256,184,238},
    {'Interface\\WorldMap\\Ghostlands\\ElrendarCrossing',512,256,326,0},
    {'Interface\\WorldMap\\Ghostlands\\ThalassiaPass',256,262,364,406},
  },
  ['HillsbradFoothills']={
    {'Interface\\WorldMap\\HillsbradFoothills\\AzurelodeMine',180,182,287,399},
    {'Interface\\WorldMap\\HillsbradFoothills\\ChillwindPoint',447,263,555,68},
    {'Interface\\WorldMap\\HillsbradFoothills\\CorrahnsDagger',135,160,426,224},
    {'Interface\\WorldMap\\HillsbradFoothills\\CrushridgeHold',134,124,463,101},
    {'Interface\\WorldMap\\HillsbradFoothills\\DalaranCrater',316,238,102,137},
    {'Interface\\WorldMap\\HillsbradFoothills\\DandredsFold',258,113,341,0},
    {'Interface\\WorldMap\\HillsbradFoothills\\DarrowHill',147,160,425,279},
    {'Interface\\WorldMap\\HillsbradFoothills\\DunGarok',269,258,542,410},
    {'Interface\\WorldMap\\HillsbradFoothills\\DurnholdeKeep',437,451,565,217},
    {'Interface\\WorldMap\\HillsbradFoothills\\GallowsCorner',155,147,451,140},
    {'Interface\\WorldMap\\HillsbradFoothills\\GavinsNaze',116,129,344,254},
    {'Interface\\WorldMap\\HillsbradFoothills\\GrowlessCave',171,136,359,191},
    {'Interface\\WorldMap\\HillsbradFoothills\\HillsbradFields',302,175,191,302},
    {'Interface\\WorldMap\\HillsbradFoothills\\LordamereInternmentCamp',250,167,194,216},
    {'Interface\\WorldMap\\HillsbradFoothills\\MistyShore',158,169,321,42},
    {'Interface\\WorldMap\\HillsbradFoothills\\NethanderSteed',204,244,502,373},
    {'Interface\\WorldMap\\HillsbradFoothills\\PurgationIsle',144,139,200,505},
    {'Interface\\WorldMap\\HillsbradFoothills\\RuinsOfAlterac',189,181,347,85},
    {'Interface\\WorldMap\\HillsbradFoothills\\SlaughterHollow',148,120,413,55},
    {'Interface\\WorldMap\\HillsbradFoothills\\SoferasNaze',148,146,484,166},
    {'Interface\\WorldMap\\HillsbradFoothills\\SouthpointTower',312,254,59,310},
    {'Interface\\WorldMap\\HillsbradFoothills\\Southshore',229,219,383,352},
    {'Interface\\WorldMap\\HillsbradFoothills\\Strahnbrad',275,193,505,44},
    {'Interface\\WorldMap\\HillsbradFoothills\\TarrenMill',165,203,494,226},
    {'Interface\\WorldMap\\HillsbradFoothills\\TheHeadland',105,148,390,255},
    {'Interface\\WorldMap\\HillsbradFoothills\\TheUplands',212,160,441,0},
    {'Interface\\WorldMap\\HillsbradFoothills\\HillsbradFields',302,175,191,302},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Sunwell']={
    {'Interface\\WorldMap\\Sunwell\\SunsReachHarbor',512,416,252,252},
    {'Interface\\WorldMap\\Sunwell\\SunsReachSanctum',512,512,251,4},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['VashjirKelpForest']={
    {'Interface\\WorldMap\\VashjirKelpForest\\GnawsBoneyard',311,217,451,325},
    {'Interface\\WorldMap\\VashjirKelpForest\\GubogglesLedge',227,207,399,280},
    {'Interface\\WorldMap\\VashjirKelpForest\\HoldingPens',316,267,456,401},
    {'Interface\\WorldMap\\VashjirKelpForest\\HonorsTomb',291,206,380,43},
    {'Interface\\WorldMap\\VashjirKelpForest\\LegionsFate',278,315,210,35},
    {'Interface\\WorldMap\\VashjirKelpForest\\TheAccursedReef',340,225,365,162},
    {'Interface\\WorldMap\\VashjirKelpForest\\DarkwhisperGorge',220,189,528,228},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['LochModan']={
    {'Interface\\WorldMap\\LochModan\\MogroshStronghold',294,249,549,52},
    {'Interface\\WorldMap\\LochModan\\TheLoch',330,474,340,81},
    {'Interface\\WorldMap\\LochModan\\SilverStreamMine',225,252,221,0},
    {'Interface\\WorldMap\\LochModan\\NorthgatePass',319,289,16,0},
    {'Interface\\WorldMap\\LochModan\\TheFarstriderLodge',349,292,570,209},
    {'Interface\\WorldMap\\LochModan\\IronbandsExcavationSite',397,291,481,296},
    {'Interface\\WorldMap\\LochModan\\GrizzlepawRidge',273,230,245,324},
    {'Interface\\WorldMap\\LochModan\\Thelsamar',455,295,0,146},
    {'Interface\\WorldMap\\LochModan\\StonesplinterValley',273,294,177,345},
    {'Interface\\WorldMap\\LochModan\\ValleyofKings',310,345,0,311},
    {'Interface\\WorldMap\\LochModan\\StronewroughtDam',333,200,339,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['StranglethornJungle']={
    {'Interface\\WorldMap\\StranglethornJungle\\BaliaMahRuins',239,205,397,243},
    {'Interface\\WorldMap\\StranglethornJungle\\MizjahRuins',157,173,387,246},
    {'Interface\\WorldMap\\StranglethornJungle\\GromGolBaseCamp',167,179,298,228},
    {'Interface\\WorldMap\\StranglethornJungle\\MoshOggOgreMound',234,206,543,253},
    {'Interface\\WorldMap\\StranglethornJungle\\LakeNazferiti',240,228,413,95},
    {'Interface\\WorldMap\\StranglethornJungle\\KalAiRuins',139,150,354,184},
    {'Interface\\WorldMap\\StranglethornJungle\\BalAlRuins',159,137,267,168},
    {'Interface\\WorldMap\\StranglethornJungle\\TheVileReef',236,224,140,208},
    {'Interface\\WorldMap\\StranglethornJungle\\RuinsOfZulKunda',228,265,158,0},
    {'Interface\\WorldMap\\StranglethornJungle\\NesingwarysExpedition',227,190,306,63},
    {'Interface\\WorldMap\\StranglethornJungle\\RebelCamp',302,166,306,0},
    {'Interface\\WorldMap\\StranglethornJungle\\KurzensCompound',244,238,499,0},
    {'Interface\\WorldMap\\StranglethornJungle\\ZulGurub',376,560,626,0},
    {'Interface\\WorldMap\\StranglethornJungle\\Bambala',190,176,566,164},
    {'Interface\\WorldMap\\StranglethornJungle\\FortLivingston',230,170,398,375},
    {'Interface\\WorldMap\\StranglethornJungle\\ZuuldalaRuins',324,263,9,22},
    {'Interface\\WorldMap\\StranglethornJungle\\Mazthoril',350,259,488,364},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Redridge']={
    {'Interface\\WorldMap\\Redridge\\ThreeCorners',323,406,0,256},
    {'Interface\\WorldMap\\Redridge\\LakeridgeHighway',392,352,148,316},
    {'Interface\\WorldMap\\Redridge\\LakeEverstill',464,250,81,214},
    {'Interface\\WorldMap\\Redridge\\Lakeshire',410,256,0,110},
    {'Interface\\WorldMap\\Redridge\\RedridgeCanyons',413,292,37,0},
    {'Interface\\WorldMap\\Redridge\\RendersCamp',357,246,214,0},
    {'Interface\\WorldMap\\Redridge\\AlthersMill',228,247,350,139},
    {'Interface\\WorldMap\\Redridge\\RendersValley',427,291,451,377},
    {'Interface\\WorldMap\\Redridge\\StonewatchFalls',316,182,525,302},
    {'Interface\\WorldMap\\Redridge\\GalardellValley',428,463,574,0},
    {'Interface\\WorldMap\\Redridge\\CampEverstill',189,193,445,286},
    {'Interface\\WorldMap\\Redridge\\ShalewindCanyon',306,324,688,283},
    {'Interface\\WorldMap\\Redridge\\StonewatchKeep',228,420,480,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['RuinsofGilneas']={
    {'Interface\\WorldMap\\RuinsofGilneas\\GilneasPuzzle',1002,668,0,0},
  },
  ['RuinsofGilneasCity']={
  },
  ['Gilneas_terrain2']={
    {'Interface\\WorldMap\\Gilneas_terrain2\\CrowleyOrchard',210,166,261,427},
    {'Interface\\WorldMap\\Gilneas_terrain2\\Duskhaven',286,178,272,333},
    {'Interface\\WorldMap\\Gilneas_terrain2\\EmberstoneMine',281,351,639,43},
    {'Interface\\WorldMap\\Gilneas_terrain2\\Greymanemanor',244,241,141,202},
    {'Interface\\WorldMap\\Gilneas_terrain2\\HammondFarmstead',194,236,167,352},
    {'Interface\\WorldMap\\Gilneas_terrain2\\HaywardFishery',177,219,293,449},
    {'Interface\\WorldMap\\Gilneas_terrain2\\Keelharbor',280,342,298,95},
    {'Interface\\WorldMap\\Gilneas_terrain2\\KorothsDen',222,268,393,386},
    {'Interface\\WorldMap\\Gilneas_terrain2\\NorthernHeadlands',267,314,387,0},
    {'Interface\\WorldMap\\Gilneas_terrain2\\StormglenVillage',321,203,516,465},
    {'Interface\\WorldMap\\Gilneas_terrain2\\TempestsReach',350,345,652,290},
    {'Interface\\WorldMap\\Gilneas_terrain2\\TheBlackwald',280,224,504,394},
    {'Interface\\WorldMap\\Gilneas_terrain2\\TheHeadlands',328,336,160,0},
    {'Interface\\WorldMap\\Gilneas_terrain2\\GilneasCity',282,263,483,210},
  },
  ['Gilneas']={
    {'Interface\\WorldMap\\Gilneas\\CrowleyOrchard',210,166,261,427},
    {'Interface\\WorldMap\\Gilneas\\Duskhaven',286,178,272,333},
    {'Interface\\WorldMap\\Gilneas\\EmberstoneMine',281,351,639,43},
    {'Interface\\WorldMap\\Gilneas\\Greymanemanor',244,241,141,202},
    {'Interface\\WorldMap\\Gilneas\\HammondFarmstead',194,236,167,352},
    {'Interface\\WorldMap\\Gilneas\\HaywardFishery',177,219,293,449},
    {'Interface\\WorldMap\\Gilneas\\Keelharbor',280,342,298,95},
    {'Interface\\WorldMap\\Gilneas\\KorothsDen',222,268,393,386},
    {'Interface\\WorldMap\\Gilneas\\NorthernHeadlands',267,314,387,0},
    {'Interface\\WorldMap\\Gilneas\\StormglenVillage',321,203,516,465},
    {'Interface\\WorldMap\\Gilneas\\TempestsReach',350,345,652,290},
    {'Interface\\WorldMap\\Gilneas\\TheBlackwald',280,224,504,394},
    {'Interface\\WorldMap\\Gilneas\\TheHeadlands',328,336,160,0},
    {'Interface\\WorldMap\\Gilneas\\GilneasCity',282,263,483,210},
  },
  ['SearingGorge']={
    {'Interface\\WorldMap\\SearingGorge\\BlackcharCave',375,307,0,361},
    {'Interface\\WorldMap\\SearingGorge\\BlackrockMountain',304,244,243,424},
    {'Interface\\WorldMap\\SearingGorge\\DustfireValley',392,355,588,0},
    {'Interface\\WorldMap\\SearingGorge\\FirewatchRidge',365,393,0,75},
    {'Interface\\WorldMap\\SearingGorge\\GrimsiltWorksite',441,266,531,241},
    {'Interface\\WorldMap\\SearingGorge\\TheCauldron',481,360,232,171},
    {'Interface\\WorldMap\\SearingGorge\\ThoriumPoint',429,301,255,38},
    {'Interface\\WorldMap\\SearingGorge\\TannerCamp',571,308,413,360},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['VashjirRuins']={
    {'Interface\\WorldMap\\VashjirRuins\\BethMoraRidge',335,223,407,445},
    {'Interface\\WorldMap\\VashjirRuins\\GlimmeringdeepGorge',272,180,270,222},
    {'Interface\\WorldMap\\VashjirRuins\\Nespirah',286,269,460,261},
    {'Interface\\WorldMap\\VashjirRuins\\RuinsOfTherseral',197,223,554,175},
    {'Interface\\WorldMap\\VashjirRuins\\RuinsOfVashjir',349,361,217,268},
    {'Interface\\WorldMap\\VashjirRuins\\ShimmeringGrotto',339,278,400,0},
    {'Interface\\WorldMap\\VashjirRuins\\SilverTideHollow',480,319,150,32},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Silverpine']={
    {'Interface\\WorldMap\\Silverpine\\TheSkitteringDark',227,172,236,0},
    {'Interface\\WorldMap\\Silverpine\\FenrisIsle',352,302,581,15},
    {'Interface\\WorldMap\\Silverpine\\TheSepulcher',218,200,341,157},
    {'Interface\\WorldMap\\Silverpine\\DeepElemMine',217,198,483,212},
    {'Interface\\WorldMap\\Silverpine\\OlsensFarthing',251,167,312,249},
    {'Interface\\WorldMap\\Silverpine\\Ambermill',283,243,509,250},
    {'Interface\\WorldMap\\Silverpine\\ShadowfangKeep',179,165,337,337},
    {'Interface\\WorldMap\\Silverpine\\TheGreymaneWall',409,162,318,506},
    {'Interface\\WorldMap\\Silverpine\\BerensPeril',318,263,505,405},
    {'Interface\\WorldMap\\Silverpine\\ForsakenHighCommand',361,175,445,0},
    {'Interface\\WorldMap\\Silverpine\\ForsakenRearGuard',186,238,369,0},
    {'Interface\\WorldMap\\Silverpine\\NorthTidesBeachhead',174,199,323,68},
    {'Interface\\WorldMap\\Silverpine\\NorthTidesRun',281,345,147,0},
    {'Interface\\WorldMap\\Silverpine\\TheBattlefront',255,180,349,429},
    {'Interface\\WorldMap\\Silverpine\\TheDecrepitFields',176,152,471,156},
    {'Interface\\WorldMap\\Silverpine\\TheForsakenFront',152,189,433,327},
    {'Interface\\WorldMap\\Silverpine\\ValgansField',162,172,461,77},
    {'',0,0,0,0},
  },
  ['SwampOfSorrows']={
    {'Interface\\WorldMap\\SwampOfSorrows\\Bogpaddle',262,193,600,0},
    {'Interface\\WorldMap\\SwampOfSorrows\\IthariusCave',268,316,7,242},
    {'Interface\\WorldMap\\SwampOfSorrows\\MarshtideWatch',330,342,478,0},
    {'Interface\\WorldMap\\SwampOfSorrows\\MistyreedStrand',402,668,600,0},
    {'Interface\\WorldMap\\SwampOfSorrows\\MistyValley',268,285,0,80},
    {'Interface\\WorldMap\\SwampOfSorrows\\PoolOfTears',257,229,575,238},
    {'Interface\\WorldMap\\SwampOfSorrows\\Sorrowmurk',229,418,703,80},
    {'Interface\\WorldMap\\SwampOfSorrows\\SplinterspearJunction',238,343,194,236},
    {'Interface\\WorldMap\\SwampOfSorrows\\Stagalbog',347,303,540,360},
    {'Interface\\WorldMap\\SwampOfSorrows\\Stonard',357,308,297,258},
    {'Interface\\WorldMap\\SwampOfSorrows\\TheHarborage',266,284,161,79},
    {'Interface\\WorldMap\\SwampOfSorrows\\TheShiftingMire',292,360,331,24},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TheCapeOfStranglethorn']={
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\BootyBay',225,255,289,341},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\CrystalveinMine',271,204,528,73},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\GurubashiArena',238,260,345,0},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\JagueroIsle',240,264,471,404},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\MistvaleValley',253,242,408,248},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\NekmaniWellspring',246,221,292,213},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\RuinsofAboraz',184,176,533,181},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\RuinsofJubuwal',155,221,468,119},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\TheSundering',244,209,452,0},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\WildShore',236,276,340,392},
    {'Interface\\WorldMap\\TheCapeOfStranglethorn\\HardwrenchHideaway',356,221,208,116},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Hinterlands']={
    {'Interface\\WorldMap\\Hinterlands\\AeriePeak',238,267,0,236},
    {'Interface\\WorldMap\\Hinterlands\\PlaguemistRavine',191,278,133,105},
    {'Interface\\WorldMap\\Hinterlands\\QuelDanilLodge',241,211,220,181},
    {'Interface\\WorldMap\\Hinterlands\\ShadraAlor',240,196,220,379},
    {'Interface\\WorldMap\\Hinterlands\\ValorwindLake',199,212,286,269},
    {'Interface\\WorldMap\\Hinterlands\\Agolwatha',208,204,367,159},
    {'Interface\\WorldMap\\Hinterlands\\TheCreepingRuin',199,199,390,252},
    {'Interface\\WorldMap\\Hinterlands\\TheAltarofZul',225,196,357,343},
    {'Interface\\WorldMap\\Hinterlands\\Seradane',303,311,475,5},
    {'Interface\\WorldMap\\Hinterlands\\SkulkRock',176,235,490,195},
    {'Interface\\WorldMap\\Hinterlands\\Shaolwatha',281,261,565,208},
    {'Interface\\WorldMap\\Hinterlands\\JinthaAlor',287,289,487,334},
    {'Interface\\WorldMap\\Hinterlands\\TheOverlookCliffs',244,401,677,267},
    {'Interface\\WorldMap\\Hinterlands\\Zunwatha',226,225,152,284},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Tirisfal']={
    {'Interface\\WorldMap\\Tirisfal\\VenomwebVale',250,279,752,150},
    {'Interface\\WorldMap\\Tirisfal\\ScarletWatchPost',161,234,692,99},
    {'Interface\\WorldMap\\Tirisfal\\CrusaderOutpost',175,210,686,232},
    {'Interface\\WorldMap\\Tirisfal\\BalnirFarmstead',242,179,594,324},
    {'Interface\\WorldMap\\Tirisfal\\BrightwaterLake',210,292,573,122},
    {'Interface\\WorldMap\\Tirisfal\\GarrensHaunt',190,214,477,129},
    {'Interface\\WorldMap\\Tirisfal\\Brill',199,182,480,252},
    {'Interface\\WorldMap\\Tirisfal\\ColdHearthManor',212,177,418,317},
    {'Interface\\WorldMap\\Tirisfal\\NightmareVale',225,281,347,325},
    {'Interface\\WorldMap\\Tirisfal\\AgamandMills',285,260,324,90},
    {'Interface\\WorldMap\\Tirisfal\\SollidenFarmstead',286,225,201,192},
    {'Interface\\WorldMap\\Tirisfal\\Deathknell',431,407,9,207},
    {'Interface\\WorldMap\\Tirisfal\\CalstonEstate',179,169,389,255},
    {'Interface\\WorldMap\\Tirisfal\\RuinsofLorderon',390,267,423,359},
    {'Interface\\WorldMap\\Tirisfal\\ScarletMonastery',262,262,740,47},
    {'Interface\\WorldMap\\Tirisfal\\TheBulwark',293,338,709,330},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TolBarad']={
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TwilightHighlands']={
    {'Interface\\WorldMap\\TwilightHighlands\\Bloodgulch',215,157,416,205},
    {'Interface\\WorldMap\\TwilightHighlands\\CrucibleOfCarnage',203,208,387,268},
    {'Interface\\WorldMap\\TwilightHighlands\\Crushblow',182,195,370,447},
    {'Interface\\WorldMap\\TwilightHighlands\\DragonmawPass',283,206,76,120},
    {'Interface\\WorldMap\\TwilightHighlands\\DragonmawPort',251,207,631,245},
    {'Interface\\WorldMap\\TwilightHighlands\\DunwaldRuins',197,218,395,367},
    {'Interface\\WorldMap\\TwilightHighlands\\FirebeardsPatrol',215,181,499,265},
    {'Interface\\WorldMap\\TwilightHighlands\\GlopgutsHollow',174,190,291,89},
    {'Interface\\WorldMap\\TwilightHighlands\\GorshakWarCamp',194,170,543,220},
    {'Interface\\WorldMap\\TwilightHighlands\\Highbank',220,227,697,403},
    {'Interface\\WorldMap\\TwilightHighlands\\HighlandForest',239,232,482,330},
    {'Interface\\WorldMap\\TwilightHighlands\\HumboldtConflaguration',143,141,344,89},
    {'Interface\\WorldMap\\TwilightHighlands\\Kirthaven',308,267,482,0},
    {'Interface\\WorldMap\\TwilightHighlands\\ObsidianForest',342,288,436,380},
    {'Interface\\WorldMap\\TwilightHighlands\\RuinsOfDrakgor',206,182,296,0},
    {'Interface\\WorldMap\\TwilightHighlands\\SlitheringCove',198,201,622,169},
    {'Interface\\WorldMap\\TwilightHighlands\\TheBlackBreach',211,210,498,121},
    {'Interface\\WorldMap\\TwilightHighlands\\TheGullet',175,180,269,179},
    {'Interface\\WorldMap\\TwilightHighlands\\TheKrazzworks',226,232,654,0},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightBreach',199,212,312,192},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightCitadel',361,354,151,314},
    {'Interface\\WorldMap\\TwilightHighlands\\Thundermar',238,229,374,93},
    {'Interface\\WorldMap\\TwilightHighlands\\TwilightShore',260,202,610,345},
    {'Interface\\WorldMap\\TwilightHighlands\\VermillionRedoubt',324,264,71,16},
    {'Interface\\WorldMap\\TwilightHighlands\\VictoryPoint',177,159,302,306},
    {'Interface\\WorldMap\\TwilightHighlands\\WyrmsBend',191,198,205,232},
    {'Interface\\WorldMap\\TwilightHighlands\\WeepingWound',214,190,358,0},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightGate',165,199,327,356},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TwilightHighlands_terrain1']={
    {'Interface\\WorldMap\\TwilightHighlands\\Bloodgulch',215,157,416,205},
    {'Interface\\WorldMap\\TwilightHighlands\\CrucibleOfCarnage',203,208,387,268},
    {'Interface\\WorldMap\\TwilightHighlands\\Crushblow',182,195,370,447},
    {'Interface\\WorldMap\\TwilightHighlands\\DragonmawPass',283,206,76,120},
    {'Interface\\WorldMap\\TwilightHighlands\\DragonmawPort',251,207,631,245},
    {'Interface\\WorldMap\\TwilightHighlands\\DunwaldRuins',197,218,395,367},
    {'Interface\\WorldMap\\TwilightHighlands\\FirebeardsPatrol',215,181,499,265},
    {'Interface\\WorldMap\\TwilightHighlands\\GlopgutsHollow',174,190,291,89},
    {'Interface\\WorldMap\\TwilightHighlands\\GorshakWarCamp',194,170,543,220},
    {'Interface\\WorldMap\\TwilightHighlands\\Highbank',220,227,697,403},
    {'Interface\\WorldMap\\TwilightHighlands\\HighlandForest',239,232,482,330},
    {'Interface\\WorldMap\\TwilightHighlands\\HumboldtConflaguration',143,141,344,89},
    {'Interface\\WorldMap\\TwilightHighlands\\Kirthaven',308,267,482,0},
    {'Interface\\WorldMap\\TwilightHighlands\\ObsidianForest',342,288,436,380},
    {'Interface\\WorldMap\\TwilightHighlands\\RuinsOfDrakgor',206,182,296,0},
    {'Interface\\WorldMap\\TwilightHighlands\\SlitheringCove',198,201,622,169},
    {'Interface\\WorldMap\\TwilightHighlands\\TheBlackBreach',211,210,498,121},
    {'Interface\\WorldMap\\TwilightHighlands\\TheGullet',175,180,269,179},
    {'Interface\\WorldMap\\TwilightHighlands\\TheKrazzworks',226,232,654,0},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightBreach',199,212,312,192},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightCitadel',361,354,151,314},
    {'Interface\\WorldMap\\TwilightHighlands\\Thundermar',238,229,374,93},
    {'Interface\\WorldMap\\TwilightHighlands\\TwilightShore',260,202,610,345},
    {'Interface\\WorldMap\\TwilightHighlands\\VermillionRedoubt',324,264,71,16},
    {'Interface\\WorldMap\\TwilightHighlands\\VictoryPoint',177,159,302,306},
    {'Interface\\WorldMap\\TwilightHighlands\\WyrmsBend',191,198,205,232},
    {'Interface\\WorldMap\\TwilightHighlands\\WeepingWound',214,190,358,0},
    {'Interface\\WorldMap\\TwilightHighlands\\TheTwilightGate',165,199,327,356},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['WesternPlaguelands']={
    {'Interface\\WorldMap\\WesternPlaguelands\\DarrowmereLake',492,314,510,354},
    {'Interface\\WorldMap\\WesternPlaguelands\\CaerDarrow',194,208,601,390},
    {'Interface\\WorldMap\\WesternPlaguelands\\SorrowHill',368,220,261,448},
    {'Interface\\WorldMap\\WesternPlaguelands\\TheBulwark',316,316,48,235},
    {'Interface\\WorldMap\\WesternPlaguelands\\FelstoneField',241,212,229,228},
    {'Interface\\WorldMap\\WesternPlaguelands\\TheWrithingHaunt',169,195,472,332},
    {'Interface\\WorldMap\\WesternPlaguelands\\NorthridgeLumberCamp',359,182,231,123},
    {'Interface\\WorldMap\\WesternPlaguelands\\Hearthglen',432,271,235,0},
    {'Interface\\WorldMap\\WesternPlaguelands\\GahrronsWithering',241,252,495,213},
    {'Interface\\WorldMap\\WesternPlaguelands\\TheWeepingCave',185,230,551,151},
    {'Interface\\WorldMap\\WesternPlaguelands\\ThondrorilRiver',311,436,533,0},
    {'Interface\\WorldMap\\WesternPlaguelands\\Andorhal',464,325,96,343},
    {'Interface\\WorldMap\\WesternPlaguelands\\DalsonsFarm',325,192,300,232},
    {'Interface\\WorldMap\\WesternPlaguelands\\RedpineDell',290,133,286,211},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Westfall']={
    {'Interface\\WorldMap\\Westfall\\TheJansenStead',202,179,474,0},
    {'Interface\\WorldMap\\Westfall\\FurlbrowsPumpkinFarm',197,213,394,0},
    {'Interface\\WorldMap\\Westfall\\SaldeansFarm',244,237,451,81},
    {'Interface\\WorldMap\\Westfall\\TheMolsenFarm',202,224,348,118},
    {'Interface\\WorldMap\\Westfall\\GoldCoastQuarry',235,306,199,79},
    {'Interface\\WorldMap\\Westfall\\TheDeadAcre',193,273,531,200},
    {'Interface\\WorldMap\\Westfall\\SentinelHill',229,265,404,226},
    {'Interface\\WorldMap\\Westfall\\Moonbrook',232,213,308,325},
    {'Interface\\WorldMap\\Westfall\\AlexstonFarmstead',346,222,167,263},
    {'Interface\\WorldMap\\Westfall\\DemontsPlace',201,195,203,376},
    {'Interface\\WorldMap\\Westfall\\WestfallLighthouse',211,167,221,477},
    {'Interface\\WorldMap\\Westfall\\TheDaggerHills',292,273,303,395},
    {'Interface\\WorldMap\\Westfall\\TheDustPlains',317,261,480,378},
    {'Interface\\WorldMap\\Westfall\\JangoloadMine',196,229,311,0},
    {'Interface\\WorldMap\\Westfall\\TheGapingChasm',184,217,294,168},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Wetlands']={
    {'Interface\\WorldMap\\Wetlands\\MenethilHarbor',325,363,0,297},
    {'Interface\\WorldMap\\Wetlands\\BlackChannelMarsh',301,232,37,240},
    {'Interface\\WorldMap\\Wetlands\\BluegillMarsh',321,248,31,102},
    {'Interface\\WorldMap\\Wetlands\\WhelgarsExcavationSite',298,447,185,195},
    {'Interface\\WorldMap\\Wetlands\\SundownMarsh',276,243,121,63},
    {'Interface\\WorldMap\\Wetlands\\IronbeardsTomb',185,224,372,76},
    {'Interface\\WorldMap\\Wetlands\\DunModr',257,185,356,7},
    {'Interface\\WorldMap\\Wetlands\\AngerfangEncampment',236,256,359,201},
    {'Interface\\WorldMap\\Wetlands\\ThelganRock',258,207,371,335},
    {'Interface\\WorldMap\\Wetlands\\MosshideFen',369,235,506,232},
    {'Interface\\WorldMap\\Wetlands\\RaptorRidge',256,245,599,123},
    {'Interface\\WorldMap\\Wetlands\\DireforgeHills',329,228,506,34},
    {'Interface\\WorldMap\\Wetlands\\DunAlgaz',298,215,346,419},
    {'Interface\\WorldMap\\Wetlands\\GreenwardensGrove',250,269,460,102},
    {'Interface\\WorldMap\\Wetlands\\Satlspray',250,282,218,0},
    {'Interface\\WorldMap\\Wetlands\\SlabchiselsSurvey',300,316,532,352},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
 -- Outland
  ['BladesEdgeMountains']={
    {'Interface\\WorldMap\\BladesEdgeMountains\\BashirLanding',256,256,422,0},
    {'Interface\\WorldMap\\BladesEdgeMountains\\BladedGulch',256,256,623,147},
    {'Interface\\WorldMap\\BladesEdgeMountains\\BladesipreHold',256,507,314,161},
    {'Interface\\WorldMap\\BladesEdgeMountains\\BloodmaulCamp',256,256,412,95},
    {'Interface\\WorldMap\\BladesEdgeMountains\\BloodmaulOutpost',256,297,342,371},
    {'Interface\\WorldMap\\BladesEdgeMountains\\BrokenWilds',256,256,733,109},
    {'Interface\\WorldMap\\BladesEdgeMountains\\CircleofWrath',256,256,439,210},
    {'Interface\\WorldMap\\BladesEdgeMountains\\DeathsDoor',256,419,512,249},
    {'Interface\\WorldMap\\BladesEdgeMountains\\ForgeCampAnger',416,256,586,147},
    {'Interface\\WorldMap\\BladesEdgeMountains\\ForgeCampTerror',512,252,144,416},
    {'Interface\\WorldMap\\BladesEdgeMountains\\ForgeCampWrath',256,256,254,176},
    {'Interface\\WorldMap\\BladesEdgeMountains\\Grishnath',256,256,286,28},
    {'Interface\\WorldMap\\BladesEdgeMountains\\GruulsLayer',256,256,527,81},
    {'Interface\\WorldMap\\BladesEdgeMountains\\JaggedRidge',256,254,446,414},
    {'Interface\\WorldMap\\BladesEdgeMountains\\MokNathalVillage',256,256,658,297},
    {'Interface\\WorldMap\\BladesEdgeMountains\\RavensWood',512,256,214,55},
    {'Interface\\WorldMap\\BladesEdgeMountains\\RazorRidge',256,336,533,332},
    {'Interface\\WorldMap\\BladesEdgeMountains\\RuuanWeald',256,512,479,98},
    {'Interface\\WorldMap\\BladesEdgeMountains\\Skald',256,256,673,71},
    {'Interface\\WorldMap\\BladesEdgeMountains\\Sylvanaar',256,318,289,350},
    {'Interface\\WorldMap\\BladesEdgeMountains\\TheCrystalpine',256,256,585,0},
    {'Interface\\WorldMap\\BladesEdgeMountains\\ThunderlordStronghold',256,396,405,272},
    {'Interface\\WorldMap\\BladesEdgeMountains\\VeilLashh',256,240,271,428},
    {'Interface\\WorldMap\\BladesEdgeMountains\\VeilRuuan',256,128,563,151},
    {'Interface\\WorldMap\\BladesEdgeMountains\\VekhaarStand',256,256,629,406},
    {'Interface\\WorldMap\\BladesEdgeMountains\\VortexPinnacle',256,462,166,206},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\BladesEdgeMountains\\RidgeofMadness',256,410,554,258},
    {'',0,0,0,0},
  },
  ['Hellfire']={
    {'Interface\\WorldMap\\Hellfire\\ExpeditionArmory',512,255,261,413},
    {'Interface\\WorldMap\\Hellfire\\FalconWatch',512,342,183,326},
    {'Interface\\WorldMap\\Hellfire\\HellfireCitadel',256,458,338,210},
    {'Interface\\WorldMap\\Hellfire\\HonorHold',256,256,469,298},
    {'Interface\\WorldMap\\Hellfire\\MagharPost',256,256,206,110},
    {'Interface\\WorldMap\\Hellfire\\PoolsofAggonar',256,512,326,45},
    {'Interface\\WorldMap\\Hellfire\\RuinsofShanaar',256,378,25,290},
    {'Interface\\WorldMap\\Hellfire\\TempleofTelhamat',512,512,38,152},
    {'Interface\\WorldMap\\Hellfire\\TheLegionFront',256,512,579,128},
    {'Interface\\WorldMap\\Hellfire\\TheStairofDestiny',256,512,737,156},
    {'Interface\\WorldMap\\Hellfire\\Thrallmar',256,256,467,154},
    {'Interface\\WorldMap\\Hellfire\\ThroneofKiljaeden',512,256,477,6},
    {'Interface\\WorldMap\\Hellfire\\ZethGor',422,238,580,430},
    {'Interface\\WorldMap\\Hellfire\\DenofHaalesh',256,256,182,412},
    {'Interface\\WorldMap\\Hellfire\\FallenSkyRidge',256,256,34,142},
    {'Interface\\WorldMap\\Hellfire\\ForgeCampRage',512,512,478,25},
    {'Interface\\WorldMap\\Hellfire\\VoidRidge',256,256,705,368},
    {'Interface\\WorldMap\\Hellfire\\WarpFields',256,260,308,408},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Nagrand']={
    {'Interface\\WorldMap\\Nagrand\\ForgeCampFear',512,420,36,248},
    {'Interface\\WorldMap\\Nagrand\\Garadar',256,256,431,143},
    {'Interface\\WorldMap\\Nagrand\\Halaa',256,256,335,193},
    {'Interface\\WorldMap\\Nagrand\\KilsorrowFortress',256,241,558,427},
    {'Interface\\WorldMap\\Nagrand\\LaughingSkullRuins',256,256,351,52},
    {'Interface\\WorldMap\\Nagrand\\OshuGun',512,334,168,334},
    {'Interface\\WorldMap\\Nagrand\\SunspringPost',256,256,219,199},
    {'Interface\\WorldMap\\Nagrand\\Telaar',256,256,387,390},
    {'Interface\\WorldMap\\Nagrand\\RingofTrials',256,256,533,267},
    {'Interface\\WorldMap\\Nagrand\\ThroneoftheElements',256,256,504,53},
    {'Interface\\WorldMap\\Nagrand\\WarmaulHill',256,256,157,32},
    {'Interface\\WorldMap\\Nagrand\\BurningBladeRUins',256,334,660,334},
    {'Interface\\WorldMap\\Nagrand\\ClanWatch',256,256,532,363},
    {'Interface\\WorldMap\\Nagrand\\ForgeCampHate',256,256,162,154},
    {'Interface\\WorldMap\\Nagrand\\SouthwindCleft',256,256,391,258},
    {'Interface\\WorldMap\\Nagrand\\TwilightRidge',256,512,10,107},
    {'Interface\\WorldMap\\Nagrand\\WindyreedPass',256,256,598,79},
    {'Interface\\WorldMap\\Nagrand\\WindyreedVillage',256,256,666,233},
    {'Interface\\WorldMap\\Nagrand\\ZangarRidge',256,256,277,54},
    {'',0,0,0,0},
  },
  ['Netherstorm']={
    {'Interface\\WorldMap\\Netherstorm\\Area52',256,128,241,388},
    {'Interface\\WorldMap\\Netherstorm\\ManaforgeBanar',256,387,147,281},
    {'Interface\\WorldMap\\Netherstorm\\ManaforgeCoruu',256,179,357,489},
    {'Interface\\WorldMap\\Netherstorm\\ManaforgeDuro',256,256,465,336},
    {'Interface\\WorldMap\\Netherstorm\\ManafrogeAra',256,256,171,155},
    {'Interface\\WorldMap\\Netherstorm\\RuinedManaforge',256,256,513,138},
    {'Interface\\WorldMap\\Netherstorm\\RuinsofFarahlon',512,256,354,49},
    {'Interface\\WorldMap\\Netherstorm\\TempestKeep',409,384,593,284},
    {'Interface\\WorldMap\\Netherstorm\\TheHeap',256,213,239,455},
    {'Interface\\WorldMap\\Netherstorm\\ArklonRuins',256,256,328,397},
    {'Interface\\WorldMap\\Netherstorm\\CelestialRidge',256,256,644,173},
    {'Interface\\WorldMap\\Netherstorm\\ForgeBaseOG',256,256,237,22},
    {'Interface\\WorldMap\\Netherstorm\\KirinVarVillage',256,145,490,523},
    {'Interface\\WorldMap\\Netherstorm\\Netherstone',256,256,411,20},
    {'Interface\\WorldMap\\Netherstorm\\RuinsofEnkaat',256,256,253,301},
    {'Interface\\WorldMap\\Netherstorm\\SunfuryHold',256,217,454,451},
    {'Interface\\WorldMap\\Netherstorm\\TheScrapField',256,256,356,261},
    {'Interface\\WorldMap\\Netherstorm\\TheStormspire',256,256,298,134},
    {'Interface\\WorldMap\\Netherstorm\\NetherstormBridge',256,256,132,294},
    {'Interface\\WorldMap\\Netherstorm\\EcoDomeFarfield',256,256,396,10},
    {'Interface\\WorldMap\\Netherstorm\\EtheriumStagingGrounds',256,256,481,208},
    {'Interface\\WorldMap\\Netherstorm\\SocretharsSeat',256,256,229,38},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['ShadowmoonValley']={
    {'Interface\\WorldMap\\ShadowmoonValley\\CoilskarPoint',512,512,348,8},
    {'Interface\\WorldMap\\ShadowmoonValley\\EclipsePoint',512,358,343,310},
    {'Interface\\WorldMap\\ShadowmoonValley\\LegionHold',512,512,104,155},
    {'Interface\\WorldMap\\ShadowmoonValley\\NetherwingLedge',492,223,510,445},
    {'Interface\\WorldMap\\ShadowmoonValley\\ShadowmoonVilliage',512,512,116,35},
    {'Interface\\WorldMap\\ShadowmoonValley\\TheBlackTemple',396,512,606,126},
    {'Interface\\WorldMap\\ShadowmoonValley\\TheDeathForge',256,512,290,129},
    {'Interface\\WorldMap\\ShadowmoonValley\\TheHandofGuldan',512,512,394,90},
    {'Interface\\WorldMap\\ShadowmoonValley\\TheWardensCage',512,410,469,258},
    {'Interface\\WorldMap\\ShadowmoonValley\\WildhammerStronghold',512,439,168,229},
    {'Interface\\WorldMap\\ShadowmoonValley\\AltarofShatar',256,256,520,93},
    {'Interface\\WorldMap\\ShadowmoonValley\\IlladarPoint',256,256,143,256},
    {'Interface\\WorldMap\\ShadowmoonValley\\NetherwingCliffs',256,256,554,308},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TerokkarForest']={
    {'Interface\\WorldMap\\TerokkarForest\\AllerianStronghold',256,256,480,277},
    {'Interface\\WorldMap\\TerokkarForest\\BleedingHollowClanRuins',256,367,103,301},
    {'Interface\\WorldMap\\TerokkarForest\\CenarionThicket',256,256,314,0},
    {'Interface\\WorldMap\\TerokkarForest\\FirewingPoint',385,512,617,149},
    {'Interface\\WorldMap\\TerokkarForest\\GrangolvarVilliage',512,256,143,171},
    {'Interface\\WorldMap\\TerokkarForest\\SkethylMountains',512,320,449,348},
    {'Interface\\WorldMap\\TerokkarForest\\StonebreakerHold',256,256,397,165},
    {'Interface\\WorldMap\\TerokkarForest\\Tuurem',256,512,455,34},
    {'Interface\\WorldMap\\TerokkarForest\\ShattrathCity',512,512,104,4},
    {'Interface\\WorldMap\\TerokkarForest\\RaastokGlade',256,256,505,154},
    {'Interface\\WorldMap\\TerokkarForest\\TheBarrierHills',256,256,116,4},
    {'Interface\\WorldMap\\TerokkarForest\\RazorthornShelf',256,256,478,19},
    {'Interface\\WorldMap\\TerokkarForest\\BonechewerRuins',256,256,521,275},
    {'Interface\\WorldMap\\TerokkarForest\\AuchenaiGrounds',256,234,247,434},
    {'Interface\\WorldMap\\TerokkarForest\\CarrionHill',256,256,377,272},
    {'Interface\\WorldMap\\TerokkarForest\\RefugeCaravan',128,256,316,268},
    {'Interface\\WorldMap\\TerokkarForest\\RingofObservance',256,256,310,345},
    {'Interface\\WorldMap\\TerokkarForest\\SethekkTomb',256,256,245,289},
    {'Interface\\WorldMap\\TerokkarForest\\SmolderingCaravan',256,208,321,460},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\TerokkarForest\\VeilRhaze',256,256,222,362},
    {'Interface\\WorldMap\\TerokkarForest\\WrithingMound',256,256,417,327},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',128,128,0,0},
    {'',0,0,0,0},
  },
  ['Zangarmarsh']={
    {'Interface\\WorldMap\\Zangarmarsh\\AngoroshGrounds',256,256,88,50},
    {'Interface\\WorldMap\\Zangarmarsh\\CenarionRefuge',308,256,694,321},
    {'Interface\\WorldMap\\Zangarmarsh\\FeralfenVillage',512,336,314,332},
    {'Interface\\WorldMap\\Zangarmarsh\\TheHewnBog',256,512,219,51},
    {'Interface\\WorldMap\\Zangarmarsh\\MarshlightLake',256,256,81,152},
    {'Interface\\WorldMap\\Zangarmarsh\\QuaggRidge',256,343,141,325},
    {'Interface\\WorldMap\\Zangarmarsh\\Telredor',256,512,569,112},
    {'Interface\\WorldMap\\Zangarmarsh\\TheDeadMire',286,512,716,128},
    {'Interface\\WorldMap\\Zangarmarsh\\TheLagoon',256,256,512,303},
    {'Interface\\WorldMap\\Zangarmarsh\\TwinspireRuins',256,256,342,249},
    {'Interface\\WorldMap\\Zangarmarsh\\UmbrafenVillage',256,207,720,461},
    {'Interface\\WorldMap\\Zangarmarsh\\Sporeggar',512,256,20,202},
    {'Interface\\WorldMap\\Zangarmarsh\\AngoroshStronghold',256,128,124,0},
    {'Interface\\WorldMap\\Zangarmarsh\\CoilfangReservoir',256,512,462,90},
    {'Interface\\WorldMap\\Zangarmarsh\\OreborHarborage',256,512,329,25},
    {'Interface\\WorldMap\\Zangarmarsh\\TheSpawningGlen',256,256,31,339},
    {'Interface\\WorldMap\\Zangarmarsh\\ZabraJin',256,256,175,232},
    {'Interface\\WorldMap\\Zangarmarsh\\BloodscaleEnclave',256,256,596,412},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
 -- Northrend
  ['BoreanTundra']={
    {'Interface\\WorldMap\\BoreanTundra\\TempleCityOfEnKilah',290,292,712,15},
    {'Interface\\WorldMap\\BoreanTundra\\SteeljawsCaravan',244,319,397,66},
    {'Interface\\WorldMap\\BoreanTundra\\RiplashStrand',382,258,293,383},
    {'Interface\\WorldMap\\BoreanTundra\\Kaskala',385,316,509,214},
    {'Interface\\WorldMap\\BoreanTundra\\GarroshsLanding',267,378,153,238},
    {'Interface\\WorldMap\\BoreanTundra\\DeathsStand',289,279,707,181},
    {'Interface\\WorldMap\\BoreanTundra\\Coldarra',460,381,50,0},
    {'Interface\\WorldMap\\BoreanTundra\\BorGorokOutpost',396,203,314,0},
    {'Interface\\WorldMap\\BoreanTundra\\AmberLedge',244,214,325,140},
    {'Interface\\WorldMap\\BoreanTundra\\WarsongStronghold',260,278,329,237},
    {'Interface\\WorldMap\\BoreanTundra\\ValianceKeep',259,302,457,264},
    {'Interface\\WorldMap\\BoreanTundra\\TheGeyserFields',375,342,480,0},
    {'Interface\\WorldMap\\BoreanTundra\\TheDensOfDying',203,209,662,11},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['CrystalsongForest']={
    {'Interface\\WorldMap\\CrystalsongForest\\TheAzureFront',416,424,0,244},
    {'Interface\\WorldMap\\CrystalsongForest\\TheDecrepitFlow',288,222,0,0},
    {'Interface\\WorldMap\\CrystalsongForest\\SunreaversCommand',446,369,536,40},
    {'Interface\\WorldMap\\CrystalsongForest\\ForlornWoods',544,668,129,0},
    {'Interface\\WorldMap\\CrystalsongForest\\WindrunnersOverlook',558,285,444,383},
    {'Interface\\WorldMap\\CrystalsongForest\\VioletStand',264,303,0,176},
    {'Interface\\WorldMap\\CrystalsongForest\\TheGreatTree',252,260,0,91},
    {'Interface\\WorldMap\\CrystalsongForest\\TheUnboundThicket',502,477,500,105},
    {'',0,0,0,0},
  },
  ['Dragonblight']={
    {'Interface\\WorldMap\\Dragonblight\\RubyDragonshrine',188,211,374,208},
    {'Interface\\WorldMap\\Dragonblight\\ObsidianDragonshrine',304,203,256,104},
    {'Interface\\WorldMap\\Dragonblight\\NewHearthglen',214,261,614,358},
    {'Interface\\WorldMap\\Dragonblight\\Naxxramas',311,272,691,160},
    {'Interface\\WorldMap\\Dragonblight\\LightsRest',299,278,703,7},
    {'Interface\\WorldMap\\Dragonblight\\LakeIndule',356,300,217,313},
    {'Interface\\WorldMap\\Dragonblight\\IcemistVillage',235,337,134,165},
    {'Interface\\WorldMap\\Dragonblight\\GalakrondsRest',258,225,433,118},
    {'Interface\\WorldMap\\Dragonblight\\EmeraldDragonshrine',196,218,543,362},
    {'Interface\\WorldMap\\Dragonblight\\ColdwindHeights',213,219,403,0},
    {'Interface\\WorldMap\\Dragonblight\\Angrathar',306,242,210,0},
    {'Interface\\WorldMap\\Dragonblight\\AgmarsHammer',236,218,258,203},
    {'Interface\\WorldMap\\Dragonblight\\WyrmrestTemple',317,353,453,219},
    {'Interface\\WorldMap\\Dragonblight\\WestwindRefugeeCamp',229,299,42,187},
    {'Interface\\WorldMap\\Dragonblight\\VenomSpite',226,212,661,264},
    {'Interface\\WorldMap\\Dragonblight\\TheForgottenShore',301,286,698,332},
    {'Interface\\WorldMap\\Dragonblight\\TheCrystalVice',229,259,487,0},
    {'Interface\\WorldMap\\Dragonblight\\ScarletPoint',235,354,569,7},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['GrizzlyHills']={
    {'Interface\\WorldMap\\GrizzlyHills\\ConquestHold',332,294,17,307},
    {'Interface\\WorldMap\\GrizzlyHills\\DrakTheronKeep',382,285,0,46},
    {'Interface\\WorldMap\\GrizzlyHills\\DrakilJinRuins',351,284,607,41},
    {'Interface\\WorldMap\\GrizzlyHills\\DunArgol',455,400,547,257},
    {'Interface\\WorldMap\\GrizzlyHills\\GraniteSprings',356,224,7,207},
    {'Interface\\WorldMap\\GrizzlyHills\\GrizzleMaw',294,227,358,187},
    {'Interface\\WorldMap\\GrizzlyHills\\RageFangShrine',475,362,312,294},
    {'Interface\\WorldMap\\GrizzlyHills\\ThorModan',329,246,509,0},
    {'Interface\\WorldMap\\GrizzlyHills\\VentureBay',274,207,18,461},
    {'Interface\\WorldMap\\GrizzlyHills\\Voldrune',283,247,176,421},
    {'Interface\\WorldMap\\GrizzlyHills\\AmberpineLodge',278,290,217,244},
    {'Interface\\WorldMap\\GrizzlyHills\\BlueSkyLoggingGrounds',249,235,232,129},
    {'Interface\\WorldMap\\GrizzlyHills\\CampOneqwah',324,265,548,137},
    {'Interface\\WorldMap\\GrizzlyHills\\UrsocsDen',328,260,331,32},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['HowlingFjord']={
    {'Interface\\WorldMap\\HowlingFjord\\CauldrosIsle',181,178,490,161},
    {'Interface\\WorldMap\\HowlingFjord\\CampWinterHoof',223,209,354,0},
    {'Interface\\WorldMap\\HowlingFjord\\ApothecaryCamp',263,265,99,37},
    {'Interface\\WorldMap\\HowlingFjord\\VengeanceLanding',223,338,664,25},
    {'Interface\\WorldMap\\HowlingFjord\\SteelGate',222,168,222,100},
    {'Interface\\WorldMap\\HowlingFjord\\ScalawagPoint',350,258,168,410},
    {'Interface\\WorldMap\\HowlingFjord\\Nifflevar',178,208,595,240},
    {'Interface\\WorldMap\\HowlingFjord\\Gjalerbron',242,189,225,0},
    {'Interface\\WorldMap\\HowlingFjord\\ExplorersLeagueOutpost',232,216,585,336},
    {'Interface\\WorldMap\\HowlingFjord\\EmberClutch',213,256,283,203},
    {'Interface\\WorldMap\\HowlingFjord\\GiantsRun',298,306,572,0},
    {'Interface\\WorldMap\\HowlingFjord\\FortWildervar',251,192,490,0},
    {'Interface\\WorldMap\\HowlingFjord\\IvaldsRuin',193,201,668,223},
    {'Interface\\WorldMap\\HowlingFjord\\Halgrind',187,263,397,208},
    {'Interface\\WorldMap\\HowlingFjord\\NewAgamand',284,308,415,360},
    {'Interface\\WorldMap\\HowlingFjord\\Skorn',238,232,343,108},
    {'Interface\\WorldMap\\HowlingFjord\\TheTwistedGlade',266,210,420,57},
    {'Interface\\WorldMap\\HowlingFjord\\UtgardeKeep',248,382,477,216},
    {'Interface\\WorldMap\\HowlingFjord\\WestguardKeep',347,220,90,180},
    {'Interface\\WorldMap\\HowlingFjord\\AncientLift',177,191,342,351},
    {'Interface\\WorldMap\\HowlingFjord\\BaelgunsExcavationSite',244,305,621,327},
    {'Interface\\WorldMap\\HowlingFjord\\Baleheim',174,173,576,170},
    {'Interface\\WorldMap\\HowlingFjord\\Kamagua',333,265,99,278},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['IcecrownGlacier']={
    {'Interface\\WorldMap\\IcecrownGlacier\\Corprethar',308,212,342,392},
    {'Interface\\WorldMap\\IcecrownGlacier\\Jotunheim',393,474,22,122},
    {'Interface\\WorldMap\\IcecrownGlacier\\IcecrownCitadel',308,202,392,466},
    {'Interface\\WorldMap\\IcecrownGlacier\\OnslaughtHarbor',204,268,0,167},
    {'Interface\\WorldMap\\IcecrownGlacier\\Scourgeholme',245,239,690,267},
    {'Interface\\WorldMap\\IcecrownGlacier\\SindragosasFall',300,343,626,31},
    {'Interface\\WorldMap\\IcecrownGlacier\\TheBrokenFront',283,231,558,329},
    {'Interface\\WorldMap\\IcecrownGlacier\\TheFleshwerks',219,283,218,291},
    {'Interface\\WorldMap\\IcecrownGlacier\\TheShadowVault',223,399,321,15},
    {'Interface\\WorldMap\\IcecrownGlacier\\Aldurthar',373,375,355,37},
    {'Interface\\WorldMap\\IcecrownGlacier\\Valhalas',238,240,217,50},
    {'Interface\\WorldMap\\IcecrownGlacier\\ValleyofEchoes',269,217,715,390},
    {'Interface\\WorldMap\\IcecrownGlacier\\Ymirheim',223,207,444,276},
    {'Interface\\WorldMap\\IcecrownGlacier\\TheConflagration',227,210,327,305},
    {'Interface\\WorldMap\\IcecrownGlacier\\TheBombardment',248,243,538,181},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\IcecrownGlacier\\ArgentTournamentGround',314,224,616,30},
  },
  ['SholazarBasin']={
    {'Interface\\WorldMap\\SholazarBasin\\RiversHeart',468,329,359,339},
    {'Interface\\WorldMap\\SholazarBasin\\TheSavageThicket',293,229,396,51},
    {'Interface\\WorldMap\\SholazarBasin\\TheMosslightPillar',239,313,265,355},
    {'Interface\\WorldMap\\SholazarBasin\\TheMakersOverlook',233,286,705,236},
    {'Interface\\WorldMap\\SholazarBasin\\TheMakersPerch',249,248,172,135},
    {'Interface\\WorldMap\\SholazarBasin\\TheSuntouchedPillar',455,316,82,186},
    {'Interface\\WorldMap\\SholazarBasin\\RainspeakerCanopy',207,235,427,244},
    {'Interface\\WorldMap\\SholazarBasin\\TheLifebloodPillar',312,369,501,134},
    {'Interface\\WorldMap\\SholazarBasin\\TheAvalanche',322,265,596,92},
    {'Interface\\WorldMap\\SholazarBasin\\TheGlimmeringPillar',294,327,308,34},
    {'Interface\\WorldMap\\SholazarBasin\\KartaksHold',329,293,76,375},
    {'Interface\\WorldMap\\SholazarBasin\\TheStormwrightsShelf',268,288,138,58},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['TheStormPeaks']={
    {'Interface\\WorldMap\\TheStormPeaks\\DunNiffelem',309,383,481,285},
    {'Interface\\WorldMap\\TheStormPeaks\\TempleofLife',182,270,570,113},
    {'Interface\\WorldMap\\TheStormPeaks\\BrunnhildarVillage',305,298,339,370},
    {'Interface\\WorldMap\\TheStormPeaks\\BorsBreath',322,195,109,375},
    {'Interface\\WorldMap\\TheStormPeaks\\Valkyrion',228,158,98,318},
    {'Interface\\WorldMap\\TheStormPeaks\\Ulduar',369,265,218,0},
    {'Interface\\WorldMap\\TheStormPeaks\\Thunderfall',306,484,627,179},
    {'Interface\\WorldMap\\TheStormPeaks\\TerraceoftheMakers',363,341,292,122},
    {'Interface\\WorldMap\\TheStormPeaks\\TempleofStorms',169,164,239,301},
    {'Interface\\WorldMap\\TheStormPeaks\\SparksocketMinefield',251,200,242,468},
    {'Interface\\WorldMap\\TheStormPeaks\\SnowdriftPlains',205,232,162,143},
    {'Interface\\WorldMap\\TheStormPeaks\\Nidavelir',221,200,108,206},
    {'Interface\\WorldMap\\TheStormPeaks\\NarvirsCradle',180,239,214,144},
    {'Interface\\WorldMap\\TheStormPeaks\\GarmsBane',184,191,395,470},
    {'Interface\\WorldMap\\TheStormPeaks\\Frosthold',244,220,134,429},
    {'Interface\\WorldMap\\TheStormPeaks\\EngineoftheMakers',210,179,316,296},
  },
  ['LakeWintergrasp']={
    {'',0,0,0,0},
  },
  ['ZulDrak']={
    {'Interface\\WorldMap\\ZulDrak\\GunDrak',336,297,629,0},
    {'Interface\\WorldMap\\ZulDrak\\DrakSotraFields',286,265,326,358},
    {'Interface\\WorldMap\\ZulDrak\\AmphitheaterOfAnguish',266,254,289,287},
    {'Interface\\WorldMap\\ZulDrak\\AltarOfSseratus',237,248,288,168},
    {'Interface\\WorldMap\\ZulDrak\\AltarOfRhunok',247,304,431,127},
    {'Interface\\WorldMap\\ZulDrak\\AltarOfQuetzLun',261,288,607,251},
    {'Interface\\WorldMap\\ZulDrak\\AltarOfMamToth',311,317,575,88},
    {'Interface\\WorldMap\\ZulDrak\\AltarOfHarKoa',265,257,533,345},
    {'Interface\\WorldMap\\ZulDrak\\ZimTorga',249,258,479,241},
    {'Interface\\WorldMap\\ZulDrak\\Zeramas',307,256,7,412},
    {'Interface\\WorldMap\\ZulDrak\\Voltarus',218,291,174,191},
    {'Interface\\WorldMap\\ZulDrak\\ThrymsEnd',272,268,0,247},
    {'Interface\\WorldMap\\ZulDrak\\LightsBreach',321,305,181,363},
    {'Interface\\WorldMap\\ZulDrak\\Kolramas',302,231,380,437},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
 -- The Maelstrom
  ['Deepholm']={
    {'Interface\\WorldMap\\Deepholm\\CrimsonExpanse',462,400,540,12},
    {'Interface\\WorldMap\\Deepholm\\DeathwingsFall',454,343,549,297},
    {'Interface\\WorldMap\\Deepholm\\NeedlerockChasm',378,359,20,0},
    {'Interface\\WorldMap\\Deepholm\\NeedlerockSlag',370,285,0,146},
    {'Interface\\WorldMap\\Deepholm\\ScouredReach',516,287,448,0},
    {'Interface\\WorldMap\\Deepholm\\StoneHearth',371,354,0,314},
    {'Interface\\WorldMap\\Deepholm\\StormsFuryWreckage',292,285,458,383},
    {'Interface\\WorldMap\\Deepholm\\TempleOfEarth',355,345,287,177},
    {'Interface\\WorldMap\\Deepholm\\ThePaleRoost',467,273,85,0},
    {'Interface\\WorldMap\\Deepholm\\TherazanesThrone',274,156,434,0},
    {'Interface\\WorldMap\\Deepholm\\TheShatteredField',430,230,141,438},
    {'Interface\\WorldMap\\Deepholm\\TwilightOverlook',411,248,570,420},
    {'Interface\\WorldMap\\Deepholm\\TwilightTerrace',237,198,297,384},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
  },
  ['Kezan']={
    {'Interface\\WorldMap\\Kezan\\Kajamine',354,360,586,308},
    {'Interface\\WorldMap\\Kezan\\KezanMap',1002,664,0,4},
    {'Interface\\WorldMap\\Kezan\\BilgewaterPort',694,290,163,148},
    {'Interface\\WorldMap\\Kezan\\Drudgetown',351,301,180,367},
    {'Interface\\WorldMap\\Kezan\\FirstbankofKezan',376,343,98,325},
    {'Interface\\WorldMap\\Kezan\\GallywixsVilla',303,452,0,41},
    {'Interface\\WorldMap\\Kezan\\KajaroField',250,307,383,260},
    {'Interface\\WorldMap\\Kezan\\SwindleStreet',168,213,317,232},
    {'Interface\\WorldMap\\Kezan\\TheSlick',592,202,219,108},
  },
  ['TheLostIsles']={
    {'Interface\\WorldMap\\TheLostIsles\\Alliancebeachhead',177,172,129,348},
    {'Interface\\WorldMap\\TheLostIsles\\BilgewaterLumberyard',248,209,462,43},
    {'Interface\\WorldMap\\TheLostIsles\\GallywixDocks',173,180,351,21},
    {'Interface\\WorldMap\\TheLostIsles\\HordeBaseCamp',222,190,244,458},
    {'Interface\\WorldMap\\TheLostIsles\\KTCOilPlatform',156,142,433,11},
    {'Interface\\WorldMap\\TheLostIsles\\landingSite',142,133,377,359},
    {'Interface\\WorldMap\\TheLostIsles\\OoomlotVillage',221,211,508,345},
    {'Interface\\WorldMap\\TheLostIsles\\Oostan',210,258,492,161},
    {'Interface\\WorldMap\\TheLostIsles\\RaptorRise',168,205,416,368},
    {'Interface\\WorldMap\\TheLostIsles\\RuinsOfVashelan',212,216,440,452},
    {'Interface\\WorldMap\\TheLostIsles\\ScorchedGully',305,288,323,185},
    {'Interface\\WorldMap\\TheLostIsles\\ShipwreckShore',172,175,189,408},
    {'Interface\\WorldMap\\TheLostIsles\\SkyFalls',190,186,416,131},
    {'Interface\\WorldMap\\TheLostIsles\\TheSavageGlen',231,216,213,325},
    {'Interface\\WorldMap\\TheLostIsles\\TheSlavePits',212,193,279,68},
    {'Interface\\WorldMap\\TheLostIsles\\WarchiefsLookout',159,230,264,144},
    {'Interface\\WorldMap\\TheLostIsles\\Lostpeak',350,517,581,21},
  },
  ['TheLostIsles_terrain1']={
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\Alliancebeachhead',177,172,129,348},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\BilgewaterLumberyard',248,209,462,43},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\GallywixDocks',173,180,351,21},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\HordeBaseCamp',222,190,244,458},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\KTCOilPlatform',156,142,433,11},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\landingSite',142,133,377,359},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\OoomlotVillage',221,211,508,345},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\Oostan',210,258,492,161},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\RaptorRise',168,205,416,368},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\RuinsOfVashelan',212,216,440,452},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\ScorchedGully',305,288,323,185},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\ShipwreckShore',172,175,189,408},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\SkyFalls',190,186,416,131},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\TheSavageGlen',231,216,213,325},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\TheSlavePits',212,193,279,68},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\WarchiefsLookout',159,230,264,144},
    {'Interface\\WorldMap\\TheLostIsles_terrain1\\Lostpeak',350,517,581,21},
  },
  ['TheLostIsles_terrain2']={
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\Alliancebeachhead',177,172,129,348},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\BilgewaterLumberyard',248,209,462,43},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\GallywixDocks',173,180,351,21},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\HordeBaseCamp',222,190,244,458},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\KTCOilPlatform',156,142,433,11},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\landingSite',142,133,377,359},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\OoomlotVillage',221,211,508,345},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\Oostan',210,258,492,161},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\RaptorRise',168,205,416,368},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\RuinsOfVashelan',212,216,440,452},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\ScorchedGully',305,288,323,185},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\ShipwreckShore',172,175,189,408},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\SkyFalls',190,186,416,131},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\TheSavageGlen',231,216,213,325},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\TheSlavePits',212,193,279,68},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\WarchiefsLookout',159,230,264,144},
    {'Interface\\WorldMap\\TheLostIsles_terrain2\\Lostpeak',350,517,581,21},
  },
 -- Pandaria
  ['TheWanderingIsle']={
    {'Interface\\WorldMap\\TheWanderingIsle\\TheWoodofStaves',989,466,13,202},
    {'Interface\\WorldMap\\TheWanderingIsle\\Fe-FangVillage',234,286,134,9},
    {'Interface\\WorldMap\\TheWanderingIsle\\MorningBreezeVillage',261,315,203,36},
    {'Interface\\WorldMap\\TheWanderingIsle\\RidgeofLaughingWinds',313,321,183,198},
    {'Interface\\WorldMap\\TheWanderingIsle\\Pei-WuForest',651,262,351,406},
    {'Interface\\WorldMap\\TheWanderingIsle\\SkyfireCrash-Site',346,263,124,405},
    {'Interface\\WorldMap\\TheWanderingIsle\\MandoriVillage',610,374,392,294},
    {'Interface\\WorldMap\\TheWanderingIsle\\TempleofFiveDawns',607,461,395,182},
    {'Interface\\WorldMap\\TheWanderingIsle\\TheDawningValley',677,668,325,0},
    {'Interface\\WorldMap\\TheWanderingIsle\\TheSingingPools',372,475,545,12},
    {'Interface\\WorldMap\\TheWanderingIsle\\TheRows',385,373,504,295},
  },
  ['DreadWastes']={
    {'Interface\\WorldMap\\DreadWastes\\BREWGARDEN',250,218,351,0},
    {'Interface\\WorldMap\\DreadWastes\\BRINYMUCK',325,270,214,311},
    {'Interface\\WorldMap\\DreadWastes\\CLUTCHESOFSHEKZEER',209,318,341,125},
    {'Interface\\WorldMap\\DreadWastes\\DREADWATERLAKE',322,211,437,313},
    {'Interface\\WorldMap\\DreadWastes\\HEARTOFFEAR',262,293,191,122},
    {'Interface\\WorldMap\\DreadWastes\\HORRIDMARCH',323,194,441,224},
    {'Interface\\WorldMap\\DreadWastes\\KLAXXIVESS',236,206,458,110},
    {'Interface\\WorldMap\\DreadWastes\\KYPARIVOR',325,190,485,0},
    {'Interface\\WorldMap\\DreadWastes\\RIKKITUNVILLAGE',218,186,236,32},
    {'Interface\\WorldMap\\DreadWastes\\SOGGYSGAMBLE',268,241,450,406},
    {'Interface\\WorldMap\\DreadWastes\\TERRACEOFGURTHAN',209,234,593,92},
    {'Interface\\WorldMap\\DreadWastes\\ZANVESS',290,283,162,385},
  },
  ['Krasarang']={
    {'Interface\\WorldMap\\Krasarang\\AnglersOutpost',265,194,545,205},
    {'Interface\\WorldMap\\Krasarang\\CradleOfChiJi',272,250,176,376},
    {'Interface\\WorldMap\\Krasarang\\DojaniRiver',190,282,513,3},
    {'Interface\\WorldMap\\Krasarang\\FallsongRiver',214,393,218,77},
    {'Interface\\WorldMap\\Krasarang\\ZhusBastion',306,204,612,0},
    {'Interface\\WorldMap\\Krasarang\\LostDynasty',217,279,589,27},
    {'Interface\\WorldMap\\Krasarang\\NayeliLagoon',246,240,343,373},
    {'Interface\\WorldMap\\Krasarang\\RedwingRefuge',212,265,317,63},
    {'Interface\\WorldMap\\Krasarang\\RuinsOfDojan',204,383,444,44},
    {'Interface\\WorldMap\\Krasarang\\RuinsOfKorja',211,395,125,88},
    {'Interface\\WorldMap\\Krasarang\\TempleOfTheRedCrane',219,259,300,215},
    {'Interface\\WorldMap\\Krasarang\\TheDeepwild',188,412,397,59},
    {'Interface\\WorldMap\\Krasarang\\TheForbiddenJungle',257,300,0,79},
    {'Interface\\WorldMap\\Krasarang\\TheSouthernIsles',252,313,23,267},
    {'Interface\\WorldMap\\Krasarang\\UngaIngoo',258,170,330,498},
    {'Interface\\WorldMap\\Krasarang\\TheSouthernIsles',252,313,23,267},
    {'Interface\\WorldMap\\Krasarang\\krasarangCove',286,268,701,19},
  },
  ['Krasarang_terrain1']={
    {'Interface\\WorldMap\\Krasarang_terrain1\\ANGLERSOUTPOST',347,199,545,200},
    {'Interface\\WorldMap\\Krasarang_terrain1\\CRADLEOFCHIJI',272,250,176,376},
    {'Interface\\WorldMap\\Krasarang_terrain1\\REDWINGREFUGE',212,265,317,63},
    {'Interface\\WorldMap\\Krasarang_terrain1\\DOJANIRIVER',190,282,513,3},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\FALLSONGRIVER',214,393,218,77},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\NAYELILAGOON',246,240,343,373},
    {'Interface\\WorldMap\\Krasarang_terrain1\\RUINSOFDOJAN',204,383,444,44},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\RUINSOFKORJA',211,395,125,88},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\TEMPLEOFTHEREDCRANE',219,259,300,215},
    {'Interface\\WorldMap\\Krasarang_terrain1\\THEDEEPWILD',188,412,397,59},
    {'Interface\\WorldMap\\Krasarang_terrain1\\THEFORBIDDENJUNGLE',257,300,0,79},
    {'Interface\\WorldMap\\Krasarang_terrain1\\LOSTDYNASTY',217,279,589,27},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\THESOUTHERNISLES',275,329,0,267},
    {'Interface\\WorldMap\\Krasarang_terrain1\\KRASARANGCOVE',295,293,701,19},
    {'Interface\\WorldMap\\Krasarang_terrain1\\UNGAINGOO',258,170,330,498},
    {'',0,0,0,0},
    {'',0,0,0,0},
    {'Interface\\WorldMap\\Krasarang_terrain1\\ZHUSBASTION',306,204,612,0},
  },
  ['KunLaiSummit']={
    {'Interface\\WorldMap\\KunLaiSummit\\BinanVillage',240,198,607,470},
    {'Interface\\WorldMap\\KunLaiSummit\\FireboughNook',224,172,322,496},
    {'Interface\\WorldMap\\KunLaiSummit\\GateoftheAugust',261,162,449,506},
    {'Interface\\WorldMap\\KunLaiSummit\\Kotapeak',252,257,233,360},
    {'Interface\\WorldMap\\KunLaiSummit\\Mogujia',253,208,462,411},
    {'Interface\\WorldMap\\KunLaiSummit\\MountNeverset',313,208,228,264},
    {'Interface\\WorldMap\\KunLaiSummit\\MuskpawRanch',229,262,603,313},
    {'Interface\\WorldMap\\KunLaiSummit\\PeakOfSerenity',287,277,333,63},
    {'Interface\\WorldMap\\KunLaiSummit\\ShadoPanMonastery',385,385,88,92},
    {'Interface\\WorldMap\\KunLaiSummit\\TEMPLEOFTHEWHITETIGER',250,260,587,170},
    {'Interface\\WorldMap\\KunLaiSummit\\TheBurlapTrail',310,276,398,310},
    {'Interface\\WorldMap\\KunLaiSummit\\ValleyOfEmperors',224,241,453,191},
    {'Interface\\WorldMap\\KunLaiSummit\\ZouchinVillage',298,219,502,64},
    {'Interface\\WorldMap\\KunLaiSummit\\Iseoflostsouls',259,233,602,4},
  },
  ['TheJadeForest']={
    {'Interface\\WorldMap\\TheJadeForest\\ChunTianMonastery',227,198,300,56},
    {'Interface\\WorldMap\\TheJadeForest\\DawnsBlossom',234,210,325,178},
    {'Interface\\WorldMap\\TheJadeForest\\DreamersPavillion',218,148,474,520},
    {'Interface\\WorldMap\\TheJadeForest\\EmperorsOmen',202,204,430,21},
    {'Interface\\WorldMap\\TheJadeForest\\GlassfinVillage',278,310,525,358},
    {'Interface\\WorldMap\\TheJadeForest\\GrookinMound',253,229,182,214},
    {'Interface\\WorldMap\\TheJadeForest\\HellscreamsHope',196,166,181,75},
    {'Interface\\WorldMap\\TheJadeForest\\JadeMines',236,142,400,146},
    {'Interface\\WorldMap\\TheJadeForest\\NectarbreezeOrchard',219,256,290,330},
    {'Interface\\WorldMap\\TheJadeForest\\NookaNooka',219,205,189,151},
    {'Interface\\WorldMap\\TheJadeForest\\RuinsOfGanShi',196,158,316,0},
    {'Interface\\WorldMap\\TheJadeForest\\SerpentsSpine',191,216,388,299},
    {'Interface\\WorldMap\\TheJadeForest\\SlingtailPits',179,180,428,416},
    {'Interface\\WorldMap\\TheJadeForest\\TempleOfTheJadeSerpent',264,211,468,295},
    {'Interface\\WorldMap\\TheJadeForest\\TheArboretum',242,210,481,215},
    {'Interface\\WorldMap\\TheJadeForest\\Waywardlanding',219,186,346,482},
    {'Interface\\WorldMap\\TheJadeForest\\WindlessIsle',251,348,539,43},
    {'Interface\\WorldMap\\TheJadeForest\\WreckOfTheSkyShark',210,158,202,0},
  },
  ['TheHiddenPass']={
    {'Interface\\WorldMap\\TheHiddenPass\\TheBlackMarket',479,493,371,175},
    {'Interface\\WorldMap\\TheHiddenPass\\TheHiddenCliffs',294,220,433,0},
    {'Interface\\WorldMap\\TheHiddenPass\\TheHiddenSteps',290,191,412,477},
  },
  ['TownlongWastes']={
    {'Interface\\WorldMap\\TownlongWastes\\GaoRanBlockade',353,200,546,468},
    {'Interface\\WorldMap\\TownlongWastes\\KriVess',255,269,420,209},
    {'Interface\\WorldMap\\TownlongWastes\\MingChiCrossroads',247,221,417,447},
    {'Interface\\WorldMap\\TownlongWastes\\NiuzaoTemple',296,359,213,241},
    {'Interface\\WorldMap\\TownlongWastes\\OsulMesa',238,296,560,185},
    {'Interface\\WorldMap\\TownlongWastes\\palewindVillage',282,306,692,362},
    {'Interface\\WorldMap\\TownlongWastes\\ShadoPanGarrison',213,170,413,385},
    {'Interface\\WorldMap\\TownlongWastes\\ShanzeDao',300,246,125,0},
    {'Interface\\WorldMap\\TownlongWastes\\Sikvess',261,235,306,433},
    {'Interface\\WorldMap\\TownlongWastes\\SriVess',294,283,92,192},
    {'Interface\\WorldMap\\TownlongWastes\\TheSumprushes',271,205,545,369},
  },
  ['ValeofEternalBlossoms']={
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\GuoLaiRuins',337,349,87,3},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\MistfallVillage',310,305,200,363},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\MoguShanPalace',373,385,629,22},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\SettingSunTraining',350,429,0,234},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\TheGoldenStair',242,254,328,16},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\TheStairsAscent',446,359,556,267},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\TheTwinMonoliths',272,522,444,97},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\TuShenBurialGround',267,308,349,316},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\WhiteMoonShrine',298,262,482,10},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\WhitepetalLake',267,281,278,170},
    {'Interface\\WorldMap\\ValeofEternalBlossoms\\WinterboughGlade',361,333,4,107},
  },
  ['ValleyoftheFourWinds']={
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\CliffsofDispair',510,264,215,404},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\DustbackGorge',209,308,0,343},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\GildedFan',208,292,438,41},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\GrandGranery',314,212,334,325},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\Halfhill',206,245,438,177},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\HarvestHome',260,251,5,239},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\KuzenVillage',199,304,224,74},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\MudmugsPlace',230,217,561,161},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\NesingwarySafari',249,342,104,326},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\PaoquanHollow',273,246,12,105},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\PoolsofPurity',213,246,513,58},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\SilkenFields',254,259,530,253},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\RumblingTerrace',277,245,582,301},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\SingingMarshes',175,291,170,130},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\StormsoutBrewery',257,288,227,380},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\Theheartland',286,392,253,75},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\ThunderfootFields',380,317,622,0},
    {'Interface\\WorldMap\\ValleyoftheFourWinds\\ZhusDecent',303,323,699,114},
  },
  ['AhnQirajTheFallenKingdom']={
    {'Interface\\WorldMap\\AhnQirajTheFallenKingdom\\AQKingdom',887,668,115,0},
  },
}

function Foglight:Startup()
	if not ZGV.db.profile.foglight then return end
	--self:Hook("GetNumMapOverlays")

	-- experimental: off
	--hooksecurefunc("WorldMapFrame_Update",function() ZGV.Foglight:WorldMapFrame_Update() end)

	--self:Hook("WorldMapFrame_Update",true)
	if WorldMapFrame:IsShown() then WorldMapFrame_Update() end
end

function Foglight:TurnOff()
	--self:Unhook("GetNumMapOverlays")
	--self:Unhook("WorldMapFrame_Update")

	--[[
	for n=1,NUM_WORLDMAP_OVERLAYS do
		local tex = _G["WorldMapOverlay"..n]
		--tex:SetVertexColor(1.0,1.0,1.0)
		--tex:SetAlpha(1.0)
		tex:Hide()
	end
	--]]
	if WorldMapFrame:IsShown() then WorldMapFrame_Update() end
end

PreZygor_GetNumMapOverlays = GetNumMapOverlays
function GetNumMapOverlays()
	if not ZGV.db or not ZGV.db.profile.foglight then return PreZygor_GetNumMapOverlays() end
	local mapfile,x,y,isMicro,microName = GetMapInfo()
	if not mapfile then return 0 end
	if microName then mapfile=microName end
	if not ZGV.Foglight.data[mapfile] then return 0 end
	return #TableKeys(ZGV.Foglight.data[mapfile])
	--[[
	if NUM_WORLDMAP_OVERLAYS == 0 or not ZGV.db.profile.foglight then
		return PreZygor_GetNumMapOverlays()
	end
	return 0
	--]]
end


PreZygor_GetMapOverlayInfo = GetMapOverlayInfo
function GetMapOverlayInfo(i)
	if not ZGV.db or not ZGV.db.profile.foglight then return PreZygor_GetMapOverlayInfo(i) end
	--[[

		-- that's trying not to break stuff. How about some breakage instead - below.

		local t,w,h,x,y,a,b = PreZygor_GetMapOverlayInfo(i)
		if w and w>0 then
			if ZGV.db.profile.debug then
				local mapfile = GetMapInfo()
				local data = ZGV.Foglight.data[mapfile][t:gsub(".*\\","")]
				if not data or w~=data[1] or h~=data[2] or x~=data[3] or y~=data[4] then
					ZGV:Print(("Foglight: wrong/new map overlay data for zone %d '%s' in %s: should be %d,%d,%d,%d"):format(i,t,mapfile,w,h,x,y))
				end
			end
			return t,w,h,x,y,z,b
		end

		local mapfile = GetMapInfo()
		if not mapfile then return nil,0,0,0,0,0,0 end
		if not ZGV.Foglight.data[mapfile] then return nil,0,0,0,0,0,0 end


		local data
		for n=1,PreZygor_GetNumMapOverlays() do
			t,w,h,x,y = PreZygor_GetMapOverlayInfo(n)
			if not w or w<1 then
				lastn=n-1
				break
			end
			t = t:gsub(".*\\","")

			data = ZGV.Foglight.data[mapfile][t]
			if data then
				data.known = 1
			else
				ZGV:Print(("Foglight: unknown zone %d '%s' in %s while fetching zone %d"):format(n,t,mapfile,i))
				return nil,0,0,0,0,0,0
			end
		end

		i=i-PreZygor_GetNumMapOverlays()
		local n

		for tname,data in pairs(ZGV.Foglight.data[mapfile]) do
			if not data.known then
				n=n+1
				if n==i then return "Interface\\WorldMap\\"..mapfile.."\\"..tname,data[1],data[2],data[3],data[4],0,0,"zygfog" end
			end
		end
	--]]

	--print("GetMapOverlayInfo("..i..") faked")
	local mapfile = GetMapInfo()
	if not mapfile then return nil,0,0,0,0,nil,nil end
	local mapdata = ZGV.Foglight.data[mapfile]   if not mapdata then return nil,0,0,0,0,nil,nil end
	local data = mapdata[i]   if not data then return nil,0,0,0,0,nil,nil end
	return unpack(data)
	--[[
	for tname,data in pairs(ZGV.Foglight.data[mapfile]) do
		i=i-1
		local tex = tname
		if i==0 then return tex,data[1],data[2],data[3],data[4],0,0,"zygorfoglight" end
	end
	--]]
end

function Foglight:DebugMap()
	print(("Debugging foglighting of map: %s"):format(GetMapInfo()))
	print(("WoW reports %d zones known, Foglight says %d%s."):format(PreZygor_GetNumMapOverlays(),GetNumMapOverlays(),ZGV.db.profile.foglight and "" or " too as it's turned off."))
	for i=1,GetNumMapOverlays() do
		local t,w,h,x,y,a,b,z = GetMapOverlayInfo(i)
		t = t:gsub(".*\\","")
		print(("%s = %d,%d,%d,%d (%s)"):format(t,w,h,x,y,z and "Foglight" or "WoW"))
	end
end

--- Cheat. Mess with the original WorldMapFrame_Update by faking GetMapOverlayInfo() responses!

-- RUN THIS.
function Foglight:DumpMapOverlayInfos(diff,all)
	if GetCurrentMapContinent()==-1 then return self:_DumpSpecialMapOverlayInfos(diff) end
	local dump
	if diff then dump = "-- DIFFERENCES from Foglight.data :\n" elseif all then dump = "Foglight.data = {\n" else dump = "" end
	local conts = {GetMapContinents()}
	local current = GetMapInfo()
	for ci,c in pairs(conts) do
		ChatFrame1:AddMessage(ci.." = "..c)
		local zones = {GetMapZones(ci)}
		dump = dump .. "  -- "..c.."\n"
		for zi,z in pairs(zones) do
			SetMapZoom(ci,zi)
			local zonetag = GetMapInfo()
			if all or zonetag==current then
				local zone,count = self:_Dump_GrabOneZone(zonetag,diff)
				if count>0 then
					dump = dump
						.. "  ['" .. zonetag .. "']={\n"
						.. zone
						.. "  },\n"
				end
			end
		end
	end
	if not diff or all then dump = dump .. "}\n" end
	ZGV:ShowDump(dump,"Paste this into Foglight.lua:")
end

function Foglight:_DumpSpecialMapOverlayInfos(diff)
	local dump
	if diff then dump = "-- DIFFERENCES from Foglight.data :\n" else dump = "Foglight.data = {\n" end
	local zonetag = GetMapInfo()
	local zone,count = self:_Dump_GrabOneZone(zone,diff)
	if count>0 then
		dump = dump
			.. "  ['" .. zonetag .. "']={\n"
			.. zone
			.. "  },\n"
	end
	if not diff then dump = dump .. "}\n" end
	ZGV:ShowDump(dump,"Paste this into Foglight.lua:")
end

function Foglight:_Dump_GrabOneZone(zonetag,diff)
	local overlaynum = PreZygor_GetNumMapOverlays()
	local dump=""
	local count=0
	for oi=1,overlaynum do
		local t,w,h,x,y,a,b = PreZygor_GetMapOverlayInfo(oi)
		if t then
			local fd = Foglight.data[zonetag] and Foglight.data[zonetag][t]
			if not diff or (not fd or fd[1]~=w or fd[2]~=h or fd[3]~=x or fd[4]~=y) then
				--dump = dump .. ("    ['%s']={%d,%d,%d,%d},\n"):format(t:gsub([[\]],[[\\]]),w,h,x,y)
				dump = dump .. ("    {'%s',%d,%d,%d,%d},\n"):format(t:gsub([[\]],[[\\]]),w,h,x,y)
				count=count+1
			end
		end
	end
	return dump,count
end


--[[
function Foglight:WorldMapFrame_Update()
	--self.hooks.WorldMapFrame_Update()
	self:WorldMapFrame_UpdateOverlays()
end

local discovered = {}
local math_mod = math.fmod
local math_floor = math.floor
local math_ceil = math.ceil

function Foglight:WorldMapFrame_UpdateOverlays()
	if not ZGV.db.profile.foglight then return end
	if not WorldMapFrame:IsShown() then return end

	local mapfile = GetMapInfo()
	if not mapfile then return end

	local prefix = "Interface\\WorldMap\\"..mapfile.."\\"
	local zoneTable = self.data[mapfile]
	if not zoneTable then return end

	local numOverlays = PreZygor_GetNumMapOverlays()
	local len = string.len(prefix)+1
	for i=1, numOverlays do
		local tname,tw,th,ofx,ofy = GetMapOverlayInfo(i)
		tname = string.sub(tname, len)
		local num = tw + th * 1024 + ofx * 1048576 + ofy * 1073741824
		if num ~= 0 and num ~= 131200 and tname ~= "" then --and tname:lower() ~= "pixelfix"
			local tab = {tw,th,ofx,ofy}
			discovered[tname] = tab
			--zoneTable[tname] = tab
		end
	end

	local textureCount = 0

	local overlayPrefix = "WorldMapOverlay"
	--local overlayPrefix = "WorldMapDetailTile"

	for tname, num in pairs(zoneTable) do
		local textureName = prefix .. tname
		local textureWidth, textureHeight, offsetX, offsetY
		if type(num)=="number" then
			textureWidth, textureHeight, offsetX, offsetY = math_mod(num, 1024), math_mod(math_floor(num / 1024), 1024), math_mod(math_floor(num / 1048576), 1024), math_floor(num / 1073741824)
			zoneTable[tname]={textureWidth,textureHeight,offsetX,offsetY}
		else
			textureWidth, textureHeight, offsetX, offsetY = unpack(num)
		end

		-- HACK: override *known incorrect* data with hard-coded fixes.
		if textureName == "Interface\\WorldMap\\Tirisfal\\BRIGHTWATERLAKE" then
			if offsetX == 587 then
				offsetX = 584
			end
		end
		if textureName == "Interface\\WorldMap\\Silverpine\\BERENSPERIL" then
			if offsetY == 417 then
				offsetY = 415
			end
		end

		local numTexturesWide = math_ceil(textureWidth / 256)
		local numTexturesTall = math_ceil(textureHeight / 256)
		local neededTextures = textureCount + numTexturesWide*numTexturesTall
		if neededTextures > NUM_WORLDMAP_OVERLAYS then
			for j = NUM_WORLDMAP_OVERLAYS+1, neededTextures do
				WorldMapDetailFrame:CreateTexture(overlayPrefix..j, "ARTWORK")
			end
			NUM_WORLDMAP_OVERLAYS = neededTextures
		end
		for j = 1, numTexturesTall do
			local texturePixelHeight
			local textureFileHeight
			if j < numTexturesTall then
				texturePixelHeight = 256
				textureFileHeight = 256
			else
				texturePixelHeight = math_mod(textureHeight, 256)
				if texturePixelHeight == 0 then
					texturePixelHeight = 256
				end
				textureFileHeight = 16
				while textureFileHeight < texturePixelHeight do
					textureFileHeight = textureFileHeight * 2
				end
			end
			for k = 1, numTexturesWide do
				ZGV:Debug(k.."x"..j..", tex "..textureCount)
				if textureCount > NUM_WORLDMAP_OVERLAYS then
					return
				end
				textureCount = textureCount + 1
				local texture = _G[overlayPrefix..textureCount]
				local texturePixelWidth
				local textureFileWidth
				if k < numTexturesWide then
					texturePixelWidth = 256
					textureFileWidth = 256
				else
					texturePixelWidth = math_mod(textureWidth, 256)
					if texturePixelWidth == 0 then
						texturePixelWidth = 256
					end
					textureFileWidth = 16
					while textureFileWidth < texturePixelWidth do
						textureFileWidth = textureFileWidth * 2
					end
				end
				texture:SetWidth(texturePixelWidth)
				texture:SetHeight(texturePixelHeight)
				texture:SetTexCoord(0, texturePixelWidth/textureFileWidth, 0, texturePixelHeight/textureFileHeight)
				texture:ClearAllPoints()
				texture:SetPoint("TOPLEFT", "WorldMapDetailFrame", "TOPLEFT", offsetX + (256 * (k-1)), -(offsetY + (256 * (j - 1))))
				texture:SetTexture(textureName..(((j - 1) * numTexturesWide) + k))

				--if discovered[tname] then
					texture:SetVertexColor(1.0,1.0,1.0)
					texture:SetAlpha(1.0)
				--else
				--	texture:SetVertexColor(self.db.profile.darkR, self.db.profile.darkG, self.db.profile.darkB)
				--	texture:SetAlpha(self.db.profile.darkA)
				--end

				texture:Show()
				ZGV:Debug(texture:GetName().." "..tostring(texture:IsShown()))
			end
		end
	end

	for i = textureCount+1, NUM_WORLDMAP_OVERLAYS do
		ZGV:Debug("Hiding "..i)
		_G["WorldMapOverlay"..i]:Hide()
	end

	for k in pairs(discovered) do
		discovered[k] = nil
	end
end

function Foglight:OnProfileEnable()
	WorldMapFrame_Update()
end
--]]