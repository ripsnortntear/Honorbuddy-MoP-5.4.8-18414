local addonName, vars = ...
local L = vars.L
IdiotCheck = LibStub("AceAddon-3.0"):NewAddon(addonName)
local addon = IdiotCheck 
vars.svnrev["core.lua"] = tonumber(("$Revision: 258 $"):match("%d+"))

local pairs, ipairs, string, wipe, select, tonumber, tostring, type, table, bit =
      pairs, ipairs, string, wipe, select, tonumber, tostring, type, table, bit
local InCombatLockdown, IsInInstance, GetTime, UnitName, UnitLevel, UnitExists, GetSpellInfo, GetItemInfo, UnitIsUnit, UnitBuff, UnitClass, UnitIsDead, GetInventoryItemLink, GetInventoryItemID, GetSpecialization, GetUnitName, UnitGUID = 
      InCombatLockdown, IsInInstance, GetTime, UnitName, UnitLevel, UnitExists, GetSpellInfo, GetItemInfo, UnitIsUnit, UnitBuff, UnitClass, UnitIsDead, GetInventoryItemLink, GetInventoryItemID, GetSpecialization, GetUnitName, UnitGUID

local defaults = {
  profile = {
    general = {
      debug = false, -- for addon debugging
      sound = true,
      soundrandom = true,
      soundname = "Troxed 1",
      chatalert = true,
      alwayschat = false,
      readycheck = true,
      targetboss = true,
      newinstance = true,
      maxlevel = true,
      bgcheck = false,
      autochecktime = 5,
      inspectcheck = true,
      reportchannel = 1,
      inspectstealth = false,
      inspectstealthgroup = true,
      bbintegration = true,
      riintegration = true,
      ttintegration = true,
      readyscan = false,
    },
    minimap = {
        hide = false,
    },
    checks = {
      gear = true,
      stat = true,
      buff = true,

      instanceonly = true,
      selfonly = false,
      buffduration = 5,
      
      dkhorn = false,
      shamanshield = false, 
      warriorshout = false,
    },
    thresh = {
      ilvl = 450,
      durability = 0.10,
      shamanimbue = 5,
      magemanagem = 2,
      magewater = 4,
      warlocksoulshards = 1,
      resilience = 300,
      spellpen = 20,
      spellhit = 0.005,
      spellhitover = 0.01,
      meleehit = 0.005,
      meleehitover = 0.01,
      rangedhit = 0.005,
      rangedhitover = 0.01,
      expertise = 0.005,
      expertiseover = 0.01,
    },
  }
}

