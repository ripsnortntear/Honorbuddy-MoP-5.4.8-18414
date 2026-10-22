local ZGV = ZygorGuidesViewer
if not ZGV then return end

ZGV.Parser = {}
local Parser = ZGV.Parser

local L = ZygorGuidesViewer_L("Main")
local LG = ZygorGuidesViewer_L("Guides")

local BZL=ZGV.BZL

local table,string,tonumber,ipairs,pairs,setmetatable,tinsert = table,string,tonumber,ipairs,pairs,setmetatable,tinsert

--[[
local function split (s,t)
	local l = {n=0}
	local f = function (s)
		l.n = l.n + 1
		l[l.n] = s
	end
	local p = "%s*(.-)%s*"..t.."%s*"
	s = string.gsub(s,"^%s+","")
	s = string.gsub(s,"%s+$","")
	s = string.gsub(s,p,f)
	l.n = l.n + 1
	l[l.n] = string.gsub(s,"(%s%s*)$","")
	return l
end
--]]

-- Spec to Talent Tree # lookup table
local classtalents=
{
	["WARRIOR"]		= { "Arms","Fury","Protection" },
	["DEATHKNIGHT"]		= { "Blood","Frost","Unholy" },
	["MONK"]		= { "Brewmaster","Mistweaver","Windwalker" },
	["PALADIN"]		= { "Holy","Protection","Retribution" },
	["HUNTER"]		= { "Beast Mastery","Marksmanship","Survival" },
	["ROGUE"]		= { "Assassination","Combat","Subtlety" },
	["PRIEST"]		= { "Discipline","Holy","Shadow" },
	["MAGE"]		= { "Arcane","Fire","Frost" },
	["WARLOCK"]		= { "Affliction","Demonology","Destruction" },
	["SHAMAN"]		= { "Elemental","Enhancement","Restoration" },
	["DRUID"]		= { "Balance","Feral","Guardian","Restoration" },
}
-- Reversing the table
for k,v in pairs(classtalents) do
	local reverse={}
	for i,j in ipairs(v) do
		reverse[j]=i
	end
	classtalents[k]=reverse
end
ZGV.SpecToNumber=classtalents

ZGV.actionmeta = {
	goto = { skippable = true },
	fpath = { skippable = true },
	home = { skippable = true },
	hearth = { skippable = true },
}

local function split(str,sep,reverse)
	local fields = {}
	str = str..sep
	local tinsert=tinsert
	str:gsub("(.-)"..sep, function(c) if reverse then tinsert(fields, c, 1) else tinsert(fields, c) end end)
	return fields
end

local function ParseMapXYDist(text,insanefloor)
	local map,x,y,dist,flr,_,err

	-- old syntax: 12.3,44.2,-150
	map,x,y,dist = text:match("^(.+)[%s,]+([0-9%.]+),([0-9%.]+),(-?[0-9%.]+)$")
	if not map then x,y,dist = text:match("^([0-9%.]+),([0-9%.]+),(-?[0-9%.]+)$") end
	if not x then map,x,y = text:match("^(.+),([0-9%.]+),(-?[0-9%.]+)$") end
	if not x then
		-- new syntax, or just old without distance: 12.3,44.2 <150
		_,disttype,dist = text:match("^(.-)%s*([<>])([0-9%.]+)$")	text=_ or text
		_,x,y = text:match("^(.-)%s*([0-9%.]+),([0-9%.]+)$")  text=_ or text
		if #text>1 then map=text end
	end

	if not x and not y and not dist and not map then map=text end

	if map then
		-- only map name should be left in the text - might be text or numeric, and might have /floor
		_,flr = map:match("^(.-)%s*/%s*(%d+)$")
		map = _ or map
	end

	map = tonumber(map) or map
	if map=="" then map=nil end
	flr = tonumber(flr)
	x = tonumber(x)  x=x and x*0.01
	y = tonumber(y)  y=y and y*0.01
	dist=tonumber(dist)
	if dist and disttype==">" then dist=-dist end   -- distance written as <40 is usual; >40 = reverse distance check: "leave the area".

	--	if x then x=x/100 end
	--	if y then y=y/100 end
	--	if dist then dist=dist/100 or 0.2 end
	--if not dist then dist=0.2 end

	-- Map IDs, here we come.
	if type(map)=="string" then
		local mapid = ZGV.MapIDsByName[map]
		if type(mapid)=="table" then mapid=mapid[1] end
		if mapid then map=mapid else err="ERROR: map '"..map.."' unknown." map=0 end
	end
	-- strictly prevent a "map, no floor" return. Either both are nil, or neither is. This is to prevent cases when the map would change but the floor would accidentally remain.
	if map and not insanefloor then
		flr=ZGV:SanitizeMapFloor(map,flr)
	end
	-- if map then flr=flr or 0 end
	--if map then map=map+0.01*(floor or 0) end

	return map,flr,x,y,dist, err
end
_G['ParseMapXYDist']=ParseMapXYDist
Parser.ParseMapXYDist=ParseMapXYDist


-- cache map IDs. Testing.
function ZGV:DumpMapIDsByName()
	ZGV.MapIDsByName = {}
	local arr=ZGV.MapIDsByName
	for i=1,2000 do
		local n=GetMapNameByID(i)
		if n then
			if arr[n] then
				if type(arr[n])=="number" then arr[n]={arr[n]} end
				tinsert(arr[n],i)
			else
				arr[n]=i
			end
		end
	end

	local s="ZGV.MapIDsByName={\n"
	for n,ids in pairs(arr) do
		s=s.." ['".. n:gsub("'","\\'") .."']="
		if type(ids)=="number" then
			s=s..ids..",\n"
		else
			s=s.."{"..table.concat(ids,",").."},\n"
		end
	end
	s=s.."}"
	self:ShowDump(s,"Map IDs",true)
end


