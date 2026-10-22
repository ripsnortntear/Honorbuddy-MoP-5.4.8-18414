-- Daily Global Check - World Bosses plugin
-- Fluffies EU-Well of Eternity
local addonName, addonTable = ...
local pluginicon = "Interface\\Icons\\Ability_Monk_LeeroftheOx"
local nalak_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Nalak:12|t"
local ordos_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Ordos:12|t"
local celestials_icon = "|TInterface\\Icons\\INV_CelestialSerpentMount:12|t"
local shaanger_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Sha of Anger:12|t"
local galleon_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Chief Salyis:12|t"
local oondasta_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Oondasta:12|t"

-- table orders
local CENTER, LEFT, RIGHT = 1,2,3

-- template = {[ZONE],[NAME],[PREF],[SUFF],{{x,y},{x,y}},defaultmapID,[QUESTTYPE],[MAPICON],showfunc
local questsdata

local plugin_data = {
 ["Title"] = "World Bosses",
 ["Icon"]  = pluginicon,
 ["Data"]  = questsdata,
 ["Order"] = {[CENTER] = {{"5.0",32099,32098},
                          {"5.2",32518,32519},
                          {"5.4",33117,33118}}}
 }
 
local function GenerateQuestsData()
 questsdata = {
 [32098] = {GetMapNameByID(807),EJ_GetEncounterInfo(725),galleon_icon,"",{[807] = {70,63}, [862] = {57,70}, [806] = {22,90}, [857] = {64,14}, [-1] = {55,89}},807,"W"},
 [32099] = {GetMapNameByID(809),EJ_GetEncounterInfo(691),shaanger_icon,"",{[809] = {51,87}, [862] = {46,46}, [810] = {94,68}, [-1] = {51,81}},809,"W"},
 [32519] = {GetMapNameByID(929),EJ_GetEncounterInfo(826),oondasta_icon,"",{[929] = {50,54}, [862] = {50,6}, [-1] = {52,70}},929,"W"},
 [32518] = {GetMapNameByID(928),EJ_GetEncounterInfo(814),nalak_icon,"",{[928] = {61,36}, [862] = {22,10}, [-1] = {44,71}},928,"W"},
 [33118] = {GetMapNameByID(951),EJ_GetEncounterInfo(861),ordos_icon,"",{[951] = {54,20}, [862] = {91,65}, [806] = {98, 80}, [-1] = {64, 87}},951,"W"},
 [33117] = {GetMapNameByID(951),"Celestial Challenge",celestials_icon,"",{[951] = {38,55}, [862] = {89,71}, [806] = {93, 92}, [-1] = {64,89}},951,"W"},
}
plugin_data["Data"] = questsdata
end

local function Initialize()
 if not DailyGlobalCheck then return end
 DailyGlobalCheck:LoadPlugin(plugin_data)
end

local initialized = false
local eventframe = CreateFrame("FRAME")
eventframe:RegisterEvent("VARIABLES_LOADED")
eventframe:RegisterEvent("ADDON_LOADED")
local function eventhandler(self, event, ...)
 if event == "ADDON_LOADED" and ... == addonName then
  if not DailyGlobalCheck then return end
  GenerateQuestsData()
  Initialize()
  initialized = true
 elseif event == "VARIABLES_LOADED" then
  if not initialized then Initialize() end
  -- Celestial Challenge localization
  DailyGlobalCheck:SetPluginData("World Bosses",33117,2,select(2,GetAchievementInfo(8535)))
 end
end
eventframe:SetScript("OnEvent", eventhandler)