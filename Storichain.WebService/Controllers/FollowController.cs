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
    public class FollowController : Controller
    {
        Biz_Follow biz = new Biz_Follow();
        private const int recommendation_count = 12;

        public ActionResult GetFollow() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
            //    message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = null;

            if(WebUtility.GetRequestByInt("to_user_idx") == 0)
                dt = biz.GetFollowAll(WebUtility.UserIdx());
            else 
                dt = biz.GetFollow(WebUtility.UserIdx(), 
                                   WebUtility.GetRequestByInt("to_user_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Toggle() 
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

                if(WebUtility.UserIdx() == WebUtility.GetRequestByInt("to_user_idx")) 
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "same idx", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
                {
                    Biz_UserBlock biz_block = new Biz_UserBlock(WebUtility.GetRequestByInt("to_user_idx"));
                    if(biz_block.Follow_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
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

                    biz_block = new Biz_UserBlock(WebUtility.GetRequestByInt("to_user_idx"));
                    if(biz_block.Follow_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
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

                int user_idx     = WebUtility.UserIdx();
                int to_user_idx  = WebUtility.GetRequestByInt("to_user_idx");
                
                bool isOK = biz.Toggle( user_idx, 
                                        to_user_idx, 
                                        DateTime.Now, 
                                        DateTime.Now, 
                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetFollow(user_idx, to_user_idx);

                    if(WebUtility.GetConfig("NOTICE_YN").Equals("Y")) 
                    {
                        Biz_User biz_user       = new Biz_User(user_idx);
                        Biz_Notice biz_notice   = new Biz_Notice();

                        if(dt.Rows.Count > 0) 
                        {
                            if(dt.Rows[0]["follow_yn"].ToString().Equals("Y")) 
                            {
                                biz_notice.SendNotice(  user_idx, 
                                                        to_user_idx.ToString().Split(','),
                                                        string.Format(WebUtility.GetConfig("MSG_FOLLOW", "{0} 나를 팔로우 하였습니다."), biz_user.Nick_Name), 
                                                        "1",
                                                        "follow", 
                                                        0, 
                                                        0,
                                                        0,
                                                        0);
                            }
                            else 
                            {
                                MongoDBCommon.RemovePostByUserID(user_idx, to_user_idx);
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

        public ActionResult ToggleAll() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequest("follow_yn")))
                    message += "follow_yn is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);
                
                int user_idx     = WebUtility.UserIdx();
                string follow_yn = WebUtility.GetRequest("follow_yn");
                
                bool isOK = biz.ToggleAll(  user_idx, 
                                            follow_yn, 
                                            DateTime.Now, 
                                            DateTime.Now, 
                                            user_idx,
                                            recommendation_count);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetFollowAll(user_idx);
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

        public ActionResult GetFriendRecommendLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetFriendRecommendLists( WebUtility.UserIdx(), 
                                                        BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                        1, 
                                                        recommendation_count,
                                                        //WebUtility.GetRequestByInt("page_rows", recommendation_count),
                                                        out total_count,
                                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetFollowingList() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            int user_idx = WebUtility.UserIdx();

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = biz.GetFollowingList(user_idx, WebUtility.GetRequest("follow_yn"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetFollowingLists() 
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

            int user_idx = WebUtility.UserIdx();

            int total_count;
            int page_count;

            DataTable dt = biz.GetFollowingLists(   user_idx, 
                                                    WebUtility.GetRequest("follow_yn"), 
                                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                    WebUtility.GetRequestByInt("page", 1), 
                                                    WebUtility.GetRequestByInt("page_rows", 20),
                                                    out total_count,
                                                    out page_count,
                                                    WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetFollowerList() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz.GetFollowerList(user_idx, WebUtility.GetRequest("follow_yn"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetFollowerLists() 
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

            int user_idx = WebUtility.UserIdx();

            int total_count;
            int page_count;

            DataTable dt = biz.GetFollowerLists(user_idx, 
                                                WebUtility.GetRequest("follow_yn"),
                                                BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                WebUtility.GetRequestByInt("page", 1), 
                                                WebUtility.GetRequestByInt("page_rows", 20),
                                                out total_count,
                                                out page_count,
                                                WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
    }
}
