using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfTemp.Model;
using static WpfTemp.Model.Global;

namespace WpfTemp.ViewModel
{
    public partial class HomeViewModel : ObservableObject
    {
        private TcpClientModel TcpClientModel = new();

        [ObservableProperty]
        private string? _ReceivedMessages;

        [ObservableProperty]
        string _Client_Ip = Global.sClientIp;

        [ObservableProperty]
        int _Client_Port = Global.iClientPort;

        [ObservableProperty]
        string _SendInput = string.Empty;

        [ObservableProperty]
        Brush _Connected = Brushes.Red;

        [ObservableProperty]
        bool _IsConnected = false;

        [ObservableProperty]
        bool _IsReadyConnected = true;

        [ObservableProperty]
        ObservableCollection<MessageData> _Messages = [];

        [ObservableProperty] 
        private OpenDrawers _OpenDrawers = new();

        [RelayCommand]
        private void Save()
        {
            Global.sClientIp = Client_Ip;
            Global.iClientPort = Client_Port;
            OpenDrawers.LeftDrawer = false;
        }

        [RelayCommand]
        private void Reset()
        {
            Client_Ip = Global.sDefaultClientIp;
            Client_Port = Global.iDefaultClientPort;
            Global.sClientIp = Client_Ip;
            Global.iClientPort = Client_Port;
            OpenDrawers.LeftDrawer = false;
        }

        [RelayCommand]
        private void Execute(string obj) {
            switch (obj)
            {
                case "OpenLeftDrawer": OpenDrawers.LeftDrawer = true; break;
                case "ClearTextBox": SendInput = ""; break;
                default: break;
            }
        }

        [RelayCommand]
        private async Task Connect()
        {
            ShowMessage("Start TCPIP connection...");

            TcpClientModel.Client_Ip = Client_Ip;
            TcpClientModel.Client_Port = Client_Port;
            TcpClientModel.DataReceived += Primary_In;

            try
            {
                await TcpClientModel.Connect();
            }
            catch (Exception ex)
            {
                TcpClientModel.DataReceived -= Primary_In;
                IsConnected = TcpClientModel.IsConnected;
                IsReadyConnected = true;
                Connected = Brushes.Red;
                ShowErrorMessage($"TCPIP Failure. The Server:{Client_Ip}:{Client_Port}. Error [{ex.Message}]");
                return;
            }

            if (TcpClientModel.IsConnected)
            {
                ShowReceiveMessage($"TCPIP Connected. The Server:{Client_Ip}:{Client_Port}  Local:{TcpClientModel.tcpClient.Client.LocalEndPoint}");
                IsConnected = TcpClientModel.IsConnected;
                IsReadyConnected = false;
                Connected = Brushes.Green;
            }
        }

        private void Primary_In(object sender, PrimaryInEventArgs e)
        {
            // This runs on the UI thread due to the Dispatcher.Invoke in the service
            if (e.Data == "CLOSE_CONNECTION")
            {
                TcpClientModel.DataReceived -= Primary_In;
                ShowErrorMessage($"TCPIP Disconnected by Server Client Server:{Client_Ip}:{Client_Port}");
                IsConnected = false;
                IsReadyConnected = true;
                Connected = Brushes.Red;
            }
            else
            {
                ShowReceiveMessage($"Received Data: {e.Data}");
            }
        }

        [RelayCommand]
        private async Task Send()
        {
            if (TcpClientModel.tcpClient == null || !TcpClientModel.IsConnected)
            {
                TcpClientModel.DataReceived -= Primary_In;
                await Connect();
            }

            if (TcpClientModel.IsConnected)
            {
                string message = SendInput;

                if (message.Trim().Length == 0)
                {
                    return;
                }

                try
                {
                    await TcpClientModel.Send(SendInput);
                }
                catch (Exception ex)
                {
                    ShowErrorMessage($"TCPIP Send Failure. The Server:{Client_Ip}:{Client_Port}. Error [{ex.Message}]");
                    return;
                }

                ShowSendMessage($"Sent Data: {message}");
            }
        }

        [RelayCommand]
        private void Disconnect()
        {
            TcpClientModel.DataReceived -= Primary_In;
            TcpClientModel.Disconnect();
            ShowWarningMessage($"TCPIP Disconnected Client Server:{Client_Ip}:{Client_Port}");
            IsConnected = false;
            IsReadyConnected = true;
            Connected = Brushes.Red;
        }

        [RelayCommand]
        private void Clear()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Clear();
            });
        }

        protected void ShowReceiveMessage(string message) => ShowMessage(message, MessageType.RECV);

        protected void ShowSendMessage(string message) => ShowMessage(message, MessageType.SEND);

        protected void ShowErrorMessage(string message) => ShowMessage(message, MessageType.ERRS);

        protected void ShowWarningMessage(string message) => ShowMessage(message, MessageType.WARN);

        protected void ShowMessage(string message, MessageType type = MessageType.INFO, string title = "")
        {
            try
            {
                Application.Current.Dispatcher.Invoke(delegate
                {
                    Messages.Add(new MessageData($"{message}", DateTime.Now, type, title));
                    while (Messages.Count > 100)
                    {
                        Messages.RemoveAt(0);
                    }

                    ScrollViewer? sv = Application.Current.Windows.OfType<Window>()
                                                                 .SelectMany(w => FindVisualChildren<ScrollViewer>(w))
                                                                 .FirstOrDefault(s => s.Name == "svTCPIP");
                    sv?.ScrollToEnd();
                });
            }
            catch (Exception) { }
        }

        // Add this helper method to the TcpClientModel class (or as a static utility in the same file/namespace)
        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child is T t)
                    {
                        yield return t;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }
}
