#region Copyright © 2013 Miracle Business Solutions

// All rights are reserved. Reproduction or transmission in whole or in part,
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename: Developer Leaves/Loader/Leaves.cs
// Date:     20/09/2013 at 11:33 AM
// Author:   Moreno Sint Hill

#endregion

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using JetBrains.Annotations;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;

namespace Loader
{
    /// <summary>
    ///   Credits to Mastahg & Inrego for information
    /// </summary>
    [UsedImplicitly]
    public class Leaves : CombatRoutine
    {
        #region Tidy : Properties

        private static readonly string SDir = Path.Combine(Utilities.AssemblyDirectory, "Routines\\Leaves");
        private readonly string _dllPath = SDir + @"\Leaves.dll";

        private readonly String[] _keep =
        {
            "Leaves.cs", "Leaves.dll", "Settings","Changelog.xml",
            "Scheduled.txt"
        };

        private readonly bool _loaded;
        private CombatRoutine _cc;
        private Assembly _ccAssembly;

        #endregion

        #region Tidy : Helpers

        /// <summary>
        ///     Leaves Instance
        /// </summary>
        public Leaves()
        {
            //Check Installation
            if (!ValidateInstallation())
            {
                _loaded = false;
                MessageBox.Show(
                    "Leaves could not validate its installation. Ensure Routines\\Leaves\\Leaves.dll and Leaves.cs are present.",
                    "Leaves installation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Cleanup();
                ConfigureHonorbuddy();
                if (LoadRoutine())
                    _loaded = true;
                else
                {
                    _loaded = false;
                    MessageBox.Show(
                        "Leaves failed to load Leaves.dll. Check the Honorbuddy log for details.",
                        "Leaves load failure", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        /// <summary>
        ///     Checks if the routine is installed right
        /// </summary>
        /// <returns></returns>
        private bool ValidateInstallation()
        {
            try
            {
                if (File.Exists(_dllPath)) return true;
                MessageBox.Show(
                    "Leaves.dll was not found in Routines\\Leaves\\. Verify the routine files are installed correctly.",
                    "Leaves is not installed correctly", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at Leaves.ValidateInstallation : {0}", ex);
                return false;
            }
        }

        /// <summary>
        ///     Checks wheter the files in the directory are right
        /// </summary>
        /// <returns></returns>
        private void Cleanup()
        {
            try
            {
                var dInfo = new DirectoryInfo(SDir);
                for (int index = 0; index < dInfo.GetFiles().Length; index++)
                {
                    FileInfo file = dInfo.GetFiles()[index];
                    if (_keep.Contains(file.Name)) continue;
                    Logging.Write("Removing " + file.Name + " from Leaves directory");
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at Leaves.Cleanup : {0}", ex);
            }
        }

        /// <summary>
        /// Makes the routine universal by configuring the bot, credits to mastahg
        /// </summary>
        /// <returns></returns>
        private void ConfigureHonorbuddy()
        {
            try
            {
                //Set the current thread to en-Us
                //Then create a byte array to store the contents of our dll file
                //Then populate our dummy assembly , by loading it into the callers domain
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                {
                    try
                    {
                        AssemblyName requestedName = new AssemblyName(e.Name);
                        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
                        {
                            if (string.Equals(a.GetName().Name, requestedName.Name, StringComparison.OrdinalIgnoreCase))
                            {
                                return a;
                            }
                        }
                        return null;
                    }
                    catch
                    {
                        return null;
                    }
                };
                if (_ccAssembly == null)
                {
                    //Assembly Magic
                    //If it all worked.. i guess it works
                    byte[] ccBuffer = File.ReadAllBytes(_dllPath);
                    _ccAssembly = Assembly.Load(ccBuffer);
                }
              
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at Leaves.ConfigureHonorbuddy : {0}", ex);
            }
        }

        /// <summary>
        ///     Loads the selected routine
        /// </summary>
        /// <returns></returns>
        private bool LoadRoutine()
        {
            try
            {
                _cc = (CombatRoutine) _ccAssembly.CreateInstance("Leaves.Root");
                return _cc != null;
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at Leaves.LoadRoutine : {0}", ex);
                return false;
            }
        }

        #endregion

        #region Tidy : CombatRoutine Overrides

        /// <summary>
        ///     The name of this CombatRoutine
        /// </summary>
        public override string Name
        {
            get { return (_loaded && _cc != null) ? _cc.Name : "Leaves (not loaded)"; }
        }

        /// <summary>
        ///     The <see cref="WoWClass" /> to be used with this routine
        /// </summary>
        public override WoWClass Class
        {
            get { return WoWClass.Druid; }
        }

        public override Composite CombatBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.CombatBehavior : base.CombatBehavior; }
        }

        public override Composite PullBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.PullBehavior : base.PullBehavior; }
        }

        /// <summary>
        ///     Behavior used when healing
        /// </summary>
        public override Composite HealBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.HealBehavior : base.HealBehavior; }
        }

        /// <summary>
        ///     Behavior used for buffing, regular buffs like 'Power Word: Fortitude', 'MotW' etc..
        /// </summary>
        public override Composite PreCombatBuffBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.PreCombatBuffBehavior : base.PreCombatBuffBehavior; }
        }

        public override Composite RestBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.RestBehavior : base.RestBehavior; }
        }

        public override Composite DeathBehavior
        {
            get { return (_loaded && _cc != null) ? _cc.DeathBehavior : base.DeathBehavior; }
        }

        public override bool NeedPreCombatBuffs
        {
            get { return (_loaded && _cc != null) ? _cc.NeedPreCombatBuffs : base.NeedPreCombatBuffs; }
        }

        public override bool NeedRest
        {
            get { return (_loaded && _cc != null) ? _cc.NeedRest : base.NeedRest; }
        }

        /// <summary>
        ///     Whether this CC want the button on the form to be enabled.
        /// </summary>
        public override bool WantButton
        {
            get { return (_loaded && _cc != null) ? _cc.WantButton : base.WantButton; }
        }

        /// <summary>
        ///     Called when this CC is selected as the current CC.
        /// </summary>
        public override void Initialize()
        {
            if (_loaded && _cc != null)
                _cc.Initialize();
            else
                base.Initialize();
        }

        /// <summary>
        ///     Called when the button for this CC is pressed.
        /// </summary>
        public override void OnButtonPress()
        {
            if (_loaded && _cc != null)
                _cc.OnButtonPress();
            base.OnButtonPress();
        }

        /// <summary>
        ///     Called in every pulse of the bot. This way you can maintain stuff per-pulse like a plugin.
        /// </summary>
        public override void Pulse()
        {
            if (_loaded && _cc != null)
                _cc.Pulse();
            base.Pulse();
        }

        /// <summary>
        ///     Called when the CC is being disposed.
        /// </summary>
        public override void ShutDown()
        {
            if (_loaded && _cc != null)
                _cc.ShutDown();
            base.ShutDown();
        }

        #endregion
    }
}