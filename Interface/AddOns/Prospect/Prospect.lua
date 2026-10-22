local aName, overlayButtons = ... --- save this for later

if overlayButtons.loaded then
	return
end

local frameNames = {
	badNames =	{
		WatchFrame = 1, -- default WatchFrame item button.
		ProspectBar = 1, -- sister addon, ProspectBar, button.
		AutoBar = 1, -- AutoBar button.
		TradesBar = 1, -- TradesBar button.
		ImprovedMerchant = 1, -- ImprovedMerchant frame.
		MerchantFrame = 1, -- default MerchantFrame.
		BankFrame = 1, -- default BankFrame.
		GuildBankFrame = 1, -- default GuildBankFrame.
	},
	badPatterns = {
		-- BaudBag's frame naming is a little unconventional, but this pattern works fine for bank stuff.
		BaudBag = {
			pattern = "BaudBagSubBag([-]-%d+)Item", -- finds BaudBagSubBag-#Item# to BaudBagSubBag##Item##. Can be named anything as long as it's referenced via "self.anything" in "func".
			func = function(self, fname) -- process the pattern. Must be named "func" and take (self, frameName) as parameters.
				if fname then
					local num = tonumber(fname:match(self.pattern))
					if num and (num == -1 or num >= 5) then
						return true
					end
				end
			end,
		},
		-- Default container frames.
		ContainerFrame = {
			pattern = "ContainerFrame(%d+)Item",
			func = function(self, fname)
				if fname then
					local num = tonumber(fname:match(self.pattern))
					if num then
						local f = _G["ContainerFrame" .. num]
						if f and f:GetID() > NUM_BAG_FRAMES then
							return true
						end
					end
				end
			end,
		},
		-- TradeSkillMaster
		TSM = {
			pattern = "TSMMainFrame(%d+)",
			func = function(self, fname)
				if fname then
					local num = tonumber(fname:match(self.pattern))
					if num then
						local f = _G["TSMMainFrame" .. num]
						if f then
							return true
						end
					end
				end
			end,
		}
	},
	goodNames = {
		CombuctorFrame1 = 1, -- Combuctor
		BagnonFrameinventory = 1, -- Bagnon
	},
	goodPatterns = {
		-- Not yet
	},
}

-- recursively go up the frame's family tree to determin whether to not make/add the overlay button.
local function BadFrame(s)
	while s do
		if s.mode == "bank" or frameNames.badNames[tostring(s:GetName())] or s.GetInventorySlot or s.price then
			return true
		else
			for _, patterns in pairs(frameNames.badPatterns) do
				if patterns:func(s:GetName()) then
					return true
				end
			end
		end
		s = s:GetParent() -- recursion :)
	end
end

-- recursively go up the frame's family tree to find frames that will be allowed.
local function GoodFrame(s)
	while s do
		if frameNames.goodNames[tostring(s:GetName())] then
			return true
		else
			for _, patterns in pairs(frameNames.goodPatterns) do
				if patterns:func(s:GetName()) then
					return true
				end
			end
		end
		s = s:GetParent() -- recursion :)
	end
end

LoadAddOn("EGDebugger")
if EGDebugger then
	EGDebugger:RegisterAddon(aName)
end

local PROSPECT_ID = 31252
local MILLING_ID = 51005
local DE_ID = 13262
local PROSPECT = GetSpellInfo(PROSPECT_ID) -- "Prospecting"
local PROSPECTABLE = ITEM_PROSPECTABLE -- "Prospectable"
local MILLING = GetSpellInfo(MILLING_ID) -- "Milling"
local MILLABLE = ITEM_MILLABLE -- "Millable"
-- local SOCKETED = ITEM_SOCKETABLE -- "<Shift Right Click to Socket>" - I might use this later
local LOCKED = LOCKED -- "Locked"
local LOCKPICKING = GetSpellInfo(1804) -- "Pick Lock"
local DE = GetSpellInfo(DE_ID) -- "Disenchant"
local version = "v201210250038"
local nospam = true
local prospecting

local localeStrings = {
	["enUS"] = {
		ARMOR = "Armor";
		WEAPON = "Weapon";
	},
	["deDE"] = {
		ARMOR = "Rüstung";
		WEAPON = "Waffe";
	},
	["esES"] = {
		ARMOR = "Armadura";
		WEAPON = "Arma";
	},
	["frFR"] = {
		ARMOR = "Armure";
		WEAPON = "Arme";
	},
	["zhCN"] = {
		ARMOR = "护甲";
		WEAPON = "武器";
	},
	["enGB"] = {
		ARMOR = "Armor";
		WEAPON = "Weapon";
	},
	["ruRU"] = {
		ARMOR = "Броня";
		WEAPON = "Оружие";
	},
}

local localeInfo = localeStrings[GetLocale()] or localeStrings["enUS"]
local localeArmorCheck, localeWeaponCheck

local ARMOR = localeInfo.ARMOR
local WEAPON = localeInfo.WEAPON

local function ValidateLocale(...)
	local localeArmorCheck, localeWeaponCheck
	for i = 1, select("#", ...) do
		local test = select(i, ...)
		if ARMOR == test then
			localeArmorCheck = true
		end
		if WEAPON == test then
			localeWeaponCheck = true
		end
	end
	return localeArmorCheck, localeWeaponCheck
