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
            // Make sure we check for existence of the the
            // selected file.
            var fileExists = File.Exists(Utilities.AssemblyDirectory + path);

            try
            {
                // Check the file exists.
                if (fileExists)
                {
                    // Assign the selected file's path to
                    // the SoundPlayer object.
                    player.SoundLocation = Utilities.AssemblyDirectory + path;
                }
                else
                    Logging.Write("Sound manager: Cant load sound " + Utilities.AssemblyDirectory + path);

                // Load the .wav file.
                player.Load();
            }
            catch (Exception e)
            {
                Logging.Write("Sound manager: exception on sound " + Utilities.AssemblyDirectory + path);
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