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
        public struct ClientInfo
        {
            public string Hostname;
            public string IPAddress;
            public int Port;
            public int Status; // 0: Not Connected, 1: Connected, 2: Error 
        }

        private Socket socket;
        public static ClientInfo[] client;
        private const int BufferSize = 1024;

        public bool Connect(string sClientName)
        {
            bool isClientFound = false;
            try
            {
                for(int i = 0; i < client.Length; i++)
                {
                    if (client[i].Hostname == sClientName)
                    {
                        isClientFound = true;
                        if (client[i].Status == 2) // Error
                        {
                            Console.WriteLine($"Cannot connect to {sClientName} due to configuration error.");
                            return false;
                        }
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);                    
                        socket.ReceiveTimeout = 5000; // 1 seconds
                        socket.Connect(new IPEndPoint(IPAddress.Parse(client[i].IPAddress), client[i].Port));

                        if (socket.Connected)
                        {
                            client[i].Status = 1; // Connected
                            Console.WriteLine($"Connected to {sClientName} at {client[i].IPAddress}:{client[i].Port}");
                            return true;
                        }
                        else
                        {
                            client[i].Status = 2; // Error
                            Console.WriteLine($"Failed to connect to {sClientName} at {client[i].IPAddress}:{client[i].Port}");
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

            client = new ClientInfo[iConn];

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
                    client[iConn].Hostname = key.ToString();

                    if (sValue == null || sValue == "")
                    {
                        client[iConn].IPAddress = "";
                        client[iConn].Port = 0;
                        client[iConn].Status = 2; // Error
                    }
                    else
                    {
                        if (!sValue.Contains(':'))
                        {
                            client[iConn].IPAddress = "";
                            client[iConn].Port = 0;
                            client[iConn].Status = 2; // Error
                        }
                        else
                        {
                            client[iConn].IPAddress = sValue.Split(':')[0];
                            isInt = int.TryParse(sValue.Split(':')[1], out iTemp);
                            if (isInt)
                            {
                                client[iConn].Port = iTemp;
                                client[iConn].Status = 0; // Not Connected
                            }
                            else
                            {
                                client[iConn].Port = 0;
                                client[iConn].Status = 2; // Error
                            }
                        }
                    }
                    
                    iConn ++;
                }
            }

            //Displaying mapped values from structure array (ClientInfo)
            iConn = client.Count();
            Console.WriteLine($"Total MCC118 connections mapped: {iConn}");
            for (int i = 0; i < iConn; i++)
            {
                Console.WriteLine($"Client {i + 1}: Hostname = {client[i].Hostname}, IP = {client[i].IPAddress}, Port = {client[i].Port}, Status = {client[i].Status}");
            }
        }
    }
}