end

localeArmorCheck, localeWeaponCheck = ValidateLocale(GetAuctionItemClasses())

local CommandAliases = {
	button = "btn button",
	modifier = "mod modifier",
	precision = "precision presicion dec decimals places round",
	usemod = "usemod usemodifier forcemod forcemodifier",
	autoloot = "autoloot loot",
	tooltip = "tooltip tt tip gt gametooltip",
	debug = "debug debugger debugging",
	badFrames = "bad badframe ignore ignoreframe badframes ignoreframes",
	goodFrames = "good goodframe add addframe goodframes addframes",
}

ProspectOreHerbsTable = {}

-- btn:2/3/4/5 (2 = right, 3 = middle, 4 and 5 = /shrug)
-- mod:shift/ctrl/alt/lshift/lctrl/lalt/rshift/rctrl/ralt
-- modified left-click will break default behavior or other addons
-- (ctrl-left-click = dressing room alt-left-click = used by a lot of different addons shift-left-click = split stack)
-- don't even ask
ProspectOptions = {
	btn = 2,
	mod = "shift",
	alwaysUseMod = false,
	tooltip = true,
	verbose = true,
	autoloot = false,
	precision = 0,
	-- Note to self: any other options will require seperate sanity checks or one big VARIABLES_LOADED sanity check.
}

local GetSpellName = GetSpellName

if not GetSpellName then
	GetSpellName = GetSpellBookItemName
end

local function ValidateModifiedClick(proftype, spam)
	-- check for Disenchant and modify the Disenchant 
	local tester = ProspectOptions.mod:upper() .. "-" .. "BUTTON" .. ProspectOptions.btn
	if (GetSpellInfo(DE) and (proftype == DE and GetModifiedClick("SOCKETITEM") == tester))
		or GetModifiedClick("CHATLINK") == tester or GetModifiedClick("COMPAREITEMS") == tester or GetModifiedClick("SPLITSTACK") == tester or GetModifiedClick("DRESSUP") == tester then
		if nospam or spam ~= false then
			if spam ~= false then
				print("|cffff0000Your modified click collides with another, please choose a different one.")
			end
			nospam = false
		end
		return false
	end
	return true
end

local function noop() end

local OnHyperlinkClick = DEFAULT_CHAT_FRAME:GetScript("OnHyperlinkClick")
DEFAULT_CHAT_FRAME:SetScript("OnHyperlinkClick", function(self, text, ...)
	local command = text:match(aName .. ":(.*)")
	if command then
		SlashCmdList["FRAMESTACK"](command == "fstackverbose" and "true" or "")
	elseif OnHyperlinkClick then
		OnHyperlinkClick(self, text, ...)
	end
end)

local temp = {}
_G["SLASH_"..aName.."1"] = "/" .. aName:lower()
SlashCmdList[aName] = function(cmd, chatEdit)
	local out
	if chatEdit == false then
		out = noop
	else
		out = print
	end
	cmd = cmd:gsub("%s+", " "):lower()
	local cmd1, cmd2, cmd3 = strsplit(" :", cmd)
	cmd1 = strtrim(cmd1 or "", " :")
	cmd2 = strtrim(cmd2 or "", " :")
	cmd3 = strtrim(cmd3 or "", " :")
	for k, v in pairs(CommandAliases) do
		for c in string.gmatch(v, "(%w+)") do
			if cmd1 == c then
				cmd1 = k
			end
		end
	end
	if ProspectOptions.debug and debugger then
