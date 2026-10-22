local ZGV = ZygorGuidesViewer
if not ZGV then return end

local standingnames={}
local standingnamesrev={}
local standingnameseng={"Hated","Hostile","Unfriendly","Neutral","Friendly","Honored","Revered","Exalted"}
local standingnamesengrev={}
--local tillerrepnameseng={["Stranger"]=5, ["Acquaintance"]=6, ["Buddy"]=106, ["Friend"]=7, ["Good Friend"]=107, ["Best Friend"]=8 } --Yes Aquaintance and Buddy are both 6 atm.. Blizzard....
local tillerrepnameseng={["Stranger"]=0, ["Acquaintance"]=1*8400, ["Buddy"]=2*8400, ["Friend"]=3*8400, ["GoodFriend"]=4*8400, ["BestFriend"]=5*8400 } --Yes Aquaintance and Buddy are both 6 atm.. Blizzard....
local tillerrepnames={}
ZGV.StandingNamesEngRev = standingnamesengrev
ZGV.StandingNamesEng = standingnameseng
ZGV.StandingNames = standingnames
ZGV.FriendshipNamesEngRev = {}
local standingcolors = {"aa0000","ff0000","ff8800","ffff00","00ff00","00ff88","00ffff","cc88ff",[105]="00ff44",[106]="00ffcc"}
local tillercolors = {[0]="ffff00",[1*8400]="00ff00",[2*8400]="00ff88",[3*8400]="00ffff",[4*8400]="44ddff",[5*8400]="cc88ff"}
for k,v in pairs(tillercolors) do standingcolors[k]=v end  tillercolors=nil

local LF=ZygorGuidesViewer_L("Faction")

local RepProto = {}

tinsert(ZGV.startups,function(self)
	self:AddEvent("PLAYER_ENTERING_WORLD","UPDATE_FACTION_Faction")
	self:AddEvent("UPDATE_FACTION","UPDATE_FACTION_Faction")
	--self:AddEvent("CHAT_MSG_COMBAT_FACTION_CHANGE","CHAT_MSG_COMBAT_FACTION_CHANGE_Faction")

	local sex = UnitSex("player")==3 and "_FEMALE" or ""
	for i=1,8 do standingnames[i]=_G['FACTION_STANDING_LABEL'..i..sex] end
	for i,v in ipairs(standingnames) do standingnamesrev[v]=i end
	for i,v in ipairs(standingnameseng) do standingnamesengrev[v]=i end
	
	for i,v in pairs(tillerrepnameseng) do tillerrepnames[v]=i end --Add the Tiller rep names in.
	for name,thresh in pairs(tillerrepnameseng) do ZGV.FriendshipNamesEngRev[name]=thresh end --Add the Tiller rep names in.
	--tillerrepnames[4]=tillerrepnames[5] --Stranger

	self.reputations[""]={
		standing=4,min=0,max=0,val=0,
		progress=0
	}
	for k,v in pairs(RepProto) do self.reputations[""][k]=v end
end)

--[[
NOTE:

local name,desc,standing,min,max,val,_,_,header,_,headerhasrep,_,_, _,_,id = GetFactionInfoByID(1281)
name="Gina"
desc=""
standing=6
min=9000
max=21000
val=13860

local friendID, friendRep, friendMaxRep, friendText, friendTexture, friendTextLevel, friendThresh = GetFriendshipReputation(1281)
friendRep=13860 ==val?
friendMaxRep=42999
friendThresh=8400

barValue = friendRep - friendThresh   == 11880-8400
barMax = min( friendMaxRep - friendThresh, 8400)   == 42999-8400 .. 8400   (so, 8400 or less)


3480/8400



*** At tillers rep change, there's:  -7410 (-88.2%) - 10% to Buddy

*** friendThresh = solution? humm?

--]]

ZGV.factions_mentioned = {}

local defaultReps = {
	["The Mag'har"]=3, --unfriendly
	["Kurenai"]=3, --unfriendly
	["Ella"]=5,
	["Chee Chee"]=5,
	["Gina Mudclaw"]=5,
	["Jogu the Drunk"]=5,
	["Sho"]=5,
}

local function GetNewRep(name)
	local rep = {
		standing=defaultReps[name] or 4,min=0,max=0,val=0,friendRep=0,
		progress=0,
	}
	for k,v in pairs(RepProto) do rep[k]=v end
	return rep
end


function ZGV:GetFakeRep(name,standing,min,max,val)
	local rep = GetNewRep(name)
	rep.standing = standing or rep.standing
	if min then rep.min=min end
	if max then rep.max=max end
	if val then rep.val=val end
	return rep
end

