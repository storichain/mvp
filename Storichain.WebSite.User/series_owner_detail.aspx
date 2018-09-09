<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="series_owner_detail.aspx.cs" Inherits="Storichain.WebSite.User.series_owner_detail" %>
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

            $('.btn.btn_set').click(function (e) {
                e.preventDefault();

                console.log('connect');
            });

            $('.btn.btn_more').click(function (e) {
                e.preventDefault();

                console.log('more');
            });

            $('.btn.btn_url_make').click(function (e) {
                e.preventDefault();

                console.log('1');
            });

            $('.btn.btn_url_copy').click(function (e) {
                e.preventDefault();

                console.log('2');
            });

            $('.btn.btn_r.btn_l.btn_cp').click(function (e) {
                e.preventDefault();

                console.log('3');
            });

            
        });
    </script>
</head>
<body class="sbd srsbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">시리즈 상세</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="mtit">
		<div class="ct_wrap">
		</div>
	</div>

	<div class="series_hd">
		<div class="ct_wrap">
			<div class="series_tit">
				<h2><img src="/images/ic_dr.png" alt="description" /> WORRIORS OF THE DAWNWORRIORS OF THE DAWNWORRIORS OF THE DAWNWORRIORS OF THE DAWNWORRIORS OF THE DAWN</h2>
				<button type="button" class="btn btn_set"><span>이 시리즈 설정</span></button>
			</div>
			<div class="series_info">
				<ul>
					<li><span><em>에피소드</em><strong>총 5개</strong></span></li>
					<li><span><em>갈래씬</em><strong>총 52451개</strong></span></li>
					<li><span><em>별점평균</em><strong>총 5개</strong></span></li>
					<li><span><em>에피소드</em><strong>총 5개</strong></span></li>
				</ul>
			</div>
		</div>
	</div>

	<div class="content">
		<div class="ct_wrap">
			<ul class="tab" id='ulOption'>
				<li data-value='0'><a href="#" class="selected">최근순</a></li>
				<li data-value='1'><a href="#">마지막순</a></li>
			</ul>

			<ul class="content_lst series_lst">
				<li>
					<div class="ct_thum">
					</div>
					<div class="ct_info">
						<div class="ct_info_wrap">
							<h2><a href="#">방자는 18세 2화방자는 18세 2화방자는 18세 2화방자는 18세 2화방자는 18세 2화방자는 18세 2화방자는 18세 2화</a></h2>
							<div class="series_num">[EPISODE 1]</div>
							<ul class="stats_lst">
                                <li class="grph"><span>1000</span></li>
                                <li class="cutin"><span>372</span></li>
                                <li class="star"><span>2.5</span></li>
							</ul>
							<div class="star_point"><strong class="stp" style="width:50%"><span class="blind">3.0</span></strong></div>
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
							<div class="series_num">[EPISODE 2]</div>
							<ul class="stats_lst">
                                <li class="grph"><span>1000</span></li>
                                <li class="cutin"><span>372</span></li>
                                <li class="star"><span>2.5</span></li>
							</ul>
							<div class="star_point"><strong class="stp" style="width:50%"><span class="blind">3.0</span></strong></div>
						</div>
					</div>
				</li>
			</ul>
			<div class="fbtn">
				<button type="button" class="btn btn_more">ADD STORIES as EPDISODE +</button>
			</div>
		</div>
	</div>

	<div class="series_own_ft">
		<div class="ct_wrap">
			<form action="#">
				<div class="series_url">http://www.storichain.com/ajhditjsitjdisltjidjtktjitklejajhditjsitjdisltjidjtktjitklejajhditjsitjdisltjidjtktjitklejajhditjsitjdisltjidjtktjitklejajhditjsitjdisltjidjtktjitklej</div>
				<div class="series_select">
					<div class="series_open">
						<button type="button" class="btn btn_open"><span>공개범위 선택</span><img src="/images/bg_arrow_layer_g.png" alt="" /></button>
						<div class="seires_open_list">
							<ul>
								<li><input type="radio" class="srdo" name="sop" id="sop1" checked="checked" /><label for="sop1">전체 공개본</label></li>
								<li><input type="radio" class="srdo" name="sop" id="sop2" /><label for="sop2">피드 공개본</label></li>
								<li><input type="radio" class="srdo" name="sop" id="sop3" /><label for="sop3">링크 공개본</label></li>
								<li><input type="radio" class="srdo" name="sop" id="sop4" /><label for="sop4">비공개본</label></li>
							</ul>
						</div>
					</div>
					<ul class="series_url_btn">
						<li><span><button type="button" class="btn btn_url_make">링크 생성</button></span></li>
						<li><span><button type="button" class="btn btn_url_copy">링크 복사</button></span></li>
					</ul>
				</div>
				<div class="series_confirm">
					<button type="button" class="btn btn_r btn_l btn_cp">링크 생성</button>
				</div>
			</form>
		</div>
	</div>

</div> 

<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
<script type="text/javascript">
	$(function() {
		$('.btn_open').on('click',function(){
			if ($(this).hasClass('show_layer')){
				$(this).removeClass('show_layer');
				$('.seires_open_list').hide();
			} else {
				$(this).addClass('show_layer');
				$('.seires_open_list').show();
			}
		});
	});
</script>

</body>
</html>
