using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Win11Style.WindowChrome
{
    public class ModernWindow : ControlzEx.WindowChromeWindow
    {
        public ModernWindow()
        {
            ThemeWatcher.OnChromeColorChanged += () => GlowColor = ThemeWatcher.ChromeColor;
            GlowColor = ThemeWatcher.ChromeColor;
        }

        static ModernWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ModernWindow), new FrameworkPropertyMetadata(typeof(ModernWindow)));
        }

        #region Dependency Properties
        public static readonly DependencyProperty ActiveTitleBarMatchGlowProperty =
            DependencyProperty.Register("ActiveTitleBarMatchGlow", typeof(bool), typeof(ModernWindow), new UIPropertyMetadata(false));
        public static readonly DependencyProperty CenterTitleProperty =
            DependencyProperty.Register("CenterTitle", typeof(bool), typeof(ModernWindow), new UIPropertyMetadata(false));
        public static readonly DependencyProperty ButtonHoverColorProperty =
            DependencyProperty.Register("ButtonHoverColor", typeof(SolidColorBrush), typeof(ModernWindow)
                , new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#E5E5E5")));
        public static readonly DependencyProperty ButtonPressedColorProperty =
            DependencyProperty.Register("ButtonPressedColor", typeof(SolidColorBrush), typeof(ModernWindow)
                , new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#CACACB")));
        public static readonly DependencyProperty TitleBarBackgroundProperty =
            DependencyProperty.Register("TitleBarBackground", typeof(SolidColorBrush), typeof(ModernWindow)
                , new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#F3F3F3")));
        public static readonly DependencyProperty TitleBarForegroundProperty =
            DependencyProperty.Register("TitleBarForeground", typeof(SolidColorBrush), typeof(ModernWindow)
                , new UIPropertyMetadata((SolidColorBrush)new BrushConverter().ConvertFrom("#000000")));
        public static readonly DependencyProperty TitleBarHeightProperty =
            DependencyProperty.Register("TitleBarHeight", typeof(double), typeof(ModernWindow)
                , new UIPropertyMetadata(25.0));

        //# E5E5E5
        //#525252
        #endregion

        public bool ActiveTitleBarMatchGlow
        {
            get { return (bool)GetValue(ActiveTitleBarMatchGlowProperty); }
            set { SetValue(ActiveTitleBarMatchGlowProperty, value); }
        }
        public bool CenterTitle
        {
            get { return (bool)GetValue(CenterTitleProperty); }
            set { SetValue(CenterTitleProperty, value); }
        }
        public SolidColorBrush ButtonHoverColor
        {
            get { return (SolidColorBrush)GetValue(ButtonHoverColorProperty); }
            set { SetValue(ButtonHoverColorProperty, value); }
        }
        public SolidColorBrush ButtonPressedColor
        {
            get { return (SolidColorBrush)GetValue(ButtonPressedColorProperty); }
            set { SetValue(ButtonPressedColorProperty, value); }
        }
        public SolidColorBrush TitleBarBackground
        {
            get { return (SolidColorBrush)GetValue(TitleBarBackgroundProperty); }
            set { SetValue(TitleBarBackgroundProperty, value); }
        }
        public SolidColorBrush TitleBarForeground
        {
            get { return (SolidColorBrush)GetValue(TitleBarForegroundProperty); }
            set { SetValue(TitleBarForegroundProperty, value); }
        }
        public double TitleBarHeight
        {
            get { return (double)GetValue(TitleBarHeightProperty); }
            set { SetValue(TitleBarHeightProperty, value); }
        }

    }
}
