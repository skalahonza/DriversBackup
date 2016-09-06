using System.Windows;

namespace DriversBackup.Views
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage
    {
        public MainPage()
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
