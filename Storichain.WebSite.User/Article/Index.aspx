<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Storichain.WebSite.User.Index" %>
<%@ Import Namespace="Storichain" %>
<%@ Import Namespace="Storichain.Models.Biz" %>
<%@ Import Namespace="Storichain.Models" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,minimum-scale=1.0,maximum-scale=1.0,user-scalable=no">
    <link rel="apple-touch-icon-precomposed"  href="" />
    <link rel="apple-touch-icon-precomposed" sizes="114x114" href="" />
    <title>Storichain</title>
    <link rel="shortcut icon" type="image/x-icon" href="Images/favicon.ico?v=1" />
    <link rel="stylesheet" type="text/css" href="Content/beautytalk.css?v=1">
    <script src="Scripts/lib/jquery.js"></script>
    <script src="Scripts/common.js"></script>
    <script src="Scripts/user/base.js"></script>
    <script src="Scripts/user/user.js"></script>
    <script>

        var g_user_idx = '<%= PageUtility.UserIdx() %>';

        $(function() {

            if(g_user_idx == 0)
            {
                location.href = "login";
                return;
            }
        
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

            $('#cbxCategory').change(function (e) {
                e.preventDefault();

                doMoreMyPost(1, false);
            });
        });

        var pageLoaded = 1;
        var page_count = 0;
        var is_get_data = false;

        function doGetMyPost(myPostData) {

            page_count = myPostData.response_option.page_count;

            $.each(myPostData.response_data, function (i, value) {

                var topic_idx = value.topic_idx;
                var event_idx = value.event_idx;
                var user_idx = value.user_idx;
                var supply_name = value.supply_name;
                var create_date = value.create_date;
                var topic_name = value.topic_name;
                var orientation_type_idx = value.orientation_type_idx;

                var liStr = stringFormat(
                    '<li>\
				        <a href="{3}">\
					        <span class="li_img"><img src="{0}" alt="" /></span>\
					        <strong class="li_tit">{1}</strong>\
					        <em class="li_date">{2}</em>\
				        </a>\
			        </li>'
                    , imgUrl(value, 'supply', 'supply_thumb')
                    , supply_name + ((orientation_type_idx == 3) ? '<font color="red">(아웃링크 기사)</font>':'')
                    , topic_name
                    , (orientation_type_idx == 3) ? '#null':'Edit?event_idx=' + event_idx
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
                data: stringFormat("page={0}&page_rows={1}&topic_idx={2}&publish_yn=Y&user_idx={3}", page, 20, $('#cbxCategory option:selected').val(), g_user_idx),
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
    <!-- Google Analytics -->
    <script>
    (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
    (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
    m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
    })(window,document,'script','https://www.google-analytics.com/analytics.js','ga');
 
    ga('create', 'UA-43949475-2', 'auto');
    ga('send', 'pageview');
    </script>
    <!-- End Google Analytics -->
</head>
<body>

    <form id="form1" runat="server">
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

        <div class="header">
	        <h1>My story</h1>
	        <a href="#asdie" class="btn_ham"><span class="blind">확장영역 열기</span></a>
	        <span class="user"><img src="<%= drUser.ImageUrl("user", "user_thumb", "/images/img_user_my.png") %>" alt="user" /></span>
        </div>

        <div class="wrap">
	        <!-- 사이드 메뉴 -->
	        <div class="asdie">
		        <aside id="aside">
			        <div class="user_info">
				        <div>
                            <img src="<%= drUser.ImageUrl("user", "user_thumb", "/images/img_user_my.png") %>" alt="<%= drUser.ItemValue("nick_name") %>" />
					        <strong><%= drUser.ItemValue("nick_name") %></strong>
					        <span><%= drUser.ItemValue("site_user_id") %></span>
				        </div>
			        </div>
			        <ul class="nav">
                        <li><a href="../discover">홈으로 이동</a></li>
				        <li><a href="index">나의 기사</a></li>
                        <li><a href="ing">작성 중 기사</a></li>
				        <li><a href="edit">기사 작성</a></li>
				        <li><a href="logout">로그아웃</a></li>
			        </ul>
		        </aside>
	        </div>
	        <div class="layer_mask"></div>
	        <!-- 사이드 메뉴 -->

	        <div id="ct" class="ct">

		        <div class="select_area">
			        <select id="cbxCategory" class="sel">
                        <% 
                            Biz_Code biz_code = new Biz_Code();
                            DataTable dt = biz_code.GetCodeList(10,"Y",0);

                            foreach(DataRow dr in dt.Rows)
                            {
                                WebUtility.Write(string.Format("<option value='{0}'>{1}</option>", dr.ItemValue("code_value"), dr.ItemValue("code_name")));
                            }
                        %>
			        </select>
		        </div>

		        <ul class="mlist" id="mlist">

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
                                                                WebUtility.GetRequest("private_view_yn", ""),
                                                                WebUtility.GetRequestByInt("page", 1),
                                                                WebUtility.GetRequestByInt("page_rows", 20),
                                                                out total_count,
                                                                out page_count,
                                                                WebUtility.GetRequestByInt("me_user_idx", WebUtility.UserIdx()));

                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("total_row_count", total_count);
                        dic.Add("page_count", page_count);

                        string myPost_json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dtEvent, dic);

                        WebUtility.Write(JSHelper.GetScript("var myPost_json = " + myPost_json + ";"));

                        //foreach(DataRow drEvent in dtEvent.Rows)
                        //{
                        //    string htmlEvent = @"<li>
                        //                <a href='#'>
                        //                 <span class='li_img'><img src='{0}' alt='' /></span>
                        //                 <strong class='li_tit'>{1}</strong>
                        //                 <em class='li_date'>{2}</em>
                        //                </a>
                        //               </li>";

                        //    WebUtility.Write(string.Format(htmlEvent,
                        //                                    drEvent.ImageUrl("supply", "supply_thumb"),
                        //                                    drEvent.ItemValue("supply_name"),
                        //                                    drEvent.ItemValue("create_date").ToDateTime().ToString("yyyy.MM.dd") ));
                        //}

                    %>

		        </ul>
		        <!-- // main_list -->
	        </div>
	        <!-- // ct -->
        </div>
        <div id="loadingDiv"></div>

    </form>

<script>
</script>


</body>
</html>
