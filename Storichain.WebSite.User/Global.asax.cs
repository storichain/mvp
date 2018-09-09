using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.FriendlyUrls;
using Storichain;
using System.Security.Principal;

namespace Storichain.WebSite.User
{
    public class Global : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterFriendlyRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.IgnoreRoute("{myWebForms}.aspx/{*pathInfo}");
 
            // Web Forms default
            //routes.MapPageRoute("WebFormDefault", "", "~/pages/index.aspx");
            routes.MapPageRoute("WebFormDefault", "", "~/login.aspx");
            //routes.MapPageRoute("products", "products/{product_idx}", "~/Pages/Products.aspx");
            //routes.MapPageRoute("sellers", "sellers/{user_idx}", "~/Pages/Sellers.aspx");
            //routes.MapPageRoute("users", "users/{user_idx}", "~/Pages/Users.aspx");
            //routes.MapPageRoute("cate1", "category1/{cate_idx}", "~/Pages/Category1.aspx");
            //routes.MapPageRoute("cate2", "category2/{cate_idx}", "~/Pages/Category2.aspx");


            //routes.MapPageRoute("index", "index", "~/Pages/Index.aspx");
            //routes.MapPageRoute("ing", "ing", "~/Pages/Ing.aspx");
            //routes.MapPageRoute("edit", "edit", "~/Pages/Edit.aspx");
            //routes.MapPageRoute("login", "login", "~/Pages/Login.aspx");
            //routes.MapPageRoute("article", "article/{event_idx}", "~/Pages/Article.aspx");
            //routes.MapPageRoute("logout", "logout", "~/Pages/Logout.aspx");

            routes.MapRoute(
                "Shop_default",                          // Route name
                "Shop/{controller}/{action}/{id}",       // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Storichain.Models.Market.Biz" }
            );// Parameter defaults
 
            // MVC default
            routes.MapRoute(
                "Default",                          // Route name
                "{controller}/{action}/{id}",       // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Storichain.Controllers" }
            );// Parameter defaults
        }

        void Application_Start(object sender, EventArgs e)
        {
            AreaRegistration.RegisterAllAreas();
            RegisterFriendlyRoutes(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server");
            Response.Headers.Remove("X-AspNet-Version");
            Response.Headers.Remove("X-AspNetMvc-Version");
        }

        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            //if(WebUtility.GetConfig("HTTPS_USE_YN", "Y").Equals("Y"))
            //{
            //    HttpApplication app = (HttpApplication)sender; 
        
            //    if (!app.Request.Url.AbsoluteUri.StartsWith("https")) 
            //    { 
            //        app.Response.Redirect(Request.Url.AbsoluteUri.Replace("http://", "https://"), true); 
            //    }
            //}
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication)sender;

            if (app.Request.IsAuthenticated && Context.User.Identity.IsAuthenticated)
            {
                if (Context.User == null)
                {
                    SitePrincipal newUser = new SitePrincipal(Convert.ToInt32(Context.User.Identity.Name));
                    Context.User = newUser;
                }
            }
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            BizUtility.SendErrorLog(System.Web.HttpContext.Current.Request, exc); 
        }
    }
}