<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_modify.aspx.cs" Inherits="Storichain.WebSite.User.account_modify" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Libs/footer.ascx" TagPrefix="uc1" TagName="footer" %>
<%@ Import Namespace="Storichain" %>
<%@ Import Namespace="Storichain.Models.Biz" %>
<%@ Import Namespace="Storichain.Models" %>
<%@ Import Namespace="System.Data" %>
<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
    <script type="text/javascript">
        $(function() {

            var user_idx = <%=WebUtility.UserIdx()%>;
            
            $('.btn_r.btn_m.btn_cw').click(function (e) {
                e.preventDefault();

                alert('Cancel');
            });

            $('.btn_r.btn_m.btn_cp').click(function (e) {
                e.preventDefault();

                if(!confirm('Do you want to save your data?'))
                    return false;

                if ($("#login_pw").val() == "") {
                    alert("Please enter a password.");
                    return false;
                } else if ($("#login_pw").val().length < 8) {
                    alert("Please enter at least 8 digits.");
                    return false;
                } else if ($("#login_pw_cf").val().length < 8) {
                    alert("Please enter at least 8 digits.");
                    return false;
                } else if ($("#login_pw").val() != $("#login_pw_cf").val()) {
                    alert("Invalid password verification. Please re-enter.");
                    $("#login_pw_cf").val("");
                    return false;
                } else if ($("#login_pw").val() == $("#login_pw_old").val()) {
                    alert("You can not use the same password.");
                    return false;
                }

                var formData = new FormData();
                formData.append('user_idx', user_idx);
                formData.append('pwd_old', $("#login_pw_old").val());
                formData.append('pwd', $("#login_pw").val());

                //var p = jQuery.param({ user_idx: '<%=WebUtility.UserIdx()%>', pwd : $("#login_pw_old").val(), pwd_old : $("#login_pw").val() });
                //var p = jQuery.param({ user_idx: '5', pwd : $("#login_pw_old").val(), pwd_old : $("#login_pw").val() });
                console.log(p);

                $.ajax({
                    type: "Post",
                    url: "/User/ChangePassword",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            alert("It was completed.");
                            location.href = "/login";
                        }
                        else if (response.response_code == "2000") {
                            alert("The current password does not match.");
                            return;
                        }
                        else 
                        {
                            alert("Please enter the correct data.");
                            return;
                        }
                    }
                });

            });

            $("#login_pw_old").on('keyup keypress blur', function (event) {
                var login_pw_old = $("#login_pw_old").val();
                
                if(login_pw_old.length < 8) {
                    $("#pw_error0").show();
                    $("#pw_error0").text('(more than 8 charactors includes alphabets, digits, symbols)');
                    return;
                }
                else {
                    $("#pw_error0").hide();
                }
            });

            $("#login_pw").on('keyup keypress blur', function (event) {
                var login_pw = $("#login_pw").val();
                var login_pw_cf = $("#login_pw_cf").val();

                if(login_pw.length < 8) {
                    $("#pw_error1").show();
                    $("#pw_error1").text('(more than 8 charactors includes alphabets, digits, symbols)');
                    return;
                }
                else {
                    $("#pw_error1").hide();
                }

                if (login_pw != login_pw_cf) 
                {
                    $("#pw_error1").show();
                    $("#pw_error1").text('Each password does not match.');    
                }
                else {
                    $("#pw_error1").hide();
                    $("#pw_error2").hide();
                }

            });

            $("#login_pw_cf").on('keyup keypress blur', function (event) {
                var login_pw    = $("#login_pw").val();
                var login_pw_cf = $("#login_pw_cf").val();

                if(login_pw_cf.length < 8) {
                    $("#pw_error2").show();
                    $("#pw_error2").text('(more than 8 charactors includes alphabets, digits, symbols)');
                    return;
                }
                else {
                    $("#pw_error1").hide();
                }

                if (login_pw != login_pw_cf) 
                {
                    $("#pw_error2").show();
                    $("#pw_error2").text('Each password does not match.');    
                }
                else {
                    $("#pw_error1").hide();
                    $("#pw_error2").hide();
                }
            });

        });
    </script>
</head>
<body class="sbd">
<div class="header shd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">Account Info Update</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w" onclick=""><span class="blind">Back</span></a>
			<a href="index" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
				<fieldset>
					<div class="form_wrap">
						<%--<div class="form_box">
							<h3 class="form_tit">Email ID</h3>
							<div class="inp_box btn_box"><!-- 수정 버튼 눌렀을 때 보이게 <input type="email" name="" id="email_id" class="inpt" placeholder="Email ID" /><label for="email_id" class="lblt">Email ID</label>--><span class="valt">leebongs@gmail.com</span><button type="button" class="btn btn_l btn_cb" id="">modify</button></div>
							<p class="error_msg">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Profile ID <span>this will be your profile address</span></h3>
							<div class="inp_box btn_box"><!-- 수정 버튼 눌렀을 때 보이게 <input type="text" name="" id="profile_id" class="inpt" placeholder="@myid" /><label for="profile_id" class="lblt">Email ID</label>--><span class="valt">leebongs</span><button type="button" class="btn btn_l btn_cb" id="">modify</button></div>
							<p class="error_msg">error_msg</p>
						</div>--%>
						<%--<div class="form_box">
							<h3 class="form_tit">Name</h3>
							<div class="inp_box"><input type="text" name="" id="uname" class="inpt" placeholder="Name" /><label for="uname" class="lblt">Name</label></div>
							<p class="error_msg">error_msg</p>
						</div>--%>
                        <div class="form_box">
							<h3 class="form_tit">Current Password <span>(Please enter your current password.)</span></h3>
							<div class="inp_box"><input type="password" name="" id="login_pw_old" class="inpt" placeholder="Password" /><label for="login_pw_old" class="lblt">password</label></div>
							<p class="error_msg" id="pw_error0"></p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Password modify <span>(more than 8 charactors includes alphabets, digits, symbols)</span></h3>
							<div class="inp_box"><input type="password" name="" id="login_pw" class="inpt" placeholder="Password" /><label for="login_pw" class="lblt">password</label></div>
							<p class="error_msg" id="pw_error1"></p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Password Again </h3>
							<div class="inp_box"><input type="password" name="" id="login_pw_cf" class="inpt" placeholder="Password" /><label for="login_pw_cf" class="lblt">password again</label></div>
							<p class="error_msg" id="pw_error2"></p>
						</div>
						<%--<div class="form_box">
							<h3 class="form_tit">휴대폰 번호</h3>
							<div class="inp_box btn_box"><input type="tel" name="" id="mobile" class="inpt" placeholder="휴대폰 번호 입력" /><label for="mobile" class="lblt">휴대폰 번호</label><button type="button" class="btn btn_l btn_cb" id="">인증하기</button></div>
							<p class="error_msg">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">인증번호</h3>
							<div class="inp_box btn_box"><input type="tel" name="" id="mobile" class="inpt" placeholder="인증번호 입력" /><label for="mobile" class="lblt">인증번호</label><button type="button" class="btn btn_l btn_cb" id="">확인</button></div>
							<p class="error_msg">error_msg</p>
						</div>--%>
						<ul class="form_btn form_btn2">
							<li><span><button class="btn_r btn_m btn_cw">Cancel</button></span></li>
							<li><span><button class="btn_r btn_m btn_cp">Save</button></span></li>
						</ul>
					</div>
				</fieldset>
		</div>
	</div>
</div>
<uc1:footer runat="server" ID="footer" />
</body>
</html>
