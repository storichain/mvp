<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="discover.aspx.cs" Inherits="Storichain.WebSite.User.discover" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Libs/footer.ascx" TagPrefix="uc1" TagName="footer" %>
<%@ Register Src="~/Libs/footer_menu.ascx" TagPrefix="uc1" TagName="footer_menu" %>

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
            doGetMyPost(myPost_json);

            $(window).scroll(function () {

                var buffer = 100;
                var storyList_view = $(window);

                if ($("html").prop('scrollHeight') - storyList_view.scrollTop() <= storyList_view.height() + buffer) 
                {
                    if (pageLoaded > 0 && scroll_loading_yn == true) 
                    {
                        if (page_count > pageLoaded) 
                        {
                            doMoreMyPost(pageLoaded + 1, true);
                        }
                    }
                }
            });


            $("#ulStory a").on("click", function(e){
                e.preventDefault();

                $("#ulStory a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })

            $('.btn.btn_alert_close').click(function (e) {
                e.preventDefault();

                $('.alert').hide();
            });

            
            $('.btn.btn_more').click(function (e) {
                e.preventDefault();
                doMoreMyPost(pageLoaded + 1, true);
                $(this).hide();
                scroll_loading_yn = true;
            });

            //$('.btn.btn_close_aside').click(function (e) {
            //    e.preventDefault();

            //    alert('close');
            //});

            //$('.btn.btn_memu').click(function (e) {
            //    e.preventDefault();

            //    location.href = 'logout';
            //});

            $('.btn.btn_info').click(function (e) {
                e.preventDefault();

                console.log('information');
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

            $('.btn.btn_search').click(function (e) {
                e.preventDefault();

                alert('search');
            });

            $(document).on("click", "button[id^='btnFollow']", function() {

                var $item = $(this).parent().closest('.user_intro');
                var to_user_idx = $item.attr('user_idx');

                if(to_user_idx == 0) 
                {
                    alert('처리 중 오류가 발생하였습니다.');
                    return;
                }

                var msg = '팔로우 하시겠습니까?'

                if($(this).hasClass('btn_uflw'))
                    msg = '언팔로우 하시겠습니까?'
                
                if(confirm(msg)) 
                {
                    $.ajax({
                    type: "POST",
                        url: url_path + "/Follow/Toggle",
                        data: stringFormat("user_idx={0}&to_user_idx={1}", g_user_idx, to_user_idx),
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}
                            if(response.response_code == '1000') 
                            {
                                if(response.response_data_count > 0) {

                                    var row = response.response_data[0];
                                    //alert($(stringFormat('#btnFollow_{0}', row["to_user_idx"])).html());
                                    var $item = $(stringFormat('#btnFollow_{0}', row["to_user_idx"]));

                                    $("button[id^='btnFollow']").each(function(i){

                                        var to_user_idx1 = $(this).attr('id').split('_')[1];

                                        if(row["to_user_idx"] == to_user_idx1) 
                                        {
                                            if(response.response_data[0]["follow_yn"] == 'Y') 
                                            {
                                                $(this).removeClass('btn_cb');
                                                $(this).removeClass('btn_flw');
                                                $(this).addClass('btn_cwb');
                                                $(this).addClass('btn_uflw');
                                                $(this).html('언팔로우');
                                            }
                                            else 
                                            {
                                                $(this).removeClass('btn_cwb');
                                                $(this).removeClass('btn_uflw');
                                                $(this).addClass('btn_cb');
                                                $(this).addClass('btn_flw');
                                                $(this).html('팔로우');
                                            }
                                        }
                                    });                                    
                                }

                                
                                
                            }
                            else {
                                alert('처리 중 오류가 발생하였습니다.');
                            }
                        },
                        error: ajaxError,
                        complete: function () {

                        }
                    });
                }
                
            });




        });




        var pageLoaded = 1;
        var page_count = 0;
        var is_get_data = false
        var scroll_loading_yn = false;

        function doGetMyPost(myPostData) {

            page_count = myPostData.response_option.page_count;
            console.log(pageLoaded + '/' + page_count);

            $.each(myPostData.response_data, function (i, value) {

                var topic_idx            = value.topic_idx;
                var event_idx            = value.event_idx;
                var user_idx             = value.user_idx;
                var user_name            = value.user_name;
                var nick_name            = value.nick_name;
                var supply_name          = value.supply_name;
                var supply_desc          = value.supply_desc.replace(/\n/g, "<br />");
                var create_date          = value.create_date;
                var topic_name           = value.topic_name;
                var follow_yn            = value.follow_yn;

                var html = '<li>\
					            <div class="user_thum">\
                                    <a href="#"><img src="{4}" alt="" class="user_thum_img" /></a>\
					            </div>\
					            <div class="user_intro" user_idx="{5}">\
						            <img src="{0}" alt="" class="user_pf_img" />\
						            <div class="wname">\
							            <h3><a href="#">{1}</a></h3>\
							            <h4>(@{2})</h4>\
						            </div>\
						            <p class="wdesc">{3}</p>\
						            <ul class="stats_lst">\
							            <li class="grph"><span>1000</span></li>\
							            <li class="cutin"><span>372</span></li>\
							            <li class="star"><span>2.5</span></li>\
						            </ul>\
						            <button type="button" class="btn_r btn_s {6}" id="btnFollow_{5}" style="display:{8}">{7}</button>\
					            </div>\
				            </li>';

                var liStr = stringFormat(html
                    , imgUrl(value, 'user', 'user_thumb', '')
                    , user_name
                    , nick_name
                    , supply_desc
                    , imgUrl(value, 'supply', 'supply', '')
                    , user_idx
                    , (follow_yn == 'Y')? 'btn_cwb btn_uflw': 'btn_cb btn_flw'
                    , (follow_yn == 'Y')? '언팔로우': '팔로우'
                    , (user_idx == g_user_idx)? 'none':'block'
                    );

                $("#mlist").append(liStr);

                //https://stackoverflow.com/questions/31049541/how-to-loop-through-li-in-ul-jquery
            });

        }

        function doMoreMyPost(page, isScroll) {

            if (is_get_data)
                return;

            is_get_data = true;

            url = url_path + "/Event/GetLists";

            var spinner = new Spinner(g_spin_common).spin();

            if (page > 1) 
            {
                var spinnerPoint = spinnerHeight($(window).scrollTop(), $(window).width(), $(window).height());
                $(spinner.el).css(spinnerPoint);
                $('body').append(spinner.el);
                var str = '<div class="loading_bar" id="loadingBar"></div>'
                
                $("#loadingDiv").append(str);
            }

            $.ajax({
                type: "POST",
                url: url,
                //data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}", page, 20, $('#cbxCategory option:selected').val(), g_user_idx),
                //data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}&me_user_idx={4}", page, 8, $('#cbxCategory option:selected').val(), 0, g_user_idx),
                data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}&me_user_idx={4}&private_view_yn=N", page, 8, 0, 0, g_user_idx),
                dataType: "json",
                cache: false,
                success: function (response) {
                    if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                    page_count = response.response_option.page_count;

                    if (isScroll)
                        pageLoaded = page;

                    if(page == 1)
                    {
                        $("#mlist").empty();
                    }

                    doGetMyPost(response);

                },
                error: ajaxError,
                complete: function () {

                    setTimeout(function () {
                        
                        is_get_data = false;

                    }, 1000);

                    spinner.stop();
                    $('#loadingBar').remove();
                }
            });

        }











    </script>
