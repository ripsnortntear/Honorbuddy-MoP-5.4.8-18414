RareSpawnOverlay.API = {}

local RSO = RareSpawnOverlay
local api = RareSpawnOverlay.API

function api:IsEnabled()
   return RSO.Options.Enabled
end

function api:GetOptionsVersion()
   return RSO.Options.Version
end

function api:GetVersion()
   return GetAddOnMetadata("RareSpawnOverlay", "Version")
end

function api:GetOptions()
   return RSO.Options
end

function api:ShowNPC(npcid)
   local mobdata = LibRareSpawns.ByNPCID[npcid]
   if mobdata then
      RSO.Options.MobVisibility[mobdata.Name] = true
   end
end

function api:HideNPC(npcid)
   local mobdata = LibRareSpawns.ByNPCID[npcid]
   if mobdata then
      RSO.Options.MobVisibility[mobdata.Name] = false
   end
end
