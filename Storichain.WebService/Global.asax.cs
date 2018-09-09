using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.ComponentModel;

namespace Storichain
{
    // 참고: IIS6 또는 IIS7 클래식 모드를 사용하도록 설정하는 지침을 보려면 
    // http://go.microsoft.com/?LinkId=9394801을 방문하십시오.

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            HttpContext.Current.Response.AddHeader("Access-Control-Allow-Origin", "*");
            /* HttpContext.Current.Response.AddHeader(
              "Access-Control-Allow-Origin", 
              "http://AllowedDomain.com"); */

            //NetworkDrives.MapDrive("O", "\\\\172.31.10.40\\storage", "Administrator", "f?@p!@Vqy)");
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // 경로 이름
                "{controller}/{action}/{id}", // 매개 변수가 있는 URL
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // 매개 변수 기본값
            );

        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
            Response.Headers.Remove("X-Powered-By");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void Application_End(object sender, EventArgs e)
        {
            //if(System.Web.HttpContext.Current.Application["worker"] == null)
            //{
            //    BackgroundWorker worker = (BackgroundWorker)System.Web.HttpContext.Current.Application["worker"];
            //    if (worker != null)
            //        worker.CancelAsync();
            //}
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            BizUtility.SendErrorLog(System.Web.HttpContext.Current.Request, exc); 
        }
    }
}