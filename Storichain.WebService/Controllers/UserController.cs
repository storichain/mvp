using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Web.Security;
using System.Collections;
using Storichain.Models.Biz;
using Newtonsoft.Json;
using System.Security.Principal;

namespace Storichain.Controllers
{
    public partial class UserController : Controller
    {
        Biz_User biz = new Biz_User();
        string[] encColumns = {"email","user_id","site_user_id","pwd","first_name","last_name","nick_name","birthday","search_name"};

        //SNSJoin
        //SNSLogin
        //SiteLogin
        //SearchPassword


        //email
        //user_id
        //site_user_id
        //pwd
        //first_name
        //last_name
        //nick_name
        //birthday

        //enc_use_yn



        /// ----------------------------------------------------------- 로그인 관련 -------------------------------------


        public ActionResult SNSJoin() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("user_id")))
                message += "user_id is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("email")))
                message += "email is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
            //    message += "pwd is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
                message += "sns_type_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("nick_name")))
                message += "nick_name is null.\n";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sex_type_idx")))
            //    message += "sex_type_idx is null.\n";

            //if (!BizUtility.ValidCheck(WebUtility.GetRequest("birthday")))
            //    message += "birthday is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //    message += "access_token is null.\n";

            //if(WebUtility.GetRequestByInt("sns_type_idx") == 2)
            //{ 
                
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 4)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 5)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 6)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 10)
            //{ 
                
            //}
            //else 
            //{ 
            //    message += "The value of sns_type_idx is not supported.\n";
            //}

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                int result;
                int user_idx;

                string file_path1 = "user";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                string user_id = WebUtility.GetRequestWithEcryption("user_id", WebUtility.GetRequest("enc_use_yn", "N"));

                //if(WebUtility.GetRequestByInt("sns_type_idx", 4) == 4)
                //{
                //    //BizUtility.GetKakaoOldUserID("70047663");
                //    string convert_user_id = BizUtility.GetKakaoOldUserID(user_id);

                //    if(!convert_user_id.Equals(""))
                //    {
                //        user_id = convert_user_id;
                //    }
                //}


                if(biz.IsExistAtSiteUserId(WebUtility.GetRequest("user_id")))
                {
                    json = DataTypeUtility.JSon("2011", Config.R_EXIST_SITE_USER_ID, "existed site_user_id.", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if(biz.IsExistAtNickName(WebUtility.GetRequest("nick_name")))
                {
                    json = DataTypeUtility.JSon("2012", Config.R_EXIST_NICKNAME, "existed nickname.", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                biz.SNSJoin(user_id, 
                            WebUtility.GetRequestWithEcryption("email", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetDeviceTypeIdx(), 
                            WebUtility.GetRequestByInt("sns_type_idx", 3),
                            WebUtility.GetRequest("access_token"),
                            WebUtility.GetRequestWithEcryption("nick_name", WebUtility.GetRequest("enc_use_yn", "N")),
                            BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, "user", list1, "user"),
                            WebUtility.GetRequestWithEcryption("first_name", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetRequestWithEcryption("last_name", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetRequestWithEcryption("email", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetRequest("location_name"),
                            WebUtility.GetRequestWithEcryption("birthday", WebUtility.GetRequest("enc_use_yn", "N")),
                            WebUtility.GetRequestByInt("sex_type_idx"),
                            WebUtility.GetRequest("enc_use_yn", "N"),
                            out result,
                            out user_idx);

                if(result == 1) 
                {
                    DataTable dt = biz.GetUser( user_idx, 
                                                0,
                                                WebUtility.GetRequest("user_id"), 
                                                WebUtility.GetRequest("enable_yn", "Y"));

                    FormsAuthentication.SetAuthCookie(user_idx.ToString(), true);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt.ToDataTableWithEncription(WebUtility.GetRequest("enc_use_yn", "N"), encColumns));
                }
                else if(result == 2) 
                {
                    json = DataTypeUtility.JSon("2011", Config.R_EXIST_SITE_USER_ID, "existed site_user_id.", null);
                }
                else if(result == 3) 
                {
                    json = DataTypeUtility.JSon("2016", Config.R_EXISTS_USER_ID, "existed user_id.", null);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, result.ToString(), null);
                }
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SearchPassword()
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("email")))
                message += "email is null.";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.SearcPwdByEmail(WebUtility.GetRequest("email"));

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);

            //if (dt.Rows.Count > 0) 
            //{ 
            //    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            //}
            //else 
            //{ 
            //    json = DataTypeUtility.JSon("1000", Config.R_FAIL, "", null);
            //}

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [ValidateInput(false)]
        public ActionResult SiteSNSLogin()
        {
            SitePrincipal site = new SitePrincipal();
            int rvalue;
            int user_idx;
            site.ValidateSnsLogin(  WebUtility.GetRequestWithEcryption("site_user_id", WebUtility.GetRequest("enc_use_yn", "N")),
                                    WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")),
                                    WebUtility.GetRequestByInt("sns_type_idx", 10),
                                    WebUtility.GetRequest("access_token"),
                                    out rvalue,
                                    out user_idx);

            string json;

            //2012	존재하는 않는 계정	NULL	NULL	NULL	NULL	NULL
            //2013	아이디는 존재하나 패스워드가 일치하지 않는 경우	NULL	NULL	NULL	NULL	NULL
            //2014	인증되지 않은 회원	NULL	NULL	NULL	NULL	NULL
            //2015	탈퇴한 회원	NULL	NULL	NULL	NULL	NULL

            if (rvalue == 1)
            {
                biz.Logined(user_idx);
                FormsAuthentication.SetAuthCookie(user_idx.ToString(), true);
                json = DataTypeUtility.JSon("1000", Config.R_LOGIN_OK, "", null);
            }
            else if (rvalue == 2)
            {
                json = DataTypeUtility.JSon("2012", Config.R_LOGIN_FAIL, "존재하는 않는 계정입니다.", null);
            }
            else if (rvalue == 3)
            {
                json = DataTypeUtility.JSon("2013", Config.R_LOGIN_FAIL, "패스워드가 일치하지 않습니다.", null);
            }
            else if (rvalue == 4)
            {
                //DataTable dtUser = biz.GetUser(user_idx);
                //if(dtUser.Rows.Count > 0) 
                //{
                //    DataRow dr = dtUser.Rows[0];

                //    string site_user_id = WebUtility.GetRequest("site_user_id");
                //    string first_name   = dr["first_name"].ToString();
                //    string last_name    = dr["last_name"].ToString();

                //    Dictionary<string, string> dic = new Dictionary<string,string>();
                //    dic.Add("message", "미인증 회원입니다. 전송된 메일에서 인증확인을 부탁드립니다.");
                //    json = DataTypeUtility.JSon("2000", Config.R_NONE_AUTH, "", dic);

                //    Dictionary<string, string> dic1 = new Dictionary<string,string>();
                //    dic1.Add("date_time", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //    dic1.Add("nick_name", last_name + first_name);
                //    dic1.Add("site_user_id", site_user_id);

                //    if(WebUtility.GetConfig("HTTPS_USE_YN").Equals("Y"))
                //        dic1.Add("auth_url", "https://" + WebUtility.GetConfig("DOMAIN_URL") + "/User/Auth?args=" + WebUtility.AuthQueryString(user_idx));
                //    else
                //        dic1.Add("auth_url", "http://" + WebUtility.GetConfig("DOMAIN_URL") + "/User/Auth?args=" + WebUtility.AuthQueryString(user_idx));

                //    MailUtility.Send(site_user_id, E_MailDataType.register, dic1);    
                //}

                json = DataTypeUtility.JSon("2014", Config.R_LOGIN_FAIL, "인증된 회원이 아닙니다.", null);
            }
            else if (rvalue == 5)
            {
                json = DataTypeUtility.JSon("2015", Config.R_LOGIN_FAIL, "탈퇴한 회원입니다.", null);
            }
            else
            {
                json = DataTypeUtility.JSon("2000", Config.R_LOGIN_FAIL, "아이디 및 패스워드를 옳지 않습니다. 다시 입력하세요.", null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [ValidateInput(false)]
        public ActionResult SiteLogin() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("site_user_id")))
                message += "site_user_id is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
                message += "pwd is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.IsSiteLogined(   WebUtility.GetRequestWithEcryption("site_user_id", WebUtility.GetRequest("enc_use_yn", "N")), 
                                                WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")));
            if(dt.Rows.Count > 0) 
            {
                DataTable dtUser = biz.GetUser( dt.Rows[0]["user_idx"].ToInt(), 
                                                0,
                                                "", 
                                                "Y");

                FormsAuthentication.SetAuthCookie(dt.Rows[0]["user_idx"].ToString(), true);
                json = DataTypeUtility.JSon("1000", Config.R_LOGIN_OK, "", dtUser.ToDataTableWithEncription(WebUtility.GetRequest("enc_use_yn", "N"), encColumns));
            }
            else
            {
                json = DataTypeUtility.JSon("2000", Config.R_LOGIN_FAIL, "", null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SnsLogin() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("user_id")))
                message += "user_id is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
                message += "access_token is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("sns_type_idx")))
            //    message += "sns_type_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            //if(!BizUtility.ValidCheck(WebUtility.GetRequest("user_id")))
            //    message += "user_id is null.\n";

            //if(WebUtility.GetRequestByInt("sns_type_idx") == 2)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 4)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 5)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 6)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("access_token")))
            //        message += "access_token is null.\n";
            //}
            //else if(WebUtility.GetRequestByInt("sns_type_idx") == 10)
            //{ 
            //    if(!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
            //        message += "pwd is null.\n";
            //}
            //else 
            //{ 
            //    message += "The value of sns_type_idx is not supported.\n";
            //}

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string user_id = WebUtility.GetRequestWithEcryption("user_id", WebUtility.GetRequest("enc_use_yn", "N"));
                string old_user_id = "";

                if(WebUtility.GetRequestByInt("sns_type_idx", 4) == 4)
                {
                    //BizUtility.GetKakaoOldUserID("70047663");
                    string convert_user_id = BizUtility.GetKakaoOldUserID(user_id);

                    if(!convert_user_id.Equals(""))
                    {
                        old_user_id = convert_user_id;
                    }
                }

                DataTable dt  = biz.SNSLogin(   user_id, 
                                                old_user_id,
                                                WebUtility.GetRequest("access_token"), 
                                                WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")), 
                                                WebUtility.GetRequestByInt("sns_type_idx", 3), 
                                                WebUtility.UserIdx());

                int r           = DataTypeUtility.GetToInt32(dt.Rows[0]["R"]);
                int user_idx    = DataTypeUtility.GetToInt32(dt.Rows[0]["user_idx"]);

                //1 : 성공
                //2 : 존재하지 않는 계정
                //3 : 아이디는 존재하나 패스워드가 일치하지 않는 경우

                if(r == 1) 
                {
                    DataTable dtUser = biz.GetUser( user_idx, 
                                                    0,
                                                    "", 
                                                    "Y");

                    FormsAuthentication.SetAuthCookie(user_idx.ToString(), true);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtUser.ToDataTableWithEncription(WebUtility.GetRequest("enc_use_yn", "N"), encColumns));
                }
                else if(r == 2) 
                {
                    json = DataTypeUtility.JSon("2012", Config.R_NONE_USER, "", null);
                }
                else if(r == 3) 
                {
                    json = DataTypeUtility.JSon("2013", Config.R_LOGIN_FAIL, "", null);
                }
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult SearchUserId()
        {
            string json = "";
            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("user_id")))
                message += "user_id is null.";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            bool isContain = biz.IsExisted(0, WebUtility.GetRequest("user_id"), "", "Y");

            if (!isContain)
            {
                json = DataTypeUtility.JSon("2000", Config.R_FAIL, "아이디가 존재하지 않습니다.", null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        /// ----------------------------------------------------------- 로그인 관련 끝 -------------------------------------














        public ActionResult Get() 
        {
            string json = "";
            string message = "";

            int user_idx    = 0;
            int me_user_idx = WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx());;

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("user_idx")))
            {
                if(me_user_idx > 0) 
                    user_idx = me_user_idx;
                else 
                    message += "user_idx is null.\n";
            }
            else
            {
                user_idx = WebUtility.GetRequestByInt("user_idx");
            }   

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.GetUser( user_idx,
                                        me_user_idx,
                                        WebUtility.GetRequest("user_id"), 
                                        WebUtility.GetRequest("enable_yn", "Y"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt.ToDataTableWithEncription(WebUtility.GetRequest("enc_use_yn", "N"), encColumns));
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRequest() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            Biz_RoleChangeRequest biz_request = new Biz_RoleChangeRequest();
            DataTable dt = biz_request.GetRoleChangeRequest(WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }        

        public ActionResult RequestRole()
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("request_desc")))
                message += "request_desc is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                //bool isOK1 = biz.IsExistedSiteUser(WebUtility.GetRequest("site_user_id"));

                //if(isOK1) 
                //{
                //    json = DataTypeUtility.JSon("2000", Config.R_DUPLICATE, "", null);
                //    return Content(json, "application/json", System.Text.Encoding.UTF8);
                //}

                Biz_RoleChangeRequest biz_request = new Biz_RoleChangeRequest();

                bool isOK2 = biz_request.IsExist(WebUtility.UserIdx());

                if(isOK2) 
                {
                    json = DataTypeUtility.JSon("2001", Config.R_EXIST_DATA, "", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                bool isOK = biz_request.AddRoleChangeRequest(WebUtility.UserIdx(), 
                                                             WebUtility.GetRequest("site_user_id"),
                                                             WebUtility.GetRequest("pwd"), 
                                                             WebUtility.GetRequest("request_desc"), 
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

        public ActionResult GetUserImage() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = biz.GetUserImage(WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Clause() 
        {
            DataTable dt = (new Biz_Clause()).GetClause();
            string json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ClauseAgree()
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.ClauseAgree(WebUtility.UserIdx());

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

        [ValidateInput(false)]
        public ActionResult Search() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("search_text")))
                message += "search_text is null.\n";

            //if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
            //    message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            DataTable dt = biz.SearchUser(WebUtility.GetRequest("search_text"), WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetProfile() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            Biz_Event biz_event = new Biz_Event();
            Biz_Follow biz_follow = new Biz_Follow();

            int user_idx = WebUtility.UserIdx();

            DataTable dtUser = biz.GetUser(user_idx, 0, "", "Y");
            DataTable dtEvent = biz_event.GetList(0, user_idx, 0, 0, 1, "Y", "", "N");
            DataTable dtFollowers = biz_follow.GetFollowerList(user_idx, "Y");
            DataTable dtFollowing = biz_follow.GetFollowingList(user_idx, "Y");
            
            Dictionary<string, DataTable> dic = new Dictionary<string,DataTable>();
            dic.Add("user", dtUser);
            dic.Add("event", dtEvent);
            dic.Add("followers", dtFollowers);
            dic.Add("following", dtFollowing);

            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dic);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetRole() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            DataTable dt = biz.GetUserSimple(WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [ValidateInput(false)]
        public ActionResult Login() 
        {
            DataTable dt = biz.IsLogined(   WebUtility.GetRequest("user_id"), 
                                            WebUtility.GetRequest("pwd"));
            string json;

            if(dt.Rows.Count > 0) 
            {
                FormsAuthentication.SetAuthCookie(dt.Rows[0]["user_idx"].ToString(), true);
                json = DataTypeUtility.JSon("1000", Config.R_LOGIN_OK, "", dt);
            }
            else
            {
                json = DataTypeUtility.JSon("1000", Config.R_LOGIN_FAIL, "", null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Block() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            try
            {
                Biz_UserBlock biz = new Biz_UserBlock();
                bool isOK = biz.AddUserBlock(   WebUtility.UserIdx(), 
                                                WebUtility.GetRequest("user_block_yn"), 
                                                WebUtility.GetRequest("post_block_yn"), 
                                                WebUtility.GetRequest("comment_block_yn"),  
                                                WebUtility.GetRequest("message_block_yn"), 
                                                WebUtility.GetRequest("book_mark_block_yn"), 
                                                WebUtility.GetRequest("like_block_yn"), 
                                                WebUtility.GetRequest("follow_block_yn"), 
                                                WebUtility.GetRequest("invi_block_yn"), 
                                                WebUtility.GetRequest("post_search_block_yn"), 
                                                WebUtility.GetRequest("friend_search_block_yn"), 
                                                WebUtility.GetRequest("alarm_all_block_yn"), 
                                                WebUtility.GetRequest("alarm_post_block_yn"), 
                                                WebUtility.GetRequest("alarm_comment_block_yn"),  
                                                WebUtility.GetRequest("alarm_book_mark_block_yn"), 
                                                WebUtility.GetRequest("alarm_like_block_yn"), 
                                                WebUtility.GetRequest("alarm_follow_block_yn"), 
                                                WebUtility.GetRequest("alarm_coupon_block_yn"), 
                                                WebUtility.GetRequest("alarm_vibration_in_app"), 
                                                WebUtility.GetRequest("alarm_sound_in_app"), 
                                                "Y", 
                                                WebUtility.GetRequest("enable_yn"), 
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

        public ActionResult Signup() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("user_id")))
                message += "user_id is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
                message += "pwd is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("first_name")))
                message += "first_name is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("last_name")))
                message += "last_name is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("nick_name")))
                message += "nick_name is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("email")))
                message += "email is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                if(biz.IsExisted(0, WebUtility.GetRequest("user_id"), "")) 
                {
                    json = DataTypeUtility.JSon("2000", Config.R_DUPLICATE, "중복된 아이디입니다.", null);
                }
                else 
                {
                    string file_path1 = "user";
                    string image_key1 = "";
                    ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                    bool isOK = biz.AddUser(WebUtility.GetRequest("user_id"), 
                                            WebUtility.GetRequest("pwd"), 
                                            WebUtility.GetRequest("first_name"), 
                                            WebUtility.GetRequest("last_name"), 
                                            WebUtility.GetRequest("nick_name"), 
                                            WebUtility.GetRequest("email"), 
                                            WebUtility.GetRequest("location_name"), 
                                            WebUtility.GetRequestByFloat("location_x"), 
                                            WebUtility.GetRequestByFloat("location_y"), 
                                            WebUtility.GetRequest("web_site"), 
                                            WebUtility.GetRequest("bio"), 
                                            BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                            "Y", 
                                            WebUtility.GetDeviceTypeIdx(),
                                            DateTime.Now, 
                                            WebUtility.UserIdx());

                    DataTable dt = biz.IsLogined(   WebUtility.GetRequest("user_id"), 
                                                    WebUtility.GetRequest("pwd"));

                    if(dt.Rows.Count > 0) 
                    {
                        FormsAuthentication.SetAuthCookie(dt.Rows[0]["user_idx"].ToString(), true);
                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                    }
                    else
                    {
                        json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                    }
                }
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Add() 
        {
            string json = "";

            try
            {
                bool isOK = biz.AddUser(WebUtility.GetRequest("user_id"), 
                                        WebUtility.GetRequest("pwd"), 
                                        WebUtility.GetRequest("first_name"), 
                                        WebUtility.GetRequest("last_name"), 
                                        WebUtility.GetRequest("nick_name"), 
                                        WebUtility.GetRequest("pwd"), 
                                        WebUtility.GetRequest("location_name"), 
                                        WebUtility.GetRequestByFloat("location_x"), 
                                        WebUtility.GetRequestByFloat("location_y"), 
                                        WebUtility.GetRequest("web_site"), 
                                        WebUtility.GetRequest("bio"), 
                                        0, 
                                        WebUtility.GetRequest("enable_yn"), 
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

        [ValidateInput(false)]
        public ActionResult Modify() 
        {
            string json = "";

            try
            {
                bool isOK = biz.ModifyUser (WebUtility.UserIdx(), 
                                            WebUtility.GetRequest("user_id"), 
                                            WebUtility.GetRequest("pwd"), 
                                            WebUtility.GetRequest("first_name"), 
                                            WebUtility.GetRequest("last_name"), 
                                            WebUtility.GetRequest("nick_name"), 
                                            WebUtility.GetRequest("email"), 
                                            WebUtility.GetRequest("location_name"), 
                                            WebUtility.GetRequestByFloat("location_x"), 
                                            WebUtility.GetRequestByFloat("location_y"), 
                                            WebUtility.GetRequest("web_site"), 
                                            WebUtility.GetRequest("bio"), 
                                            WebUtility.GetRequestByInt("file_idx"), 
                                            WebUtility.GetRequest("enable_yn"), 
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

        [ValidateInput(false)]
        public ActionResult ModifyExtension() 
        {
            string json = "";

            try
            {
                bool isOK = biz.ModifyUserExt ( WebUtility.UserIdx(), 
                                                WebUtility.GetRequest("first_name"), 
                                                WebUtility.GetRequest("last_name"), 
                                                WebUtility.GetRequest("cell_phone"), 
                                                WebUtility.GetRequest("address1"), 
                                                WebUtility.GetRequest("address2"), 
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


        // 변경 20180819
        //[ValidateInput(false)]
        //public ActionResult ModifyUserProfile() 
        //{
        //    string json = "";

        //    try
        //    {
        //        string message = "";

        //        if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
        //            message += "user_idx is null.\n";

        //        if (!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
        //            message += "pwd is null.\n";

        //        if (!message.Equals(""))
        //        {
        //            json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
        //            return Content(json, "application/json", System.Text.Encoding.UTF8);
        //        }

        //        if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

        //        string file_path1 = "user";
        //        string image_key1 = "";
        //        ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

        //        int user_idx = WebUtility.UserIdx();
        //        int file_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);

        //        bool isOK = biz.ModifyUserProfile(  user_idx, 
        //                                            WebUtility.GetRequestWithEcryption("nick_name", WebUtility.GetRequest("enc_use_yn", "N")), 
        //                                            WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")), 
        //                                            WebUtility.GetRequest("location_name"), 
        //                                            WebUtility.GetRequestByFloat("location_x"), 
        //                                            WebUtility.GetRequestByFloat("location_y"), 
        //                                            WebUtility.GetRequestWithEcryption("email", WebUtility.GetRequest("enc_use_yn", "N")), 
        //                                            WebUtility.GetRequest("web_site"), 
        //                                            WebUtility.GetRequest("bio"), 
        //                                            WebUtility.GetRequest("first_name"), 
        //                                            WebUtility.GetRequest("cell_phone"), 
        //                                            WebUtility.GetRequest("address1"), 
        //                                            WebUtility.GetRequest("address2"), 
        //                                            file_idx,
        //                                            DateTime.Now, 
        //                                            user_idx);

        //        if(isOK) 
        //        {
        //            if(file_idx > 0) 
        //            {
        //                DataTable dt = biz.GetUser( user_idx, 0, "", "Y");
        //                //MongoDBCommon.UpdateUserImage(dt);
        //            }

        //            //MongoDBCommon.UpdateNickName(user_idx, WebUtility.GetRequest("nick_name"));

        //            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
        //        }
                    
        //        else
        //            json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
        //    }
        //    catch(Exception ex) 
        //    {
        //        BizUtility.SendErrorLog(Request, ex);
        //        json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
        //    }

        //    return Content(json, "application/json", System.Text.Encoding.UTF8);
        //}

        [ValidateInput(false)]
        public ActionResult ModifyUserProfile() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("user_name")))
                    message += "user_name is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("introduce")))
                    message += "introduce is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("interest_field")))
                    message += "interest_field is null.\n";

                if (!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                //if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                string file_path1 = "user";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1);

                int user_idx = WebUtility.UserIdx();
                int file_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);

                bool isOK = biz.ModifyUserProfileSimple(user_idx, 
                                                        WebUtility.GetRequest("user_name"), 
                                                        WebUtility.GetRequest("introduce"), 
                                                        WebUtility.GetRequest("interest_field"), 
                                                        file_idx,
                                                        DateTime.Now, 
                                                        user_idx);

                if(isOK) 
                {
                    //if(file_idx > 0) 
                    //{
                        DataTable dt = biz.GetUser( user_idx, 0, "", "Y");
                        //MongoDBCommon.UpdateUserImage(dt);
                    //}

                    //MongoDBCommon.UpdateNickName(user_idx, WebUtility.GetRequest("nick_name"));

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                    
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

        public ActionResult ModifyUserProfileWithSiteUserId() 
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("site_user_id")))
                    message += "site_user_id is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
                    message += "pwd is null.\n";

                if (!BizUtility.ValidCheck(WebUtility.GetRequest("nick_name")))
                    message += "nick_name is null.\n";

                if (!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if(biz.IsExistAtSiteUserId(WebUtility.GetRequest("site_user_id")))
                {
                    json = DataTypeUtility.JSon("2000", Config.R_EXIST_SITE_USER_ID, "", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                string file_path1 = "user";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                int user_idx = WebUtility.UserIdx();
                int file_idx = BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1);

                bool isOK = biz.ModifyUserProfileWithSiteUserId(user_idx, 
                                                                WebUtility.GetRequestWithEcryption("site_user_id", WebUtility.GetRequest("enc_use_yn", "N")), 
                                                                WebUtility.GetRequestWithEcryption("nick_name", WebUtility.GetRequest("enc_use_yn", "N")),
                                                                WebUtility.GetRequestWithEcryption("pwd", WebUtility.GetRequest("enc_use_yn", "N")),
                                                                WebUtility.GetRequest("location_name"), 
                                                                WebUtility.GetRequestByFloat("location_x"), 
                                                                WebUtility.GetRequestByFloat("location_y"), 
                                                                WebUtility.GetRequestWithEcryption("email", WebUtility.GetRequest("enc_use_yn", "N")), 
                                                                WebUtility.GetRequest("web_site"), 
                                                                WebUtility.GetRequest("bio"), 
                                                                file_idx,
                                                                DateTime.Now, 
                                                                user_idx);

                if(isOK) 
                {
                    if(file_idx > 0) 
                    {
                        DataTable dt = biz.GetUser( user_idx, 0, "", "Y");
                        MongoDBCommon.UpdateUserImage(dt);
                    }

                    MongoDBCommon.UpdateNickName(user_idx, WebUtility.GetRequest("nick_name"));

                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                    
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

        public ActionResult ChangeUserImage()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                string file_path1 = "user";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                bool isOK = biz.ChangeUserImage(WebUtility.UserIdx(), 
                                                BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
                                                DateTime.Now, 
                                                WebUtility.UserIdx());

                if(isOK) 
                {
                    DataTable dt = biz.GetUser(WebUtility.UserIdx(), 0, "", "Y");

                    MongoDBCommon.UpdateUserImage(dt);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
                else
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ChangeThemeImage()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                string file_path1 = "user_theme";
                string image_key1 = "";
                ArrayList list1 = ImageUtility.GetImageSizes(file_path1, ref image_key1); 

                bool isOK = biz.ChangeThemeImage(WebUtility.UserIdx(), 
                                                 BizUtility.SaveFile(HttpContext.ApplicationInstance.Context.Request, file_path1, list1, image_key1),
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

        [ValidateInput(false)]
        public ActionResult ChangeName() 
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("first_name")))
                message += "first_name is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("last_name")))
                message += "last_name is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isOK = biz.ChangeName( WebUtility.UserIdx(), 
                                            WebUtility.GetRequest("first_name"),
                                            WebUtility.GetRequest("last_name"));

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

        [ValidateInput(false)]
        public ActionResult ChangeNickName() 
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("nick_name")))
                message += "nick_name is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isContain = biz.IsExistAtNickName(WebUtility.GetRequest("nick_name"));

                if(isContain)
                {
                    json = DataTypeUtility.JSon("2017", Config.R_EXIST_NICKNAME, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                bool isOK = biz.ChangeNickName( WebUtility.UserIdx(), 
                                                WebUtility.GetRequest("nick_name"));

                if(isOK)  
                {
                    MongoDBCommon.UpdateNickName(WebUtility.UserIdx(), WebUtility.GetRequest("nick_name"));
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
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

        [ValidateInput(false)]
        public ActionResult ExistNickName() 
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("nick_name")))
                message += "nick_name is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isContain = biz.IsExistAtNickName(WebUtility.GetRequest("nick_name"));

                if(!isContain)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_EXIST_NICKNAME, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        [ValidateInput(false)]
        public ActionResult ExistSiteUserId() 
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("site_user_id")))
                message += "site_user_id is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isContain = biz.IsExistAtSiteUserId(WebUtility.GetRequest("site_user_id"));

                if(!isContain)
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                else
                    json = DataTypeUtility.JSon("2000", Config.R_EXIST_SITE_USER_ID, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Logout()
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                FormsAuthentication.SignOut();

                if (BizUtility.ValidCheck(WebUtility.UserIdx())) 
                {
                    bool isOK = biz.ChangeLoginedYN(WebUtility.UserIdx(), "N");

                    if(isOK) 
                        json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                    else
                        json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
                }
                else 
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Resign()
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                string result = BizUtility.ResignKakao(WebUtility.UserIdx()).Replace("{", "").Replace("}", "");

                bool isOK = biz.Resign(WebUtility.UserIdx());
                //MongoDBCommon.ResignUser(WebUtility.UserIdx());
                FormsAuthentication.SignOut();

                //if(isOK) 
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", result);
                //else
                //    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "", null);
            }
            catch(Exception ex) 
            {
                BizUtility.SendErrorLog(Request, ex);
                json = DataTypeUtility.JSon("9999", Config.R_ERROR, ex.Message, null);
            }

            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ChangePassword() 
        {
            string json = "";

            string message = "";

            if (!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("pwd")))
                message += "pwd is null.\n";

            if (!BizUtility.ValidCheck(WebUtility.GetRequest("pwd_old")))
                message += "pwd_old is null.\n";

            if (!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            try
            {
                bool isContain = biz.IsExisted(WebUtility.UserIdx(), "", WebUtility.GetRequest("pwd_old"), "Y");

                if(!isContain) 
                {
                    json = DataTypeUtility.JSon("2000", Config.R_FAIL, "패스워드가 일치하지 않습니다.", null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                bool isOK = biz.ChangePwd(  WebUtility.UserIdx(), 
                                            WebUtility.GetRequest("pwd"), 
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

        public ActionResult Remove() 
        {
            string json = "";
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            int user_idx = WebUtility.UserIdx();

            try
            {
                bool isOK = biz.ChangeEnabled(user_idx, "N", DateTime.Now, user_idx);

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

        public ActionResult GetBlockUser() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            Biz_Report biz_report = new Biz_Report();
            DataTable dt = biz_report.GetUserBlock(WebUtility.UserIdx(), 
                                                   WebUtility.GetRequestByInt("to_user_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult GetHideUser() 
        {
            string json;
            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                message += "to_user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            Biz_Report biz_report = new Biz_Report();
            DataTable dt = biz_report.GetUserHide(  WebUtility.UserIdx(), 
                                                    WebUtility.GetRequestByInt("to_user_idx"));
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult ToggleUserBlock()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                    message += "to_user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx        = WebUtility.UserIdx();
                int to_user_idx     = WebUtility.GetRequestByInt("to_user_idx");

                Biz_Report biz_report = new Biz_Report();
                bool isOK = biz_report.ToggleUserBlock( user_idx, 
                                                        to_user_idx, 
                                                        DateTime.Now, 
                                                        0,
                                                        DateTime.Now, 
                                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz_report.GetUserBlock(user_idx, to_user_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
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

        public ActionResult ToggleUserHide()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!BizUtility.ValidCheck(WebUtility.GetRequestByInt("to_user_idx")))
                    message += "to_user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx        = WebUtility.UserIdx();
                int to_user_idx     = WebUtility.GetRequestByInt("to_user_idx");

                Biz_Report biz_report = new Biz_Report();
                bool isOK = biz_report.ToggleUserHide(  user_idx, 
                                                        to_user_idx, 
                                                        DateTime.Now, 
                                                        0,
                                                        DateTime.Now, 
                                                        user_idx);
                
                if(isOK) 
                {
                    DataTable dt = biz_report.GetUserHide(user_idx, to_user_idx);
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
                }
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

        public ActionResult GetAlarm() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                message += "user_idx is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

            Biz_UserBlock biz_block = new Biz_UserBlock();
            DataTable dt = biz_block.GetUserBlockAtUser(WebUtility.UserIdx());
            json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt);
            return Content(json, "application/json", System.Text.Encoding.UTF8);
        }

        public ActionResult Auth() 
        {
            string json = "";

            string message = "";

            if(!BizUtility.ValidCheck(WebUtility.GetRequest("args")))
                message += "args is null.\n";

            if(!message.Equals(""))
            {
                json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                return Content(json, "application/json", System.Text.Encoding.UTF8);
            }

            EncryptedQueryString args = new EncryptedQueryString(WebUtility.GetRequest("args"));
            int user_idx = args["user_idx"].ToInt();

            Biz_User biz = new Biz_User();
            biz.ChangeEmailAuthYN(user_idx, "Y", DateTime.Now, user_idx);
            
            return Content(JSHelper.GetAlertRedirectScript("정상적으로 인증되었습니다.", "/index"));
        }

        public ActionResult SettingAlarm()
        {
            string json = "";

            try
            {
                string message = "";

                if(!BizUtility.ValidCheck(WebUtility.UserIdx()))
                    message += "user_idx is null.\n";

                if(!message.Equals(""))
                {
                    json = DataTypeUtility.JSon("3000", Config.R_FAIL, message, null);
                    return Content(json, "application/json", System.Text.Encoding.UTF8);
                }

                if((new Biz_User()).GetUserSimple(WebUtility.UserIdx()).Rows.Count == 0) return Content(DataTypeUtility.JSon("5000", Config.R_NO_EXIST_USER, message, null), "application/json", System.Text.Encoding.UTF8);

                int user_idx        = WebUtility.UserIdx();
                //int report_user_idx = WebUtility.GetRequestByInt("to_user_idx");

                Biz_UserBlock biz_block = new Biz_UserBlock();
                bool isOK = biz_block.AddUserBlock( WebUtility.UserIdx(),
                                                    "", 
                                                    "", 
                                                    "", 
                                                    "", 
                                                    "",
                                                    "", 
                                                    "", 
                                                    "", 
                                                    "", 
                                                    "", 
                                                    WebUtility.GetRequest("alarm_all_block_yn"), 
                                                    WebUtility.GetRequest("alarm_post_block_yn"), 
                                                    WebUtility.GetRequest("alarm_comment_block_yn"),  
                                                    WebUtility.GetRequest("alarm_book_mark_block_yn"), 
                                                    WebUtility.GetRequest("alarm_like_block_yn"), 
                                                    WebUtility.GetRequest("alarm_follow_block_yn"), 
                                                    WebUtility.GetRequest("alarm_coupon_block_yn"), 
                                                    WebUtility.GetRequest("alarm_vibration_in_app"), 
                                                    WebUtility.GetRequest("alarm_sound_in_app"), 
                                                    "Y", 
                                                    "", 
                                                    DateTime.Now, 
                                                    WebUtility.UserIdx());
                
                if(isOK) 
                {
                    json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", null);
                }
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
