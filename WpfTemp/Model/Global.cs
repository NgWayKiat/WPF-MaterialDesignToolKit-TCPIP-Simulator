using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfTemp.Model
{
    public static class Global
    {
        //User Credentials
        public static string sUsrName { get; set; } = string.Empty;
        public static string sUsrPassword { get; set; } = string.Empty;
        public static eRoles iUsrRole { get; set; }

        //Client IP and Port
        public static string sDefaultClientIp { get; set; } = "127.0.0.1";
        public static int iDefaultClientPort { get; set; } = 5000;
        public static string sClientIp { get; set; } = "127.0.0.1";
        public static int iClientPort { get; set; } = 5000;

        //DataTable to hold MCC118 Voltage Data
        public static DataTable dtVoltage { get; set; } = new DataTable();

        //Command Codes for MCC118
        public static string CMD0001 = "A0001";
        public static string CMD0002 = "A0002";
        public static string CMD0003 = "A0002";
        public static string CMD0004 = "A0002";
        public static string CMD0005 = "A0002";
        public static string CMD8888 = "A8888";

        public static List<string> ListCMD = new List<string>()
        {
            CMD0001,
            CMD0002,
            CMD0003,
            CMD0004,
            CMD0005,
            CMD8888
        };

        public struct ClientInfo
        {
            public string Hostname;
            public string IPAddress;
            public int Port;
            public int Status; // 0: Not Connected, 1: Connected, 2: Error 
        }

        public static ClientInfo[] client = { };

        public enum eRoles
        {
            [Description("ADMIN")]
            ADM,
            [Description("ENGINEER")]
            ENG,
            [Description("SUPERVISOR")]
            SPR,
            [Description("OPERATOR")]
            OPT
        }

        public enum MessageType
        {
            [Description("INFORMATION")]
            INFO = 0,
            [Description("WARNING")]
            WARN = 1,
            [Description("RECEIVE")]
            RECV = 2,
            [Description("SEND")]
            SEND = 3,
            [Description("ERRORS")]
            ERRS = 4
        }
    }
}
