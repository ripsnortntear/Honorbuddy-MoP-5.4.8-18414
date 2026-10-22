local __FILE__=tostring(debugstack(1,2,0):match("(.*):1:")) -- MUST BE LINE 1
local MAJOR_VERSION = ("AlarCore-3.0.lua"):gsub(".lua","")
local MINOR_VERSION = 500 + tonumber(string.sub("$Revision: 83 $", 12, -3))
local pp=print
local _,_,_,toc=GetBuildInfo()
local me, ns = ...
--[[
Name: AlarCore-3.0.lua
Revision: $Rev: 83 $
Author: Alar of Daggerspine
Email: alar@aspide.it
Website: http://www.curse.com
SVN: $HeadUrl:$
Description: Generic library
Dependencies: Ace3
License: LGPL v2.1
--]]
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
--[[ Standard prologue end --]]
--debug("Loading " .. MAJOR_VERSION .. " r" .. MINOR_VERSION)
if (not LibStub) then
	error("Couldn't find LibStub. Please reinstall " .. MAJOR_VERSION )
end
local lib,old=LibStub:NewLibrary(MAJOR_VERSION,MINOR_VERSION)
if (not lib) then
	debug("Already loaded a newer or equal version of " .. MAJOR_VERSION)
	return -- Already loaded
end
if (old) then
	debug(format("Upgrading %s from %s to %s",MAJOR_VERSION,old,MINOR_VERSION))
	lib.list=old.list
else
  lib.list = setmetatable({}, {__mode="k"})
end
local L=LibStub("AceLocale-3.0"):GetLocale('AlarShared',true)
debugEnable(true)
--[[ Standard prologue end --]]

local C=LibStub("AlarCrayon-3.0"):GetColorTable()
--local C=LibStub("AlarCrayon-3.0")
local _G=_G
local floor=floor
local abs=abs
-- Help sections
local DESCRIPTION=L["Description"]
local RELNOTES=L["Release Notes"]
local LIBRARIES=L["Libraries"]
local TOGGLES=L["Toggles"]
local CONFIGURATION=L["Configuration"]
local HELPSECTIONS={DESCRIPTION,RELNOTES,CONFIGURATION,TOGGLES,LIBRARIES}
-- Checks for ACE3 availability
if (not LibStub("AceAddon-3.0",true)) then
    EnableAddOn("Ace3")
    if (IsAddOnLoadOnDemand("Ace3")) then
        LoadAddOn("Ace3")
    else
        error(L["Ace 3 required. Trying to enable it for next reload"])
    end
end

local AceConfig = LibStub("AceConfig-3.0",true) or debug("Missing AceConfig-3.0")
local AceRegistry = LibStub("AceConfigRegistry-3.0",true) or debug("Missing AceConfigRegistry-3.0")
local AceDBOptions=LibStub("AceDBOptions-3.0",true) or debug ("Missing AceDBOptions-3.0")
local AceConfigDialog=LibStub("AceConfigDialog-3.0",true) or debug("Missing gui support")
local AceGUI=LibStub("AceGUI-3.0",true)
local next = next
local pairs = pairs
local pcall = pcall
local type = type
local GetTime = GetTime
local gcinfo = gcinfo
local unpack = unpack
local geterrorhandler = geterrorhandler
local new, del
do
  local list=lib.list
	function new(...)
		local t = next(list)
		if t then
			list[t] = nil
			return t
		else
			return {}
		end
	end
	function del(t)
		setmetatable(t, nil)
		wipe(t)
		list[t] = true
	end
end
lib.mix=lib.mix or {}
local mix=lib.mix --#mix 
lib.hlp=lib.hlp or {} 
local hlp=lib.hlp --#hlp
lib.var=lib.var or {} 
local var=lib.var --#vars
lib.virt=lib.virt or {} 
local virt=lib.virt --#virt
lib.mixinTargets=lib.mixinTargets or {}
lib.frame=lib.frame or CreateFrame("Frame")
lib.combatSchedules = lib.combatSchedules or {}
if (not lib.wow) then
	local v,b,d,t=GetBuildInfo()
	lib.wow={version=v,build=b,date=d,toc=t}
end
local wow=lib.wow

local combatSchedules = lib.combatSchedules

local function nop(self,...)
	if (_G.ALARDEVELOPMENTPC) then
    	--self:Print('nop',...)
	end
end

-- Gestione variabili
local varmeta={}
do
	local nop=nop
    local function f1(self,flag,value)
        return self:Apply(flag,value)
    end
    local function f2(self,flag,value)
        return self['Apply' .. flag](self,value)
    end
    varmeta={
            __index = 
            function(table,cmd)
                local self=rawget(table,'_handler')
                if (type(self["Apply" .. cmd]) =='function') then
                    rawset(table,cmd,f2)
                elseif (type(self.Apply)=='function') then
                    rawset(table,cmd,f1)
                else 
                    rawset(table,cmd,function(...) nop(self,'cmd=',cmd,'args=',...) end)
                end
                return rawget(table,cmd)
            end
        }
end

mix.C=C
local function versiontonumber(version)
    if (type(version)=="number") then
        return version
    end
    local s,e,svn=version:find("$Rev%D*(%d+)%D*%$")
    version=version:gsub("$Rev%D*(%d+)%D*%$","")
    local res=0
    local fractpart=0
    local mult=1
    for i in version:gmatch("%d+") do
        local n=tonumber(i) or 0
        if (n < 1000) then
            res=res*1000
            res=res+n
        end
    end
    return tonumber(res .. '.' .. (svn or '0'))
end

lib.nops={
'HF_Cmd',
'HF_Color',
'HF_Command',
'HF_Commands',
'HF_Disabled',
'HF_Enabled',
'HF_Load',
'HF_Paragraph',
'HF_Pre',
'HF_Push',
'HF_Title',
'HF_Toggle',
'Debug',
'AddEdit',
'SetVar',
'AddCmdA',
}
function mix:NewTable(...)
  return new(...)
end
function mix:DelTable(...)
  return del(...)
end
function mix:VersionCompare(otherversion,strict)
    oterhversion=versiontonumber(otherversion)
    if (strict) then
        return self.numericversion-otherversion
    else
        return floor(self.numericversion - otherversion)
    end
