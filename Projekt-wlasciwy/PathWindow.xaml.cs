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
            DirSizeLabel.Content = string.Concat("Ilość plików: ", getDirFiles(selectedFolderPath));
            DirCountFilesLabel.Content = string.Concat("Rozmiar katalogu: ", calcBytes(getDirSize(selectedFolderPath)));
        }

        private static int getDirFiles(string path)
        {
            IEnumerable<string> dirs;
            int files = 0;
            try
            {
                dirs = Directory.EnumerateDirectories(path);
                foreach (string dir in dirs)
                {
                    files += getDirFiles(dir);
                }
                files += Directory.EnumerateFiles(path, "*.*").Count();

            }
            catch (UnauthorizedAccessException uAEx)
            {
                Console.WriteLine(uAEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return files;
        }

        private static long getDirSize(string path)
        {
            IEnumerable<string> dirs;
            long size = 0;
            try
            {
                dirs = Directory.EnumerateDirectories(path);
                foreach (string dir in dirs)
                {
                    size += getDirSize(dir);
                }

                dirs = Directory.EnumerateFiles(path, "*");
                foreach(string file in dirs)
                {
                    FileInfo fi = new FileInfo(file);
                    size += fi.Length;
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
            return size;
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