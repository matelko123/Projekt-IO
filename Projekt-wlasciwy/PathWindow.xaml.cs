using System;
using System.Windows;
using System.Windows.Forms;

namespace Projekt_wlasciwy
{
    public partial class PathWindow
    {
        private FolderBrowserDialog folderBrowserDialog1;

        //private DirectoryStructure Directory;
        private static int ID = 0;
        private int MyID;

        public PathWindow()
        {
            InitializeComponent();
            MyID = ID++;
            Console.WriteLine($"ID komponentu #{MyID}");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
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
            var Directory = new DirectoryStructure(selectedFolderPath, new string[] {  });

            // Add or update current directory
            if(DirectoryController.Dirs.Count <= MyID ) DirectoryController.Dirs.Add(Directory);
            else DirectoryController.Dirs[MyID] = Directory;

            DirSizeLabel.Content = $"Amount of files: {Directory.Files}";
            DirCountFilesLabel.Content = $"Directory size: {calcBytes(Directory.Size)}";
        }

        /// <summary>
        /// Calculate bytes for ex.: 2048B = 2kB
        /// </summary>
        /// <param name="size">Bytes</param>
        /// <returns>String of Bytes for ex.: 2kB</returns>
        private static string calcBytes(long size)
        {
            double sizes = size;
            string str = "B";
            
            if(sizes > 1024)
            {
                sizes /= 1024;
                str = "kB";
            }
            if (sizes > 1024)
            {
                sizes /= 1024;
                str = "MB";
            }
            if (sizes > 1024)
            {
                sizes /= 1024;
                str = "GB";
            }

            return string.Concat(Math.Round(sizes, 2), str);
        }

        private void Path_Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Visible;
        }

        private void Path_Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Hidden;
        }

        private void bin_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Path_Window.Visibility = Visibility.Hidden;
            this.Path_Window.Height = 0;
        }
    }
}