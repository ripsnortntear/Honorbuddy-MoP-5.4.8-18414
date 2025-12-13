#region Revision info

/*
 * $Author: wulf_ $
 * $Date: 2013-09-14 10:59:10 +1000 (Sat, 14 Sep 2013) $
 * $ID$
 * $Revision: 201 $
 * $URL: https://subversion.assembla.com/svn/oracle/trunk/Oracle/Healing/Chronicle/ChronicleSpell.cs $
 * $LastChangedBy: wulf_ $
 * $ChangesMade$
 */

#endregion Revision info

using System;
using System.Globalization;

namespace Oracle.Healing.Chronicle
{
    public enum ChronicleSpellType
    {
        None,
        PeriodicHeal,
        DirectHeal,
        HybridHeal,
        PeriodicDamage,
        DirectDamage,
        HybridDamage
    }

    public enum TickType
    {
        None,
        Duration,
        Interval,
    }

    public abstract class ChronicleSpell
    {
        #region Fields

        //private Guid _SpellId = Guid.NewGuid();

        #endregion Fields

        // Base stats
        public int SpellId { get; set; }

        public string SpellName { get; set; }

        public string SpellNameOverload { get; set; } // Required to casT things like "Holy Word: Serenity"...you have to use "Holy Word: Chastise" to cast it in HB :/

        public ChronicleSpellType SpellType { get; set; }

        public int BaseManaCost { get; set; }

        public float BaseCoefficient { get; set; }

        public float Cooldown { get; set; }

        public float BaseHeal { get; set; }

        public float BaseCastTime { get; set; }

        public bool Instant { get; set; }

        public bool HasCooldown { get; set; }

        public bool CanCrit { get; set; }

        // Network
        public float Latency { get; set; }

        public float GcdLatency { get; set; }

        // Modifiers
        public float CooldownReduction { get; set; }

        public float ManaCostScale { get; set; }

        public float HealModifier { get; set; }

        public float AOEHealModifier { get; set; } // used for Holy Radiance 50% of the amount healed.

        public float ChargeModifier { get; set; } // used for Paladin Holy Power to multiply the heal by the charges.

        public float BonusSpellPower { get; set; }

        public float HasteModifier { get; set; }

        public float PostCalculationModifier { get; set; }

        // Scaling
        public float FriendlyCount { get; set; }

        // Incremental stats
        public float SpellPower { get; set; }

        // Calculation Results
        public float CastTime { get; protected set; }

        public int ManaCost { get; protected set; }

        public int ManaBack { get; protected set; }

        public float CalculatedHeal { get; protected internal set; }

        public virtual float TotalPeriodicHeal { get; protected internal set; }

        // Derived stats
        public float EffectiveCritModifier { get; set; }

        public ChronicleSpell()
        {
            Instant = false;
            HasCooldown = false;
            CooldownReduction = 0f;
            ManaCostScale = 1f;
            HealModifier = 1f;
            BonusSpellPower = 0f;
            HasteModifier = 1f;
            EffectiveCritModifier = 1f;
            SpellNameOverload = "Ignore";
            PostCalculationModifier = 1f;
            Latency = 0f;
            GcdLatency = 0f;
            CastTime = 0;
            FriendlyCount = 1f;
            AOEHealModifier = 1f;
            ChargeModifier = 1f;
            SpellPower = 0f;
            CanCrit = false; // off for now..may implement better in future..
            ManaBack = 0;
        }

        public void Calculate()
        {
            ManaCost = (int)Math.Round(BaseManaCost * ManaCostScale, 0);

            CalculateCastTime();
            CalculateHeal();
        }

        private void CalculateCastTime()
        {
            if (Instant)
                CastTime = (float)(1.5 * HasteModifier);
            else
                CastTime = BaseCastTime * HasteModifier;
        }

        public virtual float MPS
        {
            get { return (ManaCost - ManaBack) / Math.Max(CastTime, 1); }
        }

        public virtual float HPS
        {
            get { return TotalCalculatedHeal / Math.Max(CastTime, 1); }
        }

        public virtual float HPM
        {
            get { return TotalCalculatedHeal / ManaCost; }
        }

        public virtual float AOEheal
        {
            get { return ((CalculatedHeal * AOEHealModifier) * ChargeModifier) * FriendlyCount; }
        }

        internal virtual void CalculateHeal()
        {
            CalculatedHeal = 0f;
            if (BaseHeal <= 1f)
                return;

            float spellPower = GetSpellPower();
            float nonCrit = (BaseHeal + spellPower) * HealModifier;
            if (!CanCrit)
            {
                CalculatedHeal = nonCrit * ChargeModifier;
                return;
            }
            CalculatedHeal = EffectiveCritModifier * nonCrit;
        }

        private float _totalCalculatedHeal;

