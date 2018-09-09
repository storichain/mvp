using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Market.Biz;
using Newtonsoft.Json;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace Storichain.Controllers
{
	public class SearchController : Controller
	{
        //public ActionResult Get() 
        //{
        //    string json = "";

        //    string message = "";

        //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_text")))
        //        message += "search_text is null.\n";

        //    if(!message.Equals(""))
        //    {
        //        json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //        return Content(json, "application/json", System.Text.Encoding.UTF8);
        //    }

        //    var client      = new MongoClient(WebUtility.GetConfig("MONGODB_SEARCH_URL"));
        //    var server      = client.GetServer();
        //    var database    = server.GetDatabase("search_db");
        //    var collection  = database.GetCollection<SearchData>("search_data");

        //    var query = Query.Matches("search_where", new BsonRegularExpression("/" + WebUtility.GetRequest("search_text") + "/i"));
        //    //var query = Query.Matches("search_where", BsonRegularExpression.Create(new Regex(WebUtility.GetRequest("search_text"))));
            
        //    var d = collection.Find(query).SetSortOrder(SortBy.Descending("item_count"));
        //    d.Limit = 100;

        //    DataTable dt = new DataTable();
        //    DataRow dr;
        //    dt.Columns.Add("search_text", typeof(string));
        //    dt.Columns.Add("search_where", typeof(string));
        //    dt.Columns.Add("item_count", typeof(int));

        //    foreach(var p in d) 
        //    {
        //        dr = dt.NewRow();
        //        dr["search_text"]   = p.search_text;
        //        dr["search_where"]  = p.search_where;
        //        dr["item_count"]    = p.item_count;
        //        dt.Rows.Add(dr);
        //    }

        //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}
	}
}

