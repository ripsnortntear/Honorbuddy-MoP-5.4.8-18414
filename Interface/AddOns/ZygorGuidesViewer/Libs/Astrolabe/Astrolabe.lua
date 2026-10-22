--[[
Name: Astrolabe
Revision: $Rev: 147 $
$Date: 2012-08-28 09:45:40 -0700 (Tue, 28 Aug 2012) $
Author(s): Esamynn (esamynn at wowinterface.com)
Inspired By: Gatherer by Norganna
             MapLibrary by Kristofer Karlsson (krka at kth.se)
Documentation: http://wiki.esamynn.org/Astrolabe
SVN: http://svn.esamynn.org/astrolabe/
Description:
	This is a library for the World of Warcraft UI system to place
	icons accurately on both the Minimap and on Worldmaps.
	This library also manages and updates the position of Minimap icons
	automatically.

Copyright (C) 2006-2012 James Carrothers

License:
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.

	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	Lesser General Public License for more details.

	You should have received a copy of the GNU Lesser General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA

Note:
	This library's source code is specifically designed to work with
	World of Warcraft's interpreted AddOn system.  You have an implicit
	licence to use this library with these facilities since that is its
	designated purpose as per:
	http://www.fsf.org/licensing/licenses/gpl-faq.html#InterpreterIncompat
]]

-- WARNING!!!
-- DO NOT MAKE CHANGES TO THIS LIBRARY WITHOUT FIRST CHANGING THE LIBRARY_VERSION_MAJOR
-- STRING (to something unique) OR ELSE YOU MAY BREAK OTHER ADDONS THAT USE THIS LIBRARY!!!
local LIBRARY_VERSION_MAJOR = "Astrolabe-ZGV"
local LIBRARY_VERSION_MINOR = tonumber(string.match("$Revision: 147 $", "(%d+)") or 1)

if not DongleStub then error(LIBRARY_VERSION_MAJOR .. " requires DongleStub.") end
if not DongleStub:IsNewerVersion(LIBRARY_VERSION_MAJOR, LIBRARY_VERSION_MINOR) then return end

local Astrolabe = {};

-- define local variables for Data Tables (defined at the end of this file)
local WorldMapSize, MicroDungeonSize, MinimapSize, ValidMinimapShapes, zeroData;

function Astrolabe:GetVersion()
	return LIBRARY_VERSION_MAJOR, LIBRARY_VERSION_MINOR;
end


--------------------------------------------------------------------------------------------------------------
-- Config Constants
--------------------------------------------------------------------------------------------------------------

local configConstants = {
	MinimapUpdateMultiplier = true,
}

-- this constant is multiplied by the current framerate to determine
-- how many icons are updated each frame
Astrolabe.MinimapUpdateMultiplier = 10; -- sinus: up from 1, ants need a higher freq


--------------------------------------------------------------------------------------------------------------
-- Working Tables
--------------------------------------------------------------------------------------------------------------

Astrolabe.LastPlayerPosition = { 0, 0, 0, 0 };
Astrolabe.MinimapIcons = {};
Astrolabe.IconsOnEdge = {};
Astrolabe.IconsOnEdge_GroupChangeCallbacks = {};

Astrolabe.MinimapIconCount = 0
Astrolabe.ForceNextUpdate = false;
Astrolabe.IconsOnEdgeChanged = false;

Astrolabe.MapIDsByName = {}

-- This variable indicates whether we know of a visible World Map or not.
-- The state of this variable is controlled by the AstrolabeMapMonitor library.
Astrolabe.WorldMapVisible = false;

local AddedOrUpdatedIcons = {}
local MinimapIconsMetatable = { __index = AddedOrUpdatedIcons }


--------------------------------------------------------------------------------------------------------------
-- Local Pointers for often used API functions
--------------------------------------------------------------------------------------------------------------

local twoPi = math.pi * 2;
local atan2 = math.atan2;
local sin = math.sin;
local cos = math.cos;
local abs = math.abs;
local sqrt = math.sqrt;
local min = math.min
local max = math.max
local yield = coroutine.yield
local next = next
local GetFramerate = GetFramerate

-- Zone phase sanitization: upon each call to GetCurrentMapAreaID, make sure all phases of the same zone point to the current.
local zone_aliases = {
	-- Mount Hyjal
	[606]=606,
	--[683]=606,  -- this causes errors and has some "area 1" level, what the heck
	-- The Maelstrom
	[737]=737,
	[751]=737,
	-- Twilight Highlands
	[700]=700,
	--[770]=700,   -- this causes errors and has some "area 1" level, what the heck
	-- Uldum
	[720]=720,
	--[748]=720,
	-- Zul'Gurub
	[697]=697,
	[793]=697,
}
local do_zone_aliasing=true
-- sinus: this may no longer be needed at all.

local real_GetCurrentMapAreaID = GetCurrentMapAreaID
local function GetCurrentMapAreaID()
	local id = real_GetCurrentMapAreaID();

	if do_zone_aliasing then  -- update aliases, trusting our current map id
		local alias = zone_aliases[id]
		if alias and alias~=id then
			for z,a in pairs(zone_aliases) do if a==alias then zone_aliases[z]=id end end
		end
	end

	if ( id < 0 and GetCurrentMapContinent() == WORLDMAP_WORLD_ID ) then
		return 0;
	end
	return id;
end


--------------------------------------------------------------------------------------------------------------
-- Internal Utility Functions
--------------------------------------------------------------------------------------------------------------

local function assert(level,condition,message)
	if not condition then
		error(message,level)
	end
end

local function argcheck(value, num, ...)
	assert(1, type(num) == "number", "Bad argument #2 to 'argcheck' (number expected, got " .. type(level) .. ")")

	for i=1,select("#", ...) do
		if type(value) == select(i, ...) then return end
	end

	local types = strjoin(", ", ...)
	local name = string.match(debugstack(2,2,0), ": in function [`<](.-)['>]")
	error(string.format("Bad argument #%d to 'Astrolabe.%s' (%s expected, got %s)", num, name, types, type(value)), 3)
end

local function getSystemPosition( mapData, f, x, y )
	if ( f ~= 0 ) then
		mapData = mapData[f];
	end
	x = x * mapData.width + mapData.xOffset;
	y = y * mapData.height + mapData.yOffset;
	return x, y;
end

local function printError( ... )
	if ( ASTROLABE_VERBOSE) then
		print(...)
	end
end


--------------------------------------------------------------------------------------------------------------
-- General Utility Functions
--------------------------------------------------------------------------------------------------------------

function Astrolabe:ComputeDistance( m1, f1, x1, y1, m2, f2, x2, y2 )
	--[[
	argcheck(m1, 2, "number");
	assert(3, m1 >= 0, "ComputeDistance: Illegal map id to m1: "..m1);
	argcheck(f1, 3, "number", "nil");
	argcheck(x1, 4, "number");
	argcheck(y1, 5, "number");
	argcheck(m2, 6, "number");
	assert(3, m2 >= 0, "ComputeDistance: Illegal map id to m2: "..m2);
	argcheck(f2, 7, "number", "nil");
	argcheck(x2, 8, "number");
	argcheck(y2, 9, "number");
	--]]

	m1 = zone_aliases[m1] or m1 --sinus
	m2 = zone_aliases[m2] or m2 --sinus

	if not ( m1 and m2 ) then return end;
	f1 = f1 or WorldMapSize[m1].floorMin
	f2 = f2 or WorldMapSize[m2].floorMin

	local dist, xDelta, yDelta;
	if ( m1 == m2 and f1 == f2 ) then
		-- points in the same zone on the same floor
		local mapData = WorldMapSize[m1];
		if ( f1 ~= 0 ) then
			mapData = mapData[f1];
		end
		xDelta = (x2 - x1) * mapData.width;
		yDelta = (y2 - y1) * mapData.height;

	else
		local map1 = WorldMapSize[m1];
		local map2 = WorldMapSize[m2];
		if ( map1.system == map2.system ) then
			-- points within the same system (continent)
			x1, y1 = getSystemPosition(map1, f1, x1, y1);
			x2, y2 = getSystemPosition(map2, f2, x2, y2);
			xDelta = (x2 - x1);
			yDelta = (y2 - y1);

		else
			local sp1 = map1.systemParent;
			local sp2 = map2.systemParent;
			if ( sp1 == sp2 ) then
				--local worldID = sp1;
				-- instead of a new local, reuse sp1
				local s1 = map1.system;
				local s2 = map2.system;
				x1, y1 = getSystemPosition(map1, f1, x1, y1);
				x2, y2 = getSystemPosition(map2, f2, x2, y2);
				if not ( s1 == map1.systemParent ) then
					local cont1 = WorldMapSize[sp1][s1];
					x1 = x1 - cont1.xOffset;
					y1 = y1 - cont1.yOffset;
				end
				if not ( s2 == map2.systemParent ) then
					local cont2 = WorldMapSize[sp1][s2];
					x2 = x2 - cont2.xOffset;
					y2 = y2 - cont2.yOffset;
				end

				xDelta = x2 - x1;
				yDelta = y2 - y1;
			end

		end

	end
	if ( xDelta and yDelta ) then
		dist = sqrt(xDelta*xDelta + yDelta*yDelta);
	end
	return dist, xDelta, yDelta;
end

function Astrolabe:TranslateWorldMapPosition( M, F, xPos, yPos, nM, nF )
	--[[
	argcheck(M, 2, "number");
	argcheck(F, 3, "number", "nil");
	argcheck(xPos, 4, "number");
	argcheck(yPos, 5, "number");
	argcheck(nM, 6, "number");
	argcheck(nF, 7, "number", "nil");
	--]]

	if not ( M and nM ) then return end;
	F = F or WorldMapSize[M].floorMin
	nF = nF or WorldMapSize[nM].floorMin
	if ( nM == -1 ) then
		return;
	end

	local mapData;
	if ( M == nM and F == nF ) then
		return xPos, yPos;

	else
		local map = WorldMapSize[M];
		local nMap = WorldMapSize[nM];
		if ( map.system == nMap.system ) then
			-- points within the same system (continent)
			xPos, yPos = getSystemPosition(map, F, xPos, yPos);
			mapData = WorldMapSize[nM];
			if ( nF ~= 0 ) then
				mapData = mapData[nF];
			end

		else
			-- different continents, same world
			local SP = map.systemParent;
			local nSP = nMap.systemParent;
			if ( SP == nSP ) then
				--local worldID = SP;
				-- instead of a new local, reuse SP
				local S = map.system;
				local nS = nMap.system;
				mapData = WorldMapSize[M];
				xPos, yPos = getSystemPosition(mapData, F, xPos, yPos);
				if ( M ~= SP ) then
					-- translate up to world map if we aren't there already
					local cont = WorldMapSize[SP][S];
					xPos = xPos - cont.xOffset;
					yPos = yPos - cont.yOffset;
					mapData = WorldMapSize[SP];
				end
				if ( nM ~= SP ) then
					-- translate down to the new continent
					local nCont = WorldMapSize[SP][nS];
					xPos = xPos + nCont.xOffset;
					yPos = yPos + nCont.yOffset;
					mapData = WorldMapSize[nM];
					if ( nF ~= 0 ) then
						mapData = mapData[nF];
					end
				end

			else
				return;
			end

		end

		-- need to account for the offset in the new system so we can
		-- correctly translate into 0-1 style coordinates
		xPos = xPos - mapData.xOffset;
		yPos = yPos - mapData.yOffset;

	end

	if not mapData.width or mapData.width==0 then
		--print("No map for ",M,F,nM,nF)
		return
	end

	return (xPos / mapData.width), (yPos / mapData.height);
end

--*****************************************************************************
-- This function will do its utmost to retrieve some sort of valid position
-- for the specified unit, including changing the current map zoom (if needed).
-- Map Zoom is returned to its previous setting before this function returns.
--*****************************************************************************

-- sinus: throttle map changes! No more than one per second!
local last_map_change = 0
local dungeon_maps_named_on_world = {[687]=true,[756]=true,[688]=true,[762]=true}  -- There are overworld areas, where GetRealZoneText() returns names of these maps.
local lastm,lastf,lastx,lasty = 0,0,0,0

function Astrolabe:GetUnitPosition( unit, noMapChange, allow_last )
	local x, y = GetPlayerMapPosition(unit);
	if ( x <= 0 and y <= 0 ) then
		if ( noMapChange ) then
			-- no valid position on the current map, and we aren't allowed
			-- to change map zoom, so return
			if allow_last then
				return lastm,lastf,lastx,lasty, true ----------------------
			end
			return nil,nil,nil,nil,nil,"last not allowed :(" -----------------------
		end

		-- sinus: throttling, to prevent excessive SetMapToCurrentZone calls. These would happen if the coords are 0, but a SetMapToCurrentZone call fails to produce coords.
		local time=GetTime()
		if time-last_map_change<1 then return lastm,lastf,lastx,lasty,true, nil,nil,"overtime" end
		last_map_change=time

		--print("no normal position")

		-- How about NOT returning the map, since worldmap is closed anyway.
		local lastMapID, lastFloor = GetCurrentMapAreaID(), GetCurrentMapDungeonLevel();
		SetMapToCurrentZone();

		x, y = GetPlayerMapPosition(unit);
		if ( x <= 0 and y <= 0 ) then
			--print("player not found in their zone")
			--WorldMapZoomOutButton_OnClick();
			local mapData = WorldMapSize[lastMapID]
			if mapData and mapData.system and mapData.system~=lastMapID then SetMapByID(mapData.system) end
			x, y = GetPlayerMapPosition(unit);
			--print("on continent level",x,y)
			if ( x <= 0 and y <= 0 ) then
				--print("player not found in continent!")
				-- we are in an instance or otherwise off map

				--print ("Sorry, no results even on continent")
				return
			end
			-- Sinus: better than nothing. Stay and report the results as usual.
		end

		local M, F = GetCurrentMapAreaID(), GetCurrentMapDungeonLevel();
		--[[
		if ( M ~= lastMapID or F ~= lastFloor ) then
			-- set map zoom back to what it was before
			-- Will break out of caves :/  ~sinus
			SetMapByID(lastMapID)
			SetDungeonMapLevel(lastFloor)
		end
		--]]

		lastm,lastf,lastx,lasty = M,F,x,y
		return M, F, x, y, nil,nil,"zoomedout"
	end

	--[[
	Trying our goddamn best to find the current position for the player!

	= IS WORLD MAP OPEN?
	- NO: Everything is in order; expecting GetCurrentMapAreaID() and GetPlayerMapPosition() to return proper values.
	- YES: Whoa, they might have navigated away from their current map (but the arrow is still visible!)
	  = DOES GetRealZoneText() match their current zone-name-by-ID?
	    - YES: Okay, cool, they didn't navigate away. GetCurrentMapAreaID() and GetPlayerMapPosition() are trusted.
	    - NO: Goddamnit:
	      = WHERE DID THEY NAVIGATE TO, CAN WE MAP FROM THERE TO THE CURRENT ZONE?
	        - YES: Whew. Correct the coordinates and return them, "good as new". Might be very slightly inaccurate.
		- NO: We're screwed.

	sinus@sinpi.net
	--]]

	-- sinus: double-check if it's the player; make sure we have the right map. This is important for all waypointing addons.

	-- So, we have some coordinates, but are they correct?
	local m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()

	if unit=="player" then
		if noMapChange  -- oops, map is open, we can't change it!
		or (m==13 or m==14) -- we're in continent space, halp! map might be closed, but we're in trouble anyway
		then  -- if worldmap isn't visible, then no need to worry, we must've gotten good coords.
			-- worrying now.
			local rzt = GetRealZoneText()
			if rzt~=GetMapNameByID(m) then -- player navigated away from their current map, OR their location is evil.
				--print("browsed away")
				local realmap,realfloor = self.MapIDsByName[rzt],0
				local worldmaptext = WorldMapFrameTitle:GetText()
				local worldmapid = worldmap and self.MapIDsByName[worldmap]
				if realmap then
					if dungeon_maps_named_on_world[realmap] then
						-- They're NOT in a neighboring zone, they're in a dungeon entrance. IGNORE it, we're GOOD (hopefully.)
						realmap,realfloor=last_proper_zone,last_proper_floor
						--print("using last",realmap,realfloor)
					end
					if realmap and not dungeon_maps_named_on_world[realmap] then
						-- ah-ha! Got player's current map! And it's NOT an overworld dungeon entrance!
						--print("we're browsing",GetMapNameByID(m),"really in",GetRealZoneText())
							-- Crap, they're either in a dungeon browsing world maps, or in a "dungeon entry" overworld area.
							-- Bail out, nothing to be done here. Can't fix this.
						local realx,realy = Astrolabe:TranslateWorldMapPosition( m, f, x, y, realmap, realfloor )  -- Whaaat? We don't know their true floor! This is a nightmare.
						if realx then
							m,f,x,y = realmap,realfloor,realx,realy
							--print("adjusted",m,f,x,y)
						end
					end
				end
			end
		end

		-- record sane values
		if m and x and m~=0 and m~=13 and m~=14 and m~=-1 and m~=485 and m~=466 and m~=613 and m~=862   -- multi-zone, whole-continent maps
		then lastm,lastf,lastx,lasty = m,f,x,y end

	end

	return m, f, x, y
end

--*****************************************************************************
-- This function will do its utmost to retrieve some sort of valid position
-- for the specified unit, including changing the current map zoom (if needed).
-- However, if a monitored WorldMapFrame (See AstrolabeMapMonitor.lua) is
-- visible, then will simply return nil if the current zoom does not provide
-- a valid position for the player unit.  Map Zoom is returned to its previous
-- setting before this function returns, if it was changed.
--*****************************************************************************
function Astrolabe:GetCurrentPlayerPosition(allow_last)
	return self:GetUnitPosition("player", self.WorldMapVisible, allow_last);
end

function Astrolabe:GetMapID(continent, zone)
	zone = zone or 0;
	local ret = self.ContinentList[continent];
	if ( ret ) then
		return ret[zone];
	end
	if ( continent == 0 and zone == 0 ) then
		return 0;
	end
end

function Astrolabe:GetNumFloors( mapID )
	if ( type(mapID) == "number" ) then
		return WorldMapSize[mapID].floorMin
	end
end

