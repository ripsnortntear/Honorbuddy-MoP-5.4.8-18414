RareSpawnOverlay = {
   Loaded = false,
   Options = {},
   MapManager = {},
   SpawnData = {},
   OverlayManager = {},
   ColorData = {},
   BattlefieldMapManager = {},
   API = {}
}

local RSO = RareSpawnOverlay

SLASH_RARESPAWNOVERLAY1 = "/rso"
SLASH_RARESPAWNOVERLAY2 = "/rarespawnoverlay"
SlashCmdList["RARESPAWNOVERLAY"] = function(line)
    local _, _, command, argument = string.find(line, "^%s*([^%s]-)%s+(.-)%s*$")
    if not command then
       command, argument = line, ""
    end
  
    command = string.upper(command)

    local viewChanged = false
    if (command == "ON" or command == "ENABLE") then
       RSO.Options.Enabled = true
       viewChanged = true
    elseif (command == "OFF" or command == "DISABLE") then
       RSO.Options.Enabled = false
       viewChanged = true
    elseif (command == "STANDBY" or command == "TOGGLE") then
       RSO.Options.Enabled = not RSO.Options.Enabled
       viewChanged = true
    elseif (command == "SHOW" or command == "HIDE") then
       argument = string.upper(argument)
       if argument == "WORLDMAP" then
	  RSO.Options.ShowWorldMapOverlays = (command == "SHOW")
	  RSO.MapManager:ShowOverlay()
	  DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: "..command.." World Map Overlays")
       elseif argument == "WORLDLEGEND" then
	  RSO.Options.ShowWorldMapLegend = (command == "SHOW")
	  RSO.MapManager:ShowOverlay()
	  DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: "..command.." World Map Legend")
       elseif argument == "BATTLEFIELDMAP" then
	  RSO.Options.ShowBattlefieldMapOverlays = (command == "SHOW")
	  RSO.MapManager:ShowOverlay()
	  DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: "..command.." Battlefield Map Overlays")
       else
	  DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: invalid argument "..command..": "..argument)
       end
    elseif (command == "RESET") then
       RareSpawnOverlayOptions = CloneTable(RareSpawnOverlayOptionsDefaults)
       RSO.Options = RareSpawnOverlayOptions
    elseif (command == "CONFIGURATION" or command == "CONFIG") then
       DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: Enabled = "..tostring(RSO.Options.Enabled))
	  DEFAULT_CHAT_FRAME:AddMessage("Alpha: "..RSO.Options.Alpha)
       if #RSO.Options.MobVisibility == 0 then
	  DEFAULT_CHAT_FRAME:AddMessage("All Mobs Shown")
       else
	  local k,v
	  for k,v in pairs(RSO.Options.MobVisibility) do
	     DEFAULT_CHAT_FRAME:AddMessage(k..": "..tostring(v))
	  end
       end
    elseif (command == "HELP" or command == "") then
       DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay")
       DEFAULT_CHAT_FRAME:AddMessage("/rso [on||off||toggle]")
       DEFAULT_CHAT_FRAME:AddMessage("/rso [show||hide] [worldmap||worldlegend||battlefieldmap]")
       DEFAULT_CHAT_FRAME:AddMessage("/rso config")
    else
       DEFAULT_CHAT_FRAME:AddMessage("Rare Spawn Overlay: unknown commands '"..command.."'")
    end

    if (viewChanged) then
       if (RSO.Options.Enabled) then
	  RSO.MapManager:ShowOverlay()
	  DEFAULT_CHAT_FRAME:AddMessage("RareSpawnOverlay ENABLED")
       else
	  RSO.MapManager:HideOverlay()
	  DEFAULT_CHAT_FRAME:AddMessage("RareSpawnOverlay DISABLED")
       end
    end
end

local function handleEvent(self, event, ...)
   if event == "ADDON_LOADED" then
      if RareSpawnOverlayOptions.Version ~= RareSpawnOverlayOptionsDefaults.Version then
	 RareSpawnOverlayOptions = CloneTable(RareSpawnOverlayOptionsDefaults)
      end
      RSO.Options = RareSpawnOverlayOptions
      RSO.Loaded = true
   end
   if RSO.Loaded then
      if (event == 'PLAYER_LOGIN') then
	 hooksecurefunc(WorldMapFrame, 'Show', function(self) RSO.MapManager:WorldMapFrame_Show_Hook() end)
	 hooksecurefunc(WorldMapFrame, 'Hide', function(self) RSO.MapManager:WorldMapFrame_Hide_Hook() end)
      elseif (event == 'WORLD_MAP_UPDATE') then
	 RSO.MapManager:UpdateWorldMap()
      end
   end
end

RSO.mainFrame = CreateFrame('Frame')

RSO.mainFrame:SetScript('OnEvent', handleEvent)
RSO.mainFrame:RegisterEvent("ADDON_LOADED")
RSO.mainFrame:RegisterEvent("PLAYER_LOGIN")
RSO.mainFrame:RegisterEvent("WORLD_MAP_UPDATE")
