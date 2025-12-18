using Styx;
using Styx.Pathing;

namespace Templar.Helpers {
    public static class MountHelper {
        public static void DismountIfMounted() {
            if (StyxWoW.Me.Mounted) {
                CustomLog.Normal("Dismounting from mount.");
                Flightor.MountHelper.Dismount();
            }
        }
    }
}
