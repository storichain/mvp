using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;

namespace Storichain.Controllers
{
    public class SupplyController : Controller
    {
        Biz_Supply biz = new Biz_Supply();

        public ActionResult Get() 
        {
            DataTable dt = biz.GetSupply (  WebUtility.GetRequestByInt("supply_idx"), 
                                            WebUtility.GetRequestByInt("event_idx"));
            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz.AddSupply ( WebUtility.GetRequestByInt("event_idx"),
                                            WebUtility.GetRequest("supply_name"),
                                            WebUtility.GetRequest("supply_desc"),
                                            WebUtility.GetRequest("supply_brand_name"),
                                            WebUtility.GetRequest("supply_items"),
                                            WebUtility.GetRequest("supply_location_name"),
                                            WebUtility.GetRequestByFloat("supply_pos_x"),
                                            WebUtility.GetRequestByFloat("supply_pos_y"),
                                            WebUtility.GetRequest("supply_tel"),
                                            WebUtility.GetRequestByInt("supply_price"),
                                            WebUtility.GetRequestByInt("supply_price_origin"),
                                            WebUtility.GetRequestByInt("supply_type1_idx"),
                                            WebUtility.GetRequest("supply_type1_name"),
                                            WebUtility.GetRequestByInt("supply_type2_idx"),
                                            WebUtility.GetRequest("supply_type2_name"),
                                            WebUtility.GetRequest("supply_start_date"),
                                            WebUtility.GetRequest("supply_end_date"),
                                            WebUtility.GetRequest("supply_date"),
                                            WebUtility.GetRequest("supply_url"),
                                            WebUtility.GetRequest("supply_web_title"),
                                            WebUtility.GetRequestByInt("supply_time"),
                                            WebUtility.GetRequestByInt("supply_count"),
                                            WebUtility.GetRequest("supply_text"),
                                            WebUtility.GetRequest("supply_tip"),
                                            WebUtility.GetRequest("supply_origin"),
                                            WebUtility.GetRequest("transaction_yn"),
                                            WebUtility.GetRequestByInt("data_type_idx", 1),
                                            BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, "supply", null, "supply"),
                                            WebUtility.GetDeviceTypeIdx(),
                                            WebUtility.GetRequestByInt("owner_idx"),
                                            DateTime.Now, 
                                            WebUtility.UserIdx());

                if(isOK) 
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Modify() 
        {
            string json = "";

            try
            {
                bool isOK = biz.ModifySupply (  WebUtility.GetRequestByInt("event_idx"),
                                                WebUtility.GetRequest("supply_name"),
                                                WebUtility.GetRequest("supply_desc"),
                                                WebUtility.GetRequest("supply_brand_name"),
                                                WebUtility.GetRequest("supply_items"),
                                                WebUtility.GetRequest("supply_location_name"),
                                                WebUtility.GetRequestByFloat("supply_pos_x"),
                                                WebUtility.GetRequestByFloat("supply_pos_y"),
                                                WebUtility.GetRequest("supply_tel"),
                                                WebUtility.GetRequestByInt("supply_price"),
                                                WebUtility.GetRequestByInt("supply_price_origin"),
                                                WebUtility.GetRequestByInt("supply_type1_idx"),
                                                WebUtility.GetRequest("supply_type1_name"),
                                                WebUtility.GetRequestByInt("supply_type2_idx"),
                                                WebUtility.GetRequest("supply_type2_name"),
                                                WebUtility.GetRequest("supply_start_date"),
                                                WebUtility.GetRequest("supply_end_date"),
                                                WebUtility.GetRequest("supply_date"),
                                                WebUtility.GetRequest("supply_url"),
                                                WebUtility.GetRequestByInt("supply_time"),
                                                WebUtility.GetRequestByInt("supply_count"),
                                                WebUtility.GetRequest("supply_text"),
                                                WebUtility.GetRequest("supply_tip"),
                                                WebUtility.GetRequest("supply_origin"),
                                                WebUtility.GetRequest("transaction_yn"),
                                                WebUtility.GetRequestByInt("data_type_idx", 1),
                                                0,
                                                WebUtility.GetDeviceTypeIdx(),
                                                WebUtility.GetRequestByInt("owner_idx"),
                                                WebUtility.GetRequestByInt("orientation_type_idx", 1),
                                                WebUtility.GetRequestByInt("event_coentent_type_idx", 1),
                                                WebUtility.GetRequestByInt("shop_product_idx", 0),
                                                WebUtility.GetRequest("private_view_yn", ""),
                                                DateTime.Now, 
                                                WebUtility.UserIdx(),
                                                0);

                if(isOK) 
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Remove() 
        {
            string json = "";

            try
            {
                bool isOK = biz.RemoveSupply (WebUtility.GetRequestByInt("supply_idx"));

                if(isOK) 
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
