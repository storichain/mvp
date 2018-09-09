<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile_form.aspx.cs" Inherits="Storichain.WebSite.User.profile_form" %>
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
        var g_user_idx = <%=WebUtility.UserIdx()%>;
        $(function() {

            $('.btn_r.btn_m.btn_cw').click(function (e) {
                e.preventDefault();

                location.href = 'profile';
            });

            $('.btn_r.btn_m.btn_cp').click(function (e) {
                e.preventDefault();

                if(!confirm('Do you want to save your data?'))
                    return false;

                if ($("#wname").val() == "") {
                    alert("Please enter a name.");
                    return false;
                } else if ($("#introduce").val() == "") {
                    alert("Please enter my introduce information.");
                    return false;
                } else if ($("#str_cate").val() == "") {
                    alert("Please enter interest story field.");
                    return false;
                }

                var formData = new FormData();
                formData.append('user_idx', g_user_idx);
                formData.append('user_name', $("#wname").val());
                formData.append('introduce', $("#introduce").val());
                formData.append('interest_field', $("#str_cate").val());
                if ($("#img").val() != "")
                    formData.append('upload', $("#img")[0].files[0]);

                //var p = jQuery.param({ user_idx:g_user_idx, user_name:$("#wname").val(), introduce:$("#introduce").val(), interest_field:$("#str_cate").val() });
                //console.log(p);
                
                $.ajax({
                    type: "Post",
                    url: "/User/ModifyUserProfile",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            alert("It was completed.");
                            location.href = "/profile";
                        }
                        else 
                        {
                            alert("Please enter the correct data.");
                            return;
                        }
                    }
                });

            });

            $("#wname").on('keyup keypress blur', function (event) {
                var wname = $(this).val();

                console.log(wname);
                
                //if(login_pw_old.length < 8) {
                //    $("#pw_error0").show();
                //    $("#pw_error0").text('(more than 8 charactors includes alphabets, digits, symbols)');
                //    return;
                //}
                //else {
                //    $("#pw_error0").hide();
                //}

                
            });

            $("#str_cate").on('keyup keypress blur', function (event) {
                var str_cate = $(this).val();

                console.log(str_cate);
                
                //if(login_pw_old.length < 8) {
                //    $("#pw_error2").show();
                //    $("#pw_error2").text('(more than 8 charactors includes alphabets, digits, symbols)');
                //    return;
                //}
                //else {
                //    $("#pw_error2").hide();
                //}
            });

            

            

            $('#btnInstagram').click(function (e) {
                e.preventDefault();
                console.log('Instagram');

            });

            $('#btnNaver').click(function (e) {
                e.preventDefault();
                console.info('naver');
            });

            $('#btnFacebook').click(function (e) {
                e.preventDefault();
                console.info('facebook');
            });

            $('.btn.btn_img_reg').click(function (e) {
                e.preventDefault();
                $("#img").click();
            });

            $('.pf_img_r').click(function (e) {
                e.preventDefault();
                $("#img").click();
            });

        });

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#userImg').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
            }
        }

    </script>


</head>
<body class="sbd">
<% 
    int user_idx = PageUtility.UserIdx().ToInt();
    Biz_User biz_u = new Biz_User(user_idx);
%>
<div class="header shd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">Profile</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<form action="">
				<fieldset>
					<div class="form_wrap">
						<div class="profile_img_area">
							<div class="pf_img">
                                <div class="pf_img_r"><img id="userImg" src="<%= biz_u.drData.ImageUrl("user", "user_thumb", "/images/img_user_my.png") %>" alt="" /></div>
								<button type="button" class="btn btn_img_reg"><span class="blind">Upload picture</span></button>
                                <input type="file" id="img" name="files" value="" style="display:none;" onchange="readURL(this);" />
								<!-- 삭제버튼. 아니면 등록버튼을 눌렀을때 기본이미지로 갈것인지 사진등록을 할것인지를 레이어로 넣어서 선택하게 하는건 어떨까요??
									<button type="button" class="btn btn_img_del"><span class="blind">사진삭제</span></button>
								-->
							</div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Name</h3>
							<div class="inp_box"><input type="text" name="" id="wname" class="inpt" placeholder="Name" value="<%= biz_u.User_Name %>" /><label for="wname" class="lblt">Name</label></div>
							<p class="error_msg" id="pw_error1">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">About Me</h3>
							<div class="inp_box inp_tbox"><textarea name="" id="introduce" class="ftxt" cols="30" rows="10" placeholder="About Me"><%= biz_u.Introduce %></textarea><label for="introduce" class="lblt">About Me</label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Interest story field</h3>
							<div class="inp_box"><input type="text" name="" id="str_cate" class="inpt" placeholder="Please separate it with a comma" value="<%= biz_u.InterestField %>" /><label for="str_cate" class="lblt">Interest story field</label></div>
							<p class="error_msg" id="pw_error2">error_msg</p>
						</div>
						<p class="sct">Entering the social information below will give you a better chance to get suggestions from advertisers.</p>

						<ul class="sc_link"><!-- 버튼 클릭시 각 버튼 clsss를 연결되었을 경우 btn_cb로, 안되어있을 경우 btn_cw 로 -->
							<li><span class="sc_name">Instagram</span><button type="button" class="btn btn_s btn_cw" id="btnInstagram">Connect</button></li>
							<li><span class="sc_name">Naver Blog</span><button type="button" class="btn btn_s btn_cw" id="btnNaver">Connect</button></li>
							<li><span class="sc_name">Facebook</span><button type="button" class="btn btn_s btn_cw" id="btnFacebook">Connect</button></li>
						</ul>

						<ul class="form_btn form_btn2">
							<li><span><button class="btn_r btn_m btn_cw">Cancel</button></span></li>
							<li><span><button class="btn_r btn_m btn_cp">Save</button></span></li>
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
