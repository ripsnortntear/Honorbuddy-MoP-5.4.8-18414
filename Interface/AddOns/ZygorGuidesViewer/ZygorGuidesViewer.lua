assert(not _G['ZGV'],"Two ZygorGuideViewers loaded!\nWe're doomed!")

local addonName,ZygorGuidesViewer = ...

ZygorGuidesViewer = LibStub("AceAddon-3.0"):NewAddon(ZygorGuidesViewer,addonName, "AceConsole-3.0","AceEvent-3.0","AceTimer-3.0")

local ZGV=ZygorGuidesViewer
--global exports
_G['ZygorGuidesViewer']=ZygorGuidesViewer
_G['ZGV']=ZGV

ZGV.L = ZygorGuidesViewer_L("Main")
ZGV.LS = ZygorGuidesViewer_L("G_string")

local L = ZGV.L
local LI = ZGV.LI
local LC = ZGV.LC
local LQ = ZGV.LQ
local LS = ZGV.LS

local Astrolabe = DongleStub("Astrolabe-ZGV")

--ZGV.name = L['name_plain']

ZGV.registeredguides = {}
ZGV.registeredmapspotsets = {}
ZGV.guidesets = {}
ZGV.registered_groups = { groups={},guides={}}

ZGV.Vars={}

local DIR = "Interface\\AddOns\\"..addonName
ZGV.DIR = DIR
local SKINDIR = ""

ZYGORGUIDESVIEWER_COMMAND = "zygor"

ZYGORGUIDESVIEWERFRAME_TITLE = "ZygorGuidesViewer"

BINDING_HEADER_ZYGORGUIDES = L["name_plain"]
BINDING_NAME_ZYGORGUIDES_OPENGUIDE = L["binding_togglewindow"]
BINDING_NAME_ZYGORGUIDES_PREV = L["binding_prev"]
BINDING_NAME_ZYGORGUIDES_NEXT = L["binding_next"]

local build = tonumber(select(2,GetBuildInfo()),nil)
ZGV.Expansion_Cata = true
ZGV.Expansion_Mists = (build>=15799)

local BZ = LibStub("LibBabble-SubZone-3.0")
local BZL = BZ:GetUnstrictLookupTable()
local BZR = BZ:GetReverseLookupTable()
ZGV.BZL = BZL
ZGV.BZR = BZR
local BF = LibStub("LibBabble-Faction-3.0")
local BFL = BF:GetUnstrictLookupTable()
local BFR = BF:GetReverseLookupTable()
ZGV.BFL = BFL
ZGV.BFR = BFR

local _G,assert,table,string,tinsert,tremove,tonumber,tostring,type,ipairs,pairs,setmetatable,math,resume,status,yield =
      _G,assert,table,string,tinsert,tremove,tonumber,tostring,type,ipairs,pairs,setmetatable,math,coroutine.resume,coroutine.status,coroutine.yield

--local Dewdrop = AceLibrary("Dewdrop-2.0")

ZGV.LibTaxi = LibStub("LibTaxi-1.0")


ZGV.CartographerDatabase = { }


ZGV.startups = {}

local LINES_PER_STEP = 30
ZGV.StepLimit = 20
ZGV.stepframes = {}
ZGV.spotframes = {}

ZGV.mentionedQuests = {}

ZGV.MapIDsByName = LibRover.data.MapIDsByName

-- TODO these should be culled and moved into Skin styles
local STEP_LINE_SPACING = 2
local MIN_HEIGHT=80
local ICON_INDENT=17
ZGV.ICON_INDENT = ICON_INDENT  -- so that other files can access it; silly, I know
local STEP_SPACING = 2
ZGV.STEP_SPACING=STEP_SPACING
-- ZGV.STEPMARGIN_X=3
-- ZGV.STEPMARGIN_Y=3

-- This gets really useful, probably move to functions or make it ZGV member?
local function SkinData(parm)
	return ZGV.CurrentSkinStyle:SkinData(parm)
end


ZGV.MIN_STEP_HEIGHT=12
ZGV.MIN_SPOT_HEIGHT=40

local MIN_WIDTH = 260
local TOP_HEIGHT = 65

--local FONT = STANDARD_TEXT_FONT
local FONT=L['MainFont']
local FONTBOLD=L['MainFontBold']
ZGV.Font = FONT
ZGV.FontBold = FONTBOLD

--ZGV.BUTTONS_INLINE=true


-- BAD GLOBALS!
_G['Astrolabe']=Astrolabe

local math_modf=math.modf
math.round=function(n) local x,y=math_modf(n) return n>0 and (y>=0.5 and x+1 or x) or (y<=-0.5 and x-1 or x) end
local round=math.round


StaticPopupDialogs['ZYGORGUIDESVIEWER_HELP'] = {
	text = L['static_help'],
	button1 = OKAY,
	hideOnEscape = 1,
	timeout = 0,
	whileDead = 1,
}

StaticPopupDialogs['ZYGORGUIDESVIEWER_SIS'] = {
	text = L['static_sis'],
	button1 = OKAY,
	button2 = CANCEL,
	OnAccept = function(self) ZGV:SIS_Activate() end,
	hideOnEscape = 1,
	timeout = 0,
	whileDead = 1,
}

StaticPopupDialogs['ZYGORGUIDESVIEWER_DEFAULT'] = {
	text = L['static_caption'].."%s",
	button1 = OKAY,
	hideOnEscape = 1,
	timeout = 0,
	whileDead = 1,
}

ZGV.loadtime = debugprofilestop()

function ZGV:OnInitialize()
	ZGV.db = LibStub("AceDB-3.0"):New("ZygorGuidesViewerSettings")

	assert(ZGV.version,"Ver.lua missing!")
	--print("zgv oninit")
	--if not CHECK then CHECK = 3 end

--	if not ZygorGuidesViewerMiniFrame then error("Zygor Guide Viewer step frame not loaded.") end

	ZGV.starttime = debugprofilestop()

	local t1=debugprofilestop()

	self:Debug ("Initializing...")

	if (GetCVar("scriptProfile")=="1") then
		self:Debug("Somebody set us up a bomb: Lua profiler is enabled. Disabling it on next reload")
		self.ProfilerMode=true
		SetCVar("scriptProfile","0")
	end

	self:Options_Initialize()

	if IsShiftKeyDown() then
		self.db.char.maint_startguides = false
		self.db.char.maint_queryquests = false
		self.db.char.maint_fetchquestdata = false
		self.db.char.maint_fetchitemdata = false
		ZygorGuidesViewerMaintenanceFrame_StartGuides:SetChecked(self.db.char.maint_startguides)
		ZygorGuidesViewerMaintenanceFrame_QueryQuests:SetChecked(self.db.char.maint_queryquests)
		ZygorGuidesViewerMaintenanceFrame_FetchQuestData:SetChecked(self.db.char.maint_fetchquestdata)
		ZygorGuidesViewerMaintenanceFrame_FetchItemData:SetChecked(self.db.char.maint_fetchitemdata)

		ZygorGuidesViewerMaintenanceFrame:Show()
	else
		self.db.char.maint_startguides = true
		self.db.char.maint_queryquests = true
		self.db.char.maint_fetchquestdata = true
		self.db.char.maint_fetchitemdata = true
	end

	self.db.char.completedQuests=nil --wipe and flush

	self.CurrentStepNum = self.db.char.step
	self.ClickedTime = GetTime()
	self.CurrentGuideName = self.db.char.guidename and self.db.char.guidename:gsub("ataclsym","ataclysm")  -- TODO: remove after a while. Ugly fix.

	self.briefstepexpansionlines = {}
	self.briefstepexpansionspeedlines = {}

	self.QuestCacheTime = 0
	self.QuestCacheUndertimeRepeats = 0
	self.StepCompletion = {}
	self.recentlyAcceptedQuests = {}
	self.recentlyLostQuests = {}
	self.recentlyCompletedQuests = {}
	self.LastSkip = 1

	self.quests = {}
	self.questsbyid = {}
	self.reputations = {}

	self.bandwidth = 0

	self.TomTomWaypoints = {}

	self.instantQuests = {}
	self.dailyQuests = self.dailyQuests or {}

	self.completionelapsed = 0
	self.completionintervallong = 1.0
	self.completionintervalmin = 0.01
	self.completioninterval = self.completionintervallong

	self:ClearRecentActivities() -- just to make sure they're not nils

	--self.AutoskipTemp = true

	self:SetSkin(self.db.profile.skin,self.db.profile.skinstyle)
	if not self.Frame then
		self:Print("Cannot initialize skin: "..self.db.profile.skin.."... Resetting skin.")
		self:SetSkin()
		assert(self.Frame,"Cannot initialize any skin, sorry")
	end
	ZygorGuidesViewerFrame = self.Frame
	self.Frame:SetShown(self.db.profile.visible)

	self.frameNeedsResizing = 0

	self.Frame:SetScale(self.db.profile.framescale)
	self.Frame:AlignFrame()
	self:UpdateLocking()
	self:ReanchorFrame()

	--LibSimpleOptions.AddOptionsPanel("Zygor's Guide",function(self) MakeOptionsControls(self,ZGV.options,ZGV) end)
	--LibSimpleOptions.AddSuboptionsPanel("Zygor's Guide",ZGV.options.args.map.name, function(self) MakeOptionsControls(self,ZGV.options.args.map,ZGV) end)
	--LibSimpleOptions.AddSuboptionsPanel("Zygor's Guide",ZGV.options.args.addons.name, function(self) MakeOptionsControls(self,ZGV.options.args.addons,ZGV) end)
	--LibSimpleOptions.AddSlashCommand("Zygor's Guide","/zygoropt")

	self:Debug ("Initialized in %.2f",debugprofilestop()-t1)

	if self.Pointer then self.Pointer:Startup() end
	if self.Foglight then self.Foglight:Startup() end

	-- Making sure no Bag Addon can see our dearest minimap icons
	local trueMinimapGetChildren=Minimap.GetChildren
	assert(trueMinimapGetChildren) -- if that ever breaks we might need to reschedule the operation or something
	function Minimap:GetChildren()
		local res={ trueMinimapGetChildren(self) }
		for k,v in pairs(res) do
			if v.isZygorWaypoint then
				table.remove(res,k) -- Nothing to see here, move along
			end
		end
		return unpack(res)
	end

	--[[
	if ZygorTalentAdvisor and ZygorTalentAdvisor.revision > self.revision then
		self.revision = ZygorTalentAdvisor.revision
		self.version = ZygorTalentAdvisor.version
		self.date = ZygorTalentAdvisor.date
	end
	--]]

	-- home detection, fire-and-forget style.
	hooksecurefunc("ConfirmBinder",function() ZygorGuidesViewer.recentlyHomeChanged=true end)

	if addonName:find("DEV") then self.DEV=true end
	if addonName:find("BETA") then self.DEV=true ZygorGuidesViewerFrame_DevLabel:SetText("BETA") end

	if self.DEV then
		ZGV.DebugFrame = ZGV.ChainCall(CreateFrame("FRAME","ZygorDebugFrame",UIParent)) :SetPoint("TOPLEFT") :SetSize(1,1) .__END
		ZGV.DebugFrame.text1 = ZGV.ChainCall(ZGV.DebugFrame:CreateFontString()) :SetPoint("TOPLEFT") :SetFontObject(SystemFont_Tiny) .__END
		if self.db.profile.fpsgraph then
			ZGV:StartFPSFrame()
		end
	end
end

function ZGV:OnEnable()

	self:Debug("Enabling...")

	local t1=debugprofilestop()

	ZygorGuidesViewerMapIcon:Show()

	self:UpdateMapButton()
	self:UpdateSkin()

	self:AddEvent("UNIT_INVENTORY_CHANGED")

	-- combat detection for hiding in combat
	self:AddEvent("PLAYER_REGEN_DISABLED")
	self:AddEvent("PLAYER_REGEN_ENABLED")
	self:AddEvent("WORLD_MAP_UPDATE")

	self:AddEvent("SPELL_UPDATE_COOLDOWN")

	self:AddEvent("PLAYER_CONTROL_GAINED")  -- try to force current zone updates; should prevent GoTo lines from locking up after a taxi flight

	self:AddEvent("QUEST_LOG_UPDATE")

	self:AddEvent("PLAYER_ENTERING_WORLD")  -- cache current map id
	self:AddEvent("ZONE_CHANGED")
	self:AddEvent("ZONE_CHANGED_INDOORS")
	self:AddEvent("ZONE_CHANGED_NEW_AREA")
	self:AddEvent("MAP_BAR_UPDATE")

	self:AddEvent("COMBAT_LOG_EVENT_UNFILTERED")
	self:AddEvent("LOADING_SCREEN_DISABLED")

	self:AddEvent("TAXIMAP_OPENED")

	if self.db.profile.poketarget then
		self:AddEvent("UPDATE_MOUSEOVER_UNIT")
		self:AddEvent("CURSOR_UPDATE")
	end

	self:Hook_QuestChoice()

	-- startup 'modules'
	for i,startup in ipairs(self.startups) do
		startup(self)
	end

	self:SetWaypointAddon(self.db.profile.waypointaddon)

	--self.Localizers:PruneNPCs()  -- off until we start doing it by data, not by name. ~sinus 2013-04-09

	self.Log.entries = self.db.char.debuglog
	self.Log:Add("Viewer started. ---------------------------")

	self.Parser.ConditionEnv:_Setup()

	-- waiting for QUEST_LOG_UPDATE for true initialization...
	--self:QueryQuests()

	if ZGV_DEV then ZGV_DEV() end

	if self.db.profile.frame_anchor then
		self.db.profile.frame_anchor[2]=UIParent
		self.Frame:GetParent():ClearAllPoints()
		self.Frame:GetParent():SetPoint(unpack(self.db.profile.frame_anchor))
	end

	self:Debug("Enabled in %.2f ms",debugprofilestop()-t1)

	self.loading=""
end

function ZGV:OnDisable()
--	self:UnregisterAllEvents()
	UnsetWaypointAddon()

	ZygorGuidesViewerMapIcon:Hide()
	self.Frame:Hide()
end


