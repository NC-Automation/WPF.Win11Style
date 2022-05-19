using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Win11Style.WindowChrome
{
    /// <summary>
    /// Interaction logic for TitleBar.xaml
    /// </summary>
    public partial class TitleBar : UserControl
    {
        public TitleBar()
        {
            InitializeComponent();
        }

        BitmapSource icon;
        public BitmapSource AppIcon
        {
            get
            {
                if (icon == null)
                {
                    var path = Process.GetCurrentProcess().MainModule.FileName;
                    var ico = Icon.ExtractAssociatedIcon(path);
                    icon = Imaging.CreateBitmapSourceFromHIcon(ico.Handle, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                return icon;
            }
        }

        #region Event Handlers
        private void TitleBarGrid_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void TitleBarGrid_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void ButtonMinimizeOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(Window.GetWindow(this));
        }

        private void ButtonMaximizeOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(Window.GetWindow(this));
        }

        private void ButtonRestoreOnClick(object sender, RoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(Window.GetWindow(this));
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).WindowState = WindowState.Minimized;
        }

        private void RestoreOrMaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Window.GetWindow(this).WindowState == WindowState.Maximized)
            {
                Window.GetWindow(this).WindowState = WindowState.Normal;
            }
            else
            {
                Window.GetWindow(this).WindowState = WindowState.Maximized;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }
        #endregion

    }
}
