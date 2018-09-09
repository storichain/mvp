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
	public class StepLinkController : Controller
	{
		Biz_StepLink biz = new Biz_StepLink();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
				message += "step_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetStepLink( WebUtility.GetRequestByInt("step_link_idx"),
                                            WebUtility.GetRequestByInt("event_idx"),
                                            WebUtility.GetRequestByInt("step_idx"),
                                            WebUtility.GetRequestByInt("step_link_seq"));

			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
				message += "step_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_seq")))
				message += "step_link_seq is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("position_x_rate")))
				message += "position_x_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("position_y_rate")))
				message += "position_y_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("step_link")))
				message += "step_link is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_file_idx")))
				message += "step_link_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

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
				int step_link_idx = biz.AddStepLink(	WebUtility.GetRequestByInt("event_idx"),
											WebUtility.GetRequestByInt("step_idx"),
											WebUtility.GetRequestByInt("step_link_seq"),
											WebUtility.GetRequestByInt("position_x_rate"),
											WebUtility.GetRequestByInt("position_y_rate"),
											WebUtility.GetRequest("step_link"),
											WebUtility.GetRequestByInt("sort_order"),
											WebUtility.GetRequestByInt("step_link_file_idx"),
											WebUtility.GetRequest("use_yn"),
											WebUtility.GetRequestByDateTime("create_date"),
											WebUtility.GetRequestByInt("create_user_idx"));

				if(step_link_idx > 0)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_idx")))
				message += "step_link_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_idx")))
				message += "step_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_seq")))
				message += "step_link_seq is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("position_x_rate")))
				message += "position_x_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("position_y_rate")))
				message += "position_y_rate is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("step_link")))
				message += "step_link is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_file_idx")))
				message += "step_link_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("update_date")))
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
				bool isOK = biz.ModifyStepLink(	WebUtility.GetRequestByInt("step_link_idx"),
												WebUtility.GetRequestByInt("event_idx"),
												WebUtility.GetRequestByInt("step_idx"),
												WebUtility.GetRequestByInt("step_link_seq"),
												WebUtility.GetRequestByInt("position_x_rate"),
												WebUtility.GetRequestByInt("position_y_rate"),
												WebUtility.GetRequest("step_link"),
												WebUtility.GetRequestByInt("sort_order"),
												WebUtility.GetRequestByInt("step_link_file_idx"),
												WebUtility.GetRequest("use_yn"),
												WebUtility.GetRequest("update_date"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("step_link_idx")))
				message += "step_link_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveStepLink(WebUtility.GetRequestByInt("step_link_idx"));

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



//http://camptalk.me/camptest/StepLink/Get?step_link_idx=0
//http://camptalk.me/camptest/StepLink/Add?event_idx=0&step_idx=1&step_link_seq=2&position_x_rate=3&position_y_rate=4&step_link=step_link&sort_order=6&step_link_file_idx=7&use_yn=use_yn&create_date=9&create_user_idx=10
//http://camptalk.me/camptest/StepLink/Modify?step_link_idx=0&event_idx=1&step_idx=2&step_link_seq=3&position_x_rate=4&position_y_rate=5&step_link=step_link&sort_order=7&step_link_file_idx=8&use_yn=use_yn&update_date=update_date&update_user_idx=11
//http://camptalk.me/camptest/StepLink/Remove?step_link_idx=0

