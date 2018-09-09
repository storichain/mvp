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
	public class CouponController : Controller
	{
		Biz_Coupon biz = new Biz_Coupon();
        Biz_CouponUser biz_user = new Biz_CouponUser();
        Biz_CouponCustomLog biz_log = new Biz_CouponCustomLog();

		public ActionResult Get()
		{
			string json = "";
            //string message = "";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
            //    message += "coupon_idx is null.";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
            //    message += "user_idx is null.";

            //if (!message.Equals(""))
            //{
            //    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
            //    return Content(json, "application/json", System.Text.Encoding.UTF8);
            //}

            DataTable dt = biz.GetCoupon(WebUtility.GetRequestByInt("coupon_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetList()
		{
			string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
            //    message += "user_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetCouponList(WebUtility.GetRequestByInt("coupon_idx"), WebUtility.GetRequestByInt("user_idx"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetEventList()
		{
			string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetEventList(WebUtility.GetRequestByInt("coupon_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult GetLists() 
        {
            int total_count;
            int page_count;

            string json = "";
			string message = "";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
            //    message += "user_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetLists(WebUtility.GetRequestByInt("user_idx"), 
                                        WebUtility.GetRequest("ing_yn", "Y"), 
                                        WebUtility.GetRequestByInt("page", 1), 
                                        WebUtility.GetRequestByInt("page_rows", 18),
                                        out total_count,
                                        out page_count);

            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", BizUtility.GetImageData(dt), dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult DownloadCoupon()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                int return_type = biz_user.AddCouponUser(	WebUtility.GetRequestByInt("coupon_idx"),
											                WebUtility.GetRequestByInt("user_idx"),
											                WebUtility.GetRequest("user_name"),
											                WebUtility.GetRequest("user_phone"),
											                WebUtility.GetRequest("user_email"),
											                WebUtility.GetRequest("zip_code"),
											                WebUtility.GetRequest("address1"),
											                WebUtility.GetRequest("address2"),
											                "N",
											                DateTime.Now,
											                WebUtility.UserIdx());

                if(return_type == 1)
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else if(return_type == 2)
                {
                    //만료된 쿠폰
                    json = DataTypeUtility.JSon("2801", Config.R_FAIL, "기간이 만료된 쿠폰입니다.", null);
                }
                else if(return_type == 3)
                {
                    //이미 다운받은 쿠폰
                    json = DataTypeUtility.JSon("2802", Config.R_FAIL, "이미 다운받은 쿠폰입니다.", null);
                }
                else if(return_type == 4)
                {
                    //사용된 쿠폰
                    json = DataTypeUtility.JSon("2803", Config.R_FAIL, "사용된 쿠폰입니다.", null);
                }
                else if(return_type == 5)
                {
                    //존재하지 않는 쿠폰
                    json = DataTypeUtility.JSon("2804", Config.R_FAIL, "존재하지 않는 쿠폰입니다.", null);
                }
                else if(return_type == 6)
                {
                    //보유쿠폰수량 소진
                    json = DataTypeUtility.JSon("2805", Config.R_FAIL, "쿠폰이 솔드아웃되었습니다.", null);
                }
			}
			catch(Exception ex)
			{
				BizUtility.SendErrorLog(Request, ex);
				json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
			}

			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

		public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_type_idx")))
				message += "coupon_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_name")))
				message += "coupon_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_name_sub")))
				message += "coupon_name_sub is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_desc")))
				message += "coupon_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_use_desc")))
				message += "coupon_use_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("ad_desc")))
				message += "ad_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("share_desc")))
				message += "share_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_code")))
				message += "coupon_code is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_ea")))
				message += "coupon_ea is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("download_ea")))
				message += "download_ea is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_income")))
				message += "coupon_income is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_start_date")))
				message += "coupon_start_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_end_date")))
				message += "coupon_end_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_use_start_date")))
				message += "coupon_use_start_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_use_end_date")))
				message += "coupon_use_end_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("push_use_yn")))
				message += "push_use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("push_reserve_date")))
				message += "push_reserve_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("push_type_idx")))
				message += "push_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("view_cnt")))
				message += "view_cnt is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_icon1_file_idx")))
				message += "coupon_icon1_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_icon2_file_idx")))
				message += "coupon_icon2_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_file_idx")))
				message += "coupon_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_cover_file_idx")))
				message += "coupon_cover_file_idx is null.";

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
				int coupon_idx = biz.AddCoupon(	WebUtility.GetRequestByInt("brand_idx"),
											    WebUtility.GetRequestByInt("product_idx"),
											    WebUtility.GetRequestByInt("coupon_type_idx"),
											    WebUtility.GetRequest("coupon_name"),
											    WebUtility.GetRequest("coupon_name_sub"),
											    WebUtility.GetRequest("coupon_desc"),
											    WebUtility.GetRequest("coupon_use_desc"),
                                                WebUtility.GetRequest("coupon_info"),
											    WebUtility.GetRequest("ad_desc"),
											    WebUtility.GetRequest("share_desc"),
											    WebUtility.GetRequest("coupon_code"),
											    WebUtility.GetRequestByInt("coupon_ea"),
											    WebUtility.GetRequestByInt("download_ea"),
											    WebUtility.GetRequestByInt("coupon_income"),
											    WebUtility.GetRequestByDateTime("coupon_start_date"),
											    WebUtility.GetRequestByDateTime("coupon_end_date"),
											    WebUtility.GetRequestByDateTime("coupon_use_start_date"),
											    WebUtility.GetRequestByDateTime("coupon_use_end_date"),
											    WebUtility.GetRequest("push_use_yn"),
											    WebUtility.GetRequestByDateTime("push_reserve_date"),
											    WebUtility.GetRequestByInt("push_type_idx"),
											    WebUtility.GetRequestByInt("view_cnt"),
											    WebUtility.GetRequestByInt("coupon_icon1_file_idx"),
											    WebUtility.GetRequestByInt("coupon_icon2_file_idx"),
											    WebUtility.GetRequestByInt("coupon_file_idx"),
											    WebUtility.GetRequestByInt("coupon_cover_file_idx"),
                                                WebUtility.GetRequest("coupon_use_place"),
                                                WebUtility.GetRequest("coupon_use_how"),
                                                WebUtility.GetRequest("coupon_use_url"),
											    WebUtility.GetRequest("use_yn"),
											    WebUtility.GetRequestByDateTime("create_date"),
											    WebUtility.GetRequestByInt("create_user_idx"));

				if(coupon_idx > 1)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("brand_idx")))
				message += "brand_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("product_idx")))
				message += "product_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_type_idx")))
				message += "coupon_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_name")))
				message += "coupon_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_name_sub")))
				message += "coupon_name_sub is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_desc")))
				message += "coupon_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_use_desc")))
				message += "coupon_use_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("ad_desc")))
				message += "ad_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("share_desc")))
				message += "share_desc is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("coupon_code")))
				message += "coupon_code is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_ea")))
				message += "coupon_ea is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("download_ea")))
				message += "download_ea is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_income")))
				message += "coupon_income is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_start_date")))
				message += "coupon_start_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_end_date")))
				message += "coupon_end_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_use_start_date")))
				message += "coupon_use_start_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("coupon_use_end_date")))
				message += "coupon_use_end_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("push_use_yn")))
				message += "push_use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("push_reserve_date")))
				message += "push_reserve_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("push_type_idx")))
				message += "push_type_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("view_cnt")))
				message += "view_cnt is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_icon1_file_idx")))
				message += "coupon_icon1_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_icon2_file_idx")))
				message += "coupon_icon2_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_file_idx")))
				message += "coupon_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_cover_file_idx")))
				message += "coupon_cover_file_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("use_yn")))
				message += "use_yn is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("update_date")))
				message += "update_date is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.ModifyCoupon(	WebUtility.GetRequestByInt("coupon_idx"),
												WebUtility.GetRequestByInt("brand_idx"),
												WebUtility.GetRequestByInt("product_idx"),
												WebUtility.GetRequestByInt("coupon_type_idx"),
												WebUtility.GetRequest("coupon_name"),
												WebUtility.GetRequest("coupon_name_sub"),
												WebUtility.GetRequest("coupon_desc"),
												WebUtility.GetRequest("coupon_use_desc"),
                                                WebUtility.GetRequest("coupon_info"),
												WebUtility.GetRequest("ad_desc"),
												WebUtility.GetRequest("share_desc"),
												WebUtility.GetRequest("coupon_code"),
												WebUtility.GetRequestByInt("coupon_ea"),
												WebUtility.GetRequestByInt("download_ea"),
												WebUtility.GetRequestByInt("coupon_income"),
												WebUtility.GetRequestByDateTime("coupon_start_date"),
												WebUtility.GetRequestByDateTime("coupon_end_date"),
												WebUtility.GetRequestByDateTime("coupon_use_start_date"),
												WebUtility.GetRequestByDateTime("coupon_use_end_date"),
												WebUtility.GetRequest("push_use_yn"),
												WebUtility.GetRequestByDateTime("push_reserve_date"),
												WebUtility.GetRequestByInt("push_type_idx"),
												WebUtility.GetRequestByInt("view_cnt"),
												WebUtility.GetRequestByInt("coupon_icon1_file_idx"),
												WebUtility.GetRequestByInt("coupon_icon2_file_idx"),
												WebUtility.GetRequestByInt("coupon_file_idx"),
												WebUtility.GetRequestByInt("coupon_cover_file_idx"),
                                                WebUtility.GetRequest("coupon_use_place"),
                                                WebUtility.GetRequest("coupon_use_how"),
                                                WebUtility.GetRequest("coupon_use_url"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveCoupon(WebUtility.GetRequestByInt("coupon_idx"));

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

        public ActionResult UseCoupon()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                bool isOK = biz_user.ModifyCouponUsed(  WebUtility.GetRequestByInt("coupon_idx"), 
                                                        WebUtility.GetRequestByInt("user_idx"), 
                                                        "Y", 
                                                        DateTime.Now, 
                                                        WebUtility.UserIdx());

				if(isOK)
					json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
				else
					json = DataTypeUtility.JSon("2000", Config.R_FAIL, "이미 사용되었거나 미발급 받은 쿠폰입니다.", null);
			}
			catch(Exception ex)
			{
				BizUtility.SendErrorLog(Request, ex);
				json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
			}

			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult CheckPassword()
		{
			string json = "";
			string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("password")))
				message += "password is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                Biz_Coupon biz_coupon = new Biz_Coupon();
                DataTable dt = biz_coupon.GetCode(WebUtility.GetRequestByInt("coupon_idx"), WebUtility.GetRequest("password"));

                if(dt.Rows.Count > 0)
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "패스워드가 틀립니다.", null);
                }
			}
			catch(Exception ex)
			{
				BizUtility.SendErrorLog(Request, ex);
				json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
			}

			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult ShareSNS()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
                message += "coupon_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
                message += "sns_type_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                Biz_UserSNSShare biz = new Biz_UserSNSShare();
                bool isOK = biz.AddUserSSNSShareCoupon( WebUtility.UserIdx(),
                                                        WebUtility.GetRequestByInt("coupon_idx"),
                                                        WebUtility.GetRequestByInt("sns_type_idx"),
                                                        DateTime.Now,
                                                        WebUtility.UserIdx());

                if (isOK)
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch (Exception ex)
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        public ActionResult AddCustomLog()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("coupon_idx")))
				message += "coupon_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("num_id")))
				message += "num_id is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
				message += "user_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
                if(WebUtility.GetRequestByInt("num_id") == 1)
                {
                    DataTable dt = biz_log.GetCouponCustomLogBySubKey(WebUtility.GetRequestByInt("coupon_idx"),
											                            WebUtility.GetRequestByInt("num_id"),
											                            WebUtility.GetRequestByInt("user_idx"));

                    if(dt.Rows.Count > 0)
                    {
                        json = DataTypeUtility.JSon("2100", Config.R_SUCCESS, "", null);
                        return Content(json, "application/json", System.Text.Encoding.UTF8);
                    }
                }

				int log_idx = biz_log.AddCouponCustomLog(	WebUtility.GetRequestByInt("coupon_idx"),
											                WebUtility.GetRequestByInt("num_id"),
											                WebUtility.GetRequestByInt("user_idx"),
                                                            WebUtility.GetIpAddress(),
											                Request.UserAgent,
											                DateTime.Now,
											                WebUtility.GetRequestByInt("user_idx"));

				if(log_idx > 0)
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



