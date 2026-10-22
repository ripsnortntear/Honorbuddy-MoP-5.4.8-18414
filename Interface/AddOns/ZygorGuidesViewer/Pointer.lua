local ZGV=ZygorGuidesViewer
if not ZGV then return end

local Pointer = {}
ZGV.Pointer = Pointer

local  _G,assert,table,string,tinsert,tonumber,tostring,type,ipairs,pairs,setmetatable,math,wipe,abs,ceil =
	_G,assert,table,string,tinsert,tonumber,tostring,type,ipairs,pairs,setmetatable,math,wipe,abs,ceil

local L=ZGV.L

local BZL=ZGV.BZL
local BZR=ZGV.BZR

Pointer.nummanual = 0

Pointer.waypoints = {}

Pointer.antphase=0

local Astrolabe = DongleStub("Astrolabe-ZGV")

local unusedMarkers = {}


local last_distance=0
local speed=0
local last_speed=0

local lastminimapdist=99999
local minimapcontrol_suspension=0
local minimap_lastset = 0

local cuedinged=nil

local profile={}

local inscale = 300
local MinimapSize = {
	indoor = {
		[0] = inscale, -- scale
		[1] = inscale*0.8, -- 1.25
		[2] = inscale*0.6, -- 5/3
		[3] = inscale*0.4, -- 2.5
		[4] = inscale*0.26666,  -- 3.75
		[5] = inscale*0.16666,  -- 6
	},
	outdoor = {
		[0] = 466 + 2/3, -- scale
		[1] = 400,       -- 7/6
		[2] = 333 + 1/3, -- 1.4
		[3] = 266 + 2/6, -- 1.75
		[4] = 200,       -- 7/3
		[5] = 133 + 1/3, -- 3.5
	},
}

local submap_cache = nil

local function AstrolabeFixZones(za,zb)
	if Astrolabe.WorldMapSize and Astrolabe.WorldMapSize[za].system==za then Astrolabe.WorldMapSize[za]=Astrolabe.WorldMapSize[zb] end
end


local MapFloorCountCache

function ZGV:SanitizeMapFloor(map,flr)
	--if not MapFloorCountCache then MapFloorCountCache=LibRover.MapFloorCountCache end
	--local maxfloor = 666 or MapFloorCountCache[map] or 0  --sinus: max is high, don't force it down - breaks on sneak-leveled MoP maps
	local mapData = Astrolabe.WorldMapSize[map]
	if not mapData or mapData==0 then return 0 end
	if rawget(mapData,flr) then return flr or 0 end  -- sinus: This is evil. Astrolabe is being TOO nice, striving to give us some values, but we don't want any charity. Hence, this to circumvent its metatable.
	return mapData.floorMin or 0
	--flr --[[and min (flr or 0,MapFloorCountCache[map])]] or MapFloorCountCache[map]>0 and 1 or 0 -- TODO make correct clamping if flr > max
end


Pointer.MapFloors = {}  setmetatable(Pointer.MapFloors,{__index=function(self,id) return rawget(self,id) or 0 end})

function Pointer:CacheFloors()
	local mapfloors = self.MapFloors
	for id=1,1000 do
		local sane_id = self:SanitizePhase(id)
		if GetMapNameByID(sane_id) then
			SetMapByID(sane_id)
			local l = GetNumDungeonMapLevels()
			if l>0 then mapfloors[id] = l end
		end
	end
	SetMapToCurrentZone()
end

function Pointer:Startup()
	profile = ZGV.db.profile

	self:CacheFloors()  -- universal, used NOT only by the internal pointer.

	self:SetArrowSkin(profile.arrowskin)

	if ZGV.db.profile.frame_positions and ZGV.db.profile.frame_positions.ZygorGuidesViewerPointer_ArrowCtrl then
		ZygorGuidesViewerPointer_ArrowCtrl:SetPoint(unpack(ZGV.db.profile.frame_positions.ZygorGuidesViewerPointer_ArrowCtrl))
	end

	self.Icons:SetAntColorsFromOptions()

	--[[
	self.EventFrame = CreateFrame("FRAME")
	self.EventFrame:Show()
	self.EventFrame:SetScript("OnEvent",PointerEventFrame_OnEvent)
	self.EventFrame:RegisterEvent("WORLD_MAP_UPDATE")
	--]]

	self.OverlayFrame = ZGV.ChainCall(CreateFrame("FRAME","ZygorGuidesViewerPointerOverlay",WorldMapButton))
		:SetAllPoints(true)
		:SetSize(1002,668)
		--:SetFrameStrata("DIALOG")
		--:SetFrameLevel(WorldMapButton:GetFrameLevel()+1)
		--:SetScript("OnMouseUp",self.Overlay_OnClick)
		--:EnableMouse(true)
		:SetScript("OnEvent",self.Overlay_OnEvent)
		:SetScript("OnUpdate",self.Overlay_OnUpdate)
		:RegisterEvent("PLAYER_ENTERING_WORLD")
		:RegisterEvent("PLAYER_ALIVE")
		:RegisterEvent("PLAYER_UNGHOST")
		:RegisterEvent("ZONE_CHANGED_NEW_AREA")
		:RegisterEvent("WORLD_MAP_UPDATE")
		.__END

	--hooksecurefunc("WorldMapButton_OnClick",ZGV.Pointer.hook_WorldMapButton_OnClick)

	ZGV:ScheduleRepeatingTimer(function()  if not WorldMapFrame:IsVisible() then SetMapToCurrentZone() end  end, 10.0)   -- to help the current zone cache

	if ZGV.db.profile.autotravel then
		ZGV:ScheduleRepeatingTimer(function()  Pointer:DoAutoTravel() end, 0.01)
	end

	if ZGV.db.profile.debug then
		self.OverlayFrame.LibRoverButton = ZGV.ChainCall(CreateFrame("BUTTON","ZGVLibRoverButton",WorldMapButton,"UIPanelButtonTemplate"))
		:SetPoint("TOPLEFT",WorldMapButton,"TOPLEFT")
		:SetSize(100,50)
		:SetText("Travel nodes")
		:SetFrameStrata("DIALOG")
		:SetScript("OnClick",LibRover.ShowDebugMenu)
		:RegisterForClicks("AnyUp")
		:Show()
		--:SetFrameLevel(WorldMapButton:GetFrameLevel()+1)
		.__END
	end


	--[[
	local texture = overlay:CreateTexture("ZygorGuidesViewerPointerOverlayTexture","OVERLAY")
	texture:SetAllPoints(true)
	--texture:SetTexture(ZGV.DIR .. "\\Maps\\deadmines")
	texture:SetTexCoord(0,0.975,0,0.65)
	texture:Hide()
	overlay.texture = texture

	local youarehere = overlay:CreateTexture("ZygorGuidesViewerPointerOverlayYouarehere","OVERLAY")
	youarehere:SetTexture(ZGV.DIR .. "\\Skins\\minimaparrow-green-dot")
	overlay.youarehere = youarehere
	--]]

	self:UpdateArrowVisibility()

	--hooksecurefunc("WorldMapFrame_OnShow",ZGV.Pointer.hook_WorldMapFrame_OnShow)


	--WorldMapFrame.PlayerCoord = WorldMapFrame:CreateFontString(nil,"ARTWORK","GameFontHighlightSmall")
	--WorldMapFrame.CursorCoord = WorldMapFrame:CreateFontString(nil,"ARTWORK","GameFontHighlightSmall")

	--WorldMapFrame.PlayerCoord:SetText("Player")
	--WorldMapFrame.CursorCoord:SetText("Cursor")

	--ZGV.ScheduleRepeatingTimer(self,"FixMapLevel", 1.0)

	-- try to fix Astrolabe zoning problems...
	AstrolabeFixZones(770,700) --twilight
	AstrolabeFixZones(700,770)

	AstrolabeFixZones(545,679) --Gilneas
	AstrolabeFixZones(679,545)

	AstrolabeFixZones(793,697)
	
	self:SetMinimapPlayerTexture()

	self.ready = true

	hooksecurefunc("WorldMapQuestPOI_OnClick",Pointer.QuestPOI_PointToMe)
	--hooksecurefunc("WatchFrameQuestPOI_OnClick",Pointer.QuestWatchPOI_PointToMe)
end


Pointer.Icons = {
	greendot = { icon=ZGV.DIR.."\\Skins\\minimaparrow-green-dot", size=40, minisize=25, rotates=false, edgeicon=ZGV.DIR.."\\Skins\\minimaparrow-green-edge", edgesize=60, spinner=true, onminimap=always },
	graydot = { icon=ZGV.DIR.."\\Skins\\minimaparrow-green-dot", size=40, minisize=25, rotates=false, edgeicon=ZGV.DIR.."\\Skins\\minimaparrow-green-edge", edgesize=60, spinner=true, desat=1, onminimap=always },
	arrow = { icon=ZGV.DIR.."\\Skins\\minimaparrow-path", size=70, minisize=60, rotates=true, edgeicon=ZGV.DIR.."\\Skins\\minimaparrow-path", edgesize=50 },
	ant =		   { icon=ZGV.DIR.."\\Skins\\minimaparrow-ant", alpha=0.8, size=30, minisize=25, rotates=false, edgeicon=nil, edgesize=1 },
	ant_g = { icon=ZGV.DIR.."\\Skins\\minimaparrow-ant", r=0.4, g=1, b=0, alpha=0.8, size=30, minisize=25, rotates=false, edgeicon=nil, edgesize=1 },
	ant_b =   { icon=ZGV.DIR.."\\Skins\\minimaparrow-ant", r=0, g=0.7, b=1, alpha=0.8, size=30, minisize=25, rotates=false, edgeicon=nil, edgesize=1 },
	ant_p =   { icon=ZGV.DIR.."\\Skins\\minimaparrow-ant", r=0.8, g=0.3, b=1, alpha=0.8, size=30, minisize=25, rotates=false, edgeicon=nil, edgesize=1 },
	ant_y =   { icon=ZGV.DIR.."\\Skins\\minimaparrow-ant", r=1, g=0.8, b=0, alpha=0.8, size=30, minisize=25, rotates=false, edgeicon=nil, edgesize=1 },
	none = { icon="", alpha=0.0, size=1, minisize=1, rotates=false, edgeicon=nil, edgesize=1 },

	ant_g_default = { r=0.4, g=1, b=0, alpha=0.8 },
	ant_b_default = { r=0, g=0.7, b=1, alpha=0.8 },
	ant_p_default = { r=0.8, g=0.3, b=1, alpha=0.8 },
	ant_y_default = { r=1, g=0.8, b=0, alpha=0.8 },
	ant_default = { r=1, g=1, b=1, alpha=0.8 },

	SetAntColorsFromOptions = function(self)
		local function setrgb(colortable,savedcolors)
			if not colortable then return end
			colortable.r,colortable.g,colortable.b,colortable.alpha = savedcolors.r,savedcolors.g,savedcolors.b,savedcolors.alpha or savedcolors.a  -- we use alpha, options use a
		end

		ZGV.db.profile.customcolorants=true
		if not ZGV.db.profile.customcolorants then
			setrgb(self.ant_g, self.ant_g_default)
			setrgb(self.ant_b, self.ant_b_default)
			setrgb(self.ant_p, self.ant_p_default)
			setrgb(self.ant_y, self.ant_y_default)
			setrgb(self.ant, self.ant_default)
		elseif ZGV.db.profile.customcolorants and not ZGV.db.profile.singlecolorants then
			setrgb(self.ant_g, ZGV.db.profile.colorantstaxi)
			setrgb(self.ant_b, ZGV.db.profile.colorantsship)
			setrgb(self.ant_p, ZGV.db.profile.colorantsportal)
			setrgb(self.ant_y, ZGV.db.profile.colorantsfly)
			setrgb(self.ant, ZGV.db.profile.colorantsother)
		elseif ZGV.db.profile.customcolorants and ZGV.db.profile.singlecolorants then
			setrgb(self.ant_g, ZGV.db.profile.singlecolorantscolor)
			setrgb(self.ant_b, ZGV.db.profile.singlecolorantscolor)
			setrgb(self.ant_p, ZGV.db.profile.singlecolorantscolor)
			setrgb(self.ant_y, ZGV.db.profile.singlecolorantscolor)
			setrgb(self.ant, ZGV.db.profile.singlecolorantscolor)
		end
	end
}
setmetatable(Pointer.Icons,{__index=function(k) return Pointer.Icons.greendot end})
for k,v in pairs(Pointer.Icons) do if type(v)=="table" then __CLASS[v]="PointerIcon_"..k end end


--[[
local numlevels=0
local oldlevel=1
function Pointer.FixMapLevel()
	local x,y = GetPlayerMapPosition("player")
	if x<=0 and y<=0 then
		-- perhaps wrong floor indeed.
		numlevels = GetNumDungeonMapLevels()
		if numlevels>1 then
			oldlevel = GetCurrentMapDungeonLevel()
			for lev=1,numlevels do
				if lev~=oldlevel and GetPlayerMapPosition("player")>0 then
					GetCurrentMapDungeonLevel()
			end
		end
end
--]]

--[[
	data elements:
	title - guess
	type - 'way' 'poi' 'manual' 'corpse' 'path'
	icon - texture path
	onminimap - 'always' 'zone'
	overworld - show on world map
	persistent - don't hide when arrived at
--]]

local phasedBases={ [1]=606, [2]=737, [3]=700, [4]=720, [5]=697 }
local phasedMaps = {
	[606]=1, -- Mount Hyjal
	[683]=1,
	[737]=2, -- The Maelstrom
	[751]=2,
	[700]=3, -- Twilight Highlands
	[770]=3,
	[720]=4, -- Uldum
	[748]=4,
	[697]=5, -- Zul'Gurub
	[793]=5,
} -- TODO expand as per need
setmetatable(phasedMaps,{__index=function(t,map) return map and type(map)=="number" and 10000+map or 0 end})
ZGV.Pointer.phasedMaps = phasedMaps

-- initialize phase bases with... something from the phase maps. This might suck, beware.
--for map,num in pairs(phasedMaps) do phasedBases[num]=phasedBases[num] or map end -- hardcoded the bases in because they were not getting assigned correctly here.

function Pointer:IsEnvironmentPhased(target_map)
	-- Assumption: if the player is/was in a map which is known to be phased, the best fit map is the current one
	-- WARNING Blizz works in mysterious ways. If something's broken with phased terrain corpse display, please look here
	-- keywords: worgen arrow not working qq

	-- No no no! Such extensive setmapping seems to cause lag...
	--[[
	local _mm,_ff=GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	SetMapToCurrentZone()
	local mm,ff=GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	SetMapByID(_mm) SetDungeonMapLevel(_ff)
	--]]
	local mm=ZGV.CurrentMapID
	if mm then
		return rawget(phasedMaps,mm) and phasedMaps[mm]>0 and phasedMaps[target_map]==phasedMaps[mm]
	end
end

-- Save this map as its group's base phase. Do it with map ids obtained fresh from GetCurrentMapAreaID() calls, NEVER from any ID you happen to think of.
function Pointer:SetBasePhase(map)
	local group = phasedMaps[map]
	if not group then return end
	phasedBases[group] = phasedBases[group] and map
end

function Pointer:SanitizePhase(map)
	return phasedBases[phasedMaps[map] or 0] or map
end

local waypoints_ants = {}
setmetatable(waypoints_ants,{__index=function(t,i)
	local waypoint={}
	waypoint.type="ant"
	Pointer:MakeMarkerFrames(waypoint,"Ant")

	waypoint.minimapFrame.waypoint = waypoint
	waypoint.worldmapFrame.waypoint = waypoint
	waypoint.taximapFrame.waypoint = waypoint

	--waypoint.minimapFrame.icon:SetRotation(icon.rotates and data.angle or 0)
	local icon=Pointer.Icons.ant
	waypoint.minimapFrame.icon:SetSize(icon.minisize,icon.minisize)
	waypoint.minimapFrame.icon:SetDesaturated(icon.desat)
	waypoint.minimapFrame.icon:SetAlpha(icon.alpha or 1)
	waypoint.minimapFrame.hide_on_minimap_edge=true
	waypoint.minimapFrame.self_updating=true
	waypoint.minimapFrame.arrow:SetTexture(icon.edgeicon)
	waypoint.minimapFrame.arrow:SetSize(1,1) --hide?
	--waypoint.worldmapFrame.icon:SetRotation(icon.rotates and data.angle or 0)
	waypoint.worldmapFrame.icon:SetSize(icon.size,icon.size)
	waypoint.worldmapFrame.icon:SetDesaturated(icon.desat)
	waypoint.worldmapFrame.icon:SetAlpha(icon.alpha or 1)

	waypoint.taximapFrame.icon:SetSize(icon.size*0.7,icon.size*0.7)
	waypoint.taximapFrame.icon:SetDesaturated(icon.desat)
	waypoint.taximapFrame.icon:SetAlpha(icon.alpha or 1)

	waypoint.onminimap="always"
	waypoint.overworld=true
	waypoint.showonedge=false

	__CLASS[waypoint]="WaypointAnt"

	rawset(t,i,waypoint)
	return waypoint
end})
Pointer.waypoints_ants = waypoints_ants

-- SPECIAL setwaypoint, optimized for ants
local icons=Pointer.Icons
function Pointer:SetWaypoint_ant (m,f,x,y,num,icon, ant)  -- ant is here for one-time lookup! don't reuse!!
	-- phasing? meh.
	local waypoint = waypoints_ants[num]
	waypoint.m=m
	waypoint.f=f
	waypoint.x=x
	waypoint.y=y
	waypoint.c = Astrolabe.WorldMapSize[m].system

	-- clone some data to make a smarter, more aware ant
	waypoint.p1m,waypoint.p2m,waypoint.p1f,waypoint.p2f =ant.p1m,ant.p2m,ant.p1f,ant.p2f
	waypoint.ant_dist=ant.ant_dist

	if not icon then icon=icons.ant end
	if waypoint.icon~=icon then
		waypoint.minimapFrame.icon:SetTexture(icon.icon)
		waypoint.worldmapFrame.icon:SetTexture(icon.icon)
		waypoint.taximapFrame.icon:SetTexture(icon.icon)
	end
	local r,g,b = icon.r or 1,icon.g or 1,icon.b or 1
	waypoint.minimapFrame.icon:SetVertexColor(r,g,b)
	waypoint.worldmapFrame.icon:SetVertexColor(r,g,b)
	waypoint.taximapFrame.icon:SetVertexColor(r,g,b)
	waypoint.icon=icon

	waypoint.passive=true
	waypoint.is_on = true

	local lm,lf = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	waypoint:UpdateWorldMapIcon(lm,lf)
	waypoint:UpdateMiniMapIcon(lm,lf)
	if TaxiFrame:IsShown() then
		waypoint.taximapFrame.icon:SetVertexColor(icon.r or 1,icon.g or 1,icon.b or 1)
		waypoint:UpdateTaxiMapIcon()
	end

	Pointer.MinimapButton_OnUpdate(waypoint.minimapFrame,1000)

	return waypoint
end

function Pointer:ClearWaypoints_ant(active)
	for i=active+1,#waypoints_ants do
		local waypoint = waypoints_ants[i]
		Astrolabe:RemoveIconFromMinimap(waypoint.minimapFrame)
		waypoint.minimapFrame:Hide()
		waypoint.minimapFrame.waypoint=nil  -- so that they don't reappear
		waypoint.worldmapFrame:Hide()
		waypoint.worldmapFrame.waypoint=nil
		waypoint.taximapFrame:Hide()
		waypoint.is_on = false
	end
