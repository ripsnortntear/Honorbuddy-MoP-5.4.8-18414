using System;
using System.IO;
using System.Reflection;
using System.Drawing;

namespace VitalicRotation.Helpers
{
    public static class ResourceAccessor
    {
        private static Assembly A { get { return typeof(ResourceAccessor).Assembly; } }
        private static string _routineRootDir; // racine "vitalicrotation" sur disque (si détectable)
        private static string[] _manifestNames; // cache des noms de ressources intégrées

        static ResourceAccessor()
        {
            // Tenter de localiser le dossier racine du routine sur disque pour repli fichiers
            try
            {
                // 1) Dossier de l'assembly (utile si compilé MSBuild)
                var codeBase = A.CodeBase;
                if (!string.IsNullOrEmpty(codeBase))
                {
                    var p = new Uri(codeBase).LocalPath;
                    var dir = Path.GetDirectoryName(p);
                    if (!string.IsNullOrEmpty(dir) && Directory.Exists(dir))
                    {
                        // Si on est déjà dans Routines/vitalicrotation/bin/..., remonter jusqu'à vitalicrotation
                        var cur = new DirectoryInfo(dir);
                        while (cur != null)
                        {
                            if (string.Equals(cur.Name, "vitalicrotation", StringComparison.OrdinalIgnoreCase))
                            {
                                _routineRootDir = cur.FullName;
                                break;
                            }
                            cur = cur.Parent;
                        }
                    }
                }

                // 2) Environnement Honorbuddy: utiliser BaseDirectory + Routines\vitalicrotation
                if (string.IsNullOrEmpty(_routineRootDir))
                {
                    var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                    var routinesDir = Path.Combine(baseDir ?? string.Empty, "Routines");
                    if (Directory.Exists(routinesDir))
                    {
                        foreach (var sub in Directory.GetDirectories(routinesDir))
                        {
                            var name = Path.GetFileName(sub);
                            if (string.Equals(name, "vitalicrotation", StringComparison.OrdinalIgnoreCase))
                            {
                                _routineRootDir = sub;
                                break;
                            }
                        }
                    }
                }
            }
            catch { }
        }

        // Dossier logique: VitalicRotation.Resources.Audio / Images
        public static Stream GetAudio(string name)
        {
            // 1) Tentatives via ressources intégrées
            try
            {
                // a) Nom fort avec RootNamespace connu
                var s = A.GetManifestResourceStream("VitalicRotation.Resources.Audio." + name);
                if (s != null) return s;

                // b) Variante basée sur le nom d'assembly (si RootNamespace diffère)
                var root = A.GetName().Name;
                if (!string.IsNullOrEmpty(root))
                {
                    s = A.GetManifestResourceStream(root + ".Resources.Audio." + name);
                    if (s != null) return s;
                }

                // c) Recherche souple: n'importe quelle ressource se terminant par .Resources.Audio.<name>
                if (_manifestNames == null)
                {
                    _manifestNames = A.GetManifestResourceNames() ?? new string[0];
                    try
                    {
                        if (VitalicRotation.Settings.VitalicSettings.Instance.DiagnosticMode)
                        {
                            // Log léger: compter les noms audio disponibles
                            int audioCount = 0;
                            foreach (var rn in _manifestNames) if (rn.IndexOf(".Resources.Audio.", StringComparison.OrdinalIgnoreCase) >= 0) audioCount++;
                            Logger.Write("[Diag][Audio] Manifest resources loaded: total={0} audio={1}", _manifestNames.Length, audioCount);
                        }
                    }
                    catch { }
                }
                for (int i = 0; i < _manifestNames.Length; i++)
                {
                    var rn = _manifestNames[i];
                    if (rn.EndsWith(".Resources.Audio." + name, StringComparison.OrdinalIgnoreCase))
                    {
                        s = A.GetManifestResourceStream(rn);
                        if (s != null) return s;
                    }
                }
            }
            catch { }

            // 2) Repli: lecture directe sur disque (utile avec le compilateur HB qui ignore les EmbeddedResource)
            try
            {
                // a) Essayer sous <routine>/Resources/Audio/<name>
                if (!string.IsNullOrEmpty(_routineRootDir))
                {
                    var p = Path.Combine(_routineRootDir, "Resources", "Audio", name);
                    if (File.Exists(p)) return File.OpenRead(p);
                }

                // b) Essayer relatif au répertoire courant
                var local = Path.Combine("Resources", "Audio", name);
                if (File.Exists(local)) return File.OpenRead(local);

                // c) Essayer sous BaseDirectory/Routines/vitalicrotation/Resources/Audio
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(baseDir))
                {
                    var p3 = Path.Combine(baseDir, "Routines", "vitalicrotation", "Resources", "Audio", name);
                    if (File.Exists(p3)) return File.OpenRead(p3);
                }
            }
            catch { }

            return null;
        }

        public static Stream GetImage(string name)
        {
            // Images: mêmes stratégies que l'audio
            try
            {
                var s = A.GetManifestResourceStream("VitalicRotation.Resources.Images." + name);
                if (s != null) return s;
                var root = A.GetName().Name;
                if (!string.IsNullOrEmpty(root))
                {
                    s = A.GetManifestResourceStream(root + ".Resources.Images." + name);
                    if (s != null) return s;
                }
                if (_manifestNames == null) _manifestNames = A.GetManifestResourceNames() ?? new string[0];
                for (int i = 0; i < _manifestNames.Length; i++)
                {
                    var rn = _manifestNames[i];
                    if (rn.EndsWith(".Resources.Images." + name, StringComparison.OrdinalIgnoreCase))
                    {
                        s = A.GetManifestResourceStream(rn);
                        if (s != null) return s;
                    }
                }
            }
            catch { }

            // Repli disque
            try
            {
                if (!string.IsNullOrEmpty(_routineRootDir))
                {
                    var p = Path.Combine(_routineRootDir, "Resources", "Images", name);
                    if (File.Exists(p)) return File.OpenRead(p);
                }
                var local = Path.Combine("Resources", "Images", name);
                if (File.Exists(local)) return File.OpenRead(local);
                var baseDir = AppDomain.CurrentDomain.BaseDirectory;
                if (!string.IsNullOrEmpty(baseDir))
                {
                    var p3 = Path.Combine(baseDir, "Routines", "vitalicrotation", "Resources", "Images", name);
                    if (File.Exists(p3)) return File.OpenRead(p3);
                }
            }
            catch { }

            return null;
        }

        // Backward-compat helpers kept from previous version
        public static Stream TryGetStream(string fileName)
        {
            return GetAudio(fileName) ?? GetImage(fileName);
        }

        public static Bitmap TryLoadBitmap(string fileName)
        {
            var s = GetImage(fileName);
            if (s == null) return null;
            try { return new Bitmap(s); }
            catch { return null; }
            finally { try { s.Dispose(); } catch { } }
        }
    }
}