end
local Myclass
function mix:Is(class,target)
    target=target or "player"
    if (target == "player") then
        if (not Myclass) then
        	local _
            _,Myclass=UnitClass('player')
        end
        return Myclass==strupper(class)
    else
        local    rc,_,unitclass=pcall(UnitClass,target)
        if (rc) then
            return unitclass==strupper(class)
        else
            return false
        end
    end
end
function mix:Parse(msg,skipcommand)
    if (not msg) then
        return nil
    end
    if (type(msg) == 'table' and msg.input ) then msg=msg.input end
    if (type(msg) ~= 'string') then return end
    local cmd,subcmd,param = strsplit(" ",msg,3)
    local _,fullarg=strsplit(" ",msg,2)
    return cmd,subcmd,param,fullarg
end
--[[
index is index in GetItemInfo result -1 is a special case to match just itemid
value is the value against to match. 0 is a special case for empty slot
startbag and starslot are used to restart scan from the last item found
All parameters are optional.
with no params ScanBags returns the first empty slot
--]]
function mix:ScanBags(index,value,startbag,startslot)
	index=index or 0
	value=value or 0
	startbag=startbag or 0
	startslot=startslot or 1
	for bag=startbag,NUM_BAG_SLOTS,1 do
		for slot=startslot,GetContainerNumSlots(bag),1 do
			local itemid=GetContainerItemID(bag,slot) or 0
			if (index==0) then
				if (itemid==value) then
					return itemid,bag,slot,GetItemInfo(itemid)
				end
			else
				local result={GetItemInfo(itemid)}
				if (result[index]==value) then
					return itemid,bag,slot,unpack(result)
				end
			end
		end
	end
	return false
end
function mix:GetArg(msg,arg)
	if (not msg) then
		return nil
	end
	if (type(msg) == 'table' and msg.input ) then msg=msg.input end
	if (type(msg) ~= 'string') then return end
	if (not arg) then arg=1 end
	return select(arg+1,strsplit(" ",msg))
end
function mix:Health(unit)
    local totale=UnitHealthMax(unit) or 1
    local corrente=UnitHealth(unit) or 1
    if (corrente == 0) then corrente =1 end
    if (totale==0) then totale = corrente end
    local life=corrente/totale*100
    return math.ceil(life)
end
function mix:Age(secs)
    return self:TimeToStr(GetTime() - secs)
end
function mix:Mana(unit)
    local totale=UnitManaMax(unit) or 1
    local corrente=UnitMana(unit) or 1
    if (corrente == 0) then corrente =1 end
    if (totale==0) then totale = corrente end
    local life=corrente/totale*100
    return math.ceil(life)
end
function mix:IsFriend(player)
    local i
    for i =1,GetNumFriends() do
        local name,_,_,_,_ =GetFriendInfo(i)
        if (name == player) then
            return true
        end
    end
    return false
end
function mix:GetDistanceFromMe(unit)
	if not unit then return 99999 end
	local x,y=GetPlayerMapPosition(unit)
	return self:GetUnitDistance(x,y)
end
function mix:GetUnitDistance(x,y,unit)
    unit=unit or "player"
    local from={}
    local to={}
    from.x,from.y=GetPlayerMapPosition(unit)
    to.x=x
    to.y=y
    return self:GetDistance(from,to) * 10000
end
function mix:GetDistance(a,b)
--------------
-- Calculates distance betweeb 2 points given as
-- a.x,a.y and b.x,b.y
	 local x=b.x - a.x
	 local y=b.y -a.y
	 local d=x*x + y* y
	 local rc,distance=pcall(math.sqrt,d)
	 if (rc) then
        return distance
    else
        return 99999
    end
end
function mix:LoadEvents(events)
    for k,v in pairs(events) do
        self.registry.events[k]=v
    end
end
function mix:LoadHooks(hooks)
    for k,v in pairs(hooks) do
        if (not (type(self.org[v]) ~= 'function')) then
            self.org[v]=getglobal(v)
        end
        self.registry.hooks[k]=v
    end
end
--[[
 Optional compatibility function. Registers methods with magic names
 hook* go to hooks' registry
 evt* go to events' registry
 wakeupEvent define an event to not be disabled by evtstop
  Not really needed
--]]
function mix:Register(wakeupEvent)
    debug("INVOKED REGISTER",self.name)
    local s,e,prefix,name    
    self.registry=self.registry or {}
    self.registry.wakeup=wakeupEvent
    self.registry.hooks=self.registry.hooks or {}
    self.registry.events=self.registry.events or {}
    self.org=self.org or {}
    for k,v in pairs(self) do
        s,e,prefix,name=k:find("(Hook)(.+)")
        if (s) then
            self.registry.hooks[name]=false
            self.org[name] = _G[name]
        end
        s,e,prefix,name=k:find("(Evt)(.+)")
        if (s) then
            self.registry.events[name]=0
        end
    end
    debug("REGISTER DONE",self.name)
end
function mix:ThrottleEvent(event,delay)
    self.registry=self.registry or {}
    self.registry.events=self.registry.events or {}
    self.registry.events[event]=delay
end
local function _DebugEnable(info)
	local self=info.handler
	local status=info.arg
	pp("Called _DebugeEnable",self,status)
	self:EnableDebug(status)
