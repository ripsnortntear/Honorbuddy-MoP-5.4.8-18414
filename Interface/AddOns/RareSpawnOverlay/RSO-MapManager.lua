RareSpawnOverlay.MapManager = {
   MapFrames = {},
   BattlefieldMinimapOverlayFrames = {},
   MapLegendCheckButtons = {},
   BattlefieldMinimap = nil,
   WorldMapDetailFrame = nil,
   Continent = nil,
   Zoom = nil,
   Level = nil
};

local OVERLAYS = "Interface\\AddOns\\RareSpawnOverlay\\Overlays\\"
local RSO = RareSpawnOverlay
local mapManager = RareSpawnOverlay.MapManager

function mapManager:SetupFrames()
   self.BattlefieldMinimap = getglobal("BattlefieldMinimap")
   self.WorldMapDetailFrame = getglobal("WorldMapDetailFrame")
end

function mapManager:WorldMapAvailable()
   return self.WorldMapDetailFrame
end

function mapManager:BattlefieldMapAvailable()
   return self.BattlefieldMinimap
end

function mapManager:WorldMapFrame_Show_Hook()
   if RSO.Options.Enabled then
      self:SetupFrames()
      self:ShowOverlay()
   end
end

function mapManager:WorldMapFrame_Hide_Hook()
   self:SetupFrames()
   self:HideOverlay()
end

function mapManager:ClearFrames()
   local k,v
   for k,v in ipairs(self.MapFrames) do
      if self:WorldMapAvailable() then
	 v.texture:SetTexture(nil)
	 v:Hide()

	 self.MapLegendCheckButtons[k]:Hide()
      end
      if self:BattlefieldMapAvailable() and self.BattlefieldMinimapOverlayFrames[k] then
	 self.BattlefieldMinimapOverlayFrames[k].texture:SetTexture(nil)
	 self.BattlefieldMinimapOverlayFrames[k]:Hide()
      end
   end
end

function mapManager:ShowOverlay()
   --print(self:IsUpdated())
   if self:IsUpdated() then
      return
   end

   local mapName, textureHeight, textureWidth = GetMapInfo()
   if (not mapName) then
      mapName = "World"
   end

   self:ClearFrames()

   local spawnData = RSO.OverlayManager:GetSpawnDataForMap(mapName)
   
   if spawnData then
      local k,v
      for k,v in ipairs(spawnData) do
	 if not self.MapFrames[k] then
	    if self:WorldMapAvailable() then
	       self.MapFrames[k] = self:CreateWorldMapOverlayFrame()
	       self.MapLegendCheckButtons[k] = self:CreateMapLegendCheckButton()
	    end
	 end
	 if not self.BattlefieldMinimapOverlayFrames[k] then
	    if self:BattlefieldMapAvailable() then
	       self.BattlefieldMinimapOverlayFrames[k] = self:CreateBattlefieldMapOverlayFrame()
	    end
	 end
	 local visibility = false
	 if RSO.Options.MobVisibility[v.Name] == nil or RSO.Options.MobVisibility[v.Name] then
		visibility = true
	    if self:WorldMapAvailable() and RSO.Options.ShowWorldMapOverlays then
	       self:UpdateWorldMapFrame(self.MapFrames[k], v.OverlayFilename)
	    end
	    if self:BattlefieldMapAvailable() and RSO.Options.ShowBattlefieldMapOverlays then
	       self:UpdateBattlefieldMapFrame(self.BattlefieldMinimapOverlayFrames[k], v.OverlayFilename)
	    end
	 end
	 if self:WorldMapAvailable() and RSO.Options.ShowWorldMapLegend then
	    self:UpdateMapLegendCheckButton(self.MapLegendCheckButtons[k], spawnData.LegendX, spawnData.LegendY, k, v.Name, v.Color, visibility, v.Information)
	 end
      end
   end

   self:SetUpdated()
end

function mapManager:HideOverlay()
   self:ClearFrames()
   self:ClearUpdated()
end

function mapManager:UpdateWorldMap()
   self:SetupFrames()
   if not self:IsUpdated() then
	   self:HideOverlay()
	   if RSO.Options.Enabled then
		 self:ShowOverlay()
	   end
   end
end

function mapManager:CreateWorldMapOverlayFrame()
   local overlayFrame = CreateFrame("Frame", nil, self.WorldMapDetailFrame)
   overlayFrame:SetWidth(1024)
   overlayFrame:SetHeight(1024)
   overlayFrame:SetPoint("TOPLEFT", 0, 0)
   local overlayTexture = overlayFrame:CreateTexture(nil, "BACKGROUND")
   overlayTexture:SetAlpha(RSO.Options.Alpha)
   overlayTexture:SetAllPoints(overlayFrame)
   overlayFrame.texture = overlayTexture
   overlayFrame:Hide()

   return overlayFrame
end

function mapManager:UpdateWorldMapFrame(frame, texture)
   frame.texture:SetTexture(texture)
   frame:Show()
end

