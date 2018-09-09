using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Storichain;

namespace Storichain.WebSite.User
{
    public partial class Login : PageBaseUser
    {
        private string LOGIN_COOKIE_NAME = "sccks";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
			{
                if (Context.User.Identity.IsAuthenticated)
                {
                    Response.Redirect("index");
                }
                else
                {
                    HttpCookieCollection cookies = Request.Cookies;
                    HttpCookie cookie = null;

                    if (cookies[LOGIN_COOKIE_NAME] != null)
                        cookie = cookies[LOGIN_COOKIE_NAME];

                    if (cookie != null)
                    {
                        txtSiteUserID.Value = cookie.Values["user_id"];
                        ckbSave.Checked = true;
                    }
                }
			}
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LoginWork();
        }

        private void LoginWork()
		{
            int sns_type_idx = hdfSnsTypeIdx.Value.ToInt();
            string site_user_id = txtSiteUserID.Value;

            if(sns_type_idx < 10) 
            {
                site_user_id = hdfUserID.Value;

                if (site_user_id.Equals("")) 
                {
                    ltrScript.Text = JSHelper.GetAlertScript("정상적으로 인증되지 않았습니다.");
                    return;
                }

                if (hdfAccessToken.Value.Equals("")) 
                {
                    ltrScript.Text = JSHelper.GetAlertScript("정상적으로 인증되지 않았습니다.");
                    return;
                }
            }
            else 
            {
                if (site_user_id.Equals("")) 
                {
                    ltrScript.Text = JSHelper.GetAlertScript("아이디를 입력하세요.");
                    return;
                }

                if (txtPWD.Value.Equals("")) 
                {
                    ltrScript.Text = JSHelper.GetAlertScript("패스워드 입력하세요.");
                    return;
                }
            }

			SitePrincipal site = new SitePrincipal();
            int rvalue;
            int user_idx;
            site.ValidateSnsLogin(site_user_id, txtPWD.Value, sns_type_idx, hdfAccessToken.Value, out rvalue, out user_idx);

            //1 : 성공
            //2 : 존재하지 않는 계정
            //3 : 아이디는 존재하나 패스워드가 일치하지 않는 경우
            //4 : 인증되지 않은 회원
            //5 : 탈퇴한 회원

            if(rvalue == 0) 
            {
                ltrScript.Text = JSHelper.GetAlertScript("존재하지 않는 회원입니다.");
            }
            else if(rvalue == 1) 
            {
                SitePrincipal newUser =  new SitePrincipal(user_idx);
                Context.User = newUser;
				SaveUserID(txtSiteUserID.Value, ckbSave.Checked);

                //Storichain.Models.Market.Biz.Biz_UserAccessLog log = new Storichain.Models.Market.Biz.Biz_UserAccessLog();
                //log.AddUserAccessLog(user_idx, Request.UserAgent, WebUtility.GetIpAddress(), Request.Url.PathAndQuery.ToString(), 0, DateTime.Now, user_idx);

                WebUtility.RemoveSession();
                WebUtility.RemoveSessionTemp();

                System.Web.Security.FormsAuthentication.SetAuthCookie(((SiteIdentity)newUser.Identity).UserIDX.ToString(), false);

                if (   ((SitePrincipal)Context.User).IsInRole("1")
                    || ((SitePrincipal)Context.User).IsInRole("2")
                    || ((SitePrincipal)Context.User).IsInRole("6"))
                {
                    if(hdfPUrl.Value.Equals(""))
                    {
                        Response.Redirect("index");
                    }
                    else
                    {
                        Response.Redirect("index");
                    }
                }
                else
                {
                    Response.Redirect("logout");
                    ltrScript.Text = JSHelper.GetAlertScript("접근 권한이 없습니다.");
                }
            }
            else if(rvalue == 2) 
            {
                ltrScript.Text = JSHelper.GetAlertScript("존재하지 않는 계정입니다.");
            }
            else if(rvalue == 3) 
            {
                ltrScript.Text = JSHelper.GetAlertScript("패스워드가 틀립니다.");
            }
            else if(rvalue == 4) 
            {
                ltrScript.Text = JSHelper.GetAlertScript("인증되지 않은 회원입니다.");
            }
            else if(rvalue == 5) 
            {
                ltrScript.Text = JSHelper.GetAlertScript("탈퇴한 회원입니다.");
            }
		}


		private void SaveUserID(string userId, bool isUserIDSaved)
		{
			HttpCookieCollection cookies = Request.Cookies;
			HttpCookie cookie;

			if (isUserIDSaved)
			{
				if (cookies[LOGIN_COOKIE_NAME] != null)
					cookie = cookies[LOGIN_COOKIE_NAME];
				else
					cookie = new HttpCookie(LOGIN_COOKIE_NAME);

				cookie.Values["user_id"] = userId;
				cookie.Expires = System.DateTime.Now.AddDays(30);
				Response.AppendCookie(cookie);
			}
			else
			{
				if (cookies[LOGIN_COOKIE_NAME] != null)
				{
					cookie = cookies[LOGIN_COOKIE_NAME];
					cookie.Expires = System.DateTime.Now;
					Response.AppendCookie(cookie);
				}
			}
		}

    }
}