-- Daily Global Check
-- by Fluffies
-- EU-Well of Eternity
local addonName, addonTable = ...
local addontitle = "|cff00AAFFDaily Global Check|r"
local ldb = LibStub:GetLibrary("LibDataBroker-1.1")
local ldbicon = ldb and LibStub("LibDBIcon-1.0", true)
local dgcicon = "Interface\\icons\\INV_Sigil_Freya"
local L = addonTable.L -- localization
local version = 1001

DailyGlobalCheck_Options = {}
DGC_CustomLists = {}
local DGCMenuFrame
local DGCTTmenu = {}
local DGCMMmenu = {}
local SelectedList
local questsorder = {}
local questsdata = {}
local build_mode = false
if addonTable.Plugins == nil then addonTable.Plugins = {} end
local pluginbtnsize = 25
local pluginbarwidth = 75

-- string colors
local LIGHT_RED    = "|cffFF2020"
local LIGHT_GREEN  = "|cff20FF20"
local LIGHT_BLUE   = "|cff11DDFF"
local LIGHT_YELLOW = "|cffFFFFAA"
local ZONE_BLUE    = "|cff00aacc"
local GRAY         = "|cffAAAAAA"
local NAME_COLOR   = "|cffffffcc"
local COORD_COLOR  = "|cffDDDDDD"
local GOLD         = "|cffffcc00"
local WHITE        = "|cffffffff"
local LIST_COLOR   = "|cff00DD11"
local function AddColor(str,color)
 if not str then return "|cffFF0000<Error>|r" end
 return color..str.."|r"
end

local CENTER, LEFT, RIGHT = 1,2,3
local CustomList = 
   { ["Title"] = L["customlist"],
     ["Icon"]  = dgcicon,
     ["Data"]  = {},
     ["Order"] = {[LEFT] = {{L["customlist"]}}, [CENTER] = {{L["customlist"]}}, [RIGHT] = {{L["customlist"]}}}
   }
local quest_missing = AddColor(string.format(ITEM_MISSING,BATTLE_PET_SOURCE_2),LIGHT_RED)
-- buttons' textures
local arrowl = "Interface\\icons\\misc_arrowleft"
local arrowr = "Interface\\icons\\misc_arrowright"
local tx_build = "Interface\\icons\\INV_Hammer_20"
local tx_qmark = "Interface\\icons\\INV_Misc_QuestionMark"
local tx_aboutbtn     = "Interface\\icons\\Icon_PetFamily_Beast"
local tx_optionsbtn = "Interface\\icons\\Icon_PetFamily_Mechanical"
local tx_questtype = "Interface\\icons\\Achievement_Quests_Completed_06"
local tx_pref = "Interface\\icons\\INV_Fabric_Soulcloth"
local tx_zone = "Interface\\icons\\inv_misc_dmc_destructiondeck"
local tx_suff = "Interface\\icons\\INV_Fabric_Soulcloth_Bolt"
local tx_coords = "Interface\\icons\\INV_Misc_Map_01"
local tx_worldmap  = "Interface\\icons\\Ability_Hunter_Crossfire"
local tx_minimap   = "Interface\\icons\\INV_Misc_Map03"

-- constants
local ZONE = 1
local NAME = 2
local PREF = 3
local SUFF = 4
local MAPDATA = 5
local MAPDEFAULT = 6
local QUESTTYPE = 7 -- nil == daily
local MAPICON = 8 -- global daily check icon if nil
local SHOWFUNC = 9

-- build_frame menus
local addName, addID
local function addtolist(index)
 local f = DailyGlobalCheck_mainframe.build_frame
 if not f then return end
 
 local questID = addID and addID or f.edtqID:GetText()
 local questName = addName and addName or f.edtqName:GetText()
 local pref = addID and "" or f.edtpref:GetText()
 local suff = addID and "" or f.edtsuff:GetText()
 
 if questID == "" or questName == "" then
  print(L["bf_err1"])
  return
 end
 
 if CustomList["Data"][tonumber(questID)] then
  print(L["bf_err2"])
  return  
 end
 
 CustomList["Data"][tonumber(questID)] = {"",questName,pref,suff}
 table.insert(CustomList["Order"][index][1], tonumber(questID))
 print("|cff11DDFFDaily Global Check|r - "..questName.." "..L["added"])
 addonTable.refresh(CustomList)
 
 -- add header con editbox dialog

 f.edtqID:SetText("")
 f.edtqName:SetText("")
 f.edtpref:SetText("")
 f.edtsuff:SetText("")
end

--
local addmenu = {{text = AddColor(L["questwindow"],GOLD),notCheckable = 1, isTitle = true},
                 {text = "Left",notCheckable = 1, func = function() addtolist(LEFT) end},
				 {text = "Center",notCheckable = 1, func = function() addtolist(CENTER) end},
				 {text = "Right",notCheckable = 1, func = function() addtolist(RIGHT) end}}
local remmenu = {}
--

local opt_framespeed = 20  -- frames animation speed
local centerbuttons_spacing = 80

-- buttons utils
local starttime = 0
local lastobj
local function StartTimedGlow(obj, t)

 local function turnoff(f)
  f:SetScript("OnUpdate", nil)
  ActionButton_HideOverlayGlow(f)
 end

 if lastobj then 
  turnoff(lastobj)
  lastobj = nil
 end
 
 if not obj then return end

 local starttime = GetTime()
 ActionButton_ShowOverlayGlow(obj)
 obj:SetScript("OnUpdate", function(self) 
  local currtime = GetTime()
  if currtime > starttime + t then
   turnoff(self)
  end
 end)
 lastobj = obj
end

local function buildframecheck()
 if SelectedList == CustomList then
  DailyGlobalCheck_mainframe.buildbtn:Show()
  if build_mode then
   DailyGlobalCheck_mainframe.build_frame:Show()
  else
   DailyGlobalCheck_mainframe.build_frame:Hide()
  end
 else
  DailyGlobalCheck_mainframe.build_frame:Hide()
  DailyGlobalCheck_mainframe.buildbtn:Hide()
 end
end
-------------
local template = {[ZONE] = "", [NAME] = L["namemissing"], [PREF] = "", [SUFF] = "", [QUESTTYPE] = "D"}
local function get(tab, i)
 return tab[i] and tab[i] or template[i]
