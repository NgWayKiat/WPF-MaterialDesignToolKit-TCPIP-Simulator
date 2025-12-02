using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using static WpfTemp.Model.Global;


namespace WpfTemp.Model
{
    public partial class TcpClientModel : ObservableObject
    {
        private TcpClient tcpClient;

        private NetworkStream networkStream;

        [ObservableProperty]
        string? _Client_Ip = "127.0.0.1";

        [ObservableProperty]
        int _Client_Port = 8888;

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

        [RelayCommand]
        private void Clear()
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                Messages.Clear();
            });
        }

        [RelayCommand]
        private async Task Connect()
        {
            try
            {
                if (tcpClient?.Connected == true)
                {
                    return;
                }
                tcpClient = new TcpClient();
                ShowMessage("Start TCP/IP connection...");
                await tcpClient.ConnectAsync(Client_Ip, Client_Port);
                networkStream = tcpClient.GetStream();
                ShowMessage($"TCP/IP Connected Client Server:{Client_Ip}:{Client_Port}  Client:{tcpClient.Client.LocalEndPoint}");
                IsConnected = tcpClient.Connected;
                IsReadyConnected = false;
                Connected = Brushes.Green;
                StartReceiving();
            }
            catch (Exception ex)
            {
                ShowErrorMessage ($"Connect Error: {ex.Message}");
            }
        }

        [RelayCommand]
        private void Disconnect()
        {
            try
            {
                networkStream?.Close();
                tcpClient?.Close();
                ShowWarningMessage($"TCP/IP Disconnected Client Server:{Client_Ip}:{Client_Port}");
                IsConnected = false;
                IsReadyConnected = true;
                Connected = Brushes.Red;
            }
            catch (Exception ex)
            {
                ShowErrorMessage ($"Disconnect Error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task Send()
        {
            if (tcpClient == null || !tcpClient.Connected)
            {
                Connected = Brushes.Red;
                await Connect();
                if (!IsConnected)
                {
                    return;
                }
            }

            try {
                string message = SendInput;

                byte[] utf8Data = Encoding.UTF8.GetBytes(message);
                networkStream.Write(utf8Data, 0, utf8Data.Length);
                ShowMessage($"Sent Data: {message}");
            }
            catch (Exception ex)
            {
                ShowErrorMessage ($"Send Error: {ex.Message}");
                Disconnect();
            }
        }

        private async void StartReceiving()
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (tcpClient != null && tcpClient.Connected)
                {
                    int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        networkStream?.Close();
                        tcpClient?.Close();
                        IsConnected = false;
                        IsReadyConnected = true;
                        Connected = Brushes.Red;
                        ShowErrorMessage("Connection closed by the server.");
                        break;
                    }

                    var receivedBuffer = buffer.Take(bytesRead).ToArray();
                    string utf8Data = Encoding.UTF8.GetString(receivedBuffer);
                    ShowMessage($"Received Data: {utf8Data}");
                }
            }
            catch (Exception ex)
            {
                if (tcpClient.Connected)
                {
                    ShowErrorMessage ($"Receive Error: {ex.Message}");
                    Disconnect();
                }
            }
        }

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
                });
            }
            catch (Exception) { }
        }
    }
}