function Astrolabe:GetMapInfo( mapID, mapFloor )
	argcheck(mapID, 2, "number");
	assert(3, mapID >= 0, "GetMapInfo: Illegal map id to mapID: "..mapID);
	argcheck(mapFloor, 3, "number", "nil");

	mapFloor = mapFloor or WorldMapSize[mapID].floorMin
	local mapData = WorldMapSize[mapID];
	local system, systemParent = mapData.system, mapData.systemParent
	if ( mapFloor ~= 0 ) then
		mapData = mapData[mapFloor];
	end
	if ( mapData ~= zeroData ) then
		return system, systemParent, mapData.width, mapData.height, mapData.xOffset, mapData.yOffset;
	end
end


--------------------------------------------------------------------------------------------------------------
-- Working Table Cache System
--------------------------------------------------------------------------------------------------------------

local tableCache = {};
tableCache["__mode"] = "v";
setmetatable(tableCache, tableCache);

local function GetWorkingTable( icon )
	if ( tableCache[icon] ) then
		return tableCache[icon];
	else
		local T = {};
		tableCache[icon] = T;
		return T;
	end
end


--------------------------------------------------------------------------------------------------------------
-- Minimap Icon Placement
--------------------------------------------------------------------------------------------------------------

--*****************************************************************************
-- local variables specifically for use in this section
--*****************************************************************************
local minimapRotationEnabled = false;
local minimapShape = false;

local minimapRotationOffset = GetPlayerFacing();


local function placeIconOnMinimap( minimap, minimapZoom, mapWidth, mapHeight, icon, dist, xDist, yDist )
	local mapDiameter;
	if ( Astrolabe.minimapOutside ) then
		mapDiameter = MinimapSize.outdoor[minimapZoom];
	else
		mapDiameter = MinimapSize.indoor[minimapZoom];
	end
	local mapRadius = mapDiameter / 2;
	local xScale = mapDiameter / mapWidth;
	local yScale = mapDiameter / mapHeight;
	local iconDiameter = ((icon:GetWidth() / 2) + 3) * xScale;
	local iconOnEdge = nil;
	local isRound = true;

	if ( minimapRotationEnabled ) then
		local sinTheta = sin(minimapRotationOffset)
		local cosTheta = cos(minimapRotationOffset)
		--[[
		Math Note
		The math that is acutally going on in the next 3 lines is:
			local dx, dy = xDist, -yDist
			xDist = (dx * cosTheta) + (dy * sinTheta)
			yDist = -((-dx * sinTheta) + (dy * cosTheta))

		This is because the origin for map coordinates is the top left corner
		of the map, not the bottom left, and so we have to reverse the vertical
		distance when doing the our rotation, and then reverse the result vertical
		distance because this rotation formula gives us a result with the origin based
		in the bottom left corner (of the (+, +) quadrant).
		The actual code is a simplification of the above.
		]]
		local dx, dy = xDist, yDist
		xDist = (dx * cosTheta) - (dy * sinTheta)
		yDist = (dx * sinTheta) + (dy * cosTheta)
	end

	if ( minimapShape and not (xDist == 0 or yDist == 0) ) then
		isRound = (xDist < 0) and 1 or 3;
		if ( yDist < 0 ) then
			isRound = minimapShape[isRound];
		else
			isRound = minimapShape[isRound + 1];
		end
	end

	-- for non-circular portions of the Minimap edge
	if not ( isRound ) then
		dist = max(abs(xDist), abs(yDist))
	end

	if ( (dist + iconDiameter) > mapRadius ) then
		if icon.hide_on_minimap_edge then
			icon:Hide()
			return
		end
		-- position along the outside of the Minimap
		iconOnEdge = true;
		local factor = (mapRadius - iconDiameter) / dist;
		xDist = xDist * factor;
		yDist = yDist * factor;
	end

	if ( Astrolabe.IconsOnEdge[icon] ~= iconOnEdge ) then
		Astrolabe.IconsOnEdge[icon] = iconOnEdge;
		Astrolabe.IconsOnEdgeChanged = true;
	end

	--icon:ClearAllPoints();
	icon:SetPoint("CENTER", minimap, "CENTER", xDist/xScale, -yDist/yScale);

	return true
end

function Astrolabe:PlaceIconOnMinimap( icon, mapID, mapFloor, xPos, yPos )
	-- check argument types
	--[[
	argcheck(icon, 2, "table");
	assert(3, icon.SetPoint and icon.ClearAllPoints, "Usage Message");
	argcheck(mapID, 3, "number");
	argcheck(mapFloor, 4, "number", "nil");
	argcheck(xPos, 5, "number");
	argcheck(yPos, 6, "number");
	--]]

	mapID = zone_aliases[mapID] or mapID  --sinus

	-- if the positining system is currently active, just use the player position used by the last incremental (or full) update
	-- otherwise, make sure we base our calculations off of the most recent player position (if one is available)
	local lM, lF, lx, ly;
	if ( self.processingFrame:IsShown() ) then
		lM, lF, lx, ly = unpack(self.LastPlayerPosition);
	else
		lM, lF, lx, ly = self:GetCurrentPlayerPosition();
		if ( lM and lM >= 0 ) then
			local lastPosition = self.LastPlayerPosition;
			lastPosition[1] = lM;
			lastPosition[2] = lF;
			lastPosition[3] = lx;
			lastPosition[4] = ly;
		else
			lM, lF, lx, ly = unpack(self.LastPlayerPosition);
		end
	end

	local dist, xDist, yDist = self:ComputeDistance(lM, lF, lx, ly, mapID, mapFloor, xPos, yPos);
	if not ( dist ) then
		--icon's position has no meaningful position relative to the player's current location
		return -1;
	end

	local iconData = GetWorkingTable(icon);
	if ( self.MinimapIcons[icon] ) then
		self.MinimapIcons[icon] = nil;
	else
		self.MinimapIconCount = self.MinimapIconCount + 1
	end

	AddedOrUpdatedIcons[icon] = iconData
	iconData.mapID = mapID;
	iconData.mapFloor = mapFloor;
	iconData.xPos = xPos;
	iconData.yPos = yPos;
	iconData.dist = dist;
	iconData.xDist = xDist;
	iconData.yDist = yDist;

	minimapRotationEnabled = GetCVar("rotateMinimap") ~= "0"
	if ( minimapRotationEnabled ) then
		minimapRotationOffset = GetPlayerFacing();
	end

	-- check Minimap Shape
	minimapShape = GetMinimapShape and ValidMinimapShapes[GetMinimapShape()];

	-- place the icon on the Minimap and :Show() it
	local map = Minimap
	local seen = placeIconOnMinimap(map, map:GetZoom(), map:GetWidth(), map:GetHeight(), icon, dist, xDist, yDist);
	if seen then icon:Show() end

	-- We know this icon's position is valid, so we need to make sure the icon placement system is active.
	self.processingFrame:Show()

	return 0;
end

function Astrolabe:RemoveIconFromMinimap( icon )
	if not ( self.MinimapIcons[icon] ) then
		return 1;
	end
	AddedOrUpdatedIcons[icon] = nil
	self.MinimapIcons[icon] = nil;
	self.IconsOnEdge[icon] = nil;
	icon:Hide();

	local MinimapIconCount = self.MinimapIconCount - 1
	if ( MinimapIconCount <= 0 ) then
		-- no icons left to manage
		self.processingFrame:Hide()
		MinimapIconCount = 0 -- because I'm paranoid
	end
	self.MinimapIconCount = MinimapIconCount

	return 0;
end

function Astrolabe:RemoveAllMinimapIcons()
	self:DumpNewIconsCache()
	local MinimapIcons = self.MinimapIcons;
	local IconsOnEdge = self.IconsOnEdge;
	for k, v in pairs(MinimapIcons) do
		MinimapIcons[k] = nil;
		IconsOnEdge[k] = nil;
		k:Hide();
	end
	self.MinimapIconCount = 0
	self.processingFrame:Hide()
end

local lastZoom; -- to remember the last seen Minimap zoom level

-- local variables to track the status of the two update coroutines
local fullUpdateInProgress = true
local resetIncrementalUpdate = false
local resetFullUpdate = false

-- Incremental Update Code
do
	-- local variables to track the incremental update coroutine
	local incrementalUpdateCrashed = true
	local incrementalUpdateThread

	local function UpdateMinimapIconPositions( self )
		yield()

		while ( true ) do
			self:DumpNewIconsCache() -- put new/updated icons into the main datacache

			resetIncrementalUpdate = false -- by definition, the incremental update is reset if it is here

			local M, F, x, y = self:GetCurrentPlayerPosition();
			if ( M and M >= 0 ) then
				local Minimap = Minimap;
				local lastPosition = self.LastPlayerPosition;
				local lM, lF, lx, ly = unpack(lastPosition);

				minimapRotationEnabled = GetCVar("rotateMinimap") ~= "0"
				if ( minimapRotationEnabled ) then
					minimapRotationOffset = GetPlayerFacing();
				end

				-- check current frame rate
				local numPerCycle = min(50, GetFramerate() * (self.MinimapUpdateMultiplier or 1))

				-- check Minimap Shape
				minimapShape = GetMinimapShape and ValidMinimapShapes[GetMinimapShape()];

				if ( lM == M and lF == F and lx == x and ly == y ) then
					-- player has not moved since the last update
					if ( lastZoom ~= Minimap:GetZoom() or self.ForceNextUpdate or minimapRotationEnabled ) then
						local currentZoom = Minimap:GetZoom();
						lastZoom = currentZoom;
						local mapWidth = Minimap:GetWidth();
						local mapHeight = Minimap:GetHeight();
						numPerCycle = numPerCycle * 2
						local count = 0
						for icon, data in pairs(self.MinimapIcons) do if not data.self_updating then  -- sinus: "ant" icons are constantly updated; no point in including them in this cycle.
							placeIconOnMinimap(Minimap, currentZoom, mapWidth, mapHeight, icon, data.dist, data.xDist, data.yDist);

							count = count + 1
							if ( count > numPerCycle ) then
								count = 0
								yield()
								-- check if the incremental update cycle needs to be reset
								-- because a full update has been run
								if ( resetIncrementalUpdate ) then
									break;
								end
							end
						end end
						self.ForceNextUpdate = false;
					end
				else
					-- player HAS moved
					local dist, xDelta, yDelta = self:ComputeDistance(lM, lF, lx, ly, M, F, x, y);
					if ( dist ) then
						local currentZoom = Minimap:GetZoom();
						lastZoom = currentZoom;
						local mapWidth = Minimap:GetWidth();
						local mapHeight = Minimap:GetHeight();
						local count = 0
						for icon, data in pairs(self.MinimapIcons) do if not data.self_updating then  -- sinus: "ant" icons are constantly updated; no point in including them in this cycle.
							local xDist = data.xDist - xDelta;
							local yDist = data.yDist - yDelta;
							local dist = sqrt(xDist*xDist + yDist*yDist);
							placeIconOnMinimap(Minimap, currentZoom, mapWidth, mapHeight, icon, dist, xDist, yDist);

							data.dist = dist;
							data.xDist = xDist;
							data.yDist = yDist;

							count = count + 1
							if ( count >= numPerCycle ) then
								count = 0
								yield()
								-- check if the incremental update cycle needs to be reset
								-- because a full update has been run
								if ( resetIncrementalUpdate ) then
									break;
								end
							end
						end end
						if not ( resetIncrementalUpdate ) then
							lastPosition[1] = M;
							lastPosition[2] = F;
							lastPosition[3] = x;
							lastPosition[4] = y;
						end
					else
						self:RemoveAllMinimapIcons()
						lastPosition[1] = M;
						lastPosition[2] = F;
						lastPosition[3] = x;
						lastPosition[4] = y;
					end
				end
			else
				if not ( self.WorldMapVisible ) then
					self.processingFrame:Hide();
				end
			end

			-- if we've been reset, then we want to start the new cycle immediately
			if not ( resetIncrementalUpdate ) then
				yield()
			end
		end
	end

	function Astrolabe:UpdateMinimapIconPositions()
		if ( fullUpdateInProgress ) then
			-- if we're in the middle a a full update, we want to finish that first
			self:CalculateMinimapIconPositions()
		else
			if ( incrementalUpdateCrashed ) then
				incrementalUpdateThread = coroutine.wrap(UpdateMinimapIconPositions)
				incrementalUpdateThread(self) --initialize the thread
			end
			incrementalUpdateCrashed = true
			incrementalUpdateThread()
			incrementalUpdateCrashed = false
		end
	end
end

-- Full Update Code
do
	-- local variables to track the full update coroutine
	local fullUpdateCrashed = true
	local fullUpdateThread

	local function CalculateMinimapIconPositions( self )
		yield()

		while ( true ) do
			self:DumpNewIconsCache() -- put new/updated icons into the main datacache

			resetFullUpdate = false -- by definition, the full update is reset if it is here
			fullUpdateInProgress = true -- set the flag the says a full update is in progress

			local M, F, x, y = self:GetCurrentPlayerPosition();
			if ( M and M >= 0 ) then
				minimapRotationEnabled = GetCVar("rotateMinimap") ~= "0"
				if ( minimapRotationEnabled ) then
					minimapRotationOffset = GetPlayerFacing();
				end

				-- check current frame rate
				local numPerCycle = GetFramerate() * (self.MinimapUpdateMultiplier or 1) * 2

				-- check Minimap Shape
				minimapShape = GetMinimapShape and ValidMinimapShapes[GetMinimapShape()];

				local currentZoom = Minimap:GetZoom();
				lastZoom = currentZoom;
				local Minimap = Minimap;
				local mapWidth = Minimap:GetWidth();
				local mapHeight = Minimap:GetHeight();
				local count = 0
				for icon, data in pairs(self.MinimapIcons) do
					local dist, xDist, yDist = self:ComputeDistance(M, F, x, y, data.mapID, data.mapFloor, data.xPos, data.yPos);
					if ( dist ) then
						placeIconOnMinimap(Minimap, currentZoom, mapWidth, mapHeight, icon, dist, xDist, yDist);

						data.dist = dist;
						data.xDist = xDist;
						data.yDist = yDist;
					else
						self:RemoveIconFromMinimap(icon)
					end

					count = count + 1
					if ( count >= numPerCycle ) then
						count = 0
						yield()
						-- check if we need to restart due to the full update being reset
						if ( resetFullUpdate ) then
							break;
						end
					end
				end

				if not ( resetFullUpdate ) then
					local lastPosition = self.LastPlayerPosition;
					lastPosition[1] = M;
					lastPosition[2] = F;
					lastPosition[3] = x;
					lastPosition[4] = y;

					resetIncrementalUpdate = true
				end
			else
				if not ( self.WorldMapVisible ) then
					self.processingFrame:Hide();
				end
			end

			-- if we've been reset, then we want to start the new cycle immediately
			if not ( resetFullUpdate ) then
				fullUpdateInProgress = false
				yield()
			end
		end
	end

	function Astrolabe:CalculateMinimapIconPositions( reset )
		if ( fullUpdateCrashed ) then
			fullUpdateThread = coroutine.wrap(CalculateMinimapIconPositions)
			fullUpdateThread(self) --initialize the thread
		elseif ( reset ) then
			resetFullUpdate = true
		end
		fullUpdateCrashed = true
		fullUpdateThread()
		fullUpdateCrashed = false

		-- return result flag
		if ( fullUpdateInProgress ) then
			return 1 -- full update started, but did not complete on this cycle

		else
			if ( resetIncrementalUpdate ) then
				return 0 -- update completed
			else
				return -1 -- full update did no occur for some reason
			end

		end
	end
end

function Astrolabe:GetDistanceToIcon( icon )
	local data = self.MinimapIcons[icon];
	if ( data ) then
		return data.dist, data.xDist, data.yDist;
	end
end

function Astrolabe:IsIconOnEdge( icon )
	return self.IconsOnEdge[icon];
end

function Astrolabe:GetDirectionToIcon( icon )
	local data = self.MinimapIcons[icon];
	if ( data ) then
		local dir = atan2(data.xDist, -(data.yDist))
		if ( dir > 0 ) then
			return twoPi - dir;
		else
			return -dir;
		end
	end
end

function Astrolabe:Register_OnEdgeChanged_Callback( func, ident )
	-- check argument types
	argcheck(func, 2, "function");

	self.IconsOnEdge_GroupChangeCallbacks[func] = ident;
end

--*****************************************************************************
-- INTERNAL USE ONLY PLEASE!!!
-- Calling this function at the wrong time can cause errors
--*****************************************************************************
function Astrolabe:DumpNewIconsCache()
	local MinimapIcons = self.MinimapIcons
	for icon, data in pairs(AddedOrUpdatedIcons) do
		MinimapIcons[icon] = data
		AddedOrUpdatedIcons[icon] = nil
	end
	-- we now need to restart any updates that were in progress
	resetIncrementalUpdate = true
	resetFullUpdate = true
end


--------------------------------------------------------------------------------------------------------------
-- World Map Icon Placement
--------------------------------------------------------------------------------------------------------------

function Astrolabe:PlaceIconOnWorldMap( worldMapFrame, icon, mapID, mapFloor, xPos, yPos )
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

	mapID = zone_aliases[mapID] or mapID  --sinus

	local M, F = GetCurrentMapAreaID(), GetCurrentMapDungeonLevel();
	local nX, nY = self:TranslateWorldMapPosition(mapID, mapFloor, xPos, yPos, M, F);

	-- anchor and :Show() the icon if it is within the boundry of the current map, :Hide() it otherwise
	if ( nX and nY and (0 < nX and nX <= 1) and (0 < nY and nY <= 1) ) then
		--icon:ClearAllPoints();
		icon:SetPoint("CENTER", worldMapFrame, "TOPLEFT", nX * worldMapFrame:GetWidth(), -nY * worldMapFrame:GetHeight());
		icon:Show();
	else
		icon:Hide();
	end
	return nX, nY;
end


--------------------------------------------------------------------------------------------------------------
-- Handler Scripts
--------------------------------------------------------------------------------------------------------------

