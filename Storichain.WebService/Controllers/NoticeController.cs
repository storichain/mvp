using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Text;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Storichain.Controllers
{
    public class NoticeController : Controller
    {
        Biz_Notice biz          = new Biz_Notice();
        Biz_NoticeCheck biz_check = new Biz_NoticeCheck();
        Biz_Device biz_device   = new Biz_Device();
        Biz_User biz_user       = new Biz_User();

        protected override void Initialize( System.Web.Routing.RequestContext rc) 
        {
            base.Initialize(rc);
        }

        public ActionResult Get()
        {
            string json = "";

            DataTable dt = biz.GetNotice(   WebUtility.GetRequestByInt("notice_idx"),
                                            WebUtility.GetRequestByInt("from_user_idx"),
                                            WebUtility.UserIdx(),
                                            WebUtility.GetRequest("notice_status_type"),
                                            WebUtility.GetRequest("notice_biz_type"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetAlarm() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("page")))
                message += "page is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            //if(WorkHelper.isDuplicateWork("Feed/Get", WebUtility.GetRequestByInt("event_idx"), WebUtility.GetRequestByInt("user_idx"), 1))
            //{
            //    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
            //    return Content(json, "application/json", System.Text.Encoding.UTF8);
            //}

            int user_idx = WebUtility.UserIdx();

            MongoDBCommon.CheckFeedBank(user_idx);

            int page        = WebUtility.GetRequestByInt("page");
            int page_rows   = WebUtility.GetRequestByInt("page_rows", 20);
            DateTime? std_date;

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("std_date")))
            { 
                std_date = DateTime.Now;
            }
            else 
            { 
                std_date = DataTypeUtility.GetToDateTime(WebUtility.GetRequest("std_date"));
            }

            var client      = new MongoClient(WebUtility.GetConfig("MONGODB_FEED_URL"));
            var database    = client.GetDatabase(WebUtility.GetConfig("MONGODB_FEED_DB_NAME"));
            var col_feed    = database.GetCollection<Feed>("feeds");

            var f_feed   = Builders<Feed>.Filter;
            var f_feed_u = Builders<Feed>.Update;
            var f_feed_s = Builders<Feed>.Sort;

            var d = col_feed.Find(f_feed.And(f_feed.Eq("user_idx", user_idx), f_feed.Ne("type_name", "new_post"), f_feed.Lt("create_date", std_date)))
                                .Skip((page - 1) * page_rows)
                                .Limit(page_rows)
                                .Sort(f_feed_s.Descending("create_date"))
                                ;

            //.Clone<Feed>()

            Biz_Event biz_event = new Biz_Event();
            Biz_Step biz_step = new Biz_Step();
            Biz_Bookmark biz_mark = new Biz_Bookmark();
            Biz_Follow biz_follow = new Biz_Follow();
            Biz_Like biz_like = new Biz_Like();
            Biz_Report biz_report = new Biz_Report();

            string event_idxs = "";
            string to_user_idxs = "";
            string to_user_idxs_from = "";
            bool isFirst = true;
            bool isFirst_follow = true;
            bool isFirst_follow_from = true;

            foreach(var f in d.ToCursor().ToEnumerable()) 
            {
                if (f.type_name.Equals("new_post") || f.type_name.Equals("book_mark") || f.type_name.Equals("comment") || f.type_name.Equals("like")) 
                {
                    if (isFirst) 
                    { 
                        event_idxs += f.type_event.event_idx.ToString();
                        isFirst = false;
                    }
                    else 
                    {
                        event_idxs += "," + f.type_event.event_idx.ToString();
                    }

                    if (isFirst_follow_from) 
                    { 
                        to_user_idxs_from += f.from_user.user_idx.ToString();
                        isFirst_follow_from = false;
                    }
                    else 
                    {
                        if(!to_user_idxs_from.Split(',').Contains(f.from_user.user_idx.ToString()))
                            to_user_idxs_from += "," + f.from_user.user_idx.ToString();
                    }
                }
                else if (f.type_name.Equals("follow")) 
                {
                    if (isFirst_follow) 
                    { 
                        to_user_idxs += f.type_follow.to_user_idx.ToString();
                        isFirst_follow = false;
                    }
                    else 
                    {
                        to_user_idxs += "," + f.type_follow.to_user_idx.ToString();
                    }
                }
            }

            DataTable dtB = null;
            if(!event_idxs.Equals(""))
                dtB = biz_mark.GetBookmarkCheck(event_idxs, user_idx);

            DataTable dtF = null;
            if(!to_user_idxs.Equals(""))
                dtF = biz_follow.GetFollowCheck(to_user_idxs, user_idx);

            DataTable dtF_from = null;
            if(!to_user_idxs_from.Equals(""))
                dtF_from = biz_follow.GetFollowCheck(to_user_idxs_from, user_idx);

            DataTable dtL = null;
            if(!event_idxs.Equals(""))
                dtL = biz_like.GetLikeCheck(event_idxs, user_idx);

            DataTable dtCount = null;
                dtCount = biz_event.GetEventCount(event_idxs);

            DataTable dtR = null;
                dtR = biz_report.GetEventCheck(event_idxs, user_idx);
            
            DataTable dt = DataTableSchema.GetFeed();
            DataRow dr;

            foreach(var f in d.ToCursor().ToEnumerable()) 
            {
                dr = dt.NewRow();
                dr["from_user"]         = f.from_user;
                dr["user_idx"]          = f.user_idx;
                dr["feed_name"]         = f.feed_name;
                dr["read_yn"]           = f.read_yn;
                dr["type_name"]         = f.type_name;

                if (f.type_name.Equals("new_post") || f.type_name.Equals("book_mark") || f.type_name.Equals("comment") || f.type_name.Equals("like") ) 
                { 
                    Event obj           = f.type_event;

                    if(dtB != null)
                    { 
                        DataRow[] drB = dtB.Select("event_idx=" + obj.event_idx.ToString());
                        if (drB.Length > 0) 
                        { 
                            obj.marked_yn = drB[0]["marked_yn"].ToString();
                        }
                        else 
                        {
                            obj.marked_yn = "";
                        }
                    }
                    else
                    {
                        obj.marked_yn = "";
                    }

                    //if(dtF != null)
                    //{ 
                    //    DataRow[] drF = dtF.Select("to_user_idx=" + f.from_user.user_idx.ToString());
                    //    if (drF.Length > 0) 
                    //    { 
                    //        obj.follow_yn = drF[0]["follow_yn"].ToString();
                    //    }
                    //    else 
                    //    {
                    //        obj.follow_yn = "";
                    //    }
                    //}
                    //else
                    //{
                    //    obj.follow_yn = "";
                    //}

                    if(dtF_from != null)
                    { 
                        DataRow[] drF_from = dtF_from.Select("to_user_idx=" + f.from_user.user_idx.ToString());
                        if (drF_from.Length > 0) 
                        { 
                            obj.follow_yn = drF_from[0]["follow_yn"].ToString();
                        }
                        else 
                        {
                            obj.follow_yn = "";
                        }
                    }
                    else
                    {
                        obj.follow_yn = "";
                    }

                    if(dtL != null)
                    { 
                        DataRow[] drL = dtL.Select("event_idx=" + obj.event_idx.ToString());
                        if (drL.Length > 0) 
                        { 
                            DataRow drRowL = drL[0];

                            int like_type_idx   = drRowL["like_type_idx"].ToInt();
                            string like_yn      = drRowL["like_yn"].ToValue();

                            if(like_type_idx == 1) 
                            {
                                obj.like_yn = "Y";
                                obj.hate_yn = "N";
                            }
                            else if(like_type_idx == 2) 
                            {
                                obj.like_yn = "N";
                                obj.hate_yn = "Y";
                            }
                            else 
                            {
                                obj.like_yn = "";
                                obj.hate_yn = "";        
                            }
                        }
                        else 
                        {
                            obj.like_yn = "";
                            obj.hate_yn = "";
                        }
                    }
                    else
                    {
                        obj.like_yn = "";
                        obj.hate_yn = "";
                    }

                    if(dtR != null)
                    { 
                        DataRow[] drR = dtR.Select("event_idx=" + obj.event_idx.ToString());
                        if (drR.Length > 0) 
                        { 
                            obj.report_event_yn = drR[0]["report_yn"].ToString();
                        }
                        else 
                        {
                            obj.report_event_yn = "";
                        }
                    }
                    else
                    {
                        obj.report_event_yn = "";
                    }

                    if(dtCount != null) 
                    {
                        DataRow[] drCount = dtCount.Select("event_idx=" + obj.event_idx.ToString());
                        if (drCount.Length > 0) 
                        { 
                            DataRow drRowCount = drCount[0];

                            obj.marked_count    = drRowCount["bookmark_count"].ToInt();
                            obj.like_count      = drRowCount["like_count"].ToInt();
                            obj.hate_count      = drRowCount["hate_count"].ToInt();
                            obj.comment_count   = drRowCount["comment_count"].ToInt();
                            obj.read_count      = drRowCount["read_count"].ToInt();
                        }
                        else 
                        {
                            obj.marked_count    = 0;
                            obj.like_count      = 0;
                            obj.hate_count      = 0;
                            obj.comment_count   = 0;
                            obj.read_count      = 0;
                        }
                    }
                    else
                    {
                        obj.marked_count    = 0;
                        obj.like_count      = 0;
                        obj.hate_count      = 0;
                        obj.comment_count   = 0;
                        obj.read_count      = 0;
                    }

                    dr["type_event"]    = obj;
                }
                else if (f.type_name.Equals("follow")) 
                { 
                    Follow obj           = f.type_follow;
                    
                    if(dtF != null)
                    { 
                        DataRow[] drF = dtF.Select("to_user_idx=" + obj.to_user_idx.ToString());
                        if (drF.Length > 0) 
                        { 
                            obj.follow_yn = drF[0]["follow_yn"].ToString();
                        }
                        else 
                        {
                            obj.follow_yn = "";
                        }
                    }
                    else
                    {
                        obj.follow_yn = "";
                    }

                    dr["type_follow"]    = obj;
                }
                
                dr["type_comment"]      = f.type_comment;
                //dr["type_recommend"]    = f.type_recommend;
                dr["create_date"]       = f.create_date.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", 10000000);
            dic.Add("page_count", 10000000/page_rows);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetNoticeLists()
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetNoticeLists(  WebUtility.GetRequestByInt("notice_idx"),
                                                WebUtility.GetRequestByInt("from_user_idx"),
                                                WebUtility.GetRequestByInt("user_idx"),
                                                WebUtility.GetRequest("notice_status_type"),
                                                WebUtility.GetRequest("notice_biz_type"),
                                                BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                WebUtility.GetRequestByInt("page", 1), 
                                                WebUtility.GetRequestByInt("page_rows", 18),
                                                out total_count,
                                                out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Count()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int badge = biz.GetCount(WebUtility.GetRequestByInt("to_user_idx"), "1");
            json = DataTypeUtility.JSon("1000", "", Config.R_SUCCESS, badge.ToString());
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadList()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetNoticeByFromIDX(WebUtility.GetRequestByInt("to_user_idx"), "1");

            DataTable dtNot = new DataTable();
            DataRow drNot;
            dtNot.Columns.Add("user_idx", typeof(int));
            dtNot.Columns.Add("notice_count", typeof(int));
            dtNot.Columns.Add("notice_message", typeof(DataTable));

            foreach(DataRow dr in dt.Rows) 
            {
                drNot = dtNot.NewRow();
                drNot["user_idx"]       = dr["user_idx"];
                drNot["notice_count"]   = dr["notice_count"];
                drNot["notice_message"] = biz.GetNotice(0,
                                                        (int)dr["user_idx"],
                                                        WebUtility.GetRequestByInt("to_user_idx"),
                                                        "1",
                                                        "");
                dtNot.Rows.Add(drNot);
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtNot);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadCount()
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int user_idx = WebUtility.GetRequestByInt("to_user_idx");
            int count = biz_check.GetNoticeCount(user_idx, "N");

            Dictionary<string, int> dic = new Dictionary<string,int>();
            dic.Add("notice_count", count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Send()
        {
            string json = "";

            int user_idx            = WebUtility.UserIdx();
            string[] strIDXs        = WebUtility.GetRequest("to_user_idx").Split(',');
            string notice_msg       = WebUtility.GetRequest("notice_message");
            string notice_type      = WebUtility.GetRequest("notice_status_type");
            string notice_biz_type  = WebUtility.GetRequest("notice_biz_type");
            
            json = biz.SendNotice(  user_idx, 
                                    strIDXs, 
                                    notice_msg, 
                                    notice_type, 
                                    notice_biz_type,
                                    0,
                                    0,
                                    0,
                                    0);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Modify()
        {
            string json = "";

            bool isOK = false;

            string[] n_idxs = WebUtility.GetRequest("notice_idx").Split(',');

            foreach(string m_idx in n_idxs) 
            {
                isOK = biz.ModifyNoticeStatusType(  DataTypeUtility.GetToInt32(m_idx),
                                                    WebUtility.GetRequest("notice_status_type"),
                                                    DateTime.Now);
            }                

            if(isOK) 
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            else
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult AllRead() 
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

            try
            {
                int affected = MongoDBCommon.ReadAlarmOK(user_idx);
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, affected.ToString() + " Rows", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //public ActionResult AllRead()
        //{
        //    string json = "";
        //    string message  = "";

        //    if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
        //        message += "user_idx is null.\n";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    int user_idx = WebUtility.UserIdx();
        //    bool isOK = false;

        //    isOK = biz_check.AllView(user_idx, "Y", DateTime.Now, DateTime.Now, user_idx);
        //    int count = biz_check.GetNoticeCount(user_idx, "N");

        //    Dictionary<string, int> dic = new Dictionary<string,int>();
        //    dic.Add("notice_count", count);

        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}
    }
}
