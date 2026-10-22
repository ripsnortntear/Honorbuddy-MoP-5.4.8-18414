-- Title: iLvLr
-- Author: JerichoHM / LownIgnitus
-- Version: 2.1.101
-- Desc: iLvL identifier

--Version Information
local iLvLr = {}
local addon = iLvLr
local Title = "iLvLr"
local Core = "2"
local Revision = "1"
local Build = "102"
SLASH_ILVLR1 = '/ilvlr'

local frameDB = {CharacterHeadSlot,
					CharacterNeckSlot,
					CharacterShoulderSlot,
					CharacterBackSlot,
					CharacterChestSlot,
					CharacterWristSlot,
					CharacterShirtSlot,
					CharacterTabardSlot,
					CharacterMainHandSlot,
					CharacterSecondaryHandSlot,
					CharacterHandsSlot,
					CharacterWaistSlot,
					CharacterLegsSlot,
					CharacterFeetSlot,
					CharacterFinger0Slot,
					CharacterFinger1Slot,
					CharacterTrinket0Slot,
					CharacterTrinket1Slot
				}

local slotDB = {"HeadSlot",
					"NeckSlot",
					"ShoulderSlot",
					"BackSlot",
					"ChestSlot",
					"WristSlot",
					"ShirtSlot",
					"TabardSlot",
					"MainHandSlot",
					"SecondaryHandSlot",
					"HandsSlot",
					"WaistSlot",
					"LegsSlot",
					"FeetSlot",
					"Finger0Slot",
					"Finger1Slot",
					"Trinket0Slot",
					"Trinket1Slot"
				}

local isEnchantable = {"ShoulderSlot",
					"BackSlot",
					"ChestSlot",
					"MainHandSlot",
					"SecondaryHandSlot",
					"LegsSlot",
					"FeetSlot"
					}
					
local iEqAvg, iAvg
local iLvlFrames = {}
local iQualityFrames = {}
local iDuraFrames = {}
local iModFrames = {}

addon.f = CreateFrame("Frame", Title.."main", CharacterFrame)
addon.f:SetScript("OnShow", function(self)
		--print("ILvLrOnLoad call @showpaperdoll")
		iLvLrOnLoad()
end)
				
function iLvLrMain()
	iLvLrFrame = CreateFrame("Frame", "iLvLrFrame", UIParent)
	iLvLrFrame:RegisterEvent("UPDATE_INVENTORY_DURABILITY")
	iLvLrFrame:RegisterEvent("PLAYER_EQUIPMENT_CHANGED")
	iLvLrFrame:RegisterEvent("SOCKET_INFO_UPDATE")
	iLvLrFrame:SetScript("OnEvent", iLvLrOnEvent)

end

function SlashCmdList.ILVLR(msg)
	if msg == "dtoggle" then
		if iDuraState == "enabled" then
			iDuraState = "disabled"
			iDuraToggle(iDuraState)
		elseif iDuraState == "disabled" then
			iDuraState = "enabled"
			iDuraToggle(iDuraState)
		end
	else
	print("Thank you for using " .. Title)
	print("Author: JerichoHM / LownIgnitus")
	print("Version: " .. Core .. "." .. Revision .. "." .. Build)
		print("Slash Commands are listed below and start with /iLvLr")
		print("      dtoggle - Disables or Enables the durability tracker")
	end
end

function iLvLrOnEvent(self, event, what)
	if event == "PLAYER_EQUIPMENT_CHANGED" or event == "SOCKET_INFO_UPDATE" then
		--print("Inventory/Equip/Socket Changed")
		iLvLrOnItemUpdate()
		iLvLrOnModUpdate()
	elseif event == "UPDATE_INVENTORY_DURABILITY" then
		--print("Durability Update")
		iLvLrOnDuraUpdate()
	end

end

function iLvLrOnLoad()
	--print("in OnLoad")
	for k ,v in pairs(slotDB) do
		local iLevel = fetchIlvl(v)
		if iLevel then
			makeQuality(frameDB[k], v)
			if v == "ShirtSlot" or v == "TabardSlot" then
				-- Do Nothing
			else
				makeIlvl(frameDB[k], v)
				makeDurability(frameDB[k], v)
				makeMod(frameDB[k], v)
			end
		end
	end
	if not iDuraState then
		iDuraState = "enabled"
	end

	if iDuraState == "disabled" then
		iDuraToggle(iDuraState)
	end
