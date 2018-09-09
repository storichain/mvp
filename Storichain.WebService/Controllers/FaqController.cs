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
	public class FaqController : Controller
	{
		Biz_Faq biz = new Biz_Faq();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_idx")))
            //    message += "faq_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetFaqAll();
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


        //public ActionResult Add()
        //{
        //    string json = "";
        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_idx")))
        //        message += "faq_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_group_main_idx")))
        //        message += "faq_group_main_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_group_sub_idx")))
        //        message += "faq_group_sub_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("faq_q_name")))
        //        message += "faq_q_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("faq_a_name")))
        //        message += "faq_a_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
        //        message += "use_yn is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("create_date")))
        //        message += "create_date is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("create_user_idx")))
        //        message += "create_user_idx is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        bool isOK = biz.AddFaq(	WebUtility.GetRequestByInt("faq_idx"),
        //                                    WebUtility.GetRequestByInt("faq_group_main_idx"),
        //                                    WebUtility.GetRequestByInt("faq_group_sub_idx"),
        //                                    WebUtility.GetRequest("faq_q_name"),
        //                                    WebUtility.GetRequest("faq_a_name"),
        //                                    WebUtility.GetRequest("use_yn"),
        //                                    WebUtility.GetRequestByDateTime("create_date"),
        //                                    WebUtility.GetRequestByInt("create_user_idx"));

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

        //public ActionResult Modify()
        //{
        //    string json = "";
        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_idx")))
        //        message += "faq_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_group_main_idx")))
        //        message += "faq_group_main_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_group_sub_idx")))
        //        message += "faq_group_sub_idx is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("faq_q_name")))
        //        message += "faq_q_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("faq_a_name")))
        //        message += "faq_a_name is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
        //        message += "use_yn is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("update_date")))
        //        message += "update_date is null.";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("update_user_idx")))
        //        message += "update_user_idx is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        bool isOK = biz.ModifyFaq(	WebUtility.GetRequestByInt("faq_idx"),
        //                                        WebUtility.GetRequestByInt("faq_group_main_idx"),
        //                                        WebUtility.GetRequestByInt("faq_group_sub_idx"),
        //                                        WebUtility.GetRequest("faq_q_name"),
        //                                        WebUtility.GetRequest("faq_a_name"),
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

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("faq_idx")))
        //        message += "faq_idx is null.";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    try
        //    {
        //        bool isOK = biz.RemoveFaq(WebUtility.GetRequestByInt("faq_idx"));

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
	}
}

