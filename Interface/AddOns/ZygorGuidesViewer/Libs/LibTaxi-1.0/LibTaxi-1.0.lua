--[[
Name: LibTaxi-1.0
Revision: $Rev: 1 $
Author(s): sinus (sinus@sinpi.net)
Description: A library recording all player's currently known/unknown taxi routes.
Dependencies: None
License: Free for non-commercial use, except for Zygor Guides.
]]

local MAJOR_VERSION = "LibTaxi-1.0"
local MINOR_VERSION = tonumber(("$Revision: 1 $"):match("%d+"))

local name,addon = ...
-- #AUTODOC_NAMESPACE prototype

local GAME_LOCALE = GetLocale()

do
	local LIB_MAJOR, LIB_MINOR = "LibTaxi-1.0", 1

	local Lib = LibStub:NewLibrary(LIB_MAJOR, LIB_MINOR)
	if not Lib then return end

	addon.Lib = Lib

	Lib.master = { }
	Lib.saved_tables = { }

	local Astrolabe = DongleStub("Astrolabe-ZGV")

	--Lib.TaxiNames_Local = nil
	--Lib.TaxiNames_English = nil

	-- Initialize localization. All taxis are stored as ENGLISH - lookups need pre-translation.
		if Lib.TaxiNames_Local then
			for en,lo in pairs(Lib.TaxiNames_Local) do if lo==1 then Lib.TaxiNames_Local[en]=en end end

			Lib.TaxiNames_English = {}
			for en,lo in pairs(Lib.TaxiNames_Local) do
				Lib.TaxiNames_English[lo]=en
			end

			local mt = { __index = function(t,k)  v=rawget(t,k)  if not v then print("|cffff4400Taxi |cffff8800"..k.."|r not translated to "..GetLocale()..", please report this to Zygor") return k elseif v==true then return k else return v end  end }
			setmetatable(Lib.TaxiNames_Local,mt)
			setmetatable(Lib.TaxiNames_English,mt)
		else
			local loc_stub = {}
			setmetatable(loc_stub,{ __index = function(t,k)  return k  end } )
			Lib.TaxiNames_Local = loc_stub
			Lib.TaxiNames_English = loc_stub
		end


	local minimap_exceptions = {
		["Trade District"] = "Stormwind",
		["The Great Forge"] = "Ironforge",
		["Terrace of Light"] = "Shattrath City",
		["The Stair of Destiny"] = "Hellfire Peninsula, The Dark Portal",
		["Valley of Strength"] = "Orgrimmar"
	}

	local ERR_NEWTAXIPATH=ERR_NEWTAXIPATH

	-- Add taxi to known.
	local function addTaxi(name)
		local taxi
		if type(name)=="string" then
			local eng = Lib.TaxiNames_English[name] or Lib.TaxiNames_English[name:gsub(", .*","")]
			if type(eng)=="string" then name=eng end
			--print(i..": "..TaxiNodeName(i).." = "..name)
			if not name then
				print("|cffff8888LibTaxi Error: Untranslated taxi "..name.."|r")
				return
			end
			taxi = Lib:FindTaxi(name)
		else
			taxi = name
		end

		if taxi then
			Lib.master[taxi.name]=true
			taxi.known=true
		else
			print("|cffff8888LibTaxi Error: Unknown taxi "..name.."|r")
		end
	end

	local time

	local function onEvent(this, event, arg1)
		if event == "ADDON_LOADED" and not Lib.loaded then
			Lib.loaded=true
		elseif event == "TAXIMAP_OPENED" then
			Lib:ScanTaxiMap()
			ZGV:SetWaypoint() -- Force arrow to re-check itself -- Jeremiah
		elseif event == "UI_INFO_MESSAGE" then
			if arg1==ERR_NEWTAXIPATH then
				-- discovery! cheating by zone name.
				local name=GetMinimapZoneText()
				name = Lib.TaxiNames_English[name]
				if not name then
					print("|cffff8888LibTaxi Error: Unknown zone "..GetMinimapZoneText().."|r")
					return
				end
				name = minimap_exceptions[name] or name
				Lib.master[name]=true
				ZGV.LibRover:UpdateNow("quiet") -- Try to force update of arrow ~~ Jeremiah
			end
		elseif event == "UPDATE_FACTION" then --Faction update is not needed anymore.
			Lib:MarkKnownByLevels() --Only needs to be ran once after the faction's information has been made available at startup
			--Lib.frame:UnregisterEvent("UPDATE_FACTION")
		elseif event=="PLAYER_CONTROL_LOST" then
			time=GetTime()
		elseif event=="PLAYER_CONTROL_GAINED" then
			--print("|cffffff00Total Time = |cffff0000" ..GetTime()-time.. " seconds.")
		end
	end

	Lib.frame = CreateFrame("Frame", "LibTaxiFrame")
	Lib.frame:RegisterEvent("TAXIMAP_OPENED")
	Lib.frame:RegisterEvent("UI_INFO_MESSAGE")
	Lib.frame:RegisterEvent("ADDON_LOADED")
	--Lib.frame:RegisterEvent("UPDATE_FACTION")
	--Lib.frame:RegisterEvent("PLAYER_CONTROL_LOST")
	--Lib.frame:RegisterEvent("PLAYER_CONTROL_GAINED")
	Lib.frame:SetScript("OnEvent", onEvent)

	--- Gets all the known flight paths, in current locale.
	function Lib:GetTaxis()
		local paths = {}
		for fpath,t in pairs(Lib.master) do paths[Lib.TaxiNames_Local[fpath] or fpath]=t end
		return paths
	end

	--- Gets all the known flight paths, in English.
	function Lib:GetTaxisEnglish()
		local paths = {}
		for fpath,t in pairs(Lib.master) do paths[fpath]=t end
		return paths
	end

	local initialized_continents={}
	function Lib:Startup(newsave)
		Lib.MapIDsByName = LibRover.data.MapIDsByName or ZGV.MapIDsByName

		Lib.master=newsave
		Lib:InitializeTaxis()

		setmetatable(newsave,Lib.known_by_continent_mt)
		table.insert(Lib.saved_tables,newsave)

		Lib:MarkKnownByLevels()
	end

	local function warn(message)
		local _, ret = pcall(error, message, 3)
		geterrorhandler()(ret)
	end

	-- return three-way node known status.
	-- true = known, obviously.  false = there's a marker indicating the continent is known, but the node is not.  nil = entirely unknown if known :P
	Lib.known_by_continent_mt = { __index=function(t,i)
			if rawget(t,i) then
				return true
			else
				local c = Lib.path2cont[i]
				if rawget(t,c) then
					return false
				else
					return nil
				end
			end
		end
	}

	Lib.path2cont = {}

	Lib.taxipoints = Lib.taxipoints or addon.LibTaxiData and addon.LibTaxiData.taxipoints

	Lib.flightcost = Lib.flightcost or addon.LibTaxiData and addon.LibTaxiData.flightcost

	addon.LibTaxiData = nil

	local enemyfac = "DON'T PRUNE" or (UnitFactionGroup("player")=="Alliance") and "H" or "A"-- TODO: REMOVE
	function Lib:InitializeTaxis()
		for c,cont in pairs(Lib.taxipoints) do
			for z,zone in pairs(cont) do
				z=Lib.MapIDsByName[z] or z
				if type(z)=="table" then z=z[1] end
				local n=1
				while n<=#zone do
					local node=zone[n]
					if not node then break end
					if node.faction~=enemyfac then
						Lib.path2cont[node.name] = c
						node.m = z
						node.c = Astrolabe.WorldMapSize[z].system
						n=n+1
					else
						tremove(zone,n)
					end
				end
			end
		end
	end

	local aliases={["Stormwind City"]="Stormwind", ["Theramore Isle"]="Theramore"}
	function Lib:FindTaxi(name,trim)   -- RIP manataur. Bye bye buddy.

		-- Get faction info because Andorhal has separate Horde and Alliance flight points with the same name. ~~ Jeremiah
		local faction = nil
		local factionEnglish = UnitFactionGroup("player")
		if factionEnglish == "Alliance" then
			faction = "A"
		elseif factionEnglish == "Horde" then
			faction = "H"
		end

		if trim and name then name = name:gsub(", .*","") end  -- trim zone names (in european languages, at least)
		name = aliases[name] or name
		for c,cont in pairs(Lib.taxipoints) do  for z,zone in pairs(cont) do  for n,node in ipairs(zone) do
			if (faction == nil or node.faction == faction) and ( 
				node.name==name  -- raw name, pretty rare
				or node.name==(type(name)=="string" and name:gsub(", .*",""))  -- node name with zone appended
			)
			then
				return node
			end
		end  end  end
	end

	function Lib:FindTaxiByTag(cont,tagg)
		for z,zone in pairs(Lib.taxipoints[cont]) do  for n,node in ipairs(zone) do
			if node.taxitag==tagg then  return node  end
		end  end
	end

	function Lib:ClearContinentKnowledge(cont,operator)
		for z,zone in pairs(Lib.taxipoints[cont]) do
			local zoneid = LibRover.data.MapIDsByName[z]
			if type(zoneid)=="table" then zoneid=zoneid[1] end  -- might cause trouble on phased maps :/
			zoneid=ZGV.Pointer:SanitizePhase(zoneid)

			for n,node in ipairs(zone) do
				if node.factionid~=1031
				and node.taxioperator==operator
				and node.taxioperator~="blackcat"
				then
					node.known=false
					Lib.master[node.name]=false
				end
			end

			--[[
			if LibRover.MapLevels[zoneid]<UnitLevel("player") then -- only set contin that is our level to false
				for n,node in ipairs(zone) do
					if node.factionid~=1031 then
						if node.known==true then node.known=known end
						if Lib.master[node.name]==true then Lib.master[node.name]=known end
					end
				end
			end
			--]]  -- what was that supposed to do?  mark correct level nodes as unknown? O_o  ~sinus
		end
	end

	function Lib:ScanTaxiMap()
		if not TaxiFrame:IsShown() then return end

		local cont = GetCurrentMapContinent()

		if ZGV then ZGV:Debug("LibTaxi: Scanning map...") end

		-- We now see the map. Whatever's not on the map, is surely unknown - so, mark everything as unknown and learn what's known.

		-- NASTY: Need to clear only the current operator. Try to find a node first, and its operator, only then clear the continent.
		local cont_cleared = false

		for i=1,NumTaxiNodes() do
			if TaxiNodeGetType(i)~="NONE" then
				local name = TaxiNodeName(i)
				local taxix,taxiy = TaxiNodePosition(i)
				local taxitag = ("%03d:%03d"):format(taxix*1000,taxiy*1000)
				local taxi = Lib:FindTaxiByTag(cont,taxitag)

				if taxi then
					ZGV:Debug("LibTaxi found %s [%s]",taxi.name,taxitag)

					if not cont_cleared then
						ZGV:Debug("Clearing continent %d, operator %s",cont,taxi.taxioperator or "default")
						Lib:ClearContinentKnowledge(cont,taxi.taxioperator)  -- TODO: find operator of current path!
						cont_cleared = true
					end

					taxi.known = true
					Lib.master[taxi.name]=true
				else
					ZGV:Debug("LibTaxi |cffff8888error, can't find taxi: %s [%s] [%.5f,%.5f]",name,taxitag,taxix,taxiy)
				end
			end
		end

		-- leech off QH
		if QuestHelper_KnownFlightRoutes then
			for name,_ in pairs(QuestHelper_KnownFlightRoutes) do
				addTaxi(name)
			end
		end
	end

	function Lib:MarkKnownByLevels()
		local level = UnitLevel("player")
		for c,cont in pairs(Lib.taxipoints) do
			for z,zone in pairs(cont) do
				local zoneid = LibRover.data.MapIDsByName[z]
				if type(zoneid)=="table" then zoneid=zoneid[1] end  -- might cause trouble on phased maps :/
				zoneid=ZGV.Pointer:SanitizePhase(zoneid)
				--[[if LibRover.MapLevels[zoneid]<=level  -- zone is lower level than player, we should know all taxis by now
				  and LibRover.MapLevels[zoneid]<85  -- Pandaria Zones are not learned by level
				  then
					for n,node in ipairs(zone) do
						if node.known==nil then node.known=true	end

						if  (node.quest and not ZGV.completedQuests[node.quest]) -- we didn't do the quest
							or (node.factionid and select(3,GetFactionInfoByID(node.factionid))<node.factionstanding) -- we're not esteemed enough
							or (node.condition and not node.condition()) -- condition fail
							or (node.class and select(2,UnitClass("player"))~=node.class) -- Class only! woo
						then
							node.known = false
						end

						if not node.known then Lib.master[node.name]=false end

						if Lib.master[node.name]==nil then Lib.master[node.name]=true end
					end
				else --]]
					for n,node in ipairs(zone) do
						local achieveInfo
						if node.achievemissing then
							achieveInfo = {GetAchievementInfo(node.achievemissing)}
						end

						if node.taxioperator and node.taxioperator=="blackcat" then node.known = true end --All blackcats are useable by an alliance character

						if node.available then
							node.known = node.available() --OVERWRITE. If we gave something special so don't worry about the others
							Lib.master[node.name]= node.known;
						elseif node.achievemissing then
							-- If the player has the achievement, then the node is missing.
							node.missing = achieveInfo[13] -- 13 = whether this toon has the achievement.
						elseif Lib.master[node.name]==false then --if zone is overlevel and for some reason it is false, set it back to nil
							Lib.master[node.name]=nil
						elseif Lib.master[node.name]==true then -- we know a flightpath that is over our level
							node.known=true
						end
					end
				--end
			end
		end
	end

	function Lib:ResetKnowledge()
		for c,cont in pairs(Lib.taxipoints) do
			for z,zone in pairs(cont) do
				for n,node in ipairs(zone) do
					node.known = nil
				end
			end
		end
		Lib:MarkKnownByLevels()
	end


	-- And now, the EVIL. Let's peek into a taxi before it flies.
	-- LibTaxi.LastTaxi becomes the node of the last taxi taken!

	local Saved_TakeTaxiNode = TakeTaxiNode
	_G.TakeTaxiNode = function(index)
		local x,y = TaxiNodePosition(index)
		Lib.LastTaxi = {fullname=TaxiNodeName(index)}
		Lib.LastTaxi.node = Lib:FindTaxi(Lib.LastTaxi.fullname)
		if Lib.LastTaxi.node then
			Lib.LastTaxi.name,Lib.LastTaxi.zone = Lib.LastTaxi.node.name,GetMapNameByID(Lib.LastTaxi.node.m)
