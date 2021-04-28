using System.Windows;
using System.Windows.Input;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            SettingsManager.ReadAllSettings();
        }

       

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            PathWindow pw = new PathWindow();
            WindowsComponents.Children.Add(pw);
        }

        private void Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Exit(object sender, RoutedEventArgs e) => Close();

        private void navbar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton==MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }

        private void report_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/matelko123/Projekt-IO/issues");
        }
    }
}