end

function iLvLrOnItemUpdate()
	--print("in OnItemUpdate")
	for k ,v in pairs(slotDB) do
		local iLevel = fetchIlvl(v)
		if iLevel then
			makeQuality(frameDB[k], v)
			if v == "ShirtSlot" or v == "TabardSlot" then
				-- Do Nothing
			else
				makeIlvl(frameDB[k], v)
				makeDurability(frameDB[k], v)
				makeMod(frameDB[k], v)
			end
		else
			if iLvlFrames[v] then
				iLvlFrames[v]:Hide()
			end
			if iQualityFrames[v] then
				iQualityFrames[v]:Hide()
			end
			if iDuraFrames[v] then
				iDuraFrames[v]:Hide()
			end
			if iModFrames[v] then
				iModFrames[v]:Hide()
			end
		end
	end
end

function iLvLrOnDuraUpdate()
	--print("in OnDuraUpdate")
	for k ,v in pairs(slotDB) do
		local iLevel = fetchIlvl(v)
		if iLevel then
			makeDurability(frameDB[k], v)
		else
			if iDuraFrames[v] then
				iDuraFrames[v]:Hide()
			end
		end
	end
end

function iLvLrOnModUpdate()
	for k ,v in pairs(slotDB) do
		local iLevel = fetchIlvl(v)
		if iLevel then
			if v == "ShirtSlot" or v == "TabardSlot" then
				-- Do Nothing
			else
				makeMod(frameDB[k], v)
			end
		else
			if iModFrames[v] then
				iModFrames[v]:Hide()
			end
		end
	end
end

function fetchIlvl(slotName)
	--print("in fetchIlvl")
	local slotId, texture = GetInventorySlotInfo(slotName)
	local itemId = GetInventoryItemID("player", slotId)
	if itemId then
		local itemLinks = GetInventoryItemLink("player",GetInventorySlotInfo(slotName))
		local levelAdjust={ --id field and level adjustment
		["0"]=0,["1"]=8,["373"]=4,["374"]=8,["375"]=4,["376"]=4,["377"]=4,["379"]=4,["380"]=4,
		["445"]=0,["446"]=4,["447"]=8,["451"]=0,["452"]=8,["453"]=0,["454"]=4,["455"]=8,
		["456"]=0,["457"]=8,["458"]=0,["459"]=4,["460"]=8,["461"]=12,["462"]=16,
		["465"]=0,["466"]=4,["467"]=8,["468"] = 0,["469"] = 4,["470"] = 8,["471"] = 12,["472"] = 16,
		["491"]=0,["492"]=4,["493"]=8,["494"]=4,["495"]=8,["496"]=8,["497"]=12,["498"]=16, ["504"]=12,
		["505"]=16, ["506"]=20, ["507"]=24,
		}
		local baseLevel = select(4,GetItemInfo(itemLinks))
		local upgrade = itemLinks:match(":(%d+)\124h%[")
		if baseLevel and upgrade and levelAdjust[upgrade] then
			return baseLevel + levelAdjust[upgrade]
		else
			return baseLevel
		end
		
	end
end

function fetchQuality(slotName)
	--print("in fetchQuality")
	local slotId, texture, checkRelic = GetInventorySlotInfo(slotName)
	local itemId = GetInventoryItemID("player", slotId)
	if itemId then
		local name, link, quality, iLevel, reqLevel, class, subclass, maxStack, equipSlot, texture, vendorPrice = GetItemInfo(itemId)
		local r, g, b, hex = GetItemQualityColor(quality)
		return r, g, b
	end
end

function fetchDura(slotName)
	--print("in fetchDura")
	local slotId, texture = GetInventorySlotInfo(slotName)
	if slotId then
		local itemDurability, itemMaxDurability = GetInventoryItemDurability(slotId)
		if itemDurability and itemMaxDurability then
			return itemDurability, itemMaxDurability
		else
			return -1, -1
		end
	end
