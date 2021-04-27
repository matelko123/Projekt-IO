using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using static Projekt_wlasciwy.DownloadManager;
using static Projekt_wlasciwy.SettingsManager;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {
        private SettingsManager sm = new SettingsManager();
        private DownloadManager dm = new DownloadManager();

        public MainWindow()
        {
            InitializeComponent();
            // sm.AddUpdateAppSettings("hello", "world");
            //sm.ReadAllSettings();
            // Console.WriteLine(sm.GetSettings("paths"));
            Dirs path = (Dirs)deserializeObject(sm.GetSettings("paths"));
            //path.print();

            // Console.WriteLine(sm.GetSettings("paths"));
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

            Mode.Content = "Dark";
            
        }

        private void Plus_MouseEnter(object sender, MouseEventArgs e)
        {
            Plus.Opacity = 1;
        }

        private void Plus_MouseLeave(object sender, MouseEventArgs e)
        {
            Plus.Opacity = 0.6;
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            PathWindow pw = new PathWindow();
            WindowsComponents.Children.Add(pw);
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Exit(object sender, RoutedEventArgs e) => Close();

        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void report_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/matelko123/Projekt-IO/issues");
        }
    }
}