</head>
<body class="mbd">
<%
    int user_idx = PageUtility.UserIdx().ToInt();

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
			<h1 class="logo"><a href="#"><img src="/images/logo_txt.png" alt="storichain" /></a></h1>
			<button type="button" class="btn btn_memu"><span class="bar bar_eq0"></span><span class="bar bar_eq1"></span><span class="bar bar_eq2"></span></button>
            <button type="button" class="btn btn_info"><span class="blind">information</span></button>
		</div>
	</div>
	<div class="hnav_hd">
		<div class="nav_wrap">
			<ul class="hnav">
				<li class="m1 selected"><a href="discover"><strong>Discover</strong></a></li>
				<li class="m2"><a href="feed"><strong>Feed</strong></a></li>
				<li class="m3"><a href="desk"><strong>Desk</strong></a></li>
				<li class="m4"><a href="cowork"><strong>Cowork</strong></a></li>
			</ul>
		</div>
	</div>
</div>

<div class="aside">
    <div class="aside_h">
        <h1 class="logo"><img src="/images/logo_txt.png" alt="storichain" /></h1>
        <button type="button" class="btn btn_close_aside"><span class="blind">메뉴 닫기</span></button>
    </div>
    <!--

    <div class="logout_ct">
        <p class="sc_tit">Story Asset Trading<br /> for Creators</p>
        <ul class="login_btn_area">
           <li><a href="#" class="btn_r btn_m btn_cw" id="btn_login">Log In</a></li>
           <li><a href="#" class="btn_r btn_m btn_cb" id="btn_signin">Sign Up</a></li>
        </ul>
        <div class="desc">
            <p>Now, you can be a storyteller sell your stories, co-write screenplay, cartoon stories.</p>
            <p>If you'd like to buy and sell story asset, change user role to StoryProducer. Then you can trade story asset, fund raise with the stories you supported.</p>
            <p>If you are just a reader, you can read stories and click reaction.Now, you can be a storyteller
            sell your stories, co-write screenplay, cartoon stories.</p>
        </div>
    </div>

