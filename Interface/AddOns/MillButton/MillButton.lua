--[[ For normal milling w/o information for skipped "herbs" the macro should be: /run MillButtonSetup() /click MillButton ]]
--[[ For normal milling w/ information for skipped "herbs" the macro should be: /run MillButtonSetup2() /click MillButton ]]

local L = LibStub("AceLocale-3.0"):GetLocale("MillButton")
local mill = CreateFrame("CheckButton","MillButton",UIParent,"SecureActionButtonTemplate")
mill:SetAttribute("type","macro")
local _, _, _, _, _, item_herb = GetAuctionItemSubClasses(6) --Simple way to get ingame localized ItemType "Herb" through Auctionshouseframe translation

local function findherb()
	local function f(b,s)
		if GetContainerItemLink(b,s) then
			local itemName,itemLink,_,_,_,_,itemType = GetItemInfo(GetContainerItemID(b,s))
			local itemID = GetContainerItemID(b,s)
			local db = MillButton_Herbs.NotMillable
				--search internal db for ignored herb ids and skip them
			for h=1,#db do
				local herb = db[h]
				if itemID==herb then
					if extended_millbutton==true then
						DEFAULT_CHAT_FRAME:AddMessage("|cFF00FF00MillButton: |r"..itemLink.." "..L["skipped"])
					end
					return
				end
			end
			return (itemType==item_herb or itemID==MillButton_Herbs.Millable) and select(2,GetContainerItemInfo(b,s))>=5
		end
	end
	for i=0,4 do
		for j=1,GetContainerNumSlots(i) do
			if f(i,j) then
				return i,j
			end
		end
	end
end

function MillButtonSetup()
extended_millbutton = false
local mill_spell = GetSpellInfo(51005) --SpellID for Milling
	local b,s = findherb()
	if (not b or not s) or LootFrame:IsVisible() or CastingBarFrame:IsVisible() or UnitCastingInfo("player") then
	-- do nothing if no herb, if looting or casting
		MillButton:SetAttribute("macrotext","")
		if not b then
			DEFAULT_CHAT_FRAME:AddMessage("|cFF00FF00MillButton: |r"..L["error"])
		end
	else
		MillButton:SetAttribute("macrotext","/"..L["macro_cast"].." "..mill_spell.."\n/"..L["macro_use"].." "..b.." "..s)
	end
end

function MillButtonSetup2()
extended_millbutton = true
local mill_spell = GetSpellInfo(51005) --SpellID for Milling
	local b,s = findherb()
	if (not b or not s) or LootFrame:IsVisible() or CastingBarFrame:IsVisible() or UnitCastingInfo("player") then
	-- do nothing if no herb, if looting or casting
		MillButton:SetAttribute("macrotext","")
		if not b then
			DEFAULT_CHAT_FRAME:AddMessage("|cFF00FF00MillButton: |r"..L["error"])
		end
	else
		MillButton:SetAttribute("macrotext","/"..L["macro_cast"].." "..mill_spell.."\n/"..L["macro_use"].." "..b.." "..s)
	end
end

--[[tbd flow
function MillButtonStack()
	
end]]--