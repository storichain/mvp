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
	public class EventGroupController : Controller
	{
		Biz_EventGroup biz = new Biz_EventGroup();
        Biz_EventGroupList biz_list = new Biz_EventGroupList();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetEventGroup(WebUtility.GetRequestByInt("event_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetList()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

            if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz_list.GetEventGroupListByFromEvent(WebUtility.GetRequestByInt("event_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("event_group_name")))
				message += "event_group_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("event_group_desc")))
				message += "event_group_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_group_file_idx")))
				message += "event_group_file_idx is null.";

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
				bool isOK = biz.AddEventGroup(	WebUtility.GetRequestByInt("event_idx"),
											WebUtility.GetRequest("event_group_name"),
											WebUtility.GetRequest("event_group_desc"),
											WebUtility.GetRequestByInt("event_group_file_idx"),
											WebUtility.GetRequest("use_yn"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("event_group_name")))
				message += "event_group_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("event_group_desc")))
				message += "event_group_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_group_file_idx")))
				message += "event_group_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

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
				bool isOK = biz.ModifyEventGroup(	WebUtility.GetRequestByInt("event_idx"),
												WebUtility.GetRequest("event_group_name"),
												WebUtility.GetRequest("event_group_desc"),
												WebUtility.GetRequestByInt("event_group_file_idx"),
												WebUtility.GetRequest("use_yn"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveEventGroup(WebUtility.GetRequestByInt("event_idx"));

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



//http://camptalk.me/camptest/EventGroup/Get?event_idx=0
//http://camptalk.me/camptest/EventGroup/Add?event_idx=0&event_group_name=event_group_name&event_group_desc=event_group_desc&event_group_file_idx=3&use_yn=use_yn&create_date=5&create_user_idx=6
//http://camptalk.me/camptest/EventGroup/Modify?event_idx=0&event_group_name=event_group_name&event_group_desc=event_group_desc&event_group_file_idx=3&use_yn=use_yn&update_date=5&update_user_idx=6
//http://camptalk.me/camptest/EventGroup/Remove?event_idx=0

