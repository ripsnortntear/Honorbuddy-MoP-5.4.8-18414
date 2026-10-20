using System;

namespace HighVoltz.AutoAngler
{
    internal static class Updater
    {
        public static void CheckForUpdate()
        {
            try
            {
                AutoAnglerBot.Instance.Log("AutoAngler2 is up to date (Revision {0}).", new Svn().Revision);
            }
            catch (Exception ex)
            {
                AutoAnglerBot.Instance.Err(ex.ToString());
            }
        }
    }
}
