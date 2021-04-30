using System;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media.Imaging;

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
            DirCountFilesLabel.Content = $"Directory size: {DirectoryStructure.calcBytes(Directory.Size)}";
            Console.WriteLine($"#{MyID}: {Directory}");
        }

        private void Path_Window_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Visible;
            path_bg.Opacity = 0.7;
            BitmapImage image = new BitmapImage(new Uri("../../Images/ff8f33.png", UriKind.Relative));
            path_bg.ImageSource = image;
            
        }

        private void Path_Window_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            bin_btn.Visibility = Visibility.Hidden;
            path_bg.Opacity = 0.3;
            BitmapImage image = new BitmapImage(new Uri("../../Images/bar.png", UriKind.Relative));
            path_bg.ImageSource = image;
        }

        private void bin_btn_Click(object sender, RoutedEventArgs e)
        {
            this.Path_Window.Visibility = Visibility.Hidden;
            this.Path_Window.Height = 0;

            /*var mw = new MainWindow();
            mw.WindowsComponents.Children.RemoveRange(1,1);*/

            DirectoryController.Dirs.RemoveAt(MyID);
        }
    }
}