local function _StartupThread()
	local self=ZGV
	if not self.db.char.maint_startguides then return end
	self.loading="Loading..."

	-- TODO remove this line. Blizzard calls a secure function in UpdateMicroButton that is called by functions we use. Don't need to update the micro buttons on world map open so overwrite it.
	-- Introduced in 5.4.1, will likely be patched on Blizzard's end but until then.
	setfenv(WorldMapFrame_OnShow, setmetatable({UpdateMicroButtons=function() end }, { __index = _G}))

	--[[
	if self.ChainsRaw then
		self.loading = L['loading_quests']
		self:Debug("chains start")
		self:ParseQuestChains_yielding()
		self:CreateReverseQuestChains_yielding()
		self.ChainsRaw=nil
		self:Debug("chains stop")
		yield(1)
	end
	--]]

	self.registeredmapspotsets = {}

	local full_load = self.db.profile.loadguidesfully

	if #self.registeredguides>0 then
		self.loading = L['loading_guides']
		for i,guide in ipairs(self.registeredguides) do
			guide:Parse(full_load)
			--if guide.type=="pet" or guide.type=="mount" then
			--	guide:Parse(true) -- Those guys are useful for detector
			--end
			if i%50==0 then
				self.loadprogress = i/#self.registeredguides
				self:SendMessage("ZYGORGV_LOADING")
				yield("parsing "..(full_load and "fully" or "headers"))
			end
		end
		yield("parsing "..(full_load and "fully" or "headers"))

		for i,guide in ipairs(self.registeredguides) do
			if guide.startlevel and not guide.endlevel and guide.next then
				local nextg = self:GetGuideByTitle(guide.next)
				if nextg and nextg.startlevel then
					guide.endlevel = nextg.startlevel
				end
			end
			--[[
			-- too fast for progress, eh?
			self.loadprogress = i/#self.registeredguides
			self:SendMessage("ZYGORGV_LOADING")
			yield()
			--]]
		end

		yield("startleveling")
	end


	-- Compare two guides or guide groups. Sort wisely according to either registered_sortings or registration order (first-come-first-serve)
	local function CompGroups(a,b)
		local aname=ZGV.GuideTitles[a.name]
		local bname=ZGV.GuideTitles[b.name]

		local sa=ZGV.registered_sortings[aname or a.short]
		local sb=ZGV.registered_sortings[bname or b.short]
		if sa and sb then return sa<sb else return a.ord<b.ord end
	end

	local function SortGroups(group)
		table.sort(group.groups,CompGroups)
		table.sort(group.guides,CompGroups)
		for i,gr in ipairs(group.groups) do
			SortGroups(gr)
		end
	end


	-- sort guides, according to preset sortings.
	SortGroups(self.registered_groups)
	yield("sorting groups")

	if #self.registeredmapspotsets>0 then
		for i,guide in ipairs(self.registeredmapspotsets) do
			if guide.rawdata then
				local parsedset,err,line,linedata = self.MapSpotSetProto.ParseRaw(guide)
				if parsedset then
					self.loadprogress = i/#self.registeredmapspotsets
					guide:Show()
				else
					if err then
						self:Print(L["message_errorloading_full"]:format(guide.title,line,linedata,err))
					else
						self:Print(L["message_errorloading_brief"]:format(guide.title))
					end
					guide.rawdata=nil
				end
				yield()
			end
			self:SendMessage("ZYGORGV_LOADING")
		end

		local tab1 = self.Frame.Border.Tab1
		--tab1:SetPoint("LEFT",self.Frame.Border,"TOPLEFT",65,-12)
		tab1:SetText("Guides")
		tab1:SetNormalFontObject(ZGVFTabFont)
		--PanelTemplates_TabResize(self.Tab1,0);
		--_G[self.Tab1:GetName().."HighlightTexture"]:SetWidth(self.Tab1:GetTextWidth() + 20);
		tab1:SetScript("OnClick",function() ZGV:SetDisplayMode("guide") end)
		tab1:Show()

		local tab2 = self.Frame.Border.Tab2
		tab2:SetPoint("LEFT",tab1,"RIGHT")
		tab2:SetText("Spots")
		tab2:SetNormalFontObject(ZGVFTabFont)
		--ZGVFrameTab2Text:SetText("Spots")
		--PanelTemplates_TabResize(self.Tab2,0);
		--_G[self.Tab2:GetName().."HighlightTexture"]:SetWidth(self.Tab2:GetTextWidth() + 20);
		tab2:SetScript("OnClick",function() ZGV:SetDisplayMode("gold") end)
		tab2:Show()

		self.Frame:AlignFrame()
	end
	yield("map spots")

	-- WIPE!
	--self.ParseQuestChains=nil
	--self.CreateReverseQuestChains=nil
	self.RegisterGuide=nil
	self.RegisterMapSpots=nil

	self:SendMessage("ZYGORGV_LOADING")

	self:Debug("Guides loaded. ---------")

	self:Debug("Querying for quests...")
	self:QueryQuests()

	--[[
	self:Debug("Caching follow-ups...")
	self:CacheMentionedFollowups()
	self:Debug("Cached.")
	yield(1)
	--]]

	self.loading="Cleaning up..."
	collectgarbage("step",10000)
	yield("garbage collected? why?")

	self.MagicKey:CreateFrame()

	self.completiontimer = self:ScheduleRepeatingTimer("TryToCompleteStep", 0.1)
	self.magickeytimer = self:ScheduleRepeatingTimer("SetMagicKey", 0.2)
	self.maptimer = self:ScheduleRepeatingTimer("CacheCurrentMapID", 1.0)

	--if self.DEV then self:ScheduleRepeatingTimer("ThunderStageForceUpdate", 10.0) end -- TEMPORARY TIMING OF THUNDER ISLE

	--self.notetimer = self:ScheduleRepeatingTimer("SetWaypoint", 1)
	--self.dailytimer = self:ScheduleRepeatingTimer("QuestTracking_ResetDailies", 5)

	--self:CancelTimer(self.startuptimer,true)

	self.pause = true
	self.loading=nil  -- disable startup steps
	self.guidesloaded=true  -- completely disable the startup thread

	if #self.ParseLog>0 then
		self:ShowDump(self.ParseLog,"Errors in guides",true)
	end

	self:Print(L['welcome_guides']:format(#self.registeredguides))

	self:UpdateFrame(true)

	self:OnFirstQuestLogUpdate()

	self:Debug("Loading time - guides: %.2f",self.loading_time_guides or -1)
	self:Debug("Loading time - DEV: %.2f",self.loading_time_DEV or -1)
	self:Debug("Loading time - total: %.2f",self.loading_time_total or -1)

	return "end"
end

-- This gets called every frame on startup, by MasterFrame. Needs to return true to confirm successful startup.
local thread
local startup_time=0
local STARTUP_INTENSITY=50
local startup_frames,startup_ticks=0,0
local last_gettime
ZGV.startuptimes={}
function ZGV:StartupStep()
	if not last_gettime then last_gettime=GetTime() end
	if last_gettime==GetTime() then return end  -- ah-ha, NOT loaded then!
	if not self.loading_screen_disabled then return end

	if not thread then
		thread = coroutine.create(_StartupThread)
		self:Debug("&startup It starts...")
	end

	startup_frames = startup_frames + 1

	local thistime=0
	while thistime<STARTUP_INTENSITY do
		local t = debugprofilestop()
		local good,ret = resume(thread)
		t = debugprofilestop()-t		startup_ticks = startup_ticks + 1		thistime = thistime + t		startup_time=startup_time + t
		self:Debug("&startup &_SUB0 Startup frame %d tick %d |cffeeff88%s|r took |cffffeeaa%d|rms",startup_frames,startup_ticks,tostring(ret),t)

		self.startuptimes[ret]=(self.startuptimes[ret] or 0) + t

		if not good then
			self.loading=nil
			self:Print("ERROR initializing, check the Lua errors and report them, please.")
			error("ERROR in startup frame ".. startup_frames ..": ".. tostring(ret) .."\nin\n".. debugstack(thread))
			ZygorGuidesViewerFrameMaster:SetScript("OnUpdate",nil)
			break
		elseif status(thread)=="dead" then
			self:Debug("Startup complete in %.2f (%d ticks in %d frames)",startup_time,startup_ticks,startup_frames)
			self:Debug("From file load to variables = %.2f",ZGV.starttime-ZGV.loadtime)
			self:Debug("Total startup (with distractions) = %.2f",debugprofilestop()-ZGV.starttime)
			self.loading=nil
			ZygorGuidesViewerFrameMaster:SetScript("OnUpdate",nil)
			break
		elseif ret and ret==1 then
			self.master_forceupdate=true  -- that doesn't do anything anymore...
			self.loading=nil
			break
		else
			-- just happily continue
		end
	end
end

function ZGV:LOADING_SCREEN_DISABLED()
	self:Debug("&startup LOADING_SCREEN_DISABLED! Let's go!")
	self.loading_screen_disabled=true
end



-- my event handling. Multiple handlers allowed, just for the heck of it.

local meta_newtables = {__index = function(tbl, key) tbl[key] = {} return tbl[key] end}
ZGV.Events=setmetatable({},meta_newtables)
function ZGV:AddEvent(event,func)
	tinsert(self.Events[event],func or true)
	if #self.Events[event]==1 then self:RegisterEvent(event,"EventHandler") end
end

function ZGV:AddMessage(event,func)
	tinsert(self.Events[event],func or true)
	if #self.Events[event]==1 then self:RegisterMessage(event,"EventHandler") end
end

function ZGV:RemoveHandler(event,removefunc)
    for num,func in ipairs(self.Events[event]) do
        if func == removefunc then
            tremove(self.Events[event],num)
        end
    end
end

function ZGV:EventHandler(event,...)
	for i,hand in ipairs(self.Events[event]) do
		local func
		if type(hand)=="function" then
			func=hand
		elseif type(hand)=="string" then
			func=self[hand]
			assert(func,"No function "..hand.." in event handler!")
		elseif hand==true then
			func=self[event]
			assert(func,"No function "..event.." in event handler!")
		end
		func(self,event,...)
	end
end

function ZGV:OnFirstQuestLogUpdate()
	if not self.guidesloaded then return end -- let the OnGuidesLoaded func call us.
	if self.questLogInitialized then return end

	--if self.db.char["starting"] then
	if ZGV.db.char.maint_startguides then
		local history = self.db.char.stephistory
		self:SetGuide(self.db.char.guidename,self.db.char.step)
		self.db.char.stephistory = history

		if not self.CurrentGuide then
			self:Print("Finding proper starter section.")
			local gs = self:FindSuggestedGuides()
			if gs['LEVELING'] then gs=gs['LEVELING'] end
			if not gs or #gs==0 then
				self:Print("Cannot find a proper section for you! What are you..??")
				return
			elseif #gs==1 then
				local g=gs[1]
				self:SetGuide(g)
			else
				-- no auto-choice
			end
			--self.db.char["starting"] = false
		end
	end

	self.frameNeedsResizing = 1
	self:AlignFrame()
	self:UpdateFrame(true)
	self.questLogInitialized = true
end

ZGV.GuideTitles = {
	["SUGGESTED"]=L['guidepicker_suggested'],["RECENT"]=L['guidepicker_recent'],["SEARCH"]=L['guidepicker_searchresults'],
	["LEVELING"]=L['guidepicker_leveling'] ,
	["EVENTS"]=L['guidepicker_events'] ,
	["DAILIES"]=L['guidepicker_dalies'] ,
	["LOREMASTER"]=L['guidepicker_loremaster'] ,
	["GOLD"]=L['guidepicker_gold'] ,
	["PROFESSIONS"]=L['guidepicker_professions'] ,
	["PETSMOUNTS"]=L['guidepicker_pets'] ,
	["ACHIEVEMENTS"]=L['guidepicker_achievements'] ,
	["TITLES"]=L['guidepicker_titles'] ,
	["REPUTATIONS"]=L['guidepicker_reps'] ,
	["MACROS"]=L['guidepicker_macros'] ,
	["DUNGEONS"]=L['guidepicker_dungeon'] ,
	["GEAR"]=L['guidepicker_gear'] ,
}

function ZGV:GetGuideByTitle(title)
	title = ZGV:SanitizeGuideTitle(title)  -- code-side fix for "common" guides.
	for i,v in ipairs(self.registeredguides) do
		if v.title==title then return v end
	end
end

function ZGV:SetGuide(name,step,hack) --hack used for testing
	if not name then return end
	step=step or 1
	--self:Debug("SetGuide "..name.." ("..tostring(step)..")")

	local guide
	if type(name)=="number" then
		local num = name
		if self.registeredguides[num] then
			guide = self.registeredguides[num]
		else
			self:Print("Cannot find guide number: "..num)
			--return false
		end
	elseif type(name)=="string" then
		name = name :gsub("^Zygor's Alliance ","") :gsub("^Zygor's Horde ","")  -- code-side fix for "common" guides.
		:gsub("^Zygor's ","")

		guide = self:GetGuideByTitle(name)
		if not guide then
			self:Print("Cannot find guide: "..name)
			self:Debug("Cannot find guide: "..name)
			return false
		end
	else
		guide=name  --omg, object
	end

	--if guide.is_stored then guide = self.db.global.storedguides[name] end

	local err

	if guide then
		local status,msg = guide:GetStatus()
		if hack then status = "INVALID"; msg = "TESTING" end
		if status=="INVALID" and not ZGV.db.char.goodbadguides[guide.title] then
			if not self.BadGuidePopup then
				self.BadGuidePopup =  ZGV.PopupHandler:NewPopup("ZygorBadPopup","default")

				self.BadGuidePopup.OnAccept = function(self)
					ZGV.db.char.goodbadguides[self.guide.title]=true
					ZGV:SetGuide(self.guide,self.step)
				end

				self.BadGuidePopup.OnDecline = function(self)
					ZGV.Menu:Show(self.guide)
				end

				self.BadGuidePopup.noMinimize = 1 --Can not minimize this one
			end


			self.BadGuidePopup:SetText(L['static_badguide']:format(guide.title_short,msg or ""))

			self.BadGuidePopup.guide=guide
			self.BadGuidePopup.step=step

			self.BadGuidePopup:Show()
			return "BAD"
		end

		if self.CurrentGuide then self.CurrentGuide:Unload() end

		guide:Parse(true)

		if guide.steps then
			--self.MapNotes = _G["ZygorGuides_"..faction.."Mapnotes"]
			local name = guide.title

			self.CurrentGuide = guide
			self.CurrentGuideName = name
			self.db.char.guidename = name
			self.db.char.stephistory = {}

			self.CurrentStep = nil
			self.CurrentStepNum = nil

			self.LastSkip = 1

			local stepobj = guide:GetFirstValidStep(step) -- make sure it's valid
			if stepobj then
				--self:QuestTracking_ResetDailies(true)
				name=name:gsub(self.CurrentGuide.type,ZGV.GuideTitles[self.CurrentGuide.type]) -- make LEVELING-Leveling and such.
				self:Print(L["message_loadedguide"]:format(name,step))
				self:Debug("Guide loaded: "..name)

				self:SendMessage("ZYGORGV_GUIDE_LOADED",guide.title)

				self:FocusStep(stepobj.num)

				ZygorGuidesViewerFrame_Border_Guides_GuideButton:UnlockHighlight()
			end
		else
			err = "Guide not parsed"
		end
	else
		err = L["message_missingguide"]:format(name)
	end

	if err then
		self:Print(err)
		self.db.char['guide'] = nil
		self.db.char['step'] = nil
		self.CurrentGuide = nil
		self.CurrentStep = nil
	end

	self.pause = nil

	self:UpdateFrame(true)

	ZygorGuidesViewer_ProgressBar_Update()
end

function ZGV:FindSuggestedGuides()
	local suggested={}
	for i,guide in ipairs(self.registeredguides) do
		if guide:GetStatus()=="SUGGESTED" then
			if guide.condition_suggested_exclusive then
				return {guide}
				-- suggest-exclusive guides force their suggestion, if suggested.
			else
				if not suggested[guide.type] then suggested[guide.type]={} end

				tinsert(suggested[guide.type],guide)
			end
		elseif guide.sugGroup then
			if not suggested[guide.type] then suggested[guide.type] = {} end

			-- TODO can we do this without this work? Just look at registered_groups?

			local found
			-- Go through and see if this group has been added yet.
			for i,group in ipairs(suggested[guide.type]) do
				if group.name and group.name == guide.sugGroup then
					found = true
					break
				end
			end

			if not found then
				tinsert(suggested[guide.type],{groups={},guides={guide},name = guide.sugGroup, ord=1})
			end
		end
	end
	return suggested
end

function ZGV:GetGuideFolderInfo(folder)
	local suggest
	for i,guide in ipairs(folder.guides) do
		if guide:GetStatus()=="SUGGESTED" then suggest=true end
	end
	if not suggest then
		for i,group in ipairs(folder.groups) do
			if self:GetGuideFolderInfo(group) then suggest=true end
		end
	end
	return suggest
end

-- function ZGV:SearchForCompleteableGoal() --removed

function ZGV:ClearRecentActivities()
	if not self.recentlyVisitedCoords then --First time, intialize them all as tables
		self.recentlyVisitedCoords = {}
		self.recentlyCompletedGoals = {}
		self.recentlyCompletedQuests = {} --only for instant repeatables, which sucks.
		self.recentlyAcceptedQuests = {}
		self.recentlyStickiedGoals = {}
		self.recentGoalProgress = {}
		self.recentCooldownsPulsing = {}
		self.recentCooldownsStarted = {}
		self.recentlyHomeChanged = false
		self.recentlyDiscoveredFlightpath = false
		self.recentlyLearnedRecipes = {}
		self.recentKills = {}
	else --wipe them. Creating them new every time is silly.
		wipe(self.recentlyVisitedCoords)
		wipe(self.recentlyCompletedGoals)
		wipe(self.recentlyCompletedQuests) --only for instant repeatables, which sucks.
		wipe(self.recentlyAcceptedQuests)
		wipe(self.recentlyStickiedGoals)
		wipe(self.recentGoalProgress)
		wipe(self.recentCooldownsPulsing)
		wipe(self.recentCooldownsStarted)
		self.recentlyHomeChanged = false
		self.recentlyDiscoveredFlightpath = false
		wipe(self.recentlyLearnedRecipes)
		wipe(self.recentKills)
		-- self.completedQuestTitles = {} -- let's not use this anymore, with GetQuestID available
	end
end

function ZGV:FocusStep(num,quiet)
	if type(num)=="string" and self.CurrentGuide.steplabels then local s=num  num=self.CurrentGuide.steplabels[num]  if num then num=num[1] end  self:Debug("FocusStep: "..s.." = "..tostring(num))  end
	if type(num)=="table" then num=num.num end
	if not num or num<=0 then return end
	if not self.CurrentGuide then return end
	if not self.CurrentGuide.steps then return end
	if num>#self.CurrentGuide.steps then return end

	self:Debug("FocusStep "..num..(quiet and " (quiet)" or ""))

	-- Record step into history
	if self.LastSkip>0 and self.CurrentStep then
		tinsert(self.db.char.stephistory,self.CurrentStep.num)
	end

	self.CurrentStepNum = num
	self.db.char.step = num
	self.CurrentStep = self.CurrentGuide.steps[num]
	self.CurrentGuide.CurrentStepNum = num

	self:ClearRecentActivities()

	-- Whoa whoa. The step might load a different guide at this point! Play safe.
	local cs=self.CurrentStep
	local cg=self.CurrentGuide
	self.CurrentStep:PrepareCompletion(true)
	self.CurrentStep:OnEnter()
	if (cs~=self.CurrentStep) or (cg~=self.CurrentGuide) then self:Debug("FocusStep: guide or step changed! bailing.") return end

	self.stepchanged = true

	for i,goal in ipairs(self.CurrentStep.goals) do
		if goal:IsComplete() then self.recentlyCompletedGoals[goal]=true end
	end

	local stepcomplete,steppossible = self.CurrentStep:IsComplete()
	if self.pause then
		if (self.db.profile.skipimpossible and not steppossible)
		--or (self.db.profile.skipobsolete and self.CurrentStep:IsObsolete())
		or (self.db.profile.skipauxsteps and self.CurrentStep:IsAuxiliarySkippable())
		then
			stepcomplete=true
			--self.pause=nil
		end
		self.LastSkip=1
		if not stepcomplete then
			self:Debug("unpausing")
			self.pause=nil
		end
	end
	--and self.LastSkip~=0) then self.AutoskipTemp=false else self.AutoskipTemp=true end

	-- add to last-guides history
	-- add to last-guides history
	if not self.db.char.guides_history_ZGV4clear then self.db.char.guides_history={} self.db.char.guides_history_ZGV4clear=true end
	local history = self.db.char.guides_history
	local found
	for gtype,guide in pairs(history) do
		if gtype==self.CurrentGuide.type then
			-- move to front
			history[gtype]=nil
			history[gtype]={self.CurrentGuide.title,self.CurrentStepNum}
			found=true
			break
		end
	end
	if not found then
		history[self.CurrentGuide.type]={self.CurrentGuide.title,self.CurrentStepNum}
		--if #history>self.db.profile.guidesinhistory then tremove(history) end
	end

	-- TRACK QUESTS
	if ZGV.db.profile.autotrackquests then
		for gi,goal in ipairs(cs.goals) do
			if goal.questid then self:TrackQuest(goal.questid) break end
		end
	end


	-- SANITIZE MAPS. In case there's zoning involved.
	if cs.map then cs.map=self.Pointer:SanitizePhase(cs.map) end
	for gi,goal in ipairs(cs.goals) do
		if goal.map then goal.map=self.Pointer:SanitizePhase(goal.map) end
	end


	-- pre-fetch the next step's translation, this seems like a good place to do it
	local nextstep,stepnum,guide=self.CurrentStep:GetNextStep()
	if nextstep then nextstep:Translate() end


	self:SendMessage("ZYGORGV_STEP_CHANGED",num)

	if not quiet then
		self:FocusStepUnquiet()
	end

	self:TryToDisplayCreature()
	--self:UpdateMinimapArrow(true)

	--self:AnimateGears()
end

function ZGV:FocusStepQuiet(num)
	return self:FocusStep(num,true)
end

function ZGV:FocusStepUnquiet()
	self:StopFlashAnimation()
	self.frameNeedsResizing = self.frameNeedsResizing + 1
	self:UpdateFrame(true)
	self:ScrollToCurrentStep()
	self:UpdateCooldowns()

	--self:UpdateCartographerExport()  -- moved to Waypoints where it belongs.
	self:SetWaypoint(false)
	self:SetWaypoint()
end

function ZGV:TrackQuest(id)
	local q = ZGV.questsbyid[id]
	if not q or not q.inlog then return end
	if not IsQuestWatched(q.index) then
		AddQuestWatch(q.index)
		WatchFrame_Update(self)
	end
	SetSuperTrackedQuestID(q.id)
	WORLDMAP_SETTINGS.selectedQuestId = q.id
	QuestPOIUpdateIcons()
	QuestPOI_SelectButtonByQuestId("WatchFrameLines", WORLDMAP_SETTINGS.selectedQuestId, true)
	WorldMapFrame_SelectQuestById(q.id)
end

function ZGV:PointToQuest(id)
	local _,x,y,obj = QuestPOIGetIconInfo(id)
	if x and y then
		self:SetWaypoint(x,y)
	end
end

-- return step = step obj
-- return backed = num of valid history skips
function ZGV:GetPreviousValidStep()
	local step
	local hist = ZGV.db.char.stephistory
	local hlen = #hist
	local stepnum
	local backed=0
	local okaytostay
	repeat
		-- pop stepnum from history
		stepnum = hist[hlen-backed]

		-- valid number?
		if stepnum then
			-- history popped 'pop'erly, hurr durr

			-- get the step
			s = self.CurrentGuide.steps[stepnum]
			if s then
				backed = backed + 1
				step = s
			end
		else
			-- we broke history or it just ran out, whatever

			ZGV:Debug("step history broken, omg")


			-- TODO: Currently, when running out of history, we default to the first valid of the guide. Needs a message / confirmation.

			s = self.CurrentGuide:GetFirstValidStep()  -- always returns something, or breaks.
			if s then
				backed = hlen
				step = s
				okaytostay = true
			end
		end
	until (step:AreRequirementsMet() or self.db.profile.showwrongsteps) and (step~=self.CurrentStep or okaytostay)
	return step,backed
end

function ZGV:PreviousStep(fast)
	if not self.CurrentGuide then return end

	self.LastSkip = -1
	self.lastskip_rec = -1

	self.autopause = IsAltKeyDown() and IsControlKeyDown()

	local step,backed = self:GetPreviousValidStep()
	if self.autopause then step=self.CurrentGuide.steps[self.CurrentStepNum-1] end  -- HACK.
	if not step then return end

	self:Debug("PreviousStep to "..step.num..(fast and ' (fast)' or ''))

	-- drop 'backed' history states
	local history = self.db.char.stephistory
	for i=1,backed do tremove(history) end

	if #history==0 then
		self.fastforward = false
		self.pause = true
	else
		self.fastforward = fast
		self.pause = not fast
	end

	if self.autopause then self.pause=true self.fastforward=false end

	self:FocusStep(step)

	ZygorGuidesViewer_ProgressBar_Update()
end

function ZGV:SkipStep(fast,hack) --Hack used for testing.
	if not self.CurrentGuide then return end

	self.LastSkip = 1
	self.lastskip_rec = 1
	self.fastforward = fast

	local step

	self.autopause = IsAltKeyDown() and IsControlKeyDown()

	if self.autopause then  -- forced next+1 step
		step=self.CurrentGuide.steps[self.CurrentStepNum+1]
		self.CurrentStep.needsreload=nil
	end  -- HACK.

	if self.CurrentStep.needsreload then return self:ReloadStep(fast) end

	if not step then  -- when not forced, that is: usually.
		local step2,stepnum,guide

		if fast and self.db.profile.instantskip then step2,stepnum,guide=self.CurrentStep:GetNextCompletableStep()
		else step2,stepnum,guide=self.CurrentStep:GetNextValidStep() end  -- always returns a step, unless we're at the end.

		if guide then
			--print("ABOUT TO JUMP GUIDES to:",guide,stepnum)
			self:SetGuide(guide,stepnum)
			return
		end
		step=step2
	end

	if step and not hack then
		-- not last step
		self:Debug("SkipStep to ".. step.num..(fast and ' (fast)' or ''))

		if step.num == #self.CurrentGuide.steps then
			-- final step
			self.pause = true
		else
			self.pause = not fast
		end

		if self.autopause then self.pause=true end

		self:FocusStep(step)
	else
		-- last step! or something went wrong and GetNextValidStep couldn't find anything to hop onto.

		--if self.CurrentStep.num == #self.CurrentGuide.steps then  -- never mind! assuming loss of next step = end of guide. Wondering if this is safe... ~sinus 2011-08-16
			self.pause = true
			self.fastforward = false
			if self.CurrentGuide.next then
				self:SetGuide(self.CurrentGuide.next,1)
				return
			elseif self.CurrentGuide.steps and #self.CurrentGuide.steps>1 then
				if not self.EndGuidePopup then
					self.EndGuidePopup = ZGV.PopupHandler:NewPopup("ZygorEndPopup","default")

					self.EndGuidePopup.declinebutton:Hide()
					ZGV.ChainCall(self.EndGuidePopup.acceptbutton)
						:ClearAllPoints()
						:SetPoint("BOTTOM",self.EndGuidePopup,"BOTTOM",0,5)

					self.EndGuidePopup.noMinimize = 1 --Can not minimize this one
				end

				self.EndGuidePopup:SetText(L['dialog_endguide'])
				self.EndGuidePopup:Show()
				return
			end
		--else
		--	error("Missed the end of the guide..?")
		--end
	end

	ZygorGuidesViewer_ProgressBar_Update()
end

function ZGV:ReloadStep(fast)
	self.LastSkip=1
	self.pause = not fast
	self.fastforward = fast
	self.CurrentStep.needsreload = nil
	self:FocusStep(self.CurrentStepNum)
end

--- A quest is 'interesting' if any follow-ups to it appear anywhere in the guides and they're not gray.
-- As of 3.1, no follow-ups are tracked.

--[[
local followupcache={}
function ZGV:GetMentionedFollowups(questid)
	if followupcache[questid] then return followupcache[questid] end
	local q,f
	local live = {questid}
	local fups = {}
	local lev
	--self:Debug("Caching mentioned followups of "..questid)
	local cycles=0
	while #live>0 do
		cycles=cycles+1
		assert(cycles<1000,"Quest "..questid.." has infinitely resolving live followups: "..table.concat(live,","))

		q = tremove(live,1)
		lev = self.mentionedQuests[q]
		if lev then tinsert(fups,{q,lev}) end

		f = self.RevChains[q]
		if f then
			for i=1,#f do
				-- make sure there are no circular references
				if f[i]==questid then break end
				local found
				-- don't add stuff that's already in the live group
				for l=1,#live do if live[l]==f[i] or live[l]==questid then found=true break end end
				-- and don't add stuff that's already in followups
				if not found then for l=1,#fups do if fups[l][1]==f[i] then found=true break end end end
				if found then break end
				tinsert(live,f[i])

				--error("Circular quest reference: "..q.." requires "..f[i]..", already required by "..questid)  -- screw it, they MAY require old quests, just not in circles. Fix circles quietly.
			end
		end
		assert(#live<1000,"Quest "..questid.." has live followups > 1000: "..table.concat(live,","))
	end
	followupcache[questid]=fups
	return fups
end

-- A quest's "maximum chained level" can be safely cached, I guess.
-- MAY YIELD.
function ZGV:CacheMentionedFollowups()
	local f,maxlev
	self.maxQuestLevels = {}
	local count=0
	for qid,lev in pairs(self.mentionedQuests) do
		--self.loadprogress=count/30000
		count=count+1
		if count>100 then count=0 yield() end
		f=ZGV:GetMentionedFollowups(qid)
		for i=1,#f do
			if f[i][2]>lev then lev=f[i][2] end
		end
		self.maxQuestLevels[qid]=lev
	end
end
--]]

--- Attempt to complete current step.
-- 09-09-24:
local lastcompletion=0
local lastnextsuggested
function ZGV:TryToCompleteStep(force)
	if not self.CurrentStep or not self.CurrentGuide then return end

	if self.BUTTONS_INLINE then
		if self.actionsvisible and InCombatLockdown() then return end
	end

	-- prevent overtime checks
	local t=GetTime()
	local elapsed=t-lastcompletion
	lastcompletion=t

	local interval=self.completioninterval
	self.completionelapsed=self.completionelapsed+elapsed

	if self.completionelapsed<interval and not force then
		-- what the hell was this for?
		--[[
		if self.db.profile.displaymode then
			self:UpdateFrame()
		end
		--]]
		return
	end
	self.completionelapsed = 0

	-- frame hidden? bail.
	if not self.Frame:IsVisible() or self.Frame:GetAlpha()<0.1 then return end

	local stepcomplete,steppossible = self.CurrentStep:IsComplete()

	local completing = stepcomplete



	-- smart skipping: treat invalid or impossible or skippable as completed
	if not self.CurrentStep:AreRequirementsMet()
	or (self.db.profile.skipimpossible and not steppossible)
	--or (self.db.profile.skipobsolete and self.CurrentStep:IsObsolete())
	or (self.db.profile.skipauxsteps and self.CurrentStep:IsAuxiliarySkippable()) then
		completing=true
		--self.pause=nil
	end

	if not completing then
		interval = self.completionintervallong
		self.pause=nil
	end

	local confirmcompleted = false
	local confirmfound = false
	for i,goal in ipairs(self.CurrentStep.goals) do
		if goal.action == "confirm" and goal.always then
			confirmfound = true
			if goal.status == "complete" then confirmcompleted = true end
		end
	end
	if confirmfound and confirmcompleted ~= true then completing = false end

	if self.pause or self.db.profile.dontprogress then
		interval = self.completionintervallong
		self.LastSkip = 1
	else
		if completing then
			--self.recentlyCompletedQuests = {} -- forget it! We're skipping the step, already.
			self:Debug("Skipping step: "..self.CurrentStepNum.." ("..(stepcomplete and "complete" or (steppossible and "possible?" or "impossible"))..")")
			local s=""
			for gn,goal in ipairs(self.CurrentStep.goals) do s=s.."["..gn.."] ".. goal:GetText().." ("..(goal.status or "").."),  " end
			self:Debug("Skipped goals were: %s",s)

			if self.lasttriedstep and self.lasttriedstep==self.CurrentStep and not self.lastwascompleted then
				--newly completed!
				PlaySound(self.db.profile.completesound)
				if self.db.profile.flashborder then
					self.delayFlash=1
				end
			end

			-- do, do, do the SKIP!

			if self.LastSkip<0 then self:PreviousStep(true) else self:SkipStep(true) end
			self.fastforward=true

			interval = interval * 0.8
			if interval<self.completionintervalmin then interval=self.completionintervalmin end
			--skipped=skipped+1
			--if skipped>100 then break end

			--self:UpdateFrame()
			--updated=true

			--self.completioninterval = self.completionshortinterval


			--ZygorGuidesViewerFrame_CoverFlash_blink:Play()

			--stepcomplete = self.CurrentStep:IsComplete()
		else
			interval = self.completionintervallong
			self.pause=nil
			self.fastforward=nil
			self.LastSkip = 1
			--self.completioninterval = self.completionlonginterval
		end

		--[[
		if updated and not self.db.profile.showallsteps then
			self.stepframes[1].slideup:Play()
		end
		--]]

		--if not stepcomplete then self.AutoskipTemp=true end

		--if not updated then self:UpdateFrame() end
	end

	self:MaybeSuggestNextGuide()

	self:UpdateFrame()

	self.lasttriedstep = self.CurrentStep
	self.lastwascompleted = stepcomplete

	self.completioninterval = interval
end

function ZGV:MaybeSuggestNextGuide()
	--if self.CurrentGuide.title:find("Pandaria 85") then return end  -- don't suggest when in there. simple. -- 6/25/2013 We can suggest Pandaria again.
	-- And now check if the next guide is up for suggesting.
	-- However, don't bother suggesting others when we're exclusive and still suggested.
	if self.CurrentGuide:GetStatus()~="SUGGESTED" or not self.CurrentGuide.condition_suggested_exclusive then
		local nextguide = self.CurrentGuide.next
		if nextguide and not ZGV.db.char.ignoredguides[nextguide] and not(ZGV.tempguideblock and ZGV.tempguideblock[nextguide])then
			nextguide = self:GetGuideByTitle(nextguide)
			if nextguide then
				local nextsuggested = (nextguide:GetStatus()=="SUGGESTED")
				ZGV.suggesting = nextsuggested
				if not lastnextsuggested and nextsuggested and self.db.profile.n_popup_sis then -- plain guide popup block is in AWP
					nextguide:AdvertiseWithPopup()
				end
				lastnextsuggested = nextsuggested
			end
		end
	end
end

function ZGV:InitializeDropDown(frame)
	if not self.guidesloaded then return end

	local guides = ZygorGuidesViewer.registeredguides

	if not guides then return end

	for i,guide in ipairs(guides) do

--		ChatFrame1:AddMessage(section)
		local info = {}
		info.text = guide.title
		info.value = guide.title
		info.func = ZGVFSectionDropDown_Func
		if (self.CurrentGuideName == guide.title) then
			info.checked = 1
		else
			info.checked = nil
		end
		info.button = 1
--		if (i == 1) then
--			info.isTitle = 1
--		end
		UIDropDownMenu_AddButton(info)
	end
	UIDropDownMenu_SetText(frame, self.CurrentGuideName)
end


function ZGV:UpdateLocking()
	self.Frame:UpdateLocking()
end

function ZGV:StopFlashAnimation()
	if not self.stepframes[20] or not self.stepframes[20].lines[20] then return end
	for s=1,20 do
		for l=1,LINES_PER_STEP do
			local anim_w2g = self.stepframes[s].lines[l].anim_w2g
			if not anim_w2g then break end
			anim_w2g:Stop()
		end
	end
end

--[[
function ZGV:HideCooldown(arg)
	arg.cooldown:Hide()
	self.recentCooldownsPulsing[goal] = 2
end
--]]

function ZGV:UpdateCooldowns()
	--self:Debug("UpdateCooldowns")
	if not self.CurrentStep then return end
	local stepframe = self.stepframes[self.CurrentStepframeNum]
	if not stepframe then return end
	for i=1,LINES_PER_STEP,1 do
		local cooldown = self.actionbuttons[i].cd
		if not cooldown then return end
		local goal = stepframe.lines[i].goal
		if goal and goal:IsActionable() then
			--cooldown:Show()
			--self:Debug("goal "..i.." actionable")
			if goal.castspell or goal.castspellid then
				local start,dur,en = GetSpellCooldown(goal.castspellid or goal.castspell)
				CooldownFrame_SetTimer(cooldown, start, dur, en)
				if start>0 then cooldown:Show() else cooldown:Hide() end
				--self:Debug(("spell: %d,%d,%d"):format(start,dur,en))
			elseif goal.itemid then
				local start,dur,en = GetItemCooldown(goal.itemid)
				CooldownFrame_SetTimer(cooldown, start, dur, en)
				if start>0 then cooldown:Show() else cooldown:Hide() end
				--self:Debug(("item: %d,%d,%d"):format(start,dur,en))
			elseif goal.petaction then
				local num,name,x,tex
				if type(goal.petaction)=="number" then
					num = goal.petaction
				else
					num,name,x,tex = FindPetActionInfo(goal.petaction)
				end
				local start,dur,en = GetPetActionCooldown(num)
				CooldownFrame_SetTimer(cooldown, start, dur, en)
				if start>0 then cooldown:Show() else cooldown:Hide() end
			end
		else
			cooldown:Hide()
		end
	end
end

local function gradient(a,b,p)
	return a+(b-a)*p
end

local function fromRGBA(ob)
	return ob.r,ob.g,ob.b,ob.a
end

local function fromRGB_a(ob,a)
	return ob.r,ob.g,ob.b,a
end

local function fromRGBmul_a(ob,mul,a)
	return ob.r*mul,ob.g*mul,ob.b*mul,a
end

local function fromRGB(ob)
	return ob.r,ob.g,ob.b
end

--local function gradientRGBA(f,t,p)  --removed

function ZGV:SetDisplayMode(mode)
	self.db.profile.displaymode=mode
	self:UpdateFrame(true)
end

local Tpi=6.2832
local cardinals = {"N","NW","W","SW","S","SE","E","NE","N"}
local function GetCardinalDirName(angle)
	for i=1,9 do
		if Tpi*((i*2)-1)/16>angle then return cardinals[i] end
	end
end
function GetCardinalDirNum(angle)
	while angle<0 do angle=angle+Tpi end
	while angle>Tpi do angle=angle-Tpi end
	local ret=1
	for i=1,16 do
		if Tpi*((i*2)-1)/32>angle then ret=i break end
	end
	return ret
end

local itemsources={"vendor","drop","ore","herb","skin"}

local gold_ox,gold_oy=0,0

local actionicon={
	["accept"]=5,
	["turnin"]=6,
	["kill"]=7,
	["from"]=7,
	["get"]=8,
	["collect"]=8,
	["buy"]=8,
	["goal"]=9,
	["home"]=10,
	["fpath"]=11,
	["goto"]=12,
	["talk"]=13,
	["next"]=14
}
setmetatable(actionicon,{__index=function() return 2 end})

function ZGV:UpdateFrame(full,onupdate)
	if full then self.stepchanged=true end

	if not self.Frame or not self.Frame:IsVisible() then return end

	--if InCombatLockdown() then return end
	--[[
	--		self.Frame:SetAlpha(0.5)
		return
	else
	--		self.Frame:SetAlpha(1.0)
	end
	--]]

	--self:Debug("updatemini")

	--if ZygorGuidesViewerMiniFrame_bdflash:IsPlaying() and not ZygorGuidesViewerMiniFrame_bdflash:IsDone() then return end

	self.db.profile.displaymode="guide"

	local minh = 0

	local Scroll = ZygorGuidesViewerFrame_Scroll

	local tabh = ZygorGuidesViewerFrame_Border_TabBack:GetHeight()

	if self.loading then

		ZygorGuidesViewerFrame_Border_Guides_GuideBack_SectionTitle:SetText(self.loading:format((self.loadprogress or 0)*100))

	elseif self.db.profile.displaymode=="guide" then

		local showbriefsteps = self.db.profile.showbriefsteps-- and self.db.profile.minimode

		self.Frame.Border.Guides:Show()
		self.Frame.Border.Gold:Hide()

		if self.CurrentGuide and self.CurrentGuide.steps and self.CurrentGuide.fully_parsed then

			-- hide spot frames, if visible
			if self.spotframes[1] and self.spotframes[1]:IsVisible() then for i,spotframe in ipairs(self.spotframes) do spotframe:Hide() end end

			local showallsteps = (self.db.profile.showcountsteps==0)
			if showallsteps then
				if ZygorGuidesViewerFrame_ScrollScrollBar:GetValue()<1 then ZygorGuidesViewerFrame_ScrollScrollBar:SetValue(self.CurrentStepNum) end
				ZygorGuidesViewerFrame_ScrollScrollBar:Show()
			else
				ZygorGuidesViewerFrame_ScrollScrollBar:Hide()
			end

			if full then
				ZygorGuidesViewerFrame_ScrollScrollBar:SetMinMaxValues(1,#self.CurrentGuide.steps>0 and #self.CurrentGuide.steps or 1)
				ZygorGuidesViewerFrame_Border_Guides_StepNum.Step:SetText(self.CurrentStepNum)
				ZygorGuidesViewerFrame_Border_Guides_GuideBack_SectionTitle:SetText(self.CurrentGuide.title_short)
			end

			Scroll:Show()
			--ZygorGuidesViewerFrame_MissingText:Hide()

			local totalheight = 0

			local frame
			local stepnum,stepdata

			local firststep = (showallsteps and math.floor(ZygorGuidesViewerFrame_ScrollScrollBar:GetValue()) or self.CurrentStepNum) or 1
			if firststep<1 then firststep=1 end
			local laststep = showallsteps and #self.CurrentGuide.steps or self.CurrentStepNum+self.db.profile.showcountsteps-1

			--self:Debug("first step "..firststep..", last step "..laststep)
			-- run through buttons and assign steps for them

			local nomoredisplayed=false

			local icon_indent = self.db.profile.goalicons and ICON_INDENT or 0

			for stepbuttonnum = 1,self.StepLimit do repeat
				--frame = _G['ZygorGuidesViewerFrame_Step'..stepbuttonnum]
				frame = self.stepframes[stepbuttonnum]

				stepnum = firststep + stepbuttonnum - 1

				-- show this button at all?
				if stepnum>=firststep and stepnum<=laststep and stepnum<=#self.CurrentGuide.steps then
					local stepdata = self.CurrentGuide.steps[stepnum]
					assert(stepdata,"UpdateFrame: No data for step "..stepnum)

					if nomoredisplayed then
						frame:Hide()
						break --continue
					end

					--[[
					if not self.stepchanged and not stepdata:NeedsUpdating() or (nomoredisplayed and not frame:IsVisible()) then
						break --continue
					end
					--]]
					--print("Displaying step "..stepnum)

					frame.stepnum = stepnum
					frame.step = stepdata

					stepdata:PrepareCompletion()

					--#### position step frame

					frame:SetWidth(showallsteps and ZygorGuidesViewerFrame_ScrollChild:GetWidth() or Scroll:GetWidth()) -- this is needed so the text lines below can access proper widths

					-- out of screen space? bail.
					-- but only in all steps mode!
					local top=frame:GetTop()
					local bottom=Scroll:GetBottom()
					if showallsteps and top and bottom and top<bottom then
						frame:Hide()
						nomoredisplayed=true
						break --continue!
					end

					--#### fill it with text

					local changed,dirty = stepdata:Translate()
					if dirty then
						self.frameNeedsUpdating=true
					end
					if changed and not dirty then
						self:SetWaypoint()
					end

					local line=1

					frame.lines[line].label:ClearAllPoints()

					local did_header
					if (stepdata.requirement or self.db.profile.stepnumbers) then
						local numbertext = self.db.profile.stepnumbers and L['step_num']:format(stepnum)
						local reqtext = stepdata.requirement and ((stepdata:AreRequirementsMet() and "|cff44aa44" or "|cffbb0000") .. "(" .. (table.concat(stepdata.requirement,L["stepreqor"])):gsub("!([a-zA-Z ]+)",L["req_not"]:format("%1")) .. ")")
						local leveltext = (stepdata.level and stepdata.level>0 and self.db.profile.stepnumbers) and L['step_level']:format(stepdata.level or "?")
						local titletext = stepdata:GetTitle()  titletext=titletext and " "..titletext

						if showbriefsteps and not reqtext then
							frame.lines[line].briefhidden = true
						end

						if (numbertext or leveltext or reqtext or titletext) then
							frame.lines[line].label:SetPoint("TOPLEFT")
							frame.lines[line].label:SetPoint("TOPRIGHT")
							frame.lines[line].label:SetText((numbertext or "")..(leveltext or "")..(reqtext or "")..(titletext or ""))
							--frame.lines[line].label:SetMultilineIndent(1)
							frame.lines[line].goal = nil
							frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsecsize))
							frame.lines[line].icon:Hide()
							frame.lines[line].back:Show()
							-- TODO how about we let skin decide?
							frame.lines[line].back:SetBackdropColor(0,0,0,0.3)
							frame.lines[line].back:SetBackdropBorderColor(0,0,0,0.3)
							frame.lines[line].isheader=true
							line=line+1
							did_header=true
						end
					end
					if not did_header then
						frame.lines[line].label:SetPoint("TOPLEFT",frame.lines[line],"TOPLEFT",icon_indent+2,0)
						frame.lines[line].label:SetPoint("TOPRIGHT")
						frame.lines[line].label:SetFont(FONT,self.db.profile.fontsize)
						frame.lines[line].back:Hide()
						frame.lines[line].isheader=nil
					end

					if (stepdata:AreRequirementsMet() or self.db.profile.showwrongsteps) then
						--#### insert goals

						local goals = stepdata.goals

						for i,goal in ipairs(goals) do

							if full then goal.dirtytext = true end

							local status = goal:GetStatus()

							if status=="hidden" and not self.db.profile.showwrongsteps then
								-- don't display the line, simple

							elseif self.db.profile.collapsecompleted and goal:CanBeIndentHidden() and not stepdata:IsComplete() then
								-- collapse the line, if completed children say so

							else
								local briefhidden = showbriefsteps
									and (
										not goal:IsCompleteable()
										or (self.db.profile.hidecompletedinbrief and status=="complete" and not stepdata:IsComplete())
									)
									and not goal.showinbrief
								--steptext = steptext .. ("  "):rep(goal.indent or 0) .. goal:GetText() .. "|n"
								local indent = ("  "):rep(showbriefsteps and 0 or (goal.indent or 0))
								--local goaltxt = goal:GetText(stepnum>=self.CurrentStepNum)
								--local goaltxt = goal:GetText(true,self.db.profile.showbriefsteps and (self.briefstepexpansion<=0.1 --[[or stepdata~=self.briefstepexpanded--]]))
								local goaltxt = goal:GetText(true,showbriefsteps and ((self.briefstepexpansionlines[stepbuttonnum] or 0)<=0.1 --[[or stepdata~=self.briefstepexpanded--]]))

								if self.db.profile.showwrongsteps and status=="hidden" then goaltxt = "|cff880000[*BAD*]|r "..goaltxt end

								if goaltxt~="?" and goaltxt~="" then
									local link = ((goal.tooltip and not self.db.profile.tooltipsbelow) or (goal.x and not self.db.profile.windowlocked) or goal.image) and " |cffdd44ff*|r" or ""
									frame.lines[line].label:SetFont(FONT,round(goal.action~="info" and self.db.profile.fontsize + (self.CurrentSkinStyle.StepFontSizeMod or 0) or self.db.profile.fontsecsize)) -- TODO skindata() friendly?
									frame.lines[line].label:SetText(indent..goaltxt..link)
									frame.lines[line].goal = goal
									frame.lines[line].briefhidden = briefhidden
									line=line+1
									--frame.lines[line].label:SetMultilineIndent(1)
								end

								if (goaltxt=="?" or self.db.profile.tooltipsbelow) and goal.tooltip then
									frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsecsize))
									frame.lines[line].label:SetText(indent.."|cffeeeecc".. goal.tooltip.."|r")
									--frame.lines[line].label:SetMultilineIndent(1)
									frame.lines[line].goal = nil
									frame.lines[line].briefhidden = true
									line=line+1
								end
							end
						end
					end

					-- ALL collapsed? come on...
					local allcoll=true
					for l=1,line-1 do
						if not frame.lines[l].briefhidden or not showbriefsteps then
							allcoll=false
							break
						end
					end
					if allcoll then
						frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsecsize))
						frame.lines[line].label:SetText("|cffaaaaaa"..L['stepcollapsed'].."|r")
						--frame.lines[line].label:SetMultilineIndent(1)
						frame.lines[line].goal = nil
						frame.lines[line].briefhidden = false
						line=line+1
					end

					local TMP_TRUNCATE = true
					local heightleft = 400
					if showallsteps and TMP_TRUNCATE then
						if stepbuttonnum>1 then
							local stepbottom = self.stepframes[stepbuttonnum-1]:GetBottom()
							local scrollbottom = Scroll:GetBottom()
							if stepbottom and scrollbottom then
								heightleft = stepbottom-scrollbottom - 2*SkinData("StepLineMarginY") - 5
							else
								heightleft = 0
								--self:Debug("Error in step height calculation! step "..stepbuttonnum.." stepbottom="..tostring(stepbottom).." scrollbottom="..tostring(scrollbottom)..", forcing update")
								self.frameNeedsUpdating=true
							end
						end

						if heightleft<self.MIN_STEP_HEIGHT then
							frame:Hide()
							nomoredisplayed=true
							break --continue
						end
					end

					local height=0
					--frame.goallines={}
					local textheight
					frame.truncated=nil
					local abort
					for l=1,LINES_PER_STEP do
						local lineframe = frame.lines[l]
						local text = lineframe.label
						local lineheight
						if l<line and not frame.truncated then
							text:SetWidth(frame:GetWidth()-icon_indent-2*SkinData("StepLineMarginX"))

							textheight = text:GetHeight()
							--if text:IsTruncated() then textheight=textheight+self.db.profile.fontsize+6 end
							lineheight = textheight + STEP_LINE_SPACING

							if lineframe.briefhidden and showbriefsteps --[[and stepdata==self.CurrentStep--]] then
								--lineheight = lineheight * self.briefstepexpansion
								lineheight = lineheight * (self.briefstepexpansionlines[stepbuttonnum] or 0)

								--lineframe:SetAlpha(self.briefstepexpansion)
								lineframe:SetAlpha(self.briefstepexpansionlines[stepbuttonnum] or 0)
								text:SetAlpha(self.briefstepexpansionlines[stepbuttonnum] or 0)
								lineframe.icon:SetAlpha(self.briefstepexpansionlines[stepbuttonnum] or 0)
							else
								lineframe:SetAlpha(1)
								text:SetAlpha(1)
								lineframe.icon:SetAlpha(1)
							end

							height = height + lineheight

							--text:SetWidth(ZygorGuidesViewerFrameScroll:GetWidth()-30)

							if TMP_TRUNCATE and showallsteps and height>heightleft then
								lineframe.goal=nil
								if l<=2 then
									abort=true
									break
								else
									frame.truncated=true
									frame.lines[l-1].label:SetText("   . . .")
									frame.lines[l-1].goal=nil
									lineframe:Hide()
									height=height-lineheight
								end
							else
								lineframe:Show()
								--if lineframe.goal then frame.goallines[lineframe.goal.num]=lineframe end
							end

							lineframe:SetHeight(lineheight)
						else
							lineframe:Hide()
							lineframe.goal = nil
						end
					end

					if abort then
						frame:Hide()
						nomoredisplayed=true
						break --continue
					end


					--#### display it all properly

					--[[
					if height<self.MIN_STEP_HEIGHT then
						frame.lines[1]:SetPoint("TOPLEFT",frame,ZGV.STEPMARGIN_X,-4)
						frame.lines[1]:SetPoint("TOPRIGHT",frame,-ZGV.STEPMARGIN_X,-4)
						height=self.MIN_STEP_HEIGHT
					else
						frame.lines[1]:SetPoint("TOPLEFT",frame,ZGV.STEPMARGIN_X,-4)
						frame.lines[1]:SetPoint("TOPRIGHT",frame,-ZGV.STEPMARGIN_X,-4)
					end
					-- how about NO special cases
					--]]

					frame.lines[1]:SetPoint("TOPLEFT",frame,SkinData("StepLineMarginX"),-SkinData("StepLineMarginY"))
					frame.lines[1]:SetPoint("TOPRIGHT",frame,-SkinData("StepLineMarginX"),-SkinData("StepLineMarginY"))

					--self:Debug("step "..stepbuttonnum.." height "..height)

					if not frame.truncated or not TMP_TRUNCATE then
						frame:SetHeight(height + 2*SkinData("StepLineMarginY"))
					else
						frame:SetHeight(heightleft + 2*SkinData("StepLineMarginY"))
					end

					--end

					if stepbuttonnum>1 then totalheight = totalheight + STEP_SPACING end
					totalheight = totalheight + frame:GetHeight()



					if stepdata==self.CurrentStep and showallsteps then
						frame.border:Show()
					else
						frame.border:Hide()
					end

					--[[
					if self.db.profile.showallsteps and totalheight>ZygorGuidesViewerFrameScroll:GetHeight() then
						nomoredisplayed=true
						frame:Hide()
						break --continue
					end
					--]]

					if showallsteps and frame.truncated then
						nomoredisplayed=true
					end


					--oookay, frame is visible, let's fill it for real
					frame:Show()

					-- ICONS and status for ALL steps. Why not.
					for l=1,LINES_PER_STEP do

						line = frame.lines[l]
						if not line then break end

						local icon = line.icon
						local back = line.back

						local goal = line.goal

						if goal then
							local label = line.label
							local clicker = line.clicker
							local anim_w2g = line.anim_w2g
							local anim_w2r = line.anim_w2r

							local status,progress = goal:GetStatus()
							progress = tonumber(progress) or 0

							-- prepare completion effects

							-- set justCompleted only once per completion
							local justCompleted = false
							if status=="complete" and not self.recentlyCompletedGoals[goal] then
								self.recentlyCompletedGoals[goal] = true
								justCompleted = true
							end

							-- ICONS

							if goal and self.db.profile.goalicons then
								label:SetPoint("TOPLEFT",line,"TOPLEFT",icon_indent+2,0)
								icon:SetPoint("CENTER",line,"TOPLEFT",self.db.profile.fontsize*0.5+1,-self.db.profile.fontsize*0.5-1)
								icon:SetSize(self.CurrentSkinStyle.StepLineIconSize * self.db.profile.fontsize,self.CurrentSkinStyle.StepLineIconSize * self.db.profile.fontsize) -- TODO SkinData friendly?
								icon:Show()

								if goal.next then
									icon:SetIcon(actionicon.next)
								elseif status=="passive" then

									if goal.action=="talk" or goal.action=="from" or goal.action=="goto" then
										icon:SetIcon(actionicon[goal.action])
									else
										icon:SetIcon(1)
									end
									icon:SetDesaturated(false)

								elseif status=="incomplete" then

									icon:SetIcon(actionicon[goal.action])
									icon:SetDesaturated(false)

								elseif status=="complete" then

									icon:SetIcon(3)
									icon:SetDesaturated(false)

								elseif status=="impossible" then

									icon:SetIcon(actionicon[goal.action])
									icon:SetDesaturated(true)

								elseif status=="obsolete" then

									--icon:SetIcon(actionicon[goal.action])
									--icon:SetDesaturated(false)
									icon:SetIcon(actionicon[goal.action])
									icon:SetDesaturated(true)

								else	-- maybe hidden, maybe WTF
									icon:SetIcon(1)
								end
							else
								label:SetPoint("TOPLEFT",line,"TOPLEFT",0,0)
								icon:Hide()
							end


							-- BACKGROUNDS

							if self.db.profile.goalbackgrounds then

								back:Show()

								-- COLORS

								local r,g,b,a=0,0,0,0

								if status=="passive" then

									r,g,b,a = 0,0,0,0

								elseif status=="incomplete" then

									local inc=self.db.profile.goalbackincomplete
									local pro=self.db.profile.goalbackprogressing
									local com=self.db.profile.goalbackcomplete
									r,g,b = self.gradient3(self.db.profile.goalbackprogress and progress*0.7 or 0,  inc.r,inc.g,inc.b, pro.r,pro.g,pro.b, com.r,com.g,com.b, 0.5)
									a = self.db.profile.goalbackincomplete.a

									--local r,g,b,a = gradientRGBA(self.db.profile.goalbackincomplete,self.db.profile.goalbackcomplete,self.db.profile.goalbackprogress and progress*0.7 or 0)

								elseif status=="complete" then

									r,g,b,a = fromRGBA(self.db.profile.goalbackcomplete)

								elseif status=="impossible" then

									r,g,b,a = fromRGBA(self.db.profile.goalbackimpossible)

								elseif status=="obsolete" then  -- TODO: remove?

									r,g,b,a = fromRGBA(self.db.profile.goalbackobsolete)

								elseif status=="warning" then

									r,g,b,a = fromRGBA(self.db.profile.goalbackwarning)
								end

								-- FLASHES

								if status=="incomplete" and (goal.action~="goto" and goal.action~="fly") and self.db.profile.goalupdateflash and progress>(self.recentGoalProgress[goal] or 1) and self.frameNeedsResizing==0 and stepdata==self.CurrentStep then

									anim_w2r.r,anim_w2r.g,anim_w2r.b,anim_w2r.a = r,g,b,a
									anim_w2r:Play()
									self:Debug("Animating progress: "..goal:GetText())

									-- self.completionelapsed = 0  -- experimental delay

								elseif status=="complete" and justCompleted and self.db.profile.goalcompletionflash and self.frameNeedsResizing==0 then

									anim_w2g:Play()
									self:Debug("Animating completion.")

									-- self.completionelapsed = 0  -- experimental delay

								end

								if (anim_w2r:IsDone() or not anim_w2r:IsPlaying()) and (anim_w2g:IsDone() or not anim_w2g:IsPlaying()) then
									back:SetBackdropColor(r,g,b,a)
									back:SetBackdropBorderColor(r,g,b,a)
								end

							else
								back:Hide()
							end

							if goal then
								if self.recentGoalProgress[goal]~=progress then
									goal.dirtytext=true
								end

								self.recentGoalProgress[goal] = progress

								-- unpause when completing a goal

								if justCompleted and stepdata==self.CurrentStep then
									self.pause=nil
								end
							end

							if status=="impossible" then
								line:SetAlpha(0.4)
							else
								line:SetAlpha(1.0)
							end

						elseif line.isheader then
							-- leave it! it should handle its own back.
						else
							-- no goal? ah, subtitle.
							icon:Hide()
							back:Hide()
						end
					end

					if showallsteps then
						frame:SetAlpha((stepnum<self.CurrentStepNum and 0.3) or (stepnum==self.CurrentStepNum and 1.0) or 0.8)
					else
						if stepbuttonnum==1 then
							frame:SetAlpha(1.0)
						else
							frame:SetAlpha(0.8-0.4*((stepbuttonnum-1)/(self.db.profile.showcountsteps-1)))
						end
					end

					--[[
					if stepnum==self.CurrentStepNum then
						--frame:EnableMouse(0)
						--frame:SetScript("OnClick",nil)
					else
						--frame:EnableMouse(1)
					end
					--]]

					--[[
					if stepnum==self.CurrentStepNum then
						frame:SetBackdrop({ edgeFile = ZGV.DIR.."\\Skins\\default\\midnight\\border", edgeSize = 12,  })
					else
						frame:SetBackdrop({ edgeFile = ZGV.DIR.."\\Skins\\default\\midnight\\border", edgeSize = 12 })
					end
					--]]

					-- TODO this is really dirty
					self.db.profile.stepbackalpha=1.0
					if stepdata:AreRequirementsMet() then
						if stepdata:IsComplete() then
							frame:SetBackdropColor(fromRGBmul_a(self.db.profile.goalbackcomplete,0.5,self.db.profile.stepbackalpha))
							if (not SkinData("StepBackdropPersistentBorder")) then
								frame:SetBackdropBorderColor(fromRGBmul_a(self.db.profile.goalbackcomplete,0.5,self.db.profile.stepbackalpha))
							end
							--frame:SetBackdropColor(0,0.7,0,0.5)
						elseif (self.db.profile.skipobsolete and stepdata:IsObsolete()) then
							frame:SetBackdropColor(fromRGBmul_a(self.db.profile.goalbackobsolete,0.5,self.db.profile.stepbackalpha))
							if (not SkinData("StepBackdropPersistentBorder")) then
								frame:SetBackdropBorderColor(fromRGBmul_a(self.db.profile.goalbackobsolete,0.5,self.db.profile.stepbackalpha))
							end
						elseif (self.db.profile.skipauxsteps and stepdata:IsAuxiliarySkippable()) then
							frame:SetBackdropColor(fromRGBmul_a(self.db.profile.goalbackaux,0.5,self.db.profile.stepbackalpha))
							if (not SkinData("StepBackdropPersistentBorder")) then
								frame:SetBackdropBorderColor(fromRGBmul_a(self.db.profile.goalbackaux,0.5,self.db.profile.stepbackalpha))
							end
						else
							local r,g,b = unpack(self.CurrentSkinStyle.StepBackdropColor or {0,0,0})
							frame:SetBackdropColor(r,g,b,self.db.profile.stepbackalpha)
							if (not SkinData("StepBackdropPersistentBorder")) then
								frame:SetBackdropBorderColor(0,0,0,self.db.profile.stepbackalpha)
							end
						end
					else
						frame:SetBackdropColor(0.5,0.0,0.0,self.db.profile.stepbackalpha)
						if (not SkinData("StepBackdropPersistentBorder")) then
							frame:SetBackdropBorderColor(0.5,0.0,0.0,self.db.profile.stepbackalpha)
						end
					end

					if not self.db.profile.showstepborders then
						frame:SetBackdropColor(0,0,0,0)
						frame:SetBackdropBorderColor(0,0,0,0)
					end

					--text:Show()

				else	-- not showing this one

					if frame then
						frame:Hide()
					end
				end
			until true end

			self.stepchanged=false

			-- set minimum frame size to one step
			minh = self.stepframes[1]:GetHeight() + TOP_HEIGHT + ZygorGuidesViewerFrame_Border_TabBack:GetHeight()

			self:UpdateFrameCurrent()

			--self:HighlightCurrentStep()

			-- steps displayed, clear the remaining slots

		else -- no current guide?

			local guides = self:GetGuides()
			if #guides>0 then
				ZygorGuidesViewerFrame_Border_Guides_GuideBack_SectionTitle:SetText(L["guide_notselected"])
			else
				ZygorGuidesViewerFrame_Border_Guides_GuideBack_SectionTitle:SetText(L["guide_notloaded"])
			end
		end

	elseif self.db.profile.displaymode=="gold" then

		self.Frame.Border.Gold:Show()
		self.Frame.Border.Guides:Hide()

		local x,y = GetPlayerMapPosition("player")
		local d = GetPlayerFacing()
		if x==gold_ox and y==gold_oy and d==gold_od and not full then return end
		gold_ox,gold_oy,gold_od = x,y,d

		-- get rid of tooltips, before they get messed up
		if ZGV.hasTooltipOverSpotLink then GameTooltip:Hide() ZGV.hasTooltipOverSpotLink=nil end

		-- hide step frames, if visible
		if self.stepframes[1]:IsVisible() then for i,stepframe in ipairs(self.stepframes) do stepframe:Hide() end end

		local spots
		if self.db.profile.golddistmode==1 then spots = ZGV:GetMapSpotsInRange()
		elseif self.db.profile.golddistmode==2 then spots = ZGV:GetMapSpotsInZone()
		else spots = ZGV:GetAllMapSpots()
		end

		if #spots>0 then
			if full then
				Scroll:Show()
				--ZygorGuidesViewerFrame_MissingText:Hide()
				if ZygorGuidesViewerFrame_ScrollScrollBar:GetValue()<1 then ZygorGuidesViewerFrame_ScrollScrollBar:SetValue(1) end
				ZygorGuidesViewerFrame_ScrollScrollBar:Show()
				ZygorGuidesViewerFrame_ScrollScrollBar:SetMinMaxValues(1,#spots)
				ZygorGuidesViewerFrame_Border_Guides_GuideBack_SectionTitle:SetText("Gold Spots")
			end

		else -- no gold guides or no spots in range
			Scroll:Hide()
			--ZygorGuidesViewerFrame_MissingText:Show()

			if #self.registeredmapspotsets>0 then
				ZygorGuidesViewerFrame_MissingText:SetText(L['gold_missing_nospotsinrange'])
			else
				ZygorGuidesViewerFrame_MissingText:SetText(L['gold_missing_noguidesloaded'])
			end
		end

		local totalheight = 0

		local frame
		local spotnum

		local firstspot = math.floor(ZygorGuidesViewerFrame_ScrollScrollBar:GetValue())
		if firstspot<1 then firstspot=1 end
		local lastspot = #spots

		--self:Debug("first step "..firststep..", last step "..laststep)
		-- run through buttons and assign steps for them

		local nomoredisplayed=false

		for spotbuttonnum = 1,self.StepLimit do repeat
			--frame = _G['ZygorGuidesViewerFrame_Step'..stepbuttonnum]
			frame = self.spotframes[spotbuttonnum]
			assert(frame,"Out of spot frames at "..spotbuttonnum)

			spotnum = firstspot + spotbuttonnum - 1

			-- show this button at all?
			if spotnum>=firstspot and spotnum<=lastspot and spotnum<=#spots then
				local spotdata = spots[spotnum]
				assert(spotdata,"UpdateFrame: No data for spot "..spotnum)

				if nomoredisplayed then
					frame:Hide()
					break --continue
				end

				frame.spotnum = spotnum
				frame.spot = spotdata

				--#### position step frame

				frame:SetWidth(ZygorGuidesViewerFrame_ScrollChild:GetWidth()) -- this is needed so the text lines below can access proper widths

				-- out of screen space? bail.
				-- but only in all steps mode!
				local top=frame:GetTop()
				local bottom=ZygorGuidesViewerFrame_Scroll:GetBottom()
				if top and bottom and top<bottom then
					frame:Hide()
					nomoredisplayed=true
					break --continue!
				end

				--#### fill it with text

				-- no translation here
				--[[
				local changed,dirty = stepdata:Translate()
				if dirty then self.frameNeedsUpdating=true end
				--]]

				local line=1

				assert(frame.lines[line],"Out of lines ("..line..") in spot frame "..spotbuttonnum)

				frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize))

				-- cardinal names
				--frame.lines[line].label:SetText(("|cffffbb00%s|r (%s %s)"):format(spotdata.title or "?",ZGV.FormatDistance(spotdata.waypoint.minimapFrame.dist),GetCardinalDirName(Astrolabe:GetDirectionToIcon(spotdata.waypoint.minimapFrame))))

				-- icons

				--local dirnum=GetCardinalDirNum(-Astrolabe:GetDirectionToIcon(spotdata.waypoint.minimapFrame) + GetPlayerFacing())-1 --:30:30:0:0:32:32:0:0:0:0
				--local dirnum2=dirnum>8 and 16-dirnum or dirnum
				--local arrow = ("|Tinterface\\addons\\ZygorGuidesViewer____\\skins\\arrow-mini-multi:20:20:0:0:32:512:%d:%d:%d:%d|t"):format(dirnum>8 and 32 or 0,dirnum>8 and 0 or 32,dirnum2*32,(dirnum2+1)*32)
				--frame.lines[line].label:SetText(("%s |cffffbb00%s|r (%s)"):format(arrow, spotdata.title or "?",ZGV.FormatDistance(spotdata.waypoint.minimapFrame.dist)))

				local angle = Astrolabe:GetDirectionToIcon(spotdata.waypoint.minimapFrame) + GetPlayerFacing()
				local sin,cos = math.sin(angle)*0.71, math.cos(angle) * 0.71
				frame.arrow:SetTexCoord(0.5-sin, 0.5+cos, 0.5+cos, 0.5+sin, 0.5-cos, 0.5-sin, 0.5+sin, 0.5-cos)
				frame.arrowdist:SetText(ZGV.FormatDistance(spotdata.waypoint.minimapFrame.dist):gsub(" ","|n"))

				frame.lines[line].label:SetText(("|cffffbb00%s|r"):format(spotdata.title or "?"))
				line=line+1

				--[[
				frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize))
				frame.lines[line].label:SetText(("|cffffff00%s %s,%s|r"):format(spotdata.map,spotdata.x,spotdata.y))
				line=line+1
				--]]

				if (spotdata.desc) then
					frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize))
					frame.lines[line].label:SetText(("%s"):format(spotdata.desc))
					line=line+1
				end


				if spotdata.objects then
					for s,source in ipairs(itemsources) do
						local objs = spotdata:GetObjectsOfType(source,true)
						if objs then
							local mobs = source=="drop" and spotdata.mobs
							local mobtext
							if mobs then
								mobtext = ""
								for i,mob in ipairs(spotdata.mobs) do
									if #mobtext>0 then mobtext = mobtext .. ", " end
									mobtext = mobtext .. mob.name
								end
							elseif spotdata.vendorid then
								mobtext = spotdata.vendor
							end

							--[[
							-- all in one line; tidy but impractical
							local header = L['gold_header_'..source]:format(mobtext or "mob")
							local str=""
							for o,obj in ipairs(objs) do
								if not obj.hidden then
									if obj.item.id then
										str = str .. "|Hitem:"..obj.item.id.."|h"..(obj.icon or "item").."|h "
									else
										str = str .. " ["..obj.item.name.."]"
									end
								end
							end

							if #str>0 then
								frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize))
								--frame.lines[line].label:SetText("<html><body><p>"..("|cffdddd66%s |r%s"):format(header,str).."</p></body></html>")
								frame.lines[line].label:SetText(("|cffdddd66%s |r%s"):format(header,str))
								line=line+1
							end
							--]]

							local goodobjs = {}
							for o,obj in ipairs(objs) do
								if not obj.hidden then
									tinsert(goodobjs,obj)
								end
							end

							if #goodobjs then
								frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize))
								--frame.lines[line].label:SetText("<html><body><p>"..("|cffdddd66%s |r%s"):format(header,str).."</p></body></html>")
								frame.lines[line].label:SetText(("|cffdddd66%s|r"):format(L['gold_header_'..source]:format(mobtext or "mob")))
								line=line+1

								for o,obj in ipairs(goodobjs) do
									local str
									if obj.item.id then
										str = "|Hitem:"..obj.item.id.."|h"..(obj.icon or "item").." "..(obj.string or "?").."|h "
									else
										str = obj.item.name
									end

									if obj.toohard then str = "|cffff0000"..str.."|r" end

									frame.lines[line].label:SetFont(FONT,round(self.db.profile.fontsize*1.0))
									frame.lines[line].label:SetText(str)
									frame.lines[line].label:SetHyperlinksEnabled(false)
									frame.lines[line].label.reenableHyperlinks=true
									line=line+1

								end
							end
						end
					end
				end

				local TMP_TRUNCATE = true
				local heightleft = 400
				if TMP_TRUNCATE then
					if spotbuttonnum>1 then
						local spotbottom = self.spotframes[spotbuttonnum-1]:GetBottom()
						local scrollbottom = ZygorGuidesViewerFrame_Scroll:GetBottom()
						if spotbottom and scrollbottom then
							heightleft = spotbottom-scrollbottom - 2*SkinData("StepLineMarginY") - 5
						else
							heightleft = 0
							self:Debug("Error in spot height calculation! spot "..spotbuttonnum.." spotbottom="..tostring(spotbottom).." scrollbottom="..tostring(scrollbottom)..", forcing update")
							--self.frameNeedsUpdating=true
						end
					end

					if heightleft<self.MIN_STEP_HEIGHT then
						frame:Hide()
						nomoredisplayed=true
						break --continue
					end
				end

				local height=0
				--frame.goallines={}
				local textheight
				frame.truncated=nil
				local abort

				for l=1,LINES_PER_STEP do
					local lineframe = frame.lines[l]
					local text = lineframe.label
					if l<line and not frame.truncated then
						text:SetWidth(frame:GetWidth()-icon_indent-2*SkinData("StepLineMarginX"))

						-- old non-HTML stuff
						if text:GetRegions() then
							textheight = text:GetRegions():GetHeight()
						else
							textheight = text:GetHeight()
						end
						text:SetHeight(textheight)

						height = height + (height>0 and STEP_LINE_SPACING or 0) + textheight
						--text:SetWidth(ZygorGuidesViewerFrameScroll:GetWidth()-30)

						if TMP_TRUNCATE and height>heightleft then
							if l<=2 then
								abort=true
								break
							else
								frame.truncated=true
								frame.lines[l-1].label:SetText("   . . .")
								lineframe:Hide()
								height=height-textheight-STEP_LINE_SPACING
							end
						else
							lineframe:Show()
							--if lineframe.goal then frame.goallines[lineframe.goal.num]=lineframe end
							lineframe:SetHeight(textheight+STEP_LINE_SPACING)
						end

					else
						lineframe:Hide()
					end
				end

				if abort then
					frame:Hide()
					nomoredisplayed=true
					break --continue
				end

				--self:Print(("spot %d, height %s"):format(spotbuttonnum,height))

				--#### display it properly

				if height<self.MIN_SPOT_HEIGHT then
					frame.lines[1]:SetPoint("TOPLEFT",frame.arrow:GetWidth()+SkinData("StepLineMarginX"),-(self.MIN_SPOT_HEIGHT-height)/2-0.6)
					frame.lines[1]:SetPoint("TOPRIGHT",-SkinData("StepLineMarginX"),-(self.MIN_SPOT_HEIGHT-height)/2-0.6)
					height=self.MIN_SPOT_HEIGHT
				else
					frame.lines[1]:SetPoint("TOPLEFT",frame,frame.arrow:GetWidth()+SkinData("StepLineMarginX"),-SkinData("StepLineMarginY"))
					frame.lines[1]:SetPoint("TOPRIGHT",frame,-SkinData("StepLineMarginX"),-SkinData("StepLineMarginY"))
				end

				if not frame.truncated or not TMP_TRUNCATE then
					frame:SetHeight(height + 3*SkinData("StepLineMarginY"))
				else
					frame:SetHeight(heightleft + 2*SkinData("StepLineMarginY"))
				end

				--end

				if spotbuttonnum>1 then totalheight = totalheight + STEP_SPACING end
				totalheight = totalheight + frame:GetHeight()


				if frame.truncated then
					nomoredisplayed=true
				end

				--oookay, frame is visible, let's fill it for real
				frame:Show()

				if self.db.profile.showstepborders then
					frame:SetBackdropColor(0,0,0,self.db.profile.stepbackalpha)
					frame:SetBackdropBorderColor(0,0,0,self.db.profile.stepbackalpha)
				else
					frame:SetBackdropColor(0,0,0,0)
					frame:SetBackdropBorderColor(0,0,0,0)
				end

				--text:Show()

			else	-- not showing this one

				if frame then
					frame:Hide()
					--[[
					local prename = "ZygorGuidesViewerFrame_Step"..stepnum.."_Text"
					for line=1,10 do
						local text = _G[prename..line]
						text:SetHeight(0.1)
					end
					--]]
					--[[
					frame:SetHeight(0)
					frame:ClearAllPoints()
					frame:SetPoint("TOPLEFT")
					--]]
				end
			end
		until true end

		self.stepchanged=false

		-- set minimum frame size to one step
		minh = self.spotframes[1]:GetHeight() + TOP_HEIGHT + tabh

		--self:HighlightCurrentStep()

		-- steps displayed, clear the remaining slots


		--ZygorGuidesViewerFrame_Border_TitleBar_PrevButton:Show()
		--ZygorGuidesViewerFrame_Border_TitleBar_NextButton:Show()
		--ZygorGuidesViewerFrame_Border_TitleBar_Step:Show()
		--ZygorGuidesViewerFrame_Border_TitleBar_StepText:SetText(self.CurrentStepNum)
		--ZygorGuidesViewerFrame_Border_TitleBar_StepText:Show()

	end

	if minh<MIN_HEIGHT+tabh then minh=MIN_HEIGHT+tabh end
	self.Frame:SetMinResize(MIN_WIDTH,minh)
	if self.Frame:GetHeight()<minh-0.01 then self.Frame:SetHeight(minh) end

	self:ResizeFrame()

	if self.delayFlash and self.delayFlash>0 then
		self.delayFlash=2 --ready to flash!
		--ZygorGuidesViewerFrame_bdflash:StartRGB(1,1,1,1,0,1,0,1)
	end
