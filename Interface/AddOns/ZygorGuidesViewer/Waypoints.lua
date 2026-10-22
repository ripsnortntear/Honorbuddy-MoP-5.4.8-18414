local ZGV = ZygorGuidesViewer
if not ZGV then return end

local L = ZGV.L
local BZL = ZGV.BZL

local Astrolabe = DongleStub("Astrolabe-ZGV")

local tinsert=tinsert

function ZGV:getXY(id)
	self:Debug("getXY "..id)
	return (id % 10001)/10000, math.floor(id / 10001)/10000
end

ZGV.Waypoints = {}
local Waypoints = ZGV.Waypoints

local addonnames = {"none","internal","cart2","carbonite","tomtom"}
local addonnum = {}
for i=1,#addonnames do addonnum[addonnames[i]]=i end


function ZGV:ConnectWaypointAddon(addon)
	if not addon then addon=self.db.profile.waypointaddon end
end

function ZGV:AutodetectWaypointAddon()
	self.autodetectingwaypointaddon = true
	self:Print(L["waypointaddon_detecting"])

	local checks = {"cart2","carbonite","tomtom","internal"}
	for i=1,#checks do
		if self:IsWaypointAddonReady(checks[i]) then
			return checks[i]
		end
	end

	-- else
	self:Print(L["waypointaddon_notdetected"])
end

function ZGV:GetWaypointAddon()
	return addonnum[self.db.profile.waypointaddon] or 0
end

function ZGV:SetWaypointAddon(info,addon)
	if not addon then addon=info end
	if type(addon)=="number" then addon=addonnames[addon] end
	if not addon then
		-- try to autodetect
		addon = self:AutodetectWaypointAddon()
		if not addon then addon="none" end
	end
	addon=addon:gsub("[0-9]-_","")

	self:Debug("Setting waypoint addon: "..addon)
	if addon~="none" and not self:IsWaypointAddonReady(addon) then
		self:Print(L["waypointaddon_fail"]:format(L["opt_group_addons_"..addon]))
		return
	end

	-- disconnect the current addon
	--if (addon~=self.db.profile.waypointaddon) then
	self:UnsetWaypointAddon()
	--end

	self.db.profile.waypointaddon = addon
	--self.iconsregistered = false
	--self.iconregistryretries = 0
	self.ConnectedWaypointer = self.WaypointFunctions[addon]

	self:Print(L["waypointaddon_set"]:format(L["opt_group_addons_"..addon]))

	self:SetWaypoint()
--[[
	if (self.db.profile.waypointaddon=="none") then
		self.optionsmap.args.minicons.disabled = true
	else
		self.optionsmap.args.minicons.disabled = false
	end
	LibStub("AceConfigRegistry-3.0"):NotifyChange("ZygorGuidesViewer")
]]--
end

-- Some small utilities which may be useful to several waypointing backends
-- Moved 'em out of Internal waypointer so that TomTom, for example,
-- may equally enjoy the pleasures of nettles^W path-based navigation ~aprotas



-- Waypoint functions by addon

