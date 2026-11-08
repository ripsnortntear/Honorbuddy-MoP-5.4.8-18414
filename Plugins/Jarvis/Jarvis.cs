using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.Common.Helpers;
using Styx.CommonBot;
using Styx.CommonBot.Profiles;
using Styx.Helpers;
using Styx.Plugins;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

namespace JarvisPlugin
{
    public class Jarvis : HBPlugin
    {
        private static readonly Version _version = new Version(1, 0, 0, 0);
        private readonly object _sync = new object();
        private readonly Queue<LogSample> _recentLogs = new Queue<LogSample>();
        private readonly Dictionary<string, DateTime> _lastSuggestionUtc = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private readonly Stopwatch _pulseThrottle = Stopwatch.StartNew();
        private readonly Stopwatch _sessionUptime = Stopwatch.StartNew();
        private bool _enabled;
        private bool _logHooked;
        private DateTime _lastLogSummaryUtc = DateTime.MinValue;
        private int _errorCount;
        private int _warningCount;
        private int _deathCount;
        private string _lastError;
        private string _lastZone;
        private uint _lastLevel;

        // "Safe" auto-adjustments are conservative and logged. Default false for anything that changes behavior.
        private bool _autoApplySafeFixes = false;

        public override string Name { get { return "Jarvis"; } }
        public override string Author { get { return "repo maintenance"; } }
        public override Version Version { get { return _version; } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Jarvis"; } }

        public override void OnEnable()
        {
            if (_enabled)
                return;
            _enabled = true;
            HookLogs();
            Log("Enabled. Monitoring Honorbuddy state/logs and preparing suggestions. Use the Jarvis button for a report.");
            Log("Note: Jarvis is a local HB plugin assistant; it does not contact external AI services.");
        }

        public override void OnDisable()
        {
            if (!_enabled)
                return;
            _enabled = false;
            UnhookLogs();
            Log("Disabled.");
        }

        [Obsolete("Use OnEnable instead")]
        public override void Initialize() { OnEnable(); }

        [Obsolete("Use OnDisable instead")]
        public override void Dispose() { OnDisable(); }

        public override void Pulse()
        {
            if (!_enabled)
                return;

            // Throttle: Jarvis is background assistance, not combat-critical.
            if (_pulseThrottle.ElapsedMilliseconds < 2500)
                return;
            _pulseThrottle.Restart();

            try
            {
                MonitorState();
                MaybeEmitPeriodicSuggestions();
            }
            catch (Exception ex)
            {
                LogDiagnostic("Pulse error: " + ex.Message);
            }
        }

        public override void OnButtonPress()
        {
            try
            {
                string report = BuildReport(true);
                ShowReportDialog(report);
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.OrangeRed, "[Jarvis] Failed to build report: " + ex.Message);
            }
        }

        private void HookLogs()
        {
            if (_logHooked)
                return;
            Logging.OnLogMessage += OnLogMessage;
            _logHooked = true;
        }

        private void UnhookLogs()
        {
            if (!_logHooked)
                return;
            try { Logging.OnLogMessage -= OnLogMessage; }
            catch { }
            _logHooked = false;
        }

        private void OnLogMessage(ReadOnlyCollection<Logging.LogMessage> messages)
        {
            if (!_enabled || messages == null)
                return;

            try
            {
                lock (_sync)
                {
                    foreach (Logging.LogMessage msg in messages)
                    {
                        if (msg == null)
                            continue;
                        string text = msg.Message ?? string.Empty;
                        LogLevel level = msg.Level;
                        bool isError = level == LogLevel.Normal && LooksLikeError(text);
                        bool isWarning = LooksLikeWarning(text);

                        if (isError) { _errorCount++; _lastError = Truncate(text, 220); }
                        if (isWarning) _warningCount++;
                        if (LooksLikeDeath(text)) _deathCount++;

                        _recentLogs.Enqueue(new LogSample { Utc = DateTime.UtcNow, Level = level, Text = Truncate(text, 300) });
                        while (_recentLogs.Count > 120)
                            _recentLogs.Dequeue();
                    }
                }
            }
            catch
            {
                // Never break HB logging.
            }
        }

        private void MonitorState()
        {
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid)
                return;

            uint level = me.Level;
            string zone = SafeZoneName(me);

