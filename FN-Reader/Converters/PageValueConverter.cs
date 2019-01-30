using FN_Reader.Domain;
using FN_Reader.Views.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace FN_Reader.Converters
{
    public class PageValueConverter : BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.None:
                    return null;
                case ApplicationPages.Login:
                    return new LoginPage();
                case ApplicationPages.BookShelf:
                    return new BookshelfPage();
                default:
                    Debugger.Break();
                    return null;
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
