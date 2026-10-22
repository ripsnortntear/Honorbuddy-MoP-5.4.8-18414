local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarCastSingleButton.lua"):gsub(".lua","")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 48 $", 12, -3))
local pp=print
local Type,Version,ancestor='AlarCastSingleButton',4,'AlarCastButton'
--[[
Name: AlarCastButton.lua
Revision: $Rev: 48 $
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
debugEnable(false)
local _,_,_,toc=GetBuildInfo()
--[[ Standard prologue end --]]
local _G=_G
local AWG=LibStub("AlarWidgets-3.0")
local AceGUI=LibStub("AceGUI-3.0")
local InjectStandardMethods=AWG.InjectStandardMethods
if not AceGUI or (AceGUI:GetWidgetVersion(Type) or 0) >= Version then return end
local methods={}


function methods:SetIcon(texture)
	self.texture=texture
	self:Update()
end
function methods:HideOnClick()
	debug("HideOnCLick to be implemented")
end
function methods:Pop(duration,speed)
    local status = self.status or self.localstatus
    local frame = self.frame
    local offset=AWG:Pop(self,status.size or 32)
    frame:ClearAllPoints()
    frame:SetPoint("CENTER",UIParent,"CENTER",offset,0)
    self:Show()
    debug ("Popping",self.spell,duration,speed)
    if (type(duration) == 'number') then
	    speed=5
	    self:FadeOut()
    end	
end
function methods:FadeOut(speed,delay)
	debug("FadeOut invoked")
    local fade=self.fade
    local animation=self.animation
    local frame=self.frame
    self.faded=true
    if (not frame:IsShown() or frame:GetAlpha()==0) then return end
    delay=delay or 5
    speed=speed or 25
    fade:SetChange(-1)
    fade:SetStartDelay(delay)
    fade:SetDuration(speed)
	debug("Fade started",delay,speed)
    animation:Play()
end
function methods:FadeStop()
    local animation=self.animation
    animation:Stop()
end
function methods:FadeIn(speed,delay)
    local fade=self.fade
    local frame=self.frame
    local animation=self.animation
    self.faded=false
    if (frame:IsShown() and frame:GetAlpha()==1) then return end
    if (not frame:IsShown()) then
	    frame:SetAlpha(0)
	    if (not InCombatLockdown()) then
	    	frame:Show()
	    end
    end
    delay=delay or 5
    speed=speed or 25
    fade:SetChange(1)
    fade:SetStartDelay(delay)
    fade:SetDuration(speed)
    animation:Play()
end

do
	-- Scripts
	local function AnimationComplete(this)
		local i=this:GetChange()
		local self=this.obj
		self:Fire("OnFadeEnd")
		if (not InCombatLockdown()) then
			if (i > 0) then
				self:Show()
			else
				self:Hide()
			end
		end
		self:SetAlpha(1)
	end
	local Serial=0
	local function Constructor()
 		local self=AceGUI:Create(ancestor)
		self.standalone=true
		self.active=true
		self:Inject(methods,ancestor)
		local frame=self.frame
	    local fname=frame:GetName() .. "XButton"
	    local b=CreateFrame("Button",fname,frame,"UIPanelCloseButton")
	    b:SetHeight(16)
	    b:SetWidth(16)
	    b:SetPoint("TOPRIGHT",frame,"TOPRIGHT",0,0)
	    b:SetFrameLevel(10)
	    b.obj=self
	    b:SetScript("PostClick",function(this)  this.obj:Fire("OnClose") end )
		local ag=frame:CreateAnimationGroup(frame:GetName()..'fade')
		local fade=ag:CreateAnimation('Alpha')
		fade:SetChange(-1)
		fade:SetStartDelay(0)
		fade:SetDuration(0)
		fade:SetScript("OnFinished",AnimationComplete)
		--fade:SetScript("OnUpdate",function(this,elapsed) print( elapsed) end )
		fade.obj=self
		self.fade=fade
		self.animation=ag
	    self.xbutton=b
		return self
		
	end
	AceGUI:RegisterWidgetType(Type,Constructor,Version)
	AWG.widgets[Type]=Version    
end
