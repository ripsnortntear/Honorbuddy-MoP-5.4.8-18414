local ZGV = ZygorGuidesViewer	if not ZGV then return end

local L = ZGV.L
local LM = ZGV.LM
local LI = ZGV.LI
local LC = ZGV.LC
local LQ = ZGV.LQ
local LS = ZGV.LS
local DL = ZGV.DL

function ZGV:Options_Initialize()
	--self.db:SetProfile("char/"..UnitName("player").." - "..GetRealmName())

	self:Options_DefineOptionTables()
	self:Options_RegisterDefaults()
	self:Options_SetupConfig()
	self:Options_SetupBlizConfig()
end

function ZGV:Options_SetFromMode()
	self.db.profile.dispmode = self.db.profile.dispmodepri and self.db.profile.dispprimary or self.db.profile.dispsecondary
	for k,kv in pairs(self.db.profile.dispmode) do self.db.profile[k]=kv end

	if self.db.profile.showcountsteps==0 then ZygorGuidesViewerFrame:SetHeight(self.db.profile.fullheight) end

	local hide = self.db.profile.hideborder
	self.borderfadedout = self.Frame.Border:GetAlpha()<0.1
	self.Frame.leftCount = hide and 999 or 0
	self.Frame.mouseCount = hide and 0 or 999

	self:UpdateFrame(true)
	self:AlignFrame()
	self:UpdateLocking()
	self:ScrollToCurrentStep()
end

