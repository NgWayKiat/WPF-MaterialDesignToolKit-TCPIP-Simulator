using CommunityToolkit.Mvvm.ComponentModel;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace WpfTemp.Model
{
    public partial class TcpClientModel : ObservableObject
    {
        private AsyncCallback sendCallback = null;

        public event EventHandler<PrimaryInEventArgs> DataReceived;

        public event EventHandler<PrimaryOutEventArgs> DataSent;

        public TcpClient tcpClient;

        public NetworkStream networkStream;

        public bool IsConnected
        {
            get
            {
                return tcpClient != null && tcpClient.Connected;
            }
        }

        public string Client_Ip { get; set; } = Global.sClientIp;
        public int Client_Port { get; set; } = Global.iClientPort;

        public async Task Connect()
        {
            try
            {
                if (tcpClient?.Connected == true)
                {
                    return;
                }
                tcpClient = new TcpClient();
                await tcpClient.ConnectAsync(Client_Ip, Client_Port);
                networkStream = tcpClient.GetStream();

                _ = StartReceiving();
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }
        }

        public void Disconnect()
        {
            networkStream?.Close();
            tcpClient?.Close();
        }

        public Task Send(string sMsg)
        {
            try {
                byte[] utf8Data = Encoding.UTF8.GetBytes(sMsg);
                networkStream.Write(utf8Data, 0, utf8Data.Length);
            }
            catch (Exception ex)
            {
                Disconnect();
                return Task.FromException(ex);
            }

            return Task.CompletedTask;
        }

        private async Task StartReceiving()
        {
            byte[] buffer = new byte[1024];
            string utf8Data = string.Empty;

            try
            {
                while (tcpClient != null && tcpClient.Connected)
                {
                    int bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        networkStream?.Close();
                        tcpClient?.Close();
                        utf8Data = "CLOSE_CONNECTION";
                    }
                    else
                    {
                        var receivedBuffer = buffer.Take(bytesRead).ToArray();
                        utf8Data = Encoding.UTF8.GetString(receivedBuffer);
                    }

                    OnDataReceived(utf8Data);
                }
            }
            catch (Exception ex)
            {
                if (tcpClient.Connected)
                {
                    Disconnect();
                }
            }
        }

        protected virtual void OnDataReceived(string data)
        {
            // Raise the event on the UI thread in WPF
            Application.Current.Dispatcher.Invoke(() =>
            {
                DataReceived?.Invoke(this, new PrimaryInEventArgs(data));
            });
        }
    }

    public partial class PrimaryInEventArgs : EventArgs
    {
        public string Data { get; }
        public PrimaryInEventArgs(string data) => Data = data;
    }

    public partial class PrimaryOutEventArgs : EventArgs
    {
        public string Data { get; }
        public PrimaryOutEventArgs(string data) => Data = data;
    }
}
