using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls;

namespace MyShuttle.Dashboard.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                if (parameter == null)
                    return ((bool)value) ? Visibility.Visible : Visibility.Collapsed;
                return ((bool)value) ? Visibility.Collapsed : Visibility.Visible;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
