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
    public class StickerController : Controller
    {
        Biz_Comment biz_c       = new Biz_Comment();
        Biz_Notice biz_notice   = new Biz_Notice();
        Biz_Sticker biz         = new Biz_Sticker();
        Biz_EventSticker biz_e  = new Biz_EventSticker();

        public ActionResult Get()
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sticker_type_idx")))
                message += "sticker_type_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetSticker(WebUtility.GetRequestByInt("sticker_idx"),
                                          WebUtility.GetRequestByInt("category_idx", 1),
                                          "use_yn");
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetSticker()
        {
            string json = "";

            string message = "";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sticker_ver_idx")))
            //    message += "sticker_ver_idx is null.\n";

            //if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
            //    message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int user_idx = WebUtility.UserIdx();
            Dictionary<string, object> dt = biz.GetStickerByVersion(WebUtility.GetRequestByInt("sticker_ver_idx", 1),
                                                                    user_idx,
                                                                    WebUtility.GetRequestByInt("sticker_group_idx"),
                                                                    WebUtility.GetRequestByInt("sticker_type_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        
        public ActionResult AddSticker()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
                message += "event_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sticker_idx")))
                message += "sticker_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int user_idx = WebUtility.UserIdx();

            if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
            {
                Biz_UserBlock biz_block = new Biz_UserBlock(user_idx);
                if (biz_block.Comment_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
                {
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }
                else if (biz_block.Enable_YN.Equals("N"))
                {
                    System.Web.Security.FormsAuthentication.SignOut();
                    json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }
                else
                {
                    Biz_Event biz_event = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));

                    Biz_Report biz_report = new Biz_Report();
                    DataTable dtReport = biz_report.GetUserBlock(user_idx, biz_event.UserIdx);

                    if (dtReport.Rows.Count > 0)
                    {
                        if (DataTypeUtility.GetValue(dtReport.Rows[0]["block_yn"], "N").Equals("Y"))
                        {
                            json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "block", null);
                            return Content(json, "application/json", System.Text.Encoding.UTF8);
                        }
                    }
                }    
            }

            try
            {
                if (WorkHelper.isDuplicateWork("Sticker/Add", WebUtility.GetRequestByInt("event_idx"), user_idx, 1))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                int comment_idx = biz.AddStickerComment(WebUtility.GetRequestByInt("sticker_idx"),
                                                        WebUtility.GetRequestByInt("event_idx"),
                                                        user_idx,
                                                        WebUtility.GetRequestByInt("comment_type_idx", 1),
                                                        WebUtility.GetRequest("comment_text"),
                                                        DateTime.Now);

                if (WebUtility.GetConfig("NOTICE_YN").Equals("Y"))
                {
                    Biz_User biz_user = new Biz_User(user_idx);
                    Biz_Event biz_event = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));
                    Biz_Comment biz_comment = new Biz_Comment();

                    if (WebUtility.GetRequestByInt("reply_user_idx") == 0)
                    {
                        if (WebUtility.GetRequestByInt("comment_type_idx", 1) == 1) 
                        { 
                            biz_notice.SendNoticeDataTable( user_idx,
                                                            null,
                                                            string.Format("[{0} 님의 스티커]", biz_user.Nick_Name),
                                                            "1",
                                                            "comment",
                                                            comment_idx,
                                                            WebUtility.GetRequestByInt("event_idx"),
                                                            0,
                                                            0);    
                        }
                    }
                    else
                    {
                        DataTable dtUser = new DataTable();
                        DataRow dr;
                        dtUser.Columns.Add("to_user_idx", typeof(int));

                        if (dtUser.Select("to_user_idx = " + biz_event.UserIdx.ToString()).Length == 0)
                        {
                            dr = dtUser.NewRow();
                            dr["to_user_idx"] = biz_event.UserIdx;
                            dtUser.Rows.Add(dr);
                        }

                        if (dtUser.Select("to_user_idx = " + WebUtility.GetRequestByInt("reply_user_idx").ToString()).Length == 0)
                        {
                            dr = dtUser.NewRow();
                            dr["to_user_idx"] = WebUtility.GetRequestByInt("reply_user_idx");
                            dtUser.Rows.Add(dr);
                        }

                        if (dtUser.Select("to_user_idx = " + user_idx.ToString()).Length > 0)
                        {
                            DataRow[] drCol = dtUser.Select("to_user_idx = " + user_idx.ToString());

                            foreach (DataRow dr1 in drCol)
                            {
                                dr1.Delete();
                            }
                        }

                        if (WebUtility.GetRequestByInt("comment_type_idx", 1) == 1) 
                        { 
                            biz_notice.SendNotice(  user_idx,
                                                    DataTypeUtility.GetSplitString(dtUser, "to_user_idx", ",", false),
                                                    string.Format("[{0} 님의 스티커]", biz_user.Nick_Name),
                                                    "1",
                                                    "comment",
                                                    comment_idx,
                                                    WebUtility.GetRequestByInt("event_idx"),
                                                    0,
                                                    0);
                        }
                    }
                }

                if (comment_idx > 0)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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
