using System;
using System.IO;

namespace LogMeOut.Classes
{
    /// <summary>
    /// Clears a folder in HonorBuddy when the object is disposed.
    /// </summary>
    public class BotFolderRemoval : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BotFolderRemoval" /> class.
        /// </summary>
        /// <param name="folderName">Name of the folder to clear.</param>
        public BotFolderRemoval(string folderName)
        {
            FolderName = folderName;
        }

        public string FolderName { get; set; }

        /// <summary>
        /// Determines whether a given file is locked (in use).
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns><c>true</c> if the file is locked; otherwise, <c>false</c>.</returns>
        public bool IsFileLocked(string path)
        {
            try
            {
                using (new FileStream(path, FileMode.Open)) { }
            }
            catch
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            // Get the path of the folder to clear
            var path = Path.Combine(Directory.GetCurrentDirectory(), FolderName);

            // Clear the folder given in the object's constructor
            foreach (var file in Directory.GetFiles(path))
            {
                var filePath = Path.Combine(path, file);
                if (!IsFileLocked(filePath))
                {
                    File.Delete(filePath);
                }
            }
            //Array.ForEach(Directory.GetFiles(Path.Combine(Directory.GetCurrentDirectory(), FolderName)), File.Delete);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="BotFolderRemoval" /> class.
        /// </summary>
        ~BotFolderRemoval()
        {
            Dispose();
        }
    }
}