using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Storichain;
using Storichain.Models.Biz;
using System.Data;

namespace Storichain.WebSite.User
{
    public partial class welcome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {

            }
        }

        protected void btnSend_Click(object sender, EventArgs e)
        {
            int user_idx = PageUtility.UserIdx().ToInt();

            Biz_User biz = new Biz_User();
            DataTable dtUser = biz.GetUser(user_idx, 0, "", "Y");

            if (dtUser.Rows.Count > 0)
            {
                DataRow dr = dtUser.Rows[0];

                string site_user_id = dr["site_user_id"].ToString();
                string nick_name   = dr["nick_name"].ToString();

                Dictionary<string, string> dic1 = new Dictionary<string, string>();
                dic1.Add("date_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                dic1.Add("nick_name", nick_name);
                dic1.Add("site_user_id", site_user_id);

                SiteUsers user = new SiteUsers();

                DataTable dt = user.GetAvailableRolse(user_idx).Tables[0];
                if(dt.Select("role_idx = " + hdfRole.Value).Length > 0)
                    user.AddUserToRole(user_idx, hdfRole.Value);

                if (WebUtility.GetConfig("HTTPS_USE_YN").Equals("Y"))
                    dic1.Add("auth_url", "https://" + WebUtility.GetConfig("DOMAIN_URL") + "/User/Auth?args=" + WebUtility.AuthQueryString(user_idx));
                else
                    dic1.Add("auth_url", "http://" + WebUtility.GetConfig("DOMAIN_URL") + "/User/Auth?args=" + WebUtility.AuthQueryString(user_idx));

                MailUtility.Send(site_user_id, E_MailDataType.register, dic1);

                ltrScript.Text = JSHelper.GetAlertRedirectScript("정상적으로 저장되었습니다. 메일을 확인하세요.", "/login");
            }
        }

    }
}