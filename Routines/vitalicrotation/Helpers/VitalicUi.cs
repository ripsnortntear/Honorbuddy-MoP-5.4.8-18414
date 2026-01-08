using System;
using System.Drawing;
using System.Globalization;
using Styx.WoWInternals;
using VitalicRotation.Helpers;
using VitalicRotation.Managers;
using VitalicRotation.Settings;

namespace VitalicRotation.UI
{
    internal static class VitalicUi
    {
        private static bool _bannerLuaInjected = false;

        public static void EnsureAll()
        {
            EnsureStatusFrame_Exact();
            EnsureZoneTextAssets();
            EnsureNotifyFrame();
            EnsureLowHealthFlash();
            ForceShowStatus();
            HideNotify();
        }

        // Permet de forcer l'initialisation et l'état (fonts/alerts) même si déjà injecté
        public static void InitializeBannerLuaInjected(bool enableFonts, bool enableAlerts)
        {
            try
            {
                EnsureBannerLuaInjected(enableFonts, enableAlerts);
                UpdateBannerFonts(enableFonts);
                UpdateBannerAlerts(enableAlerts);
            }
            catch { }
        }

        // --- Helper pour échapper le texte côté Lua (simple et suffisant ici)
        private static string EscapeLua(string s)
        {
            if (s == null) return string.Empty;
            return s.Replace("\\", "\\\\").Replace("'", "\\'");
        }

