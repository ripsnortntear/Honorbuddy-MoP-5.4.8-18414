local ZGV = ZygorGuidesViewer
if not ZGV then return end

ZGV.skills = {}

local LS=ZygorGuidesViewer_L("Skills")

ZGV.skillSpells = {
	['Alchemy']=2259,
	['Blacksmithing']=2018,
	['Inscription']=45357,
	['Jewelcrafting']=25229,
	['Leatherworking']=2108,
	['Tailoring']=3908,
	['Enchanting']=7411,
	['Engineering']=4036,

	--['Herbalism']=2366,  -- that's Herb Gathering
	['Mining']=2575,
	['Smelting']=2656,
	['Skinning']=8613,

	['Archaeology']=78670,
	['Cooking']=2550,
		['Way of the Grill']=124694,
		['Way of the Wok']=125584,
		['Way of the Pot']=125586,
		['Way of the Steamer']=125587,
		['Way of the Oven']=125588,
		['Way of the Brew']=125589,
	['First Aid']=3273,
	['Fishing']=7620,
}
local skillSpells=ZGV.skillSpells

local CookingSkills = { --Copy from skillSpells, just to make scanning them easier because we only want to scan cooking masteries at one point.
	--Feel free to remove the duplicate code if you think of a better way. ~Errc
	['Way of the Pot']=125586,
	['Way of the Grill']=124694,
	['Way of the Wok']=125584,
	['Way of the Steamer']=125587,
	['Way of the Oven']=125588,
	['Way of the Brew']=125589,
}

-- sinus 2013-01-10 : this uses the "skill" numbers, found on wowhead.com/skill=171 for example. This is a backup if the above spell numbers start failing like Herbalism.
ZGV.skillIDs = {
	['Alchemy']=171,
	['Blacksmithing']=164,
	['Enchanting']=333,
	['Engineering']=202,
	['Inscription']=773,
	['Jewelcrafting']=755,
	['Leatherworking']=165,
	['Tailoring']=197,

	['Herbalism']=182,
	['Mining']=186,
	--['Smelting']=2656,
	['Skinning']=393,

	['Archaeology']=794,
	['Cooking']=185,
		--['Way of the Grill']=124694,
		--['Way of the Wok']=125584,
		--['Way of the Pot']=125586,
		--['Way of the Steamer']=125587,
		--['Way of the Oven']=125588,
		--['Way of the Brew']=125589,
	['First Aid']=129,
	['Fishing']=356,
	['Riding']=762,
}

ZGV.skillLocale = {
	[129]={deDE="Erste Hilfe",esES="Primeros auxilios",frFR="Secourisme",ptBR="Primeiros Socorros",ruRU="Первая помощь"},
	[164]={deDE="Schmiedekunst",esES="Herrería",frFR="Forge",ptBR="Ferraria",ruRU="Кузнечное дело"},
	[165]={deDE="Lederverarbeitung",esES="Peletería",frFR="Travail du cuir",ptBR="Couraria",ruRU="Кожевничество"},
	[171]={deDE="Alchemie",esES="Alquimia",frFR="Alchimie",ptBR="Alquimia",ruRU="Алхимия"},
	[182]={deDE="Kräuterkunde",esES="Herboristería",frFR="Herboristerie",ptBR="Herborismo",ruRU="Травничество"},
	[185]={deDE="Kochkunst",esES="Cocina",frFR="Cuisine",ptBR="Culinária",ruRU="Кулинария"},
	[186]={deDE="Bergbau",esES="Minería",frFR="Minage",ptBR="Mineração",ruRU="Горное дело"},
	[197]={deDE="Schneiderei",esES="Sastrería",frFR="Couture",ptBR="Alfaiataria",ruRU="Портняжное дело"},
	[202]={deDE="Ingenieurskunst",esES="Ingeniería",frFR="Ingénierie",ptBR="Engenharia",ruRU="Инженерное дело"},
	[333]={deDE="Verzauberkunst",esES="Encantamiento",frFR="Enchantement",ptBR="Encantamento",ruRU="Наложение чар"},
	[356]={deDE="Angeln",esES="Pesca",frFR="Pêche",ptBR="Pesca",ruRU="Рыбная ловля"},
	[393]={deDE="Kürschnerei",esES="Desuello",frFR="Dépeçage",ptBR="Esfolamento",ruRU="Снятие шкур"},
	[755]={deDE="Juwelenschleifen",esES="Joyería",frFR="Joaillerie",ptBR="Joalheria",ruRU="Ювелирное дело"},
	[762]={deDE="Reiten",esES="Equitación",frFR="Monte",ptBR="Montaria",ruRU="Верховая езда"},
	[773]={deDE="Inschriftenkunde",esES="Inscripción",frFR="Calligraphie",ptBR="Escrivania",ruRU="Начертание"},
	[794]={deDE="Archäologie",esES="Arqueología",frFR="Archéologie",ptBR="Arqueologia",ruRU="Археология"},
} -- GETS TRIMMED.
for id,data in pairs(ZGV.skillLocale) do ZGV.skillLocale[id]=data[GetLocale()] end


