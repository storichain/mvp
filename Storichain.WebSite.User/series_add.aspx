<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="series_add.aspx.cs" Inherits="Storichain.WebSite.User.series_add" %>
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

            $("#ulOption a").on("click", function(e){
                e.preventDefault();

                $("#ulOption a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })

           
            

            $('.btn.btn_search').click(function (e) {
                e.preventDefault();

                alert('search');
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
						        <input type="checkbox" id="series_list{3}" name="" class="ckbx" value="">\
						        <label for="series_list{3}" class="sl_lb">\
							        <div class="sl_img"><img src="{0}" alt="" /></div>\
							        <div class="sl_txt">\
								        <div class="sl_txt_box">\
									        <h3>{1}</h3>\
									        <p>{2}</p>\
								        </div>\
							        </div>\
						        </label>\
					        </li>';

                var liStr = stringFormat(html
                    , imgUrl(value, 'supply', 'supply', '')
                    , supply_name
                    , supply_desc
                    , i
                    );

                $("#story_list").append(liStr);

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
    int user_idx        = WebUtility.UserIdx();
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

<div class="container ">
    <form action="">
        <fieldset>
            <div class="search_area">
                <div class="ct_wrap">
                    <div class="sch_input">
                        <input type="search" class="inp_txt" name="" placeholder="스토리 검색" title="search">
                        <button type="button" class="btn btn_search"><span class="blind">search</span></button>
                    </div>
                </div>
            </div>
        </fieldset>
    </form>

	<ul class="tab" id='ulOption'>
		<li data-value='desc'><a href="#" class="selected">Latest</a></li>
		<li data-value='asc'><a href="#">Oldest</a></li>
	</ul>

	<div class="content">
		<div class="ct_wrap">
			<form action="">
				<ul class="series_list" id="story_list">

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
						<input type="checkbox" id="series_list01" name="" class="ckbx" value="">
						<label for="series_list01" class="sl_lb">
							<div class="sl_img"><img src="/images/del/u2.png" alt="" /></div>
							<div class="sl_txt">
								<div class="sl_txt_box">
									<h3>샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.</h3>
									<p>샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.</p>
								</div>
							</div>
						</label>
					</li>--%>
					<%--<li>
						<input type="checkbox" id="series_list02" name="" class="ckbx" value="">
						<label for="series_list02" class="sl_lb">
							<div class="sl_img"><img src="/images/del/u2.png" alt="" /></div>
							<div class="sl_txt">
								<div class="sl_txt_box">
									<h3>샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.</h3>
									<p>샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.</p>
								</div>
							</div>
						</label>
					</li>
					<li>
						<input type="checkbox" id="series_list03" name="" class="ckbx" value="">
						<label for="series_list03" class="sl_lb">
							<div class="sl_img"><img src="/images/del/u2.png" alt="" /></div>
							<div class="sl_txt">
								<div class="sl_txt_box">
									<h3>샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.샘플 타이틀 텍스트입니다.</h3>
									<p>샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.샘플 텍스트 입니다.</p>
								</div>
							</div>
						</label>
					</li>--%>
				</ul>
				<div class="series_confirm">
					<button type="button" class="btn btn_r btn_l btn_cp">선택완료</button>
				</div>
			</form>
		</div>
	</div>

</div>

<div class="black_scrn"></div>

</body>
</html>
