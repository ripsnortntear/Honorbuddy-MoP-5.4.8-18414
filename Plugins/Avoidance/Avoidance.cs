//!CompilerOption:AddRef:Tripper.Tools.dll
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Styx;
using Styx.Common;
using Styx.CommonBot;
using Styx.Pathing;
using Styx.Plugins;
using Styx.TreeSharp;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;
using Action = Styx.TreeSharp.Action;

namespace Avoidance
{
#pragma warning disable 672
    public class Avoidance : HBPlugin
    {
        #region Metadata
        public override string Name { get { return "Avoidance"; } }
        public override string Author { get { return "Antigravity"; } }
        public override Version Version { get { return new Version(1, 0, 0); } }
        public override bool WantButton { get { return true; } }
        public override string ButtonText { get { return "Settings"; } }
        #endregion

        #region Fields & State
        private static readonly AvoidanceSettings Settings = AvoidanceSettings.Instance;
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        private Composite _hookComposite;
        private bool _hooksInstalled;
        private bool _isAvoiding;
        private bool _ctmOriginallyDisabled;
        private WoWPoint _currentSafeSpot = WoWPoint.Zero;
        private DateTime _lastScanTime = DateTime.MinValue;
        private DateTime _avoidanceStartTime = DateTime.MinValue;
        private readonly List<HazardInfo> _currentHazards = new List<HazardInfo>();

        private static readonly string[] HookNames = new[]
        {
            "Combat_Main",
            "Combat_Only",
            "Questbot_Main"
        };

        // Whitelist of beneficial ground effects and player-class AoE ground spells that should NOT be avoided when cast by allies
        private static readonly HashSet<int> FriendlySpellWhitelist = new HashSet<int>
        {
            // Paladin
            26573, 81297, 110701, 118283, // Consecration
            114158,                  // Light's Hammer

            // Warlock
            5740, 42223, 104232, 104233, // Rain of Fire
            1949,                    // Hellfire
            30283,                   // Shadowfury
            48018, 48020,            // Demonic Circle
            111771, 113902,          // Demonic Gateway
            698,                     // Ritual of Summoning
            29893,                   // Create Soulwell

            // Mage
            10, 42208, 110959, 110960,   // Blizzard
            2120, 44461,             // Flamestrike
            82676, 113724,           // Ring of Frost
            84714, 84721,            // Frozen Orb
            116011,                  // Rune of Power

            // Death Knight
            43265, 52212,            // Death and Decay
            51052, 50461, 145629,    // Anti-Magic Zone
            108201,                  // Desecrated Ground
            108200,                  // Remorseless Winter

            // Druid
            16914, 42231,            // Hurricane
            127663,                  // Astral Storm
            78675,                   // Solar Beam
            102793,                  // Ursol's Vortex
            88747, 102791, 102792,   // Wild Mushroom
            81262, 81269, 145205,    // Efflorescence

            // Hunter
            1543,                    // Flare
            1499, 60192,             // Freezing Trap
            13809,                   // Ice Trap
            13813,                   // Explosive Trap
            34600,                   // Snake Trap
            109248, 117405,          // Binding Shot

            // Priest
            88685, 88686,            // Holy Word: Sanctuary
            62618,                   // Power Word: Barrier
            724, 126135,             // Lightwell / Lightspring
            78203,                   // Shadowy Apparition

            // Shaman
            73920, 73921,            // Healing Rain
            61882, 77478,            // Earthquake
            8190,                    // Magma Totem
            98008, 98007,            // Spirit Link Totem
            108280,                  // Healing Tide Totem
            2484, 51485, 6474,       // Earthbind / Earthgrab Totem
            108269,                  // Capacitor Totem

            // Monk
            115460,                  // Healing Sphere
            116844,                  // Ring of Peace
            101546,                  // Spinning Crane Kick
            116847,                  // Rushing Jade Wind
            115313, 115315,          // Black Ox Statue / Jade Serpent Statue

            // Rogue
            76577,                   // Smoke Bomb
            114018,                  // Shroud of Concealment

            // Warrior
            114192, 114198, 114207,  // Banners (Demoralizing, Mocking, Skull)
            118000, 107570           // Ravager / Dragon Roar
        };

