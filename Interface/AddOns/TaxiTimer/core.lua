local addonName, ns = ...

local FALLBACK_FLIGHT_SPEED = 30 + (1 / 3)

local NONE = 0
local ALLIANCE = 1
local HORDE = 2
local NEUTRAL = 3

function ns:GetPlayerFaction()
	local faction = UnitFactionGroup("player")
	if faction == "Alliance" then
		return ALLIANCE
	elseif faction == "Horde" then
		return HORDE
	end
	return NEUTRAL
end

function ns:IsUsableFaction(faction)
	return faction == ns:GetPlayerFaction() or faction == NEUTRAL or faction == NONE
end

function ns:Localize()
	if ns.L then
		local data
		for english, localized in pairs(ns.L) do
			if english ~= localized then
				data = ns.TaxiHashMap[english]
				if data then
					ns.TaxiHashMap[localized] = data
					ns.TaxiHashMap[english] = nil
				end
			end
		end
	end
	ns.Localize = function() end
end

function ns:CleanUp()
	local valid = {}
	for name, factions in pairs(ns.TaxiHashMap) do
		for faction, id in pairs(factions) do
			if not ns:IsUsableFaction(faction) then
				factions[faction] = nil
			end
		end
		local count, _ = 0
		for faction, id in pairs(factions) do
			valid[id] = faction
			count = count + 1
			if count > 1 then
				print(ns.L.DEBUG_TEXT .. " {", name, faction, id, count, "}") -- DEBUG: this is so we know that our next() call below is making a mistake and is ignoring other nodes
			end
		end
		if count == 0 then
			ns.TaxiHashMap[name] = nil
		else
			_, ns.TaxiHashMap[name] = next(factions)
		end
	end
	for startId, destinations in pairs(ns.Taxi) do
		if valid[startId] then
			for endId, flight in pairs(destinations) do
				if not ns:IsUsableFaction(flight.faction) then
					destinations[endId] = nil
				end
			end
		else
			ns.Taxi[startId] = nil
		end
	end
	table.wipe(valid)
	valid = nil
	ns.CleanUp = function() end
end

function ns:GetID(name)
	return ns.TaxiHashMap[name]
end

function ns:FindPath(fromId, toId)
	local unchecked, distances, connections = {}, {}, {}

	local connected = {[fromId] = true, fromId}
	while #connected > 0 do
		local id = table.remove(connected)
		if type(id) == "number" and unchecked[id] ~= true then
			unchecked[id], distances[id] = true, math.huge
			if ns.Taxi[id] and ns.valid[id] then
				for destination, _ in pairs(ns.Taxi[id]) do
					if not connected[destination] then
						connected[destination] = true
						table.insert(connected, destination)
					end
				end
			end
		end
	end

	distances[fromId] = 0
	while next(unchecked) do
		local closest;
		for id in pairs(unchecked) do
			if not closest or distances[id] < distances[closest] then
				closest = id
			end
		end
		unchecked[closest] = nil
		if ns.Taxi[closest] then
			for destination, flight in pairs(ns.Taxi[closest]) do
				if ns.valid[destination] then
					local distance = distances[closest] + flight.distance
					if not distances[destination] or distances[destination] > distance then
						distances[destination], connections[destination] = distance, {from = closest, to = destination, flight = flight}
					end
				end
			end
		end
	end

	local waypoints = {}

	for destination, flight in pairs(ns.Taxi[fromId]) do
		if destination == toId then
			if connections[toId] ~= toId then
				table.insert(waypoints, 1, {from = fromId, to = toId, flight = flight})
			end
			break
		end
	end

	if #waypoints == 0 then
		local node = connections[toId]
		while node do
			table.insert(waypoints, 1, node)
			node = connections[node.from]
		end
	end

	if #waypoints > 0 then
		return waypoints
	end
end

function ns:GetFlight(from, to)
	local fromId, toId = ns:GetID(from), ns:GetID(to)

	if fromId and toId then
		local waypoints = ns:FindPath(fromId, toId)

		if waypoints then
			local distance, cost = 0, 0
			for _, waypoint in ipairs(waypoints) do
				distance, cost = distance + waypoint.flight.distance, cost + waypoint.flight.cost
			end
			return waypoints, fromId, toId, distance, cost
		end
	end
end

function ns:StartStopwatch(seconds)
	if not StopwatchFrame:IsShown() then
		Stopwatch_Toggle()
	end
	if Stopwatch_IsPlaying() then
		Stopwatch_Clear()
	end
	Stopwatch_StartCountdown(0, 0, seconds)
	Stopwatch_Play()
end

