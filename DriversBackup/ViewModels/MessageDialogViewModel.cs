using System.Collections.ObjectModel;
using System.Windows;
using DriversBackup.Models;
using WpfViewModelBase;
namespace DriversBackup.ViewModels
{
    public class MessageDialogViewModel:ViewModelBase
    {
        /// <summary>
        /// Mock constructor for xaml designer.
        /// </summary>
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
        /// <summary>
        /// Crates instance of a popup windows. It will be displayed in MessageDialogControl.
        /// </summary>
        /// <param name="actionButtons">Collection of ActionButtons, every button should have a caption, template {Default, Accept, Reject} and an action that is invoked when the button is pressed. Buttons will be displayed at the bottom, centered horizontally.</param>
        /// <param name="caption">(optional) Caption of the popup. Should be a short header.</param>
        /// <param name="text">Text that will be displayed in a popup. Should be a detail information.</param>
        public MessageDialogViewModel(ObservableCollection<ActionButton> actionButtons, string caption, string text = "")
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