ZGV.LocaleSkills={}
setmetatable(ZGV.LocaleSkills,{__index=function(t,skill) return ZGV.skillLocale[ZGV.skillIDs[skill] or 0] or GetSpellInfo(ZGV.skillSpells[skill]) or skill end})
ZGV.LocaleSkillsR={}
setmetatable(ZGV.LocaleSkillsR,{__index=function(t,q) return q end})

tinsert(ZGV.startups,function(self)
	self:AddEvent("PLAYER_ENTERING_WORLD","CacheSkills")
	self:AddEvent("SKILL_LINES_CHANGED","CacheSkills")
	self:AddEvent("TRADE_SKILL_UPDATE","CacheSkills")
	self:AddEvent("CHAT_MSG_SKILL","CacheSkills")
	self:AddEvent("CHAT_MSG_SYSTEM","Profession_CHAT_MSG_SYSTEM")
	self:AddEvent("TRADE_SKILL_SHOW","Profession_TRADE_SKILL_SHOW")
	--self:AddEvent("CHAT_MSG_COMBAT_FACTION_CHANGE","CHAT_MSG_COMBAT_FACTION_CHANGE_Faction")

	self.skills[""]={
		active=false,
		level=0,
		max=0
	}

	if GetLocale()~="enUS" then
		for spell,num in pairs(skillSpells) do -- Localize spell-based skills. So far this only leaves Herbalism out, but who knows...
			ZGV.LocaleSkills[spell]=GetSpellInfo(num)
			ZGV.LocaleSkillsR[ZGV.LocaleSkills[spell]]=spell
		end
	end
end)

local ERR_LEARN_RECIPE_S_fmt = ERR_LEARN_RECIPE_S:gsub("%.","%%."):gsub("%%s","(.+)")
--local TRADESKILL_LOG_FIRSTPERSON_fmt = TRADESKILL_LOG_FIRSTPERSON:gsub("%%s","(.-)")

function ZGV:Profession_CHAT_MSG_SYSTEM(event,text)
	local _,_,item = text:find(ERR_LEARN_RECIPE_S_fmt)
	if item then
		self.recentlyLearnedRecipes[item]=true
	end
end

function ZGV:Profession_TRADE_SKILL_SHOW()
	self:CacheSkills()
	if self.Delayed_PerformTradeSkill_step then
		self:PerformTradeSkillGoal(self.Delayed_PerformTradeSkill_step,self.Delayed_PerformTradeSkill_goal)
		self.Delayed_PerformTradeSkill_step=nil
		self.Delayed_PerformTradeSkill_goal=nil
	end
end

function ZGV:CacheSkills()
	local TradeSkillFrame = TradeSkillFrame

	if not TradeSkillFrame then
	--TODO
	end

	local profs={GetProfessions()}
	for i,prof in pairs(profs) do
		local name,icon,rank,maxrank,numspells,spelloffset,skillline = GetProfessionInfo(prof)

		local pro = self.skills[name]
		if not pro then
			pro={}
			self.skills[name]=pro
		end
		pro.level=rank
		pro.max=maxrank
		pro.active=true
		pro.skillID=skillline
		pro.spell=self.skillSpells[name]
		pro.name=name

		if skillline == 185 and rank >= 535 then --Cooking > 535, so check for masteries
			for id,level in pairs(ZGV.db.char.cookingMasteries) do
				local name = GetSpellInfo(id)

				local pro = self.skills[name]
				if not pro then
					pro={}
					self.skills[name]=pro
				end
				pro.level=level
				pro.max=600 --HARD CODED
				pro.active=true -- It is in db.char... so that means we had seen it at some point
				pro.skillID=id
				pro.spell=id
				pro.name=name
			end
		end
	end

	self:CacheRecipes(profs)  -- or try to, anyway. --Do Cooking masteries too
end

function ZGV:GetSkill(name)
	local skill,spell
	skill = self.skillIDs[name]
	if not skill then spell = self.skillSpells[name] end

	if ZGV.db.profile.fakeskills[name] then
		return ZGV.db.profile.fakeskills[name]
	else
		return self:FindSkill(skill,spell)
		--local name = ZGV.LocaleSkills[name]
		--if name~="Cooking" then print(name) end
		--return self.skills[name] or self.skills[""]
	end
end

function ZGV:FindSkill(skill,spell)
	for name,skilldata in pairs(self.skills) do
		if (skill and skilldata.skillID==skill) or (spell and skilldata.spell==spell) then return skilldata end
	end
	return self.skills[""]
end


