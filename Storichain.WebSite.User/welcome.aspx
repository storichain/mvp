<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="welcome.aspx.cs" Inherits="Storichain.WebSite.User.welcome" EnableEventValidation="true" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
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

            $('#btnSave').click(function (e) {
                e.preventDefault();

                if(!confirm('Do you want to save your data?'))
                    return false;

                $('#hdfRole').val($("input[name=choose]").filter(":checked").val());
                __doPostBack('lbnSend','');

            });

        });

        
    </script>
</head>
<body class="wbd">
    <form id="form1" runat="server">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="welcome">Welcome</a></h1>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<h2 class="ct_logo"><img src="/images/logo_txt.png" alt="storichain" /></h2>

				<fieldset>
					<div class="welcome_wrap">
						<h2 class="welcome_tit">Welcome!</h2>
						<div class="welcome_ct">
							<p class="welcome_txt1">Now, you can creat, sell, trade, <br />fund stories assets</p>
							<h3 class="choose">Choose Your User Role</h3>
							<ul class="choose_lst">
								<li><span><input type="radio" id="storyteller" name="choose" value="5" checked="checked"><label id="stLb" for="storyteller">Storyteller</label></span></li>
								<li><span><input type="radio" id="reader" name="choose" value="7"><label id="stLb" for="reader">Reader</label></span></li>
								<li><span><input type="radio" id="pd" name="choose" value="4"><label id="stLb" for="pd">PD</label></span></li>
							</ul>
							<p class="welcome_txt2">Check your email <a href="mailto:<%= PageUtility.UserId() %>">(<%= PageUtility.UserId() %>)</a><br /> and click the registration link.</p>
							<p class="welcome_txt3">the registration linke validates 7 days only.</p>
							<p class="welcome_txt4">check the spam mail box too</p>
						</div>
						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cp" id="btnSave">Save</button></span></li>
						</ul>
					</div>
				</fieldset>
		</div>
	</div>
</div>

<asp:LinkButton ID="lbnSend" runat="server" OnClick="btnSend_Click"></asp:LinkButton>
<asp:Literal ID="ltrScript" runat="server"></asp:Literal>
<asp:HiddenField ID="hdfRole" runat="server" />
    </form>
</body>
</html>
