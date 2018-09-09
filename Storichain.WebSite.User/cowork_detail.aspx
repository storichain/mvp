<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cowork_detail.aspx.cs" Inherits="Storichain.WebSite.User.cowork_detail" %>
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

            $("#ulOption a").on("click", function(e){
                e.preventDefault();

                $("#ulOption a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })

            $("#ulTab a").on("click", function(e){
                e.preventDefault();

                $("#ulTab a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).closest('li');

                console.log($this.data("value"));                
            })

            $('.btn.btn_more').click(function (e) {
                e.preventDefault();
                //doMoreMyPost(pageLoaded + 1, true);
                //$(this).hide();
                //scroll_loading_yn = true;
                console.log('more');
            });

            $('.btn.btn_info').click(function (e) {
                e.preventDefault();

                location.href = 'profile';
            });

            
            $('.btn_lk_lbl').click(function (e) {
                e.preventDefault();

                location.href = 'article/index';
            });

            $('.btn_top').click(function (e) {
                e.preventDefault();

                $("html, body").animate({ scrollTop: 0 }, "slow");
                return false;
            });
            
            $('.btn_down').click(function (e) {
                e.preventDefault();

                $('html, body').animate({scrollTop:$(document).height()}, 'slow');
                return false;
            });

            
        });
    </script>
</head>
<body class="sbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">합평회 상세</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">

			<div class="cwk_head">
				<h3 class="cwk_tit">스토리 형식으로 쓰는 SF 소설 합평회</h3>
				<div class="ckw_ct">
					<div class="str_count"><span>스토리<br />총 5개</span></div>
					<ul class="co_tab" id="ulTab">
						<li data-value='0'><span><a href="#" class="selected">Create Story</a></span></li>
						<li data-value='1'><span><a href="#">Be a Mentor</a></span></li>
						<li data-value='2'><span><a href="#">Comment(4)</a></span></li>
					</ul>
				</div>
			</div>

			<div class="user_flw">
				<h4 class="cwk_pp">이 합평회 만든이</h4>
				<div class="us_if">
					<span class="us_timg"><a href="#"><img src="/images/del/u2.png" alt="" /></a></span>
					<a href="#" class="us_name">@leejunsu</a>
					<button type="button" class="btn_r btn_s btn_cb btn_flw">팔로우</button>
				</div>
			</div>

			<div class="cwk_pn">
				<h4 class="cwk_pnl">이 합평회 패널</h4>
				<div class="sc_wrap">
					<div class="swiper-container">
						<ul class="swiper-wrapper">
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu1</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu2</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu3</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu4</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu5</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu6</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu7</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu8</strong></a></li>
							<li class="swiper-slide"><a href="#"><span><img src="/images/del/u2.png" alt="" class="user_thum_img" /></span><strong>@leejunsu9</strong></a></li>
						</ul>
					</div>
					<div class="swiper-button-next"></div>
					<div class="swiper-button-prev"></div>
				</div>
			</div>

			<ul class="tab" id="ulOption">
				<li data-value='0'><a href="#" class="selected">최근순</a></li>
				<li data-value='1'><a href="#">마지막순</a></li>
				<li data-value='2'><a href="#">별점순</a></li>
				<li data-value='3'><a href="#">즐겨찾기</a></li>
				<li data-value='4'><a href="#">내가 올린</a></li>
			</ul>

			<ul class="content_lst">
				<li>
					<div class="ct_thum">
					</div>
					<div class="ct_info">
						<div class="ct_info_wrap">
							<h2><a href="#">방자는 18세 2화</a></h2>
							<div class="opr"><a href="#" class="btn_opr">공개범위 : <strong>전체</strong></a></div>
							<ul class="stats_lst">
                                <li class="grph"><span>1000</span></li>
                                <li class="cutin"><span>372</span></li>
                                <li class="star"><span>2.5</span></li>
							</ul>
						</div>
					</div>
				</li>

				<li>
					<div class="ct_thum">
						<a href="#"><img src="/images/del/u2.png" alt="" class="user_thum_img" /></a>
					</div>
					<div class="ct_info">
						<div class="ct_info_wrap">
							<h2><a href="#">방자는 18세 2화</a></h2>
							<div class="opr"><a href="#" class="btn_opr">공개범위 : <strong>전체</strong></a></div>
							<ul class="stats_lst">
                                <li class="grph"><span>1000</span></li>
                                <li class="cutin"><span>372</span></li>
                                <li class="star"><span>2.5</span></li>
							</ul>
						</div>
					</div>
				</li>
			</ul>
			<div class="fbtn">
				<button type="button" class="btn btn_more">more</button>
			</div>
		</div>
	</div>

    <div class="fx_btn ct_lk">
        <ul class="drt_btn">
            <li><a href="#" class="btn_top"><span>top</span></a></li>
            <li><a href="javascript:history.back();" class="btn_back"><span>back</span></a></li>
            <li><a href="#" class="btn_down"><span>down</span></a></li>
        </ul>
    </div>

</div> 

<uc1:footer runat="server" id="footer" />

<script>
$(function() {
	var cwk_pn_swiper = new Swiper('.cwk_pn .swiper-container', {
		setWrapperSize: true,
		autoHeight: true,
		slidesPerView: 6,
		breakpoints: {
			720: {
			slidesPerView: 5,
			},
			580: {
			slidesPerView: 4,
			},
			440: {
			slidesPerView: 3,
			}
		},
		navigation: {
			nextEl: '.cwk_pn .swiper-button-next',
			prevEl: '.cwk_pn .swiper-button-prev'
		}
	});
});
</script>
</body>
</html>