end
local function LoadDefaults(self)
    self.OptionsTable={
        handler=self,
        type="group",
        childGroups="tab",
        name=self.title,
        desc=self.notes,
        args={
           gui = {
                name="GUI",
                desc="Activates gui",
                type="execute",
                func="Gui",
                guiHidden=true,
            },
           help = {
                name="HELP",
                desc="Show help",
                type="execute",
                func="Help",
                guiHidden=true,
            },
            on = {
                name="On",
                desc="Activates " .. self.title,
                type="execute",
                func="Enable",
                guiHidden=true,
            },
            off = {
                name="Off",
                desc="Deactivates " .. self.title,
                type="execute",
                func="Disable",
                guiHidden=true,
                arg='Active'
            },
            debug ={
                name="debug",
                desc="Toggles debug for " .. self.title,
                type="input",
                set="_SwitchDebug",
                hidden=true
            },
            standby = {
                name="Enabled",
                desc="Toggle " .. self.title .. " status",
                type="toggle",
                get="IsEnabled",
                set="Toggle",
                cmdHidden=true,
                arg='Active'
            },
            toggle={
                type="group",
                name="toggle",
                desc="configuration switches",
                guiHidden=true,
                args={}
            }
        }
    }
    self.DbDefaults={
        global={
            currentversion=self.version,
            firstrun=true,
            lastversion=0,
            lastinterface=20300,
            
        },
        profile={
            toggles={
                Active=true,
            },
            ["*"]={},
        }
    }
    self.MenuLevels={"root"}
    self.ItemOrder=setmetatable({},{__index=function(table,key) rawset(table,key,1)
    return 1
    end})
    local AceDB  = LibStub("AceDB-3.0",true) or debug("Missing AceDB-3.0")
    if (AceDB and not self.db) then
        self.db=AceDB:New(self.DATABASE)
        --self.localdb=self.db:RegisterNamespace(self.name)
    end
    self.db:RegisterDefaults(self.DbDefaults)
    self:SetEnabledState(self:GetBoolean("Active"))
    -- I have for sure some library that needs to be intialized Before the addon
    for _,library in self:IterateEmbeds(self) do
        local lib=LibStub(library)
        if (lib.OnEmbedPreInitialize) then
            lib:OnEmbedPreInitialize(self) 
         end
    end   
end
local LoadHelp
function mix:OnInitialize(...)
	if (tonumber(self.revision)< 1) then
		self.revision='Alpha' 
	end
    self:Print("Version %s %s loaded" ,self:Colorize(self.version,'green'),self:Colorize(format("(Revision: %s)",self.revision),"silver"))
    LoadDefaults(self) 
    self.help=setmetatable(
        {},
        {__index=function(table,key) 
            rawset(table,key,"") 
            return rawget(table,key) 
            end
        }
    )   
    self:OnInitialized(...)
    local main=self.name
    local profile
    if (AceDBOptions) then
        self.ProfileOpts=AceDBOptions:GetOptionsTable(self.db)
        profile=main .. 'profile'
    end
    LoadHelp(self)
    AceConfigDialog:AddToBlizOptions(main,main )

    AceConfig:RegisterOptionsTable(main .. CONFIGURATION,self.OptionsTable,{main,strlower(self.ID)})
    self.CfgDlg=AceConfigDialog:AddToBlizOptions(main .. CONFIGURATION,CONFIGURATION,main)

    if (profile) then
        AceConfig:RegisterOptionsTable(profile,self.ProfileOpts)
        AceConfigDialog:AddToBlizOptions(profile,L.Profile,main)
    end
    self.CfgRel=AceConfigDialog:AddToBlizOptions(main .. RELNOTES,RELNOTES,main)

    AceConfigDialog:AddToBlizOptions(main .. TOGGLES,TOGGLES,main)

    AceConfigDialog:AddToBlizOptions(main .. LIBRARIES,LIBRARIES,main)
end

function LoadHelp(self)
    local main=self.name
    for libname,k in LibStub:IterateLibraries() do
        if (libname:match("Ace%w*-3%.0")) then
            self:HF_Lib(libname,'yellow')
        elseif (libname:match("Ace%w*-2%.0")) then
            self:HF_Lib(libname,'yellow')
        elseif (libname:match("Alar%w*-3%.0")) then
            self:HF_Lib(libname,'green')
        else
            self:HF_Lib(libname,'gray')
        end
        if (libname:match("Alar%w*-3%.0")) then
        end
    end
    for _,section in ipairs(HELPSECTIONS) do
        if (section == DESCRIPTION) then
            self:HF_Load(section,main,' ' .. tostring(self.version) .. ' (r:' .. tostring(self.revision) ..')')
        else
            self:HF_Load(section,main .. section,'')
        end
    end
end
function mix:GetAceOptionsTable(configtype)
    configtype=configtype or 'dropdown'
    if (AceRegistry) then
        return AceRegistry:GetOptionsTable(AAU.name,configtype,MAJOR_VERSION)
    else
        return self.OptionsTable
    end
end

-- help related functions
function hlp:HF_Push(section,text)
    section=section or self.lastsection or DESCRIPTION
    self.lastsection=section
    self.help[section]=self.help[section]  .. '\n' .. text
end
local getlibs
do
	local libs={}
    function hlp:HF_Lib(libname)
        local o,minor=LibStub(libname,true)
        if (o and libs) then
            if (not libs[o] or libs[o] <minor) then
                libs[libname]=minor
            end
        end
    end
    function getlibs(self)
        local appo={}
        if (not libs) then return end
        for i,_ in pairs(libs) do
            table.insert(appo,i)
        end
        table.sort(appo)
        for _,libname in pairs(appo) do
            local minor=libs[libname]
            self:HF_Pre(format("%s release: %s",self:Colorize(libname,'green'),self:Colorize(minor,'orange')),LIBRARIES)
        end
        libs=nil
    end
end

function hlp:HF_Toggle(flag,description)
    flag=C(format("/%s toggle %s: ",strlower(self.ID),flag),'orange') ..C(description,'white')
    self:HF_Push(TOGGLES,"\n" .. C(flag,'orange'))
end



function hlp:HF_Title(text,section)
    self:HF_Push(section,C(text or '','yellow') .. "\n")
end

function hlp:HF_Paragraph(text,section)
    self:HF_Push(section,"\n"..C(text,'green'))
end
function hlp:HF_CmdA(command,description,tooltip)
    self:HF_Push(nil,
    C('/' .. command,'orange') .. ' : ' .. (description or '') .. '\n' .. C(tooltip or '','yellow'))
end
function hlp:HF_Cmd(command,description,tooltip)
    command=self.ID .. ' ' .. command
    self:HF_CmdA(command,description,tooltip)
end
function hlp:HF_Pre(testo,section)
    self:HF_Push(section,testo)
end

