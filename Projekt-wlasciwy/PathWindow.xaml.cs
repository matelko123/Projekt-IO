using Microsoft.Win32;
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
                this.DirCountFilesLabel.Content = string.Concat("Rozmiar katalogu: ", getDirSize(selectedFolderPath));
            }
        }
        
        private static int getDirFiles(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);

            return Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories).Count();
            // return info.EnumerateFiles("*", SearchOption.AllDirectories).Count(); ;
        }

        private static float getDirSize(string path)
        {
            DirectoryInfo info = new DirectoryInfo(path);
            float totalSize = 0;
            try
            {
                totalSize = info.EnumerateFiles().Sum(file => file.Length);
            }
            catch (System.UnauthorizedAccessException) { }
            
            return (totalSize);
        }
    }
}
/*
    0.123123MB = 123.1B
    12312313b = 1,2GB
    123213 = 12KB
*/