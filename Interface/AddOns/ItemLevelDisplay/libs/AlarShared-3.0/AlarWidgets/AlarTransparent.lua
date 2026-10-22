local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarTransparent.lua"):gsub(".lua","")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 84 $", 12, -3))
local pp=print
local Type,Version,ancestor = "AlarTransparent",4,"Window"
--[[
Name: AlarPanels.lua
Revision: $Rev: 84 $
Author: Alar of Daggerspine
Email: alar@aspide.it
Website: http://www.curse.com
SVN: $HeadUrl:$
Description: Generic library
Dependencies: Ace3
License: LGPL v2.1
--]]
local me, ns = ...
--[===[@debug@
print("Loading",__FILE__," inside ",me)
--@end-debug@]===]
if (LibDebug) then LibDebug() end
local function debug(...) 
--[===[@debug@
	print(...)
--@end-debug@]===]
end
local print=_G.print
local notify=_G.print
local error=_G.error
local function dump() end
local function debugEnable() end
if (LibStub("AlarLoader-3.0",true)) then
	local rc=LibStub("AlarLoader-3.0"):GetPrintFunctions(MAJOR_VERSION)
	print=rc.print
	--[===[@debug@
	debug=rc.debug
	dump=rc.dump
	--@end-debug@]===]
	notify=rc.notify
	error=rc.error
	debugEnable=rc.debugEnable
else
	debug("Missing AlarLoader-3.0")
end
local _,_,_,toc=GetBuildInfo()
debugEnable(false)
local L=LibStub("AceLocale-3.0"):GetLocale('AlarShared',true)
--[[ Standard prologue end --]]
local AWG=LibStub("AlarWidgets-3.0")
local AceGUI=LibStub("AceGUI-3.0")
local InjectStandardMethods=AWG.InjectStandardMethods
if not AceGUI or (AceGUI:GetWidgetVersion(Type) or 0) >= Version then return end
local methods={}
	function methods:OnAcquire()
		self:Parent(ancestor,'OnAcquire')
		self.frame:SetFrameStrata("HIGH")	
		self.frame:SetClampedToScreen(true)	
		self:ApplyStatus()
		self:EnableResize(false)
		self:SetHeight(20)
		self:SetWidth(50)
		self:Show()
	end
do
	local backdrop = {
		--bgFile = "Interface\\ChatFrame\\ChatFrameBackground", tile = true, tileSize = 16,
		bgFile="Interface\\QuestFrame\\UI-QuestTitleHighlight", tile = false, tileSize = 16,
		edgeFile = "Interface\\Tooltips\\UI-Tooltip-Border", edgeSize = 16,
		insets = {left = 4, right = 4, top = 4, bottom = 4},
	}
	local function Constructor()
		--local self = LibStub("AceGUI-3.0"):Create("Frame")
		--local frame=self.frame
		--local frame = AceGuiCreateFrame("Frame",nil,UIParent)
		--self={}
		local self=AceGUI:Create(ancestor)
		local frame=self.frame
		self.type = Type
		InjectStandardMethods(self)
		self:Inject(methods,ancestor)
		self.localstatus = {}
		frame.obj = self
		for _,f in pairs({frame:GetRegions()}) do
			if (f:GetObjectType()=="Texture") then f:Hide() end
		end		
		frame:SetBackdrop(backdrop)
		--self.closebutton:Hide()
		frame:SetWidth(100)
		frame:SetHeight(30)
		frame:EnableMouse()
		frame:SetMovable(true)
		return self	
	end
	AceGUI:RegisterWidgetType(Type,Constructor,Version)
	AWG.widgets[Type]=Version
end
