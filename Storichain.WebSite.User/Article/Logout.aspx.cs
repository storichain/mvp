using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Storichain;
using Storichain.WebSite;
using System.Web.Security;

namespace Storichain.WebSite.User
{
    public partial class Logout : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            WebUtility.RemoveSession();
            WebUtility.RemoveSessionTemp();
            FormsAuthentication.SignOut();
            Response.Redirect("/login");
        }
    }
}