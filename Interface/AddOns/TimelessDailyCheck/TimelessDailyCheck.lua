-- Daily Global Check - Timeless Isle plugin
-- Fluffies EU-Well of Eternity
local addonName, addonTable = ...

-- constants
local pref_achiinfo = "#achiinfo#"
local pref_achicrit = "#achicrit#"
local pref_itemname = "#item#"
local foreach = table.foreach

local tdcicon = "Interface\\ICONS\\Spell_Nature_TimeStop"
local plugintitle = "Timeless Isle"
local pluginicon = "Interface\\Icons\\Ability_Monk_LeeroftheOx"
local nalak_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Nalak:12|t"
local ordos_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Ordos:12|t"
local celestials_icon = "|TInterface\\Icons\\INV_CelestialSerpentMount:12|t"
local shafear_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Sha of Fear:12|t"
local galleon_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Chief Salyis:12|t"
local oondasta_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Oondasta:12|t"
local rares_icon = "Interface\\Icons\\INV_Misc_Bone_Skull_02"
local s_retrieving = "No data..."
DGC_TDCLocalizedStrings = {}
-- table orders
local CENTER, LEFT, RIGHT = 1,2,3
local pirates_special, ropechests_special = 1,2

local function isquestcompleted(questID)
 if questID == pirates_special then
  return (IsQuestFlaggedCompleted(32956) and IsQuestFlaggedCompleted(32957) and IsQuestFlaggedCompleted(32970))
 elseif questID == ropechests_special then
  return (IsQuestFlaggedCompleted(32969) and IsQuestFlaggedCompleted(32968) and IsQuestFlaggedCompleted(32971))
 else
  return IsQuestFlaggedCompleted(questID)
 end
end

local tdc_popupdialog = {
       text = addonName.." now requires |cff00FF00Daily Global Check|r to work, check it out on the link below and sorry for the inconvenience!",
       hasEditBox = true,
       button1 = "Ok",
       OnShow = function (self, data)
        self.editBox:SetText("http://www.curse.com/addons/wow/daily-global-check")
        self.editBox:SetWidth(self:GetWidth() - 10)
       end,
       timeout = 0,
       whileDead = true,
       hideOnEscape = true,
       preferredIndex = 3,
     }
	 
local questsdata

local plugin_data = {
 ["Title"] = plugintitle,
 ["Icon"]  = tdcicon,
 ["Data"]  = nil,
 ["Order"] = nil,
 ["Overrides"] = {["isquestcompleted"] = isquestcompleted}
 }