end

function fetchSocketCount(slotName)
	local inventoryID = GetInventorySlotInfo(slotName)
	local itemLink = GetInventoryItemLink("player", inventoryID)
	local socketCount = 0
	for i = 1, 4 do
		if  _G["iLvLrScannerTexture" .. i]  then
	 		_G["iLvLrScannerTexture" .. i]:SetTexture("")
	 	end
	end
	
	if not iLvLrScanner then CreateFrame("GameToolTip", "iLvLrScanner", UIParent, "GameTooltipTemplate") end
	local ttScanner = iLvLrScanner
	
	ttScanner:SetOwner(iLvLrFrame, "ANCHOR_NONE")
	ttScanner:ClearLines()
	ttScanner:SetHyperlink(itemLink)
	
	for i = 1, 4 do
		local texture = _G["iLvLrScannerTexture" .. i]:GetTexture()
		if texture then
			socketCount = socketCount + 1
		end
	end
	
	ttScanner:Hide()
	
	return socketCount
end

function fetchGem(slotName)
	local inventoryID = GetInventorySlotInfo(slotName)
	local itemLink = GetInventoryItemLink("player", inventoryID)
	
	local missingGems = 0
							
	local emptyTextures = {"Interface\\ItemSocketingFrame\\UI-EmptySocket-Meta", 
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-Red",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-Yellow",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-Blue",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-CogWheel",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-Hydraulic",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket-Prismatic",
							"Interface\\ItemSocketingFrame\\UI-EmptySocket"
							}
	
	for i = 1, 4 do
		if ( _G["iLvLrScannerTexture" .. i] ) then
	 		_G["iLvLrScannerTexture" .. i]:SetTexture("");
	 	end;
	end;
	
	if not iLvLrScanner then CreateFrame("GameToolTip", "iLvLrScanner", UIParent, "GameTooltipTemplate") end
	local ttScanner = iLvLrScanner
	
	ttScanner:SetOwner(iLvLrFrame, "ANCHOR_NONE")
	ttScanner:ClearLines()
	ttScanner:SetHyperlink(itemLink)
	
	for i = 1, 4 do
		local texture = _G["iLvLrScannerTexture" .. i]:GetTexture()
		if texture then
			for k, v in pairs(emptyTextures) do
				if texture == v then
					missingGems = missingGems + 1
				end
			end
		end
	end
	
	ttScanner:Hide()
	
	return missingGems
end

function fetchBaseSocket(slotName)
	local inventoryID = GetInventorySlotInfo(slotName)
	local itemLink = GetInventoryItemLink("player", inventoryID)
	
	local parsedItemDataTable = {}
	local foundStart, foundEnd, parsedItemData = string.find(itemLink, "^|c%x+|H(.+)|h%[.*%]")
	
	for v in string.gmatch(parsedItemData, "[^:]+") do
		tinsert(parsedItemDataTable, v)
	end
	
	local baseItem = "|Hitem:" .. parsedItemDataTable[2] .. ":0"
	local itemName, itemLink, itemQuality, itemLevel, itemReqLevel, itemClass, itemSubclass, itemMaxStack, itemEquipSlot, itemTexture, itemVendorPrice = GetItemInfo(baseItem)
	local baseSocketCount = 0
	for i = 1, 4 do
		if  _G["iLvLrScannerTexture" .. i]  then
	 		_G["iLvLrScannerTexture" .. i]:SetTexture("")
	 	end
	end
	
	if not iLvLrScanner then CreateFrame("GameToolTip", "iLvLrScanner", UIParent, "GameTooltipTemplate") end
	local ttScanner = iLvLrScanner
	
	ttScanner:SetOwner(iLvLrFrame, "ANCHOR_NONE")
	ttScanner:ClearLines()
	ttScanner:SetHyperlink(itemLink)
	
	for i = 1, 4 do
		local texture = _G["iLvLrScannerTexture" .. i]:GetTexture()
		if texture then
			baseSocketCount = baseSocketCount + 1
		end
	end
	
	ttScanner:Hide()
	
	return baseSocketCount