--			Lib.LastTaxi.name,Lib.LastTaxi.zone = Lib.LastTaxi.node.name,(Lib.LastTaxi.node.m and GetMapNameByID(Lib.LastTaxi.node.m) or ("(bad map in '"..Lib.LastTaxi.name.."')")) -- just in case? or was it a bad update that broke in here?
		else
			Lib.LastTaxi.name,Lib.LastTaxi.zone = Lib.LastTaxi.fullname:match("(.*), (.*)")
		end
		Saved_TakeTaxiNode(index,"",true)
	end

	local function __genOrderedIndex( t )
	    local orderedIndex = {}
	    for key in pairs(t) do
		table.insert( orderedIndex, key )
	    end
	    table.sort( orderedIndex )
	    return orderedIndex
	end

	local function orderedNext(t, state)
	    -- Equivalent of the next function, but returns the keys in the alphabetic
	    -- order. We use a temporary ordered key table that is stored in the
	    -- table being iterated.

	    --print("orderedNext: state = "..tostring(state) )
	    if state == nil then
		-- the first time, generate the index
		t.__orderedIndex = __genOrderedIndex( t )
		key = t.__orderedIndex[1]
		return key, t[key]
	    end
	    -- fetch the next value
	    key = nil
	    for i = 1,table.getn(t.__orderedIndex) do
		if t.__orderedIndex[i] == state then
		    key = t.__orderedIndex[i+1]
		end
	    end

	    if key then
		return key, t[key]
	    end

	    -- no more value to return, cleanup
	    t.__orderedIndex = nil
	    return
	end

	local function ordered_pairs(t)
	    -- Equivalent of the pairs() function on tables. Allows to iterate
	    -- in order
	    return orderedNext, t, nil
	end

	function Lib:DumpTaxiPoints()
		local s="	Lib.taxipoints = {\n"
		for contnum,contdata in ipairs(Lib.taxipoints) do
			s=s.."		["..contnum.."]={\n"
			for zonename,zonedata in ordered_pairs(contdata) do
				s=s.."			['"..zonename:gsub("'","\\'").."']={\n"
				for ti,taxi in ipairs(zonedata) do
					local taxicosts=""
					if taxi.costs then
						for tag,cost in pairs(taxi.costs) do
							taxicosts = taxicosts .. " [\"" ..tag.."\"] = " ..cost..","
						end
						if #taxicosts>0 then taxicosts=taxicosts:sub(1,-2) end
					end
					local extra=""
					if taxi.class then extra=extra.."class=\""..taxi.class.."\"," end
					if taxi.quest then extra=extra.."quest="..taxi.quest.."," end
					if taxi.factionid then extra=extra.."factionid="..taxi.factionid.."," end
					local operator=""
					if taxi.taxioperator then operator=operator.."taxioperator=\"".. taxi.taxioperator .."\"," end
					local taxitag=""
					if taxi.taxitag then taxitag="taxitag="..taxi.taxitag.."," end
					s=s..('				{name="%s",faction="%s",%snpc="%s",npcid=%d,x=%.1f,y=%.1f,%s%scosts={%s}},\n'):format(taxi.name,taxi.faction,extra,taxi.npc,taxi.npcid,(taxi.x<1 and taxi.x*100 or taxi.x),(taxi.y<1 and taxi.y*100 or taxi.y),operator,taxitag,taxicosts)
				end
				s=s.."			},\n"
			end
			s=s.."		},\n"
		end
		s=s.."	}\n"
		ZGV:ShowDump(s)
	end

	function Lib:MergeTags()
		for contnum,contdata in ipairs(LibRover.data.flightcost) do
			for tag,taxidata in pairs(contdata) do
				local taxinode = Lib:FindTaxi(taxidata.Name, "trim")
				if not taxinode then
					print("WHOA. Taxi node not found: ",taxidata.Name)
				else
					taxinode.taxitag = tag
					taxinode.pre_costs = taxidata.Neighbors
				end
			end
		end
	end

	function Lib:ImportFM()
		local floc = Lib.flightcost
		Lib.taxitagsFaction = {}
		local unitFaction=UnitFactionGroup("player")
		unitFaction=unitFaction:match("%u")
		if not floc then print("Need flightcosts data") return end

		for c,cont in pairs(Lib.taxipoints) do
			for z,zone in pairs(cont) do
				for ni,node in pairs(zone) do
					if not node then break end

					local found=0
					if floc[c] then
						for fmid,fmdata in pairs(floc[c]) do
							if fmdata.name:find(node.name,0,true) then
								if not ( (fmdata.faction and fmdata.faction~=node.faction) or (fmdata.taxioperator~=node.taxioperator) ) then
									found=found+1
									node.taxitag = fmid
									Lib.taxitagsFaction[fmid] = node.faction
									--print("|cff00ff00",node.name,node.faction,"|r = |cffff8800",node.taxitag)
									break
								else
									--print("|cffff8855",node.name,node.faction,"(",z,") [",node.taxioperator,"] found:",fmid,"but wrong",fmdata.faction,fmdata.taxioperator)
								end
							end
						end
						if found==0 then print("|cffffff00",node.name,node.faction,"(",z,") [",node.taxioperator,"] not found") end
					else
						print("|cff000000 cont",c,"not found")
					end
				end
			end

			for z,zone in pairs(cont) do
				for ni,node in pairs(zone) do while 1 do
					if not node or not node.taxitag then print(node.name,"(",z,") [",node.faction,"] is missing a tag") break end

					for title,data in pairs(floc[c][node.taxitag]) do
						if title=="neighbors" then
							node.costs = {}
							for neighborid, cost in pairs(data) do
								if node.faction == Lib.taxitagsFaction[neighborid] or Lib.taxitagsFaction[neighborid]=="B" then
									node.costs[neighborid] = cost
								elseif node.faction=="B" and unitFaction==Lib.taxitagsFaction[neighborid] or node.faction=="B" and Lib.taxitagsFaction[neighborid]=="B" then
									node.costs[neighborid] = cost
								end
							end
						end
					end
				break end end
			end
		end
	end
	Lib:ImportFM()

	function Lib:DumpNeighborFlights()

		local s = ""

		for i=1,NumTaxiNodes() do
			x,y=TaxiNodePosition(i)
			x,y=floor(x*1000),floor(y*1000)
			if x < 100 then x = "0"..x end
			if y < 100 then y = "0"..y end
			name=TaxiNodeName(i)

			if GetNumRoutes(i)==1 then
				s= s..("		[\""..x..":"..y.."\"]".." = time, --"..name.."\n")
			elseif GetNumRoutes(i)==0 then
				s=("--[\""..x..":"..y.."\"]".." = time, --"..name.."\n".."[\""..x..":"..y.."\"]".." = {\n	[\"name\"] = \""..name.."\",\n	[\"neighbors\"] = {\n")..s
			end
		end
		s=s.."	},\n},"
		ZGV:ShowDump(s)
	end

	function Lib:DumpAllFlights()

		local s = ""

		for i=1,NumTaxiNodes() do
			x,y=TaxiNodePosition(i)
			x,y=floor(x*1000),floor(y*1000)
			if x < 100 then x = "0"..x end
			if y < 100 then y = "0"..x end
			name=TaxiNodeName(i)

			s=s..("--[\""..x..":"..y.."\"]".." = time, --"..name.."\n".."[\""..x..":"..y.."\"]".." = {\n	[\"name\"] = \""..name.."\",\n	[\"neighbors\"] = {\n").."},\n},\n"
		end
		ZGV:ShowDump(s)
	end

	function Lib:TestAllFlights()
		local s = ""

		for i=1,NumTaxiNodes() do
			local x,y=TaxiNodePosition(i)
			x,y=floor(x*1000),floor(y*1000)
			if x < 100 then x = "0"..x end
			if y < 100 then y = "0"..y end
			local name=TaxiNodeName(i)
			local ttag = x..":"..y

			local BZ = LibStub("LibBabble-SubZone-3.0")
			local BZL,BZR = BZ:GetUnstrictLookupTable(),BZ:GetReverseLookupTable()

			local tagmatch = Lib:FindTaxiByTag(GetCurrentMapContinent(),ttag)

			if not tagmatch then
				s = s.."\n"..ttag.." - No tag match - "..i
			end

		end
		ZGV:ShowDump(s)
	end



	--[[
	function Lib:ConvertTag(typ,a,b,c)
		if typ=="raw" then
			return tonumber(("%d:%0d:%0d"):format(a,math.ceil(b*100),math.ceil(c*100)))
		elseif typ=="fme" then
			return a
		end
	end
	--]]

	--[[
	function Lib:TestCurrent()
		for node=1,NumTaxiNodes() do
			local typ=TaxiNodeGetType(node)
			if typ=="CURRENT" then
				local x,y = TaxiNodePosition(node)
				x=Math.floor(
		end
	end
	--]]

	_G['LibTaxi']=Lib
end