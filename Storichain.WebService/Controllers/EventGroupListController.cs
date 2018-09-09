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
	public class EventGroupListController : Controller
	{
		Biz_EventGroupList biz = new Biz_EventGroupList();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("from_event_idx")))
				message += "from_event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_event_idx")))
				message += "to_event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetEventGroupList(	WebUtility.GetRequestByInt("from_event_idx"),
										WebUtility.GetRequestByInt("to_event_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("from_event_idx")))
				message += "from_event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_event_idx")))
				message += "to_event_idx is null.";

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
				bool isOK = biz.AddEventGroupList(	WebUtility.GetRequestByInt("from_event_idx"),
											WebUtility.GetRequestByInt("to_event_idx"),
											WebUtility.GetRequestByDateTime("create_date"),
											WebUtility.GetRequestByInt("create_user_idx"));

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

		public ActionResult Modify()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("from_event_idx")))
				message += "from_event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_event_idx")))
				message += "to_event_idx is null.";

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
				bool isOK = biz.ModifyEventGroupList(	WebUtility.GetRequestByInt("from_event_idx"),
												WebUtility.GetRequestByInt("to_event_idx"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("from_event_idx")))
				message += "from_event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_event_idx")))
				message += "to_event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveEventGroupList(	WebUtility.GetRequestByInt("from_event_idx"),
												WebUtility.GetRequestByInt("to_event_idx"));

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



//http://camptalk.me/camptest/EventGroupList/Get?from_event_idx=0&to_event_idx=1
//http://camptalk.me/camptest/EventGroupList/Add?from_event_idx=0&to_event_idx=1&create_date=2&create_user_idx=3
//http://camptalk.me/camptest/EventGroupList/Modify?from_event_idx=0&to_event_idx=1&update_date=2&update_user_idx=3
//http://camptalk.me/camptest/EventGroupList/Remove?from_event_idx=0&to_event_idx=1

