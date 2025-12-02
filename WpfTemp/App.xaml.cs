using System.Runtime.Serialization.DataContracts;
using System.Windows;
using WpfTemp.Model;
using WpfTemp.View;

namespace WpfTemp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            VariableInitialize();
            ClientSocket.InitiateClientsMapping();
            base.OnStartup(e);
            MainWindow = new MainWindow();

            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() == true)
            {
                MainWindow = new MainWindow();
                MainWindow.Show();
            }
            else MainWindow.Close();
        }

        private void VariableInitialize()
        {
            Global.sUsrName = string.Empty;
            Global.sUsrPassword = string.Empty;
            Global.dtVoltage = new System.Data.DataTable();
        }
    }

}
