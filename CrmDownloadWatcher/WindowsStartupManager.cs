namespace System.Windows
{
    using CrmDownloadWatcher;
    using Microsoft.Win32;
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Manager App startup with windows
    /// </summary>
    public class WindowsStartupManager
    {
        /// <summary>
        /// Adds the application to win startup.
        /// </summary>
        public static void AddAppToWinStartup()
        {
            try
            {
                var key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (!String.Equals(key.GetValue(Application.ProductName) as string, Application.ExecutablePath, StringComparison.InvariantCultureIgnoreCase))
                {
                    key.SetValue(Application.ProductName, Application.ExecutablePath);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}