--		print(":"..cmd1..":", ":"..cmd2..":", ":"..cmd3..":")
	end
	if cmd1 == "button" then
		cmd2 = tonumber(cmd2)
		if cmd2 and cmd2 >= 2 and cmd2 <= 5 then
			ProspectOptions.btn = cmd2
			out("Button set to " .. cmd2 .. ".")
		else
			out("Invalid mouse button. Valid mouse buttons are 2, 3, 4, and 5.")
		end
	elseif cmd2 and (cmd1 == "goodFrames" or cmd1 == "badFrames") then
		ProspectOptions[cmd1][cmd2] = ProspectOptions[cmd1][cmd2] or 1
		out("Added " .. cmd2 .. " to " .. cmd1 .. ".")
	elseif cmd1 == "modifier" then
		if cmd2:find("shift") or cmd2:find("ctrl") or cmd2:find("alt") then
			ProspectOptions.mod = cmd2
			out("Modifier set to " .. cmd2 .. ".")
		else
			out("Invalid modifier. Valid modifiers are shift, ctrl, alt, lshift, lctrl, lalt, rshift, rctrl, and ralt.")
		end
	elseif cmd1 == "precision" then
		cmd2 = floor(math.abs(tonumber(cmd2)))
		ProspectOptions.precision = cmd2
		out("Precision set to " .. cmd2 .. " decimal places.")
	elseif cmd1 == "usemod" then
		ProspectOptions.alwaysUseMod = not ProspectOptions.alwaysUseMod
		if ProspectOptions.alwaysUseMod then
			out("Prospect will always use the modifier, " .. ProspectOptions.mod .. ", for all items not just Disenchanting.")
		else
			out("Prospect will only use the modifier, " .. ProspectOptions.mod .. ", for Disenchanting.")
		end
	elseif cmd1 == "debug" and debugger then
		ProspectOptions.debug = ProspectOptions.debug and nil or 1 -- not ProspectOptions.debug is not wanted because false is persistant.
	elseif cmd1 == "tooltip" then
		if cmd2 == "details" then
			if cmd3 == "on" or cmd3 == "off" then
				ProspectOptions.tooltip = (cmd3 == "on" and true or (cmd3 == "off" and false))
			else
				ProspectOptions.tooltip = not ProspectOptions.tooltip -- it's fine here, I just don't want ProspectOptions.debug to be persistant unless true.
			end
			out("Prospect will", ProspectOptions.tooltip and "show" or "not show", "detailed loot info.")
		elseif cmd2 == "verbose" then
			if cmd3 == "on" or cmd3 == "off" then
				ProspectOptions.verbose = (cmd3 == "on" and true or (cmd3 == "off" and false))
			else
				ProspectOptions.verbose = not ProspectOptions.verbose -- it's fine here, too.
			end
			out("Prospect will", ProspectOptions.verbose and  "show" or "not show", "help.")
		elseif cmd2 == "all" then
			if cmd3 == "on" or cmd3 == "off" then
				SlashCmdList[aName]("tooltip verbose "..cmd3, chatEdit)
				SlashCmdList[aName]("tooltip details "..cmd3, chatEdit)
			else
				SlashCmdList[aName]("", chatEdit)
			end
		else
			SlashCmdList[aName]("", chatEdit)
		end
	elseif cmd1 == "autoloot" then
		ProspectOptions.autoloot = not ProspectOptions.autoloot
		out("Prospect will", ProspectOptions.autoloot and  "auto loot" or "not auto loot", ".")
	elseif cmd == "reset" then
		ProspectOreHerbsTable = nil
		ReloadUI()
	else
		local chatLinkTemplate = '|cff00ff00|H' .. aName .. ':%s|h[%s]|h|r'

		out("== Prospect: Usage ==")
		out(_G["SLASH_"..aName.."1"] .. " mod <modifier key> -- change the modifier key")
		out(_G["SLASH_"..aName.."1"] .. " btn <button number> -- change the clicker")
		out(_G["SLASH_"..aName.."1"] .. ' usemod -- toggle "always use modifier" mode')
		out(_G["SLASH_"..aName.."1"] .. ' tooltip details [off|on] -- toggle "show detailed loot info" mode')
		out(_G["SLASH_"..aName.."1"] .. ' tooltip verbose [off|on] -- toggle "show help on the tooltip" mode')
		out(_G["SLASH_"..aName.."1"] .. ' tooltip all <off|on> -- sets both "details" and "verbose" to off|on')
		out(_G["SLASH_"..aName.."1"] .. ' precision <number> -- sets the number of decimal places to show on the tooltip')
		out(_G["SLASH_"..aName.."1"] .. ' autoloot <off|on> -- toggles auto looting')
		out(_G["SLASH_"..aName.."1"] .. ' reset -- clears all loot information')
		out(_G["SLASH_"..aName.."1"] .. ' add -- add frames to the filder that will be allowed to have the overlay show')
		out(_G["SLASH_"..aName.."1"] .. ' ignore -- add frames to the filter that will be ignored when it comes time to show the overlay')
		out(_G["SLASH_"..aName.."1"] .. ' To find out the name of a frame, use ' .. chatLinkTemplate:format("fstack", "/fstack") .. " or " .. chatLinkTemplate:format("fstackverbose", "/fstack true") .. ".")
		if debugger then
			out(_G["SLASH_"..aName.."1"] .. ' debug -- toggle debug mode')
		end
	end
	if not ValidateModifiedClick(DE, false) then
		SlashCmdList[aName]("btn 3", false)
		SlashCmdList[aName]("mod shift", false)
		StaticPopup_Show("PROSPECT_MODIFIER_CHANGED")
	end
end

StaticPopupDialogs["PROSPECT_MODIFIER_CHANGED"] = {
	text = "You have " .. GetSpellLink(13262) .. ". Because " .. ITEM_SOCKETING .. " uses the same modifier, Prospect's modified click has been changed to Shift Middle Click ([btn:3,mod:shift]). To change it to something else, use the /prospect command.",
	button1 = OKAY,
	OnAccept = function (self) self:Hide() end,
	OnCancel = function (self) self:Hide() end,
	hideOnEscape = 1,
	timeout = 0,
	exclusive = 1,
	whileDead = 1,
}

local sides = {"Left", "Right"}

local ignores = {}

local function SplitModifiers(...)
	local ret, curr = ""
	for i = 1, select("#", ...) do
		curr = select(i, ...)
		ret = ret .. "mod:" .. curr .. " "
	end
	return ret
end

