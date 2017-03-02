using System.Windows;
using System.Windows.Controls;

namespace DriversBackup.Controls
{
    /// <summary>
    /// Interaction logic for DriversBox.xaml
    /// </summary>
    public partial class DriversBox : UserControl
    {
        public DriversBox()
        {
            InitializeComponent();
            DataContext = this;
        }

        public static readonly DependencyProperty TestDp = DependencyProperty.Register(nameof(Test), typeof(string),
            typeof(DriversBox));

        public string Test
        {
            get { return (string) GetValue(TestDp); }
            set { SetValue(TestDp, value); }
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