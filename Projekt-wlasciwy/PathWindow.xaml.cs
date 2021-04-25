﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using MessageBox = System.Windows.Forms.MessageBox;
using System.IO;

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Logika interakcji dla klasy PathWindow.xaml
    /// </summary>
    public partial class PathWindow
    {

        private FolderBrowserDialog folderBrowserDialog1;

        public PathWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.folderBrowserDialog1.Description = "Select the directory that you want to use as the default.";

            // Default to the My PC folder.
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;

            DialogResult result = this.folderBrowserDialog1.ShowDialog();

            string selectedFolderPath;
            if (result == DialogResult.OK)
            {
                selectedFolderPath = folderBrowserDialog1.SelectedPath;
                this.pathdialog.Text = selectedFolderPath;
                this.DirSizeLabel.Content = string.Concat("Ilość plików: ", getDirFiles(selectedFolderPath));
                this.DirCountFilesLabel.Content = string.Concat("Rozmiar katalogu: ", calcBytes(getDirSize(selectedFolderPath)));
            }
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
                    // files += Directory.EnumerateFiles(dir, "*.*").Count();
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
                    // size += Directory.EnumerateFiles(dir, "*.*").Sum(file => file.Length);
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
            //string[] array = { "kB", "MB", "GB", "TB" };
            int i = 0;
            double sizes = (double)size;
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

            return String.Concat(Math.Round(sizes, 2), str);
        }
    }
}
/*
    0.123123MB = 123.1B
    12312313b = 1,2GB
    123213 = 12KB
*/