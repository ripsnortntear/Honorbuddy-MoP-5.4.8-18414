using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private static readonly Version _version = new Version(1, 1, 0, 0);
        private readonly object _sync = new object();
        private readonly Queue<LogSample> _recentLogs = new Queue<LogSample>();
        private readonly Dictionary<string, DateTime> _lastSuggestionUtc = new Dictionary<string, DateTime>(StringComparer.OrdinalIgnoreCase);
        private readonly Stopwatch _pulseThrottle = Stopwatch.StartNew();
        private readonly Stopwatch _sessionUptime = Stopwatch.StartNew();
        private bool _enabled;
        private bool _logHooked;
        private DateTime _lastLogSummaryUtc = DateTime.MinValue;
        private DateTime _lastPeriodicAiUtc = DateTime.MinValue;
        private int _errorCount;
        private int _warningCount;
        private int _deathCount;
        private string _lastError;
        private string _lastZone;
        private uint _lastLevel;
        private bool _autoApplySafeFixes = false;

        private volatile bool _aiBusy;
        private JarvisAiClient.AiResult _pendingAiResult;
        private string _pendingAiSource;

        public override string Name { get { return "Jarvis"; } }
        public override string Author { get { return "repo maintenance"; } }
        public override Version Version { get { return _version; } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Jarvis"; } }

        public override void OnEnable()
        {
            if (_enabled) return;
            _enabled = true;
            HookLogs();
            try { JarvisSettings.Instance.Load(); } catch { }
            Log("Enabled. Monitoring logs/state and preparing suggestions. Use the Jarvis button for a report or Ask (opt-in external AI).");
            Log("External AI is OFF by default. Configure it in the Jarvis dialog if you want local/cloud AI suggestions.");
        }

        public override void OnDisable()
        {
            if (!_enabled) return;
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
            if (!_enabled) return;
            if (_pulseThrottle.ElapsedMilliseconds < 2500) return;
            _pulseThrottle.Restart();

            try
            {
                ApplyPendingAiResultToLog();
                MonitorState();
                MaybeEmitPeriodicSuggestions();
                MaybeRunPeriodicAi();
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
                ShowReportDialog();
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.OrangeRed, "[Jarvis] Failed to open dialog: " + ex.Message);
            }
        }

        private void HookLogs()
        {
            if (_logHooked) return;
            Logging.OnLogMessage += OnLogMessage;
            _logHooked = true;
        }

        private void UnhookLogs()
        {
            if (!_logHooked) return;
            try { Logging.OnLogMessage -= OnLogMessage; } catch { }
            _logHooked = false;
        }

        private void OnLogMessage(ReadOnlyCollection<Logging.LogMessage> messages)
        {
            if (!_enabled || messages == null) return;
            try
            {
                lock (_sync)
                {
                    foreach (Logging.LogMessage msg in messages)
                    {
                        if (msg == null) continue;
                        string text = msg.Message ?? string.Empty;
                        LogLevel level = msg.Level;
                        bool isError = level == LogLevel.Normal && LooksLikeError(text);
                        bool isWarning = LooksLikeWarning(text);
                        if (isError) { _errorCount++; _lastError = Truncate(text, 220); }
                        if (isWarning) _warningCount++;
                        if (LooksLikeDeath(text)) _deathCount++;
                        _recentLogs.Enqueue(new LogSample { Utc = DateTime.UtcNow, Level = level, Text = Truncate(text, 300) });
                        while (_recentLogs.Count > 120) _recentLogs.Dequeue();
                    }
                }
            }
            catch { }
        }

        private void MonitorState()
        {
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid) return;

            uint level = (uint)Math.Max(0, me.Level);
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
            if (!_autoApplySafeFixes || me == null) return;
            // Intentionally conservative: no silent combat/profile/loot behavior changes.
        }

        private void MaybeEmitPeriodicSuggestions()
        {
            if ((DateTime.UtcNow - _lastLogSummaryUtc).TotalMinutes < 10) return;
            _lastLogSummaryUtc = DateTime.UtcNow;
            var me = StyxWoW.Me;
            if (me == null || !me.IsValid) return;
            var suggestions = BuildSuggestions(me, false);
            if (suggestions.Count == 0) return;
            int count = Math.Min(2, suggestions.Count);
            for (int i = 0; i < count; i++)
                SuggestOnce("periodic-" + suggestions[i].Key, TimeSpan.FromMinutes(30), Colors.LightGreen, suggestions[i].Message);
        }

        private void MaybeRunPeriodicAi()
        {
            JarvisSettings s = JarvisSettings.Instance;
            if (s == null || !s.ExternalAiEnabled || !s.AllowPeriodicSuggestions) return;
            if (_aiBusy) return;
            int minutes = Math.Max(5, s.PeriodicMinutes);
            if ((DateTime.UtcNow - _lastPeriodicAiUtc).TotalMinutes < minutes) return;
            _lastPeriodicAiUtc = DateTime.UtcNow;

            string snapshot = BuildAiContextSnapshot(s, true);
            string question = "Provide 1-3 short, practical periodic suggestions based on this snapshot. Focus on safety, efficiency, and obvious issues.";
            QueueAiRequest(s, question, snapshot, "periodic");
        }

        private void QueueAiRequest(JarvisSettings settings, string question, string snapshot, string source)
        {
            if (_aiBusy) return;
            _aiBusy = true;
            JarvisAiClient.RequestAsync(settings, question, snapshot, result =>
            {
                lock (_sync)
                {
                    _pendingAiResult = result;
                    _pendingAiSource = source;
                }
                _aiBusy = false;
            });
        }

        private void ApplyPendingAiResultToLog()
        {
            JarvisAiClient.AiResult result = null;
            string source = null;
            lock (_sync)
            {
                result = _pendingAiResult;
                source = _pendingAiSource;
                _pendingAiResult = null;
                _pendingAiSource = null;
            }
            if (result == null) return;
            if (result.Success)
            {
                if (string.Equals(source, "periodic", StringComparison.OrdinalIgnoreCase))
                    Logging.Write(Colors.MediumPurple, "[Jarvis AI] " + Truncate(result.Message, 1200));
                else
                    Logging.Write(Colors.MediumPurple, "[Jarvis AI] Response ready (open Jarvis dialog to view if requested from UI).");
            }
            else
            {
                Logging.Write(Colors.OrangeRed, "[Jarvis AI] " + (result.Error ?? "Unknown error"));
            }
        }

        private string BuildAiContextSnapshot(JarvisSettings s, bool forPeriodic)
        {
            var sb = new StringBuilder();
            var me = StyxWoW.Me;
            sb.AppendLine("Timestamp (UTC): " + DateTime.UtcNow.ToString("o"));
            if (me == null || !me.IsValid)
            {
                sb.AppendLine("Character: (not available)");
            }
            else
            {
                if (s.SendCharacter)
                    sb.AppendLine("Character: class=" + me.Class + " level=" + me.Level + " hp%=" + me.HealthPercent.ToString("0") + " mana%=" + me.ManaPercent.ToString("0") + " combat=" + me.Combat);
                if (s.SendLocation)
                    sb.AppendLine("Location: zone=" + SafeZoneName(me) + " area=" + SafeName(me.SubZoneText));
                if (s.SendInventorySummary)
                {
                    sb.AppendLine("Inventory: freeBagSlots=" + me.FreeBagSlots + " durabilityAvg%=" + AverageDurability(me).ToString("0") + " gold=" + FormatCopper(me.Copper));
                    sb.AppendLine("XP: " + GetXpSummary(me));
                }
                if (s.SendProfessions)
                    sb.AppendLine("Professions/Skills: " + string.Join(", ", GetProfessionSummary(me).ToArray()));
            }
            if (s.SendBotProfile)
            {
                sb.AppendLine("Bot: " + SafeName(BotManager.Current != null ? BotManager.Current.Name : "(none)") + " running=" + TreeRoot.IsRunning);
                sb.AppendLine("Profile: " + SafeName(ProfileManager.CurrentProfile != null ? (ProfileManager.CurrentProfile.Name ?? ProfileManager.CurrentProfile.Path) : "(none)"));
            }
            if (s.SendRecentLogSummary)
            {
                lock (_sync)
                {
                    sb.AppendLine("Log summary: errors=" + _errorCount + " warnings=" + _warningCount + " deaths=" + _deathCount);
                    if (!string.IsNullOrEmpty(_lastError)) sb.AppendLine("Last error-like: " + _lastError);
                }
            }
            sb.AppendLine("Mode: " + (forPeriodic ? "periodic" : "button"));
            return sb.ToString();
        }

        private string BuildReport(bool includeDetailedSuggestions)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Jarvis Report (v" + _version + ")");
            sb.AppendLine("Session uptime: " + FormatDuration(_sessionUptime.Elapsed));
            sb.AppendLine("Auto-apply safe fixes: " + (_autoApplySafeFixes ? "ON (conservative)" : "OFF (suggestions only)"));
            var s = JarvisSettings.Instance;
            sb.AppendLine("External AI: " + (s != null && s.ExternalAiEnabled ? "ENABLED (opt-in)" : "OFF") +
                (s != null ? " | Ask=" + s.AllowAskButton + " | Periodic=" + s.AllowPeriodicSuggestions : ""));
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
                if (suggestions.Count == 0) sb.AppendLine("- No immediate suggestions. Things look reasonable.");
                else foreach (var sug in suggestions.Take(12)) sb.AppendLine("- " + sug.Message);
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
                if (!string.IsNullOrEmpty(_lastError)) sb.AppendLine("- Last error-like line: " + _lastError);
            }
            sb.AppendLine();
            sb.AppendLine("Jarvis monitors local HB state/logs and can (opt-in) ask a configured local/cloud AI for suggestions.");
            sb.AppendLine("It does not silently change combat/profile/loot behavior.");
            return sb.ToString();
        }

        private void AppendCharacterInfo(StringBuilder sb, LocalPlayer me)
        {
            sb.AppendLine("Character:");
            sb.AppendLine("- Name: " + SafeName(me.Name) + "  Class: " + me.Class + "  Race: " + me.Race + "  Level: " + me.Level);
            sb.AppendLine("- Zone: " + SafeZoneName(me) + "  Area: " + SafeName(me.SubZoneText));
            sb.AppendLine("- Health: " + me.HealthPercent.ToString("0") + "%  Mana/Power: " + me.ManaPercent.ToString("0") + "%  Combat: " + me.Combat);
            sb.AppendLine("- XP: " + GetXpSummary(me));
            sb.AppendLine("- Gold: " + FormatCopper(me.Copper));
            sb.AppendLine("- Bags: free=" + me.FreeBagSlots + " (approx)  Items: " + SafeCount(me.BagItems));
            sb.AppendLine("- Durability: " + AverageDurability(me).ToString("0") + "% (equipped avg)");
            var profs = GetProfessionSummary(me);
            sb.AppendLine("- Professions/Skills: " + (profs.Count == 0 ? "(not detected)" : string.Join(", ", profs.ToArray())));
        }

        private List<Suggestion> BuildSuggestions(LocalPlayer me, bool detailed)
        {
            var list = new List<Suggestion>();
            if (me.FreeBagSlots <= 2)
                list.Add(new Suggestion("bags", "Bag space is very low (" + me.FreeBagSlots + " free). Consider vendor/sell/mail/disenchant or enable a bag-management plugin/profile behavior."));
            double dura = AverageDurability(me);
            if (dura > 0 && dura < 25)
                list.Add(new Suggestion("durability", "Equipped durability is low (" + dura.ToString("0") + "%). Add a repair/vendor step or check repair settings before a long run."));
            if (me.Level >= 20 && !HasMountCapabilityHint(me))
                list.Add(new Suggestion("mount", "Check riding/mount setup (level " + me.Level + "). If movement is slow, verify mount settings, riding skill, and bot movement options."));
            var profs = GetProfessionSummary(me);
            if (profs.Count == 0 && me.Level >= 5)
                list.Add(new Suggestion("professions", "No professions detected (or not readable). If intended, pick up professions; if using ProfessionBuddy/gather profiles, verify the profile matches your skills."));
            if (HasSkill(me, SkillLine.Fishing) && !HasItemHint(me, "fishing pole"))
                list.Add(new Suggestion("fishing", "Fishing skill detected but a fishing pole may not be obvious in bags/equipped. If using fishing bots, confirm pole/lures/bait setup."));
            if (HasSkill(me, SkillLine.FirstAid) && me.Level >= 20)
                list.Add(new Suggestion("firstaid", "First Aid detected. For older MoP workflows, consider carrying bandages/consumables; verify your routine uses them appropriately."));
            if (BotManager.Current == null)
                list.Add(new Suggestion("bot", "No bot is selected. Pick a bot (GrindBuddy, Questing, Gathering, etc.) that matches your goal."));
            if (ProfileManager.CurrentProfile == null && BotManager.Current != null && BotManager.Current.RequiresProfile)
                list.Add(new Suggestion("profile", "Current bot requires a profile, but none is loaded. Load an appropriate profile for your level/zone."));
            lock (_sync)
            {
                if (_errorCount >= 5)
                    list.Add(new Suggestion("errors", "Multiple error-like log lines observed (" + _errorCount + "). Check the latest HB log; share the error text if you want help pinpointing it."));
                if (!string.IsNullOrEmpty(_lastError) && _lastError.IndexOf("mesh", StringComparison.OrdinalIgnoreCase) >= 0)
                    list.Add(new Suggestion("mesh", "Recent logs mention mesh/navigation issues. Verify meshes are present and loaded for the current continent/map."));
            }
            list.Add(new Suggestion("class-" + me.Class, GetClassSuggestion(me)));
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var deduped = new List<Suggestion>();
            foreach (var sg in list)
            {
                if (sg == null || string.IsNullOrEmpty(sg.Key) || string.IsNullOrEmpty(sg.Message)) continue;
                if (seen.Add(sg.Key)) deduped.Add(sg);
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
                    if (skill == null || !skill.IsValid) continue;
                    if (skill.CurrentValue > 0 || skill.MaxValue > 0)
                        result.Add(line + " " + skill.CurrentValue + "/" + skill.MaxValue);
                }
            }
            catch (Exception ex) { Logging.WriteDiagnostic("[Jarvis] Profession scan failed: " + ex.Message); }
            return result;
        }

        private static bool HasSkill(LocalPlayer me, SkillLine line)
        {
            try { var s = me.GetSkill(line); return s != null && s.IsValid && s.CurrentValue > 0; } catch { return false; }
        }

        private static bool HasMountCapabilityHint(LocalPlayer me)
        {
            try { var r = me.GetSkill(SkillLine.Riding); return r != null && r.IsValid && r.CurrentValue > 0; } catch { return me.Level < 20; }
        }

        private static bool HasItemHint(LocalPlayer me, string nameContains)
        {
            try
            {
                if (me == null || me.BagItems == null) return false;
                foreach (var item in me.BagItems)
                {
                    if (item == null || !item.IsValid || item.Name == null) continue;
                    if (item.Name.IndexOf(nameContains, StringComparison.OrdinalIgnoreCase) >= 0) return true;
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

        private void SuggestOnce(string key, TimeSpan cooldown, System.Windows.Media.Color color, string message)
        {
            try
            {
                lock (_sync)
                {
                    DateTime last;
                    if (_lastSuggestionUtc.TryGetValue(key, out last) && (DateTime.UtcNow - last) < cooldown) return;
                    _lastSuggestionUtc[key] = DateTime.UtcNow;
                }
                Logging.Write(color, "[Jarvis] " + message);
            }
            catch { }
        }

        private static void Log(string message) { Logging.Write(Colors.DeepSkyBlue, "[Jarvis] " + message); }
        private static void LogDiagnostic(string message) { Logging.WriteDiagnostic("[Jarvis] " + message); }

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
            try { string z = me.ZoneText; if (!string.IsNullOrEmpty(z)) return z; } catch { }
            try { return me.RealZoneText; } catch { }
            return "";
        }

        private static string SafeName(string s) { return string.IsNullOrEmpty(s) ? "(unknown)" : s; }
        private static int SafeCount(System.Collections.IEnumerable e) { if (e == null) return 0; int n = 0; foreach (var _ in e) { n++; if (n > 5000) break; } return n; }
        private static string Truncate(string s, int max) { if (string.IsNullOrEmpty(s) || s.Length <= max) return s ?? ""; return s.Substring(0, max) + "..."; }

        private static string FormatDuration(TimeSpan ts)
        {
            if (ts.TotalHours >= 1) return ((int)ts.TotalHours) + "h " + ts.Minutes + "m";
            if (ts.TotalMinutes >= 1) return ((int)ts.TotalMinutes) + "m " + ts.Seconds + "s";
            return ts.Seconds + "s";
        }

        private static string GetXpSummary(LocalPlayer me)
        {
            try
            {
                // Honorbuddy docs (Help/Honorbuddy.chm, HB 3.0.1160 / 5.4.8): WoWPlayer.Experience, WoWPlayer.NextLevelExperience.
                uint cur = me.Experience;
                uint max = me.NextLevelExperience;
                if (max == 0) return cur + "/0 (max level?)";
                uint pct = cur * 100u / max;
                return cur + "/" + max + " (" + pct + "%)";
            }
            catch (Exception ex) { Logging.WriteDiagnostic("[Jarvis] XP read failed: " + ex.Message); }
            return "(unavailable)";
        }

        private static string FormatCopper(ulong copper)
        {
            ulong g = copper / 10000UL;
            ulong s = (copper / 100UL) % 100UL;
            ulong c = copper % 100UL;
            return g + "g " + s + "s " + c + "c";
        }

        private void ShowReportDialog()
        {
            JarvisSettings settings = JarvisSettings.Instance;
            var form = new Form();
            form.Text = "Jarvis - Honorbuddy Assistant";
            form.Width = 980;
            form.Height = 760;
            form.StartPosition = FormStartPosition.CenterScreen;

            var tabs = new TabControl { Dock = DockStyle.Fill };

            // Report tab
            var reportTab = new TabPage("Report");
            var reportBox = new TextBox { Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Both, WordWrap = false, Dock = DockStyle.Fill, Font = new System.Drawing.Font("Consolas", 9f) };
            reportBox.Text = BuildReport(true).Replace("\n", Environment.NewLine);
            reportTab.Controls.Add(reportBox);

            // Ask AI tab
            var askTab = new TabPage("Ask AI (Opt-in)");
            var askLayout = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 5 };
            askLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            askLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25));
            askLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            askLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 75));
            askLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            var askInfo = new Label { Dock = DockStyle.Fill, AutoSize = true, Text = "Opt-in only. Sends selected snapshot fields to your configured local/cloud OpenAI-compatible endpoint. Never sends API key to logs." };
            var questionBox = new TextBox { Multiline = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Vertical, Font = new System.Drawing.Font("Segoe UI", 9f), Text = "What should I do next based on my class, professions, level, gear state, and current bot/profile?" };
            var askButtons = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
            var askBtn = new Button { Text = "Ask (uses current settings)", Width = 180 };
            var ctxBtn = new Button { Text = "Preview context snapshot", Width = 170 };
            var aiStatus = new Label { AutoSize = true, Text = _aiBusy ? "AI request in progress..." : "Idle" };
            askButtons.Controls.Add(askBtn);
            askButtons.Controls.Add(ctxBtn);
            askButtons.Controls.Add(aiStatus);
            var responseBox = new TextBox { Multiline = true, ReadOnly = true, Dock = DockStyle.Fill, ScrollBars = ScrollBars.Both, Font = new System.Drawing.Font("Consolas", 9f) };
            askLayout.Controls.Add(askInfo, 0, 0);
            askLayout.Controls.Add(questionBox, 0, 1);
            askLayout.Controls.Add(askButtons, 0, 2);
            askLayout.Controls.Add(responseBox, 0, 3);
            askTab.Controls.Add(askLayout);

            // Settings tab
            var settingsTab = new TabPage("AI Settings");
            var sPanel = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            int y = 10;
            Func<string, CheckBox> addCheck = (text) => { var cb = new CheckBox { Left = 10, Top = y, Width = 900, Text = text }; sPanel.Controls.Add(cb); y += 26; return cb; };
            Func<string, TextBox, Label> addText = (label, tb) => { var l = new Label { Left = 10, Top = y + 3, Width = 180, Text = label }; tb.Left = 195; tb.Top = y; tb.Width = 720; sPanel.Controls.Add(l); sPanel.Controls.Add(tb); y += 28; return l; };

            var cbExternal = addCheck("Enable external AI (master opt-in). OFF = no data sent.");
            var cbAsk = addCheck("Allow button-initiated Ask");
            var cbPeriodic = addCheck("Allow periodic background AI suggestions (opt-in)");
            var tbEndpoint = new TextBox(); addText("Endpoint URL:", tbEndpoint);
            var tbModel = new TextBox(); addText("Model:", tbModel);
            var tbKey = new TextBox(); tbKey.UseSystemPasswordChar = true; addText("API key (optional local):", tbKey);
            var tbPeriodic = new TextBox(); addText("Periodic minutes:", tbPeriodic);
            var tbTimeout = new TextBox(); addText("Timeout seconds:", tbTimeout);
            var cbChar = addCheck("Send character/class/level/combat summary");
            var cbLoc = addCheck("Send zone/area");
            var cbProf = addCheck("Send professions/skills");
            var cbInv = addCheck("Send bags/durability/gold/XP summary");
            var cbBot = addCheck("Send bot/profile state");
            var cbLogs = addCheck("Send recent log error/warning summary");
            var saveBtn = new Button { Left = 10, Top = y + 8, Width = 140, Text = "Save Settings" };
            var help = new Label { Left = 160, Top = y + 12, Width = 760, AutoSize = true, Text = "Local examples: Ollama http://localhost:11434/v1/chat/completions, LM Studio http://localhost:1234/v1/chat/completions" };
            sPanel.Controls.Add(saveBtn); sPanel.Controls.Add(help);
            settingsTab.Controls.Add(sPanel);

            // Bind settings
            cbExternal.Checked = settings.ExternalAiEnabled; cbAsk.Checked = settings.AllowAskButton; cbPeriodic.Checked = settings.AllowPeriodicSuggestions;
            tbEndpoint.Text = settings.EndpointUrl ?? ""; tbModel.Text = settings.Model ?? ""; tbKey.Text = settings.ApiKey ?? "";
            tbPeriodic.Text = settings.PeriodicMinutes.ToString(); tbTimeout.Text = settings.TimeoutSeconds.ToString();
            cbChar.Checked = settings.SendCharacter; cbLoc.Checked = settings.SendLocation; cbProf.Checked = settings.SendProfessions;
            cbInv.Checked = settings.SendInventorySummary; cbBot.Checked = settings.SendBotProfile; cbLogs.Checked = settings.SendRecentLogSummary;

            saveBtn.Click += (s, e) =>
            {
                try
                {
                    settings.ExternalAiEnabled = cbExternal.Checked; settings.AllowAskButton = cbAsk.Checked; settings.AllowPeriodicSuggestions = cbPeriodic.Checked;
                    settings.EndpointUrl = tbEndpoint.Text.Trim(); settings.Model = tbModel.Text.Trim(); settings.ApiKey = tbKey.Text;
                    int pm, to; if (int.TryParse(tbPeriodic.Text, out pm)) settings.PeriodicMinutes = Math.Max(5, pm); if (int.TryParse(tbTimeout.Text, out to)) settings.TimeoutSeconds = Math.Max(5, to);
                    settings.SendCharacter = cbChar.Checked; settings.SendLocation = cbLoc.Checked; settings.SendProfessions = cbProf.Checked;
                    settings.SendInventorySummary = cbInv.Checked; settings.SendBotProfile = cbBot.Checked; settings.SendRecentLogSummary = cbLogs.Checked;
                    settings.Save();
                    MessageBox.Show(form, "Jarvis settings saved.", "Jarvis", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex) { MessageBox.Show(form, ex.Message, "Jarvis settings error", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
            };

            ctxBtn.Click += (s, e) =>
            {
                try { responseBox.Text = BuildAiContextSnapshot(settings, false).Replace("\n", Environment.NewLine); }
                catch (Exception ex) { responseBox.Text = "Snapshot error: " + ex.Message; }
            };

            askBtn.Click += (s, e) =>
            {
                try
                {
                    if (!settings.ExternalAiEnabled) { responseBox.Text = "External AI is disabled. Enable it in AI Settings (opt-in)."; return; }
                    if (!settings.AllowAskButton) { responseBox.Text = "Button-initiated Ask is disabled in AI Settings."; return; }
                    if (_aiBusy) { responseBox.Text = "An AI request is already in progress."; return; }
                    string snapshot = BuildAiContextSnapshot(settings, false);
                    string question = questionBox.Text ?? "";
                    responseBox.Text = "Requesting AI response...";
                    aiStatus.Text = "Busy";
                    _aiBusy = true;
                    JarvisAiClient.RequestAsync(settings, question, snapshot, result =>
                    {
                        _aiBusy = false;
                        try
                        {
                            if (form.IsDisposed) return;
                            form.BeginInvoke(new Action(() =>
                            {
                                aiStatus.Text = "Idle";
                                if (result != null && result.Success) responseBox.Text = (result.Message ?? "").Replace("\n", Environment.NewLine);
                                else responseBox.Text = "AI error: " + (result != null ? (result.Error ?? "unknown") : "unknown") + (result != null && !string.IsNullOrEmpty(result.Message) ? Environment.NewLine + result.Message : "");
                            }));
                        }
                        catch { }
                    });
                }
                catch (Exception ex) { responseBox.Text = "Ask error: " + ex.Message; _aiBusy = false; aiStatus.Text = "Idle"; }
            };

            tabs.TabPages.Add(reportTab);
            tabs.TabPages.Add(askTab);
            tabs.TabPages.Add(settingsTab);

            var bottom = new Panel { Dock = DockStyle.Bottom, Height = 44 };
            var refresh = new Button { Text = "Refresh Report", Left = 10, Top = 8, Width = 120 };
            var copy = new Button { Text = "Copy Report", Left = 140, Top = 8, Width = 110 };
            var close = new Button { Text = "Close", Left = 260, Top = 8, Width = 90 };
            refresh.Click += (s, e) => { reportBox.Text = BuildReport(true).Replace("\n", Environment.NewLine); };
            copy.Click += (s, e) => { try { Clipboard.SetText(reportBox.Text); } catch { } };
            close.Click += (s, e) => form.Close();
            bottom.Controls.Add(refresh); bottom.Controls.Add(copy); bottom.Controls.Add(close);

            form.Controls.Add(tabs);
            form.Controls.Add(bottom);
            form.ShowDialog();
        }

        private struct LogSample { public DateTime Utc; public LogLevel Level; public string Text; }
        private class Suggestion { public string Key; public string Message; public Suggestion(string key, string message) { Key = key; Message = message; } }
    }
}
