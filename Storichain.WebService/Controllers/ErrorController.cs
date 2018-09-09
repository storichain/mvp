using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Storichain.Models.Market.Biz;
using Newtonsoft.Json;
using Storichain.Models.Biz;

namespace Storichain.Controllers
{
	public class ErrorController : Controller
	{
		public ActionResult Add()
		{
            HttpRequest request = HttpContext.ApplicationInstance.Context.Request;

            Biz_ErrorLog error = new Biz_ErrorLog();
            error.AddAccessData(request.Url.PathAndQuery, WebUtility.GetRequest("message"), "Custom", DateTime.Now, WebUtility.UserIdx());

			string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}
        
	}
}