        private static readonly string[] FriendlySpellNameKeywords = new[]
        {
            "Consecration",
            "Rain of Fire",
            "Hellfire",
            "Shadowfury",
            "Demonic Circle",
            "Demonic Gateway",
            "Blizzard",
            "Flamestrike",
            "Ring of Frost",
            "Frozen Orb",
            "Rune of Power",
            "Death and Decay",
            "Anti-Magic Zone",
            "Desecrated Ground",
            "Remorseless Winter",
            "Hurricane",
            "Astral Storm",
            "Solar Beam",
            "Ursol's Vortex",
            "Wild Mushroom",
            "Efflorescence",
            "Flare",
            "Freezing Trap",
            "Ice Trap",
            "Explosive Trap",
            "Snake Trap",
            "Binding Shot",
            "Light's Hammer",
            "Sanctuary",
            "Power Word: Barrier",
            "Lightwell",
            "Lightspring",
            "Healing Rain",
            "Earthquake",
            "Spirit Link",
            "Healing Tide",
            "Healing Sphere",
            "Ring of Peace",
            "Spinning Crane Kick",
            "Rushing Jade Wind",
            "Smoke Bomb",
            "Shroud of Concealment",
            "Banner",
            "Dragon Roar"
        };
        #endregion

        #region Lifecycle & Events
        public override void Initialize()
        {
            Settings.Load();
            Log("Avoidance v{0} initialized.", Version);

            BotEvents.OnBotStarted += BotEventsOnBotStarted;
            BotEvents.OnBotStopped += BotEventsOnBotStopped;
            BotEvents.OnBotChanged += BotEventsOnBotChanged;
            TreeHooks.Instance.HooksCleared += TreeHooksOnHooksCleared;

            EnsureBotBasePulsing();
            InstallHooks();
        }

        public override void Dispose()
        {
            RemoveHooks();

            BotEvents.OnBotStarted -= BotEventsOnBotStarted;
            BotEvents.OnBotStopped -= BotEventsOnBotStopped;
            BotEvents.OnBotChanged -= BotEventsOnBotChanged;
            TreeHooks.Instance.HooksCleared -= TreeHooksOnHooksCleared;

            RestoreCtmIfNeeded();
            Log("Avoidance disposed.");
        }

        public override void OnEnable()
        {
            Settings.Load();
            EnsureBotBasePulsing();
            InstallHooks();
            Log("Avoidance enabled.");
        }

        public override void OnDisable()
        {
            RemoveHooks();
            StopAvoidance("Plugin disabled");
            RestoreCtmIfNeeded();
            Log("Avoidance disabled.");
        }

        public override void OnButtonPress()
        {
            var form = new AvoidanceConfigForm();
            form.ShowDialog();
        }

        private void BotEventsOnBotStarted(EventArgs args)
        {
            EnsureBotBasePulsing();
            InstallHooks();
        }

        private void BotEventsOnBotChanged(BotEvents.BotChangedEventArgs args)
        {
            EnsureBotBasePulsing();
            InstallHooks();
        }

        private void BotEventsOnBotStopped(EventArgs args)
        {
            StopAvoidance("Bot stopped");
            RestoreCtmIfNeeded();
        }

        private void TreeHooksOnHooksCleared()
        {
            _hooksInstalled = false;
            InstallHooks();
        }