-- template = {[ZONE],[NAME],[PREF],[SUFF],{{x,y},{x,y}},defaultmapID,[QUESTTYPE],[MAPICON],showfunc
local function GenerateQuestsData()
 questsdata = {
  [33118] = {GetMapNameByID(951),EJ_GetEncounterInfo(861),ordos_icon,"",{[951] = {54,20}, [862] = {91,65}, [806] = {98, 80}, [-1] = {64, 87}},951,"W"},
  [33117] = {GetMapNameByID(951),"Celestial Challenge",celestials_icon,"",{[951] = {38,55}, [862] = {89,71}, [806] = {93, 92}, [-1] = {64,89}},951,"W"},
  [33137] = {GetMapNameByID(951),"Pet Tournament","","",{[951] = {35,59}},951,"W"},
  [33222] = {GetMapNameByID(951),"Little Tommy","","",{[951] = {35,60}},951},
  [pirates_special] = {"","Pirates' Chests","","",nil,nil,"W",nil,function() return isquestcompleted(pirates_special) end},
   [32956] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8727,2),"","",{[951] = {17,58}},951,"W",nil,function() return not isquestcompleted(pirates_special) end},
   [32957] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8727,1),"","",{[951] = {40,92}},951,"W",nil,function() return not isquestcompleted(pirates_special) end},
   [32970] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8727,3),"","",{[951] = {71,80}},951,"W",nil,function() return not isquestcompleted(pirates_special) end},
  [ropechests_special] = {"","Rope Chests","","",nil,nil,"W",nil,function() return isquestcompleted(ropechests_special) end},
   [32969] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8726,1),"","",{[951] = {49,69}},951,"W",nil,function() return not isquestcompleted(ropechests_special) end},
   [32968] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8726,2),"","",{[951] = {54,47}},951,"W",nil,function() return (not isquestcompleted(ropechests_special) and isquestcompleted(32969)) end},
   [32971] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8726,3),"","",{[951] = {58,60}},951,"W",nil,function() return (not isquestcompleted(ropechests_special) and isquestcompleted(32968)) end},
   [33338] = {GetMapNameByID(951),s_retrieving,"","",{[951] = {34,53}},951,"W"},
   [33334] = {GetMapNameByID(951),"5 Rares","","",{[951] = {34,53}},951,"W"},
   [33374] = {GetMapNameByID(951),"20 Elites","","",{[951] = {43,55}},951},
   [33211] = {GetMapNameByID(951),"A Timeless Question","","",{[951] = {65,50}},951},
  -- Rares
   [33312] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,31),"","",nil,nil,nil,rares_icon},
   [33301] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,14),"","",nil,nil,nil,rares_icon},
   [33299] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,23),"","",nil,nil,nil,rares_icon},
   [32966] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,8),"","",nil,nil,nil,rares_icon},
   [33310] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,24),"","",nil,nil,nil,rares_icon},
   [32967] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,10),"","",nil,nil,nil,rares_icon},
   [33314] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,30),"","",nil,nil,nil,rares_icon},
   [33295] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,1),"","",nil,nil,nil,rares_icon},
   [33313] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,29),"","",nil,nil,nil,rares_icon},
   [33309] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,26),"","",nil,nil,nil,rares_icon},
   [33300] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,15),"","",nil,nil,nil,rares_icon},
   [33315] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,28),"","",nil,nil,nil,rares_icon},
   [33297] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,3),"","",nil,nil,nil,rares_icon},
   [33294] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,4),"","",nil,nil,nil,rares_icon},
   [33311] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,27),"","",nil,nil,nil,rares_icon},
   [33303] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,18),"","",nil,nil,nil,rares_icon},
   [33296] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,2),"","",nil,nil,nil,rares_icon},
   [33306] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,22),"","",nil,nil,nil,rares_icon},
   [33292] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,7),"","",nil,nil,nil,rares_icon},
   [33298] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,13),"","",nil,nil,nil,rares_icon},
   [33302] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,17),"","",nil,nil,nil,rares_icon},
   [33307] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,20),"","",nil,nil,nil,rares_icon},
   [33293] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,12),"","",nil,nil,nil,rares_icon},
   [33305] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,19),"","",nil,nil,nil,rares_icon},
   [33304] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,16),"","",nil,nil,nil,rares_icon},
   [33308] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,25),"","",nil,nil,nil,rares_icon},
   [33322] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,21),"","",nil,nil,nil,rares_icon},
   [33316] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,5),"","",nil,nil,nil,rares_icon},
   [32959] = {GetMapNameByID(951),GetAchievementCriteriaInfo(8714,6),"","",nil,nil,nil,rares_icon} }
   -- spelurk, rattleskew,   missing?
-- spelurk 59,49
 plugin_data["Data"] = questsdata
 plugin_data["Order"] = {[CENTER] = {{BOSSES,33118,33117},
                          {PETS,33137,33222},
						  {"Pirates' Chests",pirates_special,32956,32957,32970},
						  {"Rope Chests",ropechests_special,32969,32968,32971},
						  {BATTLE_PET_SOURCE_2,33338,33334,33374,33211}},
						 [RIGHT] = {{DAILY.." "..ELITE,33312,33301,33299,32966,33310,32967,33314,33295,33313,33309,33300,33315,
						                                   33297,33294,33311,33303,33296,33306,33292,33298,33302,33307,33293,33305,
														   33304,33308,33322,33316,32959}}}
