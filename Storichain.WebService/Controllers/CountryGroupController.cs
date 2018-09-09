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
	public class CountryGroupController : Controller
	{
		Biz_CountryGroup biz = new Biz_CountryGroup();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetCountryGroup(WebUtility.GetRequestByInt("country_group_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_group_name")))
				message += "country_group_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.AddCountryGroup(	WebUtility.GetRequestByInt("country_group_idx"),
											        WebUtility.GetRequest("country_group_name"),
											        WebUtility.GetRequest("use_yn"),
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

		public ActionResult Modify()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("country_group_name")))
				message += "country_group_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.ModifyCountryGroup(	WebUtility.GetRequestByInt("country_group_idx"),
												    WebUtility.GetRequest("country_group_name"),
												    WebUtility.GetRequest("use_yn"),
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

		public ActionResult Remove()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("country_group_idx")))
				message += "country_group_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveCountryGroup(WebUtility.GetRequestByInt("country_group_idx"));

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



//http://camptalk.me/camptest/CountryGroup/Get?country_group_idx=0
//http://camptalk.me/camptest/CountryGroup/Add?country_group_idx=0&country_group_name=country_group_name&use_yn=use_yn&create_date=3&create_user_idx=4
//http://camptalk.me/camptest/CountryGroup/Modify?country_group_idx=0&country_group_name=country_group_name&use_yn=use_yn&update_date=3&update_user_idx=4
//http://camptalk.me/camptest/CountryGroup/Remove?country_group_idx=0

