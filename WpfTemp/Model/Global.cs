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
        public static string sUsrName { get; set; }
        public static string sUsrPassword { get; set; }
        public static eRoles iUsrRole { get; set; }

        //Client IP and Port
        public static string sClientIp { get; set; }
        public static int iClientPort { get; set; }

        //DataTable to hold MCC118 Voltage Data
        public static DataTable dtVoltage { get; set; }

        //Command Codes for MCC118
        public static string CMD0001 = "A0001";
        public static string CMD0002 = "A0002";
        public static string CMD0003 = "A0002";
        public static string CMD0004 = "A0002";
        public static string CMD0005 = "A0002";
        public static string CMD8888 = "A8888";

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
            [Description("ERRORS")]
            ERRS = 3
        }
    }
}
