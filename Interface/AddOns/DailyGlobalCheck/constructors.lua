local addonName, addonTable = ...
local btnheight = 25
local pool = {}

local function CreateSingleButton(p)
 local f = CreateFrame("Frame",nil,p)
 f:SetHeight(btnheight)

 f.btn = CreateFrame("Button",nil,f)
 f.btn:SetPoint("TOPLEFT", f, "TOPLEFT", 1, -1)
 f.btn:SetPoint("BOTTOMRIGHT", f, "BOTTOMRIGHT", -1, 1)
 f.btn.text = f.btn:CreateFontString(nil, "OVERLAY", "GameTooltipText")
 f.btn.text:SetPoint("CENTER")
 
 f.btn.tex = f.btn:CreateTexture(nil, "BACKGROUND")
 f.btn.tex:SetAllPoints()
 f.btn.tex:SetTexture(0.2,0.2,0.2,0.5)
 
 f.SetIcon = function(self,s)
              f.btn.tex:SetTexture(s)
             end
 f.SetText = function(self,s)
              f.btn.text:SetText(s)
             end
 f.Select =  function(self)
			  self.btn.tex:SetVertexColor(0, 0.7, 0);
             end
 f.Deselect = function(self)
			   self.btn.tex:SetVertexColor(1, 1, 1);
              end
 f.btn:SetPoint("CENTER", f, "CENTER")
 f.btn:SetSize(13,btnheight-2)
  f.btn:SetScript("OnClick", function()
    if f.clickfunc then f:clickfunc() end
 end)
 f.btn:SetScript("OnEnter", function()
   if f.tooltiplines then
	GameTooltip:SetOwner(f, "ANCHOR_NONE")
	GameTooltip:SetPoint("BOTTOM", f, "TOP", 0, 5)
    GameTooltip:ClearLines()
	GameTooltip:AddLine("")
    table.foreach(f.tooltiplines, function(k,v)
	 GameTooltip:AddLine(v)
	end)
	GameTooltip:Show()
   end
  end)
 f.btn:SetScript("OnLeave", function()
    GameTooltip:Hide()
 end)

 f.btn:SetHighlightTexture("Interface\\buttons\\UI-Listbox-Highlight2", "ADD")
 table.insert(pool, f)
 return f
end

local function CreateListItem(p)
-- background frame
 local f
 local j
 for _,j in pairs(pool) do
  if not j.used then
   f = j
   break
  end
 end
 if not f then f = CreateSingleButton(p) end
 f.used = true
 return f
end

local frames_backdrop = {bgFile = "Interface/Tooltips/UI-Tooltip-Background", 
                         edgeFile = "Interface/Tooltips/UI-Tooltip-Border", 
                         tile = true, tileSize = 16, edgeSize = 16, 
                         insets = { left = 4, right = 4, top = 4, bottom = 4 }}

function addonTable:largebuttontemplate(p, text, tooltiptext)
  result = CreateFrame("FRAME", nil, p)
  result:SetWidth(60)
  result:SetHeight(25)
  result.text = result:CreateFontString(nil, "OVERLAY")
  result.text:SetAllPoints()
  result.text:SetFont("Fonts\\FRIZQT__.TTF", 12)
  result.text:SetText(text)
  result:SetBackdrop(frames_backdrop)
  addonTable:SetTooltip(result, {tooltiptext})
  return result
end

function addonTable:editboxtemplate(p, title)
 local edt = CreateFrame("EditBox", nil, p)
 edt.lbl = p:CreateFontString()
 edt.lbl:SetFont("Fonts\\ARIALN.ttf", 10, "")
 edt.lbl:SetText(title)
 edt.lbl:SetPoint("BOTTOMLEFT", edt, "TOPLEFT", 0, 0) 
 edt:SetFontObject("GameFontHighlight")
 edt:SetWidth(60)
 edt:SetHeight(25)
 edt:SetBackdrop(GameTooltip:GetBackdrop())
 edt:SetBackdropColor(0, 0, 0, 0.8)
 edt:SetBackdropBorderColor(0.6, 0.6, 0.6, 1)
 edt:SetAutoFocus(false)
 edt:SetScript("OnEscapePressed", function(self) self:ClearFocus()  end)
 edt:ClearFocus()
 return edt
end
						 
function addonTable:SetTooltip(f, l)
 f:SetScript("OnLeave", function(self)
    GameTooltip:Hide()
   end)
 f:SetScript("OnEnter", function(self)
    GameTooltip:SetOwner(f,"ANCHOR_TOP",0,5)
    GameTooltip:ClearLines()
	table.foreach(l, function(_,v)
     GameTooltip:AddLine(v)
	end)
    GameTooltip:Show()
   end)
end
						 
function addonTable:SetupFrame(f)
 f:SetBackdrop(frames_backdrop)
 f:SetBackdropColor(0,0,0,0.8)
 f:SetFrameStrata("HIGH")
 f.leftfont = f:CreateFontString(nil, "OVERLAY", "GameTooltipText")
 f.rightfont = f:CreateFontString(nil, "OVERLAY", "GameTooltipText")
 f.leftfont:SetPoint("TOPLEFT",5,-5)
 f.rightfont:SetPoint("TOPRIGHT",-5,-5)
end

function addonTable:GetListItem(p)
 return CreateListItem(p)
end