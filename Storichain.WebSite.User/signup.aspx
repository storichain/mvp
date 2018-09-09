<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="signup.aspx.cs" Inherits="Storichain.WebSite.User.signup" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Libs/footer.ascx" TagPrefix="uc1" TagName="footer" %>

<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
    <style>
        #email_error{color:red;display:none}
        #nickname_error{color:red;display:none}
    </style>
    <script>
        $(function() {
            
            $("#login_email").on('keyup keypress blur', function (event) {
                var userId = $("#login_email").val();

                if(userId.length <= 8) {
                    $("#email_error").show();
                    $("#email_error").text('(more than 8 charactors includes alphabets, digits, symbols)');
                    return;
                }
                else {
                    $("#email_error").hide();
                }

                if(!isEmail(userId)) {
                    $("#email_error").show();
                    $("#email_error").text('(Please enter a valid email format.)');
                    return;
                }
                else {
                    $("#email_error").hide();
                }

                if (userId != "" && userId.indexOf("@") > -1 && userId.indexOf(".") > -1) {
                    $.ajax({
                        type: "GET",
                        url: "/User/ExistSiteUserId",
                        data: "site_user_id=" + userId,
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if (response.response_code == "1000") {
                                $("#email_error").hide();
                                
                            } else if (response.response_code == "2000") {
                                $("#email_error").show();
                                $("#email_error").text('(This account exists.)');
                            }
                        }
                    });
                } else {
                    $("#email_error").hide();
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

            $("#nick_name").on('keyup keypress blur', function (event) {
                
                var nick_name = $("#nick_name").val();

                if(nick_name.length < 6) {
                    $("#nickname_error").show();
                    $("#nickname_error").text('(more than 6 charactors)');
                    return;
                }
                else {
                    $("#nickname_error").hide();
                }

                $.ajax({
                    type: "GET",
                    url: "/User/ExistNickName",
                    data: "nick_name=" + nick_name,
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            $("#nickname_error").hide();
                        } else if (response.response_code == "2000") {
                            $("#nickname_error").show();
                            $("#nickname_error").text('(This profile name exists.)');
                        }
                    }
                });



            });

            $('#btnSave').click(function (e) {
                e.preventDefault();

                if(!confirm('Do you want to save your data?'))
                    return false;

                if ($("#login_email").val() == "") {
                    alert("Please enter your ID.");
                    $("#login_email").focus();
                    return false;
                } else if ($("#login_pw").val() == "") {
                    alert("Please enter a password");
                    return false;
                } else if ($("#login_pw").val().length < 8) {
                    alert("Please enter at least 8 digits");
                    return false;
                } else if ($("#login_pw").val() != $("#login_pw_cf").val()) {
                    alert("Invalid password verification Please re-enter");
                    $("#login_pw_cf").val("");
                    return false;
                } else if ($("#nick_name").val() == "") {
                    alert("Please enter your profile name");
                    $("#nick_name").focus();
                    return false;
                } else if ($("#language").val() == "") {
                    alert("Please select a language.");
                    $("#language").focus();
                    return false;
                }

                var formData = new FormData();
                formData.append('user_id', $("#login_email").val());
                formData.append('email', $("#login_email").val());
                formData.append('nick_name', $("#nick_name").val());
                formData.append('pwd', $("#login_pw").val());
                formData.append('language', $("#language").val());
                formData.append('sns_type_idx', '10');

                console.log(formData);

                $.ajax({
                    type: "Post",
                    url: "/User/SNSJoin",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            alert("Your signup is complete.");
                            location.href = "/login";
                        }
                        else if (response.response_code == "2011") {
                            alert("This account exists.");
                            return;
                        }
                        else if (response.response_code == "2012") {
                            alert("The name of an existing profile.");
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

            $('#btnNickname').click(function (e) {
                e.preventDefault();

                var nick_name = $("#nick_name").val();

                if(nick_name.length < 6) {
                    $("#nickname_error").show();
                    $("#nickname_error").text('(more than 6 charactors)');
                    return;
                }
                else {
                    $("#nickname_error").hide();
                }

                $.ajax({
                    type: "GET",
                    url: "/User/ExistNickName",
                    data: "nick_name=" + nick_name,
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            $("#nickname_error").hide();
                        } else if (response.response_code == "2000") {
                            $("#nickname_error").show();
                            $("#nickname_error").text('(This profile name exists.)');
                        }
                    }
                });

            });


        });

        function checkEmail(userId) {
            $.ajax({
                type: "GET",
                url: "/User/ExistSiteUserId",
                data: "site_user_id=" + userId,
                dataType: "json",
                cache: false,
                success: function (response) {
                    if (response.response_code == "1000") {
                        $("#email_error").hide();
                    } else if (response.response_code == "2000") {
                        $("#email_error").show();
                    }
                }
            });
        }

        function isEmail(email) {
          var regex = /^([a-zA-Z0-9_.+-])+\@(([a-zA-Z0-9-])+\.)+([a-zA-Z0-9]{2,4})+$/;
          return regex.test(email);
        }
</script>

</head>
<body class="sbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">Signup</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<h2 class="ct_logo"><img src="/images/logo_txt.png" alt="storichain" /></h2>
			<form action="">
				<fieldset>
					<div class="form_wrap">
						<div class="form_box">
							<h3 class="form_tit">Email ID <span ></span></h3>
							<div class="inp_box"><input type="email" name="" id="login_email" class="inpt" placeholder="Email ID" /><label for="login_email" class="lblt">Email ID</label></div>
							<p class="error_msg" id="email_error"></p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Password <span>(more than 8 charactors includes alphabets, digits, symbols)</span></h3>
							<div class="inp_box"><input type="password" name="" id="login_pw" class="inpt" placeholder="Password" /><label for="login_pw" class="lblt">password</label></div>
							<p class="error_msg" id='pw_error1'></p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Password Again</h3>
							<div class="inp_box"><input type="password" name="" id="login_pw_cf" class="inpt" placeholder="Password" /><label for="login_pw_cf" class="lblt">password again</label></div>
							<p class="error_msg" id='pw_error2'></p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Profile ID <span>this will be your profile address</span></h3>
							<div class="inp_box btn_box"><input type="text" name="" id="nick_name" class="inpt" placeholder="myid" /><label for="nick_name" class="lblt">Email ID</label><button type="button" class="btn btn_l btn_cb" id="btnNickname">check</button></div>
							<p class="error_msg"  id="nickname_error">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Language</h3>
							<div class="inp_box">
								<select id="language" name="language" title="language" class="sel">
									<option value="" selected="">- select -</option>
									<option value="0">Korean</option>
									<option value="1">English</option>
								</select>
							</div>
						</div>
						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cp" id='btnSave'>Save</button></span></li>
						</ul>
					</div>
				</fieldset>
			</form>
		</div>
	</div>
</div>
<uc1:footer runat="server" id="footer" />

</body>
</html>