end

local tmp_data = {}
local function add_default_data(data)
	wipe(tmp_data)
	for k,v in pairs(data) do tmp_data[k]=v end
	--if not data.title then data.title="Waypoint" end
	if not tmp_data.type then tmp_data.type="way" end
	if not tmp_data.icon then tmp_data.icon=Pointer.Icons.greendot end
	return tmp_data
end

function Pointer:SetWaypoint (m,f,x,y,data,arrow)
	local data = add_default_data(data)  -- Clone! so this is tmp_data from above, basically.

	if data.cleartype and data.type then  self:ClearWaypoints(data.type)  end

	-- Let's see if this is evil. If current map is phased, remember it and use it to de-phase other maps of the same group.

	--local mapm,mapf = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	--self:SetBasePhase(mapm)  -- Assuming nobody in their right mind is viewing a wrong-phase map. Calling SetMapByID(phasedmap) WILL break this...

	if not m then
		--m,f=mapm,mapf  -- Use fresh values, NOT the cached crap. No map means put markers on CURRENTLY DISPLAYED map, not the player's current.
		m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	else
		--m=self:SanitizePhase(m)  -- de-phase map!
	end

	--[[
	local phased=self:IsEnvironmentPhased(m)
	if not m or phased then
		if data.type~="ant" then
			--Pointer:Debug("The player and the target are in the same phased environment, putting a marker on the current map instead.")
		end
		--m,f=mm,ff
	end
	--]]

	local waypoint = self:GetMapMarker (m,f or 0,x,y,data)

	--Pointer:Debug("Adding waypoint type "..data.type.." in "..c..","..z..","..x..","..y)

	if not waypoint then return end

	waypoint.title=data.arrowtitle or data.title or ("%s %d,%d"):format(Pointer.GetMapNameByID2(waypoint.m or 0,waypoint.f) or "?",waypoint.x*100,waypoint.y*100)

	--waypoint.type=data.type
	--waypoint.angle=data.angle	-- not needed, as that's set in GetMapMarker from data

	waypoint:SetIcon(waypoint.icon)

	Pointer.MinimapButton_OnUpdate(waypoint.minimapFrame,1000)

	if waypoint.type=="manual" then
		self.nummanual = self.nummanual + 1
	end

	tinsert(self.waypoints,waypoint)

	if arrow==nil then arrow=true end
	if arrow and (waypoint.type=="manual" or waypoint.type=="way" or waypoint.type=="route" or waypoint.type=="corpse") then
		self:ShowArrow(waypoint)
	end

	--[[
	if waypoint.find_path then
		Pointer:Debug("Starting travel, since waypoint was find_path")
		self:FindTravelPath(waypoint)
	end
	--]]

	--if waypoint and waypoint.type~="ant" then Pointer:Debug("Waypoint set to map:"..waypoint.m.." floor:"..waypoint.f) end

	if data.findpath then
		self:FindTravelPath(waypoint)
	end

	return waypoint
end

function Pointer:ShowWaiting(phase)
	self.ArrowFrame.WaitingPhase = phase
end

local tmp_tab={}
function Pointer:GetMapMarker (m,f,x,y,data)
	--Pointer:Debug("Internal GetMapMarker: "..tostring(c).." "..tostring(z).." "..tostring(x).." "..tostring(y).." "..tostring(title))
	if not m and not f then
		m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	end
	--Pointer:Debug("Internal GetMapMarker nums: "..tostring(c).." "..tostring(z).." "..tostring(x).." "..tostring(y).." "..tostring(title))

	if not m or not f or not x or not y then
		Pointer:Debug("GetMapMarker bailing out; map=%s/%d %.2f %.2f",m,f or -1,x or -99,y or -99)
		return
	end

	--if x>1 or y>1 then x,y=x/100,y/100 end

	local waypoint = self:GetUnusedMarker()
	--local c = LibRover.ContinentsByID[m] or -1
	local c = Astrolabe.WorldMapSize[m].system
	waypoint.m=m
	waypoint.f=f
	waypoint.x=x
	waypoint.y=y
	waypoint.c=c
	waypoint.minimapFrame.waypoint = waypoint
	waypoint.worldmapFrame.waypoint = waypoint
	waypoint.taximapFrame.waypoint = waypoint

	table.zygor_join(waypoint,data)
	-- TODO: add callbacks for distance detection

	waypoint.minimapFrame:EnableMouse(not waypoint.passive)
	waypoint.worldmapFrame:EnableMouse(not waypoint.passive)

	local lm,lf = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	waypoint:UpdateWorldMapIcon(lm,lf)
	waypoint:UpdateMiniMapIcon(lm,lf)
	if TaxiFrame:IsShown() then waypoint:UpdateTaxiMapIcon() end

	--if lc==c and lz==z then Astrolabe:PlaceIconOnMinimap(waypoint.minimapFrame, c, z, x, y) end

	return waypoint
end

local dont_setwaypoint=false
function Pointer:ClearWaypoints (waytype)
	Pointer:Debug("ClearWaypoints %s",waytype or "all?")
	if waytype=="ant" then return Pointer:ClearWaypoints_ant(0) end
	local n=0
	local w=1
	dont_setwaypoint=true
	while w<=#self.waypoints do
		if not waytype or self.waypoints[w].type==waytype then
			n=n+1
			self:RemoveWaypoint(w)
		else
			w=w+1
		end
	end
	dont_setwaypoint=false
	--if waytype=="path" then
	--	Pointer.TempWaypath = nil
	--end
	return n
end

function Pointer:RemoveWaypoint(waypoint)
	local wayn
	if type(waypoint)=="number" then  wayn=waypoint  waypoint=self.waypoints[wayn]  end
	if not waypoint then return end -- let's just play nice --assert(waypoint,"Waypoint not found")
	assert(__CLASS[waypoint]=="Waypoint")
	if not wayn and type(waypoint)=="table" then for w=1,#self.waypoints do if self.waypoints[w]==waypoint then wayn=w end end end
	if not wayn then return end -- let's just play nice assert(wayn,"No waypoint number found")

	--Pointer:Debug("Removing waypoint %d=%d",waypoint.num,wayn)
	Astrolabe:RemoveIconFromMinimap(waypoint.minimapFrame)
	waypoint.minimapFrame:Hide()
	waypoint.minimapFrame.waypoint=nil
	waypoint.worldmapFrame:Hide()
	waypoint.worldmapFrame.waypoint=nil
	waypoint.taximapFrame:Hide()

	--if waypoint.type~="ant" then print("Removing way "..waypoint.type) end

	if waypoint.type=="manual" then
		self.nummanual = max(0,self.nummanual - 1)
	end

	if waypoint.in_set then self:RemoveWaypointFromSets(waypoint) end

	if self.ArrowFrame.waypoint==waypoint then self:HideArrow() end
	if self.DestinationWaypoint==waypoint then
		Pointer:Debug("Removed DestinationWaypoint")
		self.DestinationWaypoint=nil  self.TempWaypath=nil
		self:ClearSet("route")
	end

	for k,v in pairs(waypoint) do if not k:match(".+Frame") then waypoint[k]=nil end end
	unusedMarkers[waypoint]=1
	table.remove(self.waypoints,wayn)
end

function Pointer:RemoveWaypointFromSets(waypoint)
	local set = self.pointsets[waypoint.in_set]
	if not set then return end
	for pi,point in ipairs(set.points) do if point==waypoint then tremove(set,pi) return end end
end

function Pointer:HideArrow()
	self.ArrowFrame.waypoint = nil
	self:ResetMinimapZoom() -- to perhaps reset the zoom
	--self.ArrowFrame:Hide()
end

function Pointer:ShowArrow(waypoint)
	self.initialdist = nil
	if not waypoint then return self:HideArrow() end
	assert(__CLASS[waypoint]=="Waypoint")
	--if waypoint.type~="manual" then self:ClearWaypoints("manual") end

	-- fix for "arrow stuck to cursor", probably
	ZygorGuidesViewerPointer_ArrowCtrl:StopMovingOrSizing()
	self.ArrowFrame.dragging=nil

	Astrolabe:PlaceIconOnMinimap(waypoint.minimapFrame, waypoint.m, waypoint.f, waypoint.x, waypoint.y) -- if it's not already there, place it

	self.ArrowFrame.waypoint = waypoint

	self.ArrowFrame.WaitingPhase = nil

	last_distance=0
	speed=0
	lastbeeptime=GetTime()+3
	cuedinged=nil

	lastminimapdist=99999

	--self.ArrowFrame.temporarilyhidden = true
	--self.ArrowFrame:Show()
end

--[[
function Pointer:GetWaypointBearings(way)
	--local dx,dy =
	if type(way)==number then way=self.waypoints[way] end

end
--]]

local markerproto = {}
local markermeta = {__index=markerproto}
local nummarkers=0

function Pointer:MakeMarkerFrames(marker,type)
	setmetatable(marker,markermeta)

	type=type or "Marker"

	marker.minimapFrame = CreateFrame(type=="Ant" and "FRAME" or "BUTTON", "ZGVMarker"..nummarkers.."Mini", Minimap, "ZygorGuidesViewerPointerMinimap".. type)
	marker.minimapFrame.isZygorWaypoint=true -- marking the marker to be protected by substituted Minimap:GetChildren() call. Pun intended.
	--marker.minimapFrame.icon:SetTexture(ZGV.DIR.."Arrows\\Cloqwerk\\minimaparrow-green-dot")
	--marker.minimapFrame.arrow:SetTexture(ZGV.DIR.."Arrows\\Cloqwerk\\minimaparrow-green-edge")

	marker.worldmapFrame = CreateFrame(type=="Ant" and "FRAME" or "BUTTON", "ZGVMarker"..nummarkers.."World", self.OverlayFrame, "ZygorGuidesViewerPointerWorldMap".. type)
	marker.worldmapFrame:SetFrameStrata(type=="Ant" and "MEDIUM" or "HIGH")

	marker.taximapFrame = CreateFrame("FRAME", "ZGVMarker"..nummarkers.."Taxi", TaxiFrame, "ZygorGuidesViewerPointerTaxiMap".. type)
	marker.taximapFrame:SetFrameStrata(type=="Ant" and "MEDIUM" or "HIGH")

	--marker.worldmapFrame.icon:SetTexture(ZGV.DIR.."Arrows\\Cloqwerk\\minimaparrow-green-dot")

	return marker
end

function Pointer:GetUnusedMarker()
	local marker = next(unusedMarkers)
	if marker then
		unusedMarkers[marker]=nil
		return marker
	end

	-- create a new marker
	nummarkers=nummarkers+1
	marker = self:MakeMarkerFrames({visible=true})

	__CLASS[marker]="Waypoint"

	return marker
end

function markerproto:Hide()
	self.minimapFrame:Hide()
	self.worldmapFrame:Hide()
	self.taximapFrame:Hide()
	self.visible = false
end

function markerproto:Show()
	self.minimapFrame:Show()
	self.worldmapFrame:Show()
	if TaxiFrame:IsShown() then self.taximapFrame:Show() end
	self.visible = true
end

function markerproto:SetIcon(icon)  -- leave icon empty to just reset the current icon
	icon = icon or self.icon
	self.minimapFrame.icon:SetTexture(icon.icon)
	self.minimapFrame.icon:SetRotation(icon.rotates and self.angle or 0)
	local size = self.minisize or self.size or icon.minisize
	self.minimapFrame.icon:SetSize(size,size)
	self.minimapFrame.icon:SetDesaturated(icon.desat)
	self.minimapFrame.icon:SetVertexColor(icon.r or 1,icon.g or 1,icon.b or 1)
	self.minimapFrame.icon:SetAlpha(self.alpha or icon.alpha or 1)
	self.minimapFrame.arrow:SetTexture(icon.edgeicon)
	self.minimapFrame.arrow:SetSize(icon.edgesize,icon.edgesize)
	self.minimapFrame.arrow:SetDesaturated(icon.desat)
	self.worldmapFrame.icon:SetTexture(icon.icon)
	self.worldmapFrame.icon:SetRotation(icon.rotates and self.angle or 0)
	if self.truesize then
		local data = Astrolabe.WorldMapSize[self.m]
		local floordata = rawget(data,self.f or 0)
		data = data or floordata
		local width = data.width
		local yd_per_px = width / WorldMapDetailFrame:GetWidth() -- horizontally AND vertically; maps may be 0..1 0..1 with a 2:3 ratio, but pixels are 1:1. Hopefully.
		self.truesize_px = self.truesize / yd_per_px * 3.4  -- wtf?
	end
	local size = self.truesize_px or self.size or icon.size
	self.worldmapFrame.icon:SetSize(size,size)
	self.worldmapFrame.icon:SetDesaturated(icon.desat)
	self.worldmapFrame.icon:SetVertexColor(icon.r or 1,icon.g or 1,icon.b or 1)
	self.worldmapFrame.icon:SetAlpha(self.alpha or icon.alpha or 1)
	self.worldmapFrame.spinner:Hide()
	self.taximapFrame.icon:SetTexture(icon.icon)
	self.taximapFrame.icon:SetRotation(icon.rotates and self.angle or 0)
	self.taximapFrame.icon:SetSize(size/2,size/2)
	self.taximapFrame.icon:SetVertexColor(icon.r or 1,icon.g or 1,icon.b or 1)
	self.taximapFrame.icon:SetAlpha(self.alpha or icon.alpha or 1)
	--if icon.spinner then self.worldmapFrame.spinner:Show() else self.worldmapFrame.spinner:Hide() end
	self.icon = icon
end

function markerproto:UpdateWorldMapIcon(m,f)
	local show=true

	local cm = GetCurrentMapAreaID()
	--[[
	if cm==13 or cm==14 or cm==862 or cm==485 or cm==466 or cm==-1 then
		-- it's world map all right.
		self.worldmapFrame:EnableMouse(false)
		local halfsize = self.size or self.icon.size or 10
		self.worldmapFrame.icon:SetSize(halfsize,halfsize)
	else
		self.worldmapFrame:EnableMouse(not self.passive)
		local fullsize = self.size or (self.icon and self.icon.size) or 20
		self.worldmapFrame.icon:SetSize(fullsize,fullsize)
	end
	--]]

	if not ZGV.Pointer.OverlayFrame:IsShown() or self.hidden then show=false end

	if not m then m,f=cm,GetCurrentMapDungeonLevel() end

	if show and not self.overworld then
		if self.m~=m then show=false end
	end

	 -- NO. FADE IT.
	--and not (Astrolabe.WorldMapSize[m][f] and Astrolabe.WorldMapSize[m][f].microName)  -- unless we're in a cave, show outside points
	--and not (Astrolabe.WorldMapSize[self.m][self.f] and Astrolabe.WorldMapSize[self.m][self.f].microName)  -- unless it's in a cave, show those from overworld
	--and self.type~="ant" then  -- or it's an ant.

	if show then
		if self.type=="ant" then
			-- fuck. Ants are system-mapped. Check their parents?
			if self.p1m==m and self.p2m==m then
				-- gradual fading, if one of floors is current
				if self.p1f==f and self.p2f~=f then
					self.worldmapFrame:SetAlpha(1-self.ant_dist*0.7)
				elseif self.p1f~=f and self.p2f==f then
					self.worldmapFrame:SetAlpha(0.3+self.ant_dist*0.7)
				elseif self.p1f==f and self.p2f==f then
					self.worldmapFrame:SetAlpha(1.0)
				else
					self.worldmapFrame:SetAlpha(0.3)
				end
			else
				self.worldmapFrame:SetAlpha(1.0)
			end
		else
			-- normal waypts
			self.worldmapFrame:SetAlpha((self.m==m and self.f~=f) and 0.3 or 1.0)
		end
	else
		self.worldmapFrame:SetAlpha(1.0) --?? not shown and alpha=1??
		--show=false
	end

	if show then
		local x,y = Astrolabe:PlaceIconOnWorldMap(ZGV.Pointer.OverlayFrame, self.worldmapFrame, self.m, self.f, self.x, self.y)
		if not x or not y
		or x<0 or y<0 or x>1 or y>1
		then
			show=false
		end
	end


	if show then
		self.worldmapFrame:Show()

		self.worldmapFrame:EnableMouse(cm~=-1 and cm~=13 and cm~=14 and cm~=485 and cm~=862)

		--self.worldmapFrame.icon:ClearAllPoints()
		--self.worldmapFrame.icon:SetAllPoints()
		--ZGV:Print("Showing "..way.title)
	else
		self.worldmapFrame:Hide()
	end
end

function markerproto:UpdateMiniMapIcon(m,f)
	if not m then m,f=GetCurrentMapAreaID(),GetCurrentMapDungeonLevel() end
	if profile.minicons
	and not self.hidden
	--and not (GetInstanceInfo()==BZL["Deeprun Tram"])
	and
	(
	 self.onminimap=="always" or
	 ZGV.Pointer.ArrowFrame.waypoint==self or
	 ((self.onminimap=="zone" or self.onminimap=="zonedistance") and m==self.m)
	) then
		local r = Astrolabe:PlaceIconOnMinimap(self.minimapFrame, self.m, self.f, self.x, self.y)
	else
		Astrolabe:RemoveIconFromMinimap(self.minimapFrame)
	end
end

local function Astrolabe_PlaceIconOnWorldMap2( taxiBgFrame, icon, taximapID, mapID, mapFloor, xPos, yPos )
	-- check argument types
	--[[
	argcheck(worldMapFrame, 2, "table");
	assert(3, worldMapFrame.GetWidth and worldMapFrame.GetHeight, "Usage Message");
	argcheck(icon, 3, "table");
	assert(3, icon.SetPoint and icon.ClearAllPoints, "Usage Message");
	argcheck(mapID, 4, "number");
	argcheck(mapFloor, 5, "number", "nil");
	argcheck(xPos, 6, "number");
	argcheck(yPos, 7, "number");
	--]]

	local nX, nY = Astrolabe:TranslateWorldMapPosition(mapID, mapFloor, xPos, yPos, taximapID, 0)

	-- anchor and :Show() the icon if it is within the boundry of the current map, :Hide() it otherwise
	if ( nX and nY and (0 < nX and nX <= 1) and (0 < nY and nY <= 1) ) then
		--icon:ClearAllPoints();
		icon:SetPoint("CENTER", taxiBgFrame, "TOPLEFT", nX * taxiBgFrame:GetWidth(), -nY * taxiBgFrame:GetHeight())
		icon:Show();
	else
		icon:Hide();
	end
	return nX, nY
end

