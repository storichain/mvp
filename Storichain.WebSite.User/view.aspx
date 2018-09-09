<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="view.aspx.cs" Inherits="Storichain.WebSite.User.view" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
</head>
<body class="view">
<div class="header vhd">
	<div class="hd_wrap">
		<h1 class="vtit">방자는 18세 1화</h1>
		<a href="javascript:history.back();" class="btn btn_back_b"><span class="blind">Back</span></a>
		<button type="button" class="btn btn_cmt_on">껴 on</button>
		<button type="button" class="btn btn_status">status</button>
	</div>
</div>

<div class="vcontainer">

	<div class="view_content">
		<div class="time">3 hours ago</div>
		<div class="cmt cmt_l">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다. 샘플텍스트 입니다. 샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<div class="cmt cmt_r">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<div class="cmt cmt_l">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다. 샘플텍스트 입니다. 샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<div class="cmt cmt_r">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<div class="cmt cmt_l">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다. 샘플텍스트 입니다. 샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<div class="cmt cmt_r">
			<div class="cmt_user"><img src="/images/del/u.png" alt="" class="user_pf_img" /><strong class="user_name">방자</strong></div>
			<ul class="cmt_lst">
				<li><span>샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
				<li><span>샘플텍스트 입니다.</span></li>
			</ul>
		</div>
		<button class="btn scene_on"><span class="blind">scene_on</span></button>
	</div>

	<div class="view_btn_ft">
		<div class="ct_wrap">
			<ul class="view_btn_lst">
				<li><a href="#" class="btn btn_view_ft">더보기</a></li>
				<li><a href="#" class="btn btn_view_ft">사러갈래</a></li>
				<li><a href="#" class="btn btn_view_ft">사지않을래</a></li>
			</ul>
		</div>
	</div>

	<div class="vcomment_wrt">
		<div class="ct_wrap">
			<div class="inp_ctxt">
				<textarea name="" id="ctxt_input" class="ctxt" cols="30" rows="10" placeholder="끼어들기 입력"></textarea>
				<button type="button" class="btn btn_ptup"><span class="blind">사진올리기</span></button>
				<button type="button" class="btn btn_cmt_reg"><span class="blind">끼어들기 등록</span></button>
			</div>
		</div>
	</div>
</div>

</body>
</html>