end
------- zone -> quests table for the world map frame -------------------------
local mapstable = {}
local function generatezonetomaptable()
 wipe(mapstable)
 table.foreach(questsdata, function(questID,info)
   if info[MAPDATA] and type(info[MAPDATA]) == "table" then
    table.foreach(info[MAPDATA], function(mapID, _)
     if not mapstable[mapID] then
      mapstable[mapID] = {}
     end
     if not tContains(mapstable[mapID], questID) then
      table.insert(mapstable[mapID], questID)
     end
    end)
   end
  end)
end
------------------------------------------------------------------------------
local function GetCoord(questID)
 local v = questsdata[questID][MAPDATA]
 local default = questsdata[questID][MAPDEFAULT]
 return v and AddColor(v[default][1]..","..v[default][2], COORD_COLOR) or ""
end
------------------------------------------------------------------------------
local function isquestcompleted(questID)
 if SelectedList["Overrides"] and SelectedList["Overrides"]["isquestcompleted"] then
  return SelectedList["Overrides"]["isquestcompleted"](questID)
 else
  return IsQuestFlaggedCompleted(questID)
 end
end
------------------------------------------------------------------------------
local function isshown(questID)
 if not questsdata[questID] then return false end
		   -- if there isn't a show function, else if it returns true
 return not questsdata[questID][SHOWFUNC] or questsdata[questID][SHOWFUNC]()
end

local function SetAddonTitle()
 if not DailyGlobalCheck_mainframe then return end

 local plugintitle = SelectedList and "\n"..AddColor(SelectedList["Title"],LIST_COLOR) or ""
 DailyGlobalCheck_mainframe.title:SetText(addontitle..plugintitle)
end

local function GenerateLine(questID)
    local s1 = "";
    if not questsdata[questID] then return L["questdatamissing"] end

    -- quest type
    if DailyGlobalCheck_Options["show_questtype"] then
     s1 = s1..AddColor("["..get(questsdata[questID], QUESTTYPE).."]",LIGHT_BLUE) 
    end
    -- prefix
    if DailyGlobalCheck_Options["show_prefix"] and questsdata[questID][PREF] then
     s1 = s1..get(questsdata[questID], PREF)
    end
    -- zone name
    if DailyGlobalCheck_Options["show_zone"] and questsdata[questID][ZONE] then
     s1 = s1..AddColor(questsdata[questID][ZONE],GRAY)
    end
    -- quest name
    s1 = s1.." "..AddColor(get(questsdata[questID], NAME),NAME_COLOR)
    -- suffix
    if DailyGlobalCheck_Options["show_suffix"] and questsdata[questID][SUFF] then
     s1 = s1..get(questsdata[questID], SUFF).." "
    end
    -- coordinates
    if DailyGlobalCheck_Options["show_coordinates"] then
     s1 = s1.." "..AddColor(GetCoord(questID),COORD_COLOR)
    end
    return s1
end
---------------------------- TomTom Menu Frame -------------------------------------
local function GenerateTomTomMenu()
 if not TomTom or not DGCMenuFrame then return end

  local function CreateSubMenu(q)
   local submenu = {}
   table.foreach(q, function(index,questID)
    if questsdata[questID] and index > 1 and isshown(questID) then
    table.insert(submenu, { text = get(questsdata[questID], NAME).." - "..AddColor(get(questsdata[questID], ZONE),GRAY).." "..GetCoord(questID),
                            disabled = isquestcompleted(questID) or not questsdata[questID][MAPDATA],
                            notCheckable = 1,
                            keepShownOnClick = true,
                            func = function(self)
                               if questsdata[questID] and questsdata[questID][MAPDATA] then
                                 TomTom:AddMFWaypoint(questsdata[questID][MAPDEFAULT], nil,
                                            questsdata[questID][MAPDATA][questsdata[questID][MAPDEFAULT]][1] / 100,
                                            questsdata[questID][MAPDATA][questsdata[questID][MAPDEFAULT]][2] / 100,
                                          { title = get(questsdata[questID], NAME)})
                                 print(L["s_tomtomset"].." ("..AddColor(get(questsdata[questID], NAME),LIGHT_BLUE)..")")
                               end
                              end
                              })
     end
    end)
    return submenu
   end

  wipe(DGCTTmenu)
  table.insert(DGCTTmenu, {text = AddColor("- TomTom Waypoints -",GOLD),notCheckable = 1, isTitle = true})
  table.insert(DGCTTmenu, {text = AddColor("Click either a single quest or a group",LIGHT_BLUE),notCheckable = 1, isTitle = true})
  table.insert(DGCTTmenu, {text = AddColor("to set TomTom waypoints",LIGHT_BLUE),notCheckable = 1, isTitle = true})
  table.insert(DGCTTmenu, {text = "",notCheckable = 1, isTitle = true})

  table.foreach(questsorder, function(k,tab)
   local pref = (k == LEFT and "|T"..arrowl..":12|t") or (k == RIGHT and "|T"..arrowr..":12|t") or "|T"..SelectedList["Icon"]..":12|t"
   table.foreach(tab, function(header,quests)
    if table.getn(quests) > 1 then
    table.insert(DGCTTmenu, { text = quests[1] ~= "" and pref..quests[1] or pref..L["notitle"],
                              keepShownOnClick = true,
                              notCheckable = 1,
                       func = function(self)
                               table.foreach(quests, function(index, questID)
							    if index > 1 then
                                  if questsdata[questID] and questsdata[questID][MAPDATA] and not isquestcompleted(questID) and isshown(questID) then
                                   TomTom:AddMFWaypoint(questsdata[questID][MAPDEFAULT], nil,
                                              questsdata[questID][MAPDATA][questsdata[questID][MAPDEFAULT]][1] / 100,
                                              questsdata[questID][MAPDATA][questsdata[questID][MAPDEFAULT]][2] / 100,
                                            { title = get(questsdata[questID], NAME) } )
                                   print(L["s_tomtomset"].." ("..AddColor(get(questsdata[questID], NAME),LIGHT_BLUE)..")")
                                 end
								end
                               end)
                              end,
                       hasArrow = true,
		               menuList = CreateSubMenu(quests)})
	end
   end)
  end)
  table.insert(DGCTTmenu, {text = "", isTitle = true, notCheckable = 1})
  table.insert(DGCTTmenu, {text = CLOSE, notCheckable = 1})
