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

        public Home()
        {
            InitializeComponent();
            viewModel = new HomeViewModel();
            DataContext = viewModel;
            viewModel.Client_Ip = Global.sClientIp;
            viewModel.Client_Port = Global.iClientPort;
        }

        private void NumericTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Allow only numeric input (0-9)
            foreach (char c in e.Text)
            {
                if (!char.IsDigit(c))
                {
                    e.Handled = true; // Prevent non-digit input
                    return;
                }
            }
        }
    }
}
