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
    public class EventReadController : Controller
    {
        Biz_EventRead biz = new Biz_EventRead();

        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz.AddEventRead(   WebUtility.GetRequestByInt("event_idx"), 
                                                WebUtility.UserIdx(), 
                                                DateTime.Now, 
                                                WebUtility.GetDeviceTypeIdx(),
                                                DateTime.Now, 
                                                WebUtility.UserIdx());

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
