RareSpawnOverlayOptionsDefaults = {
   ["Version"] = 3.2,
   Enabled = true,
   Alpha = 0.5,
   ShowWorldMapOverlays = true,
   ShowWorldMapLegend = true,
   ShowBattlefieldMapOverlays = true,
   ShowOnMiniMap = true,
   MobVisibility = {
   }
}

--Code by Grayhoof (SCT)
function CloneTable(t)
   local new = {}
   local i, v = next(t, nil)
   while i do
      if type(v)=="table" then 
	 v=CloneTable(v)
      end 
      new[i] = v
      i, v = next(t, i)
   end
   return new
end

RareSpawnOverlayOptions = CloneTable(RareSpawnOverlayOptionsDefaults);

local function Reset_Dropdowns()
   RareSpawnOverlayOptions = CloneTable(RareSpawnOverlayOptionsDefaults);
end

function RareSpawnOverlayOptions_Reset()
   RareSpawnOverlayOptions_Init();
   Reset_Dropdowns();
end

function RareSpawnOverlayOptions_OnLoad(panel)
   panel.name = "Rare Spawn Overlay";
   panel.default = RareSpawnOverlayOptions_Reset;
   InterfaceOptions_AddCategory(panel);
end

function RareSpawnOverlayOptions_Init()
--	RareSpawnOverlayOptionsFrameDropDownCats_OnShow();
end
