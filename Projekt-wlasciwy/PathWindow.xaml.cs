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

            // Default to the My Documents folder.
            this.folderBrowserDialog1.RootFolder = Environment.SpecialFolder.MyComputer;

            DialogResult result = this.folderBrowserDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                var folderName = folderBrowserDialog1.SelectedPath;
                // MessageBox.Show(folderName);
                this.pathdialog.Text = folderName;
            }

            
        }
    }
}
