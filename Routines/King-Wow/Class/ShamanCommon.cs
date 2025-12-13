#region Revision Info

// This file is part of Singular - A community driven Honorbuddy CC
// $Author$
// $Date$
// $HeadURL$
// $LastChangedBy$
// $LastChangedDate$
// $LastChangedRevision$
// $Revision$

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using Styx;
using Styx.CommonBot;
using Styx.WoWInternals;
using Styx.WoWInternals.WoWObjects;

using CommonBehaviors.Actions;

using Styx.TreeSharp;
using Action = Styx.TreeSharp.Action;
using Styx.Common;

namespace KingWoW
{
    public enum Imbue
    {
        None = 0,

        Flametongue = 5,
        Windfury = 283,
        Earthliving = 3345,
        Frostbrand = 2,
        Rockbiter = 3021
    }  

    public class ShamanCommon
    {
        private static LocalPlayer Me { get { return StyxWoW.Me; } }

        KingWoWUtility utils = null;

        public ShamanCommon()
        {
            utils = new KingWoWUtility();
        }

        #region IMBUE

        public string ToSpellName(Imbue i)
        {
            return i.ToString() + " Weapon";
        }

        public bool ImbueMainHand(Imbue ImbueSpellId)
        {
            if ((Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == (int)ImbueSpellId))
                return false;
            if (CanImbue(Me.Inventory.Equipped.MainHand) && SpellManager.HasSpell(ToSpellName(ImbueSpellId)))
            {               
                    if (Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != (int)ImbueSpellId &&
                        utils.CanCast(ToSpellName(ImbueSpellId))
                        && (Imbue)Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id != Imbue.None)
                    {
                        Lua.DoString("CancelItemTempEnchantment(1)");
                    }
                    else if ((Imbue)Me.Inventory.Equipped.MainHand.TemporaryEnchantment.Id == Imbue.None
                        && utils.CanCast(ToSpellName(ImbueSpellId)))
                    {
                        SpellManager.Cast((ToSpellName(ImbueSpellId)));
                        SetNextAllowedImbueTime();
                    }
            }

            return false;
        }


        public bool ImbueOffHand(Imbue ImbueSpellId)
        {
            if ((Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == (int)ImbueSpellId))
                return false;
            if (CanImbue(Me.Inventory.Equipped.OffHand) && SpellManager.HasSpell(ToSpellName(ImbueSpellId)))
            {
                    if (Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != (int)ImbueSpellId &&
                        utils.CanCast(ToSpellName(ImbueSpellId))
                        && (Imbue)Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id != Imbue.None)
                    {
                        Lua.DoString("CancelItemTempEnchantment(2)");
                    }
                    else if ((Imbue)Me.Inventory.Equipped.OffHand.TemporaryEnchantment.Id == Imbue.None
                        && utils.CanCast(ToSpellName(ImbueSpellId)))
                    {
                        utils.Cast((ToSpellName(ImbueSpellId)));
                        SetNextAllowedImbueTime();
                    }
            }
            return false;
        }

        // imbues are sometimes slow to appear on client... need to allow time
        // .. for buff to appear, otherwise will get in an imbue spam loop

        private DateTime nextImbueAllowed = DateTime.Now;

        public bool CanImbue(WoWItem item)
        {
            if (item != null && item.ItemInfo.IsWeapon)
            {
                // during combat, only mess with imbues if they are missing
                if (Me.Combat && item.TemporaryEnchantment.Id != 0)
                    return false;

                // check if enough time has passed since last imbue
                // .. guards against detecting is missing immediately after a cast but before buff appears
                // .. (which results in imbue cast spam)
                if (nextImbueAllowed > DateTime.Now)
                    return false;

                switch (item.ItemInfo.WeaponClass)
                {
                    case WoWItemWeaponClass.Axe:
                        return true;
                    case WoWItemWeaponClass.AxeTwoHand:
                        return true;
                    case WoWItemWeaponClass.Dagger:
                        return true;
                    case WoWItemWeaponClass.Fist:
                        return true;
                    case WoWItemWeaponClass.Mace:
                        return true;
                    case WoWItemWeaponClass.MaceTwoHand:
                        return true;
                    case WoWItemWeaponClass.Polearm:
                        return true;
                    case WoWItemWeaponClass.Staff:
                        return true;
                    case WoWItemWeaponClass.Sword:
                        return true;
                    case WoWItemWeaponClass.SwordTwoHand:
                        return true;
                }
            }

            return false;
        }

        public void SetNextAllowedImbueTime()
        {
            // 2 seconds to allow for 1 seconds
            nextImbueAllowed = DateTime.Now + new TimeSpan(0, 0, 0, 0, 1000); 
        }

        #endregion

        #region Totem Existance

