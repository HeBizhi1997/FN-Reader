using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace FN_Reader.Converters
{
    public abstract class BaseValueConverter<T> : MarkupExtension, IValueConverter
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


        #region Value Converter Methods

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public abstract object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture);

        #endregion
    }
}
