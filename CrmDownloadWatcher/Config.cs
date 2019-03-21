namespace CrmDownloadWatcher
{
    using System;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Retrieves the folder path to watch from appsettings.txt
    /// </summary>
    public class Config
    {
        /// <summary>
        /// Gets the configuration folder.
        /// </summary>
        /// <returns></returns>
        public static string GetConfigFolder()
        {
            var filename = $@"{Directory.GetCurrentDirectory()}\appsettings.txt";

            if (!File.Exists(filename))
            {
                var defaultDownloadFolder = $@"C:\Users\{Environment.UserName}\Downloads";

                File.WriteAllText(filename, defaultDownloadFolder);

                return defaultDownloadFolder;
            }

            return File.ReadAllLines(filename).FirstOrDefault();
        }
    }
}