using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using DriversBackup.Models;

namespace DriversBackup.Converters
{
    public class ButtonTypeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((ActionButton.ButtonType) value)
            {
                case ActionButton.ButtonType.Deafult:
                    return Application.Current.FindResource("BlueAccent") as Brush;
                case ActionButton.ButtonType.Accept:
                    return Application.Current.FindResource("GreenAccent") as Brush;
                case ActionButton.ButtonType.Cancel:
                    return Application.Current.FindResource("OrangeAccent") as Brush;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}