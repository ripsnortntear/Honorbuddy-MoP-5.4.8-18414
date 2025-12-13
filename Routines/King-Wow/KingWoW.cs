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

        private KingWoWUtility utils = new KingWoWUtility();
  
        private static KingWoWAbstractBaseClass KingWoWBaseClass;
        public Styx.Helpers.Settings settingsBase;
        public string activePath;
        private KingWoWUtility u = new KingWoWUtility();
        private KingHealSettingsGUIFORM Settings_Form = null;

        public KingWoWAbstractBaseClass ActiveKingWoWClass
        {
            get
            {
                // SPEC
                CurrentSpec = StyxWoW.Me.Specialization;
                
                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new RestorationShamanCombatClass();
                            break;
                        }
                    case WoWSpec.ShamanEnhancement:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new EnhancementShamanCombatClass();
                            break;
                        }
                    case WoWSpec.MageFrost:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new FrostMageCombatClass();
                            break;
                        }
                    case WoWSpec.MageArcane:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new ArcaneMageCombatClass();
                            break;
                        }
                    case WoWSpec.MageFire:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new FireMageCombatClass();
                            break;
                        }
                    case WoWSpec.PriestDiscipline:
                        {
                            if(KingWoWBaseClass==null)
                                KingWoWBaseClass = new DiscPriestCombatClass();
                            break;
                        }
                    case WoWSpec.PriestHoly:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new HolyPriestCombatClass();
                            break;
                        }
                    case WoWSpec.PriestShadow:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new ShadowPriestCombatClass();
                            break;
                        }
                    case WoWSpec.PaladinProtection:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new ProtPaladinCombatClass();
                            break;
                        }
                    case WoWSpec.PaladinRetribution:
                        {
                            if (KingWoWBaseClass == null)
                                KingWoWBaseClass = new RetriPaladinCombatClass();
                            break;
                        }
                    default:
                        {
                            KingWoWBaseClass = null;
                            break;
                        }
                }
                if (KingWoWBaseClass == null)
                {
                    utils.LogActivity("Current spec: "+CurrentSpec.ToString()+" not Supported Spec atm stay tuned for update");
                }
                
                return KingWoWBaseClass;
            }
        }

        public Styx.Helpers.Settings ActiveSettings
        {
            get
            {
                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        {
                            if (settingsBase == null)
                                settingsBase = RestorationShamanSettings.Instance;
                            break;
                        }    
                    case WoWSpec.ShamanEnhancement:
                        {
                            if (settingsBase == null)
                                settingsBase = EnhancementShamanSettings.Instance;
                            break;
                        }    
                    case WoWSpec.MageFrost:
                        {
                            if (settingsBase == null)
                                settingsBase = FrostMageSettings.Instance;
                            break;
                        }
                    case WoWSpec.MageArcane:
                        {
                            if (settingsBase == null)
                                settingsBase = ArcaneMageSettings.Instance;
                            break;
                        }
                    case WoWSpec.MageFire:
                        {
                            if (settingsBase == null)
                                settingsBase = FireMageSettings.Instance;
                            break;
                        }
                    case WoWSpec.PriestDiscipline:
                        {
                            if (settingsBase == null)
                                settingsBase = DiscPriestSettings.Instance;
                            break;
                        }
                    case WoWSpec.PriestHoly:
                        {
                            if (settingsBase == null)
                                settingsBase = HolyPriestSettings.Instance;
                            break;
                        }
                    case WoWSpec.PriestShadow:
                        {
                            if (settingsBase == null)
                                settingsBase = ShadowPriestSettings.Instance;
                            break;
                        }
                    case WoWSpec.PaladinProtection:
                        {
                            if (settingsBase == null)
                                settingsBase = ProtPaladinSettings.Instance;
                            break;
                        }
                    case WoWSpec.PaladinRetribution:
                        {
                            if (settingsBase == null)
                                settingsBase = RetriPaladinSettings.Instance;
                            break;
                        }
                    default:
                        {
                            settingsBase = null;
                            break;
                        }
                }
                if (settingsBase == null)
                {
                    utils.LogActivity("No Setting for this spec please stay tuned for updates");
                    TreeRoot.Stop();
                }

                return settingsBase;   
            }
        }

        public string ActivePath
        {
            get
            {
                switch (CurrentSpec)
                {
                    case WoWSpec.ShamanRestoration:
                        {
                            if (activePath == null)
                                activePath = RestorationShamanSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.ShamanEnhancement:
                        {
                            if (activePath == null)
                                activePath = EnhancementShamanSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.MageFrost:
                        {
                            if (activePath == null)
                                activePath = FrostMageSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.MageArcane:
                        {
                            if (activePath == null)
                                activePath = ArcaneMageSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.MageFire:
                        {
                            if (activePath == null)
                                activePath = FireMageSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.PriestDiscipline:
                        {
                            if (activePath == null)
                                activePath = DiscPriestSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.PriestHoly:
                        {
                            if (activePath == null)
                                activePath = HolyPriestSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.PriestShadow:
                        {
                            if (activePath == null)
                                activePath = ShadowPriestSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.PaladinProtection:
                        {
                            if (activePath == null)
                                activePath = ProtPaladinSettings.Instance.path_name;
                            break;
                        }
                    case WoWSpec.PaladinRetribution:
                        {
                            if (activePath == null)
                                activePath = RetriPaladinSettings.Instance.path_name;
                            break;
                        }
                    default:
                        {
                            activePath = null;
                            break;
                        }
                }
                if (activePath == null)
                {
                    utils.LogActivity("No Setting path for this spec please stay tuned for updates");
                    TreeRoot.Stop();
                }

                return activePath;
            }
        }

        public override void Combat()
        {
            if (ActiveKingWoWClass.Combat) return;
        }

        public override bool WantButton
        {
            get
            {
                return true;
            }
        }

        public override void OnButtonPress()
        {
            if (Settings_Form == null || Settings_Form.IsDisposed || Settings_Form.Disposing)
            {
                Settings_Form = new KingHealSettingsGUIFORM(ActiveSettings, ActivePath, ExtraUtilsSettings.Instance.path_name);
            }
            if (Settings_Form != null)
            {
                try
                {
                    Settings_Form.ShowDialog();
                }
                catch (Exception )
                {
                    Logging.Write("Exception Thrown (Calling Settings_Form): ");
                }
            }
        }

        public override void Pulse()
        {
            if (ActiveKingWoWClass.Pulse) return;
        }

        public override void Initialize()
        {
            if (ActiveKingWoWClass.Initialize) return;
        }

        public override bool NeedRest
        {
            get
            {
                return ActiveKingWoWClass.NeedRest;
            }
        }

        public override bool NeedCombatBuffs
        {
            get
            {
                return ActiveKingWoWClass.NeedCombatBuffs;
            }
        }

        public override bool NeedPreCombatBuffs
        {
            get
            {
                return ActiveKingWoWClass.NeedPreCombatBuffs;
            }
        }

        public override bool NeedPullBuffs
        {
            get
            {
                return ActiveKingWoWClass.NeedPullBuffs;
            }
        }

        public override void Pull()
        {
            if(ActiveKingWoWClass.Pull) return;
        }

    }
}
