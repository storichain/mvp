using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Text;
using Storichain;

namespace Storichain.WebSite
{
	public class PageBaseUser : System.Web.UI.Page
	{
        //private int _user_idx;

  //      protected int USER_IDX
		//{
		//	get
		//	{
		//		return _user_idx;
		//	}
		//}

        public WriteMode PageWriteMode 
        {
            get 
            {
                if(ViewState["WRITE_MODE"] == null)
                    return WriteMode.None;

                return (WriteMode)ViewState["WRITE_MODE"];
            }
            set 
            {
                ViewState["WRITE_MODE"] = value;
            }
        }

        public bool IsLogined
        {
            get 
            {
                return Context.User.Identity.IsAuthenticated;
            }
        }

		override protected void OnInit(EventArgs e)
		{
			base.OnInit(e);

			Response.AddHeader("P3P", "CP='CAO PSA CONi OTR OUR DEM ONL'");
		}
	}
}