end


function ZGV:ClearFrameCurrent()
	if InCombatLockdown() then return end
	for i=1,LINES_PER_STEP do
		self.actionbuttons[i]:Hide()
		self.petactionbuttons[i]:Hide()
		self.actionbuttons[i].cd:Hide()
	end
end


function ZGV:UpdateFrameCurrent()
	-- current step!

	if self.CurrentStep then	-- hey, it may be missing, if the whole guide is for another class

		--local mapped = self.CurrentStep.x

		--[[
		ZygorGuidesViewerFrame_ActiveStep:SetBackdrop({bgFile = "Interface/Tooltips/UI-Tooltip-Background",
							    edgeFile = "Interface/Tooltips/UI-Tooltip-Border",
							    tile = true, tileSize = 16, edgeSize = 16,
							    insets = { left = 4, right = 4, top = 4, bottom = 4 }})
		--]]

		--[[
		if self.CurrentStep.requirement then
			ZygorGuidesViewerFrame_ActiveStep_Line0:SetText((self.CurrentStep:AreRequirementsMet() and "|cff88cc88" or "|cffbb0000") .. "(" .. table.concat(self.CurrentStep.requirement,L["stepreqor"]) .. ")")
			height = height + ZygorGuidesViewerFrame_ActiveStep_Line0:GetHeight()+STEP_LINE_SPACING
			ZygorGuidesViewerFrame_ActiveStep_Line1:ClearAllPoints()
			ZygorGuidesViewerFrame_ActiveStep_Line1:SetPoint("TOPLEFT",ZygorGuidesViewerFrame_ActiveStep_Line0,"BOTTOMLEFT",-ICON_INDENT,-STEP_LINE_SPACING)
			ZygorGuidesViewerFrame_ActiveStep_Line1:SetPoint("TOPRIGHT",ZygorGuidesViewerFrame_ActiveStep_Line0,"BOTTOMRIGHT",0,-STEP_LINE_SPACING)
			ZygorGuidesViewerFrame_ActiveStep_Line0:Show()
		else
			ZygorGuidesViewerFrame_ActiveStep_Line1:ClearAllPoints()
			ZygorGuidesViewerFrame_ActiveStep_Line1:SetPoint("TOPLEFT",ZygorGuidesViewerFrame_ActiveStep)
			ZygorGuidesViewerFrame_ActiveStep_Line1:SetPoint("TOPRIGHT",ZygorGuidesViewerFrame_ActiveStep)
			ZygorGuidesViewerFrame_ActiveStep_Line0:Hide()
		end
		--]]

		local name, line,label,icon,back,clicker,anim_w2g,anim_w2r,action,petaction,cooldown, lastlabel
		local height = 0

		if not self.stepframes[1].stepnum then return end

		local framenum = (self.CurrentStepNum - self.stepframes[1].stepnum + 1)
		if framenum<1 or framenum>self.StepLimit then
			self.CurrentStepframeNum = nil
			return self:ClearFrameCurrent()
		else
			self.CurrentStepframeNum = framenum
		end

		local stepframe = self.stepframes[framenum]
		if not stepframe.lines[1].icon then
			ZygorGuidesViewerFrame_Step_Setup(framenum)
		end

		if not stepframe:IsVisible() then
			return self:ClearFrameCurrent()
		end

		--textline(1):ClearAllPoints()
		--textline(1):SetPoint("TOPLEFT",stepframe,"TOPLEFT",0,self.CurrentStep.requirement and -textline(1):GetHeight()-STEP_LINE_SPACING or 0)
		--textline(1):SetPoint("TOPRIGHT",stepframe,"TOPRIGHT",0,self.CurrentStep.requirement and -textline(1):GetHeight()-STEP_LINE_SPACING or 0)

		if self.BUTTONS_INLINE then
			if not InCombatLockdown() then self.actionsvisible = false end
		end

		for i=1,LINES_PER_STEP do  -- update all lines
			--local linenum = (self.CurrentStep.requirement and i+1 or i)

			line = stepframe.lines[i]
			if not line then break end
			label = line.label
			icon = line.icon
			back = line.back
			clicker = line.clicker
			anim_w2g = line.anim_w2g
			anim_w2r = line.anim_w2r

			-- don't even touch this in combat.
			action = self.actionbuttons[i]
			petaction = self.petactionbuttons[i]

			if not InCombatLockdown() then
				action:SetHitRectInsets(0,-clicker:GetWidth()+15,0,0)
				action:SetHighlightTexture(nil,nil,nil,nil)--remove highlighting for the buttons
			end

			local goal = line.goal

			if goal then
				lastlabel = label

				--steptext = ("  "):rep(goal.indent or 0)
				--if i==1 then steptext = steptext .. self.CurrentStepNum .. ". " end
				--steptext = steptext .. goal:GetText(true)

				--steptext = string.gsub(steptext,"\t([a-z]+\. )","\t|cffffff88%1|r")
				--steptext = string.gsub(steptext,"\t",">")

				-- ACTION BUTTONS
				-- all non-action code: get the hell out.

				if not InCombatLockdown() and not stepframe.slideup:IsPlaying() then
					if goal:IsActionable() then
						--	cooldown:Show()
						--self:Debug("showing cooldown "..i)
						local vis

						if not self.BUTTONS_INLINE then
							action:SetFrameLevel(ZygorGuidesViewerFrame:GetFrameLevel()+5)
							petaction:SetFrameLevel(ZygorGuidesViewerFrame:GetFrameLevel()+5)
						end

						if goal.castspell and goal.castspellid then
							action:SetAttribute("type","spell")
							action:SetAttribute("spell",goal.castspell)
							action.spellid = goal.castspellid
							action.tex:SetTexture(select(3, GetSpellInfo(goal.castspellid or goal.castspell)) or "Interface\\Icons\\Spell_Nature_FaerieFire")
							action.goal = goal
							--action:SetScript("OnClick",function(self) PetActionButton_OnClick(self,"LeftButton") end)
							vis=true
							--	local start,dur,en = GetSpellCooldown(goal.castspellid or goal.castspell)
							--DoCooldown(cooldown,start,dur,en)

						elseif goal.item or goal.itemid then
							action:SetAttribute("type","item")
							if goal.itemuse then
								action:SetAttribute("item",goal.itemid and "item:"..goal.itemid  or  goal.item)
							end
							--ZGV:Debug("&display Goal %d shows item %d",goal.num,goal.itemid)
							action.tex:SetTexture(select(10, GetItemInfo(goal.itemid or goal.item)) or "Interface\\Icons\\INV_Misc_Bag_08")
							action.goal = goal
							vis=true
							--local start,dur,en = GetItemCooldown(goal.itemid or goal.item)
							--DoCooldown(cooldown,start,dur,en)

						elseif goal.macro then
							action:SetAttribute("type","macro")
							--action:SetAttribute("macro","ZygorGuidesMacro"..goal.num)
							action:SetAttribute("macro",goal.macro)
							ZGV:Debug("&display Goal %d shows macro %d",goal.num,goal.macro)
							local tex = select(2,GetMacroInfo(goal.macro))
							if tex and tex:find("\\0") then tex=nil end
							action.tex:SetTexture(tex)
							action.goal = goal
							vis=true

						elseif goal.petaction then
							local num,name,subtext,tex = FindPetActionInfo(goal.petaction)
							if num then
								petaction:SetID(num)
								petaction.tooltipName=name
								petaction.tooltipSubtext=subtext
								--action:SetScript("OnClick",function(self) PetActionButton_OnClick(self,"LeftButton") end)
								petaction.icon:SetTexture(tex)
								petaction:Show()
								petaction:ClearAllPoints()
								petaction:SetPoint("CENTER",UIParent,"BOTTOMLEFT",(icon:GetLeft() or 0)+8 or 0,(icon:GetBottom() or 0)+8)
								petaction:SetScale(self.db.profile.framescale)
							else
								petaction:Hide()
							end
							petaction.goal = goal
						else
							-- error("IsActionable but no item/spell!")
							-- might happen when leaving combat. Just ignore silently.

							--[[
							if not InCombatLockdown() then
								action:Hide()
							end
							cooldown:Hide()
							--]]
							action.goal = nil
							petaction.goal = nil
						end

						if vis then
							action:Show()
							action:SetScale(self.db.profile.framescale)
							if self.BUTTONS_INLINE then
								action:SetParent(clicker)
								action:ClearAllPoints()
								action:SetPoint("CENTER",clicker,"LEFT",8,0)
								self.actionsvisible = true
							else
								action:ClearAllPoints()
								action:SetPoint("CENTER",UIParent,"BOTTOMLEFT",(icon:GetLeft() or 0) +8,(icon:GetBottom() or 0)+8)
							end
						end

						--cooldown:Show()
					else
						action:Hide()
						petaction:Hide()
					end

					-- cooldown flasher
					--[[
					local DoCooldown = function (cooldown,start,dur,en)
						CooldownFrame_SetTimer(cooldown, start, dur, en)

						-- is this useless or what
						if not InCombatLockdown() then
							if dur>0 then
								--cooldown:Show()
								--self.recentCooldownsPulsing[goal] = nil
								--self.recentCooldownsStarted[goal] = 1
								--self:Debug("pulse: showing")
							else
								--[ [
								if not self.recentCooldownsPulsed[goal] and not self.recentCooldownsPulsed[goal] then
									self.recentCooldownPulses[goal] = self:ScheduleTimer("HideCooldown",1.0,{goal=goal,cooldown=cooldown})
									self:Debug("pulse: not pulsed, pulsing now and delaying")
								end

								if self.recentCooldownsStarted[goal] and self.recentCooldownsPulsing[goal] and self.recentCooldownsPulsing[goal]==1 then
									cooldown:Show()
									self:Debug("pulse: showing, awaiting delayed hiding")
								else
									cooldown:Hide()
								end
								--] ]
							end
						else
							--cooldown:Show()
						end
					end
					--]]
				end

				-- ICON stuff moved to all steps handler

				if self.BUTTONS_INLINE then
					if action:IsShown() then icon:Hide() end
				end

				--clicker:Show()

				--height = height + line:GetHeight()
			else
				if not InCombatLockdown() then action:Hide() petaction:Hide() end
				action.cd:Hide()
				--label:SetText("")
				--label:SetHeight(0)

				--line:SetHeight(0)  -- NO. This breaks stuff.
				-- but... it's necessary..!

				--line:SetHeight(0)
				--cooldown:Hide()
			end
		end

		if lastlabel then
			--ZygorGuidesViewerFrame_Divider2:SetPoint("TOPLEFT",lastlabel,"BOTTOMLEFT",-15,-4)
		end

		--ZygorGuidesViewerFrame_TextTitle:SetText(self.CurrentStep.title or "")
		--if ZygorGuidesViewerFrame_TextTitle:GetRight() then ZygorGuidesViewerFrame_TextTitle:SetWidth(ZygorGuidesViewerFrame_TextTitle:GetRight()-ZygorGuidesViewerFrame_TextTitle:GetLeft()) end

		--[[
		ZygorGuidesViewerFrame_TextInfo:SetText(self.CurrentStep.info or "")
		if ZygorGuidesViewerFrame_TextInfo:GetRight() then ZygorGuidesViewerFrame_TextInfo:SetWidth(ZygorGuidesViewerFrame_TextInfo:GetRight()-ZygorGuidesViewerFrame_TextInfo:GetLeft()) end
		--ZygorGuidesViewerFrame_TextInfo:SetPoint("TOPLEFT",self.CurrentStep.title and ZygorGuidesViewerFrame_TextTitle or ZygorGuidesViewerFrame_Divider2,"BOTTOMLEFT",0,-2)
		ZygorGuidesViewerFrame_TextInfo:SetPoint("TOPLEFT",ZygorGuidesViewerFrame_Divider2,"BOTTOMLEFT",0,-2)

		ZygorGuidesViewerFrame_TextInfo2:SetText(self.CurrentStep.info2 or "")
		if ZygorGuidesViewerFrame_TextInfo2:GetRight() then ZygorGuidesViewerFrame_TextInfo2:SetWidth(ZygorGuidesViewerFrame_TextInfo2:GetRight()-ZygorGuidesViewerFrame_TextInfo2:GetLeft()) end
		--ZygorGuidesViewerFrame_TextInfo2:SetPoint("TOPLEFT",self.CurrentStep.info and ZygorGuidesViewerFrame_TextInfo or (self.CurrentStep.title and ZygorGuidesViewerFrame_TextTitle or ZygorGuidesViewerFrame_Divider2),"BOTTOMLEFT",0,-2)
		ZygorGuidesViewerFrame_TextInfo2:SetPoint("TOPLEFT",self.CurrentStep.info and ZygorGuidesViewerFrame_TextInfo or ZygorGuidesViewerFrame_Divider2,"BOTTOMLEFT",0,-2)

		height = height + ZygorGuidesViewerFrame_TextInfo:GetHeight() + ZygorGuidesViewerFrame_TextInfo2:GetHeight()
		--]]

		-- aaand anchor it.


		--ZygorGuidesViewerFrame_ActiveStep:SetHeight(height)

		--ZygorGuidesViewerFrame_ActiveStep:ClearAllPoints()

		--local t = getglobal("ZygorGuidesViewerFrame_Step"..(self.CurrentStepNum))
		--ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPLEFT",t,"TOPLEFT")
		--ZygorGuidesViewerFrame_ActiveStep:SetPoint("BOTTOMRIGHT",t,"BOTTOMRIGHT")

		--[[
		if self.db.profile.showallsteps then
			if self.CurrentStepNum==1 then
				ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPLEFT",ZygorGuidesViewerFrameScrollChild,"TOPLEFT",0,-STEP_SPACING)
				ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPRIGHT",ZygorGuidesViewerFrameScrollChild,"TOPRIGHT",0,-STEP_SPACING)
			else
				local t = getglobal("ZygorGuidesViewerFrame_Step"..(self.CurrentStepNum-1))
				ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPLEFT",t,"BOTTOMLEFT",0,-STEP_SPACING)
				ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPRIGHT",t,"BOTTOMRIGHT",0,-STEP_SPACING)
			end
		else
			-- it's all alone
			ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPLEFT",ZygorGuidesViewerFrameScrollChild,"TOPLEFT",0,-STEP_SPACING)
			ZygorGuidesViewerFrame_ActiveStep:SetPoint("TOPRIGHT",ZygorGuidesViewerFrameScrollChild,"TOPRIGHT",0,-STEP_SPACING)
		end
		--]]
	end