function markerproto:UpdateTaxiMapIcon()
	if not TaxiFrame:IsShown() then return end

	local show=true

	local taxitexture = Pointer.taxitexture

	if not taxitexture then
		Pointer:Debug("No map for taxi! texture is '%s'",TaxiFrame.InsetBg:GetTexture())
		return
	end
	if not Astrolabe.WorldMapSize[taxitexture] then
		Pointer:Debug("No map for taxi! map num is %d",taxitexture)
		return
	end

	if show and not self.overworld then
		show=false
	end

	--[[
	if show then
		if self.type=="ant" then
			-- fuck. Ants are system-mapped. Check their parents?
			if self.p1m==m and self.p2m==m then
				-- gradual fading, if one of floors is current
				if self.p1f==f and self.p2f~=f then
					self.worldmapFrame:SetAlpha(1-self.ant_dist*0.7)
				elseif self.p1f~=f and self.p2f==f then
					self.worldmapFrame:SetAlpha(0.3+self.ant_dist*0.7)
				elseif self.p1f==f and self.p2f==f then
					self.worldmapFrame:SetAlpha(1.0)
				else
					self.worldmapFrame:SetAlpha(0.3)
				end
			else
				self.worldmapFrame:SetAlpha(1.0)
			end
		else
			-- normal waypts
			self.worldmapFrame:SetAlpha((self.m==m and self.f~=f) and 0.3 or 1.0)
		end
	else
		self.worldmapFrame:SetAlpha(1.0) --?? not shown and alpha=1??
		--show=false
	end
	--]]


	if show then
		local x,y = Astrolabe_PlaceIconOnWorldMap2(TaxiFrame.InsetBg, self.taximapFrame, taxitexture, self.m, self.f, self.x, self.y)
		if not x or not y
		or x<0 or y<0 or x>1 or y>1
		then
			show=false
		end
	end

	self.taximapFrame:EnableMouse(false)

	--[[
	if show then
		self.taximapFrame:Show()

		--self.worldmapFrame:EnableMouse(cm~=-1 and cm~=13 and cm~=14 and cm~=485 and cm~=862)

		--self.worldmapFrame.icon:ClearAllPoints()
		--self.worldmapFrame.icon:SetAllPoints()
		--ZGV:Print("Showing "..way.title)
	else
		self.taximapFrame:Hide()
	end
	--]]
end



-----------------------------------------------------------------------
--[[
do
	local lastx,lasty
	local x,y,zone
	function Pointer:GetPlayerPosition()
		local x,y = GetPlayerMapPosition("player")
	end
end

function Pointer:GetDistFromPlayer(c,z,x,y)
	local pc,pz,px,py

	local px, py = GetPlayerMapPosition("player")
	px, py, pzone = self:GetCurrentPlayerPosition()
	if pzone then
		pzone = BZL[pzone]
	end

	if px == 0 or py == 0 or not px or not py then
		return nil
	end
	if pzone and BZH[pzone] then
		pzone = BZL[pzone]
	end
	if zone and BZH[zone] then
		zone = BZL[zone]
	end
	if not zone then
		zone = GetRealZoneText()
	end
	if not pzone then
		pzone = zone
	end
	local dist = Tourist:GetYardDistance(zone, x, y, pzone, px, py)
	return dist
end
--]]


-- Code taken from HandyNotes, thanks Xinhuan
---------------------------------------------------------
-- Public functions for plugins to convert between MapFile <-> C,Z
--

--[[
local continentMapFile = {
	[WORLDMAP_COSMIC_ID] = "Cosmic", -- That constant is -1
	[0] = "World",
	[1] = "Kalimdor",
	[2] = "Azeroth",
	[3] = "Expansion01",
	[scarlet_cont] = "ScarletEnclave",
}
local reverseMapFileC = {}
local reverseMapFileZ = {}
for C in pairs(Astrolabe.ContinentList) do
	for Z = 1, #Astrolabe.ContinentList[C] do
		local mapFile = Astrolabe.ContinentList[C][Z]
		reverseMapFileC[mapFile] = C
		reverseMapFileZ[mapFile] = Z
	end
end
for C = -1, 3 do
	local mapFile = continentMapFile[C]
	reverseMapFileC[mapFile] = C
	reverseMapFileZ[mapFile] = 0
end
--]]

--[[
function Pointer:GetMapFile(C, Z)
	if type(C)=="string" then return end
	if not C or not Z then return end
	if Z == 0 then
		return continentMapFile[C]
	elseif C > 0 then
		return Astrolabe.ContinentList[C][Z]
	end
end
function Pointer:GetCZ(mapFile)
	return reverseMapFileC[mapFile], reverseMapFileZ[mapFile]
end
--]]

local function FormatDistance(dist)
	if not dist then return "" end
	if profile.arrowmeters then
		local mdist = dist * 0.9144
		if mdist>1000 then
			return L['dist_km']:format(mdist/1000)
		else
			return L['dist_m']:format(mdist)
		end
	else
		if dist>1760 then
			return L['dist_mi']:format(dist/1760)
		else
			return L['dist_yd']:format(dist)
		end
	end
end
ZGV.FormatDistance=FormatDistance

---------------
function Pointer:CreateArrowFrame()
	if not self.ArrowFrameCtrl then
		self.ArrowFrameCtrl = ZygorGuidesViewerPointer_ArrowCtrl
		self.ArrowFrameCtrl:SetScript("OnUpdate",self.ArrowFrameControl_OnUpdate)
		--self.ArrowFrameCtrl:SetPoint("CENTER",UIParent,"BOTTOMLEFT",ZGV.db.profile.arrowposx,ZGV.db.profile.arrowposy)
		--self.ArrowFrameCtrl:SetMovable(1)
		--self.ArrowFrameCtrl:Show()
	end

	if not self.CurrentArrowSkin then
		self:SetArrowSkin(ZGV.db.profile.arrowskin)
	end

	self:SetupArrow()
end

local CHAIN = ZGV.ChainCall

function Pointer:SetupArrow()
	self.ArrowFrame = self.CurrentArrowSkin:CreateFrame()

	--self.ArrowFrame:SetScript("OnMouseUp",ZygorGuidesViewer.Pointer.ArrowFrame_OnMouseDown) -- messes with EnableMouse below! Do this first.
	--self.ArrowFrame:SetScript("OnMouseDown",ZygorGuidesViewer.Pointer.ArrowFrame_OnMouseUp)
	self.ArrowFrame:SetScript("OnClick",ZygorGuidesViewer.Pointer.ArrowFrame_OnClick)

	-- freeze
	self.ArrowFrame:RegisterForDrag(not profile.arrowfreeze and "LeftButton")
	self.ArrowFrame:RegisterForClicks(not profile.arrowfreeze and "AnyUp")
	self.ArrowFrame:EnableMouseWheel(not profile.arrowfreeze)
	self.ArrowFrame:EnableMouse(not profile.arrowfreeze)

	-- scale
	local scale = profile.arrowscale
	if not scale then return end
	self.ArrowFrame:SetScale(scale)
	self:SetFontSize(profile.arrowfontsize)

	-- opacity
	self.ArrowFrame:SetAlpha(profile.arrowalpha)

	local iconScale = 38*profile.arrowscale
	if not self.ArrowFrame.ArrowIcon then
		self.ArrowFrame.ArrowIcon = CHAIN(CreateFrame("Button","ZygorGuidesViewerPointerArrow_Icon",self.ArrowFrameCtrl,"SecureActionButtonTemplate"))
			:SetPoint("CENTER",self.ArrowFrame,"CENTER",0,-1)
			:RegisterForClicks("LeftButtonUp")
			:SetFrameLevel(self.ArrowFrame:GetFrameLevel()+2)
			:RegisterForDrag(not profile.arrowfreeze and "LeftButton")
			:Hide()
			:SetScript("OnDragStart",self.ArrowFrame:GetScript("OnDragStart"))
			:SetScript("OnDragStop",self.ArrowFrame:GetScript("OnDragStop"))
			:SetHighlightTexture("Interface\\Buttons\\ButtonHilight-Square")
			:SetPushedTexture("Interface\\Buttons\\CheckButtonHilight")
			.__END
		self.ArrowFrame.ArrowIcon:GetPushedTexture():SetBlendMode("ADD")
		self.ArrowFrame.ArrowIcon.texture = self.ArrowFrame.ArrowIcon:CreateTexture("ZygorGuidesViewerPointerArrow_IconTexture","BACKGROUND")
		self.ArrowFrame.ArrowIcon.cooldown = CHAIN(CreateFrame("Cooldown","",self.ArrowFrame.ArrowIcon,"CooldownFrameTemplate")) :SetAllPoints() :Show() .__END
	end
	--Set Scale and opacity of the icon over the arrow.
	self.ArrowFrame.ArrowIcon:SetSize(iconScale,iconScale)
	self.ArrowFrame.ArrowIcon.texture:SetAlpha(profile.arrowalpha)
end

function Pointer:UpdateWaypoints()
	-- worldmap updates only, so far
	local m,f=GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	for w,way in ipairs(self.waypoints) do
		way:UpdateWorldMapIcon(m,f)
		way:UpdateMiniMapIcon(m,f)
		way:UpdateTaxiMapIcon(m,f)
	end
end

function Pointer:SetFontSize(size)
	local f=self.ArrowFrame.title:GetFont()
	self.ArrowFrame.title:SetFont(f,size)
	--[[
	self.ArrowFrame.dist:SetFont(f,size)
	self.ArrowFrame.eta:SetFont(f,size)

	self.ArrowFrame.title:SetHeight(size)
	self.ArrowFrame.dist:SetHeight(size)
	self.ArrowFrame.eta:SetHeight(size)
	--]]
end



function Pointer:MinimapZoomChanged()
	if profile.minimapzoom then
		--minimapcontrolled = true
	else
		--minimapcontrolled = false
		Minimap:SetZoom(0)
		MinimapZoomOut:Disable()
		MinimapZoomIn:Enable()
	end
end

function Pointer:ResetMinimapZoom()
	if profile.minimapzoom then
		Minimap:SetZoom(0)
		MinimapZoomOut:Disable()
		MinimapZoomIn:Enable()
	end
	--minimap_lastset = 0
end

local function ShowTooltip(button,tooltip)
	if not button.waypoint or not button.waypoint.title or button.waypoint.passive then return end
	tooltip:SetOwner(button,"ANCHOR_TOP")
	tooltip:ClearLines()
	tooltip:SetText(button.waypoint.title)
	if button.waypoint.OnEnter then
		local r = button.waypoint:OnEnter(tooltip)
		if r==false then return end
	end
	--GameTooltip:SetFrameStrata("TOOLTIP")
	tooltip:Show()
end

function Pointer.MinimapButton_OnEnter(self,arg,tip)
	if self.waypoint and (self.icon:IsVisible() or (self.arrow and self.arrow:IsVisible())) then
		tip = tip or GameTooltip
		ShowTooltip(self,tip)
		tip:AddLine(L['waypoint_tip_distance']:format(FormatDistance(self.dist)))

		if self.waypoint.type=="manual" or self.waypoint.surrogate_for and self.waypoint.surrogate_for.type=="manual" then
			tip:AddLine(L['waypoint_tip_clearmanual'])
		end

		if ZGV.db.profile.debug then
			if self.waypoint.type=="route" and self.waypoint.pathnode.type~="end" then
				tip:AddLine("|cffff0000DEBUG:|r |cffddff00Right-click|cff00ff00 to ban travel node")
			end
			tip:AddLine("|cffff0000DEBUG:|r |cffddff00Ctrl-Alt-click|cff00ff00 to save map button to ZGV.MAPBUT")
		end

		tip:Show()
		self.hastooltip=true
	end
end

function Pointer.WorldmapButton_OnEnter(self,arg)
	if self.waypoint and (self.icon:IsVisible() or self.arrow:IsVisible()) then
		WorldMapPOIFrame.old_allowBlobTooltip = WorldMapPOIFrame.allowBlobTooltip
		WorldMapPOIFrame.allowBlobTooltip = false

		self.dist = self.waypoint.minimapFrame.dist
		Pointer.MinimapButton_OnEnter(self,arg,WorldMapTooltip)
	end
end

function Pointer.MinimapButton_OnLeave(self)
	GameTooltip:Hide()
	self.hastooltip=false
end

function Pointer.WorldmapButton_OnLeave(self)
	WorldMapTooltip:Hide()

	WorldMapPOIFrame.allowBlobTooltip = WorldMapPOIFrame.old_allowBlobTooltip
	WorldMapPOIFrame.old_allowBlobTooltip = nil
end

local oldfacing=-1
function Pointer.MinimapButton_OnUpdate(self,elapsed)
	if not self.waypoint then return end
	local c = self.minimap_count
	if not c then c=0 end
	c = c + elapsed
	if c < 0.1 then
		self.minimap_count = c
		return
	end
	elapsed = c
	self.minimap_count = 0

	--if GetInstanceInfo()==BZL['Deeprun Tram']
	if not profile.minicons
	then -- We're in a mapless place. Tough luck.
		self.icon:Hide() self.arrow:Hide() return
	end

	local dist,x,y = Astrolabe:GetDistanceToIcon(self)

	if not dist --[[or IsInInstance()--]] then self.icon:Hide() self.arrow:Hide() return end

	self.lastdist=self.dist
	self.dist = dist
	if self.waypoint.OnUpdate then self.waypoint:OnUpdate() end

	if self.waypoint.hidden or self.waypoint.hideminimap then
		self.icon:Hide()
		self.arrow:Hide()
		return
	end

	local edge = Astrolabe:IsIconOnEdge(self)

	local facing = GetPlayerFacing()
	local rotate_minimap = GetCVar("rotateMinimap")=="1"

	if edge then
		if self.waypoint.showonedge or ZGV.Pointer.ArrowFrame.waypoint==self.waypoint then
			self.icon:Hide()
			self.arrow:Show()

			local angle = Astrolabe:GetDirectionToIcon(self)
			--angle = angle + 2.356194  -- rad(135)

			if rotate_minimap then
				angle = angle - GetPlayerFacing()
			end

			--local sin,cos = math.sin(angle)*0.71, math.cos(angle) * 0.71
			--self.arrow:SetTexCoord(0.5-sin, 0.5+cos, 0.5+cos, 0.5+sin, 0.5-cos, 0.5-sin, 0.5+sin, 0.5-cos)
			self.arrow:SetRotation(angle)
		else
			self.icon:Hide()
			self.arrow:Hide()
		end
	else
		self.icon:Show()
		self.arrow:Hide()
		if self.waypoint.angle and self.waypoint.icon.rotates then
			self.icon:SetRotation(self.waypoint.angle - (rotate_minimap and facing or 0))
		end
	end
	--oldfacing=facing

	-- handle tooltip distance updates
	if self.lastdist~=self.dist and self.hastooltip then
		ZGV.Pointer.MinimapButton_OnEnter(self)
	end

	-- minimap autozoom
	if profile.minimapzoom then
		local Minimap = Minimap
		local getzoom = Minimap:GetZoom()
		if getzoom~=minimap_lastset then
			-- user playing with minimap; suspend our activities for a while
			minimapcontrol_suspension = 5.0
			minimap_lastset = getzoom
		end

		-- are we pointed to?
		if Pointer.ArrowFrame.waypoint==self.waypoint then
			if minimapcontrol_suspension>0 then
				minimapcontrol_suspension = minimapcontrol_suspension - elapsed
			else
				local old_minimap_lastset=minimap_lastset
				local dist = dist*2
				if dist~=lastminimapdist then
					local mapsizes = MinimapSize[Astrolabe.minimapOutside and 'outdoor' or 'indoor']

					minimap_lastset=0
					for i=1,Minimap:GetZoomLevels()-1 do
						if dist<mapsizes[i]*0.7 then minimap_lastset=i end
					end

					if old_minimap_lastset~=minimap_lastset then
						-- sanitise buttons
						if(minimap_lastset == (Minimap:GetZoomLevels() - 1)) then MinimapZoomIn:Disable() else MinimapZoomIn:Enable() end
						if(minimap_lastset == 0) then MinimapZoomOut:Disable() else MinimapZoomOut:Enable() end

						Minimap:SetZoom(minimap_lastset)
					end
				end
				lastminimapdist=dist
			end
		end
	end
end

function Pointer.MinimapButton_OnClick(self,button)
	if button=="RightButton" then
		--if ZGV.Pointer.ArrowFrame.waypoint==self.waypoint then ZGV.Pointer:HideArrow() end
		if self.waypoint.type=="manual" then ZGV.Pointer:RemoveWaypoint(self.waypoint)
		elseif self.waypoint.surrogate_for and self.waypoint.surrogate_for.type=="manual" then ZGV.Pointer:RemoveWaypoint(self.waypoint.surrogate_for)
		elseif self.waypoint.type=="route" then
			-- if we're debugging, allow for banning a node
			if ZGV.db.profile.debug and self.waypoint.pathnode and ZGV.Pointer.pathfollow=="pathfind" then
				LibRover.banned_nodes[self.waypoint.pathnode]=1
				Pointer:Debug("Banned node: %s",self.waypoint.pathnode:tostring())
				LibRover:UpdateNow()
			else
				-- it's on the route, but we're not debugging? just recalc. And kill manuals if it's a manual.
				ZGV.Pointer:ClearWaypoints("manual")
			end
		elseif self.waypoint.type=="way" then return
		end
		ZGV:SetWaypoint()
	else
		if ZGV.db.profile.debug and IsControlKeyDown() and IsAltKeyDown() then
			ZGV.MAPBUT=self
			ZGV:Print("Map button saved to ZGV.MAPBUT")
			if IsShiftKeyDown() then
				DoSpoo(ZGV.MAPBUT)
			end
			return
		end
		Pointer:Debug("Clicked way %d type %s",self.waypoint.num,self.waypoint.type)
		if self.waypoint.type=="way" or self.waypoint.type=="path" then ZGV.Pointer:ClearWaypoints("manual") end
		ZGV.Pointer:FindTravelPath(self.waypoint)
	end
end

function Pointer.MinimapButton_OnEvent(self,event,...)
	-- temporarily unused
	Pointer:Debug("MINIMAP ONEVENT "..event)
	if not self.waypoint then self:Hide() return end

	if event == "PLAYER_ENTERING_WORLD" then
		local way = self.waypoint

		if way then
			way:UpdateMiniMapIcon()
		end
	end
end

function Pointer.WorldMapButton_OnEvent(self,event,...)
	local way = self.waypoint

	--ZGV:Print("WORLDMAP ONEVENT "..event)
	if event == "WORLD_MAP_UPDATE" then
		--[[
		local show=true
		if not way.showinallzones then
			local m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
			if way.m~=m or way.f~=f then show=false end
		end

		if way and way.OnEvent then way:OnEvent(event,...) end
		if not way or way.hidden then self:Hide() return end

		local x,y = Astrolabe:PlaceIconOnWorldMap(ZGV.Pointer.OverlayFrame, self, self.waypoint.m, self.waypoint.f, self.waypoint.x, self.waypoint.y)
		if (x and y and (0 < x and x <= 1) and (0 < y and y <= 1)) then
			self:Show()
		else
			self:Hide()
		end

		self.icon:ClearAllPoints()
		self.icon:SetAllPoints()
		--]]

		--[[
		if GetCurrentMapZone()==0 then
			self:SetWidth(10)
			self:SetHeight(10)
		else
		end
		--]]

		--[[
		self:SetWidth(25)
		self:SetHeight(25)
		--]]

	elseif event == "PLAYER_ENTERING_WORLD" or event=="ZONE_CHANGED_NEW_AREA" then
		if way then way:UpdateMiniMapIcon() end
	end
end

function Pointer.TaxiMapButton_OnEvent(self,event,...)
	local way = self.waypoint

	--ZGV:Print("WORLDMAP ONEVENT "..event)
	if event == "TAXIMAP_OPENED" then
		Pointer.taxitexture = tonumber(TaxiFrame.InsetBg:GetTexture():match("TAXIMAP(%d+)"))+2000
		if way then way:UpdateMiniMapIcon() end
	end
end

function Pointer.WorldMapButton_OnMouseWheel(self,delta,...)
	if ZGV.db.profile.debug then
		self.waypoint.truesize = (self.waypoint.truesize or 50) * (delta>0 and 1.1 or 0.909090)
		print(self.waypoint.truesize)
		self.waypoint:SetIcon(Pointer.Icons.greendot)
	end
