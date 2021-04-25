using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Forms;

namespace Projekt_wlasciwy
{
    public partial class PathWindow
    {
        private FolderBrowserDialog folderBrowserDialog1;

        // App Settings Manager
        private readonly SettingsManager sm = new SettingsManager();
        private static long files = 0;
        private static long size = 0;

        public PathWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            files = 0;
            size = 0;

            folderBrowserDialog1 = new FolderBrowserDialog
            {
                Description = "Select the directory that you want to set.",
                RootFolder = Environment.SpecialFolder.MyComputer                   // Default to the My PC folder.
            };

            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result != DialogResult.OK) return;

            string selectedFolderPath = folderBrowserDialog1.SelectedPath;
            pathdialog.Text = selectedFolderPath;
            getInfo(selectedFolderPath);

            DirSizeLabel.Content = $"Amount of files: {files}";
            DirCountFilesLabel.Content = $"Directory size: {calcBytes(size)}";
        }

        private void getInfo(string path)
        {
            IEnumerable<string> dirs;
            try
            {
                // Recursive way for each Directory in the path
                dirs = Directory.EnumerateDirectories(path);
                foreach (string dir in dirs)
                {
                    getInfo(dir);
                }

                // How many files and paths to them
                dirs = Directory.EnumerateFiles(path, "*");
                files += dirs.Count();

                foreach (string file in dirs)
                {
                    size += new FileInfo(file).Length;
                }

            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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
    }
}