local LibRover = LibStub("LibRover-1.0")
local LibTaxi = LibStub("LibTaxi-1.0")
ZGV.MapIDsByName = LibRover.data.MapIDsByName

-- This should be helping with zoned maps...

local zoned = {
	['Gilneas']={539,545,678,679},
	['Mount Hyjal']={606,683},
	['The Lost Isles']={544,681,682},
	['The Maelstrom']={737,751},
	['Twilight Highlands']={700,770},
	['Uldum']={720,748},
	['Zul\'Gurub']={697,793},
}

local function ParseID(str)
	local name,id,nid,obj
	name,id = str:match("(.*)##([0-9/]*)")
	if not name then id = str:match("^([0-9/]*)$") end
	if id then
		nid,obj = id:match("([0-9]*)/([0-9]*)")
		if nid then
			id=nid
		end
	end
	if id then id = tonumber(id) end
	if obj then obj = tonumber(obj) end
	if not name and not id then name=str end
	return name, id, obj
end
Parser.ParseID = ParseID

--- parse just the header, until the first 'step' tag. No chunking, just header data extraction.
function Parser:ParseHeader(text)
	if not text then return {} end
	local guides = {}
	local index = 1

	text = text .. "\n"

	local linecount=0

	local header = {}

	while (index<#text) do
		local st,en,line=string.find(text,"(.-)\n",index)
		if not en then break end
		index = en + 1

		linecount=linecount+1
		if linecount>100000 then
			return nil,linecount,"More than 100000 lines!?"
		end

		line = line:gsub("^[%s	]+","")
		line = line:gsub("[%s	]+$","")
		line = line:gsub("//.*$","")
		line = line:gsub("||","|")

		local cmd,params = line:match("([^%s]*)%s?(.*)")

		if cmd then
			if cmd=="step" then
				break
			else
				header[cmd]=params
			end
		end
	end

	if header['guide'] then
		header['title']=header['guide']
		header['guide']=nil
	end

	return header
end

local ConditionEnv = {
	_G = _G,
	-- variables needing update
	level=1,
	intlevel=1,
	ZGV=ZGV,

	-- these must be assigned in an _Update() call, if "local" scripts are to work. HORRIBLE local-faking.
	guide=nil,
	step=nil,
	goal=nil,

	_Update = function()
		Parser.ConditionEnv.level = ZGV:GetPlayerPreciseLevel()
		Parser.ConditionEnv.intlevel = floor(Parser.ConditionEnv.level)
		if ZGV.db.char.fakelevel and ZGV.db.char.fakelevel>0 then Parser.ConditionEnv.level=ZGV.db.char.fakelevel end
	end,

	_Setup = function()
		-- reputation 'constants'
		for standing,num in pairs(ZGV.StandingNamesEngRev) do Parser.ConditionEnv[standing]=num end
		for standing,num in pairs(ZGV.FriendshipNamesEngRev) do Parser.ConditionEnv[standing]=num end
	end,

	_SetLocal = function(guide,step,goal)
		Parser.ConditionEnv.guide=guide
		Parser.ConditionEnv.step=step
		Parser.ConditionEnv.goal=goal
	end,

	-- independent data feeds
	rep = function(faction)
		if ZGV:GetReputation(faction).friendship then --dummy proof this.
			return ZGV:GetReputation(faction).friendship
		end
		return ZGV:GetReputation(faction).standing
	end,
	friend = function(faction)
		return ZGV:GetReputation(faction).friendship
	end,
	repval = function(faction,baselevel)
		baselevel = ZGV.StandingNamesEngRev[baselevel]
		local standing =  ZGV:GetReputation(faction).standing
		if standing < baselevel then return -99999 --We are under the needed standing fo sho
		elseif standing > baselevel then return 99999 --We are over the standing
		else return ZGV:GetReputation(faction).val-ZGV:GetReputation(faction).min
		end
	end,
	skill = function(skill)
		return ZGV:GetSkill(skill).level
	end,
	skillmax = function(skill)
		return ZGV:GetSkill(skill).max
	end,
	raceclass = function(raceclass)
		return ZGV:RaceClassMatch(raceclass,true)
	end,
	hasprof = function(hasprof,minlevel,maxlevel)
		return ZGV:MatchProfs(hasprof,minlevel,maxlevel)
	end,
	isevent = function(eventname)
		return ZGV:FindEvent(eventname)
	end,
	completedq = function(id)
		return ZGV.completedQuests[id]
	end,
	havequest = function(id)
		local q=ZGV.questsbyid[id]
		return q and q.inlog
	end,
	achieved = function(achieveid)
		return select(4,GetAchievementInfo(achieveid))
	end,
	knowspell = function(spellid)
		return IsSpellKnown(spellid)
	end,
	haspet = function(speciesId)
		local numCollected, limit = C_PetJournal.GetNumCollectedInfo(speciesId)
		return ((numCollected or 0) > 0)
	end,
	thunderstage = function()
		return ZGV:GetThunderStage()
	end,
	thunderprogress = function()
		return math.floor(select(2,ZGV:GetThunderStage())*100)
	end,
	hasmount = function(mountspell)
		local id,name,spell
		for i=1,GetNumCompanions("MOUNT") do
			 id,name,spell = GetCompanionInfo("MOUNT",i)
			 if spell==mountspell then return true end
		end
	end,
	itemcount = function(...)
		local total = 0
		local count = select("#", ...)
		for i = 1, count do
			total = total + GetItemCount(select(i, ...))
		end
		return total
	end,
	curcount = function(curid)
		return (select(2,GetCurrencyInfo(curid)))
	end,
	heroic_dung = function(diff)
		diff = diff or 2
		return GetDungeonDifficultyID()==diff
	end,
}
Parser.ConditionEnv=ConditionEnv  --DEBUG

local function MakeCondition(cond,forcebool)
	local s
	if cond:find("return") then
		-- leave it be
	elseif forcebool then
		cond = ("_Update()  return not not (%s)"):format(cond)
	else
		cond = ("_Update()  return (%s)"):format(cond)
	end
	local fun,err = loadstring(cond)
	if fun then setfenv(fun,ConditionEnv) end
	return fun,err
end
--local yield=coroutine.yield

--- parse ONE guide section into usable arrays.
function Parser:ParseEntry(guide,fully_parse,lastparsed)
	local text = guide.rawdata

	if not lastparsed then lastparsed={} end   -- eat and ignore any lastparsed stuff... too bad.
	lastparsed.linenum=-1
	lastparsed.linedata="(none)"
	if not text then return nil,"No text!",0 end
	local index = 1

	local step

	local prevmap,prevfloor
	local prevlevel = 0
	local prevtitle

	local prevpathvars={}

	text = text .. "\n"

	local linecount=0

	local noobsoletequests = {}
	local dailyquests = ZGV.dailyQuests

	local function COLOR_LOC(s) return "|cffffee77"..s.."|r" end

	local _

	local strfind = string.find

	if fully_parse and not guide.steps then guide.steps={} end

	-- clear some fields before reparsing
	guide.description=nil

	--local debug
	--if text:find("goto The Exodar,44.9,24.2") then debug=true end

	local last5lines = {}

	local function parseerror(msg)
		local chunk = ""
		for i=1,#last5lines do chunk=chunk.."\n"..last5lines[i] end
		return nil,msg,linecount,guide and guide.steps and #guide.steps or 0,chunk
	end

	if fully_parse then
		-- do inclusion. Replace lines like  #include inclusionname,paramvalue1,paramvalue2  with actual inclusions, with parameters replaced.
		local function do_include(line)
			local orig_line = line
			line = line:gsub("\\,","##COMMA##") :gsub("\\\"","##QUOTE##")
			line = line:gsub('%s*//.-$',"")
			local words={strsplit(",",line)}
			local title = tremove(words,1)
			title = title:gsub("^%s*",""):gsub("%s*$","")
			title = title:gsub('^"(.-)"',"%1")
			local params={}
			for i,word in ipairs(words) do
				local param,val = word:match("(.-)%s*=%s*\"(.-)\"")
				if param then params[param]=val end
			end
			local inclusion = ZGV.registered_includes[title]
			if not inclusion then ZGV:Debug("&parser #include not found in |cffffaa00%s|r: '|cffff5500%s|r' in line '|cffaaaaaa%s|r'",guide.title,title,orig_line) end
			return inclusion and inclusion:GetParsed(params) or ""
		end

		local safety=0
		while (text:find("#include")) do
			text = text:gsub("#include%s*(.-)%s-[\r\n]",do_include)
			safety=safety+1
			assert(safety<20,"#include recursion exceeded safety depth")
		end

		-- replace faux newlines
		text = text:gsub("||","\n")
	end

	local breakout

	local do_debug

	local indoors_flag

	guide.does_macrotext_follow = nil

	local GOALTYPES = ZGV.GOALTYPES

	while (index<#text) do
		local st,en,line=strfind(text,"%s*(.-)%s*\n",index)
		--if debug then print(line) end
		if not en then break end
		index = en + 1

		linecount=linecount+1
		if linecount>100000 then
			return nil,linecount,"More than 100000 lines!?"
		end
		--if linecount%50==0 then yield() end

		--line = line:gsub("^%s+","")
		--line = line:gsub("%s+$","") --done in the find

		--st,en = strfind(line,"//",1,true)
		--if st then line=line:sub(1,st-1) end
		-- not really faster

		--if guide.crapt then
		--	line,guide.crapt=__uncrapt(line,guide.crapt)
		--end


		lastparsed.linenum=linecount
		lastparsed.linedata=line

		line = line:gsub("%s*%-%-.*","",1) :gsub("%s*//.*","",1)


		-- Process the line!
		-- it's supposedly left- and right-trimmed by the find above.
		if guide.does_macrotext_follow then -- Stop processing
			if guide.macro.body == ZGV.MacroGuideProto.body then -- If it's not set, reset
				guide.macro.body=""
			end
			guide.macro.body=guide.macro.body..line.."\n"
			if #guide.macro.body>255 then
				return parseerror("macros cannot exceed 255 characters:\n"..guide.macro.body)
			end
		else
			-- remove comments, retrim
			--line = line:gsub("%s*//.*$","")
			--line = line:gsub("%-%-.*$","")

			-- extract indent, retrim
			local indent
			indent,line = line:match("^(%.*)%s*(.-)$")

			-- extract asterisk, retrim
			local had_asterisk=nil
			line=line:gsub("^%*%s*",function() had_asterisk=1 return "" end)

			-- strip leading pipes, retrim
			line = line:gsub("^|%s*","")

			local goal  -- DON'T initialize yet, we could be in a "step" line, and there's a pretty bunch of those.

			local chunkcount=1

			if do_debug then ZGV:Debug("&parser line "..linecount..": |cffffeedd"..line) end


			-- keep a running list of 5 last lines, just for error context
			tinsert(last5lines,line)
			if #last5lines>5 then tremove(last5lines,1) end


			-- cloak escaped pipes
			line = line:gsub("\\|","%%PIPE%%")

			line = line .. "|"
			for chunk in line:gmatch("%s*(.-)%s*|+") do if #chunk>0 then

				-- un-cloak escaped pipes
				chunk = chunk:gsub("%%PIPE%%","|")

				-- we have a trimmed chunk here

				-- spacify leading quotes
				chunk = chunk:gsub("^'%s*","' ")

				--chunk = chunk:gsub("^@(%S)","@ %1")  --unused so far

				--local cmd,params = strsplit(" 	",chunk,2)
				local cmd,params = chunk:match("^([^%s]*)%s*(.-)$")
				params=params or ""

				if do_debug then ZGV:Debug(": "..chunk) end

				if cmd=="leechsteps" then
					-- works anywhere
					local fromguide,from,to = params:match("^\"(.+)\"%s-,%s-(.-)%s-,%s-(.-)$")

					guide.leechsteps_guide = ZGV:SanitizeGuideTitle(fromguide or params:match("^\"(.+)\"$") or params) :gsub("\\+","\\")
					guide.leechsteps_from = tonumber(from) or 1
					guide.leechsteps_to = tonumber(to) or 999

					if fully_parse then

						-- okay, just do it now.
						local leechedguide = ZGV:GetGuideByTitle(guide.leechsteps_guide)
						if not leechedguide then ZGV:Debug("leeched '"..guide.leechsteps_guide.."' not found in "..guide.title) breakout=true break end
						if not leechedguide.fully_parsed then
							ZGV:Debug("Forcefully parsing "..guide.leechsteps_guide.." for leeching")
							local parsed = leechedguide:Parse(true)
							if not parsed or not leechedguide.steps then
								ZGV:Debug("leeched not parsing")
								breakout=true
								break
							end
						end

						ZGV:Debug("Parser:'leechsteps' guide "..guide.leechsteps_guide.." from "..guide.leechsteps_from.." to "..guide.leechsteps_to)

						local leeched=0
						-- okay, leech
						for i=guide.leechsteps_from,guide.leechsteps_to do
							-- holy shit. Clone all the steps. No memory shall be spared :(
							local step = leechedguide.steps[i]
							if step then
								local newstep = { goals = {} }
								setmetatable(newstep,ZGV.StepProto_mt)
								for k,v in pairs(step) do
									if k=="goals" then
										for gi,goal in ipairs(v) do
											local newgoal = {}
											setmetatable(newgoal,ZGV.GoalProto_mt)
											for gk,gv in pairs(goal) do
												if type(gv)~="function" then newgoal[gk]=gv end
											end
											newgoal.parentStep = newstep
											tinsert(newstep.goals,newgoal)
										end
									elseif type(v)=="function" then
										if ZGV.StepProto[k]~=v then newstep[k]=v end
									else
										newstep[k]=v
									end
								end

								newstep.parentGuide = guide
								newstep.num = #guide.steps+1

								-- cloned! save, next.
								tinsert(guide.steps,newstep)
								leeched=leeched+1
							end
						end

						ZGV:Debug("Leeched "..leeched.." steps, total now "..(#guide.steps)..".")
					end
					break ----------------------------- skip rest of line!
				end

				-- guide parameters
				if not step then -- all-guide tags
					if cmd=="defaultfor" then
						local cond = "raceclass('"..params.."',true) and " .. (params=="DeathKnight" and "level<59" or "level<15")
						guide.condition_suggested = MakeCondition(cond)
						guide.condition_suggested_raw = cond
					elseif cmd=="next" then
						params = params:gsub("^\"(.-)\"$","%1")
						params = tonumber(params) or params
						guide.next=ZGV:SanitizeGuideTitle(params)
					elseif cmd=="author" then
						guide.author=params
					elseif cmd=="hidden" then
						guide.hidden=params
					elseif cmd=="trial" then
						guide.trial=(params=="on")
					elseif cmd=="dungeon" then
						guide.dungeon = tonumber(params)
					elseif cmd=="dungeondifficulty" then
						if params=="Heroic" then guide.dungeondifficulty = 2
						elseif params=="Challenge" then guide.dungeondifficulty = 8
						else --This is normal dungeons, or default to this for badly formatted params
							guide.dungeondifficulty = 1
						end
					elseif cmd=="pet" then
						local speciesId = tonumber(params)

						if not speciesId then return parseerror("bad pet number") end

						guide.petSpeciesID = speciesId

						ZGV.PetBattle.PetJournal.RegisterGuide(speciesId,guide)

						local _, _, _, creatureID = C_PetJournal.GetPetInfoBySpeciesID(speciesId or 0)

						ZGV.CreatureDetector:RegisterPetID(creatureID,guide)

						local cond = "haspet("..speciesId..")"

						local fun,err = MakeCondition(cond,true)
						if not fun then return parseerror(err) end
						guide['condition_end_raw']=cond
						guide['condition_end']=fun

					elseif cmd=="achieveid" then
						local achievetable = split(params,",",true)
						if guide.achieveid then
							local id,i
							for id,i in pairs(guide.achieveid) do tinsert(achievetable,id,1) end
						end

						guide.achieveid = achievetable
					elseif cmd=="monkquest" then
						guide.monkquest = tonumber(params)
					elseif cmd=="dungeonfloor" then
						guide.dungeonfloor = tonumber(params)
					elseif cmd=="description" then
						local keywords = params:match("^# (.+)")
						if keywords then
							guide.keywords=keywords
						else
							guide[cmd]=(guide[cmd] and (guide[cmd].."\n") or "") .. params
						end

					--elseif cmd=="faction" then --unused
					--	guide[cmd]=params
					elseif cmd=="startlevel" then
						guide.startlevel=tonumber(params)
						if not guide[cmd] then return parseerror("bad level number") end
						prevlevel=math.floor(guide[cmd])

					elseif cmd=="endlevel" then
						guide.endlevel=tonumber(params)
						if not guide[cmd] then return parseerror("bad level number") end

					elseif cmd=="type" then
						guide.type=params

					elseif cmd=="sugGroup" then
						-- TODO this is a hack.... a little >.>
						guide.sugGroup=params
						-- add this to our registered groups by hand.
						local group = ZGV:FindGroup(ZGV.registered_groups,"SUGGESTED\\"..guide.sugGroup) or ZGV.registered_groups
						local found
						-- Make sure no duplicates show up
						for i,g in ipairs(group.guides) do
							if g.title == guide.title then
								found = true
								break
							end
						end
						if not found then tinsert(group.guides,guide) end
					elseif cmd=="completion" then
						local pars = split(params,",")
						guide.completionmode = pars[1]
						guide.completionparams = {pars[2],pars[3],pars[4]}

					elseif cmd=="condition" then

						local case,cond = params:match("(.-) (.+)$")
						if (case=="suggested" or case=="valid" or case=="invalid" or case=="end") then
							local cond2,msg = cond:match("(.+)!!(.+)")
							if cond2 then cond=cond2 end
							if cond:sub(1,1)=="!" then
								-- special condition tag
								cond=cond:sub(2)
								local tag,data = cond:match("(.-) (.+)")
								data=data or true
								tag=tag or cond
								guide['condition_'..case..'_'..cond]=data
							else
								-- Hold up, let's see if that is a pet or mount guide
								-- Umm... Parsing a Lua condition using regexps..? Oh dear... ~sinus
								cond:gsub("has(%w*)%s-%((%d+)%)",function(object,id)
									if object=="pet" then --should be obsolete but leaving in for safety.
										ZGV.CreatureDetector:RegisterPetID(tonumber(id),guide) -- TODO mark duplicates
									elseif object=="mount" then
										ZGV.CreatureDetector:RegisterMountSpell(tonumber(id),guide) -- TODO mark duplicates
									end
								end)
																--Checking for achievements to get the achievement number
								if cond:find("achieved") then
									if not guide.achieveid then guide.achieveid = {} end
									tinsert(guide.achieveid,cond:match("%d+"),1)
								end

								local fun,err = MakeCondition(cond,true)
								if not fun then return parseerror(err) end
								guide['condition_'..case..'_raw']=cond
								guide['condition_'..case]=fun
								guide['condition_'..case..'_msg']=msg
							end
						else
							return parseerror("wrong condition type (not suggested/valid/invalid/end) or rogue guide condition in steps")
						end

					-- TODO the parseerror here seems to wreak mayhem everywhere
					-- Only for certain class
					elseif cmd=="class" then
						-- Do we need the support of something like "only for druids and shamans" ?
						if guide.class and guide.class~=params then return parseerror("can't specify multiple classes for a guide, make it general") end
						local classes={}
						FillLocalizedClassList(classes)
						if not classes[params] then return parseerror("class "..params.." is unknown to science") end
						guide.icon=guide.icon or { texname="Interface\\Glues\\CharacterCreate\\UI-CharacterCreate-Classes",
													coords = CLASS_ICON_TCOORDS[params] }
						guide.classicon=guide.icon -- In case we need to restore it for some reason
						guide.class=params

					-- Only for certain specs
					elseif cmd=="spec" then

						--local function tablelength(T) -- TODO make it more beautiful, anyone?
						--Done. :P ~sinus 2013-03-15

						if not guide.class then return parseerror("spec specified without a class, what do you mean?") end
						local spec=ZGV.SpecToNumber[guide.class][params]
						if not spec then return parseerror("spec "..params.." unknown for class ".. tonumber(guide.class)) end
						if not guide.spec then guide.spec={} end
						local lclass,kclass=UnitClass("player")
						if kclass==guide.class then -- If this is our class we may want to show a spec icon
							if not next(guide.spec) then
								-- This is *probably* for a single spec and for our class
								local _,_,_,specicon=GetSpecializationInfo(spec,false,false,nil)
								guide.icon= { texname=specicon }
							else
								-- This is a multispec thing, screw it, let's show a class icon
								guide.icon=guide.classicon
							end
						end
						guide.spec[spec]=true

					elseif cmd=="model" then
						-- guide-wide
						if not ZGV.CreatureDetector.PetMirror then
							ZGV.CreatureDetector.PetMirror=CreateFrame("PlayerModel")
						end
						ZGV.CreatureDetector.PetMirror:Show() -- briefly

						local models = split(params,",")
						for i=1,#models do
							local num = tonumber(models[i])
							local npcid
							if not num then
								-- try to use NPC number as model source
								npcid = tonumber(models[i]:match("npc (%d*)"))
								if npcid then num=ZGV.NPCModels[npcid] end
							end
							if num then
								models[i]=num
								ZGV.CreatureDetector.PetMirror:SetDisplayInfo(num)
								local model=ZGV.CreatureDetector.PetMirror:GetModel()
								if model then
									ZGV.CreatureDetector:RegisterGuideModel(model,guide)
								else
									ZGV:Debug("Unknown model "..num)
								end
							end
						end
						guide.model = models

						ZGV.CreatureDetector.PetMirror:Hide() -- and stay low

					elseif cmd=="image" then
						guide.image=ZGV.guide_images_installed and ZGV.DIR.."\\Guides\\Images\\"..params -- TODO support other locations

					elseif cmd=="icon" then
						local path,coords=params:match("(.-)%s(.*)")
						if not path then
							path=params
							coords={ 0,1,0,1 }
						elseif coords then
							coords={ coords:match("([0-9.]*),%s*([0-9.]*),%s*([0-9.]*),%s*([0-9.]*)") }
							for k,v in ipairs(coords) do coords[k]=tonumber(v) end
						end
						guide.icon= { texname=path, coords=coords }

					elseif cmd=="crapt" then
						guide[cmd]=params

					elseif cmd=="dynamic" then
						guide[cmd]=(params~="off")

					elseif cmd=="macroname" then
						-- TODO make sure it aint unique
						local name=params
						if not name then return parseerror("macro paramaters absent") end
						if #name>14 then return parseerror("macroname too long: "..name) end
						name=name:sub(1,14)
						name="ZG"..name
						if not guide.macro then guide.macro={} end
						guide.macro.name=name


					elseif cmd=="macroicon" then
						if not guide.macro then guide.macro={} end
						guide.macro.icon=params

					-- Macros. NB: there shouldnt be steps
					elseif cmd=="macrotext" then
						if not guide.macro then guide.macro = {} end
						setmetatable(guide.macro,ZGV.MacroGuideProto_mt)
						guide.condition_end=function(self) return not not self.macro:LocateMacro() end
						guide.condition_end_msg = L["macro_exists"]
						guide.type="MACRO"
						guide.completionmode="macro"
						guide.completionparams=""

						guide.does_macrotext_follow = true
					end
				end

				if cmd=="step" and (not step or #step.goals>0) then
					-- only start a step if there were goals in the former one! Make "step step step step" errors allowed.
					if guide.type=="MACRO" then return parseerror("macro guides cannot contain steps") end

					if not fully_parse then
						guide.steps=nil

						-- THIS SUCKS. Find the last ding to find the last level.
						while st do
							local lev
							st,en,lev = text:find("[ \t\r\n]ding ([0-9]+)[ \t\r\n]",en+1)
							if lev then prevlevel = tonumber(lev) end
						end
						breakout=true
						break
					end

					step = { goals = {}, map = prevmap, floor=prevfloor, level = prevlevel, num = #guide.steps+1, parentGuide=guide, title=prevtitle }
					setmetatable(step,ZGV.StepProto_mt)

					tinsert(guide.steps,step)

					params = params:gsub("^\"(.-)\"$","%1") -- strip quotes
					if params~="" then
						step.label=params
					end

				end

				if step and cmd~="step" then

					-- step parameters

					if cmd=="label" then
						params = params:gsub("^\"(.-)\"$","%1")
						step.label = params

					elseif cmd=="level" then
						step[cmd]=params
						prevlevel=tonumber(params)

					elseif cmd=="title" then
						if chunkcount>1 then
							goal[cmd]=params
						else
							if params:find("^+") then
								params=params:gsub("^+%s*","")
								prevtitle=params
							end
							step[cmd]=params
						end

					elseif cmd=="map" then
						local map,flr,x,y,dist, err = ParseMapXYDist(params)  -- a bit redundant, but a map/floor pair is fine to get here.
						if not map then return parseerror("map '"..params.."' unknown.") end
						prevmap,prevfloor = map,flr
						step.map,step.floor = map,flr

					elseif cmd=="travelfor" then
						step.travelfor = tonumber(params)

					--[[
					elseif cmd=="@" then
						local map,x,y
						map,x,y = params:match("(.+),([0-9.]+),([0-9.]+)")
						if not map then
							x,y = params:match("([0-9.]+),([0-9.]+)")
						end
						if not x then
							map = params
						end
						if not map then
							map = prevmap
						end
						step['map']=map
						prevmap=map
						if x or y then
							step['x']=x
							step['y']=y
						end
					--]]



					else
						if not goal then -- initialize a new goal line, since we're in a valid step now
							goal={}
						end

						if cmd=="goto" or cmd=="at" or cmd=="fly" then

							if do_debug then ZGV:Debug(":== "..cmd..": ["..params.."]") end

							goal.action = goal.action or cmd
							local errortxt = GOALTYPES[cmd].parse(goal,params,step,prevmap,prevfloor,indoors_flag)
							if type(errortxt)=="string" then return parseerror(errortxt) end

							step.map,step.floor = goal.map,goal.floor
							prevmap,prevfloor = step.map,step.floor

						elseif cmd=="minizone" then
							goal.waypoint_minizone = params -- if nil, then it's ignored.
						elseif cmd=="region" then
							goal.waypoint_region = params -- if nil, then it's ignored.
						elseif cmd=="indoors" then
							goal.waypoint_minizone = params -- if nil, then it's ignored.
							goal.waypoint_indoors = 1 -- if nil, then it's ignored.

						elseif cmd=="path" then

							local path=params
							if not step.waypath then step.waypath={follow="loose",loop=true,ants="straight",coords={}} end
							for var,val in pairs(prevpathvars) do step.waypath[var] = val or step.waypath[var] end

							-- remove plusses, trim
							path=path:gsub("^%+%s*","")
							path=path:gsub("%s*[	;]+%s*",";")
							path=path .. ";"
							for coord in path:gmatch("(.-);") do
								local map,flr,x,y,dist,err = ParseMapXYDist(coord)
								if x then
									if err then return parseerror(err) end  -- might happen, if the coords look good but map is bogus.
									local point = {map=map or step.map or prevmap,floor=flr or step.floor or prevfloor,x=x,y=y,dist=dist}
									tinsert(step.waypath.coords,point)
									prevmap,prevfloor = point.map,point.floor
								else
									-- no coords..? maybe a path command, then?
									local var,val = coord:match("^([^%s]+)%s+(.+)$")
									if not val then var,val=coord,1 end
									if val=="off" then val=false end
									if var:sub(1,1)=="<" then
										-- assign something to the last point. This sucks, but CBA to make it any better right now.
										step.waypath.coords[#step.waypath.coords][var:sub(2)] = tonumber(val) or val
									else
										-- plain two-word variable.
										step.waypath[var] = tonumber(val) or val
										prevpathvars[var] = tonumber(val) or val
									end
								end
							end

						elseif cmd=="from" then
							goal.action = goal.action or cmd
							params=params:gsub([[\,]],"!!comma!!")
							params=params:gsub("%s*,%s*",",")
							goal.mobsraw = params
							local mobs = split(params,",")

							goal.mobs = {}
							for i,mob in ipairs(mobs) do
								mobs[i]=mob:gsub("!!comma!!",",")
								local name,plural = mob:match("^(.+)(%+)$")
								if not plural then name=mob end

								local nm,id = ParseID(name)

								tinsert(goal.mobs,{name=nm,id=id,pl=plural and true or false})
							end
							goal.mobspre = mobs

						elseif cmd=="condition" then	-- new in 3.1: supersede the "startlevel" eventually.
							goal.action = goal.action or cmd
							local fun,err = MakeCondition(params,false)
							if not fun then return parseerror(err) end
							goal.condition_complete_raw=params
							goal.condition_complete = fun


						elseif cmd=="script" then
							goal.script = params
						elseif cmd=="updatescript" then
							goal.updatescript = params
						elseif cmd=="macro" then
							goal.macrosrc = params
						elseif cmd=="buttonicon" then
							goal.buttonicon = tonumber(params) or 1

						elseif cmd=="modelnpc" then
							goal.modelname,goal.modelnpc = ParseID(params)

						elseif cmd=="modeldisplay" then
							goal.modelname,goal.displayinfo = ParseID(params)

						-- conditions

						elseif cmd=="only" then
							local cond = params:match("^if%s+(.*)$")
							if cond then
								-- condition match
								local subject = goal  if chunkcount==1 then subject=step end

								local fun,err = MakeCondition(cond,true)
								if not fun then return parseerror(err) end

								subject.condition_visible_raw=cond
								subject.condition_visible=fun
							else
								-- race/class match
								params = params:gsub("%s*,%s*",",")
								if goal.action or goal.text or chunkcount>1 then
									if not ZGV:RaceClassMatch(split(params,",")) then
										goal={}
										break
									end -- skip goal line altogether
								else
									step.requirement=split(params,",")
								end
							end

						-- extra tags

						elseif cmd=="next" then
							params = params:gsub("^\"(.-)\"$","%1")
							if params=="" then params="+1" end
							goal.next=params

						elseif cmd=="autoscript" then
							goal.autoscript = params
						elseif cmd=="n" then
							goal.force_nocomplete = true
						elseif cmd=="c" then
							goal.force_complete = true
						elseif cmd=="opt" then
							goal.optional = true

						-- waypoint creation:
						elseif cmd=="noway" then -- prevent waypointing, even if there are coords
							goal.force_noway = true
						elseif cmd=="nowayinzone" then -- prevent waypointing if we're in the same zone
							goal.force_nowayinzone = true

						elseif cmd=="sticky" then
							goal.force_sticky = true
						elseif cmd=="important" then
							had_asterisk = true
						elseif cmd=="future" then
							goal.future = true  -- if quest-related, then don't worry if the quest isn't in the log.
						elseif cmd=="noobsolete" then
							if goal then
								goal.noobsolete = true
								if goal.questid then noobsoletequests[goal.questid] = true end
							else
								guide.noobsolete = true
							end

						elseif cmd=="daily" then
							if goal and goal.questid then dailyquests[goal.questid] = true end
							if not guide.steps or #guide.steps==0 then guide.daily=true end
						elseif cmd=="repeatable" then
							if goal and goal.questid then goal.repeatablequest = true end

						elseif cmd=="showtext" then  -- show original quest/achievement text
							goal.showtext=true

						elseif cmd=="tip" then
							-- highlight _text_
							--params = params:gsub("_(.-)_","|cffffee88%1|r")
							-- or not, since it reverts to white.

							goal.tooltip = LG[params]

						elseif cmd=="image" then
							goal.image = params


						elseif cmd=="model" then
							-- local model
							goal.modelname,goal.model = ParseID(params)

						elseif cmd=="nomodels" then
							goal.nomodels=true


						elseif cmd=="quest" or cmd=="q" then
							local first=params:match("^(.-),")
							if first then params=first end
							goal.quest,goal.questid,goal.objnum = ParseID(params)
							if not goal.questid then return parseerror("no questid in parameter") end
						elseif cmd=="or" then
							goal.orlogic = params and tonumber(params) or 1
						elseif cmd=="override" then
							goal.override = true
						elseif cmd=="instant" then  -- when we HAVE to use the title, for instant-complete quests.
							if goal.questid then ZGV.instantQuests[goal.questid]=true end
							goal.usetitle=true
						elseif cmd=="killcount" then  -- use killcounter for non-quest mobs
							goal.usekillcount=true
						elseif cmd=="indoors" then
							goal.is_indoors=true
							if params=="start" then indoors_flag=true end
						elseif cmd=="outdoors" then
							indoors_flag=nil
							goal.is_indoors=nil

						elseif cmd=="simulate" then
							goal.simulate = params

						elseif cmd=="debug" then
							do_debug = (params=="on")


						-- NEW: catch-all from the goals table.
						elseif GOALTYPES[cmd] and GOALTYPES[cmd].parse then
							goal.action = goal.action or cmd
							local errortxt = GOALTYPES[cmd].parse(goal,params,step)
							if type(errortxt)=="string" then return parseerror(errortxt) end


						else

							local text = (cmd=="'") and params or chunk

							text = LG[text]

							-- protect {scripts} from pretty much everything
							local braces={} -- har har
							local function savebrace(s,magic) -- har har indeed, quite an ugly hack to discriminate between brace types
								tinsert(braces,s)
								return not magic and ("%%BRACE"..#braces.."%%") or ("@@BRACE"..#braces.."??"..magic.."@@")
							end
							text = text:gsub("{=(.-)%?%?(.-)=}",savebrace):gsub("{(.-)}",savebrace)


							-- snag coordinates for waypointing, with distance
							local miniprevmap,miniprevfloor = prevmap,prevfloor
							local ways={}
							local function saveway(s)
								local map,flr,x,y,dist, err = ParseMapXYDist(s)
								if err or not x or not y then
									return "["..s.."]"
								else
									local hide
									if s:sub(1,1)=="-" then
										hide=true
										s=s:sub(2)
									end
									tinsert(ways,{map=map or goal.map or step.map or miniprevmap,floor=flr or goal.floor or step.floor or miniprevfloor,x=x,y=y,dist=dist})
									miniprevmap, miniprevfloor = map or miniprevmap, flr or miniprevfloor
									s = ""
									if not hide then
										if map~=goal.map or flr~=goal.floor then s=GetMapNameByID(map).." " end
										s = COLOR_LOC(L['coords']:format(x*100,y*100))
									end
									return s
								end
							end

							text = text:gsub("%[(.-)%]",saveway)
							if ways[1] then
								goal.dist = tonumber(ways[1].dist)
								goal.x = ways[1].x
								goal.y = ways[1].y
								goal.map,goal.floor = ways[1].map,ways[1].floor
								goal.ways=ways
							end


							-- highlight _text_
							text = text:gsub("_(.-)_","|cffffee88%1|r")

							-- uncloak braces
							local function loadbrace(s,magic) -- for explaination what magic is see savebrace
								return not magic and ("{"..braces[tonumber(s)].."}") or ("{="..braces[tonumber(s)].."??"..magic.."=}")
							end
							text = text:gsub("@@BRACE(%d+)%?%?(.-)@@",loadbrace):gsub("%%%%BRACE(%d+)%%%%",loadbrace)

							if text=="_" then text=" " end

							goal.text = text
						end

					end
				end


				if cmd=="ding" then
					prevlevel = goal.level
				end


				chunkcount=chunkcount+1
				if chunkcount>20 then
					return nil,"More than 20 chunks in line",linecount,line
				end
			end end
			-- all chunks are in goal or step... hopefully.


			if goal and next(goal) then
				if not step then return nil,"What? Unknown data before first 'step' tag, or what?",linecount,line end  -- shouldn't happen anymore!

				-- so there's something to record? go ahead.

				setmetatable(goal,ZGV.GoalProto_mt)

				goal.parentStep = step
				goal.num = #step.goals+1

				tinsert(step.goals,goal)

				if not goal.action then
					if (goal.x or goal.map) then
						goal.action = "goto"
					elseif goal.text then
						goal.action = "text"
					end
				end

				if goal.questid and noobsoletequests[goal.questid] then
					goal.noobsolete = true
				end

				if goal.dist and math.abs(goal.dist)<5 then goal.dist=goal.dist*20 end

				goal.showinbrief = had_asterisk

				if (goal.action=="get" or goal.action=="kill" or goal.action=="goal") and not goal.questid and not goal.objnum and not goal.criteriaid and not goal.achieveid and not goal.force_nocomplete then
					goal.force_nocomplete = true
				end
			end

			if goal then goal.indent = #indent end

			if breakout then break end
		end
	end

	if guide.type=="MACRO" then
		-- Try to localize the Spell and item names
		local function ReplaceMacroIDs(guide)
			local macroguide=guide.macro
			local ready=true

			-- Iterating over all the lines
			macroguide.body=macroguide.body:gsub(".-\n",function(par)
				local isItem=par:match("/use") -- If it's not an item, it's most probably a spell
				return par:gsub("##([0-9]*)",function(id)
					if not isItem then
						id=GetSpellInfo(id) -- That's an easy part, really
					else
						local item=GetItemInfo(id)
						if not item then -- Item was not found, damn
							ready=false
							id="##"..id
						else -- Item was found
							id=item
						end
					end
					return id
				end)
			end)

			if #macroguide.body>255 then
				return parseerror("macro body exceeded 255 characters after translation:\n"..macroguide.body)
			end


			-- If some data is missing we'd better cool down and retry later
			if not ready then
				ZGV:ScheduleTimer(function() ReplaceMacroIDs(guide) end,2)
			else
				macroguide:Update() -- Localize the instance in the macro book
				guide.fully_parsed=true
			end
		end
		ReplaceMacroIDs(guide)
	end
	--[[
	if prevlevel and not guide.condition_end then
		guide.endlevel = prevlevel
		guide.endlevel_implicit = true
		local cond = "level>=self.endlevel"
		guide['condition_end']=MakeCondition(cond)
		guide['condition_end_raw']=cond
		guide['condition_end_msg']=("You've reached level %.1f"):format(prevlevel)
	end
	--]]

	return guide
end

local s=string local c=string.char
local h="" for i=33,127 do h=h .. c(i) end
function __uncrapt(text,q)

end

tinsert(ZGV.startups,function(self)
	do -- unit-check ParseMapXYDist
		function is(v1,v2) -- evil "almost equal"
			return math.abs(v1-v2)<0.001
		end

		local map,x,y,dist
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City")  assert(map==301,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Dalaran/2",true)  assert(map==504 and flr==2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("12.3,12.8")  assert(is(x,0.123) and is(y,0.128),"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("12.3,12.8,2")  assert(is(x,0.123) and is(y,0.128) and dist==2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("12.3,12.8,-2")  assert(is(x,0.123) and is(y,0.128) and dist==-2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City,12.3,12.8")  assert(map==301 and is(x,0.123) and is(y,0.128),"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City,12.3,12.8,2")  assert(map==301 and is(x,0.123) and is(y,0.128) and dist==2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City,12.3,12.8,-2")  assert(map==301 and is(x,0.123) and is(y,0.128) and dist==-2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Dalaran/2,12.3,12.8,-2",true)  assert(map==504 and flr==2 and is(x,0.123) and is(y,0.128) and dist==-2,"ParseMapXYDist fail")
		-- new syntax
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City 12.3,12.8")  assert(map==301 and is(x,0.123) and is(y,0.128),"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("Stormwind City 12.3,12.8 >5")  assert(map==301 and is(x,0.123) and is(y,0.128) and dist==-5,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("504")  assert(map==504,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("504/2",true)  assert(map==504 and flr==2,"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("504 12.3,12.8")  assert(map==504 and is(x,0.123) and is(y,0.128),"ParseMapXYDist fail")
		map,flr,x,y,dist = ParseMapXYDist("504 12.3,12.8 <5")  assert(map==504 and is(x,0.123) and is(y,0.128) and dist==5,"ParseMapXYDist fail")
	end
end)

--[[
do -- unit-check Translate
	assert(Translate("bla")=="bla")
	assert(Translate("bla [deDE] bladede","deDE")=="bladede")
	assert(Translate("bla [deDE] bladede [frFR] blafrfr","deDE")=="bladede")
	--assert(Translate("'bla")=="blaUS")
	--assert(Translate("'blabla")=="blabla",tostring(Translate("''blabla")))
end
--]]