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
    public class DeviceController : Controller
    {
        Biz_Device biz_device = new Biz_Device();

        public ActionResult Get() 
        {
            string json = "";

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz_device.AddDevice(   WebUtility.UserIdx(),
                                                    WebUtility.GetRequest("app_name"),
                                                    WebUtility.GetRequest("app_version"),
                                                    WebUtility.GetRequest("device_id"),
                                                    WebUtility.GetRequest("device_token"),
                                                    WebUtility.GetRequest("device_name"),
                                                    WebUtility.GetRequest("device_model"),
                                                    WebUtility.GetRequest("device_design"),
                                                    WebUtility.GetRequest("device_brand"),
                                                    WebUtility.GetRequest("device_manufactor"),
                                                    WebUtility.GetRequest("device_version"),
                                                    WebUtility.GetRequest("push_badge_yn"),
                                                    WebUtility.GetRequest("push_alert_yn"),
                                                    WebUtility.GetRequest("push_sound_yn"),
                                                    WebUtility.GetRequest("device_status_type"),
                                                    WebUtility.GetDeviceTypeIdx(),
                                                    DateTime.Now,
                                                    WebUtility.UserIdx());

                DataTable dt = biz_device.GetDevice(WebUtility.UserIdx(), WebUtility.GetRequest("device_id"), WebUtility.GetRequest("device_token"));

                if(dt.Rows.Count > 0)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Register() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("device_id")))
                message += "device_id is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("device_token")))
                message += "device_token is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("device_version")))
                message += "device_version is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                bool isOK = biz_device.AddDevice(   WebUtility.UserIdx(),
                                                    WebUtility.GetRequest("app_name"),
                                                    WebUtility.GetRequest("app_version"),
                                                    WebUtility.GetRequest("device_id"),
                                                    WebUtility.GetRequest("device_token"),
                                                    WebUtility.GetRequest("device_name"),
                                                    WebUtility.GetRequest("device_model"),
                                                    WebUtility.GetRequest("device_design"),
                                                    WebUtility.GetRequest("device_brand"),
                                                    WebUtility.GetRequest("device_manufactor"),
                                                    WebUtility.GetRequest("device_version"),
                                                    WebUtility.GetRequest("push_badge_yn"),
                                                    WebUtility.GetRequest("push_alert_yn"),
                                                    WebUtility.GetRequest("push_sound_yn"),
                                                    WebUtility.GetRequest("device_status_type"),
                                                    WebUtility.GetDeviceTypeIdx(),
                                                    DateTime.Now,
                                                    WebUtility.UserIdx());

                DataTable dt = biz_device.GetDevice(WebUtility.UserIdx(), WebUtility.GetRequest("device_id"), WebUtility.GetRequest("device_token"));

                if(dt.Rows.Count > 0)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);

                return Content(json, "application/json", System.Text.Encoding.UTF8);
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

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Remove() 
        {
            string json = "";

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
