using FNR.Common;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;

namespace FNR.Converter
{
    public class PageValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.NovelSearch:
                    return new SearchPage();
                case ApplicationPages.DataDownLoad:
                    return new System.Windows.Controls.Page();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
