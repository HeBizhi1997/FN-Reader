using System.Windows.Controls;
using FNR.ViewModel;

namespace FNR.Component
{
    /// <summary>
    /// ThemesChangeDialog.xaml 的交互逻辑
    /// </summary>
    public partial class ThemesChangeDialog : UserControl
    {
        public ThemesChangeDialog()
        {
            InitializeComponent();

            DataContext = new ThemesChangeDialogViewModel();
        }
    }
}
