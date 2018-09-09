using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Collections;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Storichain.Controllers
{
    public class VersionController : Controller
    {
        public ActionResult GetAppVersion() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetDeviceTypeIdx()))
                message += "device_type_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            Biz_AppVersion biz = new Biz_AppVersion();
            DataTable dt = biz.GetAppVersionOne( WebUtility.GetDeviceTypeIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

    }
}
