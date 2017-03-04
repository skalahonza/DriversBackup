using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using DriversBackup.Models;
using DriversBackup.MVVM;
using WpfViewModelBase;

namespace DriversBackup.Views.Controls
{
    /// <summary>
    /// Interaction logic for DriversBox.xaml
    /// </summary>
    public partial class DriversBox
    {
        public DriversBox()
        {
            InitializeComponent();
        }
        
        private void PlaceholderTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Visible;
        }

        private void PlaceholderTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBox.Text.Length == 0)
                DeleteButton.Visibility = Visibility.Collapsed;
        }

        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            DeleteButton.Visibility = Visibility.Collapsed;
        }
    }
}