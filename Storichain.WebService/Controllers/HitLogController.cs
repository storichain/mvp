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
	public class HitLogController : Controller
	{
		Biz_HitLog biz = new Biz_HitLog();

		public ActionResult Add()
		{
			string json = "";
			string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("code_value")))
                message += "code_value is null.";

            //if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
            //    message += "user_idx is null.\n";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("object_idx")))
            //    message += "object_idx is null.";

            if (!message.Equals(""))
			{
				json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
				return Content(json, "application/json", System.Text.Encoding.UTF8);
			}

            int log_idx = biz.AddLog(WebUtility.GetRequest("code_value"), 
                                    WebUtility.UserIdx(), 
                                    WebUtility.GetRequestByInt("object_idx", 0), 
                                    Request.UserAgent, 
                                    WebUtility.GetIpAddress(), 
                                    DateTime.Now);

			json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", log_idx);
			return Content(json, "application/json", System.Text.Encoding.UTF8);
		}

        
	}
}