//http://camptalk.me/camptest/Coupon/Get?coupon_idx=0
//http://camptalk.me/camptest/Coupon/Add?coupon_idx=0&brand_idx=1&product_idx=2&coupon_type_idx=3&coupon_name=coupon_name&coupon_name_sub=coupon_name_sub&coupon_desc=coupon_desc&coupon_use_desc=coupon_use_desc&ad_desc=ad_desc&share_desc=share_desc&coupon_code=coupon_code&coupon_ea=11&download_ea=12&coupon_income=13&coupon_start_date=14&coupon_end_date=15&coupon_use_start_date=16&coupon_use_end_date=17&push_use_yn=push_use_yn&push_reserve_date=19&push_type_idx=20&view_cnt=21&coupon_icon1_file_idx=22&coupon_icon2_file_idx=23&coupon_file_idx=24&coupon_cover_file_idx=25&use_yn=use_yn&create_date=27&create_user_idx=28&coupon_id=coupon_id&coupon_kind=coupon_kind&file_rname=file_rname&file_directory=file_directory&file_url=file_url&ci_file_rname=ci_file_rname&ci_file_directory=ci_file_directory&ci_file_url=ci_file_url&ci2_file_rname=ci2_file_rname&ci2_file_directory=ci2_file_directory&ci2_file_url=ci2_file_url&cover_rname=cover_rname&cover_directory=cover_directory&cover_url=cover_url
//http://camptalk.me/camptest/Coupon/Modify?coupon_idx=0&brand_idx=1&product_idx=2&coupon_type_idx=3&coupon_name=coupon_name&coupon_name_sub=coupon_name_sub&coupon_desc=coupon_desc&coupon_use_desc=coupon_use_desc&ad_desc=ad_desc&share_desc=share_desc&coupon_code=coupon_code&coupon_ea=11&download_ea=12&coupon_income=13&coupon_start_date=14&coupon_end_date=15&coupon_use_start_date=16&coupon_use_end_date=17&push_use_yn=push_use_yn&push_reserve_date=19&push_type_idx=20&view_cnt=21&coupon_icon1_file_idx=22&coupon_icon2_file_idx=23&coupon_file_idx=24&coupon_cover_file_idx=25&use_yn=use_yn&update_date=27&update_user_idx=28&coupon_id=coupon_id&coupon_kind=coupon_kind&file_rname=file_rname&file_directory=file_directory&file_url=file_url&ci_file_rname=ci_file_rname&ci_file_directory=ci_file_directory&ci_file_url=ci_file_url&ci2_file_rname=ci2_file_rname&ci2_file_directory=ci2_file_directory&ci2_file_url=ci2_file_url&cover_rname=cover_rname&cover_directory=cover_directory&cover_url=cover_url
//http://camptalk.me/camptest/Coupon/Remove?coupon_idx=0