function ZGV:Options_DefineOptionTables()
	local Getter_Simple = function(info)
		return self.db.profile[info[#info]]
	end
	local Setter_Simple = function(info,value)
		self.db.profile[info[#info]] = value
	end
	local Setter_Update = function(info,value)
		Setter_Simple(info,value)
		self:UpdateFrame()
	end
	local Getter_Sub = function(var,info)
		return self.db.profile[var][info[#info]]
	end
	local Setter_Sub = function(var,info,value)
		self.db.profile[var][info[#info]] = value
		self:Options_SetFromMode()
	end

	local order=1
	local target_stack={}
	local target_args
	local function AddOption(optname,optdata)
		optdata=optdata or {}
		order=order+1
		if optname=='' then optname=nil end
		optdata.name = optdata.name or (optname and L["opt_"..optname]) or ''
		optdata.desc = optdata.desc or (optname and rawget(L,"opt_"..optname.."_desc"))  -- force a nil when there's no description
		optdata.order = optdata.order or order
		target_args[(not target_args[optname]) and optname or "_"..order] = optdata
		return optdata
	end
	local function AddSubgroup(optname,optdata)
		optdata = AddOption(optname,optdata)
		optdata.type="group"
		optdata.childGroups = optdata.childGroups or "tab"
		if optdata.inline==nil then optdata.inline = true end
		optdata.args = {}
		tinsert(target_stack,target_args) --push
		target_args = optdata.args
	end
	local function EndSubgroup()
		target_args = tremove(target_stack) --pop
	end

	ZGV.optiontables_bliznames = {}

	local function AddOptionGroup(groupname,groupupname,slash,groupdata,no_header)
		groupdata = groupdata or {}
		groupdata.args = groupdata.args or {}
		groupdata.name = groupdata.name or L["opt_group_"..groupname]
		groupdata.desc = groupdata.desc or L["opt_group_"..groupname.."_desc"]
		groupdata.handler = self
		groupdata.get = Getter_Simple
		groupdata.set = Setter_Simple
		groupdata.type = "group"
		self.optiontables[groupname]=groupdata

		local blizname = "ZygorGuidesViewer"..(groupupname and ("-"..groupupname) or "")
		self.optiontables_bliznames[groupdata]=blizname

		tinsert(self.optiontables_ordered,{name=groupname,blizname=blizname,slash=slash})

		target_args = groupdata.args
		if not no_header then
			AddOption('',{ type = "description", name = groupdata.desc })
		end

		AddOption('',{type="description",cmdHidden=true,name=" "})
	end
	local function AddOptionSep(data)
		if not data then data={} end
		data.type="description"
		data.cmdHidden=true
		AddOption('',data)
	end

	self.optiontables = {}
	self.optiontables_ordered = {}

	AddOptionGroup("main","","zygor",{  name = L["name"].." |cffffff88- powered by |cffffff88G|cffffee66u|cffffdd44i|cffffcc22d|cffffbb00e|cffffaa00s|cffffbb00f|cffffcc22o|cffffdd44r.|cffffee66u|cffffff88s", desc = L["desc"],  },true)
	do
		AddOption('',{ type = "description", name = L["desc"]:format(self.version), })
		AddOption('',{ type = "header", name = L["opt_guidepacks"]:format(self.version), })
		AddOption('',{ type = "description", name = function()
			local s=""
			local myfac = select(2,UnitFactionGroup("player")):sub(1,1)
			if ZGV.guidesets['Achievements'..myfac] then s=s.."Achievement Guides".."\n" end
			if ZGV.guidesets['Dailies'..myfac..'WOTLK'] or ZGV.guidesets['Dailies'..myfac..'CATA'] then s=s.."Daily Guides".."\n" end
			if ZGV.guidesets['Dungeon'..myfac] then s=s.."Dungeon Guides".."\n" end
			if ZGV.guidesets['HunterPet'..myfac] then s=s.."Hunter's Pet Guides".."\n" end
			if ZGV.guidesets['Leveling'..myfac..'WOTLK'] or ZGV.guidesets['Leveling'..myfac..'CATA'] then s=s.."Leveling Guides".."\n" end
			if ZGV.guidesets['PetsMounts'..myfac] then s=s.."Pet and Mount Guides".."\n" end
			if ZGV.guidesets['Professions'..myfac] then s=s.."Profession Guides".."\n" end
			if ZGV.guidesets['Reputations'..myfac] then s=s.."Reputation Guides".."\n" end
			if ZGV.guidesets['Titles'..myfac] then s=s.."Title Guides".."\n" end
			s=s.."\n".."Be always up to date: |cffffff88http://j.mp/gfu_zygor".."\n"
			return s
		end})
		--AddOption('',{ type = "description", name = function() return ZGV.CurrentGuide and ZGV.CurrentGuide.title_short or "-none-" end, })
		AddOption('load',{ type = "execute", width="double", name = L['opt_loadguide'], func = function() InterfaceOptionsFrame:Hide()  ZGV:OpenGuideMenu() end, })
				--[[
		AddOption('guide',{
					type = "select",
					name = "",
					values = function() return ZGV:GetGuides() end,
					get = "GetCurrentGuideNum",
					set = function(info,i) self:SetGuide(i) self:FocusStep(1) end,
					width = "double",
				})
				--]]
		AddOption('steps',{  type = "description",  name = function() if not ZGV.CurrentGuide then return "" end  return L["opt_guide_steps"]:format(#ZGV.CurrentGuide.steps) end,  })
		AddOption('author',{  type = "description",  name = function() if not ZGV.CurrentGuide or not ZGV.CurrentGuide.author then return "" end  return L["opt_guide_author"]:format(ZGV.CurrentGuide.author) end,  })
		AddOption('next',{  type = "description",  name = function() if not ZGV.CurrentGuide or not ZGV.CurrentGuide.next then return "" end  return L["opt_guide_next"]:format(ZGV.CurrentGuide.next) end,  })

		AddOption('visible',{  type = 'toggle',  get = "IsVisible",  set = "SetVisible",  width = "full",  })
		AddOption('showmapbutton',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v)  self:UpdateMapButton()  end, })
		AddOptionSep({ type="description", name=" |n |n |n" })
		AddOption('report',{ type = 'execute', func = function() ZGV:BugReport() end, })

		-- hidden options
		AddOption('profiler',{
			hidden = true,
			type = 'toggle',
			get = function() return GetCVar("scriptProfile")=="1" end,
			set = ZGV.ProfilerEnable, -- Can only be enabled
		})
		AddOption('debug',{
			guiHidden = true,
			type = 'toggle',
			get = function() return self.db.profile.debug end,
			set = function() self.db.profile.debug = not self.db.profile.debug  ZGV:Print("Debugging: "..(self.db.profile.debug and "|cff00ff88ON|r" or "|cffff0055OFF|r")) end,
		})
		AddOption('debugflag',{
			hidden = true,
			type = 'execute',
			func = function(inp) inp = inp.input:sub(#inp[1]+2)  local f=self.db.profile.debug_flags[inp]  if f==nil then f=true end  self.db.profile.debug_flags[inp] = not f  ZGV:Print("Debug flag "..inp.." is now "..(self.db.profile.debug_flags[inp] and "ON" or "OFF"))  end,
		})

		AddOption('detectpet',{
			hidden = true,
			name = L['Detect mount/pet'],
			desc = L['Perform a mount/pet detection.'],
			type = 'execute',
			func = function() ZGV.CreatureDetector:Detect("force") end,
		})

		AddOption('way',{
			guiHidden=true,
			type="execute",
			func = function(params)
				local input=params.input:match("^way%s*(.-)%s*$")
				if not input then ZGV:Print("Error parsing command.") return end
				local map,x,y = input:match("^(.-)%s*([0-9%.]+)[ ,;:]+([0-9%.]+)%s*")
				if x and y then
					x=tonumber(x)
					y=tonumber(y)
				else
					ZGV:Print([[
Set a destination waypoint, using travel directions.
Usage: |cffffee88/zygor way |cffddff88<map> <xx,yy>|r
Examples:
    /zygor way 33,44   |cff888888(points within the current zone)|r
    /zygor way Stormwind City 61.9 75.1]])
					--ZGV:Print("    /zygor way Stormwind 12.3 56.7")
					--ZGV:Print("You can skip the map to use the current zone (or the zone in your map window). Coords can be in any format.")
					--ZGV:Print("Map can be English or in your language. Coords can be 12,34 or 12.2,34.4 or 12;34 or 12 34.")
					return
				end
				local mapid,floor
				if not map or map=="" then
					mapid=GetCurrentMapAreaID()
					floor=GetCurrentMapDungeonLevel()
				else
					local s
					s,floor=map:match("(.*)/(%d+)")
					floor=tonumber(floor)
					if s then map=s end
					local lmap=ZGV.BZR[map] or map
					mapid=ZGV.LibRover.data.MapIDsByName[lmap]
					if not mapid then ZGV:Print("Unknown map: "..map) return end
				end
				ZGV.Pointer:SetWaypoint(mapid,floor,x/100,y/100,{findpath=true,type=manual,cleartype=true})
			end,
		})
	end

	AddOptionGroup("display","Display","zgdisplay")
	do

		AddOption('skin',{
			type = "select",
			values = function()
				local t={}
				for id,skin in pairs(self.Skins) do  t[id]=skin.name  end
				return t
			end,
			set = function(_,n)
				self:SetSkin(n)
				self:ScrollToCurrentStep()
			      end,
			_default = "default", -- TODO make (default) tag autoappendable
		})
		AddOption('skinstyle',{
			type = "select",
			values = function()
				if not self.CurrentSkin then return {} end
				local t={}
				for id,style in pairs(self.CurrentSkin.styles) do
					t[id]=style.name
				end
				return t
			end,
			set = function(_,n)
				self:SetSkin(self.db.profile.skin,n)
				self:ScrollToCurrentStep()
			      end,
			hidden = function() return not self.CurrentSkin or not self.CurrentSkin.styles or not next(self.CurrentSkin.styles,next(self.CurrentSkin.styles)) end,
			_default = "stealth",
		})
		AddOptionSep()
		AddOption('opacitymain',{
			type = 'range',
			min = 0, max = 1.0, step = 0.01, bigStep = 0.1, isPercent = true,
			set = function(i,v) Setter_Simple(i,v)  self:AlignFrame() end,
			--stepBasis = 0,
			--width="double",
		})
		AddOption('framescale',{
			type = 'range',
			min = 0.5, max = 2.0, step = 0.1, bigStep = 0.1, isPercent = true,
			set = function(i,v) Setter_Simple(i,v) 	self.Frame:SetScale(ZGV.db.profile.framescale)  end,
		})
		AddOptionSep()
		AddOption('fontsize',{
			type = 'range',
			min = 7, max = 16, step = 1, bigStep = 1,
			set = function(i,v) Setter_Simple(i,v)  self:AlignFrame()  self:UpdateFrame()  end,
		})
		AddOption('fontsecsize',{
			type = 'range',
			min = 5, max = 14, step = 1, bigStep = 1,
			set = function(i,v) Setter_Simple(i,v)  self:AlignFrame()  self:UpdateFrame()  end,
		})
		AddOption('dispmodepri',{
			type = 'toggle',
			hidden=true,
			set = function(i,v)
				self.db.profile.dispmodepri = v
				self:Options_SetFromMode()
			      end,
		})
		--[[
		AddOption('backcolor',{
			type = 'color',
			hasAlpha = true,
			get = function()  return self.db.profile.backcolor.r,self.db.profile.backcolor.g,self.db.profile.backcolor.b,self.db.profile.backcolor.a  end,
		AddOption('	set = function(_,r,g,b,a)  self.db.profile.backcolor',{['r']=r,['g']=g,['b']=b,['a']=a}  self:UpdateSkin()  end,
		})
		AddOption('backopacity',{
			type = 'range',
			set = function(i,v) Setter_Simple(i,v)  self:UpdateSkin()  end,
			min=0.0, max=1.0, step = 0.01, bigStep = 0.1, isPercent = true,
			_default = 0.3,
		})
		--]]

		AddOptionSep()

		AddOption('windowlocked',{ type = 'toggle', set = function(i,v) Setter_Simple(i,v)  self:UpdateLocking()  end, })
		AddOption('resizeup',{
			type = 'toggle',
			set = function(i,v)
				Setter_Simple(i,v)
				self:ReanchorFrame()
				self:Debug("size up? "..tostring(self.db.profile.resizeup))
				--self.frameNeedsResizing = self.frameNeedsResizing + 1
				self:AlignFrame()
				-- THIS SUCKS.
			      end,
		})
		AddOption('hideincombat',{ type = 'toggle', _default = false, })

		AddOptionSep()

		AddSubgroup('prisecmodes')

			-- dual display modes now

			AddOption('',{type="description",name=L['opt_modes_desc']})

			AddSubgroup('primary',{
				width="single",
				get=function(info) return Getter_Sub("dispprimary",info) end,
				set=function(info,value) Setter_Sub("dispprimary",info,value) end,
			})
				AddOption('modeactive',{ type = "toggle", get = function() return self.db.profile.dispmodepri end,  set = function(i,v) if not v then return end self.db.profile.dispmodepri=v self:Options_SetFromMode() end, })
				AddOptionSep()
				AddOption('showcountsteps',{  type = "select",  values={  [0]=L["opt_showcountsteps_all"], [1]="1 (default)", [2]="2",[3]="3",[4]="4",[5]="5" } })
				AddOptionSep()
				AddOption('showbriefsteps',{ type = 'toggle', set=function(info,value) if not value then self.db.profile.dispprimary.hidecompletedinbrief=false end  Setter_Sub("dispprimary",info,value) end, })
				AddOption('hideborder',{ type = 'toggle', set=function(info,value) if not value then self.db.profile.dispprimary.nevershowborder=false end  Setter_Sub("dispprimary",info,value) end, })
				AddOptionSep()
				AddOption('hidecompletedinbrief',{ type = 'toggle',   disabled = function() return not self.db.profile.dispprimary.showbriefsteps end,  get = function(i) return self.db.profile.dispprimary.showbriefsteps and Getter_Sub("dispprimary",i) end, })
				AddOption('nevershowborder',{ type = 'toggle',  disabled = function() return not self.db.profile.dispprimary.hideborder end,  get = function(i) return self.db.profile.dispprimary.hideborder and Getter_Sub("dispprimary",i) end,  })
			EndSubgroup()

			AddSubgroup('secondary',{
				width="single",
				get=function(info) return Getter_Sub("dispsecondary",info) end,
				set=function(info,value) Setter_Sub("dispsecondary",info,value) end,
			})
				AddOption('modeactive',{ type = "toggle",  get = function() return not self.db.profile.dispmodepri end,  set = function(i,v) if not v then return end self.db.profile.dispmodepri=not v self:Options_SetFromMode() end,  })
				AddOptionSep()
				AddOption('showcountsteps',{  type = "select",  values={  [0]=L["opt_showcountsteps_all"], [1]="1 (default)", [2]="2",[3]="3",[4]="4",[5]="5" } })
				AddOptionSep()
				AddOption('showbriefsteps',{ type = 'toggle', set=function(info,value) if not value then self.db.profile.dispsecondary.hidecompletedinbrief=false end  Setter_Sub("dispsecondary",info,value) end, })
				AddOption('hideborder',{ type = 'toggle', set=function(info,value) if not value then self.db.profile.dispsecondary.nevershowborder=false end  Setter_Sub("dispsecondary",info,value) end, })
				AddOptionSep()
				AddOption('hidecompletedinbrief',{ type = 'toggle',   disabled = function() return not self.db.profile.dispsecondary.showbriefsteps end,  get = function(i) return self.db.profile.dispsecondary.showbriefsteps and Getter_Sub("dispsecondary",i) end, })
				AddOption('nevershowborder',{ type = 'toggle',  disabled = function() return not self.db.profile.dispsecondary.hideborder end,  get = function(i) return self.db.profile.dispsecondary.hideborder and Getter_Sub("dispsecondary",i) end,  })
			EndSubgroup()

		EndSubgroup()

		AddOption('briefopentime',{ type = 'range', min = 0.1, max = 2, step = 0.1, bigStep = 0.1, _default=0.5})
		AddOption('briefclosetime',{ type = 'range', min = 0.1, max = 5, step = 0.1, bigStep = 0.1, _default=1.0 })

		AddOptionSep()

		--[[
		AddOption('guidesinhistory',{
			type = 'range',
			min = 3, max = 15, step = 1, bigStep = 1,
			set = function(i,v) Setter_Simple(i,v)  while (#self.db.char.guides_history>v) do tremove(self.db.char.guides_history) end   end,
			_default = 5,
		})
		AddOptionSep()
		--]]

		AddOption('resetwindow',{
			type = 'execute',
			func = function()
				if self.Tutorial.Running then
					self.Tutorial:Close()
				end
				self.Frame:GetParent():ClearAllPoints()
				self.Frame:GetParent():SetPoint("CENTER")
				self:SetOption("Display","dispmodepri on")
				ZygorGuidesViewerMapIcon:ClearAllPoints()
				ZygorGuidesViewerMapIcon:SetPoint("CENTER",Minimap,"BOTTOMLEFT",16,16)
				self:UpdateFrame(true)
			end,
		})

		--[[
		-- no longer an option
		AddOption('trackchains',{
			type = 'toggle',
			width = "full",
		})
		AddOption('mapicons',{
			name = "Show map icons",
			desc = "Show icons on the world map",
			type = 'toggle',
			set = "ToggleShowingMapIcons",
			get = "IsShowingMapIcons",
		})
			},
		}
		--]]
	end

	AddOptionGroup('step',"Step","zgstep")
	do

		local function setrgb(colortable,r,g,b,a)
			if not colortable then return end

			colortable.r = r
			colortable.g = g
			colortable.b = b
			colortable.alpha = a

			self:UpdateFrame()
		end

		local function rgb2list (rgba)
			if not rgba then rgba={r=0,g=1,b=0,a=1} end
			return rgba.r,rgba.g,rgba.b,rgba.a
		end

		--AddOption('desc_mp',{ type="header", name=L["opt_modepri"], desc=L["opt_modes_desc"] },
		AddOption('showstepborders',{ type = 'toggle', _default = true, disabled=true })
		AddOption('stepbackalpha',{
			type = 'range',
			min=0.0, max=1.0, step = 0.1, bigStep = 0.1, isPercent = true,
-- 			disabled = function() return not self.db.profile.showstepborders end,
-- 			_default = 0.5,
			disabled = true,
			_default = 1.0,
		}) --[[TODOest TODO, this violates the Stealth's Skin very idea, talk to Sinus on that matter]]
		AddOptionSep({type="description",name="",order=3})

		AddOption('stepnumbers',{ type = 'toggle', _default = false })
		AddOption('goalicons',{ type = 'toggle', _default = true })
		AddOption('tooltipsbelow',{ type = 'toggle', _default = true, width = "double", })
		AddOption('goaltotals',{ type = 'toggle', _default = true,})
		--AddOption('goalcolorize',{ type = 'toggle', width = "double", _default = false,})

		AddOption('collapsecompleted',{ type = 'toggle', width = "full", })

		AddOption('',{ type="header", name=L["opt_goalbackcolor_desc"] })

		AddOption('goalbackgrounds',{ type = 'toggle', _default = true })
		AddOption('goalbackprogress',{
			type = 'toggle',
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			_default = false -- I think it was a bug setting this to false. ~aprotas --It was intended. ~Errc
		})

		AddOptionSep()
		AddOption('',{ type="description", width="double", name=L["opt_goalcolor_completion_desc"] })
		AddOption('',{ type="description", width="single", name=L["opt_goalcolor_other_desc"] })
		AddOptionSep()
		AddOption('goalbackincomplete',{
			type = 'color',
			width="double",
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return rgb2list(self.db.profile.goalbackincomplete)  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackincomplete={r=r,g=g,b=b,a=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default={r=0.65,g=0.08,b=0.10,a=0.7}
		})
		AddOption('goalbackimpossible',{
			type = 'color',
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return rgb2list(self.db.profile.goalbackimpossible)  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackimpossible={r=r,g=g,b=b,a=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default = {r=0.3,g=0.3,b=0.3,a=0.7}
		})
		AddOptionSep()
		AddOption('goalbackprogressing',{
			type = 'color',
			width="double",
			disabled = function()  return not self.db.profile.goalbackgrounds or not self.db.profile.goalbackprogress  end,
			get = function()  return rgb2list(self.db.profile.goalbackprogressing)  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackprogressing={r=r,g=g,b=b,a=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default={r=0.6,g=0.7,b=0.0,a=0.7}
		})
		AddOption('goalbackwarning',{
			type = 'color',
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return rgb2list(self.db.profile.goalbackwarning)  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackwarning={r=r,g=g,b=b,a=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default={r=0.5,g=0.0,b=0.8,a=0.7}
		})
		AddOptionSep()
		AddOption('goalbackcomplete',{
			type = 'color',
			width="double",
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return rgb2list(self.db.profile.goalbackcomplete)  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackcomplete={r=r,g=g,b=b,a=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default={r=0.2,g=0.7,b=0.0,a=0.7}
		})
		AddOption('goalbackaux',{
			type = 'color',
			hidden = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return self.db.profile.goalbackaux.r,self.db.profile.goalbackaux.g,self.db.profile.goalbackaux.b,self.db.profile.goalbackaux.a  end,
			set = function(_,r,g,b,a)  self.db.profile.goalbackaux={['r']=r,['g']=g,['b']=b,['a']=a}  self:UpdateFrame()  end,
			hasAlpha = true,
			_default = {r=0.0,g=0.5,b=0.8,a=0.5},
		})
		AddOptionSep()

		AddOption('',{ type="header", name=L["opt_flash_desc"] })

		AddOption('goalupdateflash',{
			type = 'toggle',
			disabled = function()  return not self.db.profile.goalbackgrounds  end,
			set = function(_,v)  Setter_Simple(_,v)  if v then self.db.profile.goalcompletionflash=true end  ZGV:TryToCompleteStep()  end,
			width = "single",
			_default = true,
		})
		AddOption('goalcompletionflash',{
			type = 'toggle',
			--hidden = function()  return not self.db.profile.goalbackgrounds  end,
			disabled = function()  return not self.db.profile.goalbackgrounds end,
			get = function()  return self.db.profile.goalcompletionflash --[[or self.db.profile.goalupdateflash--]]  end,
			set = function(_,v)  Setter_Simple(_,v)  if not v then self.db.profile.goalupdateflash=false end  ZGV:TryToCompleteStep()  end,
			width = "single",
			_default = true,
		})
		AddOption('flashborder',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v) if (v) then self.delayFlash=1 end  ZGV:TryToCompleteStep()  end,
			width = "single",
			_default = true,
		})
		AddOptionSep()

		AddOption('progress',{ type = 'toggle', width = "double", set = function(i,v) Setter_Simple(i,v) ZygorGuidesViewer_ProgressBar_SetUp() end, _default = true, })

	--[[	AddOption('progresscolor',{--Add color to this table
				type = 'color',
				hidden = function() return not self.db.profile.progress end,
				get = function()  return rgb2list(self.db.profile.progresscolor)  end,
				set = function(_,r,g,b,a) setrgb(self.db.profile.progresscolor, r, g, b, a) ZygorGuidesViewer_ProgressBar_SetUp() end,
				hasAlpha = true,
				_default={r=0.0,g=1.0,b=0.0,a=0.8}
			})--]]

		--[[
			AddOption('colorborder',{
				type = 'toggle',
				get = "IsColorBorder",
				set = "ToggleColorBorder",
				width="double",
			})
		--]]
	end

	do -- bye dynamic progress for now
		--[[
		self.optiontables.progress = {
			type = 'group',
			--hidden = true,
			handler = self,
			get = Getter_Simple,
			set = Setter_Simple,
			args = {
		AddOption('desc',{
			type = "description",
			name = L['opt_group_progress_desc'],
		})
		AddOption('skipobsolete',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end,
			width = "full",
		})
		AddOption('skipimpossible',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end,
			width = "full",
		})
		AddOption('skipauxsteps',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end,
			width = "full",
		})
		AddOption('levelsahead',{
			name = function() return L['opt_levelsahead']:format(self.db.profile.levelsahead) end,
			desc = L['opt_levelsahead_desc'],
			disabled = function()  return not self.db.profile.skipobsolete  end,
			type = 'range',
			min = 0,
			max = 80,
			step = 1,
			bigStep = 1,
			width="single",
			order = 4
		})
		AddOption('chainskip',{
			name = function ()
				if self.db.profile.chainskip then
					if self.db.profile.chainskipcount==21 then
						return L['opt_chainskip']:format(L['opt_chainskip_compl'])
					else
						return L['opt_chainskip']:format(L['opt_chainskip_count']:format(self.db.profile.chainskipcount))
					end
				else
					return L['opt_chainskip']:format("")
				end
			       end,
			desc = L['opt_chainskip_desc'],
			set = function(i,v)
				Setter_Simple(i,v)
				self:UpdateFrame()

				-- TODO: make this work.
				--self.optionpanels.progress.obj.children[1].children[7].lowtext:SetText("Early")
				--self.optionpanels.progress.obj.children[1].children[7].hightext:SetText("Never")
			      end,
			disabled = function()  return not self.db.profile.skipobsolete  end,
			type = 'toggle',
			width="full",
			order = 5.1
		})
		AddOption('chainskipcount',{
			name = L['opt_chainskipcount'],
			desc = L['opt_chainskipcount_desc'],
			type = 'range',
			min = 1,
			max = 21,
			step = 1,
			bigStep = 1,
			disabled = function()  return not self.db.profile.chainskip or not self.db.profile.skipobsolete  end,
			width="single",
			order = 5.2
		})

		AddOption('desc1',{ type="header", name=L["opt_progressbackcolor_desc"], order=11 },
		AddOption('goalbackaux',{
			type = 'color',
			hidden = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return self.db.profile.goalbackaux.r,self.db.profile.goalbackaux.g,self.db.profile.goalbackaux.b,self.db.profile.goalbackaux.a  end,
		AddOption('	set = function(_,r,g,b,a)  self.db.profile.goalbackaux',{['r']=r,['g']=g,['b']=b,['a']=a}  self:UpdateFrame()  end,
			hasAlpha = true,
		})
		AddOption('goalbackobsolete',{
			type = 'color',
			hidden = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return self.db.profile.goalbackobsolete.r,self.db.profile.goalbackobsolete.g,self.db.profile.goalbackobsolete.b,self.db.profile.goalbackobsolete.a  end,
		AddOption('	set = function(_,r,g,b,a)  self.db.profile.goalbackobsolete',{['r']=r,['g']=g,['b']=b,['a']=a}  self:UpdateFrame()  end,
			hasAlpha = true,
		})
		AddOption('desc2',{ type="description", name="", order=13 },
		AddOption('desc3',{
			type = "description",
			name = L['opt_group_progress_bottomdesc'],
		})
			}
		}
		--]]
	end

	AddOptionGroup('progress',"Progress","zgprogress")
	do
		AddOption('skipimpossible',{ type = 'toggle', set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end, width = "full", _default = true, })
		AddOption('skipflysteps',{ type = 'toggle', set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end, width = "full", })
		AddOption('dontprogress',{ type = 'toggle', width = "full", })
				--[[
		AddOption('skipauxsteps',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  self:UpdateFrame()  end,
			width = "full",
		})
		AddOption('levelsahead',{
			name = function() return L['opt_levelsahead']:format(self.db.profile.levelsahead) end,
			desc = L['opt_levelsahead_desc'],
			disabled = function()  return not self.db.profile.skipobsolete  end,
			type = 'range',
			min = 0,
			max = 80,
			step = 1,
			bigStep = 1,
			width="single",
			order = 4
		})
		AddOption('desc1',{ type="header", name=L["opt_progressbackcolor_desc"], order=11 },
		AddOption('goalbackaux',{
			type = 'color',
			hidden = function()  return not self.db.profile.goalbackgrounds  end,
			get = function()  return self.db.profile.goalbackaux.r,self.db.profile.goalbackaux.g,self.db.profile.goalbackaux.b,self.db.profile.goalbackaux.a  end,
		AddOption('	set = function(_,r,g,b,a)  self.db.profile.goalbackaux',{['r']=r,['g']=g,['b']=b,['a']=a}  self:UpdateFrame()  end,
			hasAlpha = true,
		})
		AddOption('desc2',{ type="description", name="", order=13 },
		AddOption('desc3',{
			type = "description",
			name = L['opt_group_progress_bottomdesc'],
		})
				--]]
	end

	do -- no gold guides for now
		--[[
		AddOptionGroup('gold',"Gold","zggold",{ hidden = not ZGV.AllianceGoldInstalled and not ZGV.HordeGoldInstalled, })

		AddOption('golddetectiondist',{
			name = L['opt_gold_detectiondist'],
			desc = L['opt_gold_detectiondist_desc'],
			type = 'range',
			min = 100,
			max = 3000,
			step = 1,
			bigStep = 1,
			set = function(i,v) Setter_Simple(i,v)  end,
			width = "double",
		})
		AddOption('goldreqmode',{
			name = L['opt_gold_reqmode'],
			desc = L['opt_gold_reqmode_desc'],
			type = "select",
			style = "radio",
		AddOption('	values',{
				[1]=L['opt_gold_reqmode_all'],
				[2]=L['opt_gold_reqmode_future'],
				[3]=L['opt_gold_reqmode_current'],
			})
			set = function(i,v) Setter_Simple(i,v)  ZGV:UpdateMapSpotVisibilities()  end,
			width = "double",
		})
			}
		}
		--]]
	end

	AddOptionGroup("arrow","Arrow","zgarrow", { })
	do
	  --[[
	  -- Unchecking this totally breaks Astrolabe-based waypointing. Silly Astro needs icons set to visible to calculate distances and bearings.
		AddOption('minicons',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v) 	self:SetWaypoint()  if self:IsWaypointAddonEnabled("cart2") then  Cartographer_Notes:MINIMAP_UPDATE_ZOOM()  Cartographer_Notes:UpdateMinimapIcons()  end end,
			disabled = function() return self.db.profile.waypointaddon=="none" end,
			width="single",
			_default = true,
		})
	  --]]

	  --[[
	  -- These two are removed because they were never supported in our icons. We can either make the icons support this setting... or remove it. For now it's removal.
		AddOption('iconalpha',{
			type = 'range',
			min = 0.1, max = 1, step = 0.01, bigStep = 0.05,
			isPercent = true,
			set = function(i,v) Setter_Simple(i,v) 	self:SetWaypoint()  if self:IsWaypointAddonEnabled("cart2") then  Cartographer_Notes:MINIMAP_UPDATE_ZOOM()  Cartographer_Notes:UpdateMinimapIcons()  end end,
			disabled = function() return not self.db.profile.minicons or (self.db.profile.waypointaddon~="cart2") end,
			_default = 1.0,
		})
		AddOption('iconscale',{
			type = 'range',
			min = 0.5, max = 2, step = 0.01, bigStep = 0.05,
			isPercent = true,
			set = function(i,v) Setter_Simple(i,v) 	self:SetWaypoint()  if self:IsWaypointAddonEnabled("cart2") then  Cartographer_Notes:MINIMAP_UPDATE_ZOOM()  Cartographer_Notes:UpdateMinimapIcons()  end end,
			disabled = function() return not self.db.profile.minicons or (self.db.profile.waypointaddon~="cart2") end,
			_default = .5,
		})

		AddOptionSep()
	  --]]

		AddOption('arrowshow',{  type = 'toggle', width="full", set = function(i,v) Setter_Simple(i,v)  self.Pointer:UpdateArrowVisibility() end, _default=true, })

		AddOption('hidearrowwithguide',{
			type = 'toggle',
			--disabled = function() return self.db.profile.waypointaddon=="none" end,
			width="full",
			_default = true,
		})

		AddOption('',{ type="header", name=L["opt_arrow_display"] })

		AddOption('arrowskin',{
			type = "select",
			values = function()
				local t={}
				for id,skin in pairs(self.Pointer.ArrowSkins) do  t[id]=skin.name  end
				return t
			end,
			set = function(_,n)
				self.Pointer:SetArrowSkin(n)
			      end,
			_default = "stealth",
		})
		AddOptionSep()


		AddOption('arrowfreeze',{ type = 'toggle', set = function(i,v) Setter_Simple(i,v)  self.Pointer:SetupArrow() end, width = "single", _default=false, })
		AddOption('arrowsmooth',{  type = 'toggle', disabled = function() return not ZGV.Pointer.CurrentArrowSkin.features['smooth'] end,  width = "single", _default=true, })
		AddOption('arrowcolordist',{ type = 'toggle',  disabled = function() return not ZGV.Pointer.CurrentArrowSkin.features['colordist'] end,  width = "single",  _default = false,  })
		AddOptionSep()

		AddOption('arrowalpha',{
			type = 'range',
			set = function(i,v) Setter_Simple(i,v) 	ZGV.Pointer:SetupArrow()  end,
			min = 0.3, max = 1.0, step = 0.1, bigStep = 0.1, isPercent = true,
			_default = 1.0,
		})
		AddOption('arrowscale',{
			type = 'range',
			set = function(i,v) Setter_Simple(i,v) 	ZGV.Pointer:SetupArrow()  end,
			min = 0.5, max = 2.0, step = 0.1, bigStep = 0.1, isPercent = true,
			_default = 1.0,
		})
		AddOption('arrowfontsize',{
			type = 'range',
			min = 5, max = 15, step = 0.5, bigStep = 1.0,
			set = function(i,v) Setter_Simple(i,v)  ZGV.Pointer:SetFontSize(v)  end,
			_default = 10,
		})

		AddOptionSep()
		AddOption('arrowmeters',{ type = 'toggle', width = "full", _default=false, })

		AddOption('',{ type="header", name=L["opt_arrow_extras"] })

		AddOption('corpsearrow',{
			type = 'toggle',
			disabled = function() return self.db.profile.waypointaddon=="none" end,
			_default = true,
		})
		AddOption('corpsearrowjokes',{
			type = 'toggle',
			disabled = function() return not self.db.profile.corpsearrow or self.db.profile.waypointaddon=="none" end,
			_default = true,
		})
		AddOption('minimapzoom',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v)  self.Pointer:MinimapZoomChanged() end, _default = false, })
		AddOption('audiocues',{ type = 'toggle', width = "full", _default = false, })

		AddOption('waypoints',{
			type = 'select',
			values={
				[2]=L["opt_group_addons_internal"],
				--[3]=(ZGV.WaypointFunctions['cart2']:isready() and "" or "|cff888888") .. L["opt_group_addons_cart2"],
				[4]=(ZGV.WaypointFunctions['carbonite']:isready() and "" or "|cff888888") .. L["opt_group_addons_carbonite"],
				--[5]=(ZGV.WaypointFunctions['tomtom']:isready() and "" or "|cff888888") .. L["opt_group_addons_tomtom"],
				--cart3=L["opt_group_addons_cart3"],
				--metamap=L["opt_group_addons_metamap"],
			},
			get = "GetWaypointAddon",
			set = "SetWaypointAddon",
		})
		AddOptionSep()
	end

	AddOptionGroup("travelsystem","Travelsystem","zgtravelsystem")
	do

		AddOption('pathfinding',{
			hidden = function() return LibRover.is_stub end, -- and not ZGV.guidesets['PetsMountsA'] and not ZGV.guidesets['PetsMountsH'],
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  LibRover.updating=v  self.Pointer.TempWaypath=nil  self:SetWaypoint() end,
			width="full",
			_default = true,
		})
		AddOptionSep()

		AddOption('pathfinding_mode',{
			type = 'select',
			--style = 'radio',
			values={
				["1fastest"]="Fastest, use every trick",
				["2nocd"]="Fast, without items",
				["3lazy"]="Lazy, prefer flights",
			},
			width = "single",
			_default="1fastest",
			disabled=function() return not self.db.profile.pathfinding end,
			set = function(i,v)
				Setter_Simple(i,v)
				LibRover:UpdateConfig()
				LibRover:UpdateNow()
			end,
		})
		AddOptionSep()

		AddOption('pathfinding_speed',{
			type = 'select', values={ [0.001]=L["opt_pathfinding_speed_slow"], [15]=L["opt_pathfinding_speed_medium"], [50]=L["opt_pathfinding_speed_fast"] },
			--type = 'range', min = 0.01, max = 150, step = 0.1, bigStep = 0.1,
			_default = 15
		})

		--[[
		AddOption('desc2',{
			type = "description",
			name = L['opt_pathfinding_subdesc']:format(ZGV.LibRover.update_interval),
		})
		--]]

		local function rgb2list (savedcolors)
			if not savedcolors then return end
			return savedcolors.r,savedcolors.g,savedcolors.b,savedcolors.alpha
		end
		local function rgbalpha2rgba (rgbalpha)
			return {r=rgbalpha.r,g=rgbalpha.g,b=rgbalpha.b,a=rgbalpha.alpha}
		end
		local function rgba2rgbalpha (rgba)
			return {r=rgba.r,g=rgba.g,b=rgba.b,alpha=rgba.a}
		end

		-- set r,g,b,alpha on a table using another table or a quad of values.
		local function setrgb(colortable,r,g,b,a)
			if not colortable then return end
			if type(r)=="table" then
				local rgbalpha=r
				colortable.r,colortable.g,colortable.b,colortable.a,colortable.alpha = rgbalpha.r,rgbalpha.g,rgbalpha.b,rgbalpha.a,rgbalpha.alpha
			else
				colortable.r,colortable.g,colortable.b,colortable.a,colortable.alpha = r,g,b,a,a
			end
		end

		AddSubgroup('ants',{width='single'})

			AddOption('antspacing',{
				type = 'select',
				disabled = function() return self.db.profile.waypointaddon~="internal" and self.db.profile.waypointaddon~="tomtom" end,
				values={ [0]=L["opt_antspacing_0"], [50]=L["opt_antspacing_yd"]:format(50), [100]=L["opt_antspacing_yd_def"]:format(100), [200]=L["opt_antspacing_yd"]:format(200), [300]=L["opt_antspacing_yd"]:format(300) },
				set = function(i,v) Setter_Simple(i,v)  self.Pointer:SetAntSpacing(v) self:SetWaypoint() end,
				_default = 100
			})

			AddOption('antspeed',{
				type = 'select',
				disabled = function() return self.db.profile.waypointaddon~="internal" and self.db.profile.waypointaddon~="tomtom" end,
				values={ [1]=L["opt_antspeed_vslow"], [5]=L["opt_antspeed_slow"]:format(5), [10]=L["opt_antspeed_normal"]:format(10), [30]=L["opt_antspeed_fast"]:format(30), [999]=L["opt_antspeed_full"] },
				_default = 10
			})

			AddOptionSep()

			--local antcolor_disabled = function()  return not self.db.profile.customcolorants or self.db.profile.singlecolorants  end
			local antcolor_disabled = function()  return false  end
			--local antcolor_hidden = function()  return ZGV.optiontables.travelsystem.args.ants.args.customcolorants:hidden() or self.db.profile.singlecolorants  end
			local antcolor_hidden = function()  return  self.db.profile.singlecolorants  end

			AddOption('colorantsother',{--Add color to this table
				type = 'color',
				width="half",
				disabled = antcolor_disabled,
				hidden = antcolor_hidden,
				get = function()  return rgb2list(antcolor_disabled() and rgbalpha2rgba(ZGV.Pointer.Icons.ant_default) or self.db.profile.colorantsother)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.colorantsother, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_default)
			})

			AddOption('colorantsfly',{
				type = 'color',
				width="half",
				disabled = antcolor_disabled,
				hidden = antcolor_hidden,
				get = function()  return rgb2list(antcolor_disabled() and rgbalpha2rgba(ZGV.Pointer.Icons.ant_y_default) or self.db.profile.colorantsfly)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.colorantsfly, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_y_default)
			})

			AddOption('colorantstaxi',{
				type = 'color',
				width="half",
				disabled = antcolor_disabled,
				hidden = antcolor_hidden,
				get = function()  return rgb2list(antcolor_disabled() and rgbalpha2rgba(ZGV.Pointer.Icons.ant_g_default) or self.db.profile.colorantstaxi)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.colorantstaxi, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_g_default)
			})

			AddOption('colorantsship',{
				type = 'color',
				width="half",
				disabled = antcolor_disabled,
				hidden = antcolor_hidden,
				get = function()  return rgb2list(antcolor_disabled() and rgbalpha2rgba(ZGV.Pointer.Icons.ant_b_default) or self.db.profile.colorantsship)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.colorantsship, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_b_default)
			})

			AddOption('colorantsportal',{
				type = 'color',
				width="half",
				disabled = antcolor_disabled,
				hidden = antcolor_hidden,
				get = function()  return rgb2list(antcolor_disabled() and rgbalpha2rgba(ZGV.Pointer.Icons.ant_p_default) or self.db.profile.colorantsportal)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.colorantsportal, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_p_default)
			})

			AddOption('singlecolorantscolor',{
				type = 'color',
				--hidden = function()  return ZGV.optiontables.travelsystem.args.ants.args.customcolorants:hidden() or not self.db.profile.singlecolorants or not self.db.profile.customcolorants  end,
				hidden = function()  return  not self.db.profile.singlecolorants   end,
				get = function()  return rgb2list(self.db.profile.singlecolorantscolor)  end,
				set = function(_,r,g,b,a)
					setrgb(self.db.profile.singlecolorantscolor, r, g, b, a)
					ZGV.Pointer.Icons:SetAntColorsFromOptions()
				end,
				hasAlpha = true,
				_default=rgbalpha2rgba(ZGV.Pointer.Icons.ant_default)
			})

			--AddOption('customcolorants',{ type = 'toggle', width="full", hidden = function() return self.db.profile.antspacing==0 end, set = function(i,v) Setter_Simple(i,v)  ZGV.Pointer.Icons:SetAntColorsFromOptions()  end})
			AddOption('singlecolorants',{ type = 'toggle', width="full", set = function(i,v) Setter_Simple(i,v)  ZGV.Pointer.Icons:SetAntColorsFromOptions()  end })

			AddOptionSep()

			AddOption('desc',{
				type = "description",
				name = "You can press [Defaults] below to revert to default colors.",
			})

		EndSubgroup()

	end

	--[[ map coords?
		AddOption('mapcoords',{
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  self.MapCoords:HandleWorldmapCoords() end,
			width = "full",
		})
	--]]

	 --[[ --magic key
		self.optiontables['magickey'] = {
			type = 'group',
			--hidden = true,
			handler = self,
			get = Getter_Simple,
			set = Setter_Simple,
			args = {
		AddOption('desc',{
			type = "description",
			name = L['opt_group_magickey_desc'],
		})
		AddOption('magickey',{
			type = 'keybinding',
			width = "single",
			set = function(i,v) if v=="" then SetBinding(self.db.profile.magickey) end  Setter_Simple(i,v)  ZGV:SetMagicKey(true)  end,
			_default = "",
		})
		AddOptionSep({ type="description", name=L['opt_magickey_actionsdesc'], order=1.02 },
		AddOption('magickey_target',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_talk',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_acceptturnin',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_attack',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_targetcorpse',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_loot',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_itemspell',{
			type = 'toggle',
			width = "full",
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
		AddOption('magickey_hint',{
			type = 'toggle',
			width = "full",
			set = function(i,v) Setter_Simple(i,v)  self.MagicKey.FR:ShowIf(v) end,
			disabled = function() return not self.db.profile.magickey or self.db.profile.magickey=="" end,
			_default = true,
		})
			}
		}
	--]]

	AddOptionGroup("gear","Gear","zggear")
	do
		AddOptionSep()
		AddOption('autogear',{ type = 'toggle',width="full", _default=true, set = function(i,v) Setter_Simple(i,v)  ZGV.ItemScore.AutoEquip:ToggleButton() end, descStyle="inline"})
		AddOption('autogearauto',{ type='toggle', width="full", _default=false,
			disabled=function() return not self.db.profile.autogear end,
			descStyle="inline",
			indent=15})
		AddOption('clearnotupgrades',{
			type = 'execute',
			func=function ()
				wipe(ZGV.db.profile.badupgrade[GetSpecialization() or 1])
				ZGV:Print(L['itemscore_ae_clearednotupgrade'])
			     end,
			 width='single',
			 disabled=function() return not self.db.profile.autogear end,
		})
		AddOption('itemBug', { guiHidden=true, type = 'execute', desc="Generate a bug report for item system with a profiling.",
		func=function(info,val)
			local s = "--==Verbose==--\n"..ZGV.ItemScore.AutoEquip:ShowGearReport(1,1,1).."\n\n--==Regular==--\n"..ZGV.ItemScore.AutoEquip:ShowGearReport(nil,1,1)
			ZGV:ShowDump(s,"Zygor Gear Bug Reportx2")
		end
		})
	end

	AddOptionGroup("modelviewer","ModelViewer","zgmv")
	do
		AddOption('mv_enabled',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v)  self:TryToDisplayCreature() end, _default = true, descStyle="inline", })
		AddOption('mv_rotation',{ type = 'toggle', width = "full", disabled = function() return not self.db.profile.mv_enabled end, _default = true, descStyle="inline", })
		AddOption('mv_slideshow',{ type = 'toggle', width = "full", disabled = function() return not self.db.profile.mv_enabled end, _default = true, descStyle="inline", })
		AddOption('mv_reset',{ type = 'execute', width = "single", disabled = function() return not self.db.profile.mv_enabled end, func=function() ZGV.CV:AlignFrame() end, descStyle="inline", })
	end

	AddOptionGroup("slidebar","SlideBar","zgsb")
	do
		AddOption('sb_enabled',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) ZGV.SlideBar:ApplyLayout() end, _default = true, })
		AddOption('sb_locked',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) end, _default = false, })
	end

	AddOptionGroup("notification","Notification","zgnc")
	do
		if ZGV.DEV then
		AddOption('',{ type="header", name=L["opt_n_nc"] })
			AddOption('n_nc_no_popups',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) end, _default = false, })
			AddOption('n_nc_hide_text',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) ZGV.NotificationCenter.ApplyLayout() end, _default = false, })
			AddOption('n_nc_enabled',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) ZGV.NotificationCenter.ApplyLayout() end, _default = true, })
			AddOption('n_nc_locked',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) end, _default = false, })
			AddOption('n_nc_alwaysshow',{ type = 'toggle', width = "full", set = function(i,v)
				Setter_Simple(i,v)
				--if ZGV.db.profile.n_nc_alwaysshow then
				--	ZGV.NotificationCenter:Capture()
				--else
				--	ZGV.NotificationCenter:Release()
				--end

				ZGV.NotificationCenter:ApplyLayout()
			end, _default = true, })

			AddOption('n_nc_numpetguides',{
				type = 'range',
				min = 1, max = 15, step = 0.5, bigStep = 1.0,
				set = function(i,v) Setter_Simple(i,v)  ZGV.CreatureDetector.MAX_DETECTED_GUIDES = v;  end,
				_default = 5,
			})
		end
		AddOption('',{ type="header", name=L["opt_n_popups"] })
			AddOption('n_popup_hideall',{ type = 'toggle', width = "full", _default = false,  descStyle="inline", })
			AddOption('n_popup_guides',{ type = 'toggle', width = "full", _default = true,  descStyle="inline", })
			AddOption('n_popup_sis',{ type = 'toggle', width = "full", _default = true, descStyle="inline", disabled=function() return not self.db.profile.n_popup_guides end, indent=15 })
			AddOption('n_popup_dungeon',{ type = 'toggle', width = "full", _default = true, descStyle="inline", disabled=function() return not self.db.profile.n_popup_guides end,  indent=15 })
			AddOption('n_popup_monk',{ type = 'toggle', width = "full", _default = true, descStyle="inline", disabled=function() return not (select(3,UnitClass("player"))==10 and self.db.profile.n_popup_guides) end,--[[ hidden=function() return select(3,UnitClass("player"))~=10 end,--]] indent=15 })

	end

	AddOptionGroup("petbattle","PetBattle","zgpetb")
	do
		AddOption('petbattleframe',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v) end, _default = true, })
	end

	AddOptionGroup("talentsystem","Talentsystem","zgtalentsystem")
	do
		AddOption('talenton',{ type = 'toggle', width="full", set = function(i,v)
			Setter_Simple(i,v)
			ZTA:Startup()

			if ZGV.db.profile.talenton then
				PlayerTalentFrame.advisorbutton:Show()
				ZTA:GetUserBuild()
				ZTA:CompareSpec()
			else
				PlayerTalentFrame.advisorbutton:Hide()
				ZygorTalentAdvisorPopout_Hide()
				ResetAllTalentIcons()
				PlayerTalentFrame.scriptedtoflash=false
			end

			end, })
		AddOption('talenticon', {type = 'toggle',  width="full", disabled= function() return not self.db.profile.talenton end, set = function(i,v)
			Setter_Simple(i,v)
			if ZGV.db.profile.talenticon then
				ZTA:CompareSpec()
			else
				ResetAllTalentIcons()
				PlayerTalentFrame.scriptedtoflash=false
			end

			end, })
		AddOption('talentpopup',{
				type = "select",
				style = "radio",
				width = "double",
				--get inherited simple
				--set inherited simple
				values = {[0]=L['opt_talentpopup_0'],L['opt_talentpopup_1'],L['opt_talentpopup_2'],L['opt_talentpopup_3']},
				disabled= function() return not self.db.profile.talenton end,
			})
	end

	AddOptionGroup("conv","Conv","zgconv")
	do
		AddSubgroup('autoquest',{width='triple'})
			AddOption('autoaccept',{ type = 'toggle', name=function() return L['opt_autoaccept'..(ZGV.db.profile.autoacceptturninall and "_all" or "")] end, desc=function() return L['opt_autoaccept_desc'] end, })
			AddOption('autoturnin',{ type = 'toggle', name=function() return L['opt_autoturnin'..(ZGV.db.profile.autoacceptturninall and "_all" or "")] end, desc=function() return L['opt_autoturnin_desc'] end, })
			--AddOptionSep()
			--[[
			local function make_accept_turnin_mnemonic()
				local s=""
				if ZGV.db.profile.autoaccept then s="accept" end
				if ZGV.db.profile.autoturnin then s=s.."turnin" end
				return s
			end
			AddOption('autoacceptturninall',{
				name=function() return L['opt_autoacceptturninall_'..make_accept_turnin_mnemonic()] end,
				desc=function() return L['opt_autoacceptturninall_'..make_accept_turnin_mnemonic().."_desc"] end,
				type = 'toggle',
				width="full",
				disabled=function() return not self.db.profile.autoaccept and not self.db.profile.autoturnin end
			})
			--]]
			AddOption('autoacceptturninall',{
				name=L['opt_autoacceptturninall_'],
				desc=L['opt_autoacceptturninall__desc'],
				type = 'toggle',
				_default=false
			})
			AddOptionSep()
			AddOption('autoacceptshowobjective',{ type = 'toggle', width="full", disabled=function() return not self.db.profile.autoaccept end })
			AddOptionSep()
			AddOption('questitemselector',{ type = 'toggle', width="full"})
			AddOption('autoselectitem',{ type = 'toggle', disabled = function() return not (self.db.profile.autoturnin and self.db.profile.questitemselector) end, width="full"})
			AddOptionSep()
			AddOption('fixblizzardautoaccept',{ type = 'toggle', width = "full", })
		EndSubgroup()

		--AddSubgroup('item',{width='double'})
		--	AddOption('',{type="description",name=L['opt_item_desc']})

		AddSubgroup('vendor',{width='double'})
			AddOption('showgreyvalue',{type = 'toggle', set = function(i,v) Setter_Simple(i,v)  ZGV.Loot:ToggleFrame() end})
			AddOption('autobuy',{ type = 'toggle',_default=true})
			AddOptionSep()
			AddOption('autosell',{ type = 'toggle', _default=false,})
			AddOption('autobuyframe',{ type='toggle',_default=true, disabled=function() return not self.db.profile.autobuy end, indent=15})
			AddOption('showgreysellbutton',{ type = 'toggle', width="full", _default=true})
		EndSubgroup()

		--EndSubgroup()

		AddOption('analyzereps',{ type = 'toggle', width = "full", })
		AddOption('autotaxi',{ type = 'toggle', width = "full", })
		AddOption('detectcreatures',{ type = 'toggle', width = "full", hidden = not ZGV.guidesets['PetsMountsA'] and not ZGV.guidesets['PetsMountsH'], _default=true })
		--AddOptionSep()
		AddOption('silentmode',{ type = 'toggle', width = "full", })

		AddOption('foglight',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v)  if v then self.Foglight:Startup() else self.Foglight:TurnOff() end end, _default = true, })

		AddOption('flashmapnodes',{ type = 'toggle', width = "full", set = function(i,v) Setter_Simple(i,v)  if (not v) then self.Pointer:MinimapNodeFlashOff() end end, _default = true, })

		AddOption('autotrackquests',{ type = 'toggle', width = "full", _default = false, hidden = function() return not ZGV.db.profile.debug end, })

		AddOption('foglightdebug',{
			name = "(Debug) Check fog",
			desc = "Check foglighting for the current map",
			type = 'execute',
			func = function() ZGV.Foglight:DebugMap() end,
			hidden = function() return not ZGV.db.profile.debug end,
		})
		AddOption('foglightdump',{
			name = "(Debug) Dump fog",
			desc = "Dump foglighting for current map (ctrl: all maps) (shift: just differences)",
			type = 'execute',
			func = function() ZGV.Foglight:DumpMapOverlayInfos(IsShiftKeyDown(),IsControlKeyDown()) end,
			hidden = function() return not ZGV.db.profile.debug end,
		})

		--[[  --tweaks
			AddOptionSep()

			AddOption('tweaks_desc',{
				type = "description",
			})
			AddOption('tweaks_domacros',{
				type = 'toggle',
				width = "full",
			})
		--]]
	end

	if ZGV.db.profile.debug then
	AddOptionGroup("debug","Debug","zgdebug", { hidden = function() return not self.db.profile.debug end, })
	do
		AddOption('fakelevel',{
			name = "Fake level (0=disable)",
			width="full",
			type = 'range', min = 0, max = 90, step = 0.2, bigStep = 0.2,
			get = function(i,v) return self.db.char[i[#i]] end,
			set = function(i,v) self.db.char[i[#i]]=v end,
		})

		AddOption('fakecombat',{
			name = "Fake combat mode",
			desc = "Check to simulate combat mode, for testing of 'delay after combat' and similar situations.",
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v)  if (v and not UnitAffectingCombat("player")) or (not v and UnitAffectingCombat("player")) then self:PLAYER_REGEN_DISABLED() else self:PLAYER_REGEN_ENABLED() end  end,
		})
		AddOptionSep()

		local skills={"Blacksmithing","Tailoring", "Leatherworking", "Inscription",  "Jewelcrafting",  "Mining",  "Herbalism",  "Enchanting",  "Engineering",  "Alchemy",  "Skinning",  "Fishing",  "Cooking", "Way of the Grill", "Way of the Wok", "Way of the Pot", "Way of the Steamer", "Way of the Oven", "Way of the Brew", "First Aid",  "Archaeology" }
		local skillvalues={}  for i,v in ipairs(skills) do skillvalues[v]=v end
		AddOption('fakeskill',{
			name = "Fake profession",
			desc = "Check to simulate skill levels.",
			type = 'select',
			values=skillvalues,
			set = function(i,v)
				Setter_Simple(i,v)

				local fs = ZGV.db.profile.fakeskills[v]

				self:SetOption("Debug","fakeskillcheck " .. (fs and "on" or "off"))

				if fs then
					local fsl = ZGV.optiontables.debug.args.fakeskilllevel
					if not fsl then return end
					fsl.min=max(0,tonumber(fs and fs.max or 0)-100)
					fsl.max=tonumber(fs and fs.max or 0)
					self:SetOption("Debug","fakeskilllevel ".. (fs and fs.level or 0))
					self:SetOption("Debug","fakeskillmax ".. (fs and fs.max or 0))
				end
			      end,
			_default = "Alchemy"
		})
		AddOption('fakeskillcheck',{
			name = "Fake",
			desc = "",
			type = 'toggle',
			set = function(i,v)
				Setter_Simple(i,v)
				if v then
					if not ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill] then
						self:SetOption("Debug","fakeskilllevel 0")
						self:SetOption("Debug","fakeskillmax 75")
					end
				else
					ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]=nil
				end
			      end,
			get = function(i)
				local skill = ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]
				return not not skill
			      end,
			width = "half",
		})
		AddOption('fakeskilllevel',{
			name = "Skill",
			desc = "Skill level.",
			type = 'range',
			min = 0,
			max = 600,
			step = 1,
			bigStep = 1,
			set = function(i,v)
				Setter_Simple(i,v)
				if ZGV.db.profile.fakeskillcheck then
					local skill = ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill] or {active=true,level=0,max=0}
					skill.level = tonumber(v)
					ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]=skill
				end
			      end,
			get = function(i)
				local skill = ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]
				return skill and skill.level or 0
			      end,
			disabled = function() return not ZGV.db.profile.fakeskillcheck end,
			width="half",
			_default = 0,
		})
		AddOption('fakeskillmax',{
			name = "Skill max",
			desc = "Max skill level.",
			type = 'select',
			values={
				[0]="none",
				[75]="75 Apprentice",
				[150]="150 Journeyman",
				[225]="225 Expert",
				[300]="300 Artisan",
				[375]="375 Master",
				[450]="450 Grand Master",
				[525]="525 Illustrious G. M.",
				[600]="600 Zen Master",
			},
			set = function(i,v)
				Setter_Simple(i,v)
				if ZGV.db.profile.fakeskillcheck then
					local skill = ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill] or {active=true,level=0,max=0}
					skill.max = tonumber(v)
					skill.level = min(skill.max,max(0,skill.level,skill.max-100))
					ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]=skill

					local fsl = ZGV.optiontables.debug.args.fakeskilllevel
					if not fsl then return end
					fsl.min=max(0,tonumber(skill.max)-100)
					fsl.max=tonumber(skill.max)
					self:SetOption("Debug","fakeskilllevel ".. skill.level)
				end
			end,
			get = function(i)
				local skill = ZGV.db.profile.fakeskills[ZGV.db.profile.fakeskill]
				return skill and skill.max or 0
			      end,
			disabled = function() return not ZGV.db.profile.fakeskillcheck end,
			--width=140,
			_default = 0,
		})
		AddOption('fakeskilllist',{
			name = function()
				local s = "Faked skills:"
				for fsn,fs in pairs(ZGV.db.profile.fakeskills) do
					s = s .. "\n" .. fsn .." = " .. fs.level .. "/" .. fs.max
				end
				if not next(ZGV.db.profile.fakeskills) then s = s .. "  none" end
				return s
			       end,
			desc = "",
			type = "description",
			width = "full",
		})
		AddOption('fakeskillclear',{
			type = "execute",
			name = "Clear all",
			func = function()
				ZGV.db.profile.fakeskills={}
				self:SetOption("Debug","fakeskillcheck off")
				self:SetOption("Debug","fakeskilllevel ".. ZGV.db.profile.fakeskilllevel)
			       end,
			order = 3.31
		})
		AddOptionSep()
		AddOption('fakerep',{
			name = "Fake reputation",
			desc = "Simulate reputations.",
			type = 'select',
			values = function() return self.factions_mentioned end,
			set = function(i,v)
				Setter_Simple(i,v)

				local fr = ZGV.db.profile.fakereps[v]

				self:SetOption("Debug","fakerepcheck " .. (fr and "on" or "off"))

				if fr then
					self:SetOption("Debug","fakerepstanding ".. (fr and fr.standing or 4))
				end
			      end,
			_default = "",
		})
		AddOption('fakerepcheck',{
			name = "Fake",
			desc = "",
			type = 'toggle',
			set = function(i,v)
				Setter_Simple(i,v)
				if v then
					if not ZGV.db.profile.fakereps[ZGV.db.profile.fakerep] then
						self:SetOption("Debug","fakerepstanding 4")
					end
				else
					ZGV.db.profile.fakereps[ZGV.db.profile.fakerep]=nil
				end
			      end,
			get = function(i)
				local rep = ZGV.db.profile.fakereps[ZGV.db.profile.fakerep]
				return not not rep
			      end,
			width = "half",
			_default = false,
		})
		AddOption('fakerepstanding',{
			name = "Standing",
			desc = "Pick your rep level.",
			type = 'select',
			values = function() return self.StandingNamesEng end,
			set = function(i,v)
				Setter_Simple(i,v)
				if ZGV.db.profile.fakerepcheck then
					ZGV.db.profile.fakereps[ZGV.db.profile.fakerep]=v
				end
			      end,
			get = function(i)
				return ZGV.db.profile.fakereps[ZGV.db.profile.fakerep]
			      end,
			disabled = function() return not ZGV.db.profile.fakerepcheck end,
			_default = 4,
		})
		AddOption('fakereplist',{
			name = function()
				local s = "Faked reputations:"
				for frn,fr in pairs(ZGV.db.profile.fakereps) do
					s = s .. "\n" .. frn .." = " .. self.StandingNamesEng[fr]
				end
				if not next(ZGV.db.profile.fakereps) then s = s .. "  none" end
				return s
			       end,
			desc = "",
			type = "description",
			width = "full",
		})
		AddOption('fakerepclear',{
			type = "execute",
			name = "Clear all",
			func = function()
				ZGV.db.profile.fakereps={}
				self:SetOption("Debug","fakerepcheck off")
				self:SetOption("Debug","fakerepstanding ".. ZGV.db.profile.fakerepstanding)
			       end,
		})
		AddOptionSep()
		AddOption('loadguidesfully',{ name = "Load full guides at startup", desc = "Horribly increases startup time, but loads and checks all guides.\nRestart for this to take effect.", type = 'toggle', width = "full", })
		AddOption('showwrongsteps',{ name = "Ignore step/line conditions", type = "toggle", width = "full", })
		AddOption('shownpcdebug',{ name = "Show NPC Debug button", type="toggle", width = "full", set = function(i,v)  Setter_Simple(i,v)  ZGV:NPCDebugUpdate()  end  })
		--AddOption('npcdebugauto',{ name = "Automatically add current npcs to list", type="toggle", width = "full", })


		AddOption('sep00pathf',{ type="header", name="Travel" })

		AddOption('debug_librover_maxspeed',{
			name = "",
			desc = "Riding skill",
			type = 'select',
			values={
				[0]="unset",
				[0.01]="No skill (0)",
				[0.6]="Apprentice (75) [slow ride]",
				[1.0]="Journeyman (150) [fast ride]",
				[1.5]="Expert (225) [slow flight]",
				[2.8]="Artisan (300) [fast flight]",
				[3.1]="Master (375) [epic flight]",
			},
			set = function(i,v)
				if v==0 then v=nil end
				Setter_Simple(i,v)
				LibRover:CheckMaxSpeeds()
				LibRover:UpdateNow()
			end,
		})
		AddOptionSep()
		AddOption('debug_librover_flightcold',{
			name = "Cold Flying",
			desc = "",
			type = 'toggle',
			tristate = true,
			width="full",
			set = function(i,v) Setter_Simple(i,v) LibRover:CheckMaxSpeeds() LibRover:UpdateNow() end,
		})
		AddOption('debug_librover_flightazeroth',{
			name = "Azeroth License",
			desc = "",
			type = 'toggle',
			tristate = true,
			width="full",
			set = function(i,v) Setter_Simple(i,v) LibRover:CheckMaxSpeeds() LibRover:UpdateNow() end,
		})
		AddOption('debug_librover_flightpandaria',{
			name = "Pandaria Flight",
			desc = "",
			type = 'toggle',
			tristate = true,
			width="full",
			set = function(i,v) Setter_Simple(i,v) LibRover:CheckMaxSpeeds() LibRover:UpdateNow() end,
		})

		--[[
		AddOption('debug_librover_steps',{
			name = "Verbose pathfinding",
			desc = "",
			type = 'toggle',
			set = function(i,v)
				Setter_Simple(i,v)
				LibRover.debug_cnodes = v
				--LibRover.debug_onodes = v
				if (v) then LibRover:GoSlow() end
			end,
		})
		AddOption('debug_librover_updating',{
			name = "Realtime",
			desc = "",
			type = 'toggle',
			set = function(i,v) Setter_Simple(i,v) LibRover.do_updating = v end,
		})
		--]]
		AddOptionSep()
		AddOptionSep()
		AddOption('debug_frame',{
			name = "Debug Output Frame",
			desc = "Usually ChatFrame1..ChatFrame9",
			type = 'input',
			set = function(i,v) Setter_Simple(i,v) ZGV.debugframe = _G[v] end,
			_default = "ChatFrame1"
		})

		AddOption('fpsgraph',{ name="FPS Graph", desc="Show a detailed FPS graph. Max=100fps.", type = 'toggle', width = "full", _default=false, set = function(i,v) Setter_Simple(i,v)  ZGV:StartFPSFrame() end, })

	end
	end

	self.optiontables['profile'] = LibStub("AceDBOptions-3.0"):GetOptionsTable(self.db)
	tinsert(self.optiontables_ordered,{name='profile',blizname="ZygorGuidesViewer-Profile"})
	self.db.RegisterCallback(self, "OnProfileChanged", "ProfileSwitch")
