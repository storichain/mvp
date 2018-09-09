<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trade.aspx.cs" Inherits="Storichain.WebSite.User.trade" %>
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
            $("#ulOpen a").on("click", function(e){
                e.preventDefault();

                $("#ulOpen a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })

            
        });
    </script>
</head>
<body class="tbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">이 스토리의 현황</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
	<div class="h_tab">
		<div class="tab_wrap">
			<ul class="htab">
				<li><a href="rewardpi">보상풀</a></li>
				<li><a href="growthgraph">성장 그래프</a></li>
				<li class="selected"><a href="trade">판권/스폰싱</a></li>
			</ul>
		</div>
	</div>
</div>

<div class="container">

	<div class="content">
		<div class="ct_wrap">
			<div class="rs_content rgt_content">
				<h3>판권</h3>
				<ul class="rs_lst">
					<li><a href="#">영화화 판권 계약</a></li>
					<li><a href="#">드라마판권 계약</a></li>
					<li><a href="#">출판 판권 계약</a></li>
					<li><a href="#">만화/웹툰 판권 계약</a></li>
					<li><a href="#">애니메이션 판권 계약</a></li>
					<li><a href="#">스낵컬쳐 무비 판권 계약</a></li>
					<li><a href="#">CF/홍보영상 판권 계약</a></li>
					<li><a href="#">라이도 팟캐스트 판권 계약</a></li>
				</ul>
			</div>
			<div class="rs_content sps_content">
				<h3>스폰싱</h3>
				<ul class="rs_lst">
					<li><a href="#">광고 후원</a></li>
					<li><a href="#">쿠폰 배포</a></li>
					<li class="bg_pk"><a href="#">배우로 출연 희망</a></li>
				</ul>
			</div>
		</div>
	</div>

</div>
<uc1:footer runat="server" id="footer" />
<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
</body>
</html>
