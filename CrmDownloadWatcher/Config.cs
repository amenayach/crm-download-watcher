namespace CrmDownloadWatcher
{
    using System;
    using System.IO;
    using System.Linq;

    public partial class DirectoryWatcherService
    {
        public class Config
        {
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
}
