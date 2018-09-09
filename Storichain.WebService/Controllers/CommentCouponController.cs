using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Collections;

namespace Storichain.Controllers
{
    public class CommentCouponController : Controller
    {
        Biz_CommentCoupon biz = new Biz_CommentCoupon();
        Biz_Notice biz_notice = new Biz_Notice();

        public ActionResult Get() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetCommentCoupon(    WebUtility.GetRequestByInt("comment_coupon_idx"), 
                                                    WebUtility.GetRequestByInt("coupon_idx"), 
                                                    WebUtility.GetRequestByInt("user_idx"),
                                                    WebUtility.GetRequestByInt("comment_type_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetCommentLists( WebUtility.GetRequestByInt("coupon_idx"),
                                                WebUtility.GetRequest("sort_asc_yn", "Y"),
                                                BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                WebUtility.GetRequestByInt("page", 1), 
                                                WebUtility.GetRequestByInt("page_rows", Int32.MaxValue),
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

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.Count(WebUtility.GetRequestByInt("coupon_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        //public ActionResult GetMyNew() 
        //{
        //    string json     = "";
        //    string message  = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
        //        message += "event_idx is null.\n";

        //    if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
        //        message += "user_idx is null.\n";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

        //    int user_idx = WebUtility.UserIdx();

        //    DataTable dt = biz.GetJoined(WebUtility.GetRequestByInt("event_idx"), user_idx);
        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        //public ActionResult Test() 
        //{
        //    string json = "";
        //    string message = "";

        //    if(WorkHelper.isDuplicateWork("Comment/Add", WebUtility.GetRequestByInt("event_idx"), 40, 100))
        //    {
        //        json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        [ValidateInput(false)]
        public ActionResult Add() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            //if(WebUtility.GetConfig("BLOCK_USE_YN", "N").Equals("Y"))
            //{
            //    Biz_UserBlock biz_block = new Biz_UserBlock(user_idx);
            //    if(biz_block.Comment_Block_YN.Equals("Y") || biz_block.User_Block_YN.Equals("Y"))
            //    {
            //        json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
            //        return Content(json, "application/json", System.Text.Encoding.UTF8);
            //    }
            //    else if(biz_block.Enable_YN.Equals("N"))
            //    {
            //        System.Web.Security.FormsAuthentication.SignOut();
            //        json = DataTypeUtility.JSon("2500", Config.R_SUCCESS, "Logout", null);
            //        return Content(json, "application/json", System.Text.Encoding.UTF8);
            //    }
            //    else 
            //    {
            //        Biz_Event biz_event = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));

            //        Biz_Report biz_report = new Biz_Report();
            //        DataTable dtReport = biz_report.GetUserBlock(user_idx, biz_event.UserIdx);

            //        if(dtReport.Rows.Count > 0) 
            //        {
            //            if(DataTypeUtility.GetValue(dtReport.Rows[0]["block_yn"], "N").Equals("Y"))
            //            {
            //                json = DataTypeUtility.JSon("2500", Config.R_BLOCK, "block", null);
            //                return Content(json, "application/json", System.Text.Encoding.UTF8);
            //            }
            //        }
            //    }    
            //}

            try
            {
                if(WorkHelper.isDuplicateWork("Commentcoupon/Add", WebUtility.GetRequestByInt("coupon_idx"), user_idx, 1))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                string file_path1 = "comment_coupon";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                int comment_coupon_idx = biz.AddCommentCoupon(      WebUtility.GetRequestByInt("comment_coupon_type_idx", 1),
                                                                    WebUtility.GetRequestByInt("coupon_idx"), 
                                                                    user_idx, 
                                                                    WebUtility.GetRequest("comment_text"), 
                                                                    WebUtility.GetRequestByInt("rate"), 
                                                                    "N",
                                                                    BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                                    "N",
                                                                    DateTime.Now, 
                                                                    user_idx);

                if (WebUtility.GetConfig("NOTICE_YN").Equals("Y"))
                {
                    Biz_User biz_user = new Biz_User(user_idx);
                    //Biz_Event biz_event = new Biz_Event(WebUtility.GetRequestByInt("event_idx"));
                    int coupon_idx = WebUtility.GetRequestByInt("coupon_idx");
                    //Biz_coupon biz_coupon = new Biz_coupon(WebUtility.GetRequestByInt("coupon_idx"));
                    Biz_CommentCoupon biz_comment = new Biz_CommentCoupon();

                    if (WebUtility.GetRequestByInt("reply_user_idx", 0) == 0)
                    {
                        if (WebUtility.GetRequestByInt("comment_coupon_type_idx", 1) == 1)
                        {
                            biz_notice.SendNoticeDataTable(user_idx,
                                                            null,
                                                            string.Format("[{0} 님의 댓글] {1}", biz_user.Nick_Name, WebUtility.GetRequest("comment_text")),
                                                            "1",
                                                            "comment_coupon",
                                                            0,
                                                            0,
                                                            comment_coupon_idx,
                                                            coupon_idx);
                        }
                    }
                    else
                    {
                        DataTable dtUser = new DataTable();
                        DataRow dr;
                        dtUser.Columns.Add("to_user_idx", typeof(int));

                        //if (dtUser.Select("to_user_idx = " + biz_coupon.CreateUserIdx.ToString()).Length == 0)
                        //{
                        //    dr = dtUser.NewRow();
                        //    dr["to_user_idx"] = biz_event.UserIdx;
                        //    dtUser.Rows.Add(dr);
                        //}

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
                            biz_notice.SendNotice(user_idx,
                                                    DataTypeUtility.GetSplitString(dtUser, "to_user_idx", ",", false),
                                                    string.Format("[{0} 님의 댓글] {1}", biz_user.Nick_Name, WebUtility.GetRequest("comment_text")),
                                                    "1",
                                                    "comment_coupon",
                                                    0,
                                                    0,
                                                    comment_coupon_idx,
                                                    coupon_idx);
                        }
                    }
                }

                if (comment_coupon_idx > 0) 
                {
                    //int total_count;
                    //int page_count;

                    DataTable dt = biz.GetCommentCoupon(comment_coupon_idx, 
                                                        0,
                                                        0,
                                                        0);

                    //DataTable dt = biz.GetCommentLists( WebUtility.GetRequestByInt("coupon_idx"),
                    //                                    "",
                    //                                    BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                    //                                    WebUtility.GetRequestByInt("page", 1), 
                    //                                    WebUtility.GetRequestByInt("page_rows", 10),
                    //                                    out total_count,
                    //                                    out page_count);

                    //Dictionary<string,object> dic = new Dictionary<string,object>();
                    //dic.Add("total_row_count", total_count);
                    //dic.Add("page_count", page_count);
                
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

        
        //[ValidateInput(false)]
        //public ActionResult Modify() 
        //{
        //    string json = "";

        //    try
        //    {
        //        bool isOK = biz.ModifyComment(  WebUtility.GetRequestByInt("comment_coupon_idx"),
        //                                        WebUtility.GetRequestByInt("event_idx"), 
        //                                        WebUtility.GetRequestByInt("user_idx"), 
        //                                        WebUtility.GetRequest("comment_text"), 
        //                                        WebUtility.GetRequestByInt("comment_type_type"), 
        //                                        WebUtility.GetRequestByInt("rate"), 
        //                                        WebUtility.GetRequestByInt("file_idx"), 
        //                                        DateTime.Now, 
        //                                        WebUtility.UserIdx());

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

        public ActionResult Remove() 
        {
            string json     = "";
            string message  = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("comment_coupon_idx")))
                message += "comment_coupon_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.UpdateForDelete(WebUtility.GetRequestByInt("comment_coupon_idx"));

                //MongoDBCommon.RemoveComment(WebUtility.GetRequestByInt("comment_coupon_idx"));

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
