using System;
using WpfViewModelBase;

namespace DriversBackup.Models
{
    public class ActionButton:ViewModelBase
    {
        private readonly Action action;
        private bool isEnabled = true;

        public ActionButton(string text, Action action, ButtonType actionButtonType)
        {
            Text = text;
            ActionButtonType = actionButtonType;
            this.action = action;
        }
        /// <summary>
        /// Design template for the button
        /// </summary>
        public enum ButtonType
        {
            Deafult,
            Accept,
            Cancel
        }
        public string Text { get; set; }
        public ButtonType ActionButtonType { get; set; }
        public RelayCommand ButtonCommand => new RelayCommand(action);

        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                isEnabled = value;
                OnPropertyChanged();
            }
        }
    }
}