            if (_lastLevel != 0 && level > _lastLevel)
                SuggestOnce("levelup", TimeSpan.FromMinutes(10), Colors.Gold,
                    "Level up: " + _lastLevel + " -> " + level + ". Consider checking trainer skills, quest rewards, and whether your current profile/grind area still fits.");

            if (!string.IsNullOrEmpty(zone) && !string.Equals(zone, _lastZone, StringComparison.Ordinal))
            {
                if (!string.IsNullOrEmpty(_lastZone))
                    SuggestOnce("zone-" + zone, TimeSpan.FromMinutes(5), Colors.DeepSkyBlue,
                        "Zone changed: " + _lastZone + " -> " + zone + ". If behavior seems off, verify profile, mesh/navigation, and vendor/repair settings for this area.");
                _lastZone = zone;
            }

            _lastLevel = level;

            ApplySafeFixesIfEnabled(me);
        }

        private void ApplySafeFixesIfEnabled(LocalPlayer me)
        {
            if (!_autoApplySafeFixes || me == null)
                return;

            // Intentionally conservative: do not silently change combat/loot/profile choices.
            // Placeholder for future opt-in safe fixes with explicit logging.
        }

        private void MaybeEmitPeriodicSuggestions()
        {
            if ((DateTime.UtcNow - _lastLogSummaryUtc).TotalMinutes < 10)
                return;
            _lastLogSummaryUtc = DateTime.UtcNow;

            var me = StyxWoW.Me;
            if (me == null || !me.IsValid)
                return;

            var suggestions = BuildSuggestions(me, false);
            if (suggestions.Count == 0)
                return;

            // Emit only top suggestions periodically to avoid log spam.
            int count = Math.Min(2, suggestions.Count);
            for (int i = 0; i < count; i++)
                SuggestOnce("periodic-" + suggestions[i].Key, TimeSpan.FromMinutes(30), Colors.LightGreen, suggestions[i].Message);
        }

        private string BuildReport(bool includeDetailedSuggestions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jarvis Report (v" + _version + ")");
            sb.AppendLine("Session uptime: " + FormatDuration(_sessionUptime.Elapsed));
            sb.AppendLine("Auto-apply safe fixes: " + (_autoApplySafeFixes ? "ON (conservative)" : "OFF (suggestions only)"));
            sb.AppendLine();

            var me = StyxWoW.Me;
            if (me == null || !me.IsValid)
            {
                sb.AppendLine("Character: not available (not logged in / player invalid).");
            }
            else
            {
                AppendCharacterInfo(sb, me);
                sb.AppendLine();
                sb.AppendLine("Suggestions:");
                var suggestions = BuildSuggestions(me, includeDetailedSuggestions);
                if (suggestions.Count == 0)
                    sb.AppendLine("- No immediate suggestions. Things look reasonable.");
                else
                {
                    foreach (var s in suggestions.Take(12))
                        sb.AppendLine("- " + s.Message);
                }
            }

            sb.AppendLine();
            sb.AppendLine("Runtime / Bot:");
            sb.AppendLine("- Current bot: " + SafeName(BotManager.Current != null ? BotManager.Current.Name : "(none)"));
            sb.AppendLine("- Profile: " + SafeName(ProfileManager.CurrentProfile != null ? (ProfileManager.CurrentProfile.Name ?? ProfileManager.CurrentProfile.Path) : "(none)"));
            sb.AppendLine("- TreeRoot running: " + TreeRoot.IsRunning);
            sb.AppendLine();

            lock (_sync)
            {
                sb.AppendLine("Log monitor (recent session):");
                sb.AppendLine("- Errors detected: " + _errorCount + ", warnings: " + _warningCount + ", death-like messages: " + _deathCount);
                if (!string.IsNullOrEmpty(_lastError))
                    sb.AppendLine("- Last error-like line: " + _lastError);
            }

            sb.AppendLine();
            sb.AppendLine("Jarvis can: monitor logs/state, suggest actions, and (optionally) apply conservative fixes.");
            sb.AppendLine("Jarvis cannot: replace this coding agent inside HB chat; for code changes use the repo/OpenCode workflow.");
            return sb.ToString();
        }

