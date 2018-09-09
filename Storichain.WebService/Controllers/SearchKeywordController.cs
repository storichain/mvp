using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Text;

namespace Storichain.Controllers
{
	public class SearchKeywordController : Controller
	{
		Biz_SearchKeyword biz = new Biz_SearchKeyword();

		public ActionResult Get()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_idx")))
				message += "search_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			DataTable dt = biz.GetSearchKeyword(WebUtility.GetRequestByInt("search_idx"));
			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        public ActionResult Search()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text")))))
                message += "search_text is null.\n";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequest("search_text")))
            //    message += "search_text is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataSet ds = biz.SearchKeyword(Utf8Encoder.GetString(Utf8Encoder.GetBytes(GetSearchText(WebUtility.GetRequest("search_text")))));
            //DataSet ds = biz.SearchKeyword(WebUtility.GetRequest("search_text"));
            //DataSet ds = biz.SearchKeyword(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text"))));

            DataTable dt = new DataTable();
            DataRow dr = null;
            dt.Columns.Add("index", typeof(int));
            dt.Columns.Add("type_key", typeof(string));
            dt.Columns.Add("type_name", typeof(string));
            dt.Columns.Add("data", typeof(DataTable));
            dt.Columns.Add("data_count", typeof(int));
            
            dr = dt.NewRow();
            dr["index"]         = 0;
            dr["type_key"]      = "event";
            dr["type_name"]     = "관련기사";
            dr["data"]          = BizUtility.GetImageData(ds.Tables[0]);
            dr["data_count"]    = ((DataTable)dr["data"]).Rows.Count;
            dt.Rows.Add(dr);

            dr                  = dt.NewRow();
            dr["index"]         = 1;
            dr["type_key"]      = "product";
            dr["type_name"]     = "인기상품";
            dr["data"]          = BizUtility.GetImageData(ds.Tables[1]);
            dr["data_count"]    = ((DataTable)dr["data"]).Rows.Count;
            dt.Rows.Add(dr);

            dr                  = dt.NewRow();
            dr["index"]         = 2;
            dr["type_key"]      = "brand";
            dr["type_name"]     = "추천브랜드";
            dr["data"]          = BizUtility.GetImageData(ds.Tables[2]);
            dr["data_count"]    = ((DataTable)dr["data"]).Rows.Count;
            dt.Rows.Add(dr);

            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("post", BizUtility.GetImageData(ds.Tables[0]));
            //dic.Add("product", BizUtility.GetImageData(ds.Tables[1]));
            //dic.Add("brand", BizUtility.GetImageData(ds.Tables[2]));

            biz.AddSearchKeyword(WebUtility.GetRequest("search_text"), 
                ds.Tables[0].Rows.Count + ds.Tables[1].Rows.Count + ds.Tables[2].Rows.Count,
                DateTime.Now, WebUtility.UserIdx());
            
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private static readonly Encoding Utf8Encoder = Encoding.GetEncoding(
            "UTF-8",
            new EncoderReplacementFallback(string.Empty),
            new DecoderExceptionFallback()
        );

        public ActionResult SearchEventAllLists()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text")))))
                message += "search_text is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.SearchEventAllLists( Utf8Encoder.GetString(Utf8Encoder.GetBytes(GetSearchText(WebUtility.GetRequest("search_text")))),
            //DataTable dt = biz.SearchEventAllLists( Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text"))),
                                                    WebUtility.GetRequestByInt("page", 1),
                                                    WebUtility.GetRequestByInt("page_rows", 20),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SearchProductAllLists()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text")))))
                message += "search_text is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.SearchProductAllLists(Utf8Encoder.GetString(Utf8Encoder.GetBytes(GetSearchText(WebUtility.GetRequest("search_text")))),
            //DataTable dt = biz.SearchProductAllLists(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text"))),
                                                    WebUtility.GetRequestByInt("page", 1),
                                                    WebUtility.GetRequestByInt("page_rows", 20),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SearchBrandAllLists()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text")))))
                message += "search_text is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            int total_count;
            int page_count;

            DataTable dt = biz.SearchBrandAllLists(Utf8Encoder.GetString(Utf8Encoder.GetBytes(GetSearchText(WebUtility.GetRequest("search_text")))),
            //DataTable dt = biz.SearchBrandAllLists(Utf8Encoder.GetString(Utf8Encoder.GetBytes(WebUtility.GetRequest("search_text"))),
                                                    WebUtility.GetRequestByInt("page", 1),
                                                    WebUtility.GetRequestByInt("page_rows", 20),
                                                    out total_count,
                                                    out page_count);

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("total_row_count", total_count);
            dic.Add("page_count", page_count);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt, dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetBasicKeyword()
        {
            string json = "";
            //string message = "";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_idx")))
            //    message += "search_idx is null.";

            //if (!message.Equals(""))
            //{
            //    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
            //    return Content(json, "application/json", System.Text.Encoding.UTF8);
            //}

            DataTable dt = null;

            //if(WebUtility.GetRequestByInt("user_idx") > 0)
            //    dt = biz.GetSearchKeywordByUserIdx(WebUtility.GetRequestByInt("user_idx"));
            //else 
                dt = biz.GetSearchKeyword();

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRecentKeyword()
        {
            string json = "";
            DataTable dt = null;

            dt = biz.GetRecentKeyword(WebUtility.UserIdx());

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        private string GetSearchText(string searchText)
        {
            string origin   = searchText;
            string temp     = "";

            // 추천 검색어 
            Biz_SearchKeywordFeature biz_f = new Biz_SearchKeywordFeature();
            DataTable dtA = biz_f.GetSearchKeywordFeature(origin);
            bool isFirst1    = true;

            if (dtA.Rows.Count > 0)
            {
                DataRow drA = dtA.Rows[0];
                string[] strA = drA["target_names"].ToValue().Split(',');

                foreach (string str in strA)
                {
                    if (str.Equals(""))
                        continue;

                    if (isFirst1)
                    {
                        temp += string.Format("\"{0}*\"", str.Replace("'", "''"));
                        isFirst1 = false;
                    }
                    else
                    {
                        temp += string.Format(" OR \"{0}*\"", str.Replace("'", "''"));
                    }
                }
            }

            if (dtA.Rows.Count > 0 && !temp.Equals(""))
                return temp;

            searchText = searchText.Replace("+", " ");

            string[] strArr = searchText.Replace("+", " ").Split(' ');
            bool isFirst    = true;

            foreach (string str in strArr)
            {
                if (str.Equals(""))
                    continue;

                if (isFirst)
                {
                    temp += string.Format("\"{0}*\"", str.Replace("'", "''"));
                    isFirst = false;
                }
                else
                {
                    temp += string.Format(" AND \"{0}*\"", str.Replace("'", "''"));
                }

                //if (isFirst)
                //{
                //    temp += string.Format("{0}", str.Replace("'", "''"));
                //    isFirst = false;
                //}
                //else
                //{
                //    temp += string.Format(" {0}", str.Replace("'", "''"));
                //}
            }

            if (searchText.Contains(" "))
            {
                temp +=  string.Format(" OR \"{0}*\"", searchText.Replace(" ", "").Replace("'", "''"));
            }

            // 순위 검색어
            Biz_SearchKeywordAlter biz_a = new Biz_SearchKeywordAlter();
            dtA = biz_a.GetSearchKeywordAlter(searchText.Replace(" ", ""));

            if(dtA.Rows.Count > 0)
            {
                DataRow drA = dtA.Rows[0];
                string[] strA = drA["target_names"].ToValue().Split(',');

                foreach (string str in strA)
                {
                    if (str.Equals(""))
                        continue;

                    temp += string.Format(" OR \"{0}*\"", str.Replace("'", "''"));
                }
            }

            return temp;
        }

        public ActionResult Add()
		{
			string json = "";
			string message = "";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_name")))
				message += "search_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_result_count")))
				message += "search_result_count is null.";

			//if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("start_date")))
			//	message += "start_date is null.";

			//if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("end_date")))
			//	message += "end_date is null.";

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
				int search_idx = biz.AddSearchKeyword(	WebUtility.GetRequest("search_name"),
											            WebUtility.GetRequestByInt("search_result_count"),
											            //WebUtility.GetRequestByDateTime("start_date"),
											            //WebUtility.GetRequestByDateTime("end_date"),
											            WebUtility.GetRequestByDateTime("create_date"),
											            WebUtility.GetRequestByInt("create_user_idx"));

				if(search_idx > 0)
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_idx")))
				message += "search_idx is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_name")))
				message += "search_name is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_result_count")))
				message += "search_result_count is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("start_date")))
				message += "start_date is null.";

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByDateTime("end_date")))
				message += "end_date is null.";

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
				bool isOK = biz.ModifySearchKeyword(	WebUtility.GetRequestByInt("search_idx"),
												WebUtility.GetRequest("search_name"),
												WebUtility.GetRequestByInt("search_result_count"),
												WebUtility.GetRequestByDateTime("start_date"),
												WebUtility.GetRequestByDateTime("end_date"),
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

			if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("search_idx")))
				message += "search_idx is null.";

			if(!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

			try
			{
				bool isOK = biz.RemoveSearchKeyword(WebUtility.GetRequestByInt("search_idx"));

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