function mapManager:CreateBattlefieldMapOverlayFrame()
   local overlayFrame = CreateFrame("Frame", nil, self.BattlefieldMinimap)
   overlayFrame:SetWidth(self.BattlefieldMinimap:GetWidth())
   overlayFrame:SetHeight(self.BattlefieldMinimap:GetHeight() * 1.5)
   overlayFrame:SetPoint("TOPLEFT", 0, 0)
   local overlayTexture = overlayFrame:CreateTexture(nil, "BACKGROUND")
   overlayTexture:SetAlpha(RSO.Options.Alpha)
   overlayTexture:SetAllPoints(overlayFrame)
   overlayFrame.texture = overlayTexture
   overlayFrame:Hide()

   return overlayFrame
end

function mapManager:UpdateBattlefieldMapFrame(frame, texture)
   frame.texture:SetTexture(texture)
   frame:Show()
end

function mapManager:CreateMapLegendCheckButton()
   local checkButton = self:CreateCheckButton(self.WorldMapDetailFrame)
   return checkButton
end

function mapManager:CreateCheckButton(par)
   local f = CreateFrame('CheckButton', nil, par)
   --f:setFrameStrata('Parent')
   f:SetHeight(32)
   f:SetWidth(32)
   f:SetPoint("TOPLEFT", 0, 0)
   f:Hide()
   f.text = f:CreateFontString(nil, nil, "GameFontNormal")
   f.text:SetPoint("LEFT", f, "RIGHT", -2, 0)
   f.text:SetJustifyH("LEFT");

   local t = f:CreateTexture()
   f:SetNormalTexture("Interface\\Buttons\\UI-CheckBox-Up")
   f:SetPushedTexture("Interface\\Buttons\\UI-CheckBox-Down")
   f:SetHighlightTexture("Interface\\Buttons\\UI-CheckBox-Highlight")
   f:GetHighlightTexture():SetBlendMode("ADD")
   f:SetCheckedTexture("Interface\\Buttons\\UI-CheckBox-Check")
   f:SetDisabledCheckedTexture("Interface\\Buttons\\UI-CheckBox-Check-Disable")
   f:SetHitRectInsets(0, -100, 0, 0)
   f:SetScript("OnEnter", function(self)
			     if self.tooltipText then
				WorldMapTooltip:SetOwner(self, "ANCHOR_RIGHT")
				WorldMapTooltip:SetText(self.tooltipText, nil, nil, nil, nil, 1)
			     end
			     if self.tooltipRequirement then
				WorldMapTooltip:AddLine(self.tooltipRequirement, "", 1.0, 1.0, 1.0)
				WorldMapTooltip:SetFrameStrata('TOOLTIP')
				WorldMapPOIFrame.allowBlobTooltip = false
				WorldMapTooltip:Show()
			     end
			  end)
   f:SetScript("OnLeave", function(self) 
					WorldMapTooltip:Hide() 
					WorldMapPOIFrame.allowBlobTooltip = true
				end)
   f:SetScript("OnClick", function(self) mapManager:HandleCheckButtonClick(self) end)

   f.tooltipText = nil
   f.tooltipRequirement = nil

   return f
end

function mapManager:HandleCheckButtonClick(checkButton)
   local mobName = checkButton.text:GetText(text)
   local checked = checkButton:GetChecked()
   RSO.Options.MobVisibility[mobName] = checked or false
   self:ClearUpdated()
   self:UpdateWorldMap()
end

function mapManager:UpdateMapLegendCheckButton(checkButton, x, y, index, text, color, checked, information)
   checkButton.text:SetText(text)
   -- XXX 20 should be configured
   checkButton:SetPoint("TOPLEFT", x, y - (index * 20))

   if information then
      checkButton.tooltipText = color.rgb..text
      checkButton.tooltipRequirement = information
   else
      checkButton.tooltipText = nil
      checkButton.tooltipRequirement = nil
   end

   checkButton:SetChecked(checked)
   checkButton.text:SetTextColor(color.r, color.g, color.b)
   checkButton.text:Show()
   checkButton:SetFrameStrata("HIGH")
   checkButton:Show()
end

function mapManager:SetUpdated()
   self.Continent = GetCurrentMapContinent()
   self.Zoom = GetCurrentMapZone()
   self.Level = GetCurrentMapDungeonLevel()
   --print("SetUpdated: c="..tostring(self.Continent)..", z="..tostring(self.Zoom)..", l="..tostring(self.Level))
end

function mapManager:ClearUpdated()
   self.Continent = nil
   self.Zoom = nil
   self.Level = nil
--   print("ClearUpdated")
end


function mapManager:IsUpdated()
   local continent = GetCurrentMapContinent()
   local zoom = GetCurrentMapZone()
   local level = GetCurrentMapDungeonLevel()
   local is_updated = (self.Continent == continent and self.Zoom == zoom and self.Level == level)
--   print("IsUpdated: "..tostring(is_updated))
--   if not is_updated then
--      print("IsUpdated: saved c="..tostring(self.Continent)..", z="..tostring(self.Zoom)..", l="..tostring(self.Level))
--      print("IsUpdated: is c="..tostring(continent)..", z="..tostring(zoom)..", l="..tostring(level))
--   end
   return is_updated
end
