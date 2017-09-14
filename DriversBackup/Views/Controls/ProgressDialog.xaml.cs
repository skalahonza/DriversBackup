using System.Windows;
using System.Windows.Input;

namespace DriversBackup.Views.Controls
{
    /// <summary>
    /// Interaction logic for ProgressDialog.xaml
    /// </summary>
    public partial class ProgressDialog
    {
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty InProgressTextProperty = DependencyProperty.Register(nameof(InProgressText), typeof(string), typeof(ProgressDialog),
        new FrameworkPropertyMetadata
        {
            BindsTwoWayByDefault = true
        });
        public static readonly DependencyProperty MaximumProgressProperty = DependencyProperty.Register(nameof(MaximumProgress), typeof(int), typeof(ProgressDialog),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(nameof(Progress), typeof(int), typeof(ProgressDialog),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });
        public static readonly DependencyProperty OnCancelProperty = DependencyProperty.Register(nameof(OnCancel), typeof(ICommand), typeof(ProgressDialog),
            new FrameworkPropertyMetadata
            {
                
            });
        public static readonly DependencyProperty IsCancelButtonDisplayedProperty = DependencyProperty.Register(nameof(IsCancelButtonDisplayed), typeof(bool), typeof(ProgressDialog),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });
        public static readonly DependencyProperty IsIndeterminateProperty = DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(ProgressDialog),
            new FrameworkPropertyMetadata
            {
                BindsTwoWayByDefault = true
            });

        public string InProgressText
        {
            get => (string)GetValue(InProgressTextProperty);
            set => SetValue(InProgressTextProperty, value);
        }

        public int MaximumProgress
        {
            get => (int)GetValue(MaximumProgressProperty);
            set => SetValue(MaximumProgressProperty, value);
        }
        public int Progress
        {
            get => (int)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }
        public ICommand OnCancel
        {
            get => (ICommand)GetValue(OnCancelProperty);
            set => SetValue(OnCancelProperty, value);
        }

        public bool IsCancelButtonDisplayed
        {
            get => (bool)GetValue(IsCancelButtonDisplayedProperty);
            set => SetValue(IsCancelButtonDisplayedProperty, value);
        }

        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

    }
}