function Astrolabe:OnEvent( frame, event )
	if ( event == "MINIMAP_UPDATE_ZOOM" ) then
		-- update minimap zoom scale
		local Minimap = Minimap;
		local curZoom = Minimap:GetZoom();
		if ( GetCVar("minimapZoom") == GetCVar("minimapInsideZoom") ) then
			if ( curZoom < 2 ) then
				Minimap:SetZoom(curZoom + 1);
			else
				Minimap:SetZoom(curZoom - 1);
			end
		end
		if ( GetCVar("minimapZoom")+0 == Minimap:GetZoom() ) then
			self.minimapOutside = true;
		else
			self.minimapOutside = false;
		end
		Minimap:SetZoom(curZoom);

		-- re-calculate all Minimap Icon positions
		if ( frame:IsVisible() ) then
			self:CalculateMinimapIconPositions(true);
		end

	elseif ( event == "PLAYER_LEAVING_WORLD" ) then
		frame:Hide(); -- yes, I know this is redunant
		self:RemoveAllMinimapIcons(); --dump all minimap icons
		-- TODO: when I uncouple the point buffer from Minimap drawing,
		--       I should consider updating LastPlayerPosition here

	elseif ( event == "PLAYER_ENTERING_WORLD" ) then
		frame:Show();
		if not ( frame:IsVisible() ) then
			-- do the minimap recalculation anyways if the OnShow script didn't execute
			-- this is done to ensure the accuracy of information about icons that were
			-- inserted while the Player was in the process of zoning
			self:CalculateMinimapIconPositions(true);
		end

	elseif ( event == "ZONE_CHANGED_NEW_AREA" ) then
		frame:Hide();
		frame:Show();

	end
end

function Astrolabe:OnUpdate( frame, elapsed )
	-- on-edge group changed call-backs
	if ( self.IconsOnEdgeChanged ) then
		self.IconsOnEdgeChanged = false;
		for func in pairs(self.IconsOnEdge_GroupChangeCallbacks) do
			pcall(func);
		end
	end

	self:UpdateMinimapIconPositions();
end

function Astrolabe:OnShow( frame )
	-- set the world map to a zoom with a valid player position
	if not ( self.WorldMapVisible ) then
		SetMapToCurrentZone();
	end
	local M, F = Astrolabe:GetCurrentPlayerPosition();
	if ( M and M >= 0 ) then
		--SetMapByID(M);
		--SetDungeonMapLevel(F);
	else
		frame:Hide();
		return
	end

	-- re-calculate minimap icon positions (if needed)
	if ( next(self.MinimapIcons) ) then
		self:CalculateMinimapIconPositions(true);
	else
		-- needed so that the cycle doesn't overwrite an updated LastPlayerPosition
		resetIncrementalUpdate = true;
	end

	if ( self.MinimapIconCount <= 0 ) then
		-- no icons left to manage
		frame:Hide();
	end
end

function Astrolabe:OnHide( frame )
	-- dump the new icons cache here
	-- a full update will performed the next time processing is re-actived
	self:DumpNewIconsCache()
end

-- called by AstrolabMapMonitor when all world maps are hidden
function Astrolabe:AllWorldMapsHidden()
	if ( IsLoggedIn() ) then
		self.processingFrame:Hide();
		self.processingFrame:Show();
	end
end

--------------------------------------------------------------------------------------------------------------
-- Library Registration
--------------------------------------------------------------------------------------------------------------

local function harvestMapData( HarvestedMapData )
	local mapData = {}
	local mapName,_,_,_,microName = GetMapInfo();
	local mapID = GetCurrentMapAreaID();
	local numFloors, firstFloor = GetNumDungeonMapLevels();
	--numFloors = max(numFloors,1)  -- sinus: some areas report numFloors==0 which prevents scanning even the current one... circumvent this.
	firstFloor=firstFloor or 0
	local usesTerrainMap = DungeonUsesTerrainMap();
	mapData.mapName = mapName;
	mapData.cont = (GetCurrentMapContinent()) or -100;
	mapData.zone = (GetCurrentMapZone()) or -100;
	mapData.numFloors = numFloors;
	local _, TLx, TLy, BRx, BRy = GetCurrentMapZone();
	if TLx and TLy and BRx and BRy and (firstFloor==0 or usesTerrainMap) then  -- we have a "ground floor"
		mapData[0] = {};
		if not ( TLx < BRx ) then
			TLx = -TLx;
			BRx = -BRx;
		end
		if not ( TLy < BRy) then
			TLy = -TLy;
			BRy = -BRy;
		end
		mapData[0].TLx = TLx;
		mapData[0].TLy = TLy;
		mapData[0].BRx = BRx;
		mapData[0].BRy = BRy;
	end
	--for f=firstFloor , firstFloor+numFloors-1 do  --sinus:MoP the floor! o ho ho ho.
	for f=1 , firstFloor+numFloors do  --sinus:Why not go full monty...
		SetDungeonMapLevel(f)
		local lev, TLx, TLy, BRx, BRy = GetCurrentMapDungeonLevel()
		if lev==f then
			if lev and not TLx then
				if usesTerrainMap then
					TLx,TLy,BRx,BRy = mapData[0].TLx,mapData[0].TLy,mapData[0].BRx,mapData[0].BRy
					mapData[0] = nil
				else
					TLx,TLy,BRx,BRy=0,0,3200,1600 -- sinus: bad fix for "uncoorded" maps
				end
			end
			if ( TLx and TLy and BRx and BRy ) then
				mapData[lev] = {};
				if not ( TLx < BRx ) then
					TLx = -TLx;
					BRx = -BRx;
				end
				if not ( TLy < BRy) then
					TLy = -TLy;
					BRy = -BRy;
				end
				mapData[lev].TLx = TLx;
				mapData[lev].TLy = TLy;
				mapData[lev].BRx = BRx;
				mapData[lev].BRy = BRy;
			end
			mapData[lev].microName = microName
			mapData[lev].floorName = _G['DUNGEON_FLOOR_' .. mapName:upper() .. (f - (usesTerrainMap and 1 or 0) )]  --sinus: additional crap for manual microdungeon scanning
		end
	end

	HarvestedMapData[mapID] = mapData;
end

local function activate( newInstance, oldInstance )
	if ( oldInstance ) then -- this is an upgrade activate
		if ( oldInstance.DumpNewIconsCache ) then
			oldInstance:DumpNewIconsCache()
		end
		for k, v in pairs(oldInstance) do
			if ( type(v) ~= "function" and (not configConstants[k]) ) then
				newInstance[k] = v;
			end
		end
		-- sync up the current MinimapIconCount value
		local iconCount = 0
		for _ in pairs(newInstance.MinimapIcons) do
			iconCount = iconCount + 1
		end
		newInstance.MinimapIconCount = iconCount

		Astrolabe = oldInstance;
	else
		local frame = CreateFrame("Frame");
		newInstance.processingFrame = frame;

		newInstance.HarvestedMapData = {};
		local HarvestedMapData = newInstance.HarvestedMapData;

		do_zone_aliasing = false

		newInstance.ContinentList = { GetMapContinents() };

		for C in pairs(newInstance.ContinentList) do
			local zones = { GetMapZones(C) };
			newInstance.ContinentList[C] = zones;
			SetMapZoom(C, 0);
			zones[0] = GetCurrentMapAreaID();
			harvestMapData(HarvestedMapData);
			for Z in ipairs(zones) do
				SetMapZoom(C, Z);
				zones[Z] = GetCurrentMapAreaID();
				harvestMapData(HarvestedMapData);
			end
		end

		for _, id in ipairs(GetAreaMaps()) do
			if not ( HarvestedMapData[id] ) then
				if ( SetMapByID(id) ) then
					harvestMapData(HarvestedMapData);
				end
			end
		end

		HarvestedMapData[683]=nil  -- sinus: kill the weird Hyjal
		HarvestedMapData[770]=nil  -- sinus: kill the weird Twilight Highlands
		HarvestedMapData[748]=nil  -- sinus: kill the weird Uldum

		-- sinus: cache map names
		for id,data in pairs(HarvestedMapData) do
			local name = GetMapNameByID(id)
			if name then
				newInstance.MapIDsByName[name]=id
			end
		end


		for new,orig in pairs(zone_aliases) do --sinus...
			--[=[
			local gooddata
			for i=1,#data do
				local z = HarvestedMapData[data[i]]
				if z and z.zone and z.zone>0 then gooddata=z break end
			end
			if gooddata then
				for i=1,#data do
					local z = HarvestedMapData[data[i]]
					if not z or z.zone==0 then z.zone=gooddata.zone break end
				end
			end
			--]=]

			local good = HarvestedMapData[orig]
			if good and good.zone and good.zone>0 then
				if not HarvestedMapData[new] then HarvestedMapData[new]={} end
				HarvestedMapData[new].zone=good.zone
			end
		end

		do_zone_aliasing = true

	end
	configConstants = nil -- we don't need this anymore

	local frame = newInstance.processingFrame;
	frame:Hide();
	frame:SetParent("Minimap");
	frame:UnregisterAllEvents();
	frame:RegisterEvent("MINIMAP_UPDATE_ZOOM");
	frame:RegisterEvent("PLAYER_LEAVING_WORLD");
	frame:RegisterEvent("PLAYER_ENTERING_WORLD");
	frame:RegisterEvent("ZONE_CHANGED_NEW_AREA");
	frame:SetScript("OnEvent",
		function( frame, event, ... )
			Astrolabe:OnEvent(frame, event, ...);
		end
	);
	frame:SetScript("OnUpdate",
		function( frame, elapsed )
			Astrolabe:OnUpdate(frame, elapsed);
		end
	);
	frame:SetScript("OnShow",
		function( frame )
			Astrolabe:OnShow(frame);
		end
	);
	frame:SetScript("OnHide",
		function( frame )
			Astrolabe:OnHide(frame);
		end
	);

	setmetatable(Astrolabe.MinimapIcons, MinimapIconsMetatable)
end

DongleStub:Register(Astrolabe, activate)


--------------------------------------------------------------------------------------------------------------
-- Data
--------------------------------------------------------------------------------------------------------------

