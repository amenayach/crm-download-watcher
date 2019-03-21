using System;
using System.IO;
using System.Security;
using System.Security.Permissions;

namespace CrmDownloadWatcher
{
    public partial class DirectoryWatcherService
    {
        private FileSystemWatcher watcher;

        public void Start()
        {
            Run();
        }

        public void Stop()
        {
            watcher.Dispose();
            Logger.LogInfo("Disposing watcher");
        }

        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        private void Run()
        {
            Logger.LogInfo("Starting ...");
            string folderToWatch = Config.GetConfigFolder();

            if (string.IsNullOrWhiteSpace(folderToWatch) || !Directory.Exists(folderToWatch))
            {
                return;
            }

            watcher = new FileSystemWatcher();

            Logger.LogInfo($"Watching {folderToWatch}");

            watcher.Path = folderToWatch;

            // Watch for changes in LastAccess and LastWrite times, and
            // the renaming of files or directories.
            watcher.NotifyFilter = NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.FileName
                                 | NotifyFilters.DirectoryName;

            // Only watch text files.
            //watcher.Filter = "*.txt";

            // Add event handlers.
            watcher.Changed += OnChanged;
            watcher.Created += OnChanged;
            watcher.Renamed += OnChanged;

            // Begin watching.
            watcher.EnableRaisingEvents = true;
        }

        // Define the event handlers.
        private void OnChanged(object source, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Deleted && e.Name.StartsWith("'") && e.Name.EndsWith("\'"))
            {
                var newName = e.FullPath.Replace(e.Name, e.Name.Substring(1, e.Name.Length - 2));
                var counter = 0;

                while (File.Exists(newName))
                {
                    var fileInfo = new FileInfo(newName);

                    newName = $@"{fileInfo.DirectoryName}\{fileInfo.Name.Substring(0, fileInfo.Name.Length - fileInfo.Extension.Length)} ({++counter}){fileInfo.Extension}".Replace($" ({counter - 1})", string.Empty);
                }

                File.Move(e.FullPath, newName);
                System.Threading.Thread.Sleep(1000);
                ExploreFile(newName);
            }
        }

        private bool ExploreFile(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                return false;
            }
            //Clean up file path so it can be navigated OK
            filePath = System.IO.Path.GetFullPath(filePath);
            System.Diagnostics.Process.Start("explorer.exe", string.Format("/select,\"{0}\"", filePath));
            return true;
        }
    }
}
