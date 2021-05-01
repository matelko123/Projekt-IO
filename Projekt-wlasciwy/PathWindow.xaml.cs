﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

namespace Projekt_wlasciwy
{
    public partial class PathWindow
    {
        private FolderBrowserDialog folderBrowserDialog1;

        //private DirectoryModel Directory;
        public static int ID = 0;
        private int MyID;

        public PathWindow()
        {
            InitializeComponent();
            MyID = ID++;
            // Console.WriteLine($"ID komponentu #{MyID}");
        }

        public static async Task<PathWindow> NewWindowComponent(DirectoryModel dir)
        {
            var pw = new PathWindow();
            pw.pathdialog.Text = dir.FullPath;
            await Task.Run(() => dir.GetAsyncInfo(dir.FullPath));
            pw.DirSizeLabel.Content = $"Amount of files: {dir.Files}";
            pw.DirCountFilesLabel.Content = $"Directory size: {DirectoryModel.calcBytes(dir.Size)}";
            return pw;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            DirSizeLabel.Content = $"Amount of files: Loading...";
            DirCountFilesLabel.Content = $"Directory size: Loading...";

            folderBrowserDialog1 = new FolderBrowserDialog
            {
                Description = "Select the directory that you want to set.",

                // Default to the My PC folder.
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            string selectedFolderPath = folderBrowserDialog1.SelectedPath;

            pathdialog.Text = selectedFolderPath;
            var Directory = new DirectoryModel(selectedFolderPath, new List<string>());

            // Add or update current directory
            if (DirectoryController.Dirs.Count <= MyID) DirectoryController.Dirs.Add(Directory);
            else DirectoryController.Dirs[MyID] = Directory;

            await Task.Run(() => Directory.GetAsyncInfo(selectedFolderPath));

            DirSizeLabel.Content = $"Amount of files: {Directory.Files}";
            DirCountFilesLabel.Content = $"Directory size: {DirectoryModel.calcBytes(Directory.Size)}";

            Console.WriteLine($"#{MyID}: {Directory}");
        }

        private void Path_Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Visible;
            path_bg.Opacity = 0.7;
            BitmapImage image = new BitmapImage(new Uri("../../Images/ff8f33.png", UriKind.Relative));
            path_bg.ImageSource = image;
            
        }

        private void Path_Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Hidden;
            path_bg.Opacity = 0.3;
            BitmapImage image = new BitmapImage(new Uri("../../Images/bar.png", UriKind.Relative));
            path_bg.ImageSource = image;
        }

        private void bin_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Path_Window.Visibility = Visibility.Hidden;
            this.Path_Window.Height = 0;

            /*var mw = new MainWindow();
            mw.WindowsComponents.Children.RemoveRange(1,1);*/
            try
            {
                DirectoryController.Dirs.RemoveAt(MyID);
            }
            catch (Exception ex)
            {
                LoggerController.PrintException(ex);
            }
        }
    }
}