namespace CrmDownloadWatcher
{
    using Newtonsoft.Json.Linq;
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
            var defaultDownloadFolder = $@"C:\Users\{Environment.UserName}\Downloads";

            if (!File.Exists(filename))
            {
                var downloadFolderPreference = $@"C:\Users\{Environment.UserName}\AppData\Local\Google\Chrome\User Data\Default\Preferences";

                if (File.Exists(downloadFolderPreference))
                {
                    var json = JObject.Parse(File.ReadAllText(downloadFolderPreference));
                    defaultDownloadFolder = json["download"]["default_directory"].ToString();
                }

                File.WriteAllText(filename, defaultDownloadFolder);

                return defaultDownloadFolder;
            }

            return File.ReadAllLines(filename).FirstOrDefault();
        }
    }
}