end

function ZGV:SetFrameScale(scale)
	scale = self.db.profile.framescale
	frame:SetScale(scale)
end

function ZGV:ReanchorFrame()
	local frame = self.Frame
	local framemaster = frame:GetParent()
	local upsideup = not self.db.profile.resizeup


	local tabbar_height = 2
	if #ZGV.registeredmapspotsets>0 then
		tabbar_height = 20
	end


	frame:ClearAllPoints()
	local tabh = ZygorGuidesViewerFrame_Border_TabBack:GetHeight()

	if frame.sizedleft then
		local q,w,e,x,y = framemaster:GetPoint()
		framemaster:ClearAllPoints()
		framemaster:SetPoint(q,w,e,x+frame:GetLeft()-frame.sizedleft,y)
		frame.sizedleft=nil
	end

	if upsideup then
		--frame:SetPoint("TOP",nil,"TOP",(left+right)/2-(uiwidth/2/scale),top-uiheight/scale)
		--frame:SetPoint("TOP",frame:GetParent(),"BOTTOMLEFT",left+width/2,top)
		frame:SetPoint("TOPLEFT",framemaster,"TOPLEFT",0,0)
		frame:SetClampRectInsets(0,0,-48-tabh,0)
	else
		--frame:SetPoint("BOTTOM",nil,"BOTTOM",(left+right)/2-(uiwidth/2/scale),bottom)
		--frame:SetPoint("BOTTOM",frame:GetParent(),"BOTTOMLEFT",left+width/2,bottom)
		frame:SetPoint("BOTTOMLEFT",framemaster,"BOTTOMLEFT",0,0)
		frame:SetClampRectInsets(0,0,0,48+tabh)
	end

	--frame:UpdateMiniMode()
