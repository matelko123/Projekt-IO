using Projekt_wlasciwy.Properties;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            Console.WriteLine("Ładowanie zawartości z ustawień...");
            Console.WriteLine($"Settings: {Settings.Default["path"]}");
            //DirectoryController.Load();
        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            DirectoryController.Save();
            // DirectoryController.PrintAll();
            Close();
        }

        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void report_Click(object sender, RoutedEventArgs e) => System.Diagnostics.Process.Start("https://github.com/matelko123/Projekt-IO/issues");

        private void Plus_Click(object sender, RoutedEventArgs e) => WindowsComponents.Children.Add(new PathWindow());

        private void Minimize(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void Plus_MouseEnter(object sender, MouseEventArgs e)
        {
            BitmapImage image = new BitmapImage(new Uri("../../Images/bar_guzik-2_akty.png", UriKind.Relative));
            Plus_bg.ImageSource = image;
        }

        private void Plus_MouseLeave(object sender, MouseEventArgs e)
        {
            BitmapImage image = new BitmapImage(new Uri("../../Images/bar_guzik-2.png", UriKind.Relative));
            Plus_bg.ImageSource = image;
        }
    }
}
