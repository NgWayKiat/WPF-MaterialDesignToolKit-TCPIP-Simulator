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
using WpfTemp.ViewModel;
using WpfTemp.Model;

namespace WpfTemp.View
{
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        private HomeViewModel viewModel;
        private TcpClientModel objTcpClientModel = new TcpClientModel();

        public Home()
        {
            InitializeComponent();
            viewModel = new HomeViewModel();
            DataContext = viewModel;
            viewModel.TcpClientModel.Client_Ip = "10.60.144.84";
            viewModel.TcpClientModel.Client_Port = 9999;
        }

        private void BtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            viewModel.OpenDrawers.LeftDrawer = false;
        }
    }
}