local function GetMacroConditionals(proftype)
	local demodCheck = ProspectOptions.alwaysUseMod
	if ProspectOptions.btn <= 2 then
		demodCheck = proftype == DE and true or false
	end
	local btn = "btn:" .. ProspectOptions.btn
	local mod
	mod = SplitModifiers(strsplit("-", ProspectOptions.mod))
	return "[" .. (demodCheck and mod or (ProspectOptions.alwaysUseMod and "mod:" .. ProspectOptions.mod .. " " or "")) ..
		btn .. "] "
end

local function deAble(link)
	if not GetSpellInfo(DE) or not link then -- if Disenchant is not in the spellbook, GetSpellInfo returns nil
		return false
	end
	local _, _, qual, itemLevel, _, itemType = GetItemInfo(link)
	if ((itemType == ARMOR and localeArmorCheck) or (itemType == WEAPON and localeWeaponCheck)) and localeWeaponCheck and localeArmorCheck and qual > 1 and qual < 5 then return true end
end

local function makePlural(itemName)
	if strsub(itemName, -1) == "s" then
		itemName = itemName .. "es"
	else
		itemName = itemName .. "s"
	end
	return itemName
end

local function isDoAble()
	local proftype, itemName, tipLine, itemLink, ret
--	itemName, itemLink = GameTooltip:GetItem()
	local self = GameTooltip:GetOwner()
	if not self then
		return
	end
	local parent = self:GetParent()
	if not parent then
		return
	end
	itemLink = GetContainerItemLink(parent:GetID(), self:GetID())
	if itemLink then
		itemName = strmatch(itemLink, "%[(.*)%]")
	end
	if not itemName then
		itemName = GameTooltip:GetItem()
	end
	if not itemName then
		return
	end
	tipLine = "|cffff8800%s|r "
	for i = 1, GameTooltip:NumLines() do
		for _, side in pairs(sides) do
			local tt = _G["GameTooltipText" .. side .. i]:GetText()
			if tt == PROSPECTABLE then
				proftype = PROSPECT
				ret = format(tipLine, GetMacroConditionals(proftype) or "") .. "Turn five " .. makePlural(itemName) .. " into gems."
--				return proftype, tipLine
			elseif tt == MILLABLE then
				proftype = MILLING
				ret = format(tipLine, GetMacroConditionals(proftype) or "") .. "Turn five " .. makePlural(itemName) .. " into pigments."
--				return proftype, tipLine
			elseif tt == LOCKED then
				proftype = LOCKPICKING
				ret = format(tipLine, GetMacroConditionals(proftype) or "") .. "Unlock " .. makePlural(itemName) .. "."
--				return proftype, tipLine
			elseif deAble(itemLink) then
				proftype = DE
				ret = format(tipLine, GetMacroConditionals(proftype) or "") .. DE .. " " .. itemName .. "."
--				return proftype, tipLine
			end
		end
	end
	if not ValidateModifiedClick(proftype) then
		return false
	end
	return proftype, ret
end

local buttons = {}

local function hookParent(f)
	f:HookScript("OnHide", function(self)
		for _, v in ipairs(buttons) do
			v:Hide()
		end
	end)
	function hookParent(f) end
end

local function GetMacroText(self)
	local condition = GetMacroConditionals(self.proftype)
	if not condition then
		return false
	end
	if self.proftype then
		return "/cast " .. condition .. self.proftype .. "\n/use " .. self.bag .. " " .. self.slot
	end
end

local i = 1
local proftype, tipLine
local theDraggingFrame

local buttonNames = {"LeftButton", "RightButton", "MiddleButton", "Button4", "Button5", }
local itemID, itemLink, checkOre

local function CreateItemButtonOverlay(itemFrame)
	if itemFrame.action then
		return
	end
	-- ugly hack to prevent interfering with the quest log frame
	-- also, a hack for item buff buffs
	if not itemFrame:GetName() or itemFrame:GetName():find("Quest") then
		return
	end
--	if not itemFrame.hasItem then
--		return
--	end
	if InCombatLockdown() then
		return
	end
	local parent = itemFrame:GetParent()
	if not parent then
		return
	end
	local fname = itemFrame:GetName()
	if fname then
		fname = fname .. "ProspectOverlay"
	else
		fname = "ProspectOverlay" .. i
		i = i + 1
	end
	if buttons[fname] then
		return buttons[fname]
	end
	local bag = parent:GetID()
	local slot = itemFrame:GetID()
	local button = CreateFrame("Button", fname, itemFrame, "SecureActionButtonTemplate")
