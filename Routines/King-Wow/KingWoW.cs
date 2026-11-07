using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Styx.WoWInternals.DBC;
using Styx.WoWInternals.WoWObjects;
using Styx.WoWInternals;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.Helpers;
using Styx.Pathing;
using Styx;
using System.Diagnostics;
using System.Threading;
using System.IO;
using Styx.CommonBot;
using System.Windows.Forms;

namespace KingWoW
{
    class KingWoW : CombatRoutine
    {
        public override sealed string Name { get { return "KingWoW CC (by Attilio76)"; } }
        private static LocalPlayer Me { get { return StyxWoW.Me; } }
        
        public override WoWClass Class
        {
            get
            {
                if (Me.Class == WoWClass.Shaman && StyxWoW.Me.Specialization == WoWSpec.ShamanEnhancement || StyxWoW.Me.Specialization == WoWSpec.ShamanRestoration)
                    return WoWClass.Shaman;
                if (Me.Class == WoWClass.Mage ) 
                    return WoWClass.Mage;
                if (Me.Class == WoWClass.Paladin && 
                    (StyxWoW.Me.Specialization == WoWSpec.PaladinProtection || StyxWoW.Me.Specialization == WoWSpec.PaladinRetribution)) 
                    return WoWClass.Paladin;
                if (Me.Class == WoWClass.Priest &&
                    (StyxWoW.Me.Specialization == WoWSpec.PriestDiscipline || StyxWoW.Me.Specialization == WoWSpec.PriestHoly || StyxWoW.Me.Specialization == WoWSpec.PriestShadow)) 
                    return WoWClass.Priest;
                return WoWClass.None;
            }
        }
        private WoWSpec CurrentSpec = WoWSpec.None;
        private WoWSpec _lastSpec = WoWSpec.None;

        private readonly KingWoWUtility utils = new KingWoWUtility();
  
        private KingWoWAbstractBaseClass KingWoWBaseClass;
        public Styx.Helpers.Settings settingsBase;
        public string activePath;
        private KingHealSettingsGUIFORM Settings_Form = null;

        private void EnsureCurrentSpec()
        {
            if (Me == null || !Me.IsValid)
                return;

            var spec = Me.Specialization;
            if (spec == _lastSpec && KingWoWBaseClass != null)
            {
                CurrentSpec = spec;
                return;
            }

            // Spec changed or class not initialized: reset cached spec-specific state.
            CurrentSpec = spec;
            _lastSpec = spec;
            KingWoWBaseClass = null;
            settingsBase = null;
            activePath = null;
        }

        public KingWoWAbstractBaseClass ActiveKingWoWClass
        {
            get
            {
                EnsureCurrentSpec();
                if (KingWoWBaseClass != null)
                    return KingWoWBaseClass;

                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        KingWoWBaseClass = new RestorationShamanCombatClass();
                        break;
                    case WoWSpec.ShamanEnhancement:
                        KingWoWBaseClass = new EnhancementShamanCombatClass();
                        break;
                    case WoWSpec.MageFrost:
                        KingWoWBaseClass = new FrostMageCombatClass();
                        break;
                    case WoWSpec.MageArcane:
                        KingWoWBaseClass = new ArcaneMageCombatClass();
                        break;
                    case WoWSpec.MageFire:
                        KingWoWBaseClass = new FireMageCombatClass();
                        break;
                    case WoWSpec.PriestDiscipline:
                        KingWoWBaseClass = new DiscPriestCombatClass();
                        break;
                    case WoWSpec.PriestHoly:
                        KingWoWBaseClass = new HolyPriestCombatClass();
                        break;
                    case WoWSpec.PriestShadow:
                        KingWoWBaseClass = new ShadowPriestCombatClass();
                        break;
                    case WoWSpec.PaladinProtection:
                        KingWoWBaseClass = new ProtPaladinCombatClass();
                        break;
                    case WoWSpec.PaladinRetribution:
                        KingWoWBaseClass = new RetriPaladinCombatClass();
                        break;
                    default:
                        KingWoWBaseClass = null;
                        break;
                }
                if (KingWoWBaseClass == null)
                {
                    utils.LogActivity("Current spec: " + CurrentSpec.ToString() + " is not supported (MoP 5.4.8).");
                }
                
                return KingWoWBaseClass;
            }
        }

