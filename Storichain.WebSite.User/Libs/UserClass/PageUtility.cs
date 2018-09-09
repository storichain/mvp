using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Web;
using System.IO;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Runtime.InteropServices;
using Storichain.Models.Biz;
using System.Text;
using System.Drawing.Imaging;
using Microsoft.AspNet.FriendlyUrls;

namespace Storichain
{
    public class PageUtility
    {
        public static string Idx(string var, int default_value = 0) 
        {
            string r = null;

            Page page = HttpContext.Current.Handler as Page;
            if(page != null)
            {
                r = page.RouteData.Values[var] as string; 
            }

            if (r.ToValue().Equals("")) 
            {
                if (HttpContext.Current.Request.GetFriendlyUrlSegments().Count > 0)
			    {
				    IList<string> segments = HttpContext.Current.Request.GetFriendlyUrlSegments();
                    return DataTypeUtility.GetToInt32(segments[0]).ToString();
			    }
			
                return WebUtility.GetRequestByInt(var, default_value).ToString();    
            }

            return r.ToInt().ToValue();
        }

        public static string UserIdx() 
        {
            if(WebUtility.GetConfig("DEV_YN").Equals("Y"))
            {
                if(WebUtility.GetRequestByInt("user_idx") > 0)
                {
                    return WebUtility.GetRequest("user_idx");
                }

                return DataTypeUtility.GetToInt32(HttpContext.Current.User.Identity.Name).ToString();
            }

            return DataTypeUtility.GetToInt32(HttpContext.Current.User.Identity.Name).ToString();
        }

        public static bool IsAuthenticated() 
        {
            if(WebUtility.GetConfig("DEV_YN").Equals("Y"))
            {
                if(WebUtility.GetRequestByInt("user_idx") > 0)
                {
                    return true;
                }

                return HttpContext.Current.User.Identity.IsAuthenticated;
            }

            return HttpContext.Current.User.Identity.IsAuthenticated;
        }

        public static string UserId()
        {
            if(!IsAuthenticated())
                return "";

            Biz_User biz = new Biz_User();
            return biz.GetUserID(UserIdx().ToInt()).ToValue();
        }
        
    }
}