-- Daily Global Check - Isle of Thunder plugin
-- Fluffies - EU-Well of Eternity
local addonName, addonTable = ...
local plugintitle = "Isle of Thunder"

local foreach = table.foreach

local iotwiicon = "Interface\\ICONS\\inv_qiraj_jewelglyphed"
local nalak_icon = "|TInterface\\EncounterJournal\\UI-EJ-BOSS-Nalak:14|t"
local s_retrieving = "No data..."
DGC_IoTLocalizedStrings = {}
-- table orders
local CENTER, LEFT, RIGHT = 1,2,3

local iot_popupdialog = {
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

-- template = {[ZONE],[NAME],[PREF],[SUFF],{{x,y},{x,y}},defaultmapID,[QUESTTYPE],[MAPICON],showfunc
local questsdata = {
 [32626] = {GetMapNameByID(928),s_retrieving,"","",nil,nil,"W"},
 [32610] = {GetMapNameByID(928),s_retrieving or "Ritual Stone (rare)","","",nil,nil,"W"},
 [32609] = {GetMapNameByID(928),s_retrieving,"","",nil,nil,"W"},
 [32708] = {GetMapNameByID(928),"Setting the Trap","","",{[928] = {51,46}},928,"Q",nil,function() return not IsQuestFlaggedCompleted(32708) end},
 [32640] = {GetMapNameByID(928),"Champions of the Thunder King","","",{[928] = {51,46}},928,"W",nil,function() return IsQuestFlaggedCompleted(32708) and UnitFactionGroup("player") == "Horde" end},
 [32641] = {GetMapNameByID(928),"Champions of the Thunder King","","",{[928] = {51,46}},928,"W",nil,function() return IsQuestFlaggedCompleted(32708) and UnitFactionGroup("player") == "Alliance" end},
 [32505] = {GetMapNameByID(928),"The Crumbled Chamberlain","","",nil,nil,"W"},
 [32611] = {GetMapNameByID(928),"Incantation","","",nil,nil,"W"},
 [32518] = {GetMapNameByID(928),EJ_GetEncounterInfo(814),nalak_icon,"",{[928] = {61,36}, [862] = {22,10}, [-1] = {44,71}},928,"W"},
}

local plugin_data = {
 ["Title"] = plugintitle,
 ["Icon"]  = iotwiicon,
 ["Data"]  = questsdata,
 ["Order"] = {[CENTER] = {{"General",32626,32610,32609,32708,32640,32641,32505,32611,32518}}}
 }

local function Initialize()
 DailyGlobalCheck:LoadPlugin(plugin_data)
end

-- server query
GetItemInfo(94221)
GetItemInfo(94222)
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

 checkiteminfo(32626,94222,"","")
 checkiteminfo(32610,94221,""," ("..EXAMPLE_TARGET_MONSTER..")")
 checkiteminfo(32609,94221,""," (Chest)")

 return result
end

local questslocalizationdata = { ["setting_the_trap"] = {32708},
                                 ["champions_tk"] = {32640,32641},
                                 ["chamberlain"] = {32505}}
local initialized = false
local eventframe = CreateFrame("FRAME")
eventframe:RegisterEvent("ADDON_LOADED")
eventframe:RegisterEvent("PLAYER_ENTERING_WORLD")
eventframe:RegisterEvent("GET_ITEM_INFO_RECEIVED")
eventframe:RegisterEvent("QUEST_LOG_UPDATE")
local function eventhandler(self, event, ...)
 if event == "QUEST_LOG_UPDATE" then							   
  if not DailyGlobalCheck then return end
  local allset = DailyGlobalCheck:LocalizeQuestNames(plugintitle, DGC_IoTLocalizedStrings, questslocalizationdata)
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
  Initialize()
  setitemnames()
  foreach(questslocalizationdata, function(name, ids)
   if DGC_IoTLocalizedStrings[name] then
    setquestsdata(ids, DGC_IoTLocalizedStrings[name])
   end
  end)
  initialized = true
  elseif event == "PLAYER_ENTERING_WORLD" then
   if not DailyGlobalCheck then
    if not DGC_IoTLocalizedStrings["dgcmessageshown"] then
	 StaticPopupDialogs["IOTDGCDialog"] = iot_popupdialog
	 StaticPopup_Show ("IOTDGCDialog")
	 DGC_IoTLocalizedStrings["dgcmessageshown"] = true
	end
    return
   end
   if not initialized then Initialize() end
   eventframe:UnregisterEvent("PLAYER_ENTERING_WORLD")
  end
end
eventframe:SetScript("OnEvent", eventhandler)