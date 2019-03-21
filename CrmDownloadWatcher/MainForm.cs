using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrmDownloadWatcher
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.Manual;
            Location = new System.Drawing.Point(-2000, -2000);
            Size = new System.Drawing.Size(1, 1);
            Visible = false;

            AddAppToWinStartup();

            var watchService = new DirectoryWatcherService();
            watchService.Start();

            FormClosing += (object sender, FormClosingEventArgs e) =>
            {
                watchService.Stop();    
            };
        }

        public static void AddAppToWinStartup()
        {
            try
            {
                Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true).SetValue(Application.ProductName, Application.ExecutablePath);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
