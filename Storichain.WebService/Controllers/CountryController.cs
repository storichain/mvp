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
	public class CountryController : Controller
	{
		Biz_Country biz = new Biz_Country();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_idx")))
				message += "country_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetCountry(WebUtility.GetRequestByInt("country_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_idx")))
				message += "country_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_name")))
				message += "country_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_name_en")))
				message += "country_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

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
				bool isOK = biz.AddCountry(	WebUtility.GetRequestByInt("country_idx"),
											WebUtility.GetRequestByInt("country_group_idx"),
											WebUtility.GetRequest("country_name"),
											WebUtility.GetRequest("country_name_en"),
											WebUtility.GetRequestByInt("sort_order"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_idx")))
				message += "country_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_name")))
				message += "country_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_name_en")))
				message += "country_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sort_order")))
				message += "sort_order is null.";

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
				bool isOK = biz.ModifyCountry(	WebUtility.GetRequestByInt("country_idx"),
												WebUtility.GetRequestByInt("country_group_idx"),
												WebUtility.GetRequest("country_name"),
												WebUtility.GetRequest("country_name_en"),
												WebUtility.GetRequestByInt("sort_order"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_idx")))
				message += "country_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveCountry(WebUtility.GetRequestByInt("country_idx"));

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



//http://camptalk.me/camptest/Country/Get?country_idx=0
//http://camptalk.me/camptest/Country/Add?country_idx=0&country_group_idx=1&country_name=country_name&country_name_en=country_name_en&sort_order=4&use_yn=use_yn&create_date=6&create_user_idx=7
//http://camptalk.me/camptest/Country/Modify?country_idx=0&country_group_idx=1&country_name=country_name&country_name_en=country_name_en&sort_order=4&use_yn=use_yn&update_date=6&update_user_idx=7
//http://camptalk.me/camptest/Country/Remove?country_idx=0

