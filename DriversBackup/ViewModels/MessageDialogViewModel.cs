using System.Collections.ObjectModel;
using System.Windows;
using DriversBackup.Models;
using WpfViewModelBase;

namespace DriversBackup.ViewModels
{
    public class MessageDialogViewModel:ViewModelBase
    {
        public MessageDialogViewModel()
        {
            Caption = "Test";
            Text = "Lorem ipsum dolor sit amet.";
            ActionButtons = new ObservableCollection<ActionButton>
            {
                new ActionButton("Hello", () => MessageBox.Show("Hello"), ActionButton.ButtonType.Deafult),
                new ActionButton("Accept", () => MessageBox.Show("Accepted"), ActionButton.ButtonType.Accept),
                new ActionButton("Reject", () => MessageBox.Show("Rejected"), ActionButton.ButtonType.Cancel)
            };
        }
        public MessageDialogViewModel(ObservableCollection<ActionButton> actionButtons, string caption, string text)
        {
            ActionButtons = actionButtons;
            Caption = caption;
            Text = text;
        }
        public ObservableCollection<ActionButton> ActionButtons { get; set; }
        public string Caption { get; set; }
        public string Text { get; set; }
    }
}