function ZGV:CacheRep(name,standing,min,max,val,isFriend,friendThresh)
	local oldval,oldmin,oldprog
	local rep = self.reputations[name]
	if rep then
		if rep.val~=val then
			oldval=rep.val
			oldmin=rep.min
			oldprog=rep.progress
		end
	else
		rep = GetNewRep(name)
		self.reputations[name]=rep
	end
	if max==min then max=min+1 end
	local progress = (val-min)/(max-min)
	rep.standing = standing
	rep.min=min
	rep.max=max
	rep.val=val
	rep.progress=progress
	rep.isFriend = isFriend
	rep.friendship = friendThresh
	if oldval and self.db.profile.analyzereps then
		print(("|cffbbbbff%s|r: %s%d (%.1f%%) - %s"):format(
			name,
			val-oldval>=0 and "+" or "", val-oldval,
			oldmin==min and (progress-oldprog)*100 or progress*100,
			rep:Going(true)
			))
	end
	return rep
end

function ZGV:CacheReputations()
	for i=1,200 do
		local name,desc,standing,bmin,bmax,val,_,_,header,_,headerhasrep,_,_, id , hasBonusRepGain, canBeLFGBonus = GetFactionInfo(i)
		if name and bmin and (not header or headerhasrep) then
			local isFriend, friendRep, friendMaxRep, friendName, friendText, friendTexture, friendTextLevel, friendThresh, nextFriendThreshold = GetFriendshipReputation(id)
			--FriendName and nextFriendThreshold are new with 5.1
			if isFriend then
				val = friendRep - friendThresh
				bmax = min( friendMaxRep - friendThresh, 8400)
				bmin = 0
			end
			local rep = self:CacheRep(name,standing,bmin,bmax,val,isFriend and true,friendThresh)
			rep.id = id
			rep.hasBonus = hasBonusRepGain
			rep.friendRep = friendRep or 0
		end
	end
end

function ZGV:GetReputation(name)
	if type(name)=="number" then
		for k,v in pairs(self.reputations) do if v.id==name then name=k break end end
	end
	ZGV.factions_mentioned[name] = name
	if self.BFL[name] then name=self.BFL[name] end
	return self.db.profile.fakereps[name] and self:GetFakeRep(name,self.db.profile.fakereps[name])
		or self.reputations[name]
		or GetNewRep(name)
end






---
-- @return
function RepProto:CalcTo(standing)
	if type(standing)=="string" then standing=standingnamesengrev[standing] if not standing then return end end
	if standing-self.standing>1 then
		return nil,nil
	elseif standing<=self.standing then
		return 0
	else
		return self.max-self.val
	end
end

local function bar(cur,max,len)
	local a=floor(cur/max*len)
	return "|cffffff00[|cff00ff00".. strrep("||",a) .. "|cff666666" .. strrep("||",len-a) .. "|cffffff00]|r"
end

function RepProto:Current()
	return self.isFriend and tillerrepnames[self.friendship] or standingnames[self.standing]
end

function RepProto:Next()
	return self.isFriend and tillerrepnames[self:GetNextStanding()] or standingnames[self:GetNextStanding()]
end

function RepProto:Going(color)
	if color then
		if self.standing==8 then return self:GetFormattedStanding(8) else return LF["going"]:format(self.progress*100,self:GetFormattedStanding(self:GetNextStanding())) .. ": "..bar(self.progress*100,100,20) end
	else
		if self.standing==8 then return self:Current() else return LF["going"]:format(self.progress*100,self:Next()) end
	end
end

function RepProto:GetNextStanding()
	if self.isFriend then  return min(self.friendship+8400,7*8400)  end
	return self.standing+1
end

function RepProto:EqualOrAbove(standing)
	if type(standing)=="string" then standing=standingnamesengrev[standing] if not standing then return end end
	return self.standing>=standing
end

function RepProto:Below(standing)
	if type(standing)=="string" then standing=standingnamesengrev[standing] if not standing then return end end
	return self.standing<standing
end

function RepProto:GetFormattedStanding(standing)
	if not standing then standing=self.isFriend and self.friendship or self.standing end
	return ("|cff%s%s|r"):format(standingcolors[standing],self.isFriend and tillerrepnames[standing] or standingnames[standing])
end

function ZGV:UPDATE_FACTION_Faction()
	self:CacheReputations()
	self:UpdateFrame(true)
end

local messages={FACTION_STANDING_DECREASED,FACTION_STANDING_DECREASED_GENERIC,FACTION_STANDING_INCREASED,FACTION_STANDING_INCREASED_BONUS,FACTION_STANDING_INCREASED_GENERIC}
for i,msg in ipairs(messages) do messages[i]=msg:gsub("%%[.%d]?[sdf]","(.*)") end

function ZGV:CHAT_MSG_COMBAT_FACTION_CHANGE_Faction(event,text)
	--print("Faction!",text)
	for i,msg in ipairs(messages) do
		local faction = text:match(msg)
		if faction then
			local rep = self:GetReputation(faction)
			print(("%s: %s (%s)"):format(faction,rep:Current(),rep:Going()))
			return
		end
	end
end
