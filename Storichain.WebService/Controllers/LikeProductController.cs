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
    public class LikeProductController : Controller
    {
        Biz_LikeProduct biz     = new Biz_LikeProduct();
        Biz_Sticker biz_s       = new Biz_Sticker();
        Biz_StickerGroup biz_g  = new Biz_StickerGroup();
        Biz_Notice biz_notice   = new Biz_Notice();

        public ActionResult Get() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                message += "product_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            DataTable dt = biz.GetLike(WebUtility.GetRequestByInt("product_idx"), 
                                       user_idx);
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetProductLikeLists() 
        {
            string json;
            string message = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
            //    message += "product_idx is null.\n";

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

            DataTable dt = biz.GetProductLikeLists( WebUtility.GetRequestByInt("product_idx"), 
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

        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz.AddLike(WebUtility.GetRequestByInt("product_idx"), 
                                        WebUtility.UserIdx(), 
                                        WebUtility.GetRequest("like_yn"),
                                        WebUtility.GetRequestByInt("int like_type_idx,"),
                                        DateTime.Now,
                                        WebUtility.GetDeviceTypeIdx(),
                                        DateTime.Now, 
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

        //public ActionResult Modify() 
        //{
        //    string json = "";

        //    try
        //    {
        //        bool isOK = biz.ModifyLike( WebUtility.GetRequestByInt("product_idx"), 
        //                                    WebUtility.GetRequestByInt("user_idx"), 
        //                                    WebUtility.GetRequest("like_yn"),
        //                                    WebUtility.GetRequestByInt("int like_type_idx,"),
        //                                    DateTime.Now, 
        //                                    DateTime.Now, 
        //                                    WebUtility.GetRequestByInt("user_idx"));

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

        public ActionResult Toggle() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                    message += "product_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int product_idx   = WebUtility.GetRequestByInt("product_idx");
                int user_idx    = WebUtility.UserIdx();

                if(WorkHelper.isDuplicateWork("LikeProduct/Toggle", WebUtility.GetRequestByInt("product_idx"), user_idx, 1))
                {
                    json = DataTypeUtility.JSon("2001", Config.R_PROCESSING_NOW, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                bool isOK = biz.Toggle( product_idx, 
                                        user_idx, 
                                        WebUtility.GetRequestByInt("like_type_idx", 1),
                                        DateTime.Now,
                                        DateTime.Now, 
                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz.GetLike(product_idx, user_idx);

                    //if(WebUtility.GetConfig("NOTICE_YN").Equals("Y")) 
                    //{
                    //    Biz_User biz_user       = new Biz_User(user_idx);
                    //    Biz_Event biz_event     = new Biz_Event(WebUtility.GetRequestByInt("product_idx"));
                    //    Biz_Notice biz_notice   = new Biz_Notice();

                    //    if(dt.Rows.Count > 0) 
                    //    {
                    //        if(dt.Rows[0]["like_yn"].ToString().Equals("Y")) 
                    //        {
                    //            //if(dt.Rows[0]["like_type_idx"].ToInt() == 1) 
                    //            //{
                    //            //    biz_notice.SendNotice(  user_idx, 
                    //            //                            biz_event.UserIdx.ToString().Split(','),
                    //            //                            string.Format(WebUtility.GetConfig("MSG_LIKE", "{0} 님이 내 게시물을 좋아요 하였습니다.") , biz_user.Nick_Name), 
                    //            //                            "1",
                    //            //                            "like",
                    //            //                            0,
                    //            //                            WebUtility.GetRequestByInt("product_idx"));
                    //            //}
                    //            //else if(dt.Rows[0]["like_type_idx"].ToInt() == 2) 
                    //            //{
                    //            //    //biz_notice.SendNotice(  user_idx, 
                    //            //    //                        biz_event.UserIdx.ToString().Split(','),
                    //            //    //                        string.Format(WebUtility.GetConfig("MSG_HATE", "{0} 님이 내 게시물을 싫어요 하였습니다.") , biz_user.Nick_Name), 
                    //            //    //                        "1",
                    //            //    //                        "hate",
                    //            //    //                        0,
                    //            //    //                        WebUtility.GetRequestByInt("product_idx"));
                    //            //}
                    //        }
                    //        else 
                    //        {
                    //            //MongoDBCommon.RemoveLike(user_idx, biz_event.UserIdx, product_idx);
                    //        }
                    //    }
                    //}

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

        public ActionResult Remove() 
        {
            string json = "";

            try
            {
                bool isOK = biz.RemoveLike( WebUtility.GetRequestByInt("product_idx"), 
                                            WebUtility.GetRequestByInt("int step_idx"));

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

        public ActionResult SentStickerAtEvent()
        {
            string json = "";

            try
            {
                string message = "";

                if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
                    message += "product_idx is null.\n";

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

                bool isDuplicate = false;

                Biz_EventSticker biz_e = new Biz_EventSticker();

                if (WebUtility.GetRequest("duplicate_check_yn", "N").Equals("Y"))
                {
                    isDuplicate = biz_e.IsExist(WebUtility.GetRequestByInt("product_idx"),
                                                WebUtility.GetRequestByInt("sticker_idx"),
                                                user_idx);
                }

                if (isDuplicate)
                {
                    json = DataTypeUtility.JSon("2000", Config.R_DUPLICATE, "", null);
                }
                else
                {
                    bool isOK = biz_e.AddEventSticker  (WebUtility.GetRequestByInt("product_idx"),
                                                        WebUtility.GetRequestByInt("sticker_idx"),
                                                        user_idx,
                                                        DateTime.Now,
                                                        user_idx);

                    if (isOK)
                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                    else
                        json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }
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
