<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="rewardpi.aspx.cs" Inherits="Storichain.WebSite.User.rewardpi" %>
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
            $("#ulSale a").on("click", function(e){
                e.preventDefault();

                $("#ulSale a").removeClass("selected");
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
				<li class="selected"><a href="rewardpi">보상풀</a></li>
				<li><a href="growthgraph">성장 그래프</a></li>
				<li><a href="trade">판권/스폰싱</a></li>
			</ul>
		</div>
	</div>
</div>

<div class="container">
	<div class="category">
		<div class="ct_wrap">
			<div class="category_tit">
				<h2>성장그래프 조건보기</h2>
			</div>
			<ul class="cat_lst" id='ulSale'>
				<li data-value='100_l'><a href="#" class="selected">100이하</a></li>
				<li data-value='100_g'><a href="#">100+</a></li>
				<li data-value='300_g'><a href="#">300+</a></li>
				<li data-value='700_g'><a href="#">700+</a></li>
				<li data-value='900_g'><a href="#">900+</a></li>
				<li data-value='1k_g'><a href="#">1K+</a></li>
				<li data-value='3k_g'><a href="#">3K+</a></li>
				<li data-value='1m_g'><a href="#">1M+</a></li>
				<li data-value='0'><a href="#">판권판매시</a></li>
			</ul>
		</div>
	</div>

	<div class="content">
		<div class="ct_wrap">
			<div class="chart_desc">
				<p>분배주기 별 주기 내 쌓인 수익을 아래와 같이 분배 성장지수별 보상품 비율은 다를 수 있습니다.<br />이 스토리의 분배주기는 기여활동 <strong>7일 후</strong> 입니다.<br />지갑으로 인출은 판권/스폰싱 된 후 다음 분배주기 때 가능합니다.</p>
			</div>
			<ul class="chart_lst">
				<li>
					<div class="chart_area"><img src="/images/del/chart1.png" alt="" /></div>
				</li>
			</ul>
		</div>
	</div>
</div>
<uc1:footer runat="server" id="footer" />
<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
</body>
</html>