        /// <summary>
        /// Ensures that BotBases with manual user control (such as Tyrael or LazyRaider)
        /// have PulseFlags.Plugins enabled, allowing Avoidance to monitor hazards even
        /// when rotation routines have movement disabled.
        /// </summary>
        private void EnsureBotBasePulsing()
        {
            try
            {
                var bot = TreeRoot.Current;
                if (bot == null)
                    return;

                if ((bot.PulseFlags & PulseFlags.Plugins) == 0)
                {
                    var botType = bot.GetType();
                    var flagsField = botType.GetField("_pulseFlags", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (flagsField != null)
                    {
                        var flags = (PulseFlags)flagsField.GetValue(bot);
                        if ((flags & PulseFlags.Plugins) == 0)
                        {
                            flags |= PulseFlags.Plugins;
                            flagsField.SetValue(bot, flags);
                            Log("Enabled Plugin Pulsing on [{0}] so Avoidance can operate with manual control.", bot.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogDebug("EnsureBotBasePulsing exception: {0}", ex.Message);
            }
        }
        #endregion

        #region TreeHook Installation
        private void InstallHooks()
        {
            if (_hooksInstalled)
                return;

            if (_hookComposite == null)
            {
                _hookComposite = new Decorator(
                    ret => Settings.Enabled && ShouldAvoid(),
                    new Action(ret => RunAvoidance())
                );
            }

            foreach (var hookName in HookNames)
            {
                try
                {
                    TreeHooks.Instance.InsertHook(hookName, 0, _hookComposite);
                    LogDebug("Inserted avoidance hook into {0}.", hookName);
                }
                catch (Exception ex)
                {
                    LogDebug("Failed to insert hook into {0}: {1}", hookName, ex.Message);
                }
            }

            _hooksInstalled = true;
        }

        private void RemoveHooks()
        {
            if (!_hooksInstalled || _hookComposite == null)
                return;

            foreach (var hookName in HookNames)
            {
                try
                {
                    TreeHooks.Instance.RemoveHook(hookName, _hookComposite);
                    LogDebug("Removed avoidance hook from {0}.", hookName);
                }
                catch (Exception ex)
                {
                    LogDebug("Failed to remove hook from {0}: {1}", hookName, ex.Message);
                }
            }

            _hooksInstalled = false;
        }
        #endregion

        #region Pulse (Fallback / Out of Combat / Standalone)
        public override void Pulse()
        {
            if (!Settings.Enabled)
                return;

            if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || !Me.IsAlive || Me.IsGhost)
                return;

            // Ensure hooks stay registered if cleared
            if (!_hooksInstalled)
            {
                InstallHooks();
            }

            // Rate throttle scan
            if ((DateTime.UtcNow - _lastScanTime).TotalMilliseconds < Settings.ThrottleMs)
                return;

            _lastScanTime = DateTime.UtcNow;

            // If we are currently avoiding, manage movement in pulse as well
            if (_isAvoiding)
            {
                ManageAvoidanceState();
                return;
            }

            // Check if standing in something
            if (ShouldAvoid())
            {
                RunAvoidance();
            }
        }
        #endregion

        #region Threat Detection
        private bool ShouldAvoid()
        {
            if (!Settings.Enabled)
                return false;

            if (!StyxWoW.IsInWorld || Me == null || !Me.IsValid || !Me.IsAlive || Me.IsGhost)
                return false;

            // Don't interrupt mounted movement/flight unless configured
            if (Me.Mounted || Me.IsOnTransport)
                return false;

            if (Settings.OnlyInCombat && !Me.IsActuallyInCombat)
                return false;

            _currentHazards.Clear();
            var myLoc = Me.Location;

            // 1. Check Dynamic Objects (e.g. Blizzard, Rain of Fire, Death & Decay, Void Zones, Poison Clouds)
            if (Settings.AvoidDynamicObjects)
            {
                var dynObjects = ObjectManager.GetObjectsOfType<WoWDynamicObject>(true, false);
                foreach (var dyn in dynObjects)
                {
                    if (!dyn.IsValid)
                        continue;

                    // Skip our own effects
                    if (dyn.CasterGuid == Me.Guid)
                        continue;

                    // Skip friendly beneficial effects or whitelisted spells (e.g. Consecration)
                    if (IsWhitelisted(dyn.SpellId, dyn.Spell != null ? dyn.Spell.Name : null) ||
                        IsFriendlyHelpful(dyn.Spell, dyn.Caster, dyn.CasterGuid))
                        continue;

                    float radius = dyn.Radius;
                    if (radius <= 0.1f)
                        radius = 4.0f; // Default baseline radius

                    float effectiveRadius = radius + Settings.SafetyBuffer;
                    if (myLoc.Distance2D(dyn.Location) <= effectiveRadius)
                    {
                        string spellName = dyn.Spell != null ? dyn.Spell.Name : ("Spell #" + dyn.SpellId);
                        _currentHazards.Add(new HazardInfo(dyn.Location, effectiveRadius, spellName, dyn.SpellId, HazardSource.DynamicObject, dyn.Guid));
                    }
                }
            }

            // 2. Check Area Triggers (e.g. traps, boss ground lines, environmental puddles)
            if (Settings.AvoidAreaTriggers)
            {
                var areaTriggers = ObjectManager.GetObjectsOfType<WoWAreaTrigger>(true, false);
                foreach (var at in areaTriggers)
                {
                    if (!at.IsValid)
                        continue;

                    if (at.CasterGuid == Me.Guid)
                        continue;

                    // Skip friendly beneficial effects or whitelisted spells (e.g. Consecration)
                    if (IsWhitelisted(at.SpellId, at.Spell != null ? at.Spell.Name : null) ||
                        IsFriendlyHelpful(at.Spell, at.Caster as WoWUnit, at.CasterGuid))
                        continue;

                    float radius = 3.5f;
                    if (at.Spell != null && at.Spell.MaxRange > 0 && at.Spell.MaxRange <= 25f)
                        radius = at.Spell.MaxRange;

                    if (at.Scale > 0.1f && at.Scale < 5.0f)
                        radius *= at.Scale;

                    if (radius > 25.0f)
                        radius = 25.0f;

                    float effectiveRadius = radius + Settings.SafetyBuffer;
                    if (myLoc.Distance2D(at.Location) <= effectiveRadius)
                    {
                        string spellName = at.Spell != null ? at.Spell.Name : ("AreaTrigger #" + at.SpellId);
                        _currentHazards.Add(new HazardInfo(at.Location, effectiveRadius, spellName, at.SpellId, HazardSource.AreaTrigger, at.Guid));
                    }
                }
            }

            // 3. Check Harmful Ground Debuffs / Ticking DoTs on player
            if (Settings.AvoidGroundAuras)
            {
                foreach (var aura in Me.GetAllAuras())
                {
                    if (!aura.IsHarmful)
                        continue;

                    // If a debuff on the player is harmful and dealing damage, it is ticking HP away!
                    // Only skip if explicitly in user's custom whitelist
                    if (IsCustomWhitelisted(aura.SpellId, aura.Name))
                        continue;

                    bool isGroundDamage = false;
                    if (aura.Spell != null)
                    {
                        foreach (var effect in aura.Spell.SpellEffects)
                        {
                            if (effect == null)
                                continue;

                            if (effect.EffectType == WoWSpellEffectType.PersistentAreaAura ||
                                effect.EffectType == WoWSpellEffectType.ApplyAreaAuraEnemy ||
                                effect.AuraType == WoWApplyAuraType.PeriodicDamage ||
                                effect.AuraType == WoWApplyAuraType.PeriodicDamagePercent)
                            {
                                isGroundDamage = true;
                                break;
                            }
                        }
                    }

                    if (isGroundDamage)
                    {
                        // Any harmful aura actively ticking damage away is bad!
                        if (!_currentHazards.Any(h => h.SpellId == aura.SpellId))
                        {
                            float radius = 4.0f + Settings.SafetyBuffer;
                            _currentHazards.Add(new HazardInfo(myLoc, radius, aura.Name, aura.SpellId, HazardSource.GroundAura));
                        }
                    }
                }
            }

            return _currentHazards.Count > 0;
        }

        private static bool IsPartyOrFriendly(ulong casterGuid, WoWUnit caster)
        {
            if (casterGuid == Me.Guid)
                return true;

            // Direct check against group / raid member GUIDs
            if (Me.GroupInfo != null)
            {
                var partyGuids = Me.GroupInfo.PartyMemberGuids;
                if (partyGuids != null && partyGuids.Contains(casterGuid))
                    return true;

                var raidGuids = Me.GroupInfo.RaidMemberGuids;
                if (raidGuids != null && raidGuids.Contains(casterGuid))
                    return true;
            }

            if (caster == null && casterGuid != 0)
            {
                caster = ObjectManager.GetObjectByGuid<WoWUnit>(casterGuid);
            }

            if (caster != null)
            {
                // Friendly player or pet created by player
                if (caster.IsFriendly && (caster.IsPlayer || caster.CreatedByUnitGuid == Me.Guid))
                    return true;

                // Pet or minion created by party/raid member
                if (caster.CreatedByUnitGuid != 0 && Me.GroupInfo != null)
                {
                    var partyGuids = Me.GroupInfo.PartyMemberGuids;
                    if (partyGuids != null && partyGuids.Contains(caster.CreatedByUnitGuid))
                        return true;

                    var raidGuids = Me.GroupInfo.RaidMemberGuids;
                    if (raidGuids != null && raidGuids.Contains(caster.CreatedByUnitGuid))
                        return true;
                }
            }

            return false;
        }

        private static bool IsCustomWhitelisted(int spellId, string spellName)
        {
            if (string.IsNullOrEmpty(Settings.CustomWhitelist))
                return false;

            var tokens = Settings.CustomWhitelist.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var t in tokens)
            {
                var trimmed = t.Trim();
                int id;
                if (int.TryParse(trimmed, out id))
                {
                    if (spellId != 0 && id == spellId)
                        return true;
                }
                else if (!string.IsNullOrEmpty(spellName) &&
                         spellName.IndexOf(trimmed, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool IsWhitelisted(int spellId, string spellName)
        {
            if (spellId != 0 && FriendlySpellWhitelist.Contains(spellId))
                return true;

            if (!string.IsNullOrEmpty(spellName))
            {
                foreach (var kw in FriendlySpellNameKeywords)
                {
                    if (spellName.IndexOf(kw, StringComparison.OrdinalIgnoreCase) >= 0)
                        return true;
                }
            }

            return IsCustomWhitelisted(spellId, spellName);
        }

        private static bool IsFriendlyHelpful(WoWSpell spell, WoWUnit caster, ulong casterGuid)
        {
            // 1. You or your pet cast it
            if (casterGuid == Me.Guid)
                return true;

            // 2. If the caster is known and hostile, it is an ENEMY attack -> NOT safe to stand in!
            if (caster != null && caster.IsHostile)
                return false;

            // 3. Anything cast by a party member, raid member, or friendly unit is safe
            if (IsPartyOrFriendly(casterGuid, caster))
                return true;

            // 4. Healing / beneficial buff spells
            if (spell != null && spell.IsHealingSpell)
                return true;

            // 5. Whitelisted player class ground AoE (when not cast by a hostile enemy)
            int spellId = spell != null ? spell.Id : 0;
            string spellName = spell != null ? spell.Name : null;

            if (IsWhitelisted(spellId, spellName))
                return true;

            return false;
        }
        #endregion

        #region Safe Spot Calculation (Shortest Distance to Safety)
        private WoWPoint CalculateClosestSafeSpot(List<HazardInfo> hazards)
        {
            var myLoc = Me.Location;
            if (hazards.Count == 0)
                return myLoc;

            var primaryHazard = hazards.OrderBy(h => myLoc.Distance2D(h.Location)).First();
            var candidates = new List<Tuple<WoWPoint, float>>();

            // 1. Direct outward exit vector from primary hazard
            var diff = myLoc - primaryHazard.Location;
            var dir = new WoWPoint(diff.X, diff.Y, 0);

            if (dir.Length < 0.1f)
            {
                // Player is right on the center of the hazard; project forward or backward based on player facing
                float rot = Me.Rotation;
                dir = new WoWPoint((float)Math.Cos(rot), (float)Math.Sin(rot), 0);
            }
            else
            {
                dir.Normalize();
            }

            var directExitPoint = primaryHazard.Location + (dir * (primaryHazard.Radius + 0.5f));
            float directHeight;
            if (Navigator.FindHeight(directExitPoint.X, directExitPoint.Y, out directHeight))
                directExitPoint.Z = directHeight;
            else
                directExitPoint.Z = myLoc.Z;

            if (IsPointSafeFromAll(directExitPoint, hazards))
            {
                candidates.Add(Tuple.Create(directExitPoint, myLoc.Distance(directExitPoint)));
            }

            // 2. Radial sweep around the player (24 directions, 15-degree intervals)
            float maxRadius = hazards.Max(h => h.Radius);
            float maxTestDistance = maxRadius + Settings.SafetyBuffer + 3.0f;

            for (float angleDeg = 0; angleDeg < 360f; angleDeg += 15f)
            {
                double angleRad = angleDeg * Math.PI / 180.0;
                float rx = (float)Math.Cos(angleRad);
                float ry = (float)Math.Sin(angleRad);
                var rayDir = new WoWPoint(rx, ry, 0);

                // Step outward from 2 yards up to max test distance
                for (float testDist = 2.0f; testDist <= maxTestDistance; testDist += 1.5f)
                {
                    var testPoint = myLoc + (rayDir * testDist);

                    // Check if test point is outside all hazards
                    if (!IsPointSafeFromAll(testPoint, hazards))
                        continue;

                    // Validate terrain height
                    float height;
                    if (Navigator.FindHeight(testPoint.X, testPoint.Y, out height))
                    {
                        if (Math.Abs(height - myLoc.Z) > 3.5f)
                            continue; // Too steep / off a cliff or through floor
                        testPoint.Z = height;
                    }
                    else
                    {
                        testPoint.Z = myLoc.Z;
                    }

                    // Check mesh line-of-sight / pathability if navigation provider available
                    if (Navigator.NavigationProvider != null)
                    {
                        if (!Navigator.CanNavigateFully(myLoc, testPoint))
                            continue;
                    }

                    float travelDist = myLoc.Distance(testPoint);
                    candidates.Add(Tuple.Create(testPoint, travelDist));
                    break; // First safe distance along this ray is the shortest for this angle
                }
            }

            if (candidates.Count > 0)
            {
                // Return candidate with the absolute minimum distance to player
                return candidates.OrderBy(c => c.Item2).First().Item1;
            }

            // Fallback: use direct outward point even if unmeshed so Click-To-Move can escape
            return directExitPoint;
        }

        private static bool IsPointSafeFromAll(WoWPoint point, List<HazardInfo> hazards)
        {
            foreach (var h in hazards)
            {
                if (point.Distance2D(h.Location) <= h.Radius)
                    return false;
            }
            return true;
        }
        #endregion

        #region Avoidance Execution & CTM Control
        private RunStatus RunAvoidance()
        {
            if (_currentHazards.Count == 0)
            {
                StopAvoidance("No hazards remaining");
                return RunStatus.Failure;
            }

            // Calculate closest safe spot
            _currentSafeSpot = CalculateClosestSafeSpot(_currentHazards);

            if (_currentSafeSpot == WoWPoint.Zero)
            {
                StopAvoidance("Could not determine safe spot");
                return RunStatus.Failure;
            }

            // Activate Click To Move if enabled in settings
            EnableCtmIfNeeded();

            _isAvoiding = true;
            _avoidanceStartTime = DateTime.UtcNow;

            string hazardSummary = string.Join(", ", _currentHazards.Select(h => h.Name).Distinct());
            Log("Standing in ground effect: [{0}]. Moving to safety at {1} (Distance: {2:F1} yds)...",
                hazardSummary, _currentSafeSpot, Me.Location.Distance(_currentSafeSpot));

            // Issue movement
            IssueMoveCommand(_currentSafeSpot);

            return RunStatus.Running;
        }

        private void ManageAvoidanceState()
        {
            if (!_isAvoiding)
                return;

            // Timeout safety check (if stuck or took more than 8 seconds, release)
            if ((DateTime.UtcNow - _avoidanceStartTime).TotalSeconds > 8.0)
            {
                StopAvoidance("Avoidance movement timed out (8s)");
                return;
            }

            // Re-check current hazards
            if (!ShouldAvoid())
            {
                StopAvoidance("Successfully reached safety outside hazard");
                return;
            }

            // Check if reached destination
            if (Me.Location.Distance2D(_currentSafeSpot) <= 1.2f)
            {
                // We reached the safe spot; check if still in hazard
                if (!ShouldAvoid())
                {
                    StopAvoidance("Arrived at safe spot");
                    return;
                }
                else
                {
                    // Recalculate if still in danger
                    _currentSafeSpot = CalculateClosestSafeSpot(_currentHazards);
                }
            }

            // Keep moving towards safe spot
            IssueMoveCommand(_currentSafeSpot);
        }

        private void IssueMoveCommand(WoWPoint destination)
        {
            if (Settings.UseClickToMove)
            {
                WoWMovement.ClickToMove(destination);
            }

            // Also call Navigator so mesh pathing can guide around corners
            try
            {
                Navigator.MoveTo(destination);
            }
            catch
            {
                // Fallback to Click-To-Move if Navigator has no mesh
                WoWMovement.ClickToMove(destination);
            }
        }

        private void EnableCtmIfNeeded()
        {
            if (!Settings.UseClickToMove)
                return;

            try
            {
                int ctm = Lua.GetReturnVal<int>("return GetCVar('autoInteract')", 0);
                if (ctm == 0)
                {
                    _ctmOriginallyDisabled = true;
                    Lua.DoString("SetCVar('autoInteract', '1')");
                    LogDebug("Turned ON Click To Move for avoidance maneuver.");
                }
            }
            catch (Exception ex)
            {
                LogDebug("Error enabling CTM: {0}", ex.Message);
            }
        }

        private void RestoreCtmIfNeeded()
        {
            if (!_ctmOriginallyDisabled || !Settings.RestoreClickToMove)
                return;

            try
            {
                Lua.DoString("SetCVar('autoInteract', '0')");
                _ctmOriginallyDisabled = false;
                LogDebug("Restored Click To Move to OFF.");
            }
            catch (Exception ex)
            {
                LogDebug("Error restoring CTM: {0}", ex.Message);
            }
        }

        private void StopAvoidance(string reason)
        {
            if (!_isAvoiding)
                return;

            _isAvoiding = false;
            _currentSafeSpot = WoWPoint.Zero;
            _currentHazards.Clear();

            try
            {
                WoWMovement.MoveStop();
            }
            catch
            {
            }

            RestoreCtmIfNeeded();
            Log("Avoidance finished: {0}.", reason);
        }
        #endregion

        #region Helpers & Logging
        private static void Log(string format, params object[] args)
        {
            Logging.Write(string.Format("[Avoidance] " + format, args));
        }

        private static void LogDebug(string format, params object[] args)
        {
            if (Settings.DebugLogging)
            {
                Logging.WriteDiagnostic(string.Format("[Avoidance Debug] " + format, args));
            }
        }
        #endregion
    }

    #region Hazard Data Structures
    public enum HazardSource
    {
        DynamicObject,
        AreaTrigger,
        GroundAura
    }

    public class HazardInfo
    {
        public WoWPoint Location { get; set; }
        public float Radius { get; set; }
        public string Name { get; set; }
        public int SpellId { get; set; }
        public HazardSource Source { get; set; }
        public ulong Guid { get; set; }

        public HazardInfo(WoWPoint location, float radius, string name, int spellId, HazardSource source, ulong guid = 0)
        {
            Location = location;
            Radius = radius;
            Name = name;
            SpellId = spellId;
            Source = source;
            Guid = guid;
        }
    }
    #endregion
}
