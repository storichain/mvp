﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;

namespace Storichain.Controllers
{
    public class BookmarkController : Controller
    {
        Biz_Bookmark biz = new Biz_Bookmark();

        public ActionResult Get() 
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

            DataTable dt = biz.GetBookmarkList( WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetBookmarkUserLists() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) 
            {
                json = DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.GetBookmarkLists(WebUtility.UserIdx(), 
                                                BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                WebUtility.GetRequestByInt("page", 1), 
                                                WebUtility.GetRequestByInt("page_rows", 20),
                                                out total_count,
                                                out page_count,
                                                WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

            if(WebUtility.GetRequest("step_use_yn", "N").Equals("Y")) 
            {
                dt.Columns.Add("step_data", typeof(DataTable));
                dt.Columns.Add("step_data_count", typeof(int));

                Biz_Step biz_step = new Biz_Step();

                foreach(DataRow drEvent in dt.Rows) 
                {
                    int event_idx_1             = drEvent["event_idx"].ToInt();
                    drEvent["step_data"]        = biz_step.GetStep(event_idx_1, 0);
                    drEvent["step_data_count"]  = ((DataTable)drEvent["step_data"]).Rows.Count;
                }
            }

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetBookmarkLists() 
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

            int total_count;
            int page_count;

            DataTable dt = biz.GetBookmarkUserLists(    WebUtility.GetRequestByInt("event_idx"), 
                                                        WebUtility.UserIdx(), 
                                                        WebUtility.GetRequestByInt("page", 1), 
                                                        WebUtility.GetRequestByInt("page_rows", 10),
                                                        out total_count,
                                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Toggle() 
        {
            string json = "";

            try
            {
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

                if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
                {
                    Biz_UserBlock biz_block = new Biz_UserBlock(WebUtility.UserIdx());
                    if(biz_block.Book_mark_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
                    {
                        json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                    else if(biz_block.Enable_YN.Equals("N"))
                    {
                        System.Web.Security.FormsAuthentication.SignOut();
                        json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }    
                }

                int event_idx   = WebUtility.GetRequestByInt("event_idx");
                int user_idx    = WebUtility.UserIdx();

                bool isOK = biz.Toggle( event_idx, 
                                        user_idx, 
                                        DateTime.Now, 
                                        DateTime.Now, 
                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetBookmarkList( event_idx, 
                                                        user_idx);

                    if(WebUtility.GetConfig("NOTICE_YN").Equals("Y")) 
                    {
                        Biz_User biz_user       = new Biz_User(user_idx);
                        Biz_Event biz_event     = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));
                        Biz_Notice biz_notice   = new Biz_Notice();

                        if(dt.Rows.Count > 0) 
                        {
                            if(dt.Rows[0]["marked_yn"].ToString().Equals("Y")) 
                            {
                                biz_notice.SendNotice(  user_idx, 
                                                        biz_event.UserIdx.ToString().Split(','),
                                                        string.Format(WebUtility.GetConfig("MSG_BOOKMARK", "{0} 님이 내 게시물을 찜 하였습니다.") , biz_user.Nick_Name), 
                                                        "1",
                                                        "book_mark",
                                                        0,
                                                        WebUtility.GetRequestByInt("event_idx"),
                                                        0,
                                                        0);
                            }
                            else 
                            {
                                MongoDBCommon.RemoveBookmark(user_idx, biz_event.UserIdx, event_idx);
                            }
                        }
                    }

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




        // =================================================================================================================================================





        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz.AddBookmark(WebUtility.GetRequestByInt("event_idx"), 
                                            WebUtility.UserIdx(), 
                                            WebUtility.GetRequest("marked_yn"), 
                                            DateTime.Now, 
                                            WebUtility.GetDeviceTypeIdx(),
                                            DateTime.Now, 
                                            WebUtility.GetRequestByInt("create_user_idx"));

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
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("marked_yn")))
                message += "marked_yn is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                bool isOK = biz.ModifyBookmark( WebUtility.GetRequestByInt("event_idx"), 
                                                user_idx, 
                                                WebUtility.GetRequest("marked_yn", "N"), 
                                                DateTime.Now, 
                                                DateTime.Now, 
                                                user_idx);

                Biz_Event biz_evnet = new Biz_Event();
                
                DataTable dt = biz_evnet.GetEventListByBookmark(0, user_idx);

                if(isOK)
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

        public ActionResult Remove() 
        {
            string json = "";

            try
            {
                bool isOK = biz.RemoveBookmark( WebUtility.GetRequestByInt("event_idx"), 
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

        
    }
}
