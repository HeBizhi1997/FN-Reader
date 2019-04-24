using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FNR.View
{
    public abstract class BaseMultiValueConverter<T> : MarkupExtension, IMultiValueConverter
        where T : class, new()
    {
        /// <summary>
        /// A Single static instance of this Value Converter
        /// </summary>
        private static T mConverter = null;

        /// <summary>
        /// Provides a static instance of value converter
        /// </summary>
        /// <param name="serviceProvider">The service provider</param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return mConverter ?? (mConverter = new T());
        }

        public abstract object Convert(object[] values, Type targetType, object parameter, CultureInfo culture);

        public abstract object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture);
    }
}
