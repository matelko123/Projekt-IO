using System;
using System.Windows;
using System.Windows.Input;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            SettingsController.ReadAllSettings();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            DirectoryController.SaveDataToSettings();
            DirectoryController.PrintAll();
            Close();
        }

        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void report_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start("https://github.com/matelko123/Projekt-IO/issues");

        private void Plus_Click(object sender, RoutedEventArgs e) => WindowsComponents.Children.Add(new PathWindow());

        private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    }
}
