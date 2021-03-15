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
using ScrollViewer = System.Windows.Controls.ScrollViewer;

namespace LocalizerEditor
{
    /// <summary>
    /// LEWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LEWindow : HandyControl.Controls.Window
    {
        internal static HandyControl.Controls.Window Instance;
        internal static Properties.Settings Setting => Properties.Settings.Default;
        public static void Invoke(Action action) => Instance.Dispatcher.Invoke(action);
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
            else
            {
                RefreshFiles(null, null);
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
            Editor.DataContext = ((LocpackData)datagrid.SelectedItem).Entries["BasicItemFile"];
            Editor_FileList.ItemsSource = ((LocpackData)datagrid.SelectedItem).FilesName;
            MainTab.SelectedIndex = 1;
            if (sv1 == null || sv2 == null)
            {
                //设置同步滚动
                //分别获取两个DataGrid的ScrollViewer
                sv1 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(Editor_EntriesList_Name, 0), 0) as ScrollViewer;
                sv2 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(Editor_EntriesList_Description, 0), 0) as ScrollViewer;
                //将滚动条属性值VerticalOffset关联到OnScrollChanged1方法
                var offsetChangeListener = DependencyProperty.RegisterAttached("ListenerOffset1", typeof(object), typeof(UserControl), new PropertyMetadata(OnScrollChanged));
                var binding = new Binding("VerticalOffset") { Source = sv1 };
                sv1.SetBinding(offsetChangeListener, binding);

                offsetChangeListener = DependencyProperty.RegisterAttached("ListenerOffset2", typeof(object), typeof(UserControl), new PropertyMetadata(OnScrollChanged));
                binding = new Binding("VerticalOffset") { Source = sv2 };
                sv2.SetBinding(offsetChangeListener, binding);

                sv1.ScrollChanged += new ScrollChangedEventHandler(delegate (object s, ScrollChangedEventArgs _e) { if (_e.VerticalChange == 3) e.Handled = true; });
                sv2.ScrollChanged += new ScrollChangedEventHandler(delegate (object s, ScrollChangedEventArgs _e) { if (_e.VerticalChange == 3) e.Handled = true; });
            }
        }
        #endregion
        #region 编辑器处理
        ScrollViewer sv1;
        ScrollViewer sv2;
        public void OnScrollChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            Invoke(() => {
                double old = e.OldValue == null ? 0 : (double)e.OldValue;
                double a = (double)e.NewValue;
                if(((double)e.NewValue - old) % 3 == 0) a = a > old ? old + 1 : old - 1;  //获取滚动条位置变化后的属性值
                sv1.ScrollToVerticalOffset(a);
                sv2.ScrollToVerticalOffset(a);
            });
        }
        private void SelectEntry(object sender, SelectionChangedEventArgs e)
        {
            var datagrid = sender as DataGrid;
            var data = (LocalizeEntry)datagrid.SelectedItem;


            Editor_Origin.Document.Blocks.Clear();
            Editor_Translate.Document.Blocks.Clear();

            Paragraph paragraph_Origin = new Paragraph();
            Paragraph paragraph_Translate = new Paragraph();

            if (datagrid == Editor_EntriesList_Name)
            {
                paragraph_Origin.Inlines.Add(data.OriginName);
                paragraph_Translate.Inlines.Add(data.TranslateName);
            }
            else
            {
                paragraph_Origin.Inlines.Add(data.OriginDescription);
                paragraph_Translate.Inlines.Add(data.TranslateDescription);
            }
            Editor_Origin.Document.Blocks.Add(paragraph_Origin);
            Editor_Translate.Document.Blocks.Add(paragraph_Translate);
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

