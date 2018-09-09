<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="growthgraph.aspx.cs" Inherits="Storichain.WebSite.User.growthgraph" %>
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

            $("#ulDate a").on("click", function(e){
                e.preventDefault();

                $("#ulDate a").removeClass("selected");
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
			<h1 class="ptit"><a href="#null">이 스토리의 현황</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
	<div class="h_tab">
		<div class="tab_wrap">
			<ul class="htab">
				<li><a href="rewardpi">보상풀</a></li>
				<li class="selected"><a href="growthgraph">성장 그래프</a></li>
				<li><a href="trade">판권/스폰싱</a></li>
			</ul>
		</div>
	</div>
</div>

<div class="container">
	<div class="category">
		<div class="ct_wrap">
			<div class="category_tit">
				<h2>성장지수별 보상풀 조건보기</h2>
			</div>
			<ul class="cat_lst" id='ulDate'>
				<li data-value='d'><a href="#" class="selected">Day</a></li>
				<li data-value='w'><a href="#">Week</a></li>
				<li data-value='m'><a href="#">Month</a></li>
				<li data-value='y'><a href="#">Year</a></li>
			</ul>
		</div>
	</div>

	<div class="content">
		<div class="ct_wrap">
			<ul class="chart_lst">
				<li>
					<h3 class="chart_rtit">성장지수 1,835,329 <a href="#" class="qst" id="qst1"><span class="blind">?</span></a></h3>
					<div class="chart_area"><img src="/images/del/chart2.png" alt="" /></div>
				</li>
				<li>
					<h3 class="chart_rtit">성장지표 5,329 <a href="#" class="qst" id="qst2"><span class="blind">?</span></a></h3>
					<div class="chart_area"><img src="/images/del/chart3.png" alt="" /></div>
				</li>
			</ul>
		</div>
	</div>

</div>

<div class="popup" id="qst1_popup" style="display:none">
	<div class="popup_wrap">
		<h3 class="popup_tit">성장지수</h3>
		<div class="popup_ct">
			<div class="growthindex">
				<p><strong>성장지수</strong> = 비교기준시점성장지표의 합 / 기준시점 성장지표의 합 X 100</p>
				<p>예) 일별 성장지수 = <br />오늘성장지표 /  어제성장지표 X 100</p>
				<p>인증메일이 보이지 않을 경우 스팸 메일함도 살펴주세요</p>
			</div>
		</div>
		<ul class="popup_btn popup_btn1">
			<li><span><button class="btn_r btn_m btn_cp btn_popup_close">확인</button></span></li>
		</ul>
	</div>
</div>

<div class="popup" id="qst2_popup" style="display:none">
	<div class="popup_wrap">
		<h3 class="popup_tit">활동지표(Activity Metrics)</h3>
		<div class="popup_ct">
			<div class="metrics">
				<ul class="metrics_ul">
					<li><span class="gpc1"></span>작가군 지표
						<ul class="metrics_ul2">
							<li>글라인 지표 : 시간당 평균 글자수</li>
						</ul>
					</li>
					<li><span class="gpc2"></span>독자군 지표</li>
					<li><span class="gpc3"></span>스폰싱 지표
						<ul class="metrics_ul2">
							<li>유료결제금액의 백단위 절사한 수</li>
							<li>광고금액의 천단위 절사한 수</li>
							<li>판권금액의 만단위 절사한 수</li>
						</ul>
					</li>
					<li><span class="gpc4"></span>채워진 Bar 비교 시점</li>
					<li><span class="gpc5"></span>비어있는 Bar 비교기준 시점</li>
				</ul>
			</div>
		</div>
		<ul class="popup_btn popup_btn1">
			<li><span><button class="btn_r btn_m btn_cp btn_popup_close">확인</button></span></li>
		</ul>
	</div>
</div>
<uc1:footer runat="server" id="footer" />
<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
<script type="text/javascript">
	$(function() {
		$('.qst').on('click',function(){
			var poptg = $(this).attr('id') + '_popup';
			$('#' + poptg).show();
			$('.black_scrn').addClass('active');
		});
		$('.btn_popup_close').on('click',function(){
		   $(this).parents('.popup').hide();
			$('.black_scrn').removeClass('active');
		});
	});
</script>
</body>
</html>
