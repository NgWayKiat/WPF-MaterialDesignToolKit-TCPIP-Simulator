using LiveCharts;
using System;
using System.Collections.Generic;
using System.Data;
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
using WpfTemp.Model;
using WpfTemp.ViewModel;

namespace WpfTemp.View
{
    /// <summary>
    /// Interaction logic for Chart.xaml
    /// </summary>
    public partial class Chart : UserControl
    {
        public Chart()
        {
            InitializeComponent();
            DataContext = new ChartViewModel();

            Thread connectThread = new Thread(() => { MCC118_VOLTAGE(); });
            connectThread.Start();
        }

        private void MCC118_VOLTAGE()
        {
            var homeViewModel = new ChartViewModel();

            while (true)
            {
                if (Global.dtVoltage != null && Global.dtVoltage.Rows.Count > 0)
                {
                    var labellist = new List<string>();
                    var intlist = new List<double>();
                    int getLastRow = Global.dtVoltage.Rows.Count - 1;
                    

                    foreach (DataRow row in Global.dtVoltage.Rows)
                    {
                        if (double.TryParse(row["ChVal0"].ToString(), out double val))
                        {
                            intlist.Add(val);
                            labellist.Add(row["DateTime"].ToString());
                        }
                    }

                    Dispatcher.Invoke(() =>
                    {
                        // Code that modifies the UI element
                        DataContext = homeViewModel;
                        homeViewModel.Labels = new string[] { };
                        homeViewModel.SeriesValues = new ChartValues<double>(intlist);
                        homeViewModel.Labels = labellist.ToArray();
                        DataRow dr = Global.dtVoltage.Rows[getLastRow];
                        homeViewModel.GaugeValue = Convert.ToDouble(dr["ChVal0"].ToString());
                    });
                }
                Thread.Sleep(3000); // 1000ms = 1 second
            }
        }
    }
}
