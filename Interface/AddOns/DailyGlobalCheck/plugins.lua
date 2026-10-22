-- Daily Global Check
-- by Fluffies
-- EU-Well of Eternity
local addonName, addonTable = ...
DailyGlobalCheck = {}

local foreach = table.foreach

function DailyGlobalCheck:LoadPlugin(data)

 print("|cff00FF00Daily Global Check |r- plugin loaded: "..data["Title"])
 
 local found = nil
 foreach(addonTable.Plugins, function(k,v)
  if v["Title"] and v["Title"] == data["Title"] then
   found = k
  end
 end)
 
 if not found then
  table.insert(addonTable.Plugins, data)
 else
  addonTable.Plugins[found] = data
  addonTable.refresh(addonTable.Plugins[found])
 end
end

local function findplugin(pname)
local result
 local function lflist(l,t)
   if t["Title"] == pname then result = t end
 end
 table.foreach(addonTable.Plugins, lflist)
 return result
end

local function contains(t, s)
 local result = false
 foreach(t, function(k,v)
  if v == tonumber(s) then result = true end
 end)
 return result
end

local cooldowntime = {}
-- called by plugins' QUEST_LOG_UPDATE until they're fully localized
function DailyGlobalCheck:LocalizeQuestNames(pname, plugin_var, data)
 local plugin = findplugin(pname)
 if not plugin_var or not plugin then return end
 if not cooldowntime[pname] then cooldowntime[pname] = GetTime()-11 end

 local changed, allset = false, false
 if GetTime() > cooldowntime[pname] + 10 then
  cooldowntime[pname] = GetTime()
  local i, s
  allset = true
  for i = 1, GetNumQuestLogEntries() do
   s = GetQuestLink(i)
   if s ~= nil then
    s = string.match(s, "Hquest:(%d+)")
    local newstr = select(1,GetQuestLogTitle(i))
	foreach(data, function(name, ids)
	 if contains(ids, s) then
      plugin_var[name] = newstr
      foreach(ids, function(k,v) 
	   if plugin["Data"][v][2] ~= newstr then
		--print("|cff00FF00Daily Global Check - Quest found and localized |r("..plugin["Data"][v][2].." -> "..newstr..")")
        plugin["Data"][v][2] = newstr
        changed = true
       end
      end)
	 else
	  allset = false
	 end
	end)
   end
  end
 end
 if changed then addonTable.refresh(plugin) end
 return allset
end

function DailyGlobalCheck:SetPluginData(pname,qID,section,str)
 local plugin = findplugin(pname)
 if not plugin or not str then return end
 
 if plugin["Data"][qID][section] ~= str then
  plugin["Data"][qID][section] = str
  addonTable.refresh(plugin)
 end
end

function DailyGlobalCheck:SetPluginOrderTitle(pname,index,section,str)
 local plugin = findplugin(pname)
 if not plugin or not str then return end
 
 if plugin["Order"][index][section][1] ~= str then
  plugin["Order"][index][section][1] = str
  addonTable.refresh(plugin)
 end
end