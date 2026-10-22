--[[
************************************************************************
Custom.lua
************************************************************************
File date: 2014-06-12T00:49:20Z
File hash: e052339
Project hash: 7dcdac2
Project version: 2.0.11-1-g7dcdac2
************************************************************************
Please see http://www.wowace.com/addons/arl/ for more information.
************************************************************************
This source code is released under All Rights Reserved.
************************************************************************
]]--

-----------------------------------------------------------------------
-- Upvalued Lua API.
-----------------------------------------------------------------------
local _G = getfenv(0)

-----------------------------------------------------------------------
-- AddOn namespace.
-----------------------------------------------------------------------
local FOLDER_NAME, private	= ...

local LibStub = _G.LibStub

local addon		= LibStub("AceAddon-3.0"):GetAddon(private.addon_name)
local L			= LibStub("AceLocale-3.0"):GetLocale(private.addon_name)

local Z			= private.ZONE_NAMES

private.custom_list	= {}

function addon:InitCustom()
	local function AddCustom(identifier, zone_name, coord_x, coord_y, faction)
		private:AddListEntry(private.custom_list, identifier, L[identifier], zone_name, coord_x, coord_y, nil)
	end

	AddCustom("CHAMPION_SEALS",	Z.ICECROWN)
	AddCustom("SOR")
	AddCustom("RECRUIT")
	AddCustom("MYSTERIOUS_EGG",	Z.SHOLAZAR_BASIN,		54.6,	56.2)
	AddCustom("TCG",		Z.THE_CAPE_OF_STRANGLETHORN,	42.6,	71.6)
	AddCustom("STORE")
	AddCustom("PLUSHIE")
	AddCustom("WEBSTORE")
	AddCustom("ARENA")
	AddCustom("ANNUAL_PASS")
	AddCustom("CITY")
	AddCustom("NOBLE_VENDOR")
	AddCustom("CE")
	AddCustom("BLIZZCON")
	AddCustom("STARCRAFTCE")
	AddCustom("DIABLOCE")
	AddCustom("AUTH")
	AddCustom("WWI")
	AddCustom("CHINA")
	AddCustom("KOREA")
	AddCustom("BATTLEMERGE")
	AddCustom("TAKKSNEST",		Z.NORTHERN_BARRENS)
	AddCustom("RAZORMAWNEST",	Z.WETLANDS,			69.9,	29.1)
	AddCustom("DARTSNEST",		Z.DUSTWALLOW_MARSH,		48.0,	14.5)
	AddCustom("MATRIARCHNEST",	Z.UNGORO_CRATER,		62.0,	73.6)
	AddCustom("EUPROMO")
	AddCustom("SHOEXALTED")
	AddCustom("PETBATTLEDAILY")
	AddCustom("COT",		Z.TANARIS)
	AddCustom("PETTAMER")
	AddCustom("MOUNTAIN_DEW")
	AddCustom("FIONA'S_CHARM",	Z.EASTERN_PLAGUELANDS,		34.8,	69.2)
	AddCustom("OOZING_BAG")
	AddCustom("PRIMAL_EGG",		Z.PANDARIA)
	AddCustom("HEROIC")
	AddCustom("NORMAL")
	AddCustom("LFR")
	AddCustom("FLEX")
	AddCustom("FISHING_DAILIES")
	AddCustom("OLYMPICS")
	AddCustom("DIREHORN")
	AddCustom("BLINGTRON")
	AddCustom("CRANENEST",		Z.TIMELESS_ISLE)
	AddCustom("TIMELESS_CHEST",	Z.TIMELESS_ISLE,		59.4,	40.6)
	AddCustom("UNCLAIMED_BM",	Z.THE_VEILED_STAIR,		63.8,	75.8)


	self.InitCustom = nil
end

