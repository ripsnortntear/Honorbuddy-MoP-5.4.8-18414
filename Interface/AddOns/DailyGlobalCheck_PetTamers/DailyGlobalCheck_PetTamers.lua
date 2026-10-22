-- Daily Global Check - Pet Tamers plugin
-- Fluffies EU-Well of Eternity
local addonName, addonTable = ...
DGC_PTLocalizedStrings = {}
local pluginicon = "Interface\\ICONS\\INV_MISC_PETMOONKINTA"
local plugintitle = "Pet Tamers"
-- pet families icons
local pfme  = "|TInterface\\icons\\Icon_PetFamily_Mechanical:12|t"
local pfbe  = "|TInterface\\icons\\Icon_PetFamily_Beast:12|t"
local pfhu  = "|TInterface\\icons\\Icon_PetFamily_Humanoid:12|t"
local pfdr  = "|TInterface\\icons\\Icon_PetFamily_Dragon:12|t"
local pffl  = "|TInterface\\icons\\Icon_PetFamily_Flying:12|t"
local pfcr  = "|TInterface\\icons\\Icon_PetFamily_Critter:12|t"
local pfma  = "|TInterface\\icons\\Icon_PetFamily_Magical:12|t"
local pfun  = "|TInterface\\icons\\Icon_PetFamily_Undead:12|t"
local pfwa  = "|TInterface\\icons\\Icon_PetFamily_Water:12|t"
local pfel  = "|TInterface\\icons\\Icon_PetFamily_Elemental:12|t"
--
local icon_firespirit = "Interface\\icons\\INV_Pet_PandarenElemental_Fire"
local icon_waterspirit = "Interface\\icons\\Inv_pet_PandarenElemental"
local icon_earthspirit = "Interface\\icons\\INV_Pet_PandarenElemental_Earth"
local icon_airspirit = "Interface\\icons\\INV_Pet_PandarenElemental_Air"
--
local tx_bagicon      = "Interface\\icons\\INV_Misc_Bag_CenarionHerbBag"
local ep_icon         = "Interface\\icons\\Ability_Mount_WhiteDireWolf"
--
local epspecial = "#elitepets#"
local ep_quest_missing = "|cffFF0000"..string.format(ITEM_MISSING,BATTLE_PET_SOURCE_2).."|r"

---------- elite pets methods -----------
local ep_books = { { index = -1, qname = "", completed = false, pets = {1,2,3,4}, questID = 32604},
				   { index = -1, qname = "", completed = false, pets = {5,6,7},   questID = 32868},
                   { index = -1, qname = "", completed = false, pets = {8,9,10},  questID = 32869}
                 }
				 
local function EP_resetbooks()
 table.foreach(ep_books, function(k,v)
  v.index = -1
  v.completed = false
 end)
end

local function EP_GetBook_pet(pet)
 if type(pet) ~= "number" then
  pet = tonumber(pet)
 end

 if pet < 1 or pet > 10 then return nil end
 
 if pet <= 4 then
  return ep_books[1] 
 elseif pet <= 7 then
  return ep_books[2]
 else
  return ep_books[3]
 end
end

local function EP_iscriteriacompleted(pet)
 local result = false
 local book = EP_GetBook_pet(pet)
 if book and not book.completed and book.index > -1 then
  local _,_,done = GetQuestLogLeaderBoard(pet - (book.pets[1] - 1), book.index)
  result = done
 end
 return result
end

local function getelitepetname(pet)
 local desc
 local book = EP_GetBook_pet(pet)
 if book and book.index > -1 then
  desc,_,_ = GetQuestLogLeaderBoard(pet - (book.pets[1] - 1),book.index)
  return desc
 end
end

local function isquestcompleted(questID)
 local result = false
 if tostring(questID):find(epspecial) then
  result = EP_iscriteriacompleted(string.gsub(tostring(questID), epspecial, ""))
 elseif questID == "aonly" or questID == "honly" then
  result = false
 elseif questID == "book1c" or questID == "book2c" or questID == "book3c" then
  result = true
 elseif questID == "pvph" or questID == "pvpa" then
  result = IsQuestFlaggedCompleted(32863)
 else
  result = IsQuestFlaggedCompleted(questID)
 end
 return result
end
---------------------