        public Styx.Helpers.Settings ActiveSettings
        {
            get
            {
                EnsureCurrentSpec();
                if (settingsBase != null)
                    return settingsBase;

                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        settingsBase = RestorationShamanSettings.Instance;
                        break;
                    case WoWSpec.ShamanEnhancement:
                        settingsBase = EnhancementShamanSettings.Instance;
                        break;
                    case WoWSpec.MageFrost:
                        settingsBase = FrostMageSettings.Instance;
                        break;
                    case WoWSpec.MageArcane:
                        settingsBase = ArcaneMageSettings.Instance;
                        break;
                    case WoWSpec.MageFire:
                        settingsBase = FireMageSettings.Instance;
                        break;
                    case WoWSpec.PriestDiscipline:
                        settingsBase = DiscPriestSettings.Instance;
                        break;
                    case WoWSpec.PriestHoly:
                        settingsBase = HolyPriestSettings.Instance;
                        break;
                    case WoWSpec.PriestShadow:
                        settingsBase = ShadowPriestSettings.Instance;
                        break;
                    case WoWSpec.PaladinProtection:
                        settingsBase = ProtPaladinSettings.Instance;
                        break;
                    case WoWSpec.PaladinRetribution:
                        settingsBase = RetriPaladinSettings.Instance;
                        break;
                    default:
                        settingsBase = null;
                        break;
                }
                if (settingsBase == null)
                {
                    utils.LogActivity("No settings available for spec: " + CurrentSpec.ToString());
                }

                return settingsBase;   
            }
        }

        public string ActivePath
        {
            get
            {
                EnsureCurrentSpec();
                if (activePath != null)
                    return activePath;

                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        activePath = RestorationShamanSettings.Instance.path_name;
                        break;
                    case WoWSpec.ShamanEnhancement:
                        activePath = EnhancementShamanSettings.Instance.path_name;
                        break;
                    case WoWSpec.MageFrost:
                        activePath = FrostMageSettings.Instance.path_name;
                        break;
                    case WoWSpec.MageArcane:
                        activePath = ArcaneMageSettings.Instance.path_name;
                        break;
                    case WoWSpec.MageFire:
                        activePath = FireMageSettings.Instance.path_name;
                        break;
                    case WoWSpec.PriestDiscipline:
                        activePath = DiscPriestSettings.Instance.path_name;
                        break;
                    case WoWSpec.PriestHoly:
                        activePath = HolyPriestSettings.Instance.path_name;
                        break;
                    case WoWSpec.PriestShadow:
                        activePath = ShadowPriestSettings.Instance.path_name;
                        break;
                    case WoWSpec.PaladinProtection:
                        activePath = ProtPaladinSettings.Instance.path_name;
                        break;
                    case WoWSpec.PaladinRetribution:
                        activePath = RetriPaladinSettings.Instance.path_name;
                        break;
                    default:
                        activePath = null;
                        break;
                }
                if (activePath == null)
                {
                    utils.LogActivity("No settings path available for spec: " + CurrentSpec.ToString());
                }

                return activePath;
            }
        }

        public override void Combat()
        {
            var cc = ActiveKingWoWClass;
            if (cc != null && cc.Combat) return;
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override void OnButtonPress()
        {
            try
            {
                var settings = ActiveSettings;
                var path = ActivePath;
                if (settings == null || string.IsNullOrEmpty(path))
                {
                    utils.LogActivity("KingWoW: no settings available for current spec.");
                    return;
                }

                if (Settings_Form == null || Settings_Form.IsDisposed || Settings_Form.Disposing)
                {
                    Settings_Form = new KingHealSettingsGUIFORM(settings, path, ExtraUtilsSettings.Instance.path_name);
                }
                Settings_Form.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging.Write("KingWoW: exception opening settings form: " + ex.Message);
                Logging.WriteDiagnostic(ex.ToString());
            }
        }

        public override void Pulse()
        {
            var cc = ActiveKingWoWClass;
            if (cc != null && cc.Pulse) return;
        }

        public override void Initialize()
        {
            var cc = ActiveKingWoWClass;
            if (cc != null && cc.Initialize) return;
        }

        public override bool NeedRest
        {
            get
            {
                var cc = ActiveKingWoWClass;
                return cc != null && cc.NeedRest;
            }
        }

        public override bool NeedCombatBuffs
        {
            get
            {
                var cc = ActiveKingWoWClass;
                return cc != null && cc.NeedCombatBuffs;
            }
        }

        public override bool NeedPreCombatBuffs
        {
            get
            {
                var cc = ActiveKingWoWClass;
                return cc != null && cc.NeedPreCombatBuffs;
            }
        }

        public override bool NeedPullBuffs
        {
            get
            {
                var cc = ActiveKingWoWClass;
                return cc != null && cc.NeedPullBuffs;
            }
        }

        public override void Pull()
        {
            var cc = ActiveKingWoWClass;
            if (cc != null && cc.Pull) return;
        }

    }
}
