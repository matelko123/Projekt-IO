using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        
        private static string UserRoot = Environment.GetEnvironmentVariable("USERPROFILE");
        private static string DownloadFolder = Path.Combine(UserRoot, "Downloads");

        public MainWindow()
        {
            InitializeComponent();
            LoadPathWindowsFromSettings();
            // Get event on new files
            FileSystemWatcher watcher = new FileSystemWatcher(DownloadFolder)
            {
                NotifyFilter = NotifyFilters.Attributes
                             | NotifyFilters.CreationTime
                             | NotifyFilters.DirectoryName
                             | NotifyFilters.FileName
                             | NotifyFilters.LastAccess
                             | NotifyFilters.LastWrite
                             | NotifyFilters.Security
                             | NotifyFilters.Size
            };

            watcher.Created += DownloadController.OnCreated;
            // watcher.Deleted += DownloadController.OnDeleted;
            // watcher.Renamed += DownloadController.OnRenamed;
            watcher.Error += DownloadController.OnError;

            watcher.Filter = "*";
            watcher.IncludeSubdirectories = false;
            watcher.EnableRaisingEvents = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FilesFound.Content = "Files found: Loading...";

            // Getting info about download folder
            int files = DirectoryModel.GetFilesCount(DownloadFolder);

            FilesFound.Content = "Files found: " + files;
        }

        private async void LoadPathWindowsFromSettings()
        {
            var stopwatch = Stopwatch.StartNew();
            await SettingsController.LoadDataDir();
            await DownloadController.CleanUp();

            
            List<Task> tasks = new List<Task>();
            List<PathWindow> pws = new List<PathWindow>();
            int index = 0;

            try
            {
                foreach(var dir in DirectoryController.Dirs)
                {
                    var pw = new PathWindow();
                    WindowsComponents.Children.Add(pw);
                    pws.Add(pw);
                }

                foreach(var dir in DirectoryController.Dirs)
                {
                    tasks.Add(PathWindow.SetInfoLabel(pws[index++], dir));
                }

                await Task.WhenAll(tasks);
            }
            catch(Exception ex)
            {
                LoggerController.PrintException(ex);
            }

            stopwatch.Stop();
            Console.WriteLine($"Loading paths from setings finished in {stopwatch.ElapsedMilliseconds}ms");

        }

        #region Mouse Events
        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Plus_MouseEnter(object sender, MouseEventArgs e) => Plus_bg.ImageSource = new BitmapImage(new Uri("../../Images/bar_guzik-2_akty.png", UriKind.Relative));

        private void Plus_MouseLeave(object sender, MouseEventArgs e) => Plus_bg.ImageSource = new BitmapImage(new Uri("../../Images/bar_guzik-2.png", UriKind.Relative));

        private void Left_btn_MouseEnter(object sender, MouseEventArgs e) => ChangeImage(sender, "../../Images/button_left_aktyw.png");

        private void Left_btn_MouseLeave(object sender, MouseEventArgs e) => ChangeImage(sender, "../../Images/button_left.png");
        #endregion

        #region Click
        private void report_Click(object sender, RoutedEventArgs e) => Process.Start("https://github.com/matelko123/Projekt-IO/issues/new");

        private void Plus_Click(object sender, RoutedEventArgs e) => WindowsComponents.Children.Add(new PathWindow());

        private void left_btn3_Click(object sender, RoutedEventArgs e) => Process.Start(LoggerController.path);
        #endregion


        // Async close Program
        private async void Exit(object sender, RoutedEventArgs e)
        {
            await Task.Run(() => SettingsController.SaveDataDir());
            Close();
        }

        private void ChangeImage(object sender, string path)
        {
            if(path == null || path.Length == 0)
                return;

            Button btn = (Button)sender;
            var brush = new ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri(path, UriKind.Relative));
            btn.Background = brush;
        }
    }
}
