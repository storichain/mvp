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
	public class BannerController : Controller
	{
		Biz_Banner biz              = new Biz_Banner();
        Biz_BannerLog biz_log       = new Biz_BannerLog();
        Biz_BannerKeyVisual biz_key = new Biz_BannerKeyVisual();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_idx")))
				message += "banner_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetBanner(WebUtility.GetRequestByInt("banner_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetKeyVisual()
		{
			string json = "";
			string message = "";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz_key.GetAvailable();
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetVideoLists() 
        {
            int total_count;
            int page_count;

            DataTable dt = biz.GetBannerLists(   "", 
                                                 "N",
                                                 3, 
                                                 WebUtility.GetRequestByInt("page", 1), 
                                                 WebUtility.GetRequestByInt("page_rows", 20),
                                                 out total_count,
                                                 out page_count);

            dt.Columns.Add("movie_name", typeof(string));

            foreach(DataRow dr in dt.Rows)
            {
                if(dr["clip_name"].ToString().Equals("")) 
                    dr["movie_name"] = dr["banner_name"].ToValue();
                else
                    dr["movie_name"] = dr["clip_name"].ToValue();
            }

            dt.Columns.Remove("banner_name");
            dt.Columns.Remove("clip_name");
            dt.Columns.Remove("banner_type_idx");
            dt.Columns.Remove("banner_type_name");
            dt.Columns.Remove("valid_yn");
            dt.Columns.Remove("publish_start_date");
            dt.Columns.Remove("publish_end_date");
            dt.Columns.Remove("banner_home_view");
            dt.Columns.Remove("banner_all_click");

            //banner_desc": "에뛰드(바자) 영상배너(~3/10)",
            //"banner_type_idx": 3,
            //"banner_type_name": "영상 배너",
            //"banner_url": "https://www.youtube.com/watch?v=fCeai3dzSuU",
            //"valid_yn": "N",
            //"publish_start_date": "2017-03-10 00:00:00",
            //"publish_end_date": "2017-03-10 23:59:59",
            //"ad_yn": "N",
            //"use_yn": "Y",
            //"banner_home_view": 3226,
            //"banner_all_click": 60,

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt,dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Go()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_idx")))
				message += "banner_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			int banner_log_idx = biz_log.AddBannerLog(  WebUtility.GetRequestByInt("banner_idx"), 
                                                        WebUtility.UserIdx(), 
                                                        WebUtility.GetIpAddress(),
                                                        Request.UserAgent,
                                                        DateTime.Now, 
                                                        WebUtility.UserIdx());

            if(banner_log_idx > 0)
            {
                biz = new Biz_Banner(WebUtility.GetRequestByInt("banner_idx"));
                return Redirect(biz.BannerUrl);
            }

			json = DataTypeUtility.JSon("2000", Config.R_ERROR, "", null);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}




		//public ActionResult Add()
		//{
		//	string json = "";
		//	string message = "";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequest("banner_name")))
		//		message += "banner_name is null.";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_type_idx")))
		//		message += "banner_type_idx is null.";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequest("banner_url")))
		//		message += "banner_url is null.";

		//	if(!message.Equals(""))
		//	{
		//		json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
		//		return Content(json, "application/json", System.Text.Encoding.UTF8);
		//	}

		//	try
		//	{
  //              string file_path1 = "banner";
  //              string image_key1 = "";
  //              ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

		//		int banner_idx = biz.AddBanner(	WebUtility.GetRequest("banner_name"),
		//									    WebUtility.GetRequest("banner_desc"),
		//									    WebUtility.GetRequestByInt("banner_type_idx", 1),
		//									    WebUtility.GetRequest("banner_url"),
		//									    (WebUtility.GetRequestByInt("banner_file_idx") > 0)? WebUtility.GetRequestByInt("banner_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
		//									    WebUtility.GetRequest("use_yn", "Y"),
		//									    DateTime.Now,
		//									    WebUtility.UserIdx());

		//		if(banner_idx > 0)
		//			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
		//		else
		//			json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
		//	}
		//	catch(Exception ex)
		//	{
		//		BizUtility.SendErrorLog(Request, ex);
		//		json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
		//	}

		//	return Content(json, "application/json", System.Text.Encoding.UTF8);
		//}

		//public ActionResult Modify()
		//{
		//	string json = "";
		//	string message = "";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_idx")))
		//		message += "banner_idx is null.";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequest("banner_name")))
		//		message += "banner_name is null.";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequest("banner_desc")))
		//		message += "banner_desc is null.";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequest("banner_url")))
		//		message += "banner_url is null.";

		//	if(!message.Equals(""))
		//	{
		//		json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
		//		return Content(json, "application/json", System.Text.Encoding.UTF8);
		//	}

		//	try
		//	{
  //              string file_path1 = "banner";
  //              string image_key1 = "";
  //              ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

		//		bool isOK = biz.ModifyBanner(	WebUtility.GetRequestByInt("banner_idx"),
		//										WebUtility.GetRequest("banner_name"),
		//										WebUtility.GetRequest("banner_desc"),
		//										WebUtility.GetRequestByInt("banner_type_idx", 1),
		//										WebUtility.GetRequest("banner_url"),
		//										(WebUtility.GetRequestByInt("banner_file_idx") > 0)? WebUtility.GetRequestByInt("banner_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
		//										WebUtility.GetRequest("use_yn", "Y"),
		//										DateTime.Now,
		//									    WebUtility.UserIdx());

		//		if(isOK)
		//			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
		//		else
		//			json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
		//	}
		//	catch(Exception ex)
		//	{
		//		BizUtility.SendErrorLog(Request, ex);
		//		json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
		//	}

		//	return Content(json, "application/json", System.Text.Encoding.UTF8);
		//}

		//public ActionResult Remove()
		//{
		//	string json = "";
		//	string message = "";

		//	if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("banner_idx")))
		//		message += "banner_idx is null.";

		//	if(!message.Equals(""))
		//	{
		//		json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
		//		return Content(json, "application/json", System.Text.Encoding.UTF8);
		//	}

		//	try
		//	{
		//		bool isOK = biz.RemoveBanner(WebUtility.GetRequestByInt("banner_idx"));

		//		if(isOK)
		//			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
		//		else
		//			json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
		//	}
		//	catch(Exception ex)
		//	{
		//		BizUtility.SendErrorLog(Request, ex);
		//		json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
		//	}

		//	return Content(json, "application/json", System.Text.Encoding.UTF8);
		//}
	}
}



//http://camptalk.me/camptest/Banner/Get?banner_idx=0
//http://camptalk.me/camptest/Banner/Add?banner_name=banner_name&banner_desc=banner_desc&banner_type_idx=2&banner_url=banner_url&banner_file_idx=4&use_yn=use_yn&create_date=6&create_user_idx=7
//http://camptalk.me/camptest/Banner/Modify?banner_idx=0&banner_name=banner_name&banner_desc=banner_desc&banner_type_idx=3&banner_url=banner_url&banner_file_idx=5&use_yn=use_yn&update_date=7&update_user_idx=8
//http://camptalk.me/camptest/Banner/Remove?banner_idx=0

