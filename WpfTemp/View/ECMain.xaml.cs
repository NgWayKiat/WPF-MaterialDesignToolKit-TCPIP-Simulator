using System.Windows.Controls;
using WpfTemp.ViewModel;

namespace WpfTemp.View
{
    /// <summary>
    /// Interaction logic for ECMain.xaml
    /// </summary>
    public partial class ECMain : UserControl
    {
        public ECMain()
        {
            InitializeComponent();
            DataContext = new ECMainViewModel();
        }
    }
}
