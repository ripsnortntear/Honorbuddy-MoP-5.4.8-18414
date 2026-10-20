using System.Windows.Media;
using Styx.Common;

namespace Templar.Helpers {
    public class CustomLog {

        // ===========================================================
        // Constants
        // ===========================================================

        // ===========================================================
        // Fields
        // ===========================================================

        // ===========================================================
        // Constructors
        // ===========================================================

        // ===========================================================
        // Getter & Setter
        // ===========================================================

        private static string PreviousNormal { get; set; }
        private static string PreviousDiagnostic { get; set; }
        private static object[] PreviousNormalArgs { get; set; }
        private static object[] PreviousDiagnosticArgs { get; set; }
        public static string Name { get; set; }

        // ===========================================================
        // Methods for/from SuperClass/Interfaces
        // ===========================================================

        // ===========================================================
        // Methods
        // ===========================================================

        private static bool ArgsEqual(object[] a, object[] b) {
            if (a == null && b == null) {
                return true;
            }
            if (a == null || b == null) {
                return false;
            }
            if (a.Length != b.Length) {
                return false;
            }
            for (int i = 0; i < a.Length; i++) {
                if (!object.Equals(a[i], b[i])) {
                    return false;
                }
            }
            return true;
        }

        public static void Normal(string message, params object[] args) {
            if (message == PreviousNormal) {
                if (args.Length > 0) {
                    if (ArgsEqual(args, PreviousNormalArgs)) {
                        return;
                    }
                } else {
                    return;
                }
            }

            if (args.Length > 0) {
                PreviousNormalArgs = (object[])args.Clone();
            } else {
                PreviousNormalArgs = null;
            }

            PreviousNormal = message;
            CustomNormalLog(message, args);
        }

        public static void Diagnostic(string message, params object[] args) {
            if (message == PreviousDiagnostic) {
                if (args.Length > 0) {
                    if (ArgsEqual(args, PreviousDiagnosticArgs)) {
                        return;
                    }
                } else {
                    return;
                }
            }

            if (args.Length > 0) {
                PreviousDiagnosticArgs = (object[])args.Clone();
            } else {
                PreviousDiagnosticArgs = null;
            }

            PreviousDiagnostic = message;
            CustomDiagnosticLog(message, args);
        }

        public static void NormalSpam(string message, params object[] args) {
            CustomNormalLog(message, args);
        }

        public static void DiagnosticSpam(string message, params object[] args) {
            CustomDiagnosticLog(message, args);
        }

        // ===========================================================
        // Inner and Anonymous Classes
        // ===========================================================

        private static void CustomNormalLog(string message, params object[] args) {
            if(args.Length <= 0) {
                Logging.Write(Colors.DeepSkyBlue, string.Format("[{0}]: ", Name) + message);
                return;
            }

            Logging.Write(Colors.DeepSkyBlue, string.Format("[{0}]: ", Name) + message, args);
        }

        private static void CustomDiagnosticLog(string message, params object[] args) {
            if(args.Length <= 0) {
                Logging.WriteDiagnostic(Colors.DeepSkyBlue, string.Format("[{0} Diag]: ", Name) + message);
                return;
            }

            Logging.WriteDiagnostic(Colors.DeepSkyBlue, string.Format("[{0} Diag]: ", Name) + message, args);
        }
    }
}