end
------------------------------------------ Draw Frame --------------------------------------------------------
local function DrawFrame(obj, index, istooltip)

 local tmpleft  = ""
 local tmpright = ""
 
  local function adddoubleline(left,right)
  if istooltip then obj:AddDoubleLine(left,right) return end

  tmpleft  = tmpleft.."\n"..left
  tmpright = tmpright.."\n"..right
 end
 
 if not SelectedList then
  if table.getn(addonTable.Plugins) == 0 and CustomList == nil then
   adddoubleline(AddColor("No lists available",LIGHT_RED)," ")
   adddoubleline(" "," ")
   adddoubleline(AddColor("Check out on curse:",LIGHT_RED)," ")
   adddoubleline(AddColor("Daily Global Check_World Bosses",WHITE)," ")
   adddoubleline(AddColor("Daily Global Check_Isle of Thunder",WHITE)," ")
   adddoubleline(AddColor("Daily Global Check_Timeless Isle",WHITE)," ")
   adddoubleline(AddColor("Daily Global Check_Pet Tamers",WHITE)," ")
   adddoubleline(AddColor("...and more to come!",WHITE)," ")
  else
   adddoubleline(AddColor(L["nolist"],LIGHT_RED)," ")
  end
 else
  if not questsorder[index] then return end
  
  local s1,s2
  table.foreach(questsorder[index], function(k,v)
   if v[1] ~= "" then
    adddoubleline(AddColor(v[1],GOLD)," ")
   end
   table.foreach(v,function(i,questID)
           if i > 1 then
		     if isshown(questID) then
              s2 = isquestcompleted(questID) and AddColor(COMPLETE,LIGHT_GREEN) or AddColor(INCOMPLETE,LIGHT_RED)
              s1 = GenerateLine(questID)
              adddoubleline(s1,s2)
		 	end
	      end
    end)
    adddoubleline(" "," ")
   end)
  end
 if not istooltip then
 -- create some space for the buttons
  local plus = TomTom and pluginbtnsize * 5 or pluginbtnsize * 4
  obj.leftfont:SetText(tmpleft)
  obj.rightfont:SetText(tmpright)
  if index == CENTER then
   obj:SetSize(math.max(300, obj.leftfont:GetWidth() + obj.rightfont:GetWidth() + 30),obj.leftfont:GetHeight() + plus)
   GenerateTomTomMenu()
  else
   obj.maxwidth = math.max(100, obj.leftfont:GetWidth() + obj.rightfont:GetWidth() + 30)
   obj:SetHeight(math.max(DailyGlobalCheck_mainframe:GetHeight(), obj.leftfont:GetHeight()))
  end
 else
  obj:AddLine(AddColor(L["lclicktoopen"],LIGHT_BLUE))
  obj:AddLine(AddColor(L["rclicktoopen"],LIGHT_BLUE))
  obj:Show()
 end
end
---------------------------- Chat Output ---------------------------------------------
local function ChatOutput()
 if not questsorder[CENTER] then return end

 local s1,s2
 table.foreach(questsorder[CENTER], function(k,v)
 if v[1] ~= "" then
  print(AddColor(v[1],GOLD))
 end
 table.foreach(v,function(i,questID)
        if i > 1 then
	    if isshown(questID) then
            s2 = isquestcompleted(questID) and AddColor(COMPLETE,LIGHT_GREEN) or AddColor(INCOMPLETE,LIGHT_RED)
            s1 = GenerateLine(questID)
            print(s1.." "..s2)
		end
     end
  end)
 end)
end
---------------------------- set frames data -----------------------------------------
local function CheckSideButton(i)
 if not DailyGlobalCheck_mainframe.buttons[i] or i == 4 then return end

 if questsorder[i] then
  DailyGlobalCheck_mainframe.buttons[i].enabled = true
  DailyGlobalCheck_mainframe.buttons[i].texture:SetVertexColor(1,1,1,1)
 else
  DailyGlobalCheck_mainframe.buttons[i].enabled = false
  DailyGlobalCheck_mainframe.buttons[i].texture:SetVertexColor(0.4,0.4,0.4,1)
 end
end

local function SetFramesData(data, order)
 if not data or not order then return false end

 wipe(questsdata)
 table.foreach(data, function(k,v) questsdata[k] = v end)
 wipe(questsorder)
 table.foreach(order, function(k,v) questsorder[k] = v end)
 generatezonetomaptable()
end

local function OpenList(v)
 local list

 local function lflist(_,t)
  if t["Title"] == v then list = t end
 end

 if type(v) == "string" then
  table.foreach(addonTable.Plugins, lflist)
  if not list then list = CustomList end
 else 
  list = v
 end

 if not list then return end
 SelectedList = list
 DailyGlobalCheck_Options["last_selected"] = list["Title"]
 SetFramesData(list["Data"], list["Order"])
 GenerateTomTomMenu()
 addonTable.forcemapupdate()
 if DGCMenuFrame then DGCMenuFrame:Hide() end
 if DailyGlobalCheck_mainframe then
  buildframecheck()
  SetAddonTitle()
  CheckSideButton(LEFT)
  CheckSideButton(RIGHT)
  local btns = DailyGlobalCheck_mainframe.listbuttons
  if btns[SelectedList["Title"]] then
   table.foreach(btns, function(k,v) v:Deselect() end)
   btns[SelectedList["Title"]]:Select()
   StartTimedGlow(btns[SelectedList["Title"]], 2)
  end
  DrawFrame(DailyGlobalCheck_mainframe, CENTER, false)
 end
end
---------------------------------------------------------------------
local function GenerateMinimapMenu()
 if not DGCMMmenu then return end

  local function CreateListsMenu()
   local submenu = {}
   table.insert(submenu, {text = L["customlist"], notCheckable = 1, keepShownOnClick = true, func = function() OpenList(CustomList) end})
   table.foreach(addonTable.Plugins, function(k,v)
    table.insert(submenu, { text = v["Title"],
                            --disabled = false,
                            notCheckable = 1,
                            keepShownOnClick = true,
                            func = function()
                               OpenList(v)
                              end
                              })
    end)
    return submenu
   end

  DGCMMmenu = {}
  table.insert(DGCMMmenu, {text = AddColor(L["availablelists"],GOLD),notCheckable = 1, hasArrow = true, menuList = CreateListsMenu()})
  if TomTom and DGCTTmenu then
   table.insert(DGCMMmenu, {text = AddColor("TomTom",GOLD),notCheckable = 1, hasArrow = true, menuList = DGCTTmenu})
  end
--  table.insert(DGCTTmenu, {text = "",notCheckable = 1, isTitle = true})
  table.insert(DGCMMmenu, {text = "", isTitle = true, notCheckable = 1})
  table.insert(DGCMMmenu, {text = "Close", notCheckable = 1})
  EasyMenu(DGCMMmenu, DGCMenuFrame, "cursor", -100 , 0, "MENU")
