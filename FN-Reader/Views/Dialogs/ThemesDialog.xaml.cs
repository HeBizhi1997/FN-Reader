using MahApps.Metro.Controls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FN_Reader.Views.Dialogs
{
    /// <summary>
    /// ThemesDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ThemesDialog : UserControl
    {
        public ThemesDialog()
        {
            InitializeComponent();

            DataContext = new ViewModels.ThemesDialogViewModel();
        }
    }
}
