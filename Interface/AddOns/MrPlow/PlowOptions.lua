local db
local AceConfig = LibStub("AceConfig-3.0", true)
local AceConfigDialog = LibStub("AceConfigDialog-3.0", true)
local AceDBOptions = LibStub("AceDBOptions-3.0", true)
local icon = LibStub("LibDBIcon-1.0", true)

-- Temporary fields for manual input in options UI
local tempBagInput = 0
local tempSlotInput = 1
local tempIsGuild = false
local tempItemInput = ""

function MrPlow:InitialiseOptions()
	db = MrPlow.db.profile
	self:BuildOptions()
end

local createBorder = function(self, point)
	local bc = self:CreateTexture(nil, "OVERLAY")
	bc:SetTexture("Interface\\Buttons\\UI-ActionButton-Border")
	bc:SetBlendMode("ADD")
	bc:SetAlpha(0.8)
	bc:SetPoint("CENTER", point or self)
	return bc
end

local colorTable = setmetatable({
	["ignore-slot"] = {r = 0.9, g = 0, b = 0, s = 60},
	["ignore-item"] = {r = 1, g = 1, b = 0, s = 60},
}, {__call = function(self, val)
	local c = self[val]
	if c then return c.r, c.g, c.b, c.s end
end})

function MrPlow:Overlay(frame, quality)
	if not frame then return end
	if frame[quality] then
		if frame[quality]:IsVisible() then 
			frame[quality]:Hide()
		else 
			frame[quality]:Show()
		end
		return		
	end

	local bc = createBorder(frame)
	frame[quality] = bc
	if bc then
		local r, g, b, s = colorTable(quality)
		if r and g and b then
			bc:SetVertexColor(r, g, b)
		end
		if s then
			bc:SetWidth(s)
			bc:SetHeight(s)
		end
		bc:Show()
	end
end

function MrPlow:IsIgnored(bag, slot, guild)
	if not bag then return false end
	if not db then db = MrPlow.db and MrPlow.db.profile end
	if not db then return false end

	if guild then
		if db.IgnoreGuild and (db.IgnoreGuild[bag] == 1 or db.IgnoreGuild[bag] == true) then
			return true
		end
		if slot and db.IgnoreGuild and type(db.IgnoreGuild[bag]) == "table" and db.IgnoreGuild[bag][slot] then
			return true
		end
		return false
	end

	if db.IgnoreBags and db.IgnoreBags[bag] then
		return true
	end

	if not slot then
		return false
	end

	if db.IgnoreSlots and db.IgnoreSlots[bag] and db.IgnoreSlots[bag][slot] then
		return true
	end

	return false
end

function MrPlow:Ignore(bag, slot, guild)
	if not bag then return end
	if not db then db = MrPlow.db and MrPlow.db.profile end
	if not db then return end

	if guild then
		if not slot then
			db.IgnoreGuild[bag] = 1
			return
		end
		if not db.IgnoreGuild[bag] or type(db.IgnoreGuild[bag]) ~= "table" then
			db.IgnoreGuild[bag] = {}
		end
		db.IgnoreGuild[bag][slot] = 1
		return
	end

	if not slot then
		db.IgnoreBags[bag] = 1
		return
	end

	if not db.IgnoreSlots[bag] or type(db.IgnoreSlots[bag]) ~= "table" then
		db.IgnoreSlots[bag] = {}
	end
	db.IgnoreSlots[bag][slot] = 1
end

function MrPlow:Unignore(bag, slot, guild)
	if not bag then return end
	if not db then db = MrPlow.db and MrPlow.db.profile end
	if not db then return end

	if guild then
		if not slot then
			db.IgnoreGuild[bag] = nil
			return
		end
		if type(db.IgnoreGuild[bag]) == "table" then
			db.IgnoreGuild[bag][slot] = nil
		end
		return
	end

	if not slot then
		db.IgnoreBags[bag] = nil
		return
	end

	if db.IgnoreSlots and type(db.IgnoreSlots[bag]) == "table" then
		db.IgnoreSlots[bag][slot] = nil
	end
