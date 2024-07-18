using Newtonsoft.Json;
using Project03.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Project03.Controllers
{
    public class ItemController : ApiController
    {
        private SqlConnection conBroker;
        private SqlDataAdapter _adapter;


        private void connectionBroker()
        {
            string constr = ConfigurationManager.AppSettings["SapConnectionB1"].ToString();
            conBroker = new SqlConnection(constr);
        }
        public IEnumerable<OITM> GetItem()
        {
            DataTable _dt = new DataTable();
            var query = "select top 100 ItemCode, ItemName from OITM";
            connectionBroker();
            _adapter = new SqlDataAdapter
            {
                SelectCommand = new SqlCommand(query, conBroker)
            }; ;
            _adapter.Fill(_dt);
            conBroker.Close();
            List<OITM> oitm = new List<OITM>(_dt.Rows.Count);
            if (_dt.Rows.Count > 0)
            {
                foreach (DataRow row_record in _dt.Rows)
                {
                    oitm.Add(new ReadOITMList(row_record));
                }
            }
            return oitm;
        }
        private string AddOITM(string json)
        {
            DataTable dt = null;
            List<OITM> oitm = null;
            if (json == null)
            {
                return "object is null";

            }
            string jsons = json.ToString();
            oitm = JsonConvert.DeserializeObject<List<OITM>>(jsons);
            string ErrMsg = "";
            SAPbobsCOM.Company oCompany;
            if (SAPConnection.Connect())
            {
                oCompany = SAPConnection.SapCompany;
                SAPbobsCOM.Items item;
                oCompany.StartTransaction();
                try
                {
                    foreach (OITM rowLine in oitm)
                    {
                        item = (SAPbobsCOM.Items)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oItems);
                        try
                        {
                            if (item.GetByKey(rowLine.ItemCode))
                            {
                                item.ItemName = rowLine.ItemName;
                                item.Update();

                            }
                            else
                            {
                                item.ItemCode = rowLine.ItemCode;
                                item.ItemType = SAPbobsCOM.ItemTypeEnum.itItems;
                                item.ItemName = rowLine.ItemName;
                                int count = item.Add();
                                ErrMsg = oCompany.GetLastErrorDescription();
                            }
                            if (!oCompany.InTransaction)
                            {
                                oCompany.StartTransaction();
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrMsg = ex.Message;
                            if (!oCompany.InTransaction)
                            {
                                oCompany.StartTransaction();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrMsg = ex.Message;
                }
                if (string.IsNullOrEmpty(ErrMsg) || string.IsNullOrWhiteSpace(ErrMsg))
                {
                    if (oCompany.InTransaction)
                    {
                        try
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit);
                            ErrMsg = "OK";
                        }
                        catch (Exception ex)
                        {
                            ErrMsg = ex.Message;
                        }
                    }
                }
                else
                {
                    if (oCompany.InTransaction)
                    {
                        try
                        {
                            oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack);
                        }
                        catch (Exception ex)
                        {
                            ErrMsg = ex.Message;
                        }
                    }
                }
            }
            else
            {
                ErrMsg = "SAP B1 Connection is failure";
            }
            return ErrMsg;
        }
        [HttpGet]
        [Route("api/item")]
        public HttpResponseMessage GetProductions()
        {
            Globals.GetInformationConfig();
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, GetItem());
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.OK, ex.Message);
            }
        }
        [HttpPost]
        [Route("api/additem")]
        public HttpResponseMessage PostOITM([FromBody] object jsonString)
        {
            string _Mess = "0";

            if (jsonString == null)
            {
                _Mess = "object is null";
            }
            else
            {
                string json = jsonString.ToString();
                _Mess = AddOITM(json);
            }
            if (_Mess.Equals("OK"))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(_Mess, System.Text.Encoding.UTF8, "text/plain");
                return resp;
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest);
                resp.Content = new StringContent(_Mess, System.Text.Encoding.UTF8, "text/plain");
                return resp;
            }
        }
    }
}