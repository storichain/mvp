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
	public class StepImgTagController : Controller
	{
		Biz_StepImgTag biz = new Biz_StepImgTag();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("img_tag_idx")))
				message += "img_tag_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetStepImgTag(WebUtility.GetRequestByInt("img_tag_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("s_idx")))
				message += "s_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_name")))
				message += "img_tag_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_desc")))
				message += "img_tag_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("img_tag_x_pos")))
				message += "img_tag_x_pos is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("img_tag_y_pos")))
				message += "img_tag_y_pos is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_url")))
				message += "img_tag_url is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_popup_view_yn")))
				message += "use_popup_view_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("create_date")))
				message += "create_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("create_user_idx")))
				message += "create_user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				int img_tag_idx = biz.AddStepImgTag(	WebUtility.GetRequestByInt("s_idx"),
											WebUtility.GetRequest("img_tag_name"),
											WebUtility.GetRequest("img_tag_desc"),
											WebUtility.GetRequestByFloat("img_tag_x_pos"),
											WebUtility.GetRequestByFloat("img_tag_y_pos"),
											WebUtility.GetRequest("img_tag_url"),
											WebUtility.GetRequestByInt("product_idx"),
											WebUtility.GetRequest("use_popup_view_yn"),
											WebUtility.GetRequestByDateTime("create_date"),
											WebUtility.GetRequestByInt("create_user_idx"));

				if(img_tag_idx > 0)
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

		public ActionResult Modify()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("img_tag_idx")))
				message += "img_tag_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("s_idx")))
				message += "s_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_name")))
				message += "img_tag_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_desc")))
				message += "img_tag_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("img_tag_x_pos")))
				message += "img_tag_x_pos is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByFloat("img_tag_y_pos")))
				message += "img_tag_y_pos is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("img_tag_url")))
				message += "img_tag_url is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_popup_view_yn")))
				message += "use_popup_view_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("update_date")))
				message += "update_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("update_user_idx")))
				message += "update_user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.ModifyStepImgTag(	WebUtility.GetRequestByInt("img_tag_idx"),
												WebUtility.GetRequestByInt("s_idx"),
												WebUtility.GetRequest("img_tag_name"),
												WebUtility.GetRequest("img_tag_desc"),
												WebUtility.GetRequestByFloat("img_tag_x_pos"),
												WebUtility.GetRequestByFloat("img_tag_y_pos"),
												WebUtility.GetRequest("img_tag_url"),
												WebUtility.GetRequestByInt("product_idx"),
												WebUtility.GetRequest("use_popup_view_yn"),
												WebUtility.GetRequestByDateTime("update_date"),
												WebUtility.GetRequestByInt("update_user_idx"));

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

		public ActionResult Remove()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("img_tag_idx")))
				message += "img_tag_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveStepImgTag(WebUtility.GetRequestByInt("img_tag_idx"));

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



//http://camptalk.me/camptest/StepImgTag/Get?img_tag_idx=0
//http://camptalk.me/camptest/StepImgTag/Add?s_idx=0&img_tag_name=img_tag_name&img_tag_desc=img_tag_desc&img_tag_x_pos=3&img_tag_y_pos=4&img_tag_url=img_tag_url&product_idx=6&use_popup_view_yn=use_popup_view_yn&create_date=8&create_user_idx=9
//http://camptalk.me/camptest/StepImgTag/Modify?img_tag_idx=0&s_idx=1&img_tag_name=img_tag_name&img_tag_desc=img_tag_desc&img_tag_x_pos=4&img_tag_y_pos=5&img_tag_url=img_tag_url&product_idx=7&use_popup_view_yn=use_popup_view_yn&update_date=9&update_user_idx=10
//http://camptalk.me/camptest/StepImgTag/Remove?img_tag_idx=0