        private void AppendCharacterInfo(StringBuilder sb, LocalPlayer me)
        {
            sb.AppendLine("Character:");
            sb.AppendLine("- Name: " + SafeName(me.Name) + "  Class: " + me.Class + "  Race: " + me.Race + "  Level: " + me.Level);
            sb.AppendLine("- Zone: " + SafeZoneName(me) + "  Area: " + SafeName(me.SubZoneText));
            sb.AppendLine("- Health: " + me.HealthPercent.ToString("0") + "%  Mana/Power: " + me.ManaPercent.ToString("0") + "%  Combat: " + me.Combat);
            sb.AppendLine("- XP: " + me.CurrentXP + "/" + Math.Max(1u, me.NextLevelXP) + " (" + (me.NextLevelXP == 0 ? 0 : (me.CurrentXP * 100 / me.NextLevelXP)) + "%)");
            sb.AppendLine("- Gold: " + FormatCopper(me.Copper));
            sb.AppendLine("- Bags: free=" + me.FreeBagSlots + " (approx)  Items: " + SafeCount(me.BagItems));
            sb.AppendLine("- Durability: " + AverageDurability(me).ToString("0") + "% (equipped avg)");

            var profs = GetProfessionSummary(me);
            sb.AppendLine("- Professions/Skills: " + (profs.Count == 0 ? "(not detected)" : string.Join(", ", profs.ToArray())));
        }

        private List<Suggestion> BuildSuggestions(LocalPlayer me, bool detailed)
        {
            var list = new List<Suggestion>();

            // Bags / durability / basics
            if (me.FreeBagSlots <= 2)
                list.Add(new Suggestion("bags", "Bag space is very low (" + me.FreeBagSlots + " free). Consider vendor/sell/mail/disenchant or enable a bag-management plugin/profile behavior."));
            double dura = AverageDurability(me);
            if (dura > 0 && dura < 25)
                list.Add(new Suggestion("durability", "Equipped durability is low (" + dura.ToString("0") + "%). Add a repair/vendor step or check repair settings before a long run."));
            if (me.Level >= 20 && !HasMountCapabilityHint(me))
                list.Add(new Suggestion("mount", "Check riding/mount setup (level " + me.Level + "). If movement is slow, verify mount settings, riding skill, and bot movement options."));

            // Professions
            var profs = GetProfessionSummary(me);
            if (profs.Count == 0 && me.Level >= 5)
                list.Add(new Suggestion("professions", "No professions detected (or not readable). If intended, pick up professions; if using ProfessionBuddy/gather profiles, verify the profile matches your skills."));
            if (HasSkill(me, SkillLine.Fishing) && !HasItemHint(me, "fishing pole"))
                list.Add(new Suggestion("fishing", "Fishing skill detected but a fishing pole may not be obvious in bags/equipped. If using fishing bots, confirm pole/lures/bait setup."));
            if (HasSkill(me, SkillLine.FirstAid) && me.Level >= 20)
                list.Add(new Suggestion("firstaid", "First Aid detected. For older MoP workflows, consider carrying bandages/consumables; verify your routine uses them appropriately."));

            // Bot/profile context
            if (BotManager.Current == null)
                list.Add(new Suggestion("bot", "No bot is selected. Pick a bot (GrindBuddy, Questing, Gathering, etc.) that matches your goal."));
            if (ProfileManager.CurrentProfile == null && BotManager.Current != null && BotManager.Current.RequiresProfile)
                list.Add(new Suggestion("profile", "Current bot requires a profile, but none is loaded. Load an appropriate profile for your level/zone."));

            // Log/state heuristics
            lock (_sync)
            {
                if (_errorCount >= 5)
                    list.Add(new Suggestion("errors", "Multiple error-like log lines observed (" + _errorCount + "). Check the latest HB log; Jarvis can help pinpoint the source if you share the error text."));
                if (!string.IsNullOrEmpty(_lastError) && _lastError.IndexOf("mesh", StringComparison.OrdinalIgnoreCase) >= 0)
                    list.Add(new Suggestion("mesh", "Recent logs mention mesh/navigation issues. Verify meshes are present and loaded for the current continent/map."));
            }

            // Class-flavored generic suggestions (non-prescriptive)
            list.Add(new Suggestion("class-" + me.Class, GetClassSuggestion(me)));

            // Deduplicate by key, keep first occurrence
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var deduped = new List<Suggestion>();
            foreach (var s in list)
            {
                if (s == null || string.IsNullOrEmpty(s.Key) || string.IsNullOrEmpty(s.Message))
                    continue;
                if (seen.Add(s.Key))
                    deduped.Add(s);
            }
            return deduped;
        }

