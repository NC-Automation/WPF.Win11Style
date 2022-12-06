using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using Microsoft.Win32;
using System;
using System.Globalization;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Win11Style.WindowChrome
{
    public enum WindowsTheme
    {
        Default = 0,
        Light = 1,
        Dark = 2,
        HighContrast = 3
    }

    public class ThemeChangedArgument
    {
        public WindowsTheme WindowsTheme { set; get; }
    }

    public static class ThemeWatcher
    {
        static ThemeWatcher() { StartThemeWatching(); }
        private static bool _isWatching = false;
        private const string RegistryKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";

        private const string RegistryValueName = "AppsUseLightTheme";
        private static WindowsTheme windowsTheme;

        public static WindowsTheme WindowsTheme
        {
            get { return windowsTheme; }
            set { windowsTheme = value; }
        }
        public static Color ChromeColor { get; set; }

        public static void StartThemeWatching()
        {
            if (_isWatching) return;
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User.Value,
                RegistryKeyPath.Replace(@"\", @"\\"),
                RegistryValueName);

            try
            {
                windowsTheme = GetWindowsTheme();
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += Watcher_EventArrived;
                SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
                //SystemEvents.UserPreferenceChanged += SystemEvents_UserPreferenceChanged;
                // Start listening for events
                watcher.Start();
                _isWatching = true;
                ChromeColor = GetChromeColor().Value;
            }
            catch (Exception ex)
            {
                // This can fail on Windows 7
                windowsTheme = WindowsTheme.Default;

            }

            //subscribe to window messages so we can get the color changed event
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                if (!window.IsLoaded)
                {
                    window.Loaded += Window_Loaded;
                }
                else
                {
                    try
                    {
                        HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
                        source.AddHook(WndProc);
                    }
                    catch { }
                }
            }

        }

        private static void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var window = Application.Current.MainWindow;
            if (window != null)
            {
                try
                {
                    HwndSource source = PresentationSource.FromVisual(window) as HwndSource;
                    source.AddHook(WndProc);
                }
                catch { }
            }
        }

        private static IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            var WMmsg = (WindowChrome.WM)msg;
            if (WMmsg == WindowChrome.WM.DWMCOLORIZATIONCOLORCHANGED)
            {
                var color = GetChromeColor();

                if (color != null && color.Value.ToString() != ChromeColor.ToString())
                {
                    ChromeColor = color.Value;
                    WeakReferenceMessenger.Default.Send(new ChromeColorChangedMessage(ChromeColor));
                }
            }
            return IntPtr.Zero;
        }

        private static void SystemParameters_StaticPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
        }

        private static void Watcher_EventArrived(object sender, EventArrivedEventArgs e)
        {
            WindowsTheme = GetWindowsTheme();
            WeakReferenceMessenger.Default.Send(new ThemeChangedMessage(WindowsTheme));
        }

        public static WindowsTheme GetWindowsTheme()
        {
            WindowsTheme theme = WindowsTheme.Light;

            try
            {
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RegistryKeyPath))
                {
                    object registryValueObject = key?.GetValue(RegistryValueName);
                    if (registryValueObject == null)
                    {
                        return WindowsTheme.Light;
                    }

                    int registryValue = (int)registryValueObject;

                    if (SystemParameters.HighContrast)
                        theme = WindowsTheme.HighContrast;

                    theme = registryValue > 0 ? WindowsTheme.Light : WindowsTheme.Dark;
                }

                return theme;
            }
            catch (Exception ex)
            {
                return theme;
            }
        }

        public static Color? GetChromeColor()
        {
            bool isEnabled;
            var hr1 = DwmIsCompositionEnabled(out isEnabled);
            if ((hr1 != 0) || !isEnabled) // 0 means S_OK.
                return null;

            DWMCOLORIZATIONPARAMS parameters;
            try
            {
                // This API is undocumented and so may become unusable in future versions of OSes.
                var hr2 = DwmGetColorizationParameters(out parameters);
                if (hr2 != 0) // 0 means S_OK.
                    return null;
            }
            catch
            {
                return null;
            }

            // Convert colorization color parameter to Color ignoring alpha channel.
            var targetColor = Color.FromRgb(
                (byte)(parameters.colorizationColor >> 16),
                (byte)(parameters.colorizationColor >> 8),
                (byte)parameters.colorizationColor);

            return targetColor;
        }

        [DllImport("Dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled([MarshalAs(UnmanagedType.Bool)] out bool pfEnabled);

        [DllImport("Dwmapi.dll", EntryPoint = "#127")] // Undocumented API
        private static extern int DwmGetColorizationParameters(out DWMCOLORIZATIONPARAMS parameters);

        [StructLayout(LayoutKind.Sequential)]
        private struct DWMCOLORIZATIONPARAMS
        {
            public uint colorizationColor;
            public uint colorizationAfterglow;
            public uint colorizationColorBalance; // Ranging from 0 to 100
            public uint colorizationAfterglowBalance;
            public uint colorizationBlurBalance;
            public uint colorizationGlassReflectionIntensity;
            public uint colorizationOpaqueBlend;
        }

    }
    public class ThemeChangedMessage : ValueChangedMessage<WindowsTheme>
    {
        public ThemeChangedMessage(WindowsTheme value) : base(value)
        {
        }
    }
    public class ChromeColorChangedMessage : ValueChangedMessage<Color>
    {
        public ChromeColorChangedMessage(Color value) : base(value)
        {
        }

    }
}
