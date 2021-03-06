using System;
using System.Collections.Generic;
using System.IO;
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
        public PathWindow(string filepath)
        {
            filepath = Path.GetFileName(filepath);
            InitializeComponent();
            MyID = ID++;
            Name = $"PathWindow_{filepath}";
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

            Component.DirSizeLabel.Content = "Loading...";
            Component.DirCountFilesLabel.Content = "Loading...";
            Component.pathdialog.Text = Directory.FullPath;

            await Task.Run(() => Directory.GetAsyncInfo());

            Component.DirSizeLabel.Content = Directory.Files;
            Component.DirName.Text = Directory.Name;
            Component.DirCountFilesLabel.Content = DirectoryModel.CalcBytes(Directory.Size);
        }

        public static void Update(PathWindow Component, string fileFullPath, bool increament = true)
        {
            if(Component is null || fileFullPath is null)
                return;

            int count = (increament) ? 1 : -1;
            long filesize = new FileInfo(fileFullPath).Length;
            long size = (increament) ? filesize : -filesize;

            Component.DirSizeLabel.Content = (int)Component.DirSizeLabel.Content + count;
            Component.DirCountFilesLabel.Content = DirectoryModel.CalcBytes((int)Component.DirCountFilesLabel.Content + size);
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