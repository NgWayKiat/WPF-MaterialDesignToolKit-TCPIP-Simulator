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
    /// Interaction logic for Devices.xaml
    /// </summary>
    public partial class Devices : UserControl
    {
        public Devices()
        {
            InitializeComponent();
            DataContext = new DeviceViewModel();

            Thread connectThread = new Thread(() => { MCC118_VOLTAGE(); });
            connectThread.Start();
        }

        private void MCC118_VOLTAGE()
        {
            int ret = 0;
            string sMsg = "";
            string[] temp = { "" };
            string[] temp1 = { "" };

            var deviceViewModel = new DeviceViewModel();

            Global.dtVoltage = GenerateTblVoltageDC();
            List<string> value = new List<string>();

            while (true)
            {
                value = new List<string>();
                ret = 0;
                sMsg = "";

                ret = ClientSocket.sendClientMessage("MCC118_1", $"{Global.CMD8888}|:|Check all channel", out sMsg);

                if (ret > 0)
                {
                    temp = sMsg.Trim().Split(";");

                    if (temp.Length > 0)
                    {
                        foreach (string sTemp in temp)
                        {
                            temp1 = sTemp.Split("=");
                            if (temp1.Length > 1)
                            {
                                value.Add(temp1[1]);
                            }
                        }
                    }

                    Global.dtVoltage.Rows.Add(DateTime.Now.ToString("dd-MMM-yyyy HH:mm:ss:tt"), value[0], value[1], value[2], value[3], value[4], value[5], value[6], value[7]);
                    
                    var labellist = new List<string>();
                    var intlist = new List<double>();

                    foreach (DataRow row in Global.dtVoltage.Rows)
                    {
                        if (double.TryParse(row["ChVal0"].ToString(), out double val) && double.TryParse(row["ChVal1"].ToString(), out double val1))
                        {
                            intlist.Add(val);
                            labellist.Add(row["DateTime"].ToString());
                        }
                    }

                    Dispatcher.Invoke(() =>
                    {
                        // Code that modifies the UI element
                        DataContext = deviceViewModel;
                        deviceViewModel.CH0_VAL = value[0];
                        deviceViewModel.CH1_VAL = value[1];
                        deviceViewModel.CH2_VAL = value[2];
                        deviceViewModel.CH3_VAL = value[3];
                        deviceViewModel.CH4_VAL = value[4];
                        deviceViewModel.CH5_VAL = value[5];
                        deviceViewModel.CH6_VAL = value[6];
                        deviceViewModel.CH7_VAL = value[7];
                    });
                }
                else
                {
                    Global.dtVoltage.Rows.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:tt"), "0", "0", "0", "0", "0", "0", "0", "0");

                    var labellist = new List<string>();
                    var intlist = new List<double>();

                    foreach (DataRow row in Global.dtVoltage.Rows)
                    {
                        if (double.TryParse(row["ChVal0"].ToString(), out double val) && double.TryParse(row["ChVal1"].ToString(), out double val1))
                        {
                            intlist.Add(val);
                            labellist.Add(row["DateTime"].ToString());
                        }
                    }

                    Dispatcher.Invoke(() => {
                        // Code that modifies the UI element
                        DataContext = deviceViewModel;
                        deviceViewModel.CH0_VAL = "0";
                        deviceViewModel.CH1_VAL = "0";
                        deviceViewModel.CH2_VAL = "0";
                        deviceViewModel.CH3_VAL = "0";
                        deviceViewModel.CH4_VAL = "0";
                        deviceViewModel.CH5_VAL = "0";
                        deviceViewModel.CH6_VAL = "0";
                        deviceViewModel.CH7_VAL = "0";
                    });
                }

                if (Global.dtVoltage.Rows.Count > 4)
                {
                    Global.dtVoltage.Rows[0].Delete();
                    Global.dtVoltage.AcceptChanges();
                }

                Thread.Sleep(3000); // 1000ms = 1 second
            }
        }

        private DataTable GenerateTblVoltageDC()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("DateTime", typeof(string));
            dt.Columns.Add("ChVal0", typeof(string));
            dt.Columns.Add("ChVal1", typeof(string));
            dt.Columns.Add("ChVal2", typeof(string));
            dt.Columns.Add("ChVal3", typeof(string));
            dt.Columns.Add("ChVal4", typeof(string));
            dt.Columns.Add("ChVal5", typeof(string));
            dt.Columns.Add("ChVal6", typeof(string));
            dt.Columns.Add("ChVal7", typeof(string)); 

            return dt;
        }
    }
}