end

local function GenerateRemoveButtonMenu()

 local function CreateSubMenu(q)
   local submenu = {}
   table.foreach(q, function(index,questID)
    if questsdata[questID] and index > 1 and isshown(questID) then
    table.insert(submenu, { text = get(questsdata[questID], NAME),
                            notCheckable = 1,
                            func = function(self)
							    if CustomList["Data"][questID] then
                                 print("|cff11DDFFDaily Global Check|r - "..get(CustomList["Data"][questID], NAME).." "..L["deleted"])
								 local i = 1
                                 while q[i] do
                                  if questID == q[i] then
                                   table.remove(q, i)
								  else
								   i = i + 1
								  end
								 end
								 CustomList["Data"][questID] = nil
								 addonTable.refresh(CustomList)
								end
                              end
                              })
     end
    end)
    return submenu
   end

  wipe(remmenu)
  table.insert(remmenu, {text = L["remmenuheader"], isTitle = true, notCheckable = 1})
  table.insert(remmenu, {text = "",notCheckable = 1, isTitle = true})

  table.foreach(CustomList["Order"], function(k,tab)
   local pref = (k == LEFT and "|T"..arrowl..":12|t") or (k == RIGHT and "|T"..arrowr..":12|t") or "|T"..SelectedList["Icon"]..":12|t"
   table.foreach(tab, function(header,quests)
    if table.getn(quests) > 1 then
    table.insert(remmenu, { text = quests[1] ~= "" and pref..quests[1] or pref..L["notitle"],
                            keepShownOnClick = true,
                            notCheckable = 1,
                            hasArrow = true,
		                    menuList = CreateSubMenu(quests)})
	end
   end)
  end)
  table.insert(remmenu, {text = "", isTitle = true, notCheckable = 1})
  table.insert(remmenu, {text = CLOSE, notCheckable = 1})
 
end


local function CheckFramesAnimation(frame,id)
 if (frame) and (frame:IsVisible()) and (frame.maxwidth ~= nil) and (frame.width ~= nil) and (frame.opening ~= nil) then
  if frame.opening then
   if frame.width < frame.maxwidth then
    frame.width = frame.width + opt_framespeed
    if id == 4 then
     frame:SetHeight(frame.width)
    else
     frame:SetWidth(frame.width)
    end
   elseif (not frame.leftfont:IsVisible()) then
    frame.leftfont:Show()
    frame.rightfont:Show()
   end
  else -- closing
   if frame.width > opt_framespeed then
    frame.width = frame.width - opt_framespeed
	 -- about frame hack
    if id == 4 then
     frame:SetHeight(frame.width)
    else
     frame:SetWidth(frame.width)
    end
    if (frame.leftfont:IsVisible()) then
     frame.leftfont:Hide()
     frame.rightfont:Hide()
    end
   else
    frame:Hide()
   end
  end
 end
end

local function OnOptionChanged(opt)
 if opt == "show_mapicons" then
  addonTable.forcemapupdate()
 elseif opt == "show_questlogbtn" then
  if DailyGlobalCheck_Options["show_questlogbtn"] then
   DGC_AddQuestbtn:Show() else DGC_AddQuestbtn:Hide()
  end
 end
end

local ID_MINIMAP_SPECIAL = 99
local function CreateCheckbox(parent,id,option,mo_tooltip,tx,offsetX,offsetY)
 local chkbox

 local function setcheckboxtexture(flag)
  local shaderSupported = chkbox.texture:SetDesaturated(not flag);
  if not shaderSupported then
   if not flag then
     chkbox.texture:SetVertexColor(0.5, 0.5, 0.5);
   else
     chkbox.texture:SetVertexColor(1.0, 1.0, 1.0);
   end
  end
 end

  if not parent.checkbox[id] then
   chkbox = CreateFrame("FRAME", nil, parent.optionsframe)
   chkbox.texture = chkbox:CreateTexture()
  else 
   chkbox = parent.checkbox[id]
  end

  chkbox:SetPoint("CENTER", parent.optionsframe, "CENTER",offsetX,offsetY)
  --chkbox:SetFrameStrata("HIGH")
  chkbox:SetWidth(20)
  chkbox:SetHeight(20)
  chkbox.texture:SetPoint("LEFT", chkbox, "LEFT")
  chkbox.texture:SetTexture(tx)
  chkbox.texture:SetWidth(20)
  chkbox.texture:SetHeight(20)
  if id == ID_MINIMAP_SPECIAL then
   setcheckboxtexture(not DailyGlobalCheck_Options["minimap_icon"].hide)
  else
   setcheckboxtexture(DailyGlobalCheck_Options[option])
  end

  chkbox:SetScript("OnMouseUp", function(self)
   if id == ID_MINIMAP_SPECIAL then
    DailyGlobalCheck_Options["minimap_icon"].hide = not DailyGlobalCheck_Options["minimap_icon"].hide
    ldbicon:Refresh("DailyGlobalCheck_broker", DailyGlobalCheck_broker, DailyGlobalCheck_Options["minimap_icon"])
    setcheckboxtexture(not DailyGlobalCheck_Options["minimap_icon"].hide)
   else
    DailyGlobalCheck_Options[option] = not DailyGlobalCheck_Options[option]
    OnOptionChanged(option)
    DrawFrame(DailyGlobalCheck_mainframe,CENTER,false)
    setcheckboxtexture(DailyGlobalCheck_Options[option])
   end
  end)

  chkbox:SetScript("OnEnter", function(self)
   GameTooltip:SetOwner(self,"ANCHOR_BOTTOM",0,-5)
   GameTooltip:ClearLines()
   GameTooltip:AddLine(mo_tooltip)
   GameTooltip:Show()
  end)  

  chkbox:SetScript("OnLeave", function(self)
   GameTooltip:Hide()
  end)
  
  parent.checkbox[id] = chkbox
  chkbox:Hide()
end --CreateCheckBox

