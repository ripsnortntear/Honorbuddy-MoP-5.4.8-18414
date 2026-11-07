using System;

namespace Superbad
{
    // Legacy auto-updater removed: endpoints (Google Code/SVN) are defunct and the
    // implementation could delete/overwrite local files. Kept as safe no-op stubs.
    internal class Updater
    {
        public static void CheckForUpdate()
        {
        }

        public static double GetOnlineRevision()
        {
            return 0;
        }

        public static string GetOnlineChangelog(double revision)
        {
            return string.Empty;
        }
    }
}