function hlp:RelNotes(major,minor,revision,t)
    local fmt=self:Colorize("Release note for %d.%d.%s",'Yellow') .."\n%s"
    local lines={}
    local spacer=""
    local maxpanlen=70
    lines={strsplit("\n",t)}
    for i,tt in ipairs(lines) do
        local prefix,text=tt:match("^(Fixed):(.*)")
        if (prefix == "Fixed") then
            prefix=self:Colorize("Fixed: ",'Red')
            spacer=           "       "
        else
            prefix,text=tt:match("^(Feature):(.*)")
            if (prefix == "Feature") then
                prefix=self:Colorize("Feature: ",'Green')
                spacer=             "         "
            else
                text=tt
                prefix=spacer
            end
        end
        local tta=""
        tt=text
        while (tt:len() > maxpanlen)  do
            local p=tt:find("[%s%p]",maxpanlen -10) or maxpanlen
            tta=tta..prefix..tt:sub(1,p) .. "\n"
            prefix=spacer
            tt=tt:sub(p+1)
        end
        tta=tta..prefix..tt
        tta=tta:gsub("Upgrade:",self:Colorize("Upgrade:",'Azure'))
        lines[i]=tta:gsub("Example:",self:Colorize("Example:",'Orange'))
    end
    self:HF_Push(RELNOTES,fmt:format(major,minor,revision,strjoin("\n",unpack(lines))))
end

function hlp:HF_Load(section,optionname,versione)
-- Creazione pannello di help
-- Livello due del
    if (section == LIBRARIES) then
        getlibs(self)
    end
    local testo =self.help[section] or 'No text available'
    --debug(section)
    --debug(optionname)
    --debug(self.title)
	AceConfig:RegisterOptionsTable(optionname, {
		name = self.title .. (versione or ""),
		type = "group",
		args = {
			help = {
				type = "description",
				name = testo,
			},
		},
	})
	AceConfigDialog:SetDefaultSize(optionname, 600, 400)
end

function virt:Localize(...)
    return
end
function virt:OnInitialized(...)
    return
end
function virt:OnEnabled(...)
    return
end
function virt:OnDisabled(...)
    return
end
-- var area
local function getgroup(self)
    local group=self.OptionsTable
    local m=self.MenuLevels
    for i=2,#m do
        group=group.args[self.MenuLevels[i]]
    end
    if (type(group) ~= "table") then
        group={}
    end
    return group
end
local function getorder(self,group)
    local i=self.ItemOrder[group.name]+1
    self.ItemOrder[group.name]=i
    return i
end
local function toflag(...)
	local appo=''
	for i=1,select("#",...) do
		appo=appo .. tostring(select(i,...))
	end
    return appo:gsub("%W",'')