-- table orders
local CENTER, LEFT, RIGHT = 1,2,3

-- template = {[ZONE],[NAME],[PREF],[SUFF],{{x,y},{x,y}},defaultmapID,[QUESTTYPE],[MAPICON],showfunc
local questsdata

local plugin_data = {
 ["Title"] = plugintitle,
 ["Icon"]  = pluginicon,
 ["Data"]  = questsdata,
 ["Order"] = {[CENTER] = {{"Pandaren Spirits",32434,32439,32441,32440},
                          {"Pandaria Satchels",31957,31991,31956,31955,31954,31953,31958},
                          {GetMapNameByID(951),33137,33222},
                          {"Other Satchels",31916,31909,31926,31935,31971},
						  {CALENDAR_TYPE_PVP,"pvpa","pvph"},
						  {"",32175}},
			  [LEFT]   = {{"Kalimdor",31818,31819,31854,31862,31872,31904,31871,31905,31906,31907,31908,"honly"},
			              {"Eastern Kingdoms",31693,31780,31781,31850,31852,31851,31910,31911,31912,31913,31914,"aonly"}},
			  [RIGHT]  = {{"Outlands",31922,31923,31924,31925},
			              {"Northrend",31931,31934,31932,31933},
						  {"Cataclysm",31973,31972,31974},
						  {"Beasts of Fable Book I",epspecial..1,epspecial..2,epspecial..3,epspecial..4,"book1c"},
						  {"Beasts of Fable Book II",epspecial..5,epspecial..6,epspecial..7,"book2c"},
						  {"Beasts of Fable Book III",epspecial..8,epspecial..9,epspecial..10,"book3c"}}
			  },
 ["Overrides"] = {["isquestcompleted"] = isquestcompleted}
 }

local playerfaction = UnitFactionGroup("PLAYER")
local function showhorde()
 return playerfaction == "Horde"
end

local function showally()
 return playerfaction == "Alliance"
end

local WEfound = {}
local function GenerateQuestsData()
questsdata = {
     -- Pandaren Spirits --
      [32434] = {GetMapNameByID(810),"Burning Pandaren Spirit","|T"..icon_firespirit..":12|t25",pfdr..pfel..pffl,{[810] = {57,42},[862] = {32,36},[-1] = {46,79},[809] = {16,64}},810,nil,icon_firespirit},
      [32439] = {GetMapNameByID(858),"Flowing Pandaren Spirit","|T"..icon_waterspirit..":12|t25",pfwa..pfcr..pfel,{[858] = {61,88},[862] = {38,81},[-1] = {48,92}},858,nil,icon_waterspirit},
      [32441] = {GetMapNameByID(809),"Thundering Pandaren Spirit","|T"..icon_earthspirit..":12|t25",pfel..pfma..pfbe,{[809] = {65,94},[862] = {51,48},[-1] = {52,82},[811] = {67,14},[806] = {9,41}},809,nil,icon_earthspirit},
      [32440] = {GetMapNameByID(806),"Whispering Pandaren Spirit","|T"..icon_airspirit..":12|t25",pffl..pfdr..pfel,{[806] = {29,36},[862] = {60,45},[-1] = {55,81}, [809] = {86,87}},806,nil,icon_airspirit},
     -- Pandaria --
      [31953] = {GetMapNameByID(806),"Grand Master Hyuna",25,pffl..pfbe..pfwa,{[806] = {48,54},[862] = {69,54},[-1] = {58,84}},806,nil,tx_bagicon},
      [31955] = {GetMapNameByID(807),"Grand Master Nishi",25,pfel..pfel..pfbe,{[807] = {46,44},[862] = {51,65},[-1] = {52,87}},807,nil,tx_bagicon},
      [31954] = {GetMapNameByID(857),"Grand Master Mo'ruk",25,pfbe..pffl..pfwa,{[857] = {62,45},[862] = {56,79},[-1] = {54,92}},857,nil,tx_bagicon},
      [31956] = {GetMapNameByID(809),"Grand Master Yon",25,pffl..pfcr..pfbe,{[809] = {36,74},[862] = {40,40},[-1] = {49,79}},809,nil,tx_bagicon},
      [31991] = {GetMapNameByID(810),"Grand Master Zusshi",25,pfel..pfcr..pfwa,{[810] = {36,52},[862] = {24,40},[-1] = {44,80}},810,nil,tx_bagicon},
      [31957] = {GetMapNameByID(858),"Grand Master Shu",25,pfwa..pfel..pfbe,{[858] = {55,38},[862] = {36,64},[-1] = {48,86}},858,nil,tx_bagicon},
      [31958] = {GetMapNameByID(811),"Grand Master Aki",25,pfcr..pfdr..pfwa,{[811] = {31,74},[862] = {45,58},[858] = {83,20},[807] = {25,15},[-1] = {50,85}},811,nil,tx_bagicon},
     -- Outlands --
      [31922] = {GetMapNameByID(465),GetAchievementCriteriaInfo(6604,1),20,pfme..pfme..pfme,{[465] = {64,49},[466] = {62,52}},465},
      [31923] = {GetMapNameByID(467),GetAchievementCriteriaInfo(6604,2),21,pffl..pfhu..pfma,{[467] = {17,50},[466] = {25,48}},467},
      [31924] = {GetMapNameByID(477),GetAchievementCriteriaInfo(6604,3),22,pfwa..pfcr..pfbe,{[477] = {61,49},[466] = {35,65}},477},
      [31925] = {GetMapNameByID(481),GetAchievementCriteriaInfo(6604,4),23,pfwa..pfwa..pfwa,{[481] = {59,70},[478] = {32,30},[466] = {44,68}},481},
      [31926] = {GetMapNameByID(473),"Grand Master Antari",24,pfma..pfel..pfdr,{[473] = {31,42},[466] = {60,79}},473,nil,tx_bagicon},
     -- Northrend --
      [31931] = {GetMapNameByID(491),GetAchievementCriteriaInfo(6605,1),25,pffl..pfwa..pffl,{[491] = {29,34},[485] = {69,75},[-1] = {58,26}},491},
      [31933] = {GetMapNameByID(488),GetAchievementCriteriaInfo(6605,3),25,pfdr..pfun..pfun,{[488] = {59,77},[485] = {50,67},[-1] = {52,23}},488},
      [31934] = {GetMapNameByID(496),GetAchievementCriteriaInfo(6605,4),25,pfbe..pfbe..pfcr,{[496] = {13,67},[485] = {59,44},[-1] = {55,16}},496},
      [31932] = {GetMapNameByID(510),GetAchievementCriteriaInfo(6605,2),25,pfun..pfun..pfun,{[510] = {50,59},[485] = {51,44},[-1] = {53,16}},510},
      [31935] = {GetMapNameByID(492),"Grand Master Payne",25,pfbe..pfme..pfel, {[492] = {77,20},[485] = {49,17},[-1] = {52,8}},492,nil,tx_bagicon},
     -- Cataclysm --
      [31972] = {GetMapNameByID(606),GetAchievementCriteriaInfo(7525,1),25,pfma..pfbe..pfcr,{[606] = {61,33},[182] = {87,48},[281] = {41,85},[13] = {56,31},[-1] = {23,37}},606},
      [31973] = {GetMapNameByID(640),GetAchievementCriteriaInfo(7525,2),25,pfel..pfcr..pfel,{[640] = {50,57},[751] = {50,36},[-1] = {50,50}},640},
      [31971] = {GetMapNameByID(720),"Grand Master Obalis",25,pfbe..pffl..pfcr,{[720] = {57,42},[13] = {49,92},[-1] = {19,75}},720,nil,tx_bagicon},
      [31974] = {GetMapNameByID(700),GetAchievementCriteriaInfo(7525,3),25,pfel..pfma..pfbe,{[700] = {57,57},[14] = {58,57},[-1] = {90,52}},700},
     -- Kalimdor --
      [31818] = {GetMapNameByID(4),GetAchievementCriteriaInfo(6602,1),2,pfbe..pfcr,{[4] = {44,28},[13] = {58,49},[-1] = {25,48},[11] = {78,27}},4,nil,nil,showhorde},
      [31819] = {GetMapNameByID(11),GetAchievementCriteriaInfo(6602,2),3,pfbe..pfbe..pfcr,{[11] = {58,53},[13] = {55,53},[-1] = {22,51},[4] = {23,58},[607] = {61,9}},11,nil,nil,showhorde},
      [31909] = {GetMapNameByID(281),"Grand Master Trixxy",19,pfdr..pfbe..pffl,{[281] = {66,65},[13] = {60,27},[-1] = {26,35}},281,nil,tx_bagicon},
      [31904] = {GetMapNameByID(607),GetAchievementCriteriaInfo(6602,8),11,pfme..pfme..pfme,{[607] = {39,79},[13] = {51,67},[-1] = {20,59},[9] = {69,98},[141] = {11,48}},607,nil,nil,showhorde},
      [31854] = {GetMapNameByID(43),GetAchievementCriteriaInfo(6602,3),5,pfwa..pfcr..pffl,{[43] = {20,29},[13] = {45,38},[-1] = {16,41},[182] = {21,91}},43,nil,nil,showhorde},
      [31906] = {GetMapNameByID(61),GetAchievementCriteriaInfo(6602,11),15,pfbe..pfcr..pfcr,{[61] = {32,33},[13] = {51,72},[-1] = {20,63},[141] = {16,83}},61,nil,nil,showhorde},
      [31862] = {GetMapNameByID(81),GetAchievementCriteriaInfo(6602,4),7,pfbe..pfbe..pfcr,{[81] = {59,71},[13] = {45,50},[-1] = {16,48}},81,nil,nil,showhorde},
      [31872] = {GetMapNameByID(101),GetAchievementCriteriaInfo(6602,5),9,pfwa..pfcr..pfel,{[101] = {57,45},[13] = {42,56},[-1] = {14,53},[9] = {10,21}},101,nil,nil,showhorde},
      [31905] = {GetMapNameByID(141),GetAchievementCriteriaInfo(6602,9),14,pfdr..pfdr..pfdr,{[141] = {53,74},[13] = {57,71},[-1] = {24,62},[61] = {77,24}},141,nil,nil,showhorde},
      [31871] = {GetMapNameByID(121),GetAchievementCriteriaInfo(6602,6),13,pfbe..pfcr..pfdr,{[121] = {60,50},[13] = {43,72},[-1] = {15,62}},121,nil,nil,showhorde},
      [31907] = {GetMapNameByID(182),GetAchievementCriteriaInfo(6602,10),16,pfma..pfma..pfme,{[182] = {40,56},[13] = {48,31},[42] = {56,75},[-1] = {18,37}},182,nil,nil,showhorde},
      [31908] = {GetMapNameByID(241),GetAchievementCriteriaInfo(6602,7),17,pfdr..pffl..pfma,{[241] = {46,60},[13] = {53,21},[42] = {84,15},[-1] = {21,30}},241,nil,nil,showhorde},
      ["honly"] = {"",string.format(RACE_CLASS_ONLY,FACTION_HORDE),"","",nil,nil,nil,nil,showally},
     -- Eastern Kingdoms --
      [31693] = {GetMapNameByID(30),GetAchievementCriteriaInfo(6603,1),2,pfbe..pfbe,{[30] = {42,84},[14] = {44,78},[-1] = {81,66}},30,nil,nil,showally},
      [31914] = {GetMapNameByID(29),GetAchievementCriteriaInfo(6603,10),17,pffl..pfcr..pfel,{[29] = {25,47},[14] = {48,70},[-1] = {83,62}},29,nil,nil,showally},
      [31850] = {GetMapNameByID(34),GetAchievementCriteriaInfo(6603,4),7,pfbe..pfbe..pfbe,{[34] = {20,44},[14] = {44,80},[-1] = {81,68}},34,nil,nil,showally},
      [31781] = {GetMapNameByID(36),GetAchievementCriteriaInfo(6603,3),5,pfcr..pfcr..pfcr,{[36] = {33,52},[14] = {50,76},[-1] = {85,65}},36,nil,nil,showally},
      [31912] = {GetMapNameByID(28),GetAchievementCriteriaInfo(6603,9),15,pfdr..pfdr..pfdr,{[28] = {35,27},[14] = {47,65},[-1] = {83,58}},28,nil,nil,showally},
      [31852] = {GetMapNameByID(37),GetAchievementCriteriaInfo(6603,5),9,pfbe..pfbe..pfma,{[37] = {46,40},[689] = {47,26},[14] = {45,85},[-1] = {81,72}},37,nil,nil,showally},
      [31851] = {GetMapNameByID(673),GetAchievementCriteriaInfo(6603,6),11,pfhu..pffl..pffl,{[673] = {51,73},[689] = {44,79},[14] = {44,94},[-1] = {81,78}},673,nil,nil,showally},
      [31913] = {GetMapNameByID(38),GetAchievementCriteriaInfo(6603,11),16,pffl..pfwa..pfbe,{[38] = {76,41},[14] = {54,79},[-1] = {88,67}},38,nil,nil,showally},
      [31910] = {GetMapNameByID(26),GetAchievementCriteriaInfo(6603,7),13,pfcr..pfbe..pfma,{[26] = {62,54},[14] = {54,41},[-1] = {88,41}},26,nil,nil,showally},
      [31780] = {GetMapNameByID(39),GetAchievementCriteriaInfo(6603,2),3,pfme..pffl..pfcr,{[39] = {61,19},[14] = {42,77},[-1] = {80,66}},39,nil,nil,showally},
      [31916] = {GetMapNameByID(32),"Grand Master Lydia Accoste",19,pfel..pfun..pfun,{[32] = {40,77},[14] = {49,82},[-1] = {84,70}},32,nil,tx_bagicon},
      [31911] = {GetMapNameByID(23),GetAchievementCriteriaInfo(6603,8),14,pfbe..pfbe..pfun,{[23] = {67,54},[14] = {57,32},[-1] = {90,36}},23,nil,nil,showally},
	  ["aonly"] = {"",string.format(RACE_CLASS_ONLY,FACTION_ALLIANCE),"","",nil,nil,nil,nil,showhorde},
     -- Timeless Isle --
      [33137] = {GetMapNameByID(951),"The Celestial Tournament","25+","",{[951] = {35,59}},951,"W"},
	 -- PvP Weekly
	  ["pvpa"] = {GetMapNameByID(811),"What We've Been Training For","","",{[811] = {86,60}},811,"W",nil,showally},
	  ["pvph"] = {GetMapNameByID(811),"What We've Been Training For","","",{[811] = {61,23}},811,"W",nil,showhorde},
     -- Elite Pets --
      [epspecial..1] = {GetMapNameByID(806),ep_quest_missing,"25+",pfcr,{[806] = {48,71},[862] = {68,61},[-1] = {57,85}},806,nil,ep_icon,function() return not isquestcompleted(ep_books[1].questID) end}, -- Kawi I
      [epspecial..2] = {GetMapNameByID(809),ep_quest_missing,"25+",pfbe,{[809] = {35,56},[862] = {39,32},[-1] = {48,77},[810] = {77,33}},809,nil,ep_icon,function() return not isquestcompleted(ep_books[1].questID) end}, -- kafi I
      [epspecial..3] = {GetMapNameByID(809),ep_quest_missing,"25+",pfwa,{[809] = {68,85},[862] = {52,44},[-1] = {52,80},[806] = {12,33}},809,nil,ep_icon,function() return not isquestcompleted(ep_books[1].questID) end}, -- dos ryga I
      [epspecial..4] = {GetMapNameByID(806),ep_quest_missing,"25+",pfcr,{[806] = {57,29},[862] = {72,42},[-1] = {58,80}},806,nil,ep_icon,function() return not isquestcompleted(ep_books[1].questID) end}, -- Nitun I
      ["book1c"]     = {"",DONE,"","",nil,nil,nil,nil,function() return isquestcompleted(ep_books[1].questID) end},
      [epspecial..5] = {GetMapNameByID(807),ep_quest_missing,"25+",pfbe,{[807] = {25,78},[862] = {45,73},[-1] = {50,89},[857] = {26,27},[858] = {83,66}},807,nil,ep_icon,function() return not isquestcompleted(ep_books[2].questID) end}, -- Greyhoof II
      [epspecial..6] = {GetMapNameByID(807),ep_quest_missing,"25+",pfcr,{[807] = {40,43},[862] = {49,65},[-1] = {51,87},[858] = {94,41}},807,nil,ep_icon,function() return not isquestcompleted(ep_books[2].questID) end}, -- Lucky Yi II
      [epspecial..7] = {GetMapNameByID(857),ep_quest_missing,"25+",pfwa,{[857] = {36,37},[862] = {48,76},[858] = {91,75},[807] = {36,90},[-1] = {51,90}},857,nil,ep_icon,function() return not isquestcompleted(ep_books[2].questID) end}, -- xia II
	  ["book2c"]     = {"",DONE,"","",nil,nil,nil,nil,function() return isquestcompleted(ep_books[2].questID) end},
      [epspecial..8] = {GetMapNameByID(858),ep_quest_missing,"25+",pfbe,{[858] = {26,50},[862] = {25,68},[-1] = {44,88}},858,nil,ep_icon,function() return not isquestcompleted(ep_books[3].questID) end}, -- Gorespine III
      [epspecial..9] = {GetMapNameByID(811),ep_quest_missing,"25+",pfwa,{[811] = {11,70},[862] = {42,57},[858] = {73,18}, [807] = {12,13},[-1] = {49,84}},811,nil,ep_icon,function() return not isquestcompleted(ep_books[3].questID) end}, -- no-no III
     [epspecial..10] = {GetMapNameByID(810),ep_quest_missing,"25+",pfwa,{[810] = {72,80},[862] = {37,50},[-1] = {48,82},[809] = {30,98}},810,nil,ep_icon,function() return not isquestcompleted(ep_books[3].questID) end}, -- ti'un III
	  ["book3c"]     = {"",DONE,"","",nil,nil,nil,nil,function() return isquestcompleted(ep_books[3].questID) end},
             [33222] = {GetMapNameByID(951),"Little Tommy Newcomer","25+",pfbe,{[951] = {35,60},[862] = {88,71},[806] = {91,94},[-1] = {64,89}},951,nil},
    -- Events
    -- Darkmoon Fairie
      [32175] = {CALENDAR_FILTER_DARKMOON,
                           "Darkmoon Pet Battle!","|TInterface\\icons\\INV_Misc_Eye_01:12|t 25",pfma..pfme..pfbe,{[823] = {47,60}}, 823, nil, nil, function() return tContains(WEfound, CALENDAR_FILTER_DARKMOON) end}
      }
 plugin_data["Data"] = questsdata
end

local function Initialize()
 if not DailyGlobalCheck then return end
 DailyGlobalCheck:LoadPlugin(plugin_data)
end

local questslocalizationdata = { ["fire_spirit"]  = {32434},
                                 ["water_spirit"] = {32439},
                                 ["earth_spirit"] = {32441},
								 ["air_spirit"]   = {32440},
								 ["majorpayne"]   = {31935},
								 ["trixxy"]       = {31909},
								 ["obalis"]       = {31971},
								 ["antari"]       = {31926},
								 ["hyuna"]        = {31953},
								 ["nishi"]        = {31955},
								 ["moruk"]        = {31954},
								 ["yon"]          = {31956},
								 ["zusshi"]       = {31991},
								 ["shu"]          = {31957},
								 ["aki"]          = {31958},
								 ["darkmoon"]     = {32175},
								 ["little_tommy"] = {33222},
								 ["darkmoon"]     = {32175},
							   }
							   
local eventframe = CreateFrame("FRAME")
local initialized = false
local epnameset = {}
local pvpnameset
local booknameset = {}
local first_login
local WEtable = {[CALENDAR_FILTER_DARKMOON] = {32175}}
-- check for active world events
local function CheckWorldEventTamers()
 local i
 local accessible
 local tmp = {}
 wipe(WEfound)
 OpenCalendar()
   
 local _,_,today = CalendarGetDate()
 for i = 1,CalendarGetNumDayEvents(0,today) do
  local title = CalendarGetDayEvent(0, today, i)
  if title then accessible = true end
  table.foreach(WEtable,function(k,v)
   if title == k then
    table.insert(WEfound, k)
    table.foreach(v, function(_, id)
	 table.insert(tmp, id)
	end)
   end
  end)
 end
 
 if table.getn(WEfound) > 0 then
  DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,6,EVENTS_LABEL)
 else
  DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,6,"")
 end
 
 if accessible then
  -- the calendar was accessible, we can stop looking for events
  eventframe:UnregisterEvent("PLAYER_ENTERING_WORLD")
  WEtable = nil
 end