        internal float TotalCalculatedHeal
        {
            get
            {
                switch (SpellType)
                {
                    case ChronicleSpellType.PeriodicDamage:
                    case ChronicleSpellType.PeriodicHeal:
                        _totalCalculatedHeal = TotalPeriodicHeal * PostCalculationModifier;
                        break;

                    case ChronicleSpellType.DirectDamage:
                    case ChronicleSpellType.DirectHeal:
                        _totalCalculatedHeal = CalculatedHeal * PostCalculationModifier;
                        break;

                    case ChronicleSpellType.HybridDamage:
                    case ChronicleSpellType.HybridHeal:
                        _totalCalculatedHeal = (TotalPeriodicHeal + CalculatedHeal) * PostCalculationModifier;
                        break;
                }
                return _totalCalculatedHeal;
            }
            set { _totalCalculatedHeal = value; }
        }

        internal float? TotalCombinedHeal { get; set; } // TotalPeriodicHeal + CalculatedHeal

        protected virtual float GetSpellPower()
        {
            return (SpellPower + BonusSpellPower) * (BaseCoefficient);
        }

        public static bool IsDamageHeal(ChronicleSpellType st)
        {
            return st == ChronicleSpellType.DirectDamage || st == ChronicleSpellType.HybridDamage || st == ChronicleSpellType.PeriodicDamage;
        }

        public static bool IsPeriodicHeal(ChronicleSpellType st)
        {
            return st == ChronicleSpellType.HybridDamage || st == ChronicleSpellType.PeriodicDamage || st == ChronicleSpellType.HybridHeal || st == ChronicleSpellType.PeriodicHeal;
        }

        public override string ToString()
        {
            string retval = string.Empty;

            retval += String.Format("\n-------[ Spell Info ] ------------\n");
            retval += String.Format("\nSpellId: {0}", SpellId.ToString("0"));
            retval += String.Format("\nSpellName: {0}", SpellName);
            retval += String.Format("\nSpellType: {0}", SpellType.ToString());

            if (BaseCoefficient > 0)
                retval += String.Format("\nBaseCoefficient: {0}", BaseCoefficient.ToString("0.0%"));

            if (BaseCastTime > 0)
                retval += String.Format("\nBaseCastTime: {0}", BaseCastTime.ToString("0.00"));

            if (HasteModifier > 0)
                retval += String.Format("\nCastTimeReduction: {0}", HasteModifier.ToString("0.00"));

            if (CastTime > 0)
                retval += String.Format("\nCast Time: {0} seconds", (Instant) ? String.Format("Instant {0}", 1.5) : CastTime.ToString("0.00"));

            retval += String.Format("\nInstant: {0}", Instant.ToString());
            retval += String.Format("\nHasCooldown: {0}", HasCooldown.ToString());
            retval += String.Format("\nCanCrit: {0}", CanCrit.ToString());

            if (CooldownReduction > 0)
                retval += String.Format("\nCooldownReduction: {0}", CooldownReduction.ToString("0.00"));

            if (BonusSpellPower > 0)
                retval += String.Format("\nBonusSpellPower: {0}", BonusSpellPower.ToString("0.00"));

            retval += String.Format("\n\n-------[ Network Info ] ------------\n");

            if (Latency > 0)
                retval += String.Format("\nLatency: {0}", Latency.ToString("0.00"));

            if (GcdLatency > 0)
                retval += String.Format("\nGcdLatency: {0}", GcdLatency.ToString("0.00"));

            retval += String.Format("\n\n-------[ Mana Info] ------------\n");

            if (BaseManaCost > 0)
                retval += String.Format("\nBaseManaCost: {0}", BaseManaCost.ToString("0.00"));

            if (ManaCost > 0)
                retval += String.Format("\nMana Cost: {0}", ManaCost.ToString("0"));

            if (ManaBack > 0)
                retval += String.Format("\nMana Back: {0}", ManaBack.ToString("0"));

            if (MPS > 0)
                retval += String.Format("\nMana Per Second: {0}", MPS.ToString("0"));

            if (ManaCostScale > 0)
                retval += String.Format("\nManaCostScale: {0}", ManaCostScale.ToString("0.0"));

            retval += String.Format("\n\n-------[ Direct heal Info] ------------\n");

            if (HPS > 0)
                retval += String.Format("\nHeals Per Second: {0}", HPS.ToString("0"));

            if (HPM > 0)
                retval += String.Format("\nHeals Per Mana: {0}", HPM.ToString("0"));

            if (EffectiveCritModifier > 0)
                retval += String.Format("\nEffectiveCritModifier: {0}", EffectiveCritModifier.ToString("0.00"));

            if (HealModifier > 0)
                retval += String.Format("\nHealModifier: {0}", HealModifier.ToString("0.00"));

            if (BaseHeal > 0)
                retval += String.Format("\nBaseHeal: {0}", BaseHeal.ToString("0.0"));

            if (GetSpellPower() > 0)
                retval += String.Format("\nSpell Power Contribution to base heal: {0}", GetSpellPower().ToString(CultureInfo.InvariantCulture));

            if (CalculatedHeal > 0)
                retval += String.Format("\nDirect Heal: {0}", CalculatedHeal.ToString(CultureInfo.InvariantCulture));

            if (FriendlyCount > 1)
                retval += String.Format("\nTotal Heal for {1} friendly's: {0}", AOEheal.ToString(CultureInfo.InvariantCulture), FriendlyCount);

            if (SpellType == ChronicleSpellType.DirectHeal || SpellType == ChronicleSpellType.DirectDamage)
                retval += ToStringDirectHeal();

            retval += String.Format("\n\n-------[ Periodic heal Info] ------------\n");

            if (SpellType == ChronicleSpellType.PeriodicHeal || SpellType == ChronicleSpellType.HybridHeal || SpellType == ChronicleSpellType.PeriodicDamage || SpellType == ChronicleSpellType.HybridDamage)
                retval += ToStringPeriodicHeal();

            retval += String.Format("\n\n-------[ TOTAL HEAL ] ------------\n");

            if (TotalCalculatedHeal > 1)
                retval += String.Format("\nTotal Heal: {0}", TotalCalculatedHeal.ToString(CultureInfo.InvariantCulture));

            if (TotalCombinedHeal > 1)
                retval += String.Format("\n TotalCombinedHeal Heal: {0}", TotalCombinedHeal);

            retval += String.Format("\n\n-------[ Player Info] ------------\n");
            if (SpellPower > 0)
                retval += String.Format("\nCalculated based on player's SpellPower: {0}", SpellPower.ToString("0"));

            return retval;
        }

