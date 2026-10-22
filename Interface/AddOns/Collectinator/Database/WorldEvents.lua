--[[
************************************************************************
Seasonal.lua
************************************************************************
File date: 2013-03-09T18:32:14Z
File hash: 2547f1c
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/arl/ for more information.
************************************************************************
License:
	Please see LICENSE.txt
This source code is released under All Rights Reserved.
************************************************************************
]] --

-----------------------------------------------------------------------
-- Upvalued Lua API.
-----------------------------------------------------------------------
local _G = getfenv(0)

-----------------------------------------------------------------------
-- AddOn namespace.
-----------------------------------------------------------------------
local FOLDER_NAME, private = ...

local LibStub = _G.LibStub

local addon = LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L = LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

private.world_events_list = {}

function addon:InitWorldEvents()
	local function AddWorldEvent(identifier, name)
		private:AddListEntry(private.world_events_list, identifier, name, _G.GetCategoryInfo(155))
	end

	AddWorldEvent("DAY_OF_THE_DEAD", L["Day of the Dead"])
	AddWorldEvent("FEAST_OF_WINTER_VEIL", _G.GetCategoryInfo(156))
	AddWorldEvent("HALLOWS_END", _G.GetCategoryInfo(158))
	AddWorldEvent("LUNAR_FESTIVAL", _G.GetCategoryInfo(160))
	AddWorldEvent("MIDSUMMER_FIRE_FESTIVAL", _G.GetCategoryInfo(161))
	AddWorldEvent("BREWFEST", _G.GetCategoryInfo(162))
	AddWorldEvent("CHILDRENS_WEEK", _G.GetCategoryInfo(163))
	AddWorldEvent("NOBLEGARDEN",_G.GetCategoryInfo(181))
	AddWorldEvent("LOVE_IS_IN_THE_AIR", _G.GetCategoryInfo(187))
	AddWorldEvent("PILGRIMS_BOUNTY", _G.GetCategoryInfo(14981))
	AddWorldEvent("DARKMOON_FAIRE", _G.GetCategoryInfo(15101))
	AddWorldEvent("WANDERERS_FESTIVAL", L["Wanderer's Festival"])

	self.InitWorldEvents = nil
end
