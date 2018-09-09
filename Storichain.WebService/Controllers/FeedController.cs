using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Storichain.Models;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Storichain.Controllers
{
    public class FeedController : Controller
    {
        public ActionResult Get() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            //if(WorkHelper.isDuplicateWork("Feed/Get", WebUtility.GetRequestByInt("event_idx"), WebUtility.GetRequestByInt("user_idx"), 1))
            //{
            //    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
            //    return Content(json, "application/json", System.Text.Encoding.UTF8);
            //}

            int user_idx = WebUtility.UserIdx();

            Biz_User biz_user = new Biz_User(user_idx);
            DateTime create_date = biz_user.Create_Date;

            if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
            {
                MongoDBCommon.CheckFeedBank(user_idx);
            }
            else
            {
                MongoDBCommon.CheckFeedBankPostForAllUser(user_idx, create_date);
            }

            MongoDBCommon.CheckFeedBankBoardForAllUser(user_idx, create_date);
            
            int page        = WebUtility.GetRequestByInt("page");
            int page_rows   = WebUtility.GetRequestByInt("page_rows", 10);
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

            var f_feed      = Builders<Feed>.Filter;
            var f_feed_u    = Builders<Feed>.Update;
            var f_feed_s    = Builders<Feed>.Sort;

            //var d = col_feed.Find(Query.And(Query.EQ("user_idx", user_idx), Query.EQ("type_name", "new_post"), Query.LT("create_date", std_date)))
            //                    .SetSkip((page - 1) * page_rows)
            //                    .SetLimit(page_rows)
            //                    .SetSortOrder(SortBy.Descending("create_date")).Clone<Feed>();

            var d = col_feed.Find(f_feed.And(f_feed.Eq("user_idx", user_idx), f_feed.Lt("create_date", std_date)))
                                .Skip((page - 1) * page_rows)
                                .Limit(page_rows)
                                .Sort(f_feed_s.Descending("create_date"));

            Biz_Event biz_event     = new Biz_Event();
            Biz_Step biz_step       = new Biz_Step();
            Biz_Bookmark biz_mark   = new Biz_Bookmark();
            Biz_Follow biz_follow   = new Biz_Follow();
            Biz_Like biz_like       = new Biz_Like();
            Biz_Report biz_report   = new Biz_Report();
            //Biz_User biz_user       = new Biz_User();

            string event_idxs        = "";
            string event_user_idxs   = "";
            string to_user_idxs_from = "";
            string to_user_idxs      = "";
            bool isFirst             = true;
            bool isFirst_user        = true;
            bool isFirst_follow_from = true;
            bool isFirst_follow      = true;

            //foreach(var f in d.ToCursor().ToEnumerable()) 
            foreach(var f in d.ToCursor().ToEnumerable()) 
            {
                if (f.type_name.Equals("new_post") || 
                    f.type_name.Equals("book_mark") || 
                    f.type_name.Equals("comment") || 
                    f.type_name.Equals("comment_product") || 
                    f.type_name.Equals("product") || 
                    f.type_name.Equals("like") ||
                    f.type_name.Equals("notice_board") ||
                    f.type_name.Equals("event_board") ||
                    f.type_name.Equals("coupon"))
                {
                    if(f.type_event != null)
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

                    if(f.type_event != null)
                    {
                        if (isFirst_user) 
                        { 
                            event_user_idxs += f.type_event.user_idx.ToString();
                            isFirst_user = false;
                        }
                        else 
                        {
                            if(!event_user_idxs.Split(',').Contains(f.type_event.user_idx.ToString()))
                                event_user_idxs += "," + f.type_event.user_idx.ToString();
                        }
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

            DataTable dtUserLevel = null;
                dtUserLevel = biz_user.GetUserLevelCheck(event_user_idxs);
            
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




                    if(dtUserLevel != null)
                    { 
                        DataRow[] drUserLevel = dtUserLevel.Select("user_idx=" + f.type_event.user_idx.ToString());
                        if (drUserLevel.Length > 0) 
                        { 
                            obj.user_level_idx  = drUserLevel[0]["user_level_idx"].ToInt();
                            obj.user_level_name = drUserLevel[0]["user_level_name"].ToValue();

                            List<ImageObject> list = new List<ImageObject>();

                            if(drUserLevel[0]["user_level_file_idx"].ToInt() > 0) 
                            {
                                Biz_File biz_f = new Biz_File();

                                foreach (DataRow dr1 in biz_f.GetFile(drUserLevel[0]["user_level_file_idx"].ToInt()).Rows)
                                { 
                                    ImageObject imageO  = new ImageObject();
                                    imageO.image_idx    = DataTypeUtility.GetToInt32(dr1["image_idx"]);
                                    imageO.image_seq    = DataTypeUtility.GetToInt32(dr1["image_seq"]);
                                    imageO.image_id     = DataTypeUtility.GetValue(dr1["image_id"]);
                                    imageO.image_url    = DataTypeUtility.GetValue(dr1["image_url"]);
                                    imageO.image_width  = DataTypeUtility.GetToInt32(dr1["image_width"]);
                                    imageO.image_height = DataTypeUtility.GetToInt32(dr1["image_height"]);
                                    imageO.image_key    = DataTypeUtility.GetValue(dr1["image_key"]);
                                    imageO.mime_type    = DataTypeUtility.GetValue(dr1["mime_type"]);
                                    list.Add(imageO);
                                }       
                            }

                            obj.user_level_images = list;
                            obj.user_level_image_count = 0;
                        }
                        else 
                        {
                            obj.user_level_idx  = 0;
                            obj.user_level_name = "";
                            obj.user_level_image_count = 0;
                        }
                    }
                    else
                    {
                        obj.user_level_idx  = 0;
                        obj.user_level_name = "";
                        obj.user_level_image_count = 0;
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

                dr["type_product"]          = f.type_product;
                dr["type_comment"]          = f.type_comment;
                dr["type_comment_product"]  = f.type_comment_product;
                dr["type_notice_board"]     = f.type_notice_board;
                dr["type_event_board"]      = f.type_event_board;
                dr["type_coupon"]           = f.type_coupon;

                //dr["type_recommend"]    = f.type_recommend;
                dr["create_date"]       = f.create_date.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss");
                dt.Rows.Add(dr);
            }

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", 10000000);
            dic.Add("page_count", 10000000/page_rows);

            //int total_count;
            //int page_count;

            //추천과 피드
            //Biz_Follow biz_u = new Biz_Follow();
            //DataTable dtUser = biz_u.GetFriendRecommendLists(   WebUtility.UserIdx(), 
            //                                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
            //                                                    WebUtility.GetRequestByInt("page", 1), 
            //                                                    WebUtility.GetRequestByInt("page_rows", 5),
            //                                                    out total_count,
            //                                                    out page_count);

            //Dictionary<string,object> dic1 = new System.Collections.Generic.Dictionary<string,object>();
            //dic1.Add("event", dt);
            //dic1.Add("user", dtUser);

            //json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic1, dic);

            // 피드만
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
                {
                    int affected = MongoDBCommon.ReadOK(user_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, affected.ToString() + " Rows", null);
                }
                else
                {
                    MongoDBCommon.CheckFeedBankPostForAllUser(user_idx, create_date);
                    MongoDBCommon.CheckFeedBankBoardForAllUser(user_idx, create_date);
                    MongoDBCommon.ReadOK(user_idx);

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }                
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NoticeBoardRead() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("Y"))
                {
                    MongoDBCommon.CheckFeedBankNoticeBoardForAllUser(user_idx, create_date);
                }
                
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult EventBoardRead() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("Y"))
                {
                    MongoDBCommon.CheckFeedBankEventBoardForAllUser(user_idx, create_date);
                }
                
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadAllCount() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("total_not_read_count", typeof(int));
                dt.Columns.Add("post_not_read_count", typeof(int));
                dt.Columns.Add("notice_not_read_count", typeof(int));
                dt.Columns.Add("event_not_read_count", typeof(int));
                dt.Columns.Add("other_not_read_count", typeof(int));
                dt.Columns.Add("date", typeof(DateTime));
                dr = dt.NewRow();
                dr["date"] = DateTime.Now;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
                {    
                    dr["total_not_read_count"]  =  0;
                    dr["post_not_read_count"]   =  0;
                    dr["notice_not_read_count"] =  0;
                    dr["event_not_read_count"]  =  0;
                    dr["other_not_read_count"]  =  0;
                }
                else
                {
                    int post_not_read_count     = MongoDBCommon.GetCountNotReadPostForAllUser(user_idx, create_date);
                    int notice_not_read_count   = MongoDBCommon.GetCountNotReadNoticeBoardForAllUser(user_idx, create_date);
                    int event_not_read_count    = MongoDBCommon.GetCountNotReadEventBoardForAllUser(user_idx, create_date);
                    int other_not_read_count    = MongoDBCommon.GetCountNotReadOthers(user_idx);
                    int all_not_read_count      = post_not_read_count + notice_not_read_count + event_not_read_count + other_not_read_count;

                    dr["total_not_read_count"]  =  all_not_read_count;
                    dr["post_not_read_count"]   =  post_not_read_count;
                    dr["notice_not_read_count"] =  notice_not_read_count;
                    dr["event_not_read_count"]  =  event_not_read_count;
                    dr["other_not_read_count"]  =  other_not_read_count;
                }   
                
                dt.Rows.Add(dr);
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadNoticeBoardCount() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("not_read_count", typeof(int));
                dt.Columns.Add("date", typeof(DateTime));
                dr = dt.NewRow();
                dr["date"] = DateTime.Now;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
                {    
                    dr["not_read_count"] =  0;
                }
                else
                {
                    dr["not_read_count"] =  MongoDBCommon.GetCountNotReadNoticeBoardForAllUser(user_idx, create_date);
                }   
                
                dt.Rows.Add(dr);
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult NotReadEventBoardCount() 
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                Biz_User biz_user = new Biz_User(user_idx);
                DateTime create_date = biz_user.Create_Date;

                DataTable dt = new DataTable();
                DataRow dr = null;
                dt.Columns.Add("not_read_count", typeof(int));
                dt.Columns.Add("date", typeof(DateTime));
                dr = dt.NewRow();
                dr["date"] = DateTime.Now;

                if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
                {    
                    dr["not_read_count"] =  0;
                }
                else
                {
                    dr["not_read_count"] =  MongoDBCommon.GetCountNotReadEventBoardForAllUser(user_idx, create_date);
                }   
                
                dt.Rows.Add(dr);
                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        //public ActionResult CreateRecommendUser() 
        //{
        //    string json = "";
        //    //string message = "";

        //    //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
        //    //    message += "user_idx is null.\n";

        //    //if(!BizUtility.ValidCheck(WebUtility.GetRequest("std_date")))
        //    //    message += "std_date is null.\n";

        //    //if(!message.Equals(""))
        //    //{
        //    //    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //    //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    //}

        //    try
        //    {
        //    //    int affected = MongoDBCommon.ReadOK(WebUtility.GetRequestByInt("user_idx"), 
        //    //                                        DataTypeUtility.GetToDateTime(WebUtility.GetRequest("std_date")));

        //    //    if(affected > 0) 
        //    //        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //    //    else
        //            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
        //    }
        //    catch(Exception ex) 
        //    {
        //        BizUtility.SendErrorLog(Request, ex);
        //        json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
        //    }

        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}
    }
}