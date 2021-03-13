using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HandyControl.Controls;
using HandyControl.Data;
using static LocalizerEditor.Data;
using MessageBox = HandyControl.Controls.MessageBox;

namespace LocalizerEditor
{
    /// <summary>
    /// LEWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LEWindow : HandyControl.Controls.Window
    {
        internal static HandyControl.Controls.Window Instance;
        internal static Properties.Settings Setting => Properties.Settings.Default;
        public LEWindow()
        {
            
        }
        #region 主窗体的各种事件
        protected override void OnInitialized(EventArgs e) //此为窗口已加载
        {
            base.OnInitialized(e);
            Instance = this;
            //默认设定为日间模式
            ResourceDictionary theme = new ResourceDictionary
            {
                Source = new Uri("pack://application:,,,/Resource/Style/Theme/BaseLight.xaml")
            };
            Resources.MergedDictionaries.Add(theme);

            //判断之前设定的路径是否存在
            string location = Properties.Settings.Default.FileLocation;
            if (location != null && location != string.Empty && !Directory.Exists(location))
            {
                Growl.Warning("所指定的文件夹位置已不存在, 请重新指定.");
                Properties.Settings.Default.FileLocation = null;
            }
        }
        #endregion

        #region 主页面文件处理
        private void ChooseFileLocation(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog m_Dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = m_Dialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.Cancel)
            {
                return;
            }
            string m_Dir = m_Dialog.SelectedPath.Trim();
            Setting.FileLocation = m_Dir;
            Growl.Success($"成功选择汉化包位置为 {m_Dir}");
        }
        private void ImportFile(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new Microsoft.Win32.OpenFileDialog()
            {
                Filter = "Localizer汉化包 (*.locpack)|*.locpack"
            };
            var result = openFileDialog.ShowDialog();
            if (result == true)
            {
                
            }
        }
        private void RefreshFiles(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() => Main_AllFiles.ItemsSource = Processer.ReadAllLocpack());
        }
        private void SelectPackage(object sender, SelectionChangedEventArgs e)
        {
            var datagrid = sender as DataGrid;
            Editor.ItemsSource = ((LocpackData)datagrid.SelectedItem).Entries["BasicItemFile"];
            MainTab.SelectedIndex = 1;
        }
        #endregion
        private void ChangeNightMode(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            string resourceStr = (bool)checkbox.IsChecked ? "pack://application:,,,/Resource/Style/Theme/BaseDark.xaml" : "pack://application:,,,/Resource/Style/Theme/BaseLight.xaml";
            ResourceDictionary resource = new ResourceDictionary
            {
                Source = new Uri(resourceStr)
            };
            Resources.MergedDictionaries.RemoveAt(0);
            Resources.MergedDictionaries.Insert(0, resource);
        }
    }
}

