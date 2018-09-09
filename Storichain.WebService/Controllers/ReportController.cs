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
    public class ReportController : Controller
    {
        private Biz_Report biz = new Biz_Report();

        public ActionResult GetComment() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("comment_idx")))
                message += "comment_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();
            DataTable dt = biz.GetComment(  user_idx,
                                            WebUtility.GetRequestByInt("comment_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetEvent() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();
            DataTable dt = biz.GetEvent(user_idx,
                                        WebUtility.GetRequestByInt("event_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetUser() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();
            DataTable dt = biz.GetUser(user_idx, 
                                       WebUtility.GetRequestByInt("to_user_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ToggleComment() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("comment_idx")))
                    message += "comment_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int comment_idx = WebUtility.GetRequestByInt("comment_idx");
                int user_idx    = WebUtility.UserIdx();

                bool isOK = biz.ToggleComment(  user_idx, 
                                                comment_idx, 
                                                DateTime.Now, 
                                                WebUtility.GetRequestByInt("report_reason_type_idx"),
                                                WebUtility.GetRequest("report_reason_desc"),
                                                WebUtility.GetDeviceTypeIdx(),
                                                DateTime.Now, 
                                                user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetComment(user_idx, comment_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
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

        public ActionResult ToggleEvent() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                    message += "event_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx    = WebUtility.UserIdx();
                int event_idx   = WebUtility.GetRequestByInt("event_idx");

                
                bool isOK = biz.ToggleEvent(user_idx,
                                            event_idx, 
                                            DateTime.Now, 
                                            WebUtility.GetRequestByInt("report_reason_type_idx"),
                                            WebUtility.GetRequest("report_reason_desc"),
                                            WebUtility.GetDeviceTypeIdx(),
                                            DateTime.Now, 
                                            user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetEvent(user_idx, event_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
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

        public ActionResult ToggleUser()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                    message += "to_user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx    = WebUtility.UserIdx();
                int to_user_idx = WebUtility.GetRequestByInt("to_user_idx");

                bool isOK = biz.ToggleUser( user_idx, 
                                            to_user_idx, 
                                            DateTime.Now, 
                                            WebUtility.GetRequestByInt("report_reason_type_idx"),
                                            WebUtility.GetRequest("report_reason_desc"),
                                            WebUtility.GetDeviceTypeIdx(),
                                            DateTime.Now, 
                                            user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetUser(user_idx, to_user_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
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
    }
}