-->


    <div class="login_ct">
    <div class="user_info">
        <div class="user_pf">
            <div class="user_img"><img src="<%= drUser.ImageUrl("user", "user_thumb", "/images/img_user_my.png") %>" alt=""></div>
            <div class="user_id">
                <div class="user_email"><%= drUser.ItemValue("site_user_id") %></div>
                <div class="user_wid">@<%= drUser.ItemValue("nick_name") %></div>
            </div>
        </div>
        <div class="user_follow">
            <span class="ufl"><strong><%= drUser.ItemValue("following_count") %></strong> following</span> <span class="ufr"><strong><%= drUser.ItemValue("follower_count") %></strong> follower</span>
        </div>
        <div class="user_pf_lk">
            <a href="#" class="pf_lk">Profile</a>
        </div>
    </div>
    <ul class="mn mn1">
        <li class=""><a href="#">Open Stories</a></li>
        <li class=""><a href="#">Pro Stories</a></li>
        <li class=""><a href="#">Contesting Stories</a></li>
        <li class=""><a href="#">Trading Stories</a></li>
        <li class=""><a href="#">Funding Stories</a></li>
    </ul>
    <ul class="mn mn2">
        <li class="mn21"><a href="#">팔로우 및 작가 추천</a></li>
        <li class="mn22"><a href="#">광고 관리</a></li>
        <li class="mn23"><a href="#">지갑</a></li>
        <li class="mn24"><a href="#">지인에게 추천 리워드 받기</a></li>
        <li class="mn25"><a href="profile?user_idx=<%= WebUtility.UserIdx("profile_user_idx").ToValue() %>">설정 및 계정 정보</a></li>
        <li class="mn26"><a href="customer_question">문의 하기</a></li>
        <li class="mn27"><a href="logout">로그아웃</a></li>
    </ul>
    </div>
    <uc1:footer_menu runat="server" id="footer_menu" />
</div>

