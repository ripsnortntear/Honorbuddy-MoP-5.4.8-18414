local _G = _G
local AddonLoader = AddonLoader


-- GLOBALS: INTERFACEOPTIONS_ADDONCATEGORIES InterfaceOptionsFrame_OpenToCategory InterfaceOptions_AddCategory
-- GLOBALS: IsInInstance InCombatLockdown GetNumRaidMembers GetNumPartyMembers
-- GLOBALS: GetRealmName IsInGuild UnitIsPVP UnitClass IsResting UnitLevel
-- GLOBALS: GetAddOnDependencies
-- GLOBALS: CreateFrame hooksecurefunc geterrorhandler LibStub UIParent setfenv
-- GLOBALS: IsAddOnLoaded

local tonumber, string, type, next, select, ipairs, tremove, tostring, pcall, loadstring = 
      tonumber, string, type, next, select, ipairs, tremove, tostring, pcall, loadstring

local function returnTrue() return true end

-- There should be a cleaner way to handle this
local hookenv = setmetatable({}, {__index = getfenv(0)})
function hookenv.LoadAddOn(name)
	return AddonLoader:LoadAddOn(name)
end

local delayFrame -- will be filled if needed with a frame for loading addons delayed.
local BZ -- will be a reference to babble-zone-3.0 if needed

function AddonLoader:RemoveInterfaceOptions(name)
	for k, f in ipairs(INTERFACEOPTIONS_ADDONCATEGORIES) do
		if f == name or f.name == name then
			tremove(INTERFACEOPTIONS_ADDONCATEGORIES, k)
			break
		end
	end
end

local function optionsOnShow(self)
	if not IsAddOnLoaded(self.addon) then
		-- remove from options frame
		AddonLoader:RemoveInterfaceOptions(self)
		self:Hide()
		-- load addon
		AddonLoader:LoadAddOn(self.addon)
		-- refresh optionsframe
		InterfaceOptionsFrame_OpenToCategory(self.name)
	end
end