end

function ZGV:AlignFrame()
	self.Frame:AlignFrame()
	self.db.profile.frame_anchor = {self.Frame:GetParent():GetPoint()}
end

function ZGV:UpdateSkin()
	self.Frame:UpdateSkin()
end

function ZGV:ResizeFrame()
	--autosize
	--if (self.db.profile.autosize) then
	--print("resize")
	if self.frameNeedsResizing then
		if self.frameNeedsResizing>0 then self.frameNeedsResizing = self.frameNeedsResizing - 1 end
		if self.frameNeedsResizing>0 then return nil end
	end
	if not self.db then return end

	--[[
	if ZygorGuidesViewerFrame_Border_Bottom:GetRect() then
		local xsize = select(3,ZygorGuidesViewerFrame_Border_Bottom:GetRect())/200
		local ysize = select(4,ZygorGuidesViewerFrame_Border_Left:GetRect())/100
		local ysize2 = select(4,ZygorGuidesViewerFrame_Border_Right:GetRect())/100
		ZygorGuidesViewerFrame_Border_Left:SetTexCoord(0.2,0.8,0,1*ysize)
		ZygorGuidesViewerFrame_Border_Right:SetTexCoord(0.2,0.8,0,1*ysize2)
		ZygorGuidesViewerFrame_Border_Bottom:SetTexCoord(0,-xsize,1,-xsize,0,xsize,1,xsize)
	end
	--]]

	--ZygorGuidesViewerFrame_Border:SetBackdropColor(self.db.profile.skincolors.back[1],self.db.profile.skincolors.back[2],self.db.profile.skincolors.back[3],self.db.profile.backopacity)


	--self:Debug("resizing from "..tostring(ZygorGuidesViewerFrame:GetHeight()))

	if self.db.profile.showcountsteps==0 or self.db.profile.displaymode=="gold" then
		ZygorGuidesViewerFrame_ScrollScrollBar:Show()
	else
		-- only autoresize when showing ONE step. If we have many steps, the user handles resizing.
		ZygorGuidesViewerFrame_ScrollScrollBar:Hide()
		--if not self.CurrentStepNum or not _G['ZygorGuidesViewerFrame_Step'..self.CurrentStepNum] then return end
		local height = 0
		for i=1,self.db.profile.showcountsteps do
			if i>1 then height = height + STEP_SPACING end
			height = height + self.stepframes[i]:GetHeight()
		end

		local tabh = ZygorGuidesViewerFrame_Border_TabBack:GetHeight()
		local ProgressBarHeight=7
		if ZGV.db.profile.progress then	ProgressBarHeight=7
		else ProgressBarHeight=0 end

		height = height + TOP_HEIGHT + tabh
		--self:Debug("Height "..height.."  min "..MIN_HEIGHT)
		if height < MIN_HEIGHT + tabh then height=MIN_HEIGHT + tabh end
		self.Frame:SetHeight(height+ProgressBarHeight)
	end

	ZygorGuidesViewer_ProgressBar_Refresh()


	--self:Debug(("%d %d"):format(left,bottom))
--		ZygorGuidesViewerFrame:SetHeight(ZygorGuidesViewerFrame_Text:GetHeight()+35)


--	if ZygorGuidesViewerFrame_ActiveStep_Line1:GetTop() then
		--ZygorGuidesViewerFrame_Resize.max = ZygorGuidesViewerFrame_Line1:GetTop()-ZygorGuidesViewerFrame_TextInfo2:GetBottom()+35
		--ZygorGuidesViewerFrame_Resize:Stop()
		--ZygorGuidesViewerFrame_Resize:Play()

--		ZygorGuidesViewerFrame:SetHeight(ZygorGuidesViewerFrame_ActiveStep_Line1:GetTop()-ZygorGuidesViewerFrame_TextInfo2:GetBottom()+35)
--	end

--	end
end

function ZGV:GoalProgress(goal)
	return "epic fail"
end

function ZGV:ScrollToCurrentStep()
--	if self.ForceScrollToCurrentStep and self.CurrentStep then
--		self.ForceScrollToCurrentStep = false
		if self.CurrentStep and self.db.profile.displaymode=="guide" then

			local height=0
			local step
			if self.db.profile.showcountsteps==0 then
				local topstep = self.stepframes[1].stepnum
				if not topstep then return end
				if self.stepframes[1].stepnum>self.CurrentStepNum --above
				or (topstep+self.StepLimit-1<self.CurrentStepNum) --way below
--				or (ZygorGuidesViewerFrame_Step1:GetTop()-_G['ZygorGuidesViewerFrame_Step'..(self.CurrentStepNum-topstep+1)]:GetBottom()+STEP_SPACING>ZygorGuidesViewerFrameScroll:GetHeight()) --barely offscreen
				or not self.stepframes[self.CurrentStepNum-topstep+1]:IsShown()
				or self.stepframes[self.CurrentStepNum-topstep+1].truncated
				then
					ZygorGuidesViewerFrame_ScrollScrollBar:SetValue(self.CurrentStepNum)
					ZygorGuidesViewerFrame_ScrollScrollBar:Show()
				end
			else
				ZygorGuidesViewerFrame_ScrollScrollBar:Hide()
			end
		end
--	else
--		self.ForceScrollToCurrentStep = true
--	end
end

function ZGV:IsVisible()
	return self.Frame:IsVisible()
end

function ZGV:SetVisible(info,onoff)
	if onoff then self.Frame:Show() else self.Frame:Hide() end
end

function ZGV:ToggleFrame()
	if self.Frame:IsShown() then self.Frame:Hide() else self.Frame:Show() end
end

function ZGV:IsDefaultFitting(default)
	-- deprecated?
	local _,race = UnitRace("player")
	local _,class = UnitClass("player")
	if (class=="DEATHKNIGHT") then race=class end
	default=default:upper()
	race=race:upper()
	class=class:upper()
	return race==default or class==default or race.." "..class==default
end

--- Checks if the player's race/class matches the requirements.
-- @param requirement May be a string or a table of strings (which are then ORed).
-- @return true if matching, false if not.
function ZGV:RaceClassMatch(fit)
	if type(fit)=="table" then
		for i,v in ipairs(fit) do if self:RaceClassMatch(v) then return true end end
		return false --otherwise
	end

	local _,race = UnitRace("player")
	local _,class = UnitClass("player")
	local faction = UnitFactionGroup("player")
	race=race:upper()
	class=class:upper()
	faction=faction:upper()
	fit=fit:upper()
	local neg=false
	if fit:sub(1,1)=="!" then
		neg=true
		fit=fit:sub(2)
	end
	local ret = (race==fit or class==fit or faction==fit or race.." "..class==fit)
	if neg then return not ret else return ret end
end

function ZGV:RaceClassMatchList(list)
	list=list..","
	local st,en=1
	for fit in list:gmatch("(.-),") do
		if self:RaceClassMatch(fit) then return true end
	end
end

function ZGV:Print(s,ifdebug)
	if not self.db.profile.silentmode then
		ChatFrame1:AddMessage(L['name']..": "..tostring(s))
		if ifdebug then self:Debug(s) end
	end
end

local thunder_stack
function ZGV:ThunderStageForceUpdate()
	if thunder_stack then return end
	if WorldMapFrame:IsShown() and GetCurrentMapAreaID()~=928 then --Wait for them to close the map then update
		ZGV.WaitingOnThunderStage = true
		return
	end
	thunder_stack=1

	ZGV.WaitingOnThunderStage = nil

	local lastmap,lastfloor

	if GetCurrentMapAreaID()~=928 then
		lastmap,lastfloor = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
		SetMapByID(928) --Thunder Isle
	end

	if C_MapBar.GetTag()=="THUNDER_ISLE" then
		ZGV.db.char.thunderstage = (C_MapBar.GetPhaseIndex() + 1) or 1
		ZGV.db.char.thunderprogress = (C_MapBar.GetCurrentValue()/C_MapBar.GetMaxValue()) or 0

		if ZGV.DEV then
			-- TEMPORARY TIMING OF THUNDER ISLE
			if not ZGV.db.global.thundertimes then ZGV.db.global.thundertimes={} end
			local val = C_MapBar.GetCurrentValue()
			if val>0 and ZGV.db.global.thunderprogress_last~=val then
				ZGV.db.global.thundertimes[time()] = val
				ZGV.db.global.thunderprogress_last=val
			end
		end
	end

	if lastmap then SetMapByID(lastmap) SetDungeonMapLevel(lastfloor) end

	thunder_stack=nil
end

function ZGV:GetThunderStage()
	if not ZGV.db.char.thunderstage or ZGV.WaitingOnThunderStage then
		ZGV:ThunderStageForceUpdate()
	end

	return (ZGV.db.char.thunderstage or 1),(ZGV.db.char.thunderprogress or 0)
end




	local function avgdev(data,basetime,progtime)
		local cnt,totaldev=0,0
		for time,prog in pairs(data) do
			cnt=cnt+1
			local dev = (time-basetime) * progtime  -  prog
			totaldev=totaldev+dev
		end
		return totaldev/cnt
	end

	function ZGV:AnalyzeThunderData(reset)
		local mintime,maxtime=9999999999,0
		local progs = ZGV.db.global.thundertimes
		if reset then wipe(progs) end
		for time,data in pairs(progs) do
			if time<mintime then mintime=time end
			if time>maxtime then maxtime=time end
		end

		local progtime=(progs[maxtime]-progs[mintime])/(maxtime-mintime)


		-- at this point,   progress = (time()-zerotime) * progtime

		local minprog,maxprog=0.01,5
		local mindev,maxdev=99999999,0

		for i=1,20 do
			progtime=(minprog+maxprog)/2
			local zerotime = mintime - progs[mintime]/progtime
			print ("progtime",progtime,"dev",avgdev(progs,zerotime,progtime))
			if avgdev(progs,zerotime,progtime)<0 then
				minprog=progtime
			else maxprog=progtime
			end
		end

		local zerotime =  mintime - progs[mintime]/progtime

		print("progress/time = ",progtime)

		local s = ""
		for t=mintime,maxtime do
			if progs[t] then
				local expected = (t-mintime) * progtime + progs[mintime]
				print("time",t,"progress",progs[t],"deviation",progs[t]-expected)
				s = s .. t .. "\t" .. progs[t] .. "\n"
			end
		end
		ZGV:ShowDump(s)

		local fulldate = zerotime + 1000000/progtime

		print("|cffffbb001000000|cff88ff00 estimated in ",fulldate," = |cffffbb00",date("%Y-%m-%d %H:%M:%S",fulldate))
	end

	function ZGV:GetThunderStageQQ()
		if WorldMapFrame:IsShown() and GetCurrentMapAreaID()~=928 then  -- wrong map shown! should we force it or not..?
			if ZGV.db.char.thunderstage then
				-- cached? return that
				return ZGV.db.char.thunderstage
			else
				-- FORCE. EVIL.
				SetMapByID(928) --Thunder Isle
			end
		end

		if GetCurrentMapAreaID()~=928 then return ZGV.db.char.thunderstage end  -- the best we can do, if we weren't forcing it

		local stage = (C_MapBar and C_MapBar.BarIsShown() and C_MapBar.GetPhaseIndex() + 1)  or  1

		ZGV.db.char.thunderstage = stage  -- save

		last_thunder_check = GetTime()

		return ZGV.db.char.thunderstage
	end

function ZGV:MatchProfs(fitprof,levelmin,levelmax)
	local data = self:GetSkill(fitprof)

	if not data.active then
		return false --We don't have this profession so forget it.
	elseif data.level > 0 then --sanity check.
		return data.level>=levelmin and data.level<levelmax
	end
end

function ZGV:FindEvent(eventName)
	eventName=eventName:upper()
	local _,month,day,year=CalendarGetDate()
	local calmonth=CalendarGetMonth()

	local eventtex = {
		CALENDAR_DARKMOONFAIRETEROKKA = "DARKMOON FAIRE",
		CALENDAR_DARKMOONFAIREELWYNN = "DARKMOON FAIRE",
		CALENDAR_DARKMOONFAIREMULGORE = "DARKMOON FAIRE",
		CALENDAR_NOBLEGARDEN = "NOBLEGARDEN",
		CALENDAR_BREWFEST = "BREWFEST",
		CALENDAR_HARVESTFESTIVAL = "HARVEST FESTIVAL",
		CALENDAR_HARVESTFESTIVAL_PILGRIM = "PILGRIM'S BOUNTY",
		CALENDAR_WINTERVEIL = "FEAST OF WINTER VEIL",
		CALENDAR_LUNARFESTIVAL = "LUNAR FESTIVAL",
		CALENDAR_LOVEINTHEAIR = "LOVE IS IN THE AIR",
		CALENDAR_CHILDRENSWEEK = "CHILDREN'S WEEK",
		CALENDAR_MIDSUMMER = "MIDSUMMER FIRE FESTIVAL",
		CALENDAR_HALLOWSEND = "HALLOW'S END",
	}

	local cal=CalendarFrame
	if not cal then Calendar_LoadUI() OpenCalendar() cal=CalendarFrame end --Makes the UI available, OpenCalendar() does not really open it.
	--OpenCalendar() does not even play any sound that I noticed, but it does make so the even information is available, this is the important stuff.

	if cal:IsVisible() then --They are looking at the calendar so don't change the month on them
		return false -- Bad Suggestion. If they look at suggested with calendar open shame on them.
	elseif month~=calmonth then --Not visible so change away!
		while month~=calmonth do --Make sure it is really he correct month
			CalendarSetMonth(1) --Move to next month.
			calmonth=CalendarGetMonth()
		end
	end

	for event=1, CalendarGetNumDayEvents(0,day) do --0 current month, 1 next month, -1 last month... Always want the current day.
		local name,_,_,eventType,eventStatus,_,texture = CalendarGetDayEvent(0,day,event)
		name=name:upper() texture=texture:upper()

		if (texture=="CALENDAR_HARVESTFESTIVAL" and month>10) then --Stay the same
			texture="CALENDAR_HARVESTFESTIVAL_PILGRIM" --Lovely work around for Harvest Festival and Pilgrim's Bounty having the same texture
		end

		if eventType=="HOLIDAY" --We don't care about any other events
		and eventStatus~="END"  --If it is the last day of a holiday it ended at 2am so ignore it.
		and (eventName==name or eventtex[texture]==eventName) then --Does the ring fit?
			return true
		end
	end
	return false -- Nothing else was returned so it is all wrong.
