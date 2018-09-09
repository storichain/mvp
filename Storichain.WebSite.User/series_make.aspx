<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="series_make.aspx.cs" Inherits="Storichain.WebSite.User.series_make" %>
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
        var g_event_idx = 0;
        $(function() {
            $("#ulOpen a").on("click", function(e){
                e.preventDefault();

                $("#ulOpen a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            });

            $(".btn_r.btn_l.btn_cp").on("click", function(e){
                e.preventDefault();

                if ($("#sr_tit").val() == "") {
                    alert("Please enter series name.");
                    return false;
                } else if ($("#img").val() == "") {
                    alert("Please choose a series image.");
                    return false;
                }

                if(!confirm('Do you want to save your data?'))
                    return false;

                var formData = new FormData();
                formData.append('supply_name', $("#sr_tit").val());
                
                if ($("#img").val() != "")
                    formData.append('upload', $("#img")[0].files[0]);

                var supply_name             = $("#sr_tit").val();
                var event_content_type_idx  = 2;
                var private_view_yn         = 'N';
                var collabor_yn             = ($('#cw_of').prop('checked'))?'Y':'N';
                var collabor_author_count   = $('#cw_count').val();

                if(g_event_idx == 0) 
                {
                    url = url_path + "/Event/Add";
                    data = stringFormat("user_idx={0}&supply_name={1}&event_content_type_idx={2}&private_view_yn={3}&collabor_yn={4}&collabor_author_count={5}", g_user_idx, supply_name, event_content_type_idx, private_view_yn, collabor_yn, collabor_author_count);
                }
                else 
                {
                    url = url_path + "/Event/Modify";
                    data = stringFormat("event_idx={0}&user_idx={1}&supply_name={2}&event_content_type_idx={3}&private_view_yn={4}&collabor_yn={5}&collabor_author_count={6}", g_event_idx, g_user_idx, supply_name, event_content_type_idx, private_view_yn, collabor_yn, collabor_author_count);
                }

                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                        if(response.response_data_count > 0) 
                        {
                            if(g_event_idx == 0) 
                            {
                                g_event_idx = response.response_data[0].event_idx;

                                $('.btn_r.btn_l.btn_cp').hide();
                                $('.btn_r.btn_l.btn_cwb').show();
                            }
                            else 
                            {
                                
                            }
                        }
                    },
                    error: ajaxError,
                    complete: function () {
                            
                    }
                });



                
            });

            $(".btn_r.btn_l.btn_cwb").on("click", function(e){
                e.preventDefault();

                location.href = 'series_setting?event_idx=' + g_event_idx;
            });

            $(".btn.btn_img_up").on("click", function(e){
                e.preventDefault();

                $("#img").click();
            });

            $(".btn.btn_img_del").on("click", function(e){
                e.preventDefault();

                $("#userImg").hide();
                $('#userImg').attr('src', '');
                $("#img").val('');
            });
        });

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#userImg').attr('src', e.target.result);
                }
                reader.readAsDataURL(input.files[0]);
                $("#userImg").show();
            }
        }

    </script>
</head>
<body class="sbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">시리즈 만들기</a></h1>
            <a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<dl class="sr_df">
				<dt>시리즈란?</dt>
				<dd>여러 개의 톡스토리를 에피소드로 묶어 놓는 것을 뜻합니다. 이슈 주제별 묶음, 소셜챕터별 묶음, 대본 시퀸스별 묶음 등 원하는 용도로 사용하세요.</dd>
			</dl>
			<form action="">
				<fieldset>
					<div class="form_wrap">
						<div class="form_box">
							<h3 class="form_tit">시리즈 타이틀</h3>
							<div class="inp_box"><input type="text" name="" id="sr_tit" class="inpt" placeholder="ex) 방자는 18세 웹소설" /><label for="sr_tit" class="lblt">시리즈 타이틀</label></div>
							<div class="inp_img_box">
								<button type="button" class="btn btn_img_up"><span class="blind">이미지 등록</span></button>
								<span class="cc_txt">최소 400px X 300px<br />비율 4:3(비필수)</span>
								<img id="userImg" src="/images/del/u2.png" alt="" class="sr_img" style="display:none" />
								<button type="button" class="btn btn_img_del"><span class="blind">이미지 삭제</span></button>
                                <input type="file" id="img" name="files" value="" style="display:none;" onchange="readURL(this);" />
							</div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">공동 창작</h3>
							<p class="form_desc">피드에 공개한 이후 팔로워와 함께 에피소드를 추가 할 수 있습니다.</p>
							<div class="onoff prt"><input type="checkbox" id="cw_of" class="ofchk"><label for="cw_of" class="oflbl"><span></span></label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">공동 창작</h3>
							<p class="form_desc">협업 작가들의 창작에 기여하는 기여도에 따라 분배율이 확정 됩니다. 스토리에 수익이 생길 시 분배율에 따라 배분 됩니다. 작가군 보상풀에서 원작자는 30% 이상은 보상 받지 않습니다.</p>
							<div class="prt_sel">
								<select id="cw_count" name="cw_count" title="cw_count" class="sel_s">
									<option value="1" selected="">1</option>
									<option value="2">2</option>
									<option value="3">3</option>
								</select>
							</div>
						</div>
						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cp">완료</button></span></li>
							<li><span><a href="#" class="btn_r btn_l btn_cwb" style='display:none;'>상세설정</button></a></li>
						</ul>
					</div>
				</fieldset>
			</form>

		</div>
	</div>

</div> 

<uc1:footer runat="server" id="footer" />

</body>
</html>