        /// <summary>
        /// One-shot Lua injection for zone banner system
        /// </summary>
        public static void EnsureBannerLuaInjected(bool enableFonts, bool enableAlerts)
        {
            // If we think we're injected, verify the Lua table still exists (could have been cleared during cleanup)
            if (_bannerLuaInjected)
            {
                try
                {
                    var present = Lua.GetReturnVal<string>("return tostring(_G.VitalicBanner ~= nil)", 0);
                    if (string.Equals(present, "true", StringComparison.OrdinalIgnoreCase))
                    {
                        // Still present — just apply runtime flags and bail
                        Lua.DoString("_G.Vitalic_EnableAlerts = " + (enableAlerts ? "true" : "false") + ";");
                        if (enableFonts)
                            Lua.DoString("if VitalicBanner and VitalicBanner.ApplyFonts then VitalicBanner.ApplyFonts(true) end");
                        return;
                    }
                    // Lua state lost — force reinjection
                    _bannerLuaInjected = false;
                }
                catch
                {
                    // On any error, try to reinject
                    _bannerLuaInjected = false;
                }
            }

            string lua = @"
-- Table globale pour stocker fontes et helpers
VitalicBanner = VitalicBanner or {};

-- Sauvegarde des fontes actuelles pour restoration
local zf, zs, zflag = ZoneTextString:GetFont();
local pf, ps, pflag = PVPInfoTextString:GetFont();
VitalicBanner.orig = { zf=zf, zs=zs, zflag=zflag, pf=pf, ps=ps, pflag=pflag };

-- Applique ou restaure les fontes
VitalicBanner.ApplyFonts = function(apply)
  if apply then
        -- Parité v.zip (fonts calibrib/FRIZ gérées ailleurs via EnsureZoneTextAssets)
        ZoneTextString:SetFont([[Fonts\\calibrib.ttf]], 28, 'OUTLINE');
        PVPInfoTextString:SetFont([[Fonts\\calibrib.ttf]], 20, 'OUTLINE');
  else
    ZoneTextString:SetFont(VitalicBanner.orig.zf, VitalicBanner.orig.zs, VitalicBanner.orig.zflag);
    PVPInfoTextString:SetFont(VitalicBanner.orig.pf, VitalicBanner.orig.ps, VitalicBanner.orig.pflag);
  end
end

-- Affiche un texte via ZoneTextFrame avec fade/hold natif
VitalicBanner.ShowZoneText = function(msg, r, g, b, hold)
  if not msg or msg == '' then return end
  ZoneTextFrame:Hide();
  ZoneTextString:SetText(msg);
  if r and g and b then ZoneTextString:SetTextColor(r, g, b) end
  -- Timings d'animation
    ZoneTextFrame.fadeInTime = 0;
    ZoneTextFrame.holdTime   = hold or 1.0;
    ZoneTextFrame.fadeOutTime= 2.0;
  ZoneTextFrame:Show();
end

-- Affiche un texte via PVPInfoTextFrame (plus petit)
VitalicBanner.ShowPvpText = function(msg, r, g, b, hold)
  if not msg or msg == '' then return end
  PVPInfoTextFrame:Hide();
  PVPInfoTextString:SetText(msg);
  if r and g and b then PVPInfoTextString:SetTextColor(r, g, b) end
  PVPInfoTextFrame.fadeInTime = 0.3;
  PVPInfoTextFrame.holdTime   = hold or 1.2;
  PVPInfoTextFrame.fadeOutTime= 0.6;
  PVPInfoTextFrame:Show();
end

-- Fallback mini autonome (si PVPInfoTextFrame absent)
VitalicBanner.EnsureMini = function()
  if _G.PVPInfoTextFrame and _G.PVPInfoTextString then
    VitalicBanner.MiniFrame  = _G.PVPInfoTextFrame
    VitalicBanner.MiniString = _G.PVPInfoTextString
    return
  end
  if not VitalicBanner.MiniFrame then
    local f = CreateFrame('Frame', 'VitalicMiniFrame', UIParent)
    f:SetFrameStrata('HIGH')
    f:SetSize(640, 64)
    f:SetPoint('CENTER', UIParent, 'CENTER', 0, 120)
    local fs = f:CreateFontString(nil, 'OVERLAY', 'GameFontNormalHuge')
    fs:SetAllPoints(true)
    fs:SetJustifyH('CENTER')
    fs:SetJustifyV('MIDDLE')
    f.text = fs
    f:Hide()
    VitalicBanner.MiniFrame  = f
    VitalicBanner.MiniString = fs
  end
end

VitalicBanner.ShowMiniSimple = function(msg, hold)
  if not msg or msg == '' then return end
  VitalicBanner.EnsureMini()
  local f  = VitalicBanner.MiniFrame
  local fs = VitalicBanner.MiniString
  if not f or not fs then return end
  fs:SetText(msg)
  if fs.SetTextColor then fs:SetTextColor(1, 0.82, 0) end
  f:Show()
  f:SetAlpha(1)

  -- Fade/hold simple (compatible MoP sans dépendre de C_Timer)
  if f._vbTicker then f:SetScript('OnUpdate', nil); f._vbTicker = nil end
  local t, h, fo = 0, (hold or 1.2), 0.4
  f:SetScript('OnUpdate', function(self, elapsed)
    t = t + elapsed
    if t >= h and t < h + fo then
      local a = 1 - ((t - h) / fo); if a < 0 then a = 0 end
      self:SetAlpha(a)
    elseif t >= h + fo then
      self:SetScript('OnUpdate', nil)
      self:Hide()
    end
  end)
end

-- (debug optionnel)
-- (pas de hooks debug dans la version originale)

-- Pas de hooks d'événements additionnels (parité v.zip)

-- Hook Lua pour voir si quelque chose masque la mini-bannière
-- Pas de hooks debug PVPInfoTextFrame
";

            Lua.DoString(lua);

            // Passer les flags initiaux depuis les settings
            Lua.DoString("_G.Vitalic_EnableAlerts = " + (enableAlerts ? "true" : "false") + ";");
            if (enableFonts)
                Lua.DoString("if VitalicBanner and VitalicBanner.ApplyFonts then VitalicBanner.ApplyFonts(true) end");

            _bannerLuaInjected = true;
        }