local function CreateNewFrameButton(id,pos,tx)
local result, button
 if not DailyGlobalCheck_mainframe.buttons[id] then
  button = CreateFrame("FRAME", nil, DailyGlobalCheck_mainframe)
 else button = DailyGlobalCheck_mainframe.buttons[id]
 end
 if not button.texture then
  button.texture = button:CreateTexture()
 end
 if not DailyGlobalCheck_mainframe.frames[id] then
  result = CreateFrame("Frame")
 else result = DailyGlobalCheck_mainframe.frames[id]
 end
 result:SetParent(DailyGlobalCheck_mainframe)
 addonTable:SetupFrame(result)
 result.width = 1
 button.enabled = false
 if id == 4 then
   result.leftfont:SetText(AddColor(" Daily Global Check ",GOLD).."\n"..
                                         AddColor(" by\n",GRAY)..
                                         AddColor(" Fluffies",LIGHT_BLUE)..
                                         AddColor(" EU-Well of Eternity\n\n",GRAY)..
										 AddColor("Plugins available on Curse:\n",LIGHT_GREEN)..
                                         AddColor("Daily Global Check_World Bosses\n"..
                                         "Daily Global Check_Isle of Thunder\n"..
                                         "Daily Global Check_Timeless Isle\n"..
                                         "Daily Global Check_Pet Tamers\n\n",GRAY)..
										 AddColor("What's new in v1.1.0 :\n",GOLD)..
										 AddColor("Custom List - ", WHITE)..AddColor("you can now build your own\n"..
										 "custom list using the specific frame or simply\n"..
										 "by adding a quest directly from the quest log\n"..
										 "(the DGC button is located in the top-right\n"..
										 "corner of the quest log)", GRAY)
										 )
   result.rightfont:SetText("|T"..dgcicon..":24|t\n")
   result.maxwidth = 210
   button.enabled = true
  else
   result.maxwidth = 200
  end

 result.leftfont:Hide()
 result.rightfont:Hide()
 
 result:SetFrameStrata("DIALOG")
 --result:SetScale(0.9)
 result:SetClampedToScreen(true)
 result:ClearAllPoints()
 if pos == "left" then
   result:SetPoint("RIGHT", DailyGlobalCheck_mainframe, "LEFT")
 elseif pos == "right" then
   result:SetPoint("LEFT", DailyGlobalCheck_mainframe, "RIGHT")
 elseif pos == "bottomright" or pos == "bottomleft" then
  result:SetPoint("TOPLEFT", DailyGlobalCheck_mainframe, "BOTTOMLEFT")
  result:SetPoint("TOPRIGHT", DailyGlobalCheck_mainframe, "BOTTOMRIGHT")
 end
 -- frame's opening/closing animation
 result:SetScript("OnUpdate", function(self)
   CheckFramesAnimation(result,id)
  end)

  if pos == "left" then
   button:SetPoint("BOTTOMLEFT", DailyGlobalCheck_mainframe, "BOTTOMLEFT",10,10)
  elseif pos == "right" then
   button:SetPoint("BOTTOMRIGHT", DailyGlobalCheck_mainframe, "BOTTOMRIGHT",-10,10)
  elseif pos == "bottomright" then
   button:SetPoint("BOTTOM", DailyGlobalCheck_mainframe, "BOTTOM",centerbuttons_spacing,10)
  elseif pos == "bottomleft" then
   button:SetPoint("BOTTOMLEFT", DailyGlobalCheck_mainframe, "BOTTOMLEFT",45,10)
  end
  button:SetWidth(25)
  button:SetHeight(25)
  button.texture:SetAllPoints()
  button.texture:SetTexture(tx)
  button:SetScript("OnEnter", function(self)
   if button.enabled then
    result.opening = true
    result:SetWidth(result.width)
    if questsorder[id] then -- side frames
     DrawFrame(result, id, false)
    end
    result:Show()
   end
 end)

  button:SetScript("OnLeave", function(self)
   if result then
    result.opening = false 
   end
  end)
 button:Show()
 if result.opening then result:Show() else result:Hide() end
 DailyGlobalCheck_mainframe.frames[id] = result;
 DailyGlobalCheck_mainframe.buttons[id] = button;
 CheckSideButton(id)
end --CreateNewFrameButton

-- Initialize Mainframe
local function DGCInit(arg)
 local f = DailyGlobalCheck_mainframe

 if arg == "print" then
  print(AddColor("Daily Global Check",LIGHT_BLUE))
  ChatOutput()
  print(AddColor("---",LIGHT_BLUE))
  return
 end
 
 if f then
  if f:IsVisible() then
   f:Hide()
   if DGCMenuFrame then DGCMenuFrame:Hide() end
  else
   DrawFrame(f, CENTER, false)
   if SelectedList and SelectedList["Title"] and f.listbuttons[SelectedList["Title"]] then
    StartTimedGlow(f.listbuttons[SelectedList["Title"]], 3)
   end
   f:Show()
  end
 else
  DailyGlobalCheck_mainframe = CreateFrame("Frame", "DGC_Mainframe", UIParent)
  f = DailyGlobalCheck_mainframe
  f:SetPoint("CENTER")
  f:EnableMouse(true)
  f:SetMovable()
  --f:SetScale(0.9)
  f:RegisterForDrag("LeftButton")
  local sx,sy,ex,ey
  f:SetScript("OnDragStart", function(self) self:StartMoving() end)
  f:SetScript("OnDragStop", 
   function(self) self:StopMovingOrSizing() end)
  f.buttons  = {}
  f.frames = {}
  addonTable:SetupFrame(f)
  -- lateral and about frames
  CreateNewFrameButton(LEFT,"left",arrowl)
  CreateNewFrameButton(RIGHT,"right",arrowr)
  CreateNewFrameButton(4,"bottomright",tx_aboutbtn)

  -- options
  f.optionsframe = CreateFrame("FRAME", nil, f)
  f.optionsframe.texture = f.optionsframe:CreateTexture()
  f.optionsframe:SetPoint("BOTTOM",f,"BOTTOM",-centerbuttons_spacing,10)
  f.optionsframe:SetWidth(25)
  f.optionsframe:SetHeight(25)
  f.optionsframe.texture:SetAllPoints()
  f.optionsframe.texture:SetTexture(tx_optionsbtn)
  addonTable:SetTooltip(f.optionsframe, {L["opt_main"]})
  
  f.optionsframe:SetScript("OnMouseUp", function(self)
   if not f.optionsframe.open then
    table.foreach(f.checkbox, function(k,v) if not v:IsVisible() then v:Show() end end)
    f.optionsframe.open = true;
    f.optionsframe.texture:SetVertexColor(0.5,1,0.5,1)
   else
    table.foreach(f.checkbox, function(k,v) if v:IsVisible() then v:Hide() end end)
    f.optionsframe.open = false;
    f.optionsframe.texture:SetVertexColor(1,1,1,1)
   end
  end)
 f.optionsframe.texture:SetVertexColor(1,1,1,1)
 f.optionsframe.open = false
 DailyGlobalCheck_mainframe.checkbox = {}
 CreateCheckbox(f,0,"show_questtype",
                       L["opt_questtype"],tx_questtype,-40,-25)
 CreateCheckbox(f,1,"show_prefix",
                       L["opt_prefix"],tx_pref,-20,-25)
 CreateCheckbox(f,2,"show_zone",
                       L["opt_zonename"],tx_zone,0,-25)
 CreateCheckbox(f,3,"show_suffix",
                       L["opt_suffix"],tx_suff,20,-25)
 CreateCheckbox(f,4,"show_coordinates",
                       L["opt_coords"],tx_coords,40,-25)
 CreateCheckbox(f,5,"show_mapicons",
                       L["opt_map"],tx_worldmap,0,-45)
 CreateCheckbox(f,6,"show_questlogbtn",
                       L["opt_showquestbtn"],dgcicon,-20,-45)
 CreateCheckbox(f,ID_MINIMAP_SPECIAL,"",
                       L["opt_minimap"],tx_minimap,20,-45)