end

eventframe:RegisterEvent("ADDON_LOADED")
eventframe:RegisterEvent("PLAYER_ENTERING_WORLD")
eventframe:RegisterEvent("QUEST_LOG_UPDATE")
local function eventhandler(self, event, ...)
 if event == "QUEST_LOG_UPDATE" then
  if not DailyGlobalCheck then return end
  
  local i, j, s, book
  EP_resetbooks()
  for i = 1, GetNumQuestLogEntries() do
   book = nil
   s = GetQuestLink(i)
   if s then
    local qID = string.match(s, "Hquest:(%d+)")
    if qID == "32863" then -- pvp weekly
     DGC_PTLocalizedStrings["pvp_vale_weekly"] = select(1,GetQuestLogTitle(i))
	elseif qID == "32604" then
	 DGC_PTLocalizedStrings["book1"] = select(1,GetQuestLogTitle(i))
    elseif qID == "32868" then
	 DGC_PTLocalizedStrings["book2"] = select(1,GetQuestLogTitle(i))
    elseif qID == "32869" then
	 DGC_PTLocalizedStrings["book3"] = select(1,GetQuestLogTitle(i))
    end
  
   table.foreach(ep_books, function(k,v)
     if tonumber(string.match(s, "Hquest:(%d+)")) == v.questID then
      book = v
     end
    end)
   end

   if book then
    book.index = i
	for _,j in pairs(book.pets) do
     if not epnameset[j] then
      local petname = getelitepetname(j)
      if petname ~= nil then
	   DailyGlobalCheck:SetPluginData(plugintitle,epspecial..j,2,petname)
       epnameset[j] = true
      end
     end
    end
   end -- quest found
  end

  if not pvpnameset and DGC_PTLocalizedStrings["pvp_vale_weekly"] then
   DailyGlobalCheck:SetPluginData(plugintitle,"pvpa",2,DGC_PTLocalizedStrings["pvp_vale_weekly"])
   DailyGlobalCheck:SetPluginData(plugintitle,"pvph",2,DGC_PTLocalizedStrings["pvp_vale_weekly"])
   pvpnameset = true
  end
  local i
  for i = 1,3 do
   if not booknameset[i] and DGC_PTLocalizedStrings["book"..i] then
    DailyGlobalCheck:SetPluginOrderTitle(plugintitle,RIGHT,3 + i,DGC_PTLocalizedStrings["book"..i]) -- pandaren spirits
    booknameset[i] = true
   end
  end
  
  local allset = DailyGlobalCheck:LocalizeQuestNames(plugintitle, DGC_PTLocalizedStrings, questslocalizationdata)
  if allset and pvpnameset and table.getn(booknameset) == 3 and table.getn(epnameset) == 10 then
   epnameset = nil
   booknameset = nil
   eventframe:UnregisterEvent("QUEST_LOG_UPDATE")
  end
 elseif event == "ADDON_LOADED" and ... == addonName then
  if not DailyGlobalCheck then return end
  local function setquestsdata(ids, name)
   table.foreach(ids, function(k,id) DailyGlobalCheck:SetPluginData(plugintitle,id,2,name) end)
  end
  GenerateQuestsData()
  Initialize()
  foreach(questslocalizationdata, function(name, ids)
   if DGC_PTLocalizedStrings[name] then
    setquestsdata(ids, DGC_PTLocalizedStrings[name])
   end
  end)
  initialized = true
 elseif event == "PLAYER_ENTERING_WORLD" then
  if not DailyGlobalCheck then return end
  CheckWorldEventTamers()
  if not first_login then 
   if not initialized then Initialize() end
   local tmp = select(2,GetAchievementInfo(7936))
   DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,1,tmp) -- pandaren spirits
   tmp = select(2,GetAchievementInfo(6606))
   DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,2,tmp) -- pandaria satchels
   tmp = select(2,GetAchievementInfo(6607))
   DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,4,tmp) -- other satchels
   tmp = select(2,GetAchievementInfo(8410))
   DailyGlobalCheck:SetPluginData(plugintitle,33137,2,tmp) -- pet tournament
   first_login = true   
  end  
 end
end
eventframe:SetScript("OnEvent", eventhandler)