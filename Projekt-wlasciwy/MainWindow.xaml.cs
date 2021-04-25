using System.Windows;
using System.Windows.Input;

namespace Projekt_wlasciwy
{
    public partial class MainWindow : Window
    {
        private SettingsManager sm = new SettingsManager();

        public MainWindow()
        {
            InitializeComponent();
            sm.ReadAllSettings();
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {

            Mode.Content = "Dark";
            
        }

        private void Plus_MouseEnter(object sender, MouseEventArgs e)
        {
            Plus.Opacity = 1;
        }

        private void Plus_MouseLeave(object sender, MouseEventArgs e)
        {
            Plus.Opacity = 0.6;
        }
    }
}
