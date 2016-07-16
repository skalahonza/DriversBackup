using System.Windows;
using DriversBackup.MVVM;

namespace DriversBackup.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            AppContext.MainFrame = MainFrame;
        }
    }
}