        private static string GetClassSuggestion(LocalPlayer me)
        {
            switch (me.Class)
            {
                case WoWClass.Warrior: return "Warrior: keep repair/gold in mind; for grinding, check pull size, stance/movement, and food/bandage settings.";
                case WoWClass.Paladin: return "Paladin: good self-sustain options; verify seal/aura/buff settings in your combat routine and use repairs/vendors on long grinds.";
                case WoWClass.Hunter: return "Hunter: monitor pet status, ammo/consumables as applicable for your setup, and bag space for loot.";
                case WoWClass.Rogue: return "Rogue: stealth/openers and pull control matter; keep lockpicks/poisons in mind if your routine uses them.";
                case WoWClass.Priest: return "Priest: watch mana/wand or downtime settings; if grinding, ensure your profile density matches survivability.";
                case WoWClass.DeathKnight: return "Death Knight: rune/runic power flow depends on routine; check pull recovery and repair cycles.";
                case WoWClass.Shaman: return "Shaman: totems/weapon imbues and mana can affect uptime; verify routine settings for your spec/role.";
                case WoWClass.Mage: return "Mage: keep water/food and mana breaks configured; avoid overpulling without routine support.";
                case WoWClass.Warlock: return "Warlock: monitor pet, soul shards/bag space, and health/funnel settings in your routine.";
                case WoWClass.Monk: return "Monk: mobility is strong; verify movement/roll usage in routine and keep gear repaired for long sessions.";
                case WoWClass.Druid: return "Druid: forms cover many roles; ensure your routine/profile matches the form/spec and gather settings if herbalism/mining.";
                default: return "Check class/routine settings, profile suitability for your level/zone, and vendor/repair/loot configuration.";
            }
        }