end

-- Some things, if not switched right away on a profile switch, will save the
-- old settings over the new when reloading the game. It's also nice to see
-- things dynamically update themselves on a switch :).
function ZGV:ProfileSwitch()
	if ZGV.db.profile.frame_positions and ZGV.db.profile.frame_positions.ZygorGuidesViewerPointer_ArrowCtrl then
		ZygorGuidesViewerPointer_ArrowCtrl:SetPoint(unpack(ZGV.db.profile.frame_positions.ZygorGuidesViewerPointer_ArrowCtrl))
	end
end

function ZGV:Options_RegisterDefaults()
	local defaults = {
		char = {
			starting = true,
			section = 1,
			step = 1,
			completedQuests = {},
			--permaCompletedDailies = {}, -- deprecating this, let's see if it works.
			completedDailies = {},
			debuglog = {},

			maint_startguides = true,
			maint_queryquests = true,

			guides_history = {},

			cookingMasteries = {},
			RecipesKnown = {},
			goodbadguides = {},
			ignoredguides = {},
			stephistory = {},
			taxis = {}
		},
		global = {
			storedguides = { },
			-- instantDailies = {}, -- let's not use this anymore, with GetQuestID available
		},
		profile = {
			debug = false,
			debug_flags = {["display"]=false,},
			--autosizemini = true,
			--minimode = false,
			visible = true,
			ranconfig = false,

			-- Talent System
				currentBuild = "None",
				talenton = true,
				talenticon = true,
				talentpopup = 2,

			showmapbutton = true,
			levelprogbar=false,

			suggestiondungeonnum={},

			-- convenience
			autoaccept = false,
			autoturnin = false,
			questitemselector = true,
			autoselectitem = false,
			fixblizzardautoaccept = false,
			autosell=false,
			badupgrade = { --There are four specs possible.
				[1] = {},
				[2] = {},
				[3] = {},
				[4] = {},
			},
			questitemcache = {},
			customcolorants=false,
			colorantstaxi={r=0.4,b=0.0,g=1.0,alpha=0.8},
			colorantsship={r=0.0,b=1.0,g=0.7,alpha=0.8},
			colorantportal={r=0.8,b=1.0,g=0.3,alpha=0.8},
			colorantsfly={r=1.0,b=0.0,g=0.8,alpha=0.8},
			colorantsother={r=1.0,b=1.0,g=1.0,alpha=0.8},
			singlecolorantscolor={r=0.5,b=0.5,g=0.5,alpha=0.8},
			singlecolorants=false,
			showgreyvalue=false,
			analyzereps = false,
			autotaxi = false,
			cvanchor = true,
			hideborder = false, --hidden anyway
			nevershowborder = false,
			showcountsteps = 1,
			showbriefsteps = false,
			hidecompletedinbrief = true,
			framescale = 1.0,
			fontsize = 12,
			fontsecsize = 11,

			progress=true,
			progresscolor={r=0.0,b=0.0,g=1.0,alpha=0.8},
			--backcolor = {r=0.18,g=0.05,b=0.23,a=0.56},
			opacitymain = 1.0,

			trackchains = true,

			--goalbackobsolete   = {r=0.0,g=0.5,b=0.8,a=0.5},
			--skipobsolete = true,
			--levelsahead = 0,
			--chainskip = true,
			--chainskipcount = 5,

			filternotes = true,
			minimapnotedesc = true,

			waypointaddon = "internal",

			golddetectiondist = 400,
			goldreqmode = 3, -- current
			golddistmode = 1, -- in range

			antspacing = 100,

			fullheight = 400,

			completesound = "MapPing",
			flipsounds = true,

			--colorborder = true,

			-- hidden

			slidebarconfig = {},

			displaymode = "guide",

			dispmodepri = true,
			dispprimary = {showcountsteps=1,hideborder=false,nevershowborder=false,showbriefsteps=false},
			dispsecondary = {showcountsteps=1,hideborder=true,nevershowborder=true,showbriefsteps=true,hidecompletedinbrief=true},

			badguidewarning = true,

			fakeskills = {},
			fakereps = {},

			tweaks_domacros = true,

			pathfinding_preferfly = true,
			pathfinding_leasthops = true,
		}
	}

	for k,v in pairs(self.optiontables) do  self:Options_GrabDefaults(v,defaults)  end

	self.db:RegisterDefaults(defaults)

	if self.db.profile.arrowskin=="sheen" then self.db.profile.arrowskin="fancy" end

	--self.db.profile.waypointaddon = "internal"
	self.db.profile.minicons = true

	if not self.db.profile.tmp__was_sheened then  -- one-time switch to stealth
		self.Pointer:SetArrowSkin("stealth")
		self.db.profile.tmp__was_sheened = true
	end
