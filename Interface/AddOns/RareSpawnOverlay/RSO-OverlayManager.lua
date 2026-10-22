RareSpawnOverlay.OverlayManager = {
};

local RSO = RareSpawnOverlay
local overlayManager = RareSpawnOverlay.OverlayManager
local spawnData = RareSpawnOverlay.SpawnData

function overlayManager:GetSpawnDataForMap(mapName)
   return spawnData[mapName]
end
