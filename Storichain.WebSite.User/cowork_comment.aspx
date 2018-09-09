<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cowork_comment.aspx.cs" Inherits="Storichain.WebSite.User.cowork_comment" %>
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
        var g_user_idx  = <%=WebUtility.UserIdx()%>;
        var g_event_idx = <%=PageUtility.Idx("event_idx")%>;
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

            $(document).on("click", ".btn.btn_cmt_del", function () {
				
                var comment_idx = $(this).attr('idx');
                var $item = $(this);
          
                var formData = new FormData();
                formData.append('comment_idx', comment_idx);
                
                $.ajax({
                    type: "Post",
                    url: "/Comment/Remove",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            $item.closest("li").remove();
                        }
                        else 
                        {
                            alert("Please enter the correct data.");
                            return;
                        }
                    }
                });

            });

            $('.btn.btn_more').click(function (e) {
                e.preventDefault();
                doMoreMyPost(pageLoaded + 1, true);
                $(this).hide();
                scroll_loading_yn = true;
            });

            $('.btn.btn_s.btn_cb.btn_cmt_sbm').click(function (e) {
                e.preventDefault();
                
                var cmt = $('#cmt').val();
                
                if(cmt == '') {
                    alert('Please enter the comment text.');
                    return;
                }

                var formData = new FormData();
                formData.append('user_idx', g_user_idx);
                formData.append('event_idx', g_event_idx);
                formData.append('comment_text', cmt);
                if ($("#img").val() != "")
                    formData.append('upload', $("#img")[0].files[0]);
                
                $.ajax({
                    type: "Post",
                    url: "/Comment/Add",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            //alert("It was saved");
                            doMoreMyPost(1, true);
                            $('#cmt').val('');
                            $("#userImg").hide();
                            $('#userImg').attr('src', '');
                            $("#img").val('');
                            $(".btn.btn_img_up").css('z-index', 3);
                        }
                        else 
                        {
                            alert("Please enter the correct data.");
                            return;
                        }
                    }
                });

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

            $('.btn.btn_img_up').click(function (e) {
                e.preventDefault();

                $("#img").click();
            });

            $('.btn.btn_img_del').click(function (e) {
                e.preventDefault();

                $("#userImg").hide();
                $('#userImg').attr('src', '');
                $("#img").val('');
                $(".btn.btn_img_up").css('z-index', 3);
            });
            
            
        });

        var pageLoaded = 1;
        var page_count = 0;
        var is_get_data = false
        var scroll_loading_yn = false;

        function doGetMyPost(myPostData) {

            page_count = myPostData.response_option.page_count;
            console.log(pageLoaded + '/' + page_count);

            if(pageLoaded == 1 && page_count > 1)
                $(".btn.btn_more").show();

            $.each(myPostData.response_data, function (i, value) {

                var event_idx            = value.event_idx;
                var comment_idx          = value.comment_idx;
                var user_idx             = value.user_idx;
                var user_name            = value.user_name;
                var nick_name            = value.nick_name;
                var comment_text         = value.comment_text.replace(/\n/g, "<br />");
                var create_date          = value.create_date;

                var html = '<li>\
					            <div class="cmt_us_if">\
						            <span class="us_timg"><a href="#"><img src="{0}" alt="" /></a></span>\
						            <a href="#" class="us_name">@{1}</a>\
					            </div>\
					            <span class="cmt_time">{2}</span>\
					            <div class="cmt_ct">\
						            <p>{3}</p>\
						            <div class="cmt_ct_img"><img src="{4}" alt="" /></div>\
					            </div>\
					            <button type="button" class="btn btn_cmt_del" idx="{5}"><span class="blind">코멘트 삭제</span></button>\
				            </li>';

                var liStr = stringFormat(html
                    , imgUrl(value, 'user', 'user_thumb', '')
                    , nick_name
                    , create_date
                    , comment_text
                    , imgUrl(value, 'comment', 'comment', '')
                    , comment_idx
                    );

                $("#mlist").append(liStr);

            });

        }

        function doMoreMyPost(page, isScroll) {

            if (is_get_data)
                return;

            is_get_data = true;

            url = url_path + "/Comment/GetLists";

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
                data: stringFormat("page={0}&page_rows={1}&event_idx={2}", page, 4, g_event_idx),
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

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {

                    $('#userImg').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
                $("#userImg").show();
                $(".btn.btn_img_up").css('z-index', 1);
            }
        }

    </script>
