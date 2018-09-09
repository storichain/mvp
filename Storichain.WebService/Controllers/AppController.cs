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
	public class AppController : Controller
	{
		public ActionResult GoToStore()
		{
            if(WebUtility.GetRequestByInt("device_type_idx", 0) == 2)
            {
                return Redirect(WebUtility.GetConfig("GOOGLE_PLAY_STORE_URL"));
            }

            var userAgent = Request.UserAgent.ToLower();
            if (userAgent.Contains("iphone") || userAgent.Contains("ipad"))
            {
                return Redirect(WebUtility.GetConfig("APP_STORE_URL"));
            }
                
            return Redirect(WebUtility.GetConfig("GOOGLE_PLAY_STORE_URL"));
		}
	}
}


