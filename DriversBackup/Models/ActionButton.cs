using System;
using WpfViewModelBase;

namespace DriversBackup.Models
{
    /// <summary>
    /// Crates a generic button. 3 templates available - Default, Accept, Cancel. Mainly used in MessageDialog.
    /// </summary>
    public class ActionButton
    {
        private readonly Action action;
        /// <summary>
        /// Crates a generic button. Using given text, action and buttonType.
        /// </summary>
        /// <param name="text">Text, that should be displayed on a button.</param>
        /// <param name="action">Action that should be invoked when the button is pressed - use delegate for safe pointers.</param>
        /// <param name="actionButtonType">Layout template for a button. {Default(blue), Accept(green), Cancel(orange)}</param>
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
    }
}