end

function fetchChant(slotName)
	local inventoryID = GetInventorySlotInfo(slotName)
	local itemLink = GetInventoryItemLink("player", inventoryID)
	local parsedItemDataTable = {}
	local foundStart, foundEnd, parsedItemData = string.find(itemLink, "^|c%x+|H(.+)|h%[.*%]")
	for v in string.gmatch(parsedItemData, "[^:]+") do
		tinsert(parsedItemDataTable, v)
	end
	
	return parsedItemDataTable[3]
end

function fetchProfs()
	local prof1, prof2, archaeology, fishing, cooking, firstAid = GetProfessions()
	local profs = {prof1, prof2, archaeology, fishing, cooking, firstAid}
	local profNames = {}
	
	for k, v in pairs(profs) do
		local name, texture, rank, maxRank, numSpells, spelloffset, skillLine, rankModifier = GetProfessionInfo(v)
		tinsert(profNames, name)
	end
	
	return profNames
end

function fetchSubclass(slotName)
	local slotId, texture, checkRelic = GetInventorySlotInfo(slotName)
	local itemId = GetInventoryItemID("player", slotId)
	if itemId then
		local name, link, quality, iLevel, reqLevel, class, subclass, maxStack, equipSlot, texture, vendorPrice = GetItemInfo(itemId)
		return(subclass)
	end
end

function makeIlvl(frame, slot)
	--print("in makeText")
	iAvg, iEqAvg = GetAverageItemLevel()
	local iLvl = iLvlFrames[slot]
	if not iLvl then
		iLvl = CreateFrame("Frame", nil, frame)
		if frame == CharacterHeadSlot or frame == CharacterNeckSlot or frame == CharacterShoulderSlot or frame == CharacterBackSlot or frame == CharacterChestSlot or frame == CharacterWristSlot or frame == CharacterShirtSlot or frame == CharacterTabardSlot then
			iLvl:SetPoint("CENTER", frame, "CENTER", 42, -1)
		elseif frame == CharacterHandsSlot or frame == CharacterWaistSlot or frame == CharacterLegsSlot or frame == CharacterFeetSlot or frame == CharacterFinger0Slot or frame == CharacterFinger1Slot or frame == CharacterTrinket0Slot or frame == CharacterTrinket1Slot then
			iLvl:SetPoint("CENTER", frame, "CENTER", -42, -1)
		elseif frame == CharacterMainHandSlot or frame == CharacterSecondaryHandSlot or frame == CharacterRangedSlot then
			iLvl:SetPoint("CENTER", frame, "CENTER", 0, 41)
		end
		
		iLvl:SetSize(10,10)
		iLvl:SetBackdrop({bgFile = nil, edgeFile = nil, tile = false, tileSize = 32, edgeSize = 0, insets = {left = 0, right = 0, top = 0, bottom = 0}})
		iLvl:SetBackdropColor(0,0,0,0)
		
		local iLvlText = iLvl:CreateFontString(nil, "ARTWORK")
		isValid = iLvlText:SetFont("Fonts\\FRIZQT__.TTF", 10, "OUTLINE")
		iLvlText:SetPoint("CENTER", iLvl, "CENTER", 0, 0)
		iLvl.text = iLvlText
		
		local iLevel = fetchIlvl(slot)
		if iLevel <= iEqAvg - 10 then
			iLvl.text:SetFormattedText("|cffff0000%i|r", iLevel)
		elseif iLevel >= iEqAvg + 10 then
			iLvl.text:SetFormattedText("|cff00ff00%i|r", iLevel)
		else
			iLvl.text:SetFormattedText("|cffffffff%i|r", iLevel)
		end
		iLvlFrames[slot] = iLvl
	else
		local iLevel = fetchIlvl(slot)
		if iLevel <= iEqAvg - 10 then
			iLvl.text:SetFormattedText("|cffff0000%i|r", iLevel)
		elseif iLevel >= iEqAvg + 10 then
			iLvl.text:SetFormattedText("|cff00ff00%i|r", iLevel)
		else
			iLvl.text:SetFormattedText("|cffffffff%i|r", iLevel)
		end
		
		iLvlFrames[slot] = iLvl
	end
	iLvl:Show()
