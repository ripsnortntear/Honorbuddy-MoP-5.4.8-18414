#region Copyright © 2014 Miracle Business Solutions
// All rights are reserved. Reproduction or transmission in whole or in part,
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Document:	Loader\Leaves.cs
// Creation Date:	09/12/2013
// Last Edit:		10/12/2013
// Author:  Mirabis
#endregion

using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Net;
using JetBrains.Annotations;
using System.ComponentModel;
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
    public class GladiatorSuite : CombatRoutine
    {
        #region Tidy : Properties
        public void SlimCheck()
        {
            string SlimDll = Path.Combine(Utilities.AssemblyDirectory, "SlimDX.dll");
            string SlimXml = Path.Combine(Utilities.AssemblyDirectory, "SlimDX.xml");

            if (!File.Exists(SlimDll) || !File.Exists(SlimXml))
            {
                MessageBox.Show("Downloading missing DLL's for Gladiator Suite");

                WebClient client = new WebClient();

                try
                {
                    if (!File.Exists(SlimDll))
                        client.DownloadFile(new Uri(@"http://gladiatorsuite.com/SlimDX/SlimDX.dll"), SlimDll);

                    if (!File.Exists(SlimXml))
                        client.DownloadFile(new Uri(@"http://gladiatorsuite.com/SlimDX/SlimDX.xml"), SlimXml);
                }
                catch (WebException e)
                {
                    MessageBox.Show("Error downloading missing dlls.\nMessage: " + e.Message + "\n\nResponse: " + e.Response);
                    return;
                }
                MessageBox.Show("Download complete");
            }
        }
        private static readonly string SDir = Path.Combine(Utilities.AssemblyDirectory,
            @"Routines\GladiatorSuite\Warrior");

        private readonly string _dllPath = SDir + @"\GSWarrior.dll";

        private readonly String[] _keep =
        {
            "GSLoader.cs",
            "GSWarrior.dll",
            "Settings",
            "changelog.txt"
        };

        private readonly bool _loaded;
        private CombatRoutine _cc;
        private Assembly _ccAssembly;

        #endregion

        #region Tidy : Helpers


        public GladiatorSuite()
        {
            //Check Installation
            if (!ValidateInstallation())
            {
                _loaded = false;
                MessageBox.Show(
                    "GladiatorSuite.ValidateInstallation Failed");
            }
            else
            {
                Cleanup();
                SlimCheck();
                ConfigureHonorbuddy();
                if (LoadRoutine())
                    _loaded = true;
                else
                {
                    _loaded = false;
                    MessageBox.Show(
                        "GladiatorSuite.LoadRoutine Failed");
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
                    "Dll not found",
                    "GladiatorSuite is not installed correctly.\n" +
                    "Make sure your folders look like this (spelling matters):\n" +
                    @"\honorbuddy\Routines\GladiatorSuite\Warrior\n" +
                    "The Warrior folder has to contain two files called GSLoader.cs and GSWarrior.dll",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at GladiatorSuite.ValidateInstallation : {0}", ex);
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
                    Logging.Write("Removing " + file.Name + " from GladiatorSuite directory");
                    file.Delete();
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at GladiatorSuite.Cleanup : {0}", ex);
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
                        var requestedName = new AssemblyName(e.Name);
                        switch (requestedName.Name)
                        {
                            case "Honorbuddy": //Auto Rebuild on new Honorbuddy.exe
                                return Assembly.LoadFile(Process.GetCurrentProcess().MainModule.FileName);
                            case "GreyMagic": //Auto Rebuild on new GreyMagic.dll
                                return Assembly.LoadFile(Utilities.AssemblyDirectory + @"\GreyMagic.dll");
                            case "Tripper.Tools": //Auto Rebuild on new Tripper.Tools.dll
                                return Assembly.LoadFile(Utilities.AssemblyDirectory + @"\Tripper.Tools.dll");
                        }
                        return null;
                    }
                    catch //(System.Exception)
                    {
                        return null;
                    }
                };
                if (_ccAssembly == null)
                {
                    //Assembly Magic
                    byte[] ccBuffer = File.ReadAllBytes(_dllPath);
                    _ccAssembly = Assembly.Load(ccBuffer);
                }
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at GladiatorSuite.ConfigureHonorbuddy : {0}", ex);
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
                _cc =
                    (CombatRoutine)
                        _ccAssembly.CreateInstance(
                            @"GSWarrior.Root");
                return _cc != null;
            }
            catch (Exception ex)
            {
                Logging.Write("Exception thrown at GladiatorSuite.LoadRoutine : {0}", ex);
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
            get { return _loaded ? _cc.Name : "Unknown"; }
        }

        /// <summary>
        ///     The <see cref="WoWClass" /> to be used with this routine
        /// </summary>
        public override WoWClass Class
        {
            get { return WoWClass.Warrior; }
        }

        //<summary>
        //    Behavior used when in combat
        //</summary>
        public override Composite CombatBehavior
        {
            get { return _loaded ? _cc.CombatBehavior : base.CombatBehavior; }
        }

        /// <summary>
        ///     Behavior used for buffing, regular buffs like 'Power Word: Fortitude', 'MotW' etc..
        /// </summary>
        public override Composite PreCombatBuffBehavior
        {
            get { return _loaded ? _cc.PreCombatBuffBehavior : base.PreCombatBuffBehavior; }
        }

        /// <summary>
        ///     Whether this CC want the button on the form to be enabled.
        /// </summary>
        public override bool WantButton
        {
            get { return _loaded ? _cc.WantButton : base.WantButton; }
        }

        /// <summary>
        ///     Called when this CC is selected as the current CC.
        /// </summary>
        public override void Initialize()
        {
            _cc.Initialize();
        }

        /// <summary>
        ///     Called when the button for this CC is pressed.
        /// </summary>
        public override void OnButtonPress()
        {
            if (_loaded)
                _cc.OnButtonPress();
            else base.OnButtonPress();
        }

        /// <summary>
        ///     Called in every pulse of the bot. This way you can maintain stuff per-pulse like a plugin.
        /// </summary>
        public override void Pulse()
        {
            if (_loaded)
                _cc.Pulse();
            else base.Pulse();
        }

        /// <summary>
        ///     Called when the CC is being disposed.
        /// </summary>
        public override void ShutDown()
        {
            if (_loaded)
                _cc.ShutDown();
            else base.ShutDown();
        }

        #endregion
    }
}