-- diameter of the Minimap in game yards at
-- the various possible zoom levels
MinimapSize = {
	indoor = {
		[0] = 300, -- scale
		[1] = 240, -- 1.25
		[2] = 180, -- 5/3
		[3] = 120, -- 2.5
		[4] = 80,  -- 3.75
		[5] = 50,  -- 6
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

ValidMinimapShapes = {
	-- { upper-left, lower-left, upper-right, lower-right }
	["SQUARE"]                = { false, false, false, false },
	["CORNER-TOPLEFT"]        = { true,  false, false, false },
	["CORNER-TOPRIGHT"]       = { false, false, true,  false },
	["CORNER-BOTTOMLEFT"]     = { false, true,  false, false },
	["CORNER-BOTTOMRIGHT"]    = { false, false, false, true },
	["SIDE-LEFT"]             = { true,  true,  false, false },
	["SIDE-RIGHT"]            = { false, false, true,  true },
	["SIDE-TOP"]              = { true,  false, true,  false },
	["SIDE-BOTTOM"]           = { false, true,  false, true },
	["TRICORNER-TOPLEFT"]     = { true,  true,  true,  false },
	["TRICORNER-TOPRIGHT"]    = { true,  false, true,  true },
	["TRICORNER-BOTTOMLEFT"]  = { true,  true,  false, true },
	["TRICORNER-BOTTOMRIGHT"] = { false, true,  true,  true },
}

--local is_patch_5 = (select(4,GetBuildInfo()))>=50001

-- distances across and offsets of the world maps
-- in game yards
WorldMapSize = {
	[0] = {
		height = 59325.116949553*.666,
		system = 0,
		systemParent = 0,
		width = 59325.116949553,
		xOffset = 0,
		yOffset = 0,
		[13] = {
			xOffset = -10259.733721251,
			yOffset = -19751.863527253,
		},
		[14] = {
			xOffset = -48226.917881395,
			yOffset = -16411.787024772,
		},
		[485] = {
			xOffset = -31204.00, -- -31150.488530038,
			yOffset = -11883.32, -- -11809.200871227,
			--[[ okay, these get weird by surveys :/
			Surveys say: -31142.54 -11966.32
			Manual adjustments go: -31204.00 -11883.32
			--]]
		},
		[862] = {
			xOffset = -31387.80695199,
			--xOffset = -32236.0,
			yOffset = -34078.244798818,
			--yOffset = -34522.0,
		},
	},
	[13] = {
		height = 24533.19945,
		systemParent = 0,
		width = 36799.81197,
		xOffset = -17066.60126,
		yOffset = -12799.89961,
	},
	[14] = {
		height = 27149.68748,
		systemParent = 0,
		width = 40741.17907,
		xOffset = -18171.96844,
		yOffset = -11176.34367,
	},
	[321] = {
		{ -- [1]
			height = 1159.5835,
			width = 1739.375,
			xOffset = 3506.354,
			yOffset = -2486.66675,
		},
		{ -- [2]
			height = 241.39025,
			width = 362.08962,
			xOffset = 4163.96713,
			yOffset = -1932.27183,
		},
		xOffset = 8690.02719,
		yOffset = -3623.15183,
	},
	[401] = {
		height = 2825.00046,
		width = 4237.49828,
		xOffset = 0,
		yOffset = 0,
	},
	[443] = {
		height = 764.58325,
		width = 1145.83472,
		xOffset = 0,
		yOffset = 0,
	},
	[461] = {
		height = 1170.83309,
		width = 1756.25023,
		xOffset = 0,
		yOffset = 0,
	},
	[462] = {
		height = 3283.33296,
		width = 4924.99935,
		xOffset = 2087.50023,
		yOffset = -8641.66578,
	},
	[463] = {
		height = 2200.0001,
		width = 3300.00085,
		xOffset = 2883.33272,
		yOffset = -5866.66614,
	},
	[464] = {
		height = 2714.58133,
		width = 4070.83028,
		xOffset = -7099.9985,
		yOffset = -7339.58233,
	},
	[466] = {
		height = 11642.71843,
		systemParent = 466,
		width = 17464.0779,
		xOffset = -12996.03902,
		yOffset = -5821.35914,
	},
	[471] = {
		height = 704.68794,
		width = 1056.7699,
		xOffset = -6533.63241,
		yOffset = -6523.64995,
	},
	[476] = {
		height = 2174.99908,
		width = 3262.50031,
		xOffset = -7525.00003,
		yOffset = -9374.99943,
	},
	[480] = {
		height = 806.7719,
		width = 1211.45871,
		xOffset = 4000.74932,
		yOffset = -7753.70939,
	},
	[482] = {
		height = 1514.58311,
		width = 2270.83529,
		xOffset = 0,
		yOffset = 0,
	},
	[485] = {
		height = 11834.26515,
		systemParent = 0,
		width = 17751.39839,
		xOffset = -9217.15231,
		yOffset = -10593.37487,
	},
	[499] = {
		height = 2218.75027,
		width = 3327.08362,
		xOffset = 2902.08234,
		yOffset = -11168.74965,
	},
	[502] = {
		height = 2108.33411,
		width = 3162.49385,
		xOffset = 0,
		yOffset = 0,
	},
	[504] = {  -- Dalaran
		{ -- [1]
			height = 553.33995,
			width = 830.01643,
			xOffset = -1052.51109,
			yOffset = -6066.67115,
		},
		{ -- [2]
			height = 375.48927,
			width = 563.22279,
			xOffset = -915.86865,
			yOffset = -5975.33259,
		},
		xOffset = -1270.796,
		yOffset = -11581.57677,
	},
	[512] = {
		height = 1162.50002,
		width = 1743.74946,
		xOffset = 0,
		yOffset = 0,
	},
	[521] = {
		{ -- [1]
			height = 1216.66649,
			width = 1824.99985,
			xOffset = 435.33678,
			yOffset = 2235.80349,
		},
	},
	[529] = {
		{ -- [1]
			height = 2191.66598,
			width = 3287.50074,
			xOffset = -1804.35279,
			yOffset = 2062.9701,
		},
	},
	[531] = {
		height = 774.99991,
		width = 1162.49961,
		xOffset = 0,
		yOffset = 0,
	},
	[540] = {
		height = 1766.66716,
		width = 2650.00164,
		xOffset = 0,
		yOffset = 0,
	},
	[544] = {
		system = 544,
	},
	[602] = {
		height = 1022.91668,
		width = 1533.33347,
		xOffset = 0,
		yOffset = 0,
	},
	[605] = {
		system = 605,
	},
	[609] = {
		height = 502.08329,
		width = 752.08331,
		xOffset = 0,
		yOffset = 0,
	},
	[611] = {
		height = 593.74988,
		width = 889.58325,
		xOffset = 0,
		yOffset = 0,
	},
	[626] = {
		height = 810.41329,
		width = 1214.58151,
		xOffset = 0,
		yOffset = 0,
	},
	[640] = {
		system = 640,
	},
	[677] = {
		height = 593.74988,
		width = 889.58325,
		xOffset = 0,
		yOffset = 0,
	},
	[679] = { --Gilneas
		height = 2097.91669,
		width = 3145.83325,
		xOffset = 0,
		yOffset = 0,
	},
	[686] = {
		height = 922.9166,
		width = 1383.33334,
		xOffset = 0,
		yOffset = 0,
	},
	[708] = {
		height = 1343.75002,
		width = 2014.58153,
		xOffset = -4810.41584,
		yOffset = 2160.41647,
	},
	[709] = {
		height = 1224.99954,
		width = 1837.49974,
		xOffset = -5212.4997,
		yOffset = 1222.91666,
	},
	[717] = {
		height = 1674.99988,
		width = 2512.4912,
		xOffset = 0,
		yOffset = 0,
	},
	[736] = {
		height = 868.74697,
		width = 1302.08448,
		xOffset = 0,
		yOffset = 0,
	},
	[737] = {
		system = 737,
	},
	[747] = {
		height = 647.91734,
		width = 970.83627,
		xOffset = 0,
		yOffset = 0,
	},
	[753] = {  -- Blackrock Caverns
		{ -- [1]
			height = nil,
			width = nil,
			xOffset = nil,
			yOffset = nil,
		},
		{ -- [2]
			height = nil,
			width = nil,
			xOffset = 639,
			yOffset = nil
		},
	},
-- sinus:
	[800] = {
		{ -- [1]
			height = 1058.3332824707,
			width = 1587.4999389648,
			xOffset = -718.75,
			yOffset = -424.99996948242,
		},
	},
	[819] = {
		-- Hour of Twilight main map surrogate
		[1] = {
			width = 7000,
			height = 7000*0.6666,
			xOffset = 0,
			yOffset = 0,
		},
	},
	[820] = {
		-- End Time. Floor 1, Entryway of Time, doesn't return all coords for a GetNumDungeonMapLevels() call, as expected, and is thus not harvested properly. Here's what GetCurrentMapZone() returns, instead. Recalculated... somewhat.
		[1] = {
			width = 3200,
			height = 3200*0.6666,
			xOffset = 0,
			yOffset = 0,
		},
	},
	-- :sinus
	[862] = {
		height = 10343.54145,
		systemParent = 0,
		width = 15515.30029,
		xOffset = -8750,
		yOffset = -6680,
	},
	[895] = {
	},

	[750] = { [2]={} },
	[691] = { [2]={},[3]={},[4]={}},
	[688] = { [2]={},[3]={} },

	[893] = {
		xOffset = 2980,
		yOffset = -8435,
	},

	[928] = {
		xOffset = -6600,
		yOffset = -6200,
		width = 2451,
		height = 2451*0.666,
	},

	-- Taxi maps!! They come in the form of TAXIMAP###, just offsetting it by 2000.
	-- /dump tonumber(TaxiFrame.InsetBg:GetTexture():match("TAXIMAP(%d+)"))+2000
	-- /run W=Astrolabe.WorldMapSize[2571] W.width,W.height,W.xOffset,W.yOffset=12000,12000,-9000,-10000
	[2000] = { -- eastern
		height = 29000,
		width = 29000,
		xOffset = -12300,
		yOffset = -12400,
		system=14,
		systemParent = 0,
	},
	[2001] = { -- kalimdor
		height = 24200,
		width = 24200,
		xOffset = -10900,
		yOffset = -12500,
		system=13,
		systemParent = 0,
	},
	[2530] = { -- outlands
		height = 12500,
		width = 12500,
		xOffset = -10800,
		yOffset = -6600,
		system=466,
		systemParent = 0,
	},
	[2571] = { -- northrend
		height = 15500,
		width = 15500,
		xOffset = -8500,
		yOffset = -11700,
		system=485,
		systemParent = 0,
	},
	[2870] = { -- pandaria
		width=11800,
		height=11800,
		xOffset=-6750,
		yOffset=-7350,
		system=862,
		systemParent = 0
	},
}

WorldMapSize[750][2].yOffset=870  -- Maraudon floor 2 is waaaay inaccurate.
WorldMapSize[691][2].xOffset,WorldMapSize[691][2].yOffset=-480,-675  -- Gnomeregan is a NIGHTMARE.
WorldMapSize[691][3].xOffset,WorldMapSize[691][3].yOffset=-530,-491  -- Gnomeregan is a NIGHTMARE.
WorldMapSize[691][4].xOffset,WorldMapSize[691][4].yOffset=-726,-516  -- Gnomeregan is a NIGHTMARE.
WorldMapSize[688][2].xOffset,WorldMapSize[688][2].yOffset=-181,-427
WorldMapSize[688][3].xOffset,WorldMapSize[688][3].yOffset=117,-69

MicroDungeonSize = {}

local function zeroDataFunc(tbl, key)
	if ( type(key) == "number" ) then
		return zeroData;
	else
		return 0;
	end
end

zeroData = { xOffset = 0, height = 0, yOffset = 0, width = 0, __index = zeroDataFunc };
setmetatable(zeroData, zeroData);

-- get data on useful transforms
-- sinus: Adding this "just in case". No idea what these are for, yet.
local TRANSFORMS = {}
Astrolabe.TRANSFORMS = TRANSFORMS
for _, ID in ipairs(GetWorldMapTransforms()) do
	local terrainMapID, newTerrainMapID, _, _, transformMinY, transformMaxY, transformMinX, transformMaxX, offsetY, offsetX = GetWorldMapTransformInfo(ID)
	if ( offsetX ~= 0 or offsetY ~= 0 ) then
		TRANSFORMS[ID] = {
			terrainMapID = terrainMapID,
			newTerrainMapID = newTerrainMapID,
			BRy = -transformMinY,
			TLy = -transformMaxY,
			BRx = -transformMinX,
			TLx = -transformMaxX,
			offsetY = offsetY,
			offsetX = offsetX,
		}
	end
end

for mapID, harvestedData in pairs(Astrolabe.HarvestedMapData) do
	local terrainMapID = GetAreaMapInfo(mapID)
	local mapData = WorldMapSize[mapID]
	if not mapData then mapData={} WorldMapSize[mapID]=mapData end  -- not yet metatabled, so could return nil; fill it

		--if ( harvestedData.numFloors > 0 ) then
	for f, harvData in pairs(harvestedData) do
		if ( type(f) == "number" ) then --and f > 0
			local floorData
			if f>0 then
				if not ( mapData[f] ) then
					mapData[f] = {}
				end
				floorData = mapData[f]
			else
				floorData = mapData
			end
			floorData.width = floorData.width or (harvData.BRx - harvData.TLx)
			floorData.height = floorData.height or (harvData.BRy - harvData.TLy)
			floorData.xOffset = floorData.xOffset or harvData.TLx
			floorData.yOffset = floorData.yOffset or harvData.TLy

			floorData.mapName = harvData.mapName
			floorData.floorName = harvData.floorName
		end
	end
	mapData[0]=nil

	if not ( next(mapData, nil) ) then
		mapData.width,mapData.height,mapData.xOffset,mapData.yOffset = 1,1,0,0
		-- if this is an outside continent level or world map then throw up an extra warning
		--[[
		if ( harvestedData.cont > 0 and harvestedData.zone == 0 ) then
			printError(("Astrolabe is missing data for world map %s [%d] (%d, %d)."):format(harvestedData.mapName, mapID, harvestedData.cont, harvestedData.zone));
		end
		--]]
	end

	--[[
	for f = 1, harvestedData.numFloors do
		if not ( mapData[f] ) then
			printError(("Astrolabe is missing data for %s [%d], floor %d."):format(harvestedData.mapName, mapID, f));
		end
	end
	--]]
			-- TODO: handle floored world maps

		--[[
		else
			local harvData = harvestedData[0]
			if not ( mapData.width ) then
				mapData.width = harvData.BRx - harvData.TLx
			end
			if not ( mapData.height ) then
				mapData.height = harvData.BRy - harvData.TLy
			end
			if not ( mapData.xOffset ) then
				mapData.xOffset = harvData.TLx
			end
			if not ( mapData.yOffset ) then
				mapData.yOffset = harvData.TLy
			end

		end
		--]]

	--[=[
	else
		mapData = {};

		--if ( harvestedData.numFloors > 0 ) then
			for f, harvData in pairs(harvestedData) do
				if ( type(f) == "number" ) then --and f > 0
					mapData[f] = {};
					local floorData = mapData[f]
					floorData.width = harvData.BRx - harvData.TLx
					floorData.height = harvData.BRy - harvData.TLy
					floorData.xOffset = harvData.TLx
					floorData.yOffset = harvData.TLy
				end
			end
			for f = 1, harvestedData.numFloors do
				if not ( mapData[f] ) then
					printError(("Astrolabe is missing data for %s [%d], floor %d."):format(harvestedData.mapName, mapID, f));
				end
			end

		--[[else
			local harvData = harvestedData[0]
			if ( harvData ) then
				mapData.width = harvData.BRx - harvData.TLx
				mapData.height = harvData.BRy - harvData.TLy
				mapData.xOffset = harvData.TLx
				mapData.yOffset = harvData.TLy

			else
				printError(("Astrolabe is missing data for %s [%d]."):format(harvestedData.mapName, mapID));

			end

		end
		--]]


		-- if we don't have any data, we're gonna use zeroData, but we also need to
		-- setup the system and systemParent IDs so things don't get confused
		if not ( next(mapData, nil) ) then
			mapData = { xOffset = 0, height = 1, yOffset = 0, width = 1 };
			-- if this is an outside continent level or world map then throw up an extra warning
			if ( harvestedData.cont and harvestedData.cont > 0 and harvestedData.zone == 0 ) then
				printError(("Astrolabe is missing data for world map %s [%d] (%d, %d)."):format(harvestedData.mapName, mapID, harvestedData.cont, harvestedData.zone));
			end
		end

		-- store the data in the WorldMapSize DB
		WorldMapSize[mapID] = mapData;

	end
	--]=]

	-- setup system and systemParent IDs
	if ( mapData and mapData ~= zeroData ) then
		if not ( mapData.system ) then
			mapData.system = mapID;

			if ( harvestedData.cont and harvestedData.zone and harvestedData.cont > 0 and harvestedData.zone > 0 ) then
				mapData.system = Astrolabe:GetMapID(harvestedData.cont, nil);
			end
		end
		if not ( mapData.systemParent ) then
			mapData.systemParent = mapID;

			local systemData = WorldMapSize[mapData.system];
			if ( systemData and systemData.systemParent ) then
				mapData.systemParent = systemData.systemParent;
			end
		end

		-- systemParent sanity checks
		if ( mapData.system ~= mapData.systemParent ) then
			if not ( WorldMapSize[mapData.systemParent] and WorldMapSize[mapData.systemParent][mapData.system] ) then
				printError("Astrolabe detected a child system that the parent doesn't know about.  VERY BAD!!!");
			end
		end

		-- determine terrainMapID for micro-dungeons
		if ( harvestedData.cont > 0 and harvestedData.zone > 0 ) then
			MicroDungeonSize[terrainMapID] = {}
		end

		setmetatable(mapData, zeroData);
	end
end

setmetatable(WorldMapSize[0], zeroData); -- special case for World Map


-- micro dungeons
-- Just as a test; we have microdungeons as a separate, hand-gathered table, so probably no need for that shit here.
for _, ID in ipairs(GetDungeonMaps()) do
	local floorIndex, minX, maxX, minY, maxY, terrainMapID, parentWorldMapID = GetDungeonMapInfo(ID);
	local TLx, TLy, BRx, BRy = -maxX, -maxY, -minX, -minY
	-- apply any necessary transforms
	local transformApplied = false
	for transformID, transformData in pairs(TRANSFORMS) do
		if ( transformData.terrainMapID == terrainMapID ) then
			if ( (transformData.TLx < TLx and BRx < transformData.BRx) and (transformData.TLy < TLy and BRy < transformData.BRy) ) then
				TLx = TLx - transformData.offsetX;
				BRx = BRx - transformData.offsetX;
				BRy = BRy - transformData.offsetY;
				TLy = TLy - transformData.offsetY;
				terrainMapID = transformData.newTerrainMapID;
				transformApplied = true;
				break;
			end
		end
	end
	if ( MicroDungeonSize[terrainMapID] ) then
		-- only consider systems that can have micro dungeons
		if ( MicroDungeonSize[terrainMapID][floorIndex] and not transformApplied ) then
			printError("Astrolabe detected a duplicate microdungeon floor!", terrainMapID, ID);
		end
		MicroDungeonSize[terrainMapID][floorIndex] = {
			width = BRx - TLx,
			height = BRy - TLy,
			xOffset = TLx,
			yOffset = TLy,
		};
	end
end

for _, data in pairs(MicroDungeonSize) do
	setmetatable(data, zeroData);
end
setmetatable(MicroDungeonSize, zeroData);

-- make sure we don't have any EXTRA data hanging around
for mapID, mapData in pairs(WorldMapSize) do
	if ( mapID ~= 0 and getmetatable(mapData) ~= zeroData ) then
		printError("Astrolabe has hard coded data for an invalid map ID", mapID);
	end
end

setmetatable(WorldMapSize, zeroData); -- setup the metatable so that invalid map IDs don't cause Lua errors

-- register this library with AstrolabeMapMonitor, this will cause a full update if PLAYER_LOGIN has already fired
local AstrolabeMapMonitor = DongleStub("AstrolabeMapMonitor");
AstrolabeMapMonitor:RegisterAstrolabeLibrary(Astrolabe, LIBRARY_VERSION_MAJOR);

Astrolabe.WorldMapSize = WorldMapSize
Astrolabe.MicroDungeonSize = MicroDungeonSize



-- SINUS: Pandaria's micro dungeons are a BIG PILE OF POOP to work with. :[

MICRODUNGEONS = {

	-- SINUS: microdungeons! Export these using Astrolabe:CalculateMicroDungeon() when in a cave.
	  [4] = {
	    [10] = {
	      floorName = "Tiragarde Keep",
	      height = 83.333999633789,
	      microName = "TiragardeKeep",
	      width = 125.0009765625,
	      xOffset = 5052.4995117188,
	      yOffset = 203.33299255371,
	    },
	    [11] = {
	      floorName = "Great Hall",
	      height = 83.333999633789,
	      microName = "TiragardeKeep",
	      width = 125.0009765625,
	      xOffset = 5052.4995117188,
	      yOffset = 203.33299255371,
	    },
	    [12] = {
	      floorName = "Skull Rock",
	      height = 180,
	      microName = "SkullRock",
	      width = 270,
	      xOffset = 4629,
	      yOffset = -1580.5,
	    },
	    [19] = {
	      floorName = "Dustwind Cave",
	      height = 172,
	      microName = "DustwindCave",
	      width = 258,
	      xOffset = 4622.5,
	      yOffset = -971.25,
	    },
	    [8] = {
	      floorName = "Burning Blade Coven",
	      height = 177.33334064484,
	      microName = "BurningBladeCoven",
	      width = 266,
	      xOffset = 4166.5,
	      yOffset = -5.5003328323364,
	    },
	  },
	  [9] = {
	    [6] = {
	      floorName = "Palemane Rock",
	      height = 233.33984375,
	      microName = "PalemaneRock",
	      width = 350.01000976563,
	      xOffset = -580.00500488281,
	      yOffset = 2263.330078125,
	     },
	    [7] = {
	      floorName = "The Venture Co. Mine",
	      height = 494,
	      microName = "TheVentureCoMine",
	      width = 741,
	      xOffset = 818,
	      yOffset = 1436,
	    },
	  },
	  [11] = {
	    [20] = {
	      floorName = "Wailing Cavern Entrance",
	      height = 380,
	      microName = "WailingCavernsBarrens",
	      width = 570,
	      xOffset = 1902.5,
	      yOffset = 490,
	    },
 	  },
	  [17] = {
	    [18] = {
	      floorName = "Uldaman Entrance",
	      height = 375,
	      microName = "Uldaman",
	      width = 562.5,
	      xOffset = 2748.75,
	      yOffset = 5955,
	    },
	  },
	  [20] = {
	    [13] = {
	      floorName = "Scarlet Monastery Entrance",
	      height = 136.66015625,
	      microName = "ScarletMonasteryEntrance",
	      width = 204.990234375,
	      xOffset = 660.0048828125,
	      yOffset = -2948.830078125,
	    },
	  },
	  [27] = {
	    [6] = {
	      height = 220,
	      width = 330,
	      xOffset = -245,
	      yOffset = 6020,
	      microName = "ColdridgePass",
	      floorName = "Coldridge Pass"
	    },
	    [7] = {
	      height = 337,
	      width = 505.5,
	      xOffset = -32.75,
	      yOffset = 5313,
	      microName = "TheGrizzledDen",
	      floorName = "The Grizzled Den"
	    },
	    [10] = {
	      floorName = "Gnomeregan",
	      height = 380,
	      microName = "Gnomeregan",
	      width = 570,
	      xOffset = -1070,
	      yOffset = 4862.5,
	    },
	    [11] = {
	      height = 249.33984375,
	      width = 374.009765625,
	      xOffset = 1542.9951171875,
	      yOffset = 5486.830078125,
	      microName = "GolBolarQuarry",
	      floorName = "Gol'Bolar Quarry"
	    },
	  },
	  [29] = {
	    [14] = {
	      floorName = "Blackrock Spire",
	      height = 475,
	      microName = "BlackrockMountain",
	      width = 712.5,
	      xOffset = 761.25,
	      yOffset = 7325,
	    },
	    [15] = {
	      floorName = "Blackrock Caverns",
	      height = 170,
	      microName = "BlackrockMountain",
	      width = 255,
	      xOffset = 1145,
	      yOffset = 7480,
	    },
	    [16] = {
	      floorName = "Blackrock Depths",
	      height = 506.6826171875,
	      microName = "BlackrockMountain",
	      width = 760.02398681641,
	      xOffset = 627.48602294922,
	      yOffset = 7086.6586914063,
	    },
	  },
	  [28] = { -- Searing Gorge, cloning Blackrock here, since it's retarded.
	    [14] = {
	      floorName = "Blackrock Spire",
	      height = 475,
	      microName = "BlackrockMountain",
	      width = 712.5,
	      xOffset = 761.25,
	      yOffset = 7325,
	    },
	    [15] = {
	      floorName = "Blackrock Caverns",
	      height = 170,
	      microName = "BlackrockMountain",
	      width = 255,
	      xOffset = 1145,
	      yOffset = 7480,
	    },
	    [16] = {
	      floorName = "Blackrock Depths",
	      height = 506.6826171875,
	      microName = "BlackrockMountain",
	      width = 760.02398681641,
	      xOffset = 627.48602294922,
	      yOffset = 7086.6586914063,
	    },
	  },
	  [30] = {
	    [1] = {
	      floorName = "Fargodeep Mine",
	      height = 160,
	      microName = "FargodeepMine",
	      width = 240,
	      xOffset = -281.75,
	      yOffset = 9700,
	    },
	    [2] = {
	      floorName = "Lower Mines",
	      height = 170,
	      microName = "FargodeepMine",
	      width = 255,
	      xOffset = -289.25,
	      yOffset = 9710,
	    },
	    [19] = {
	      floorName = "Jasperlode Mine",
	      height = 215.5,
	      microName = "JasperlodeMine",
	      width = 323.25,
	      xOffset = 445.875,
	      yOffset = 8985,
	    },
	  },
	  [39] = {
	    [17] = {
	      floorName = "The Deadmines Entrance",
	      height = 300,
	      microName = "DeadminesWestfall",
	      width = 450,
	      xOffset = -1792.5,
	      yOffset = 11055,
	    },
	    [5] = {
	      floorName = "Jangolode Mine",
	      height = 184.666015625,
	      microName = "JangolodeMine",
	      width = 277,
	      xOffset = -1570.5,
	      yOffset = 9798.6669921875,
	    },
	  },
	  [41] = {
	    [2] = {
	      floorName = "Shadowthread Cave",
	      height = 320,
	      microName = "ShadowthreadCave",
	      width = 480,
	      xOffset = -1140,
	      yOffset = -11035,
	    },
	    [3] = {
	      floorName = "Fel Rock",
	      height = 186.400390625,
	      microName = "FelRock",
	      width = 279.6005859375,
	      xOffset = -1249.8002929688,
	      yOffset = -10211.700195313,
	    },
	    [4] = {
	      floorName = "Upper Den",
	      height = 153.33984375,
	      microName = "BanethilBarrowDen",
	      width = 230.009765625,
	      xOffset = -1685.0048828125,
	      yOffset = -9881.669921875,
	    },
	    [5] = {
	      floorName = "Lower Den",
	      height = 253.33984375,
	      microName = "BanethilBarrowDen",
	      width = 380.009765625,
	      xOffset = -1735.0048828125,
	      yOffset = -9931.669921875,
	    },
	  },
	  [101] = {
	    [21] = {
	      floorName = "The Wicked Grotto",
	      height = 400,
	      microName = "MaraudonOutside",
	      width = 600,
	      xOffset = -3085.5,
	      yOffset = 1250,
	    },
	    [22] = {
	      floorName = "Foulspore Cavern",
	      height = 373.3466796875,
	      microName = "MaraudonOutside",
	      width = 560.02001953125,
	      xOffset = -3165.0100097656,
	      yOffset = 1092.3266601563,
	    },
	  },
	  [161] = {
	    [15] = {
	      floorName = "The Noxious Lair",
	      height = 500,
	      microName = "TheNoxiousLair",
	      width = 750,
	      xOffset = 2190,
	      yOffset = 7640,
	    },
	    [16] = {
	      floorName = "The Gaping Chasm",
	      height = 590,
	      microName = "TheGapingChasm",
	      width = 885,
	      xOffset = 3612.5,
	      yOffset = 8900,
	    },
	    [17] = {
	      floorName = "Timeless Tunnel",
	      height = 738.33984375,
	      microName = "CavernsofTime",
	      width = 1107.509765625,
	      xOffset = 4163.7451171875,
	      yOffset = 7955.830078125,
	    },
	    [18] = {
	      floorName = "Caverns of Time",
	      height = 870.66650390625,
	      microName = "CavernsofTime",
	      width = 1306,
	      xOffset = 3707.5,
	      yOffset = 8036.6665039063,
	    },
	  },
	  [261] = {
	    [13] = {
	      floorName = "Twilights Run",
	      height = 168.33984375,
	      microName = "TwilightsRun",
	      width = 252.5099029541,
	      xOffset = -161.25500488281,
	      yOffset = 6183.330078125,
	    },
	  },
	  [463] = {
	    [1] = {
	      floorName = "Amani Catacombs",
	      height = 200,
	      microName = "AmaniCatacombs",
	      width = 300,
	      xOffset = 4780,
	      yOffset = -5311,
	    },
	  },
	  [464] = {
	    [2] = {
	      floorName = "Tides' Hollow",
	      height = 250,
	      microName = "TidesHollow",
	      width = 375,
	      xOffset = -6225,
	      yOffset = -5490,
	    },
	    [3] = {
	      floorName = "Stillpine Hold",
	      height = 316.66015625,
	      microName = "StillpineHold",
	      width = 474.990234375,
	      xOffset = -5350, -- LIE: 12247.5546875,
	      yOffset = -7125, -- LIE: 3009.169921875,
	    },
	  },
	  [520] = {
	    [1] = {
	      floorName = "The Nexus",
	      height = 734.1875,
	      width = 1101.2809753418,
	      xOffset = -393.21200561523,
	      yOffset = -798.26300048828,
	    },
	  },
	  [521] = {
	    [1] = {
	      floorName = "The Road to Stratholme",
	      height = 1216.6665039063,
	      width = 1824.9999389648,
	      xOffset = 327.08331298828,
	      yOffset = 1081.25,
	    },
	    [2] = {
	      floorName = "Stratholme City",
	      height = 750.19995117188,
	      width = 1125.299987793,
	      xOffset = -1856.3599853516,
	      yOffset = -2641.9599609375,
	    },
	  },
	  [522] = {
	    [1] = {
	      floorName = "Ahn'Kahet",
	      height = 648.2790222168,
	      width = 972.41796875,
	      xOffset = 233.30200195313,
	      yOffset = -848.68402099609,
	    },
	  },
	  [523] = {
	    [1] = {
	      floorName = "Njorndir Preparation",
	      height = 489.72150039673,
	      width = 734.58099365234,
	      xOffset = -424.17498779297,
	      yOffset = -515.38800048828,
	    },
	    [2] = {
	      floorName = "Dragonflayer Ascent",
	      height = 320.72029304504,
	      width = 481.08100891113,
	      xOffset = -242.92500305176,
	      yOffset = -304.3869934082,
	    },
	    [3] = {
	      floorName = "Tyr's Terrace",
	      height = 491.05451202393,
	      width = 736.58100891113,
	      xOffset = -225.67500305176,
	      yOffset = -415.72100830078,
	    },
	  },
	  [524] = {
	    [1] = {
	      floorName = "Lower Pinnacle",
	      height = 365.95701599121,
	      width = 548.93601989746,
	      xOffset = 148.62300109863,
	      yOffset = -552.87701416016,
	    },
	    [2] = {
	      floorName = "Upper Pinnacle",
	      height = 504.1190032959,
	      width = 756.17994308472,
	      xOffset = -8.6219596862793,
	      yOffset = -661.9580078125,
	    },
	  },
	  [525] = {
	    [1] = {
	      floorName = "Unyielding Garrison",
	      height = 377.48999023438,
	      width = 566.23501586914,
	      xOffset = -283.68600463867,
	      yOffset = -1534.5400390625,
	    },
	    [2] = {
	      floorName = "Walk of the Makers",
	      height = 472.16003417969,
	      width = 708.23701477051,
	      xOffset = -169.68800354004,
	      yOffset = -1431.8800048828,
	    },
	  },
	  [526] = {
	    [1] = {
	      floorName = "Halls of Stone",
	      height = 613.46606445313,
	      width = 920.1960144043,
	      xOffset = -1126.5200195313,
	      yOffset = -1375.9100341797,
	    },
	  },
	  [527] = {
	    [1] = {
	      floorName = "The Eye of Eternity",
	      height = 286.71301269531,
	      width = 430.07006835938,
	      xOffset = -1466.7800292969,
	      yOffset = -897.84100341797,
	    },
	  },
	  [528] = {
	    [0] = {
	      height = 343.13897705078,
	      width = 514.70697021484,
	      xOffset = -1301.9599609375,
	      yOffset = -1220.2099609375,
	    },
	    [1] = {
	      floorName = "Band of Variance",
	      height = 343.13897705078,
	      width = 514.70697021484,
	      xOffset = -1301.9599609375,
	      yOffset = -1220.2099609375,
	    },
	    [2] = {
	      floorName = "Band of Acceleration",
	      height = 443.13897705078,
	      width = 664.70697021484,
	      xOffset = -1376.9599609375,
	      yOffset = -1370.2099609375,
	    },
	    [3] = {
	      floorName = "Band of Transmutation",
	      height = 343.13897705078,
	      width = 514.70697021484,
	      xOffset = -1301.9599609375,
	      yOffset = -1270.2099609375,
	    },
	    [4] = {
	      floorName = "Band of Alignment",
	      height = 196.46398925781,
	      width = 294.70098876953,
	      xOffset = -1191.9599609375,
	      yOffset = -1186.8699951172,
	    },
	  },
	  [529] = {
	    [1] = {
	      floorName = "The Grand Approach ",
	      height = 2191.6666259766,
	      width = 3287.4998779297,
	      xOffset = -1704.1666259766,
	      yOffset = -1022.9166259766,
	    },
	    [2] = {
	      floorName = "The Antechamber of Ulduar",
	      height = 446.30004882813,
	      width = 669.45098876953,
	      xOffset = -224.21600341797,
	      yOffset = -1839.0100097656,
	    },
	    [3] = {
	      floorName = "The Inner Sanctum of Ulduar",
	      height = 885.63989257813,
	      width = 1328.4609985352,
	      xOffset = -653.72100830078,
	      yOffset = -2564.6799316406,
	    },
	    [4] = {
	      floorName = "The Prison of Yogg-Saron",
	      height = 607,
	      width = 910.5,
	      xOffset = -594.75,
	      yOffset = -2219,
	    },
	    [5] = {
	      floorName = "The Spark of Imagination",
	      height = 1046.3000488281,
	      width = 1569.4599609375,
	      xOffset = -3254.4499511719,
	      yOffset = -3168.830078125,
	    },
	    [6] = {
	      floorName = "The Mind's Eye",
	      height = 412.97998046875,
	      width = 619.46899414063,
	      xOffset = -309.45498657227,
	      yOffset = -2247.75,
	    },
	  },
	  [530] = {
	    [1] = {
	      floorName = "Gundrak",
	      height = 603.35009765625,
	      width = 905.03305053711,
	      xOffset = -1164.9200439453,
	      yOffset = -2068.8701171875,
	    },
	  },
	  [532] = {
	    [1] = {
	      floorName = "Vault of Archavon",
	      height = 932.17001342773,
	      width = 1398.2550048828,
	      xOffset = -585.73602294922,
	      yOffset = -298.08999633789,
	    },
	  },
	  [533] = {
	    [1] = {
	      floorName = "The Brood Pit",
	      height = 501.98300170898,
	      width = 752.97399902344,
	      xOffset = -722.97399902344,
	      yOffset = -794.125,
	    },
	    [2] = {
	      floorName = "Hadronox's Lair",
	      height = 195.31597900391,
	      width = 292.97399902344,
	      xOffset = -692.97399902344,
	      yOffset = -645.78997802734,
	    },
	    [3] = {
	      floorName = "The Gilded Gate",
	      height = 245,
	      width = 367.5,
	      xOffset = -829.625,
	      yOffset = -640,
	    },
	  },
	  [534] = {
	    [1] = {
	      floorName = "The Vestibules of Drak'Tharon",
	      height = 413.29399108887,
	      width = 619.94100952148,
	      xOffset = 307.06900024414,
	      yOffset = 182.5659942627,
	    },
	    [2] = {
	      floorName = "Drak'Tharon Overlook",
	      height = 413.29399108887,
	      width = 619.94100952148,
	      xOffset = 382.06900024414,
	      yOffset = 182.5659942627,
	    },
	  },
	  [535] = {
	    [1] = {
	      floorName = "The Construct Quarter",
	      height = 729.21997070313,
	      width = 1093.830078125,
	      xOffset = 2640.2700195313,
	      yOffset = -3615.830078125,
	    },
	    [2] = {
	      floorName = "The Arachnid Quarter",
	      height = 729.21997070313,
	      width = 1093.830078125,
	      xOffset = 3140.2700195313,
	      yOffset = -3615.830078125,
	    },
	    [3] = {
	      floorName = "The Military Quarter",
	      height = 800,
	      width = 1200,
	      xOffset = 2587,
	      yOffset = -3136,
	    },
	    [4] = {
	      floorName = "The Plague Quarter",
	      height = 800.21997070313,
	      width = 1200.330078125,
	      xOffset = 3087.0200195313,
	      yOffset = -3136.830078125,
	    },
	    [5] = {
	      floorName = "The Lower Necropolis",
	      height = 1379.8798828125,
	      width = 2069.8098144531,
	      xOffset = 2330.2800292969,
	      yOffset = -3691.2199707031,
	    },
	    [6] = {
	      floorName = "The Upper Necropolis",
	      height = 437.2900390625,
	      width = 655.93994140625,
	      xOffset = 4866.3500976563,
	      yOffset = -3816.5400390625,
	    },
	  },
	  [536] = {
	    [1] = {
	      floorName = "The Violet Hold",
	      height = 170.82006835938,
	      width = 256.22900390625,
	      xOffset = -921.57598876953,
	      yOffset = -1984.1700439453,
	    },
	  },
	  [542] = {
	    [1] = {
	      floorName = "The Argent Coliseum",
	      height = 246.65802001953,
	      width = 369.98596191406,
	      xOffset = -807.40997314453,
	      yOffset = -876.10601806641,
	    },
	  },
	  [543] = {
	    [1] = {
	      floorName = "The Argent Coliseum",
	      height = 246.65798950195,
	      width = 369.9861869812,
	      xOffset = -328.73098754883,
	      yOffset = -693.01898193359,
	    },
	    [2] = {
	      floorName = "The Icy Depths",
	      height = 493.33001708984,
	      width = 739.99601745605,
	      xOffset = -528.73602294922,
	      yOffset = -926.35400390625,
	    },
	  },
	  [544] = {
	    [1] = {
	      floorName = "Kaja'Mite Cavern",
	      height = 182.66662597656,
	      microName = "KajamiteCavern",
	      width = 274,
	      xOffset = -3089,
	      yOffset = -696.83331298828,
	    },
	    [2] = {
	      height = 675,
	      microName = "VolcanothsLair",
	      width = 1012.5,
	      xOffset = -1728.75,
	      yOffset = -1500,
	    },
	  },
	  [545] = { -- former Gilneas...
	    [1] = {
	      height = 280,
	      microName = "EmberstoneMine",
	      floorName = "Emberstone Mine",
	      width = 420,
	      xOffset = 2300,
	      yOffset = 450,
	    },
	    [2] = {
	      height = 166.97998046875,
	      microName = "GreymaneManor",
	      floorName = "Greymane Manor",
	      width = 250.4697265625,
	      xOffset = 735, --Offset for this floor and floor 3 are really messed up
	      yOffset = 970,
	    },
	   [3] = {
	      height = 186.97998046875,
	      microName = "GreymaneManor",
	      floorName = "Greymane Manor",
	      width = 280.4697265625,
	      xOffset = 725,
	      yOffset = 955,
	    },
	  },
	  [601] = {
	    [1] = {
	      floorName = "The Forge of Souls",
	      height = 965.400390625,
	      width = 1448.0998535156,
	      xOffset = -3134.1298828125,
	      yOffset = -5779.9301757813,
	    },
	  },
	  [603] = {
	    [1] = {
	      floorName = "Halls of Reflection",
	      height = 586.01953125,
	      width = 879.02001953125,
	      xOffset = -2349.0100097656,
	      yOffset = -5713.009765625,
	    },
	  },
	  [604] = {
	    [1] = {
	      floorName = "The Lower Citadel",
	      height = 903.64703369141,
	      width = 1355.4700927734,
	      xOffset = -2739.8000488281,
	      yOffset = -138.80700683594,
	    },
	    [2] = {
	      floorName = "The Rampart of Skulls",
	      height = 711.33369064331,
	      width = 1067,
	      xOffset = -2698,
	      yOffset = 43.333301544189,
	    },
	    [3] = {
	      floorName = "Deathbringer's Rise",
	      height = 130.31500244141,
	      width = 195.46997070313,
	      xOffset = -2311.8000488281,
	      yOffset = 449.99798583984,
	    },
	    [4] = {
	      floorName = "The Frost Queen's Lair",
	      height = 515.81030273438,
	      width = 773.71008300781,
	      xOffset = -2767.2800292969,
	      yOffset = -4528.2202148438,
	    },
	    [5] = {
	      floorName = "The Upper Reaches",
	      height = 765.82006835938,
	      width = 1148.7399902344,
	      xOffset = -3364.8000488281,
	      yOffset = -4768.2299804688,
	    },
	    [6] = {
	      floorName = "Royal Quarters",
	      height = 249.1298828125,
	      width = 373.7099609375,
	      xOffset = -2960.2800292969,
	      yOffset = -4704.8798828125,
	    },
	    [7] = {
	      floorName = "The Frozen Throne",
	      height = 195.50701904297,
	      width = 293.26000976563,
	      xOffset = 1978.2900390625,
	      yOffset = -605.79901123047,
	    },
	    [8] = {
	      floorName = "Frostmourne",
	      height = 165.28799438477,
	      width = 247.92993164063,
	      xOffset = 2400.330078125,
	      yOffset = -579.39599609375,
	    },
	  },
	  [605] = {
	    [5] = {
	      floorName = "Kaja'Mine Gold",
	      height = 116.666015625,
	      microName = "Kajamine",
	      width = 175,
	      xOffset = -1232.5,
	      yOffset = 8321.6669921875,
	    },
	    [6] = {
	      floorName = "Kaja'Mine Silver",
	      height = 116.666015625,
	      microName = "Kajamine",
	      width = 175,
	      xOffset = -1172.5,
	      yOffset = 8451.6669921875,
	    },
	    [7] = {
	      floorName = "Kaja'Mine Copper",
	      height = 220,
	      microName = "Kajamine",
	      width = 330,
	      xOffset = -1460,
	      yOffset = 8480,
	    },
	  },
	  [679] = { --Gilneas
	    [1] = {
	      height = 280,
	      microName = "EmberstoneMine",
	      floorName = "Emberstone Mine",
	      width = 420,
	      xOffset = 2300,
	      yOffset = 450,
	    },
	    [2] = {
	      height = 166.97998046875,
	      microName = "GreymaneManor",
	      floorName = "Greymane Manor",
	      width = 250.4697265625,
	      xOffset = 735, --Offset for this floor and floor 3 are really messed up
	      yOffset = 970,
	    },
	   [3] = {
	      height = 186.97998046875,
	      microName = "GreymaneManor",
	      floorName = "Greymane Manor",
	      width = 280.4697265625,
	      xOffset = 725,
	      yOffset = 955,
	    },
	  },
	  [680] = {
	    [1] = {
	      floorName = "Ragefire Chasm",
	      height = 492.57620239258,
	      width = 738.86401367188,
	      xOffset = -452.87100219727,
	      yOffset = -39.623199462891,
	    },
	  },
	  [687] = {
	    [1] = {
	      floorName = "The Temple of Atal' Hakkar",
	      height = 463.35298156738,
	      width = 695.02899169922,
	      xOffset = -442.75799560547,
	      yOffset = 255.16600036621,
	    },
	  },
	  [688] = {
	    [1] = {
	      floorName = "The Pool of Ask'Ar",
	      height = 589.47997283936,
	      width = 884.2200012207,
	      xOffset = -502.92401123047,
	      yOffset = 87.666999816895,
	    },
	    [2] = {
	      floorName = "Moonshrine Sanctum",
	      height = 589.48001098633,
	      width = 884.22003173828,
	      xOffset = -302.92401123047,
	      yOffset = 337.66696166992,
	    },
	    [3] = {
	      floorName = "The Forgotten Pool",
	      height = 189.48266601563,
	      width = 284.22400426865,
	      xOffset = -2.9260001182556,
	      yOffset = 687.66564941406,
	    },
	  },
	  [690] = {
	    [1] = {
	      floorName = "Stormwind Stockade",
	      height = 252.10249519348,
	      width = 378.15299987793,
	      xOffset = -189.82200622559,
	      yOffset = -220.63299560547,
	    },
	  },
	  [691] = {
	    [1] = {
	      floorName = "The Hall of Gears",
	      height = 513.11199951172,
	      width = 769.66799926758,
	      xOffset = -491.89599609375,
	      yOffset = 180.88800048828,
	    },
	    [2] = {
	      floorName = "The Dormitory",
	      height = 513.11199951172,
	      width = 769.66799926758,
	      xOffset = -691.89599609375,
	      yOffset = 200.88800048828,
	    },
	    [3] = {
	      floorName = "Launch Bay",
	      height = 579.77801513672,
	      width = 869.66799926758,
	      xOffset = -741.89599609375,
	      yOffset = 387.55499267578,
	    },
	    [4] = {
	      floorName = "Tinkers' Court",
	      height = 579.7799987793,
	      width = 869.66970825195,
	      xOffset = -942.66900634766,
	      yOffset = 357.5539855957,
	    },
	  },
	  [692] = {
	    [1] = {
	      floorName = "Hall of the Keepers",
	      height = 595.77899169922,
	      width = 893.66801452637,
	      xOffset = -645.11901855469,
	      yOffset = -204.30599975586,
	    },
	    [2] = {
	      floorName = "Khaz'Goroth's Seat",
	      height = 328.38049316406,
	      width = 492.57041931152,
	      xOffset = -545.11901855469,
	      yOffset = -270.60699462891,
	    },
	  },
	  [696] = {
	    [1] = {
	      floorName = "The Molten Core",
	      height = 843.19906616211,
	      width = 1264.8000640869,
	      xOffset = 130.75999450684,
	      yOffset = -1303.0600585938,
	    },
	  },
	  [699] = {
	    [1] = {
	      floorName = "Gordok Commons",
	      height = 850,
	      width = 1275,
	      xOffset = -887.5,
	      yOffset = -1050,
	    },
	    [2] = {
	      floorName = "Capital Gardens",
	      height = 350,
	      width = 525,
	      xOffset = -650,
	      yOffset = -200,
	    },
	    [3] = {
	      floorName = "Court of the Highborne",
	      height = 325,
	      width = 487.5,
	      xOffset = -718.75,
	      yOffset = -175,
	    },
	    [4] = {
	      floorName = "Prison of Immol'Thar",
	      height = 500,
	      width = 750,
	      xOffset = -1075,
	      yOffset = -250,
	    },
	    [5] = {
	      floorName = "Warpwood Quarter",
	      height = 533.33399963379,
	      width = 800.00080108643,
	      xOffset = 99.999198913574,
	      yOffset = -251.66700744629,
	    },
	    [6] = {
	      floorName = "The Shrine of Eldretharr",
	      height = 650,
	      width = 975,
	      xOffset = -112.5,
	      yOffset = -450,
	    },
	  },
	  [704] = {
	    [1] = {
	      floorName = "Detention Block",
	      height = 938.04075622559,
	      width = 1407.0609741211,
	      xOffset = -522.33697509766,
	      yOffset = -1186.6804199219,
	    },
	    [2] = {
	      floorName = "Shadowforge City",
	      height = 1004.7074279785,
	      width = 1507.0609741211,
	      xOffset = -572.33697509766,
	      yOffset = -1500.0102539063,
	    },
	  },
	  [710] = {
	    [1] = {
	      floorName = "The Shattered Halls",
	      height = 709.16499328613,
	      width = 1063.747467041,
	      xOffset = -631.10821533203,
	      yOffset = -617.4169921875,
	    },
	  },
	  [718] = {
	    [1] = {
	      floorName = "Onyxia's Lair",
	      height = 322.07878875732,
	      width = 483.11798858643,
	      xOffset = -111.38300323486,
	      yOffset = -98.245796203613,
	    },
	  },
	  [721] = {
	    [1] = {
	      floorName = "Tazz'Alaor",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [2] = {
	      floorName = "Skitterweb Tunnels",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [3] = {
	      floorName = "Hordemar City",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [4] = {
	      floorName = "Hall of Blackhand",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [5] = {
	      floorName = "Dragonspire Hall",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [6] = {
	      floorName = "The Rookery",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	    [7] = {
	      floorName = "Blackrock Stadium",
	      height = 591.22601318359,
	      width = 886.83901405334,
	      xOffset = -10.586999893188,
	      yOffset = -304.39801025391,
	    },
	  },
	  [722] = {
	    [1] = {
	      floorName = "Halls of the Hereafter",
	      height = 495.02699279785,
	      width = 742.54043579102,
	      xOffset = -327.55722045898,
	      yOffset = -354.95098876953,
	    },
	    [2] = {
	      floorName = "Bridge of Souls",
	      height = 545.02699279785,
	      width = 817.54046630859,
	      xOffset = -215.05725097656,
	      yOffset = -334.95098876953,
	    },
	  },
	  [723] = {
	    [1] = {
	      floorName = "Veil Sethekk",
	      height = 468.99699401855,
	      width = 703.49548339844,
	      xOffset = -515.79724121094,
	      yOffset = -173.65400695801,
	    },
	    [2] = {
	      floorName = "Halls of Mourning",
	      height = 468.99699401855,
	      width = 703.49548339844,
	      xOffset = -515.79724121094,
	      yOffset = -173.65400695801,
	    },
	  },
	  [724] = {
	    [1] = {
	      floorName = "Shadow Labyrinth",
	      height = 561.01488876343,
	      width = 841.52235412598,
	      xOffset = -185.07917785645,
	      yOffset = -62.796901702881,
	    },
	  },
	  [725] = {
	    [1] = {
	      floorName = "The Blood Furnace",
	      height = 669.01268768311,
	      width = 1003.5190124512,
	      xOffset = -498.98901367188,
	      yOffset = -603.13598632813,
	    },
	  },
	  [726] = {
	    [1] = {
	      floorName = "The Underbog",
	      height = 596.61335754395,
	      width = 894.91999816895,
	      xOffset = -240.97700500488,
	      yOffset = -423.73516845703,
	    },
	  },
	  [727] = {
	    [1] = {
	      floorName = "The Steamvault",
	      height = 584.50941467285,
	      width = 876.76400756836,
	      xOffset = -160.02200317383,
	      yOffset = -158.56721496582,
	    },
	    [2] = {
	      floorName = "The Cooling Pools",
	      height = 584.50941467285,
	      width = 876.76400756836,
	      xOffset = -160.02200317383,
	      yOffset = -158.56721496582,
	    },
	  },
	  [728] = {
	    [1] = {
	      floorName = "The Slave Pens",
	      height = 593.3720703125,
	      width = 890.05812454224,
	      xOffset = -53.934101104736,
	      yOffset = -201.72003173828,
	    },
	  },
	  [729] = {
	    [1] = {
	      floorName = "The Botanica",
	      height = 504.93499755859,
	      width = 757.4024810791,
	      xOffset = -649.75323486328,
	      yOffset = -248.02499389648,
	    },
	  },
	  [732] = {
	    [1] = {
	      floorName = "Mana Tombs",
	      height = 548.85681152344,
	      width = 823.28515625,
	      xOffset = -276.93408203125,
	      yOffset = -90.07080078125,
	    },
	  },
	  [749] = {
	    [1] = {
	      floorName = "Wailing Caverns",
	      height = 624.31666564941,
	      width = 936.47500610352,
	      xOffset = -560.52899169922,
	      yOffset = -214.1703338623,
	    },
	  },
	  [750] = {
	    [1] = {
	      floorName = "Caverns of Maraudon",
	      height = 650,
	      width = 975,
	      xOffset = -147.5,
	      yOffset = -1200,
	    },
	    [2] = {
	      floorName = "Zaetar's Grave",
	      height = 1091.6660003662,
	      width = 1637.5,
	      xOffset = -478.75,
	      yOffset = -883.3330078125,
	    },
	  },
	  [752] = {
	    [1] = {
	      floorName = "Baradin Hold",
	      height = 390,
	      width = 585,
	      xOffset = -1617.5,
	      yOffset = -515,
	    },
	  },
	  [753] = {
	    [1] = {
	      floorName = "Chamber of Incineration",
	      height = 679.67231941223,
	      width = 1019.5079345703,
	      xOffset = -1459.4399414063,
	      yOffset = -686.33898925781,
	    },
	    [2] = {
	      floorName = "Twilight Forge",
	      height = 679.67231941223,
	      width = 1019.5079345703,
	      xOffset = -1259.4399414063,
	      yOffset = -686.33898925781,
	    },
	  },
	  [754] = {
	    [1] = {
	      floorName = "The Broken Hall",
	      height = 566.46234107018,
	      width = 849.69401550293,
	      xOffset = -174.10899353027,
	      yOffset = 0.99963402748108,
	    },
	    [2] = {
	      floorName = "Vault of the Shadowflame",
	      height = 666.46200561523,
	      width = 999.69297790527,
	      xOffset = -249.10899353027,
	      yOffset = -359,
	    },
	  },
	  [755] = {
	    [1] = {
	      floorName = "Dragonmaw Garrison",
	      height = 332.94970703125,
	      width = 499.42803955078,
	      xOffset = 844.62200927734,
	      yOffset = 7394.1201171875,
	    },
	    [2] = {
	      floorName = "Halls of Strife",
	      height = 432.94970703125,
	      width = 649.42706298828,
	      xOffset = 729.62298583984,
	      yOffset = 7344.1201171875,
	    },
	    [3] = {
	      floorName = "Crimson Laboratories",
	      height = 432.94970703125,
	      width = 649.42706298828,
	      xOffset = 719.62298583984,
	      yOffset = 7324.1201171875,
	    },
	    [4] = {
	      floorName = "Nefarian's Lair",
	      height = 432.94970703125,
	      width = 649.42706298828,
	      xOffset = 769.62298583984,
	      yOffset = 7204.1201171875,
	    },
	  },
	  [756] = {
	    [1] = {
	      floorName = "The Deadmines",
	      height = 372.84250259399,
	      width = 559.26400756836,
	      xOffset = 237.35800170898,
	      yOffset = -35.333499908447,
	    },
	    [2] = {
	      floorName = "Ironclad Cove",
	      height = 332.84230041504,
	      width = 499.26300048828,
	      xOffset = 517.35699462891,
	      yOffset = -65.333297729492,
	    },
	  },
	  [757] = {
	    [1] = {
	      floorName = "Grim Batol",
	      height = 579.36499023438,
	      width = 869.0474319458,
	      xOffset = 83.732597351074,
	      yOffset = 301.62200927734,
	    },
	  },
	  [758] = {
	    [1] = {
	      floorName = "The Twilight Enclave",
	      height = 718.88998413086,
	      width = 1078.3349990845,
	      xOffset = 106.43550872803,
	      yOffset = 180.95700073242,
	    },
	    [2] = {
	      floorName = "Throne of the Apocalypse",
	      height = 518.89495849609,
	      width = 778.34301757813,
	      xOffset = 256.42700195313,
	      yOffset = 770.95501708984,
	    },
	    [3] = {
	      floorName = "The Twilight Caverns",
	      height = 694.89495849609,
	      width = 1042.3420257568,
	      xOffset = 224.92799377441,
	      yOffset = 707.95501708984,
	    },
	  },
	  [759] = {
	    [1] = {
	      floorName = "The Vault of Lights",
	      height = 1021.1673054695,
	      width = 1531.7509765625,
	      xOffset = -1226.9699707031,
	      yOffset = -2.9972615242004,
	    },
	    [2] = {
	      floorName = "Tomb of the Earthrager",
	      height = 848.50342559814,
	      width = 1272.7550354004,
	      xOffset = -157.46499633789,
	      yOffset = 86.331596374512,
	    },
	    [3] = {
	      floorName = "The Four Seats",
	      height = 752.51266479492,
	      width = 1128.7689819336,
	      xOffset = -725.47497558594,
	      yOffset = 132.32766723633,
	    },
	  },
	  [760] = {
	    [1] = {
	      floorName = "Razorfen Downs",
	      height = 472.69995117188,
	      width = 709.04895019531,
	      xOffset = -1279.9399414063,
	      yOffset = -2682.5500488281,
	    },
	  },
	  [761] = {
	    [1] = {
	      floorName = "Razorfen Kraul",
	      height = 490.95983886719,
	      width = 736.44995117188,
	      xOffset = -2058.919921875,
	      yOffset = -2349.6398925781,
	    },
	  },
	  [764] = {
	    [1] = {
	      floorName = "The Courtyard",
	      height = 234.95339202881,
	      width = 352.43005371094,
	      xOffset = -2356.1999511719,
	      yOffset = 84.92960357666,
	    },
	    [2] = {
	      floorName = "Dining Hall",
	      height = 141.61799621582,
	      width = 212.4267578125,
	      xOffset = -2359.9833984375,
	      yOffset = 161.59700012207,
	    },
	    [3] = {
	      floorName = "The Vacant Den",
	      height = 101.61996459961,
	      width = 152.42993164063,
	      xOffset = -2256.1999511719,
	      yOffset = 91.596069335938,
	    },
	    [4] = {
	      floorName = "Lower Observatory",
	      height = 101.62469482422,
	      width = 152.42993164063,
	      xOffset = -2256.1999511719,
	      yOffset = 91.590301513672,
	    },
	    [5] = {
	      floorName = "Upper Observatory",
	      height = 101.62469482422,
	      width = 152.42993164063,
	      xOffset = -2256.1999511719,
	      yOffset = 91.590301513672,
	    },
	    [6] = {
	      floorName = "Lord Godfrey's Chamber",
	      height = 132.28662872314,
	      width = 198.42993164063,
	      xOffset = -2279.1999511719,
	      yOffset = 50.259391784668,
	    },
	    [7] = {
	      floorName = "The Wall Walk",
	      height = 181.61996459961,
	      width = 272.42993164063,
	      xOffset = -2296.1999511719,
	      yOffset = 96.596069335938,
	    },
	  },
	  [765] = {
	    [1] = {
	      floorName = "Crusader's Square",
	      height = 470.47998046875,
	      width = 705.71997070313,
	      xOffset = 2911.9599609375,
	      yOffset = -3809.4499511719,
	    },
	    [2] = {
	      floorName = "The Gauntlet",
	      height = 670.48022460938,
	      width = 1005.7204589844,
	      xOffset = 2961.9597167969,
	      yOffset = -4169.4501953125,
	    },
	  },
	  [766] = {
	    [1] = {
	      floorName = "The Hive Undergrounds",
	      height = 1851.6962890625,
	      width = 2777.5441131592,
	      xOffset = -2915.6201171875,
	      yOffset = 7659.287109375,
	    },
	    [2] = {
	      floorName = "The Temple Gates",
	      height = 651.70654296875,
	      width = 977.55993652344,
	      xOffset = -2515.6298828125,
	      yOffset = 8051.7963867188,
	    },
	    [3] = {
	      floorName = "Vault of C'Thun",
	      height = 385.0400390625,
	      width = 577.56005859375,
	      xOffset = -2315.6298828125,
	      yOffset = 8335.1298828125,
	    },
	  },
	  [767] = {
	    [1] = {
	      floorName = "Abyssal Halls",
	      height = 665.44799804688,
	      width = 998.17193603516,
	      xOffset = -1307.0699462891,
	      yOffset = 14,
	    },
	    [2] = {
	      floorName = "Throne of Neptulon",
	      height = 665.44799804688,
	      width = 998.17193603516,
	      xOffset = -1307.0699462891,
	      yOffset = -336,
	    },
	  },
	  [768] = {
	    [1] = {
	      floorName = "The Stonecore",
	      height = 878.08697509766,
	      width = 1317.1289978027,
	      xOffset = -1701.25,
	      yOffset = -1675.5699462891,
	    },
	  },
	  [769] = {
	    [1] = {
	      floorName = "The Vortex Pinnacle",
	      height = 1345.8180236816,
	      width = 2018.7250366211,
	      xOffset = -1107.7900390625,
	      yOffset = 111.33200073242,
	    },
	  },
	  [773] = {
	    [1] = {
	      floorName = "Throne of the Four Winds",
	      height = 1000,
	      width = 1500,
	      xOffset = -1525,
	      yOffset = -450,
	    },
	  },
	  [776] = {
	    [1] = {
	      floorName = "Gruul's Lair",
	      height = 350,
	      width = 525,
	      xOffset = -475,
	      yOffset = -337.5,
	    },
	  },
	  [779] = {
	    [1] = {
	      floorName = "Magtheridon's Lair",
	      height = 370.66669464111,
	      width = 556,
	      xOffset = -385.5,
	      yOffset = -255.33334350586,
	    },
	  },
	  [780] = {
	    [1] = {
	      floorName = "Serpentshrine Cavern",
	      height = 1050.0020141602,
	      width = 1575.0029754639,
	      xOffset = -212.50248718262,
	      yOffset = -650.00201416016,
	    },
	  },
	  [782] = {
	    [1] = {
	      floorName = "The Eye",
	      height = 1050,
	      width = 1575,
	      xOffset = -787.5,
	      yOffset = -950,
	    },
	  },
	  [789] = {
	    [1] = {
	      floorName = "Sunwell Plateau",
	      height = 604.16662597656,
	      width = 906.25,
	      xOffset = 300,
	      yOffset = 1408.3332519531,
	    },
	    [2] = {
	      floorName = "Shrine of the Eclipse",
	      height = 310,
	      width = 465,
	      xOffset = -842.5,
	      yOffset = -1890,
	    },
	  },
	  [796] = {
	    [1] = {
	      floorName = "Illidari Training Grounds",
	      height = 522.91662597656,
	      width = 783.33334350586,
	      xOffset = 366.66665649414,
	      yOffset = 427.08331298828,
	    },
	    [2] = {
	      floorName = "Karabor Sewers",
	      height = 834.8330078125,
	      width = 1252.249578476,
	      xOffset = -1276.1201171875,
	      yOffset = -594.8330078125,
	    },
	    [3] = {
	      floorName = "Sanctuary of Shadows",
	      height = 650,
	      width = 975,
	      xOffset = -799,
	      yOffset = -1030,
	    },
	    [4] = {
	      floorName = "Halls of Anguish",
	      height = 670,
	      width = 1005,
	      xOffset = -814,
	      yOffset = -1070,
	    },
	    [5] = {
	      floorName = "Gorefiend's Vigil",
	      height = 293.333984375,
	      width = 440.0009765625,
	      xOffset = -575.00048828125,
	      yOffset = -636.6669921875,
	    },
	    [6] = {
	      floorName = "Den of Mortal Delights",
	      height = 446.66668701172,
	      width = 670,
	      xOffset = -600,
	      yOffset = -1110.8303222656,
	    },
	    [7] = {
	      floorName = "Chamber of Command",
	      height = 470,
	      width = 705,
	      xOffset = -637.5,
	      yOffset = -920,
	    },
	    [8] = {
	      floorName = "Temple Summit",
	      height = 236.66662597656,
	      width = 355,
	      xOffset = -492.5,
	      yOffset = -843.33331298828,
	    },
	  },
	  [797] = {
	    [1] = {
	      floorName = "Hellfire Ramparts",
	      height = 463.0400390625,
	      width = 694.56005859375,
	      xOffset = -1989.2800292969,
	      yOffset = 1029.4399414063,
	    },
	  },
	  [798] = {
	    [1] = {
	      floorName = "Grand Magister's Asylum",
	      height = 353.55596923828,
	      width = 530.33401489258,
	      xOffset = -226.10800170898,
	      yOffset = -325.76013183594,
	    },
	    [2] = {
	      floorName = "Observation Grounds",
	      height = 353.55599212646,
	      width = 530.33401489258,
	      xOffset = -226.10800170898,
	      yOffset = -325.76010131836,
	    },
	  },
	  [799] = {
	    [1] = {
	      floorName = "Servant's Quarters",
	      height = 366.69921875,
	      width = 550.048828125,
	      xOffset = 1674.9755859375,
	      yOffset = 10822.900390625,
	    },
	    [2] = {
	      floorName = "Upper Livery Stables",
	      height = 171.90625,
	      width = 257.85986328125,
	      xOffset = 1823.5600585938,
	      yOffset = 11017.096679688,
	    },
	    [3] = {
	      floorName = "The Banquet Hall",
	      height = 230.099609375,
	      width = 345.1494140625,
	      xOffset = 1787.4252929688,
	      yOffset = 10836.200195313,
	    },
	    [4] = {
	      floorName = "The Guest Chambers",
	      height = 346.69921875,
	      width = 520.048828125,
	      xOffset = 1669.9755859375,
	      yOffset = 10772.900390625,
	    },
	    [5] = {
	      floorName = "Opera Hall Balcony",
	      height = 156.099609375,
	      width = 234.14990234375,
	      xOffset = 1698.4300537109,
	      yOffset = 10813.200195313,
	    },
	    [6] = {
	      floorName = "Master's Terrace",
	      height = 387.69921875,
	      width = 581.548828125,
	      xOffset = 1624.2255859375,
	      yOffset = 10802.900390625,
	    },
	    [7] = {
	      floorName = "Lower Broken Stair",
	      height = 127.69921875,
	      width = 191.548828125,
	      xOffset = 1875.2255859375,
	      yOffset = 10987.900390625,
	    },
	    [8] = {
	      floorName = "Upper Broken Stair",
	      height = 92.900390625,
	      width = 139.3505859375,
	      xOffset = 1898.3297119141,
	      yOffset = 11012.299804688,
	    },
	    [9] = {
	      floorName = "The Menagerie",
	      height = 506.69921875,
	      width = 760.048828125,
	      xOffset = 1509.9755859375,
	      yOffset = 10952.900390625,
	    },
	    [10] = {
	      floorName = "Guardian's Library",
	      height = 300.166015625,
	      width = 450.25,
	      xOffset = 1589.8800048828,
	      yOffset = 11086.166992188,
	    },
	    [11] = {
	      floorName = "The Repository",
	      height = 180.69921875,
	      width = 271.05004882813,
	      xOffset = 1554.4799804688,
	      yOffset = 11104.400390625,
	    },
	    [12] = {
	      floorName = "Upper Library",
	      height = 396.69921875,
	      width = 595.048828125,
	      xOffset = 1587.4755859375,
	      yOffset = 11047.900390625,
	    },
	    [13] = {
	      floorName = "The Celestial Watch",
	      height = 352.69921875,
	      width = 529.048828125,
	      xOffset = 1433.9755859375,
	      yOffset = 10986.400390625,
	    },
	    [14] = {
	      floorName = "Gamesman's Hall",
	      height = 163.5,
	      width = 245.25,
	      xOffset = 1787.3800048828,
	      yOffset = 10979.5,
	    },
	    [15] = {
	      floorName = "Medivh's Chambers",
	      height = 140.765625,
	      width = 211.14990234375,
	      xOffset = 1813.9300537109,
	      yOffset = 10972.8671875,
	    },
	    [16] = {
	      floorName = "The Power Station",
	      height = 67.5,
	      width = 101.25,
	      xOffset = 1918.8800048828,
	      yOffset = 11029.5,
	    },
	    [17] = {
	      floorName = "Netherspace",
	      height = 227.5,
	      width = 341.25,
	      xOffset = 1813.8798828125,
	      yOffset = 10874.5,
	    },
	  },
	  [800] = {
	    [1] = {
	      floorName = "The Firelands",
	      height = 1058.3332824707,
	      width = 1587.4999389648,
	      xOffset = -868.74993896484,
	      yOffset = -633.33331298828,
	    },
	    [2] = {
	      floorName = "The Anvil of Conflagration",
	      height = 250,
	      width = 375,
	      xOffset = -677.0830078125,
	      yOffset = -593.75,
	    },
	    [3] = {
	      floorName = "Sulfuron Keep",
	      height = 960,
	      width = 1440,
	      xOffset = -670,
	      yOffset = -1225,
	    },
	  },
	  [806] = {
	    [15] = {
	      height = 220,
	      microName = "TheWidowsWail",
	      width = 330,
	      xOffset = 2322.5,
	      yOffset = -2220,
	    },
	    [16] = {
	      floorName = "Oona Kagu",
	      height = 211.66650390625,
	      microName = "OonaKagu",
	      width = 317.5,
	      xOffset = 2291.25,
	      yOffset = -2463.3332519531,
	    },
	    [6] = {
	      floorName = "Upper Quarry",
	      height = 118.66674804688,
	      microName = "GreenstoneQuarry",
	      width = 178,
	      xOffset = 1610,
	      yOffset = -2341.0034179688,
	    },
	    [7] = {
	      floorName = "Lower Quarry",
	      height = 275,
	      microName = "GreenstoneQuarry",
	      width = 412.5,
	      xOffset = 1468.75,
	      yOffset = -2435,
	    },
	  },
	  [937] = {
	    [1] = {
	      height = 380,
	      width = 570,
	      xOffset = -1351.25,
	      yOffset = -1000,
	    },
	  },
	  [809] = {
	    [8] = {
	      floorName = "Howlingwind Cavern",
	      height = 333.36181640625,
	      microName = "HowlingwindCavern",
	      width = 500.04302978516,
	      xOffset = -1492.5200195313,
	      yOffset = -3721.6809082031,
	    },
	    [9] = {
	      floorName = "Pranksters' Hollow",
	      height = 131.68017578125,
	      microName = "PrankstersHollow",
	      width = 197.52025604248,
	      xOffset = -276.26013183594,
	      yOffset = -2588.3400878906,
	    },
	    [10] = {
	      floorName = "Knucklethump Hole",
	      height = 130,
	      microName = "KnucklethumpHole",
	      width = 195,
	      xOffset = -1847,
	      yOffset = -3174.5,
	    },
	    [11] = {
	      floorName = "Upper Deep",
	      height = 156.66650390625,
	      microName = "TheDeeper",
	      width = 235,
	      xOffset = -1665,
	      yOffset = -2653.3332519531,
	    },
	    [12] = {
	      floorName = "Lower Deep",
	      height = 177,
	      microName = "TheDeeper",
	      width = 265.5,
	      xOffset = -1680.25,
	      yOffset = -2649,
	    },
	    [17] = {
	      floorName = "Tomb of Conquerors",
	      height = 310,
	      microName = "TombofConquerors",
	      width = 465,
	      xOffset = -1945,
	      yOffset = -3835,
	    },
	    [20] = {
	      floorName = "Ruins of Korune",
	      height = 425,
	      microName = "RuinsofKorune",
	      width = 637.5,
	      xOffset = -3096.25,
	      yOffset = -4530,
	    },
	    [21] = {
	      floorName = "Crypt of Korune",
	      height = 225,
	      microName = "RuinsofKorune",
	      width = 337.5,
	      xOffset = -2997.75,
	      yOffset = -4400,
	    },
	  },
	  [810] = {
	    [13] = {
	      height = 325,
	      floorName = "Niuzao Catacombs",
	      microName = "NiuzaoCatacombs",
	      width = 487.5,
	      xOffset = -5256.25,
	      yOffset = -2425,
	    },
	  },
	  [811] = {
	    [3] = {
	      floorName = "The Emperor's Step",
	      height = 173.333984375,
	      microName = "ShrineofSevenStars",
	      width = 260.0009765625,
	      xOffset = -375.00048828125,
	      yOffset = -899.6669921875,
	    },
	    [4] = {
	      floorName = "The Imperial Exchange",
	      height = 210,
	      microName = "ShrineofSevenStars",
	      width = 315,
	      xOffset = -402.5,
	      yOffset = -903.75,
	    },
	    [18] = {
	      floorName = "Guo-Lai Halls",
	      height = 420,
	      microName = "GuoLaiHalls",
	      width = 630,
	      xOffset = -2272.5,
	      yOffset = -1900,
	    },
	    [19] = {
	      floorName = "The Hall of the Serpent",
	      height = 1600, -- TODO: re-caveman --How do you get in here?
	      microName = "GuoLaiHalls",
	      width = 3200,
	      xOffset = -3200,
	      yOffset = -1600,
	    },
	  },
	  [820] = {
	    [1] = {
	      floorName = "Entryway of Time",
	      height = 2197.9165039063,
	      width = 3295.8331298828,
	      xOffset = -1035.4166259766,
	      yOffset = 2500,
	    },
	    [2] = {
	      floorName = "Azure Dragonshrine",
	      height = 375,
	      width = 562.5,
	      xOffset = -793.75,
	      yOffset = -3293.75,
	    },
	    [3] = {
	      floorName = "Ruby Dragonshrine",
	      height = 577.08032226563,
	      width = 865.62054443359,
	      xOffset = -1400.5187988281,
	      yOffset = -4077.080078125,
	    },
	    [4] = {
	      floorName = "Obsidian Dragonshrine",
	      height = 316.66650390625,
	      width = 475,
	      xOffset = -1656.25,
	      yOffset = -4545.8315429688,
	    },
	    [5] = {
	      floorName = "Emerald Dragonshrine",
	      height = 464.58984375,
	      width = 696.884765625,
	      xOffset = -406.775390625,
	      yOffset = -3035.419921875,
	    },
	    [6] = {
	      floorName = "Bronze Dragonshrine",
	      height = 302.08984375,
	      width = 453.13500976563,
	      xOffset = 193.22399902344,
	      yOffset = -4272.919921875,
	    },
	  },
	  [824] = {
	    [1] = {
	      floorName = "Dragonblight",
	      height = 2063.0651855469,
	      width = 3106.7084960938,
	      xOffset = -3940.3012695313,
	      yOffset = -2628.1579589844,
	    },
	    [2] = {
	      floorName = "Maw of Go'rath",
	      height = 265,
	      width = 397.5,
	      xOffset = 1716.25,
	      yOffset = 1625,
	    },
	    [3] = {
	      floorName = "Maw of Shu'ma",
	      height = 285,
	      width = 427.5,
	      xOffset = 2834.5,
	      yOffset = 1625,
	    },
	    [4] = {
	      floorName = "Eye of Eternity",
	      height = 123.466796875,
	      width = 185.19921875,
	      xOffset = -13709.099609375,
	      yOffset = -13651.733398438,
	    },
	    [5] = {
	      floorName = "Skyfire Airship",
	      height = 1,
	      width = 1.5,
	      xOffset = -1.25,
	      yOffset = -1,
	    },
	    [6] = {
	      floorName = "Spine of Deathwing",
	      height = 1,
	      width = 1.5,
	      xOffset = -1.25,
	      yOffset = -1,
	    },
	    [7] = {
	      floorName = "The Maelstrom",
	      height = 738.900390625,
	      width = 1108.3515625,
	      xOffset = -12535.42578125,
	      yOffset = 11516,
	    },
	  },
	  [857] = {
	    [1] = {
	      floorName = "Explorers' League HQ",
	      height = 175,
	      microName = "RuinsofOgudei",
	      width = 262.5,
	      xOffset = 731.875,
	      yOffset = 575,
	    },
	    [2] = {
	      floorName = "Ruins of Ogudei",
	      height = 335.00073242188,
	      microName = "RuinsofOgudei",
	      width = 502.5009765625,
	      xOffset = 632.96899414063,
	      yOffset = 549.99963378906,
	    },
	    [3] = {
	      floorName = "Reliquary Incursion",
	      height = 141.666015625,
	      microName = "RuinsofOgudei",
	      width = 212.4990234375,
	      xOffset = 796.87097167969,
	      yOffset = 621.6669921875,
	    },
	  },
	  [866] = {
	    [9] = {
	      floorName = "Frostmane Hovel",
	      height = 177.66015625,
	      microName = "ColdridgeValley",
	      width = 266.490234375,
	      xOffset = -508.2451171875,
	      yOffset = 6445.669921875,
	    },
	  },
	  [867] = {
	    [1] = {
	      floorName = "Temple of the Jade Serpent",
	      height = 366.66668701172,
	      width = 550,
	      xOffset = 2315,
	      yOffset = -1118.3303222656,
	    },
	    [2] = {
	      floorName = "The Scrollkeeper's Sanctum",
	      height = 132,
	      width = 198,
	      xOffset = 2378.5,
	      yOffset = -920.5,
	    },
	  },
	  [871] = {
	    [1] = {
	      floorName = "Training Grounds",
	      height = 310,
	      width = 465,
	      xOffset = -767.5,
	      yOffset = -1100,
	    },
	    [2] = {
	      floorName = "Athenaeum",
	      height = 312,
	      width = 468,
	      xOffset = -733,
	      yOffset = -1347.5,
	    },
	  },
	  [873] = {
	    [5] = {
	      floorName = "The Ancient Passage",
	      height = 595,
	      microName = "TheAncientPassage",
	      width = 892.5,
	      xOffset = -478.75,
	      yOffset = -1735,
	    },
	  },
	  [874] = {
	    [1] = {
	      floorName = "Forlorn Cloister",
	      height = 228.67999267578,
	      width = 343.02001953125,
	      xOffset = -772.01000976563,
	      yOffset = -1229.3399658203,
	    },
	    [2] = {
	      floorName = "Crusader's Chapel",
	      height = 400,
	      width = 600,
	      xOffset = -900.5,
	      yOffset = -1060,
	    },
	  },
	  [875] = {
	    [1] = {
	      floorName = "Gate of the Setting Sun",
	      height = 866.66668701172,
	      width = 1300,
	      xOffset = -2905,
	      yOffset = -1483.3303222656,
	    },
	    [2] = {
	      floorName = "Gate Watch Tower",
	      height = 148.3466796875,
	      width = 222.52001953125,
	      xOffset = -2413.7600097656,
	      yOffset = -1271.6733398438,
	    },
	  },
	  [876] = {
	    [1] = {
	      floorName = "Grain Cellar",
	      height = 173.333984375,
	      width = 260.0009765625,
	      xOffset = -1472.5004882813,
	      yOffset = 663.3330078125,
	    },
	    [2] = {
	      floorName = "Stormstout Brewhall",
	      height = 173.333984375,
	      width = 260.0009765625,
	      xOffset = -1472.5004882813,
	      yOffset = 663.3330078125,
	    },
	    [3] = {
	      floorName = "The Great Wheel",
	      height = 226.66662597656,
	      width = 340,
	      xOffset = -1387.5,
	      yOffset = 591.66668701172,
	    },
	    [4] = {
	      floorName = "The Tasting Room",
	      height = 153.333984375,
	      width = 230.0009765625,
	      xOffset = -1275.0004882813,
	      yOffset = 623.3330078125,
	    },
	  },
	  [877] = {
	    [1] = {
	      floorName = "Shado-Pan Monastery",
	      height = 708.333984375,
	      width = 1062.5,
	      xOffset = 2300,
	      yOffset = 3502.0830078125,
	    },
	    [2] = {
	      floorName = "Cloudstrike Dojo",
	      height = 206.66650390625,
	      width = 310,
	      xOffset = -2815,
	      yOffset = -3773.3332519531,
	    },
	    [3] = {
	      floorName = "Snowdrift Dojo",
	      height = 140,
	      width = 210,
	      xOffset = -3142.5,
	      yOffset = -3750,
	    },
	    [4] = {
	      floorName = "Sealed Chambers",
	      height = 260,
	      width = 390,
	      xOffset = -3090,
	      yOffset = -4155,
	    },
	  },
	  [885] = {
	    [1] = {
	      floorName = "The Crimson Assembly Hall",
	      height = 490,
	      width = 735,
	      xOffset = 2317.5,
	      yOffset = 3870,
	    },
	    [2] = {
	      floorName = "Vaults of Kings Past",
	      height = 360,
	      width = 540,
	      xOffset = 2365,
	      yOffset = 4125,
	    },
	    [3] = {
	      floorName = "Throne of Ancient Conquerors",
	      height = 498.33984375,
	      width = 747.509765625,
	      xOffset = 2311.2451171875,
	      yOffset = 4278.330078125,
	    },
	  },
	  [887] = {
	    [1] = {
	      floorName = "Forward Assault Camp",
	      height = 677.083984375,
	      width = 1014.583984375,
	      xOffset = 4747.916015625,
	      yOffset = 1352.0830078125,
	    },
	    [2] = {
	      floorName = "The Hollowed Out Tree",
	      height = 193.34008789063,
	      width = 290.009765625,
	      xOffset = -5300.0048828125,
	      yOffset = -1631.6700439453,
	    },
	    [3] = {
	      floorName = "Upper Tree Ring",
	      height = 193.34008789063,
	      width = 290.009765625,
	      xOffset = -5300.0048828125,
	      yOffset = -1631.6700439453,
	    },
	  },
	  [891] = {
	    [9] = {
	      floorName = "Spitescale Cavern",
	      height = 248.333984375,
	      microName = "SpitescaleCavern",
	      width = 372.5009765625,
	      xOffset = 5366.2495117188,
	      yOffset = 576.3330078125,
	    },
	  },
	  [892] = {
	    [12] = {
	      floorName = "Night Web's Hollow",
	      height = 146.66674804688,
	      microName = "NightWebsHollow",
	      width = 220,
	      xOffset = -2020,
	      yOffset = -2123.3334960938,
	    },
	  },
	  [895] = {
	    [8] = {
	      height = 130,
	      microName = "FrostmaneHold",
	      width = 195,
	      xOffset = -779,
	      yOffset = 5470,
	    },
	  },
	  [896] = {
	    [1] = {
	      floorName = "Dais of Conquerors",
	      height = 458.33984375,
	      width = 687.509765625,
	      xOffset = -1562.5003662109,
	      yOffset = -4194.169921875,
	    },
	    [2] = {
	      floorName = "The Repository",
	      height = 288.33984375,
	      width = 432.509765625,
	      xOffset = -1677.5048828125,
	      yOffset = -4376.669921875,
	    },
	    [3] = {
	      floorName = "Forge of the Endless",
	      height = 500,
	      width = 750,
	      xOffset = -2065,
	      yOffset = -4280,
	    },
	  },
	  [897] = {
	    [1] = {
	      floorName = "Oratorium of the Voice",
	      height = 466.66674804688,
	      width = 700,
	      xOffset = -700,
	      yOffset = 1966.6667480469,
	    },
	    [2] = {
	      floorName = "Heart of Fear",
	      height = 960.0029296875,
	      width = 1440.0043802261,
	      xOffset = -1430.0100097656,
	      yOffset = 1769.9985351563,
	    },
	  },
	  [898] = {
	    [1] = {
	      floorName = "The Reliquary",
	      height = 166.66801452637,
	      width = 250.00200653076,
	      xOffset = -170.50100708008,
	      yOffset = -301.3330078125,
	    },
	    [2] = {
	      floorName = "Chamber of Summoning",
	      height = 296.66690826416,
	      width = 445.00036621094,
	      xOffset = -315.00018310547,
	      yOffset = -340.3330078125,
	    },
	    [3] = {
	      floorName = "The Upper Study",
	      height = 175,
	      width = 262.5,
	      xOffset = -188.75,
	      yOffset = -87.5,
	    },
	    [4] = {
	      floorName = "Headmaster's Study",
	      height = 175,
	      width = 262.5,
	      xOffset = -188.75,
	      yOffset = -87.5,
	    },
	  },
	  [903] = {
	    [1] = {
	      floorName = "Hall of the Crescent Moon",
	      height = 146,
	      microName = "ShrineofTwoMoons",
	      width = 219,
	      xOffset = -1043.5,
	      yOffset = -1770.5,
	    },
	    [2] = {
	      floorName = "The Imperial Mercantile",
	      height = 172.6,
	      microName = "ShrineofTwoMoons",
	      width = 259.0,
	      xOffset = -1063.5,
		--Was same as first floor, changed slightly... It acts oddly.
	      yOffset = -1808.8,
		-- Adjusted both again... hopefully this makes more sense
	    },
	  },
	  [905] = {
	    [3] = {
	      floorName = "The Emperor's Step",
	      height = 173.333984375,
	      microName = "ShrineofSevenStars",
	      width = 260.0009765625,
	      xOffset = -375.00048828125,
	      yOffset = -899.6669921875,
	    },
	    [4] = {
	      floorName = "The Imperial Exchange",
	      height = 210,
	      microName = "ShrineofSevenStars",
	      width = 315,
	      xOffset = -402.5,
	      yOffset = -903.75,
	    },
	  },
	  [914] = {
	    [1] = {
	      floorName = "The Ancient Passage",
	      height = 595,
	      width = 892.5,
	      xOffset = -478.75,
	      yOffset = -1735,
	    },
	  },
	  [922] = {
	    [1] = {
	      floorName = "Deeprun Tram",
	      height = 208,
	      width = 312,
	      xOffset = -2623.5,
	      yOffset = -95.5,
	    },
	    [2] = {
	      floorName = "Bizmo's Brawlpub",
	      height = 123.33332824707,
	      width = 185,
	      xOffset = -2593.5,
	      yOffset = 49.62467956543,
	    },
	  },
	  [924] = {
	    [1] = {
	      floorName = "Dalaran City",
	      height = 553.34375,
	      width = 830.01501464844,
	      xOffset = -1052.5100097656,
	      yOffset = -6066.671875,
	    },
	  },
	  [925] = {
	    [1] = {
	      height = 143.33996582031,
	      width = 215.009765625,
	      xOffset = 4642.4951171875,
	      yOffset = -2106.669921875,
	    },
	  },
	  [928] = {
	    [1] = {
	      floorName = "Ghostly Veins",
	      height = 320,
	      width = 480,
	      xOffset = -5546,
	      yOffset = -5942,
	    },
	    [2] = {
	      floorName = "The Swollen Vault",
	      height = 241.3330078125,
	      width = 362,
	      xOffset = -5331,
	      yOffset = -7241.3315429688,
	    },
	  },
	  [930] = {
	    [1] = {
	      floorName = "Overgrown Statuary",
	      height = 856.6669921875,
	      width = 1285,
	      xOffset = -7027.5,
	      yOffset = -6113.3334960938,
	    },
	    [2] = {
	      floorName = "Royal Amphitheater",
	      height = 1033.33984375,
	      width = 1550.009765625,
	      xOffset = -6175.0048828125,
	      yOffset = -6246.669921875,
	    },
	    [3] = {
	      floorName = "Forgotten Depths",
	      height = 686.6669921875,
	      width = 1030,
	      xOffset = -5245,
	      yOffset = -6605.8334960938,
	    },
	    [4] = {
	      floorName = "Roost of Ji-Kun",
	      height = 394.18701171875,
	      width = 591.28002929688,
	      xOffset = -4609.6401367188,
	      yOffset = -6312.8735351563,
	    },
	    [5] = {
	      floorName = "Halls of Flesh-Shaping",
	      height = 686.6669921875,
	      width = 1030,
	      xOffset = -5245,
	      yOffset = -6128.3334960938,
	    },
	    [6] = {
	      floorName = "Hall of Kings",
	      height = 606.6669921875,
	      width = 910,
	      xOffset = -4950,
	      yOffset = -6402.8334960938,
	    },
	    [7] = {
	      floorName = "Pinnacle of Storms",
	      height = 540,
	      width = 810,
	      xOffset = -4505,
	      yOffset = -5985,
	    },
	    [8] = {
	      floorName = "Hidden Cell",
	      height = 411.6669921875,
	      width = 617.5,
	      xOffset = -4978.75,
	      yOffset = -5733.3334960938,
	    },
	  },
	  [933] = {
	    [1] = {
	      height = 320,
	      microName = "GhostlyVeins",
	      width = 480,
	      xOffset = -5772.5,
	      yOffset = -7530,
	    },
	  },
	  [937] = {
	    [1] = {
	      height = 380,
	      width = 570,
	      xOffset = -1351.25,
	      yOffset = -1000,
	    },
	  },
	  [951] = {
	    [22] = {
	      floorName = "Cavern of Lost Spirits",
	      height = 275,
	      microName = "CavernofLostSpirits",
	      width = 412.5,
	      xOffset = 4943.75,
	      yOffset = 187.5,
	     },
	   },
	  [953] = {
	    [1] = {
	      floorName = "Scarred Vale",
	      height = 504.1669921875,
	      width = 758.3330078125,
	      xOffset = 481.25,
	      yOffset = 806.25,
	    },
	    [10] = {
	      floorName = "The Siegeworks",
	      height = 430.00012207031,
	      width = 645,
	      xOffset = 5230,
	      yOffset = -2157.080078125,
	    },
	    [11] = {
	      floorName = "Chamber of the Paragons",
	      height = 590,
	      width = 885,
	      xOffset = 5082.5,
	      yOffset = -1790,
	    },
	    [12] = {
	      floorName = "The Inner Sanctum",
	      height = 315,
	      width = 472.5,
	      xOffset = 5403.75,
	      yOffset = -1275,
	    },
	    [13] = {
	      floorName = "Terrace of Endless Spring",
	      height = 553.33703613281,
	      width = 830.005859375,
	      xOffset = 5179.9970703125,
	      yOffset = -1204.1700439453,
	    },
	    [14] = {
	      floorName = "Temple of the Jade Serpent",
	      height = 230,
	      width = 345,
	      xOffset = 5222.5,
	      yOffset = -1215,
	    },
	    [15] = {
	      floorName = "Temple of the Red Crane",
	      height = 175,
	      width = 262.5,
	      xOffset = 5709.1298828125,
	      yOffset = -1150,
	    },
	    [2] = {
	      floorName = "Pools of Power",
	      height = 633.34326171875,
	      width = 950.01501464844,
	      xOffset = -1150.0100097656,
	      yOffset = -1729.1716308594,
	    },
	    [3] = {
	      floorName = "Vault of Y'Shaarj",
	      height = 375,
	      width = 562.5,
	      xOffset = -1278.75,
	      yOffset = -1000,
	    },
	    [4] = {
	      floorName = "Gates of Orgrimmar",
	      height = 761.11322021484,
	      width = 1141.669921875,
	      xOffset = 4020.830078125,
	      yOffset = -1654.5166015625,
	    },
	    [5] = {
	      floorName = "The Valley of Strength",
	      height = 806.25,
	      width = 1210.416015625,
	      xOffset = 3789.583984375,
	      yOffset = -2125,
	    },
	    [6] = {
	      floorName = "The Cleft of Shadow",
	      height = 241.39318847656,
	      width = 362.08984375,
	      xOffset = 4163.9702148438,
	      yOffset = -1932.2716064453,
	    },
	    [7] = {
	      floorName = "The Descent",
	      height = 400,
	      width = 600,
	      xOffset = 4237.5,
	      yOffset = -1900,
	    },
	    [8] = {
	      floorName = "Kor'Kron Barracks",
	      height = 590,
	      width = 885,
	      xOffset = 4582.5,
	      yOffset = -2165,
	    },
	    [9] = {
	      floorName = "The Menagerie",
	      height = 806.66674804688,
	      width = 1210,
	      xOffset = 4490,
	      yOffset = -1865.8333740234,
	    },
	  },
}

local function ImportCaves()
	for zid,zdata in pairs(MICRODUNGEONS) do
		local mapdata = WorldMapSize[zid]
		if mapdata then
			for floor,data in pairs(zdata) do
				mapdata[floor]=data
			end
		end
	end
end

ImportCaves()

local function CalcFloorMinMax()
	for z,zonedata in pairs(WorldMapSize) do
		zonedata.floorMin = (zonedata.width and zonedata.width>0 and 0 or 999)  -- 0 for default floor when there's some coords here; otherwise wait for other floors to bring it down to a sane level.
		zonedata.floorMax = 0
		for fl,fldata in pairs(zonedata) do
			if type(fl)=="number" then
				if fl<zonedata.floorMin then zonedata.floorMin=fl end
				if fl>zonedata.floorMax then zonedata.floorMax=fl end
			end
		end
	end
end

CalcFloorMinMax()

function Astrolabe:CalculateMicroDungeon()
	local m,f,x,y = Astrolabe:GetUnitPosition("player")
	local microName = select(5,GetMapInfo())

	if not MICRODUNGEONS[m] then MICRODUNGEONS[m]={} end
	local mapData = MICRODUNGEONS[m]

	print("|cff88ff00Captain Caveman reporting:")

	if not microName then
		if Astrolabe.minimapOutside then
			print("|cffff8800  We need a cave, Fred.")
		else
			print("|cffff8800  That's no cave, man.")
		end
	else
		if Astrolabe.minimapOutside then
			print("|cff88ff00What the heck is this, an outdoor cave?|r",microName)
		else
			print("|cff88ff00Houston, we have a cave here. Codename:|r",microName)
		end
	end

	HARVEST = {}
	harvestMapData(HARVEST)

	--ZGV:ShowDump(ZGV:Serialize(HARVEST))

	local data = HARVEST[m]

	-- kill the 0 floor, that's by all accounts the main map
	--data[0] = nil

	for f, harvData in pairs(data) do
		if ( type(f) == "number" and f > 0 ) then
			mapData[f] = {}
			local floorData = mapData[f]
			--[[
			floorData.width = harvData.BRx - harvData.TLx
			floorData.height = harvData.BRy - harvData.TLy
			floorData.xOffset = harvData.TLx
			floorData.yOffset = harvData.TLy
			--]]
			floorData.width = harvData.BRx - harvData.TLx
			floorData.height = harvData.BRy - harvData.TLy
			floorData.xOffset = -harvData.BRx
			floorData.yOffset = -harvData.BRy

			floorData.floorName = harvData.floorName
			floorData.microName = harvData.microName
		end
	end

	--mapData.system = WorldMapSize[m].system
	--mapData.systemParent = WorldMapSize[m].systemParent
	--mapData.system = m

	ImportCaves()
	CalcFloorMinMax()

	if IsShiftKeyDown() then
		ZGV:ShowDump(ZGV:Serialize(MICRODUNGEONS))
	else
		local oneZone = {}
		oneZone[m]=mapData
		ZGV:ShowDump(ZGV:Serialize(oneZone),"Cave report:")
		local count=0
		for f,fd in pairs(mapData) do if type(f)=="number" then count=count+1 end end
		print(" Status:")
		print("  Zone:",GetMapNameByID(m)," [",m,"]")
		print("  Caves (",count,"):")
		for f,fd in pairs(mapData) do if type(f)=="number" then print("  -",f,":",fd.floorName) end end
	end
end

function CAVEMAN()
	Astrolabe:CalculateMicroDungeon()
end



  -- NOT NEEDED till we get a new continent. Hibernating this. Sleep tight, little function.

SURVEYS = {zone={},cont={},world={}}

-- Dun'Morogh survey
--[=[
SURVEYS.zone = {
    x1 = 0.27232903242111,
    x2 = 0.43178981542587,
    y1 = 0.54211765527725,
    y2 = 0.74989044666290,
}
SURVEYS.cont = {
    x1 = 0.42630857229233,
    x2 = 0.44547897577286,
    y1 = 0.62202548980713,
    y2 = 0.64700889587402,
}
SURVEYS.world = {
    x1 = 0.79937916994095,
    x2 = 0.81254434585571,
    y1 = 0.55993282794952,
    y2 = 0.57708299160004,
}
-- Dun'Morogh:
-- xOffset = -2137.5
-- yOffset = 3941.6665
--]=]

function SURVEY_ZONE()
	local x,y

	local S = SURVEYS

	SetMapToCurrentZone()
	local zone = GetCurrentMapAreaID()

	x,y = GetPlayerMapPosition("player")
	if not S.zone.x1 then S.zone.x1,S.zone.y1 = x,y
	elseif not S.zone.x2 then S.zone.x2,S.zone.y2 = x,y end

	SetMapByID(WorldMapSize[zone].system)
	x,y = GetPlayerMapPosition("player")
	if not S.cont.x1 then S.cont.x1,S.cont.y1 = x,y
	elseif not S.cont.x2 then S.cont.x2,S.cont.y2 = x,y end

	SetMapZoom(0)
	x,y = GetPlayerMapPosition("player")
	if not S.world.x1 then S.world.x1,S.world.y1 = x,y
	elseif not S.world.x2 then S.world.x2,S.world.y2 = x,y end

	if S.zone.x2 then
		-- the big calc

		S.zone.xd,S.zone.yd = math.abs(S.zone.x2-S.zone.x1),math.abs(S.zone.y2-S.zone.y1)
		S.cont.xd,S.cont.yd = math.abs(S.cont.x2-S.cont.x1),math.abs(S.cont.y2-S.cont.y1)
		S.world.xd,S.world.yd = math.abs(S.world.x2-S.world.x1),math.abs(S.world.y2-S.world.y1)

		local zonedata = WorldMapSize[zone]

		S.zone.width = zonedata.width
		S.zone.height = zonedata.height

		S.cont.scale = S.zone.xd / S.cont.xd
		S.world.scale = S.cont.xd / S.world.xd
		S.world.zscale = S.zone.xd / S.world.xd

		local yd = zonedata.width * (S.zone.x2-S.zone.x1)

		S.cont.width = S.cont.scale * S.zone.width
		S.cont.height = S.cont.width * 0.66667

		S.world.width = S.world.scale*S.zone.width
		S.world.height = S.world.width * 0.66667

		S.cont.xOffset = SURVEYS.zone.x1 * WorldMapSize[zone].width + WorldMapSize[zone].xOffset - WorldMapSize[0].xOffset - WorldMapSize[0].width * S.world.x1
		S.cont.yOffset = SURVEYS.zone.y1 * WorldMapSize[zone].height + WorldMapSize[zone].yOffset - WorldMapSize[0].yOffset - WorldMapSize[0].height * S.world.y1

		ZGV:ShowDump(ZGV:Serialize(S.cont),"Continent offsets:")
	else
		print("Okay, got one point, get me another")
	end
end