        protected virtual string ToStringDirectHeal()
        {
            return String.Empty;
        }

        protected virtual string ToStringPeriodicHeal()
        {
            return String.Empty;
        }
    }

    // Such as Healing Touch...
    public class DirectHeal : ChronicleSpell
    {
    }

    // Such as LifeBloom...
    public class Periodicheal : ChronicleSpell
    {
        public Periodicheal()
        {
            DurationModifer = 0f;
            FriendlyCount = 1f;
        }

        public TickType TickType { get; set; }

        // Base stats
        public float BasePeriodicDuration { get; set; }

        public float BasePeriodicTickFrequency { get; set; }

        public float BasePeriodicCoefficient { get; set; }

        public float BasePeriodicHeal { get; set; }

        // Modifiers
        public float DurationModifer { get; set; }

        // calculation results
        public float CalculatedPeriodicHeal { get; protected internal set; }

        // Derived stats
        public float PeriodicDuration
        {
            get { return BasePeriodicDuration + DurationModifer; }
        }

        public override float TotalPeriodicHeal
        {
            get
            {
                if (TickType == TickType.Duration)
                    return CalculatedPeriodicHeal;

                return CalculatedPeriodicHeal * BaseTickCount;
            }
        }

        public float HealPerTick
        {
            get
            {
                if (TickType == TickType.Duration)
                    return CalculatedPeriodicHeal / BaseTickCount;

                return CalculatedPeriodicHeal;
            }
        }

        public float BaseTickCount
        {
            get { return BasePeriodicDuration / BasePeriodicTickFrequency; }
        }

        //public override float MPS
        //{
        //    get
        //    {
        //        if (Instant)
        //            return (ManaCost / (PeriodicDuration));
        //        else
        //            return (ManaCost / (PeriodicDuration + CastTime));
        //    }
        //}

        public override float AOEheal
        {
            get
            {
                return TotalPeriodicHeal * FriendlyCount;
            }
        }

        internal override void CalculateHeal()
        {
            base.CalculateHeal();

            float spellPower = GetSpellPower();
            float nonCrit = (BasePeriodicHeal + spellPower) * HealModifier;

            if (!CanCrit)
            {
                CalculatedPeriodicHeal = nonCrit * ChargeModifier;
                return;
            }
            CalculatedPeriodicHeal = EffectiveCritModifier * nonCrit;
        }

        private new float GetSpellPower()
        {
            return (SpellPower + BonusSpellPower) * (BasePeriodicCoefficient);
        }

        protected override string ToStringPeriodicHeal()
        {
            string s = base.ToStringPeriodicHeal();
            if (CalculatedPeriodicHeal > 0)
            {
                s += String.Format("\nCalculated Periodic Heal: {0}", CalculatedPeriodicHeal.ToString("0"));
                s += String.Format("\nTotal Periodic Heal : {0}", TotalPeriodicHeal.ToString("0"));
                s += String.Format("\nHeal Per Tick: {0}", HealPerTick.ToString("0.00"));
                if (FriendlyCount > 1)
                    s += String.Format("\nTotal Heal for {1} friendly's: {0}", AOEheal.ToString(CultureInfo.InvariantCulture), FriendlyCount);
                s += String.Format("\nPeriodic Heal ticks a total of {0} times, once every {1} seconds, for a duration of {2} seconds", BaseTickCount.ToString("0"), BasePeriodicTickFrequency.ToString("0.00"), BasePeriodicDuration.ToString("0.00"));
            }
            return s;
        }
    }
}