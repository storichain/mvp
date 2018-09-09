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
	public class QuestionController : Controller
	{
		Biz_Question biz = new Biz_Question();

        public ActionResult Get()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
                message += "user_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetQuestion(0, WebUtility.GetRequestByInt("user_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetAnswerYn()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
                message += "user_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetQuestionAnswer(WebUtility.GetRequestByInt("user_idx"));
            bool answerYn = false;
            if (dt.Rows.Count > 0) {
                answerYn = true;
            }
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", answerYn);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }


        public ActionResult Add()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
                message += "user_idx is null.";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("question_type_idx")))
                message += "question_type_idx is null.";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("title")))
                message += "title is null.";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("question_content")))
                message += "question_content is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.AddQuestion(WebUtility.GetRequestByInt("user_idx"),
                                            WebUtility.GetRequestByInt("question_type_idx"),
                                            WebUtility.GetRequest("title"),
                                            WebUtility.GetRequest("question_content"),
                                            DateTime.Now,
                                            WebUtility.GetRequestByInt("user_idx"));

                if (isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

        public ActionResult Modify()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
                message += "user_idx is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.ModifyReadYn(   DateTime.Now,
                                                WebUtility.GetRequestByInt("user_idx"));

                if (isOK)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
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

        
    }
}