-- lists and plugins buttons
-- background
  f:SetScript("OnSizeChanged", function()
							   if f.listbuttons[L["customlist"]] then
							    f.listbuttons[L["customlist"]]:SetPoint("TOPLEFT", f, "TOPLEFT", 5, -8 + pluginbtnsize)
							   end
							   local i = 1
                               table.foreach(f.listbuttons, function(k,v)
							    if k == L["customlist"] then return end
                                v:SetPoint("TOPLEFT", f, "TOPLEFT", 5 + (i * pluginbtnsize), -8 + pluginbtnsize)
								i = i + 1
                               end)
                              end)

-- Mainframe Title
 f.title = f:CreateFontString()
 f.title:SetFont("Fonts\\ARIALN.ttf", 20, "")
 f.title:SetPoint("TOP", f, "TOP", 0, -5)
 SetAddonTitle()
-- Mainframe Icon
 f.dgcicon = f:CreateTexture("ARTWORK")
 f.dgcicon:SetPoint("TOPRIGHT", f, "TOPRIGHT", -20, 10)
 f.dgcicon:SetTexture(dgcicon)
 f.dgcicon:SetSize(50,50)
-- Mainframe fonts offset
 f.leftfont:SetPoint("TOPLEFT",10,-pluginbtnsize * 2)
 f.rightfont:SetPoint("TOPRIGHT",-10,-pluginbtnsize * 2)
-- Plugin Buttons
 f.listbuttons = {}
 local counter = 0
 local function createlistbutton(_,v)
  local btn = addonTable:GetListItem(f)
  f.listbuttons[v["Title"]] = btn
  btn:SetSize(25,25)
  --btn:SetPoint("TOPLEFT", f, "TOPLEFT", 15 + (25 * counter), -15)
  btn.tooltiplines = {v["Title"]}
  if v == SelectedList then
   btn:Select()
   StartTimedGlow(btn, 3)
  end
  btn:SetIcon(v["Icon"] and v["Icon"] or tx_qmark)
  btn.clickfunc = function()
                   OpenList(v)
                  end
  counter = counter + 1
 end

 -- custom list button
 createlistbutton(_, CustomList)
 -- plugin buttons
 table.foreach(addonTable.Plugins, createlistbutton)
 pluginbarwidth = counter * pluginbtnsize + counter
 
 -------------------------------------------------------

 -- "show/hide build frame" button
 f.buildbtn = addonTable:GetListItem(f)
 f.buildbtn:SetPoint("TOPLEFT", f, "TOPLEFT", 10, -10)
 f.buildbtn:SetSize(25,25)
 f.buildbtn:SetIcon(tx_build)
 f.buildbtn.tooltiplines = {L["buildframe"]}
 f.buildbtn.clickfunc = function()
     build_mode = not build_mode
	 buildframecheck()
	 local h = f.build_frame:GetHeight()
	 if h < 170 then
	  f.build_frame:SetPoint("BOTTOMRIGHT",f,"BOTTOMLEFT", 0, -( 170 - h ))
	 end
  end
-- build frame
 -- build frame icon
 f.build_frame = CreateFrame("FRAME", nil, f)
 f.build_frame:SetPoint("TOPLEFT",f,"TOPLEFT", -150, 0)
 f.build_frame:SetPoint("BOTTOMRIGHT",f,"BOTTOMLEFT")
 f.build_frame.tex = f.build_frame:CreateTexture()
 f.build_frame.tex:SetAllPoints()
 f.build_frame.tex:SetTexture(0,0,0,1)
 -- quest ID editbox
 f.build_frame.edtqID = addonTable:editboxtemplate(f.build_frame, L["b_questid"])
 f.build_frame.edtqID:SetPoint("TOPLEFT", f.build_frame, "TOPLEFT", 5, -15)
 f.build_frame.edtqID:SetScript("OnEnterPressed", function(self) f.build_frame.edtqName:SetFocus() end)
 local lasttext = ""
 f.build_frame.edtqID:SetScript("OnTextChanged", function(self)
   local text = self:GetText()
   if strfind(text, "%D") then
    self:SetText(lasttext)
   else
    lasttext = text
   end
  end)
 -- quest name editbox
 f.build_frame.edtqName = addonTable:editboxtemplate(f.build_frame, L["b_questname"])
 f.build_frame.edtqName:SetPoint("TOPLEFT", f.build_frame.edtqID, "BOTTOMLEFT", 0, -15)
 f.build_frame.edtqName:SetScript("OnEnterPressed", function(self) f.build_frame.edtpref:SetFocus() end)
 -- prefix editbox
 f.build_frame.edtpref = addonTable:editboxtemplate(f.build_frame, L["b_prefix"])
 f.build_frame.edtpref:SetPoint("TOPLEFT", f.build_frame.edtqName, "BOTTOMLEFT", 0, -15)
 f.build_frame.edtpref:SetScript("OnEnterPressed", function(self) f.build_frame.edtsuff:SetFocus() end)
 -- suffix editbox
 f.build_frame.edtsuff = addonTable:editboxtemplate(f.build_frame, L["b_suffix"])
 f.build_frame.edtsuff:SetPoint("TOPLEFT", f.build_frame.edtpref, "BOTTOMLEFT", 0, -15)
 f.build_frame.edtsuff:SetScript("OnEnterPressed", function(self) self:ClearFocus() end)
 -- add button
 f.build_frame.addbtn = addonTable:largebuttontemplate(f.build_frame, L["add"], L["add"])
 f.build_frame.addbtn:SetPoint("TOPRIGHT")
 f.build_frame.addbtn:SetBackdropColor(0,1,0,0.8)
 f.build_frame.addbtn:SetScript("OnMouseUp", function()
    addName = nil
	addID = nil
    EasyMenu(addmenu, DGCMenuFrame, "cursor", -100 , 0, "MENU")
  end)
 -- remove button
 f.build_frame.rembtn = addonTable:largebuttontemplate(f.build_frame, L["rem"], L["rem"])
 f.build_frame.rembtn:SetPoint("TOPRIGHT",f.build_frame,"TOPRIGHT",0,-25)
 f.build_frame.rembtn:SetBackdropColor(1,0,0,0.8)
 f.build_frame.rembtn:SetScript("OnMouseUp", function()
    GenerateRemoveButtonMenu()
    EasyMenu(remmenu, DGCMenuFrame, "cursor", -100 , 0, "MENU")
  end)
 f.build_frame:Hide()
 -- build frame end
 