function ns:Init()
	ns.hooked = {}
	ns.idToName = {}
	ns.valid = {}

	function ns:OnEnter()
		local buttonIndex = self:GetID()
		if buttonIndex and ns.fromNode then
			local buttonName = TaxiNodeName(buttonIndex)
			if buttonName and TaxiNodeGetType(buttonIndex) == "REACHABLE" then
				ns:ShowTooltip(ns.fromNode, buttonName)
			end
		end
	end

	function ns:ShowTimer(startName, endName, speed)
		local waypoints, startId, endId, distance, cost = ns:GetFlight(startName, endName)
		if waypoints then
			local seconds = distance / speed
			seconds = math.floor(seconds + .5)
			ns:StartStopwatch(seconds)
			DEFAULT_CHAT_FRAME:AddMessage(format(ns.L.CHAT_TEXT, GetTimeStringFromSeconds(seconds, false, true), startName, endName, GetMoneyString(cost)), 1, 1, 0)
		end
	end

	function ns:ShowTooltip(startName, endName)
		local waypoints, startId, endId, distance, cost = ns:GetFlight(startName, endName)
		if waypoints then
			local seconds = distance / (ns.currentSpeed or (FALLBACK_FLIGHT_SPEED * (IsSpellKnown(117983) and 1.25 or 1))) -- Guild Perk: Ride Like the Wind (25% flight speed boost)
			seconds = math.floor(seconds + .5)
			GameTooltip:AddLine("\n|cffFFFFFF" .. GetTimeStringFromSeconds(seconds, false, true) .. " " .. ns.L.FLIGHT_TIME .. "|r")
			if #waypoints > 1 then GameTooltip:AddLine(" ") local waypoint for index = 1, #waypoints - 1 do waypoint = waypoints[index] GameTooltip:AddLine("|cffFFFFFF" .. index .. ". " .. (ns.idToName[waypoint.to] or "#" .. waypoint.to) .. "|r") end end -- DEBUG: show list of visited flight paths
			GameTooltip:Show()
		end
	end

	ns.frame = CreateFrame("Frame")
	ns.frame:SetScript("OnEvent", function(f, event, ...) f[event](f, event, ...) end)

	function ns.frame:OnUpdate(elapsed)
		ns.elapsed = ns.elapsed + elapsed
		if ns.elapsed > .2 then
			if UnitOnTaxi("player") then
				ns.sumElapsed = ns.sumElapsed + ns.elapsed
				ns.currentSpeed = GetUnitSpeed("player")
				if ns.lastSpeed > 0 and ns.lastSpeed == ns.currentSpeed then
					ns.frame:SetScript("OnUpdate", nil)
					return ns:ShowTimer(ns.fromNode, ns.toNode, ns.currentSpeed)
				end
				if ns.currentSpeed and ns.currentSpeed > ns.lastSpeed then
					ns.lastSpeed = ns.currentSpeed
				end
			end
			ns.elapsed = 0
			if ns.sumElapsed > 5 or (ns.closedUI and GetTime() - ns.closedUI > 5) then
				ns.frame:SetScript("OnUpdate", nil)
			end
		end
	end

	function ns.frame:PLAYER_LOGIN(event)
		ns.frame:UnregisterEvent(event)

		ns:Localize()
		ns:CleanUp()
		for name, id in pairs(ns.TaxiHashMap) do
			ns.idToName[id] = name
		end

		ns.frame:RegisterEvent("TAXIMAP_OPENED")
		ns.frame:RegisterEvent("TAXIMAP_CLOSED")

		hooksecurefunc("TakeTaxiNode", function(i)
			ns.toNode = TaxiNodeName(i)
		end)
	end

	function ns.frame:TAXIMAP_OPENED()
		ns.hasUI = 1
		ns.closedUI = nil
		ns.fromNode, ns.toNode = nil
		for i = 1, NumTaxiNodes(), 1 do
			local name = TaxiNodeName(i)
			if TaxiNodeGetType(i) == "CURRENT" then
				ns.fromNode = name
			end
			if name then
				ns.valid[name] = 1
				local id = ns:GetID(name)
				if id then
					ns.valid[id] = 1
				end
			end
		end
		ns.elapsed, ns.sumElapsed, ns.lastSpeed = 0, 0, 0
		ns.frame:SetScript("OnUpdate", ns.frame.OnUpdate)
		local button
		for i = 1, NumTaxiNodes(), 1 do
			button = _G["TaxiButton" .. i]
			if button and not ns.hooked[button] then
				ns.hooked[button] = 1
				button:HookScript("OnEnter", ns.OnEnter)
			end
		end
	end

	function ns.frame:TAXIMAP_CLOSED()
		ns.hasUI = nil
		ns.closedUI = GetTime()
	end

	ns.frame:RegisterEvent("PLAYER_LOGIN")
	ns.Init = function() end
end

ns:Init()