--	button:SetParent(parent)
	button:SetScript("OnEvent", function(self, buttonClicked, ...)
		self:UnregisterEvent("UNIT_SPELLCAST_FAILED")
--		self:UnregisterEvent("UNIT_INVENTORY_CHANGED")
		prospecting = nil
	end)
	button:SetScript("PreClick", function(self, buttonClicked, ...)
		local activate = false
		local demodCheck = ProspectOptions.alwaysUseMod
		if ProspectOptions.btn <= 2 then
			demodCheck = self.proftype == DE and true or false
		end
		local btn = ProspectOptions.btn
		if (demodCheck and ProspectOptions.mod) or ProspectOptions.alwaysUseMod then
			for v in ProspectOptions.mod:gmatch("[lrctashif]+") do -- this is better than making a throw away table each time.
				if buttonClicked == buttonNames[btn] and (((v == "ctrl" or v == "lctrl" or v == "rctrl") and IsControlKeyDown()) or ((v == "alt" or v == "lalt" or v == "ralt") and IsAltKeyDown()) or ((v == "shift" or v == "lshift" or v == "rshift") and IsShiftKeyDown())) then
					activate = true -- maybe that will fix the logic here...
				end
			end
		elseif buttonClicked == buttonNames[btn] then
			activate = true
		end
		if activate then
			itemID = strmatch((select(2, GetContainerItemLink(self:GetParent():GetParent():GetID(), self:GetParent():GetID())) or ""), "item:(%d*):")
			self:RegisterEvent("UNIT_SPELLCAST_FAILED")
--			self:RegisterEvent("UNIT_INVENTORY_CHANGED")
			prospecting = true
		end
	end)
	
	overlayButtons[fname] = {
		fname = itemFrame:GetName(),
		point = {itemFrame:GetPoint()},
	}
--	ProspectOptions["debugStack"] = ProspectOptions["debugStack"] or {}
--	tinsert(ProspectOptions["debugStack"], overlayButtons[fname])

--	table.insert(buttons, button)
--	hookParent(parent)
	button:SetID(slot)
	button.isProspectButton = 1
	button:SetAttribute("*type*", "macro")
	button:SetAttribute("*type1", "click")
--	button:SetAttribute("*clickbutton*", ATTRIBUTE_NOOP)
	button:SetAttribute("*clickbutton1", itemFrame)
	button:RegisterForClicks("AnyUp")
	button:RegisterForDrag("LeftButton")	
	button:SetFrameStrata(itemFrame:GetFrameStrata())
	button:SetParent(itemFrame)
	button:SetAllPoints(--[[self:GetPoint(1)]])
	button:SetFrameLevel(itemFrame:GetFrameLevel() + 1)
	button:SetScript("OnShow", function(self)
		if InCombatLockdown() then
			return
		end
		proftype, tipLine = isDoAble()
		local macrotext = GetMacroText(self)
		if proftype and macrotext then
			self.proftype = proftype
			self:SetAttribute("*macrotext*", macrotext)
			self:SetAttribute("*macrotext1", ATTRIBUTE_NOOP)
		else
			self:Hide()
		end
	end)
	local OL_Script = itemFrame:GetScript("OnLeave")
	local ODSa_Script = itemFrame:GetScript("OnDragStart")
	local OE_Script = itemFrame:GetScript("OnEnter")
	button:SetScript("OnLeave", function(self, ...)
		tipLine = nil
--		self:Hide()
		if OL_Script then
			OL_Script(itemFrame, ...)
		end
	end)
	itemFrame:HookScript("OnLeave", function(self, ...)
		if GameTooltip:IsOwned(self) and GameTooltip:GetOwner() == self then
			GameTooltip:Hide()
		end
		tipLine = nil
		if button:IsVisible() then
			button:Hide()
		end
	end)
	-- Gah!!!
	--[[ This was supposed to fix OnReceiveDrag, but it doesn't.
	local ORD_Script = itemFrame:GetScript("OnReceiveDrag")
	button:SetScript("OnUpdate", function(self, e, ...)
		local itemFrame = self:GetParent()
		local rallyPoint = GetMouseFocus() -- StarCraft reference
		if CursorHasItem() and not IsMouseButtonDown("LeftButton") and itemFrame.isDragging and rallyPoint then
			itemFrame.isDragging = false
			if ORD_Script then
				ORD_Script(rallyPoint, ...)
			end			
		end
	end)
	--]]
--		print(GetMouseFocus():GetName())
--		print(self:GetName())
	button:SetScript("OnDragStart", function(self, ...)
--		self:Hide()
		theDraggingFrame = self
		itemFrame.isDragging = true
		itemFrame.oldBag = itemFrame:GetParent():GetID()
		itemFrame.oldSlot = itemFrame:GetID()
		if ODSa_Script then
			ODSa_Script(itemFrame, ...)
		end
	end)
	button:SetScript("OnEnter", function(self, ...)
		if OE_Script then
			OE_Script(itemFrame, ...)
		end 
	end)
	button.UpdateTooltip = function(self, ...) 
		itemFrame:UpdateTooltip(...)
	end
	button.SplitStack = itemFrame.SplitStack
	itemFrame:HookScript("OnEnter", function(self, ...)
		if self.ProspectButton then
			if (ArkInventory and not ArkInventory.Global.Mode.Edit) or not ArkInventory then
				self.ProspectButton:Show()
			end
		end
	end)
	itemFrame:HookScript("OnHide", function(self, ...)
		if GameTooltip:IsOwned(self) and GameTooltip:GetOwner() == self then
			GameTooltip:Hide()
		end
		tipLine = nil
	end)
--	button:HookScript("OnClick", print)
--	button:Show()
	buttons[fname] = button
	return button
end
-- count returns 1 only if there's at least one element, it does not return the number of elements in the table.
local function count(tab)
	for k, v in pairs(tab) do
		return 1
	end
	return nil
end