end

function MrPlow:IsBagIgnored(bag)
	if not db then db = MrPlow.db and MrPlow.db.profile end
	return (db and db.IgnoreBags and db.IgnoreBags[bag]) and true or false
end

function MrPlow:SetBagIgnored(bag, val)
	if val then
		self:Ignore(bag)
	else
		self:Unignore(bag)
	end
end

function MrPlow:IsGuildTabIgnored(tab)
	if not db then db = MrPlow.db and MrPlow.db.profile end
	return (db and db.IgnoreGuild and (db.IgnoreGuild[tab] == 1 or db.IgnoreGuild[tab] == true)) and true or false
end

function MrPlow:SetGuildTabIgnored(tab, val)
	if val then
		self:Ignore(tab, nil, true)
	else
		self:Unignore(tab, nil, true)
	end
end

BagFrames = {}

function MrPlow:ScanFrame(children, parent)
	local FullFrameList = children and children or {GetMouseFocus():GetChildren()}
	for i, v in ipairs(FullFrameList) do
		if v and v["GetID"] then
			if not parent then
				parent = true
				self:ScanFrame({v:GetChildren()}, parent)
			end
			local name = v:GetName()
			if v:GetID() ~= 0 and name and name:find("%d.*%d") and v:GetParent() and v:GetParent()["GetID"] then
				BagFrames[v:GetParent():GetID() .. "-" .. v:GetID()] = name
			end
		end
	end
end

function MrPlow:ScanAllFrames()
	BagFrames = {}
	local frame = EnumerateFrames()
	while frame do
		if frame:IsVisible() and frame["GetID"] and frame:GetParent() and frame:GetParent()["GetID"] and
			frame:GetID() ~= 0 and frame:GetName() and frame:GetName():find("%d.*%d") then
			BagFrames[frame:GetParent():GetID() .. "-" .. frame:GetID()] = frame:GetName()
		end
		frame = EnumerateFrames(frame)
	end
end

function MrPlow:OverlayBags()
	if not db then db = MrPlow.db and MrPlow.db.profile end
	if not db or not db.IgnoreSlots then return end

	self:ScanAllFrames()
	for b, bag in pairs(db.IgnoreSlots) do
		if type(bag) == "table" then
			for s, _ in pairs(bag) do
				local frameName = BagFrames[b .. "-" .. s]
				if frameName and _G[frameName] then
					MrPlow:Overlay(_G[frameName], "ignore-slot")
				end
			end
		end
	end
end

-- Mouse picker for interactive slot/item ignoring
local mouser = CreateFrame("Frame")

function mouser:OnUpdate(elap)
	local frame = GetMouseFocus()
	if not frame then return end
	local name = frame:GetName() or tostring(frame)

	SetCursor("CAST_CURSOR")

	-- Right-Click ignores the specific bag slot
	if IsMouseButtonDown("RightButton") then
		self:Stop()
		local parent = frame:GetParent()
		if frame.GetID and parent and parent.GetID then
			local slot = frame:GetID()
			local bag = parent:GetID()
			local isGuild = name:find("GuildBank") and true or false

			MrPlow:Ignore(bag, slot, isGuild)
			MrPlow:Overlay(frame, "ignore-slot")
			MrPlow:Print(("Ignored slot: %s Bag %d, Slot %d"):format(isGuild and "Guild Bank" or "Personal/Bank", bag, slot))
			return
		end
	end

	-- Left-Click ignores the item type
	if IsMouseButtonDown("LeftButton") then
		self:Stop()
		local parent = frame:GetParent()
		if frame.GetID and parent and parent.GetID then
			local slot = frame:GetID()
			local bag = parent:GetID()
			local isGuild = name:find("GuildBank") and true or false
			local ItemFunc = isGuild and GetGuildBankItemLink or GetContainerItemLink
			local link = ItemFunc(bag, slot)
			if link then
				local itemID = tonumber(link:match("item:(%d+)"))
				if itemID then
					if not db then db = MrPlow.db and MrPlow.db.profile end
					if not db.IgnoreItems then db.IgnoreItems = {} end
					db.IgnoreItems[itemID] = true
					MrPlow:Overlay(frame, "ignore-item")
					MrPlow:Print(("Ignored item: %s (ID %d)"):format(link, itemID))
					return
				end
			else
				MrPlow:Print("No item in clicked slot.")
			end
		end
	end