end

function ZGV:UNIT_INVENTORY_CHANGED(event,unit)
	self:UpdateFrame(true)
	if unit=="player" then
		self:TryToCompleteStep(true)
	end
end

--local MapBarLastupdate=0
function ZGV:MAP_BAR_UPDATE(event)
	--local time = GetTime();
	--if MapBarLastupdate~= time then
	--	MapBarLastupdate = time
		ZGV:ThunderStageForceUpdate()
	--end
end

function ZGV:QUEST_LOG_UPDATE(event,unit)
	self:UpdateFrame(true)
	if unit=="player" then
		self:TryToCompleteStep(true)
	end
end

local blobstate=nil
function ZGV:PLAYER_REGEN_DISABLED()
	--ZygorGuidesViewerFrame_Cover:Show()
	--ZygorGuidesViewerFrame_Cover:EnableMouse(true)
	self:UpdateCooldowns()
	if self.db.profile.hideincombat then
		if self.Frame:IsVisible() then
			UIFrameFadeOut(self.Frame,0.5,ZGV.db.profile.opacitymain,0.0)
			self.hiddenincombat = true
		end

		if self.CV.Frame:IsVisible() then
			UIFrameFadeOut(self.CV.Frame,0.5,ZGV.db.profile.opacitymain,0.0)
			self.cvhiddenincombat = true
		end
	end

	blobstate = WorldMapBlobFrame:IsShown()
	WorldMapBlobFrame:SetParent(nil)
	--WorldMapBlobFrame:ClearAllPoints()
	WorldMapBlobFrame:Hide()
	WorldMapBlobFrame.Hide = function() blobstate=nil end
	WorldMapBlobFrame.Show = function() blobstate=true end
end

function ZGV:PLAYER_REGEN_ENABLED()
	--ZygorGuidesViewerFrame_Cover:Hide()
	--ZygorGuidesViewerFrame_Cover:EnableMouse(false)
	if self.CurrentStep then self.CurrentStep:PrepareCompletion(true) end
	self:UpdateFrameCurrent()
	self:UpdateCooldowns()

	if self.hiddenincombat then
		UIFrameFadeIn(self.Frame,0.5,0.0,ZGV.db.profile.opacitymain)
		self.hiddenincombat = nil
	end

	if self.cvhiddenincombat then
		UIFrameFadeIn(self.CV.Frame,0.5,0.0,ZGV.db.profile.opacitymain) --This will fade the creature viewer to the same level as the window. Not a bad thing imo
		self.cvhiddenincombat = nil
	end

	self:UpdateLocking()

	WorldMapBlobFrame:SetParent(WorldMapFrame)
	--WorldMapBlobFrame:SetAllPoints(WorldMapDetailFrame)
	WorldMapBlobFrame.Hide = nil
	WorldMapBlobFrame.Show = nil
	if blobstate then WorldMapBlobFrame:Show() end

	if self.call_after_combat then self.call_after_combat() self.call_after_combat=nil end
end

function ZGV:SPELL_UPDATE_COOLDOWN()
	--self:Debug("Updating cooldowns")
	self:UpdateFrameCurrent()
	self:UpdateCooldowns()
end

function ZGV:PLAYER_CONTROL_GAINED()
	GetRealZoneText()
	self:TryToCompleteStep(true)
	self:CacheCurrentMapID()
end

function ZGV:WORLD_MAP_UPDATE()
	self:CacheCurrentMapID()
end

function ZGV:PLAYER_ENTERING_WORLD()
	self:CacheCurrentMapID()
end

function ZGV:ZONE_CHANGED_INDOORS()
	self:CacheCurrentMapID()
end

function ZGV:ZONE_CHANGED()
	self:CacheCurrentMapID()
end

function ZGV:ZONE_CHANGED_NEW_AREA()
	self:CacheCurrentMapID()
end

function ZGV:TAXIMAP_OPENED()
	if self.db.profile.autotaxi and self.CurrentStep then
		local destination
		for gi,goal in ipairs(self.CurrentStep.goals) do
			--if goal.action=="fly" and goal.map==ZGV.CurrentMapID then

			if goal.action=="fly" then
				destination = goal.landing
				break
			end
		end
		if destination then
			self:Debug("Autotaxi destination: "..destination)
			local dest_i
			for i=1,NumTaxiNodes() do
				if TaxiNodeName(i):find(destination)==1 then
					dest_i=i
					destination = TaxiNodeName(i)
					break
				end
			end
			if not dest_i then
				self:Debug("Cannot fly to "..destination..": destination not found on map.")
			elseif TaxiNodeGetType(dest_i)~="REACHABLE" then
				self:Print("Cannot fly to "..destination..": destination unreachable.")
			elseif TaxiNodeCost(dest_i)>GetMoney() then
				self:Print("Cannot fly to "..destination..": not enough money.")
			else
				-- finally!
				self:Print("Taking taxi to ".. TaxiNodeName(dest_i)..".")
				TakeTaxiNode(dest_i)
			end
		end
	end
end

ZGV.CurrentMapID,ZGV.CurrentMapFloor = 0,0

function ZGV:CacheCurrentMapID()
	--if Astrolabe.LastPlayerPosition then  -- try to avoid too much calling of SetMapToCurrentZone
		local m,f = Astrolabe:GetCurrentPlayerPosition()
		if m and m~=0 and m~=13 and m~=14 and m~=-1 and m~=485 and m~=466 and m~=613   -- multi-zone, whole-continent maps
		--and (GetRealZoneText()==GetMapNameByID(m) or not WorldMapFrame:IsShown()) -- WHOA! Browsed away, maybe?
		-- not needed anymore! GetCurrentPlayerPosition is now playing nice.
		then
			ZGV.CurrentMapID,ZGV.CurrentMapFloor = m,f
			--ZGV:Debug(("Cached map/floor: %s %d/%d"):format(GetMapNameByID(self.CurrentMapID),self.CurrentMapID,self.CurrentMapFloor))
		end
		--return
	--end
	-- NOOOooo... Astrolabe.LastPlayerPosition is STUPID. It switches to continent map coord when browsing the map :/



	-- Okay, this might be the most naive thing ever. If the map is not open, can we assume GetCurrent* calls do return player's own map..?

	--if WorldMapFrame:IsShown() then return end  -- do not bother when maps are being browsed.
	--[[
	local m,f = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	self.CurrentMapID,self.CurrentMapFloor = m,f
	--]]



	--SetMapToCurrentZone()
	--self.CurrentMapID,self.CurrentMapFloor = GetCurrentMapAreaID(),GetCurrentMapDungeonLevel()
	--if old_id~=self.CurrentMapID or old_floor~=self.CurrentMapFloor then SetMapByID(old_id)  SetDungeonMapLevel(old_floor) end
end

function ZGV:FindData(array,what,data)
	if not (type(array)=="table") then return nil end
	local i,d
	for i,d in pairs(array) do if d[what]==data then return d end end
end

function ZGV:Frame_OnShow()
	PlaySound("igQuestLogOpen")
	self:Debug("ZGV:Frame_OnShow")
	--ZygorGuidesViewerFrame_Filter()
	--[[
	if UnitFactionGroup("player")=="Horde" then
		ZygorGuidesViewerFrameTitleAlliance:Hide()
	else
		ZygorGuidesViewerFrameTitleHorde:Hide()
	end
	--]]

	if self.CurrentStep then
		self.db.profile.visible = true
		self:UpdateFrame(true)
		self:AlignFrame()

		if self.db.profile.hidearrowwithguide then
			self:SetWaypoint()
		end

		-- Trying to show the modelviewer frame
		-- During startup operation we may be shown numerous times,
		-- so let's make sure we're trying to do a nice thing
		if self.db.profile.mv_enabled and self.CV then
			self:TryToDisplayCreature(true)
		end
	end
end

function ZGV:Frame_OnHide()
	PlaySound("igQuestLogClose")
	self.db.profile.visible = false
	if not InCombatLockdown() then
		for i=1,LINES_PER_STEP do
			ZGV.actionbuttons[i]:Hide()
			ZGV.petactionbuttons[i]:Hide()
		end
	end

	--[[

	-- this is a HELL ugly hack.
	-- "Do not hide when it's the World Map that hid us".
	if not WorldMapFrame.blockWorldMapUpdate -- this would mean we're enlarging the small map
	and not debugstack():find("TOGGLEWORLDMAP") -- UGLY hack
	then
		if self.db.profile.hidearrowwithguide then
			self:Debug("Hiding arrow with guide")
			self:SetWaypoint(false)
		end

		-- if modelviewer is on, also hide it as well
		if self.CV.Frame:IsShown() then
			self:Debug("Hiding modelviewer with guide.")
			self.CV:Hide()
		end
	end
	--]]

	if self.CV then self.CV:Hide() end
end


function ZGV:GoalOnClick(goalframe,button)
	--timer is a hack to make sure multiple consecutive goals arent clicked
	--at the same time. 0.2 seconds between clicks distinguishes between humans
	local curTime = GetTime()
	if curTime - self.ClickedTime < 0.2 then
		return
	end
	self.ClickedTime = curTime
	goalframe = goalframe:GetParent()
	local stepframe = goalframe:GetParent()
	local goal = goalframe.goal

	if not goal then return end
	if self.db.profile.showcountsteps>0 and stepframe.step~=self.CurrentStep then return end -- no clicking on non-current steps in compact mode
	--if stepframe:GetScript("OnClick") then stepframe:GetScript("OnClick")(stepframe,button) end

	--local num=goalframe.goalnum
	self:Debug("goal clicked "..tostring(goal.num))
	--local goal = self.CurrentStep.goals[num]
	if button=="LeftButton" then
		if IsModifiedClick("DRESSUP") and goal.itemid and IsDressableItem("item:"..goal.itemid) then
			DressUpItemLink("item:"..goal.itemid)
			return
		end

		-- Clicked goal? Set waypoint, and set map if it's open.
		if goal.x and not goal.force_noway then
			self:SetWaypoint(goal.num)
			if WorldMapFrame:IsShown() then SetMapByID(goal.map) SetDungeonMapLevel(goal.floor or 1) end
		end
		if ZGV.GOALTYPES[goal.action].click_to_complete then
			if goal.clickfun then
				goal:clickfun()
			else
				--self:SkipStep(true)
				self:FakeCompleteGoal(goal,true)
			end
		end
		if goal.next then
			local step,guide = goal.parentStep:GetJumpDestination(goal.next)
			if guide then
				ZGV:Debug("WHOA. GUIDE JUMP! |cffffdd00%s|r in |cffff00dd%s|r",step,guide)
				self:SetGuide(guide,step)
			else
				ZGV:FocusStep(step)
			end
		end

		if goal.script then
			if not goal.scriptfun then
				ZGV:Debug("goal ".. goal.num.." loading script: ".. goal.script)

				local fun,err = loadstring(goal.script)
				if fun then
					setfenv(fun,ZGV.Parser.ConditionEnv)
					goal.scriptfun = fun
				else
					self:Print("Error in |script ".. goal.script.." : ".. tostring(err))
				end
			end
			if goal.scriptfun then  goal:scriptfun()  end
		end

		if goal.questid then
			self:TrackQuest(goal.questid)
			self:PointToQuest(goal.questid)
		end

		if goal.achieveid then
			-- copied from WatchFrame.lua
			if ( not AchievementFrame ) then
				AchievementFrame_LoadUI();
			end
			if ( not AchievementFrame:IsShown() ) then
				AchievementFrame_ToggleAchievementFrame()
				AchievementFrame_SelectAchievement(goal.achieveid);
			else
				if ( AchievementFrameAchievements.selection ~= goal.achieveid ) then
					AchievementFrame_SelectAchievement(goal.achieveid);
				else
					AchievementFrame_ToggleAchievementFrame();
				end
			end
		end

		--[[
		-- NOT allowed :/
		if goal.itemid and goal.itemuse and self.actionbuttons[goal.num] then
			self.actionbuttons[goal.num]:Click()
		end
		--]]
	else
		ZGV:OpenQuickStepMenu(stepframe,goalframe)
	end
end

function ZGV:GoalOnEnter(goalframe)
	local goal = goalframe:GetParent().goal
	if not goal then return end

	local wayline,infoline,image

	if goal.tooltip and not self.db.profile.tooltipsbelow then
		infoline = "|cff00ff00"..goal.tooltip.."|r"
	end
	if goal.x and goal.y and goal.map then
		-- if locked or force_noway, then no clicking, bare info.
		local tipformat = (self.db.profile.windowlocked or goal.force_noway) and 'tooltip_waypoint_coords' or 'tooltip_waypoint'
		local coords = math.round(goal.x*100)..";"..math.round(goal.y*100)
		local map = (GetMapNameByID(goal.map) or ('#'..goal.map)).." "
		wayline = L[tipformat]	:format(goal.waytitle and (goal.waytitle.." ("..map..coords..")") or map..coords)
	end

	if goal.image then
		image = DIR.."\\Images\\"..goal.image..".tga"
	end

	if infoline or wayline or image or goal.itemid then
		GameTooltip:SetOwner(self.Frame,"ANCHOR_TOP") --GameTooltip moved above the Viewer
		GameTooltip:ClearAllPoints()
		GameTooltip:SetPoint("BOTTOM",self.Frame,"TOP")
		--GameTooltip:SetOwner(goalframe,"ANCHOR_TOPLEFT")
		--GameTooltip:SetPoint("BOTTOM",goalframe,"TOP")

		if goal.itemid then
			GameTooltip:SetOwner(goalframe,"ANCHOR_LEFT") -- Items tooltip to the left of the line
			GameTooltip:ClearAllPoints()
			GameTooltip:SetPoint("BOTTOMRIGHT",goalframe,"TOPLEFT")

			GameTooltip:SetHyperlink("item:"..goal.itemid)
		--else
		--	GameTooltip:SetText(goal:GetText())
		end


		local lines=1
		if infoline then
			GameTooltip:AddLine(infoline,0,1,0)
			if _G['GameTooltipTextLeft'..lines]:GetWidth()>300 then _G['GameTooltipTextLeft'..lines]:SetWidth(300) end
			lines=lines+1
		end
		if wayline then
			GameTooltip:AddLine(wayline,0,1,0)
			if _G['GameTooltipTextLeft'..lines]:GetWidth()>300 then _G['GameTooltipTextLeft'..lines]:SetWidth(300) end
			lines=lines+1
		end
		GameTooltip:Show()
		if image then
			local img

			--[[
			local img = _G['GameTooltipZygorImage']
			if not img then
				img = GameTooltip:CreateTexture("GameTooltipZygorImage","ARTWORK")
			end
			--]]
			img = GameTooltipTexture1
			GameTooltip:AddLine(" ")
			GameTooltip:AddTexture(image)
			img:ClearAllPoints()
			img:SetPoint("TOPLEFT",_G['GameTooltipTextLeft'..lines],"BOTTOMLEFT")
			--img:SetTexture(image)
			img:SetWidth(128)
			img:SetHeight(128)
			img:Show()
			GameTooltip:Show()
			GameTooltip:SetHeight(150 + lines*20)
		end
	end

	if (IsModifiedClick("DRESSUP") and goal.itemid and IsDressableItem("item:"..goal.itemid)) then
		ShowInspectCursor()
	else
		ResetCursor()
	end
end

function ZGV:GoalOnLeave(goalframe,num)
	if goalframe and (GameTooltip:GetOwner()==goalframe or GameTooltip:GetOwner()==self.Frame) then GameTooltip:Hide() end
end


local function insert_guides(arr,guides)
	local data
	for i,guide in ipairs(guides) do
		data = ZGV:GetGuideByTitle(guide.full)
		local item = {
			text = guide.step and L['menu_last_entry']:format(guide.short or "?",guide.step) or (guide.short or "?"),
			checked = function() return ZGV.CurrentGuideName==guide.full end,
			func = function()  CloseDropDownMenus()  ZGV:SetGuide(guide.full,guide.step) end,
			tooltipTitle = data and data.description and guide.short,
			tooltipText = data and data.description,
			tooltipOnButton = true,
		}
		tinsert(arr,item)
	end
end

local function group_to_array(group)
	local arr = {}
	for i,group in ipairs(group.groups) do
		local item = {
			text = group.name,
			hasArrow = true,
			menuList = group_to_array(group),
			keepShownOnClick = true,
			func = function(self) _G[self:GetName().."Check"]:Hide() end,
			--notCheckable = true
		}
		--if #item.menuTable>0 then
			tinsert(arr,item)
		--end
	end
	insert_guides(arr,group.guides)
	return arr
end

local function BuildDropDown_GuideMenu(level,value)
	local self=ZGV
	--[[
	local menu = { }

	menu = group_to_array(self.registered_groups)
	EasyMenu(menu,ZGVFMenu,"ZygorGuidesViewerFrame_Border_TitleBar",30,10,"MENU",3)
	--]]
end

function ZGV:GetGuidesHistory(gtype)
	local unwrapped={}
	local data=self.db.char.guides_history[gtype]
	if data then
		local g = self:GetGuideByTitle(data[1])
		if g then
			g.CurrentStepNum = data[2]
			tinsert(unwrapped,g)
		end
		return unwrapped
	end
	return nil
end

function ZGV:OpenGuideMenu(path)
	if self.Menu then self.Menu:Show(path) return end
end

function ZGV:FakeCompleteGoal(goal,docomplete)
	if docomplete==nil then docomplete=not self.recentlyCompletedGoals[goal] end
	if docomplete then
		--self.recentlyCompletedGoals[goal]=true
		self.recentlyStickiedGoals[goal]=true
	else
		self.recentlyCompletedGoals[goal]=false
		self.recentlyStickiedGoals[goal]=false
		self.recentlyVisitedCoords[goal]=false
	end
	--goal.fake_complete = true
	self.pause=nil
	self.LastSkip=1
	--self.AutoskipTemp = true
	self:TryToCompleteStep(true)
end

function ZGV:FakeCompleteQuest(questid,docomplete,questtitle)
	self.completedQuests[questtitle]=docomplete
	if questid then self.completedQuests[questid]=docomplete end
	self:Print("Marking quest '"..questtitle.."'"..(questid and " (#"..questid..")" or "").." as "..(docomplete and "completed" or "incomplete"))
	self:TryToCompleteStep(true)
end

function ZGV:OpenMapToQuest(questid)
	if self.questsbyid[questid] and WorldMap_OpenToQuest then -- 3.3.0
		WorldMap_OpenToQuest(questid)
		local done,posX,posY,obj = QuestPOIGetIconInfo(questid)
		if posX or posY then
			local q = self.questsbyid[questid]
			local title
			if q then title=q.title end
			self:Debug("Setting waypoint to POI: "..posX.." "..posY)
			self:SetWaypoint(posX*100,posY*100,title)
		end
	end
end

local lastquestid,lastquesttitle
local showqiretries=0
--[[
function ZGV:ShowQuestInfo(questid,questtitle,indump)
	self:Debug("Showing chains for "..tostring(questid).." '"..tostring(questtitle).."'")
	if not questid then questid=lastquestid questtitle=lastquesttitle else showqiretries=0 end
	lastquestid=questid lastquesttitle=questtitle showqiretries=showqiretries+1

	--if InCombatLockdown() then return end
	local max = self.maxQuestLevels[questid] or -1
	local lev = self.mentionedQuests[questid] or -1
	local col = GetQuestDifficultyColor(lev)
	local s
	if (max>lev) then
		s = ("Quest |cff%02x%02x%02x[|Hquest:%d:%d|h%s|h]|r (#%d) [level %s + chains to %s]"):format(col.r*255,col.g*255,col.b*255,questid,UnitLevel("player"),questtitle,questid,lev>0 and lev or "?",max>0 and max or "?")
	else
		s = ("Quest |cff%02x%02x%02x[|Hquest:%d:%d|h%s|h]|r (#%d) [level %s]"):format(col.r*255,col.g*255,col.b*255,questid,UnitLevel("player"),questtitle,questid,lev>0 and lev or "?")
	end

	local pre = self.Chains[questid]
	if pre then
		if type(pre)=="number" then
			s = s .. "\nPrerequisite:"
			pre={"",pre}
		else
			s = s .. "\nPrerequisites ("..(pre[1]=="AND" and "ALL" or "ANY").."):"
		end
		for i=2,#pre do
			local id = tonumber(pre[i])
			local quest,inlog = self.Localizers:GetQuestData(id)
			lev = self.mentionedQuests[id] or -1
			local caching=nil
			if (quest and not caching) then
				col = GetQuestDifficultyColor(lev)
				s=s..("\n- |cff%02x%02x%02x[|Hquest:%d:%d|h%s|h]|r (#%d) [level %s]"):format(col.r*255,col.g*255,col.b*255,id,UnitLevel("player"),quest.title,id,lev>0 and lev or "?")
			else
				if showqiretries<5 then
					if showqiretries==1 then self:Print("Retrieving quest information, please wait...") end
					self:ScheduleTimer("ShowQuestInfo",1) return
				else
					s=s.."\n- #"..id.." (retrieving quest information, please try again)"
					caching=true
				end
			end
		end
	end
	local mentioned = self:GetMentionedFollowups(questid)
	local q
	if #mentioned>1 then
		q = "Follow-ups:"
		for i=2,#mentioned do
			local id = tonumber(mentioned[i][1])
			local quest = self.Localizers:GetQuestData(id)
			lev = mentioned[i][2]
			local caching=nil
			if (quest and not caching) then
				col = GetQuestDifficultyColor(lev)
				q=q..("\n- |cff%02x%02x%02x[|Hquest:%d:%d|h%s|h]|r (#%d) [level %s]"):format(col.r*255,col.g*255,col.b*255,id,UnitLevel("player"),quest.title,id,lev>0 and lev or "?")
			else
				if showqiretries<5 then
					if showqiretries==1 then self:Print("Retrieving quest information, please wait...") end
					self:ScheduleTimer("ShowQuestInfo",1) return
				else
					q=q.."\n- #"..id.." (retrieving quest information, please try again)"
					caching=true
				end
			end
		end
	else
		q = "No follow-ups."
	end

	local rem=""
	local remaining = self:GetQuestRemainingInChain(questid)
	if remaining then
		rem = "\n\n"..remaining.." quests remain till end of chain."
	end

	if indump then
		self:ShowDump(s.."\n"..q..rem,"Quest information",false)
	else
		self:Print("Quest information:")
		local ss = s.."\n"..q..rem
		local sslines = {strsplit("\n",ss)}
		for i,l in ipairs(sslines) do print(l) end
	end
end

function ZGV:ToggleWatchQuest(questid)
end
--]]