end

function makeQuality(frame, slot)
	--print("in makeQuality")
	local iQuality = iQualityFrames[slot]
	if not iQuality then
		iQuality = CreateFrame("Frame", nil, frame)
		iQuality:SetPoint("CENTER", frame, "CENTER", 0, 0)
		iQuality:SetSize(frame:GetWidth(), frame:GetHeight())
		iQuality:SetBackdrop({bgFile = nil, edgeFile = "Interface\\Tooltips\\UI-Tooltip-Border.blp", tile = true, tileSize = 32, edgeSize = 16, insets = {left = 0, right = 0, top = 0, bottom = 0}})
		
		local r, g, b = fetchQuality(slot)
		iQuality:SetBackdropBorderColor(r, g, b, 1.0)
		
		iQualityFrames[slot] = iQuality
	else
		local r, g, b = fetchQuality(slot)
		iQuality:SetBackdropBorderColor(r, g, b, 1.0)
	end
	
	iQuality:Show()
end

function makeDurability(frame, slot)
	--print("in makeDurability")
	local iDura = iDuraFrames[slot]
	if not iDura then
		iDura = CreateFrame("Frame", nil, frame)
		
		if frame == CharacterHeadSlot or frame == CharacterNeckSlot or frame == CharacterShoulderSlot or frame == CharacterBackSlot or frame == CharacterChestSlot or frame == CharacterWristSlot or frame == CharacterShirtSlot or frame == CharacterTabardSlot then
			iDura:SetPoint("BOTTOM", frame, "BOTTOM", 42, 0)
		elseif frame == CharacterHandsSlot or frame == CharacterWaistSlot or frame == CharacterLegsSlot or frame == CharacterFeetSlot or frame == CharacterFinger0Slot or frame == CharacterFinger1Slot or frame == CharacterTrinket0Slot or frame == CharacterTrinket1Slot then
			iDura:SetPoint("BOTTOM", frame, "BOTTOM", -42, 0)
		elseif frame == CharacterMainHandSlot or frame == CharacterSecondaryHandSlot or frame == CharacterRangedSlot then
			iDura:SetPoint("BOTTOM", frame, "BOTTOM", 0, 42)
		end
		
		iDura:SetSize(10,10)
		iDura:SetBackdrop({bgFile = nil, edgeFile = nil, tile = false, tileSize = 32, edgeSize = 0, insets = {left = 0, right = 0, top = 0, bottom = 0}})
		iDura:SetBackdropColor(0,0,0,0)
		
		local iDuraText = iDura:CreateFontString(nil, "ARTWORK")
		isValid = iDuraText:SetFont("Fonts\\FRIZQT__.TTF", 10, "OUTLINE")
		iDuraText:SetPoint("CENTER", iDura, "CENTER", 0, 0)
		iDura.text = iDuraText
		itemDurability, itemMaxDurability = fetchDura(slot)
		if itemDurability == -1 and itemMaxDurability == -1 then
			iDura.text:SetFormattedText("|cffffffff|r")
		else
			if itemDurability then
				local itemDurabilityPercentage = (itemDurability / itemMaxDurability) * 100
				if itemDurabilityPercentage > 25 then
					iDura.text:SetFormattedText("|cff00ff00%i%%|r", itemDurabilityPercentage)
					elseif itemDurabilityPercentage > 0 and itemDurabilityPercentage <= 25 then
					iDura.text:SetFormattedText("|cff00ffff%i%%|r", itemDurabilityPercentage)
				elseif itemDurabilityPercentage == 0 then
					iDura.text:SetFormattedText("|cffff0000%i%%|r", itemDurabilityPercentage)
				end
			else
				iDura.text:SetFormattedText("")
			end
			iDuraFrames[slot] = iDura
		end
	else
		itemDurability, itemMaxDurability = fetchDura(slot)
		if itemDurability == -1 and itemMaxDurability == -1 then
			iDura.text:SetFormattedText("|cffffffff|r")
		else
			if itemDurability then
				local itemDurabilityPercentage = (itemDurability / itemMaxDurability) * 100
				if itemDurabilityPercentage > 25 then
					iDura.text:SetFormattedText("|cff00ff00%i%%|r", itemDurabilityPercentage)
				elseif itemDurabilityPercentage > 0 and itemDurabilityPercentage <= 25 then
					iDura.text:SetFormattedText("|cff00ffff%i%%|r", itemDurabilityPercentage)
				elseif itemDurabilityPercentage == 0 then
					iDura.text:SetFormattedText("|cffff0000%i%%|r", itemDurabilityPercentage)
				end
			else
				iDura.text:SetFormattedText("")
			end
			iDuraFrames[slot] = iDura
		end
	end
        if iDuraState == "enabled" then
        	iDura:Show()
        end
