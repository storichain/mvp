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
	public class BrandController : Controller
	{
		Biz_Brand biz = new Biz_Brand();

		public ActionResult Get()
		{
			string json = "";
			//string message = "";

			//if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
			//	message += "brand_idx is null.";

			//if(!message.Equals(""))
			//{
			//	json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
			//	return Content(json, "application/json", System.Text.Encoding.UTF8);
			//}

			DataTable dt = biz.GetBrand(WebUtility.GetRequestByInt("brand_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetIndexAll()
        {
            string json = "";
            
            DataTable dt = biz.GetBrandIndexAll();
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_name")))
				message += "brand_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_name_en")))
				message += "brand_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_type_idx")))
				message += "brand_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_url")))
				message += "brand_url is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_name")))
				message += "charge_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_phone")))
				message += "charge_phone is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_email")))
				message += "charge_email is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_file_idx")))
				message += "brand_file_idx is null.";

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
				int brand_idx = biz.AddBrand(	WebUtility.GetRequest("brand_name"),
											    WebUtility.GetRequest("brand_name_en"),
											    WebUtility.GetRequestByInt("brand_type_idx"),
											    WebUtility.GetRequest("brand_url"),
											    WebUtility.GetRequest("charge_name"),
											    WebUtility.GetRequest("charge_phone"),
											    WebUtility.GetRequest("charge_email"),
                                                WebUtility.GetRequest("brand_init_name"),
											    WebUtility.GetRequestByInt("brand_file_idx"),
											    WebUtility.GetRequest("use_yn"),
											    WebUtility.GetRequestByDateTime("create_date"),
											    WebUtility.GetRequestByInt("create_user_idx"));

				if(brand_idx > 0)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_name")))
				message += "brand_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_name_en")))
				message += "brand_name_en is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_type_idx")))
				message += "brand_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("brand_url")))
				message += "brand_url is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_name")))
				message += "charge_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_phone")))
				message += "charge_phone is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("charge_email")))
				message += "charge_email is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_file_idx")))
				message += "brand_file_idx is null.";

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
				bool isOK = biz.ModifyBrand(	WebUtility.GetRequestByInt("brand_idx"),
												WebUtility.GetRequest("brand_name"),
												WebUtility.GetRequest("brand_name_en"),
												WebUtility.GetRequestByInt("brand_type_idx"),
												WebUtility.GetRequest("brand_url"),
												WebUtility.GetRequest("charge_name"),
												WebUtility.GetRequest("charge_phone"),
												WebUtility.GetRequest("charge_email"),
                                                WebUtility.GetRequest("brand_init_name"),
												WebUtility.GetRequestByInt("brand_file_idx"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveBrand(WebUtility.GetRequestByInt("brand_idx"));

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



//http://camptalk.me/camptest/Brand/Get?brand_idx=0
//http://camptalk.me/camptest/Brand/Add?brand_name=brand_name&brand_name_en=brand_name_en&brand_type_idx=2&brand_url=brand_url&charge_name=charge_name&charge_phone=charge_phone&charge_email=charge_email&brand_file_idx=7&use_yn=use_yn&create_date=9&create_user_idx=10&bid=bid&bkind=bkind&file_rname=file_rname&file_directory=file_directory&file_url=file_url
//http://camptalk.me/camptest/Brand/Modify?brand_idx=0&brand_name=brand_name&brand_name_en=brand_name_en&brand_type_idx=3&brand_url=brand_url&charge_name=charge_name&charge_phone=charge_phone&charge_email=charge_email&brand_file_idx=8&use_yn=use_yn&update_date=10&update_user_idx=11&bid=bid&bkind=bkind&file_rname=file_rname&file_directory=file_directory&file_url=file_url
//http://camptalk.me/camptest/Brand/Remove?brand_idx=0

