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
	public class FollowChannelController : Controller
	{
		Biz_FollowChannel biz = new Biz_FollowChannel();

		public ActionResult GetFollow() 
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

            DataTable dt = null;

            if(WebUtility.GetRequestByInt("channel_idx") == 0)
                dt = biz.GetFollowAll( WebUtility.UserIdx());
            else 
                dt = biz.GetFollow(WebUtility.UserIdx(),
                                    WebUtility.GetRequestByInt("channel_idx"));

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

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
                    message += "channel_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx      = WebUtility.UserIdx();
                int channel_idx   = WebUtility.GetRequestByInt("channel_idx");

                bool isOK = biz.Toggle( user_idx, 
                                        channel_idx, 
                                        DateTime.Now,
                                        DateTime.Now, 
                                        user_idx);

                if(isOK) 
                {
                    DataTable dt = biz.GetFollow(user_idx, channel_idx);

                    if(dt.Rows.Count > 0) 
                    {
                        if(dt.Rows[0]["follow_yn"].ToString().Equals("Y")) 
                        {
                            MongoDBCommon.FollowChannelYes(user_idx, channel_idx);
                        }
                        else 
                        {
                            MongoDBCommon.FollowChannelNo(user_idx, channel_idx);
                        }
                    
                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                    }
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

        public ActionResult GetChannelRecommendLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetChannelRecommendLists(WebUtility.UserIdx(), 
                                                        BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                        WebUtility.GetRequestByInt("page", 1), 
                                                        WebUtility.GetRequestByInt("page_rows", 10),
                                                        out total_count,
                                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetFollowChannelLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetFollowChannelLists(WebUtility.UserIdx(), 
                                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                    WebUtility.GetRequestByInt("page", 1), 
                                                    WebUtility.GetRequestByInt("page_rows", 10),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }
	}
}

