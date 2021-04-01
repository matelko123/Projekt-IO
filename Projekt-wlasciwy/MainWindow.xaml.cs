﻿using System;
using System.Collections.Generic;
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

namespace Projekt_wlasciwy
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
