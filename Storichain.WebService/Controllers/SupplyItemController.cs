using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;

namespace Storichain.Controllers
{
    public class SupplyItemController : Controller
    {
        Biz_SupplyItem biz = new Biz_SupplyItem();

        public ActionResult Get() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetSupplyItem (  WebUtility.GetRequestByInt("supply_item_idx"), 
                                                WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.GetRequestByInt("step_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Add() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("item_name")))
                message += "item_name is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("unit_count")))
                message += "unit_count is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("unit_name")))
                message += "unit_name is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string file_path1 = "supply_item";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                int supply_item_idx = biz.AddSupplyItem (   WebUtility.GetRequestByInt("event_idx"), 
                                                            WebUtility.GetRequestByInt("step_idx"), 
                                                            WebUtility.GetRequest("item_name"), 
                                                            WebUtility.GetRequest("item_desc"), 
                                                            WebUtility.GetRequest("unit_count"), 
                                                            WebUtility.GetRequest("unit_name"), 
                                                            BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                            DateTime.Now, 
                                                            WebUtility.UserIdx());

                DataTable dt = biz.GetSupplyItem (  supply_item_idx, 
                                                    WebUtility.GetRequestByInt("event_idx"), 
                                                    0);

                if(supply_item_idx > 0) 
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
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
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("supply_item_idx")))
                message += "supply_item_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("item_name")))
                message += "item_name is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("unit_count")))
                message += "unit_count is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("unit_name")))
                message += "unit_name is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string file_path1 = "supply_item";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                bool isOK = biz.ModifySupplyItem (  WebUtility.GetRequestByInt("supply_item_idx"), 
                                                    WebUtility.GetRequest("item_name"), 
                                                    WebUtility.GetRequest("item_desc"), 
                                                    WebUtility.GetRequest("unit_count"), 
                                                    WebUtility.GetRequest("unit_name"), 
                                                    BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                    DateTime.Now, 
                                                    WebUtility.UserIdx());

                if(isOK) 
                {
                    DataTable dt = biz.GetSupplyItem (  WebUtility.GetRequestByInt("supply_item_idx"), 
                                                        WebUtility.GetRequestByInt("event_idx"), 
                                                        0);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
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
                bool isOK = biz.RemoveSupplyItem (WebUtility.GetRequestByInt("supply_item_idx"));

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