end

function mouser:Start()
	self:SetScript("OnUpdate", self.OnUpdate)
	MrPlow:Print("Mouse picker active. Left-Click item to ignore item type; Right-Click slot to ignore slot. Press ESC or open game menu to cancel.")
end

function mouser:Stop()
	self:SetScript("OnUpdate", nil)
	ResetCursor()
end

hooksecurefunc(_G.GameMenuFrame, "Show", function() mouser:Stop() end)

function MrPlow:StartMouser()
	mouser:Start()
end

local function GetTabLabel(tab)
	if GetGuildBankTabInfo then
		local name = GetGuildBankTabInfo(tab)
		if name and name ~= "" then
			return ("Tab %d (%s)"):format(tab, name)
		end
	end
	return ("Tab %d"):format(tab)
end

function MrPlow:GetOptionsTable()
	local options = {
		type = "group",
		name = "Mr Plow",
		handler = MrPlow,
		childGroups = "tab",
		args = {
			general = {
				type = "group",
				name = "General",
				order = 10,
				args = {
					description = {
						type = "description",
						name = "Mr Plow is an inventory restacker, defragmenter, and sorter that physically organizes your bags, bank, and guild bank.",
						fontSize = "medium",
						order = 1,
					},
					spaceHeader = {
						type = "header",
						name = "Inventory Organization Settings",
						order = 2,
					},
					emptySpace = {
						type = "select",
						name = "Empty Space Position",
						desc = "Controls whether empty spaces are placed at the bottom (packing items towards the front) or at the top (packing items towards the back).",
						order = 3,
						values = {
							["Bottom"] = "Bottom (Pack items forward)",
							["Top"] = "Top (Pack items backward)",
						},
						get = function()
							if not db then db = MrPlow.db and MrPlow.db.profile end
							return db and db.EmptySpace or "Bottom"
						end,
						set = function(_, val)
							if not db then db = MrPlow.db and MrPlow.db.profile end
							if db then db.EmptySpace = val end
						end,
					},
					minimap = {
						type = "toggle",
						name = "Show Minimap Button",
						desc = "Toggle the visibility of the Mr Plow launcher button on the minimap.",
						order = 4,
						get = function()
							if not db then db = MrPlow.db and MrPlow.db.profile end
							return not (db and db.hide)
						end,
						set = function(_, val)
							if not db then db = MrPlow.db and MrPlow.db.profile end
							if db then
								if val then
									db.hide = nil
									if icon then icon:Show("MrPlowLDB") end
								else
									db.hide = true
									if icon then icon:Hide("MrPlowLDB") end
								end
							end
						end,
					},
					actionsHeader = {
						type = "header",
						name = "Quick Actions",
						order = 10,
					},
					bagGroup = {
						type = "group",
						name = "Personal Bags",
						inline = true,
						order = 11,
						args = {
							works = {
								type = "execute",
								name = "The Works",
								desc = "Full cycle: restacks, consolidates, defragments, and sorts personal bags.",
								order = 1,
								func = function() MrPlow:DoStuff("theworks") end,
							},
							sort = {
								type = "execute",
								name = "Sort",
								desc = "Sorts items by quality, type, and name.",
								order = 2,
								func = function() MrPlow:DoStuff("sort") end,
							},
							stack = {
								type = "execute",
								name = "Stack",
								desc = "Combines partial stacks across bags.",
								order = 3,
								func = function() MrPlow:DoStuff("stack") end,
							},
							defrag = {
								type = "execute",
								name = "Defrag",
								desc = "Pushes empty spaces together.",
								order = 4,
								func = function() MrPlow:DoStuff("defrag") end,
							},
							consolidate = {
								type = "execute",
								name = "Consolidate",
								desc = "Packs items from later bags into earlier bags.",
								order = 5,
								func = function() MrPlow:DoStuff("consolidate") end,
							},
						},
					},
					bankGroup = {
						type = "group",
						name = "Bank Bags",
						inline = true,
						order = 12,
						args = {
							bankworks = {
								type = "execute",
								name = "The Works (Bank)",
								desc = "Full cycle for bank inventory.",
								order = 1,
								func = function() MrPlow:DoStuff("banktheworks") end,
							},
							banksort = {
								type = "execute",
								name = "Sort Bank",
								desc = "Sorts items in the bank.",
								order = 2,
								func = function() MrPlow:DoStuff("banksort") end,
							},
							bankstack = {
								type = "execute",
								name = "Stack Bank",
								desc = "Combines partial stacks in the bank.",
								order = 3,
								func = function() MrPlow:DoStuff("bankstack") end,
							},
							bankdefrag = {
								type = "execute",
								name = "Defrag Bank",
								desc = "Defragments empty slots in the bank.",
								order = 4,
								func = function() MrPlow:DoStuff("bankdefrag") end,
							},
							bankconsolidate = {
								type = "execute",
								name = "Consolidate Bank",
								desc = "Consolidates items in the bank.",
								order = 5,
								func = function() MrPlow:DoStuff("bankconsolidate") end,
							},
						},
					},
					guildGroup = {
						type = "group",
						name = "Guild Bank (Current Tab)",
						inline = true,
						order = 13,
						args = {
							gworks = {
								type = "execute",
								name = "The Works (Guild Bank)",
								desc = "Full cycle for the active guild bank tab.",
								order = 1,
								func = function() MrPlow:DoStuff("gbanktheworks") end,
							},
							gsort = {
								type = "execute",
								name = "Sort Guild Tab",
								desc = "Sorts items in active guild bank tab.",
								order = 2,
								func = function() MrPlow:DoStuff("gbanksort") end,
							},
							gstack = {
								type = "execute",
								name = "Stack Guild Tab",
								desc = "Combines partial stacks in active guild bank tab.",
								order = 3,
								func = function() MrPlow:DoStuff("gbankstack") end,
							},
							gdefrag = {
								type = "execute",
								name = "Defrag Guild Tab",
								desc = "Defragments active guild bank tab.",
								order = 4,
								func = function() MrPlow:DoStuff("gbankdefrag") end,
							},
						},
					},
					emergencyGroup = {
						type = "group",
						name = "Control",
						inline = true,
						order = 14,
						args = {
							stop = {
								type = "execute",
								name = "Emergency Stop",
								desc = "Immediately stops all pending and active inventory sorting operations.",
								order = 1,
								confirm = true,
								confirmText = "Stop all active Mr Plow operations?",
								func = function() MrPlow:DoStuff("stop") end,
							},
						},
					},
				},
			},
			ignoreBags = {
				type = "group",
				name = "Ignored Bags",
				order = 20,
				args = {
					info = {
						type = "description",
						name = "Checked bags will be completely skipped by Mr Plow during restacking, sorting, and defragmenting.",
						fontSize = "medium",
						order = 1,
					},
					personalBags = {
						type = "group",
						name = "Personal Bags (0 to 4)",
						inline = true,
						order = 10,
						args = {
							bag0 = {
								type = "toggle",
								name = "Backpack (Bag 0)",
								desc = "Ignore the 16-slot Backpack.",
								order = 1,
								get = function() return MrPlow:IsBagIgnored(0) end,
								set = function(_, val) MrPlow:SetBagIgnored(0, val) end,
							},
							bag1 = {
								type = "toggle",
								name = "Bag 1 (First Bag)",
								desc = "Ignore personal bag slot 1.",
								order = 2,
								get = function() return MrPlow:IsBagIgnored(1) end,
								set = function(_, val) MrPlow:SetBagIgnored(1, val) end,
							},
							bag2 = {
								type = "toggle",
								name = "Bag 2 (Second Bag)",
								desc = "Ignore personal bag slot 2.",
								order = 3,
								get = function() return MrPlow:IsBagIgnored(2) end,
								set = function(_, val) MrPlow:SetBagIgnored(2, val) end,
							},
							bag3 = {
								type = "toggle",
								name = "Bag 3 (Third Bag)",
								desc = "Ignore personal bag slot 3.",
								order = 4,
								get = function() return MrPlow:IsBagIgnored(3) end,
								set = function(_, val) MrPlow:SetBagIgnored(3, val) end,
							},
							bag4 = {
								type = "toggle",
								name = "Bag 4 (Fourth Bag)",
								desc = "Ignore personal bag slot 4.",
								order = 5,
								get = function() return MrPlow:IsBagIgnored(4) end,
								set = function(_, val) MrPlow:SetBagIgnored(4, val) end,
							},
							ignoreAll = {
								type = "execute",
								name = "Ignore All Personal Bags",
								order = 10,
								func = function()
									for b = 0, 4 do MrPlow:SetBagIgnored(b, true) end
									MrPlow:Print("All personal bags are now ignored.")
								end,
							},
							unignoreAll = {
								type = "execute",
								name = "Unignore All Personal Bags",
								order = 11,
								func = function()
									for b = 0, 4 do MrPlow:SetBagIgnored(b, false) end
									MrPlow:Print("All personal bags are now unignored.")
								end,
							},
						},
					},
					bankBags = {
						type = "group",
						name = "Bank Bags (-1, 5 to 11)",
						inline = true,
						order = 20,
						args = {
							bankMain = {
								type = "toggle",
								name = "Main Bank (Bag -1)",
								desc = "Ignore the primary 28 bank slots.",
								order = 1,
								get = function() return MrPlow:IsBagIgnored(-1) end,
								set = function(_, val) MrPlow:SetBagIgnored(-1, val) end,
							},
							bank1 = {
								type = "toggle",
								name = "Bank Bag 1 (Bag 5)",
								desc = "Ignore bank bag slot 1.",
								order = 2,
								get = function() return MrPlow:IsBagIgnored(5) end,
								set = function(_, val) MrPlow:SetBagIgnored(5, val) end,
							},
							bank2 = {
								type = "toggle",
								name = "Bank Bag 2 (Bag 6)",
								desc = "Ignore bank bag slot 2.",
								order = 3,
								get = function() return MrPlow:IsBagIgnored(6) end,
								set = function(_, val) MrPlow:SetBagIgnored(6, val) end,
							},
							bank3 = {
								type = "toggle",
								name = "Bank Bag 3 (Bag 7)",
								desc = "Ignore bank bag slot 3.",
								order = 4,
								get = function() return MrPlow:IsBagIgnored(7) end,
								set = function(_, val) MrPlow:SetBagIgnored(7, val) end,
							},
							bank4 = {
								type = "toggle",
								name = "Bank Bag 4 (Bag 8)",
								desc = "Ignore bank bag slot 4.",
								order = 5,
								get = function() return MrPlow:IsBagIgnored(8) end,
								set = function(_, val) MrPlow:SetBagIgnored(8, val) end,
							},
							bank5 = {
								type = "toggle",
								name = "Bank Bag 5 (Bag 9)",
								desc = "Ignore bank bag slot 5.",
								order = 6,
								get = function() return MrPlow:IsBagIgnored(9) end,
								set = function(_, val) MrPlow:SetBagIgnored(9, val) end,
							},
							bank6 = {
								type = "toggle",
								name = "Bank Bag 6 (Bag 10)",
								desc = "Ignore bank bag slot 6.",
								order = 7,
								get = function() return MrPlow:IsBagIgnored(10) end,
								set = function(_, val) MrPlow:SetBagIgnored(10, val) end,
							},
							bank7 = {
								type = "toggle",
								name = "Bank Bag 7 (Bag 11)",
								desc = "Ignore bank bag slot 7.",
								order = 8,
								get = function() return MrPlow:IsBagIgnored(11) end,
								set = function(_, val) MrPlow:SetBagIgnored(11, val) end,
							},
							ignoreAllBank = {
								type = "execute",
								name = "Ignore All Bank Bags",
								order = 20,
								func = function()
									MrPlow:SetBagIgnored(-1, true)
									for b = 5, 11 do MrPlow:SetBagIgnored(b, true) end
									MrPlow:Print("All bank bags are now ignored.")
								end,
							},
							unignoreAllBank = {
								type = "execute",
								name = "Unignore All Bank Bags",
								order = 21,
								func = function()
									MrPlow:SetBagIgnored(-1, false)
									for b = 5, 11 do MrPlow:SetBagIgnored(b, false) end
									MrPlow:Print("All bank bags are now unignored.")
								end,
							},
						},
					},
				},
			},
			ignoreGuild = {
				type = "group",
				name = "Guild Bank Tabs",
				order = 30,
				args = {
					info = {
						type = "description",
						name = "Select which guild bank tabs (1 through 8) to ignore when sorting or defragmenting your guild bank.",
						fontSize = "medium",
						order = 1,
					},
					tabsGroup = {
						type = "group",
						name = "Guild Bank Tabs",
						inline = true,
						order = 2,
						args = {
							tab1 = {
								type = "toggle",
								name = function() return GetTabLabel(1) end,
								desc = "Ignore Guild Bank Tab 1.",
								order = 1,
								get = function() return MrPlow:IsGuildTabIgnored(1) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(1, val) end,
							},
							tab2 = {
								type = "toggle",
								name = function() return GetTabLabel(2) end,
								desc = "Ignore Guild Bank Tab 2.",
								order = 2,
								get = function() return MrPlow:IsGuildTabIgnored(2) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(2, val) end,
							},
							tab3 = {
								type = "toggle",
								name = function() return GetTabLabel(3) end,
								desc = "Ignore Guild Bank Tab 3.",
								order = 3,
								get = function() return MrPlow:IsGuildTabIgnored(3) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(3, val) end,
							},
							tab4 = {
								type = "toggle",
								name = function() return GetTabLabel(4) end,
								desc = "Ignore Guild Bank Tab 4.",
								order = 4,
								get = function() return MrPlow:IsGuildTabIgnored(4) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(4, val) end,
							},
							tab5 = {
								type = "toggle",
								name = function() return GetTabLabel(5) end,
								desc = "Ignore Guild Bank Tab 5.",
								order = 5,
								get = function() return MrPlow:IsGuildTabIgnored(5) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(5, val) end,
							},
							tab6 = {
								type = "toggle",
								name = function() return GetTabLabel(6) end,
								desc = "Ignore Guild Bank Tab 6.",
								order = 6,
								get = function() return MrPlow:IsGuildTabIgnored(6) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(6, val) end,
							},
							tab7 = {
								type = "toggle",
								name = function() return GetTabLabel(7) end,
								desc = "Ignore Guild Bank Tab 7.",
								order = 7,
								get = function() return MrPlow:IsGuildTabIgnored(7) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(7, val) end,
							},
							tab8 = {
								type = "toggle",
								name = function() return GetTabLabel(8) end,
								desc = "Ignore Guild Bank Tab 8.",
								order = 8,
								get = function() return MrPlow:IsGuildTabIgnored(8) end,
								set = function(_, val) MrPlow:SetGuildTabIgnored(8, val) end,
							},
							ignoreAllTabs = {
								type = "execute",
								name = "Ignore All Guild Tabs",
								order = 10,
								func = function()
									for t = 1, 8 do MrPlow:SetGuildTabIgnored(t, true) end
									MrPlow:Print("All guild bank tabs are now ignored.")
								end,
							},
							unignoreAllTabs = {
								type = "execute",
								name = "Unignore All Guild Tabs",
								order = 11,
								func = function()
									for t = 1, 8 do MrPlow:SetGuildTabIgnored(t, false) end
									MrPlow:Print("All guild bank tabs are now unignored.")
								end,
							},
						},
					},
				},
			},
			ignoreSlotsItems = {
				type = "group",
				name = "Slots & Items",
				order = 40,
				args = {
					interactive = {
						type = "group",
						name = "Interactive Mouse Picker",
						inline = true,
						order = 1,
						args = {
							desc = {
								type = "description",
								name = "Click the button below to activate the targeting cursor:\n• |cff00ff00Left-Click|r an item to ignore that item type anywhere in your bags.\n• |cff00ff00Right-Click|r a slot to ignore that exact bag and slot coordinate.\nPress |cffffcc00ESC|r or open Game Menu to cancel.",
								order = 1,
							},
							startPicker = {
								type = "execute",
								name = "Activate Mouse Picker",
								desc = "Click an item or slot with the targeting cursor.",
								order = 2,
								func = function() MrPlow:StartMouser() end,
							},
							highlightSlots = {
								type = "execute",
								name = "Toggle Ignored Slots Highlight",
								desc = "Highlights currently ignored slots in open bags with a red border.",
								order = 3,
								func = function() MrPlow:OverlayBags() end,
							},
						},
					},
					manualSlot = {
						type = "group",
						name = "Manual Slot Ignore",
						inline = true,
						order = 2,
						args = {
							bag = {
								type = "input",
								name = "Bag / Tab ID",
								desc = "Enter Bag ID (-1 for bank, 0-4 for bags, 5-11 for bank bags, 1-8 for guild tabs).",
								order = 1,
								get = function() return tostring(tempBagInput) end,
								set = function(_, val) tempBagInput = tonumber(val) or 0 end,
							},
							slot = {
								type = "input",
								name = "Slot ID",
								desc = "Enter Slot number (1 to bag size, or 1 to 98 for guild tab).",
								order = 2,
								get = function() return tostring(tempSlotInput) end,
								set = function(_, val) tempSlotInput = tonumber(val) or 1 end,
							},
							isGuild = {
								type = "toggle",
								name = "Is Guild Bank Slot",
								desc = "Check if this slot coordinate belongs to a Guild Bank Tab.",
								order = 3,
								get = function() return tempIsGuild end,
								set = function(_, val) tempIsGuild = val end,
							},
							ignoreBtn = {
								type = "execute",
								name = "Ignore Slot",
								desc = "Ignore the specified bag and slot.",
								order = 4,
								func = function()
									MrPlow:Ignore(tempBagInput, tempSlotInput, tempIsGuild)
									MrPlow:Print(("Ignored %s Bag %d, Slot %d"):format(tempIsGuild and "Guild" or "Personal/Bank", tempBagInput, tempSlotInput))
								end,
							},
							unignoreBtn = {
								type = "execute",
								name = "Unignore Slot",
								desc = "Remove ignore from the specified bag and slot.",
								order = 5,
								func = function()
									MrPlow:Unignore(tempBagInput, tempSlotInput, tempIsGuild)
									MrPlow:Print(("Unignored %s Bag %d, Slot %d"):format(tempIsGuild and "Guild" or "Personal/Bank", tempBagInput, tempSlotInput))
								end,
							},
						},
					},
					manualItem = {
						type = "group",
						name = "Manual Item Ignore",
						inline = true,
						order = 3,
						args = {
							item = {
								type = "input",
								name = "Item ID or Item Link",
								desc = "Enter numeric Item ID or paste an item link.",
								order = 1,
								get = function() return tempItemInput end,
								set = function(_, val)
									local id = tonumber(val) or tonumber(val:match("item:(%d+)"))
									tempItemInput = id and tostring(id) or val
								end,
							},
							ignoreItemBtn = {
								type = "execute",
								name = "Ignore Item",
								desc = "Ignore this item ID across all bags.",
								order = 2,
								func = function()
									local id = tonumber(tempItemInput) or tonumber(tempItemInput:match("item:(%d+)"))
									if id then
										if not db then db = MrPlow.db and MrPlow.db.profile end
										if not db.IgnoreItems then db.IgnoreItems = {} end
										db.IgnoreItems[id] = true
										local itemName = GetItemInfo(id)
										MrPlow:Print(("Item %d (%s) is now ignored."):format(id, itemName or "Unknown"))
									else
										MrPlow:Print("Invalid Item ID or link.")
									end
								end,
							},
							unignoreItemBtn = {
								type = "execute",
								name = "Unignore Item",
								desc = "Remove item ID from ignore list.",
								order = 3,
								func = function()
									local id = tonumber(tempItemInput) or tonumber(tempItemInput:match("item:(%d+)"))
									if id and db and db.IgnoreItems then
										db.IgnoreItems[id] = nil
										MrPlow:Print(("Item %d is no longer ignored."):format(id))
									else
										MrPlow:Print("Invalid Item ID.")
									end
								end,
							},
						},
					},
					reset = {
						type = "group",
						name = "Reset Ignores",
						inline = true,
						order = 4,
						args = {
							clearSlots = {
								type = "execute",
								name = "Clear All Ignored Slots",
								desc = "Removes all slot-specific ignores for bags and bank.",
								order = 1,
								confirm = true,
								confirmText = "Clear all ignored individual slot coordinates?",
								func = function()
									if not db then db = MrPlow.db and MrPlow.db.profile end
									if db then db.IgnoreSlots = {} end
									MrPlow:Print("All slot ignores have been cleared.")
								end,
							},
							clearItems = {
								type = "execute",
								name = "Clear All Ignored Items",
								desc = "Resets ignored item IDs back to default (Soul Shards).",
								order = 2,
								confirm = true,
								confirmText = "Reset all item ignores to default (Soul Shards)?",
								func = function()
									if not db then db = MrPlow.db and MrPlow.db.profile end
									if db then db.IgnoreItems = { [6265] = true } end
									MrPlow:Print("Ignored items reset to default.")
								end,
							},
							clearBagsTabs = {
								type = "execute",
								name = "Clear All Bag & Tab Ignores",
								desc = "Unignores all bags and guild bank tabs.",
								order = 3,
								confirm = true,
								confirmText = "Clear all whole bag and guild bank tab ignores?",
								func = function()
									if not db then db = MrPlow.db and MrPlow.db.profile end
									if db then
										db.IgnoreBags = {}
										db.IgnoreGuild = {}
									end
									MrPlow:Print("All whole bag and guild tab ignores cleared.")
								end,
							},
						},
					},
				},
			},
		},
	}
	return options
end

function MrPlow:BuildOptions()
	local options = self:GetOptionsTable()
	if AceConfig then
		AceConfig:RegisterOptionsTable("MrPlow", options)
	end
	if AceConfigDialog then
		AceConfigDialog:AddToBlizOptions("MrPlow", "Mr Plow")
		if AceDBOptions and self.db then
			local profileOptions = AceDBOptions:GetOptionsTable(self.db)
			AceConfig:RegisterOptionsTable("MrPlow_Profiles", profileOptions)
			AceConfigDialog:AddToBlizOptions("MrPlow_Profiles", "Profiles", "Mr Plow")
		end
	end
end

function MrPlow:OpenOptions()
	if AceConfigDialog then
		AceConfigDialog:Open("MrPlow")
	else
		InterfaceOptionsFrame_OpenToCategory("Mr Plow")
		InterfaceOptionsFrame_OpenToCategory("Mr Plow")
	end
end