end

local function sort_by_order(a,b)
	return (a[2].order or 0)<(b[2].order or 0)
end

function ZGV:Options_GrabDefaults(options_tab,defaults)
	if options_tab.args then
		for k,v in pairs(options_tab.args) do
			if v._default~=nil then
				defaults.profile[k]=v._default
				v._default=nil
			elseif v.args then
				self:Options_GrabDefaults(v,defaults)
			end
		end
	end
end

local function ResetToDefaults(options_tab,parent)
	if options_tab.args then
		-- store args in a sorted table
		local t={}
		for k,v in pairs(options_tab.args) do
			tinsert(t,{k,v})
		end
		sort(t,sort_by_order)

		for i,j in ipairs(t) do
			local k,v = j[1],j[2]
			local oldval = ZGV.db.profile[k]
			local defval = ZGV.db.defaults.profile[k]
			if oldval~=defval then

				-- first force it
				--[[
				if v.type=="color" then
					local c = ZGV.db.defaults.profile[k]
					ZGV.db.profile[k] = {r=c.r,g=c.g,b=c.b,a=c.a}
				else
					ZGV.db.profile[k]=ZGV.db.defaults.profile[k]
				end
				--]]

				-- then pretend to be nice
				if type(v.set)=="function" then
					if v.type=="color" then
						local c = defval
						v.set({k},c.r,c.g,c.b,c.a)
					else
						v.set({k},defval)
					end
				elseif type(v.set)=="string" then
					parent.handler[v.set] (parent.handler, {k},defval)
				elseif parent.set then
					parent.set ({k},defval)
				else -- just set it
					if v.type=="color" then
						local c = defval
						ZGV.db.profile[k] = {r=c.r,g=c.g,b=c.b,a=c.a}
					else
						ZGV.db.profile[k]=defval
					end
				end
			end
			if v.args then
				ResetToDefaults(v,parent)
			end
		end
	end