AddonLoader.conditions = {
	["X-LoadOn-Mailbox"] = {
		events = {"MAIL_SHOW"},
		handler = returnTrue
	},
	["X-LoadOn-AuctionHouse"] = {
		events = {"AUCTION_HOUSE_SHOW"},
		handler = returnTrue
	},
	["X-LoadOn-Bank"] = {
		events = {"BANKFRAME_OPENED"},
		handler = returnTrue,
	},
	["X-LoadOn-Arena"] = {
		events = {"ZONE_CHANGED_NEW_AREA", "PLAYER_ENTERING_WORLD"},
		handler = function() return select(2, IsInInstance()) == "arena" end,
	},
	["X-LoadOn-Battleground"] = {
		events = {"ZONE_CHANGED_NEW_AREA", "PLAYER_ENTERING_WORLD"},
		handler = function() return select(2, IsInInstance()) == "pvp" end,
	},
	["X-LoadOn-Reforge"] = {
		events = {"FORGE_MASTER_OPENED"},
		handler = returnTrue,
	},
	["X-LoadOn-Instance"] = {
		events = {"ZONE_CHANGED_NEW_AREA", "PLAYER_ENTERING_WORLD"},
		handler = function()
			local instanceType = select(2, IsInInstance())
			return instanceType == "party" or instanceType == "raid"
		end,
	},
	["X-LoadOn-Combat"] = {
		events = {"PLAYER_REGEN_DISABLED", "PLAYER_ENTERING_WORLD"},
		handler = function( event, name, arg )
			if event == "PLAYER_REGEN_DISABLED" then return true end
			if event == "PLAYER_ENTERING_WORLD" then return InCombatLockdown() end
		end,
	},
	["X-LoadOn-Crafting"] = {
		events = {"TRADE_SKILL_SHOW", "CRAFT_SHOW"},
		handler = returnTrue,
	},
	["X-LoadOn-Group"] = {
		events = {"GROUP_ROSTER_UPDATE", "PLAYER_ENTERING_WORLD"},
		handler = function() return GetNumGroupMembers() > 0 or GetNumSubgroupMembers() > 0 end,
	},
	["X-LoadOn-Merchant"] = {
		events = {"MERCHANT_SHOW"},
		handler = returnTrue,
	},
	["X-LoadOn-PvPFlagged"] = {
		events = {"UNIT_FACTION", "PLAYER_ENTERING_WORLD"},
		handler = function() return UnitIsPVP("player") end,
	},
	["X-LoadOn-Raid"] = {
		events = {"GROUP_ROSTER_UPDATE", "PLAYER_ENTERING_WORLD"},
		handler = function() return GetNumGroupMembers() > 0 and IsInRaid() end,
	},
	["X-LoadOn-Class"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			for class in arg:gmatch("[^ ,]+") do
				if tostring(class):upper() == select(2, UnitClass("player")) then
					return true
				end
			end
		end,
	},
	["X-LoadOn-Realm"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg) return GetRealmName() == arg end,
	}, 
	["X-LoadOn-Guild"] = {
		events = {"PLAYER_LOGIN", "GUILD_ROSTER_UPDATE", "GUILD_XP_UPDATE"},
		handler = function() return IsInGuild() end,
	},
	["X-LoadOn-Always"] = {
		events = {"PLAYER_LOGIN"},
		handler = function( event, name, arg )
			if not (arg or ""):lower():match("^delayed") then return true end
			-- delayed loading, 0.15s between addons
			if not delayFrame then
				delayFrame = CreateFrame("Frame")
				delayFrame.addons = {}
				delayFrame.elapsed = -1
				
				local function loadOne(...)	-- Load an addon, or a dependency of it. One at a time. Recursively. Return true if something was loaded (i.e. we should wait before loading one more)
					for i=2,select("#",...) do
						local addon = select(i,...)
						if IsAddOnLoaded(addon) then	
							-- slight optimization, not strictly needed
						elseif loadOne(addon, GetAddOnDependencies(addon)) then
							return true -- we loaded something!
						end
					end
					local addon = (...)
					delayFrame.addons[addon] = nil -- nuke from the list (might not have been there but thats fine)
					if not IsAddOnLoaded(addon) then
						AddonLoader:LoadAddOn(addon)
						return not not IsAddOnLoaded(addon)   -- we loaded something! (or not, in which case we just keep looking for something else to load)
					end
				end
				
				local InCombatLockdown = InCombatLockdown
				
				delayFrame:SetScript("OnUpdate", function(self, elapsed)
					if InCombatLockdown() then
						return	-- 5.2 hates using too much CPU during combat
					end
					if self.elapsed < 0 then
						self.elapsed = 0	-- We do this to start counting the time from AFTER the previous addon loaded. Not from when it started loading.
					else
						self.elapsed = self.elapsed + elapsed
						if self.elapsed >= 0.15 then
							self.elapsed = -1

							while next(self.addons) do
								local addon = next(self.addons)
								if loadOne(addon, GetAddOnDependencies(addon)) then
									break
								end
							end
							if not next(self.addons) then
								self:SetScript("OnUpdate", nil)
								self.addons=nil
								self:Hide()
							end
						end
					end
				end)
			end
			delayFrame.addons[name] = true
			delayFrame:Show()
		end,
	},
	["X-LoadOn-Resting"] = {
		events = {"PLAYER_UPDATE_RESTING", "PLAYER_ENTERING_WORLD"},
		handler = function() return IsResting() end,
	},
	["X-LoadOn-NotResting"] = {
		events = {"PLAYER_UPDATE_RESTING", "PLAYER_ENTERING_WORLD"},
		handler = function() return not IsResting() end,
	},
	["X-LoadOn-Level"] = {
		events = {"PLAYER_LEVEL_UP", "PLAYER_ENTERING_WORLD"},
		handler = function(event, name, arg)
			local level = UnitLevel("player")
			for chunk in arg:gmatch('([%d%p^,]+)') do
				if tonumber(chunk) then -- '68'
					if level == tonumber(chunk) then return true end
				elseif chunk:match('%+') then -- '40+'
					if level >= tonumber(chunk:match('%d+')) then return true end
				elseif chunk:match('%-$') then -- '30-'
					if level <= tonumber(chunk:match('%d+')) then return true end
				elseif chunk:match('%d+%-%d+') then -- '20-47'
					local low, high = tonumber(chunk:match('(%d+)%-(%d+)'))
					if level >= low and level <= high then return true end
				end
			end
		end,
	},
	["X-LoadOn-Events"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			for metaevent in arg:gmatch("[^ ,]+") do
				local meta
				local lookfor = "X-LoadOn-"..metaevent
				local conditiontext = AddonLoader.conditiontexts[name]
				for line in conditiontext:gmatch("[^\n]+") do
					local condname, text = string.match(line, "^([^:]*): (.*)$")
					if condname and text and condname == lookfor then
						meta = text
					end
				end
				if meta then
					local status, func, err = pcall(loadstring, meta)
					if func then
						setfenv(func, hookenv)
						if not AddonLoader.events[metaevent] then
							AddonLoader.events[metaevent] = {}
							AddonLoader.frame:RegisterEvent(metaevent)
						end
						if not AddonLoader.events[metaevent][name] then
							AddonLoader.events[metaevent][name] = {}
						end
						AddonLoader.events[metaevent][name][name..metaevent] = { -- name..metaevent to fake a unique condition name
							handler = func,
							arg = "",
						}
					else
						geterrorhandler()('## X-LoadOn-'..event..' ('..name..'): '..err)
					end
				end
			end
			-- We specifically DO NOT return true here, this handler just sets up the other conditions. And will remain dorment for the remainder
		end,
	},
	["X-LoadOn-Hooks"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			for hook in arg:gmatch("[^ ,]+") do
				if type(_G[hook]) == "function" then
					local meta
					local lookfor = "X-LoadOn-"..hook
					local conditiontext = AddonLoader.conditiontexts[name]
					for line in conditiontext:gmatch("[^\n]+") do
						local condname, text = string.match(line, "^([^:]*): (.*)$")
						if condname and text and condname == lookfor then
							meta = text
						end
					end
					if meta then
						local status, func, err = pcall(loadstring, meta)
						if func then
							hooksecurefunc( hook, func )
						else
							geterrorhandler()('## X-LoadOn-'..hook..' ('..name..'): '..err)
						end
					end
				else
					geterrorhandler()('## X-LoadOn-Hooks: '..arg..' ('..hook..'): not a function')
				end
			end
			-- We specifically DO NOT return true here, this handler just sets up the other conditions. And will remain dorment for the remainder
		end,
	},
	["X-LoadOn-Slash"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			local name_upper = name:upper():gsub('[^%w]','')
			local slashes = AddonLoader.slashes[name:upper()] or {}
			AddonLoader.slashes[name:upper()] = slashes
			
			for slash in arg:gmatch('([^, ]+)') do
				if slash:sub(1,1) ~= '/' then
					slash = '/'..slash
				end
				-- below could be slightly optimized but my scoping skills fail me today, it works though :p
				_G['SLASH_'..string.sub(slash:upper(),2)..'1'] = slash
				slashes[string.sub(slash:upper(), 2)] = true
				_G.SlashCmdList[string.sub(slash:upper(),2)] = function(text)
					local new = _G['SLASH_'..string.sub(slash:upper(),2)..'1']
					AddonLoader:LoadAddOn(name)
					_G.ChatFrame_OpenChat("")
					local editbox = _G.ChatFrameEditBox
					if not editbox then
						-- Support for 3.3.5 and newer
						editbox = _G.ChatEdit_GetActiveWindow()
					end
					editbox:SetText(new..' '..text)
					_G.ChatEdit_SendText(editbox,1)
				end
			end
			-- We specifically DO NOT return true here, this handler just sets up the other conditions. And will remain dorment for the remainder
		end,
	},
	["X-LoadOn-LDB-Launcher"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			local texture, brokername = string.split(" ", arg)
			brokername = brokername or name

			local OnClick, dataobj, OnTooltipShow
			OnClick = function(...)
				AddonLoader:LoadAddOn(name)
				if OnClick ~= dataobj.OnClick then dataobj.OnClick(...) end
			end
			OnTooltipShow = function(tt)
				tt:AddLine(brokername)
				tt:AddLine(AddonLoader.L.clicktoload, 0.2, 1, 0.2, 1)
			end
			dataobj = LibStub:GetLibrary("LibDataBroker-1.1"):NewDataObject(brokername, {type = "launcher", tocname = name, icon = texture, OnClick = OnClick, OnTooltipShow = OnTooltipShow})
			
			-- We specifically DO NOT return true here, this handler just sets up the other conditions. And will remain dorment for the remainder
		end,
	},
	["X-LoadOn-Zone"] = {
		events = {"ZONE_CHANGED_NEW_AREA", "PLAYER_ENTERING_WORLD", "ZONE_CHANGED", "ZONE_CHANGED_INDOORS", "MINIMAP_ZONE_CHANGED"},
		handler = function(event, name, arg)
			if not BZ then
				BZ = LibStub and LibStub("LibBabble-Zone-3.0", true) -- silent check for BZ
			end
			local subzone = string.trim(GetSubZoneText()) -- yeah really...
			local realzone = GetRealZoneText()
			for zone in arg:gmatch('(%w[^,]+%w)') do
				if (BZ and BZ[zone] and (realzone == BZ[zone] or subzone == BZ[zone])) or 
					realzone == zone or subzone == zone then
					return true
				end
			end
		end,
	},
	["X-LoadOn-Execute"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			for i = 2, 5 do
				local lookfor =  "X-LoadOn-Execute"..i
				local md
				local conditiontext = AddonLoader.conditiontexts[name]
				for line in conditiontext:gmatch("[^\n]+") do
					local condname, text = string.match(line, "^([^:]*): (.*)$")
					if condname and text and condname == lookfor then
						md = text
					end
				end
				if md then
					arg = arg..' '..md
				else
					break
				end
			end
			local status, func, err = pcall(loadstring, arg)
			if func then
				func()
			else
				geterrorhandler()('## X-LoadOn-Execute '..name..': '..err)
			end
		end,
	},
	["X-LoadOn-InterfaceOptions"] = {
		events = {"PLAYER_LOGIN"},
		handler = function(event, name, arg)
			local frame = CreateFrame("Frame", nil, UIParent)
			frame.name = arg
			frame.addon = name
			frame:Hide()
			frame:SetScript("OnShow", optionsOnShow)
			InterfaceOptions_AddCategory(frame)
			-- we do not return true here, the optionsOnShow function will actually load the addon
		end,
	}
}