ZGV.WaypointFunctions = {}
--=========================================== TOMTOM =============================================
ZGV.WaypointFunctions['tomtom'] = {
	isready = function()
		-- make SURE we have TomTom and not Carbonite emulating it
		return not not (TomTom and IsAddOnLoaded("TomTom"))
	end,
	setwaypoint = function (self,goalnumORx,y,title)
		local waypath=self.CurrentStep and self.CurrentStep.waypath or nil
		if not (waypath and #waypath.coords>1) then
			self:ClearTomTomWaypoints()
		end

		if goalnumORx==false then return end

		if y then
			self:CreateTomTomWaypointXY(goalnumORx,y,title,true)
		else
			local waypath=self.CurrentStep and self.CurrentStep.waypath or nil
			if waypath then
				-- Copied from Internal one not to break things with scopes etc
				local antpoints
				if waypath.ants=="straight" or #waypath.coords<3 then
					antpoints = ZGV.Pointer.spawn_straight_ants(waypath.coords,waypath.loop,ZGV.Pointer.antphase)
				else
					antpoints = ZGV.Pointer.spawn_curve_ants(waypath.coords,waypath.loop,ZGV.Pointer.antphase)
				end

				--calc_angles(antpoints,waypath.loop)
				if antpoints then
					self.Pointer:ClearWaypoints("ant")
					self.Pointer:SetAntSpacing(self.db.profile.antspacing)

					for k,point in ipairs(antpoints) do
						ZGV.Pointer:SetWaypoint_ant (point.map,point.floor,point.x,point.y, k, point.icon)
					end
				end
				-- Cut here
				self:CreateTomTomWaypath(waypath)
			else
				self:CreateTomTomWaypoints(goalnumORx)
			end
		end
	end,
	addmapnote = function (self,map,flr,x,y,data)
		self:CreateTomTomWaypointMFXY(map,flr,x,y,data.title,false)
	end,
	disconnect = function(self)
		-- TomTom can ask for clearing all waypoints; Carbonite should not.
		if StaticPopupDialogs["TOMTOM_REMOVE_ALL_CONFIRM"] then StaticPopupDialogs["TOMTOM_REMOVE_ALL_CONFIRM"]:OnAccept() end

		-- Carbonite doesn't do this, either
		if TomTomCrazyArrow then TomTomCrazyArrow:Hide() end
	end
}

do --=========================================== CARBONITE =============================================

	-- mapid_to_cz removed

	local lastwaypath -- Optimization trick, last waypath we were asked to display

	local function ClearCarboniteWaypoints()
		local map=Nx.Map:GetMap(1)
		if map then wipe(map.Targets) end
	end

	-- Finds an optimal travel route.
	local function FindTravelPathCarbonite(mid,x,y,data)
		--if not way then return end
		--if way.pathnode and way.pathnode.player then return end -- no pointing to self
		--self:ShowArrow(way)
		--if way.type=="route" then return end
		ZGV:Debug("&pointer FindTravelPathCarbonite")
		--Nx.ZygorTravelDestination = way
		LibRover:FindPath(0,0,0,0,mid,0,x,y, ZGV.WaypointFunctions.carbonite.PathFoundHandler, {title=data.title})
	end

	local function CreateCarboniteWaypointMXY(mapid,x,y,title,keep,pathfind)
		local map=Nx.Map:GetMap(1) if not map then return end
		local mid=Nx.AIdToId[mapid] -- AIdToId → AreaID to Carbonite ID ~ aprotas
		if mid then
			-- TODO should we need to ever give floor id?
			map:SetTargetXY(mid,x,y,title,keep)
			if pathfind and ZGV.db.profile.pathfinding then FindTravelPathCarbonite(mapid,x,y,{title=title}) end
			--map:ChangeTargetOrder(-1,1) -- TODO I have neither ability nor desire to find out what it does, but I'm just mimicking Nx.TTST behaviour here ~aprotas
		else
			self:Debug("Carbonite does not have an ID for map "..mapid.." please report")
		end
	end

	local function RecycleRouteCarbonite()
		-- WARNING we are being of evil by overriding user addon settings
		-- we could possibly restore it back but do we really have to?
		local map=Nx.Map:GetMap(1)
		if map then
			if not map.GOpts["RouteRecycle"] then
				ZGV:Debug("Forcing Carbonite setting: RouteRecycle")
				map.GOpts["RouteRecycle"]=true
			end
		end
	end

	local function OptimizeRouteCarbonite()
		-- TODO make sure this does not reverse the direction when it's NOT needed
		-- TODO testing on multipoint "regular" waypoints
		-- TODO testing on acyclic waypaths
		local map=Nx.Map:GetMap(1)
		if map then
			map:RouteTargets()
		end
	end

	-- Makes the waypoints set clear out at long (60 yard) radius
	local function LongClearanceCarbonite()
		local map=Nx.Map:GetMap(1)
		if map and map.Targets then
			-- Setting the distances
			local DIST=60 -- WARNING: hardcoded value
			local sqrt=math.sqrt
			for k,v in ipairs(map.Targets) do
				v.Rad=DIST
				if #map.Targets>1 then -- Making sure we're giving enough space not to overlap the previous point
					local u=map.Targets[k==1 and #map.Targets or k-1]
					local dist=sqrt((v.TargetMX-u.TargetMX)^2+(v.TargetMY-u.TargetMY)^2)	-- Distance between the points
					-- Correcting the waypath distance to the next waypoint
					if v.Radius>=dist and dist>0 then
						v.Radius=dist/2-1
						if v.Radius<1 then v.Radius=1 end
						ZGV:Debug("Waypoints ["..k.."→"..(k==1 and #map.Targets or k-1).."] are too packed, correcting clear distance to "..v.Radius)
					end
				end
			end
		end
	end

	-- Makes the waypoints not clear at all
	local function NoClearanceCarbonite()
		local map=Nx.Map:GetMap(1)
		if map and map.Targets then
			for k,v in ipairs(map.Targets) do
				v.Radius=-1 -- Carbonite must be -very- astonished
			end
		end
	end

	ZGV.WaypointFunctions['carbonite'] = {
		isready = function()
			return not not (Nx and Nx.Map)
		end,
		setwaypoint = function (self,goalnumORx,y,title)
			local map=Nx.Map:GetMap(1)

			local waypath=self.CurrentStep and self.CurrentStep.waypath or nil

			if not (waypath and #waypath.coords>1) then
				ClearCarboniteWaypoints() -- There may be a need to preserve current waypath points
			end

			if goalnumORx==false then
				-- clear! TODO
				return
			end

			if y then
				CreateCarboniteWaypointMXY(GetCurrentMapAreaID(),goalnumORx,y,title,not "keep","pathfind")
				--map:SetTargetAtStr("%.2f %.2f",goalnumORx,y)
				return
			end

			if not self.CurrentStep or not self.CurrentStep.goals then return end
			--	if self.CurrentStep.mapnote then

			local x,y,zone
			local waypointscount

			if waypath and #waypath.coords>1 then
				if waypath~=lastwaypath then
					ClearCarboniteWaypoints() -- Postponed wiping action, saving the waypath setting
					local cordcount=#waypath.coords
					lastwaypath=waypath -- Remembering the waypath not to update it every time
					-- TODO please optimize this ~aprotas

					self:Debug("New waypath available, installing the points")

					waypointscount=0
					-- Carbonite has a noice ant system by itself, let's make use of it
					for i=1,cordcount do -- TODO reverse iterator, cmon, there should be one
						v=waypath.coords[cordcount-i+1]
						CreateCarboniteWaypointMXY(v.map,v.x*100,v.y*100,k,i>1,not "pathfind") -- TODO Where's the title?
						waypointscount=waypointscount+1
					end

					RecycleRouteCarbonite()
				end
			else
				lastwaypath=nil -- No waypath in this step, good
				-- "Simply" setting the waypoint
				for i=#self.CurrentStep.goals,1,-1 do
					local goal = self.CurrentStep.goals[i]
					local first=true
					if goal.map and goal.x and not goal.force_noway then
						self:Debug("Placing Carbonite waypoint")
						CreateCarboniteWaypointMXY(goal.map,goal.x*100,goal.y*100,goal.title or self.CurrentStep.title or goal.autotitle or self.CurrentStep:GetWayTitle() or "Step "..self.CurrentStepNum,not first, first and "pathfind")
						first=false
						waypointscount=(waypointscount or 0)+1
					end
				end
			end

			-- Making use of Carbonite waypoint organizer whereas possible
			if waypointscount then
				if waypointscount>1 then
					OptimizeRouteCarbonite()
					LongClearanceCarbonite()
				else
					NoClearanceCarbonite()
				end
			end

		end,
		clearmapnotes = function (self)
			local folders = Nx.Fav:FiF("Notes")
			for i,fol in ipairs(folders) do
				for j=1,#fol do
					if fol[j] and fol[j]:match("~#~.*%(ZG%)~") then
						tremove(fol,j)
						j=j-1
					end
				end
			end
			Nx.Fav:Upd()
		end,
		addmapnote = function (self,map,flr,x,y,data)  -- UNUSED
			--[[
			local folder=Nx.Fav:FiF("Zygor Guides")
			if not folder then
				folder=Nx.Fav:AdF1("Zygor Guides")
			end
			local fav = Nx.Fav:FiF1("Gold Guide","Name",folder)
			if not fav then
				fav=Nx.Fav:AdF2("Gold Guide",folder)
				fav["ID"]=maI
				sort(fav,function(a,b) return a["Name"]<-b["Name"] end)
			end

			local carbZone = map  --Nx.MNTI1[map] --zone IDs
			local fav = Nx.Fav:GNF(carbZone)
			local s=Nx.Fav:CrI("N",0,(data and data.title or "Gold Spot") .. " (ZG)",3,carbZone,x,y)
			Nx.Fav:AdI1(fav,nil,s)
			-- ...
			Nx.Fav:Upd()

			--]]
			--Nx:TTSTCZXY(contid,zoneid,x,y,data and data.title,false,true,true,nil)  -- cont,zone,x,y,name,persist,minimap,world,data
		end,
		disconnect = function(self)
			ClearCarboniteWaypoints()
		end,
		PathFoundHandler = function(state,path,ext)
			--if ZGV.Pointer.corpsearrow then return end

			if ext and ext.token and ext.token~=Nx.ZygorTravelDestination then ZGV:Debug("Found wrong path!") return end

			local map=Nx.Map:GetMap(1)

			if state=="success" then
				ClearCarboniteWaypoints() --Pointer:ClearSet("route")
				--local future_waypoints = {follow="pathfind",loop=false,ants="straight",coords={}}

				local first=true
				for i,node in ipairs(path) do while 1 do
					local icon
					if node.player then break end
					--and node.type~="end" then icon=Pointer.Icons.arrow else icon=Pointer.Icons.none end  -- start and end nodes are HIDDEN.

					CreateCarboniteWaypointMXY(node.m,node.x*100,node.y*100,node.maplabel,not first and "keep",not "pathfind")
					first = false
					do break end

					--[[

					local wayp = {map=node.m,floor=node.f,x=node.x,y=node.y, title=node.maplabel, arrowtitle=node.text,player=node.player, type="route", icon=icon, pathnode=node}
					-- force_noway=node.player,

					if node.t=="end" then wayp.noskip = true end

					-- set up skipping

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
					--]]

					-- set up ant colors
					-- NOT in Carbonite.

				break end end

				--[[
				if ZGV.Pointer.CurrentPathTarget then
					local w = ZGV.Pointer.CurrentPathTarget
					tinsert(ZGV.Pointer.TempWaypath.coords, {map=w.m,floor=w.f,x=w.x,y=w.y,title=w.title,force_noway=true})
				end
				--]]

				--Pointer.ArrowFrame.waypoint = nil -- clear arrow, so that it updates around line 804 this file. We :SetWaypoint right under here.
				--FAILED_PATH = nil
				--Pointer:ShowSet(future_waypoints,"route") -- clear none, just refresh

				--Pointer:ShowArrow(Pointer.pointsets.route.points[2])  -- point 1 is player

			elseif state=="failure" then
				--Pointer:ClearSet("route")
				ZGV:Debug("Path Not Found!")
				--FAILED_PATH = ZGV.Pointer.DestinationWaypoint

				--Pointer:ShowArrow(ZGV.Pointer.DestinationWaypoint)

				--ZGV:SetWaypoint(nil,nil,nil,"path",{dontcleartemp=true}) -- clear none, just refresh

			-- Causing the Lovely arrow problems ~Errc
			-- Well let's cure, not amputate ;) ~sin
			elseif state=="arrival" then
				--Pointer:ClearSet("route")
				ZGV:Debug("Waypoints: Arrived at destination!")
				--FAILED_PATH = ZGV.Pointer.CurrentPathTarget
				--Pointer:ShowArrow(ZGV.Pointer.DestinationWaypoint)

				--Pointer:ResetFollowing()
			elseif state=="progress" then
				-- DON'T clear anything.
				--Pointer:ShowWaiting(ext and ext.progress or 0)
			end
			--ZGV:SetWaypoint() -- clear none, just refresh
		end
	}
end

ZGV.WaypointFunctions['cart2'] = {
	isready = function()
		return not not (Cartographer_Notes and Cartographer_Notes:IsActive() and Cartographer_Notes.externalDBs)
	end,

	clearmapnotes = function (self)
	end,
	addmapnote = function (self,map,flr,x,y,data)
	end,

	setwaypoint = function (self,goalnumORx,y,title)
		self:Debug("Setting cart2 waypoint")
		--self:Debug(self.CurrentStep.mapnote)
	--[[
		if self.oldnote then
			Cartographer_Notes:DeleteNote(self.oldzone,self.oldnote)
		end
	--]]
		self:UpdateCartographerExport()
		self:ClearCartographerWaypoints()
		if goalnumORx==false then return end

		if y then
			self:CreateCartographerWaypointXY(goalnumORx,y,title)
		else
			self:CreateCartographerWaypoints(goalnumORx)
		end

		--[[
		local queue = Cartographer_Waypoints.Queue
		for i,v in ipairs(queue) do
			if v and v.Db=="ZygorGuides" then
				table.remove(queue,i)
			end
		end
		--]]

		--local note = Cartographer_Notes:SetNote(zone,x/100,y/100,"Circle","ZygorGuidesViewer",'manual',true,'title',)
	--		if mapnote and mapzone and Cartographer_Notes.externalDBs then
	--			Cartographer_Waypoints:SetNoteAsWaypoint(mapzone,mapnote)
	--		end
	--		self.oldzone = zone
	end,
	disconnect = function(self)
		self:ClearCartographerWaypoints()
		--if Cartographer_Notes and Cartographer_Notes.externalDBs and Cartographer_Notes.externalDBs["ZygorGuidesViewer"] then
		Cartographer_Notes:UnregisterNotesDatabase("ZygorGuidesViewer")
	end
}

ZGV.WaypointFunctions['cart3'] = {
	isready = function()
		return not not (Cartographer3 and Cartographer3.db)
	end,
	disconnect = function(self)
		--[[
		if Cartographer3 and Cartographer3.db then
			self:Print("Cartographer3 disconnected.")
		else
			self:Print("Cartographer3 not connected.")
		end
		--]]
	end
}

ZGV.WaypointFunctions['metamap'] = {
	isready = function()
		return false
	end
}

--============================================= internal =============================================

local farmPathEvent=CreateFrame("Frame")
farmPathEvent:RegisterEvent("ZONE_CHANGED_NEW_AREA")
farmPathEvent:RegisterEvent("PLAYER_CONTROL_GAINED")
local inZone

local setwaypoint_data={persistent=true,overworld=true}
ZGV.WaypointFunctions['internal'] = {
	isready = function(self)
		return not not self.Pointer.ready
	end,
	setwaypoint = function (self,goalnumORx,y,title,only_type,extradata) while(1) do

		--if only_type~="ant" then print("|cff8000ff SetWaypoint "..(only_type or "?")) end


		-- Don't do anything if we have a corpse arrow
		if self.Pointer.corpsearrow or self.Pointer:IsCorpseArrowNeeded() then
			break --------------------------------------------------------------------------------
		end

		local t1=debugprofilestop()

		if not ZGV.db.profile.waypointerredux and false then
			-- OLD WAY

			if only_type=="ant" then
				self.Pointer:ClearWaypoints_ant(#ZGV.Pointer.antpoints)
			elseif only_type then
				self.Pointer:ClearWaypoints(only_type)
			else
				self.Pointer:ClearWaypoints("way")
				self.Pointer:ClearWaypoints("path")
				self.Pointer:ClearWaypoints_ant(0)
				wipe(self.Pointer.antpoints)  ants_set=nil

				LibRover:Abort()

				ZGV.Pointer.CurrentPathTarget = nil

				--if not PATHFOUND_TO_MANUAL then self.Pointer.TempWaypath=nil end

			end
			if goalnumORx==false then
				self.Pointer.TempWaypath = nil

				return
			end

			if type(goalnumORx)=="number" and not y then
				-- goal number given: show just that goal, remove manuals.
				ZGV:Debug("WAY clicked goal, killing manuals")
				self.Pointer:ClearWaypoints("manual")
			end

			--self.Pointer:HideArrow() --should hide itself if waypoint is removed
			-- self.Pointer.TempWaypath dies automatically with its target waypoint!

			--[[
			if PATHFOUND_TO_MANUAL and self.Pointer.nummanual==0 then
				self.Pointer.TempWaypath=nil
				PATHFOUND_TO_MANUAL = nil
			end
			--]]

			if y then
				ZGV:Debug("Waypoint coords: %.2f %.2f",goalnumORx,y)
				-- coords, plain as day
				wipe(setwaypoint_data)  setwaypoint_data.persistent=true  setwaypoint_data.overworld=true
				setwaypoint_data.title=title
				setwaypoint_data.type=only_type
				local way = self.Pointer:SetWaypoint (nil,nil,goalnumORx,y,setwaypoint_data)


				if self.db.profile.pathfinding and not LibRover.is_stub then
					ZGV.Pointer.CurrentPathTarget = way
					--if way~=FAILED_PATH then
					LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
					--end
				else
					ZGV.Pointer.CurrentPathTarget = nil
				end
			end


			local map = GetCurrentMapAreaID()
			local overworld = (map==13 or map==14 or map==0 or map==689 or map==-1 or map==485 or map==466)
			self.Pointer:SetAntSpacing(overworld and self.db.profile.antspacing*10 or self.db.profile.antspacing)

			local step = self.CurrentStep

			local arrowpoint,pathpoint,farmpoint


			local activeants=0

			local function set_waypoints(points,worldsize,minisize,type)
				local pathmode = (type=="path")
				local antmode = (type=="ant")
				for k,point in ipairs(points) do
					if not point.force_noway and point.x then
						if not antmode then

							wipe(setwaypoint_data)  setwaypoint_data.persistent=true  setwaypoint_data.overworld=true
							local data = setwaypoint_data

							data.title = not antmode and
									(title
									or point.waytitle or point.title
									or (pathmode and (step and step.waypath and step.waypath.title and step.waypath.title or "Path").." ("..k..")")
									or (step and step:GetWayTitle())
									or (point.map and point.x and ("%s %d,%d"):format(ZGV.Pointer.GetMapNameByID2(point.map),point.x*100,point.y*100))
									or L['waypoint_step']:format(self.CurrentStepNum)
									)
							data.onminimap="always"
							data.showonedge=not antmode and not pathmode
							data.passive=antmode
							data.type=type
							data.num=k

							for k,v in pairs(point) do if k~="map" and k~="floor" and k~="x" and k~="y" then data[k]=v end end
							local way = self.Pointer:SetWaypoint (point.map,point.floor,point.x,point.y, data, false)
							point.pointer_way=way
							if way then
								if (not antmode and not point.player) or point.arrow or type=="manual" then
									arrowpoint = arrowpoint --[[or way.surrogate_for--]] or way
									farmpoint = arrowpoint or way
									--pathpoint = pathpoint or way  -- in case they ARE different somehow
								end
								-- or (step.waypath and step.waypath.current==point)
							else
								--self:Print(("Unabmle to create waypoint: %s/%d %.2f %.2f"):format(point.map,point.floor,point.x,point.y))
							end
						else -- ants!!!
							ZGV.Pointer:SetWaypoint_ant (point.map,point.floor,point.x,point.y, k, point.icon)
						end
					end
				end
				if antmode then activeants=#points end
			end

			local function update_ant_waypoints(points)  -- optimized for ants
				for k,point in ipairs(points) do
					local way = point.pointer_way
					if way then
						way.map,way.floor,way.x,way.y=point.map,point.floor,point.x,point.y
					end
				end
			end
			-- TODO we are updating this constantly. that's really inefficient ~aprotas
			-- No, we're not really re-defining this function over and over. ~sinus

			 --or (step and step.waypath)
			local farmpath=(step and step.waypath)
			local waypath = ZGV.Pointer.TempWaypath

			-- manuals don't mix well with farm/follow paths. REMOVE manuals, when finding a path. Oh well.
			-- OR NOT?


			--[[
			if step and step.waypath and ZGV.Pointer.TempWaypath and self.Pointer.nummanual>0 then
				self.Pointer:ClearWaypoints("manual")
				ZGV.Pointer.TempWaypath=nil
				waypath = step and step.waypath
			end--]]


			local function showPaths(waypath)
				if waypath and #waypath.coords>1 then  -- show ants, or just the path, anyway.
					self.Pointer.pathfollow = waypath.follow
					self.Pointer.pathloop = waypath.loop

					for wpi,wp in ipairs(waypath.coords) do
						wp.curve_accuracy=nil  -- clear this cached value, we might change accuracy.

						-- point is player? get new location
						if wp.player then

							local m,f,x,y = Astrolabe:GetCurrentPlayerPosition("last")
							wp.map,wp.floor,wp.x,wp.y = m,f,x,y
							wp.gx,wp.gy,wp.gm,wp.gf = nil,nil,nil,nil
							wp.icon=nil
							--ZGV.Pointer:SetWaypoint (wp.map,wp.floor,wp.x,wp.y,nil,nil)--data,arrow)
							--ants=spawn(waypath)
							--show(waypath, ants)
						end

						--globalize position! fill gm,gx,gy with world-global values. Otherwise ants can't travel over zone crossings.
						if not wp.gx and wp.map then
							local mastermap = Astrolabe.WorldMapSize[wp.map].systemParent or 0
							local masterminflr = ZGV:SanitizeMapFloor(mastermap,0)
							--if Astrolabe.WorldMapSize[wp.map].system==466 then mastermap=466 end  -- outland, do NOT translate onto Azeroth
							--if Astrolabe.WorldMapSize[wp.map].system==640 then mastermap=640 end  -- deepholm, do NOT translate onto Azeroth
							--if wp.c==-1 then mastermap=Astrolabe.WorldMapSize[wp.map].system end  -- instances, do NOT translate onto Azeroth
							if wp.map~=mastermap then
								wp.gx,wp.gy = Astrolabe:TranslateWorldMapPosition( wp.map, wp.floor, wp.x, wp.y, mastermap, masterminflr )
							end
							if wp.gx then
								wp.gm,wp.gf=mastermap,masterminflr
							else
								wp.gm,wp.gf=wp.map,wp.floor
								wp.gx,wp.gy=wp.x,wp.y
							end
						end
					end

					if waypath.ants and ZGV.Pointer.curve_spacing>0
					and only_type=="ant"
					then --and not step.waypath_curved
						--step.waypath_curved = true

						local antpoints
						--print("spawning, player = "..waypath.coords[1].x)
						if waypath.ants=="straight" or #waypath.coords<3 then
							antpoints = ZGV.Pointer.spawn_straight_ants(waypath.coords,waypath.loop,ZGV.Pointer.antphase)
						else
							antpoints = ZGV.Pointer.spawn_curve_ants(waypath.coords,waypath.loop,ZGV.Pointer.antphase)
						end

						--calc_angles(antpoints,waypath.loop)
						if antpoints then
							if ants_optimized_which_isnt_implemented then
								if ZGV.Pointer.ants_set then
									update_ant_waypoints(antpoints)
								else
									set_waypoints(antpoints,35,30,"ant")
									ZGV.Pointer.ants_set=true
								end
							else
								set_waypoints(antpoints,35,30,"ant")
							end
						end
					end

					if not only_type or only_type=="path" then
						if waypath.ants=="curved" then
							ZGV.Pointer.calc_angles_curved(waypath.coords,waypath.loop)
						else
							ZGV.Pointer.calc_angles(waypath.coords,waypath.loop)
						end

						for k,point in ipairs(waypath.coords) do
							if point.angle and self.Pointer.pathloop then
								point.icon = ZGV.Pointer.Icons.none
							elseif point.angle then
								point.icon=ZGV.Pointer.Icons.arrow
							else
								point.icon=ZGV.Pointer.Icons.dot
							end
						end
						set_waypoints(waypath.coords,nil,nil,"path")
					end
				else
					self.Pointer.pathfollow = nil
				end

			end


			if farmpath and waypath and GetCurrentMapAreaID()~=farmpath.coords[1].map then
				showPaths(waypath)
				showPaths(farmpath)
				ZGV.Pointer.pathfollow="pathfind"
			elseif farmpath and waypath and GetCurrentMapAreaID()==farmpath.coords[1].map then -- only show the farmpath when in the zone
				showPaths(farmpath)
			else
				local waypathing = waypath or farmpath
				showPaths(waypathing)
			end



			ZGV.Pointer:ClearWaypoints_ant(activeants)

			if only_type=="ant"  then return end   --------------------- only non-ants beyond this point

			local points
			if step and step.goals and not only_type then
				ZGV:Debug("WAY showing goal(s)")
				if goalnumORx then
					points={step.goals[goalnumORx]}
				else
					points={}
					for i=1,#step.goals do
						if step.goals[i].x then tinsert(points,step.goals[i]) end
					end
				end

				if #points>0 then
					ZGV:Debug("setting wayps, waypath=%s, cpt=%s",tostring(waypath),tostring(ZGV.Pointer.CurrentPathTarget))
					set_waypoints(points,35,30,"way")








					if not waypath
					and step.L
					and not ZGV.Pointer.CurrentPathTarget
					then
						ZGV:Debug("WAY arrowpoint=%s",(arrowpoint and arrowpoint.type or "NONE"))
						if self.db.profile.pathfinding then  ZGV.Pointer.CurrentPathTarget = arrowpoint  else  ZGV.Pointer.CurrentPathTarget = nil  end
						Path=ZGV.Pointer.CurrentPathTarget
					end

				end
			end

			-- ALL MAP POINTS ARE IN PLACE!
			-- Now point to some of them, perhaps...

			 function changeZone()
				if not UnitIsDeadOrGhost("player") and farmpoint and farmpath and not WorldMapButton:IsVisible() and farmpath.coords then
					if GetCurrentMapAreaID()==farmpath.coords[1].map then

						ZGV.Pointer.TempWaypath=nil
						ZGV.Pointer:RemoveWaypoint(farmpoint)
						ZGV:SetWaypoint()
						showPaths(farmpath)
						LibRover:Abort()
					else
						local way=farmpoint

						if way.num~=1 then
							LibRover:FindLastPath()
						else
							if way.map and not way.m then
								LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
							else
								LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
							end
						end

						LibRover:UpdateNow()
					end
				end
			end

			farmPathEvent:SetScript("OnEvent", changeZone)

			if arrowpoint and not UnitIsDeadOrGhost("player")
			and not self.Pointer.ArrowFrame.waypoint
			and not arrowpoint.passive
			then
				-- don't overwrite the stinking arrow
				self.Pointer:ShowArrow (arrowpoint)
			end


			-- And perhaps initiate a search for a path.


			oldpathtarget="dupa" -- ah fuck, waypoints are REUSED!    foo=way ; way = ZGV.Pointer:SetWaypoint(...) ; assert(foo==way)
			local way = ZGV.Pointer.CurrentPathTarget
			if self.db.profile.pathfinding then

				if way~=oldpathtarget and only_type~="path" then

					--oldpathtarget = way
					if way and not farmpath then
					--and way~=FAILED_PATH
						if way.map and not way.m then
							LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
						else
							LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
						end

					elseif farmpath and farmpoint then
						way=farmpoint

						if way.map and not way.m then
							if way.map~=GetCurrentMapAreaID() then
								LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
							end
						else
							if way.m~=GetCurrentMapAreaID() then
								LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
							end
						end
					end
				end
			else
				LibRover:Abort()
			end










		else









			-- NEW WAY
			if goalnumORx==nil then
				ZGV:Debug("&waypoints Using waypointerredux: show step waypoint")
			else
				ZGV:Debug("&waypoints Using waypointerredux: %.3f %.3f title=%s only_type=%s",goalnumORx or -1,y or -1,title or "nil",only_type or "nil")
			end

			-- Clear stuff out

			self.Pointer:ClearWaypoints("way")
			--self.Pointer:ClearWaypoints("manual")
			--self.Pointer:ClearWaypoints("path")
			--self.Pointer:ClearSet("route")

			ZGV.Pointer:ClearSet("route")
			ZGV:Debug("&waypoints clearset route done")


			--LibRover:Abort()
			--ZGV.Pointer.DestinationWaypoint = nil

			if (type(goalnumORx)=="number" and not y) -- goal number given: show just that goal, remove manuals
			or (ZGV.Pointer.nummanual>0 and ZGV.Pointer.DestinationWaypoint and ZGV.Pointer.DestinationWaypoint.type~="manual") -- there's a manual, but abandoned
			then
				self.Pointer:ClearWaypoints("manual")
			end


			-- false: HIDE THE WAYPOINTS!
			if goalnumORx==false then -- well, already cleared...
				ZGV:Debug("&waypoints Clearing, out.")
				break ------------------------------------------------------------------------------------------------
			end


			-- Coordinates given: SHOW ONE WAYPOINT (and let waypointer path to it if it wants to)
			if y then
				ZGV:Debug("&waypoints SetWaypoint coords: %.2f %.2f",goalnumORx,y)
				-- coords, plain as day
				wipe(setwaypoint_data)
				setwaypoint_data.persistent=true
				setwaypoint_data.overworld=true
				setwaypoint_data.title=title
				setwaypoint_data.type="way"
				setwaypoint_data.is_destination=true
				setwaypoint_data.find_path=true
				local way = self.Pointer:SetWaypoint (nil,nil,goalnumORx,y,setwaypoint_data)
				break ------------------------------------------------------------------------------------------------
			end

			if not ZGV.CurrentStep then ZGV:Debug("&waypoints No step to point to.") break end

			---------------- NORMAL FUNCTION -------------

			if type(goalnumORx)=="number" and not y then  ZGV:Debug("&waypoints Pointing to goal %d")  end


			local step = self.CurrentStep
			local arrowpoint,pathpoint,farmpoint

			-- SHOW FARM PATH.

			local farmpath = step and step.waypath
			if farmpath then
				ZGV.Pointer:ShowSet(farmpath,"farm")
				--if not farmpath.loop then
				--	arrowpoint = ZGV.Pointer.pointsets["farm"].points[1]
				--end
			else
				ZGV.Pointer:ClearSet("farm")
			end

			-- SHOW STEP WAYPOINTS.

			local pointed = false

			local points
			if step and step.goals and not only_type then
				ZGV:Debug("&waypoints showing goal(s)")
				if goalnumORx then
					points={step.goals[goalnumORx]}
				else
					points={}
					for i,goal in ipairs(step.goals) do
						if goal.x then tinsert(points,goal) end
						if goal.ways then
							for j,way in ipairs(goal.ways) do
								tinsert(points,way)
							end
						end
					end
				end

				if #points>0 then
					ZGV:Debug("&waypoints setting wayps: #ps=%d, waypath=%s, destwaypt=%s",#points,tostring(waypath),tostring(ZGV.Pointer.DestinationWaypoint))

					arrowpoint,farmpoint = ZGV.Pointer.set_waypoints(points,35,30,"way")
				end
			end

			-- Waypoints are set.

			-- NOW, POINT THE ARROW / PLOT THE PATH.
			ZGV:Debug("&waypoints arrowpoint",arrowpoint,"farmpoint",farmpoint,"farmpath",farmpath)


			local function onChangeZone()	-- who the hell named a function "changeZone" when it doesn't change a zone? ~sinus
				if not UnitIsDeadOrGhost("player") and farmpath and farmpath.coords then
					self:Debug("&waypoints Zone changed, setting to farm path")
					ZGV:SetWaypoint()
					--[[
					if GetCurrentMapAreaID()==farmpath.coords[1].map then

						self:Debug("&waypoints We're in farm path zone! Clearing travel, just follow the route.")

						ZGV.Pointer.TempWaypath=nil
						ZGV.Pointer:RemoveWaypoint(farmpoint)
						ZGV:SetWaypoint()
						ZGV.Pointer:ShowSet(farmpath)
						LibRover:Abort()

					else

						local way=farmpoint

						if way.num~=1 then
							LibRover:FindLastPath()
						else
							if way.map and not way.m then
								LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
							else
								LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
							end
						end

						LibRover:UpdateNow()
					end
					--]]
				else
					self:Debug("&waypoints Zone changed, no farm path here")
				end
			end
			farmPathEvent:SetScript("OnEvent", onChangeZone)

			if ZGV.Pointer.nummanual>0 then
				-- leave it the hell alone.
				ZGV.Pointer:FindTravelPath(ZGV.Pointer.DestinationWaypoint)
			elseif arrowpoint then
				ZGV.Pointer:FindTravelPath(arrowpoint)
			elseif farmpath and not pointed and ZGV.Pointer.nummanual==0 then
				-- TRY TO POINT TO A FARM PATH.
				-- find center of farm path and point there
				-- BAD.

				-- If the farmpath is not looped then follow it stricting. Otherwise just go in circles in whatever order
				local pathfollow = not farmpath.loop and "strict" or "smart"

				if ZGV.CurrentMapID == farmpath.coords[1].map then
					self:Debug("&waypoints We're in farm path zone! Clearing travel, just follow the route.")
					LibRover:Abort()
					ZGV.Pointer:ClearSet("route")
					ZGV.Pointer:PointToNextInPath("farm", pathfollow)
				else
					ZGV.Pointer:FindTravelPath("farm")
				end

				--[[
				local firstcoord = farmpath.coords[1]
				if firstcoord and GetCurrentMapAreaID()~=firstcoord.map then
					local x,y=0,0
					for i,coord in ipairs(farmpath.coords) do x=x+coord.x y=y+coord.y end
					x=x/#farmpath.coords
					y=y/#farmpath.coords

					local way = ZGV.Pointer:SetWaypoint(firstcoord.map,firstcoord.floor,x,y,{icon=ZGV.Pointer.Icons.none})

					if way then ZGV.Pointer:FindTravelPath(way) end
				end
				--]]
			else
				LibRover:Abort()
			end



			--[[
			if arrowpoint and not UnitIsDeadOrGhost("player")
			and not self.Pointer.ArrowFrame.waypoint
			then
				-- don't overwrite the stinking arrow
				self.Pointer:ShowArrow (arrowpoint)
			end
			--]]

			-- And perhaps initiate a search for a path.





			--[[
			oldpathtarget="dupa" -- ah fuck, waypoints are REUSED!    foo=way ; way = ZGV.Pointer:SetWaypoint(...) ; assert(foo==way)
			local way = ZGV.Pointer.CurrentPathTarget
			if self.db.profile.pathfinding then

				if way~=oldpathtarget and only_type~="path" then

					--oldpathtarget = way
					if way and not farmpath then
					--and way~=FAILED_PATH
						if way.map and not way.m then
							LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
						else
							LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
						end

					elseif farmpath and farmpoint then
						way=farmpoint

						if way.map and not way.m then
							if way.map~=GetCurrentMapAreaID() then
								LibRover:FindPath(0,0,0,0,way.map,way.floor,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, is_indoors=way.is_indoors, waypoint=way })
							end
						else
							if way.m~=GetCurrentMapAreaID() then
								LibRover:FindPath(0,0,0,0,way.m,way.f,way.x,way.y, ZGV.Pointer.PathFoundHandler, {title=way.title, waypoint=way})
							end
						end
					end
				end
			else
				LibRover:Abort()
			end
			--]]

		end

		self:Debug("&waypoints SetWaypoint time: %.2f",debugprofilestop()-t1)
	break end	end,

	addmapnote = function (self,map,flr,x,y,data)
		local way = self.Pointer:SetWaypoint (map,flr,x,y,{title=data.title or ("%s %d,%d"):format(zone,x*100,y*100),persistent=true,overworld=true})
	end,
	disconnect = function(self)
		self.Pointer:ClearWaypoints()
	end
}

ZGV.WaypointFunctions['none'] = {
	isready = function()
		return true
	end,
	setwaypoint = function (self)
		self:Debug("No waypointing addon connected.")
	end
}

-- call empty funcs under missing indices
local nilfuncs = {__index=function() end}
for k,v in pairs(ZGV.WaypointFunctions) do setmetatable (v,nilfuncs) end


function ZGV:SetWaypoint(...)
	if not self.ConnectedWaypointer then return end
	if ...~=false and self.db.profile.hidearrowwithguide and not ZGV.Frame:IsShown() then return end
	if not self:IsWaypointAddonReady() then
		self:Print("Waypoint addon '"..self.db.profile.waypointaddon.."' failed.")
		return
	end
	self.ConnectedWaypointer.setwaypoint(self,...)
end

function ZGV:UnsetWaypointAddon()
	if not self.ConnectedWaypointer then return end
	local addon = self.db.profile.waypointaddon
	if not addon or addon=="none" then return end

	if not self:IsWaypointAddonEnabled() then
		self:Debug("Not enabled, out.")
		return
	end --nothing to do here, move along

	if not self:IsWaypointAddonReady() then return end

	self.ConnectedWaypointer.disconnect(self)
	self.ConnectedWaypointer = nil

	self:Print(L["waypointaddon_disconnected"]:format(L["opt_group_addons_"..addon]))
end

function ZGV:IsWaypointAddonReady(addon)
	if not addon then addon = self.db.profile.waypointaddon end
	return self.WaypointFunctions[addon].isready(self)
end

function ZGV:IsWaypointAddonEnabled(addon)
	if not addon then addon = self.db.profile.waypointaddon end
	return self.db.profile.waypointaddon==addon and self:IsWaypointAddonReady(addon) -- and self.iconsregistered
end



function ZGV:qRegisterNotes()
	if not self.CurrentStep then return end
	-- use for pre-registering. Cartographer needs that, while TomTom does not.

	-- retrying 3 times
	if self.iconsregistered then return end
	if not self.iconregistryretries then self.iconregistryretries=0 end
	if self.iconregistryretries==3 then
		self:Print(L["waypointaddon_fail"]:format(L["opt_group_addons_"..self.db.profile.waypointaddon]))
		if not self.autodetectingwaypointaddon then
			self:AutodetectWaypointAddon()
		end

	end
	if self.iconregistryretries>3 then return end
	self.iconregistryretries = self.iconregistryretries + 1

	if not self:IsWaypointAddonReady() then return end

	--self:Print(L["waypointaddon_connecting"]:format(self.optionsmap.args.waypoints.values[self.db.profile.waypointaddon]))

	local addon = self.db.profile.waypointaddon

	if addon=="tomtom" then
		--[[
		if not self.db.profile.filternotes then
			self:Print("Creating all waypoints for TomTom. This may take a while.")
			local contid,zoneid
			for zone in pairs(self.MapNotes) do
				local zoneTr = BZL[zone]
				contid,zoneid = self:GetMapZoneNumbers(zoneTr)
				self:Debug("contid="..ns(contid).." zoneid="..ns(zoneid).." for "..ns(zoneTr))
				if contid and zoneid and (type(self.MapNotes[zone])=="table") then
					if (TomTom:GetMapFile(contid,zoneid)) then
						for note,mapnote in pairs(self.MapNotes[zone]) do
							x,y = self:getXY(note)
							--self:Debug("x="..ns(x).." y="..ns(y))
							if x and y then
								--self:Debug(GetCurrentMapContinent().." "..ns(note).." "..ns(zone).." x"..ns(x).." y"..tostring(y))
								self.TomTomWaypoints[#self.TomTomWaypoints+1] = TomTom:AddZWaypoint(
									contid,zoneid,x*100,y*100,
									self.MapNotes[zone][note].title, --desc
									false, --persistent
									true, true, --minimap,world
									nil,true, --callbacks,silent
									(zone==self.CurrentStep.mapzone and note==self.CurrentStep.mapnote) --arrow
								)
							end
						end
					else
						self:Print("No map data for continent id "..ns(contid)..", zone id "..ns(zoneid)..", zone "..ns(zone)..", please report.")
					end
				end
			end
		end
		--]]
	elseif addon=="cart2" then
		--[[
		self:Debug("registering database "..#self.MapNotes)
		Cartographer_Notes:RegisterNotesDatabase('ZygorGuides', self.MapNotes, self)
		self:Debug("registered database")

		self:Debug("registering icons")
		if not self.iconsregistered then
			for k,v in pairs(self.icons) do
				Cartographer_Notes:RegisterIcon(k, v)
			end
		end
		--]]
	elseif addon=="internal" then
	end

	self:Print(L["waypointaddon_connected"]:format(L["opt_group_addons_"..addon]))
	self:Debug("registered icons")
	self.iconsregistered = true
	self.iconregistryretries = 0

	self:SetWaypoint()
end





-- icon handlers:

function ZGV:GetNoteScaling(zone,id,data)
	return self.db.profile.iconScale
end

function ZGV:IsNoteHidden(zone,id,data)
	return self.db.profile.filternotes and (not self.CurrentStep or not self.CurrentStep.mapnote or (id~=self.CurrentStep.mapnote) or (zone~=self.CurrentStep.mapzone))
end

function ZGV:IsMiniNoteHidden(zone,id,data)
	return not self.db.profile.minicons or (self.db.profile.filternotes and ((id~=self.CurrentStep.mapnote) or (zone~=self.CurrentStep.mapzone)))
end

function ZGV:GetNoteTransparency(zone,id,data)
	return self.db.profile.iconAlpha
end

function ZGV:GetNoteIcon(zone,id,data)
--	return (not self.db.profile.filternotes and self.CurrentStep and (id==self.CurrentStep.mapnote) and (zone==self.CurrentStep.mapzone)) and "hilite" or data.icon
	return (self.CurrentStep and (id==self.CurrentStep.mapnote) and (zone==self.CurrentStep.mapzone)) and (data.icon=="Square" and "hilitesquare" or "hilite") or data.icon
end



-------------------------- Cartographer stuff

function ZGV:ClearCartographerWaypoints()
	if Cartographer_Waypoints then
		for i,v in ipairs(Cartographer_Waypoints.Queue) do
			v:Cancel()
			Cartographer_Waypoints.Queue[i]=nil
		end
	end
	if Cartographer_Notes and Cartographer_Notes.externalDBs["ZygorGuidesViewer"] then
		Cartographer_Notes:UnregisterNotesDatabase("ZygorGuidesViewer")
	end
end

function ZGV:CreateCartographerWaypoints(goalnum)
	if not self.CurrentStep or not self.CurrentStep.goals then return end

	local x,y,zone

	local db = {version=3}

	local waypoints = {}

	-- set mapnotes for all the coordinates found in step lines
	-- REVERSE direction to create proper waypoint queue
	for i=#self.CurrentStep.goals,1,-1 do
		local g = self.CurrentStep.goals[i]
		if g.x and not g.force_noway then
			zone = g.map
			if zone then
				if self.BZR[zone] then zone = self.BZR[zone] end
				local note = Cartographer_Notes.getID(g.x,g.y)
				if not db[zone] then db[zone]={} end
				db[zone][note]={icon="Circle",title=g.title or self.CurrentStep.title or g.autotitle or self.CurrentStep:GetWayTitle() or L['waypoint_step']:format(self.CurrentStepNum)}

				if (i==goalnum) or not goalnum then
					table.insert(waypoints,{zone=zone,note=note})
				end
			end
		end
	end

	Cartographer_Notes:RegisterNotesDatabase("ZygorGuidesViewer",db)

	for i,way in ipairs(waypoints) do
		Cartographer_Waypoints:SetNoteAsWaypoint(way.zone,way.note)
	end

	Cartographer_Notes:MINIMAP_UPDATE_ZOOM()
end

function ZGV:CreateCartographerWaypointXY(x,y,title)
	local zone = select(GetCurrentMapZone(), GetMapZones(GetCurrentMapContinent())) -- likely fails in Scarlet Enclave
	Cartographer_Waypoints:AddWaypoint(NotePoint:new(zone, x, y, title or "Waypoint"))
end


function ZGV:UpdateCartographerExport()
	if ((self.db.profile.waypointaddon~="cart2") and (self.db.profile.waypointaddon~="cart3")) then return end  -- or (not self.iconsregistered)

	Cartographer_Notes:MINIMAP_UPDATE_ZOOM()
	Cartographer_Notes:UpdateMinimapIcons()
	Cartographer_Notes:RefreshMap()
end



-------------------------- TomTom stuff

do
	local lastwaypath 		-- Optimization: don't reset the path each time

	function ZGV:ClearTomTomWaypoints()
		--self:Debug("Clearing TomTom waypoints:")
		for i,p in ipairs(self.TomTomWaypoints) do
			--self:Debug(p)
			TomTom:RemoveWaypoint(p)
		end
		self.TomTomWaypoints = {}
		lastwaypath=nil
		self.Pointer:ClearWaypoints("ant") -- if anything
	end

	function ZGV:CreateTomTomWaypath(waypath)
		if waypath and #waypath.coords>1 and waypath~=lastwaypath then
			self:ClearTomTomWaypoints()
			lastwaypath=waypath
			self:Debug("New waypath available, installing the points")
			-- Table containing the next element in path for each point
			local waypathnavigation={}
			local firstuid,prevuid,currentcrazy -- WARNING if the user manually adds a point, we're DOOOMED

			for k,v in ipairs(waypath.coords) do
				-- Creating waypoint, remembering its uid
				local uid=self:CreateTomTomWaypointMFXY(v.map,v.floor or 0,v.x,v.y,"Path("..k..")",--[[TODO this should be localized perhaps]]
				false,function(event,uid)
					if uid and uid==currentcrazy then
						if waypathnavigation and waypathnavigation[uid] then
							-- Strangely enough I see no way to ask TomTom which waypoint the Crazy Arrow is looking at
							-- hence this ugly hack. We should probably conctact him, because it's plainly silly. ~aprotas
							currentcrazy=waypathnavigation[uid]
							TomTom:SetCrazyArrow(waypathnavigation[uid], -- I guess I've learned lexical closure, Obi-Wan. ~aprotas
								TomTom.profile.arrow.arrival,waypathnavigation[uid].title)
						end
					end
				end
				)

				-- Setting the linkage
				if firstuid then -- "if we're not the first waypoint"
					waypathnavigation[prevuid]=uid
				end

				-- Remembering the uid
				prevuid=uid if not firstuid then firstuid = uid end
			end
			-- Connecting the last and the first
			-- TODO what about acyclic waypaths?
			waypathnavigation[prevuid]=firstuid

			-- CopyPasted from TomTom.lua, otherwise we'd be writing it either way
			local m,f,x,y = TomTom:GetCurrentPlayerPosition()
			local closest_waypoint = nil
			local closest_dist = nil
			if self.TomTomWaypoints then
				for key, waypoint in pairs(self.TomTomWaypoints) do -- Only iterating through OUR waypoints unlike the original
					local dist, x, y = TomTom:GetDistanceToWaypoint(waypoint)
					if (dist and closest_dist == nil) or (dist and dist < closest_dist) then
						closest_dist = dist
						closest_waypoint = waypoint
					end
				end
			end
			-- Important: we can't track CrazyArrow, so let's just remember
			currentcrazy=closest_waypoint
			TomTom:SetCrazyArrow(closest_waypoint,TomTom.profile.arrow.arrival,closest_waypoint.title)
		else
-- 			ZGV:CreateTomTomWaypointMFXY( TODO
		end
	end     -- TODO unify the logic with Carbonite and Internal waypointer
end

function ZGV:CreateTomTomWaypoints(goalnum)
	lastwaypath=nil -- Just in case

	--if not Astrolabe.ContinentList[101] then Astrolabe.ContinentList[101] = {[1]="ScarletEnclave"} end
	if not self.CurrentStep or not self.CurrentStep.goals then return end

	if (TomTom.profile and TomTom.profile.persistence) then
		TomTom.profile.persistence.cleardistance = 0
	end

--	if self.CurrentStep.mapnote then

	local x,y,zone

	for i=#self.CurrentStep.goals,1,-1 do
		local goal = self.CurrentStep.goals[i]
		if goal.x and not goal.force_noway then
			if goal.map then
				-- TODO should we need to ever give floor id?
				self:CreateTomTomWaypointMFXY(
					goal.map, goal.floor or 0,
					goal.x, goal.y,
					goal.title or self.CurrentStep.title or goal.autotitle or self.CurrentStep:GetWayTitle() or "Step "..self.CurrentStepNum,
					(i==goalnum or not goalnum)
				)

				--self:Debug("added to TomTom as:"..(way or 'nil'))
				if way then table.insert(self.TomTomWaypoints, way) end
			end
		end

	end

end

function ZGV:CreateTomTomWaypointXY(x,y,title,arrow,func)
	return self:CreateTomTomWaypointMFXY(GetCurrentMapAreaID(),GetCurrentMapDungeonLevel(),x/100,y/100,title,arrow,func)
end

function ZGV:CreateTomTomWaypointMXY(m,x,y,title,arrow,func)
	return self:CreateTomTomWaypointMFXY(m,0,x,y,title,arrow,func)
end

function ZGV:CreateTomTomWaypointMFXY(m,f,x,y,title,arrow,func)
	-- Sanity, sanity everywhere
	if ZGV and ZGV.Pointer and ZGV.Pointer:IsEnvironmentPhased(m) then
		ZGV:Debug("The player and the target are in the same phased environment, putting a marker on the current map instead.")
		m,f=Astrolabe:GetCurrentPlayerPosition("last")
	end

	self:Debug("Setting TomTom waypoint: "..m..' '..f..' '..x..' '..y)
	-- Okay, if we accidently set the callback to arrivaldistance set in TomTom, he's gonna mercilessly overwrite us
	-- Let's pick some other number
	local procdistance=(max(TomTom.profile.arrow.arrival,TomTom.profile.arrow.arrival) or 15)*2 -- 30 if the option is nil for some reason

	-- This is an internal TomTom's function so it uses different syntax, table is correct here
	local way = TomTom:AddMFWaypoint(
		m, f,
		x, y,
		{
			title = title,
			persistent = false,
			minimap = true,
			world = true,
			callbacks = func and { distance = { [procdistance] = func} } or nil,
			silent = true,
			crazy = arrow,
		})

	if way then table.insert(self.TomTomWaypoints, way) end
	return way
end

--- Ah, who needs this crap anymore anyway... ~sinus 2011-08-15

--[[
function ZGV:CreateTomTomWaypointZXY(m,f,x,y,title,arrow)
	-- Can't find a mention of SpecialMapNames which are used by GetMapZoneNumbers
	-- using this stub implementation by now, should work every time the other one used to ~aprotas
end

-- TODO following implementations were mapzone-text dependent and used a contid,zoneid system
-- since TomTom natively supports mapid addressation I reworked this code to use mapid, and leave
-- these originals in case it's not intended
-- Also, it should make the last lookup functions rudimentary as well. ~aprotas

-- function ZGV:CreateTomTomWaypointXY(x,y,title,arrow)
-- 	return self:CreateTomTomWaypointZXY(GetRealZoneText(),x,y,title,arrow)
-- end
--
-- function ZGV:CreateTomTomWaypointZXY(zone,x,y,title,arrow)
-- 	local contid,zoneid = self:GetMapZoneNumbers(zone)
-- 	return self:CreateTomTomWaypointCZXY(contid,zoneid,x,y,title,arrow)
-- end
--
-- function ZGV:CreateTomTomWaypointCZXY(contid,zoneid,x,y,title,arrow)
-- 	print(contid..' '..zoneid..' '..x..' '..y)
-- 	self:Debug(x..' '..y)
-- 	local way = TomTom:AddZWaypoint(
-- 		contid, zoneid,
-- 		x, y,
-- 		title or self.CurrentStep.title or "Step "..self.CurrentStepNum,
-- 		false, --persistent
-- 		true, --minimap
-- 		true, --world
-- 		nil, --custom_callbacks
-- 		true, --silent
-- 		arrow --arrow
-- 	)
-- 	if way then table.insert(self.TomTomWaypoints, way) end
-- end

local MapZoneCache={}
-- supply ENGLISH zone names please.
function ZGV:GetMapZoneNumbers(zonename)
	--local engzone = self.BZR[zonename]
	local cached = MapZoneCache[zonename]
	if cached then return cached[1],cached[2] end

	if Astrolabe.SpecialMapNames and Astrolabe.SpecialMapNames[zonename] then
		local data=Astrolabe.SpecialMaps[Astrolabe.SpecialMapNames[zonename] ]
		MapZoneCache[zonename]=data
		return data[1],data[2]
	end
	local loczone=self.BZL[zonename]
	for cont in pairs{GetMapContinents()} do
		for zone,name in pairs{GetMapZones(cont)} do
			if name==loczone then
				MapZoneCache[zonename]={cont,zone}
				return cont,zone
			end
		end
	end
	return 0
end

-- only for TomTom support, Astrolabe bundled
local MapFileCache={}
function ZGV:GetMapZoneFile(zonename)
	local cached = MapFileCache[zonename]
	if cached then return cached[1],cached[2] end

	local data = Astrolabe.SpecialMapNames[zonename]
	if data then
		return data
	end

	local loczone=self.BZL[zonename]
	for cont in pairs{GetMapContinents()} do
		for zone,name in pairs{GetMapZones(cont)} do
			if name==loczone then
				return Astrolabe.ContinentList[cont][zone]
			end
		end
	end
	return ""
end
--]]

--EVIL STUFF. Hacking the ORIGINAL GetMapContinents(). This is bad, bad, bad - but Blizzard broke the rules by creating an off-world zone first... ;P
--[[
local continentlist = { GetMapContinents() }
table.insert(continentlist,ZygorGuidesViewer.BZL["Plaguelands: The Scarlet Enclave"])
function GetMapContinents()
	return unpack(continentlist)
end
local _GetMapZones = GetMapZones
function GetMapZones(cont)
	if cont<5 then
		return _GetMapZones(cont)
	else
		return ZygorGuidesViewer.BZL["Plaguelands: The Scarlet Enclave"]
	end
end
--]]