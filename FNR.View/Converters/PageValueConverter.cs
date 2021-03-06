﻿using FNR.Common;
using System;
using System.Diagnostics;
using System.Globalization;

namespace FNR.View
{
    public class PageValueConverter :  BaseValueConverter<PageValueConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((ApplicationPages)value)
            {
                case ApplicationPages.NovelSearch:
                    return new SearchPage();
                case ApplicationPages.DataDownLoad:
                    return new DataDownload();
                case ApplicationPages.Register:
                    return new Register();
                case ApplicationPages.BookShelf:
                    return new BookShelfPage();
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
