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

            // Add or update current directory
            if(DirectoryController.Dirs.Count <= MyID)
                DirectoryController.Dirs.Add(Directory);
            else
                DirectoryController.Dirs[MyID] = Directory;

            await SetInfoLabel(this, Directory);
        }

        /// <summary>
        /// Set Label info about directory
        /// </summary>
        /// <param name="Component">Component to set info</param>
        /// <param name="Directory">Directory assigned to the component</param>
        /// <returns></returns>
        public static async Task SetInfoLabel(PathWindow Component, DirectoryModel Directory)
        {
            if(Component == null || Directory == null) return;

            Directory.Files = 0;
            Directory.Size = 0;

            Component.DirSizeLabel.Content = $"Amount of files: Loading...";
            Component.DirCountFilesLabel.Content = $"Directory size: Loading...";
            Component.pathdialog.Text = Directory.FullPath;

            await Task.Run(() => Directory.GetAsyncInfo());

            Component.DirSizeLabel.Content = $"Amount of files: {Directory.Files}";
            Component.DirName.Text = Directory.Name;
            Component.DirCountFilesLabel.Content = $"Directory size: {DirectoryModel.CalcBytes(Directory.Size)}";
        }

        public static async Task Update(PathWindow Component)
        {
            DirectoryModel dir = new DirectoryModel(Component.pathdialog.Text);
            await dir.GetAsyncInfo();

            Component.DirSizeLabel.Content = $"Amount of files: {dir.Files}";
            Component.DirCountFilesLabel.Content = $"Directory size: {DirectoryModel.CalcBytes(dir.Size)}";
        }


        private void bin_btn_Click(object sender, RoutedEventArgs e)
        {
            Path_Window.Visibility = Visibility.Hidden;
            Path_Window.Height = 0;

            try
            {
                //! Bug with bad index MyID
                DirectoryController.Dirs[MyID] = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
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