-- TomTom button
if TomTom then
 f.TomTombutton = addonTable:largebuttontemplate(f, "TomTom", L["tomtom_button"])
 f.TomTombutton:SetPoint("BOTTOMLEFT",f,"BOTTOMLEFT",5,40)
 f.TomTombutton:SetBackdropColor(1,0,0.3,1) 
 f.TomTombutton:SetScript("OnMouseUp", function()
   if DGCMenuFrame then
    if DGCMenuFrame:IsVisible() then
     DGCMenuFrame:Hide()
    end
    DGCMenuFrame:SetPoint("TOP",f.TomTombutton,"BOTTOM",0,-5)
    EasyMenu(DGCTTmenu, DGCMenuFrame, DGCMenuFrame, 0 , 0, "MENU")
   end
   end)
end
 -- close button
  f.closebtn = addonTable:GetListItem(f)
  f.closebtn:SetPoint("BOTTOM", f, "BOTTOM",0,10)
  f.closebtn:SetSize(100,25)
  f.closebtn:SetText(CLOSE)
  f.closebtn.clickfunc = function()
                        if f.frames then
                         for _, v in pairs(f.frames) do v:Hide() end
                        end
                        f:Hide()
                        if DGCMenuFrame then DGCMenuFrame:Hide() end
                       end
  f:SetSize(300,150)
  DrawFrame(f, CENTER, false)
  buildframecheck()
  f:Show()
 end
end

--- world map section
DailyGlobalCheck_mapframe = CreateFrame("Frame")
DailyGlobalCheck_mapframe:SetParent(WorldMapButton)
DailyGlobalCheck_mapframe:SetAllPoints()
DailyGlobalCheck_mapframe.framespool = {}
CreateFrame("GameTooltip","DailyGlobalCheck_maptooltip", nil, "GameTooltipTemplate")
DailyGlobalCheck_maptooltip:SetFrameStrata("TOOLTIP")
DailyGlobalCheck_maptooltip:SetScale(0.8)

local function createmapbutton()
 local f = CreateFrame("FRAME")
 f:SetParent(DailyGlobalCheck_mapframe)
 f:SetPoint("CENTER",DailyGlobalCheck_mapframe,"CENTER")
 f.data = {}
 f:SetScript("OnMouseUp", function(_, mousebutton)
   --if IsControlKeyDown() and mousebutton == "LeftButton" then
    -- actually TomTom can set waypoints by Ctrl-Rightclicking, so no need of it
    --return
   --end
   WorldMapButton_OnClick(WorldMapButton, mousebutton)
  end)
 f:SetScript("OnEnter", function()
  DailyGlobalCheck_maptooltip:SetOwner(f,"ANCHOR_BOTTOM")
  DailyGlobalCheck_maptooltip:ClearLines()
  DailyGlobalCheck_maptooltip:AddLine(questsdata[f.data][PREF].." "..questsdata[f.data][NAME])
  DailyGlobalCheck_maptooltip:AddLine(questsdata[f.data][SUFF])
  DailyGlobalCheck_maptooltip:Show()
 end)
 f:SetScript("OnLeave", function()
  DailyGlobalCheck_maptooltip:Hide()
 end)
 f.tex = f:CreateTexture()
 f.tex:SetAllPoints()
 table.insert(DailyGlobalCheck_mapframe.framespool,f)
 return f
end

local mapID
local active_buttons = 1
local function DGCworldmapupdate()
  DailyGlobalCheck_mapframe:Hide()
  DailyGlobalCheck_maptooltip:Hide()
  if not DailyGlobalCheck_Options["show_mapicons"] or GetCurrentMapContinent() == WORLDMAP_COSMIC_ID 
     or GetNumDungeonMapLevels() ~= 0 then --or not WorldMapButton:IsVisible() then
   return
  end

  if mapID ~= GetCurrentMapAreaID() then
   table.foreach(DailyGlobalCheck_mapframe.framespool,function(k,v) v:Hide() end)
   active_buttons = 1
   mapID = GetCurrentMapAreaID()
   if mapstable[mapID] then
    table.foreach(mapstable[mapID], function(_,v)
     if not isquestcompleted(v) and questsdata[v] and isshown(v) then
      if questsdata[v][MAPDATA] and questsdata[v][MAPDATA][mapID]  then
       local tex = questsdata[v][MAPICON] and questsdata[v][MAPICON] or pfme
       -- pick up an unused frame or create a new one
       local f = DailyGlobalCheck_mapframe.framespool[active_buttons] ~= nil and DailyGlobalCheck_mapframe.framespool[active_buttons] or createmapbutton()
       local x = questsdata[v][MAPDATA][mapID][1]
       local y = questsdata[v][MAPDATA][mapID][2]
       f:SetWidth(questsdata[v][MAPDEFAULT] == mapID and 22 or 14)
       f:SetHeight(questsdata[v][MAPDEFAULT] == mapID and 22 or 14)
       f:SetPoint("CENTER",DailyGlobalCheck_mapframe,"TOPLEFT",
         -- I simply treat coordinates as a percentage of the width/height of the map frame
              DailyGlobalCheck_mapframe:GetWidth()  * x / 100,
             -DailyGlobalCheck_mapframe:GetHeight() * y / 100)
       f.data = v
	   if questsdata[v][MAPICON] then
        f.tex:SetTexture(questsdata[v][MAPICON],1)
	   else
	    f.tex:SetTexture(dgcicon,1)
	   end
       f:Show()
       active_buttons = active_buttons + 1
      end
     end
    end)
   end
  end

  if active_buttons > 1 then
   DailyGlobalCheck_mapframe:Show()
  end
