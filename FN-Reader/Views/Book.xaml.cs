using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FN_Reader.Views
{
    /// <summary>
    /// Book.xaml 的交互逻辑
    /// </summary>
    public partial class Book : UserControl
    {

        #region DependencyProperty

        //书名
        public string BookName
        {
            get { return (string)GetValue(BookNameProperty); }
            set { SetValue(BookNameProperty, value); }
        }

        public static readonly DependencyProperty BookNameProperty =
            DependencyProperty.Register("BookName", typeof(string), typeof(Book), new PropertyMetadata(string.Empty));

        //作者
        public string BookAuthor
        {
            get { return (string)GetValue(BookAuthorProperty); }
            set { SetValue(BookAuthorProperty, value); }
        }

        public static readonly DependencyProperty BookAuthorProperty =
            DependencyProperty.Register("BookAuthor", typeof(string), typeof(Book), new PropertyMetadata(string.Empty));

        //最后更新
        public string LastUpDate
        {
            get { return (string)GetValue(LastUpDateProperty); }
            set { SetValue(LastUpDateProperty, value); }
        }

        public static readonly DependencyProperty LastUpDateProperty =
            DependencyProperty.Register("LastUpDate", typeof(string), typeof(Book), new PropertyMetadata(DateTime.Now.ToString("yyyy-MM-dd")));

        //最新章节
        public string Lately
        {
            get { return (string)GetValue(LatelyProperty); }
            set { SetValue(LatelyProperty, value); }
        }

        public static readonly DependencyProperty LatelyProperty =
            DependencyProperty.Register("Lately", typeof(string), typeof(Book), new PropertyMetadata(string.Empty));

        //简介
        public string Intro
        {
            get { return (string)GetValue(IntroProperty); }
            set { SetValue(IntroProperty, value); }
        }

        public static readonly DependencyProperty IntroProperty =
            DependencyProperty.Register("Intro", typeof(string), typeof(Book), new PropertyMetadata(string.Empty));

        //封面图
        public BitmapImage Picture
        {
            get { return (BitmapImage)GetValue(PictureProperty); }
            set { SetValue(PictureProperty, value); }
        }

        public static readonly DependencyProperty PictureProperty =
            DependencyProperty.Register("Picture", typeof(BitmapImage), typeof(Book), new PropertyMetadata(new BitmapImage(new Uri("pack://application:,,,/FN-Reader;component/Real.jpg", UriKind.Absolute))));

        //类型
        public string Type
        {
            get { return (string)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        public static readonly DependencyProperty TypeProperty =
            DependencyProperty.Register("Type", typeof(string), typeof(Book), new PropertyMetadata(string.Empty));

        #endregion


        public Book()
        {
            InitializeComponent();
        }
    }
}
