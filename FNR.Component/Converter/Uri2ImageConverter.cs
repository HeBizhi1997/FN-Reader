using System;
using System.Globalization;
using FNR.Crawler;

namespace FNR.Component.Converter
{
    public class Uri2ImageConverter : BaseValueConverter<Uri2ImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return HtmlCrawler.GetHtmlImage(value as Uri);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
