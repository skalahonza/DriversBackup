using System;
using WpfViewModelBase;

namespace DriversBackup.Models
{
    public class ActionButton : ViewModelBase
    {
        private readonly Action action;
        private bool isEnabled = true;

        /// <summary>
        /// Creates instance for action button
        /// </summary>
        /// <param name="text">Text for the button label</param>
        /// <param name="action">Action that will invoke on click</param>
        /// <param name="actionButtonType">Button type - affects design</param>
        public ActionButton(string text, Action action, ButtonType actionButtonType)
        {
            Text = text;
            ActionButtonType = actionButtonType;
            this.action = action;
        }

        /// <summary>
        /// Creates instance of action button
        /// </summary>
        /// <param name="text">Text for the button label</param>
        /// <param name="action">Action that will invoke on click</param>
        /// <param name="actionButtonType">Button type - affects design</param>
        /// <param name="icon">Any valuable SEGOE MD2 icon character</param>
        /// <param name="meta">For binding or other purposes</param>
        public ActionButton(string text, Action action, ButtonType actionButtonType, string icon, string meta = "")
            : this(text, action, actionButtonType)
        {
            Icon = icon;
            Meta = meta;
        }

        /// <summary>
        /// Creates instance of action button without command
        /// </summary>
        /// <param name="text"></param>
        /// <param name="icon"></param>
        /// <param name="actionButtonType"></param>
        /// <param name="meta"></param>
        public ActionButton(string text, ButtonType actionButtonType, string icon, string meta = "")
        {
            Text = text;
            Icon = icon;
            Meta = meta;
            ActionButtonType = actionButtonType;
        }

        /// <summary>
        /// Design template for the button
        /// </summary>
        public enum ButtonType
        {
            Deafult,
            Accept,
            Cancel,
            NoHighlight
        }

        /// <summary>
        /// Will be displayed in content
        /// </summary>
        public string Text { get; set; }

        //Will be displayed in front of text
        public string Icon { get; set; }

        /// <summary>
        /// Can be used for command parameter or code behind stuff
        /// </summary>
        public string Meta { get; set; }

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