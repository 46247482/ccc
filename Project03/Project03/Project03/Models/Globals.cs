using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Project03.Models
{
    public class Globals
    {
        private static string path;
        public static bool checkCopy { get; set; }
        public static string connectionStringB1 { get; set; }
        private static SqlConnection conBroker;
        private static SqlDataAdapter _adapter;
        public static bool GetInformationConfig()
        {
            try
            {
                connectionStringB1 = ConfigurationManager.AppSettings["SapConnectionB1"].ToString();
                return true;
            }
            catch { return false; }
        }
        private static void connectionBroker()
        {
            string constr = ConfigurationManager.AppSettings["SapConnectionB1"].ToString();
            conBroker = new SqlConnection(constr);
        }
    }
}