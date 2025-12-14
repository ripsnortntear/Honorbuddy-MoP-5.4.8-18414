using System;
using Styx.WoWInternals;
using VitalicRotation.Helpers;

namespace VitalicRotation.UI
{
    public static class DiagOverlay
    {
        private static DateTime _last = DateTime.MinValue;

        public static void Pulse()
        {
            if ((DateTime.UtcNow - _last).TotalMilliseconds < 300) return;
            _last = DateTime.UtcNow;

            string text = string.Format("GCD:{0}  LoS:{1}  Range:{2}",
                CastCounters.GcdDenied, CastCounters.LineOfSightDenied, CastCounters.RangeDenied);

            try
            {
                Lua.DoString(
                    "if not VitalicDiag then " +
                    "  VitalicDiag=CreateFrame('Frame',nil,UIParent);" +
                    "  VitalicDiag:SetSize(200,18);" +
                    "  VitalicDiag.text=VitalicDiag:CreateFontString(nil,'OVERLAY','GameFontNormal');" +
                    "  VitalicDiag.text:SetPoint('CENTER');" +
                    "  VitalicDiag:SetPoint('TOP',0,-120);" +
                    "end; VitalicDiag.text:SetText('" + text.Replace("'", "’") + "');");
            }
            catch { }
        }
    }
}