end

function Pointer:IsCorpseArrowNeeded() -- small utility against bulky ifs, NB: waypointer-independent
	return ZGV.db.profile.corpsearrow and UnitIsDeadOrGhost("player") and select(2, IsInInstance()) ~= "pvp" and not IsActiveBattlefieldArena()
end

function Pointer.Overlay_OnEvent(self,event,...)
	if event == "WORLD_MAP_UPDATE" then
		if ZGV.db.profile.waypointaddon=="internal" then
			--[[
				-- VERY OBSOLETE. This was supposed to show plain "YOU ARE HERE" markers on artificial instance maps, for lack of coordinates.

				if not WorldMapFrame:IsVisible() then
					return

				elseif IsInInstance() and GetPlayerMapPosition("player")==0 then
					--magic!
					local inst = instancemaps[GetZoneText()]
					if inst then
						ZGV.Pointer.OverlayFrame.texture:SetTexture(ZGV.DIR .. "\\Maps\\" ..inst.map)
						ZGV.Pointer.OverlayFrame.texture:Show()
						ZGV.Pointer.OverlayFrame:EnableMouse(true)

						local room = inst.rooms and inst.rooms[GetMinimapZoneText()]
						if room then
							--ZGV:Print("room")
							self.youarehere:SetPoint("CENTER",self,"TOPLEFT",room.x*self:GetWidth(),-room.y*self:GetHeight())
							self.youarehere:Show()
						else
							self.youarehere:Hide()
						end

						WorldMapFrameTitle:SetText(GetZoneText())
						WorldMapFrameAreaLabel:SetAlpha(0)
					end

					for way,w in pairs(ZGV.Pointer.waypoints) do
						way:Hide()
					end

				else
					--magic!
					-- hide instance overlay
					ZGV.Pointer.OverlayFrame.texture:Hide()
					ZGV.Pointer.OverlayFrame:EnableMouse(false)
					WorldMapFrameAreaLabel:SetAlpha(1)

					--ZGV:Print("showing...")
				end
			 --]]

			local m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
			local count=0
			for w,way in ipairs(ZGV.Pointer.waypoints) do
				way:UpdateWorldMapIcon(m,f)
				if way.worldmapFrame:IsShown() and way.OnEvent then way:OnEvent(event,...) end
			end

			-- force ants update
			Pointer:AnimateAnts()
		end

	elseif (event=="PLAYER_ALIVE" or event=="PLAYER_ENTERING_WORLD" or event=="ZONE_CHANGED_NEW_AREA")
			and ZGV.db.profile.waypointaddon=="internal" then
		if ZGV.Pointer:IsCorpseArrowNeeded() then
			Pointer:Debug(event.." (dead)")
			-- corpse arrow
			if not ZGV.Pointer.corpsearrow then ZGV.Pointer:SetCorpseArrow() end
		else
			Pointer:Debug(event.." (not dead)")
			ZGV.Pointer.corpsearrow = nil
			local n=ZGV.Pointer:ClearWaypoints("corpse")
			if n>0 then ZGV:SetWaypoint() end
		end

		--[[
		for way,w in pairs(ZGV.Pointer.waypoints) do
			way:UpdateMiniMapIcon()
		end
		--]]

	elseif event=="PLAYER_UNGHOST" then
		Pointer:Debug("Player unghosted!")
		ZGV.Pointer:ClearWaypoints("corpse")
		ZGV.Pointer.corpsearrow = nil
		ZGV:SetWaypoint()
	end
end
------------------------------------------- ARROW -----------------


--[[
function Pointer.ArrowFrame_OnEvent(self,event,...)
	if event=="WORLD_MAP_UPDATE" then
		ZGV.Pointer:UpdateWaypoints()
	end
end
--]]



local arrowctrl_elapsed=0
local arrowfps = 1/60
local forceupdate_elapsed = 0

function Pointer.ArrowFrameControl_OnUpdate(self,elapsed)
	arrowctrl_elapsed = arrowctrl_elapsed + elapsed
	if arrowctrl_elapsed >= arrowfps then   -- 20fps MAX
		-- update skin IF WE HAVE ONE ON only, dammit
		if Pointer.ArrowFrame then Pointer.ArrowFrame_OnUpdate_Common(Pointer.ArrowFrame,arrowctrl_elapsed) end
		arrowctrl_elapsed = 0
	end

	local icon=Pointer.ArrowFrame.ArrowIcon
	if icon:IsVisible() and not ZGV.Pointer.ArrowFrame:IsVisible() then Pointer.ArrowFrame_HideSpellArrow(self) end

	-- update waypoints periodically, in case some get stuck on player-out-of-map,-go-away state
	forceupdate_elapsed = forceupdate_elapsed + elapsed
	if forceupdate_elapsed > 3 then
		Pointer:UpdateWaypoints()
		forceupdate_elapsed = 0
	end
end

-- And we have an onupdating frame even if hidden. Yay!

-- map perc as: t1..t2 to 0.0..0.5  t3..t4 to 0.5..1.0
function Pointer.CalculateDirectionTiers(perc,t1,t2,t3,t4)
	if perc<t1 then return 0 , 1
	elseif perc<t2 then return (perc-t1)/(t2-t1)*0.5 , 2
	elseif perc<t3 then return 0.5 , 3
	elseif perc<t4 then return ((perc-t3)/(t4-t3)*0.5) + 0.5 , 4
	else return 1.0 , 5 end
end

local oldangle = 0

local title,disttxt,etatxt

local speeds={}
local stoptime=0
local avgspeed=0

local eta_elapsed=0
local etadisp_elapsed=0

local lastbeeptime=GetTime()
local lastturntime=lastbeeptime
local laststoptime=lastbeeptime
local lastmovetime=lastbeeptime

local msin,mcos,mabs=math.sin,math.cos,math.abs

local eta

function Pointer:GetDefaultStepDist()
	return IsFlying("player") and 15 or 5
end

local pathfindlockout = 0

local function TableFind(table,val)  -- unused, yeah
	for k,v in pairs(table) do if v==val then return k end end
end

--- Find 2 values in a table
-- @return key1,key2
local function TableFind2(table,val1,val2)
	local k1,k2
	for k,v in pairs(table) do  if v==val1 then k1=k end  if v==val2 then k2=k end  end
	return k1 or -1,k2 or -1
end

-- Floor orderings:
-- 1 = "floors" (higher number is above lower)
-- -1 = "dungeons" (higher number is below lower)
-- table - order your floors from TOP to BOTTOM.
-- upstairs, downstairs
local FloorUpDowns = {
	[721] = 1, --Blackrock Spire
	[753] = -1, --Blackrock Spire
	[321] = -1, --Orgrimmar
	[504] = -1, --Dalaran
	[691] = -1, --Gnomeregan
	[688] = {2,1,3},
	[679] = 1, -- Gileas
	[903] = 1,
	[922] = -1,
}
-- For very specific crossing descriptions, see Localization/Core_enUS.lua, entries "pointer_floors_29_14_16" etc.

function FloorOrder(map,a,b)
	local order = FloorUpDowns[map]
	if not order then return a-b end -- default: -1
	if type(order)=="number" then return order*(b-a) end
	if type(order)=="table" then
		if order[1] then
			-- plain ordering
			a,b=TableFind2(order,a,b)
			return a-b
		end
		-- "2_3" ordering
		return order[a.."_"..b] or -1
	end
end

local function ZoneIsOutdoor(map)
	local mapdata = Astrolabe.WorldMapSize[map]   if not mapdata then return end
	local system = mapdata.system
	return (system==13 or system==14 or system==466 or system==485 or system==862 or system==605 or system==544) and map~=504 and map~=321 and map~=903
	-- continents
end

local function L_or_nil(id)
	local l = L[id]
	if l==id then return nil else return l end
end

local function GetPreciseFloorCrossingText(map,f_from,f_to)
	-- try explicit first
	local explicit_text = L_or_nil('pointer_floors_'..map..'_'..f_from..'_'..f_to)
					or L_or_nil('pointer_floors_'..map..'_'..f_from..'_*')
					or L_or_nil('pointer_floors_'..map..'_*_'..f_to)
					or L_or_nil('pointer_floors_'..map)
	if explicit_text and explicit_text:sub(1,1)=="*" then -- *something - look up in pointer_floors_something
		explicit_text = L_or_nil('pointer_floors_'..explicit_text:sub(2))
	end
	if explicit_text then return explicit_text end

	-- assume outdoors are caves (for now)
	if ZoneIsOutdoor(map) then -- overworld, assuming we're in/out of a cave or mine.
		if f_from==0 then return L['pointer_floors_incave'] end
		if f_to==0 then return L['pointer_floors_outcave'] end
	end

	-- use floor ordering, perhaps?
	local floororder=FloorOrder(map,f_from,f_to)
	return floororder>0 and L['pointer_floors_up'] or L['pointer_floors_down']
end


local cache_throttle=0
local were_in_unknown_location

function Pointer.ArrowFrame_HideSpellArrow(self)
	local icon = Pointer.ArrowFrame.ArrowIcon

	local name,link,_,_,_,_,_,_,equipslot,texture = GetItemInfo(icon.item or 0)


	if equipslot and equipslot:find("^INV") then
		local slot = GetInventorySlotInfo(ZGV.ItemScore:GetItemSlot(equipslot))
		local curItemInSlot = ZGV.ItemScore:GetItemInSlot(equipslot) or 0
		local oldCurItem = icon.curItemInSlot -- 0 means the slot was empty

		if curItemInSlot == icon.item then -- They have the item to teleport equipped.
			if oldCurItem == 0 then --They had no item equipped before the tp. So take off their item.

				local RemoveItem = function(slot) --functioned so that we can just return once a open slot is found instead of break break breaking
					local bag,bagslot
					for bag=0, NUM_BAG_SLOTS do
						for bagslot=1, GetContainerNumSlots(bag) do
							local itemid=GetContainerItemID(bag,bagslot)
							if not itemid then
								PickupInventoryItem(slot) --Pickup the quest item
								PickupContainerItem(bag,bagslot) --put it in an empty slot
								return
							end
						end
					end
				end

				RemoveItem(slot)

			elseif oldCurItem ~= curItemInSlot then -- They had a different item equipped. Equip that one.
				ZGV:Print(L['pointer_reequip_item']:format(link))
				EquipItemByName(oldCurItem)
			end --else they had the tp item equipped. So leave it.
		end
	end

	icon.item = nil
	icon.curItemInSlot = nil

	icon:Hide()
end

function Pointer.ArrowFrame_ShowSpellArrow(self,waypoint)
	local icon = Pointer.ArrowFrame.ArrowIcon
	local safe =  not InCombatLockdown() and not UnitIsDeadOrGhost("player")
	local found,name,_,texture,equipslot

	local node = waypoint.pathnode
	local link = node and node.parentlink
	local mode = link and link.mode
	-- now mode contains mode of transport!

	local spell = node.spell or (node.link and node.link.spell)
	local item = node.item or (node.link and node.link.item)
	local cdFunc = spell and GetSpellCooldown or GetItemCooldown
	local cooltime,cooldur,coolcharges = cdFunc(item or spell or 0)

	if not mode then return end

	if safe then
		if mode=="hearth" then
				name,_,_,_,_,_,_,_,_,texture = GetItemInfo(6948) --Yes, get local name for hearthstone then cast it by item name. Casting by spell doesn't work
				icon:SetAttribute("type","item")
				icon:SetAttribute("item",name)
		elseif spell then
				name,_,texture = GetSpellInfo(spell)
				icon:SetAttribute("type","spell")
				icon:SetAttribute("spell",name)
		elseif item and GetItemCount(item or 0)>0 then
				name,_,_,_,_,_,_,_,equipslot,texture = GetItemInfo(item)
				icon:SetAttribute("type","item")
				icon:SetAttribute("item",name)
		end
		if cooltime then icon.cooldown:SetCooldown(cooltime,cooldur,coolcharges)  end
	end
	if texture then
		icon:Show()
		icon.item = node.item or nil

		if equipslot then -- We need to equip this item to use it
			local curItem = ZGV.ItemScore:GetItemInSlot(equipslot) or 0

			if not icon.curItemInSlot or --nothing set.
			 (icon.curItemInSlot ~= curItem and curItem ~= icon.item)
			 then
				icon.curItemInSlot = curItem
			end

		end
		icon.texture:SetAllPoints(true)
		icon.texture:SetTexture(texture)

		self:ShowNothing()
		local pretext

		if cooltime>0 then
			local time = Pointer.FormatTime(cooltime+cooldur-GetTime())
			pretext = L['pointer_arrow_itemcooldown']:format(time,item and GetItemInfo(item) or GetSpellInfo(spell)) .. "\n"
			--else
			--	pretext = L['pointer_arrow_itemuse']:format(GetItemInfo(item)) .. "\n"
		end

		self:ShowText(pretext or (waypoint.arrowtitle or waypoint.title))
		return true
	end
end

local noskip_time=0

