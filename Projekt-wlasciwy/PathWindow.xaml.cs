using System;
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

        #region Contructor
        public PathWindow()
        {
            InitializeComponent();
            MyID = ID++;
            // Console.WriteLine($"ID komponentu #{MyID}");
        }
        #endregion

        /// <summary>
        /// Create new component PathWindow
        /// </summary>
        /// <param name="Directory">Directory assigned to the component</param>
        /// <returns>New component</returns>
        public static async Task<PathWindow> NewWindowComponent(DirectoryModel Directory)
        {
            var pw = new PathWindow();
            await SetInfoLabel(pw, Directory);
            return pw;
        }

        /// <summary>
        /// Select directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog
            {
                Description = "Select the directory that you want to set.",
                RootFolder = Environment.SpecialFolder.MyComputer
            };

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            string selectedFolderPath = folderBrowserDialog1.SelectedPath;
            var Directory = new DirectoryModel(selectedFolderPath, new List<string>());

            await SetInfoLabel(this, Directory);

            // Add or update current directory
            if (DirectoryController.Dirs.Count <= MyID) DirectoryController.Dirs.Add(Directory);
            else DirectoryController.Dirs[MyID] = Directory;
        }

        /// <summary>
        /// Set Label info about directory
        /// </summary>
        /// <param name="Component">Component to set info</param>
        /// <param name="Directory">Directory assigned to the component</param>
        /// <returns></returns>
        private static async Task SetInfoLabel(PathWindow Component, DirectoryModel Directory)
        {
            Component.DirSizeLabel.Content = $"Amount of files: Loading...";
            Component.DirCountFilesLabel.Content = $"Directory size: Loading...";
            Component.pathdialog.Text = Directory.FullPath;

            // Waiting for get info
            await Task.Run(() => Directory.GetAsyncInfo(Directory.FullPath, true));

            Component.DirSizeLabel.Content = $"Amount of files: {Directory.Files}";
            Component.DirCountFilesLabel.Content = $"Directory size: {DirectoryModel.CalcBytes(Directory.Size)}";
        }




        private void bin_btn_Click(object sender, RoutedEventArgs e)
        {
            Path_Window.Visibility = Visibility.Hidden;
            Path_Window.Height = 0;

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

        private void Path_Window_MouseEnter_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Visible;
            path_bg.Opacity = 0.7;
            BitmapImage image = new BitmapImage(new Uri("../../Images/ff8f33.png", UriKind.Relative));
            path_bg.ImageSource = image;
        }

        private void Path_Window_MouseLeave_1(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Hidden;
            path_bg.Opacity = 0.3;
            BitmapImage image = new BitmapImage(new Uri("../../Images/bar.png", UriKind.Relative));
            path_bg.ImageSource = image;
        }
    }
}