--local worker = function(self)
local worker = function(GameTooltip)
	local GameTooltip_item, GameTooltip_itemLink = GameTooltip:GetItem()
	if not prospecting
		and not CursorHasItem()
		and not InCombatLockdown()
		and (ProspectOptions.btn > 2
			or not (ignores["BANKFRAME"]
				or ignores["GUILDBANKFRAME"]
				or ignores["MAIL"]
				or ignores["MERCHANT"]
				or ignores["TRADE"]
			)
		)
		and GameTooltip_item then -- weird error discovered when the tooltip is from the chat frame (only affects you if you have an addon that makes chat links into tooltips).
		local self = GameTooltip:GetOwner()
		if not self then
			return
		end
		if self.isProspectButton then
			self = self:GetParent() -- I am my own father!
		end
		if not self then -- Something has gone horrible wrong.
			return
		end
		if not GoodFrame(self) then
			if BadFrame(self) then
				return
			end
		end
		local parent = self:GetParent()
		if not parent then -- Catastrophic failure! Abandon ship!
			return
		end
		local bag = parent:GetID()
		local slot = self:GetID()
		proftype, tipLine = isDoAble()
		local button = CreateItemButtonOverlay(self)
		if not button then
			return
		end
		if proftype then
			button.slot = slot
			button.bag = bag
			button:Show()
			local r, g, b = ChatTypeInfo["SYSTEM"]
			local itemID = strmatch((select(2, GameTooltip:GetItem()) or ""), "item:(%d*):")
			if itemID and checkOre --[[ * why do I care if it's greater than 4? * and (select(2, GetContainerItemInfo(button.bag, button.slot))) >= 5 ]] and not ProspectOreHerbsTable[itemID] then
				ProspectOreHerbsTable[itemID] = {}
			end
			if ProspectOreHerbsTable[itemID] and ProspectOptions.tooltip then
				for itemName, data in pairs(ProspectOreHerbsTable[itemID]) do
					local timesLooted, looted, multipleDiff, multipleDiffText, countPerLoot, chancePerLoot = ProspectOreHerbsTable[itemID]["timesLooted"], (ProspectOreHerbsTable[itemID]["looted"] or 0), 0, ""
					local precision = ProspectOptions.precision or 0
					local chanceForDropNextLoot
					if itemName ~= "looted" and itemName ~= "timesLooted" then
						timesLooted = timesLooted or data["count"]
						chanceForDropNextLoot = tonumber(timesLooted > 0 and format("%."..precision.."f", data["total"] / timesLooted * 100) or 0)
						countPerLoot = format("%."..precision.."f", data["total"] / data["count"])
						chancePerLoot = looted > 0 and floor(data["total"] / looted * 100) or 0
						if chancePerLoot > 100 then
							multipleDiff = min(chancePerLoot - 100, 100)
							chancePerLoot = 100
						end
						if chanceForDropNextLoot > 100 then
							chanceForDropNextLoot = 100
						end
						if multipleDiff > 0 then
							multipleDiffText = "(" .. multipleDiff .. "% chance for milti-drop)"
						end
						GameTooltip:AddDoubleLine(itemName, format("%s/loot %s%% chance %s (%s%% next loot)",
							countPerLoot,
							chancePerLoot,
							multipleDiffText,
							chanceForDropNextLoot
							), r, g, b, r, g, b)
					end
				end
			end
			button:ClearAllPoints()
		--			local scale = self:GetEffectiveScale()
		--			button:SetWidth(self:GetWidth() * scale)
		--			button:SetHeight(self:GetHeight() * scale)
		--			button:SetParent(self)
			button:SetAllPoints(--[[self:GetPoint(1)]])
			button.proftype = proftype
			
			local macrotext = GetMacroText(button)
			if macrotext then
				button:SetAttribute("*macrotext*", macrotext)
				button:SetAttribute("*macrotext1", ATTRIBUTE_NOOP)
			end
			if tipLine and ProspectOptions.verbose and GetMouseFocus() == button then
				GameTooltip:AddLine(tipLine, 0, 1, 0)
				if debugger and ProspectOptions.debug then
					local macroText = button:GetAttribute("*macrotext*")
					if macroText then
						GameTooltip:AddLine("Macro")
						GameTooltip:AddLine(macroText, 0, 1, 0)
					end
					local typeText = button:GetAttribute("*type*")
					if typeText then
						GameTooltip:AddLine("Type")
						GameTooltip:AddLine(typeText, 0, 1, 0)
					end
				end
			end
			if (ArkInventory and not ArkInventory.Global.Mode.Edit) or not ArkInventory then
				button:Show()
			end
			GameTooltip:Show()
		else
			button:Hide()
		end
		if self:GetName():match("BrowseButton") then
			button:Hide()
		end
	end
end

local f = CreateFrame("Frame")
f:SetScript("OnEvent", function(self, event, ...)
	local frame1, frame2, opened = strsplit("_", event)
	if not opened then
		opened = frame2
		frame2 = nil
	end
	if frame2 then
		frame1 = frame1 .. "_" .. frame2
	end
	if opened == "OPENED" or opened == "SHOW" then
		ignores[frame1] = 1
	elseif opened == "CLOSED" then
		ignores[frame1] = nil
	end
	-- clean up the table
	if event == "PLAYER_LEAVING_WORLD" then
		for k,v in pairs(ProspectOreHerbsTable) do
			if not count(v) then
				ProspectOreHerbsTable[k] = nil
			end
		end
	elseif event == "PLAYER_ENTERING_WORLD" then
		ProspectOptions.badFrames = ProspectOptions.badFrames or {}
		ProspectOptions.goodFrames = ProspectOptions.goodFrames or {}
		for _, id in pairs{"good", "bad"} do
			local index = id .. "Frames"
			for fname in pairs(ProspectOptions[index]) do
				frameNames[index][fname] = frameNames[index][fname] or 1
			end
		end
		ProspectOptions.btn = ProspectOptions.btn or 2
		ProspectOptions.mod = ProspectOptions.mod or "shift"
		if type(ProspectOptions.alwaysUseMod) ~= "boolean" then
			ProspectOptions.alwaysUseMod = false
		end
		if type(ProspectOptions.tooltip) ~= "boolean" then
			ProspectOptions.tooltip = true
		end
		if type(ProspectOptions.verbose) ~= "boolean" then
			ProspectOptions.verbose = true
		end
		if type(ProspectOptions.autoloot) ~= "boolean" then
			ProspectOptions.autoloot = false
		end
		ProspectOptions.precision = ProspectOptions.precision or 0
--		GameTooltip:HookScript("OnShow", worker)
		GameTooltip:HookScript("OnTooltipSetItem", worker)
		local points = {}
		local frame, frames
		frames = {}
		for bag = 0, NUM_BAG_FRAMES do
			for slot = 1, GetContainerNumSlots(bag) do
				local container = _G["ContainerFrame" .. bag]
				local button = _G["ContainerFrame" .. bag .. "Item" .. slot]
				local b
				if button then
					b = CreateItemButtonOverlay(button)
					b:Hide()
				end
			end
		end
		if not ValidateModifiedClick(DE, false) then
			SlashCmdList[aName]("btn 3", false)
			SlashCmdList[aName]("mod shift", false)
			StaticPopup_Show("PROSPECT_MODIFIER_CHANGED")
		end
		self:UnregisterEvent("PLAYER_ENTERING_WORLD")
	elseif event == "LOOT_OPENED" then
		if EGDebugger and EGDebugger:AddonRegistered(aName) then
			--print(prospecting)
		end
		if itemID then
			ProspectOreHerbsTable[itemID] = ProspectOreHerbsTable[itemID] or {}
		end
		for i = 1, GetNumLootItems() do
			if itemID then
				ProspectOreHerbsTable[itemID]["looted"] = (ProspectOreHerbsTable[itemID]["looted"] or 0) + 1
				local itemName, lootQuantity = select(2, GetLootSlotInfo(i))
				if not ProspectOreHerbsTable[itemID][itemName] then
					ProspectOreHerbsTable[itemID][itemName] = {}
				end
				ProspectOreHerbsTable[itemID]["timesLooted"] = ProspectOreHerbsTable[itemID]["timesLooted"] or (ProspectOreHerbsTable[itemID][itemName]["count"] or 0)
				ProspectOreHerbsTable[itemID][itemName]["total"] = (ProspectOreHerbsTable[itemID][itemName]["total"] or 0) + lootQuantity
				ProspectOreHerbsTable[itemID][itemName]["count"] = (ProspectOreHerbsTable[itemID][itemName]["count"] or 0) + 1
--			else can't process this item
			end
			if ProspectOptions.autoloot and prospecting then
				LootSlot(i)
			end
		end
		if itemID then
			ProspectOreHerbsTable[itemID]["timesLooted"] = (ProspectOreHerbsTable[itemID]["timesLooted"] or 0) + 1
		end
		prospecting = nil
	elseif event == "LOOT_CLOSED" or event == "UNIT_SPELLCAST_FAILED" then
		local arg1 = ...
		if event == "LOOT_CLOSED" or arg1 == "player" then
			prospecting, itemID, itemLink, checkOre = nil, nil, nil, nil
		end
	elseif event == "PLAYER_REGEN_DISABLED" then
		local f
		for fname in pairs(overlayButtons) do
			f = _G[fname]
			f:SetParent(WorldFrame)
			f:ClearAllPoints()
			if f:IsShown() then
				if f:GetParent():IsShown() then
					overlayButtons[fname].wasShowing = 1
				else
					overlayButtons[fname].wasShowing = nil
				end
				f:Hide()
			end
		end
	elseif event == "PLAYER_REGEN_ENABLED" then
		local f
		for fname, parent in pairs(overlayButtons) do
			f = _G[fname]
			f:SetParent(_G[parent.fname])
			f:SetPoint(unpack(parent.point))
			if overlayButtons[fname].wasShowing and f:GetParent():IsShown() then
				f:Show()
			end
		end
	end
end)

f:RegisterEvent("PLAYER_REGEN_ENABLED")
f:RegisterEvent("PLAYER_REGEN_DISABLED")
f:RegisterEvent("BANKFRAME_OPENED")
f:RegisterEvent("BANKFRAME_CLOSED")
f:RegisterEvent("GUILDBANKFRAME_OPENED")
f:RegisterEvent("GUILDBANKFRAME_CLOSED")
f:RegisterEvent("MAIL_SHOW")
f:RegisterEvent("MAIL_CLOSED")
f:RegisterEvent("MERCHANT_SHOW")
f:RegisterEvent("MERCHANT_CLOSED")
f:RegisterEvent("TRADE_SHOW")
f:RegisterEvent("TRADE_CLOSED")
f:RegisterEvent("AUCTION_HOUSE_SHOW")
f:RegisterEvent("AUCTION_HOUSE_CLOSED")
f:RegisterEvent("PLAYER_ENTERING_WORLD")
f:RegisterEvent("PLAYER_LEAVING_WORLD")

f:RegisterEvent("LOOT_OPENED")
f:RegisterEvent("LOOT_CLOSED")

f:RegisterEvent("UNIT_SPELLCAST_FAILED")

--[[ So I don't have to keep scrolling up for copy 'n paste
local PROSPECT_ID = 31252
local MILLING_ID = 51005
local DE_ID = 13262
local PROSPECT = GetSpellInfo(PROSPECT_ID) -- "Prospecting"
local PROSPECTABLE = ITEM_PROSPECTABLE -- "Prospectable"
local MILLING = GetSpellInfo(MILLING_ID) -- "Milling"
local MILLABLE = ITEM_MILLABLE -- "Millable"
local LOCKED = LOCKED -- "Locked"
local LOCKPICKING = GetSpellInfo(1804) -- "Pick Lock"
local DE = GetSpellInfo(DE_ID) -- "Disenchant"
--]]

hooksecurefunc("UseContainerItem", function(...)
	if checkOre then
		itemLink = GetContainerItemLink(...)
		itemID = strmatch((itemLink or ""), "item:(%d*):")
	end
end)
hooksecurefunc("ContainerFrameItemButton_OnDrag", function(self, button)
	if button then
		return
	end
	if theDraggingFrame then
		theDraggingFrame:Hide()
	end
	--[[
	local f
	for fname in pairs(overlayButtons) do
		f = _G[fname]
		if f then
			f:Hide()
		end
	end
	--]]
end)
hooksecurefunc("UseItemByName", function(itemName)
	if checkOre then
		itemLink = select(2, GetItemInfo(itemName))
		itemID = strmatch((itemName or ""), "item:(%d*):")
	end
end)

hooksecurefunc("CastSpell", function(...)
	if GetSpellInfo(PROSPECT_ID) == GetSpellName(...) or GetSpellInfo(MILLING_ID) == GetSpellName(...) then
		checkOre = 1
	end
end)
hooksecurefunc("CastSpellByName", function(spellName, onSelf)
	if spellName == PROSPECT or spellName == MILLING then
		checkOre = 1
	end
end)

do
	local function GetWatchingCurrency()
		local watched
		for i = 1, GetCurrencyListSize() do
			_, _, _, _, watched = GetCurrencyListInfo(i)
			if watched then
				return watched
			end
		end
	end
	local GenerateFrame = ContainerFrame_GenerateFrame
	function ContainerFrame_GenerateFrame(frame, size, id)
		notified = false
		GenerateFrame(frame, size, id)
		local watching = GetWatchingCurrency()
		frame.size = size;
		local name = frame:GetName();
		local bgTextureTop = _G[(name.."BackgroundTop")]
		local columns = NUM_CONTAINER_COLUMNS;
		local index, itemButton;
		for i=1, size, 1 do
			index = size - i + 1;
			itemButton = _G[name.."Item"..i];
			itemButton:SetID(index);
			itemButton:ClearAllPoints()
			-- Set first button
			if ( i == 1 ) then
				-- Anchor the first item differently if its the backpack frame
				if ( id == 0 ) then
	--				print("backpack slot 1")
					if watching then
						itemButton:SetPoint("BOTTOMRIGHT", name, "BOTTOMRIGHT", -12, 30 + BACKPACK_TOKENFRAME_HEIGHT);
					else
						itemButton:SetPoint("BOTTOMRIGHT", name, "BOTTOMRIGHT", -12, 30);
					end
				else
	--				print("other bag slot 1")
					itemButton:SetPoint("BOTTOMRIGHT", name, "BOTTOMRIGHT", -12, 9);
				end
				
			else
				if ( mod((i-1), columns) == 0 ) then
					itemButton:SetPoint("BOTTOMRIGHT", name.."Item"..(i - columns), "TOPRIGHT", 0, 4);	
				else
					itemButton:SetPoint("BOTTOMRIGHT", name.."Item"..(i - 1), "BOTTOMLEFT", -5, 0);	
				end
			end
			itemButton:Show();
		end
	end
	TokenFramePopupBackpackCheckBox:HookScript("OnClick", function(self, button)
		if InCombatLockdown() then
			--return
		end
		local backpack = GetBackpackFrame()
		if backpack and backpack:IsShown() then
			backpack:Hide()
			ContainerFrame_GenerateFrame(backpack, backpack.size, backpack:GetID())
		end
	end)
end
DEFAULT_CHAT_FRAME:AddMessage("Prospect: Loaded")
