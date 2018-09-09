<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="series_detail.aspx.cs" Inherits="Storichain.WebSite.User.series_detail" %>
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
        var g_user_idx = <%=WebUtility.GetRequestByInt("profile_user_idx", WebUtility.UserIdx()).ToValue()%>;

        $(function() {

            $("#ulOption a").on("click", function(e){
                e.preventDefault();

                $("#ulOption a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })

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

            $('.btn_r.btn_s.btn_cb.btn_flw').click(function (e) {
                e.preventDefault();

                console.log('follow');
            });

            //$('.btn.btn_memu').click(function (e) {
            //    e.preventDefault();

            //    location.href = 'logout';
            //});

            
        });
    </script>
</head>
<body class="sbd">
<% 
    int user_idx        = WebUtility.GetRequestByInt("profile_user_idx", WebUtility.UserIdx());
    Biz_User biz_user   = new Biz_User();
    DataTable dtUser    = biz_user.GetUser(user_idx, 0);
    DataRow drUser      = null;

    if(dtUser.Rows.Count > 0)
        drUser = dtUser.Rows[0];

    if(drUser == null)
    {
        Response.Redirect("login");
        return;
    }
%>
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
				<button type="button" class="btn btn_sbc"><span>이 시리즈 구독</span></button>
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
			<div class="user_flw">
				<h4 class="cwk_pp">이 시리즈의 작가</h4>
				<div class="us_if">
					<span class="us_timg"><a href="#"><img src="/images/del/u2.png" alt="" /></a></span>
					<a href="#" class="us_name">@leejunsu</a>
                    <%   string view_profile = "block";
                        if (WebUtility.GetRequestByInt("user_idx", WebUtility.UserIdx()) != WebUtility.UserIdx()) 
                            view_profile = "none";%>
					<button type="button" class="btn_r btn_s btn_cb btn_flw" style="display:<%= view_profile %>">팔로우</button>
				</div>
			</div>

			<ul class="tab" id='ulOption'>
		        <li data-value='desc'><a href="#" class="selected">Latest</a></li>
		        <li data-value='asc'><a href="#">Oldest</a></li>
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

    <div class="fx_btn ct_lk">
        <ul class="drt_btn">
            <li><a href="#" class="btn_top"><span>top</span></a></li>
            <li><a href="javascript:history.back();" class="btn_back"><span>back</span></a></li>
            <li><a href="#" class="btn_down"><span>down</span></a></li>
        </ul>
    </div>

</div> 

<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
</body>
</html>