end

-- resizing the map through those buttons does not fire the world_map_update event,
-- even if that changes the mapID to the player's current map, so I hook them
WorldMapFrameSizeDownButton:HookScript("OnClick", DGCworldmapupdate)
WorldMapFrameSizeUpButton:HookScript("OnClick", DGCworldmapupdate)
--- world map section end

addonTable.refresh = function(plugin)
 if SelectedList == plugin then
  OpenList(plugin)
 end
end

addonTable.forcemapupdate = function()
 mapID = -1
 DGCworldmapupdate()
end

local ldbset = false
local lastselset = false
local eventframe = CreateFrame("FRAME","DGCEventFrame")
eventframe:RegisterEvent("WORLD_MAP_UPDATE")
eventframe:RegisterEvent("PLAYER_LOGIN")
local function eventhandler(self, event, ...)
 if event == "WORLD_MAP_UPDATE" and WorldMapFrame:IsVisible() then
  DGCworldmapupdate()
 elseif event == "PLAYER_LOGIN" then
  if DailyGlobalCheck_Options == nil then
   DailyGlobalCheck_Options = {}
  end
  if DailyGlobalCheck_Options["show_questtype"] == nil then -- show quest type
   DailyGlobalCheck_Options["show_questtype"] = false
  end
  if DailyGlobalCheck_Options["show_coordinates"] == nil then -- show coords
   DailyGlobalCheck_Options["show_coordinates"] = true
  end
  if DailyGlobalCheck_Options["show_zone"] == nil then -- show zone name
   DailyGlobalCheck_Options["show_zone"] = false
  end
  if DailyGlobalCheck_Options["show_prefix"] == nil then -- show prefix
   DailyGlobalCheck_Options["show_prefix"] = false
  end
  if DailyGlobalCheck_Options["show_suffix"] == nil then -- show suffix
   DailyGlobalCheck_Options["show_suffix"] = false
  end
  if DailyGlobalCheck_Options["show_mapicons"] == nil then -- show world map icons
   DailyGlobalCheck_Options["show_mapicons"] = true
  end
  if DailyGlobalCheck_Options["show_questlogbtn"] == nil then -- show suffix
   DailyGlobalCheck_Options["show_questlogbtn"] = true
  end
  if DailyGlobalCheck_Options["show_questlogbtn"] then
   DGC_AddQuestbtn:Show() else DGC_AddQuestbtn:Hide()
  end
  if not DGC_CustomLists["Main"] then
   DGC_CustomLists["Main"] = CustomList else
   CustomList = DGC_CustomLists["Main"]
  end
  -- saved selected list
  if not lastselset then
   local lastsel = DailyGlobalCheck_Options["last_selected"]
   if lastsel then 
    OpenList(lastsel)
   elseif CustomList then
    OpenList(CustomList)
   elseif table.getn(addonTable.Plugins) > 0 then
    OpenList(addonTable.Plugins[1])
   end
   lastselset = true
  end
  
  --- TomTom integration ---
  --if TomTom then
  DGCMenuFrame = CreateFrame("Frame", "DGCMenuFrame", UIParent, "UIDropDownMenuTemplate")
  --end
  ---
  if DailyGlobalCheck_Options["minimap_icon"] == nil then -- show minimap icon
    DailyGlobalCheck_Options["minimap_icon"] = {
        hide = false,
        minimapPos = 220,
    }
  end
  DailyGlobalCheck_Options["DGC_Version"] = version

  if ldb and not ldbset then
        local DailyGlobalCheck_broker = ldb:NewDataObject("DailyGlobalCheck_broker", {
	        type = "data source",
	        icon = dgcicon,
	        label = "Daily Global Check",
	        OnClick = function(self,button)
			 if button == "LeftButton" then
               DGCInit()
			 elseif button == "RightButton" then
			  GenerateMinimapMenu()
			 end
	        end,
	        OnTooltipShow = function(tooltip)
			 if SelectedList then
			  tooltip:ClearLines()
			  tooltip:AddLine(addontitle..AddColor(" ("..SelectedList["Title"]..")",LIGHT_BLUE))
		      DrawFrame(tooltip,CENTER,true)
			 end
	    end,
        })
        if ldbicon then
            ldbicon:Register("DailyGlobalCheck_broker", DailyGlobalCheck_broker, DailyGlobalCheck_Options["minimap_icon"])
        end
   ldbset = true
  end
 end
end
eventframe:SetScript("OnEvent", eventhandler)

-- "Add quest to Custom List" button in the quest log frame
 CreateFrame("Button", "DGC_AddQuestbtn", QuestLogFrame)
 DGC_AddQuestbtn:SetPoint("RIGHT", QuestLogFrameShowMapButton, "LEFT", -10, 0)
 DGC_AddQuestbtn:SetSize(25, 25)
 DGC_AddQuestbtn.tex = DGC_AddQuestbtn:CreateTexture()
 DGC_AddQuestbtn.tex:SetAllPoints()
 DGC_AddQuestbtn.tex:SetTexture(dgcicon)
 DGC_AddQuestbtn:SetHighlightTexture("Interface\\buttons\\UI-Listbox-Highlight2", "ADD")
 DGC_AddQuestbtn:SetScript("OnEnter", function(self)
   GameTooltip:ClearLines()
   GameTooltip:SetOwner(self,"ANCHOR_TOP",0,5)
   GameTooltip:AddLine(L["addbtntooltip"])
   GameTooltip:Show()
  end)
 DGC_AddQuestbtn:SetScript("OnLeave", function() GameTooltip:Hide() end)
 DGC_AddQuestbtn:SetScript("OnClick", function()
   local sel = GetQuestLogSelection()
   local s = GetQuestLink(sel)
   if s then
    local qID = string.match(s, "Hquest:(%d+)")
    local name = GetQuestLogTitle(sel)
	if qID and name then
	 addID = qID
	 addName = name
	 EasyMenu(addmenu, DGCMenuFrame, "cursor", -100 , 0, "MENU")
	end
   end
  end)
 

-- slash command
SLASH_DAILYGLOBALCHECK1 = "/dgcheck"
SLASH_DAILYGLOBALCHECK2 = "/dgc"
SlashCmdList["DAILYGLOBALCHECK"] = DGCInit