        public bool Exist(WoWTotem ti)
        {
            return ti != WoWTotem.None
                && ti != WoWTotem.DummyAir
                && ti != WoWTotem.DummyEarth
                && ti != WoWTotem.DummyFire
                && ti != WoWTotem.DummyWater;
        }

        public bool Exist(WoWTotemInfo ti)
        {
            return Exist(ti.WoWTotem);
        }

        public bool Exist(WoWTotemType type)
        {
            WoWTotem wt = GetTotem(type).WoWTotem;
            return Exist(wt);
        }

        public bool Exist(params WoWTotem[] wt)
        {
            return wt.Any(t => Exist(t));
        }

        public bool ExistInRange(WoWPoint pt, WoWTotem tt)
        {
            if (!Exist(tt))
                return false;

            WoWTotemInfo ti = GetTotem(tt);
            return ti.Unit != null && ti.Unit.Location.Distance(pt) < GetTotemRange(tt);
        }

        public bool ExistInRange(WoWPoint pt, params WoWTotem[] awt)
        {
            return awt.Any(t => ExistInRange(pt, t));
        }

        public bool ExistInRange(WoWPoint pt, WoWTotemType type)
        {
            WoWTotemInfo ti = GetTotem(type);
            return Exist(ti) && ti.Unit != null && ti.Unit.Location.Distance(pt) < GetTotemRange(ti.WoWTotem);
        }

        #endregion

        #region TOTEM

        public bool NeedToRecallTotems()
        {           
            return TotemsInRange == 0
                && StyxWoW.Me.Totems.Count(t => t.Unit != null) != 0
                && !utils.NearbyFriendlyPlayersInRange(40).Any(f => f.Combat)
                && !StyxWoW.Me.Totems.Any(t => t.WoWTotem == WoWTotem.FireElemental || t.WoWTotem == WoWTotem.EarthElemental);

        }

        public void DestroyTotem(WoWTotemType type)
        {
            if (type == WoWTotemType.None)
            {
                return;
            }

            Lua.DoString("DestroyTotem({0})", (int)type);
        }

        public void RecallTotems()
        {
            Logging.Write("Recalling totems!");
            if (SpellManager.HasSpell("Totemic Recall"))
            {
                SpellManager.Cast("Totemic Recall");
                return;
            }

            List<WoWTotemInfo> totems = StyxWoW.Me.Totems;
            foreach (WoWTotemInfo t in totems)
            {
                if (t != null && t.Unit != null)
                {
                    DestroyTotem(t.Type);
                }
            }
        }

        public int ToSpellId(WoWTotem totem)
        {
            return (int)(((long)totem) & ((1 << 32) - 1));
        }

        public bool TotemIsKnown(WoWTotem totem)
        {
            return SpellManager.HasSpell(ToSpellId(totem));
        }

        public WoWTotemType ToType(WoWTotem totem)
        {
            return (WoWTotemType)((long)totem >> 32);
        }

        public WoWTotemInfo GetTotem(WoWTotem wt)
        {
            return GetTotem(ToType(wt));
        }

        public WoWTotemInfo GetTotem(WoWTotemType type)
        {
            return Me.Totems[(int)type - 1];
        }

        public int TotemsInRange
        {
            get
            {
                return TotemsInRangeOf(StyxWoW.Me);
            }
        }

        public int TotemsInRangeOf(WoWUnit unit)
        {
            return StyxWoW.Me.Totems.Where(t => t.Unit != null).Count(t => unit.Location.Distance2DSqr(t.Unit.Location) < (GetTotemRange(t.WoWTotem)*GetTotemRange(t.WoWTotem)));
        }

        public float GetTotemRange(WoWTotem totem)
        {
            switch (totem)
            {
                case WoWTotem.HealingStream:
                case WoWTotem.Tremor:
                    return 30f;

                case WoWTotem.Searing:
                    if (SpellManager.HasSpell(29000))
                        return 35f;
                    return 20f;

                case WoWTotem.Earthbind:
                    return 10f;

                case WoWTotem.Grounding:
                case WoWTotem.Magma:
                    return 8f;

                case WoWTotem.EarthElemental:
                case WoWTotem.FireElemental:
                    // Not really sure about these 3.
                    return 20f;

                case WoWTotem.ManaTide:
                    // Again... not sure :S
                    return 40f;


                case WoWTotem.Earthgrab:
                    return 10f;

                case WoWTotem.StoneBulwark:
                    // No idea, unlike former glyphed stoneclaw it has a 5 sec pluse shield component so range is more important
                    return 40f;

                case WoWTotem.HealingTide:
                    return 40f;

                case WoWTotem.Capacitor:
                    return 8f;

                case WoWTotem.Stormlash:
                    return 30f;

                case WoWTotem.Windwalk:
                    return 40f;

                case WoWTotem.SpiritLink:
                    return 10f;
            }

            return 0f;
        }
        
        #endregion

    }
}