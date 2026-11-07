// Credits to Phelon and Mirabis for making this work without using Assembly redirection.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Media;
using Styx;
using Styx.Common;
using Styx.CommonBot.Routines;
using Styx.TreeSharp;

namespace Loader
{
    public class DemonicLoader : CombatRoutine
    {
        private CombatRoutine CC;

        private readonly String[] Keep = new[] { "DemonicLoader.cs", "Demonic.dll", "Settings", "Changelog.txt", "de-DE", "ru-RU", "zh-Hans" };

        public DemonicLoader()
        {
            string settingsDirectory = Path.Combine(Utilities.AssemblyDirectory, "Routines\\Demonic");

            string path = settingsDirectory + @"\Demonic.dll";

            if (!File.Exists(path))
            {
                MessageBox.Show("Demonic is not installed correctly! Ensure files look like;" +
                            Environment.NewLine +
                            Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/Demonic.dll" + Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/DemonicLoader.cs" + Environment.NewLine +
                            "<Honorbuddy>/Routines/Demonic/Settings/<XML Files>");
                return;
            }

            bool removed = false;
            var DInfo = new DirectoryInfo(settingsDirectory);
            foreach (FileInfo file in DInfo.GetFiles())
            {
                if (!Keep.Contains(file.Name))
                {
                    removed = true;
                    Logging.Write("Removing " + file.Name + " from Demonic directory");
                    file.Delete();
                }
            }
            if (removed)
            {
                MessageBox.Show("Removed unnecessary files from Demonic directory, if Demonic fails to work please restart honorbuddy.");
            }

            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                // Ensure modern TLS protocols are enabled before any network calls in Demonic.dll
                try
                {
                    ServicePointManager.Expect100Continue = true;
                    // Added TLS 1.2, 1.1, and 1.0 support even on older .NET versions where enums may be missing
                    ServicePointManager.SecurityProtocol |= (SecurityProtocolType)3072; // Tls12
                    ServicePointManager.SecurityProtocol |= (SecurityProtocolType)768;  // Tls11
                    ServicePointManager.SecurityProtocol |= (SecurityProtocolType)192;  // Tls
                }
                catch { }

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

                byte[] Bytes = File.ReadAllBytes(path);
                Assembly asm = Assembly.Load(Bytes);

                foreach (Type t in asm.GetTypes())
                {
                    try
                    {
                        if (t.IsSubclassOf(typeof(CombatRoutine)) && !t.IsAbstract && t.IsClass)
                        {
                            object obj = Activator.CreateInstance(t);
                            CC = (CombatRoutine)obj;
                            break;
                        }
                    }
                    catch
                    {
                    }
                }

                UnlockInnerRoutine(asm, CC);
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (ReflectionTypeLoadException ex)
            {
                var sb = new StringBuilder();
                foreach (Exception exSub in ex.LoaderExceptions)
                {
                    if (exSub == null) continue;
                    sb.AppendLine(exSub.Message);
                    var exFileNotFound = exSub as FileNotFoundException;
                    if (exFileNotFound != null && !string.IsNullOrEmpty(exFileNotFound.FusionLog))
                    {
                        sb.AppendLine("Fusion Log:");
                        sb.AppendLine(exFileNotFound.FusionLog);
                    }
                    sb.AppendLine();
                }
                Logging.Write(sb.ToString());
            }
            catch (Exception e)
            {
                Logging.Write(Colors.DarkRed, "An error occured while loading Demonic!");
                Logging.Write(e.ToString());
            }
        }

        private static int _pulseGateCheck;