        /// <summary>
        /// Assure la création de l'icône ZoneTextFrame et applique les fontes identiques à v.zip
        /// </summary>
        private static void EnsureZoneTextAssets()
        {
            try
            {
                Lua.DoString(@"
-- icône sur ZoneTextFrame (v.zip)
if not ZoneTextFrame.icon then
    ZoneTextFrame.icon = ZoneTextFrame:CreateTexture('ZoneTextFrameIcon', 0);
    ZoneTextFrame:SetScript('OnHide', function() ZoneTextFrame.icon:SetTexture(nil) end);
    ZoneTextFrame.icon:SetWidth(35); ZoneTextFrame.icon:SetHeight(35);
    ZoneTextFrame.icon:SetTexCoord(.08, .92, .08, .92);
end
");
                // Applique les fontes comme dans l'original: calibrib quand AlertFontsEnabled, sinon FRIZ par défaut
                if (VitalicSettings.Instance.AlertFontsEnabled)
                {
                    Lua.DoString("ZoneTextString:SetFont([[Fonts\\calibrib.ttf]], 28, 'OUTLINE'); PVPInfoTextString:SetFont([[Fonts\\calibrib.ttf]], 20, 'OUTLINE'); SubZoneTextString:SetFont([[Fonts\\calibrib.ttf]], 20, 'OUTLINE'); UIErrorsFrame:SetFont([[Fonts\\calibrib.ttf]], 16, 'OUTLINE')");
                }
                else
                {
                    Lua.DoString("ZoneTextString:SetFont([[Fonts\\FRIZQT__.TTF]], 28, 'THICKOUTLINE'); PVPInfoTextString:SetFont([[Fonts\\FRIZQT__.TTF]], 22, 'OUTLINE, THICKOUTLINE'); SubZoneTextString:SetFont([[Fonts\\FRIZQT__.TTF]], 26, 'THICKOUTLINE'); UIErrorsFrame:SetFont([[Fonts\\FRIZQT__.TTF]], 16)");
                }
            }
            catch { }
        }

        // Exact v.zip: create font and status frame near chat with drag handlers + exact anchor
        public static void EnsureStatusFrame_Exact()
        {
            try
            {
                Lua.DoString(@"
    -- Font 'v' strictement identique à v.zip
    if not v then
        CreateFont('v')
        v:SetFont('Fonts\\calibrib.ttf', 12, 'OUTLINE')
        v:SetJustifyH('LEFT')
    else
        v:SetFont('Fonts\\calibrib.ttf', 12, 'OUTLINE')
        v:SetJustifyH('LEFT')
    end

    -- Status frame (sf) : même taille/strata/drag
    if not sf then
        local f = CreateFrame('Frame','sf',UIParent)
        f:SetSize(300, 12)
        f:SetFrameStrata('HIGH')
        f:EnableMouse(true)
        f:SetMovable(true)
        f:RegisterForDrag('LeftButton')
        f:SetScript('OnDragStart', function(self) self:StartMoving() end)
    f:SetScript('OnDragStop',  function(self) self:StopMovingOrSizing(); StatusFrameMoved = true end)
        f.text = f:CreateFontString(nil,'BACKGROUND','v')
        f.text:SetAllPoints()
        sf = f
    end
    sf:ClearAllPoints()
    sf:SetPoint('CENTER', GeneralDockManager, -26, 20)
");
            }
            catch { }
        }

        public static void StatusFrameSetText_Raw(string coloredText)
        {
            if (coloredText == null) coloredText = string.Empty;
            var s = coloredText.Replace("\\", "\\\\").Replace("'", "\\'");
            try { Lua.DoString("if sf then sf.text:SetText('" + s + "') end"); } catch { }
        }

        public static void StatusFrameMove_Exact(double left, double bottom)
        {
            try
            {
                var L = left.ToString(System.Globalization.CultureInfo.InvariantCulture);
                var B = bottom.ToString(System.Globalization.CultureInfo.InvariantCulture);
                Lua.DoString("if sf then sf:ClearAllPoints() sf:SetPoint('BOTTOMLEFT', " + L + ", " + B + ") end");
            }
            catch { }
        }

        public static void StatusFrameShutdown_Exact()
        {
            try { Lua.DoString("if sf then sf:Hide() sf = nil end"); } catch { }
        }

        // Low health red flash overlay driven by ff (same frame), v.zip behavior
        public static void EnsureLowHealthFlash()
        {
            try
            {
                Lua.DoString(@"
            if ff and not ff.texture then
                ff.texture = ff:CreateTexture(nil, 'BACKGROUND')
                ff.texture:SetAllPoints(ff)
                ff.texture:SetTexture([[Interface\FullScreenTextures\LowHealth]])
                ff.texture:SetAlpha(0)
            end
            if ff and not ff.onupdate then
                ff.onupdate = true
                local t, dir = 0, 1
                ff:SetScript('OnUpdate', function(self, elapsed)
                    local hp = (UnitHealth('player') / max(1, UnitHealthMax('player'))) * 100
                    if hp <= " + VitalicRotation.Settings.VitalicSettings.Instance.LowHealthWarning.ToString(System.Globalization.CultureInfo.InvariantCulture) + @" then
                        t = t + elapsed * dir * 2.0
                        if t > 1 then t = 1; dir = -1 elseif t < 0 then t = 0; dir = 1 end
                        self.texture:SetAlpha(t * 0.8)
                        self:Show()
                    else
                        self.texture:SetAlpha(0)
                        self:Hide()
                    end
                end)
            end
        ");
            }
            catch { }
        }

        /// <summary>
        /// Update banner alert state at runtime
        /// </summary>
        public static void UpdateBannerAlerts(bool enable)
        {
            try
            {
                Lua.DoString("_G.Vitalic_EnableAlerts = " + (enable ? "true" : "false") + ";");
            }
            catch { }
        }

        /// <summary>
        /// Update banner fonts at runtime
        /// </summary>
        public static void UpdateBannerFonts(bool enable)
        {
            try
            {
                Lua.DoString("if VitalicBanner and VitalicBanner.ApplyFonts then VitalicBanner.ApplyFonts(" + (enable ? "true" : "false") + ") end");
            }
            catch { }
        }

        public static void OnAlertFontsSettingChanged()
        {
            UpdateBannerFonts(VitalicSettings.Instance.AlertFontsEnabled);
        }

        public static void OnSpellAlertsSettingChanged()
        {
            UpdateBannerAlerts(VitalicSettings.Instance.SpellAlertsEnabled);
        }

        public static void ShowBigBanner(string msg, float r = 1f, float g = 1f, float b = 1f, float hold = 1.5f)
        {
            if (!VitalicSettings.Instance.SpellAlertsEnabled) return;
            try
            {
                var esc = msg.Replace("\"", "\\\"").Replace("'", "\\'");
                var rStr = r.ToString(CultureInfo.InvariantCulture);
                var gStr = g.ToString(CultureInfo.InvariantCulture);
                var bStr = b.ToString(CultureInfo.InvariantCulture);
                var holdStr = hold.ToString(CultureInfo.InvariantCulture);

                Lua.DoString("if VitalicBanner and VitalicBanner.ShowZoneText then VitalicBanner.ShowZoneText(\"" + esc + "\", " + rStr + ", " + gStr + ", " + bStr + ", " + holdStr + ") end");
            }
            catch { }
        }

        public static void ShowMiniBanner(string msg, float r = 1f, float g = 1f, float b = 1f, float hold = 1.2f)
        {
            // Mini-banners DO display even if Spell Alerts are disabled (parité v.zip)
            try
            {
                var esc = msg.Replace("\"", "\\\"").Replace("'", "\\'");
                var rStr = r.ToString(CultureInfo.InvariantCulture);
                var gStr = g.ToString(CultureInfo.InvariantCulture);
                var bStr = b.ToString(CultureInfo.InvariantCulture);
                var holdStr = hold.ToString(CultureInfo.InvariantCulture);

                Lua.DoString("if VitalicBanner and VitalicBanner.EnsureMini then VitalicBanner.EnsureMini() end; if VitalicBanner and VitalicBanner.ShowMiniSimple then VitalicBanner.ShowMiniSimple(\"" + esc + "\"," + holdStr + ") elseif VitalicBanner and VitalicBanner.ShowPvpText then VitalicBanner.ShowPvpText(\"" + esc + "\", " + rStr + ", " + gStr + ", " + bStr + ", " + holdStr + ") end");
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                {
                    Logger.Write("[UI][Mini] call: \"{0}\"", msg ?? "");
                    DebugDumpBanner("pre-mini");
                }

                // petit état après l'appel
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                {
                    var ret = Lua.GetReturnValues("return tostring(_G.PVPInfoTextFrame and _G.PVPInfoTextFrame:IsShown() and 1 or 0)");
                    Logger.Write("[UI][Mini] post: PVPShown={0}", ret.Count > 0 ? ret[0] : "?");
                }
            }
            catch (Exception e)
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[UI][Mini] EX: {0}", e.Message);
            }
        }

        public static void RestoreFontsIfNeeded()
        {
            try
            {
                Lua.DoString(@"
if VitalicBanner and VitalicBanner.ApplyFonts then
  VitalicBanner.ApplyFonts(false);
end");
            }
            catch { }
        }

        public static void KillBannerSystem()
        {
            try
            {
                Lua.DoString(@"
            if VitalicBanner and VitalicBanner.ApplyFonts then
                VitalicBanner.ApplyFonts(false);
            end
            if VitalicBanner and VitalicBanner.frame then
                VitalicBanner.frame:UnregisterAllEvents();
                VitalicBanner.frame:SetScript('OnEvent', nil);
                VitalicBanner.frame:Hide();
                VitalicBanner.frame = nil;
            end
            VitalicBanner = nil;
            ZoneTextFrame:Hide();
            PVPInfoTextFrame:Hide();
        ");
            }
            catch { }
            finally
            {
                // Ensure we will reinject on next Initialize
                _bannerLuaInjected = false;
            }
        }

        public static void EnsureStatusFrame()
        {
            try
            {
                Lua.DoString(@"
                    if not v then
                        CreateFont('v')
                        v:SetFont('Fonts\\calibrib.ttf', 12, 'OUTLINE')
                        v:SetJustifyH('LEFT')
                    else
                        v:SetFont('Fonts\\calibrib.ttf', 12, 'OUTLINE')
                        v:SetJustifyH('LEFT')
                    end
                    if not sf then
                        local f = CreateFrame('Frame','sf',UIParent)
                        f:SetHeight(12)
                        f:SetWidth(300)
                        f:SetFrameStrata('HIGH')
                        f:EnableMouse(true)
                        f:SetMovable(true)
                        f:RegisterForDrag('LeftButton')
                        f:SetScript('OnDragStart', function(self) self:StartMoving() end)
                        f:SetScript('OnDragStop',  function(self)
                            self:StopMovingOrSizing()
                            StatusFrameMoved = true
                        end)
                        f.text = f:CreateFontString(nil, 'BACKGROUND', 'v')
                        f.text:SetAllPoints()
                        f:Hide()
                    end

                    if sf and (not VitalicStatusFrameApplied) then
                        VitalicStatusFrameApplied = true
                        sf:ClearAllPoints()
                        if GeneralDockManager then
                            sf:SetPoint('CENTER', GeneralDockManager, -26, 20)
                        else
                            sf:SetPoint('CENTER', UIParent, 'CENTER', -220, -180)
                        end
                    end
                ");

                if (VitalicSettings.Instance.StatusFrameLeft != 0d || VitalicSettings.Instance.StatusFrameBottom != 0d)
                {
                    var left = VitalicSettings.Instance.StatusFrameLeft.ToString(CultureInfo.InvariantCulture);
                    var bottom = VitalicSettings.Instance.StatusFrameBottom.ToString(CultureInfo.InvariantCulture);
                    Lua.DoString("if sf then sf:ClearAllPoints(); sf:SetPoint('BOTTOMLEFT', UIParent, 'BOTTOMLEFT', " + left + ", " + bottom + ") end");
                }

                Lua.DoString("VitalicStatus = sf");
            }
            catch { }
        }

        public static void EnsureNotifyFrame()
        {
            try
            {
                Lua.DoString(@"
                    if not ff then
                        local f = CreateFrame('Frame','ff',UIParent)
                        f:SetSize(900, 60)
                        f:SetFrameStrata('FULLSCREEN_DIALOG')
                        f.text = f:CreateFontString(nil,'OVERLAY','GameFontHighlightHuge')
                        f.text:SetPoint('CENTER', 0, 0)
                        f.text:SetText('')
                        f.text:SetFont(STANDARD_TEXT_FONT, 28, 'OUTLINE')
                        f.text:SetShadowColor(0,0,0,0)
                        f:ClearAllPoints()
                        f:SetPoint('TOP', UIParent, 'TOP', 0, -60)
                        f:Hide()
                        ff = f
                    end
                ");
                Lua.DoString("VitalicNotify = ff");
            }
            catch { }
        }

        public static void ShowStatus()
        {
            try
            {
                if (!VitalicSettings.Instance.StatusFrameEnabled)
                    return;
                Lua.DoString("if sf then sf:Show() end");
            }
            catch { }
        }

        public static void HideStatus()
        {
            try { Lua.DoString("if sf then sf:Hide() end"); } catch { }
        }

        public static void ForceShowStatus()
        {
            try
            {
                if (!VitalicSettings.Instance.StatusFrameEnabled)
                    return;

                if (VitalicSettings.Instance.StatusFrameLeft != 0d || VitalicSettings.Instance.StatusFrameBottom != 0d)
                {
                    var left = VitalicSettings.Instance.StatusFrameLeft.ToString(CultureInfo.InvariantCulture);
                    var bottom = VitalicSettings.Instance.StatusFrameBottom.ToString(CultureInfo.InvariantCulture);
                    Lua.DoString("if sf then sf:ClearAllPoints(); sf:SetPoint('BOTTOMLEFT', UIParent, 'BOTTOMLEFT', " + left + ", " + bottom + ") end");
                }

                Lua.DoString("if sf then sf:Show() end");
            }
            catch { }
        }

        // Texte SEUL (cache l'icône si présente) — via ZoneTextFrame
        public static void ShowNotify(string text)
        {
            try
            {
                var msg = EscapeLua(text);
                Lua.DoString(@"
PVPInfoTextString:SetText('')
if ZoneTextFrame.icon then ZoneTextFrame.icon:SetTexture(nil); ZoneTextFrame.icon:Hide() end
ZoneTextString:SetText('" + msg + @"')
ZoneTextFrame.startTime = GetTime()
ZoneTextFrame.fadeInTime = 0; ZoneTextFrame.holdTime = 1; ZoneTextFrame.fadeOutTime = 2
ZoneTextString:SetTextColor(1, 0.82, 0)
ZoneTextFrame:Show()");
            }
            catch { }
        }

        // Texte + ICÔNE via spellId — via ZoneTextFram
        public static void ShowNotify(int spellId, string overrideText)
        {
            try
            {
                var msg = EscapeLua(overrideText ?? string.Empty);
                Lua.DoString(@"
local id = " + spellId + @"
local name,_,icon = GetSpellInfo(id)
PVPInfoTextString:SetText('')
if ZoneTextFrame.icon then
    if icon then ZoneTextFrame.icon:SetTexture(icon); ZoneTextFrame.icon:Show() else ZoneTextFrame.icon:Hide() end
end
local txt = '" + msg + @"'; if txt=='' then txt = (name or '') end
ZoneTextString:SetText(txt)
ZoneTextFrame.startTime = GetTime()
ZoneTextFrame.fadeInTime = 0; ZoneTextFrame.holdTime = 1; ZoneTextFrame.fadeOutTime = 2
ZoneTextString:SetTextColor(1, 0.82, 0)
if ZoneTextFrame.icon and ZoneTextString and ZoneTextString.GetWidth then
  ZoneTextFrame.icon:ClearAllPoints();
  ZoneTextFrame.icon:SetPoint('LEFT', ZoneTextString, ((ZoneTextString:GetWidth() / 2) - (ZoneTextString:GetStringWidth() / 2) - 43), 1)
end
ZoneTextFrame:Show()");
            }
            catch { }
        }

        public static void ShowNotify(string message, double seconds)
        {
            try
            {
                string safe = (message ?? string.Empty).Replace("\\", "\\\\").Replace("'", "\\'");
                string s = seconds.ToString("F2", CultureInfo.InvariantCulture);

                Lua.DoString(@"
PVPInfoTextString:SetText('')
if ZoneTextFrame.icon then ZoneTextFrame.icon:SetTexture(nil); ZoneTextFrame.icon:Hide() end
ZoneTextString:SetText('" + safe + @"')
ZoneTextFrame.startTime = GetTime()
ZoneTextFrame.fadeInTime = 0; ZoneTextFrame.holdTime = " + s + @"; ZoneTextFrame.fadeOutTime = 2
ZoneTextString:SetTextColor(1, 0.82, 0)
ZoneTextFrame:Show()");
            }
            catch { }
        }

        public static void HideNotify()
        {
            try { Lua.DoString("if ff then ff:Hide() end"); } catch { }
        }

        /// <summary>
        /// Status frame shutdown (v.zip: Class136.smethod_4)
        /// Hides status frame and sets to nil for complete cleanup
        /// </summary>
        public static void StatusFrameShutdown()
        {
            try { Lua.DoString("if sf then sf:Hide() sf = nil end", "WoW.lua"); } catch { }
            try { Lua.DoString("if sf then sf:Hide(); sf = nil; end", "WoW.lua"); } catch { }
        }

        /// <summary>
        /// Notify frame shutdown (v.zip: Class135.smethod_3)
        /// Hides notify frame and sets to nil for complete cleanup
        /// </summary>
        public static void NotifyFrameShutdown()
        {
            try { Lua.DoString("if ff then ff:Hide() ff = nil end", "WoW.lua"); } catch { }
        }

        public static void HideAll()
        {
            try { Lua.DoString("if ZoneTextFrame then ZoneTextFrame:Hide() end; if PVPInfoTextFrame then PVPInfoTextFrame:Hide() end"); } catch { }
            HideNotify();
            HideStatus();
        }

        // Stub pour compat : l'original n'affiche pas explicitement un "mode"
        public static void SetMode(string mode) { }

        // Ligne de statut fidèle v.zip : "Burst: Enabled  Lazy: Enabled  Macros: …"
        public static void UpdateStatusAuto()
        {
            try
            {
                if (!VitalicSettings.Instance.StatusFrameEnabled)
                    return;

                bool burst = ToggleState.IsBurstOn;
                bool lazy = ToggleState.IsLazyOn;
                bool showMacros = VitalicSettings.Instance.MacrosEnabled;
                // Vitalic ajoute un '*' après Enabled côté Burst selon certaines conditions (ex: NoShadowBlades)

                // Parité v.zip : lire depuis C# (CurrentQueuedSpell), pas via une fonction Lua dédiée
                string macro = "";
                try { macro = MacroManager.CurrentQueuedSpell; } catch { macro = ""; }

                string label = "|cffFFBE69";
                string ok = "|cFF00FF00";
                string bad = "|cffb73737";
                string warn = "|cffFF6600"; // Orange for NoBlades
                string reset = "|r";

                bool specialBurst = false;
                try { specialBurst = ToggleState.IsBurstOn && ToggleState.IsNoShadowBlades; } catch { }
                string burstPart = burst ? ok + (specialBurst ? "Enabled*" : "Enabled") + reset : bad + "Disabled" + reset;
                // When PvE mode is enabled, original shows "Enabled (PvE)" on the Lazy section
                bool pveMode = false;
                try { pveMode = VitalicSettings.Instance.PveMode; } catch { }
                string lazyPart = lazy
                    ? (pveMode ? ok + "Enabled (PvE)" + reset : ok + "Enabled" + reset)
                    : bad + "Disabled" + reset;

                string line;
                if (showMacros)
                {
                    string macroPart;
                    if (!string.IsNullOrEmpty(macro))
                    {
                        macroPart = ok + macro;
                        try
                        {
                            // Ajoute '*' si macro vise focus/mouseover (pas la cible courante), '(pool)' pour Garrote pooling
                            var kind = MacroManager.CurrentMacroTargetKind;
                            if (kind == MacroManager.MacroTargetKind.Focus || kind == MacroManager.MacroTargetKind.Mouseover)
                                macroPart += "*";
                            if (string.Equals(macro, "Garrote", StringComparison.Ordinal) && MacroManager.GarrotePoolingIndicator)
                                macroPart += " (pool)";
                            macroPart += reset;
                        }
                        catch { macroPart = ok + macro + reset; }
                    }
                    else
                    {
                        macroPart = bad + "None" + reset;
                    }
                    line = label + "Burst: " + burstPart
                         + "  " + label + "Lazy: " + lazyPart
                         + "  " + label + "Macros: " + macroPart;
                }
                else
                {
                    line = label + "Burst: " + burstPart
                         + "  " + label + "Lazy: " + lazyPart;
                }

                // No extra suffixes in original line

                string safe = line.Replace("\\", "\\\\").Replace("'", "\\'");
                Lua.DoString("if sf and sf.text then sf.text:SetText('" + safe + "'); sf:Show(); end");
            }
            catch { }
        }

        public static void Cleanup()
        {
            try
            {
                RestoreFontsIfNeeded();
                KillBannerSystem();
                try { Lua.DoString("if ZoneTextFrame and ZoneTextFrame.icon then ZoneTextFrame.icon:SetTexture(nil) end"); } catch { }
                Lua.DoString("if sf then sf:Hide() end; if ff then ff:Hide() end");
            }
            catch { }
        }

        /// <summary>
        /// Apply overlay theme to Lua frames (status/banner)
        /// </summary>
        public static void ApplyOverlayTheme(ThemePalette p)
        {
            try
            {
                // Fidèle à Vitalic: pas d'arrière-plan/bordure; uniquement le texte coloré via codes |cff dans la chaîne.
                // On retire toute déco existante si on l'avait ajoutée dans une version précédente.
                Lua.DoString(@"
                    local ok,err = pcall(function()
                        if sf and sf.SetBackdrop then sf:SetBackdrop(nil); sf.backdrop = nil end
                    end)
                ");
            }
            catch { }
        }

        /// <summary>
        /// Rafraîchit tous les éléments UI (utilisé lors des resets).
        /// Recrée les frames et réapplique les thèmes si nécessaire.
        /// </summary>
        public static void RefreshAll()
        {
            try
            {
                EnsureStatusFrame_Exact();
                EnsureNotifyFrame();
                EnsureLowHealthFlash();
                EnsureZoneTextAssets();
                UpdateStatusAuto();
                var palette = UiTheme.Resolve(VitalicSettings.Instance.UIColorStyle);
                ApplyOverlayTheme(palette);
                UpdateBannerAlerts(VitalicSettings.Instance.SpellAlertsEnabled);
                UpdateBannerFonts(VitalicSettings.Instance.AlertFontsEnabled);
                if (VitalicSettings.Instance.StatusFrameEnabled)
                    ForceShowStatus();
                else
                    HideStatus();
            }
            catch { }
        }

        /// <summary>
        /// Debug method to dump the current state of the banner system for troubleshooting
        /// </summary>
        public static void DebugDumpBanner(string where)
        {
            try
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && !VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    return;

                var ret = Lua.GetReturnValues(@"
                    local vb = _G.VitalicBanner ~= nil
                    local hasMini = vb and (VitalicBanner.ShowMiniSimple or VitalicBanner.ShowPvpText) and 1 or 0
                    local hasPVP = _G.PVPInfoTextFrame ~= nil and 1 or 0
                    local shown = (_G.PVPInfoTextFrame and _G.PVPInfoTextFrame:IsShown()) and 1 or 0
                    local zHas = _G.ZoneTextFrame ~= nil and 1 or 0
                    local zShown = (_G.ZoneTextFrame and _G.ZoneTextFrame:IsShown()) and 1 or 0
                    return tostring(vb), tostring(hasMini), tostring(hasPVP), tostring(shown), tostring(zHas), tostring(zShown)
                ");
                Logger.Write("[UI][Dump:{0}] VB={1} miniFns={2} PVPInfo={3} PVPShown={4} Zone={5} ZoneShown={6}",
                    where,
                    ret.Count > 0 ? ret[0] : "?",
                    ret.Count > 1 ? ret[1] : "?",
                    ret.Count > 2 ? ret[2] : "?",
                    ret.Count > 3 ? ret[3] : "?",
                    ret.Count > 4 ? ret[4] : "?",
                    ret.Count > 5 ? ret[5] : "?"
                );
            }
            catch (Exception e)
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[UI][Dump:{0}] EX: {1}", where, e.Message);
            }
        }

        /// <summary>
        /// Show center-screen notification using the ff frame (FULLSCREEN_DIALOG strata)
        /// This method ensures proper initialization and display
        /// </summary>
        public static void ShowCenterNotification(string message, double holdSeconds = 3.0)
        {
            try
            {
                // Ensure ff frame is created first
                EnsureNotifyFrame();
                
                if (string.IsNullOrEmpty(message)) return;
                
                string safe = message.Replace("\\", "\\\\").Replace("'", "\\'");
                string holdStr = holdSeconds.ToString("F2", CultureInfo.InvariantCulture);

                // MoP parity: no C_Timer; do simple OnUpdate-based auto-hide
                Lua.DoString(
                    "if ff then\n" +
                    "    ff.text:SetText('" + safe + "')\n" +
                    "    ff.text:SetTextColor(1, 0.82, 0)\n" +
                    "    ff:Show()\n" +
                    "    ff._vb_elapsed = 0\n" +
                    "    ff:SetScript('OnUpdate', function(self, e)\n" +
                    "        self._vb_elapsed = (self._vb_elapsed or 0) + e\n" +
                    "        if self._vb_elapsed >= " + holdStr + " then self:SetScript('OnUpdate', nil); self:Hide() end\n" +
                    "    end)\n" +
                    "end");
                
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[UI][Center] Showing notification: \"{0}\" for {1}s", message, holdSeconds);
            }
            catch (Exception e)
            {
                if (VitalicRotation.Settings.VitalicSettings.Instance != null && VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                    Logger.Write("[UI][Center] Error: {0}", e.Message);
            }
        }
    }
}
