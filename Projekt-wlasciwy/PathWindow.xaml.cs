using System;
using System.Windows;
using System.Windows.Forms;

namespace Projekt_wlasciwy
{
    public partial class PathWindow
    {
        private FolderBrowserDialog folderBrowserDialog1;

        private DirectoryStructure Directory;

        public PathWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            folderBrowserDialog1 = new FolderBrowserDialog
            {
                Description = "Select the directory that you want to set.",
                RootFolder = Environment.SpecialFolder.MyComputer                   // Default to the My PC folder.
            };

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            string selectedFolderPath = folderBrowserDialog1.SelectedPath;
            pathdialog.Text = selectedFolderPath;
            Directory = new DirectoryStructure(selectedFolderPath, new string[] {  });

            DirSizeLabel.Content = $"Amount of files: {Directory.Files}";
            DirCountFilesLabel.Content = $"Directory size: {calcBytes(Directory.Size)}";
        }

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