        private static void UnlockInnerRoutine(Assembly asm, CombatRoutine cc)
        {
            if (asm == null) return;

            try
            {
                // Neutralize defunct auth/UI gate fields found in Demonic.Demonic (public static bool bool_0)
                // and ns1.Debug (checked by inner Initialize/OnButtonPress/Pulse). Releases gated access for local/private use.
                foreach (Type t in asm.GetTypes())
                {
                    try
                    {
                        if (t == null) continue;

                        // Primary routine gate: Demonic.Demonic.bool_0 (auth failed/locked)
                        if (cc != null && t.IsInstanceOfType(cc))
                        {
                            SetStaticField(t, "bool_0", false);
                        }
                        else if (t.IsSubclassOf(typeof(CombatRoutine)) && string.Equals(t.Name, "Demonic", StringComparison.Ordinal))
                        {
                            SetStaticField(t, "bool_0", false);
                        }

                        // Inner debug/auth UI gate: ns1.Debug.string_0="True", string_1="It's Been Checked", bool_0=false, int_0=678
                        if (t.FullName == "ns1.Debug")
                        {
                            SetStaticField(t, "string_0", "True");
                            SetStaticField(t, "string_1", "It's Been Checked");
                            SetStaticField(t, "bool_0", false);
                            SetStaticField(t, "int_0", 678);
                        }
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.Write("DemonicLoader: failed to unlock inner routine gates: " + ex.Message);
            }
        }

        private static void SetStaticField(Type t, string name, object value)
        {
            try
            {
                FieldInfo f = t.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                if (f == null) return;
                if (value == null || f.FieldType.IsAssignableFrom(value.GetType()))
                    f.SetValue(null, value);
            }
            catch
            {
            }
        }

        private static void DismissDefunctAuthDialogs(Assembly asm, CombatRoutine cc)
        {
            try
            {
                UnlockInnerRoutine(asm, cc);
                FormCollection forms = Application.OpenForms;
                if (forms == null) return;

                for (int i = forms.Count - 1; i >= 0; i--)
                {
                    Form f = null;
                    try { f = forms[i]; } catch { }
                    if (f == null || f.IsDisposed) continue;

                    Type ft = f.GetType();
                    string full = ft.FullName ?? string.Empty;
                    string text = string.Empty;
                    try { text = f.Text ?? string.Empty; } catch { }

                    bool demonicForm = asm != null && ft.Assembly == asm;
                    bool authDebugForm = string.Equals(full, "ns1.Debug", StringComparison.Ordinal)
                                         || (demonicForm && string.Equals(ft.Name, "Debug", StringComparison.Ordinal));
                    bool loginLike = text.IndexOf("Login", StringComparison.OrdinalIgnoreCase) >= 0
                                     || text.IndexOf("Auth", StringComparison.OrdinalIgnoreCase) >= 0
                                     || text.IndexOf("Demonic", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (!authDebugForm && !(demonicForm && loginLike))
                        continue;

                    try
                    {
                        if (f.InvokeRequired)
                            f.BeginInvoke(new Action(delegate { try { f.DialogResult = DialogResult.OK; f.Close(); } catch { try { f.Dispose(); } catch { } } }));
                        else
                        { try { f.DialogResult = DialogResult.OK; f.Close(); } catch { try { f.Dispose(); } catch { } } }
                    }
                    catch
                    {
                    }
                }

                UnlockInnerRoutine(asm, cc);
            }
            catch
            {
            }
        }

        #region Overrides of CombatRoutine

        public override string Name
        {
            get { return CC != null ? CC.Name : "Demonic"; }
        }

        public override void Initialize()
        {
            if (CC == null) return;

            Assembly asm = null;
            System.Windows.Forms.Timer dismissTimer = null;
            try
            {
                asm = CC.GetType().Assembly;
                UnlockInnerRoutine(asm, CC);

                // Inner Demonic.Initialize() shows ns1.Debug (defunct auth dialog) via ShowDialog(), which blocks.
                // Dismiss it automatically and keep gate fields unlocked.
                try
                {
                    dismissTimer = new System.Windows.Forms.Timer();
                    dismissTimer.Interval = 250;
                    dismissTimer.Tick += delegate { DismissDefunctAuthDialogs(asm, CC); };
                    dismissTimer.Start();
                    DismissDefunctAuthDialogs(asm, CC);
                }
                catch
                {
                }

                CC.Initialize();
            }
            catch (Exception ex)
            {
                Logging.Write(Colors.DarkRed, "DemonicLoader: Initialize failed: " + ex);
            }
            finally
            {
                try
                {
                    if (dismissTimer != null)
                    {
                        dismissTimer.Stop();
                        dismissTimer.Dispose();
                    }
                }
                catch
                {
                }

                UnlockInnerRoutine(asm, CC);
                DismissDefunctAuthDialogs(asm, CC);
            }
        }

        public override Composite CombatBehavior
        {
            get { return CC != null ? CC.CombatBehavior : null; }
        }

        public override Composite PreCombatBuffBehavior
        {
            get { return CC != null ? CC.PreCombatBuffBehavior : null; }
        }

        public override Composite PullBehavior
        {
            get { return CC != null ? CC.PullBehavior : null; }
        }

        public override Composite RestBehavior
        {
            get { return CC != null ? CC.RestBehavior : null; }
        }

        public override Composite DeathBehavior
        {
            get { return CC != null ? CC.DeathBehavior : null; }
        }

        public override Composite CombatBuffBehavior
        {
            get { return CC != null ? CC.CombatBuffBehavior : null; }
        }

        public override Composite HealBehavior
        {
            get { return CC != null ? CC.HealBehavior : null; }
        }

        public override Composite PullBuffBehavior
        {
            get { return CC != null ? CC.PullBuffBehavior : null; }
        }

        public override Composite MoveToTargetBehavior
        {
            get { return CC != null ? CC.MoveToTargetBehavior : null; }
        }

        public override WoWClass Class
        {
            get { return WoWClass.Warlock; }
        }

        public override void OnButtonPress()
        {
            if (CC == null) return;
            UnlockInnerRoutine(CC.GetType().Assembly, CC);
            CC.OnButtonPress();
        }

        public override void Pulse()
        {
            if (CC == null) return;
            try
            {
                // Re-apply unlock periodically in case inner auth/gate logic re-locks (e.g. defunct network/auth checks).
                int n = Interlocked.Increment(ref _pulseGateCheck);
                if ((n % 200) == 0)
                    UnlockInnerRoutine(CC.GetType().Assembly, CC);
            }
            catch
            {
            }
            CC.Pulse();
        }

        public override void ShutDown()
        {
            if (CC == null) return;
            try { CC.ShutDown(); } catch { }
        }

        public override bool WantButton
        {
            get { return true; }
        }

        public override bool NeedPreCombatBuffs
        {
            get { return CC != null && CC.NeedPreCombatBuffs; }
        }

        public override bool NeedRest
        {
            get { return CC != null && CC.NeedRest; }
        }

        public override bool NeedPullBuffs
        {
            get { return CC != null && CC.NeedPullBuffs; }
        }

        public override bool NeedCombatBuffs
        {
            get { return CC != null && CC.NeedCombatBuffs; }
        }

        public override bool NeedHeal
        {
            get { return CC != null && CC.NeedHeal; }
        }

        #endregion
    }
}
