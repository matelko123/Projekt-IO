﻿using System.Windows;
using System.Windows.Input;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            SettingsManager.ReadAllSettings();
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
    }
}
