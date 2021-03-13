using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace LocalizerEditor
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static System.Threading.Mutex mutex;
        protected override void OnStartup(StartupEventArgs e)
        {
            mutex = new System.Threading.Mutex(true, "LocalizerEditor");
            if (mutex.WaitOne(0, false))
            {
                if (LocalizerEditor.Properties.Settings.Default.UpgradeRequired)
                {
                    HandyControl.Controls.Growl.Success("已更新至新版本.");
                    LocalizerEditor.Properties.Settings.Default.Upgrade();
                    LocalizerEditor.Properties.Settings.Default.UpgradeRequired = false;
                    LocalizerEditor.Properties.Settings.Default.Save();
                }
                base.OnStartup(e);
            }
            else
            {
                MessageBox.Show("程序已经在运行\nThis program is already running", "Warn");
                Shutdown();
            }
        }
    }
}

