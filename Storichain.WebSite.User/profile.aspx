<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="profile.aspx.cs" Inherits="Storichain.WebSite.User.profile" %>
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

            doGetMyPost(myPost_json);

            $(window).scroll(function () {

                var buffer = 100;
                var storyList_view = $(window);

                if ($("html").prop('scrollHeight') - storyList_view.scrollTop() <= storyList_view.height() + buffer) 
                {
                    if (pageLoaded > 0) 
                    {
                        if (page_count > pageLoaded) 
                        {
                            doMoreMyPost(pageLoaded + 1, true);
                        }
                    }
                }
            });


            $('.btn_r.btn_cb.btn_go_pfm').click(function (e) {
                e.preventDefault();
                location.href = 'profile_form';
            });

            $('.btn.btn_s.btn_cb').click(function (e) {
                e.preventDefault();
                console.log('Write together');
                location.href = 'article/edit';
            });
            
            $('.btn.btn_s.btn_cwb').click(function (e) {
                e.preventDefault();
                console.log('Write like this');
                location.href = 'article/edit';
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
                //data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}&private_view_yn=N", page, 4, $('#cbxCategory option:selected').val(), g_user_idx),
                data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}&private_view_yn=N", page, 4, 0, g_user_idx),
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
	<div class="profile_info">
		<div class="profile_wrap">
			<div class="profile_ct">
				<div class="pf_img_r"><img src="<%= drUser.ImageUrl("user", "user_thumb", "/images/img_user_my.png") %>" alt="" /></div>
				<div class="pf_uname">
					<strong>@<%= drUser.ItemValue("nick_name") %></strong>
                    <%   string view_profile = "block";
                        if (WebUtility.GetRequestByInt("user_idx", WebUtility.UserIdx()) != WebUtility.UserIdx()) 
                            view_profile = "none";%>
                    <a href="#null" class="btn_r btn_cb btn_go_pfm" style="display:<%= view_profile %>">Edit Profile</a>
				</div>
				<p class="pf_desc"><%= drUser.ItemValue("introduce").Replace(System.Environment.NewLine, "<br>") %> </p>
				<div class="follow_num">
					<span class="fwi"><strong><%= drUser.ItemValue("following_count") %></strong> following</span>
					<span class="fwe"><strong><%= drUser.ItemValue("follower_count") %></strong> follower</span>
				</div>
				<div class="pf_tag"><b>Interest story field :</b>
                    <%
                        string interest_field = drUser.ItemValue("interest_field");

                        string[] ifs = interest_field.Split(',');
                        string temp = @"";

                        foreach(string ifstring in ifs)
                            temp += string.Format("<a href='#'>#{0}</a> ", ifstring);
                        
                        Response.Write(temp);
                    %>
				</div>
			</div>
			<ul class="pf_sns">
				<li><span>Instagram followers</span><strong>2321</strong></li>
				<li><span>Naver blog visitors</span><strong>2321</strong></li>
				<li><span>facebook friends</span><strong>2321</strong></li>
			</ul>
			<ul class="pf_write_btn">
				<li><span><a href="#" class="btn btn_s btn_cb">Write together</a></span></li>
				<li><span><a href="#" class="btn btn_s btn_cwb">Write like this</a></span></li>
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
                                                            user_idx,
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
                                                            WebUtility.GetRequestByInt("page_rows", 4),
                                                            out total_count,
                                                            out page_count,
                                                            WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

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

		</div>
	</div>

</div>
<uc1:footer runat="server" id="footer" />
<script type="text/javascript">
$(function() {
	/* profile height에 따른 padding 적용 상단 프로필 부분이 뿌려진 후에 계산되도록
	var hd_h = $('.header').height();
	var pf_h = $('.profile_info').height();
	$('.container').css('padding-top', hd_h + pf_h + 'px');
	*/
});
</script>

</body>
</html>
