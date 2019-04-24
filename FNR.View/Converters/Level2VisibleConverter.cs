using System;
using System.Globalization;
using System.Windows;

namespace FNR.View
{
    public class Level2VisibleConverter : BaseMultiValueConverter<Level2VisibleConverter>
    {
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if ((int)values[0] >= int.Parse(values[1] as string))
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public override object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
