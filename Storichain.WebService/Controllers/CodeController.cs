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
	public partial class CodeController : Controller
	{
        Biz_Code biz_code = new Biz_Code();

        public ActionResult GetList()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("code_group_idx")))
                message += "code_group_idx is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            string view_yn = WebUtility.GetRequest("view_yn", "Y");

            //if(WebUtility.GetRequest("view_yn").Equals("-"))
            //    view_yn = "";

            DataTable dt = biz_code.GetCodeList(WebUtility.GetRequestByInt("code_group_idx"),
                                                view_yn,
                                                WebUtility.GetRequestByInt("code_type_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

	}
}
