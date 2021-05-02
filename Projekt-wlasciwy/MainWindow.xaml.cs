﻿using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            Console.WriteLine($"Path to logger.txt: {LoggerController.path}");
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() => SettingsController.LoadDataDir());

                if(DirectoryController.Dirs == null || DirectoryController.Dirs.Count == 0)
                {
                    WindowsComponents.Children.Add(new PathWindow());
                    return;
                }

                foreach(var dir in DirectoryController.Dirs)
                {
                    var pw = new PathWindow();
                    WindowsComponents.Children.Add(pw);
                    PathWindow.SetInfoLabel(pw, dir);
                }
            } 
            catch(Exception ex) 
            {
                LoggerController.PrintException(ex);
            }
        }

        // Async close Program
        private async void Exit(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => SettingsController.SaveDataDir());
            Close();
        }


        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void report_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start("https://github.com/matelko123/Projekt-IO/issues");

        private void Plus_Click(object sender, RoutedEventArgs e) => WindowsComponents.Children.Add(new PathWindow());

        private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Plus_MouseEnter(object sender, MouseEventArgs e) => Plus_bg.ImageSource = new BitmapImage(new Uri("../../Images/bar_guzik-2_akty.png", UriKind.Relative));

        private void Plus_MouseLeave(object sender, MouseEventArgs e) => Plus_bg.ImageSource = new BitmapImage(new Uri("../../Images/bar_guzik-2.png", UriKind.Relative));

   
        private void Left_btn_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("../../Images/button_left_aktyw.png", UriKind.Relative));
            btn.Background = brush;            
        }

        private void Left_btn_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("../../Images/button_left.png", UriKind.Relative));
            btn.Background = brush;
        }
    }
}
