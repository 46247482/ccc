using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Project03.Models
{
    public class OITM
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
    }
    public class ReadOITMList : OITM
    {
        public ReadOITMList(DataRow row)
        {
            ItemCode = row["ItemCode"].ToString();
            ItemName = row["ItemName"].ToString();
        }
    }
}