local settings = defaults.profile
local optionsFrame, profileFrame
local check_class, check_inspect, check_manual, check_unit, check_ilvl
addon.scantt = CreateFrame("GameTooltip", addonName.."_ScanTooltip", UIParent, "GameTooltipTemplate")
addon.scantt:SetOwner(UIParent, "ANCHOR_NONE");
local revision = tonumber(("$Revision: 258 $"):match("%d+"))
local minimapIcon = LibStub("LibDBIcon-1.0")
local LGT
local LDB
local MAXLEVEL = MAX_PLAYER_LEVEL_TABLE[#MAX_PLAYER_LEVEL_TABLE]
local LCLASS = LOCALIZED_CLASS_NAMES_MALE

local function chatMsg(msg)
     local oframe = DEFAULT_CHAT_FRAME
     local cw = settings.general.chatwindow
     if cw and _G[cw] and _G[cw].AddMessage then
        oframe = cw
     elseif cw then
        for i=1,10 do
	  local c = _G["ChatFrame"..i]
	  if c and c.name == cw and c.AddMessage then
	    oframe = c
	  end
	end
     end
     oframe:AddMessage("\124cff9999ff"..addonName.."\124r: "..msg)
end
local function chatAlert(msg)
   if settings.general.chatalert then
     chatMsg(msg)
   end
end
local function debug(msg)
  if addon.db.profile.general.debug then
     chatMsg(msg)
  end
end

addon.soundfiles = {
  { "Ermey", 9 },
  { "Faelagund", 5 },
  { "Onyxia", 2 },
  { "Letomi", 3 },
  { "Sinnerman", 1 },
  { "Carrey", 1 },
  { "Troxed", 11 },
  "Sound/creature/Illidan/BLACK_Illidan_04.ogg",
  "Sound/Creature/XT002Deconstructor/UR_XT002_Special01.wav",
  "Sound/Creature/Moroes/MoroesSlay03.ogg",
}
addon.soundtable = {} -- name=>filename and index=>filename
addon.soundok = "Interface\\AddOns\\"..addonName.."\\sound\\tada.mp3"
for _,v in pairs(addon.soundfiles) do
 if type(v) == "table" then
   local name = v[1]
   for i = 1,v[2] do
     local filename = "Interface\\AddOns\\"..addonName.."\\sound\\"..name:gsub(" ","_").."_"..i..".mp3"
     addon.soundtable[name.." "..i] = filename
     table.insert(addon.soundtable, filename)
   end
 else
   local name = v:gsub("^.+/([^/]+)$","%1"):gsub("%....$",""):gsub("_"," ")
   local filename = v
   addon.soundtable[name] = filename
   table.insert(addon.soundtable, filename)
 end
end

addon.reportchannels = {
  [1] = { name = CHAT_MSG_WHISPER_INFORM, 
          send = function(str) SendChatMessage(str, "WHISPER", nil, addon.reportframe.unitname:gsub("%s","")) end },
  [2] = { name = CHAT_MSG_SAY,     send = function(str) SendChatMessage(str, "SAY") end },
  [3] = { name = CHAT_MSG_INSTANCE_CHAT,
                                   send = function(str) SendChatMessage(str, "INSTANCE_CHAT") end },
  [4] = { name = CHAT_MSG_PARTY,   send = function(str) SendChatMessage(str, "PARTY") end },
  [5] = { name = CHAT_MSG_RAID,    send = function(str) SendChatMessage(str, "RAID") end },
  [6] = { name = CHAT_MSG_GUILD,   send = function(str) SendChatMessage(str, "GUILD") end },
  [7] = { name = CHAT_MSG_OFFICER, send = function(str) SendChatMessage(str, "OFFICER") end },
  [8] = { name = L["Self"], send = chatMsg },
  [9] = { name = L["Self"].." "..CHAT_MSG_WHISPER_INFORM, 
          send = function(str) SendChatMessage(str, "WHISPER", nil, UnitName("player")) end },
}

function addon:hash(str) -- a dumbed-down crc32
  str = tostring(str)
  local count = string.len(str)
  local val = tonumber(count)
  for i = 1,count do
    local byte = tonumber(string.byte(str,i))
    val = bit.bxor(bit.lshift(val,8), bit.lshift(bit.bxor(bit.rshift(val,24), byte),i%3))
  end
  return tonumber(val)
end

function addon:myOptions() 
return {
  type = "group",
  set = function(info,val)
          local s = settings ; for i = 1,#info-1 do s = s[info[i]] end
          s[info[#info]] = val; debug(info[#info].." set to: "..tostring(val))
          addon:Update()
        end,
  get = function(info)
          local s = settings ; for i = 1,#info-1 do s = s[info[i]] end
          return s[info[#info]] end,
  args = {
   general = {
    type = "group",
    inline = true,
    name = L["General"],
    args = {
      debug = {
        name = L["Debug"],
        desc = L["Toggle debugging output"],
        type = "toggle",
        guiHidden = true,
      },
      config = {
        name = L["Config"],
        desc = L["Open the configuration GUI"],
        type = "execute",
        guiHidden = true,
        func = function() addon:Config() end,
      },
      check = {
        name = L["Check"],
        desc = L["Perform a check now"],
        type = "execute",
        guiHidden = true,
        func = function() addon:RunCheck("player",true) end,
      },
      minimap = {
        order = 15,
        name = L["Minimap Icon"],
        desc = L["Display minimap icon"],
        type = "toggle",
        set = function(info,val)
          settings.minimap.hide = not val
          addon:Update()
	end,
        get = function() return not settings.minimap.hide end,
      },
      checkheader = {
        order = 16,
	name = L["Automated Checks"].." ("..L["Self"]..")",
	type = "header",
      },
      newinstance = {
        order = 16.5,
        name = L["New Instance"],
        desc = L["Check when entering an instance"],
        type = "toggle",
      },
      readycheck = {
        order = 17,
        name = L["Ready Check"],
        desc = L["Check during ready checks"],
        type = "toggle",
      },
      targetboss = {
        order = 18,
        name = L["Boss Target"],
        desc = L["Check when you first target a boss"],
        type = "toggle",
      },
      autochecktime = {
        order = 19,
        name = L["Auto-check interval"],
        desc = L["Minimum number of minutes between automatic checks"],
        type = "range",
        step = 0.5,
        min = 0,
        max = 60,
      },
      bgcheck = {
        order = 19.3,
        name = L["Battleground"],
        desc = L["Perform automatic checks in battlegrounds"],
        type = "toggle",
      },
      maxlevel = {
        order = 19.5,
        name = L["Max Level"],
        desc = L["Only perform automatic checks on max level characters"],
        type = "toggle",
      },
      otherheader = {
        order = 20,
	name = L["Automated Checks"].." ("..OTHER..")",
	type = "header",
      },
      inspectcheck = {
        order = 21,
        name = L["Inspect Window"],
        desc = L["Perform automatic checks on inspection"],
        type = "toggle",
      },
      inspectstealth = {
        order = 22,
        name = L["Stealth Inspects"],
        desc = L["Perform automatic checks on automated inspection by other addons"],
        type = "toggle",
      },
      inspectstealthgroup = {
        order = 22.1,
        name = L["Group only"],
        desc = L["Only stealth check group members"],
	disabled = function() return not settings.general.inspectstealth end,
        type = "toggle",
      },
      readyscan = {
        order = 23,
        name = L["Ready Check Scan"],
        desc = L["Scan the group on ready check"],
        type = "toggle",
      },
      bbintegration = {
        order = 23.5,
        name = L["BigBrother Integration"],
        desc = L["Add group scan results to the BigBrother buff window"],
	disabled = function() return not (BigBrother and BigBrother.unitstatus) end,
        type = "toggle",
      },
      riintegration = {
        order = 23.7,
        name = L["RoleIcons Integration"],
        desc = L["Add group scan results to the Blizzard raid tab (requires RoleIcons)"],
	disabled = function() return not (RoleIcons and RoleIcons.unitstatus) end,
        type = "toggle",
      },
      ttintegration = {
        order = 23.9,
        name = L["Tooltip Integration"],
        desc = L["Add scan results to unit tooltips"],
        type = "toggle",
      },
      alertheader = {
        order = 24.9,
	name = L["Alerts"],
	type = "header",
      },
      sound = {
        order = 25,
        name = L["Alert Sound"],
        desc = L["Play an alert sound when a fail is detected"],
        type = "toggle",
      },
      soundrandom = {
        order = 27,
        name = L["Random Sound"],
        desc = L["Use a randomly-chosen sound"],
	disabled = function() return not settings.general.sound end,
        type = "toggle",
      },
      soundname = {
        order = 29,
        name = L["Sound"],
        desc = L["Sound to play when a fail is detected"],
	disabled = function() return not settings.general.sound or settings.general.soundrandom end,
        type = "select",
	style = "dropdown",
	values = function() 
	  local t = {}
	  for n,_ in pairs(addon.soundtable) do
	    if not tonumber(n) then
	      t[n] = n
	    end
	  end
	  return t
	end,
	set = function(info,val) 
	  settings.general.soundname = val
	  if val ~= random then
	    PlaySoundFile(addon.soundtable[val])
	  end
	end,
      },
      chatalert = {
        order = 30,
        name = L["Chat Alert"],
        desc = L["Print an alert in chat when a fail is detected"],
        type = "toggle",
      },
      alwayschat = {
        order = 35,
        name = L["Always Chat"],
        desc = L["Print to chat whenever a check is performed"],
        type = "toggle",
      },
      chatwindow = {
        order = 37,
        name = L["Chat window"],
        desc = L["Chat window to use for addon output"],
        type = "select",
	style = "dropdown",
	values = function() 
	  local t = {}
          for i=1,10 do
            local c = _G["ChatFrame"..i]
            if c and c.name and c.AddMessage then
              t[c.name] = c.name
            end
          end
	  return t
	end,
      },
    },
   },
   gearchecks = {
	name = L["Gear Checks"],
	type = "group",
	order = 1,
	args = addon:getCheckOptions("gear", L["Gear Checks"]),
   },
   statchecks = {
	name = L["Stat Checks"],
	type = "group",
	order = 2,
	args = addon:getCheckOptions("stat", L["Stat Checks"]),
   },
   buffchecks = {
	name = L["Buff Checks"],
	type = "group",
	order = 3,
	args = addon:getCheckOptions("buff", L["Buff Checks"]),
   },
  }
} 
end

function addon:getCheckOptions(checktype, groupname)
  local ret = {}
  ret[checktype] = {
      order = 0,
      name = groupname,
      type = "toggle",
      width = (checktype == "buff" and "single") or "full",
  }
  -- special cases
  if checktype == "buff" then
    ret.instanceonly = {
      order = 0.1,
      name = L["Instance Only"],
      desc = L["Only check buffs inside instances"],
      type = "toggle",
      disabled = function() return not settings.checks[checktype] end,
    }
    ret.selfonly = {
      order = 0.2,
      name = L["Self Only"],
      desc = L["Only check buffs on self"],
      type = "toggle",
      disabled = function() return not settings.checks[checktype] end,
    }
    ret.buffduration = {
      order = 0.5,
      name = L["Buff Duration"],
      desc = L["Minimum time remaining in minutes to consider the buff valid"],
      type = "range",
      min = 0,
      max = 15,
      step = 1,
      disabled = function() return not settings.checks[checktype] end,
    }
  end
  for o,v in pairs(addon.checks_ordered) do
   local ct = v.checktype or "gear"
   if (ct == checktype) then
    local desc = v.desc or v.failname
    --if v.playeronly then desc = desc.." "..L["(player only)"]
    if v.header then
    ret[v.checkname] = {
      order = o,
      name = v.header,
      type = "header",
    }
    else
    ret[v.checkname] = {
      order = o,
      name = v.failname,
      desc = desc,
      type = "toggle",
      disabled = function() return not settings.checks[checktype] end,
    }
    if v.threshdesc then
     if ct ~= "buff" then
      ret[v.checkname].order = o*10 + 10002
      ret[v.checkname.."_header"] = {
        order = o*10 + 10001,
	type = "header",
	name = "",
	cmdHidden = true,
      }
     end
      ret[v.checkname.."_thresh"] = {
        order = ret[v.checkname].order+0.5,
	name = v.threshname or v.failname.." "..L["threshold"],
	desc = v.threshdesc,
	step = v.threshstep,
	isPercent = v.threshpercent,
	disabled = function() return not settings.checks[checktype] or not settings.checks[v.checkname] end,
        type = "range",
	min = v.threshmin,
	max = v.threshmax,
        get = function() return settings.thresh[v.checkname] end,
        set = function(info,val)
	        settings.thresh[v.checkname] = val
                addon:Update()
              end
      }
    end -- thesh
    end -- header
   end -- checktype
  end -- for
  return ret
end

local function table_clone(t)
  if not t then return nil
  elseif type(t) == "table" then
    local res = {}
    for k,v in pairs(t) do
      res[table_clone(k)] = table_clone(v)
    end
    return res
  else
    return t
  end
end

local function order_table(t) -- sort and compact a table by order field
    local tmp = {}
    local index = {}
    for k,v in pairs(t) do
       v.checkname = k 
       if v.order then
         if tmp[v.order] then
           debug("ERROR: duplicate table order value: "..v.order)
         else
           tmp[v.order] = v
         end
	 table.insert(index, v.order)
       end
    end
    local max = table.maxn(tmp)
    for k,v in pairs(t) do
       if not v.order then
          max = max + 1
          table.insert(retval, max, v)
          v.order = max
	  table.insert(index, v.order)
       end
    end
    table.sort(index)
    local retval = {}
    for i,o in ipairs(index) do
      retval[i] = tmp[o]
      retval[i].order = i
    end
    return retval
end

-- -------------------------------------------------------------------------------------------------
-- Item queries and helper functions
local FPsubclass
function addon:isFishingPole(itemid)
  if not FPsubclass then
    FPsubclass = select(7, GetItemInfo(6256))
  end
  return select(7, GetItemInfo(itemid)) == FPsubclass
end

local OHsubclass
function addon:isHeldInOffhand(itemid)
  if not OHsubclass then
    OHsubclass = select(7, GetItemInfo(43656))
  end
  local subclass,_,equipslot = select(7, GetItemInfo(itemid))
  return subclass == OHsubclass or equipslot == "INVTYPE_HOLDABLE" -- second clause for load-time latency
end
  
local SHsubclass
function addon:isShield(itemid)
  if not SHsubclass then
    SHsubclass = select(7, GetItemInfo(82847))
  end
  local subclass,_,equipslot = select(7, GetItemInfo(itemid))
  return subclass == SHsubclass or equipslot == "INVTYPE_SHIELD" -- second clause for load-time latency
end
  
local MGsubclass
function addon:isMetaGem(itemid)
  if not MGsubclass then
    MGsubclass = select(7, GetItemInfo(41401))
  end
  return select(7, GetItemInfo(itemid)) == MGsubclass
end
 
local TwoHandsubclass = nil
function addon:is2HWeapon(itemid)
  if not itemid then return false end
  if not TwoHandsubclass then
      TwoHandsubclass = {}
      for _,id in pairs({2491, 2489, 2495, 2493, 6367, 44654, 44643, 44637, 44635}) do
        local subclass = select(7, GetItemInfo(id))
        if subclass and TwoHandsubclass then -- might fail on first call due to lag
          TwoHandsubclass[subclass] = true
        else
          TwoHandsubclass = nil
        end
      end
  end
  local subclass = select(7, GetItemInfo(itemid))
  if TwoHandsubclass then
     return TwoHandsubclass[subclass]
  else
     return true -- conservative don't know
  end
end

local _teleportItems = {
  32757, -- BT neck

  63206, 63352, 65360, -- guild cloaks of teleport

  40585, 40586, 44934, 44935,  -- kirin-tor dalaran rings
  45688, 45689, 45690, 45691,
  48954, 48955, 48956, 48957,
  51557, 51558, 51560, 51559,
}
local teleportItems = {}
for _,i in pairs(_teleportItems) do teleportItems[i] = true end

local armorSubClass = {
  CLOTH = 3602,
  LEATHER = 2122,
  MAIL = 2387,
  PLATE = 62254,
  first = true,
}
local classArmorType = {
  MAGE = "CLOTH",
  WARLOCK = "CLOTH",
  PRIEST = "CLOTH",
  DRUID = "LEATHER",
  ROGUE = "LEATHER",
  MONK = "LEATHER",
  SHAMAN = "MAIL",
  HUNTER = "MAIL",
  PALADIN = "PLATE",
  WARRIOR = "PLATE",
  DEATHKNIGHT = "PLATE",
}
function addon:armorType(itemid)
  if armorSubClass.first then
    local init = true
    for t,n in pairs(armorSubClass) do
      if type(n) == "number" then
        local subclass = select(7, GetItemInfo(n))
	if subclass then 
	  armorSubClass[subclass] = t
	else
	  init = false
	end
      end
    end
    if init then armorSubClass.first = nil end
  end
  local subclass = select(7, GetItemInfo(itemid))
  return armorSubClass[subclass]
end

local tankclasses = { PALADIN = true, WARRIOR = true, DEATHKNIGHT = true, DRUID = true }
local function tankClass(class)
  return tankclasses[class]
end

local buffids = {
  fort = {
        469, -- Commanding shout (must be first)
        21562, -- Prayer of Fortitude
	109773, -- Warlock Dark Intent
	90364, -- Qiraji Fortitude
  },
  stats = {
        20217, -- Blessing of Kings (must be first)
        1126, -- Mark of the Wild
	117666, -- Legacy of the Emperor
        90363, -- Embrace of the Shale Spider
  },
  mastery = {
        19740, -- Blessing of Might (must be first)
	116956, -- Grace of Air
        93435,  -- Roar of Courage
	128997, -- Spirit Beast Blessing
  },
  ap = {
   	6673, -- Battle Shout (must be first)
   	57330, -- Horn of Winter
	19506, -- Trueshot Aura
  },
  sp = {
  	1459,   -- Mage Arcane Brilliance
	61316,  -- Mage Dalaran Brilliance
	109773, -- Warlock Dark Intent
	77747,  -- Shaman Burning Wrath
	126309, -- Still Water
  },
  crit = {
  	116781, -- Legacy of the White Tiger
	17007,  -- Leader of the Pack
	1459,   -- Arcane Brilliance
	61316,  -- Dalaran Brilliance
	24604,  -- Furious Howl
	90309,  -- Terrifying Roar
	97229,  -- Bellowing Roar
	126373, -- Fearless Roar
	126309, -- Still Water
  },
  poison = {
    2818,  -- Deadly
    3409,  -- Crippling
    8679,  -- Wound 
    5760,  -- Mind-numbing
  }
}
local function hasBuff(unitid, buffs)
  for _,id in pairs(buffs) do
    local n = GetSpellInfo(id)
    if not n then
      chatMsg("bad spellid: "..id)
      return false
    end
    local name, _,_,_,_,duration,expires, caster, _,_, spellid = UnitBuff(unitid, n)
    -- if spellid == id then  -- too restrictive
    if name == n then -- allow aliases
       if duration and duration > 10*60 then -- check time on buffs over 5 min long
         if expires - GetTime() < settings.checks.buffduration*60 then
	   addon.lowduration = true
	   return false
	 end
       end
       if not caster then
         caster = unitid
       end
       return caster, name
    end
  end
  return nil
end
local gmhbm_tmp = {}
local function groupMemberHasMyBuff(unitid, spellid, checkcaster)
  local prefix
  gmhbm_tmp[1] = spellid
  if IsInRaid() then
    prefix = "raid"
  elseif GetNumGroupMembers() > 0 then
    prefix = "party"
  else
    return true -- not in group
  end
  for j=1,2 do
   for i=0,GetNumGroupMembers() do
    local unitnm = (i == 0 and "player") or prefix..i
    if j == 2 then unitnm = unitnm.."pet" end
    local caster = UnitExists(unitnm) and (checkcaster or not UnitIsUnit(unitnm, unitid)) and hasBuff(unitnm, gmhbm_tmp)
    if caster and UnitIsUnit(caster, unitid) then 
       return true
    end
   end
  end
  return false
end
local function buffHeader(Order, Name)
 return {
    order = Order,
    checktype = "buff",
    header = Name,
 }
end
local function buffCheck(Order, Failname, Class, buffs)
 return {
    order = Order,
    failname = Failname,
    checktype = "buff",
    class = Class,
    aliveonly = true,
    playercheck = function(unitid)
      return hasBuff(unitid, buffs)
    end,
  }
end
local function buffCheckTwo(Order, Failname, Class, buff1, buff2, requireboth)
 return {
    order = Order,
    failname = Failname,
    checktype = "buff",
    class = Class,
    aliveonly = true,
    playercheck = function(unitid)
      local c1 = hasBuff(unitid, buff1)
      local c2 = hasBuff(unitid, buff2)
      if c1 and c2 then -- have both
        return true
      elseif requireboth then
        return false
      elseif UnitIsUnit(c1 or "none", unitid) or UnitIsUnit(c2 or "none", unitid) then -- already buffed one
        return true
      elseif not c1 then
        return false, tostring(GetSpellInfo(buff1[1]))
      elseif not c2 then
        return false, tostring(GetSpellInfo(buff2[1]))
      end 
    end,
  }
end

local classRoleToSpec = {
  ["PALADIN"] =     { ["HEALER"] = 1, ["TANK"] = 2, ["DAMAGER"] = 3 },
  ["PRIEST"] =      { ["discipline"] = 1, ["holy"] = 2, ["DAMAGER"] = 3 },
  ["DRUID"] =       { ["balance"] = 1, ["cat"] = 2, ["TANK"] = 3, ["HEALER"] = 4 },
  ["DEATHKNIGHT"] = { ["TANK"] = 1, ["frost"] = 2, ["unholy"] = 3 },
  ["WARRIOR"] =     { ["arms"] = 1, ["fury"] = 2, ["TANK"] = 3 },
  ["SHAMAN"] =      { ["elemental"] = 1, ["enhancement"] = 2, ["HEALER"] = 3 },
  ["MONK"] =        { ["TANK"] = 1, ["HEALER"] = 2, ["DAMAGER"] = 3 },
  ["ROGUE"] =       { ["assassination"] = 1, ["combat"] = 2, ["subtlety"] = 3 },
  ["MAGE"] =        { ["arcane"] = 1, ["fire"] = 2, ["frost"] = 3 },
  ["WARLOCK"] =     { ["affliction"] = 1, ["demonology"] = 2, ["destruction"] = 3 },
  ["HUNTER"] =      { ["beastmaster"] = 1, ["marksman"] = 2, ["survival"] = 3 },
}

-- Fix Blizzards GetSpecialization to behave like it SHOULD for inspect targets
local function GetSpecialization(isInspect)
  if not isInspect or isInspect == 0 then
    return _G.GetSpecialization()
  end
  local unit = check_unit
  local specid = GetInspectSpecialization(unit)
  if not specid or specid == 0 then
    debug("GetSpecialization: got empty GetInspectSpecialization")
    return nil
  end
  local _, Lname, desc, icon, texture, role, class = GetSpecializationInfoByID(specid)
  if not texture or not role then return nil end
  local shorttex = texture:match("-([^-]+)$")
  local ret = classRoleToSpec[class][role]
  ret = ret or (shorttex and classRoleToSpec[class][shorttex])
  ret = ret or classRoleToSpec[class][texture]
  if not ret then
    debug("GetSpecialization failed to decode GetInspectSpecialization")
  end
  return ret
end
addon.GetSpecialization = GetSpecialization

local PID_BS   = 2018
local PID_JC   = 25229
local PID_ENG  = 4036
local PID_ENCH = 7411
local PID_INSC = 45357
local PID_LW   = 2108
local PID_TAIL = 3908

local profenchants = {
  [4115]=1, [4116]=1, [4118]=1, -- Tailoring
  [4892]=1, [4893]=1, [4894]=1, -- Tailoring MoP
  [4078]=1, [4079]=1, [4080]=1, [4081]=1, -- Enchanting
  [4359]=1, [4360]=1, [4361]=1, [4807]=1, -- Enchanting MoP
  [4189]=1, [4190]=1, [4191]=1, [4192]=1, -- Leatherworking
  [4875]=1, [4877]=1, [4878]=1, [4879]=1, -- Leatherworking MoP
  [4193]=1, [4194]=1, [4195]=1, [4196]=1, -- Inscription
  [4915]=1, [4912]=1, [4914]=1, [4913]=1, -- Inscription MoP
  [4179]=1, [4180]=1, [4181]=1, [4182]=1, [4183]=1, -- Engineering Gloves
  [4898]=1, [4697]=1, [4698]=1, -- Engineering Gloves MoP
  [4187]=1, [4188]=1, -- Engineering Belt
  [5000]=1, -- Engineering Belt MoP
}

-- returns rank
function addon:hasProfession(unitid, pid)
  if UnitIsUnit(unitid, "player") then
    local pname = GetSpellInfo(pid)
    for _,profid in pairs({ GetProfessions() }) do
      local pn, _, rank, rankmax = GetProfessionInfo(profid)
      if (pn == pname) then return rank end
    end
    return 0
  else
    if not addon.guildcrafters then 
       GuildRoster() -- populate guild professions data
    end
    local name, server = UnitName(unitid)
    if server and server ~= "" then 
      name = name .. "-" .. server:gsub(" ","")
    else
      name = name .. "-" .. GetRealmName():gsub(" ","")
    end
    return (addon.guildcrafters and addon.guildcrafters[name] and addon.guildcrafters[name][pid]) or 0
  end
end

local badImbues = {
  [1] = { -- ele
            [GetSpellInfo(51730)]=true, -- earthliving
            [GetSpellInfo(8232)]=true,  -- windfury 
            [GetSpellInfo(8033)]=true,  -- frostbrand
            [GetSpellInfo(8017)]=true,  -- rockbiter
  }, 
  [2] = { -- enh
            [GetSpellInfo(51730)]=true, -- earthliving
            [GetSpellInfo(8033)]=true,  -- frostbrand
            [GetSpellInfo(8017)]=true,  -- rockbiter
  }, 
  [3] = { -- resto
            [GetSpellInfo(8232)]=true,  -- windfury 
            [GetSpellInfo(8024)]=true,  -- flametongue 
            [GetSpellInfo(8033)]=true,  -- frostbrand
            [GetSpellInfo(8017)]=true,  -- rockbiter
  }, 
}

-- a combined version of GetInventoryItemID and GetInventoryItemLink that works in all cases
function addon:GetInventoryItem(unitid, slotid)
    local itemid = GetInventoryItemID(unitid, slotid) -- unreliable with transmog inspect
    local itemlink = GetInventoryItemLink(unitid, slotid)
    if not itemlink then -- above two fail for remote inspects, need a tooltip scan >.<
        addon:scanBegin(nil, unitid, slotid)
	itemlink = addon:scanLink()
    end
    if itemlink then itemid = itemlink:match("\124Hitem:(%d+):") end
    itemid = itemid and tonumber(itemid)
    return itemlink, itemid
end

local function checkWeaponImbue(unitid)
      local MH = addon:GetInventoryItem(unitid, INVSLOT_MAINHAND)
      local OH = addon:GetInventoryItem(unitid, INVSLOT_OFFHAND)
      local spec = GetSpecialization(check_inspect)
      local report = ""
      local hasMainHandEnchant, mainHandExpiration, _,
            hasOffHandEnchant, offHandExpiration, _,
	    hasRangedEnchant, rangedExpiration, _ = GetWeaponEnchantInfo()
      if check_class == "SHAMAN" and spec then
         for _,hand in pairs({INVSLOT_MAINHAND, INVSLOT_OFFHAND}) do
           for i=2,addon:scanBegin(nil, unitid, hand) do
             local text = addon:scanText(i)
             for imbue,_ in pairs(badImbues[spec]) do
               if text:find(imbue) then
                 report = report..L["Bad weapon imbue "]..imbue.." "
               end
             end
           end
         end 
         if #report > 0 then
           return false, report
         end
      end

      if MH and (not hasMainHandEnchant or mainHandExpiration < settings.checks.buffduration*60000) then
        report = " "..MH.." ("..addon.itemslots.MainHandSlot.shortname..")"
      end
      if (check_class == "ROGUE" or spec == 2) and
         OH and (not hasOffHandEnchant or offHandExpiration < settings.checks.buffduration*60000) then
        report = report.." "..OH.." ("..addon.itemslots.SecondaryHandSlot.shortname..")"
      end
      return #report == 0, L["Missing weapon imbue on"]..report
end

local function glyphcheck(unitid,checkempty)
  local level = UnitLevel(unitid)
  local spec = GetSpecialization(check_inspect)
  local empty = 0
  local s = ""
  if not vars.glyphlevel or not vars.glyphdata then return true end -- backward compat
  if not spec then return true end -- untalented or missing spec data
  for i = 1, NUM_GLYPH_SLOTS do
    if level and vars.glyphlevel[i] <= level then -- enabled return of GetGlyphSocketInfo is wrong for inspect targets
      local _, glyphType, _, spellid, _, glyphid = 
        GetGlyphSocketInfo(i, nil, check_inspect, unitid);
      if glyphType == GLYPH_TYPE_MAJOR then -- ignore minor glyphs
        if not glyphid then 
	  empty = empty + 1
	else
          local ps = vars.glyphdata[glyphid]
          if ps then
            local fail
	    if type(ps) == "table" then 
	      fail = true
	      for _,pps in pairs(ps) do
	        if spec == pps then 
	           fail = false
	        end
	      end
	    elseif spec ~= ps then
	      fail = true
	    end
	    if fail then
              if #s > 0 then s = s .. ", " end
              s = s .. GetGlyphLinkByID(glyphid) -- GetSpellLink(spellid) is unreliable
	    end
          end
	end -- not empty
      end -- major
    end -- level check
  end -- for glyphs
  if not checkempty and #s > 0 then
     return false, s
  elseif checkempty and empty > 0 then
     return false, empty.." "..L["Empty Glyph Slots"]
  else
     return true
  end
end

-- scan glyph data and dump to populate glyphdata.lua
function addon:scanGlyphs(wipe)
  if wipe then
    addon.db.global.glyphScanDB = nil
    chatMsg("glyphScanDB wiped.")
    return
  end
  local class = UnitClass("player")
  local output = "-- "..class.."  "
  local specnametoidx = {}
  local cnt = 0
  local subtext_exceptions = { -- handle inconsistencies in spec spelling
    ["Marksman"]    = "Marksmanship",
    ["Beastmaster"] = "Beast Mastery",
  }
  for idx=1,GetNumSpecializations() do
    local id, name, description, icon, background, role = GetSpecializationInfo(idx)
    specnametoidx[name] = idx
    output=output..(idx > 1 and ", " or "")..idx.." = "..name
  end
  output=output.."\n"
  local glyphlines = {}
  for i=1,GetNumGlyphs() do
    local name, glyphType, isKnown, icon, glyphid, link, subText = GetGlyphInfo(i)
    if name and name ~= "header" and 
       glyphType == GLYPH_TYPE_MAJOR then -- ignore minor glyphs
       cnt = cnt + 1
       local specstr = "" 
       local comment = "\t-- ["..name.."]"
       if subText and #subText > 0 then
         comment = comment.." ("..subText..")"
         for _,spec in pairs({ strsplit(",", subText) }) do
	   spec = strtrim(spec)
	   spec = subtext_exceptions[spec] or spec
           local idx = specnametoidx[spec]
	   if idx then
	     specstr = specstr..(#specstr > 0 and "," or "")..idx
	   elseif #spec > 0 then
	     chatMsg("ERROR: glyphScan unrecognized subText: '"..spec.."' "..link)
	     return
	   end
	 end
	 if specstr:find(",") then
	   specstr = "{"..specstr.."}"
	 end
       end
       if #specstr > 0 then
         table.insert(glyphlines,"     ["..glyphid.."] = "..specstr..",    "..comment.."\n")
       else
         table.insert(glyphlines,"  -- ["..glyphid.."] = ,    "..comment.."\n")
       end
    end
  end
  table.sort(glyphlines,function(a,b) a=a:match("%[(%d+)%]"); b=b:match("%[(%d+)%]"); return tonumber(a) < tonumber(b) end)
  for _,line in ipairs(glyphlines) do
    output=output..line
  end
  addon.db.global.glyphScanDB = addon.db.global.glyphScanDB or {}
  addon.db.global.glyphScanDB[class] = "\n"..output
  for _,s in pairs({strsplit("\n",output)}) do
    s = s:gsub("\t"," ")
    print(s)
  end
  chatMsg("glyphScanDB scanned "..cnt.." major "..class.." glyphs.")
end

-- Color, Ltype, Id, Enchant, Gem1, Gem2, Gem3, Gem4, Suffix, Unique, LinkLvl, reforging, Name
function addon:decomposeItemLink(itemlink)
  return string.match(itemlink, "|?c?f?f?(%x*)|?H?([^:]*):?(%d+):?(%d*):?(%d*):?(%d*):?(%d*):?(%d*):?(%-?%d*):?(%-?%d*):?(%d*):?(%d*)|?h?%[?([^%[%]]*)%]?|?h?|?r?")
end

function addon:scanBegin(itemlink, unitid, invid, itemid)
  addon.scantt:ClearLines()
  addon.scantt:SetOwner(UIParent, "ANCHOR_NONE");
  if itemid then
    addon.scantt:SetItemByID(itemid)
  elseif itemlink then
    addon.scantt:SetHyperlink(itemlink)
  else
    addon.scantt:SetInventoryItem(unitid, invid)
  end
  return addon.scantt:NumLines()
end

function addon:scanLink()
  local itemname, itemlink = addon.scantt:GetItem()
  return itemlink
end


function addon:scanLine(i)
  return getglobal(addon.scantt:GetName() .. "TextLeft"..i)
end
function addon:scanText(i)
  local line = addon:scanLine(i)
  local text = line and line:GetText()
  text = text or ""
  return text
end

-- returns true if the item contains an inactive meta
local Lgem = string.match(ENCHANT_CONDITION_EQUAL_VALUE,"\1244([^:]+):[^;]+;")
local Lgems = string.match(ENCHANT_CONDITION_EQUAL_VALUE,"\1244[^:]+:([^;]+);")
function addon:metaInactive(itemlink)
  for i=1,addon:scanBegin(itemlink) do
    local text = addon:scanText(i)
    if text:match(ENCHANT_CONDITION_REQUIRES) and 
       (text:match(Lgem) or text:match(Lgems)) and 
       text:match("\124cff808080") then
        return true
    end
  end
  return false
end  


-- returns number of empty sockets of each color in the unmodified item
local sockettext = {
  EMPTY_SOCKET_RED, EMPTY_SOCKET_YELLOW, EMPTY_SOCKET_BLUE, EMPTY_SOCKET_PRISMATIC,
  EMPTY_SOCKET_META, EMPTY_SOCKET_COGWHEEL, EMPTY_SOCKET_HYDRAULIC,
}
local socketcount = {}
function addon:countEmptySockets(itemlink)
  for i=1,#sockettext do socketcount[i] = 0 end
  local total=0
 
  for l=1,addon:scanBegin(itemlink) do
    local text = addon:scanText(l)
    for i=1,#sockettext do 
        if text:find(sockettext[i],nil,true) and -- disable patterns for - in "Sha-Touched"
	   not text:match('^"') -- ignore flavor text ""Sha-Touched""
	   then 
           socketcount[i] = socketcount[i] + 1
           total = total + 1
           break
        end
    end
  end 

  return total, unpack(socketcount)
end

-- return the number of sockets in the raw (ungemmed/unmodded) version of this item
function addon:countRawSockets(itemlink)
  local strippedlink = itemlink:gsub("(item:%d+:%d*):%d*:%d*:%d*:%d*:","%1:0:0:0:0:")
  return addon:countEmptySockets(strippedlink)
end

-- table rewritten to translate localized color names into table of colors
local gemsc = {
  [23094] = { r = 1 },
  [23114] = { y = 1 },
  [23118] = { b = 1 },
  [23099] = { r = 1, y = 1 },
  [23100] = { r = 1, b = 1 },
  [23104] = { y = 1, b = 1 },
  [22460] = { r = 1, y = 1, b = 1},
  [25897] = { m = 1 }
}
function addon:gemColor(itemid)
  local sc = select(7,GetItemInfo(itemid))
  local ge = sc and gemsc[sc]
  if not ge then
    for id,e in pairs(gemsc) do
      if tonumber(id) then
        local esc = select(7,GetItemInfo(id))
	if esc then gemsc[esc] = e end
      end
    end
  end
  ge = sc and gemsc[sc]
  if not ge then return 0,0,0,0 end
  return ge.r or 0, ge.y or 0, ge.b or 0, ge.m or 0
end

-- return the number of socketed gems in an item
-- total, red, yellow, blue, meta itemid, chimera, serpent
local Lchimera, Lserpent
local maxgems = 4
function addon:countGems(itemlink)
  if not Lchimera then
    Lchimera = GetItemInfo(52196)
  end
  if not Lserpent then
    Lserpent = GetItemInfo(76734)
  end
  local tot,r,y,b,m,chim,serp = 0,0,0,0,nil,0,0
  for i=1,maxgems do
    local gname,glink = GetItemGem(itemlink, i)
    if gname then
      tot = tot + 1
      local id = select(3,addon:decomposeItemLink(glink))
      local gr, gy, gb, gm = addon:gemColor(id)
      r = r + gr; y = y + gy; b = b + gb; 
      if gm > 0 then m = id end
    end
    if gname and Lchimera and gname:lower():match(Lchimera:lower()) then -- ticket 27: ruRU changes case
      chim = chim + 1
    elseif gname and Lserpent and gname:lower():match(Lserpent:lower()) then
      serp = serp + 1
    end
  end
  return tot, r, y, b, m, chim, serp
end

local _metacheck = {
  [{52298,41376}] = 
              function(r,y,b) return r >= 2 end,
  [{52291,68778,68779,68780,41285,41333,34220,35503,32409,41398}] = 
              function(r,y,b) return r >= 3 end,

  [{52289,52294,52296}] = 
              function(r,y,b) return y >= 2 end,
  [{32641}] = function(r,y,b) return y >= 3 end,

  [{52299}] = function(r,y,b) return b >= 2 end,
  [{52293,41397,44087,25896}] = 
              function(r,y,b) return b >= 3 end,
  [{25898}] = function(r,y,b) return b >= 5 end,

  [{52292,52297,52300,52301}] = 
              function(r,y,b) return y >= 1 and b >= 1 end,
  [{41378,41381,44084}] = 
              function(r,y,b) return y >= 2 and b >= 1 end,
  [{44088,35501}] = 
              function(r,y,b) return y >= 1 and b >= 2 end,
  [{25893,32640}] = 
              function(r,y,b) return b > y end,

  [{52295}] = function(r,y,b) return r >= 1 and y >= 1 end,
  [{41335,41389}] = 
              function(r,y,b) return r >= 2 and y >= 1 end,
  [{41339,44076,25894,28556,28557}] = 
              function(r,y,b) return r >= 1 and y >= 2 end,
  [{25895}] = function(r,y,b) return r > y end,

  [{52302}] = function(r,y,b) return r >= 1 and b >= 1 end,
  [{41377,41380,41385,44082}] = 
              function(r,y,b) return r >= 1 and b >= 2 end,
  [{41379,41395,41396,44081}] = 
              function(r,y,b) return r >= 2 and b >= 1 end,
  [{25897}] = function(r,y,b) return r > b end,

  [{41307,41375,41382,41400,41401,44078,44089}] = 
              function(r,y,b) return r >= 1 and y >= 1 and b >= 1 end,
  [{25899,25890,25901,32410}] =
              function(r,y,b) return r >= 2 and y >= 2 and b >= 2 end,

  --[{}] = function(r,y,b) return end,
}
local metacheck = {}
for ids,f in pairs(_metacheck) do
  for _,id in pairs(ids) do
    metacheck[id] = f
  end
end

function addon:upgradeLevel(itemlink)
  local pat = ITEM_UPGRADE_TOOLTIP_FORMAT:gsub("%%d","(%%d+)")
  for l=1,addon:scanBegin(itemlink) do
    local text = addon:scanText(l)
    local cur, tot = text:match(pat)
    if cur and tot then return cur, tot, text end
  end
  return nil
end

-- returns raid role for the unit (TANK/HEALER/DAMAGER)
function addon:raidRole(unitid,ignoretalents)
  local currrole = UnitGroupRolesAssigned(unitid)
  if currrole and currrole ~= "NONE" then
    return currrole
  end
  if UnitInRaid(unitid) and GetPartyAssignment("MAINTANK", unitid) then
    return "TANK"
  end
  if ignoretalents then
    return "NONE"
  else
    return addon:talentRole(unitid)
  end
end

function addon:talentRole(unitid)
  local tabIndex = GetSpecialization(check_inspect)
  if not tabIndex then return "NONE" end -- untalented
  if not check_inspect then
    return GetSpecializationRole(tabIndex,false,false)
  else
    local specid = GetInspectSpecialization(unitid)
    local role = specid and specid > 0 and GetSpecializationRoleByID(specid)
    return role or "NONE"
  end
end

local outdoor_bg
function addon:inBG()
  if not outdoor_bg then
    outdoor_bg = {}
    for i = 1,2 do
       local _,n = GetWorldPVPAreaInfo(i)
       outdoor_bg[n] = i
    end
  end
  local i, t = IsInInstance()

  if t == "none" then
    local id = outdoor_bg[GetRealZoneText()]
    if id then
      local _,_, isActive = GetWorldPVPAreaInfo(id)
      if isActive then
        return true
      end
    end
  elseif t == "pvp" then
    return true
  else
    return false
  end
end

local dpsstats = {
    MAGE     = { spellhit=true },
    WARLOCK  = { spellhit=true },
    PRIEST   = { spellhit=true },
    HUNTER   = { rangedhit=true, expertise=true },
    DEATHKNIGHT = { meleehit=true, spellhit=true, expertise=true },
    ROGUE    = { meleehit=true, spellhit=true, expertise=true },
    PALADIN  = { meleehit=true, expertise=true },
    WARRIOR  = { meleehit=true, expertise=true },
    SHAMAN   = { [1] = { spellhit=true }, [2] = { meleehit=true, spellhit=true, expertise=true } }, 
    DRUID    = { [1] = { spellhit=true }, [2] = { meleehit=true, expertise=true } }, 
    MONK     = { meleehit=true, expertise=true }, 
}

function addon:dpsStat(unitid, statname)
    local _, class = UnitClass(unitid)
    local tabIndex = GetSpecialization(check_inspect)
    local stats = dpsstats[class][tabIndex] or dpsstats[class]
    statname = statname:gsub("over$","")
    return stats[statname]
end

-- badstats support
-- omit stats that are always acceptable: crit, haste, mastery, stam
local spiritcaster = { "strength", "agility", "expertise", "attackpower" }
local nonspiritcaster = { "strength", "agility", "expertise", "attackpower", "spirit" }
local badstats = {
    TANK     = { "intel", "spirit" },
    DAMAGER  = { "dodge", "parry", "block" },
    HEALER   = { "strength", "agility", "hit", "expertise", "dodge", "parry", "attackpower", "block" },
    MAGE     = nonspiritcaster,
    WARLOCK  = nonspiritcaster,
    PRIEST   = spiritcaster,
    HUNTER   = { "intel", "spirit", "strength" },
    DEATHKNIGHT = { [1] = { "agility", "block" }, "intel", "spirit" },
    ROGUE    = { "intel", "spirit" },
    PALADIN  = { [1] = spiritcaster, [2] = { "intel", "agility" }, [3] = { "intel", "spirit" } },
    WARRIOR  = { [3] = { "agility" }, "intel", "spirit" },
    SHAMAN   = { [1] = spiritcaster, [2] = { "intel" }, [3] = spiritcaster }, -- enh uses spirit via elem precision
    DRUID    = { [1] = spiritcaster, [2] = { "intel", "spirit" }, [3] = { "parry", "block" }, [4] = spiritcaster },
    MONK     = { [1] = { "block" }, [2] = spiritcaster, [3] = { "intel", "spirit" }},
}

-- the acceptable flask stats for each role/class/spec
local flask_tank = { "stamina" }
local flask_agidps = { "agility" }
local flask_strdps = { "strength" }
local flask_healer = { "intel", "spirit" }
local flask_intdps = { "intel" }
local flask_stats = {
  MAGE 		= flask_intdps,
  WARLOCK	= flask_intdps,
  HUNTER	= flask_agidps,
  ROGUE		= flask_agidps,
  PRIEST	= { [1] = flask_healer, [2] = flask_healer, [3] = flask_intdps },
  DEATHKNIGHT 	= { [1] = flask_tank,   [2] = flask_strdps, [3] = flask_strdps },
  PALADIN  	= { [1] = flask_healer, [2] = flask_tank,   [3] = flask_strdps },
  WARRIOR 	= { [1] = flask_strdps, [2] = flask_strdps, [3] = flask_tank },
  SHAMAN   	= { [1] = flask_intdps, [2] = flask_agidps, [3] = flask_healer },
  DRUID    	= { [1] = flask_intdps, [2] = flask_agidps, [3] = { "agility", "stamina" }, [4] = flask_healer },
  MONK    	= { [1] = { "agility", "stamina" }, [2] = flask_healer, [3] = flask_agidps },
}
local flask_list = {
  ["agility"] = { 79471, 92725, 105689 },
  ["spirit"]  = { 94160,        105693 },
  ["stamina"] = { 79469, 92729, 105694 },
  ["intel"]   = { 79470, 92730, 105691 },
  ["strength"]= { 79472, 92731, 105696 },
} 
local allflask = {}
for _,flasks in pairs(flask_list) do
   for _,fl in pairs(flasks) do
       allflask[fl] = fl
   end
end

-- map stat symbol to the localized item text
local statmap = {
  strength =    { ITEM_MOD_STRENGTH },
  agility =     { ITEM_MOD_AGILITY },
  spirit =      { ITEM_MOD_SPIRIT },  
  -- intel is rolled together with other spell caster only stats
  intel =       { ITEM_MOD_INTELLECT, 
                  ITEM_MOD_SPELL_POWER_SHORT, ITEM_MOD_SPELL_POWER,
                  ITEM_MOD_SPELL_DAMAGE_DONE_SHORT, ITEM_MOD_SPELL_DAMAGE_DONE,
                  ITEM_MOD_SPELL_HEALING_DONE_SHORT, ITEM_MOD_SPELL_HEALING_DONE,
                  ITEM_MOD_MANA, ITEM_MOD_MANA_REGENERATION_SHORT, ITEM_MOD_MANA_REGENERATION },   
  hit =         { ITEM_MOD_HIT_RATING_SHORT, ITEM_MOD_HIT_RATING,
                  ITEM_MOD_HIT_MELEE_RATING_SHORT, ITEM_MOD_HIT_MELEE_RATING,
                  ITEM_MOD_HIT_RANGED_RATING_SHORT, ITEM_MOD_HIT_RANGED_RATING, 
                  ITEM_MOD_HIT_SPELL_RATING_SHORT, ITEM_MOD_HIT_SPELL_RATING }, 
  expertise =   { ITEM_MOD_EXPERTISE_RATING_SHORT, ITEM_MOD_EXPERTISE_RATING },                 
  attackpower = { ITEM_MOD_ATTACK_POWER_SHORT, ITEM_MOD_ATTACK_POWER,
                  ITEM_MOD_MELEE_ATTACK_POWER_SHORT, ITEM_MOD_MELEE_ATTACK_POWER,
                  ITEM_MOD_RANGED_ATTACK_POWER_SHORT, ITEM_MOD_RANGED_ATTACK_POWER,
                  ITEM_MOD_FERAL_ATTACK_POWER_SHORT, ITEM_MOD_FERAL_ATTACK_POWER },
  parry =       { ITEM_MOD_PARRY_RATING_SHORT, ITEM_MOD_PARRY_RATING },
  dodge =       { ITEM_MOD_DODGE_RATING_SHORT, ITEM_MOD_DODGE_RATING },  
  block =       { ITEM_MOD_BLOCK_RATING_SHORT, ITEM_MOD_BLOCK_RATING,
                  ITEM_MOD_BLOCK_VALUE_SHORT, ITEM_MOD_BLOCK_VALUE },
}

function addon:normalizeStat(stat)
      stat = stat:gsub("%%%d%$(.)","%%%1") -- ticket 3: non US clients use %1$c%2$d
      stat = stat:gsub("%%c","[%%-%%+]") -- %c is '+' or '-'
      stat = stat:gsub("%%d","(%%d+)") -- capture number output
      stat = stat:gsub("%%s","(%%d+)") -- 5.x some number output is %s
      stat = stat:lower()
      return stat
end

-- normalize stat entries
for stat, stable in pairs(statmap) do
  for idx, stat in ipairs(stable) do
      stable[idx] = addon:normalizeStat(stat)
  end
end

local falsestats = { -- stat texts that can cause false positives in some locales
	ITEM_MOD_CRIT_RATING_SHORT, -- ticket 21: deDE "Kritische Trefferwertung" [Critical hit] / "Trefferwertung" [hit] 
	ITEM_MOD_SPELL_POWER_SHORT, -- ticket 25: ruRU Strength vs Spell Power
}
for idx,stat in ipairs(falsestats) do
  falsestats[idx] = addon:normalizeStat(stat)
end
function addon:itemtextHasStat(text, statentry)
  local mb, me = text:find(statentry)
  if not mb then return false end
  if text:find(statentry.."%a") or -- ticket 24: ignore substrings of larger words
     text:find("%a"..statentry) then
     return false
  end
  for _,fs in pairs(falsestats) do
    local fb, fe = text:find(fs)
    if fb and (fe - fb) > (me - mb) then 
    	--print("Ignoring false positive: '"..fs.."' ".." is not '"..statentry.."' in "..text)
    	return false 
    end
  end
  return true
end

-- map stat symbol to the enchant id for enchants with non-stat display text
local enchmap = {
  agility = { 2673, -- mongoose
            },
  strength = {  -- melee only enchants
               3870, -- blood draining
	       3241, -- lifeward
               4099, -- landslide
	       3789, -- berserking
	       3225, -- executioner
	       3273, -- deathfrost
	       1900, -- crusader
	       1898, -- lifestealing
	       1899, -- unholy
	       1894, -- icy chill
	       803, -- fiery
	       912, -- demonslaying
	       853, 854, 943, 241, 249, 250, -- beastslayers and striking
            },
  intel = { 4097, -- power torrent
            3722, 4115, 4892, -- lightweave embroidery
            4442, -- jade spirit (also has a spirit proc)
            },
  spirit = { 4084, -- heartsong
             3728, 4116, 4893, -- darkglow embroidery 
            },
  attackpower = {
             3730, 4118, 4894, -- Swordguard Embroidery
             4444, -- Dancing steel
            },
  hit = { -- dps only enchants
            4066, -- Mending
	    4067, -- Avalanche
	    4074, -- Elemental slayer
	    3790, -- Black magic (caster only)
	    2672, -- soulfrost (shadow/frost sp)
	    2671, -- sunfire (fire/arcane sp)
	    2443, -- winter's might
            4443, -- Elemental force
            },
  parry = { 3869, -- blade ward
            },
  dodge = { 4098, -- windwalk
            4446, -- river's song
            4445, -- colossus (dmg absorb)
            
            3595, 3367, -- spellbreaking dk runes
	    3594, 3365, -- swordbreaking dk runes
	    3847, 3883, -- armor dk runes
            },
}

local dkrunes = {
  TANK = {
    ["1H"] = { [3883]=true, [3594]=true, [3595]=true },
    ["2H"] = { [3847]=true, [3365]=true, [3367]=true },
  },
  DAMAGER = {
    ["1H"] = { [3368]=true, [3366]=true, [3370]=true, [3369]=true },
    ["2H"] = { [3368]=true, [3366]=true, [3370]=true, [3369]=true },
  }
}

function addon:InitData()
  -- init
  if not addon.itemchecks_ordered then -- first call
    for name,data in pairs(addon.itemslots) do
      data.id = GetInventorySlotInfo(name)
      data.shortname = name:gsub("Slot",""):gsub("[01]",""):gsub("Secondary","Off")
    end
    addon:InitChecks()
    addon.checks_ordered = order_table(addon.checks)
    addon.checktmp = {}

    for s=1,3 do
      local tmp = {}
      for im,_ in pairs(badImbues[s]) do
         tmp[im:gsub("%s*"..ENCHSLOT_WEAPON.."%s*","")] = true
      end
      badImbues[s] = tmp
    end
  end

  addon:isFishingPole(6256)   
  addon:is2HWeapon(6256)   
  addon:isHeldInOffhand(6256) 
  addon:isShield(82847)
  addon:isMetaGem(6256)
  addon:armorType(3602)
  addon:gemColor(23104)
  addon:countGems("")
  addon:inBG()

end

local profIDs = {
    [164] = PID_BS,  
    [755] = PID_JC,
    [202] = PID_ENG, 
    [333] = PID_ENCH,
    [773] = PID_INSC,
    [165] = PID_LW,  
    [197] = PID_TAIL,
}
function addon:GUILD_ROSTER_UPDATE() 
  debug("GUILD_ROSTER_UPDATE")
  if addon.guildcrafters or InCombatLockdown() then return end
  local res = {}
  local gotone
  local oldoff = GetGuildRosterShowOffline()
  SetGuildRosterShowOffline(true)
  for id in pairs(profIDs) do ExpandGuildTradeSkillHeader(id) end
  for i=1,GetNumGuildTradeSkill() do
    local professionID, _, _, _, _, _, _, _, playerName, _, _, _, skill = GetGuildTradeSkillInfo(i)
    local pid = professionID and profIDs[professionID]
    if playerName and pid then
       res[playerName] = res[playerName] or {}
       res[playerName][pid] = skill
       gotone = true
    end
  end
  for id in pairs(profIDs) do CollapseGuildTradeSkillHeader(id) end
  SetGuildRosterShowOffline(oldoff)
  if gotone then
    addon.guildcrafters = res
    addon:UnregisterEvent("GUILD_ROSTER_UPDATE")
  end
end
-- -------------------------------------------------------------------------------------------------
-- slot table
addon.itemslots = {
	ChestSlot    = { enchantable=true }, -- INVSLOT_CHEST
	FeetSlot     = { enchantable=true }, -- INVSLOT_FEET
	WristSlot    = { enchantable=true }, -- INVSLOT_WRIST
	HandsSlot    = { enchantable=true }, -- INVSLOT_HAND
	HeadSlot     = { enchantable=true }, -- INVSLOT_HEAD
	LegsSlot     = { enchantable=true }, -- INVSLOT_LEGS
	ShoulderSlot = { enchantable=true }, -- INVSLOT_SHOULDER
	BackSlot     = { enchantable=true }, -- INVSLOT_BACK
	MainHandSlot = { enchantable=true }, -- INVSLOT_MAINHAND
	SecondaryHandSlot = { enchantable=true }, -- INVSLOT_OFFHAND
	--RangedSlot   = { enchantable=true }, -- INVSLOT_RANGED
	Finger0Slot  = {  }, -- INVSLOT_FINGER1
	Finger1Slot  = {  }, -- INVSLOT_FINGER2
	WaistSlot    = {  }, -- INVSLOT_WAIST
	NeckSlot     = {  }, -- INVSLOT_NECK
	Trinket0Slot = {  }, -- INVSLOT_TRINKET1
	Trinket1Slot = {  }, -- INVSLOT_TRINKET2
}
local est
function addon:enchantableSlots() 
  if not est then
    est = {}
    for _,s in pairs(addon.itemslots) do
      if s.enchantable then
        table.insert(est, s.shortname)
      end
    end
  end
  return est
end
-- -------------------------------------------------------------------------------------------------
-- cloned from FrameXML/PaperDollFrame.lua and modified to remove floor
local function GetMeleeMissChance(levelOffset, special)
        local chance = BASE_MISS_CHANCE_PHYSICAL[levelOffset];
        chance = chance - GetCombatRatingBonus(CR_HIT_MELEE) - GetHitModifier();
        if (IsDualWielding() and not special) then
                chance = chance + DUAL_WIELD_HIT_PENALTY;
        end
        return chance;
end
local function GetRangedMissChance(levelOffset, special)
        local chance = BASE_MISS_CHANCE_PHYSICAL[levelOffset];
        chance = chance - GetCombatRatingBonus(CR_HIT_RANGED) - GetHitModifier();
        return chance;
end
local function GetSpellMissChance(levelOffset, special)
        local chance = BASE_MISS_CHANCE_SPELL[levelOffset];
        chance = chance - GetCombatRatingBonus(CR_HIT_SPELL) - GetSpellHitModifier();
        return chance;
end
local function GetEnemyDodgeChance(levelOffset)
        local chance = BASE_ENEMY_DODGE_CHANCE[levelOffset];
        local offhandChance = BASE_ENEMY_DODGE_CHANCE[levelOffset];
        local rangedChance = BASE_ENEMY_DODGE_CHANCE[levelOffset];
        local expertisePct, offhandExpertisePct, rangedExpertisePct = GetExpertise();
        chance = chance - expertisePct;
        offhandChance = offhandChance - offhandExpertisePct;
        rangedChance = rangedChance - rangedExpertisePct;
	return chance, offhandChance, rangedChance;
end

-- -------------------------------------------------------------------------------------------------
-- Item Checks
local function statCheck(Order, Statname, Failchance, Failname, Threshdesc, overage)
 return {
    order = Order,
    failname = Failname,
    playeronly = true,
    checktype = "stat",
    aliveonly = true,
    threshdesc = Threshdesc,
    threshmin = 0,
    threshmax = 0.20,
    threshstep = 0.0025,
    threshpercent = true,
    threshname = overage and L["Over cap allowance"] or L["Miss allowance"],
    condition = function(unitid) 
      if addon:raidRole(unitid) == "HEALER" or  -- dont check healers
         (addon:raidRole(unitid) == "TANK" and  -- tanks dont need to be hit/exp capped
	  not Statname:match("hitover")) or -- prohibit hit overage, but allow exp overage for parry cap
         not addon:dpsStat(unitid, Statname) then   -- for relevant stat
	 return false
      end
      if overage and Statname:match("hit") then
         local maxmiss = max((addon:dpsStat(unitid, "spellhit") and settings.checks.spellhitover and GetSpellMissChance(3,false)) or -10000,
	                     (addon:dpsStat(unitid, "meleehit") and settings.checks.meleehitover and GetMeleeMissChance(3,false)) or -10000,
	                     (addon:dpsStat(unitid, "rangedhit") and settings.checks.rangedhitover and GetRangedMissChance(3,false)) or -10000)
	 -- only check overcap for the stat closest to missing
	 return Failchance(3, false) == maxmiss
      end
      return true
    end,
    playercheck = function(unitid) 
      local max = 100*settings.thresh[Statname]
      local lvldelta
      if overage or UnitInRaid(unitid) then
        lvldelta = 3
      else
        lvldelta = 2
      end
      local tgtlvl = UnitLevel(unitid) + lvldelta
      local a,b,c = Failchance(lvldelta, not overage)
      if c and select(2,UnitClass(unitid)) == "HUNTER" then -- ticket 17 hunters use ranged miss, can differ on a dwarf
         a = c
      elseif b and b > a and addon:GetInventoryItem(unitid, INVSLOT_OFFHAND) then -- offhand dodge
         a = b 
      end 

      if Statname == "spellhitover" and a < 0 then
	 local specidx = GetSpecialization(check_inspect)
	 -- some classes get hit from spirit
	 -- don't complain about over hit cap caused by a temporary spirit buffs
	 if  check_class == "PRIEST" or 	          -- priest
	     (check_class == "DRUID" and specidx == 1) or -- boomkin
	     (check_class == "SHAMAN" and specidx == 1)   -- ele shaman
	    then
	    if select(11,UnitBuff(unitid,(GetSpellInfo(127230)))) == 127230 then -- Crystal of Insanity: Visions of Insanity buff
	      a = a + 500/340 -- ignore the spirit contribution to hit overage
	    end
	    if select(11,UnitBuff(unitid,(GetSpellInfo(147282)))) == 147282 then -- Wisdom of Yu'lon, Timeless Isle buff 20%
	      local spirit = UnitStat("player",5)
	      a = a + spirit*0.2/1.2 -- ignore the spirit contribution to hit overage
	    end
	 end
      end

      --print(Statname.." "..(overage and "true" or "false").." "..a.." "..max)
      if overage and a < 0 and -a > max then
        return false, string.format("%.02f%% "..L["Over cap"].." ("..string.format(L["Level %d target"],tgtlvl)..")", -a)
      elseif not overage and a > max then
        return false, string.format("%.02f%% "..MISS.." ("..string.format(L["Level %d target"],tgtlvl)..")", a)
      else
        return true
      end
    end,
  }
end

function addon:InitChecks() 
addon.checks = {

  ilvlsum = { -- calculation matches Blizzard's GetAverageItemLevel()
    order = 5,
    failname = "",
    checktype = "hidden",
    empty = true,
    condition = function(unitid)   
      local t = addon.checktmp.ilvlsum or {}
      addon.checktmp.ilvlsum = t
      t.cnt = 0
      t.sum = 0
      t.emptyhand = 0
      check_ilvl = 0
      return true
    end,
    itemcheck = function(itemid, itemlink, slotid)
      local t = addon.checktmp.ilvlsum
      if itemid then
        local quality, ilvl = select(3, GetItemInfo(itemid))
	ilvl = ilvl or 0
	if quality == 7 and ilvl < 10 then 
	  ilvl = 0.5  -- Blizzard counts some level 1 heirlooms as zero and some as one
	elseif itemlink then -- scan tooltip for upgraded ilvl
	  local ilvlscan = ITEM_LEVEL:gsub("%%d","(%%d+)")
	  --local ilvlscanalt = ITEM_LEVEL_ALT:gsub("%(","%%("):gsub("%)","%%)"):gsub("%%d","(%%d+)")
	  local ilvlscanalt = ITEM_LEVEL:gsub("%%d",".+%%((%%d+)%%)")
          for i=1,math.min(4,addon:scanBegin(itemlink)) do
            local text = addon:scanText(i)
	    local silvl = text:match(ilvlscanalt) or text:match(ilvlscan)
	    silvl = tonumber(silvl)
	    if silvl and silvl > ilvl then
	      ilvl = silvl
	      break
	    end
	  end
	end
	t.sum = t.sum + ilvl
      elseif slotid == INVSLOT_MAINHAND or slotid == INVSLOT_OFFHAND then
        t.emptyhand = t.emptyhand + 1
      end
      t.cnt = t.cnt + 1
      return true
    end,
    playercheck = function(unitid)
      local t = addon.checktmp.ilvlsum
      if t.emptyhand == 2 then -- both hands empty only counts once
         t.cnt = t.cnt - 1
      end
      if t.cnt > 0 then
        check_ilvl = t.sum / t.cnt
      end 
      return true
    end,
  },

  empty = {
    order = 10,
    empty = true,
    failname = L["Empty Slot"],
    itemstop = true, -- stop checking this slot on failure
    -- scanignore = true, -- ignore intermittent failures during scans
    itemcheck = function(itemid, itemlink, slotid)
      return itemid and itemlink
    end,
  },

  fishingpole = {
    order = 20,
    failname = L["Fishing Pole"],
    slots = { "MainHand" },
    itemstop = true, -- stop checking this slot on failure
    itemcheck = function(itemid, itemlink, slotid)
        return not addon:isFishingPole(itemid) 
    end,
  },

  teleportitems = {
    order = 25,
    failname = L["Teleport Items"],
    itemstop = true, -- stop checking this slot on failure
    itemcheck = function(itemid, itemlink, slotid)
        return not teleportItems[itemid] 
    end,
  },

  ilvl = {
    order = 30,
    failname = L["Item Ilvl"],
    threshdesc = L["Minimum equipped item level to allow"],
    threshmin = 25,
    threshmax = 500,
    threshstep = 1,
    itemstop = true, -- stop checking this slot on failure
    itemcheck = function(itemid, itemlink, slotid, unitid)
      local min = settings.thresh.ilvl
      local quality, ilvl = select(3, GetItemInfo(itemid))
      local unitlvl = UnitLevel(unitid)
      if unitlvl < 35 then
        min = 3
      elseif unitlvl < MAXLEVEL then
        min = 25
      end
      return (ilvl and ilvl >= min) or (quality == 7) -- allow heirlooms
      --- XXX: tooltip scan for ITEM_LEVEL_RANGE_CURRENT to determine level appropriate
    end,
  },

  armortype = {
    order = 35,
    failname = L["Armor Type"],
    desc = L["Check that armor type is class-appropriate for armor specialization"],
    itemstop = true, -- stop checking this slot on failure
    condition = function(unitid)
      return UnitLevel(unitid) >= 40
    end,
    itemcheck = function(itemid, itemlink, slotid)
      local atype = addon:armorType(itemid)
      if atype and atype ~= classArmorType[check_class] then
        if slotid == INVSLOT_BACK then 
	   return true
	else
          return false
	end
      else
        return true
      end
    end
  },

  durability = {
    order = 40,
    playeronly = true,
    failname = L["Item Durability"],
    threshdesc = L["Minimum item durability percentage to allow"],
    threshmin = 0,
    threshmax = 1,
    threshpercent = true,
    itemcheck = function(itemid, itemlink, slotid)
      local cur, max = GetInventoryItemDurability(slotid)
      if not cur then return true end -- slot with no repair or info not available
      local val = cur/max
      local min = settings.thresh.durability
      return val >= min
    end,
  },

  enchant = {
    order = 50,
    failname = L["Item Enchant"],
    slots = addon:enchantableSlots(),
    itemcheck = function(itemid, itemlink, slotid, unitid)
      local enchant = select(4,addon:decomposeItemLink(itemlink))
      local _,_,_,ilvl = GetItemInfo(itemid)
      local _, class = UnitClass(unitid)
      if enchant and tonumber(enchant) and tonumber(enchant) > 0 then
        return true -- XXX: check for "good" enchants
      elseif slotid == INVSLOT_HEAD and settings.checks.enchantignorehead then 
        return true
      elseif slotid == INVSLOT_SHOULDER and settings.checks.enchantignoreshoulder then 
        return true
      elseif slotid == INVSLOT_SHOULDER and 
         not (UnitLevel(unitid) >= 60 and ilvl >= 60) and
	 not (UnitLevel(unitid) >= 64) then
         -- lvl 60 player/item heavy knothide kit, lvl 64 for BC scryer/aldor inscriptions
        return true
      elseif slotid == INVSLOT_HEAD and 
         not (UnitLevel(unitid) >= 60 and ilvl >= 60) and
	 not (UnitLevel(unitid) >= 70) then 
         -- lvl 60 player/item heavy knothide kit, lvl 70 for BC rep arcanums
        return true
      else
        return false
      end
    end,
  },

  enchantignoreshoulder = {
    order = 51,
    failname = L["Ignore shoulder enchant"],
    default = false,
    condition = function() return false end,
  },

  enchantignorehead = {
    order = 52,
    failname = L["Ignore head enchant"],
    default = true,
    condition = function() return false end,
  },

  enchantreq = {
    order = 55,
    failname = L["Requirements Not Met"],
    playeronly = true,
    slots = addon:enchantableSlots(),
    itemcheck = function(itemid, itemlink, slotid, unitid)
      for i=1,addon:scanBegin(itemlink) do
        local text = addon:scanText(i)
        if (text:match(ENCHANT_ITEM_MIN_SKILL) or
	    text:match(ENCHANT_ITEM_REQ_LEVEL) or
	    text:match(ENCHANT_ITEM_REQ_SKILL)) and
	    slotid ~= INVSLOT_HAND then -- eng tinkers cant be removed
	  local r,g,b = addon:scanLine(i):GetTextColor()
	  if r > 0.9 and g < 0.15 and b < 0.15 then
            return false, text
	  end
        end
      end
      return true
    end,
  },

  profenchant = {
    order = 60,
    failname = L["Profession Enchant"],
    slots = { "Finger", "Wrist", "Shoulder", "Back", "Hands" },
    itemcheck = function(itemid, itemlink, slotid, unitid)
      local enchant = select(4,addon:decomposeItemLink(itemlink))
      enchant = (enchant and tonumber(enchant)) or 0
      if slotid == INVSLOT_FINGER1 or slotid == INVSLOT_FINGER2 then
        if addon:hasProfession(unitid, PID_ENCH) >= 475 and not profenchants[enchant] then
	  return false
	end
      elseif slotid == INVSLOT_BACK then
        if addon:hasProfession(unitid, PID_TAIL) >= 500 and not profenchants[enchant] then
	  return false
	end
      elseif slotid == INVSLOT_WRIST then
        if addon:hasProfession(unitid, PID_LW) >= 500 and not profenchants[enchant] then
	  return false
	end
      elseif slotid == INVSLOT_SHOULDER then
        if addon:hasProfession(unitid, PID_INSC) >= 500 and not profenchants[enchant] then
	  return false
	end
      elseif slotid == INVSLOT_HAND then
        if addon:hasProfession(unitid, PID_ENG) >= 400 and UnitIsUnit(unitid,"player") then -- cannot directly check eng chants
          local _,_,hasCD = GetInventoryItemCooldown("player",slotid)
	  return hasCD and hasCD > 0
	end
      end
      return true
    end,
  },

  emptysocket = { 
    order = 80,
    failname = L["Empty Socket"],
    itemstop = true, -- stop checking this slot on failure
    -- scanignore = true, -- ignore intermittent failures during scans
    itemcheck = function(itemid, itemlink, slotid)
      local empty,_,_,_,_,_,_,shatouched = addon:countEmptySockets(itemlink)
      if shatouched == 1 and empty == 1 and not settings.checks.gemshatouched then
        return true
      end
      return (empty == 0)
    end,
  },

  gemquality = { 
    order = 85,
    failname = L["Gem Quality"],
    desc = L["Require highest quality gems"],
    itemcheck = function(itemid, itemlink, slotid)
       local _,_,_,ilvl = GetItemInfo(itemid)
       local mingemlvl = 85
       local mingemqual = (settings.checks.gemcataepic and 4) or 3
       if ilvl < 285 then
          mingemlvl = 80
          mingemqual = 4
       end
       local s = ""
       for i=1,maxgems do
         local gname,glink = GetItemGem(itemlink, i)
	 local gitemid = glink and glink:match("\124Hitem:(%d+):")
         if glink and gitemid ~= 41401 then -- ticket 1: allow Insightful Earthsiege
           local _,_,qual,ilvl = GetItemInfo(glink)
           if ilvl < mingemlvl or (qual < mingemqual and not addon:isMetaGem(gitemid)) then 
             if #s > 0 then s = s..", " end
             s = s..glink
           end
         end
       end
       return (#s == 0),s
    end,
  },

  gemcataepic = {
    order = 86,
    failname = L["Cata Epic Gems"],
    default = false,
    condition = function() return false end,
  },


  gemshatouched = {
    order = 87,
    failname = L["Sha-Touched Gems"],
    default = true,
    condition = function() return false end,
  },

  gemeye = {
    order = 88,
    failname = L["Eye of the Black Prince"],
    default = true,
    playeronly = true,  -- cannot verify quest for inspect
    slots = { "MainHand", "OffHand" },
    itemcheck = function(itemid, itemlink, slotid, unitid)
      if addon:isHeldInOffhand(itemid) or addon:isShield(itemid) then return true end
      local shatouched = select(7,addon:countRawSockets(itemlink))
      local ilvl = select(4, GetItemInfo(itemid))
      local rf, flex
      for i=1,addon:scanBegin(itemlink) do
        local text = addon:scanText(i)
        rf = rf or text:match(RAID_FINDER) 
	flex = flex or text:match(PLAYER_DIFFICULTY4)
      end 
      if not IsQuestFlaggedCompleted(32390) and not IsQuestFlaggedCompleted(32432) then -- horde/ally legendary quest step that gives it
        return true
      end
      if shatouched > 0 or -- eye works in 5.2 ToT weapons and sha-touched 5.1, but not heroic 5.1
	  (not flex and rf and ilvl == 502) or  -- ToT LFR
	  (not flex and not rf and ilvl >= 522 and ilvl <= 541 ) -- ToT reg and heroic
	  then 
        -- applied socket is not directly represented in the link, just an extra gem
        local gems = addon:countGems(itemlink)
        local raw = addon:countRawSockets(itemlink)
        return gems == raw + 1
      else -- item ineligble
        return true
      end
    end,

  },

  itemupgrade = { 
    order = 89,
    default = false,
    failname = L["Item Upgrade"],
    itemcheck = function(itemid, itemlink, slotid)
      local cur, tot, text = addon:upgradeLevel(itemlink)
      if cur and cur < tot then
        return false, cur.."/"..tot
      else
        return true
      end
    end,
  },

  beltbuckle = { 
    order = 90,
    failname = L["Belt Buckle"],
    itemstop = true, -- stop checking this slot on failure
    slots = { "Waist" },
    condition = function(unitid)
      return UnitLevel(unitid) >= 70
    end,
    itemcheck = function(itemid, itemlink, slotid)
      local rawsockets = addon:countRawSockets(itemlink)
      local gems = addon:countGems(itemlink)
      if check_inspect and gems == 0 and rawsockets > 0 then
        return true -- prevent false negatives due to slow gem loading
      end
      return (gems == rawsockets + 1)
    end,
  },

  meta = { 
    order = 95,
    failname = L["Meta Gem Active"],
    -- scanignoresecondary = true, -- ignore intermittent failures during scans
    condition = function(unitid)
      local t = addon.checktmp.meta or {}
      addon.checktmp.meta = t
      t.r = 0
      t.y = 0
      t.b = 0
      t.id = nil
      return true
    end,
    itemcheck = function(itemid, itemlink, slotid)
      local t = addon.checktmp.meta
      local _,r,y,b,m = addon:countGems(itemlink)
      t.r = t.r + r; t.y = t.y + y; t.b = t.b + b
      t.id = t.id or m
      return true
      --return not addon:metaInactive(itemlink)
    end,
    playercheck = function(unitid)
      local t = addon.checktmp.meta
      local meta = t.id and tonumber(t.id)
      local func = meta and metacheck[meta]
      if not func then return true
      else
        local link = select(2,GetItemInfo(meta)) or ""
        return func(t.r, t.y, t.b), link.." "..t.r.." "..RED_GEM..", "..
	                                       t.y.." "..YELLOW_GEM..", "..
	                                       t.b.." "..BLUE_GEM
      end
    end,
  },

  profgems = {
    order = 100,
    failname = L["Profession Gemming"],
    -- scanignoresecondary = true, -- ignore intermittent failures during scans
    condition = function(unitid)
      addon.checktmp.profgems_serp = 0
      addon.checktmp.profgems_chim = 0
      addon:countGems("")
      return Lchimera and Lserpent -- prevent load-time failures on cache miss
    end,
    itemcheck = function(itemid, itemlink, slotid, unitid)
      local chim, serp = select(6,addon:countGems(itemlink))
      addon.checktmp.profgems_chim = addon.checktmp.profgems_chim + chim
      addon.checktmp.profgems_serp = addon.checktmp.profgems_serp + serp
      if addon:hasProfession(unitid, PID_BS) >= 400 then
        if slotid == INVSLOT_HAND or slotid == INVSLOT_WRIST then
           local rawsockets = addon:countRawSockets(itemlink)
           local gems = addon:countGems(itemlink)
           return (gems == rawsockets + 1), L["Missing Blacksmith Socket"]
        end
      end
      return true
    end,
    playercheck = function(unitid)
      local expect, has, name
      if (addon:hasProfession(unitid, PID_JC) >= 550 and UnitLevel(unitid) >= 85) or
         addon.checktmp.profgems_serp > 0 then
        has = addon.checktmp.profgems_serp
	expect = 2
	name = Lserpent
      elseif (addon:hasProfession(unitid, PID_JC) >= 500 and UnitLevel(unitid) < 85) or
        addon.checktmp.profgems_chim > 0 then
	has = addon.checktmp.profgems_chim
	expect = 3
	name = Lchimera
      end
      if expect then
        return (has == expect), has.." "..(name or "").." ("..L["Missing"].." "..(expect-has)..")"
      else
        return true
      end
    end,
  },

  badstats = {
    order = 200,
    failname = L["Bad Item Stats"],
    condition = function(unitid)   
      -- gather bad stats
      local role = addon:raidRole(unitid)
      addon.checktmp.badstats = addon.checktmp.badstats or {}
      wipe(addon.checktmp.badstats)
      local tabIndex = GetSpecialization(check_inspect)
      if role and badstats[role] then
        for _,stat in pairs(badstats[role]) do
          addon.checktmp.badstats[stat] = true
        end
      end    
      for idx,stat in pairs(badstats[check_class]) do
        if type(stat) == "string" then
          addon.checktmp.badstats[stat] = true
        elseif type(stat) == "table" and idx == tabIndex then
          for _,stat in pairs(stat) do
            addon.checktmp.badstats[stat] = true
          end
        end
      end
      if role == "DAMAGER" and addon:dpsStat(unitid, "spellhit") then
        -- ticket16: expertise also provides spell hit so allow it for toons that need spellhit
        addon.checktmp.badstats["expertise"] = nil
      end
      return true
    end,
    itemcheck = function(itemid, itemlink, slotid)
      local failtxt = ""
      local failcnt = 0
      local failpat, failline
      for i=2,addon:scanBegin(itemlink) do -- skip item name
        local text = addon:scanText(i):lower()
	text = text:gsub("%("..REFORGED:lower()..".+%)","") -- ignore the reforge source note to avoid duplication
	if text:find(ITEM_SOCKET_BONUS:gsub("%%s",""):lower()) then -- ignore socket bonus
	  --print("Ignoring socket bonus: "..text)
	  text = nil
	end
	if text and #text > 0 then
         for stat, txtmap in pairs(statmap) do
          if addon.checktmp.badstats[stat] then
            for _,mapentry in pairs(txtmap) do
              if addon:itemtextHasStat(text,mapentry) then
	        local statname = txtmap[1]
		statname = statname:gsub(RATING,"")
		statname = statname:gsub("%[?%%%S+%s","")
		statname = strtrim(statname)
		if not failtxt:find(statname) then -- avoid repetition in report
                  failtxt = failtxt..statname.." "
		end
		failcnt = failcnt + 1
		failpat = mapentry
		failline = text
		--print(failcnt,failline,mapentry)
                break
              end
            end
          end
	 end
        end
      end
      if failcnt == 1 and settings.checks.badstatreforged then -- allow one bad stat per item that has been reforged away
	local curval = failline:match("(%d+)")
	curval = curval and tonumber(curval)
        local strippedlink = itemlink:gsub("(item:%d+):.-:.-:.-:.-:.-:(.-:.-:.-):%d+","%1:0:0:0:0:0:%2:0")
	--[[
	print("stripped:"..strippedlink)
	print("failpat:"..failpat)
	print("failline:"..failline)
	print("curval="..curval)
	--]]
        for i=2,addon:scanBegin(strippedlink) do -- skip item name
          local text = addon:scanText(i):lower()
	  local origval = addon:itemtextHasStat(text,failpat) and text:match("(%d+)")
	  origval = origval and tonumber(origval)
	  if curval and origval and curval < origval then
             --print(curval,origval)
	     debug("Ignoring badstat reforged on "..itemlink)
             failtxt = ""
	     break
	  end
	end
      end
      local _,_,_, enchid = addon:decomposeItemLink(itemlink)
      enchid = enchid and tonumber(enchid)
      if enchid and enchid > 0 then
        for stat, ids in pairs(enchmap) do
	  if addon.checktmp.badstats[stat] then
	    for _,id in pairs(ids) do
              if enchid == id then
	        failtxt = failtxt.." "..L["Bad enchant"].." "
	        break
	      end
	    end
	  end
	end
      end
      return #failtxt == 0, failtxt
    end,
  },

  badstatreforged = {
    order = 210,
    failname = L["Allow bad stats reforged away"],
    default = true,
    condition = function() return false end,
  },

  baddkrune = {
    order =  300,
    failname = L["DK Runeforge"],
    checktype = "gear",
    class = { "DEATHKNIGHT" },
    slots = { "MainHand", "OffHand" },
    itemcheck = function(itemid, itemlink, slotid, unitid)
      local role = addon:raidRole(unitid)
      local hand = (addon:is2HWeapon(itemid) and "2H") or "1H"
      local enchant = select(4,addon:decomposeItemLink(itemlink))
      enchant = (enchant and tonumber(enchant)) or 0
      if not role or role == "NONE" then 
        return true
      else
        return dkrunes[role][hand][enchant]
      end
    end,
  },

  talentpts = {
    order = 1000,
    failname = L["Unspent Talent Points"],
    checktype = "stat",
    playeronly = true,  -- xxx
    playercheck = function(unitid) 
      local unspent = GetNumUnspentTalents()
      return (not unspent or unspent == 0), unspent and unspent.." "..L["Unspent Talent Points"]
    end,
  },

  talentptspet = {
    order = 1001,
    failname = L["Untalented Pet"],
    checktype = "stat",
    class = { "HUNTER" },
    playeronly = true, 
    playercheck = function(unitid) 
      local spec = GetSpecialization(false,true)
      return (spec and spec > 0), L["Untalented Pet"]
    end,
  },

  talentspec = {
    order = 1005,
    failname = L["Talent Spec"],
    checktype = "stat",
    playercheck = function(unitid) 
      local talentrole = addon:talentRole(unitid)
      local raidrole = addon:raidRole(unitid, true)
      if not raidrole or raidrole == "NONE" or not talentrole or talentrole == "NONE" or
         (raidrole == "TANK" and not tankClass(check_class)) then -- allow "fake" tanks
         return true
      else
         return (raidrole == talentrole), 
	    -- for localization script: L["TANK"] L["DAMAGER"] L["HEALER"]
	    string.format(L["%s talents with %s group role"], L[talentrole], L[raidrole])
      end
    end,
  },

  glyphs = {
    order = 1010,
    failname = L["Empty Glyph Slots"],
    checktype = "stat",
    playercheck = function(unitid) 
      return glyphcheck(unitid,true)
    end,
  },

  badglyphs = {
    order = 1020,
    failname = L["Bad Glyphs"],
    checktype = "stat",
    playercheck = function(unitid) 
      return glyphcheck(unitid,false)
    end
  },

  resilience = {
    order = 1090,
    failname = L["PvP Gear"],
    playeronly = true,
    threshdesc = L["Resilience"],
    checktype = "stat",
    threshmin = 0,
    threshmax = 4000,
    threshstep = 1,
    threshname = L["Maximum PvP resilience to allow"],
    playercheck = function(unitid) 
      local val = GetCombatRating(COMBAT_RATING_RESILIENCE_CRIT_TAKEN)
      local max = settings.thresh.resilience
      return val <= max, val.." "..L["Resilience"]
    end,
  },

  spellpen = {
    order = 1095,
    failname = L["PvP Gear"],
    playeronly = true,
    threshdesc = L["Spell Penetration"],
    checktype = "stat",
    threshmin = 0,
    threshmax = 400,
    threshstep = 1,
    threshname = L["Maximum PvP spell penetration to allow"],
    playercheck = function(unitid) 
      local val = GetSpellPenetration()
      local max = settings.thresh.spellpen
      return val <= max, val.." "..L["Spell Penetration"]
    end,
  },

  spellhit     =  statCheck(1100, "spellhit",      GetSpellMissChance,   L["Spell Hit"], L["Maximum spell miss chance to allow"], false),
  spellhitover =  statCheck(1105, "spellhitover",  GetSpellMissChance,   L["Spell Hit Over Cap"], L["Maximum spell hit over cap to allow"], true),
  meleehit     =  statCheck(1110, "meleehit",      GetMeleeMissChance,   L["Melee Hit"], L["Maximum melee miss chance to allow"], false),
  meleehitover =  statCheck(1115, "meleehitover",  GetMeleeMissChance,   L["Melee Hit Over Cap"], L["Maximum melee hit over cap to allow"], true),
  rangedhit    =  statCheck(1120, "rangedhit",     GetRangedMissChance, L["Ranged Hit"], L["Maximum ranged miss chance to allow"], false),
  rangedhitover=  statCheck(1125, "rangedhitover", GetRangedMissChance, L["Ranged Hit Over Cap"], L["Maximum ranged hit over cap to allow"], true),
  expertise    =  statCheck(1130, "expertise",     GetEnemyDodgeChance, L["Expertise"], L["Maximum dodge chance to allow"], false),
  expertiseover=  statCheck(1135, "expertiseover", GetEnemyDodgeChance, L["Expertise Over Cap"], L["Maximum expertise over dodge cap to allow"], true),

  pet = {
    order = 2000,
    failname = L["Pet Summoned"],
    checktype = "buff",
    aliveonly = true,
    condition = function(unitid)
        if not UnitIsUnit(unitid,"player") and not UnitInRaid(unitid) and not UnitInParty(unitid) then
          return false
        elseif check_class == "HUNTER" or check_class == "WARLOCK" then
	  return true
        elseif check_class == "MAGE" then
	  local tabIndex = GetSpecialization(check_inspect)
          return (tabIndex == 3)
        elseif check_class == "DEATHKNIGHT" then
	  local tabIndex = GetSpecialization(check_inspect)
          return (tabIndex == 3)
	else
	  return false
	end
    end,
    playercheck = function(unitid) 
      if IsMounted() or UnitInVehicle(unitid) or not UnitIsVisible(unitid) then
        return true -- cannot reliably check when mounted
      elseif check_class == "WARLOCK" and hasBuff(unitid, {108503}) then
        return true -- Grimoire of Sacrifice
      else
        return UnitExists(unitid.."pet")
      end
    end,
  },

  flask = {
    order = 2005,
    failname = L["Flask"],
    checktype = "buff",
    aliveonly = true,
    default = false,
    condition = function(unitid)
      return UnitLevel(unitid) == MAXLEVEL
    end,
    playercheck = function(unitid)
      return hasBuff(unitid, allflask)
    end,
  },

  badflask = {
    order = 2006,
    failname = L["Bad Flask"],
    checktype = "buff",
    aliveonly = true,
    condition = function(unitid)
      return UnitLevel(unitid) == MAXLEVEL
    end,
    playercheck = function(unitid)
      addon.checktmp.badflask_stats = addon.checktmp.badflask_stats or {}
      wipe(addon.checktmp.badflask_stats)
      local tabIndex = GetSpecialization(check_inspect)
      for idx,stat in pairs(flask_stats[check_class]) do
        if type(stat) == "string" then
          addon.checktmp.badflask_stats[stat] = true
        elseif type(stat) == "table" and idx == tabIndex then
          for _,s in pairs(stat) do
            addon.checktmp.badflask_stats[s] = true
          end
        end
      end
      addon.checktmp.badflask_flist = addon.checktmp.badflask_flist or {}
      wipe(addon.checktmp.badflask_flist)
      for stat,flasks in pairs(flask_list) do
        if not addon.checktmp.badflask_stats[stat] then
           for _,fl in pairs(flasks) do
              addon.checktmp.badflask_flist[fl] = fl
           end
        end
      end
      local caster,name = hasBuff(unitid, addon.checktmp.badflask_flist)
      return not caster, name
    end,
  },

  wellfed = {
    order = 2007,
    failname = L["Well Fed"],
    checktype = "buff",
    aliveonly = true,
    default = false,
    playercheck = function(unitid)
      return hasBuff(unitid, {35272,44106,43730,43722,43763})
    end,
  },

  failbuffs = {
    order = 2010,
    failname = L["Fail Buffs"],
    checktype = "buff",
    aliveonly = true,
    class = { "HUNTER", "PALADIN" },
    playercheck = function(unitid) 
      local crusader = GetSpellInfo(32223)
      local pack = GetSpellInfo(13159)
      local cheetah = GetSpellInfo(5118)
      local caster = select(8, UnitBuff(unitid, crusader))
      if UnitIsUnit(hasBuff(unitid, {32223}) or "none", unitid) then -- crusader
        return false, tostring(GetSpellInfo(32223))
      end
      if UnitIsUnit(hasBuff(unitid, {13159}) or "none", unitid) then -- pack
        return false, tostring(GetSpellInfo(13159))
      end
      if UnitIsUnit(hasBuff(unitid, {5118}) or "none", unitid) then -- cheetah
        return false, tostring(GetSpellInfo(5118))
      end
      return true
    end,
  },

  tankstance = {
    order = 2100,
    failname = L["Tank Stance"],
    checktype = "buff",
    aliveonly = true,
    condition = function(unitid) return UnitInParty(unitid) or UnitInRaid(unitid) end,
    playercheck = function(unitid) 
      local istank = (addon:raidRole(unitid) == "TANK") and tankClass(check_class)
      if hasBuff(unitid, {48263,25780}) then -- Blood presence, RF
        return istank
      elseif check_class == "WARRIOR" then
        if UnitIsUnit(unitid,"player") then
	  return istank == (GetShapeshiftFormID() == 18)
	else
	  return true -- no way to check
	end
      elseif check_class == "DRUID" then -- dont check shapeshift forms
        return true
      else
        return not istank
      end
    end,
  },

  dkheader =     buffHeader(3000, LCLASS.DEATHKNIGHT),
  dkhorn       = buffCheck(3005, GetSpellInfo(57330), "DEATHKNIGHT", buffids.ap),
  dkpresence   = buffCheck(3007, L["Presence"], "DEATHKNIGHT", {48263, 48266, 48265}),
  druidheader =  buffHeader(3010, LCLASS.DRUID),
  druidmark    = buffCheck(3015, GetSpellInfo(1126), "DRUID", buffids.stats),
  hunterheader = buffHeader(3020, LCLASS.HUNTER),
  hunteraspect = buffCheck(3025, L["Aspect"], "HUNTER", {13165, 109260}),
  mageheader =   buffHeader(3030, LCLASS.MAGE),
  magearmor =    buffCheck(3032, L["Magical Armor"], "MAGE", {6117, 7302, 30482}),
  mageintel =    buffCheckTwo(3034, GetSpellInfo(1459), "MAGE", buffids.sp, buffids.crit, true),
  monkheader =   buffHeader(3039.1, LCLASS.MONK),
  monkstats =    buffCheck(3039.2, GetSpellInfo(117666), "MONK", buffids.stats),
  paladinheader =buffHeader(3040, LCLASS.PALADIN),
  paladinbless = buffCheckTwo(3047, L["Blessing"], "PALADIN", buffids.mastery, buffids.stats),
  priestheader = buffHeader(3050, LCLASS.PRIEST),
  priestarmor =  buffCheck(3052, L["Inner Fire/Will"], "PRIEST", {588, 73413}),
  priestfort   = buffCheck(3055, GetSpellInfo(21562), "PRIEST", buffids.fort),
  rogueheader =  buffHeader(3060, LCLASS.ROGUE),
  shamanheader = buffHeader(3070, LCLASS.SHAMAN),
  shamanshield = buffCheck(3072, L["Water/Lightning Shield"], "SHAMAN", {324, 52127}),
  warlockheader =buffHeader(3080, LCLASS.WARLOCK),
  warriorheader =buffHeader(3090, LCLASS.WARRIOR),
  warriorshout = buffCheckTwo(3095, L["Battle/Commanding Shout"], "WARRIOR", buffids.strength, buffids.fort),

  paladinseal =  {
    order = 3045, 
    failname = L["Seal"], 
    checktype = "buff",
    aliveonly = true,
    playeronly = true,
    class = { "PALADIN" }, 
    playercheck = function(unitid) 
      return GetShapeshiftForm() > 0
    end,
  },
  
  magemanagem = {
    order =  3039,
    failname = L["Mana Gem"],
    checktype = "buff",
    aliveonly = true,
    playeronly = true,
    class = { "MAGE" },
    threshdesc = L["Minimum number of charges remaining to allow"],
    threshmin = 1,
    threshmax = 10,
    threshstep = 1,
    playercheck = function(unitid) 
      local cnt = GetItemCount(81901,false,true)
      if cnt == 0 then
        cnt = GetItemCount(36799,false,true)
      end
      return cnt >= settings.thresh.magemanagem, cnt.." "..L["charges remaining"]
    end,
  },

  magewater = {
    order =  3038,
    failname = L["Conjured Water"],
    checktype = "buff",
    aliveonly = true,
    playeronly = true,
    class = { "MAGE" },
    threshdesc = L["Minimum number of charges remaining to allow"],
    threshmin = 1,
    threshmax = 80,
    threshstep = 1,
    playercheck = function(unitid) 
      local cnt = GetItemCount(65499,false,true) + 
                  GetItemCount(80610,false,true) + -- self-food
                  GetItemCount(80618,false,true)   -- table food
      return cnt >= settings.thresh.magewater, cnt.." "..L["charges remaining"]
    end,
  },

  monkcrit = {
    order =  3039.3,
    failname = GetSpellInfo(116781), -- Legacy of the White Tiger
    checktype = "buff",
    aliveonly = true,
    condition = function(unitid) return check_class == "MONK" and GetSpecialization(check_inspect) == 3 end,
    playercheck = function(unitid) 
      return hasBuff(unitid, buffids.crit), GetSpellInfo(116781)
    end,
  },

  warlockhealthstone = {
    order =  3086,
    failname = L["Healthstone"],
    checktype = "buff",
    aliveonly = true,
    playeronly = true,
    class = { "WARLOCK" },
    playercheck = function(unitid)
      local cnt = GetItemCount(5512,false,false)
      return cnt and cnt > 0 
    end,
  },

  warlocksoulshards = {
    order =  3087,
    failname = L["Soul Shards"],
    checktype = "buff",
    aliveonly = true,
    playeronly = true,
    class = { "WARLOCK" },
    threshdesc = L["Minimum number of charges remaining to allow"],
    threshmin = 1,
    threshmax = 3,
    threshstep = 1,
    playercheck = function(unitid) 
      local cnt = UnitPower(unitid, 7)
      return cnt >= settings.thresh.warlocksoulshards, cnt.." "..L["charges remaining"]
    end,
  },

  warlockdarkintent =  buffCheckTwo(3084,  GetSpellInfo(109773), "WARLOCK", buffids.sp, buffids.fort, true),

  spriestbuff = {
    order =  3058,
    failname =  GetSpellInfo(15473),
    checktype = "buff",
    aliveonly = true,
    condition = function(unitid) return check_class == "PRIEST" and GetSpecialization(check_inspect) == 3 end,
    playercheck = function(unitid) 
      return hasBuff(unitid, {15473}) , GetSpellInfo(15473)
    end,
  },

  roguepoison = buffCheck(3065, L["Weapon Poison"], "ROGUE", buffids.poison),

  shamanimbue = {
    order = 3075,
    playeronly = true,
    failname = L["Weapon Imbue"],
    checktype = "buff",
    class = { "SHAMAN" },
    playercheck = function(unitid) return checkWeaponImbue(unitid) end,
  },
  shamanearthshield = {
    order =  3074,
    failname = GetSpellInfo(974), -- Earth Shield
    checktype = "buff",
    aliveonly = true,
    class = { "SHAMAN" },
    condition = function(unitid) return GetSpecialization(check_inspect) == 3 end,
    playercheck = function(unitid)
      return groupMemberHasMyBuff(unitid, 974, true)
    end,
  },

}
end
-- -------------------------------------------------------------------------------------------------
function addon:RunCheck(unitid, ismanual)
  debug("RunCheck", unitid)
  check_manual = ismanual
  addon:ResetFails(unitid)
  addon:CheckUnit(unitid)
  return addon:ReportFails(unitid)
end
local checkcond = {}
function addon:CheckUnit(unitid)
  local isplayer = UnitIsUnit(unitid, "player")
  local _, class = UnitClass(unitid)
  local ft = addon.failtable[GetUnitName(unitid,true)]
  check_class = class
  check_unit = unitid
  check_inspect = (unitid ~= "player")
  if check_inspect and not addon:UserInspect() then -- try to force load of inspect items
    --NotifyInspect(unitid)
    GetSpecialization(true)
    GetInspectSpecialization(unitid)
    for _,data in pairs(addon.itemslots) do
      addon:scanBegin(nil, unitid, data.id)
    end
  end
  -- preconditions
  wipe(checkcond)
  for _,check in ipairs(addon.checks_ordered) do
     local ctype = check.checktype or "gear"
     local doit = not check.header and (
        (ctype == "hidden" or (settings.checks[check.checkname] and settings.checks[ctype])))
     if doit and ctype == "buff" and settings.checks.instanceonly then
       local isInstance, instanceType = IsInInstance()
       doit = isInstance and (instanceType == "party" or instanceType == "raid")
     end
     if doit and check.playeronly and not isplayer then
       doit = false
     end
     if doit and ctype == "buff" and settings.checks.selfonly and not isplayer then
       doit = false
     end
     if doit and check.aliveonly and UnitIsDead(unitid) then
       doit = false
     end
     if doit and check.class then
       if type(check.class) == "string" and check_class ~= check.class then
         doit = false
       elseif type(check.class) == "table" then
         doit = false
         for _,c in pairs(check.class) do
	   if c == check_class then
	     doit = true
	     break
	   end
	 end
       end
     end
     if doit and check.condition then
       checkcond[check] = check.condition(unitid)
     else
       checkcond[check] = doit
     end
  end
  -- item checks
  for name,data in pairs(addon.itemslots) do
    local itemlink, itemid = addon:GetInventoryItem(unitid, data.id)

    if not itemid and data.id == INVSLOT_OFFHAND and 
       addon:is2HWeapon(addon:GetInventoryItem(unitid, INVSLOT_MAINHAND)) and
       not (check_class == "WARRIOR" and GetSpecialization(check_inspect) == 2) --- titan's grip
    then
      -- offhand is empty for two-hander, skip checks
    else
     for _,check in ipairs(addon.checks_ordered) do
       local doit = checkcond[check]
       if doit and check.slots then
         doit = false
         for _,s in pairs(check.slots) do
	   if s == name or s == data.shortname then
	     doit = true
	     break
	   end
	 end
       end
       if doit and not itemid and not check.empty then
         doit = false
       end
       if doit and check.itemcheck then
         --debug("Checking "..data.shortname.." "..itemid.." "..itemlink)
         local passed, note = check.itemcheck(itemid, itemlink, data.id, unitid)
         if not passed then
	   if check_inspect and not addon:ReportToWindow() then
	     if check.scanignore or (check.scanignoresecondary and ft.scanfail) then
	       ft.scanfail = true
	       break
	     end
	   end
	   addon:RecordFail(unitid, check, name, (itemlink or "").." "..(note or ""))
           if check.itemstop then
	     break
	   end
	 end
       end
     end
    end
  end
  -- player checks
  for _,check in ipairs(addon.checks_ordered) do
     local doit = checkcond[check]
     if doit and check.playercheck then
       addon.lowduration = false
       local passed, note = check.playercheck(unitid)
       if not note and addon.lowduration then
         note = L["(Low duration)"]
       end
       if not passed then
	 if check_inspect and not addon:ReportToWindow() and 
	   (check.scanignore or (check.scanignoresecondary and ft.scanfail)) then
	       ft.scanfail = true
	 else
	   addon:RecordFail(unitid, check, nil, nil, note)
	 end
       end
     end
  end
end

addon.failtable = {}
function addon:ResetFails(unitid)
  local n = GetUnitName(unitid,true)
  addon.failtable[n] = addon.failtable[n] or {}
  local rf = addon.reportframe
  if rf and UnitIsUnit(unitid, rf.unitid) then 
    rf.textcache = ""
    rf.textcnt = 0
  end
  wipe(addon.failtable[n])
  addon.failtable[n].lastcheck = GetTime()
end

function addon:RecordFail(unitid, check, slot, itemlink, note)
  local n = GetUnitName(unitid,true)
  local ft = addon.failtable[n] or {}
  addon.failtable[n] = ft
  local faildata = ft[check.order] or {}
  ft[check.order] = faildata
  faildata.check = check
  faildata.note = note
  if slot then
    faildata.slot = faildata.slot or {}
    faildata.slot[slot] = itemlink
  end
  ft.count = (ft.count or 0) + 1
  ft.checksum = addon:hash((ft.checksum or 0)..check.checkname..(itemlink or "")..(slot or "")..(note or ""))
end

-- returns inspect frame
function addon:UserInspect() 
    if InspectFrame and InspectFrame:IsVisible() then
       return InspectFrame
    elseif Examiner and Examiner:IsVisible() then
       return Examiner
    else
       return nil
    end
end

function addon:ReportToWindow()
  return addon.reportframe and addon.reportframe:IsVisible() and UnitIsUnit(addon.reportframe.unitid, check_unit)
end

function addon:ReportPrint(str)
  if addon:ReportToWindow() then
    addon.reportframe.textcache = addon.reportframe.textcache .. str .. "\n"
    addon.reportframe.textcnt = addon.reportframe.textcnt + 1
  else
    chatAlert(str)
  end
end

function addon:ReportFinish()
  local rf = addon.reportframe
  if addon:ReportToWindow() and rf.textcache ~= rf.textlast then
    rf.text:Clear()
    rf.text:AddMessage(rf.textcache)
    rf.textlast = rf.textcache
    for i=1,25-addon.reportframe.textcnt do
      rf.text:AddMessage(" ")
    end
    rf.text:ScrollToTop() -- seems to make the hyperlinks more reliable
    rf.text:ScrollToBottom()
    rf.text:PageUp()
  end
end

function addon:ReportFails(unitid)
  local n = GetUnitName(unitid,true)
  local ft = addon.failtable[n]
  local lclass, class = UnitClass(unitid)
  local cname = n
  local win = addon:ReportToWindow()
  if class and RAID_CLASS_COLORS[class] then
    local c = RAID_CLASS_COLORS[class]
    cname = string.format("\124cff%02x%02x%02x%s\124r", c.r*255, c.g*255, c.b*255, cname)
    local spec = ""
    if check_inspect then
      local specid = GetInspectSpecialization(check_unit)
      if specid and specid > 0 then
        spec = select(2,GetSpecializationInfoByID(specid))
        if spec then spec = spec.." " else spec = "" end
      end
    else
      local tab = GetSpecialization(check_inspect)
      spec = (tab and select(2,GetSpecializationInfo(tab)).." ") or ""
    end
    local nl = (addon:ReportToWindow() and "\n ") or ""
    local il = string.format("%.1f",check_ilvl)
    cname = cname..nl.." ("..spec..lclass..", "..il.." "..ITEM_LEVEL_ABBR..")"
  end
  local rname = cname:gsub("%s*%(","\n       "):gsub("%)","")
  ft.summary = string.format(L["%i failures for %s"],ft.count or 0,rname)
  if not ft.count then
      debug("No failures for "..cname)
      if settings.general.alwayschat or check_manual or win then
        addon:ReportPrint(string.format(L["Detected %i failures for %s"],0,cname))
	if UnitIsUnit(unitid,"player") then
	  addon:ReportPrint(L["Congratulations - You're (probably) not an idiot!"])
	end
      end
  else
    addon:ReportPrint(string.format(L["Detected %i failures for %s"],ft.count,cname)..":")
    for _, check in ipairs(addon.checks_ordered) do
       local faildata = ft[check.order]
       if faildata then
         if win then addon:ReportPrint(" "); end
         local str = "\124cffff0000"..faildata.check.failname.."\124r"
         if faildata.slot then
	   local cnt = 0
	   str = str..": "
	   for slot,itemlink in pairs(faildata.slot) do cnt = cnt + 1 end
	   if cnt > 1 then 
	     str = str.."("..cnt..")"
	     if win then addon:ReportPrint(str); str = "   " end
	   end
	   cnt = 0
	   for slot,itemlink in pairs(faildata.slot) do
	     cnt = cnt + 1
	     if cnt > 1 and not win then str = str..", " end
	     str = str..itemlink.."("..addon.itemslots[slot].shortname..")"
	     if win then addon:ReportPrint(str); str = "   "
	     elseif cnt > 2 then addon:ReportPrint(str); str = "   "; cnt = 0 end
           end
	   if #str > 10 then
	     addon:ReportPrint(str)
	   end
         else
	   if faildata.note then
	     str = str..(faildata.note and ": "..faildata.note or "")
	   end
           addon:ReportPrint(str)
         end
       end
    end
  end
  addon:ReportFinish()
  if settings.general.bbintegration and BigBrother and BigBrother.unitstatus then
    local guid = UnitGUID(unitid)
    if guid then
      local status = BigBrother.unitstatus[guid] or {}
      BigBrother.unitstatus[guid] = status
      local fail = (ft.count and (ft.count > 0))
      status.highlight = fail
      status[addonName] = ft.summary
      if BigBrother.unitstatus.refresh then
        BigBrother.unitstatus.refresh()
      end
    end
  end
  if settings.general.riintegration and RoleIcons and RoleIcons.unitstatus then
    local guid = UnitGUID(unitid)
    if guid then
      local fail = (ft.count and (ft.count > 0))
      RoleIcons.unitstatus[guid] = fail
      if RoleIcons.unitstatus.refresh then
         RoleIcons.unitstatus.refresh()
      end
    end
  end
    if settings.general.sound and not addon:ReportToWindow() and not check_inspect then
      local soundfile = addon.soundtable[settings.general.soundname]
      if not ft.count then
        if check_manual then 
          soundfile = addon.soundok
	else
	  soundfile = nil
	end
      elseif settings.general.soundrandom then
        local idx = math.random(#addon.soundtable)
	soundfile = addon.soundtable[idx]
      end
      addon.lastsoundtime = addon.lastsoundtime or 0
      local now = GetTime()
      if soundfile and now - addon.lastsoundtime >= 5 then
        addon.lastsoundtime = now
        PlaySoundFile(soundfile, "Master")
      end
    end
    return ft.count or 0
end
function addon:OnGameTooltipSetUnit(name,unitid) 
  local n = name
  if unitid and UnitIsPlayer(unitid) then 
    n = GetUnitName(unitid,true)
  else
    return
  end
  local ft = addon.failtable[n]
  if settings.general.ttintegration and ft and ft.summary then
    GameTooltip:AddLine(addonName..": "..ft.summary)
    GameTooltip:Show()
  end
end
-- -------------------------------------------------------------------------------------------------
local function table_default(t, d)
  for k,v in pairs(d) do
    if type(v) == "table" then
      t[k] = t[k] or {}
      table_default(t[k], v)
    elseif t[k] == nil then
      t[k] = v
    end
  end
end

function addon:RefreshConfig()
  -- things to do after load or settings are reset
  debug("RefreshConfig")
  settings = addon.db.profile
  addon.settings = settings
  for n,c in pairs(addon.checks) do
    if defaults.profile.checks[n] == nil then
      if c.default ~= nil then
        defaults.profile.checks[n] = c.default
      else
        defaults.profile.checks[n] = true
      end
    end
  end
  table_default(settings,defaults.profile)
  for k,v in pairs(defaults.profile) do
     if settings[k] == nil then
       settings[k] = table_clone(v)
     end
  end
  settings.gearchecks = settings.checks
  settings.statchecks = settings.checks
  settings.buffchecks = settings.checks
  settings.loaded = true
  addon:Update()
end

function addon:Update()
  -- things to do when settings change
  if addon.LDBo then
    minimapIcon:Refresh(addonName)
  end
end

function addon:SetupVersion()
   local svnrev = 0
   local files = vars.svnrev
   files["X-Build"] = tonumber((GetAddOnMetadata(addon.name, "X-Build") or ""):match("%d+"))
   files["X-Revision"] = tonumber((GetAddOnMetadata(addon.name, "X-Revision") or ""):match("%d+"))
   for _,v in pairs(files) do -- determine highest file revision
     if v and v > svnrev then
       svnrev = v
     end
   end
   addon.revision = svnrev

   files["X-Curse-Packaged-Version"] = GetAddOnMetadata(addon.name, "X-Curse-Packaged-Version")
   files["Version"] = GetAddOnMetadata(addon.name, "Version")
   addon.version = files["X-Curse-Packaged-Version"] or files["Version"] or "@"
   if string.find(addon.version, "@") then -- dev copy uses "@.project-version.@"
      addon.version = "r"..svnrev
   end
end


function addon:OnInitialize()
  -- AddonLoader support
  SLASH_IDIOTCHECK1 = nil
  SlashCmdList["IDIOTCHECK"] = nil
  hash_SlashCmdList["/idiotcheck"] = nil
  SLASH_RTC1 = nil
  SlashCmdList["IC"] = nil
  hash_SlashCmdList["/ic"] = nil
  GameTooltip:Hide()

  addon:SetupVersion()
  addon.db = LibStub("AceDB-3.0"):New("IdiotCheckDB", defaults)
  addon:InitData()
  addon:RefreshConfig()

  local options = addon:myOptions()
  LibStub("AceEvent-3.0"):Embed(addon)
  LibStub("AceConfigRegistry-3.0"):ValidateOptionsTable(options, addonName)
  LibStub("AceConfig-3.0"):RegisterOptionsTable(addonName, options, {"idiotcheck", "ic"})
  optionsFrame = LibStub("AceConfigDialog-3.0"):AddToBlizOptions(addonName, addonName, nil, "general")
  optionsFrame.default = function()
       for k,v in pairs(defaults.profile) do settings[k] = table_clone(v) end
       addon:RefreshConfig()
       if InterfaceOptionsFrame:IsShown() then
         addon:Config(); addon:Config()
       end
  end
  LibStub("AceConfigDialog-3.0"):AddToBlizOptions(addonName, L["Gear Checks"], addonName, "gearchecks").default = optionsFrame.default
  LibStub("AceConfigDialog-3.0"):AddToBlizOptions(addonName, L["Stat Checks"], addonName, "statchecks").default = optionsFrame.default
  LibStub("AceConfigDialog-3.0"):AddToBlizOptions(addonName, L["Buff Checks"], addonName, "buffchecks").default = optionsFrame.default
  options.args.profiles = LibStub("AceDBOptions-3.0"):GetOptionsTable(addon.db)
  profileFrame = LibStub("AceConfigDialog-3.0"):AddToBlizOptions(addonName, L["Profiles"], addonName, "profiles")

  -- Add dual-spec support if the library is available
  local LibDualSpec = LibStub:GetLibrary('LibDualSpec-1.0',true)
  if LibDualSpec then
    LibDualSpec:EnhanceDatabase(self.db, addonName)
    LibDualSpec:EnhanceOptions(options.args.profiles, self.db)
  end

  -- we don't directly call LibTalentQuery, but hook its events if other addons are using it
  local TalentQuery = LibStub:GetLibrary("LibTalentQuery-1.0",true)
  if TalentQuery then
     TalentQuery.RegisterCallback(self, "TalentQuery_Ready")
  end
  --LGT = LibStub:GetLibrary("LibGroupTalents-1.0",true)

  debug("OnInitialize")

  self.db.RegisterCallback(self, "OnProfileChanged", "RefreshConfig")
  self.db.RegisterCallback(self, "OnProfileCopied", "RefreshConfig")
  self.db.RegisterCallback(self, "OnProfileReset", "RefreshConfig")
  self.db.RegisterCallback(self, "OnDatabaseReset", "RefreshConfig")

end

function addon:Config()
  if optionsFrame then
    if ( InterfaceOptionsFrame:IsShown() ) then
      InterfaceOptionsFrame:Hide();
    else
      InterfaceOptionsFrame_OpenToCategory(profileFrame)
      InterfaceOptionsFrame_OpenToCategory(optionsFrame)
    end
  end
end

function addon:OnEnable()
  debug("OnEnable")
  addon:RegisterEvent("READY_CHECK")
  addon:RegisterEvent("UNIT_TARGET")
  addon:RegisterEvent("INSPECT_READY")
  addon:RegisterEvent("ZONE_CHANGED")
  addon:RegisterEvent("ZONE_CHANGED_NEW_AREA", "ZONE_CHANGED")
  addon:RegisterEvent("ZONE_CHANGED_INDOORS", "ZONE_CHANGED")
  addon:RegisterEvent("PLAYER_ENTERING_WORLD")
  addon:RegisterEvent("PLAYER_LEAVING_WORLD")
  addon:RegisterEvent("ADDON_LOADED")

  -- hooksecurefunc(GameTooltip,"SetUnit", function(self, unit) addon:OnGameTooltipSetUnit(nil,unit) end)
  GameTooltip:HookScript("OnTooltipSetUnit",function(self,...) addon:OnGameTooltipSetUnit(GameTooltip:GetUnit()) end)

  -- handle dynamic load on instance entrance
  addon:ZONE_CHANGED()
 
  addon:RegisterEvent("GUILD_ROSTER_UPDATE")
  GuildRoster() -- populate guild professions data

  if LDB then
    return
  end
  if AceLibrary and AceLibrary:HasInstance("LibDataBroker-1.1") then
    LDB = AceLibrary("LibDataBroker-1.1")
  elseif LibStub then
    LDB = LibStub:GetLibrary("LibDataBroker-1.1",true)
  end
  if LDB then
    local dataobj = LDB:GetDataObjectByName(addonName) or
     LDB:NewDataObject(addonName, {
        type = "launcher",
        label = addonName,
        icon = "Interface\\AddOns\\"..addonName.."\\icon",
     })
     dataobj.OnClick = function(self, button)
                if button == "RightButton" then
                        addon:Config()
                elseif button == "MiddleButton" then
                        addon:groupscan()
                else
                        addon:RunCheck("player",true)
                end
        end
     dataobj.OnTooltipShow = function(tooltip)
                if tooltip and tooltip.AddLine then
                        tooltip:SetText(addonName.." "..addon.version)
                        tooltip:AddLine("|cffff8040"..L["Left Click"].."|r "..L["to run a check"])
                        tooltip:AddLine("|cffff8040"..L["Middle Click"].."|r "..L["to scan group members"])
                        tooltip:AddLine("|cffff8040"..L["Right Click"].."|r "..L["for options"])
                        tooltip:Show()
                end
        end
     addon.LDBo = dataobj
  end 

  if addon.LDBo then
    minimapIcon:Register(addonName, addon.LDBo, settings.minimap)
  end
  addon:Update()
end

-------------------------------------------------------------------------------------
-- Auto checks

function addon:autocheck(unitid, force) 
  local n = GetUnitName(unitid,true)
  local now = GetTime() 
  if addon:inBG() and not settings.general.bgcheck and not force then
    return 
  end
  if not addon.inworld then
    debug("Skipping check while not in world")
    return
  end
  if UnitLevel(unitid) < MAXLEVEL and settings.general.maxlevel and not force then
    return 
  end
  local ft = addon.failtable[n]
  if now > (ft and ft.lastcheck or 0)+(60*settings.general.autochecktime) or force then
    if UnitIsUnit(unitid, "player") then
      addon.instancecheck = false
    end
    return addon:RunCheck(unitid)
  end
end

function addon:groupscan() 
  local base
  if IsInRaid() then
    base = "raid"
  elseif GetNumGroupMembers() > 0 then
    base = "party"
  else
    return true -- not in group
  end
  if addon:UserInspect() or InCombatLockdown() or UnitIsDead("player") or not addon.inworld then return end
  chatAlert(L["Scanning group members..."])
  addon.groupscanpos = addon.groupscanpos or {}
  wipe(addon.groupscanpos)
  for i=1,GetNumGroupMembers() do
    addon.groupscanpos[base..i] = 1
  end
  addon.groupscanexp = GetTime()+30
  addon.groupscancnt = 0
  addon.groupscanfail = 0
  addon.groupscanfailp = 0
  addon:groupscanbump(nil)
end

local function groupscanUpdate(frame,elap)
  addon.groupscanelap = (addon.groupscanelap or 0) + elap
  if addon.groupscanelap < 0.25 then return end
  addon.groupscanelap = 0
  addon:groupscanbump(nil)
end

hooksecurefunc("NotifyInspect", function(unit)
   if (UnitExists(unit) and UnitIsConnected(unit) and CanInspect(unit)) then
       addon.InspectTime = GetTime()
       addon.InspectPending = (addon.InspectPending or 0) + 1
       addon.LastNotify = UnitGUID(unit)
   end
end)

function addon:groupscanbump(unitid) 
  if not addon.groupscanpos then return end
  if not addon.groupscanframe then
     addon.groupscanframe = CreateFrame("Frame", addonName.."GroupScanFrame", UIParent)
  end
  if addon:UserInspect() or GetTime() > addon.groupscanexp or 
     InCombatLockdown() or UnitIsDead("player") then 
       -- cancel scan
  else
    addon.groupscanframe:SetScript("OnUpdate", groupscanUpdate)
    if unitid then 
      -- see if inspect items appear to be loaded yet
      local good = 0
      local bad = 0
      for _,data in pairs(addon.itemslots) do
        local itemlink, itemid = addon:GetInventoryItem(unitid, data.id)
        if itemlink and itemid and itemlink:find("%[.+%]") then
          good = good + 1
        elseif not (itemlink == nil and itemid == nil) then
          bad = bad + 1
        end
      end
      if bad > 0 or good < 2 then
        debug("Delaying groupscan inspect for: "..(UnitName(unitid) or unitid).." bad="..bad.." good="..good)
        return
      end
      local fail = addon:autocheck(unitid, true)
      addon.groupscancnt = addon.groupscancnt + 1
      addon.groupscanfail = addon.groupscanfail + (fail or 0)
      addon.groupscanfailp = addon.groupscanfailp + ((fail and fail > 0 and 1) or 0)
      --ClearInspectPlayer(unitid)
      addon.groupscanpos[unitid] = nil 
      return
    end
    if (not addon.InspectTime or addon.InspectTime + 5 < GetTime()) then
        addon.InspectPending = 0
    end
    if addon.InspectPending > 0 then
       return
    end
    local minv, minu
    for u,v in pairs(addon.groupscanpos) do
      if not UnitExists(u) or not UnitIsConnected(u) or UnitIsUnit(u,"player") then
         addon.groupscanpos[u] = nil 
      elseif CanInspect(u) then
         if not minv or v < minv then
	   minv = v; minu = u
	 end
      end
    end
    if minu then
      debug("NotifyInspect: "..minu.." ("..minv..")")
      addon.groupscanpos[minu] = minv+1
      NotifyInspect(minu)
      if false then -- quickscan path, ungemmed gear and messes up talents
        local p = GetSpecialization(true)
	local t = GetInspectSpecialization(true)
        if addon:GetInventoryItem(minu, INVSLOT_MAINHAND) and ((p and t and t > 0) or minv > 5) then
          addon:groupscanbump(minu)
        end
      end
      return
    end
  end
  addon.groupscanexp = 0
  addon.groupscanframe:SetScript("OnUpdate", nil)
  -- scan complete
  chatAlert(L["Scanned %d group members, %d failures in %d players."]:format(
             addon.groupscancnt, addon.groupscanfail, addon.groupscanfailp))
end

function addon:READY_CHECK(sender)
  if settings.general.readycheck then 
    addon:autocheck("player")
  end
  if settings.general.readyscan then
    addon:groupscan()
  end
end

function addon:ZONE_CHANGED()
  local isInstance, instanceType = IsInInstance()
  if (isInstance and (instanceType == "party" or instanceType == "raid")) then
    if not addon.insideinstance then
      addon.insideinstance = true
      if settings.general.newinstance then 
        addon.instancecheck = true
      end
    end
  else
    addon.insideinstance = false
    addon.instancecheck = false
  end
end

addon.inworld = IsLoggedIn()
function addon:PLAYER_ENTERING_WORLD()
  addon.inworld = true
  addon:ZONE_CHANGED()
end
function addon:PLAYER_LEAVING_WORLD()
  addon.inworld = false
end

function addon:UnitUpdate(unit)
  if InCombatLockdown() or not UnitExists(unit) then return end
  local tgtlvl = UnitLevel(unit)
  local mobtype = UnitClassification(unit)
  if UnitIsEnemy("player",unit) and not UnitIsDead(unit) and
       (mobtype == "elite" or mobtype == "rareelite" or mobtype == "worldboss") and
       (addon.instancecheck or 
        (settings.general.targetboss and (tgtlvl == -1 or tgtlvl >= UnitLevel("player")+2))) then
      addon:autocheck("player")
  end
end

function addon:UNIT_TARGET(evt, unit)
  if InCombatLockdown() then return end
  --debug("UNIT_TARGET: "..(unit or "nil"))
  addon:UnitUpdate(unit.."target")
end
-------------------------------------------------------------------------------------
function addon:TalentQuery_Ready(e, name, realm, unitid)
  debug("TalentQuery_Ready: "..(name or "nil").." "..(unitid or "nil"))
  addon:INSPECT_READY(nil,UnitGUID(unitid))
end

function addon:INSPECT_READY(event, guid)
  addon.InspectPending = (addon.InspectPending or 0) - 1
  local userf = addon:UserInspect()  
  if (InCombatLockdown() and not userf) or not guid then return end
  local class, classFilename, race, raceFilename, sex, name, realm = GetPlayerInfoByGUID(guid)
  local unitid
  if UnitInRaid("player") then
    for i=1,40 do
      if UnitGUID("raid"..i) == guid then
        unitid = "raid"..i
	break
      end
    end
  elseif UnitInParty("player") then
    for i=1,5 do
      if UnitGUID("party"..i) == guid then
        unitid = "party"..i
	break
      end
    end
  end
  if not unitid then
   if UnitGUID("player") == guid then
      unitid = "player"
   elseif UnitGUID("target") == guid then
      unitid = "target"
   elseif UnitGUID("focus") == guid then
      unitid = "focus"
   end
  end
  debug("INSPECT_READY: "..(name or "nil").." "..(unitid or "nil"))
  if addon.LastNotify and addon.LastNotify ~= guid and unitid ~= "player" then
    debug("Ignoring INSPECT_READY overwritten by: "..(select(6,GetPlayerInfoByGUID(addon.LastNotify)) or "unknown"))
    return
  end
  if unitid then
    local grouped = UnitInParty(unitid) or UnitInRaid(unitid)
    if userf and settings.general.inspectcheck then
      addon:ShowReportFrame(userf, unitid)
    elseif grouped and addon.groupscanpos and addon.groupscanpos[unitid] and
           addon.groupscanexp and GetTime() < addon.groupscanexp then
      addon:groupscanbump(unitid)    
    elseif settings.general.inspectstealth and 
      (grouped or not settings.general.inspectstealthgroup) then
      addon:autocheck(unitid)
    end
  end
end
-------------------------------------------------------------------------------------
function addon:ADDON_LOADED(evt, loadname)
  if InspectFrame and not addon.ifhooked then
    addon.ifhooked = true
    local function ifhook() 
      if InspectFrame:IsVisible() and settings.general.inspectcheck and
         not (addon.reportframe and addon.reportframe:IsShown()) then
        addon:ShowReportFrame(InspectFrame, InspectFrame.unit)
      end
    end
    InspectFrame:HookScript("OnShow",ifhook)
    ifhook()
  end
end
-------------------------------------------------------------------------------------
function addon:ShowReportFrame(parent, unitid)
  if not addon.reportframe then
    local f = CreateFrame("Frame",addonName.."ReportWindow",UIParent,"BasicFrameTemplate")
    f:SetFrameStrata("TOOLTIP")
    f:SetAlpha(0.75)
    f:SetSize(400,400)
    f:EnableMouse(true)
    _G[addonName.."ReportWindow"].TitleText:SetText(addonName)
    local sendf = CreateFrame("Button",addonName.."ReportSend",f,"StaticPopupButtonTemplate")
    sendf:SetPoint("TOPLEFT", 20, -25)
    _G[sendf:GetName().."Text"]:SetText(L["Report"])
    sendf:SetScript("OnClick",function() addon:SendReport() end)
    local sendch = CreateFrame("Button",addonName.."ReportChannel",f,"StaticPopupButtonTemplate")
    addon:SetChannel(nil, settings.general.reportchannel)
    sendch:SetPoint("TOPLEFT", sendf, "TOPRIGHT", 10)
    sendch:SetScript("OnClick",function() addon:ShowDropdown() end)
    local rf = CreateFrame("ScrollingMessageFrame",addonName.."ReportText",f)
    rf:SetSize(400,400)
    rf:SetPoint("TOPLEFT",10,-50)
    rf:SetPoint("BOTTOMRIGHT",-10,-10)
    rf:SetFading(false)
    --rf:SetHyperlinkFormat("%s")
    --rf:SetHyperlinkFormat("\124H%s\124h%s\124h")
    rf:SetMaxLines(50)
    rf:SetJustifyH("LEFT")
    --rf:SetIndentedWordWrap(true)
    --rf:SetInsertMode("top")
    rf:SetFontObject(GameFontNormal)
    rf:SetScript("OnHyperlinkClick",function(self, link, text, button) SetItemRef(link, text, button, self) end);
    rf:SetScript("OnHyperlinkEnter",function(self, link, text) GameTooltip:SetOwner(self, "ANCHOR_CURSOR"); 
                                                     GameTooltip:SetHyperlink(link) 
						     GameTooltip:Show()
						     end);
    rf:SetScript("OnHyperlinkLeave",function(self, link, text) GameTooltip:Hide() end);
    rf:EnableMouse(true)
    rf:SetHyperlinksEnabled(true)
    rf:Show()
    f.text = rf
    f:SetScript("OnShow", function(self) -- handle the case when our parent shows us after INSPECT_READY has fired
      f.text:Clear()
      f.textlast = nil
      f.refreshcnt = 0
      f.unitid = f:GetParent().unit or "target"
      f.unitname = (f.unitid and GetUnitName(f.unitid, true)) or "unknown"
    end)
    f:SetScript("OnUpdate", function(self, elap)
      local thresh
      if f.refreshcnt < 2 then thresh = 0.5
      elseif f.refreshcnt < 10 then thresh = 1.0
      else thresh = 3.0
      end
      f.elap = (f.elap or 0) + elap
      if f.elap < thresh then return end
      f.elap = 0
      f.refreshcnt = f.refreshcnt + 1
      if UnitExists(f.unitid) and CanInspect(f.unitid) then
        addon:RunCheck(f.unitid)
      end
    end)
    addon.reportframe = f
  end
  local f = addon.reportframe 
  f.refreshcnt = 0
  f.unitid = unitid
  f.unitname = GetUnitName(unitid, true)
  f.text:Clear()
  f.textlast = nil
  f:SetParent(parent)
  f:ClearAllPoints()
  f:SetPoint("TOPLEFT",parent,"TOPRIGHT")
  f:Show()
end
-------------------------------------------------------------------------------------
addon.DropDownMenu = CreateFrame("Frame", addonName.."_DropDownMenu")
addon.DropDownMenu.displayMode = "MENU"
addon.DropDownMenu.onHide = function(...)
        MenuParent = nil
        MenuItem = nil
end

function addon.SetChannel(button,id)
  if id then
    settings.general.reportchannel = id
  else
    id = settings.general.reportchannel or 1
    settings.general.reportchannel = id
  end
  _G[addonName.."ReportChannel".."Text"]:SetText(">> "..addon.reportchannels[id].name)
end

function addon:SendReport()
  local send = addon.reportchannels[settings.general.reportchannel].send
  local report = addon.reportframe.textcache
  local prefix = "" -- addonName.." "..L["Addon"].." "
  report = strjoin("",strsplit("\n", report, 2)) -- strip header newline
  while (#report > 0) do
    local line
    line, report = strsplit("\n", report, 2)
    if line:match("^%s*$") then
      -- skip blank lines
    else
      line = prefix..line
      prefix = ""
      line = line:gsub("\124c%x%x%x%x%x%x%x%x([^\124]+)\124r","%1") -- strip color
      send(line)
    end
  end 
  send(L["This report was provided by the addon"].." "..addonName.." "..addon.version)
end

function addon:ShowDropdown()
        GameTooltip:Hide()
        HideDropDownMenu(1)
        ToggleDropDownMenu(1, nil, addon.DropDownMenu, "cursor", 0, 0)
end

local menuinfo = {}
addon.DropDownMenu.initialize = function(self, level)
    if not level then return end
    wipe(menuinfo)
    if level == 1 then
       menuinfo.isTitle      = nil
       menuinfo.notCheckable = 1
       for id,e in ipairs(addon.reportchannels) do
           menuinfo.text = e.name
           menuinfo.disabled = nil
           menuinfo.arg1 = id 
           menuinfo.checked = (settings.general.reportchannel == id)
           menuinfo.func = addon.SetChannel

           UIDropDownMenu_AddButton(menuinfo, level)
       end

       -- Close menu item
       menuinfo.disabled     = nil
       menuinfo.text         = CLOSE
       menuinfo.func         = function() CloseDropDownMenus() end
       menuinfo.checked      = nil
       menuinfo.notCheckable = 1
       UIDropDownMenu_AddButton(menuinfo, level)

    end
end

