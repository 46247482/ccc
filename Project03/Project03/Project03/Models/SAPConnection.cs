using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Project03.Models
{
    public static class SAPConnection
    {

        private static SAPbobsCOM.Company company;
        public static SAPbobsCOM.Company SapCompany { get { return company; } }
        private static SAPbobsCOM.Company company1;
        public static SAPbobsCOM.Company SapCompany1 { get { return company1; } }
        public static bool Connect()
        {
            company = new SAPbobsCOM.Company(); ;
            company.Server = ConfigurationManager.AppSettings[@"MSI\LOCAL"].ToString();
            company.LicenseServer = ConfigurationManager.AppSettings["MSI:40000"].ToString();
            company.CompanyDB = ConfigurationManager.AppSettings["DB_Golive_10"].ToString();
            company.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            company.DbUserName = ConfigurationManager.AppSettings["sa"].ToString();
            company.DbPassword = ConfigurationManager.AppSettings["123456"].ToString();
            company.language = SAPbobsCOM.BoSuppLangs.ln_English;
            company.UseTrusted = false;
            company.UserName = ConfigurationManager.AppSettings["manager"].ToString();
            company.Password = ConfigurationManager.AppSettings["sapb11"].ToString();

            if (company.Connected == true)
                return true;
            else
            {
                if (company.Connect() != 0)
                    return false;
            }
            return true;
        }
        public static bool Connect(string user, string pass)
        {
            company1 = new SAPbobsCOM.Company(); ;
            company1.Server = ConfigurationManager.AppSettings[@"MSI\LOCAL"].ToString();
            company1.LicenseServer = ConfigurationManager.AppSettings["MSI:40000"].ToString();
            company1.CompanyDB = ConfigurationManager.AppSettings["DB_Golive_10"].ToString();
            company1.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
            company1.DbUserName = ConfigurationManager.AppSettings["sa"].ToString();
            company1.DbPassword = ConfigurationManager.AppSettings["123456"].ToString();
            company1.language = SAPbobsCOM.BoSuppLangs.ln_English;
            company1.UseTrusted = false;
            company1.UserName = user;
            company1.Password = pass;
            int i = 0;
            if (company1.Connected == true)
                return true;
            else
            {
                i = company1.Connect();
                if (i != 0)
                    return false;
            }
            return true;
        }
    }
}