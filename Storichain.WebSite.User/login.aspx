<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Storichain.WebSite.User.login" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Libs/footer.ascx" TagPrefix="uc1" TagName="footer" %>

<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
    <script type="text/javascript">

        var logined = 0;

        $(function () {

            $('#txtSiteUserID').focus();
            $("#btnLogin").click(function (e) {
                e.preventDefault();
                return doLogin();
            });

            $('#btnRenew').click(function (e) {
                e.preventDefault();
                $("#txtPWD").val('');
                $("#txtPWD").focus();
            });

            $('#btnSignup').click(function (e) {
                e.preventDefault();
                location.href = 'signup';
            });

            function doLogin()
            {
                if(logined == 1)
                    return;

                logined = 1;

                var site_user_idx = $("#txtSiteUserID").val();
                var sns_type_idx = 10;

                if (site_user_idx == "") {
                    $("#txtSiteUserID").focus();
                    alert("아이디를 입력하세요.");
                    logined = 0;
                    return false;
                }

                if ($("#txtPWD").val().replaceBlankTo("") == "") {
                    $("#txtPWD").focus();
                    alert("비밀번호를 입력하세요.");
                    logined = 0;
                    return false;
                }

                var spinner = new Spinner(g_spin_common).spin();
                //$(spinner.el).css({'left':'330px', 'top':'240px'});
                var spinnerPoint = spinnerHeight($(window).scrollTop(), $(window).width(), $(window).height());
                $(spinner.el).css(spinnerPoint);
                $('body').append(spinner.el);

                $.ajax({
                    type: "POST",
                    url: url_path + "/User/SiteSnsLogin",
                    data: stringFormat("site_user_id={0}&pwd={1}&sns_type_idx={2}&access_token={3}", site_user_idx, $('#txtPWD').val(), sns_type_idx, $('#hdfAccessToken').val()),
                    dataType: "json",
                    cache: false,
                    success: function (response) {

                        console.log(response.response_code);
                        
                        if(response.response_code == '1000')
                        {
                            if(response.response_status == 'login_ok') 
                            {
                                __doPostBack('lbnLogin','');
                            }
                            else
                            {
                                alert('아이디 및 패스워드가 일치하지 않습니다.');
                                $('#txtPWD').val('');
                                spinner.stop();
                                return;
                            }
                        }
                        else if(response.response_code == '2012')
                        {
                            alert(response.response_message);
                            $('#txtPWD').val('');
                            spinner.stop();
                            return false;
                        }
                        else if(response.response_code == '2013')
                        {
                            alert(response.response_message);
                            $('#txtPWD').val('');
                            spinner.stop();
                            return false;
                        }
                        else if(response.response_code == '2014')
                        {
                            alert(response.response_message);
                            $('#txtPWD').val('');
                            spinner.stop();
                            return false;
                        }
                        else if(response.response_code == '2015')
                        {
                            alert(response.response_message);
                            $('#txtPWD').val('');
                            spinner.stop();
                            return false;
                        }
                        else if(response.response_code == '2000')
                        {
                            alert(response.response_message);
                            $('#txtPWD').val('');
                            spinner.stop();
                            return false;
                        }
                        else 
                        {
                            alert('처리 중 오류가 발생하였습니다.');
                            $('#<%=txtPWD.ClientID%>').val('');
                            spinner.stop();
                            return false;
                        }
                    },
                    error: function () {
					    alert('네트워크 오류가 발생하였습니다.');
                        spinner.stop();
				    },
                    complete: function () {

                        logined = 0;
                    }
                });
                return false;
            }


        });
    </script>
</head>
<body class="sbd">
    <form id="form1" runat="server">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="login">Login</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="index" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<h2 class="ct_logo"><img src="/images/logo_txt.png" alt="storichain" /></h2>
				<fieldset>
					<div class="form_wrap">
						<div class="form_box">
							<h3 class="form_tit">Email ID</h3>
							<div class="inp_box"><input type="email" name="" id="txtSiteUserID" class="inpt" placeholder="Email ID" runat="server" /><label for="txtSiteUserID" class="lblt">Email ID</label></div>
							<p class="error_msg">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Password <span>(more than 8 charactors includes alphabets, digits, symbols)</span></h3>
							<div class="inp_box"><input type="password" name="" id="txtPWD" class="inpt" placeholder="Password" onkeypress="if (event.keyCode==13) {return doLogin(10);}" runat="server" /><label for="pw" class="lblt">password</label></div>
							<p class="error_msg"></p>
						</div>

						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cp" id="btnLogin">LOG IN</button></span></li>
						</ul>

						<div class="login_check">
							<div class="login_check_box">
								<input type="checkbox" id="ckbSave" name="" class="ckbx" value="" runat="server">
								<label for="login_chk" class="lblck">7 days auto login</label>
							</div>
							<a href="#null" class="rnew_pw" id="btnRenew">Renew Password</a>
						</div>

						<!-- id pw 찾기
						<div class="find_info">
							<a href="#">Forgot User ID</a> <span class="bar">|</span> <a href="#">Forgot User Password</a>
						</div>
						-->

						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cw" id="btnSignup">SIGN UP for 10 seconds</button></span></li>
						</ul>

						<div class="login_msg">
							<p class="login_msg1">Story Asset Platform <br /> Enjoy Inter-Creating Stories</p>
							<p class="login_msg2">Be the Storyteller today for free</p>
						</div>
					</div>
				</fieldset>
		</div>
	</div>
</div>
<asp:LinkButton ID="lbnLogin" runat="server" OnClick="btnLogin_Click"></asp:LinkButton>
<asp:Literal ID="ltrScript" runat="server"></asp:Literal>
<asp:HiddenField ID="hdfPUrl" runat="server" />
<asp:HiddenField ID="hdfSnsTypeIdx" runat="server" Value="10" />
<asp:HiddenField ID="hdfAccessToken" runat="server" Value="" />
<asp:HiddenField ID="hdfUserID" runat="server" Value="" />
<uc1:footer runat="server" id="footer" />
        </form>
</body>
</html>