end

local AceConfigRegistry = LibStub("AceConfigRegistry-3.0")
function ZGV:Options_ResetToDefaults(group)
	for k,v in pairs(self.optionpanels) do
		if v.obj==group then
			local options_tab = v.optiontable
			ResetToDefaults(options_tab,options_tab)
			AceConfigRegistry:NotifyChange(ZGV.optiontables_bliznames[v.optiontable])
			return
		end
	end
end

function ZGV:Options_SetupConfig()
	local AceConfig = LibStub("AceConfig-3.0")

	for i,v in ipairs(self.optiontables_ordered) do
		AceConfig:RegisterOptionsTable(v.blizname, self.optiontables[v.name], v.slash );
	end
end

function ZGV:Options_SetupBlizConfig()
	local AceConfigDialog = LibStub("AceConfigDialog-3.0")

	InterfaceOptionsFrame:GetRegions():SetTexture(0,0,0,0.9)
	AceConfigDialog:SetDefaultSize("ZygorGuidesViewer", 600, 400)
	self.optionpanels = {}
	for i,v in ipairs(self.optiontables_ordered) do
		local panel = AceConfigDialog:AddToBlizOptions(v.blizname, self.optiontables[v.name].name, v.name~='main' and self.optiontables.main.name)
		panel.optiontable = self.optiontables[v.name]
		self.optionpanels[v.name=='main' and '' or v.name] = panel
		AceConfigDialog.BlizOptions[v.blizname][v.blizname]:SetCallback("default",function(group) ZGV:Options_ResetToDefaults(group) end)
	end

	self.db.profile.skipauxsteps = true
	self.db.profile.magickey_bind = ""
	if self.db.profile.autogearframe~= nil then
		self.db.profile.autogearauto = not self.db.profile.autogearframe   self.db.profile.autogearframe = nil
	end
end


function ZGV:GetCurrentGuideNum()
	if not self.CurrentGuide then return nil end
	for i,data in ipairs(ZygorGuidesViewer.registeredguides) do
		if data.title==self.CurrentGuide.title then return i end
	end
end


function ZGV:OpenOptions(v)
	--self:OpenConfigMenu()
	InterfaceOptionsFrame_OpenToCategory(self.optionpanels[v or ''])

	for i=1,100 do
		local but = _G['InterfaceOptionsFrameAddOnsButton'..i]
		if but then
			if but.element == self.optionpanels[''] and but.element.collapsed then but.toggle:Click() break end
		end
	end
end


function ZGV:SetOption(cat,cmd)
	LibStub("AceConfigCmd-3.0").HandleCommand(self, "zygor", "ZygorGuidesViewer"..(cat~="" and "-"..cat or ""), cmd)
end
