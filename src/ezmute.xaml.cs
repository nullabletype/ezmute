using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using System.Windows.Media.Imaging;
using Hardcodet.Wpf.TaskbarNotification;
using System.Drawing;
using ezmute.Tools;
using System.Windows.Media;

namespace ezmute
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private KeyBinding globalKey;
        private BitmapFrame mutedIcon;
        private BitmapFrame unmutedIcon;
        private Icon mutedIconIcon;
        private Icon unmutedIconIcon;
        //private TaskbarIcon tbi;
        private MuteStatus mutedStylingSet;
        Configuration config;

        public MainWindow(Configuration config)
        {
            this.config = config;
            InitializeComponent();
            mutedStylingSet = MuteStatus.NotSet;

            SetupIcons();
            SetupStatusCheckTimer();
            SetStyling();
            ConfigureHotkeys(config);
            SetInitialConfiguration(config);
        }

        #region Logic

        private void StatusCheck(object sender, EventArgs e)
        {
            SetStyling();
        }

        private void SetStyling()
        {
            if (MicInterface.AllMuted())
            {
                SetMutedStyling();
            }
            else
            {
                SetUnMutedStyling();
            }
        }

        private void SetMutedStyling()
        {
            if (mutedStylingSet == MuteStatus.Muted)
            {
                return;
            }
            if (!string.IsNullOrEmpty(config.MutedColour))
            {
                MuteToggleButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(config.MutedColour);
            }
            else
            {
                MuteToggleButton.Background = System.Windows.Media.Brushes.Red;
            }
            MuteToggleButton.Content = "MUTED";
            Title = "MUTED - ezmute";
            Icon = mutedIcon;
            IndicatorIcon.ImageSource = mutedIcon;
            tbi.Icon = mutedIconIcon;
            tbi.ToolTipText = "MUTED - ezmute";
            ShowBalloonTip("Muted", "All input devices muted!");
            mutedStylingSet = MuteStatus.Muted;
        }

        private void SetUnMutedStyling()
        {
            if (mutedStylingSet == MuteStatus.Unmuted)
            {
                return;
            }
            if (!string.IsNullOrEmpty(config.UnmutedColour))
            {
                MuteToggleButton.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(config.UnmutedColour);
            }
            else
            {
                MuteToggleButton.Background = System.Windows.Media.Brushes.Green;
            }
            MuteToggleButton.Content = "UNMUTED";
            Title = "UNMUTED - ezmute";
            Icon = unmutedIcon;
            IndicatorIcon.ImageSource = unmutedIcon;
            tbi.Icon = unmutedIconIcon;
            tbi.ToolTipText = "UNMUTED - ezmute";
            ShowBalloonTip("Unmuted", "All input devices unmuted!");
            mutedStylingSet = MuteStatus.Unmuted;
        }

        private void ShowBalloonTip(string title, string message)
        {
            if (config.ShowToolTipsOnChange)
            {
                tbi.ShowBalloonTip(title, message, BalloonIcon.Info);
            }
        }

        #endregion

        #region Setup

        private void ConfigureHotkeys(Configuration config)
        {
            ToggleMuteCommand cmd = new ToggleMuteCommand(MuteToggleButton_Click);
            Key key = Key.J;
            ModifierKeys modKey = ModifierKeys.Alt;
            if (!string.IsNullOrEmpty(config.Key?.Key))
            {
                Enum.TryParse(config.Key?.Key, out key);
            }

            if (!string.IsNullOrEmpty(config.ModifierKey?.Key))
            {
                Enum.TryParse(config.ModifierKey?.Key, out modKey);
            }

            globalKey = new KeyBinding(cmd, key, modKey);
            NHotkey.Wpf.HotkeyManager.SetRegisterGlobalHotkey(globalKey, true);
        }

        private void SetInitialConfiguration(Configuration config)
        {
            //Configure Initial size
            if (config.StartingSize != null)
            {
                this.Width = config.StartingSize.X;
                this.Height = config.StartingSize.Y;
            }

            //Configure Initial position
            if (config.StartingPosition != null)
            {
                this.Left = config.StartingPosition.X;
                this.Top = config.StartingPosition.Y;
            }

            // Configure always on top
            if (config.AlwaysOntop)
            {
                this.Topmost = true;
            }

            // Configure Window styles
            if (!string.IsNullOrEmpty(config.BackgroundColour))
            {
                this.Background = (SolidColorBrush)new BrushConverter().ConvertFrom(config.BackgroundColour);
            }

            if (config.FontSize != 0)
            {
                MuteToggleButton.FontSize = config.FontSize;
            }
        }

        private void SetupStatusCheckTimer()
        {
            // Create status check timer
            DispatcherTimer dtClockTime = new DispatcherTimer();
            dtClockTime.Interval = new TimeSpan(0, 0, 2);
            dtClockTime.Tick += StatusCheck;
            dtClockTime.Start();
        }

        private void SetupIcons()
        {
            // Create icons
            mutedIcon = BitmapFrame.Create(Application.GetResourceStream(new Uri("Icons/muted.ico", UriKind.RelativeOrAbsolute)).Stream);
            unmutedIcon = BitmapFrame.Create(Application.GetResourceStream(new Uri("Icons/unmuted.ico", UriKind.RelativeOrAbsolute)).Stream);
            mutedIconIcon = new Icon(Application.GetResourceStream(new Uri("Icons/muted.ico", UriKind.RelativeOrAbsolute)).Stream);
            unmutedIconIcon = new Icon(Application.GetResourceStream(new Uri("Icons/unmuted.ico", UriKind.RelativeOrAbsolute)).Stream);

            // Create NotifyIcon
            tbi.Icon = mutedIconIcon;
            tbi.MenuActivation = PopupActivationMode.LeftOrRightClick;
        }

        #endregion

        #region Window Statuses

        private void Window_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            if (config.AlwaysOntop)
            {
                Window window = (Window)sender;
                window.Topmost = true;
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tbi.Dispose();
        }

        #endregion

        #region UI Interaction Handling

        private void MuteToggleButton_Click(object sender, RoutedEventArgs e)
        {
            MicInterface.ToggleAllMicMute();
            SetStyling();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void DragHandle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        #endregion

        #region Enums

        private enum MuteStatus
        {
            NotSet,
            Muted,
            Unmuted
        }

        #endregion
    }
}
