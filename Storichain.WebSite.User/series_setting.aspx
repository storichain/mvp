<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="series_setting.aspx.cs" Inherits="Storichain.WebSite.User.series_setting" %>
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

            if(g_event_idx == 0) 
            {
                //location.href = 'desk';
                //return;
            }

            $("#ulOpen a").on("click", function(e){
                e.preventDefault();

                $("#ulOpen a").removeClass("selected");
                $(this).closest('a').addClass('selected');
                var $this = $(this).parent();

                console.log($this.data("value"));                
            })


            $(".btn_r.btn_l.btn_cp").on("click", function(e){
                e.preventDefault();

                location.href = 'desk';
            });

            $('.btn_r.btn_l.btn_cp').click(function (e) { 
                e.preventDefault();

                alert('fdsfasdf');
                return false;

                var collabor_with_follower_yn   = ($('#sr_set1').prop('checked'))?'Y':'N';
                var interfere_talk_yn           = ($('#sr_set2').prop('checked'))?'Y':'N';
                var section_scene_yn            = ($('#sr_set3').prop('checked'))?'Y':'N';
                var fork_yn                     = ($('#sr_set4').prop('checked'))?'Y':'N';

                var url = url_path + "/Event/ModifySeriesInfo";
                var data = stringFormat("event_idx={0}&collabor_with_follower_yn={1}&interfere_talk_yn={2}&section_scene_yn={3}&fork_yn={4}", event_idx, collabor_with_follower_yn, interfere_talk_yn, section_scene_yn, fork_yn);

                $.ajax({
                    type: "POST",
                    url: url,
                    data: data,
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        //if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                        if(response.response_data_count > 0) 
                        {
                            //alert('success');
                            //location.href = 'desk';
                            //return;
                        }
                    },
                    error: ajaxError,
                    complete: function () {
                            
                    }
                });

    
            });


        });
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

			<form action="">
				<fieldset>
					<div class="form_wrap">
						<div class="form_box">
							<h3 class="form_tit">팔로워와 함께 쓰기</h3>
							<p class="form_desc">여피드에 공개한 이후 팔로워화 함께 에피소드를 추가 할 수 있습니다.</p>
							<div class="onoff prt"><input type="checkbox" id="sr_set1" class="ofchk"><label for="sr_set1" class="oflbl"><span></span></label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">껴든 톡 허용</h3>
							<p class="form_desc">독자들이 스토리에서 껴든 톡 작성 가능</p>
							<div class="onoff prt"><input type="checkbox" id="sr_set2" class="ofchk"><label for="sr_set2" class="oflbl"><span></span></label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">갈래 씬 허용</h3>
							<p class="form_desc">독자들이 씬을 자신이 생각하는 이야기 갈래로 새로 써 나갈 수 있게 허용</p>
							<div class="onoff prt"><input type="checkbox" id="sr_set3" class="ofchk"><label for="sr_set3" class="oflbl"><span></span></label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">포크 허용</h3>
							<p class="form_desc">독자들이 시리즈를 통째로 가져다가 새로 편집 해서 작성하게 허용. 포크된 시리즈는 원작자가 표시되면 유료과금시 원작자에게 일정 비율 배분 됩니다.</p>
							<div class="onoff prt"><input type="checkbox" id="sr_set4" class="ofchk"><label for="sr_set4" class="oflbl"><span></span></label></div>
						</div>
						<ul class="form_btn form_btn1">
							<li><span><button class="btn_r btn_l btn_cp">완료</button></span></li>
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
