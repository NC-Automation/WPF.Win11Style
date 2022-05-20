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
using Win11Style.WindowChrome;

namespace Win11Style.Showcase
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            if (ThemeWatcher.WindowsTheme == WindowsTheme.Dark) DarkMode.IsChecked = true;
        }
        private void DarkMode_UnChecked(object sender, RoutedEventArgs e)
        {
            var dark = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/DarkMode.xaml"));
            var light = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/LightMode.xaml"));
            var win11 = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/Win11Style.xaml"));
            if (dark != null && win11 != null)
            {
                App.Current.Resources.MergedDictionaries.Remove(win11);
                App.Current.Resources.MergedDictionaries.Remove(dark);
            }
            if (light != null) App.Current.Resources.MergedDictionaries.Remove(light);
            if (Win11Style.IsChecked == true)
            {
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/LightMode.xaml") });
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/Win11Style.xaml") });
            }
        }

        private void DarkMode_Checked(object sender, RoutedEventArgs e)
        {
            Win11Style.IsChecked = false;
            var dark = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/DarkMode.xaml"));
            var light = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/LightMode.xaml"));
            var win11 = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/Win11Style.xaml"));
            if (light != null && win11 != null)
            {
                App.Current.Resources.MergedDictionaries.Remove(win11);
                App.Current.Resources.MergedDictionaries.Remove(dark);
            }
            if (dark != null) App.Current.Resources.MergedDictionaries.Remove(dark);
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/DarkMode.xaml") });
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/Win11Style.xaml") });
        }

        private void Win11_Unchecked(object sender, RoutedEventArgs e)
        {
            var light = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/LightMode.xaml"));
            var win11 = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/Win11Style.xaml"));
            if (light != null && win11 != null && DarkMode.IsChecked == false)
            {
                App.Current.Resources.MergedDictionaries.Remove(win11);
                App.Current.Resources.MergedDictionaries.Remove(light);
            }
        }

        private void Win11_Checked(object sender, RoutedEventArgs e)
        {
            DarkMode.IsChecked = false;
            var light = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/LightMode.xaml"));
            var win11 = App.Current.Resources.MergedDictionaries.FirstOrDefault(t => t.Source.OriginalString.Contains("Themes/Win11Style.xaml"));
            if (light != null && win11 != null)
            {
                App.Current.Resources.MergedDictionaries.Remove(win11);
                App.Current.Resources.MergedDictionaries.Remove(light);
            }
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/LightMode.xaml") });
            App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri("pack://application:,,,/Win11Style;component/Themes/Win11Style.xaml") });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var win = new MainWindow();
            win.Owner = this;
            win.WindowStyle = WindowStyle.ToolWindow;
            win.ShowDialog();
        }
    }
}