function ZGV:CacheRecipes(profs)
	-- assume tradeskill window is open?
	local skill = GetTradeSkillLine()
	if skill=="UNKNOWN" then return end

	local profID;

	for i,prof in pairs(profs) do
		local name,icon,rank,maxrank,numspells,spelloffset,skillline = GetProfessionInfo(prof)
		if skill == name then profID = skillline end
	end
	-- Runeforging does not have a profID so return.
	if not profID then return end

	-- ah fuck this
	--[[
	-- clear filters
	if TradeSkillFrameAvailableFilterCheckButton:GetChecked() then
		TradeSkillOnlyShowMakeable(false)
		TradeSkillFrameAvailableFilterCheckButton:SetChecked(false)
	end
	--UIDropDownMenu_Initialize(TradeSkillInvSlotDropDown, TradeSkillInvSlotDropDown_Initialize)
	UIDropDownMenu_SetSelectedID(TradeSkillInvSlotDropDown,1)
	SetTradeSkillInvSlotFilter(0,1,1)
	--UIDropDownMenu_Initialize(TradeSkillSubClassDropDown, TradeSkillSubClassDropDown_Initialize)
	UIDropDownMenu_SetSelectedID(TradeSkillSubClassDropDown,1)
	SetTradeSkillSubClassFilter(0,1,1)

	--expand headers
	local openedheaders={}
	for i=GetNumTradeSkills(),1,-1 do
		local name,ttype,_,expanded = GetTradeSkillInfo(i)
		if ttype=="header" and not expanded then
			ExpandTradeSkillSubClass(i)
			openedheaders[name]=true
		end
	end
	--]]

	if IsTradeSkillLinked() then return end
	-- scan!

	local recipes = self.db.char.RecipesKnown --used in Goal.lua and Options.lua

	-- make sure it's the new format
	if not self.db.char.RecipeWipe01142013 or (recipes and type(recipes[next(recipes)])~="table") then wipe(recipes) self.db.char.RecipeWipe01142013 = true end
	if not recipes[profID] then recipes[profID] = {} end
	recipes = recipes[profID]

	wipe(recipes)

	local scanned=0
	for i = 1,500 do
		local tradeName,tradeType = GetTradeSkillInfo(i)
		local rank,maxrank = select(9,GetTradeSkillInfo(i))

		if tradeName and tradeType~="header" and tradeType~="subheader" then
			local link = GetTradeSkillRecipeLink(i)
			if link then
				local spell = strmatch(link,"|H%w+:(%d+)")
				recipes[tonumber(spell)]=true
				scanned=scanned+1
			end
		elseif tradeName and tradeType=="subheader" then --Cooking Masteries
			for UsName,id in pairs(CookingSkills) do
				local name = GetSpellInfo(id) --local name

				if tradeName == name then
					self.db.char.cookingMasteries[id] = rank

					local pro = self.skills[name]
					if not pro then
						pro={} self.skills[name]=pro
					end
					pro.level = rank pro.max = maxrank pro.active = true
					pro.name = name --localized.. Does it matter?
					pro.spell = id pro.skillID = id --this Id is not actually what we need. But we can use it to match properly.

					self:Debug(tradeName.." has level "..rank)
					break
				end
			end
		end
	end
	self:Debug(scanned.." "..skill.." recipes found")

	--[[
	--collapse headers
	for i=GetNumTradeSkills(),1,-1 do
		local name = GetTradeSkillInfo(i)
		if openedheaders[name] then CollapseTradeSkillSubClass(i) end
	end
	--]]
end

function ZGV:DelayPerformTradeSkillGoal(step,goal)
	self.Delayed_PerformTradeSkill_step=step
	self.Delayed_PerformTradeSkill_goal=goal
end

function ZGV:PerformTradeSkillGoal(step,goal)
	if not step or not goal or type(step)~="number" or type(goal)~="number" or not GetTradeSkillLine() then return end
	step = ZGV.CurrentGuide.steps[step]   if not step then return end
	goal = step.goals[goal]   if not goal then return end
	if goal.skillnum then
		-- skillup-based
		self:PerformTradeSkill(goal.spellid,goal.skillnum)
	elseif goal.targetid then
		self:PerformTradeSkill(goal.spellid,goal.count-GetItemCount(goal.targetid))
	end
end

function ZGV:PerformTradeSkill(id,count)
	if not count then count=1 end
	if count<=0 then return end

	local skillNum = self:FindTradeSkillNum(id)

	if skillNum then
		DoTradeSkill(skillNum,count)
	end
end

function ZGV:FindTradeSkillNum(id)
	if not id then return end
	for i = 1,500 do
		local tradeName,tradeType = GetTradeSkillInfo(i)

		if tradeName and tradeType~="header" then
			local link = GetTradeSkillRecipeLink(i)
			if link then
				local spell = tonumber(strmatch(link,"|H%w+:(%d+)"))
				if spell==id then
					return i
				end
			end
		end
	end
end