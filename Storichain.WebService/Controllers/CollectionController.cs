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
	public class CollectionController : Controller
	{
		Biz_Collection biz = new Biz_Collection();
        Biz_CollectionEvent biz_coll_event = new Biz_CollectionEvent();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coll_idx")))
            //    message += "coll_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			//DataTable dt = biz.GetCollection(WebUtility.GetRequestByInt("coll_idx"));
            DataTable dt = biz.GetCollectionByUser(WebUtility.UserIdx(), WebUtility.GetRequestByInt("coll_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coll_name")))
				message += "coll_name is null.";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("coll_desc")))
            //    message += "coll_desc is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			try
			{
				int coll_idx = biz.AddCollection(	WebUtility.UserIdx(),
											        WebUtility.GetRequest("coll_name"),
											        WebUtility.GetRequest("coll_desc"),
											        WebUtility.GetRequest("use_yn", "Y"),
											        DateTime.Now,
											        WebUtility.UserIdx());

				if(coll_idx > 0)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coll_idx")))
				message += "coll_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			try
			{
                string file_path1 = "collection";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

				bool isOK = biz.ModifyCollection(	WebUtility.GetRequestByInt("coll_idx"),
												    WebUtility.UserIdx(),
												    WebUtility.GetRequest("coll_name"),
												    WebUtility.GetRequest("coll_desc"),
												    (WebUtility.GetRequestByInt("collection_file_idx") > 0)? WebUtility.GetRequestByInt("collection_file_idx") : BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
												    DateTime.Now,
												    WebUtility.UserIdx());

                DataTable dt = biz.GetCollection(WebUtility.GetRequestByInt("coll_idx"));

				if(isOK)
					json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coll_idx")))
				message += "coll_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			try
			{
				bool isOK = biz.RemoveCollection(WebUtility.UserIdx(), WebUtility.GetRequestByInt("coll_idx"));

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

        public ActionResult GetEventList()
		{
			string json = "";
			string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			DataTable dt = biz_coll_event.GetCollectionEvent(WebUtility.UserIdx(), WebUtility.GetRequestByInt("coll_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}


		public ActionResult AddEvent()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coll_idx")))
				message += "coll_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			try
			{
				bool isOK = biz_coll_event.AddCollectionEvent(	WebUtility.UserIdx(),
											                    WebUtility.GetRequestByInt("coll_idx"),
											                    WebUtility.GetRequestByInt("event_idx"),
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
		
		public ActionResult RemoveEvent()
		{
			string json = "";
			string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
				message += "user_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coll_idx")))
				message += "coll_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("event_idx")))
				message += "event_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

			try
			{
				bool isOK = biz_coll_event.RemoveCollectionEvent(WebUtility.UserIdx(), 
                                                                WebUtility.GetRequestByInt("coll_idx"), 
                                                                WebUtility.GetRequestByInt("event_idx"));

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

