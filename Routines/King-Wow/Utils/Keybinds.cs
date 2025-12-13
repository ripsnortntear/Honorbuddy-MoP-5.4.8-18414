using Styx.WoWInternals;
using Styx.Common;

namespace KingWoW
{
    public static class Keybinds
    {
        // Sounds from http://www2.research.att.com/~ttsweb/tts/demo.php

        internal static void Pulse()
        {
            // KeybindPauseRotation \\
            /*if (IsKeyDown(ExtraUtilsSettings.Instance.PauseButton))
            {
                switch (ExtraUtilsSettings.Instance.PauseRotation)
                {
                    case true:
                        ExtraUtilsSettings.Instance.PauseRotation = !ExtraUtilsSettings.Instance.PauseRotation;
                        Lua.DoString(@"print('Rotation \124cFF15E61C Resumed!')");
                        break;

                    case false:
                        ExtraUtilsSettings.Instance.PauseRotation = !ExtraUtilsSettings.Instance.PauseRotation;
                        Lua.DoString(@"print('Rotation \124cFFE61515 Paused!')");
                        break;

                    default:
                        return;
                }
            }*/
            if (IsKeyDown(ExtraUtilsSettings.Instance.AOE_HealingButton))
            {
                switch (ExtraUtilsSettings.Instance.DisableAOE_HealingRotation)
                {
                    case true:
                        ExtraUtilsSettings.Instance.DisableAOE_HealingRotation = !ExtraUtilsSettings.Instance.DisableAOE_HealingRotation;
                        Lua.DoString(@"print('AOE_Healing \124cFF15E61C Resumed!')");
                        break;

                    case false:
                        ExtraUtilsSettings.Instance.DisableAOE_HealingRotation = !ExtraUtilsSettings.Instance.DisableAOE_HealingRotation;
                        Lua.DoString(@"print('AOE_Healing \124cFFE61515 Paused!')");
                        break;

                    default:
                        return;
                }
            }
            else if (IsKeyDown(ExtraUtilsSettings.Instance.AtonementNormalHealingSwitchButton))
            {
                switch (ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation)
                {
                    case true:
                        ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation = !ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation;
                        Lua.DoString(@"print('Atonement healing \124cFFE61515 DISABLED!')");
                        break;

                    case false:
                        ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation = !ExtraUtilsSettings.Instance.UseDisciplineAtonementHealingRotation;
                        Lua.DoString(@"print('Atonement Healing \124cFF15E61C ENABLED!')");
                        break;

                    default:
                        return;
                }
            }

        }

        /// <summary>
        /// checks to see if the specified key has been pressed within wow.
        /// </summary>
        /// <param name="key">The key to check for (see Keyboardfunctions)</param>
        /// <returns>true if the player has pressed the key</returns>
        private static bool IsKeyDown(ExtraUtilsSettings.Keyboardfunctions key)
        {
            try
            {
                if (key == ExtraUtilsSettings.Keyboardfunctions.Nothing) return false;
                var raw = Lua.GetReturnValues("if " + key.ToString("g") + "() then return 1 else return 0 end");
                return raw[0] == "1";
            }
            catch
            {
                return false;
            }
        }
    }
}