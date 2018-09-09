using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;

namespace Storichain.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult NeedToLogin()
        {
            string json = DataTypeUtility.JSon("4000", Config.R_NEED_TO_LOGIN, @"need to login", null);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Get() 
        {
            //Biz_Event biz_event     = new Biz_Event();
            //Biz_Channel biz_channel = new Biz_Channel();
            //Biz_Product biz_product = new Biz_Product();
            //Biz_Board biz_board     = new Biz_Board();
            //Biz_Coupon biz_coupon   = new Biz_Coupon();
            //Biz_Banner biz_banner   = new Biz_Banner();

            //DataTable dtEvent = biz_event.GetEventTop(15);
            //DataTable dt0 = dtEvent.Clone();
            //DataTable dt1 = dtEvent.Clone();
            //DataTable dt2 = dtEvent.Clone();
            //DataTable dt3 = dtEvent.Clone();

            //for(int i = 0; i < 1; i++)
            //{
            //    dt0.ImportRow(dtEvent.Rows[i]);
            //}

            //for(int i = 1; i < 5; i++)
            //{
            //    dt1.ImportRow(dtEvent.Rows[i]);
            //}

            //for(int i = 5; i < 9; i++)
            //{
            //    dt2.ImportRow(dtEvent.Rows[i]);
            //}

            //for(int i = 9; i < 15; i++)
            //{
            //    dt3.ImportRow(dtEvent.Rows[i]);
            //}

            //DataTable dtLayout = new DataTable();
            //DataRow drLayout = null;
            //dtLayout.Columns.Add("index", typeof(int));
            //dtLayout.Columns.Add("type_key", typeof(string));
            //dtLayout.Columns.Add("type_name", typeof(string));
            //dtLayout.Columns.Add("type_desc", typeof(string));
            //dtLayout.Columns.Add("type_info", typeof(string));
            //dtLayout.Columns.Add("data", typeof(DataTable));
            //dtLayout.Columns.Add("data_count", typeof(int));
            
            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 0;
            //drLayout["type_key"]    = "top_event";
            //drLayout["type_name"]   = "";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "상단 기사";
            //drLayout["data"]        = dt0;
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 1;
            //drLayout["type_key"]    = "event_1";
            //drLayout["type_name"]   = "";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "기사";
            //drLayout["data"]        = dt1;
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 2;
            //drLayout["type_key"]    = "channel";
            //drLayout["type_name"]   = "시리즈";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "시리즈";
            //drLayout["data"]        = biz_channel.GetChannelTop(5);
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"] = 3;
            //drLayout["type_key"] = "banner_1";
            //drLayout["type_name"] = "광고배너";
            //drLayout["type_desc"] = "";
            //drLayout["type_info"] = "";
            //drLayout["data"] = biz_banner.GetBannerType(2);
            //drLayout["data_count"] = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 4;
            //drLayout["type_key"]    = "event_2";
            //drLayout["type_name"]   = "";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "기사";
            //drLayout["data"]        = dt2;
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"] = 5;
            //drLayout["type_key"] = "product";
            //drLayout["type_name"] = "추천 신상품";
            //drLayout["type_desc"] = "탄력에 신경써야할 계절 추천 상품";
            //drLayout["type_info"] = "새로운 상품 소개";
            //drLayout["data"] = biz_product.GetProductTop(3);
            //drLayout["data_count"] = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);



            ////Biz_VMainNewProduct biz_v_p = new Biz_VMainNewProduct();
            ////Biz_VMainNewProductRelation biz_v_r = new Biz_VMainNewProductRelation();
            ////DataTable dtP = biz_v_p.GetVMainNewProductValid();
            ////dtP.Columns.Add("product_data", typeof(DataTable));
            ////dtP.Columns.Add("product_data_count", typeof(int));

            ////foreach(DataRow drP in dtP.Rows)
            ////{
            ////    drP["product_data"] = biz_v_r.GetVMainNewProductRelation(drP.ItemValue("new_product_idx").ToInt());
            ////    drP["product_data_count"] = ((DataTable)drP["product_data"]).Rows.Count;
            ////}

            ////drLayout = dtLayout.NewRow();
            ////drLayout["index"]       = 3;
            ////drLayout["type_key"]    = "new_product";
            ////drLayout["type_name"]   = "추천 신상품";
            ////drLayout["type_desc"]   = "";
            ////drLayout["type_info"]   = "새로운 상품 소개";
            ////drLayout["data"]        = dtP;
            ////drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            ////dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"] = 3;
            //drLayout["type_key"] = "banner_2";
            //drLayout["type_name"] = "파트너 배너";
            //drLayout["type_desc"] = "";
            //drLayout["type_info"] = "";
            //drLayout["data"] = biz_banner.GetBannerType(3);
            //drLayout["data_count"] = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 6;
            //drLayout["type_key"]    = "board";
            //drLayout["type_name"]   = "이벤트";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "추천 이벤트";
            //drLayout["data"]        = biz_board.GetBoardTop(3);
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            //drLayout = dtLayout.NewRow();
            //drLayout["index"]       = 7;
            //drLayout["type_key"]    = "event_3";
            //drLayout["type_name"]   = "";
            //drLayout["type_desc"]   = "";
            //drLayout["type_info"]   = "기사";
            //drLayout["data"]        = dt3;
            //drLayout["data_count"]  = ((DataTable)drLayout["data"]).Rows.Count;
            //dtLayout.Rows.Add(drLayout);

            Biz_MainUiVersion biz = new Biz_MainUiVersion();
            string menu_name = "";


            // 커머스로 잠시 앱에서는 보이지 않토록한다. 11-17
            bool isAll = false;

            if(Request.UrlReferrer.ToValue().ToLower().Contains("beautytalk"))
            {
                isAll = true;
            }

            DataTable dt = biz.GetHomeData(ref menu_name, WebUtility.GetRequestByInt("day", 0), isAll);
            
            string json = DataTypeUtility.JSon("1000", Request.UrlReferrer.ToValue(), menu_name, dt);
            //string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, menu_name, dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetByDate() 
        {
            string json = "";
			string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("date")))
                message += "date is null.";

            if (!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            Biz_MainUiVersion biz = new Biz_MainUiVersion();
            
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", biz.GetHomeDataByDate(WebUtility.GetRequest("date") + " 23:59:59"));
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetService()
        {
            string json = "";
            string message = "";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_ServiceManager biz = new Biz_ServiceManager();
            DataTable dt = biz.Get();

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