function ZGV:OpenQuickStepMenu(stepframe,goalframe)
	local ZGVFMenu = ZGV.CurrentSkin.name=="Default" and ZGVF_Default_Menu or ZGVF_Default_Menu
	ZGVFMenu.stepframe=stepframe
	ZGVFMenu.goalframe=goalframe

	local step = stepframe.step
	local goal = goalframe.goal

	local menu = {
		{
			text = L['qmenu_step']:format(step.num,step.level or "?"),
			isTitle = true,
			notCheckable=true,
		},
		{
			text = L['qmenu_step_skip'],
			tooltipTitle = L['qmenu_step_skip'],
			tooltipText = L["qmenu_step_skip_desc"],
			tooltipOnButton = true,
			func = function() self:SkipStep(true) end,
			--icon = ZGV.DIR .. "\\Skins\\minimaparrow-green-dot",
			keepShownOnClick = false,
		}
	}

	if goal:IsCompleteable() or (goal.map and goal.x) then
		tinsert(menu,{
			text = L['qmenu_goal']:format(goal:GetText():gsub("%|r",""):gsub("%|c........","")),
			isTitle = true,
			notCheckable=true,
		})
	end

	if goal.map and goal.x then
		local map = GetMapNameByID(goal.map) or ('#'..goal.map)
		tinsert(menu,{
			text = L['qmenu_goal_waypoint']:format(map,goal.x*100,goal.y*100),
			tooltipTitle = L['qmenu_goal_waypoint']:format(map,goal.x*100,goal.y*100),
			tooltipText = L['qmenu_goal_waypoint_desc'],
			tooltipOnButton = true,
			func = function()  self:SetWaypoint(goal.num)  end,
			isNotRadio=true,
		})
	end

	local id = goal.npcid or (goal.mobs and goal.mobs[1] and goal.mobs[1].id) or (goal.action=="kill" and goal.targetid)
	if id then
		local name = self.Localizers:GetTranslatedNPC(id) or "(creature)"
		tinsert(menu,{
			text = L['qmenu_goal_creature_data']:format(name),
			tooltipTitle = L['qmenu_goal_creature'],
			tooltipTitleText = L['qmenu_goal_creature_desc']:format(name),
			tooltipOnButton = true,
			func = function()
				self.db.profile.viewcreature=true
				self.CreatureViewer:ShowCreature(id,name)
				if self.CreatureViewer.failed then
					self:Print("Creature is not yet available - too far.")
				end
			end,
			--Try both and hopefully one works.
			isNotRadio=true,
		})
	end

	if goal:IsCompleteable() then
		tinsert(menu,{
			text = L['qmenu_goal_complete'],
			tooltipTitle = L['qmenu_goal_complete'],
			tooltipText = L['qmenu_goal_complete_desc'],
			tooltipOnButton = true,
			checked = function()  return goal:IsComplete()  end,
			func = function()  self:FakeCompleteGoal(goal,not self.recentlyCompletedGoals[goal])  end,
			isNotRadio=true,
		})
		if goal.quest then
			local quest,inlog = self.Localizers:GetQuestData(goal.quest.id)
			local title = quest and quest.title or "?"
			tinsert(menu,{
				text = L['qmenu_quest']:format(title),
				isTitle = true,
				notCheckable = true,
			})
			--[[
			-- no quest info anymore
			tinsert(menu,{
				text = L['qmenu_quest_info']:format(title),
				tooltipTitle = L['qmenu_quest_info']:format(title),
				tooltipText = L['qmenu_quest_info_desc'],
				tooltipOnButton = true,
				func = function() self:ShowQuestInfo(goal.quest.id,title,IsShiftKeyDown())  end,
			})
			--]]
			if inlog then
				local ind=goal.quest.index
				tinsert(menu,{
					text = L['qmenu_quest_openlog'],
					tooltipTitle = L['qmenu_quest_openlog'],
					tooltipText = L['qmenu_quest_openlog_desc'],
					tooltipOnButton = true,
					func = function()  ShowUIPanel(QuestLogFrame)  QuestLog_SetSelection(ind) QuestLog_Update() end,
				})
				tinsert(menu,{
					text = L['qmenu_quest_watched'],
					tooltipTitle = L['qmenu_quest_watched'],
					tooltipText = L['qmenu_quest_watched_desc'],
					tooltipOnButton = true,
					checked = function() return IsQuestWatched(ind) end,
					func = function()
						if IsQuestWatched(ind) then
							RemoveQuestWatch(ind)
							WatchFrame_Update()
						else
							if ( GetNumQuestWatches() < MAX_WATCHABLE_QUESTS ) then
								AddQuestWatch(ind)
								WatchFrame_Update()
							end
						end
					end,
					isNotRadio=true,
				})
				if GetQuestWorldMapAreaID(goal.quest.id)>0 then
					tinsert(menu,{
						text = L['qmenu_quest_openmap'],
						tooltipTitle = L['qmenu_quest_openmap'],
						tooltipText = L['qmenu_quest_openmap_desc'],
						tooltipOnButton = true,
						func = function()  WorldMap_OpenToQuest(goal.quest.id,nil)  ZGV:PointToQuest(goal.quest.id)  end,
					})
				end
			end

			-- woo, LightHeaded support!
			if LightHeaded then
				tinsert(menu,{
					text = L['qmenu_quest_lightheaded'],
					tooltipTitle = L['qmenu_quest_lightheaded'],
					tooltipText = L['qmenu_quest_lightheaded_desc'],
					tooltipOnButton = true,
					func = function()
						if IsShiftKeyDown() then
							local s = LightHeaded:GetPageText(goal.quest.id)
							self:ShowDump(s,"Quest info - courtesy of LightHeaded",false)
						else
							ShowUIPanel(QuestLogFrame)
							LightHeaded:UpdateFrame(goal.quest.id, LightHeaded.db.profile.singlepage and -1 or 1)
						end
					end,
				})
			end

			if self.db.profile.debug then
				tinsert(menu,{
					text = "(debug) force quest complete",
					tooltipTitle = self.completedQuests[goal.quest.id] and L['qmenu_quest_complete_tip'] or L['qmenu_quest_complete_in_tip'],
					tooltipText = self.completedQuests[goal.quest.id] and L['qmenu_quest_complete_desc'] or L['qmenu_quest_complete_in_desc'],
					checked = function()  return self.completedQuests[goal.quest.id]  end,
					func = function()  self:FakeCompleteQuest(goal.quest.id,not self.completedQuests[goal.quest.id],goal.quest.title)  end,
					isNotRadio=true,
				})
			end
			--[[
			tinsert(menu,{
				text = L['qmenu_quest_complete'],
				tooltipTitle = self.completedQuests[goal.quest.id] and L['qmenu_quest_complete_tip'] or L['qmenu_quest_complete_in_tip'],
				tooltipText = self.completedQuests[goal.quest.id] and L['qmenu_quest_complete_desc'] or L['qmenu_quest_complete_in_desc'],
				checked = function()  return self.completedQuests[goal.quest.id]  end,
				func = function()  self:FakeCompleteQuest(goal.quest.id,not self.completedQuests[goal.quest.id],goal.quest.title)  end,
				isNotRadio=true,
			})
			--]]
		end
	end
	if self.borderfadedout and not self.db.profile.delayshowborder then
		tinsert(menu,{
			text = L['opt_group_display'],
			isTitle = true,
			notCheckable=true,
		})
		tinsert(menu,{
			text = L['qmenu_border_restore'],
			tooltipTitle = L['qmenu_border_restore'],
			tooltipText = L['qmenu_border_restore_desc'],
			tooltipOnButton = true,
			--checked = function()  return self.db.profile.hideborder  end,
			func = function()  self.borderfadedout=nil  UIFrameFadeIn(self.Frame.Border,0.15,0.0,ZGV.db.profile.opacitymain)  self.Frame.mouseCount=999  self.Frame.leftCount=-3  end,
			isNotRadio=true,
		})
	end
	--[[
		{
			text = L['opt_miniresizeup'],
			tooltipTitle = L['opt_miniresizeup'],
			func = function() self:SetOption("Display","resizeup") end,
			checked = function() return self.db.profile.resizeup end,
			isNotRadio=true,
			keepShownOnClick = true,
		},
		{
			text = L['opt_hideincombat'],
			tooltipTitle = L['opt_hideincombat'],
			tooltipText = L['opt_hideincombat_desc'],
			checked = function()  return self.db.profile.hideincombat  end,
			func = function()  self:SetOption("Display","hideincombat")  end,
			isNotRadio=true,
			keepShownOnClick = true,
		},
		{
			text = L['opt_configuration'],
			tooltipTitle = L['opt_configuration'],
			tooltipText = L['opt_configuration_desc'],
			isNotRadio=true,
			notCheckable=true,
			func = function()  self:OpenOptions()  end,
		},
		{
			name = L['opt_group_step'],
			isTitle = true,
		},
		{
			text = L["opt_do_searchforgoal"],
			notCheckable = true,
			func = function() ZGV:SearchForCompleteableGoal() end
		}
	--]]
	EasyMenu(menu,ZGVFMenu,goalframe:GetName(),0,0,"MENU",3)
end

--[[
function ZGV:OpenQuickSteps()
	local menu = {
		{
			text=L["opt_showcountsteps"],
			isTitle = true,
			notCheckable = true,
		},
		{
			text=L["opt_showbriefsteps"],
			func=function() self:SetOption("Step","showbriefsteps") end,
			checked=function() return self.db.profile.showbriefsteps end,
			isNotRadio = true,
		},
		{
			text=L["opt_showcountsteps_all"],
			func=function() self:SetOption("Step","showcountsteps 0") end,
			checked=function() return self.db.profile.showallsteps end,
		},
		{
			text='1',
			func=function() self:SetOption("Step","showcountsteps 1") end,
			checked=function() return not self.db.profile.showallsteps and self.db.profile.showcountsteps==1 end,
		},
		{
			text='2',
			func=function() self:SetOption("Step","showcountsteps 2") end,
			checked=function() return not self.db.profile.showallsteps and self.db.profile.showcountsteps==2 end,
		},
		{
			text='3',
			func=function() self:SetOption("Step","showcountsteps 3") end,
			checked=function() return not self.db.profile.showallsteps and self.db.profile.showcountsteps==3 end,
		},
		{
			text='4',
			func=function() self:SetOption("Step","showcountsteps 4") end,
			checked=function() return not self.db.profile.showallsteps and self.db.profile.showcountsteps==4 end,
		},
		{
			text='5',
			func=function() self:SetOption("Step","showcountsteps 5") end,
			checked=function() return not self.db.profile.showallsteps and self.db.profile.showcountsteps==5 end,
		},
	}

	ZGVFMenu.onHide = function()
		ZGVFMenu.stepframe=nil
		ZGVFMenu.goalframe=nil
	end

	EasyMenu(menu,ZGVFMenu,"cursor",0,0,"MENU",3)
end
--]]

function ZGV:OpenMapToQuestGoal(questid,goalnum)
end

local function split(str,sep)
	local fields = {}
	str = str..sep
	str:gsub("(.-)"..sep, function(c) tinsert(fields, c) end)
	return fields
end

