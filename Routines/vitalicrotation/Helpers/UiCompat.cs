using System;
using System.Reflection;

namespace VitalicRotation.Helpers
{
    internal static class UiCompat
    {
        // Affiche une notif "style Vitalic" si l'UI existe, sinon fallback en log.
        public static void Notify(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return;

            try
            {
                // Tentatives par reflection : VitalicUi.ShowNotify(string), UiManager.Notify(string), Overlay.Show(string)
                var candidates = new[]
                {
                    new { TypeName = "VitalicRotation.UI.VitalicUi", Method = "ShowNotify" },
                    new { TypeName = "VitalicRotation.UI.UiManager",   Method = "Notify" },
                    new { TypeName = "VitalicRotation.UI.Overlay",     Method = "Show" }
                };

                foreach (var c in candidates)
                {
                    var t = Type.GetType(c.TypeName, throwOnError: false);
                    if (t == null) continue;

                    var m = t.GetMethod(c.Method, BindingFlags.Public | BindingFlags.Static, null, new[] { typeof(string) }, null);
                    if (m == null) continue;

                    m.Invoke(null, new object[] { text });
                    return;
                }
            }
            catch
            {
                // ignore et fallback log
            }

            Logger.Write(text);
        }
    }
}
