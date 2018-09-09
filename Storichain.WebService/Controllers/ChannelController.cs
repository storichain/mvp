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
    public class ChannelController : Controller
    {
        Biz_Channel biz = new Biz_Channel();
        Biz_Event biz_event = new Biz_Event();

        public ActionResult Get()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
                message += "channel_idx is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetChannel(WebUtility.GetRequestByInt("channel_idx"), WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Search()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_text")))
                message += "search_text is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.SearchChannel(WebUtility.GetRequest("search_text"), WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        //public ActionResult Add()
        //{
        //    string json = "";
        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("channel_name")))
        //        message += "channel_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("channel_desc")))
        //        message += "channel_desc is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_type_idx")))
        //        message += "channel_type_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_idx")))
        //        message += "game_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("text_color")))
        //        message += "text_color is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("background_color")))
        //        message += "background_color is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("private_yn")))
        //        message += "private_yn is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("owner_user_idx")))
        //        message += "owner_user_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
        //        message += "sort_order is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_file_idx")))
        //        message += "channel_file_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_back_file_idx")))
        //        message += "channel_back_file_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
        //        message += "use_yn is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        int channel_idx = biz.AddChannel(	WebUtility.GetRequest("channel_name"),
        //                                            WebUtility.GetRequest("channel_desc"),
        //                                            WebUtility.GetRequestByInt("channel_type_idx"),
        //                                            WebUtility.GetRequestByInt("game_idx"),
        //                                            WebUtility.GetRequest("text_color"),
        //                                            WebUtility.GetRequest("background_color"),
        //                                            WebUtility.GetRequest("private_yn"),
        //                                            WebUtility.GetRequestByInt("owner_user_idx"),
        //                                            WebUtility.GetRequestByInt("sort_order"),
        //                                            WebUtility.GetRequestByInt("channel_file_idx"),
        //                                            WebUtility.GetRequestByInt("channel_back_file_idx"),
        //                                            WebUtility.GetRequest("use_yn"),
        //                                            WebUtility.GetRequestByDateTime("create_date"),
        //                                            WebUtility.GetRequestByInt("create_user_idx"));

        //        if(channel_idx > 0)
        //            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //        else
        //            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
        //    }
        //    catch(Exception ex)
        //    {
        //        BizUtility.SendErrorLog(Request, ex);
        //        json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
        //    }

        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        //public ActionResult Modify()
        //{
        //    string json = "";
        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
        //        message += "channel_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("channel_name")))
        //        message += "channel_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("channel_desc")))
        //        message += "channel_desc is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_type_idx")))
        //        message += "channel_type_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("game_idx")))
        //        message += "game_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("text_color")))
        //        message += "text_color is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("background_color")))
        //        message += "background_color is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("private_yn")))
        //        message += "private_yn is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("owner_user_idx")))
        //        message += "owner_user_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
        //        message += "sort_order is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_file_idx")))
        //        message += "channel_file_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_back_file_idx")))
        //        message += "channel_back_file_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
        //        message += "use_yn is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        bool isOK = biz.ModifyChannel(	WebUtility.GetRequestByInt("channel_idx"),
        //                                        WebUtility.GetRequest("channel_name"),
        //                                        WebUtility.GetRequest("channel_desc"),
        //                                        WebUtility.GetRequestByInt("channel_type_idx"),
        //                                        WebUtility.GetRequestByInt("game_idx"),
        //                                        WebUtility.GetRequest("text_color"),
        //                                        WebUtility.GetRequest("background_color"),
        //                                        WebUtility.GetRequest("private_yn"),
        //                                        WebUtility.GetRequestByInt("owner_user_idx"),
        //                                        WebUtility.GetRequestByInt("sort_order"),
        //                                        WebUtility.GetRequestByInt("channel_file_idx"),
        //                                        WebUtility.GetRequestByInt("channel_back_file_idx"),
        //                                        WebUtility.GetRequest("use_yn"),
        //                                        WebUtility.GetRequestByDateTime("update_date"),
        //                                        WebUtility.GetRequestByInt("update_user_idx"));

        //        if(isOK)
        //            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //        else
        //            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
        //    }
        //    catch(Exception ex)
        //    {
        //        BizUtility.SendErrorLog(Request, ex);
        //        json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
        //    }

        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        //public ActionResult Remove()
        //{
        //    string json = "";
        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("channel_idx")))
        //        message += "channel_idx is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        bool isOK = biz.RemoveChannel(WebUtility.GetRequestByInt("channel_idx"));

        //        if(isOK)
        //            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //        else
        //            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
        //    }
        //    catch(Exception ex)
        //    {
        //        BizUtility.SendErrorLog(Request, ex);
        //        json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
        //    }

        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        public ActionResult GetLists() 
        {
            string json = "";
            string message = "";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.GetLists(WebUtility.GetRequestByInt("channel_idx"),
                                        WebUtility.GetRequestByInt("channel_type_idx"),
                                        WebUtility.GetRequest("use_yn", "Y"),
                                        WebUtility.GetRequest("sort_type_name", "date"),
                                        DateTime.Now,
                                        WebUtility.GetRequestByInt("page", 1), 
                                        WebUtility.GetRequestByInt("page_rows", 20),
                                        out total_count,
                                        out page_count);

            if(WebUtility.GetRequest("sort_type_name", "date").Equals("best")) 
            {
                dt.Columns.Add("step_data", typeof(DataTable));
                dt.Columns.Add("step_data_count", typeof(int));

                Biz_Step biz_step = new Biz_Step();

                foreach(DataRow drEvent in dt.Rows) 
                {
                    int event_idx_1             = drEvent["event_idx"].ToInt();
                    drEvent["step_data"]        = BizUtility.GetImageData(biz_step.GetTop(event_idx_1, 0, 3));
                    drEvent["step_data_count"]  = ((DataTable)drEvent["step_data"]).Rows.Count;
                }
            }

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetEventLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz_event.GetChannelEventLists(  WebUtility.GetRequestByInt("user_idx"), 
                                                            WebUtility.GetRequestByInt("channel_idx"), 
                                                            WebUtility.GetRequestByInt("data_type_idx"),
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

        //GetWithLikeOrderForPaging
        public ActionResult GetPopularEventLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz_event.GetChannelEventPopularLists( WebUtility.GetRequestByInt("channel_idx"), 
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

    }
}
