using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Styx.Common;
using Styx.Pathing;

namespace Plugins.GroundMountOnly
{
    public static class LevelbotFormHook
    {
        private static bool _initialized;
        private static readonly object _initLock = new object();
        private static List<uint> _flightorMapsBackup;

        public static void Initialize()
        {
            if (_initialized)
                return;

            lock (_initLock)
            {
                if (_initialized)
                    return;

                _initialized = true;

                try
                {
                    Application.Idle += OnApplicationIdle;
                }
                catch (Exception ex)
                {
                    Logging.WriteDiagnostic("[GroundMountOnly] Application.Idle hook error: " + ex.Message);
                }

                try
                {
                    Type dispatcherType = Type.GetType("System.Windows.Interop.ComponentDispatcher, WindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35");
                    if (dispatcherType != null)
                    {
                        EventInfo idleEvent = dispatcherType.GetEvent("ThreadIdle", BindingFlags.Public | BindingFlags.Static);
                        if (idleEvent != null)
                        {
                            EventHandler handler = OnWpfThreadIdle;
                            idleEvent.AddEventHandler(null, handler);
                        }
                    }
                }
                catch
                {
                }

                if (GroundMountSettings.Instance.UseGroundMountsOnly)
                {
                    ApplyFlightorRestriction(true);
                }
            }
        }

        private static void OnApplicationIdle(object sender, EventArgs e)
        {
            CheckOpenForms();
        }

        private static void OnWpfThreadIdle(object sender, EventArgs e)
        {
            CheckOpenForms();
        }

        public static void CheckOpenForms()
        {
            try
            {
                foreach (Form form in Application.OpenForms)
                {
                    if (form != null && !form.IsDisposed && form.GetType().FullName == "Levelbot.FormLevelbotSettings")
                    {
                        TryInjectCheckbox(form);
                    }
                }
            }
            catch
            {
            }
        }

        public static void TryInjectCheckbox(Form form)
        {
            if (form == null || form.IsDisposed)
                return;

            if (form.Tag as string == "GroundMountsHooked")
                return;

            var splitContainer = form.Controls.OfType<SplitContainer>().FirstOrDefault();
            if (splitContainer == null)
                return;

            var panel1 = splitContainer.Panel1;
            if (panel1.Controls.Find("cbUseGroundMountsOnly", false).Length > 0)
            {
                form.Tag = "GroundMountsHooked";
                return;
            }

            var cbFarming = panel1.Controls.Find("cbGroundMountFarmingMode", false).FirstOrDefault() as CheckBox;
            int yPos = cbFarming != null ? cbFarming.Bottom + 6 : 38;
            int xPos = cbFarming != null ? cbFarming.Left : 12;

            var cbGroundOnly = new CheckBox
            {
                Name = "cbUseGroundMountsOnly",
                Text = "Use ground mounts only",
                AutoSize = true,
                Location = new Point(xPos, yPos),
                Checked = GroundMountSettings.Instance.UseGroundMountsOnly
            };

            cbGroundOnly.CheckedChanged += delegate
            {
                GroundMountSettings.Instance.UseGroundMountsOnly = cbGroundOnly.Checked;
                GroundMountSettings.Instance.Save();
                ApplyFlightorRestriction(cbGroundOnly.Checked);
            };

            panel1.Controls.Add(cbGroundOnly);

            form.Height += 28;
            splitContainer.SplitterDistance += 28;

            form.FormClosing += delegate
            {
                GroundMountSettings.Instance.UseGroundMountsOnly = cbGroundOnly.Checked;
                GroundMountSettings.Instance.Save();
                ApplyFlightorRestriction(cbGroundOnly.Checked);
            };

            var btnSave = splitContainer.Panel2.Controls.Find("BtnSave", false).FirstOrDefault() as Button;
            if (btnSave != null)
            {
                btnSave.Click += delegate
                {
                    GroundMountSettings.Instance.UseGroundMountsOnly = cbGroundOnly.Checked;
                    GroundMountSettings.Instance.Save();
                    ApplyFlightorRestriction(cbGroundOnly.Checked);
                };
            }

            var btnSaveAndClose = splitContainer.Panel2.Controls.Find("BtnSaveAndClose", false).FirstOrDefault() as Button;
            if (btnSaveAndClose != null)
            {
                btnSaveAndClose.Click += delegate
                {
                    GroundMountSettings.Instance.UseGroundMountsOnly = cbGroundOnly.Checked;
                    GroundMountSettings.Instance.Save();
                    ApplyFlightorRestriction(cbGroundOnly.Checked);
                };
            }

            form.Tag = "GroundMountsHooked";
        }

        public static void ApplyFlightorRestriction(bool useGroundOnly)
        {
            try
            {
                FieldInfo listField = typeof(Flightor).GetField("list_0", BindingFlags.NonPublic | BindingFlags.Static);
                if (listField != null)
                {
                    var mapList = listField.GetValue(null) as List<uint>;
                    if (mapList != null)
                    {
                        if (useGroundOnly)
                        {
                            if (_flightorMapsBackup == null || _flightorMapsBackup.Count == 0)
                            {
                                _flightorMapsBackup = new List<uint>(mapList);
                            }
                            if (mapList.Count > 0)
                            {
                                mapList.Clear();
                                Logging.Write("[GroundMountOnly] Flightor maps cleared. Bot will use ground mounts and ground pathing only.");
                            }
                        }
                        else
                        {
                            if (_flightorMapsBackup != null && _flightorMapsBackup.Count > 0)
                            {
                                mapList.Clear();
                                mapList.AddRange(_flightorMapsBackup);
                                Logging.Write("[GroundMountOnly] Flightor maps restored. Normal flying mount behavior active.");
                            }
                            else if (mapList.Count == 0)
                            {
                                mapList.AddRange(new uint[] { 0, 1, 530, 571, 646, 870, 609 });
                                Logging.Write("[GroundMountOnly] Flightor maps defaulted. Normal flying mount behavior active.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.WriteException(ex);
            }
        }
    }
}