        private static List<string> GetProfessionSummary(LocalPlayer me)
        {
            var result = new List<string>();
            try
            {
                var skills = me.Skills;
                if (skills == null)
                    return result;

                SkillLine[] interesting = new[]
                {
                    SkillLine.Alchemy, SkillLine.Blacksmithing, SkillLine.Enchanting, SkillLine.Engineering,
                    SkillLine.Herbalism, SkillLine.Inscription, SkillLine.Jewelcrafting, SkillLine.Leatherworking,
                    SkillLine.Mining, SkillLine.Skinning, SkillLine.Tailoring,
                    SkillLine.Archaeology, SkillLine.Cooking, SkillLine.Fishing, SkillLine.FirstAid
                };

                foreach (SkillLine line in interesting)
                {
                    var skill = me.GetSkill(line);
                    if (skill == null || !skill.IsValid)
                        continue;
                    // CurrentValue/MaxValue naming can vary by HB version; ToString fallback is safe.
                    string val = skill.CurrentValue.ToString();
                    string max = skill.MaxValue.ToString();
                    if (skill.CurrentValue > 0 || skill.MaxValue > 0)
                        result.Add(line + " " + val + "/" + max);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteDiagnostic("[Jarvis] Profession scan failed: " + ex.Message);
            }
            return result;
        }

        private static bool HasSkill(LocalPlayer me, SkillLine line)
        {
            try
            {
                var s = me.GetSkill(line);
                return s != null && s.IsValid && s.CurrentValue > 0;
            }
            catch { return false; }
        }

        private static bool HasMountCapabilityHint(LocalPlayer me)
        {
            try { return me.GetSkill(SkillLine.Riding) != null && me.GetSkill(SkillLine.Riding).CurrentValue > 0; }
            catch { return me.Level < 20; }
        }

        private static bool HasItemHint(LocalPlayer me, string nameContains)
        {
            try
            {
                if (me == null || me.BagItems == null) return false;
                foreach (var item in me.BagItems)
                {
                    if (item == null || !item.IsValid || item.Name == null) continue;
                    if (item.Name.IndexOf(nameContains, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }
            catch { }
            return false;
        }

        private static double AverageDurability(LocalPlayer me)
        {
            try
            {
                if (me.Inventory == null || me.Inventory.Equipped == null) return 100;
                double sum = 0; int n = 0;
                foreach (WoWItem item in me.Inventory.Equipped.Items)
                {
                    if (item == null || !item.IsValid) continue;
                    if (item.MaxDurability <= 0) continue;
                    sum += (item.Durability * 100.0 / item.MaxDurability);
                    n++;
                }
                return n == 0 ? 100 : sum / n;
            }
            catch { return 100; }
        }

        private void SuggestOnce(string key, TimeSpan cooldown, Color color, string message)
        {
            try
            {
                lock (_sync)
                {
                    DateTime last;
                    if (_lastSuggestionUtc.TryGetValue(key, out last) && (DateTime.UtcNow - last) < cooldown)
                        return;
                    _lastSuggestionUtc[key] = DateTime.UtcNow;
                }
                Logging.Write(color, "[Jarvis] " + message);
            }
            catch { }
        }

        private static void Log(string message)
        {
            Logging.Write(Colors.DeepSkyBlue, "[Jarvis] " + message);
        }

        private static void LogDiagnostic(string message)
        {
            Logging.WriteDiagnostic("[Jarvis] " + message);
        }

        private static bool LooksLikeError(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return text.IndexOf("exception", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("error", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("failed", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("nullreference", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool LooksLikeWarning(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return text.IndexOf("warning", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("stuck", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("blacklist", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static bool LooksLikeDeath(string text)
        {
            if (string.IsNullOrEmpty(text)) return false;
            return text.IndexOf(" died", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("death", StringComparison.OrdinalIgnoreCase) >= 0
                || text.IndexOf("ghost", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string SafeZoneName(LocalPlayer me)
        {
            try
            {
                string z = me.ZoneText;
                if (!string.IsNullOrEmpty(z)) return z;
            }
            catch { }
            try { return me.RealZoneText; } catch { }
            return "";
        }

        private static string SafeName(string s) { return string.IsNullOrEmpty(s) ? "(unknown)" : s; }
        private static int SafeCount(System.Collections.IEnumerable e) { if (e == null) return 0; int n=0; foreach (var _ in e) { n++; if (n>5000) break; } return n; }
        private static string Truncate(string s, int max) { if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? ""; return s.Substring(0, max) + "..."; }

        private static string FormatDuration(TimeSpan ts)
        {
            if (ts.TotalHours >= 1) return ((int)ts.TotalHours) + "h " + ts.Minutes + "m";
            if (ts.TotalMinutes >= 1) return ((int)ts.TotalMinutes) + "m " + ts.Seconds + "s";
            return ts.Seconds + "s";
        }

        private static string FormatCopper(ulong copper)
        {
            ulong g = copper / 10000UL;
            ulong s = (copper / 100UL) % 100UL;
            ulong c = copper % 100UL;
            return g + "g " + s + "s " + c + "c";
        }

        private static void ShowReportDialog(string report)
        {
            try
            {
                // Simple, HB-friendly UI. Keep it modeless-safe by using ShowDialog on UI thread context if possible.
                var form = new Form();
                form.Text = "Jarvis - Honorbuddy Assistant";
                form.Width = 860;
                form.Height = 640;
                form.StartPosition = FormStartPosition.CenterScreen;

                var box = new TextBox();
                box.Multiline = true;
                box.ReadOnly = true;
                box.ScrollBars = ScrollBars.Both;
                box.WordWrap = false;
                box.Font = new System.Drawing.Font("Consolas", 9f);
                box.Dock = DockStyle.Fill;
                box.Text = report.Replace("\n", Environment.NewLine);

                var panel = new Panel();
                panel.Dock = DockStyle.Bottom;
                panel.Height = 44;

                var copyBtn = new Button();
                copyBtn.Text = "Copy";
                copyBtn.Width = 90;
                copyBtn.Left = 10;
                copyBtn.Top = 8;
                copyBtn.Click += (s, e) => { try { Clipboard.SetText(box.Text); } catch { } };

                var closeBtn = new Button();
                closeBtn.Text = "Close";
                closeBtn.Width = 90;
                closeBtn.Left = 110;
                closeBtn.Top = 8;
                closeBtn.Click += (s, e) => form.Close();

                panel.Controls.Add(copyBtn);
                panel.Controls.Add(closeBtn);
                form.Controls.Add(box);
                form.Controls.Add(panel);

                form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.OrangeRed, "[Jarvis] UI unavailable: " + ex.Message);
                Logging.Write(Colors.LightGray, "[Jarvis]\n" + report);
            }
        }

        private struct LogSample
        {
            public DateTime Utc;
            public LogLevel Level;
            public string Text;
        }

        private class Suggestion
        {
            public string Key;
            public string Message;
            public Suggestion(string key, string message) { Key = key; Message = message; }
        }
    }
}