end
function var:EndLabel()
    local m=self.MenuLevels
    if (#m > 1) then
        table.remove(m)
    end
end    

--self:AddLabel("General","General Options",C.Green)
function var:AddLabel(title,description,stringcolor)
    self:EndLabel()
    description=description or title
    stringcolor=stringcolor or C.yellow
    local t=self:AddSubLabel(title,description,stringcolor)
    t.childGroups="tab"
    self:AddSeparator(description)
    return t
        
end
--self:AddSubLabel("Local","Local Options",C.Green)
function var:AddSubLabel(title,description,stringcolor)
    local m=self.MenuLevels
    description=description or title
    stringcolor=stringcolor or C.orange
    local group=getgroup(self)
    local flag=toflag(group.name,title)
    group.args[flag]={
        name="|cff" .. stringcolor .. title .. "|r",
        desc=description,
        type="group",
        cmdHidden=true,
        args={},
        order=getorder(self,group),
    }
    table.insert(m,flag)
    return group.args[flag]
end

--self:AddText("Testo"[,texture[,height[,width[,texcoords]]]])
function var:AddText(text,image,imageHeight,imageWidth,imageCoords)
    local group=getgroup(self)
    local flag=toflag(group.name,text)
    local t={
        name=text,
        type="description",
        image=image,
        imageHeight=imageHeight,
        imageWidth=imageWidth,
        imageCoords=imageCoords,
        desc=text,
        order=getorder(self,group),
        
    }
    group.args[flag]=t
    return t
end

--self:AddToggle("AUTOLEAVE",true,"Quick Battlefield Leave","Alt-Click on hide button in battlefield alert leaves the queue")
function var:AddBoolean(flag,defaultvalue,name,description)
    description=description or name
    local group=getgroup(self)
    local t={
        name=name,
        type="toggle",
        get="OptToggleGet",
        set="OptToggleSet",
        desc=description,
        arg=flag,
        order=getorder(self,group),
        
    }
    group.args[flag]=t
    if (self.db.profile.toggles[flag]== nil) then
        self.db.profile.toggles[flag]=defaultvalue
    end
    self.OptionsTable.args.toggle.args[flag]=t
    return t
end
function var:AddToggle(flag,defaultvalue,name,description)
    description=description or name
    self:HF_Toggle(flag,description)
    return self:AddBoolean(flag,defaultvalue,name,description)
end
--self:AddSlider("RESTIMER",5,1,10,"Enable res timer","Shows a timer for battlefield resser",1)
function var:AddSlider(flag,defaultvalue,min,max,name,description,step)
	description=description or name
	min=min or 0
	max=max or 100
	local group=getgroup(self)
	local isPercent=nil
	if (type(step)=="boolean") then
		isPercent=step
		step=nil
	else
		step=tonumber(step)
	end
	local t={
		name=name,
		type="range",
		get="OptToggleGet",
		set="OptToggleSet",
		desc=description,
		arg=flag,
		step=step,
		isPercent=isPercent,
		min=min,
		max=max,
		order=getorder(self,group),
	}
	group.args[flag]=t
	if (self.db.profile.toggles[flag]== nil) then
		self.db.profile.toggles[flag]=defaultvalue
	end
	return t
end
-- self:AddEdit("REFLECTTO","","Whisper reflection receiver:","All your whispers will be forwarded to this guy","How to use it")
function var:AddEdit(flag,defaultvalue,name,description,usage)
    description=description or name
    usage = usage or description
    local group=getgroup(self)
    local t={
        name=name,
        type="input",
        get="OptToggleGet",
        set="OptToggleSet",
        desc=description,
        arg=flag,
        usage=usage,
        order=getorder(self,group),
        
    }
    group.args[flag]=t
    if (self.db.profile.toggles[flag]== nil) then
        self.db.profile.toggles[flag]=defaultvalue
    end
    return t
end
-- self:AddEdit("REFLECTTO",1,{a=1,b=2},"Whisper reflection receiver:","All your whispers will be forwarded to this guy")
function var:AddSelect(flag,defaultvalue,values,name,description)
    description=description or name
    local group=getgroup(self)
    local t={
        name=name,
        type="select",
        get="OptToggleGet",
        set="OptToggleSet",
        desc=description,
        values=values,
        arg=flag,
        order=getorder(self,group)
    }
    group.args[flag]=t
    if (self.db.profile.toggles[flag]== nil) then
        self.db.profile.toggles[flag]=defaultvalue
    end
    return t
end

-- self:AddAction(["btopenspells",]"openSpells","Opens spell panel","You can choose yoru spells in spell panel")
function var:AddAction(method,label,description,private)
	label=label or method
	description=description or label
    local group=getgroup(self)
    if type(self[method]) ~="function" and type(self[label])=="function" then
    	local appo=method
    	method=label
    	label=appo
    	appo=nil
    end
    local t={
			func=method,
			name=label,
			type="execute",
			desc=description,
			confirm=false,
			order=getorder(self,group)
		}
	if (private) then t.hidden=true end
    group.args[strlower(label)]=t
    return t
end

function var:AddPrivateAction(method,name,description)
	return self:AddAction(method,name,description,true)
end
function var:AddKeyBinding(flag,name,description)
	name=name or strlower(name)
    description=description or name
    local group=getgroup(self)
    local t={
	    name=name,
	    type="keybinding",
	    get="OptToggleGet",
	    set="OptToggleSet",
	    desc=description,
	    arg=flag,
	    order=getorder(self,group)
    }
    group.args[flag]=t
    return t
end
function var:AddTable(flag,table)
    local group=getgroup(self)
    group.args[flag]=table
end
function var:_OpenCmd(info,args)
	local method=info.arg
	self[method](self,strsplit(' ',args))
end
function var:AddOpenCmd(command,method,description,arguments,private)
	method=method or command
	description=description or command
	local group=getgroup(self)
	if (not private) then
		local command=C('/' .. self.ID .. ' ' .. command .. " (" .. description .. ")" ,'orange')
		local t={
			name=command,
			type="description",
			order=getorder(self,group),
		}
		group.args[command .. 'title']=t
	end	
	local t={
		name=command,
		type="input",
		arg=method,
		get=function() end,
		set="_OpenCmd",
		desc=description,
		order=getorder(self,group),
		guiHidden=true,
		hidden=private
	}
	if (type(arguments)=="table") then
		local validate={}
		for _,v in pairs(arguments) do
			validate[v]=v
		end
		t.values=validate
		t.type="select"
	end
	self.OptionsTable.args[command]=t	

	return t
end
function var:AddPrivateOpenCmd(command,method,description,arguments)
	return self:AddOpenCmd(command,method,description,arguments,true)
end

--self:AddSubCmd(flagname,method,label,description)
function var:AddSubCmd(flag,method,name,description,input)
	method=method or flag
	name=name or flag
	description=description or name
	local group=getgroup(self)
	debug("AddSubCmd " .. flag .. " for " .. method)
	local t={
		func=method,
		name=name,
		type="execute",
		input=input,
		desc=description,
		confirm=true,
		order=getorder(self,group),
		guiHidden=true,
	}
	group.args[flag]=t
	return t
end



--self:AddCmd(flag,method,name,description)
function var:AddChatCmd(flag,method,name,description)
    self:RegisterChatCommand(flag,method)
    name=name or flag
    description=description or name

    local group=getgroup(self)
    local t={
        name=C('/' .. flag ..  " (" .. description .. ")",'orange'),
        type="description",
        order=getorder(self,group),
    }
    group.args[flag .. 'title']=t
    return t
end

--self:AddSeparator(text)

function var:AddSeparator(text)
    local group=getgroup(self)
    local i=getorder(self,group)
    local flag=group.name .. i
    flag=flag:gsub('%W','')
    local t={
        name=text,
        type="header",
        order=i,
    }
    group.args[flag]=t
    return t
end
--self:AddCmd(flagname,method,label,description)
function var:AddCmd(flag,method,name,description)
	self:FullTrace("AddCmd is deprecated")
	return self:AddChatCmd(method,name,description)
end
--self:AddButton(flagname,method,label,description)
function var:AddButton(flag,method,name,description)
	self:FullTrace("AddButton is deprecated")
	return self:AddAction(method,name,description)
end
--self:AddExec(flagname,method,label,description)
function var:AddExec(flag,method,label,description)
	self:FullTrace("AddExec is deprecated")
	return self:AddAction(method,name,description)
end
--self:AddLocalCmd(flag,method,name,description)
function var:AddLocalCmd(flag,method,name,description)
	self:FullTrace("AddLocalCmd is deprecated")
	return self:AddOpenCmd(flag,method,name,description)
end
function var:AddICmd(flag,method,name,description,input)
	self:FullTrace("AddICmd is deprecated")
	return self:AddPrivateOpenCmd(flag,method,name,description)
end


function lib:OnEmbedEnable(first)
end

function lib:OnEmbedDisable()
end
local function str2num(versione)
	versione =versione or 0
	if (type(versione) == "string") then
		local a,b,c=versione:match("(%d*)%D?(%d*)%D?(%d*)%D*")
		a=tonumber(a) or 0
		b=tonumber(b) or 0
		c=tonumber(c) or 0
		return a*1000+b+c/100
	else
		return versione
	end
end
--[[
        global={
        currentversion=self.version,
        firstrun=true,
        lastversion=0,
        lastinterface=20300,
        
        },
--]]
function mix:UpdateVersion()
	if (type(self.db.char) == "table") then
		self.db.char.version=str2num(self.version)
	end
	if (type(self.db.global)=="table") then
		self.db.global.lastversion=self.version
		self.db.global.firstrun=false
		self.db.global.lastinterface=self:GetTocVersion()
	end
end
function mix:IsNewVersion()
	return str2num(self.version) > str2num(self.db.char.version)
end
function mix:VersionIsAtLeast(compareto)
	return str2num(compareto) >= str2num(self.version)
end

function mix:OnEnable(...)
    self:Print(C("enabled",'green'))
    self:ApplySettings()
    self:OnEnabled(...)
end
function mix:OnDisable(...)
    self:Print(C("disabled",'red'))
    self:OnDisabled(...)
end
local _GetMethod 
function _GetMethod(target,prefix,func)
    if (func == 'Start' or func == 'Stop') then return end
    local method=prefix .. func
    if (type(target[method])== "function") then
        return method
    elseif (type(target["_" .. prefix])) then
        return "_" .. prefix
    end
end 
function mix:HookStart(hooks)
    hooks=hooks or self.registry.hooks
    for hook,security in pairs(hooks) do
        local hooked=_G[hook]
        if (hooked) then
            local method=_GetMethod(self,"Hook",hook)
            if (method) then
                debug("Hooking " .. hook .. ' to ' .. tostring(method) .. ' Security:' .. tostring(security))
                if (not self:IsHooked(hook)) then
                    if (not type(self.org[hook]) ~= 'function') then
                        self.org[hook]=hooked
                    end
                    if (security=='RAW') then
                        self:RawHook(hook,method,true)
                    elseif (security=='SECURE') then
                        self:SecureHook(hook,method)
                    else
                        self:Hook(hook,method)
                    end
                end
            end
        else
            hooks[hook]=nil
        end
    end
end
function mix:HookStop(hooks)
    hooks=hooks or self.registry.hooks
    for hook,level in pairs(hooks) do
        if (self:IsHooked(hook)) then
            debug("UnHooking " .. hook)
            self:Unhook(hook)
        end
    end
end
function mix:EvtStart(events)
    events=events or self.registry.events
    for evt,delay in pairs(events) do
        local method=_GetMethod(self,"Evt",evt)
        if (method) then
            --debug("Observing " .. evt .. ' with ' .. tostring(method))
            if (tonumber(delay)) then
                if (delay>0) then
                    self:RegisterBucketEvent(evt,delay,method)
                else
                    self:RegisterEvent(evt,method)
                end
            else
                self:RegisterEvent(evt,method)
            end
        end
    end
end
function mix:EvtStop(events,wakeup)
    events=events or self.registry.events
    wakeup=wakeup or self.registry.wakeup
    for evt,delay in pairs(events) do
    	if (wakeup and wakeup~=evt) then
	        if (tonumber(delay)) then
	            if (delay>0) then
	                self:UnregisterBucketEvent(evt)
	            else
	                self:UnregisterEvent(evt)
	            end
	        end
        end
    end
end
--[[
function mix:Notify(...)
	return self:CustomPrint(C.orange.r,C.orange.g,C.orange.b, nil, nil, ' ', ...)
end
function mix:Debug(...)
	if (self.DebugOn or _G.ALARDEVELOPMENTPC) then
		self:Print(...)
	end
end
function mix:Print(...)
	print
	return self:CustomPrint(nil, nil, nil, nil, nil, ' ', ...)
end

function mix:Dump(...)
	return self:CustomPrint(nil, nil, nil, nil, nil, true, ...)
end
--]]
--alias

function mix:Colorize(stringa,colore)
    return C(stringa,colore) .. "|r"
end
function mix:GetTocVersion()
	return tonumber(wow.TocVersion) or 0
end
function mix:Toggle()
    if (self:IsEnabled()) then
        self:Disable()
    else
        self:Enable()
    end
end
function var:Vars()
    return pairs(self.db.profile.toggles)
end
function var:SetBoolean(flag,value)
    if (value) then
        value=true
    else
        value=false
    end
    self.db.profile.toggles[flag]=value  
    return not value
end
function var:GetBoolean(flag)
    if (self.db.profile.toggles[flag]) then
        return true
    else
        return false
    end  
end
var.GetToggle=var.GetBoolean -- alias
function var:GetNumber(flag,default)
	return tonumber(self:GetSet(flag) or default or 0)
end
function var:GetString(flag,default)
	return tostring(self:GetSet(flag) or default or '')
end

do
    local CLOSE=_G.FONT_COLOR_CODE_CLOSE or '|r'   
    local off=(_G.RED_FONT_COLOR_CODE or '|cffff0000') .. 'Off' ..  CLOSE
    local on=(_G.GREEN_FONT_COLOR_CODE or '|cff00ff00') .. 'On' ..  CLOSE
    function var:PrintBoolean(flag)
        if (type(flag) == "string") then
            flag=self:GetBoolean(flag)
        end
        if (flag) then
            return on
        else
            return off
        end            
    end
end
function var:GetSet(...)
    local flag,value=select(1,...)
    if (select('#',...) == 2) then
        self.db.profile.toggles[flag]=value
    else
        return self.db.profile.toggles[flag]
    end  
end
function var:GetVar(flag)
	return self:GetSet(flag)
end
function var:SetVar(flag,value)
	return self:GetSet(flag,value)
end
function var:OptToggleSet(info,value)
    local flag=info.option.arg
    local tipo=info.option.type
    --self:SendMessage(self.ID .. "_UPDATECONFIG",flag,tipo)
    
    if (tipo=="toggle") then
        self:SetBoolean(flag,value)
    else
        self:GetSet(flag,value)
    end  
    if (self:IsEnabled()) then
        self._Apply[flag](self,flag,value)
    end
end
function var:OptToggleGet(info)
    local flag=info.option.arg
    local tipo=info.option.type
    if (tipo=="toggle") then
        return self:GetBoolean(flag)
    else
        return self:GetSet(flag)
    end  
end
function var:ApplySettings()
    if (type(self.ApplyAll)=="function") then
        self:ApplyAll()
    else
        for i,v in self:Vars() do
            self._Apply[i](self,i,v)
        end
    end
end
local neveropened=true
function mix:Gui(info)
    if (AceConfigDialog and AceGUI) then
      if (neveropened) then
        InterfaceAddOnsList_Update()
        neveropened=false
      end
      InterfaceOptionsFrame_OpenToCategory(self.CfgDlg)
    else
        self:Print("No GUI available")
    end
        
end
function mix:Help(info)
    if (AceConfigDialog and AceGUI) then 
        InterfaceOptionsFrame_OpenToCategory(self.CfgRel)
    else
        self:Print("No GUI available")
    end
        
end
function mix:IsEventScheduled(flag)
    lib.timerhandles=lib.timerhandles or {}
	return lib.timerhandles[flag]
end
function mix:ScheduleRepeatingEvent(flag,...)
    lib.timerhandles=lib.timerhandles or {}
    lib.timerhandles[flag]=self:ScheduleRepeatingTimer(...)
end
function mix:CancelScheduledEvent(flag)
    lib.timerhandles=lib.timerhandles or {}
    local h=lib.timerhandles[flag]
    self:CancelTimer(h)
end
function mix:Trace(...)
--[===[@debug@	
	self:_Trace(false,...)
--@end-debug@]===]	
end
function mix:FullTrace(...)
	--[===[@debug@	
	self:_Trace(true,...)
	--@end-debug@]===]	
end
function mix:_Trace(ft,fmt,...)
    local stack={strsplit("\n",debugstack(3,5,0))}
	local info=stack[1]
    local file,line,func=tostringall(strsplit(":",info))
    
    local r,g,b=C.Yellow()
    _G.DEFAULT_CHAT_FRAME:AddMessage(
    	format("Trace\nWhere: %s:%s%s",self:Colorize(file,'azure'),self:Colorize(line,'red'),self:Colorize(func,'orange')) ..
    	format("\nWhat: "..fmt,tostringall(...)),
    	r,g,b)
	if (ft) then
		print "Full stack dump"
    	for i,info in ipairs(stack) do
    		print (format("Stack: %d. %s",i,self:Colorize(info,'green')))
    	end
    	print("--------------")
    end
end


do
	local C=C
	function mix:Long(msg) C:OnScreen('Yellow',msg,20) end
	function mix:Onscreen_Orange(msg) C:OnScreen('Orange',msg,2) end
	function mix:Onscreen_Purple(msg) C:OnScreen('Purple',msg,8) end
	function mix:Onscreen_Yellow(msg) C:OnScreen('Yellow',msg,1) end
	function mix:Onscreen_Azure(msg) C:OnScreen('Azure',msg,1) end
	function mix:Onscreen_Red(msg) C:OnScreen('Red',msg,1) end
	function mix:Onscreen_Green(msg) C:OnScreen('Green',msg,1) end
	function mix:OnScreen(color,...) C:OnScreen(color,strjoin(' ',tostringall(...))) end
end
function mix:TimeToStr(time) -- Converts time data to a string format
	local p,s,m,h;
	if (not time) then
		return ("0:00")
	end
	if (time < 0) then
		time=abs(time)
		p='-'
	else
		p=''
	end
	s = floor(mod(time, 60));
	m = floor(time/ 60);
	if (m > 59) then
		h=floor(m/60)
		m=floor(mod(m,60))
	end
	if (h) then
		return format("%s%d:%02d:%02d",p,h,m,s)
	else
		return format("%s%d:%02d",p,m,s)
	end
end

function lib:getColorTable()
	return C
end
-- In case of upgrade, we need to redo embed for ALL Addons
-- This function get called on addon creation
-- Anything I define here is immediately available to addon code
function lib:Embed(target)
    debug("Embedding " .. MAJOR_VERSION .. "." .. MINOR_VERSION .. " into " .. target.name)
    -- Info from TOC
    local v=GetAddOnMetadata(tostring(target),"version")
    v=v or '0'
    target.tocversion=v
    local version,revision=v:match("([^$ ]*) *(.*)")
    target.version=version or "0"
    local title=target.name or target.fullname
    if (target.version == '@'..'project-version@' and title) then
        target.version=GetAddOnMetadata(title,'X-Version')
    end
    if (target.revision == '@'..'project-revision@' and title) then
        target.revision=GetAddOnMetadata(title,'X-Revision')
    end
    target.revision=revision:match("%d+") or "0"
    target.prettyversion=format("%s (Revision: %s)",tostringall(target.version,target.revision))
    target.numericversion=versiontonumber(v)
    target.title=GetAddOnMetadata(tostring(target),"title") or 'No title'
    target.notes=GetAddOnMetadata(tostring(target),"notes") or 'No notes'
    -- Setting sensible default for mandatory fields
    target.ID=GetAddOnMetadata(title,"X-ID") or (target.name:gsub("[^%u%d]","") .. "XXXX"):sub(1,3)
    target.DATABASE=GetAddOnMetadata(title,"X-Database") or "db" .. target.ID
    debug("Info for",target.name,'(',target.ID,')',target.DATABASE,GetAddOnMetadata(target.name,"X-Database"))
    -- Standard Mixins
    for name,method in pairs(mix) do
        target[name] = method
    end
    -- Printing func
    local rc=LibStub("AlarLoader-3.0"):GetPrintFunctions(target:GetName(),2)
    for name,func in pairs(rc) do
    	target[name]=func
    	target[name:capitalize()]=func
    end
    -- Help system mixins
    for name,method in pairs(hlp) do
        target[name] = method
    end
    -- var management mixins
    for name,method in pairs(var) do
        target[name] = method
    end
    target._Apply=target._Apply or {}
    target._Apply._handler=target
    setmetatable(target._Apply,varmeta)
    -- virtual methods, they can be ovverriden
    -- versioning is not important, because virtual methods are always nop
    for name,_ in pairs(virt) do
        target[name]=target[name] or nop
    end
    -- nops, placeholders. To be removed when done with porting
    for _,name in pairs(lib.nops) do
        target[name]=target[name] or nop
    end
    
    target.registry=target.registry or {}
    local r=target.registry
    for _,subtable in pairs{'events','hooks','commands','options','icommands'} do
        r[subtable]=r[subtable] or {}
    end
    target.org=target.org or {}
    target.coroutines=target.coroutines or {}
    lib.mixinTargets[target] = true
    if (me == "AlarShared") then
    	local as=LibStub("AceAddon-3.0"):GetAddon("AlarShared",true)
    	if (as) then as:LoadOptions() end
    end
end


----- New Global Function, I check if someone already defined then
if (not string.capitalize) then
    function string.capitalize(stringa)
        return string.gsub(strlower(stringa),
                "%w",function (s) return strupper(s) end,1)
    end
end
if (not _G.capitalize) then
    _G.capitalize=string.capitalize
end
if (not _G.table.kpairs) then
    function _G.table.kpairs(t,f)
      local a = {}
      for n in pairs(t) do table.insert(a, n) end
      table.sort(a, f)
      local i = 0      -- iterator variable
      local iter = function ()   -- iterator function
        i = i + 1
        if a[i] == nil then
            return nil
        else
            local k=a[i]
            a[i]=nil -- Should optimize memory usage
            return k, t[k]
        end
      end
      return iter
    end
end
if (not _G.kpairs) then
    _G.kpairs=table.kpairs
end
-------------------------------------------------------------------------------
-- ScheduleLeaveCombatAction Port
-- Shamelessly stolen from Ace2
-------------------------------------------------------------------------------
function mix:CancelAllCombatSchedules()
	local i = 0
	while true do
		i = i + 1
		if not combatSchedules[i] then
			break
		end
		local v = combatSchedules[i]
		if v.self == self then
			v = del(v)
			table.remove(combatSchedules, i)
			i = i - 1
		end
	end
end


do
	local tmp = {}
	local doaftercombataction
	function doaftercombataction()
		for i, v in ipairs(combatSchedules) do
			tmp[i] = v
			combatSchedules[i] = nil
		end
		debugEnable(true)
		for i, v in ipairs(tmp) do
			local func = v.func
			if func then
				local success, err = pcall(func, unpack(v, 1, v.n))
				if not success then geterrorhandler()(err:find("%.lua:%d+:") and err or (debugstack():match("(.-: )in.-\n") or "") .. err) end
			else
				local obj = v.obj or v.self
				local method = v.method
				debug("Calling",method)
				local obj_method = obj[method]
				if obj_method then
					local success, err = pcall(obj_method, obj, unpack(v, 1, v.n))
					if not success then geterrorhandler()(err:find("%.lua:%d+:") and err or (debugstack():match("(.-: )in.-\n") or "") .. err) end
				end
			end
			tmp[i] = del(v)
		end
	end
	lib.frame:SetScript("OnEvent",nil)
	lib.frame:RegisterEvent("PLAYER_REGEN_ENABLED")
    lib.frame:SetScript("OnEvent",doaftercombataction)
end

function mix:ScheduleLeaveCombatAction(method, ...)
	local style = type(method)
	if style == "string" and type(self[method]) ~= "function" then
		debug("Cannot schedule a combat action to method %q, it does not exist", method)
	elseif style == "table" then
		local func = (...)
		if type(method[func]) ~= "function" then
			debug("Cannot schedule a combat action to method %q, it does not exist", func)
		end
	end
	
	if not InCombatLockdown() then
		local success, err
		if type(method) == "function" then
			success, err = pcall(method, ...)
		elseif type(method) == "table" then
			local func = (...)
			success, err = pcall(method[func], method, select(2, ...))
		else
			success, err = pcall(self[method], self, ...)
		end
		if not success then geterrorhandler()(err:find("%.lua:%d+:") and err or (debugstack():match("(.-: )in.-\n") or "") .. err) end
		return
	end
	local t
	local n = select('#', ...)
	if style == "table" then
		t = new(select(2, ...))
		t.obj = method
		t.method = (...)
		t.n = n-1
	else
		t = new(...)
		t.n = n
		if style == "function" then
			t.func = method
		else
			t.method = method
		end
	end
	t.self = self
	table.insert(combatSchedules, t)
end
function mix:_iCmd(info,args)
	
end
function mix:_SwitchDebug(info,p)
	local p=p or ''
	p=strlower(p)
	local target=tostring(self) or ''
	pp ("dbg",target,p)
	local a=LibStub("AlarLoader-3.0")
	local status
	if (p=='on') then pp("p = on") status=true end
	if (p=='off') then pp("p = off") status = false end
	if (p=='on' or p =='off') then
		self:Print("Debug was",self:PrintBoolean(a:GetDebug(target)))
		a:SetDebug(target,status)
	end 
	self:Print("Debug is",self:PrintBoolean(a:GetDebug(target)))
end
function mix:Void(...)
	if select('#',...) > 0 then print(...) end
end
function mix:Test(...)
	print(GetTime(),'Test',...)
	return "Test Ran"
end
local function Sink(durata)
	for i=1,durata do
		
	end
end
local function coclear(rc,...)
	if (rc) then
		return ...
	else
		return nil
	end
end
function mix:CoroutineCreate(name,method)
	print("Creating coroutine",name,"for",method)
	local c=self.coroutines
	if (c[name]) then return c[name] end
	local func
	do
		local addon=self
		local method=self[method]
		function func(...)
			method(addon,...)
		end
	end
	c[name]=coroutine.create(func)
	return c[name]
end
function mix:CoroutineExecute(name,method,tick,...)
	self:CoroutineCreate(name,method)
	self:CoroutineStart(name,tick,...)
end
function mix:CoroutineRun(name,...)
	local co=self.coroutines[name]
	if (type(co)=="thread" and coroutine.status(co) ~= "dead") then
		return(coclear(coroutine.resume(co,...)))
	else
		return nil
	end
end
function mix:CoroutineStart(name,tick,...)
	tick=tonumber(tick) or 1
	local co=self.coroutines[name]
	if (not self:IsEventScheduled(name)) then
		if (type(co)=='thread') then
			self:ScheduleRepeatingEvent(name,co,...)
		else
			print(name,'is not a coroutine')
		end
	end
end

function mix:CoroutinePause(name)
	if (self:IsEventScheduled(name)) then
		self:CancelScheduledEvent(name)
	end
end
function mix:Popup(msg,timeout)
	msg=msg or "Something strange happened"
	StaticPopupDialogs["AMO_POPUP"] = {
	text = TEXT(msg),
	button1 = TEXT("OK"),
	showAlert = 1,
	timeout = timeout or 60,
	exclusive = 1,
	whileDead = 1,
	interruptCinematic = 1
	};
	StaticPopup_Show("AMO_POPUP");
end



--- reembed routine
for target,_ in pairs(lib.mixinTargets) do
lib:Embed(target)
end

