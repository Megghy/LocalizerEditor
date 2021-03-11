using System;
using System.Collections.Generic;
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

namespace LocalizerEditor
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LTEWindow : HandyControl.Controls.Window
    {
        public LTEWindow()
        {

        }
        #region 主窗体的各种事件
        protected override void OnInitialized(EventArgs e) //此为窗口已加载
        {
            base.OnInitialized(e);
        }
        #endregion

        #region 主页面文件处理
        private void ChooseFileLocation(object sender, RoutedEventArgs e)
        {

        }
        #endregion
    }
}

