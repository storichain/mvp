<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Storichain.WebSite.User.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <link rel="apple-touch-icon-precomposed"  href="" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="" />
    <title>Storichain</title>
    <link rel="shortcut icon" type="image/x-icon" href="Images/favicon.ico?v=1" />
    <link rel="stylesheet" type="text/css" href="Content/beautytalk.css">
    <script src="Scripts/lib/jquery.js"></script>
    <script src="Scripts/common.js"></script>
    <script src="Scripts/user/base.js"></script>
    <script src="Scripts/user/user.js"></script>
    <script>

        var logined = 0;

        $(function() {

            $('#txtSiteUserID').focus();
            $("#btnLogin").click(function (e) {
                e.preventDefault();
                return doLogin(10);
            });

            
        });

        function doLogin(sns_type_idx) 
        {
        	if(logined == 1)
                return;

            logined = 1;

            var site_user_idx = $("#txtSiteUserID").val();

            if(sns_type_idx < 10) 
            {
                site_user_idx = $("#hdfUserID").val();

                if(site_user_idx == "" || $("#hdfAccessToken").val() == "")
        	    {
                    alert("정상적으로 인증되지 않았습니다.");
                    logined = 0;
        		    return false;
                }
            }
            else 
            {
                if(site_user_idx == "")
        	    {
        		    $("#txtSiteUserID").focus();
        		    alert("아이디를 입력하세요.");
                    logined = 0;
        		    return false;
        	    }    

                if ($("#txtPWD").val().replaceBlankTo("") == "") 
                {
        		    $("#txtPWD").focus();
        		    alert("비밀번호를 입력하세요.");
                    logined = 0;
        		    return false;
        	    }
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


    </script>
    <!-- Google Analytics -->
    <script>
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
    })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');
 
    ga('create', 'UA-43949475-2', 'auto');
    ga('send', 'pageview');
    </script>
    <!-- End Google Analytics -->
</head>
<body>
    <form id="form1" runat="server">
        <div class="header">
	    <h1>로그인</h1>
    </div>
    <div class="wrap">
	    <div class="login">
			<fieldset>
				<dl class="login_inp">
					<dt><label for="txtSiteUserID">이메일</label></dt>
					<dd><input type="text" id="txtSiteUserID" class="inp_txt" placeholder="me@example.com" runat="server" /><button type="button" class="btn_del_inp"><span class="blind">삭제</span></button></dd>
				</dl>
				<dl class="login_inp">
					<dt><label for="txtPWD">비밀번호</label></dt>
					<dd><input type="password" id="txtPWD" class="inp_txt" onkeypress="if (event.keyCode==13) {return doLogin(10);}" runat="server"  /><button type="button" class="btn_del_inp"><span class="blind">삭제</span></button></dd>
				</dl>
                <div class="id_save"><input type="checkbox" id="ckbSave" class="id_chk" runat="server" /><label for="ckbSave">아이디저장</label></div>
				<div class="login_btn_area">
					<button type="button" class="btn login_btn" id="btnLogin">로그인</button>
                    <asp:LinkButton ID="lbnLogin" runat="server" OnClick="btnLogin_Click"></asp:LinkButton>
				</div>
			</fieldset>
	    </div>

	    <div class="login_txt">
		    <h2>혹시 비밀번호를 잊으셨나요?</h2>
		    <ul>
			    <li><a href="#">이메일로 비밀번호 찾기</a></li>
			    <li><a href="#">전화번호로 비밀번호 찾기</a></li>
		    </ul>
	    </div>
    </div>

        <asp:Literal ID="ltrScript" runat="server"></asp:Literal>
        <asp:HiddenField ID="hdfPUrl" runat="server" />
        <asp:HiddenField ID="hdfSnsTypeIdx" runat="server" Value="10" />
        <asp:HiddenField ID="hdfAccessToken" runat="server" Value="" />
        <asp:HiddenField ID="hdfUserID" runat="server" Value="" /> <%--user_id--%>

<script>
jQuery(function(){
	$('.login_inp dd input').each(function(index, obj){
		$(this).bind("keyup keydown",function(){
			if ($.trim($(this).val())){
				$(this).parents('.login_inp').find('.btn_del_inp').show();
				console.log($(this).val());
			} else {
				$(this).parents('.login_inp').find('.btn_del_inp').hide();
			}
		}).blur(function(){
			if(!$.trim($(this).val())){
				$(this).parents('.login_inp').find('.btn_del_inp').hide();
			}
		}).trigger('blur');
    });

	$('.btn_del_inp').click(function(){
		$(this).parents('.login_inp').find('input').val('').focus();
	});
});
</script>
    </form>
</body>
</html>
