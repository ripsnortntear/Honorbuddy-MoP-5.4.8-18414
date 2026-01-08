using VitalicRotation.Managers;

namespace VitalicRotation.Helpers
{
    // Parity wrapper with original structure: delegate to PoisonManager
    internal static class PoisonHelper
    {
        public static void Execute()
        {
            PoisonManager.Execute();
        }

        public static void ExecuteOoc()
        {
            PoisonManager.ExecuteOoc();
        }
    }
}