end

function makeMod(frame, slot)
	--print("in makeMod")
	local missingGem, numSockets, isEnchanted, canEnchant
	local iMod = iModFrames[slot]
	if not iMod then
		iMod = CreateFrame("Frame", nil, frame)
		
		if frame == CharacterHeadSlot or frame == CharacterNeckSlot or frame == CharacterShoulderSlot or frame == CharacterBackSlot or frame == CharacterChestSlot or frame == CharacterWristSlot or frame == CharacterShirtSlot or frame == CharacterTabardSlot then
			iMod:SetPoint("TOP", frame, "TOP", 42, -3)
		elseif frame == CharacterHandsSlot or frame == CharacterWaistSlot or frame == CharacterLegsSlot or frame == CharacterFeetSlot or frame == CharacterFinger0Slot or frame == CharacterFinger1Slot or frame == CharacterTrinket0Slot or frame == CharacterTrinket1Slot then
			iMod:SetPoint("TOP", frame, "TOP", -42, -3)
		elseif frame == CharacterMainHandSlot or frame == CharacterSecondaryHandSlot or frame == CharacterRangedSlot then
			iMod:SetPoint("TOP", frame, "TOP", 0, 39)
		end
		
		iMod:SetSize(10,10)
		iMod:SetBackdrop({bgFile = nil, edgeFile = nil, tile = false, tileSize = 32, edgeSize = 0, insets = {left = 0, right = 0, top = 0, bottom = 0}})
		iMod:SetBackdropColor(0,0,0,0)
		
		local iModText = iMod:CreateFontString(nil, "ARTWORK")
		isValid = iModText:SetFont("Fonts\\FRIZQT__.TTF", 10, "OUTLINE")
		iModText:SetPoint("CENTER", iMod, "CENTER", 0, 0)
		iMod.text = iModText
		
		missingGem = fetchGem(slot)
		numSockets = fetchSocketCount(slot)
		canEnchant = false
		missingSpecial = 0
		
		if slot == "Finger0Slot" or slot == "Finger1Slot" then
			local profNames = fetchProfs()
			for k, v in pairs(profNames) do
				if v == "Enchanting" then
					canEnchant = true
					isEnchanted = fetchChant(slot)
				end
			end
		elseif slot == "RangedSlot" then
			local subClass = fetchSubclass(slot)
			if subClass == "Bows" or subClass == "Guns" or subClass == "Crossbows" then
				canEnchant = true
				isEnchanted = fetchChant(slot)
			end
		elseif slot == "WristSlot" or slot == "HandsSlot" then
			local isBS = false
			canEnchant = true
			local profNames = fetchProfs()
			for k, v in pairs(profNames) do
				if v == "Blacksmithing" then
					isBS = true
					local baseSockets = fetchBaseSocket(slot)
					if (baseSockets - numSockets) == -1 then
						missingSpecial = 1
					else
						missingSpecial = 0
					end
				end
			end
			isEnchanted = fetchChant(slot)
			
			if isBs and missingSpecial == 0 then
				isEnchanted = 0
			end
		elseif slot == "WaistSlot" then
			canEnchant = true
			local baseSockets = fetchBaseSocket(slot)
			if (baseSockets - numSockets) == -1 then
				isEnchanted = 1
			else
				isEnchanted = 0
			end
		else
			for k ,v in pairs(isEnchantable) do
				if v == slot then
					canEnchant = true
					isEnchanted = fetchChant(slot)
				end
			end
		end
		
		isEnchanted = tonumber(isEnchanted)
		
		if numSockets > 0 and canEnchant == true then
			if isEnchanted == 0 and missingGem > 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r|cffff0000%s|r", "E", "G")
			elseif isEnchanted == 0 and missingGem == 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r|cff00ff00%s|r", "E", "G")
			elseif isEnchanted > 0 and missingGem > 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r|cffff0000%s|r", "E", "G")
			elseif isEnchanted > 0 and missingGem == 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r|cff00ff00%s|r", "E", "G")
			end
		elseif numSockets > 0 and canEnchant == false then
			if missingGem > 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r", "G")
			elseif missingGem == 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r", "G")
			end
		elseif numSockets == 0 and canEnchant == true then
			if isEnchanted == 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r", "E")
			elseif isEnchanted > 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r", "E")
			end
		elseif numSockets == 0 and canEnchant == false then
			iMod.text:SetFormattedText("")
		end
		
		iModFrames[slot] = iMod
		
	else
	
		missingGem = fetchGem(slot)
		numSockets = fetchSocketCount(slot)
		canEnchant = false
		missingSpecial = 0
		
		if slot == "Finger0Slot" or slot == "Finger1Slot" then
			local profNames = fetchProfs()
			for k, v in pairs(profNames) do
				if v == "Enchanting" then
					canEnchant = true
					isEnchanted = fetchChant(slot)
				end
			end
		elseif slot == "RangedSlot" then
			local subClass = fetchSubclass(slot)
			if subClass == "Bows" or subClass == "Guns" or subClass == "Crossbows" then
				canEnchant = true
				isEnchanted = fetchChant(slot)
			end
		elseif slot == "WristSlot" or slot == "HandsSlot" then
			local isBS = false
			canEnchant = true
			local profNames = fetchProfs()
			for k, v in pairs(profNames) do
				if v == "Blacksmithing" then
					isBS = true
					local baseSockets = fetchBaseSocket(slot)
					if (baseSockets - numSockets) == -1 then
						missingSpecial = 1
					else
						missingSpecial = 0
					end
				end
			end
			isEnchanted = fetchChant(slot)
			
			if isBs and missingSpecial == 0 then
				isEnchanted = 0
			end
		elseif slot == "WaistSlot" then
			canEnchant = true
			local baseSockets = fetchBaseSocket(slot)
			if (baseSockets - numSockets) == -1 then
				isEnchanted = 1
			else
				isEnchanted = 0
			end
		else
			for k ,v in pairs(isEnchantable) do
				if v == slot then
					canEnchant = true
					isEnchanted = fetchChant(slot)
				end
			end
		end
		isEnchanted = tonumber(isEnchanted)
		if numSockets > 0 and canEnchant == true then
			if isEnchanted == 0 and missingGem > 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r|cffff0000%s|r", "E", "G")
			elseif isEnchanted == 0 and missingGem == 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r|cff00ff00%s|r", "E", "G")
			elseif isEnchanted > 0 and missingGem > 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r|cffff0000%s|r", "E", "G")
			elseif isEnchanted > 0 and missingGem == 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r|cff00ff00%s|r", "E", "G")
			end
		elseif numSockets > 0 and canEnchant == false then
			if missingGem > 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r", "G")
			elseif missingGem == 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r", "G")
			end
		elseif numSockets == 0 and canEnchant == true then
		if isEnchanted == 0 then
				iMod.text:SetFormattedText("|cffff0000%s|r", "E")
			elseif isEnchanted > 0 then
				iMod.text:SetFormattedText("|cff00ff00%s|r", "E")
			end
		elseif numSockets == 0 and canEnchant == false then
			iMod.text:SetFormattedText("")
		end
		
		iModFrames[slot] = iMod
	end
	
	iMod:Show()
end

function iDuraToggle(state)
	for k, v in pairs(iDuraFrames) do
		if state == "enabled" then
			v:Show()
		elseif state == "disabled" then
			v:Hide()
		end
	end
end
iLvLrMain()