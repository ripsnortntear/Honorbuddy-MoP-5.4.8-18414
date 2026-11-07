//******SOURCES FROM CLU CC ty**********************//

namespace KingWoW
{
    using System.IO;
    using System.Media;

    using Styx.Common;
    using System;

    public static class SoundManager
    {
        /* A quick class to handle sound for Keybinds */

        private static SoundPlayer player = new SoundPlayer();

        
        public static void LoadSoundFilePath(string path)
        {
            var fullPath = Path.Combine(Utilities.AssemblyDirectory, path.TrimStart('\\', '/'));
            if (!File.Exists(fullPath))
            {
                Logging.Write("Sound manager: can't load sound " + fullPath);
                return;
            }

            try
            {
                player.SoundLocation = fullPath;
                player.Load();
            }
            catch (Exception e)
            {
                Logging.Write("Sound manager: exception loading sound " + fullPath);
                Logging.Write(e.ToString());
            }
        }

        // Asynchronously plays the selected .wav file once.
        public static void SoundPlay()
        {
            player.Play();
        }

        // Stops the currently playing .wav file, if any.
        public static void SoundStop()
        {
            player.Stop();
        }
    }
}