<div class="container ">
    <form action="">
        <fieldset>
            <div class="search_area">
                <div class="ct_wrap">
                    <div class="sch_input">
                        <input type="search" class="inp_txt" name="" placeholder="" title="search">
                        <button type="button" class="btn btn_search"><span class="blind">search</span></button>
                    </div>
                </div>
            </div>
        </fieldset>
    </form>

    <%--
	<div class="mtit">
		<div class="ct_wrap">
			<h2>MENU</h2>
		</div>
	</div>--%>
	<div class="category">
		<div class="ct_wrap">
			<ul class="cat_lst" id='ulStory'>
				<li data-value='0'><a href="#">NEW</a></li>
				<li data-value='1'><a href="#">Open Stories</a></li>
				<li data-value='2'><a href="#" class="selected">Pro Stories</a></li>
				<li data-value='3'><a href="#">Contesting Stories</a></li>
				<li data-value='4'><a href="#">Trading Stories</a></li>
				<li data-value='5'><a href="#">Funding Stories</a></li>
			</ul>
		</div>
	</div>

    

	<div class="content">
		<div class="ct_wrap">
			<ul class="dscv_lst" id="mlist">

                <%
                    Biz_Event biz_event = new Biz_Event();
                        
                    int total_count;
                    int page_count;

                    DataTable dtEvent = biz_event.GetLists( 0,
                                                            0,
                                                            WebUtility.GetRequestByInt("channel_idx", 0),
                                                            WebUtility.GetRequestByInt("game_idx", 0),
                                                            WebUtility.GetRequestByInt("topic_idx", 0),
                                                            WebUtility.GetRequestByInt("event_theme_type_idx", 0),
                                                            WebUtility.GetRequestByInt("data_type_idx", 0),
                                                            "Y",
                                                            WebUtility.GetRequest("on_air_yn", ""),
                                                            WebUtility.GetRequest("temp_yn", ""),
                                                            "Y",
                                                            WebUtility.GetRequest("sort_type_name", "date"),
                                                            BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                            WebUtility.GetRequest("video_yn", ""),
                                                            WebUtility.GetRequest("private_view_yn", "N"),
                                                            WebUtility.GetRequestByInt("page", 1),
                                                            WebUtility.GetRequestByInt("page_rows", 8),
                                                            out total_count,
                                                            out page_count,
                                                            WebUtility.UserIdx());

                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("total_row_count", total_count);
                    dic.Add("page_count", page_count);

                    string myPost_json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtEvent, dic);

                    WebUtility.Write(JSHelper.GetScript("var myPost_json = " + myPost_json + ";"));
                %>


				<%--<li>
					<div class="user_thum">
                        <a href="#"><img src="/images/del/u2.png" alt="" class="user_thum_img" /></a>
					</div>
					<div class="user_intro">
						<img src="/images/del/u.png" alt="" class="user_pf_img" />
						<div class="wname">
							<h3><a href="#">작가필명</a></h3>
							<h4>(작가아이)</h4>
						</div>
						<p class="wdesc">The Rockies made some roster adjustments on Tuesday, reinstating DJ LeMahieu and optioning Pat Valaika.</p>
						<ul class="stats_lst">
							<li class="grph"><span>1000</span></li>
							<li class="cutin"><span>372</span></li>
							<li class="star"><span>2.5</span></li>
						</ul>
						<button type="button" class="btn_r btn_s btn_cb btn_flw">팔로우</button>
					</div>
				</li>--%>
                
			</ul>
            <div class="fbtn">
                <button type="button" class="btn btn_more">load more</button>
            </div>
		</div>
	</div>

    <div class="fx_btn ct_lk">
        <ul class="drt_btn">
            <li><a href="#" class="btn_top"><span>top</span></a></li>
            <li><a href="javascript:history.back();" class="btn_back"><span>back</span></a></li>
            <li><a href="#" class="btn_down"><span>down</span></a></li>
        </ul>
        <ul class="rd_btn">
            <li><a href="#" class="btn_lk_lbl"><span class="blind">Label</span></a></li>
        </ul>
    </div>
</div>

<!-- label layer 추가해야함 -->
<div class="label_layer">
</div>

<%
    if(drUser["email_auth_yn"].ToValue().Equals("N"))
    {
        string alert = @"<!-- alert -->
                        <div class='alert'>
                            <div class='alert_wrap'>
                                <p>Please confirm you email to certify</p>
                                <p class='fcp'>If you don't confirm within 7days, your ID will be deleted</p>
                                <button type='button' class='btn btn_alert_close'><span class='blind'>닫기</span></button>
                            </div>
                        </div>";
        WebUtility.Write(alert);
    }

%>

    <div id="loadingDiv"></div>


<uc1:footer runat="server" id="footer" />
<!-- 팝업 배경 화면 -->
<div class="black_scrn"></div>
<script type="text/javascript">
	$(function() {
		$('.btn_alert_close').on('click',function(){
		   $(this).parents('.alert').hide();
		});
	});
</script>
</body>
</html>
