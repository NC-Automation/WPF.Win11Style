using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Win11Style.WindowChrome
{
    public class ModernWindow : ControlzEx.WindowChromeWindow
    {
        public ModernWindow()
        {
            WeakReferenceMessenger.Default.Register<ChromeColorChangedMessage>(this, (r, m) =>
            {
                GlowColor = m.Value;
            });

            GlowColor = ThemeWatcher.ChromeColor;
            Loaded += Win_Loaded;
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

        #region Properties

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

        #endregion

        private void Win_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                HwndSource source = PresentationSource.FromVisual(this) as HwndSource;
                source.AddHook(WndProc);
            }
            catch { }
        }


        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var WMmsg = (WindowChrome.WM)msg;
            if (WMmsg == WM.NCACTIVATE)
            {
                //only flash frame after second consecutive NCACTIVATE - this prevents a flash when activating the window
                if (_flash) AnimateGlowFlash();
                _flash = true;
            }
            else _flash = false;
            return IntPtr.Zero;
        }

        private bool _flash = false;
        private bool _isAnimating = false;
        private void AnimateGlowFlash()
        {
            if (_isAnimating || !GlowColor.HasValue || !NonActiveGlowColor.HasValue) return;
            _isAnimating = true;
            var colorAnimation = new ColorAnimation
            {
                To = NonActiveGlowColor.Value,
                Duration = TimeSpan.FromMilliseconds(10)
            };
            var colorAnimation2 = new ColorAnimation
            {
                To = GlowColor.Value,
                Duration = TimeSpan.FromMilliseconds(10),
                BeginTime = TimeSpan.FromMilliseconds(50)
            };

            var storyboard = new Storyboard();
            Storyboard.SetTarget(colorAnimation, this);
            Storyboard.SetTargetProperty(colorAnimation, new PropertyPath("GlowColor"));
            Storyboard.SetTarget(colorAnimation2, this);
            Storyboard.SetTargetProperty(colorAnimation2, new PropertyPath("GlowColor"));
            storyboard.Children.Add(colorAnimation);
            storyboard.Children.Add(colorAnimation2);
            storyboard.Duration = TimeSpan.FromMilliseconds(100);
            storyboard.Completed += Storyboard_Completed;
            storyboard.Begin();
        }

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            _isAnimating = false;
        }
    }
}
