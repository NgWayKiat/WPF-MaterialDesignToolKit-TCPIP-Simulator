using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WpfTemp.Model
{
    internal class ClientSocket
    {
        private Socket socket;
        
        private const int BufferSize = 1024;

        public bool Connect(string sClientName)
        {
            bool isClientFound = false;
            try
            {
                for(int i = 0; i < Global.client.Length; i++)
                {
                    if (Global.client[i].Hostname == sClientName)
                    {
                        isClientFound = true;
                        if (Global.client[i].Status == 2) // Error
                        {
                            Console.WriteLine($"Cannot connect to {sClientName} due to configuration error.");
                            return false;
                        }
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);                    
                        socket.ReceiveTimeout = 5000; // 1 seconds
                        socket.Connect(new IPEndPoint(IPAddress.Parse(Global.client[i].IPAddress), Global.client[i].Port));

                        if (socket.Connected)
                        {
                            Global.client[i].Status = 1; // Connected
                            Console.WriteLine($"Connected to {sClientName} at {Global.client[i].IPAddress}:{Global.client[i].Port}");
                            return true;
                        }
                        else
                        {
                            Global.client[i].Status = 2; // Error
                            Console.WriteLine($"Failed to connect to {sClientName} at {Global.client[i].IPAddress}:{Global.client[i].Port}");
                            return false;
                        }
                    }
                }

                if(!isClientFound)
                {
                    Console.WriteLine($"Client name {sClientName} not found in configuration.");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                return false;
            }
        }

        public string SendAndReceive(string message)
        {
            try
            {
                // Send message
                byte[] sendBuffer = Encoding.UTF8.GetBytes(message);
                socket.Send(sendBuffer);

                // Receive reply
                byte[] recvBuffer = new byte[BufferSize];
                int received = socket.Receive(recvBuffer);
                string reply = Encoding.UTF8.GetString(recvBuffer, 0, received);

                return reply;
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
            finally
            {
                socket?.Shutdown(SocketShutdown.Both);
                socket?.Close();
            }
        }

        public void Close()
        {
            try
            {
                socket?.Shutdown(SocketShutdown.Both);
                socket?.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error closing socket: {ex.Message}");
            }
        }

        public static int sendClientMessage(string sClientName, string message, out string sMsg)
        {
            int ret = 0;
            sMsg = "";

            var client = new ClientSocket();

            if (client.Connect(sClientName))
            {
                sMsg = client.SendAndReceive(message);
                ret = 1;
            }

            return ret;
        }

        public static void InitiateClientsMapping()
        {
            string sValue = "";
            string sTemp = "";
            int iConn = 0;
            int iTemp = 0;
            bool isInt = false;

            //Counting number of connections from App.config file with "MCC118" prefix
            iConn = ConfigurationManager.AppSettings.AllKeys
                           .Where(key => key.StartsWith("MCC118"))
                           .Select(key => ConfigurationManager.AppSettings[key])
                           .Count();

            Global.client = new  Global.ClientInfo[iConn];

            var appSettings = ConfigurationManager.AppSettings;

            //Mapping into structure array
            iConn = 0;
            foreach (var key in appSettings.AllKeys)
            {
                sValue = "";
                iTemp = 0;

                if (key.StartsWith("MCC118"))
                {
                    sValue = appSettings[key].ToString();
                    Global.client[iConn].Hostname = key.ToString();

                    if (sValue == null || sValue == "")
                    {
                        Global.client[iConn].IPAddress = "";
                        Global.client[iConn].Port = 0;
                        Global.client[iConn].Status = 2; // Error
                    }
                    else
                    {
                        if (!sValue.Contains(':'))
                        {
                            Global.client[iConn].IPAddress = "";
                            Global.client[iConn].Port = 0;
                            Global.client[iConn].Status = 2; // Error
                        }
                        else
                        {
                            Global.client[iConn].IPAddress = sValue.Split(':')[0];
                            isInt = int.TryParse(sValue.Split(':')[1], out iTemp);
                            if (isInt)
                            {
                                Global.client[iConn].Port = iTemp;
                                Global.client[iConn].Status = 0; // Not Connected
                            }
                            else
                            {
                                Global.client[iConn].Port = 0;
                                Global.client[iConn].Status = 2; // Error
                            }
                        }
                    }
                    
                    iConn ++;
                }
            }

            //Displaying mapped values from structure array (ClientInfo)
            iConn = Global.client.Count();
            Console.WriteLine($"Total MCC118 connections mapped: {iConn}");
            for (int i = 0; i < iConn; i++)
            {
                Console.WriteLine($"Client {i + 1}: Hostname = {Global.client[i].Hostname}, IP = {Global.client[i].IPAddress}, Port = {Global.client[i].Port}, Status = {Global.client[i].Status}");
            }
        }
    }
}
