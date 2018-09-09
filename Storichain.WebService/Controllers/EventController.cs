using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Storichain.Models;
using Newtonsoft.Json;
using System.Text;
using System.Globalization;

namespace Storichain.Controllers
{
    public class EventController : Controller
    {
        Biz_Event biz                       = new Biz_Event();
        Biz_Supply biz_supply               = new Biz_Supply();
        Biz_SupplyItem biz_supply_item      = new Biz_SupplyItem();
        Biz_Step biz_step                   = new Biz_Step();
        Biz_CollectionEvent biz_coll        = new Biz_CollectionEvent();

        public ActionResult GetLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetLists(0,
                                        WebUtility.GetRequestByInt("user_idx", 0), 
                                        WebUtility.GetRequestByInt("channel_idx", 0),
                                        WebUtility.GetRequestByInt("game_idx", 0),
                                        WebUtility.GetRequestByInt("topic_idx", 0),
                                        WebUtility.GetRequestByInt("event_theme_type_idx", 0),
                                        WebUtility.GetRequestByInt("data_type_idx", 0),
                                        WebUtility.GetRequest("publish_yn", "Y"),
                                        WebUtility.GetRequest("on_air_yn", ""),
                                        WebUtility.GetRequest("temp_yn", ""),
                                        "Y",
                                        WebUtility.GetRequest("sort_type_name", "date"),
                                        BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                        WebUtility.GetRequest("video_yn", ""),
                                        WebUtility.GetRequest("private_view_yn", ""),
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

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetListsByBrand() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetListsByBrand( WebUtility.GetRequestByInt("brand_idx", 0), 
                                                WebUtility.GetRequestByInt("page", 1), 
                                                WebUtility.GetRequestByInt("page_rows", 20),
                                                out total_count,
                                                out page_count);

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

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetCollectionLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetCollectionLists(WebUtility.GetRequestByInt("coll_idx"), 
                                                0,
                                                WebUtility.GetRequestByInt("user_idx"), 
                                                WebUtility.GetRequestByInt("channel_idx"),
                                                WebUtility.GetRequestByInt("topic_idx"),
                                                WebUtility.GetRequestByInt("event_theme_type_idx"),
                                                WebUtility.GetRequestByInt("data_type_idx"),
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

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetList() 
        {
            DataTable dt = biz.GetList( WebUtility.GetRequestByInt("event_idx"), 
                                        WebUtility.GetRequestByInt("user_idx"), 
                                        WebUtility.GetRequestByInt("channel_idx"),
                                        WebUtility.GetRequestByInt("topic_idx"), 
                                        WebUtility.GetRequestByInt("event_theme_type_idx"),
                                        WebUtility.GetRequest("publish_yn"),
                                        WebUtility.GetRequest("on_air_yn"),
                                        WebUtility.GetRequest("temp_yn"),
                                        WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

            dt.Columns.Add("web_url", typeof(string));

            if(WebUtility.GetRequest("step_use_yn", "N").Equals("Y")) 
            {
                dt.Columns.Add("step_data", typeof(DataTable));
                dt.Columns.Add("step_data_count", typeof(int));

                //# 제외
                //dt.Columns.Add("coll_data", typeof(DataTable));
                //dt.Columns.Add("coll_data_count", typeof(int));

                Biz_Step biz_step = new Biz_Step();
                Biz_Product biz_product = new Biz_Product();
                DataTable dtProduct = biz_product.GetProductByStep(WebUtility.GetRequestByInt("event_idx"), 0);

                foreach(DataRow drEvent in dt.Rows) 
                {
                    int event_idx_1             = drEvent["event_idx"].ToInt();
                    DataTable dtStep            = biz_step.GetStep(event_idx_1, 0);

                    dtStep.Columns.Add("product_data", typeof(DataTable));
                    dtStep.Columns.Add("product_data_count", typeof(int));

                    foreach(DataRow drStep in dtStep.Rows) 
                    {
                        drStep["product_data"] = DataTypeUtility.FilterSortDataTable(dtProduct, string.Format("step_idx = {0}", drStep["step_idx"]));
                        drStep["product_data_count"] = ((DataTable)drStep["product_data"]).Rows.Count;
                    }

                    drEvent["step_data"]        = dtStep;
                    drEvent["step_data_count"]  = dtStep.Rows.Count;
                    drEvent["web_url"]          = WebUtility.GetConfig("WEB_ARTICLE_URL", "http://www.beautytalk.co.kr/m/Article?event_idx=") + event_idx_1.ToString();

                    //WEB_ARTICLE_URL

                    //# 제외
                    //dt.Rows[0]["coll_data"]         = biz_coll.Get(WebUtility.GetRequestByInt("user_idx"), event_idx_1);
                    //dt.Rows[0]["coll_data_count"]   = ((DataTable)dt.Rows[0]["coll_data"]).Rows.Count;
                }
            }

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
            "UTF-8",
            new EncoderReplacementFallback(string.Empty),
            new DecoderExceptionFallback()
        );

        public ActionResult SearchLists() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text")))))
                message += "search_text is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.SearchLists( WebUtility.GetRequestByInt("user_idx"), 
                                            WebUtility.GetRequestByInt("channel_idx"),
                                            WebUtility.GetRequestByInt("topic_idx"),
                                            WebUtility.GetRequestByInt("event_theme_type_idx"),
                                            Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text"))),
                                            Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("supply_tag"))),
                                            WebUtility.GetRequest("sort_type_name", "date"),
                                            BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                            "Y",
                                            "Y",
                                            "",
                                            WebUtility.GetRequestByInt("event_content_type_idx", 1),
                                            "",
                                            WebUtility.GetRequestByInt("page", 1), 
                                            WebUtility.GetRequestByInt("page_rows", 36),
                                            out total_count,
                                            out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetOthers_Backup() 
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

            DataTable dt = biz.GetOthers(WebUtility.GetRequestByInt("event_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetOthers() 
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

            //DataTable dt = biz.GetPopulars(WebUtility.GetRequestByInt("event_idx"));
            DataTable dt = biz.GetOthers(WebUtility.GetRequestByInt("event_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRelative() 
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

            DataTable dt = biz.GetRelative( WebUtility.GetRequestByInt("event_idx"),
                                                WebUtility.UserIdx());

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRelativeByName() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_name")))
                message += "search_name is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetRelativeByName( WebUtility.GetRequest("search_name"),
                                                  WebUtility.UserIdx());

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRelativeByProductIdx() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                message += "product_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetRelativeByProductIdx( WebUtility.GetRequestByInt("product_idx"),
                                                        WebUtility.UserIdx());

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetEventRecommendLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetEventRecommendLists(  WebUtility.UserIdx(), 
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

        public ActionResult GetStickerLikeLists() 
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

            int total_count;
            int page_count;

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz.GetEventStickerLikeMyList(   user_idx,
                                                            DateTime.Now,
                                                            WebUtility.GetRequestByInt("page", 1), 
                                                            20,
                                                            out total_count,
                                                            out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //그룹 페이징
        //GetGroupForPaging
        public ActionResult GetGroupEventLists() 
        {
            string json = "";
            
            int total_count;
            int page_count;

            DataTable dt = biz.GetEventGroupLists(WebUtility.GetRequestByInt("page", 1), 
                                                  WebUtility.GetRequestByInt("page_rows", 20),
                                                  out total_count,
                                                  out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //GetFollowingStickerLikeForPaging
        public ActionResult GetFollowingStickerLikeLists()
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

            int total_count;
            int page_count;

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz.GetEventFollowingStickerLikeLists(WebUtility.UserIdx(),
                                                                 DateTime.Now,
                                                                 WebUtility.GetRequestByInt("page", 1), 
                                                                 20,
                                                                 out total_count,
                                                                 out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //GetWithLikeOrderForPaging
        public ActionResult GetPopularEventLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetEventPopularLists(WebUtility.GetRequestByInt("event_idx"), 
                                                    WebUtility.GetRequestByInt("user_idx"), 
                                                    WebUtility.GetRequestByInt("topic_idx"), 
                                                    WebUtility.GetRequest("publish_yn"),
                                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                    WebUtility.GetRequestByInt("page", 1), 
                                                    WebUtility.GetRequestByInt("page_rows", 36),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //GetWithFollowingForPaging
        public ActionResult GetFollowingEventLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetEventFollowingLists(WebUtility.GetRequestByInt("event_idx"), 
                                                        WebUtility.GetRequestByInt("user_idx"), 
                                                        WebUtility.GetRequestByInt("topic_idx"), 
                                                        WebUtility.GetRequest("publish_yn"),
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

        //GetWithBookmarkForPaging
        public ActionResult GetBookmarkEventLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetBookmarkEventLists( WebUtility.GetRequestByInt("event_idx"), 
                                                    Config.CATEGORY, 
                                                    WebUtility.GetRequestByInt("user_idx"),
                                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                    WebUtility.GetRequestByInt("page", 1), 
                                                    WebUtility.GetRequestByInt("page_rows", 25),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Add() 
        {
            string json = "";
            string message = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("topic_idx")))
            //    message += "topic_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
            //    message += "channel_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y")) 
            {
                Biz_UserBlock biz_block = new Biz_UserBlock(user_idx);
                if(biz_block.Post_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }    
            }

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("supply_name")))
                message += "supply_name is null.\n";

            if(WebUtility.GetRequestByInt("topic_idx") == 1) 
            {
                    
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 2) 
            {
                
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 3) 
            {
                    
            }

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DateTime start_date = DateTime.Now;
            DateTime end_date = DateTime.Now;

            if(WebUtility.GetRequest("service_event_use_yn", "N").Equals("Y"))
            {
                start_date  = DateTime.ParseExact(WebUtility.GetRequest("service_start_date") + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                end_date    = DateTime.ParseExact(WebUtility.GetRequest("service_end_date") + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }

            try
            {
                string file_path1 = "supply";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                int event_idx = biz.AddEventWithSupply( WebUtility.GetRequestByInt("topic_idx"), 
                                                        WebUtility.GetRequestByInt("channel_idx", 1), 
                                                        user_idx, 
                                                        WebUtility.GetRequest("publish_yn","N"),
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
                                                        WebUtility.GetRequestByInt("supply_type_idx"),
                                                        WebUtility.GetRequest("supply_type_name"),
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
                                                        WebUtility.GetRequestByInt("owner_idx"),
                                                        WebUtility.GetRequest("supply_tag"),
                                                        WebUtility.GetRequest("transaction_yn"),
                                                        WebUtility.GetRequest("supply_pic_name"),
                                                        WebUtility.GetRequestByInt("data_type_idx", 1),
                                                        (WebUtility.GetRequestByInt("supply_file_idx") > 0)? WebUtility.GetRequestByInt("supply_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                        WebUtility.GetDeviceTypeIdx(),
                                                        WebUtility.GetRequest("on_air_yn", "N"),
                                                        WebUtility.GetRequest("temp_yn", "N"),
                                                        WebUtility.GetRequest("service_event_use_yn", "N"),
                                                        WebUtility.GetRequest("event_theme_type_idxs", ""),
                                                        start_date,
                                                        end_date,
                                                        WebUtility.GetRequestByInt("orientation_type_idx", 1),
                                                        WebUtility.GetRequestByInt("event_content_type_idx", 3),
                                                        WebUtility.GetRequestByInt("shop_product_idx", 0),
                                                        WebUtility.GetRequest("private_view_yn", "N"),
                                                        DateTime.Now,
                                                        user_idx
                                                        );

                biz.ModifyEventContentTypeIdx(event_idx, WebUtility.GetRequestByInt("event_content_type_idx", 3), DateTime.Now);

                if(!WebUtility.GetRequest("collabor_yn").Equals(""))
                {
                    biz.ModifySereisInfo(event_idx, 
                                        WebUtility.GetRequest("collabor_yn", "N"),
                                        WebUtility.GetRequestByInt("collabor_author_count", 1),
                                        WebUtility.GetRequest("collabor_with_follower_yn", "N"),
                                        WebUtility.GetRequest("interfere_talk_yn", "N"),
                                        WebUtility.GetRequest("section_scene_yn", "N"),
                                        WebUtility.GetRequest("fork_yn", "N"),
                                        DateTime.Now,
                                        user_idx);
                }

                //if(WebUtility.GetRequestByInt("coll_idx") > 0) 
                //{
                //    biz_coll.AddCollectionEvent(user_idx, WebUtility.GetRequestByInt("coll_idx"), event_idx, DateTime.Now, user_idx);
                //}
                //else if(WebUtility.GetRequestByInt("coll_idx") < 0)
                //{
                //    biz_coll.RemoveCollectionEvent(event_idx);
                //}

                //DataTable dt = biz.GetEvent(event_idx);
                DataTable dt = biz.GetList(event_idx, 0, 0, 0, 0, "", "", "N");

                //if(WebUtility.GetRequestByInt("coll_idx") > 0)  
                //{
                //    dt.Columns.Add("coll_data", typeof(DataTable));
                //    dt.Columns.Add("coll_data_count", typeof(int));

                //    if(dt.Rows.Count > 0) 
                //    {
                //        dt.Rows[0]["coll_data"]         = biz_coll.Get(user_idx, event_idx);
                //        dt.Rows[0]["coll_data_count"]   = ((DataTable)dt.Rows[0]["coll_data"]).Rows.Count;
                //    }    
                //}

                if(dt.Rows.Count > 0) 
                {
                    //if(WebUtility.GetRequest("publish_yn", "N").Equals("Y")) 
                    //{
                    //    if(WebUtility.GetConfig("NOTICE_YN").Equals("Y")) 
                    //    {
                    //        Biz_User biz_user       = new Biz_User(user_idx);
                    //        Biz_Notice biz_notice   = new Biz_Notice();

                    //        if(dt.Rows.Count > 0) 
                    //        {
                    //            biz_notice.SendNoticeDataTable( user_idx, 
                    //                                            null,
                    //                                            string.Format("[{0} 님의 새 게시물] {1}", biz_user.Nick_Name, WebUtility.GetRequest("supply_name")), 
                    //                                            "1",
                    //                                            "new_post", 
                    //                                            0, 
                    //                                            event_idx);
                    //        }
                    //    }
                    //}

                    //Biz_User biz_user       = new Biz_User(user_idx);
                    //Biz_Notice biz_notice   = new Biz_Notice();

                    //DataTable dt1 = biz_notice.SendPushQueue(user_idx, 
                    //                                        null,
                    //                                        string.Format("{0}", WebUtility.GetRequest("supply_name")), 
                    //                                        "new_post", 
                    //                                        event_idx,
                    //                                        WebUtility.GetRequest("publish_yn", "N"));

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [AcceptVerbs(HttpVerbs.Post), ValidateInput(false)]
        public ActionResult Modify() 
        {
            string json = "";
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

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
            {
                Biz_UserBlock biz_block = new Biz_UserBlock(user_idx);
                if(biz_block.Post_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
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

            if(WebUtility.GetRequestByInt("topic_idx") == 1) 
            {
                
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 2) 
            {
                
            }
            else if(WebUtility.GetRequestByInt("topic_idx") == 3) 
            {
                
            }

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DateTime start_date = DateTime.Now;
            DateTime end_date = DateTime.Now;

            if (WebUtility.GetRequest("service_event_use_yn", "N").Equals("Y"))
            {
                start_date = DateTime.ParseExact(WebUtility.GetRequest("service_start_date") + "000000", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                end_date = DateTime.ParseExact(WebUtility.GetRequest("service_end_date") + "235959", "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }

            try
            {
                string file_path1 = "supply";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1);

                string overwrite = WebUtility.GetRequest("overwrite", "N");
                string file_key = WebUtility.GetRequest("file_key", "");
                int file_idx = 0;

                Biz_Event biz_e = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));

                if((WebUtility.GetRequestByInt("supply_file_idx") > 0))
                {
                    file_idx = WebUtility.GetRequestByInt("supply_file_idx");
                }
                else
                {
                    file_idx = biz_e.FileIdx;

                    if(file_idx > 0)
                    {
                        file_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, file_idx, list1, image_key1, file_key, (overwrite.Equals("Y")));
                    }
                    else
                    {
                        file_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, 0, list1, image_key1);
                    }
                }

                if(biz_e.FileIdx != file_idx) 
                {
                    MongoDBCommon.UpdateEventImage(WebUtility.GetRequestByInt("event_idx"), file_idx);
                }
                
                if(overwrite.Equals("Y"))
                {
                    if(file_idx > 0)
                        MongoDBCommon.UpdateImage(file_idx);
                }

                int event_idx = biz.ModifyEventWithSupply(  WebUtility.GetRequestByInt("event_idx"), 
                                                            WebUtility.GetRequestByInt("topic_idx"), 
                                                            WebUtility.GetRequestByInt("channel_idx", -1), 
                                                            user_idx,
                                                            WebUtility.GetRequest("publish_yn", "N"),
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
                                                            WebUtility.GetRequestByInt("supply_type_idx"),
                                                            WebUtility.GetRequest("supply_type_name"),
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
                                                            WebUtility.GetRequestByInt("owner_idx"),
                                                            WebUtility.GetRequest("supply_tag"),
                                                            WebUtility.GetRequest("transaction_yn"),
                                                            WebUtility.GetRequest("supply_pic_name"),
                                                            file_idx,
                                                            WebUtility.GetDeviceTypeIdx(),
                                                            WebUtility.GetRequest("service_event_use_yn", "N"),
                                                            WebUtility.GetRequest("event_theme_type_idxs", ""),
                                                            start_date,
                                                            end_date,
                                                            WebUtility.GetRequestByInt("orientation_type_idx", 1),
                                                            WebUtility.GetRequestByInt("event_content_type_idx", 1),
                                                            WebUtility.GetRequestByInt("shop_product_idx", 0),
                                                            WebUtility.GetRequest("private_view_yn", ""),
                                                            DateTime.Now, 
                                                            user_idx
                                                            );

                biz.ModifyEventContentTypeIdx(event_idx, WebUtility.GetRequestByInt("event_content_type_idx", 3), DateTime.Now);

                //if(WebUtility.GetRequestByInt("coll_idx") > 0) 
                //{
                //    biz_coll.AddCollectionEvent(user_idx, WebUtility.GetRequestByInt("coll_idx"), event_idx, DateTime.Now, user_idx);
                //}
                //else if(WebUtility.GetRequestByInt("coll_idx") < 0)
                //{
                //    biz_coll.RemoveCollectionEvent(event_idx);
                //}

                DataTable dt = biz.GetList(event_idx, 0, 0, 0, 0, "", "", "");
                MongoDBCommon.UpdateEvent(dt);

                //if(WebUtility.GetRequestByInt("coll_idx") > 0)  
                //{
                //    dt.Columns.Add("coll_data", typeof(DataTable));
                //    dt.Columns.Add("coll_data_count", typeof(int));

                //    if(dt.Rows.Count > 0) 
                //    {
                //        dt.Rows[0]["coll_data"]         = biz_coll.Get(user_idx, event_idx);
                //        dt.Rows[0]["coll_data_count"]   = ((DataTable)dt.Rows[0]["coll_data"]).Rows.Count;
                //    }    
                //}

                // DB_T tbl_content 수기 UPDATE
                //if (biz_e.PIdx != 0) {
                //    Biz_Content bizC = new Biz_Content();
                //    bizC.ModifySelfDataYnByAdmin(biz_e.PIdx);
                //}

                if (dt.Rows.Count > 0)
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

        public ActionResult ModifyPublishYN() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
            //    message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("publish_yn")))
                message += "publish_yn is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                if(WorkHelper.isDuplicateWork("Event/Modify_Publish_yn", WebUtility.GetRequestByInt("event_idx"), user_idx, 5))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                DataTable dt2 = biz.GetEvent(WebUtility.GetRequestByInt("event_idx"));

                if(dt2.Rows.Count > 0) 
                {
                    if(dt2.Rows[0]["publish_yn"].ToString().Equals(WebUtility.GetRequest("publish_yn"))) 
                    {
                        json = DataTypeUtility.JSon("2002", Config.R_PROCESSED, message, null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                }

                int event_idx = WebUtility.GetRequestByInt("event_idx");

                bool isOK = biz.ModifyPublishYN(event_idx,
                                                user_idx, 
                                                WebUtility.GetRequest("publish_yn"), 
                                                DateTime.Now, 
                                                user_idx);

                //DataTable dt = biz.GetEvent(WebUtility.GetRequestByInt("event_idx"));
                DataTable dt = biz.GetList(WebUtility.GetRequestByInt("event_idx"), 0, 0, 0, 0, "Y", "", "N");

                if(isOK)
                {
                    Biz_User biz_user = new Biz_User(user_idx);
                    Biz_Notice biz_notice = new Biz_Notice();
                    string biz_type = "new_post";
                    string message_name = string.Format("{0}", dt.Rows[0]["supply_name"]);

                    if(WebUtility.GetConfig("POST_ALL_USER_PUBLISH_YN", "N").Equals("N"))
                    {
                        if (WebUtility.GetRequest("publish_yn", "N").Equals("Y"))
                        {
                            if (WebUtility.GetConfig("NOTICE_YN").Equals("Y"))
                            {
                                if (dt.Rows.Count > 0)
                                {
                                    biz_notice.SendNoticeDataTable(user_idx,
                                                                    null,
                                                                    message_name,
                                                                    //string.Format("[{0} 님의 새 게시물] {1}", biz_user.Nick_Name, dt.Rows[0]["supply_name"]),
                                                                    "1",
                                                                    biz_type,
                                                                    0,
                                                                    WebUtility.GetRequestByInt("event_idx"),
                                                                    0,
                                                                    0);

                                }
                            }
                        }
                    }
                    else
                    {
                        DataTable dt1 = biz_notice.SendPushQueue(user_idx, 
                                                                message_name, 
                                                                biz_type, 
                                                                event_idx);

                        if(WebUtility.GetRequest("publish_yn", "N").Equals("Y"))
                        {
                            //Biz_PushQueue biz_push = new Biz_PushQueue();
                            //int queue_idx = biz_push.AddPushQueue( biz_type,
                            //                                       message_name,
                            //                                       DateTime.Now,
                            //                                       event_idx,
                            //                                       0,
                            //                                       "Y",
                            //                                       DateTime.Now,
                            //                                       WebUtility.UserIdx());
                        }
                    }

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }

                if (WebUtility.GetRequest("publish_yn", "N").Equals("N")) 
                { 
                    MongoDBCommon.RemovePost(WebUtility.GetRequestByInt("event_idx"));
                }

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SetReservePublishDate()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("publish_reserve_date")))
                message += "push_start_date is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if ((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                int user_idx = WebUtility.UserIdx();

                if (WorkHelper.isDuplicateWork("Event/SetReservePublishDate", WebUtility.GetRequestByInt("event_idx"), user_idx, 5))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                int event_idx = WebUtility.GetRequestByInt("event_idx");

                DateTime? publish_reserve_date = null;

                try
                {
                    publish_reserve_date = DateTime.ParseExact(WebUtility.GetRequest("publish_reserve_date"), "yyyyMMdd HH:mm", CultureInfo.InvariantCulture);
                }
                catch
                {
                    publish_reserve_date = DateTime.Now.AddDays(1);
                }

                DateTime pubilsh_d = (DateTime)publish_reserve_date;
                biz.ModifyPublishReserveDate(event_idx, pubilsh_d, WebUtility.GetRequest("publish_reserve_yn", "N"));

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

        public ActionResult RestoreDB() 
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

            try
            {
                int user_idx = WebUtility.UserIdx();

                MongoDBCommon.RemoveEventByUser(user_idx);

                int total_count;
                int page_count;

                DataTable dt = biz.GetLists(0,
                                            user_idx, 
                                            WebUtility.GetRequestByInt("channel_idx"),
                                            WebUtility.GetRequestByInt("game_idx"),
                                            WebUtility.GetRequestByInt("topic_idx"),
                                            WebUtility.GetRequestByInt("event_theme_type_idx"),
                                            WebUtility.GetRequestByInt("data_type_idx"),
                                            WebUtility.GetRequest("publish_yn", "Y"),
                                            WebUtility.GetRequest("on_air_yn", ""),
                                            WebUtility.GetRequest("temp_yn", "N"),
                                            "Y",
                                            WebUtility.GetRequest("sort_type_name", "date"),
                                            BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                            WebUtility.GetRequest("video_yn", "N"),
                                            WebUtility.GetRequest("private_view_yn", "N"),
                                            WebUtility.GetRequestByInt("page", 1), 
                                            WebUtility.GetRequestByInt("page_rows", 100000),
                                            out total_count,
                                            out page_count,
                                            WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

                foreach(DataRow drE in dt.Rows) 
                {
                    DataTable dtFeed = biz.GetFeedList(user_idx, drE["event_idx"].ToInt());
                    dtFeed.Columns.Add("step_data", typeof(DataTable));
                    dtFeed.Columns.Add("step_data_count", typeof(int));

                    Storichain.Models.Dac.Dac_Step dac_s = new Storichain.Models.Dac.Dac_Step();

                    foreach(DataRow drEvent in dtFeed.Rows) 
                    {
                        int event_idx_1             = drEvent["event_idx"].ToInt();
                        drEvent["step_data"]        = dac_s.Select(event_idx_1, 0).Tables[0];
                        drEvent["step_data_count"]  = ((DataTable)drEvent["step_data"]).Rows.Count;
                    }

                    MongoDBCommon.SendFeedBank(dtFeed);
                }

                json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ModifyFile() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("file_idx")))
            //    message += "file_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string file_path1 = "event";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                Biz_Supply biz_s = new Biz_Supply();
                bool isOK = biz_s.ModifySupplyFile(WebUtility.GetRequestByInt("event_idx"), 
                                                  (WebUtility.GetRequestByInt("supply_file_idx") > 0)? WebUtility.GetRequestByInt("supply_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1));

                if(isOK)
                {
                    Biz_Event biz_ee  = new Biz_Event();
                    DataTable dt      = BizUtility.GetImageData(biz_ee.GetEvent(WebUtility.GetRequestByInt("event_idx")));
                    MongoDBCommon.UpdateSupplyCoverImage(dt);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }

                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ModifyUseYN() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
                message += "use_yn is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                bool isOK = biz.ModifyUseYN(WebUtility.GetRequestByInt("event_idx"),
                                            WebUtility.UserIdx(),
                                            WebUtility.GetRequest("use_yn", "N"), 
                                            DateTime.Now, 
                                            WebUtility.UserIdx());

                //DataTable dt = biz.GetList( 0,
                //                            WebUtility.UserIdx(),
                //                            0,
                //                            0,
                //                            0,
                //                            "Y");

                if (WebUtility.GetRequest("use_yn", "N").Equals("N")) 
                { 
                    MongoDBCommon.RemovePost(WebUtility.GetRequestByInt("event_idx"));

                    DataTable dt = biz.GetEvent(WebUtility.GetRequestByInt("event_idx"));

                    if(dt.Rows.Count > 0) 
                    {
                        if(!dt.Rows[0]["video_id"].ToString().Equals(""))
                        {
                            UstreamHelper.RemoveVideoDetail(dt.Rows[0]["video_id"].ToString());
                        }
                    }
                }

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

        public ActionResult ModifySeriesInfo() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("collabor_yn")))
            //    message += "collabor_yn is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("collabor_author_count")))
            //    message += "collabor_author_count is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("collabor_with_follower_yn")))
                message += "collabor_with_follower_yn is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("interfere_talk_yn")))
                message += "interfere_talk_yn is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("section_scene_yn")))
                message += "section_scene_yn is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("fork_yn")))
                message += "fork_yn is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                bool isOK = biz.ModifySereisInfo(WebUtility.GetRequestByInt("event_idx"),
                                                 WebUtility.GetRequest("collabor_yn", ""), 
                                                 WebUtility.GetRequestByInt("collabor_author_count", 0), 
                                                 WebUtility.GetRequest("collabor_with_follower_yn", "N"), 
                                                 WebUtility.GetRequest("interfere_talk_yn", "N"), 
                                                 WebUtility.GetRequest("section_scene_yn", "N"), 
                                                 WebUtility.GetRequest("fork_yn", "N"), 
                                                 DateTime.Now, 
                                                 WebUtility.UserIdx());

                if (isOK)
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

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
                message += "sns_type_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_UserSNSShare biz = new Biz_UserSNSShare();
                bool isOK = biz.AddUserSSNSShare (  WebUtility.UserIdx(),
                                                    WebUtility.GetRequestByInt("event_idx"), 
                                                    WebUtility.GetRequestByInt("sns_type_idx"), 
                                                    DateTime.Now, 
                                                    WebUtility.UserIdx());

                if(isOK) 
                {
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

        

    }
}



//public ActionResult Add()
//        {
//            string json = "";
//            string message = "";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_p_idx")))
//                message += "event_p_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_g_idx")))
//                message += "event_g_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("depth_order")))
//                message += "depth_order is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
//                message += "sort_order is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("topic_idx")))
//                message += "topic_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
//                message += "channel_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_private_idx")))
//                message += "channel_private_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
//                message += "user_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
//                message += "to_user_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequest("secret_yn")))
//                message += "secret_yn is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequest("publish_yn")))
//                message += "publish_yn is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("publish_date")))
//                message += "publish_date is null.";

//            if(!message.Equals(""))
//            {
//                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
//                return Content(json, "application/json", System.Text.Encoding.UTF8);
//            }

//            try
//            {
//                int event_idx = biz.AddEvent(	WebUtility.GetRequestByInt("event_p_idx"),
//                                                WebUtility.GetRequestByInt("event_g_idx"),
//                                                WebUtility.GetRequestByInt("depth_order"),
//                                                WebUtility.GetRequestByInt("sort_order"),
//                                                WebUtility.GetRequestByInt("topic_idx"),
//                                                WebUtility.GetRequestByInt("channel_idx"),
//                                                WebUtility.GetRequestByInt("channel_private_idx"),
//                                                WebUtility.GetRequestByInt("user_idx"),
//                                                WebUtility.GetRequestByInt("to_user_idx"),
//                                                WebUtility.GetRequest("secret_yn"),
//                                                WebUtility.GetRequest("publish_yn"),
//                                                WebUtility.GetRequestByDateTime("publish_date"),
//                                                WebUtility.GetRequest("use_yn"),
//                                                DateTime.Now,
//                                                WebUtility.UserIdx());

//                if(event_idx > 0)
//                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
//                else
//                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
//            }
//            catch(Exception ex)
//            {
//                BizUtility.SendErrorLog(Request, ex);
//                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
//            }

//            return Content(json, "application/json", System.Text.Encoding.UTF8);
//        }

//        public ActionResult Modify()
//        {
//            string json = "";
//            string message = "";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
//                message += "event_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_p_idx")))
//                message += "event_p_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_g_idx")))
//                message += "event_g_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("depth_order")))
//                message += "depth_order is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
//                message += "sort_order is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("topic_idx")))
//                message += "topic_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
//                message += "channel_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_private_idx")))
//                message += "channel_private_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
//                message += "user_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
//                message += "to_user_idx is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequest("secret_yn")))
//                message += "secret_yn is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequest("publish_yn")))
//                message += "publish_yn is null.";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("publish_date")))
//                message += "publish_date is null.";

//            if(!message.Equals(""))
//            {
//                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
//                return Content(json, "application/json", System.Text.Encoding.UTF8);
//            }

//            try
//            {
//                bool isOK = biz.ModifyEvent(	WebUtility.GetRequestByInt("event_idx"),
//                                                WebUtility.GetRequestByInt("event_p_idx"),
//                                                WebUtility.GetRequestByInt("event_g_idx"),
//                                                WebUtility.GetRequestByInt("depth_order"),
//                                                WebUtility.GetRequestByInt("sort_order"),
//                                                WebUtility.GetRequestByInt("topic_idx"),
//                                                WebUtility.GetRequestByInt("channel_idx"),
//                                                WebUtility.GetRequestByInt("channel_private_idx"),
//                                                WebUtility.GetRequestByInt("user_idx"),
//                                                WebUtility.GetRequestByInt("to_user_idx"),
//                                                WebUtility.GetRequest("secret_yn"),
//                                                WebUtility.GetRequest("publish_yn"),
//                                                WebUtility.GetRequestByDateTime("publish_date"),
//                                                WebUtility.GetRequest("use_yn"),
//                                                DateTime.Now,
//                                                WebUtility.UserIdx());

//                if(isOK)
//                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
//                else
//                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
//            }
//            catch(Exception ex)
//            {
//                BizUtility.SendErrorLog(Request, ex);
//                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
//            }

//            return Content(json, "application/json", System.Text.Encoding.UTF8);
//        }

//        public ActionResult Remove()
//        {
//            string json = "";
//            string message = "";

//            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
//                message += "event_idx is null.";

//            if(!message.Equals(""))
//            {
//                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
//                return Content(json, "application/json", System.Text.Encoding.UTF8);
//            }

//            try
//            {
//                bool isOK = biz.RemoveEvent(WebUtility.GetRequestByInt("event_idx"));

//                if(isOK)
//                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
//                else
//                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
//            }
//            catch(Exception ex)
//            {
//                BizUtility.SendErrorLog(Request, ex);
//                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
//            }

//            return Content(json, "application/json", System.Text.Encoding.UTF8);
//        }











        //public ActionResult GetListsForSite() 
        //{
        //    int total_count;
        //    int page_count;

        //    DataTable dt = biz.GetLists(0,
        //                                WebUtility.GetRequestByInt("user_idx"), 
        //                                WebUtility.GetRequestByInt("channel_idx"),
        //                                WebUtility.GetRequestByInt("topic_idx"),
        //                                WebUtility.GetRequestByInt("data_type_idx"),
        //                                WebUtility.GetRequest("publish_yn"),
        //                                WebUtility.GetRequest("use_yn"),
        //                                WebUtility.GetRequest("sort_type_name"),
        //                                BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
        //                                WebUtility.GetRequestByInt("page", 1), 
        //                                WebUtility.GetRequestByInt("page_rows", 36),
        //                                out total_count,
        //                                out page_count);

        //    Dictionary<string,object> dic = new Dictionary<string,object>();
        //    dic.Add("total_row_count", total_count);
        //    dic.Add("page_count", page_count);

        //    string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}