function Pointer.ArrowFrame_OnUpdate_Common(self,elapsed)

	-- NASTY. Replace master object, Indy Jones-style.
	local ArrowFrame = self
	self=Pointer

	local waypoint = ArrowFrame.waypoint

	if not waypoint
	or not ZGV.db.profile.arrowshow
	or not UIParent:IsVisible()
	or PetBattleFrame:IsVisible()
	or (not ZGV.Frame:IsVisible() and profile.hidearrowwithguide and waypoint.type~="manual")
	then
		if not InCombatLockdown() then
			ArrowFrame:Hide()
		end
		if not WorldMapFrame:IsVisible() and self.waypoints and #self.waypoints > 0 and not ZGV.Frame:IsVisible() then self:ClearWaypoints() end -- Hide waypoints and ants if we are not looking at the world map.
		return
	end
	if not InCombatLockdown() then ArrowFrame:Show() end
	--if GetCurrentMapContinentAndZone()~=waypoint.c then end

	cache_throttle = cache_throttle + elapsed
	if cache_throttle > 1.0 then
		cache_throttle = 0
		ZGV:CacheCurrentMapID()
	end


	local safe =  not InCombatLockdown()


	local icon = ArrowFrame.ArrowIcon


	-- adding icons over arrow for different types of teleports
	-- sequential digging in, safe and pretty fast
	--local node = (Pointer.TempWaypath and Pointer.TempWaypath.coords[2]) or (Pointer.pointsets.route and Pointer.pointsets.route.points[2])

	-- Handle spell icons
	if waypoint.pathnode and Pointer.ArrowFrame_ShowSpellArrow(ArrowFrame,waypoint) then
		if MouseIsOver(icon) and IsMouseButtonDown("RightButton") then
			Pointer.ArrowFrame_OnClick(nil,"RightButton") --***Is possible for it to not work if click goes up and down without OnUpdate running***
		end
		return
	else
		if safe then Pointer.ArrowFrame_HideSpellArrow(ArrowFrame) end
	end

	-- normal operation...


	local dist,x,y
	local errortxt
	local cm,cf,cc = ZGV.CurrentMapID,ZGV.CurrentMapFloor,Astrolabe.WorldMapSize[ZGV.CurrentMapID or 0].system --,LibRover.ContinentsByID[ZGV.CurrentMapID]

	--if IsInInstance() and cm~=waypoint.m then ArrowFrame:Hide() return end

	-- If the Minimap is shown, then there is no need for overhead, just take what Astrolabe calculated for us this frame

	--[[
	if GetInstanceInfo()==BZL['Deeprun Tram']
	then -- We're in a mapless place. Tough luck.
		return
	end
	-- Since 5.1.0, Deeprun Tram has a map. Too bad the Ironforge side is not on it. Blizzard, why you so inconsistent!?
	--]]

	if not Astrolabe:GetCurrentPlayerPosition() then
		if GetUnitSpeed("player")>0 then
		-- we're in an unknown location, and moving - our location is totally unknown now. DON'T display distances.
			were_in_unknown_location = true
		end
	else
		were_in_unknown_location = false
	end


	-- Calculate distance, or at least get a fake one

	if Minimap:IsShown() then
		dist,x,y = Astrolabe:GetDistanceToIcon(waypoint.minimapFrame)
	else
		-- When Minimap is hidden, Astrolabe ceases updating, so let us force calculation
		local _x, _y=GetPlayerMapPosition("player")
		local wp=waypoint.minimapFrame.waypoint
		dist, x, y = Astrolabe:ComputeDistance(ZGV.CurrentMapID, ZGV.CurrentMapFloor, _x, _y, wp.m, wp.f, wp.x, wp.y)
		-- Since Astrolabe does not update it when the minimap is hidden, let's do it on our own instead
		waypoint.minimapFrame.dist=dist
	end


	local transcontinental
	if waypoint.c~=cc then
		dist=nil
		transcontinental = true
	end


	if were_in_unknown_location then
		dist=nil
		if WorldMapFrame:IsVisible() then
			errortxt = L['pointer_close_map']
		else
			errortxt = "(current position unknown)"
		end
	end


	dist=dist or 99999999  -- this means FAR or UNKNOWN or whatever. Causes "zone, continent" display.

	-- trigger rover update if we got 100yd away from current target (are we lost?)
	if self.initialdist and ZGV.db.profile.pathfinding then
		self.closingdist = min(self.initialdist,self.closingdist or 9999)
		lastforcedupdate=lastforcedupdate or 0
		if dist-self.closingdist>100 and GetTime()-lastforcedupdate>120 then
			LibRover:UpdateNow(true)
			lastforcedupdate = GetTime()
		end
	end



	local show_stairs

	local samemap = (waypoint.m==cm or phasedMaps[waypoint.m]==phasedMaps[cm])   -- same map, just maybe different phase

	-- TODO: clean this shit up properly. Decide actions first, then strings to display, etc., not randomly like this. ~sinus

	--if samemap or waypoint.c==cc then   -- why check dist here..?
		-- same map, or at least same (sane) continent.
		-- can point, but might be wrong map or floor.
		if samemap and waypoint.f~=cf then
			errortxt = GetPreciseFloorCrossingText(cm,cf,waypoint.f)
			show_stairs = nil --We are trying to enter a cave, point to the location inside the cave

			--TODO show_stairs should show the special arrow for going up and down between floors,
			--as of 12/5/12 it was not working properly for me(Erich) because it was not appearing at all.
			--Instead of showing no arrow, pointing straight toward the location is better.

			--if ZoneIsOutdoor(waypoint.m) then errortxt,show_stairs=nil end  -- Don't warn about floors on outdoor maps. TODO: remove later.
				-- diff floor? prepare to do floor warnings.
		end
		-- Otherwise just point. No funny stuff here.
	--else
	--	errortxt = "far"
	--end

	-- Safety measure, make sure self.badfloortxt is updated to reflect current surroundings
	ArrowFrame.errortxt = errortxt


	-- okay, we're live. 3, 2, 1, action!

	if safe then ArrowFrame:Show() end

	local playerangle = GetPlayerFacing()
	local angle=0

	local going_up
	if errortxt then
		local plusminus,err2=errortxt:match("([+-])(.*)")
		if plusminus then  going_up=(plusminus=="+") and 1 or -1  errortxt=err2  end
	end

	if show_stairs
	then
		-- wrong floor, omg
		if ArrowFrame.ShowStairs then ArrowFrame:ShowStairs(going_up) end

	elseif dist <= (waypoint.radius or self:GetDefaultStepDist())
	and not waypoint.player  -- don't ever "arrive" on player waypoint
	and not (waypoint.pathnode and	(
			(waypoint.pathnode.zone and waypoint.pathnode.zone~=BZR[GetZoneText()])
		or	(waypoint.pathnode.realzone and waypoint.pathnode.realzone~=BZR[GetRealZoneText()])
		or	(waypoint.pathnode.subzone and waypoint.pathnode.subzone~=BZR[GetSubZoneText()])
		or	(waypoint.pathnode.minizone and waypoint.pathnode.minizone~=BZR[GetMinimapZoneText()])
		or	(waypoint.pathnode.indoors and not Astrolabe.minimapOutside)
		-- don't arrive on wrong map zone
							)
		)
	then

		if not waypoint.arrived then
			-- ARRIVED!
			waypoint.arrived = true
		end

		-- remove waypoint when standing on the destination for a bit
		if waypoint.clearonarrival then
			-- clear-on-arrival waypoints? who uses these?
			self.heretime = (self.heretime or 0) + elapsed
			if self.heretime>1 then
				self:RemoveWaypoint(waypoint)
				ZGV:SetWaypoint()
				return
			end
		end
		---------------------------------------------------

		if waypoint.arrived then  -- last sanity check! avoid single-frame "arrived" blinks, when arriving and skipping to next.
			ArrowFrame:ShowArrived()
		end

		-- pick next in path
		if (waypoint.type=="path" and (self.pathfollow=="strict" or self.pathfollow=="pathfind"))
		and not waypoint.noskip
		then
			if GetTime()<pathfindlockout then
				self:Debug("Would skip to next point - but timeout...",pathfindlockout-GetTime())
			else
				local nextway = self:GetNextInPath()
				if nextway and type(nextway)=="table" and nextway~=waypoint then
					self:ShowArrow(nextway)
					pathfindlockout = GetTime()+0.5  -- don't try to pathfind sooner than in 0.5 secs
				end
			end
			--return
			-----------------------------------------------

		elseif waypoint.type=="route" then
			if waypoint.noskip then
				if GetTime()-noskip_time>3 then
					self:Debug("Arrived, but point is route noskip.")
					noskip_time = GetTime()
				end
			else
				-- arrived on a route midpoint - recalculate path immediately. This might take time, so just drop the initial point first.
				local nextway = self:GetNextInPath("route")
				if type(nextway)=="table" then
					waypoint.pathnode.optaway = "midpoint!"		-- Allow hairpin opts to get rid of this if possible when the path is updated. Prevents the arrow from spazzing at cave entrances. 
					self:RemoveWaypoint(waypoint)
					self:ShowArrow(nextway)
					if nextway.pathnode and not nextway.pathnode.dark then
						-- If the next node is dark then updating will cause this node to be lost. Just continue using old path until a non-dark node is encountered.
						self:Debug("Forcing LibRover to update quietly")
						ZGV.LibRover:UpdateNow("quiet")
					end
					--return
				end
			end
			-------------------------------------------------
		end

		-- removing returns; we DO still need to update the text, after all this just handles the icon. ~sinus 2013-02-27

	elseif ArrowFrame.WaitingPhase then

		if ArrowFrame.ShowWaiting then
			ArrowFrame:ShowWaiting(ArrowFrame.WaitingPhase)
		end

	else

		waypoint.arrived = false

		self.heretime=0

		--self.eta:Show()
		--self.dist:Show()


		--[[ angle ]]--

		if waypoint.actionicon then

			if waypoint.actionicon=="upstairs" then ArrowFrame:ShowStairs(true)
			elseif waypoint.actionicon=="downstairs" then ArrowFrame:ShowStairs(false)
			end
			showstairs=true  -- after all!

		else

			angle = Astrolabe:GetDirectionToIcon(waypoint.minimapFrame)
			if not angle or errortxt=="far" then
				angle=3.1415
			else
				--local player = profile.arrowcam and cam_yaw - (is_moving and GetPlayerFacing() or 0) or GetPlayerFacing()
				angle = angle - playerangle
			end
			while angle<0 do angle=angle+6.28319 end

			--angle = angle + 2.356194  -- rad(135)

			if profile.arrowsmooth and self.CurrentArrowSkin and self.CurrentArrowSkin.features.smooth then
				local dif = angle-oldangle
				if dif>0.001 or dif<0.001 then
					while dif>3.14159 do dif=dif-6.28319 end
					while dif<-3.14159 do dif=dif+6.28319 end

					angle = angle-dif/(1+elapsed*20) --speed!

					--local newdif = newangle-oldangle
					--while newdif>3.14159 do newdif=newdif-6.28319 end
					--while newdif<-3.14159 do newdif=newdif+6.28319 end

					--if newdif*dif>0 then  -- no jittering
					--	angle=newangle
					while angle>6.28319 do angle=angle-6.28319 end
					while angle<0 do angle=angle+6.28319 end
					--end
				end
				oldangle=angle
			end

			ArrowFrame:ShowTraveling(elapsed,angle,dist)

		end

		--


		-------------

		--[[
		local perc = mabs(1-angle*0.3183)  -- 1/pi  ;  0=target backwards, 1=target ahead
		local t1,t2,t3,t4,t5 = 0.7,0.75,0.95,1.0
		if perc<t1 then perc=0
		elseif perc<t2 then perc=(t2-perc)/(t2-t1)*0.5
		elseif perc<t3 then perc=0.5
		elseif perc<t4 then perc=(t4-perc)/(t4-t3)*0.5 + 0.5
		else perc=1.0 end

		ArrowFrame:ShowTraveling(elapsed,angle,dist)

		local cell

		local perc = math.abs((math.pi - math.abs(angle)) / math.pi)

		local gr,gg,gb = unpack(TomTom.db.profile.arrow.goodcolor)
		local mr,mg,mb = unpack(TomTom.db.profile.arrow.middlecolor)
		local br,bg,bb = unpack(TomTom.db.profile.arrow.badcolor)
		local r,g,b = ColorGradient(perc, br, bg, bb, mr, mg, mb, gr, gg, gb)
		arrow:SetVertexColor(r,g,b)

		cell = floor(angle / twopi * 108 + 0.5) % 108
		local column = cell % 9
		local row = floor(cell / 9)

		local xstart = (column * 56) / 512
		local ystart = (row * 42) / 512
		local xend = ((column + 1) * 56) / 512
		local yend = ((row + 1) * 42) / 512
		arrow:SetTexCoord(xstart,xend,ystart,yend)
		--]]
	end


	-- labels

	--Pointer:Debug(("dist %.2f  chg %.2f  speed %.2f  ela %.2f"):format(dist,last_distance-dist,speed,eta_elapsed))

	local limit,minlimit=30,5

	eta_elapsed = eta_elapsed + elapsed
	if eta_elapsed >= 0.2 then

		if (self.pathfollow=="smart" or self.pathfollow=="loose" or self.pathfollow=="smart2")
		and waypoint.type=="path" then
			local nextway = self:GetNextInPath()
			if nextway and type(nextway)=="table" and nextway~=waypoint then self:ShowArrow(nextway) end
		end

		speed = (last_distance-dist) / eta_elapsed
		if last_distance == 0 then speed = 0 end
		if last_distance==dist then stoptime=stoptime+eta_elapsed else stoptime=0 end

		--speed=tonumber(("%.2f"):format(speed))
		--ZGV:Print(("dist %.2f  chg %.2f  speed %.2f  thr %.2f"):format(dist,last_distance-dist,speed,eta_elapsed))


		--self:Debug(stoptime)

		if speed>=0 and stoptime<2 then
			table.insert(speeds,1,speed)
			if #speeds>limit then table.remove(speeds) end
		else
			--if stoptime>=10 then
			speed=0
			wipe(speeds)
			--end
		end

		-- Speed meter. Perhaps one day.
		--[[
		profile.arrowshowspeed = true
		if profile.arrowshowspeed then
			local spd
			if profile.arrowmeters then
				spd=("%.02f km/h"):format(speed) --*3.6
			else
				spd=("%.02f mph"):format(speed) --*2.0454
			end
			print(spd)
			self.eta:SetText(spd)
		end
		--]]
		--ZGV:Print(eta_elapsed)


		if ZGV.db.profile.audiocues and IsFlying() then
			local t=GetTime()
			if lastplayerangle~=playerangle then lastturntime=t end
			if GetUnitSpeed("player")==0 then laststoptime=t else lastmovetime=t end
			if t-lastmovetime<=1 and t-laststoptime>3 and t-lastturntime>5 then
				-- if flying, basically.
				-- and beelining for the last 3 seconds.

				-- self:Debug(("will cue; dist=%d initial=%d lastbeep=%d"):format(dist,initialdist,GetTime()-lastbeeptime))
				if dist<=100 and not cuedinged then
					PlaySoundFile("Sound\\Doodad\\BoatDockedWarning.wav")
					-- lastwayding=waypoint  -- DO NOT COMPARE WAYPOINTS. They come from a POOL and are REUSED!
					cuedinged=true
					--self:Debug("dinging")
				else
					--self:Debug("not dinging, dist="..dist..", lastway="..(lastwayding and lastwayding.t or "nil"))
				end
				--self:Debug("cuedinged "..tostring(cuedinged))

				-- warning beeps
				if ArrowFrame.arrow:IsVisible()  then
					local perc = mabs(1-angle*0.3183)  -- 1/pi
					if perc<=0.9 then
						if t-lastbeeptime>2 then
							--PlaySoundFile( [[Sound\Item\Weapons\Ethereal\Ethereal2H3.wav]] )
							PlaySoundFile( [[Sound\Interface\RaidWarning.ogg]] )

							if ArrowFrame.ShowWarning then ArrowFrame:ShowWarning() end

							lastbeeptime=t
						end
					end
				end
			end
			lastplayerangle=playerangle
		end



		last_distance = dist
		eta_elapsed = 0
	end

	--ZGV:Print(table.concat(speeds,"  "))

	etadisp_elapsed = etadisp_elapsed + elapsed
	if etadisp_elapsed >= 0.9 then

		local avg=speed
		for i=2,#speeds do avg=avg+speeds[i] end
		avg=avg/max(#speeds,1)

		--self:Debug("eta: #speeds="..#speeds)
		if #speeds>=minlimit and avg>0 then
			eta = math.abs(dist / avg)
		else
			local spd,mntspd,flyspd,swimspd = GetUnitSpeed("player")
			spd = IsSwimming() and swimspd or (LibRover.maxspeedinzone[ZGV.CurrentMapID][1]*7) -- *7 is for recalc from multiplier to yds/s  -- or max(mntspd,flyspd)
			eta = math.abs(dist / spd)
		end
		etadisp_elapsed = 0
	end

	-- spew it out.

	ArrowFrame:ShowText(
		waypoint.arrowtitle or waypoint.title,
		dist,
		eta,
		errortxt)

end

-- The function! It does nothing!
function Pointer.ArrowFrame_Proto_ShowText(frame)
end

function Pointer.ArrowFrame_Proto_GetFarText(self)
	local way = self.waypoint
	local m = way.m or 0

	local lastm = Astrolabe.LastPlayerPosition and Astrolabe.LastPlayerPosition[1]
	local lastc = Astrolabe.WorldMapSize[lastm].system or 0
	return (Pointer.GetMapNameByID2(way.m,way.f) or ("(bad map #%d)"):format(way.m))
		  .. (way.c and way.c~=lastc and way.c>0 and way.c~=way.m and (", " .. (Pointer.GetMapNameByID2(way.c) or "?")) or "")  -- continent, if applicable
end

function Pointer.ArrowFrame_Proto_GetDistTxt(self,dist)
	if not dist or dist=="far" or (tonumber(dist or 0)>9999998) then return Pointer.ArrowFrame_Proto_GetFarText(self)
	elseif type(dist)=="string" then return dist
	else  return ZGV.FormatDistance(dist)
	end
end

local mfloor=math.floor
function Pointer.FormatTime(eta)
	return ("%01d:%02d|r"):format(eta / 60, eta % 60)
end

function Pointer.ArrowFrame_Proto_GetETATxt(self,eta)
	if eta and eta<7200 and eta>0 then
		local subsec=GetTime()%1
		local etacolor = (eta<10 and GetUnitSpeed("player")>0 and subsec>0.7) and "ffff7700" or "ffffbb00"
		return ("  |c".. etacolor .. Pointer.FormatTime(eta) .. "|r")
	end
end

function Pointer.ArrowFrame_OnShow(frame)
	lastturntime=GetTime()
end

--[[
function Pointer.ArrowFrame_OnMouseDown(frame,button)
	--if button=="RightButton" then
	--	ZGV.Pointer.prev_cameraYawMoveSpeed = GetCVar("cameraYawMoveSpeed")
	--end
end
--]]

function Pointer.ArrowFrame_OnClick(frame,button)
	if ZGV.db.profile.arrowfreeze then return end  -- how did we get the OnClick event, anyway?
	if button=="LeftButton" then
		if not frame.dragging then -- and ZGV.db.profile.pathfinding and ZGV.Pointer.pathfollow=="pathfind" then
			--LibRover:UpdateNow()
			ZGV:SetWaypoint()
		end
	elseif button=="RightButton" then
		Pointer.ArrowFrame_ShowMenu()
	end
end

function Pointer.ArrowFrame_ShowMenu()
	local self=ZGV.Pointer.ArrowFrame
	if not self.menu then self.menu = CreateFrame("FRAME",self:GetName().."Menu",self,"UIDropDownMenuTemplate") end

	UIDropDownMenu_SetAnchor(self.menu, 0, 0, "TOP", self, "BOTTOM")
	--local backdrop = DropDownList1:GetBackdrop()
	--backdrop.edgeSize=16
	--DropDownList1:SetBackdrop(backdrop)
	local menu = {}

	local simpleway = not ZGV.db.profile.pathfinding
	if simpleway then

		tinsert(menu,{
				text = self.waypoint.title,  isTitle = true,notCheckable=true,
			})

		local cont = self.waypoint.c>0 and select(self.waypoint.c,GetMapContinents())
		if cont then tinsert(menu,{
				text = ("%s, %s/%d %.1f;%.1f"):format(cont,Pointer.GetMapNameByID2(self.waypoint.m,self.waypoint.f),self.waypoint.f,self.waypoint.x*100,self.waypoint.y*100),
				isTitle = true, notCheckable=true,
		}) else tinsert(menu,{
				text = ("%.1f;%.1f"):format(self.waypoint.x*100,self.waypoint.y*100),
				isTitle = true, notCheckable=true,
		}) end

		if self.waypoint.type=="manual" then
			tinsert(menu,{
					text = L['pointer_arrowmenu_removeway'],
					func = function() ZGV.Pointer:RemoveWaypoint(self.waypoint) ZGV:SetWaypoint() end,
					notCheckable=true,
				})
		end

		tinsert(menu,{
				text = L['pointer_arrowmenu_route_enable'],
				tooltipTitle = L['pointer_arrowmenu_route_enable'],
				tooltipText = L['pointer_arrowmenu_route_enable_desc'],
				tooltipOnButton=1,
				--checked = function() return ZGV.db.profile.pathfinding end,
				func = function() ZGV:SetOption("Travelsystem","pathfinding on") end,
				notCheckable=1,
				isNotRadio=1,
			})

	else  -- pathfinding

		local list=menu
		--[[
		local list = {}
		tinsert(menu,{
				text = L['pointer_arrowmenu_route'],
				hasArrow=true,
				menuList = list
		})
		--]]
		local route = Pointer.pointsets.route
		if route then  -- no path plotted, but might have been attempted.
			local CPT = ZGV.Pointer.DestinationWaypoint or (route.coords and route.coords[1]) or (route.points and route.points[1])
			tinsert(list,{
				text = L['pointer_arrowmenu_route_destination']:format(CPT and CPT.title or "waypoint",Pointer.GetMapNameByID2(CPT.m or CPT.map,CPT.f),CPT.x*100,CPT.y*100),
				isTitle=true, notCheckable=true,
			})
			local points = route.coords or route.points
			local n=1 --ugly second counter >_<
			for i=2,#points do
				local node=points[i].pathnode
				if not (node.is_arrival and LibRover.cfg.strip_arrivals) then
					local text = (node.text or "?"):gsub("\n","; ")
					if ZGV.DEV then text=text .. " -- " .. node:tostring() end
					tinsert(list,{
						text = L['pointer_arrowmenu_route_node'..(n==1 and '1' or '')]:format(n,text),
						isTitle=true, notCheckable=true,
					})
					n=n+1
				end
			end
			local last=points[#points].pathnode
			tinsert(list,{
				text = L['pointer_arrowmenu_route_est']:format(floor(last.time/60),last.time%60),
				isTitle=true, notCheckable=true,
			})
		end

		if Pointer.DestinationWaypoint then
			if Pointer.DestinationWaypoint.type=="manual" then
				tinsert(menu,{
						text = L['pointer_arrowmenu_removeway'],
						func = function() Pointer:ClearWaypoints("manual")  ZGV:SetWaypoint()  end,
						notCheckable=true,
						isNotRadio=1,
					})
			end
			if self.waypoint.type=="route" and self.waypoint.pathnode.type=="taxi" then
				tinsert(menu,{
						text = L['opt_autotaxi'],
						func = function() ZGV:SetOption("Conv","autotaxi") end,
						checked = function() return ZGV.db.profile.autotaxi end,
						isNotRadio=1,
					})
			end
			tinsert(list,{
					text = L['pointer_arrowmenu_route_retry'],
					tooltipTitle = L['pointer_arrowmenu_route_retry'],
					tooltipText = L['pointer_arrowmenu_route_retry_desc'],
					tooltipOnButton=1,
					notCheckable=true,
					func = function()
						--LibRover:UpdateNow()
						ZGV:SetWaypoint()
					end,
			})
		end

		if ZGV.db.profile.debug and self.waypoint.pathnode then
			tinsert(menu,{
					text = L["Ban this point |cffff0000[DEV]|r"],
					func = function() LibRover.banned_nodes[self.waypoint.pathnode]=1  LibRover:UpdateNow() end,
					notCheckable=1,
					isNotRadio=1,
				})
			tinsert(menu,{
					text = L["Unban all points |cffff0000[DEV]|r"],
					func = function() LibRover.banned_nodes={}  LibRover:UpdateNow() end,
					notCheckable=1,
					isNotRadio=1,
				})
		end

		tinsert(menu,{
				text = L['opt_pathfinding_mode']..":",
				tooltipTitle = L['opt_pathfinding_mode'],
				tooltipText = L['opt_pathfinding_mode_desc'],
				isTitle=1, isNotRadio=1, notCheckable=1,
			})
		for k,v in pairs{"1fastest","2nocd","3lazy"} do
			tinsert(menu,{
					text = L['opt_pathfinding_mode_'..v],
					tooltipTitle = L['opt_pathfinding_mode_'..v],
					tooltipText = L['opt_pathfinding_mode_'..v..'_desc'],
					tooltipOnButton=1,
					checked = function() return ZGV.db.profile.pathfinding_mode==v end,
					func = function() ZGV:SetOption("Travelsystem","pathfinding_mode "..v) end,
				})
		end

		tinsert(menu,{
				text = L['pointer_arrowmenu_route_disable'],
				tooltipTitle = L['pointer_arrowmenu_route_disable'],
				tooltipText = L['pointer_arrowmenu_route_disable_desc'],
				tooltipOnButton=1,
				--checked = function() return ZGV.db.profile.pathfinding end,
				func = function() ZGV:SetOption("Travelsystem","pathfinding off") end,
				notCheckable=1,
				isNotRadio=1,
			})

	end
	--end

	local arrowoptions = {}
	tinsert(menu,{
			text = L['pointer_arrowmenu_arrowheader'],
			hasArrow=true,
			menuList = arrowoptions,
			notCheckable=true,
		})
	tinsert(arrowoptions,{
			text = L['pointer_arrowmenu_hide'],
			tooltipTitle = L['pointer_arrowmenu_hide'],
			tooltipText = L['pointer_arrowmenu_hide_desc'],
			tooltipOnButton=1,
			func = function() ZGV:SetOption("Arrow","arrowshow off") end,
			notCheckable=1,
		})
	tinsert(arrowoptions,{
			text = L['pointer_arrowmenu_freeze'],
			tooltipTitle = L['pointer_arrowmenu_freeze'],
			tooltipText = L['pointer_arrowmenu_freeze_desc'],
			tooltipOnButton=1,
			checked = function() return ZGV.db.profile.arrowfreeze end,
			func = function()  ZGV:SetOption("Arrow","arrowfreeze "..(profile.arrowfreeze and "off" or "on"))  end,
			notCheckable=1,
		})
	tinsert(arrowoptions,{
			text = L['pointer_arrowmenu_opacity'],
			hasArrow = true,
			menuList = {
				{ text = "100%", checked = function() return (profile.arrowalpha>0.95) end, func = ZGV.Pointer.ArrowFrame_SetAlpha, arg1=1 },
				{ text = "75%",  checked = function() return (profile.arrowalpha>0.70 and profile.arrowalpha<0.80) end, func = ZGV.Pointer.ArrowFrame_SetAlpha, arg1=0.75 },
				{ text = "50%",  checked = function() return (profile.arrowalpha>0.45 and profile.arrowalpha<0.55) end, func = ZGV.Pointer.ArrowFrame_SetAlpha, arg1=0.5 },
			},
			notCheckable=1,
		})
	tinsert(arrowoptions,{
			text = L['pointer_arrowmenu_scale'],
			hasArrow = true,
			menuList = {
				{ text = "200%", checked = function() return (profile.arrowscale>1.90) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=2 },
				{ text = "175%", checked = function() return (profile.arrowscale>1.65 and profile.arrowscale<1.85) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=1.75 },
				{ text = "150%", checked = function() return (profile.arrowscale>1.40 and profile.arrowscale<1.60) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=1.50 },
				{ text = "125%", checked = function() return (profile.arrowscale>1.15 and profile.arrowscale<1.35) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=1.25 },
				{ text = "100%", checked = function() return (profile.arrowscale>0.90 and profile.arrowscale<1.10) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=1.00 },
				{ text = "80%",  checked = function() return (profile.arrowscale>0.75 and profile.arrowscale<0.85) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=0.80 },
				{ text = "60%",  checked = function() return (profile.arrowscale>0.55 and profile.arrowscale<0.65) end, func = ZGV.Pointer.ArrowFrame_SetScale, arg1=0.60 },
			},
			notCheckable=1,
		})
	tinsert(arrowoptions,{
			text = L['pointer_arrowmenu_options'],
			func = function() InterfaceOptionsFrame_OpenToCategory(ZGV.optionpanels['arrow']) end,
			notCheckable=1,
		})

	EasyMenu(menu,self.menu,nil,0,0,"MENU",10)
	UIDropDownMenu_SetWidth(self.menu, 300)
end

function Pointer.ArrowFrame_SetAlpha(but,v)
	ZGV:SetOption("Arrow","arrowalpha "..v)
end

function Pointer.ArrowFrame_SetScale(but,v)
	ZGV:SetOption("Arrow","arrowscale "..v)
end

local leftbutdown,downx,downy
local rightbutdown
local old_c,old_z
local zonechangecount=0

local oldzone,zone
function Pointer.Overlay_OnUpdate(frame,but,...)
	-- zone change behaviour is out

	--[[
	local c,z = GetCurrentMapContinentAndZone()
	local zonechanged
	if c~=old_c or z~=old_z then zonechangecount=1 end
	old_c,old_z=c,z
	if zonechangecount>0 then
		if not IsMouseButtonDown("LeftButton") then leftbutdown=false end
		if not IsMouseButtonDown("RightButton") then rightbutdown=false end
		zonechangecount=zonechangecount-1
		return
	end
	--]]

	-- set waypoints by shift-leftclicking the world map

	if IsMouseButtonDown("LeftButton") and (IsShiftKeyDown() or ZGV.db.profile.no_shift_for_waypoints) then
		-- okay, clicked, but aren't we on a waypoint?
--		for w,way in Pointer.waypoints do
--			if
--		end
		leftbutdown=true
		oldzone=GetCurrentMapAreaID()
		downx,downy = GetCursorPosition()
	else
		if leftbutdown then
			leftbutdown=nil
			-- left click

			local map = GetCurrentMapAreaID()
			if map~=oldzone then
				-- map changed with the click! Don't put any notes here.
				return
			end

			local x,y=GetCursorPosition()
			if math.abs(downx-x)>10 or math.abs(downy-y)>10 then return end

			-- these are processed AFTER click procs. Necessary to IGNORE (not DELAY) clicks.
			local foc,foundWF=GetMouseFocus(),nil
			while foc do if foc==WorldMapButton then foundWF=true end foc=foc:GetParent() end
			if not foundWF then return end

			local mapframe = frame:GetParent()

			--ZGV:Print(x.." "..y)
			x=(x-(frame:GetLeft()*frame:GetEffectiveScale()))/(frame:GetWidth()*frame:GetEffectiveScale())
			y=(y-(frame:GetBottom()*frame:GetEffectiveScale()))/(frame:GetHeight()*frame:GetEffectiveScale())
			y=1-y
			--ZGV:Print(x.." "..y)
			if (x>0 and x<1 and y>0 and y<1) then
				--ZGV.Pointer:ClearWaypoints("manual")
				--ZGV.Pointer:SetWaypoint(nil,nil,x*100,y*100,{title=WorldMapFrameAreaLabel:GetText(),type="manual",clearonarrival=true,overworld=true,onminimap="always"})
				local txt = WorldMapFrameAreaLabel:GetText()
				local fmt = ZGV.db.profile.debug and "%s %.1f,%.1f /%d" or "%s %d,%d"

				if map<1 then return end

				--Pointer:ClearWaypoints("manual")
				local way = Pointer:SetWaypoint(nil,nil,x,y,{
					title=(txt and txt.." ("..fmt..")" or fmt):format(Pointer.GetMapNameByID2(map,GetCurrentMapDungeonLevel()),x*100,y*100,GetCurrentMapDungeonLevel()),
					type="manual",
					cleartype=true,
					icon=Pointer.Icons.graydot,
					onminimap="always",
					overworld=true,
					showonedge=true,
					findpath=true
				})

				-- put coords in chat editbox
				if ChatFrame1EditBox:HasFocus() then
					ChatFrame1EditBox:SetText(ChatFrame1EditBox:GetText()..("%s/%s %.1f,%.1f"):format(Pointer.GetMapNameByID2(oldzone),GetCurrentMapDungeonLevel(),x*100,y*100))
					ChatFrame1EditBox:SetFocus(true)
				end
			end
		end
	end
end

local function GenerateSubmapCache()
	if not submap_cache then submap_cache={} end
	local system
	-- FIXME that's ugly
	for i=0,999 do
		system=Astrolabe:GetMapInfo(i,0)
		if system then -- make Astrolabe do the research if such a map exists
			if not submap_cache[system] then submap_cache[system]={} end
			table.insert(submap_cache[system],i)
		end
	end
end

local function RestartCorpseSearch(where) -- made it a func not to duplicate code
	-- WTF, no corpse?
	Pointer:Debug("SetCorpseArrow, corpse not found anywhere [%s], will retry",where)
	ZGV:ScheduleTimer(function() ZGV.Pointer:SetCorpseArrow(true) end,1.0)
end

function Pointer:SetCorpseArrow(reset)

	if self.corpsearrow and not reset then return end
	if not UnitIsDeadOrGhost("player") then Pointer:Debug("Pointer.SetCorpseArrow: not dead!") return end

	local x=0
	local y=0

	local origm,origf = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	if not origm or origm <= 0 then
		RestartCorpseSearch(2)
		return
	end
	local origx,origy=GetCorpseMapPosition()
	-- some magic here...

	Pointer:Debug("SetCorpseArrow, current map %d/%d, corpse %.1f,%.1f",origm,origf,origx*100,origy*100)

	-- If submap cache is not ready, recreate it
	if not submap_cache then
		GenerateSubmapCache()
		RestartCorpseSearch(1) return -- FIXME temporary workaround the bug giving the body in Dalaran Crater
	end

	-- We probably want to put a dot on all maps where the corpse is seen, just to look consistent
	local mapcandidates = {}

	-- Locating the player on the parent level map
	local system,_,_,_,_,_=Astrolabe:GetMapInfo(origm,origf)
	if not system then RestartCorpseSearch(2) return end

	SetMapByID(system)
	--SetDungeonMapLevel(0) -- sanity
	x,y=GetCorpseMapPosition()
	if x>0 then
		Pointer:Debug("SetCorpseArrow, Corpse found in system %s (%s) at %.1f,%.1f",system,Pointer.GetMapNameByID2(system),x*100,y*100)
		Pointer:Debug("SetCorpseArrow, Searching the best fit among its "..#submap_cache[system].." maps")

		-- Right, so what our criteries are in the order of descending importance
		-- • The body is seen within the map
		-- • Map is small(for example Darnassus in Teldrassil)
		local smallestarea,bestmap,bestfloor,bestx,besty
		for i,id in ipairs(submap_cache[system]) do -- Also each map includes self in the list
			SetMapByID(id)
			local _x, _y
			local levels = GetNumDungeonMapLevels()
			for l=levels>0 and 1 or 0,levels do -- perusing the current map dungeon levels  --sinus: 1..max   or just 0
				SetDungeonMapLevel(l)
				_x,_y = GetCorpseMapPosition()
				if _x>0 -- aha, we can see the body on that map, let's note it
				and (id==origm or _x~=origx) -- AND it's not the SAME position that we saw on "our" map!!!! IMPORTANT!!!   /run SetMapByID(666) print(GetCorpseMapPosition())   you'll see magic happen, it'll return CURRENT map's corpse positions
				then
					table.insert(mapcandidates,{mapid=id,floor=l,x=_x,y=_y})
					local _,_,_w,_h,_,_=Astrolabe:GetMapInfo(id,l)
					if _w then
						Pointer:Debug("Corpse Candidate on map: %d/%d %.1f,%.1f (size:%d)",id,l,_x*100,_y*100,_w)
						if not bestmap or smallestarea>_w then
							smallestarea=_w
							bestmap,bestfloor,bestx,besty=id,l,_x,_y
							Pointer:Debug("^--- best so far")
						end
					end
				end
			end
		end


		if #mapcandidates==0 then   -- -- sinus>alex: #table can't be <0, can it..?
			RestartCorpseSearch("no corpses found on maps")
		else
			-- Now we have a list of maps where the body is seen and the id of the smallest one
			self:ClearWaypoints("corpse")

			Pointer:Debug("SetCorpseArrow: *** Pointing to corpse at: |cffffffff%s %d/%d %.1f,%.1f",Pointer.GetMapNameByID2(bestmap),bestmap,bestfloor,bestx*100,besty*100)
			local way = Pointer:SetWaypoint(bestmap,bestfloor,bestx,besty,{
				title= ZGV.db.profile.corpsearrowjokes and L["pointer_corpselabel"..math.random(L["pointer_corpselabel#"])] or L["pointer_corpselabel"],
				type="corpse",
				icon=Pointer.Icons.greendot,
				onminimap="always",
				overworld=true,
				showonedge=true,
			})

			--if way then Pointer:FindTravelPath(way) end  -- DON'T Travel.

			self.corpsearrow=true

		end
	else
		RestartCorpseSearch("not in system")
	end
		--[[
		if not m then
			-- failed! set a flag
			self.corpsewait=true
		end
		--]]

	-- Clean up
	SetMapByID(origm)
	SetDungeonMapLevel(origf)
end


Pointer.ArrowSkins = {}

local function _new_skin(id,name)
	local skin = {id=id or "skin", name=name or "unnamed skin",styles={} }
	return skin
end

function Pointer:AddArrowSkin(id,name)
	local skin = _new_skin(id,name)
	self.ArrowSkins[id] = skin
	return skin
end

function Pointer:GetSkinPath(skin)
	if not skin then skin=ZGV.db.options.arrowskin end
	return ZGV.DIR .. "\\Arrows\\" .. skin .. "\\"
end

function Pointer:SetArrowSkin(skin)
	-- no style? grab default
	if not skin then
		skin = next(self.ArrowSkins)
		assert(skin,"No arrow skin to fall back to!")
		return self:SetArrowSkin(skin)
	end
	local skindata = self.ArrowSkins[skin]
	if not skindata then
		ZGV:Print("Unknown arrow skin '"..skin.."', falling back to default")
		return self:SetArrowSkin()
	end

	local way
	if self.CurrentArrowSkin then
		way = self.ArrowFrame.waypoint
		self.ArrowFrame:Hide()
	end

	ZGV.db.profile.arrowskin = skin

	self.CurrentArrowSkin = skindata
	self.ArrowSkinDir = self:GetSkinPath(skin,style)

	self:CreateArrowFrame()

	self.ArrowFrame.waypoint = way
end

function Pointer:UpdateArrowVisibility()
	if not self.ArrowFrame then return end
	if ZGV.db.profile.arrowshow then self.ArrowFrame:Show() else self.ArrowFrame:Hide() end
end

function Pointer:SetMinimapPlayerTexture()
	Minimap:SetPlayerTexture(ZGV.DIR.."\\skins\\minimaparrow-player")
end

function Pointer:PointToNextInPath(pathname,pathfollow)
	if pathname=="farm" then
		self.pathfollow=pathfollow or "smart"
		local way = self:GetNextInPath("farm")
		if type(way)=="table" then  -- OK, waypoint
			self:Debug("PointToNext: #%d '%s' %.1f,%.1f",way.num,way.title or "(untitled)",way.x,way.y)
			self:ShowArrow(way)
		else
			self:Debug("PointToNext: to %s ?",tostring(way))
		end
	end
end

-- find the next point for the current position.
-- Can it base on the current point..? Doubtful, it can run continuously, but perhaps it can be safeguarded.
-- return true  to stay on the current point.
local smart_path_dist = 50

function Pointer:GetNextInPath(pathname,testway)
	if self.pathfollow == "smart" then

		--[[ let's try a completely different approach

		-- establish nearest point
		local neardist=9999999999
		local nearnum
		for w,way in ipairs(self.waypoints) do  if way.type=="path" then
			if way.minimapFrame and way.minimapFrame.dist<neardist then
				neardist=way.minimapFrame.dist
				nearnum=w
			end
		end  end
		local nearway=self.waypoints[nearnum]

		-- establish next point
		local nextnum=nearnum+1
		if nextnum>#self.waypoints then
			if self.pathloop then nextnum=1 else return true end
		end
		local nextway=self.waypoints[nextnum]

		-- if we're "on" the current point, give the next.
		if neardist<smart_path_dist then return nextway end -----------------------

		local nextdist = nextway.minimapFrame.dist
		local nearnextdist = Astrolabe:ComputeDistance(nearway.m,nearway.f,nearway.x,nearway.y,nextway.m,nextway.f,nextway.x,nextway.y)
		if not nearnextdist then
			return nearway
		else

			--[=[
			-- if we ever need angles here...

			-- GetDist:
				local dist,xd,yd = Astrolabe:ComputeDistance(node1.m,node1.f,node1.x,node1.y,node2.m,node2.f,node2.x,node2.y)
				if dist==0 and node1.c~=node2.c or (node1.c==node2.c and node1.c==-1 and node1.m~=node2.m) then dist=nil end   -- the latter condition shouldn't matter anymore, since we moved to Astrolabe systems instead of continents
				return dist or 99999999,xd,yd
			--

			local dist,xd,yd = GetDist(self,node2)
			if not xd then return end
			local dir = atan2(xd, -yd)
			return dir%360
			--]=]

			-- fun part. Are we near the straight line..?
			if neardist+nextdist < nearnextdist*1.2 then return nextway else return nearway end
		end
		--]]



		--[[
		local tempway = self.tempway
		if not tempway then
			local m,f = ZGV.CurrentMapID,ZGV.CurrentMapFloor
			if testway then m,f=testway.m,testway.f end
			tempway = self:SetWaypoint(m,f,0,0,{title="Follow farm path",overworld=true,type="path"})
			self.tempway = tempway
		end
		--]]

		-- establish least pulled segment
		local min_pull=9999
		local min_pull_w,min_pull_nextw=0,0
		local good_next_way

		local waypoints = self.pointsets['farm'] and self.pointsets['farm'].points
		if not waypoints then return true end

		for w,way in ipairs(waypoints) do  if way.type=="path" and way.minimapFrame then
			local dist = way.minimapFrame.dist
			if testway then dist=Astrolabe:ComputeDistance(way.m,way.f,way.x,way.y,testway.m,testway.f,testway.x,testway.y) end
			local nextw = w+1
			if nextw>#waypoints then
				if not self.pathloop then  break  else  nextw=1  end
			end
			local nextway = waypoints[nextw]
			local nextdist = nextway.minimapFrame.dist
			if testway then nextdist=Astrolabe:ComputeDistance(nextway.m,nextway.f,nextway.x,nextway.y,testway.m,testway.f,testway.x,testway.y) end

			local dist_between = Astrolabe:ComputeDistance(way.m,way.f,way.x,way.y,nextway.m,nextway.f,nextway.x,nextway.y)

			-- don't bother with segments too short
			if dist_between>smart_path_dist*0.5 then
				--local pull = (dist+nextdist) / dist_between
				local pull = (dist+nextdist) - dist_between

				if dist<smart_path_dist then pull=0 end  -- perfect pull at start point of each segment!
				if nextdist<smart_path_dist then pull=2 end  -- lousy pull at end point

				if (pull<min_pull) or (pull-min_pull<0.2) then
					min_pull = pull
					min_pull_w,min_pull_nextw = w,nextw
					good_next_way = nextway
				end
				--print(("%d-%d: %d + %d / %d = %.2f"):format(w,nextw,dist,nextdist,dist_between,pull))
			end
		end  end

		-- how far are we along the line?
		-- perhaps later.
		--[[
		do
			local way1,way2 = waypoints[min_pull_w],waypoints[min_pull_nextw]

			local dist1 = way1.minimapFrame.dist
			local dist2 = way2.minimapFrame.dist
			if testway then dist1=Astrolabe:ComputeDistance(way1.m,way1.f,way1.x,way1.y,testway.m,testway.f,testway.x,testway.y) end
			if testway then dist2=Astrolabe:ComputeDistance(way2.m,way2.f,way2.x,way2.y,testway.m,testway.f,testway.x,testway.y) end
			local dist_between = Astrolabe:ComputeDistance(way1.m,way1.f,way1.x,way1.y,way2.m,way2.f,way2.x,way2.y)

			local pull2 = (dist+nextdist) / dist_between  -- 1..inf
			local along = .........
		end

		local progress_band = 500
		local progress = 1 - (min(progress_band,pull)/progress_band)
		tempway.x = way.x + (nextway.x-way.x)*progress
		tempway.y = way.y + (nextway.y-way.y)*progress


		tempway:SetIcon(tempway.icon)
		return tempway
		--]]

		return good_next_way

	elseif self.pathfollow == "smart2" then
		-- broken with ants
		local neardist=9999999999
		local nears={}
		local nearnum
		for w,way in ipairs(self.waypoints) do  if way.minimapFrame and way.minimapFrame.dist<neardist then  neardist=way.minimapFrame.dist  nearnum=w  end  end
		tinsert(nears,nearnum)
		repeat
			local neardistmin=neardist
			neardist=neardist*1.1
			for w,way in ipairs(self.waypoints) do  if way.minimapFrame and way.minimapFrame.dist>=neardistmin and way.minimapFrame.dist<neardist then  tinsert(nears,w)  end  end
		until #nears>=3
		Pointer:Debug("nears",unpack(nears))

		local minnum = min(unpack(nears))
		local maxnum = max(unpack(nears))
		local size = #self.waypoints
		local halfway = floor(size/2)
		if maxnum-minnum>halfway then -- first and last choice span more than half the path?
			--for i in ipairs(nears) do  if way.minimapFrame and way.minimapFrame.dist<neardist then  neardist=way.minimapFrame.dist  nearnum=w  end  end
			for i=1,#nears do nears[i]=nears[i]-halfway  if nears[i]<0 then nears[i]=nears[i]+size end  end
			maxnum = max(unpack(nears))
			Pointer:Debug("nears fix",unpack(nears))
		end
		Pointer:Debug("maxnum",maxnum)

		return self.waypoints[maxnum]
	elseif self.pathfollow == "strict"  then
		local nextnum=self.ArrowFrame.waypoint and (self.ArrowFrame.waypoint.num+1) or 1
		if nextnum>#self.waypoints or self.waypoints[nextnum].type=="ant" then
			if self.pathloop then
				for w,way in ipairs(self.waypoints) do if way.type=="path" then nextnum=w break end end
			else
				return
			end
		end
		return self.waypoints[nextnum]
	elseif self.pathfollow == "loose" then
		local curnum
		for w,way in ipairs(self.waypoints) do
			if way.type=="path" then
				local mf = way.minimapFrame
				local dist = mf and mf.dist or 9999
				if dist<=(way.radius or Pointer:GetDefaultStepDist()) then
					curnum=w
				end
			end
		end
		if not curnum then return self.ArrowFrame.waypoint end  -- current stays.

		local nextnum = curnum+1
		if nextnum>#self.waypoints or self.waypoints[nextnum].type~="path" then
			if self.pathloop then
				for w,way in ipairs(self.waypoints) do if way.type=="path" then nextnum=w break end end
			else
				return
			end
		end
		return self.waypoints[nextnum]
	elseif self.pathfollow == "route" or pathname=="route" then
		if self.pointsets.route then
			if #self.pointsets.route.points>=3 then return self.pointsets.route.points[3] -- 1 is current position, 2 is currently next point, so 3 is proper "next".
			else return true -- keep!
			end
		else return true end
		--LibRover.updating = true
		--LibRover:UpdateNow()
		--ZGV:SetWaypoint() -- UGLY. TODO. Clears manual path, that's bad.
	end
end

function Pointer:TestGetNextInPathOne(x,y)
	local way = Pointer:SetWaypoint(GetCurrentMapAreaID(),GetCurrentMapDungeonLevel(),x,y,{
					type="manual",
					icon=Pointer.Icons.arrow,
					onminimap="always",
					overworld=true,
					showonedge=false,
					angle=0
				})
	self.pathfollow="smart"
	local nextway = self:GetNextInPath("farm",way)

	if not nextway or type(nextway)~="table" then return end

	way.angle = math.atan2(nextway.x-x,(y-nextway.y)*0.66)
	if way.angle>0 then way.angle=6.2831-way.angle else way.angle=-way.angle end
	way.title = "To "..nextway.title
	way:SetIcon(way.icon)
end

function Pointer:TestGetNextInPath(n)
	self:ClearWaypoints("manual")
	for x=0,1,1/n do  for y=0,1,1/n/0.66 do
		self:TestGetNextInPathOne(x,y)
	end end
end

--[[
function Pointer.GetMapName(id)
	local map,floor = Pointer.UnwrapMapFloor(mapfloor)
	local mapname = GetMapNameByID(map)
end
-- TODO later.
--]]

-- store continent numbers, for zone continent comparison later.
local conts = {GetMapContinents()}
local extramaps = {[-1]="Azeroth",[0]="Azeroth",[13]=conts[1],[14]=conts[2],[466]=conts[3],[485]=conts[4],[751]=conts[5],[862]=conts[6],[906]="Theramore"}
function Pointer.GetMapNameByID2(id,floor)
	if floor then
		floor = ZGV:SanitizeMapFloor(id,floor)
		local mapdata = Astrolabe.WorldMapSize[id]
		mapdata = mapdata and mapdata[floor]
		if mapdata.floorName and mapdata.floorName~=0 then return mapdata.floorName
		elseif mapdata.microName and mapdata.microName~=0 then return mapdata.microName  end
	end
	return GetMapNameByID(id or 0) or extramaps[id] or "(map "..(id or 0)..")"
end


local flash_interval=0.25

local ant_interval=0.001
local ant_speed = 1.7  -- ant steps per second

local flash=nil
function Pointer:MinimapNodeFlash(s)
	flash=not flash
	Minimap:SetBlipTexture(ZGV.DIR.."\\Skins\\objecticons_"..(flash and "on" or "off"))
end
function Pointer:MinimapNodeFlashOff()
	Minimap:SetBlipTexture("INTERFACE\\MINIMAP\\OBJECTICONS")
end

local q=0

do
	local F = CreateFrame("FRAME","ZGVPointerExtraFrame")
	local ant_last=GetTime()
	local flash_last=GetTime()
	F:SetScript("OnUpdate",function(self,elapsed)
		if not ZGV.db then error("WTF? No ZGV.db when there's already OnUpdates running!?") end
		local t=GetTime()
		
		-- ant_last and flash_last need to be increments of their respective intervals

		ant_interval = (ZGV.db.profile.antspeed>900) and 0.001 or (ZGV.db.profile.antspeed==0) and 999 or (1/ZGV.db.profile.antspeed)

		if t-ant_last>=ant_interval then
			if ZGV.db.profile.waypointaddon=="internal" then
				ZGV.Pointer:AnimateAnts()
			end
			ant_last=t-(t-ant_last)%ant_interval  -- make sure ant_last advances in exactly ant_interval increments.
		end

		-- Flashing node dots. Prettier than the standard, too. And slightly bigger.  Also, s/ode do/ude ti/.
		if ZGV.db.profile.flashmapnodes then
			if t-flash_last>=flash_interval then
				ZGV.Pointer:MinimapNodeFlash()
				flash_last=t-(t-flash_last)%flash_interval
			end
		end
	end)
	
	local CHAIN = ZGV.ChainCall
	F:SetPoint("CENTER",UIParent)
	F:Show()

	-- these make sure the flashing dots don't blink-glitch when their texture changes.
	CHAIN(F:CreateTexture("ZGVPointerDotOn","OVERLAY")) :SetTexture(ZGV.DIR.."\\Skins\\objecticons_on") :SetSize(50,50) :SetPoint("CENTER") :SetNonBlocking(true) :Show()
	CHAIN(F:CreateTexture("ZGVPointerDotOff","OVERLAY")) :SetTexture(ZGV.DIR.."\\Skins\\objecticons_off") :SetSize(50,50) :SetPoint("RIGHT") :SetNonBlocking(true) :Show()
end




-- Some small utilities which may be useful to several waypointing backends
-- Moved 'em out of Internal waypointer so that TomTom, for example,
-- may equally enjoy the pleasures of nettles^W path-based navigation ~aprotas

local curve_spacing = 200  -- overwritten with antspacing from options anyway
Pointer.curve_spacing = 200
local max_ants_per_segment = 40

local function calc_catmull_rom(t,t2,t3,p0,p1,p2,p3)
	return 0.5 * ( (2*p1.gx) + (-p0.gx+p2.gx)*t + (2*p0.gx-5*p1.gx+4*p2.gx-p3.gx) * t2 + (-p0.gx+3*p1.gx-3*p2.gx+p3.gx) * t3),
		   0.5 * ( (2*p1.gy) + (-p0.gy+p2.gy)*t + (2*p0.gy-5*p1.gy+4*p2.gy-p3.gy) * t2 + (-p0.gy+3*p1.gy-3*p2.gy+p3.gy) * t3)
	-- kept separate just in case. Inlined below for optimization.
end
Pointer.calc_catmull_rom=calc_catmull_rom

local function calc_angles(points,do_loop,recalc)
	local atan2=math.atan2
	for k,point in ipairs(points) do
		if recalc then point.angle=nil end
		if not point.angle and point.gx then
			local nextpoint = points[k+1]
			if not nextpoint then
				if do_loop then nextpoint=points[1] else break end
			end
			if nextpoint and nextpoint.gm==point.gm and nextpoint.gx then
				local angle = atan2(nextpoint.gx-point.gx,(point.gy-nextpoint.gy)*0.66)
				if angle>0 then angle=6.2831-angle else angle=-angle end
				point.angle = angle
			end
		end
	end
end
Pointer.calc_angles=calc_angles

local function calc_angles_curved(points,do_loop,recalc)
	local atan2=math.atan2
	for i=1,#points do
		local point=points[i]
		if recalc then point.angle=nil end
		if not point.angle and point.gx then
			local p0i,p1i,p2i,p3i = i-1,i,i+1,i+2
			if p0i<1 then p0i = do_loop and p0i+#points or 1 end
			if p2i>#points then p2i = do_loop and p2i-#points or #points end
			if p3i>#points then p3i = do_loop and p3i-#points or #points end
			local p0,p1,p2,p3=points[p0i],points[p1i],points[p2i],points[p3i]
			local x,y = calc_catmull_rom(0.2,0.04,0.008,p0,p1,p2,p3)
			if x then
				local angle = atan2(x-point.gx,(point.gy-y)*0.66)
				if angle>0 then angle=6.2831-angle else angle=-angle end
				point.angle = angle
			end
		end
	end
end
Pointer.calc_angles_curved=calc_angles_curved



-- optimization madness: localize EVERYTHING

local antpoints = {}
Pointer.antpoints = antpoints

local def_ant_icon = ZGV.Pointer.Icons.ant

local function spawn_curve_ants(points,loop,phase)
	if #points<3 then return end
	--print("curving!!")
	local abs=abs
	local ceil=ceil

	local antpoints_num = 0

	local np=#points
	for i=1,np do  while true do
		--tinsert(antpoints,points[i])
		local p0i,p1i,p2i,p3i = i-1,i,i+1,i+2
		if p0i<1 then p0i = loop and p0i+#points or 1 end
		if p2i>#points then p2i = loop and p2i-#points or #points end
		if p3i>#points then p3i = loop and p3i-#points or #points end

		local p0,p1,p2,p3=points[p0i],points[p1i],points[p2i],points[p3i]

		local p0gx,p0gy,p1gx,p1gy,p2gx,p2gy,p3gx,p3gy=p0.gx,p0.gy,p1.gx,p1.gy,p2.gx,p2.gy,p3.gx,p3.gy
		if not p0gx or not p1gx or not p2gx or not p3gx then break end

		local curve_accuracy = p1.curve_accuracy
		if not curve_accuracy then
			--local dist = Astrolabe:ComputeDistance(p1.map,p1.floor,p1.x/100,p1.y/100,p2.map,p2.floor,p2.x/100,p2.y/100)   -- Astrolabe thinks x and y are 0..1, and they're 0..100 here. Results will be valid, though exaggerated.
			local dist
			if false then -- use straight dist
				dist = Astrolabe:ComputeDistance(p1.gm,p1.gf,p1.gx,p1.gy,p2.gm,p2.gf,p2.gx,p2.gy)   -- Astrolabe thinks x and y are 0..1, and they're 0..100 here. Results will be valid, though exaggerated.
			else
				dist=0
				local lx,ly=p1gx,p1gy
				for t=0.1,1,0.1 do
					local t2 = t*t
					local t3 = t*t*t
					local x = 0.5 * ( (2*p1gx) + (-p0gx+p2gx)*t + (2*p0gx-5*p1gx+4*p2gx-p3gx) * t2 + (-p0gx+3*p1gx-3*p2gx+p3gx) * t3)
					local y = 0.5 * ( (2*p1gy) + (-p0gy+p2gy)*t + (2*p0gy-5*p1gy+4*p2gy-p3gy) * t2 + (-p0gy+3*p1gy-3*p2gy+p3gy) * t3)
					dist=dist+(x-lx)*(x-lx)+(y-ly)*(y-ly)*0.66
					lx,ly=x,y
				end
				dist=sqrt(dist)*Astrolabe.WorldMapSize[p1.gm].width*3
			end
			if not dist or dist<1 then dist=1 end
			curve_accuracy = ceil(dist/curve_spacing)
			if curve_accuracy>max_ants_per_segment then curve_accuracy=max_ants_per_segment end
			curve_accuracy = 1/curve_accuracy
			p1.curve_accuracy = curve_accuracy
		end

		--print("acc",curve_accuracy)
		--for t=phase*curve_accuracy,1-(1-phase)*curve_accuracy,curve_accuracy*0.999 do
		for t=phase*curve_accuracy,0.9999,curve_accuracy*0.999 do
			local t2 = t*t
			local t3 = t*t*t

			-- Catmull-Rom
			--local x,y = calc_catmull_rom(t,t2,t3,p0,p1,p2,p3)
			local x = 0.5 * ( (2*p1gx) + (-p0gx+p2gx)*t + (2*p0gx-5*p1gx+4*p2gx-p3gx) * t2 + (-p0gx+3*p1gx-3*p2gx+p3gx) * t3)
			local y = 0.5 * ( (2*p1gy) + (-p0gy+p2gy)*t + (2*p0gy-5*p1gy+4*p2gy-p3gy) * t2 + (-p0gy+3*p1gy-3*p2gy+p3gy) * t3)

			--local x = 0.5 * ( (2*p1.gx) + (-p0.x+p2.x)*t + (2*p0.x-5*p1.x+4*p2.x-p3.x) * t2 + (-p0.x+3*p1.x-3*p2.x+p3.x) * t3)
			--local y = 0.5 * ( (2*p1.gy) + (-p0.y+p2.y)*t + (2*p0.y-5*p1.y+4*p2.y-p3.y) * t2 + (-p0.y+3*p1.y-3*p2.y+p3.y) * t3)

			--if (abs(x-p1.x)+abs(y-p1.y)>0.1) and (abs(x-p2.x)+abs(y-p2.y)>0.1) then

			antpoints_num = antpoints_num+1
			local ant = antpoints[antpoints_num]
			if not ant then
				ant = {}
				antpoints[antpoints_num]=ant
			end

			ant.map,ant.floor,ant.x,ant.y=p1.gm,p1.gf,x,y
			ant.sub=i+t
			ant.icon = p2.ant_icon or def_ant_icon
			ant.p1f,ant.p2f=p1.f,p2.f
			ant.p1m,ant.p2m=p1.m,p2.m
			ant.ant_dist=t

			--print(("%d/%.2f: [%.1f,%.1f]->[%.1f,%.1f] = [%.1f,%.1f]"):format(i,t,p1.x,p1.y,p2.x,p2.y,x,y))
			--end
		end
		break
	end end
	return antpoints,antpoints_num
end
Pointer.spawn_curve_ants = spawn_curve_ants


local function spawn_straight_ants(points,loop,phase)
	if #points<2 then return end
	--print("curving!!")
	local abs=abs
	local ceil=ceil

	local antpoints_num = 0

	local np=#points
	local breakall
	for i=1,np do  while true do
		local p1 = points[i]
		local p2 = points[i+1]
		if not p2 then
			if loop then p2=points[1] else breakall=true break end
		end

		local base_t=0

		-- NEW CHECK. Points are supposedly on global maps. If points do NOT share a global map, NO ANTS BETWEEN THEM.
		if p1.gm
		and p1.gm==p2.gm
		--and p1.gf==p2.gf
		then

			local curve_accuracy = p1.curve_accuracy
			if not curve_accuracy then
				--local dist = Astrolabe:ComputeDistance(p1.map,p1.floor,p1.x/100,p1.y/100,p2.map,p2.floor,p2.x/100,p2.y/100)   -- Astrolabe thinks x and y are 0..1, and they're 0..100 here. Results will be valid, though exaggerated.
				local dist = Astrolabe:ComputeDistance(p1.gm,p1.gf,p1.gx,p1.gy,p2.gm,p2.gf,p2.gx,p2.gy)   -- Astrolabe thinks x and y are 0..1, and they're 0..100 here. Results will be valid, though exaggerated.
				if not dist or dist<1 then dist=1 end  -- correct for the above exaggeration
				curve_accuracy = ceil(dist/curve_spacing)
				--curve_accuracy = dist/curve_spacing
				if curve_accuracy>max_ants_per_segment then curve_accuracy=max_ants_per_segment end
				curve_accuracy = 1/curve_accuracy
				p1.curve_accuracy = curve_accuracy
			end

			--print("acc",curve_accuracy)
			for t=base_t+phase*curve_accuracy,1,curve_accuracy*0.999 do
				-- straight line
				local x = p1.gx + t*(p2.gx-p1.gx)
				local y = p1.gy + t*(p2.gy-p1.gy)

				antpoints_num = antpoints_num+1
				local ant = antpoints[antpoints_num]
				if not ant then
					ant = {}
					antpoints[antpoints_num]=ant
				end

				ant.map,ant.floor,ant.x,ant.y=p1.gm,p1.gf,x,y
				ant.sub=i+t
				ant.icon = p2.ant_icon or def_ant_icon
				ant.p1f,ant.p2f=p1.f,p2.f
				ant.p1m,ant.p2m=p1.m,p2.m
				ant.ant_dist=t

				--base_t=t

				--tinsert(antpoints,{map=0,floor=0,x=x,y=y,sub=i+t,icon=ZGV.Pointer.Icons.ant})
			end

			--base_t=(base_t+curve_accuracy) % 1
		end
		break
	end  if breakall then break end  end
	return antpoints,antpoints_num
end
Pointer.spawn_straight_ants = spawn_straight_ants


local function update_ant_waypoints(points)  -- optimized for ants
	for k,point in ipairs(points) do
		local way = point.pointer_way
		if way then
			way.map,way.floor,way.x,way.y=point.map,point.floor,point.x,point.y
		end
	end
end

local temp_setwaypoint_data={}
local function set_waypoints(points,worldsize,minisize,type,setname)
	local arrowpoint,farmpoint
	local pathmode = (type=="path")
	local farmmode = (type=="farm")

	local step = ZGV.CurrentStep

	Pointer:Debug("set_waypoints: type %s",type)

	for k,point in ipairs(points) do
		if not point.force_noway and point.x then

			local data = temp_setwaypoint_data
			wipe(data)
			data.overworld = not farmmode
			data.onminimap=not farmmode and "always"
			data.showonedge=not pathmode and not farmmode
			data.type=type
			data.in_set = setname


			--data.persistent=true
			data.title = (title
					or point.waytitle or point.title
					or (pathmode and (step and step.waypath and step.waypath.title and step.waypath.title or "Path").." ("..k..")")
					or (step and step:GetWayTitle())
					or (point.map and point.x and ("%s %d,%d"):format(ZGV.Pointer.GetMapNameByID2(point.map),point.x*100,point.y*100))
					or L['waypoint_step']:format(ZGV.CurrentStepNum)
					)
			data.num=k

			-- Copy point vars to waypoint
			for k,v in pairs(point) do if k~="map" and k~="floor" and k~="x" and k~="y" then data[k]=v end end
			if data.player then data.passive = true end

			local way = Pointer:SetWaypoint (point.map,point.floor,point.x,point.y, data, false)
			--Pointer:Debug("set_waypoints: point %d/%d, way=%s",k,#points,tostring(way))

			point.pointer_way=way

			if way then

				if not point.player or point.arrow or type=="manual" then
					arrowpoint = arrowpoint or way  ---or way.surrogate_for or way
					farmpoint = arrowpoint or way
					--pathpoint = pathpoint or way  -- in case they ARE different somehow
				end

				-- or (step.waypath and step.waypath.current==point)

				if setname then  -- add to set
					local set = Pointer.pointsets[setname]
					if not set then set={points={}} __CLASS[set]="PointSet" Pointer.pointsets[setname]=set end
					tinsert(set.points,way)
					--Pointer:Debug("Added point to set %s, now storing %d",setname,#set.points)
				end
			else
				--self:Print(("Unabmle to create waypoint: %s/%d %.2f %.2f"):format(point.map,point.floor,point.x,point.y))
			end
		end
	end
	Pointer:Debug("set_waypoints end")

	return arrowpoint,farmpoint
end
Pointer.set_waypoints = set_waypoints

local function set_waypoints_ants(ants,num,start_at,worldsize,minisize)
	for k=1,num do
		local ant=ants[k]
		Pointer:SetWaypoint_ant (ant.map,ant.floor,ant.x,ant.y, k+start_at, ant.icon, ant)
	end
end

local function move_point_to_global(point)
	if not point then return end
	local m = point.m	if not m then return end
	local data = Astrolabe.WorldMapSize[m]
	local mastermap = data.systemParent or 0
	local masterfloor = ZGV:SanitizeMapFloor(mastermap,0) -- even if it's the same map, remap to be able to work with bare coords

	--if Astrolabe.WorldMapSize[point.map].system==466 then mastermap=466 end  -- outland, do NOT translate onto Azeroth
	--if Astrolabe.WorldMapSize[point.map].system==640 then mastermap=640 end  -- deepholm, do NOT translate onto Azeroth
	--if point.c==-1 then mastermap=Astrolabe.WorldMapSize[point.map].system end  -- instances, do NOT translate onto Azeroth
	if m~=mastermap or point.f~=masterfloor then
		point.gx,point.gy = Astrolabe:TranslateWorldMapPosition( m, point.f, point.x, point.y, mastermap, masterfloor )
	end
	if point.gx then
		point.gm,point.gf=mastermap,masterfloor
	else
		point.gm,point.gf=point.m,point.f
		point.gx,point.gy=point.x,point.y
	end
end
_G['move_point_to_global']=move_point_to_global


function Pointer:SetAntSpacing(spacing)
	curve_spacing = spacing
	Pointer.curve_spacing = spacing
end

-- Display (time-phased) ants between all .ants -enabled sets in Pointer.pointsets .

function Pointer:AnimateAnts()
	local ant_speed = ant_speed
	if (ant_interval>0.2) then ant_speed=0.3 end
	local phase = (GetTime()*ant_speed)%1

	-- Set ant spacing properly for overworld maps
	local map = GetCurrentMapAreaID()
	local overworld = (map==13 or map==14 or map==0 or map==689 or map==-1 or map==485 or map==466 or map==862)

	self:SetAntSpacing((overworld and ZGV.db.profile.antspacing*7) or (TaxiFrame:IsShown() and ZGV.db.profile.antspacing*5) or ZGV.db.profile.antspacing)

	local total_ants = 0

	for name,pointset in pairs(self.pointsets) do
		if pointset.ants and curve_spacing>0
		--and only_type=="ant"
		then --and not step.waypath_curved
			--step.waypath_curved = true

			for pi,wp in ipairs(pointset.points) do
				wp.curve_accuracy=nil  -- clear this cached value, we might change accuracy.

				if wp.player then
					-- point is player? get new location
					local m,f,x,y = Astrolabe:GetCurrentPlayerPosition("last")
					wp.m,wp.f,wp.x,wp.y = m,f,x,y
					wp.gx,wp.gy,wp.gm,wp.gf = nil,nil,nil,nil
				end

				move_point_to_global(wp)

				--ZGV.Pointer:SetWaypoint (wp.map,wp.floor,wp.x,wp.y,nil,nil)--data,arrow)
				--ants=spawn(waypath)
				--show(waypath, ants)
			end

			local antpoints,num
			--print("spawning, player = "..waypath.coords[1].x)
			if pointset.ants=="straight" or #pointset.points<3 then
				antpoints,num = spawn_straight_ants(pointset.points,pointset.loop,phase)
			else
				antpoints,num = spawn_curve_ants(pointset.points,pointset.loop,phase)
			end

			--calc_angles(antpoints,pointset.loop)
			if antpoints then
				--Pointer:Debug("Ants: %d %s spawned for set %s",num,pointset.ants,name)
				if ants_optimized_which_isnt_implemented then
					if Pointer.ants_set then
						update_ant_waypoints(antpoints)
					else
						set_waypoints_ants(antpoints,num,35,30)
						Pointer.ants_set=true
					end
				else
					set_waypoints_ants(antpoints,num,total_ants,35,30)
				end
			else
				--Pointer:Debug("Ants: No ants spawned for set %s",name)
			end

			if num then total_ants = total_ants + num end
		end
	end

	self:ClearWaypoints_ant(total_ants)

end


Pointer.pointsets = {}

function Pointer:ClearSets()
	for k,v in pairs(self.pointsets) do
		self:ClearSet(k)
	end
end

local lv=0
function Pointer:ClearSet(name)
	local t1=debugprofilestop()
	lv=lv+1
	local set = self.pointsets[name]
	if not set then
		--Pointer:Debug("Clearing set %s, but none present",name)
		lv=lv-1
		return
	end
	--Pointer:Debug("Clearing set %s, %d points",name,#set.points)
	for pi,point in ipairs(set.points) do
		--Pointer:Debug("Removing point %d from set %s",pi,name)
		point.in_set = nil  -- so that it's not attempted to remove from set again
		self:RemoveWaypoint(point)
	end
	self.pointsets[name]=nil
	assert(lv<=3,"No nesting ClearSet too deep!")
	--Pointer:Debug("Cleared set %s",name)
	lv=lv-1

	self:AnimateAnts()  --force

	Pointer:Debug("ClearSet %s + ants = %.2f",name,debugprofilestop()-t1)
end

-- Show a series of points as a path. TODO: make this add ants on all segments.

function Pointer:ShowSet(waypath,name)
	self:ClearSet(name)

	local t1=debugprofilestop()

	if waypath and #waypath.coords>1 then  -- show ants, or just the path, anyway.

		-- SHOW IT NOW, please. Also add it to set.
		set_waypoints(waypath.coords,nil,nil,waypath.coords[1].type or "path",name)

		-- calculate global map coords

		local points = self.pointsets[name] and self.pointsets[name].points
		if not points then self:Debug("No points in set!") return end  -- there were none to begin with? what?


		-- let's handle these proper waypoints now

		--globalize position! fill gm,gx,gy with world-global values. Otherwise ants can't travel over zone crossings.
		for wpi,wp in ipairs(points) do
			if not wp.gx and wp.m then  move_point_to_global(wp)  end
		end

		-- calculate path arrow angles
		if waypath.ants=="curved" then
			calc_angles_curved(points,waypath.loop)
		else
			calc_angles(points,waypath.loop)
		end

		-- set up icons based on angles/loops
		for k,point in ipairs(points) do
			local icon=point.icon
			if point.player or waypath.loop or (point.pathnode and point.pathnode.a_b__c_d) == "taxi_taxi__taxi_taxi" then
				icon = Pointer.Icons.none
			elseif point.angle then
				icon=Pointer.Icons.arrow
			else
				icon=icon or Pointer.Icons.greendot
			end
			point:SetIcon(icon)
		end

		-- Get all the other fields
		for k,v in pairs(waypath) do if k~="coords" then Pointer.pointsets[name][k]=v end end

		self.pathfollow = waypath.follow
		self.pathloop = waypath.loop

	else
		self.pathfollow = nil
	end

	local t2=debugprofilestop()
	Pointer:Debug("ShowSet %s = %.2f",name,t2-t1)

	self:AnimateAnts()  --force
	Pointer:Debug("ShowSet %s + ants = %.2f",name,debugprofilestop()-t2)
end

local PATHFOUND_TO_MANUAL, PATHFINDING_TARGET

local oldpathtarget
--local FAILED_PATH
local function PathFoundHandler(state,path,ext)
	if ZGV.Pointer.corpsearrow then return end

	Pointer:Debug("PathFoundHandler! state=%s",state)

	if ext and ext.token and ext.token~=ZGV.Pointer.DestinationWaypoint then Pointer:Debug("Found wrong path!") return end

	if state=="success" then
		Pointer:ClearSet("route")
		local future_waypoints = {follow="pathfind",loop=false,ants="straight",coords={}}

		local first=true
		for i,node in ipairs(path) do
			local icon
			if not node.player and node.type~="end" then icon=Pointer.Icons.arrow else icon=Pointer.Icons.none end  -- start and end nodes are HIDDEN.
			--if w.node==LibRover.endnode then break end -- don't add the last one! add it separately. WHY!??
			local wayp = {map=node.m,floor=node.f,x=node.x,y=node.y, title=node.maplabel, arrowtitle=node.text,player=node.player, type="route", icon=icon, pathnode=node}
			-- force_noway=node.player,

			if node.type=="end" then wayp.noskip = true end

			if first and not node.player then first=false  wayp.arrow=true  end

			wayp.radius = node.radius or ZGV.Pointer:GetDefaultStepDist() -- will account for flying

			if node.type=="taxi" then
				-- source taxi: never complete waypoint
				-- destination taxi: early complete waypoint, let LibRover wait for touchdown
				if node.link.mode~="taxi" then
					--start
					wayp.radius = 5
					wayp.noskip = true
				end
			elseif node.type=="portal" then
				if node.link.mode~="portal" then
					wayp.radius = 5
					wayp.noskip = true
				end
			elseif node.type=="ship" or node.type=="zeppelin" then
				if node.link.mode~="ship" and node.link.mode~="zeppelin" then
					wayp.radius = 5
					wayp.noskip = true
				else
					wayp.radius = 100
				end
			end

			local mode = node.link and node.link.mode
			if mode=="taxi" then wayp.ant_icon = ZGV.Pointer.Icons.ant_g
			elseif mode=="ship" then wayp.ant_icon = ZGV.Pointer.Icons.ant_b
			elseif mode=="portal" or mode=="teleport" or mode=="hearth" or mode=="astralrecall" or mode=="courtesy" or mode=="useitem" then wayp.ant_icon = ZGV.Pointer.Icons.ant_p
			elseif mode=="fly" then wayp.ant_icon = ZGV.Pointer.Icons.ant_y
			else wayp.ant_icon = ZGV.Pointer.Icons.ant
			end

			wayp.actionicon = node:GetActionIcon(path[i-1],path[i+1])

			-- OMG if it's the LAST waypoint in a travel path, point DIRECTLY, instead of at the placeholder.
			if node.type=="end" then wayp.surrogate_for=ZGV.Pointer.DestinationWaypoint end

			tinsert(future_waypoints.coords, wayp)
		end

		--[[
		if ZGV.Pointer.CurrentPathTarget then
			local w = ZGV.Pointer.CurrentPathTarget
			tinsert(ZGV.Pointer.TempWaypath.coords, {map=w.m,floor=w.f,x=w.x,y=w.y,title=w.title,force_noway=true})
		end
		--]]

		Pointer.ArrowFrame.waypoint = nil -- clear arrow, so that it updates around line 804 this file. We :SetWaypoint right under here.
		--FAILED_PATH = nil
		Pointer:ShowSet(future_waypoints,"route") -- clear none, just refresh

		Pointer:ShowArrow(Pointer.pointsets.route.points[2])  -- point 1 is player

	elseif state=="failure" then
		Pointer:ClearSet("route")
		--FAILED_PATH = ZGV.Pointer.DestinationWaypoint

		Pointer:ShowArrow(ZGV.Pointer.DestinationWaypoint)

		--ZGV:SetWaypoint(nil,nil,nil,"path",{dontcleartemp=true}) -- clear none, just refresh

	-- Causing the Lovely arrow problems ~Errc
	-- Well let's cure, not amputate ;) ~sin
	elseif state=="arrival" then
		Pointer:ClearSet("route")
		Pointer:Debug("Waypoints: Arrived at destination!")
		--FAILED_PATH = ZGV.Pointer.CurrentPathTarget
		Pointer:ShowArrow(ZGV.Pointer.DestinationWaypoint)

		Pointer:ResetFollowing()
	elseif state=="progress" then
		-- DON'T clear anything.
		Pointer:ShowWaiting(ext and ext.progress or 0)
	end
	--ZGV:SetWaypoint() -- clear none, just refresh
end
Pointer.PathFoundHandler = PathFoundHandler

function Pointer:ResetFollowing()
	local path = self.pointsets.route or self.pointsets.path
	if path then
		self.pathfollow = path.follow
		self.pathloop = path.loop
	else
		self.pathfollow = nil
		self.pathloop = nil
	end
end

-- Finds an optimal travel route. Or, just a beeline, if options say so.
function Pointer:FindTravelPath(way)
	if not way then return end
	if way.pathnode and way.pathnode.player then return end -- no pointing to self

	if type(way)=="table" then
		self:ShowArrow(way)
		if way.type=="route" then return end
		Pointer:Debug("FindTravelPath")
		ZGV.Pointer.DestinationWaypoint = way
		local display_zone = way.waypoint_minizone or way.waypoint_subzone
		LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, PathFoundHandler,
			{title=way.title .. (display_zone and ("\n(in %s)"):format(display_zone) or ""), waypoint=way, direct=not ZGV.db.profile.pathfinding,
			waypoint_zone=way.waypoint_zone, waypoint_realzone=way.waypoint_realzone, waypoint_subzone=way.waypoint_subzone, waypoint_minizone=way.waypoint_minizone,
			waypoint_region=way.waypoint_region, waypoint_indoors = way.waypoint_indoors
			}
			)
	elseif type(way)=="string" then
		-- set!
		local set=self.pointsets[way] and self.pointsets[way].points
		if not set then Pointer:Debug("No set '%s'",way) return end
		way=set[1]
		if not way then Pointer:Debug("No points in set '%s'",way) return end
		Pointer:Debug("FindTravelPath to pointset")
		ZGV.Pointer.DestinationWaypoint = way
		local more_points = {}
		for i=2,#set do
			local w=set[i]
			more_points[#more_points+1]={m=w.m,f=w.f,x=w.x,y=w.y,title=w.title}
		end
		LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, PathFoundHandler, {title=way.title, waypoint=way, direct=not ZGV.db.profile.pathfinding, multiple_ends=more_points })
	end
end



function Pointer:DoAutoTravel()
	if ZGV.db.profile.autotravel then
		if not Pointer.AutoTravelFrame then
			Pointer.AutoTravelFrame = CHAIN(CreateFrame("FRAME","ZygorGuidesViewerPointer_AutoTravelFrame"))
				:SetPoint("TOPLEFT",UIParent,"TOPLEFT")
				:SetSize(1,1)
				.__END
			Pointer.AutoTravelFrame.t1 = CHAIN(Pointer.AutoTravelFrame:CreateTexture(nil,"OVERLAY"))
				:SetPoint("TOPLEFT",Pointer.AutoTravelFrame,"TOPLEFT",0,0)
				:SetTexture(1,1,1)
				:SetSize(5,5)
				.__END
			Pointer.AutoTravelFrame.t2 = CHAIN(Pointer.AutoTravelFrame:CreateTexture(nil,"OVERLAY"))
				:SetPoint("TOPLEFT",Pointer.AutoTravelFrame,"TOPLEFT",5,0)
				:SetTexture(0,1,0)
				:SetSize(5,5)
				.__END
			Pointer.AutoTravelFrame.t3 = CHAIN(Pointer.AutoTravelFrame:CreateTexture(nil,"OVERLAY"))
				:SetPoint("TOPLEFT",Pointer.AutoTravelFrame,"TOPLEFT",10,0)
				:SetTexture(1,0,1)
				:SetSize(5,5)
				.__END
			print("creating autotravel frame")
		end

		local angle = Pointer.ArrowFrame.arrow.angle
		if angle then
			if angle>0.2 and angle<3.1415 then Pointer.AutoTravelFrame.t1:SetTexture(0,1,0) --green:left
			elseif angle>=3.1415 and angle<6.083 then Pointer.AutoTravelFrame.t1:SetTexture(1,0,0) --red:right
			else Pointer.AutoTravelFrame.t1:SetTexture(0,0,0) --black:none
			end

			-- TODO distance
		end
	end
end

function Pointer.QuestPOI_PointToMe(poiBut,args)
	if not ZGV.db.profile.point_to_pois then return end
	local _
	if not poiBut.questId then _,poiBut = poiBut:GetPoint() end  -- the "Swap Frame", whatever that is... Dig the quest POI button out of it.
	--local _,_,_,x,y=poiBut:GetPoint(1)
	--local parent = poiBut:GetParent()
	--x= x/parent:GetWidth()/poiBut:GetEffectiveScale()*1.409
	--y=-y/parent:GetHeight()/poiBut:GetEffectiveScale()*1.409
	local questId = poiBut.questId or poiBut.quest.questId
	_,x,y=QuestPOIGetIconInfo(questId)
	Pointer:ClearWaypoints("manual")
	--Pointer:SetWaypoint(nil,nil,x,y,{title=ZGV.questsbyid[poiBut.questId].title,type="manual"},true)
	Pointer:SetWaypoint(nil,nil,x,y,{title=ZGV.questsbyid[questId].title,type="manual",onminimap="always",overworld=true,showonedge=true,findpath=true},true)
end
--[[
function Pointer.QuestWatchPOI_PointToMe(poiBut,args)
	do return end
	if not ZGV.db.profile.point_to_pois then return end
	local _
	if not poiBut.questId then _,poiBut = poiBut:GetPoint() end  -- the "Swap Frame", whatever that is... Dig the quest POI button out of it.
	local _,_,_,x,y=poiBut:GetPoint(1)
	local parent = poiBut:GetParent()
	x= x/parent:GetWidth()/poiBut:GetEffectiveScale()*1.409
	y=-y/parent:GetHeight()/poiBut:GetEffectiveScale()*1.409
	Pointer:ClearWaypoints("manual")
	--Pointer:SetWaypoint(nil,nil,x,y,{title=ZGV.questsbyid[poiBut.questId].title,type="manual"},true)
	local way = Pointer:SetWaypoint(nil,nil,x,y,{title=ZGV.questsbyid[poiBut.questId or poiBut.quest.questId].title,type="manual",onminimap="always",overworld=true,showonedge=true},true)
	Pointer:FindTravelPath(way)
end
--]]

function Pointer:Debug(msg,...)
	ZGV:Debug("&_SUB &pointer ".. msg, ...)
end