</head>
<body class="sbd">
<%
    int user_idx    = PageUtility.UserIdx().ToInt();
    int event_idx   = PageUtility.Idx("event_idx").ToInt();

    if(user_idx == 0)  { Response.Redirect("login"); return; }
    if(event_idx == 0) { Response.Redirect("discover"); return; }
    
%>

<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">A joint review</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">

			<form action="">
				<fieldset>
					<div class="comment_inp">
						<div class="cmt_inp_tbox"><textarea name="" id="cmt" class="ftxt" cols="30" rows="10" title="코멘트 입력"></textarea><label for="cmt" class="lblt">코멘트 입력</label></div>
						<ul class="cmt_img">
							<li><button type="button" class="btn btn_img_up" style='z-index:3'><span class="blind">이미지 등록</span></button><span class="cimg"><img id="userImg" alt="" style="display:none" /></span><button type="button" class="btn btn_img_del"><span class="blind">이미지 삭제</span></button></li>
                            <input type="file" id="img" name="files" value="" style="display:none;" onchange="readURL(this);" />

							<%--<li><button type="button" class="btn btn_img_up"><span class="blind">이미지 등록</span></button></li>--%>
						</ul>
						<button type="button" class="btn btn_s btn_cb btn_cmt_sbm">게시</button>
					</div>
				</fieldset>
			</form>

			<ul class="cw_cmt_lst" id='mlist'>

                <%
                    int total_count;
                    int page_count;

                    Biz_Comment biz_comment = new Biz_Comment();
                    DataTable dt_commnet = biz_comment.GetCommentLists( event_idx,
                                                                        WebUtility.GetRequest("sort_asc_yn", "Y"),
                                                                        "",
                                                                        "",
                                                                        BizUtility.GetStdDate(WebUtility.GetRequest("std_date")),
                                                                        WebUtility.GetRequestByInt("page", 1), 
                                                                        WebUtility.GetRequestByInt("page_rows", 4),
                                                                        out total_count,
                                                                        out page_count);

                    Dictionary<string,object> dic = new Dictionary<string,object>();
                    dic.Add("total_row_count", total_count);
                    dic.Add("page_count", page_count);

                    string myPost_json = DataTypeUtility.JSon("1000", Config.R_SUCCESS, "", dt_commnet, dic);

                    WebUtility.Write(JSHelper.GetScript("var myPost_json = " + myPost_json + ";"));
                    
                %>

				<%--<li>
					<div class="cmt_us_if">
						<span class="us_timg"><a href="#"><img src="/images/del/u2.png" alt="" /></a></span>
						<a href="#" class="us_name">@leejunsu</a>
						<em class="us_cc">joint</em>
					</div>
					<span class="cmt_time">2018-01-01 22:34:21</span>
					<div class="cmt_ct">
						<p>샘플텍스트 입니다.</p>
						<p>샘플텍스트 입니다.</p>
						<p>샘플텍스트 입니다.</p>
					</div>
					<button type="button" class="btn btn_cmt_del"><span class="blind">코멘트 삭제</span></button>
				</li>
				<li>
					<div class="cmt_us_if">
						<span class="us_timg"><a href="#"><img src="/images/del/u2.png" alt="" /></a></span>
						<a href="#" class="us_name">@leejunsu</a>
					</div>
					<span class="cmt_time">2018-01-01 22:34:21</span>
					<div class="cmt_ct">
						<p>샘플텍스트 입니다.</p>
						<div class="cmt_ct_img"><img src="/images/del/u2.png" alt="" /></div>
					</div>
					<button type="button" class="btn btn_cmt_del"><span class="blind">코멘트 삭제</span></button>
				</li>
				<li>
					<div class="cmt_us_if">
						<span class="us_timg"><a href="#"><img src="/images/del/u2.png" alt="" /></a></span>
						<a href="#" class="us_name">@leejunsu</a>
					</div>
					<span class="cmt_time">2018-01-01 22:34:21</span>
					<div class="cmt_ct">
						<p>샘플텍스트 입니다.</p>
					</div>
					<button type="button" class="btn btn_cmt_del"><span class="blind">코멘트 삭제</span></button>
				</li>--%>
			</ul>

			<div class="fbtn">
				<button type="button" class="btn btn_more" style="display:none">더보기</button>
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

</body>
</html>