end

local function Initialize()
 DailyGlobalCheck:LoadPlugin(plugin_data)
end

-- server query
GetItemInfo(105715)
local function setitemnames()
 local result = true

 local function checkiteminfo(questID,itemID,pref,suff)
  if questsdata[questID][2] == s_retrieving or not questsdata[questID][2] then -- key
   local tmp = GetItemInfo(itemID)
   if tmp then
    DailyGlobalCheck:SetPluginData(plugintitle,questID,2,pref..tmp..suff)
    questsdata[questID][2] = pref..tmp..suff
   else
    result = false
   end
  end
 end
 
 checkiteminfo(33338,105715,"50 ","")

 return result
end
local questslocalizationdata = { ["5rares"] = {33334},
                                 ["20elites"] = {33374},
                                 ["timeless_question"] = {33211},
								 ["little_tommy"] = {33222}}
local initialized = false
local eventframe = CreateFrame("FRAME")
eventframe:RegisterEvent("ADDON_LOADED")
eventframe:RegisterEvent("PLAYER_ENTERING_WORLD")
eventframe:RegisterEvent("QUEST_LOG_UPDATE")
eventframe:RegisterEvent("GET_ITEM_INFO_RECEIVED")
local function eventhandler(self, event, ...)
 if event == "QUEST_LOG_UPDATE" then
  if not DailyGlobalCheck then return end
  local allset = DailyGlobalCheck:LocalizeQuestNames(plugintitle, DGC_TDCLocalizedStrings, questslocalizationdata)
  if allset then
   eventframe:UnregisterEvent("QUEST_LOG_UPDATE")
  end
 elseif event == "GET_ITEM_INFO_RECEIVED" then
  if not DailyGlobalCheck then return end
  if setitemnames() then
   eventframe:UnregisterEvent("GET_ITEM_INFO_RECEIVED") -- all item names set
  end
 elseif event == "ADDON_LOADED" and ... == addonName then
  if not DailyGlobalCheck then return end
  local function setquestsdata(ids, name)
   table.foreach(ids, function(k,id) DailyGlobalCheck:SetPluginData(plugintitle,id,2,name) end)
  end

  GenerateQuestsData()
  Initialize()
  setitemnames()
  foreach(questslocalizationdata, function(name, ids)
   if DGC_TDCLocalizedStrings[name] then
    setquestsdata(ids, DGC_TDCLocalizedStrings[name])
   end
  end)
  initialized = true
 elseif event == "PLAYER_ENTERING_WORLD" then
  if not DailyGlobalCheck then
   if not DGC_TDCLocalizedStrings["dgcmessageshown"] then
    StaticPopupDialogs["TDCDGCDialog"] = tdc_popupdialog
    StaticPopup_Show ("TDCDGCDialog")
    DGC_TDCLocalizedStrings["dgcmessageshown"] = true
   end
   return
  end
  if not initialized then Initialize() end
  local tmp = select(2,GetAchievementInfo(8535))
  DailyGlobalCheck:SetPluginData(plugintitle,33117,2,tmp)
  tmp = select(2,GetAchievementInfo(8410))
  DailyGlobalCheck:SetPluginData(plugintitle,33137,2,tmp)
  tmp = select(2,GetAchievementInfo(8727))
  DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,3,tmp)
  DailyGlobalCheck:SetPluginData(plugintitle,pirates_special,2,tmp)
  tmp = select(2,GetAchievementInfo(8726))
  DailyGlobalCheck:SetPluginOrderTitle(plugintitle,CENTER,4,tmp)
  DailyGlobalCheck:SetPluginData(plugintitle,ropechests_special,2,tmp)
  eventframe:UnregisterEvent("PLAYER_ENTERING_WORLD")
 end
end
eventframe:SetScript("OnEvent", eventhandler)