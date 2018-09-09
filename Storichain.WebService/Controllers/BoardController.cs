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
    public class BoardController : Controller
    {
        Biz_Board biz = new Biz_Board();
        Biz_BannerPopup biz_p = new Biz_BannerPopup();
        Biz_BoardCheck biz_check = new Biz_BoardCheck();

        public ActionResult GetPopupNotice() 
        {
            string json     = "";
            string message  = "";

            if((!BizUtility.ValidCheck(WebUtility.UserIdx())) && (!BizUtility.ValidCheck(WebUtility.GetConfig("WEB_TMP_USER_IDX", "0"))))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz_p.GetPopup(user_idx);

            Biz_HitLog biz_h = new Biz_HitLog();

            foreach(DataRow dr in dt.Rows)
            {
                //dr["banner_popup_url"] = "http://" + Request.Url.Host + "/Board/GetUrl";

                int log_idx = biz_h.AddLog("full_popup_view_all", 
                                            user_idx, 
                                            dr["banner_popup_idx"].ToInt(),
                                            Request.UserAgent,
                                            WebUtility.GetIpAddress(),
                                            DateTime.Now);
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //public ActionResult GetUrl()
        //{
        //    string json = "";
        //    string message = "";

        //    //if (!BizUtility.ValidCheck(WebUtility.GetRequest("key")))
        //    //    message += "key is null.\n";

        //    if (!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    return Redirect("beautytalk://event_board_view?board_idx=349");
        //}

        public ActionResult PopupReadOK() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_popup_idx")))
                message += "banner_popup_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                bool isOK = biz_p.ReadOK(WebUtility.GetRequestByInt("banner_popup_idx"), user_idx);

                if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

        public ActionResult ToouchViewLog()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_popup_idx")))
                message += "banner_popup_idx is null.\n";

            if ((!BizUtility.ValidCheck(WebUtility.UserIdx())) && (!BizUtility.ValidCheck(WebUtility.GetConfig("WEB_TMP_USER_IDX", "0"))))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                Biz_HitLog biz_h = new Biz_HitLog();

                int log_idx = biz_h.AddLog("full_popup_all_click",
                                            user_idx,
                                            WebUtility.GetRequestByInt("banner_popup_idx"),
                                            Request.UserAgent,
                                            WebUtility.GetIpAddress(),
                                            DateTime.Now);

                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult DeleteCheck() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                bool isOK = biz_p.RemovePopupCheck(user_idx);

                if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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


        public ActionResult GetNotice() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz.GetBoardByIdx(1, user_idx);
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetList() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("board_idx")))
                message += "board_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = biz.GetList(WebUtility.GetRequestByInt("board_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetBoardLists() 
        {
            int total_count;
            int page_count;

            string ing_yn = "";

            //if(WebUtility.GetRequestByInt("board_type_idx", 1) == 1)
            //{
            //    ing_yn = "Y";
            //}

            DataTable dt = biz.GetLists(WebUtility.GetRequestByInt("board_type_idx", 1), 
                                        DateTime.Now,
                                        "Y",
                                        WebUtility.GetRequest("ing_yn", ing_yn),
                                        WebUtility.GetRequestByInt("page", 1), 
                                        WebUtility.GetRequestByInt("page_rows", 18),
                                        out total_count,
                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", BizUtility.GetImageData(dt), dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
            //return Content(JsonConvert.SerializeObject(dt), "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ReadOK() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                bool isOK = biz_check.ReadOK(user_idx);

                if(isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

        public ActionResult ShareSNS()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("board_idx")))
                message += "board_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
                message += "sns_type_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_UserSNSShare biz = new Biz_UserSNSShare();
                bool isOK = biz.AddUserSSNSShareBoard(  WebUtility.UserIdx(),
                                                        WebUtility.GetRequestByInt("board_idx"),
                                                        WebUtility.GetRequestByInt("sns_type_idx"),
                                                        DateTime.Now,
                                                        WebUtility.UserIdx());

                if (isOK)
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch (Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