function ZGV:FindGroup(self,title)
	local path = split(title,"\\")

	-- create one
	local group=self
	for i=1,#path do
		local found = false
		for n,gr in ipairs(group.groups) do
			if gr.name==path[i] then
				found=true
				group=gr
			end
		end
		if not found then
			local gr = {name=path[i],groups={},guides={},ord=#group.groups+1}
			tinsert(group.groups,gr)
			group=gr
		end
	end
	return group
end

-- IN: "Zygor's Alliance Leveling Guides"
-- OUT: "Leveling"
function ZGV:SanitizeGuideTitle(title)
	title = title:gsub([[\\]],[[\]])
	title = title :gsub("^Zygor's ","")
	title = title :gsub("^Alliance ","") :gsub("^Horde ","")  -- code-side fix for "common" guides.
	--title = title :gsub(" Guide$","") :gsub(" Guides$","")

	-- fix old-style guide paths
	title = title
		:gsub("^Event.-\\","EVENTS\\")
		:gsub("^Dail.-\\","DAILIES\\")
		:gsub("^Leveling.-\\","LEVELING\\")
		:gsub("^Loremaster.-\\","LOREMASTER\\")
		:gsub("^Profession.-\\","PROFESSIONS\\")
		:gsub("^Achievement.-\\","ACHIEVEMENTS\\")
		:gsub("^Pet.-\\","PETSMOUNTS\\")
		:gsub("^Reputation.-\\","REPUTATIONS\\")
		:gsub("^Title.-\\","TITLES\\")
		:gsub("^Macro.-\\","MACROS\\")
		:gsub("^Dungeon.-\\","DUNGEONS\\")
		:gsub("^Gear.-\\","GEAR\\")

	return title
end

function ZGV:RegisterGuide(title,data,extra)
	title = self:SanitizeGuideTitle(title)

	local group,tit = title:match("^(.*)\\+(.-)$")
	local maingroup = group and group:match("^(.-)\\") or group or "GUIDES"

	if group then
		group = self:FindGroup(self.registered_groups,group)
	else
		group = self.registered_groups
	end

	local guide = ZGV.GuideProto:New(tit,title,data,extra)
	guide.ord=#group.guides+1
	guide.type = maingroup

	tinsert(group.guides,guide)
	tinsert(self.registeredguides,guide)
end

ZGV.registered_mapspotset_groups = { groups={},guides={}}

function ZGV:RegisterMapSpots(title,data)
	local group,tit = title:match("^(.*)\\+(.-)$")
	if group then
		group = self:FindGroup(self.registered_mapspotset_groups,group)
	else
		group = self.registered_mapspotset_groups
	end

	local set = self.MapSpotSetProto:NewRaw(title,tit or title,data)

	tinsert(group.guides,{full=title,short=tit or title,num=#self.registeredmapspotsets+1})
	tinsert(self.registeredmapspotsets,set)
end

ZGV.registered_sortings = {}
local test=0
function ZGV:RegisterGuideSorting(array)
	for i,gr in ipairs(array) do
		self.registered_sortings[gr]=i
	end
end

ZGV.registered_includes = {}
function ZGV:RegisterInclude(title,text)
	self.registered_includes[title]={text=text}

	self.registered_includes[title].GetParsed = function (self,params)
		local function parse_param(param)
			return params and params[param] or ""
		end
		return self.text:gsub("%%(%w+)%%",parse_param)
	end
end

ZGV.registered_functions = {}
function ZGV:RegisterFunction(title,func)
	self.registered_functions[title]={func=func}

	self.registered_functions[title].GetParsed = function (self,params)
		local function parse_param(param)
			return params[param] or ""
		end
		local text = func()
		return text:gsub("%%(%w+)%%",parse_param)
	end
end

--[[
function ZGV:UnregisterGuide(name)
	local data
	if type(name)=="number" then
		if self.registeredguides[name] then
			data = self.registeredguides[name].data
			table.remove(self.registeredguides,name)
			self:Print("Unregistered guide number: "..name)
		else
			self:Print("Cannot find guide number: "..name)
			return false
		end
	else
		local i,v
		for i,v in ipairs(self.registeredguides) do
			if v.title==name then
				data = v
				table.remove(self.registeredguides,i)
				self:Print("Unregistered guide: "..name)
			end
		end
		if not data then
			self:Print("Cannot find guide: "..name)
			return false
		end
	end
	if data.is_stored then
		self.db.global.storedguides[name] = nil
		self:Print("Removed stored data for: "..name)
	end
	return true
end
--]]

function coroutine_safe_pcall(f,arg)
	local co = coroutine.create(f)
	while true do
		local status, a,b,c,d,e = coroutine.resume(co,arg)
		if coroutine.status(co) == "suspended" then
			arg = coroutine.yield(a)   -- suspend across `mypcall'
		else
			return status, a,b,c,d,e   -- error or normal return
		end
	end
end


--[[
function ZGV:RegisterStoredGuides()
	local k,v
	for k,v in pairs(self.db.global.storedguides) do
		table.insert(self.registeredguides,{title=k,data=v,is_stored=true})
		self:Print("Retrieved guide "..k.." from storage.")
	end
end
--]]

function ZGV:UpdateMapButton()
	if self.db.profile.showmapbutton then ZygorGuidesViewerMapIcon:Show() else ZygorGuidesViewerMapIcon:Hide() end
end

function ZGV:GetGuides()
	if not ZygorGuidesViewer or not ZygorGuidesViewer.db or not ZygorGuidesViewer.registeredguides then return {} end
	local t = {}
	for i,data in ipairs(ZygorGuidesViewer.registeredguides) do
		t[i]=data.title
	end
	return t
end

function ZGV.GetGuidesRev()
	if not ZygorGuidesViewer or not ZygorGuidesViewer.db or not ZygorGuidesViewer.registeredguides then return {} end
	local t = {}
	for i,data in ipairs(ZygorGuidesViewer.registeredguides) do
		t[data.title]=i
	end
	return t
end

ZGV.ProfilerRunning=nil -- Just not to forget

function ZGV:ProfilerEnable()
	SetCVar("scriptProfile","1")
	ReloadUI()
end

-- This attempts to create a CSV profile report based on any ZGV function we can get our hands on
-- If you are doing funny stuff with functions, keep in mind that any function is accounted only once
-- so if a function is encountered in several namespaces, you only get one
function ZGV:ProfilerReport()
    -- Sanity
    if GetCVar("scriptProfile")~="1" and not self.ProfilerMode then
        self:Print("Profiling is not enabled, cannot do this, sorry. Please enable the profiler in Zygor settings and rety.")
        return
    end

	if self.ProfilerRunning then
		local gscope=getfenv(0) -- Global scope
		local culprits={} -- Table of all functions we are interested in
		local tablesVisited={} -- Tables we have already touched, a countermeasure against cyclical links
		tablesVisited[tablesVisited]=true
		tablesVisited[culprits]=true

		local function tryToRegisterFunc(f,n)
			if not culprits[f] or #culprits[f]>#n then
				culprits[f]={name=n} -- Yeah the function is the table key, you saw it right ~aprotas
			end
		end

		-- First, let's gather the culprits
		inspect=function(obj,objname) -- local func to recursively inspect an object and its subobjects and so on
			tablesVisited[obj]=true
			for kk,vv in pairs(obj) do
				if type(vv)=="function" then
					tryToRegisterFunc(vv,objname..":"..tostring(kk))
					elseif type(vv)=="table" and not tablesVisited[vv] then
					inspect(vv,objname.."."..tostring(kk))
				end
			end
		end
		inspect(gscope,"")

		-- Getting the data and saving them in a database
		self.db.profile.profiler_stats={}
		for fun,tab in pairs(culprits) do
			tab.fulltime,tab.count=GetFunctionCPUUsage(fun,true)
			tab.puretime,_=GetFunctionCPUUsage(fun,false)
			if tab.count>0 then -- We don't want THIS much litter, do we?
				table.insert(self.db.profile.profiler_stats,{name=tab.name,fulltime=tab.fulltime,
						puretime=tab.puretime,count=tab.count,fullavg=tab.fulltime/tab.count,pureavg=tab.puretime/tab.count})
			end
		end

		-- Nice, now we have to sort it and find 100-top entries, I guess that's gonna be enough for testing and not too much for overflood
		table.sort(self.db.profile.profiler_stats,function(n1,n2)return n1.fulltime>n2.fulltime end)

		local s=""
		s="fulltime,puretime,count,fullavg,pureavg,func\n"
		for i=1,min(#self.db.profile.profiler_stats,100) do
			local v=self.db.profile.profiler_stats[i]
			s=s..v.fulltime..","..v.puretime..","..v.count..","..v.fullavg..","..v.pureavg..","..v.name.."\n"
		end

		self:ShowDump(s,"Profiler Report",true) -- TODO emails and stuff,make it readonly
		self:Print("Profiler report created.")
		self.ProfilerRunning=nil
	else
		self:Print("Profiling recording started.")
		self.ProfilerRunning=true
	end
    -- Reset the counters
    ResetCPUUsage()
end


local math_floor = math.floor
local function round(num, digits)
	-- banker's rounding
	local mantissa = 10^digits
	local norm = num*mantissa
	norm = norm + 0.5
	local norm_f = math_floor(norm)
	if norm == norm_f and (norm_f % 2) ~= 0 then
		return (norm_f-1)/mantissa
	end
	return norm_f/mantissa
end
function ZGV:Test (arg1,arg2)
	local a={GetMapZones(GetCurrentMapContinent())}
	local x,y = GetPlayerMapPosition("player")
	local id = round(x*10000, 0) + round(y*10000, 0)*10001
	self:Print("You're in "..a[GetCurrentMapZone()].." at Cart2 coords "..id)
end

function ZGV:Echo (s)
	--if not self.db.profile.silent then
	self:Print(tostring(s))
	--end
end


local debugcolor="|cffff88dd"

local lastdate=0
local mscycle=0
local mscolors,mscolorsn={[0]="|cffffcc00",[1]="|cffffaa00"},2
local timecolor=mscolors[0]

function ZGV:Debug (msg,...)
	local flagsmsg
	local stack_depth=2
	while msg:sub(1,1)=="&" do
		local flag,rest = msg:match("^&([a-zA-Z0-9_]+)%s+(.*)$")
		if flag then
			if flag=="_SUB0" then
				stack_depth=nil
			elseif flag=="_SUB" then
				stack_depth=3
			elseif flag=="_SUB2" then
				stack_depth=4
			elseif flag=="_SUB3" then
				stack_depth=5
			else
				local flagdata = ZGV.db.profile.debug_flags and ZGV.db.profile.debug_flags[flag]
				if flagdata==false then return end -- otherwise assume it SET!
				if type(flagdata)=="table" and flagdata.color then flag = "|c"..flagdata.color..flag.."|r" end
				flagsmsg = (flagsmsg and (flagsmsg.." ") or "") .. "[" .. flag .. "]"
			end
			msg = rest
		else
			msg="?"..msg:sub(2) -- failsafe, cut the & off
		end
	end
	if flagsmsg then msg = flagsmsg.." "..msg end
	local formatted_msg = format(tostring(msg),...) :gsub("|r",debugcolor)

	local func
	if stack_depth then
		func = debugstack(stack_depth,1,-1)
		func = func:match("^(.-)\n") or func
		func = func:gsub(".*\\([^\\]-:%d+): in function `(.-)'","%1:%2") or func
		func = func:gsub(".*\\([^\\]-:%d+): in function.-string \"*(:.-)\"","%1:%2") or func
		func = func:gsub(".*\\([^\\]-:%d+): in function <.->","%1:<local>") or func
	else
		func=""
	end

	if self and self.db and self.db.profile and self.db.profile.debug and not self.db.profile.silentmode then
		self.DebugI = (self.DebugI or 0) + 1
		--func = func:match("in function `(.-)'") or func
		--func = func:match("in function.-string \"*(:.-)\"") or func
		local t = GetTime()
		local datestamp = ("%06.03f"):format(t%60)
		if t~=lastdate then
			mscycle=(mscycle+1)%mscolorsn
			lastdate=t
			timecolor=mscolors[mscycle]
		end
		local debugms = debugprofilestop() - t*1000

		local chatframe = ZGV.debugframe
		if not chatframe then
			chatframe = _G[ZGV.db.profile.debug_frame]
			ZGV.debugframe = chatframe
		end
		if not chatframe then chatframe=ChatFrame1 end

		chatframe:AddMessage(("|cffffee77Z|r: %s%s+%03d|r |cff00ddbb#%d: %s%s  |cffaaaaaa(%s)"):format(timecolor,datestamp,debugms,self.DebugI,debugcolor,formatted_msg,func))
	end
	self.Log:Add("%s (%s)",formatted_msg,func)
end


-- HACKS
function ZGV:ListQuests(from,to)
	local CQI=Cartographer_QuestInfo
	local qlog = ""
	for i=from,to do
		local level = CQI:PeekQuest(i)
		--if not level then level=0 end
		if level then
			local title,_,_,_,nobjs = CQI:GetQuestText(i,level)
			--if not title then title = CQI:GetQuestText(i,level) end -- well, they said to repeat it...
			--self:Print(i..": |cff808080|Hquest:"..i..":"..level.."|h["..tostring(title).."]|h|r "..(type(objs)=="table" and "{"..table.concat(nobjs,",").."}" or ""))
			qlog = qlog .. i..": "..tostring(title)..(type(nobjs)=="table" and " {"..table.concat(nobjs,",").."}" or "") .. "|n"
		end
	end
	if Chatter then
		Chatter:GetModule("Chat Copy").editBox:SetText(qlog)
		Chatter:GetModule("Chat Copy").editBox:HighlightText(0)
		Chatter:GetModule("Chat Copy").frame:Show()
	end
end

function ZGV:ReloadTranslation()
	for i,guide in ipairs(self.registeredguides) do
		for s,step in ipairs(guide.steps) do
			for g,goal in ipairs(step.goals) do
				goal.L=false
			end
		end
	end
end

-- used for steps and goals
--[[
function ZGV.ConditionTrue(subject,case)
	if not subject.conditions then return false end
	local f=subject.conditions[case]
	if type(f)=="function" then
		return f()
	elseif type(f)=="string" then
		f=subject.conditions[f]
		assert(type(f)=="function","What? This step has cross-referencing conditions? wtf.")
		return not f()
	end
end
--]]

function ZGV.gradient3(perc,ar,ag,ab,br,bg,bb,cr,cg,cb, middle)
	if perc >= 1 then
		return cr,cg,cb
	elseif perc<=0 then
		return ar,ag,ab
	else
		if perc<=middle then
			perc=perc/middle
			return ar+(br-ar)*perc, ag+(bg-ag)*perc, ab+(bb-ab)*perc
		else
			perc=(perc-middle)/(1-middle)
			return br+(cr-br)*perc, bg+(cg-bg)*perc, bb+(cb-bb)*perc
		end
	end
end

--hooksecurefunc("WorldMapFrame_UpdateQuests",function() if not InCombatLockdown() then text=nil end end)
--hooksecurefunc("QuestInfo_Display",function() if not InCombatLockdown() then shownFrame=nil bottomShownFrame=nil end end)



--[[


function FindAch_AchievementFrame_OnShow(self)
	local editbox = AchievementFrame_FindAch_Edit
	if not editbox then
		editbox = CreateFrame("EditBox","AchievementFrame_FindAch_Edit",AchievementFrame,"InputBoxTemplate")
		editbox:SetSize(150,30)
		editbox:SetPoint("TOPLEFT",AchievementFrame,"TOPLEFT",150,15)
		editbox:Show()
		editbox:SetFocus(false)
	end
end

function FindAch_Listen_OnEvent(self,event,...)
	if event=="ADDON_LOADED" and ...=="Blizzard_AchievementUI" then
		AchievementFrame:HookScript("OnShow",FindAch_AchievementFrame_OnShow)
	end
end
local frame=CreateFrame("FRAME","FindAch_Listen")
frame:SetScript("OnEvent",FindAch_Listen_OnEvent)
frame:RegisterEvent("ADDON_LOADED")

--]]

-- encraption.
function ZGV:CraptOnReload(name)

end


function ZGV:COMBAT_LOG_EVENT_UNFILTERED(event,time,evtype,a1,a2,a3,a4,a5)
	--print(evtype,a3)
	if evtype=="PARTY_KILL" then
		if a3==UnitName("player") then
			self.MagicKey.retarget_time = GetTime()
			self:SetMagicKey()
		end
	end
end


-- CONVENIENCE: /re for Reload
SLASH_RE1 = "/re"
function SlashCmdList.RE(text)  ReloadUI()  end

local lastbind
function ZGV:SetMagicKey(reset)
	if not self.MagicKey.FR then return end
	if reset then lastbind=nil end

	if not ZGV.db.profile.magickey or ZGV.db.profile.magickey==""
		or InCombatLockdown()
		or not ZGV.CurrentStep or not ZGV.CurrentStep.goals or not ZGV.Frame:IsShown()
		then
			self.MagicKey.FR:Hide()
			return
		end

	self.MagicKey.FR:Show()

	local bind,bind2

	local function DoBind(bind,bind2,desc)
		if not bind then
			self.MagicKey:SetHint("")
		end

		if lastbind~=(bind2 or bind) then
			if bind=="CLICK" then
				SetBindingClick(self.db.profile.magickey,bind2)
			else
				SetBinding(self.db.profile.magickey,bind)
			end
			lastbind=bind2 or bind
			self:Debug("Magic Key binding: "..tostring(bind2 or bind))
		end

		self.MagicKey:SetHint(desc)
	end

	-- turn in quests
	if self.db.profile.magickey_acceptturnin and GossipFrame:IsShown() or QuestFrame:IsShown() then
		for gi,g in ipairs(self.CurrentStep.goals) do
			if g.action=="accept" or g.action=="turnin" then
				return DoBind("CLICK","ZygorGuidesViewer_MagicKeyHint_Button", "Accept/Turn In")
			end
		end
	end

	-- suggest retargeting corpses
	if self.db.profile.magickey_targetcorpse and self.MagicKey.retarget_time and (GetTime()-self.MagicKey.retarget_time < 3) and not UnitName("target") then
		return DoBind("TARGETLASTTARGET",nil, "Target corpse")
	end


	local targetid = self.GetTargetId()
	if self.db.profile.magickey_loot and UnitIsDead("target") then
		return DoBind("InteractTarget",nil,"Loot")
	end

	for gi,g in ipairs(self.CurrentStep.goals) do
		if g:IsComplete() then
			-- do nothing ;P

		elseif targetid and ((UnitName("target") == g.target) or (targetid == (g.npcid or (g.mobs and g.mobs[1] and g.mobs[1].id)))) then
			-- interact in its varieties
			local hint
			if UnitIsFriend("target","player") and self.db.profile.magickey_talk then
				return DoBind("InteractTarget",nil,"Talk")
			elseif not UnitIsFriend("target","player") and self.db.profile.magickey_attack then
				return DoBind("InteractTarget",nil,"Attack")
			end

		elseif g.macro then
			local src = g.macrosrc
			local hint
			if src then
				local npc = src:match("/target (.+)\n")
				if npc then
					if self.db.profile.magickey_target then
						return DoBind("MACRO "..g.macro, nil, "Target: "..npc)
					end
				else
					if self.db.profile.magickey_itemspell then
						return DoBind("MACRO "..g.macro, nil, "Use macro: "..g:GetText())
					end
				end
			else
				return DoBind("MACRO "..g.macro, nil, "(macro?)")
			end


		elseif _G['ZygorGuidesViewerFrame_Act'..(g.num+1)..'Action']:IsShown() then
			if self.db.profile.magickey_itemspell then
				return DoBind("CLICK","ZygorGuidesViewerFrame_Act"..(g.num+1).."Action", g:GetText())
			end
		end

		--[[
		if (g.action=="kill" and g.target) or (g.action=="talk" and g.npcid) or (g.action=="from" and (g.mobs and g.mobs[1] and g.mobs[1].name)) then
			firstname=(g.target or g.npcid) or (g.mobs and g.mobs[1] and g.mobs[1].name)

			break
		end
		--]]
	end

	return DoBind(nil,nil,nil)
end

function ZGV.MagicButton_OnClick(but)
	ZGV:QuestAutoStuff()
end

function ZGV:MagicRaidMarker()
	ZGV.db.profile.magicmarker_mob=8
	ZGV.db.profile.magicmarker_npc=4
	if not UnitIsFriend("target","player") and ZGV.db.profile.magicmarker_mob and ZGV.db.profile.magicmarker_mob>0 then
		local icon = GetRaidTargetIndex("target")
		if icon~=ZGV.db.profile.magicmarker_mob then
			SetRaidTarget("target",ZGV.db.profile.magicmarker_mob)
		end
	elseif UnitIsFriend("target","player") and ZGV.db.profile.magicmarker_npc and ZGV.db.profile.magicmarker_npc>0 then
		local icon = GetRaidTargetIndex("target")
		if icon~=ZGV.db.profile.magicmarker_npc then
			SetRaidTarget("target",ZGV.db.profile.magicmarker_npc)
		end
	end
end
ZGV.MRM = ZGV.MagicRaidMarker

function ZGV:Unparse(id,y,m,d)
	local c,b=string.char,string.byte
	local function enc(a)
		print("coding "..a)
		return c(a%10+1+16*math.floor(a/10))
	end
	local s=("%08d%2d%2d%2d"):format(id,y%100,m,d)
	--local s="12345678010203"
	local s2=""
	for di,i in ipairs{id/100000,id/1000,id/10,id,y,m,d} do s2=s2..enc(b(math.floor(i)%10)) end
	return s2
end

function ZGV:DoMutex(m)
	if self.guidesets[m] then return true else self.guidesets[m]=true end
end

function ZGV:FindGuides(sub)
	sub=sub:lower()
	local found={}
	for gi,g in pairs(self.registered_groups.groups) do
		if g.name and (g.name:lower():find(sub,1,true) or g.name:lower():gsub("%p",""):find(sub,1,true)) then
			tinsert(found,g)
		end
	end
	for gi,g in pairs(self.registeredguides) do
		if (g.title_short and (g.title_short:lower():find(sub,1,true) or g.title_short:lower():gsub("%p",""):find(sub,1,true)))
		or (g.keywords and (g.keywords:lower():find(sub,1,true) or g.keywords:lower():gsub("%p",""):find(sub,1,true))) then
			tinsert(found,g)
		end
	end
	return found
end

function ZGV:UnloadUnusedGuides()
	self:Debug("Unimplemented: unloading unused guides.")
end

-- WARNING Heuristic in this thread. We assume that this is called only after all the guides are registered
-- and therefore, to optimize things a bit, we cache the results of parsing, so that subsequent request to
-- load the same type will immediately return. This will malfunction if the guides are added online between
-- the calls to this function. ~aprotas

local guideTypesLoaded={}

function ZGV:LoadGuidesByType(guidetype)
	assert(guidetype)
	if guideTypesLoaded[guidetype] then return end

	for gi,g in pairs(self.registeredguides) do
		if g.type==guidetype then
			g:Parse(true)
		end
	end
	guideTypesLoaded[guidetype]=true
end

function ZGV:LoadNeededGuides()
	local t0=debugprofilestop()
	for gi,g in pairs(self.registeredguides) do
		if g.need_to_parse then
			local success = g:Parse(true)
			g.need_to_parse = nil
			if debugprofilestop()-t0>100 then return nil end  -- 0.1s allowed here
		end
	end
	return true
end

ZGV.ParseLog = ""

function ZGV:Error(s,...)
	if (...) then s=s:format(...) end
	self:Print(s)
	ZGV.ParseLog = ZGV.ParseLog .. s .. "\n"
end

function ZGV_DEV()
	--ZGV:AddEvent("")
	--if ZGV.TimeShip then ZGV:ScheduleRepeatingTimer("TimeShip",0.1) end
	--ZGV:ScheduleRepeatingTimer("MemHogging",0.1)
end

--[=[ do	--[[-- EXPERIMENTAL : Ship Arrival Times --]]--
	ZGV.ShipTimes = {
		["stormwind-borean"] = { interval=272.0, m=301,f=0,x=0.1715,y=0.2584, reverse="borean-stormwind", shift=-121.0 },
		["borean-stormwind"] = { interval=272.0, m=486,f=0,x=0.5991,y=0.6957, reverse="stormwind-borean", shift=121.0 },
		["stormwind-darnassus"] = { interval=235.8, m=301,f=0,x=0.2165,y=0.5621, reverse="darnassus-stormwind", shift=116.5 },
		["darnassus-stormwind"] = { interval=235.8, m=41,f=0,x=0.5492,y=0.9405, reverse="stormwind-darnassus", shift=-116.5 },
	}

	function ZGV:TimeShip(ship)
		local TIMES = self.ShipTimes

		local time = GetTime()

		if not ship then
			local m,f,x,y = Astrolabe:GetCurrentPlayerPosition()
			for shipname,shipdata in pairs(TIMES) do
				local dist = Astrolabe:ComputeDistance(m,f,x,y,shipdata.m,shipdata.f,shipdata.x,shipdata.y)
				if dist and dist<3 and not IsFlying() and not IsSwimming() and (not shipdata.last_detected or time-shipdata.last_detected>90) then
					shipdata.last_detected = time
					print("SHIP DETECTED! OMG!",shipname)
					ship=shipname
					break
				end
			end
		end

		if ship=="?" then
			print("Ships at the moment:")
			for shipname,shipdata in pairs(TIMES) do
				if shipdata.interval then
					--local progress = time()-shipdata.last
					--local perc = progress % shipdata.interval
					--local missed = math.floor(progress/shipdata.interval)
					local nxt = shipdata.last
					if nxt then
						while (nxt<time) do nxt=nxt+shipdata.interval end
						print(shipname,": arrival in",nxt-time,", interval",shipdata.interval,", missed",math.floor((time-shipdata.last)/shipdata.interval))
					else
						print(shipname,": arrives every",shipdata.interval,"s")
					end
				elseif shipdata.last then
					print(shipname,": timing - last seen",time-shipdata.last,"s ago")
				end
			end
			return
		end

		if ship then
			if not TIMES[ship] then TIMES[ship]={} end
			local shipdata = TIMES[ship]

			if not shipdata.last then
				if not shipdata.interval then
					print("Ship",ship,"initial time recorded. Wait for another.")
				else
					print("Ship",ship,"arrival synchronized.")
				end
			else
				if not shipdata.interval then shipdata.interval = time-shipdata.last end
				print(time-shipdata.last,"seconds since last",ship,"trip.")
				if not shipdata.intervals then shipdata.intervals={} end
				tinsert(shipdata.intervals,time-shipdata.last)
				local sum=0
				for i=1,#shipdata.intervals do  sum=sum+shipdata.intervals[i]  end
				print("That's average",sum/#shipdata.intervals,"over",#shipdata.intervals)
			end
			shipdata.last = time
			if shipdata.reverse then TIMES[shipdata.reverse].last=time+shipdata.shift end
		end
	end
end --]=]

local lasttime,lastmem=GetTime(),0
local memavg={0,0,0,0,0}
function ZGV:MemHogging()
	UpdateAddOnMemoryUsage()
	local mem = GetAddOnMemoryUsage(addonName)
	local time = GetTime()
	if time~=lasttime and mem~=lastmem then
		local total=0
		for i=1,4 do memavg[i]=memavg[i+1] total=total+memavg[i+1] end
		local kbs = floor((mem-lastmem)/(time-lasttime))
		memavg[5]=kbs

		kbs=(kbs+total)/5

		ZGV.DebugFrame.text1:SetText(("%s %d"):format(strrep(".",kbs/3),kbs))
	end
	lasttime,lastmem=time,mem
end

local memmark=0
function ZGV:MemHogStart()
	UpdateAddOnMemoryUsage()
	memmark=GetAddOnMemoryUsage(addonName)
end

function ZGV:MemHogStop(desc)
	UpdateAddOnMemoryUsage()
	local memmark2=GetAddOnMemoryUsage(addonName)
	print("|cff00ff88",desc,("%.1f"):format(memmark2-memmark))
end

function ZGV:MemHogTest()
	self:MemHogStart()
	self:MemHogStop()
	self:MemHogStart()
	self:MemHogStop()
	self:MemHogStart()
	local a={}
	for i=1,1000 do a[i]={} end
	self:MemHogStop()
end

function ZGV:DumpScenario()
	local s = ""
	local name, currentStage, numStages = C_Scenario.GetInfo()
	if not name then self:Print("You're not in a scenario, dude.") return end
	s=s..("SCENARIO: %s\n"):format(name)

	local stageName, stageDescription, numCriteria = C_Scenario.GetStepInfo()
	if not stageName then self:Print(s.."No stage at this time, sorry.") return end
	s=s..("STAGE %d/%d: %s (%s)\n"):format(currentStage, numStages, stageName, stageDescription)

	for crit=1,numCriteria do
		local criteriaString, criteriaType, criteriaCompleted, quantity, totalQuantity, flags, assetID, quantityString, criteriaID = C_Scenario.GetCriteriaInfo(crit)
		criteriaString = string.format("%d/%d %s", quantity, totalQuantity, criteriaString);

		s=s..("CRITERIA %d/%d [#%s]: %s  --  %s  (%s, asset %s, flags %s)\n"):format(
			crit,numCriteria, criteriaID, criteriaString,  criteriaCompleted and "completed" or "incomplete",
			criteriaType,assetID,flags)
	end

	ZGV:ShowDump(s,name)
end


function ZGV:StartFPSFrame()
	if not ZGV.FPSFrame then
		ZGV.FPSFrame = ZGV.ChainCall(CreateFrame("FRAME","ZygorFPSFrame",UIParent)) :SetPoint("BOTTOMLEFT") :SetSize(201,50) :SetFrameStrata("DIALOG") .__END
		ZGV.FPSFrame.back = ZGV.ChainCall(ZGV.FPSFrame:CreateTexture()) :SetAllPoints() :SetTexture(0,0,0,1) .__END
		ZGV.FPSFrame.bars={}
		for b=1,200 do
			tinsert(ZGV.FPSFrame.bars, ZGV.ChainCall(ZGV.FPSFrame:CreateTexture()) :SetPoint("BOTTOMLEFT",ZGV.FPSFrame,"BOTTOMLEFT",b,0) :SetSize(1,50) :SetTexture(1,1,1) .__END)
		end
		local elapses={0,0,0,0,0,0,0,0,0}
		function ZGV.FPSFrame:OnUpdate(elapsed)
			--local fps=GetFramerate()
			local fps=1/elapsed
			local n=floor(min(200,max(1,elapsed*100)))  -- n bars to cover
			for b=1,200-n do
				self.bars[b]:SetHeight(self.bars[b+n]:GetHeight())
			end
			local h=min(fps/2,50)  -- 1..50
			for b=200-n+1,200 do
				self.bars[b]:SetHeight(h)
			end
			for b=1,200 do
				local h1=self.bars[b]:GetHeight()/50
				local r=(h1<0.5) and 1 or 2-(h1*2)
				local g=(h1<0.5) and h1*2 or 1
				self.bars[b]:SetTexture(r,g,0)
			end
		end
		ZGV.FPSFrame:SetScript("OnUpdate",ZGV.FPSFrame.OnUpdate)
	end
	ZGV.FPSFrame:SetShown(ZGV.db.profile.fpsgraph)
end


ZGV.Quest_Choices = {}
function ZGV:ZYGOR__QUEST_CHOICE_SENT(event,id,choice)
	ZGV:Debug("Quest choice: choice id |cffffeeaa%d|r, selected |cffffeeaa%d|r",id,choice)
	ZGV.Quest_Choices[id]=choice
end

function ZGV:Hook_QuestChoice()
	ZGV.Original_SendQuestChoiceResponse = SendQuestChoiceResponse
	SendQuestChoiceResponse = ZGV.Surrogate_SendQuestChoiceResponse
	ZGV:AddMessage("ZYGOR__QUEST_CHOICE_SENT")
end

function ZGV.Surrogate_SendQuestChoiceResponse(choice)
	local id = GetQuestChoiceInfo()
	ZGV:SendMessage("ZYGOR__QUEST_CHOICE_SENT",id,choice)
	ZGV.Original_SendQuestChoiceResponse(choice)
end

--local Gratuity = LibStub("LibGratuity-3.0")
--[[
function ZGV:UPDATE_MOUSEOVER_UNIT()
	if not ZGV.TargetPingFrame then
		ZGV.TargetPingFrame = ZGV.ChainCall(CreateFrame("FRAME","ZGVTargetPingFrame")) :SetSize(100,100) .__END
		ZGV.TargetPingFrame.tex = ZGV.ChainCall(ZGV.TargetPingFrame:CreateTexture()) :SetPoint("CENTER") :SetTexture(ZGV.DIR.."\\skins\\minimaparrow-ant") :SetAlpha(0.3) :SetBlendMode("ADD") .__END
		ZGV.TargetPingFrame:SetScript("OnUpdate",function(self,elapsed)
			if self.cycle>0 then
				self.tex:SetAlpha(self.cycle*0.3)
				local siz=self.cycle*10+60
				self.tex:SetSize(siz,siz)
				self.cycle=self.cycle-elapsed*3
			end
			if self.cycle<=0 then self:Hide() end
		end)
		ZGV.TargetPingFrame.cycle=0
	end

	local npc = GameTooltipTextLeft1:GetText()
	if npc and self.CurrentStep and self.CurrentStep.goals then
		local found
		for g,goal in ipairs(ZGV.CurrentStep.goals) do
			if npc==goal.target or npc==goal.npc then
				found=true
				break
			elseif goal.mobs then
				for m,mob in ipairs(goal.mobs) do
					if mob.name==npc then found=true break end
				end
			end
		end
		if found then
			PlaySound("gsTitleOptionExit")
			ZGV.TargetPingFrame:ClearAllPoints()
			local x,y=GetCursorPosition()
			ZGV.TargetPingFrame:SetPoint("CENTER",nil,"BOTTOMLEFT",x,y)
			ZGV.TargetPingFrame.cycle=1
			ZGV.TargetPingFrame:Show()
		end
	end
end
--]]

ZGV.Spark={}

-- These were sparks. They were distracting. Leaving for future.
-- Actually, they're fun.
function ZGV:UPDATE_MOUSEOVER_UNIT()
	local time=GetTime()
	if not ZGV.TargetPingFrame then
		ZGV.TargetPingFrame = ZGV.ChainCall(CreateFrame("FRAME","ZGVTargetPingFrame")) :SetSize(1,1) :SetPoint("BOTTOMLEFT") .__END
		local NUMFLY=20
		ZGV.TargetPingFrame.tex={}
		local function Fly(tex,elapsed)
			tex.life=tex.life-elapsed
			if tex.life<=0 then tex.life=0.001 end
			local siz = tex.siz*tex.life
			tex:SetSize(siz,siz)
			tex.y=tex.y+tex.dy*elapsed
			tex.dy=tex.dy-elapsed*45
			tex.x=tex.x+tex.dx*elapsed
			tex:SetPoint("CENTER",tex:GetParent(),"BOTTOMLEFT",tex.x,tex.y)
		end
		local function Start(tex,x,y)
			tex.dx=60*(random()-0.5)
			tex.dy=60*(random()-0.3)
			tex.x=x+tex.dx*random()*0.3
			tex.y=y+tex.dy*random()*0.3
			tex.siz=random(5,10)
			tex.life=1
			tex:Show()
		end
		for i=1,NUMFLY do
			ZGV.TargetPingFrame.tex[i] = ZGV.ChainCall(ZGV.TargetPingFrame:CreateTexture()) :SetPoint("CENTER") :SetSize(20,20) :SetTexture(ZGV.DIR.."\\skins\\minimaparrow-ant") :SetBlendMode("ADD") .__END
			ZGV.TargetPingFrame.tex[i].Fly = Fly
			ZGV.TargetPingFrame.tex[i].Start = Start
			ZGV.TargetPingFrame.tex[i].life=0
		end
		ZGV.TargetPingFrame.nexttex=1
		ZGV.TargetPingFrame.firing=0

		-- continuous firing.
		---[[
		ZGV.TargetPingFrame:SetScript("OnUpdate",function(self,elapsed)
			if self.firing then
				--self.firing=self.firing-elapsed
				local x,y=GetCursorPosition()
				-- generate new
				self.nexttex=self.nexttex+1  if self.nexttex>NUMFLY then self.nexttex=1 end
				self.tex[self.nexttex]:Start(x,y)
			end
			for i=1,NUMFLY do local t=self.tex[i]  if t.life>0.01 then t:Fly(elapsed) else t:Hide() end end
			if not GameTooltip:IsShown() or GameTooltip:GetAlpha()<0.9 then self.firing=nil end
		end)
		function ZGV.TargetPingFrame:Start()
			PlaySound("gsTitleOptionExit")
			--self:SetPoint("CENTER",nil,"BOTTOMLEFT",x,y)
			self.firing=1
			self:Show()
		end
		--]]

		--[[
		ZGV.TargetPingFrame:SetScript("OnUpdate",function(self,elapsed)
			if self.tex[1].life<0 then return end
			for i=1,NUMFLY do local t=self.tex[i]  if t.life>0.01 then t:Fly(elapsed) else t:Hide() end end
		end)
		function ZGV.TargetPingFrame:Start()
			PlaySound("gsTitleOptionExit")
			--self:SetPoint("CENTER",nil,"BOTTOMLEFT",x,y)
			local x,y=GetCursorPosition()
			for i=1,NUMFLY do  self.tex[i]:Start(x,y)  end
			self:Show()
		end
		--]]
	end

	if ZGV.Spark.LastEvent=="CURSOR_UPDATE" and ZGV.Spark.LastCursorUpdate~=time then return end

	local npc = GameTooltip:IsShown() and GameTooltip:GetAlpha()>0.9 and GameTooltipTextLeft1:GetText()
	if npc and self.CurrentStep and self.CurrentStep.goals then
		local found
		for g,goal in ipairs(ZGV.CurrentStep.goals) do
			if npc==goal.target or npc==goal.npc then
				found=true
				break
			elseif goal.mobs then
				for m,mob in ipairs(goal.mobs) do
					if mob.name==npc then found=true break end
				end
			end
		end
		if found then
			ZGV.TargetPingFrame:Start()
		else
			ZGV.TargetPingFrame.firing=nil
		end
	else
		ZGV.TargetPingFrame.firing=nil
	end
	ZGV.Spark.LastEvent="UPDATE_MOUSEOVER_UNIT"
end
function ZGV:CURSOR_UPDATE()
	--ZGV:UPDATE_MOUSEOVER_UNIT()
	ZGV.Spark.LastCursorUpdate=GetTime()
	if ZGV.TargetPingFrame then ZGV.TargetPingFrame.firing=nil end
	ZGV.Spark.LastEvent="CURSOR_UPDATE"
end
--

