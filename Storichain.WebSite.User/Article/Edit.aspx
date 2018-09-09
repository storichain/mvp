<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Storichain.WebSite.User.Edit" %>
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
    <link rel="stylesheet" type="text/css" href="Content/user.css">
    <link rel="stylesheet" type="text/css" href="Content/swiper.css">
    <script src="Scripts/lib/jquery.js"></script>
    <script src="Scripts/lib/jquery.plugin.js"></script>
    <script src="Scripts/lib/swiper.min.js"></script>
    <script src="Scripts/upload/jquery.ui.widget.js"></script>
    <script src="Scripts/upload/jquery.iframe-transport.js"></script>
    <script src="Scripts/upload/jquery.fileupload.js"></script>
    <script src="Scripts/common.js"></script>
    <script src="Scripts/user/base.js"></script>
    <script src="Scripts/user/user.js"></script>
    <script>

        var g_step_count                = 0;
        var g_event_idx                 = 0;
        var g_event_content_type_idx    = 0;
        var g_shop_product_idx          = 0;
        var g_user_idx                  = '<%= PageUtility.UserIdx() %>';

        var file_idx        = 0;
        var uuid            = stringUUID();
        var file_key        = 'supply';
        var file_count      = 0;
        var file_count_max  = 1;

        //var arr_swip        = [];

        var product_step_idx    = 0;
        var product_step_num    = 0;
        var scroll_height_y     = 0;
        var is_edit_view        = true;

        var pageLoaded      = 1;
        var page_count      = 0;
        var is_get_data     = false;

        var ing_yn          = 'N';

        $(function() {

            loadEditor();

            if(g_user_idx == 0)
            {
                location.href = "login";
                return;
            }

            g_event_idx = $.request('event_idx');
            ing_yn = $.request('ing_yn');

            if(g_event_idx > 0) 
            {
                //$('#form1 > .header > h1').text('기사수정');
                $('#btnPublish').show();
                $('#btnView').show();
                $('#btnEventDelete').show();

                postData();
                loadFileUpload_step_multi();
                $('#dvChangeImg').show();
            }
            else 
            {
                $('#form1 > .header > h1').text('글쓰기');
                $('#btnPublish').hide();
                $('#btnView').hide();
                $('#btnEventDelete').hide();
            }
            
            loadFileUpload();
            
            $('#btnCancel').click(function (e) {
                e.preventDefault();
                //location.href = 'index';
                history.back();
            });

            $('#btnAdd').click(function (e) {
                e.preventDefault();

                g_step_count++;
                createStep(g_step_count, 0);
            });

            $('#btnView').click(function (e) {
                e.preventDefault();

                if (g_event_idx > 0)
                {
                    console.info('미리보기');
                    //window.open('article?event_idx=' + g_event_idx, '', 'toolbar=0,location=0,directories=0,status=0,menubar=0,scrollbars=1,resizable=1,width=' + $(window).width + ',height=' + $(window).height);
                }
                else
                {
                    alert('기본 정보 저장 후 보기가 가능합니다.');
                }

            });

            $('#ckbOtherImage').click(function() {
                loadFileUpload();
            });

            $('#btnEventDelete').click(function (e) {
                e.preventDefault();

                var msg = '';

                if(ing_yn == 'Y')
                    msg = '작성 중인 기사를 삭제하시겠습니까?';
                else
                    msg = '이미 배포된 기사를 삭제하시겠습니까?';

                if(!confirm(msg)) 
                {
                    return;
                }

                $.ajax({
                        type: "POST",
                        url: url_path + "/Event/ModifyUseYN",
                        data: stringFormat("event_idx={0}&user_idx={1}&use_yn={2}", g_event_idx, g_user_idx, "N"),
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                            if(Number(response.response_code) == 1000) 
                            {
                                location.href = 'index';
                                return;
                            }
                        },
                        error: ajaxError,
                        complete: function () {
                            
                        }
                    });
            });
            
            $(document).on("click", "button[id ^= 'btnPublish']", function () {

                if(confirm('기사를 배포하시겠습니까?')) 
                {
                    $.ajax({
                        type: "POST",
                        url: url_path + "/Event/ModifyPublishYN",
                        data: stringFormat("event_idx={0}&user_idx={1}&publish_yn={2}", g_event_idx, g_user_idx, "Y"),
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                            if(Number(response.response_code) == 1000) 
                            {
                                alert('정상적으로 배포 되었습니다.');
                                location.href = 'Index';
                            }
                            else 
                            {
                                alert('처리 중 오류가 발생되었습니다.')
                                return false;
                            }
                        },
                        error: ajaxError,
                        complete: function () {
                            
                        }
                    });
                }


            });

            $(document).on("click", "input[id ^= 'ckbShopProduct']", function () {
				
                $("input[id ^= 'ckbShopProduct']").not(this).prop('checked', false);
            });

            $('.hbtn_back').click(function() {
		        $('.flayer').hide();
		        $('.edit').show();
                $(window).scrollTop(scroll_height_y);

                is_edit_view = true;

	        });

            $(window).scroll(function (event) {
                if(is_edit_view) 
                {
                    scroll_height_y = $(window).scrollTop();
                }
            });

            $(document).on("click", "button[id^='btnStepMoveDown']", function () {

                var $parent = $(this).parent().closest('.art_page');
                var nextO   = $parent.next();

                var step_idx = $parent.attr('idx');
                var step_num = $parent.attr('num');

                var step_idx_next = nextO.attr('idx');
                var step_num_next = nextO.attr('num');

                if (step_num_next != "" && step_num_next > 0)
                {
                    nextO.after($parent);

                    if(step_idx_next == 0 || step_idx == 0) 
                    {
                        return;
                    }
                    
                    var sort_values = step_idx + ";" + step_num_next + "|" + step_idx_next + ";" + step_num;

                    $.ajax({
                            type: "POST",
                            url: url_path + "/Step/ChangeSort",
                            data: stringFormat("event_idx={0}&sort_values={1}", g_event_idx, sort_values),
                            dataType: "json",
                            cache: false,
                            success: function (response) {
                                if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}
                                if(response.response_code == '1000') 
                                {
                                    $("div[id^='divPage']").each(function(index,item) {
                                        
                                        var step_num = $(this).attr('num');
                                        $(stringFormat('#spanPage_{0}', step_num)).text(index + 1);

                                    });

                                }
                            },
                            error: ajaxError,
                            complete: function () {

                            }
                        });
                }
            });

            $(document).on("keypress", "input[id^='txtStartDate']", function(e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });

            $(document).on("keypress", "input[id^='txtEndDate']", function(e) {
                if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
                    return false;
                }
            });

            $(document).on("change", "input[id^='ckbStepDataTypeIdx']", function() {

                var $parent             = $(this).parent().closest('.art_page');
                var step_idx            = $parent.attr('idx');
                var step_num            = $parent.attr('num');

                var dl_name             = $(stringFormat('#dlName_{0}', step_num));
                var dl_desc             = $(stringFormat('#dlDesc_{0}', step_num));
                var dl_vid              = $(stringFormat('#dlVId_{0}', step_num));
                
                if ($(this).is(':checked')) 
                {
                    lbl_start_date.show();
                    lbl_end_date.show();
                    txt_start_date.show();
                    txt_end_date.show();
                    dl_name.hide();
                    dl_desc.hide();
                    dl_vid.hide();

                    var now = new Date();
                    var day = ("0" + now.getDate()).slice(-2);
                    var month = ("0" + (now.getMonth() + 1)).slice(-2);
                    var today = now.getFullYear() + (month) + (day);
                    //alert(today);

                    if(txt_start_date.val() == '') 
                    {
                        txt_start_date.val(today);
                    }

                    if(txt_end_date.val() == '') 
                    {
                        txt_end_date.val(today);
                    }
                }
                else
                {
                    lbl_start_date.hide();
                    lbl_end_date.hide();
                    txt_start_date.hide();
                    txt_end_date.hide();
                    dl_name.show();
                    dl_desc.show();
                    dl_vid.show();
                }
            });

            $(document).on("click", "button[id^='btnStepRemove']", function() {

                var $item = $(this).parent().closest('.art_page');

                var step_idx = $item.attr('idx');

                if(step_idx == 0) 
                {
                    $item.remove();
                    return;
                }
                
                if(!confirm('기사내용을 삭제하시겠습니까?')) 
                {
                    return;
                }

                $.ajax({
                    type: "POST",
                        url: url_path + "/Step/Remove",
                        data: stringFormat("event_idx={0}&step_idx={1}", g_event_idx, step_idx),
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}
                            if(response.response_code == '1000') 
                            {
                                $item.remove();

                                $("div[id^='divPage']").each(function(index,item) {
                                        
                                    var step_num = $(this).attr('num');
                                    $(stringFormat('#spanPage_{0}', step_num)).text(index + 1);

                                });

                                $('.total_page').text($('.art_page').length);
                            }
                        },
                        error: ajaxError,
                        complete: function () {

                        }
                    });
            });

            $(document).on("click", "button[id^='btnStepSave']", function() {

                var $parent             = $(this).parent().closest('.art_page');
                var step_idx            = $parent.attr('idx');
                var step_num            = $parent.attr('num');

                var step_name           = $(stringFormat('#txtStepName_{0}', step_num)).val();
                var step_desc           = $(stringFormat('#txtStepDesc_{0}', step_num)).val();
                var step_pic_name       = $(stringFormat('#txtPickName_{0}', step_num)).val();
                var step_writer_name    = $(stringFormat('#txtStepWriterName_{0}', step_num)).val();
                var step_v_id           = $(stringFormat('#txtStepVId_{0}', step_num)).val();
                var step_v_name         = $(stringFormat('#txtStepVName_{0}', step_num)).val();
                var step_url            = $(stringFormat('#txtStepUrl_{0}', step_num)).val();
                var file_idx            = $(stringFormat('#hdFileIdx_{0}', step_num)).val();
                var step_data_type_idx  = "1";
                
                if(step_v_id == "") 
                {
                    
                }
                else 
                {
                    if(step_v_id.length != 11) 
                    {                    
                        if(step_v_id.indexOf('http://') == -1 && step_v_id.indexOf('https://') == -1) 
                        {
                            step_v_id = "http://" + step_v_id;
                            $(stringFormat('#txtStepVId_{0}', step_num)).val(step_v_id);
                        }
                    }
                }

                if(step_url == "") 
                {
                    
                }
                else 
                {
                    if(step_url.indexOf('http://') == -1 && step_url.indexOf('https://') == -1 && step_url.indexOf('beautytalk://') == -1) 
                    {
                        step_url = "http://" + step_url;
                        $(stringFormat('#txtStepUrl_{0}', step_num)).val(step_url);
                    }
                }

                if(ytVidId(step_v_id) == false) 
                {
                    step_v_id = "";
                }
                else 
                {
                    if(step_v_id.length != 11) 
                    {
                        step_v_id = ytVidId(step_v_id);
                    }
                }
                
                step_name           = encodeURIComponent(step_name);
                step_desc           = encodeURIComponent(step_desc);
                step_pic_name       = encodeURIComponent(step_pic_name);
                step_writer_name    = encodeURIComponent(step_writer_name);
                step_url            = encodeURIComponent(step_url);
                step_v_name         = encodeURIComponent(step_v_name);

                start_date = '';
                end_date = '';

                var url;

                if(step_idx > 0)
                    url = url_path + "/Step/Modify";
                else
                    url = url_path + "/Step/Add";

                var data = stringFormat("event_idx={0}&step_idx={1}&user_idx={2}&step_name={3}&step_desc={4}&step_pic_name={5}&step_writer_name={6}&step_v_id={7}&step_url={8}&step_data_type_idx={9}&start_date={10}&end_date={11}&step_v_name={12}", 
                    g_event_idx, step_idx, g_user_idx, step_name, step_desc, step_pic_name, step_writer_name, step_v_id, step_url, step_data_type_idx, start_date, end_date, step_v_name);

                $.ajax({
                    type: "POST",
                        url: url,
                        data: data,
                        dataType: "json",
                        cache: false,
                        success: function (response) {
                            if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}
                            if(response.response_code == '1000') 
                            {
                                if(step_idx == 0) 
                                {
                                    if(response.response_data_count > 0) 
                                    {
                                        $(stringFormat('#divPage_{0}', step_num)).attr("idx", response.response_data[0].step_idx);
                                    }
                                }
                                
                                alert('저장되었습니다.');
                            }
                        },
                        error: ajaxError,
                        complete: function () {

                        }
                    });
            });

            $('#btnSave').click(function (e) {
                e.preventDefault();

                if($('#txtSupplyName').val() == "")
                {
                    alert('기사명을 입력하세요.');
                    return;
                }

                if($('#txtSupplyDesc').val() == "")
                {
                    alert('기사전문을 입력하세요.');
                    return;
                }

                if(file_idx == "0")
                {
                    alert('기사 커버를 등록하세요.');
                    return;
                }

                var service_event_use_yn        = 'N';
                
                var url;
                var data;

                var event_content_type_idx  = 1;
                var channel_idx             = $('#art_series option:selected').val();
                var channel_idx             = 0;
                var orientation_type_idx    = 1;
                var topic_idx               = $('#art_cate option:selected').val();
                var supply_name             = $('#txtSupplyName').val().split("+").join("＋");
                var supply_desc             = $('#txtSupplyDesc').val().split("+").join("＋");
                var supply_tag              = $('#txtSupplyTag').val();
                var supply_pic_name         = $('#txtSupplyPicName').val();
                var shop_product_idx        = 0;
                var private_view_yn         = 'N';

                if($("#ckbPrivateViewYn").is(':checked'))
                    private_view_yn = 'Y';

                if(event_content_type_idx == 3) 
                {
                    var topic_idx = '9999';
                }

                var event_theme_type_idxs = '';
                var isFirst = true;

                if(event_content_type_idx == 1) 
                {
                    if($('#art_cate option:selected').val() == "")
                    {
                        alert('카테고리를 선택하세요.');
                        return;
                    }

                    if(!confirm('기사내용을 저장하시겠습니까?'))
                    {
                        return false;
                    }
                }
                else 
                {
                    if(!confirm('기사내용을 저장하시겠습니까?'))
                    {
                        return false;
                    }
                }

                supply_name = encodeURIComponent(supply_name);
                supply_desc = encodeURIComponent(supply_desc);
                supply_tag  = encodeURIComponent(supply_tag);

                service_start_date  = '';
                service_end_date    = '';

                console.log('channel_idx : ' + channel_idx);

                if(g_event_idx == 0) 
                {
                    url = url_path + "/Event/Add";
                    data = stringFormat("user_idx={0}&channel_idx={1}&topic_idx={2}&supply_name={3}&supply_desc={4}&supply_tag={5}&supply_file_idx={6}&service_event_use_yn={7}&service_start_date={8}&service_end_date={9}&event_theme_type_idxs={10}&supply_pic_name={11}&orientation_type_idx={12}&event_content_type_idx={13}&shop_product_idx={14}&private_view_yn={15}", g_user_idx, channel_idx, topic_idx, supply_name, supply_desc, supply_tag, file_idx, service_event_use_yn, service_start_date, service_end_date, event_theme_type_idxs,supply_pic_name,orientation_type_idx,event_content_type_idx,g_shop_product_idx,private_view_yn);
                }
                else 
                {
                    url = url_path + "/Event/Modify";
                    data = stringFormat("user_idx={0}&channel_idx={1}&topic_idx={2}&supply_name={3}&supply_desc={4}&supply_tag={5}&supply_file_idx={6}&event_idx={7}&service_event_use_yn={8}&service_start_date={9}&service_end_date={10}&event_theme_type_idxs={11}&supply_pic_name={12}&orientation_type_idx={13}&event_content_type_idx={14}&shop_product_idx={15}&private_view_yn={16}", g_user_idx, channel_idx, topic_idx, supply_name, supply_desc, supply_tag, file_idx, g_event_idx, service_event_use_yn, service_start_date, service_end_date, event_theme_type_idxs,supply_pic_name,orientation_type_idx,event_content_type_idx,g_shop_product_idx,private_view_yn);
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

                                loadFileUpload_step_multi();

                                console.info('event_idx ' + g_event_idx);
                                
                                $('#step_box').show();
                                $('#btnAdd').show();
                                $('#btnAddFiles').show();

                                $('#btnPublish').show();
                                $('#btnView').show();
                                $('#btnEventDelete').show();
                                $('#dvChangeImg').show();

                                supplyData();
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

            $(window).scroll(function () {

                if(!is_edit_view) 
                {
                    var buffer = 100;
                    var storyList_view = $(window);

                    if ($("html").prop('scrollHeight') - storyList_view.scrollTop() <= storyList_view.height() + buffer) 
                    {
                        if (pageLoaded > 0) 
                        {
                            if (page_count > pageLoaded) 
                            {
                                
                            }
                        }
                    }
                }
            });
        });

        function createStep(step_count, step_idx) 
        {
            var script = stringFormat(
                            '<div class="art_page" id="divPage_{0}" num="{0}" idx="{1}">\
					            <div class="wbox">\
                                    <dl class="fd">\
							            <dt>사진</dt>\
							            <dd>\
								            <div class="art_img_box">\
									            <div class="art_img_area">\
                                                    <span style="display:none;"><input id="fileupload_{0}" type="file" name="files[]" accept="image/*" onchange="changeImg(this)"/></span>\
										            <button type="button" class="btn_img_up" onclick="checkFileStep({0});"><div class="art_img"><img src="/Images/btn_img_up1.png" id="imgStep_{0}" /></div><span class="blind">사진 올리기</span></button>\
                                                    <input type="hidden" id="hdFileIdx_{0}" value="0" />\
									            </div>\
									            <div class="art_source">\
										            <label for="txtPickName_{0}">사진출처</label>\
										            <input type="text" id="txtPickName_{0}" class="inp_txt" />\
									            </div>\
								            </div>\
							            </dd>\
						            </dl>\
						            <dl class="fd" id="dlName_{0}">\
							            <dt><label for="txtStepName_{0}">제목(25)</label></dt>\
							            <dd><input type="text" id="txtStepName_{0}" class="inp_txt" maxlength="25" placeholder="(제목이 없으면 본문만 노출됩니다)"/></dd>\
						            </dl>\
						            <dl class="fd" id="dlDesc_{0}">\
							            <dt><label for="txtStepDesc_{0}">본문</label></dt>\
							            <dd><textarea id="txtStepDesc_{0}" class="inp_textarea" cols="30" rows="10" placeholder="(본문이 없으면 이미지만 노출됩니다)"></textarea></dd>\
							            <dd>\
								            <div class="art_source">\
									            <label for="txtStepWriterName_{0}">글 출처</label>\
									            <input type="text" id="txtStepWriterName_{0}" class="inp_txt" />\
								            </div>\
							            </dd>\
						            </dl>\
						            <dl class="fd fd_bdt" id="dlVId_{0}">\
							            <dt><label for="txtStepVId_{0}">유튜브</label></dt>\
							            <dd><input type="text" id="txtStepVId_{0}" class="inp_txt inp_link" placeholder="https://www.youtube.com/watch?v=xxxxxxxxxxx 형식으로 입력해주세요." /></dd>\
						            </dl>\
                                    <dl class="fd" id="dlVName_{0}">\
							            <dt><label for="txtStepVName_{0}">영상제목</label></dt>\
							            <dd><input type="text" id="txtStepVName_{0}" class="inp_txt" maxlength="50" placeholder="(유튜브 링크를 입력시 영상제목을 넣어주세요.)"/></dd>\
						            </dl>\
						            <dl class="fd fd_bdt">\
							            <dt><label for="txtStepUrl_{0}">관련링크</label></dt>\
							            <dd><input type="text" id="txtStepUrl_{0}" class="inp_txt inp_link" /></dd>\
						            </dl>\
					            </div>\
					            <div class="page_control">\
						            <p class="page_num"><span class="tpage" id="spanPage_{0}">{0}</span> / <em class="total_page">1</em></p>\
						            <div class="pos_l">\
							            <button type="button" class="btn btn_page_save" id="btnStepRemove_{0}">삭제</button>\
							            <button type="button" class="btn btn_page_save" id="btnStepSave_{0}">저장</button>\
						            </div>\
						            <div class="pos_r">\
							            <button type="button" class="btn btn_page_up" id="btnStepMoveUp_{0}"><span class="blind">페이지 순서변경 위로</span></button>\
							            <button type="button" class="btn btn_page_down" id="btnStepMoveDown_{0}"><span class="blind">페이지 순서변경 아래로</span></button>\
						            </div>\
					            </div>\
				            </div>', 
                            step_count,
                            step_idx);

            $('#step_box').append(script);
            $('.total_page').text(step_count);
            loadFileUpload_step(step_count);

            var $parent = $('#fileupload_' + step_count).parent().closest('.art_page');

            var step_idx = $parent.attr('idx');
            var step_num = $parent.attr('num');

            //arr_swip[step_count - 1] = new Swiper(stringFormat('#rel_wrap{0}', step_count), {
            //                                    slidesPerView: "auto",
            //                                    paginationClickable: true,
            //                                    spaceBetween: 7,
            //                                    freeMode: true
            //                                });
        }

        $('#fileupload').prop('name', file_key);
        function loadFileUpload() 
        {
            var url;
            var overwrite = 'N';

            if($("#ckbOtherImage").is(':checked'))
                overwrite = 'Y';

            if(g_event_idx > 0)
                url = url_path + "/Event/Modify?event_idx=" + g_event_idx + "&user_idx=" + g_user_idx + '&overwrite=' + overwrite + '&file_key=supply_thumb';
            else
                url = url_path + '/File/Upload?file_path=' + file_key + '&temp_key=' + uuid + '&file_idx=' + file_idx;

            $('#fileupload').fileupload({
                url: url,
                dataType: 'json',
                add: function(e, data) {

                    if (g_user_idx == 0) 
                    {
                        alert("로그인해 주세요.");
                        var jqXHR = data.submit();
                        jqXHR.abort();
                    }
                    else 
                    {
                        var uploadErrors = [];
                        
                        if ($("#ckbOtherImage").is(':checked'))
                        {
                            if (data.originalFiles[0].size > (1.5 * 1024 * 1024)) {
                                uploadErrors.push('썸네일의 경우 1.5MB이하 파일로 업로드 하세요.');
                            }
                        }
                        else
                        {
                            if (data.originalFiles[0].size > (10 * 1024 * 1024)) {
                                uploadErrors.push('파일 사이즈가 너무 큽니다. 10MB이하 파일로 업로드 하세요.');
                            }
                        }

                        if(uploadErrors.length > 0) 
                        {
                            alert(uploadErrors.join("\n"));
                        } 
                        else 
                        {
                            data.submit();
                        }
                    }
                },
                done: function (e, data) {

                    if(g_event_idx > 0) 
                    {
                        if(data.result.response_data_count > 0)
                        {
                            $.each(data.result.response_data, function(i, value){

                                if(value.supply_image_count > 0) 
                                {
                                    file_idx = value.supply_images[0].image_idx;    
                                }
                                else 
                                {
                                    file_idx = 0;
                                }

                                $('#imgCover').attr('src', imgUrl(value, file_key, 'supply_thumb'));
                            });
                        }
                    }
                    else 
                    {
                        $.each(data.result.response_data, function (index, file) {

                            file_idx = file.image_idx;

                            if(data.result.response_data_count - 1 == index) 
                            {
                                $('#imgCover').attr('src',file.image_url);
                            }

                            if(index == 0)
                            {
                                file_count++;
                                return; 
                            }
                        });    
                    }
                    
                },
                start: function (e, data) {
                    $(".art_img_area").append('<span class="loading"><em style="width:0%"></em></span>');
                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $(".art_img_area > .loading").html('<span class="loading"><em style="width:' + progress + '%"></em></span>');

                    if(progress == 100) 
                    {
                        $(".art_img_area > .loading").remove();
                    }
                }
            }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
        }

        function loadFileUpload_step(step_num) 
        {
            var $parent = $('#fileupload_' + step_num).parent().closest('.art_page');

            var step_idx = $parent.attr('idx');
            var step_num = $parent.attr('num');

            if(step_idx > 0)
                url = url_path + "/Step/Modify";
            else
                url = url_path + "/Step/Add";

            var enable = true;

            $('#fileupload_' + step_num).fileupload({
                url: stringFormat('{0}?event_idx={1}&step_idx={2}&user_idx={3}', url, g_event_idx, step_idx, g_user_idx),
                dataType: 'json',
                add: function(e, data) {

                    var uploadErrors = [];
                    
                    if(data.originalFiles[0].size > (10*1024*1024)) 
                    {
                        uploadErrors.push('파일 사이즈가 너무 큽니다. 10MB이하 파일로 업로드 하세요.');
                    }

                    if(uploadErrors.length > 0) 
                    {
                        alert(uploadErrors.join("\n"));
                    } 
                    else 
                    {
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $.each(data.result.response_data, function (index, value) {

                        if(step_idx == 0) 
                        {
                            $('#fileupload_' + step_num).parent().closest('.art_page').attr("idx", value.step_idx);
                            loadFileUpload_step(step_num);
                        }

                        var step_img_url = imgUrl(value, "step", "step_thumb");
                        var step_file_idx = 0;

                        if(value.step_image_count > 0) 
                        {
                            step_file_idx = value.step_images[0].image_idx;
                        }
                            
                        if(step_img_url != "")
                        {
                            $('#step_box').find(stringFormat('#imgStep_{0}', step_num)).attr('src', step_img_url);
                            $('#step_box').find(stringFormat('#hdFileIdx_{0}', step_num)).val(step_file_idx);
                        }
                        

                    });
                        
                },
                start: function (e, data) {
                    $('#fileupload_' + step_num).parent().closest('.art_page').find(".art_img_area").append('<span class="loading"><em style="width:0%"></em></span>');
                },
                progressall: function (e, data) {
                    var progress = parseInt(data.loaded / data.total * 100, 10);
                    $('#fileupload_' + step_num).parent().closest('.art_page').find(".art_img_area > .loading").html('<span class="loading"><em style="width:' + progress + '%"></em></span>');

                    if(progress == 100) 
                    {
                        $('#fileupload_' + step_num).parent().closest('.art_page').find(".art_img_area > .loading").remove();
                    }
                }
            }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
        }

        function sleep(ms) {
            var unixtime_ms = new Date().getTime();
            while(new Date().getTime() < unixtime_ms + ms) {}
        }

        function loadFileUpload_step_multi() 
        {
            url = url_path + "/Step/Add";

            var enable = true;

            $('#fileupload_multi').fileupload({
                url: stringFormat('{0}?event_idx={1}&step_idx={2}&user_idx={3}', url, g_event_idx, 0, g_user_idx),
                dataType: 'json',
                add: function(e, data) {

                    var uploadErrors = [];
                    
                    if(data.originalFiles[0].size > (10*1024*1024)) 
                    {
                        uploadErrors.push('파일 사이즈가 너무 큽니다. 10MB이하 파일로 업로드 하세요.');
                    }

                    if(uploadErrors.length > 0) 
                    {
                        alert(uploadErrors.join("\n"));
                    } 
                    else 
                    {
                        sleep(300);
                        data.submit();
                    }
                },
                done: function (e, data) {

                    $.each(data.result.response_data, function (index, value) {

                        var step_num = ++g_step_count;
                        createStep(g_step_count, 0);

                        $('#fileupload_' + step_num).parent().closest('.art_page').attr("idx", value.step_idx);
                        loadFileUpload_step(step_num);
                        
                        var step_img_url = imgUrl(value, "step", "step_thumb");
                        var step_file_idx = 0;

                        if(value.step_image_count > 0) 
                        {
                            step_file_idx = value.step_images[0].image_idx;
                        }
                            
                        if(step_img_url != "")
                        {
                            $('#step_box').find(stringFormat('#imgStep_{0}', step_num)).attr('src', step_img_url);
                            $('#step_box').find(stringFormat('#hdFileIdx_{0}', step_num)).val(step_file_idx);
                        }
                    });
                        
                },
                start: function (e, data) {
                    
                },
                progressall: function (e, data) {
                    
                }
            }).prop('disabled', !$.support.fileInput)
                .parent().addClass($.support.fileInput ? undefined : 'disabled');
        }

        function postData() 
        {
            supplyData();

        }

        function supplyData() 
        {
            if(g_event_idx == 0)
                return;

            $('#lblIdx').text(g_event_idx);

            $.ajax({
                type: "POST",
                url: url_path + "/Event/GetList",
                data: stringFormat("event_idx={0}", g_event_idx),
                dataType: "json",
                cache: false,
                success: function (response) {
                    if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}

                    if(response.response_data_count > 0)
                    {
                        $.each(response.response_data, function(i, value){

                            if(value.supply_image_count > 0) 
                            {
                                file_idx = value.supply_images[0].image_idx;    
                            }
                            else 
                            {
                                file_idx = 0;
                            }

                            $('#imgCover').attr('src', imgUrl(value, file_key, 'supply_thumb'));
                            $("#art_series").val(value.channel_idx).change();
                            $("#art_cate").val(value.topic_idx).change();
                            $('#txtSupplyName').val(value.supply_name);
                            $('#txtSupplyDesc').val(value.supply_desc);
                            $('#txtSupplyTag').val(value.supply_tag);
                            $('#txtSupplyPicName').val(value.supply_pic_name);
                            $('#ckbPrivateViewYn').prop( "checked", (value.private_view_yn == 'Y')? true:false);

                            var event_content_type_idx = value.event_content_type_idx;

                            g_event_content_type_idx    = event_content_type_idx;
                            g_shop_product_idx          = value.shop_product_idx;
                            
                            var strs = value.event_theme_type_idxs.split(',');

                            if(value.publish_yn == 'Y') 
                            {
                                
                            }
                            else 
                            {
                                $('#btnPublish').remove();
                                if(value.publish_reserve_date == '' || value.publish_reserve_date == null) 
                                {
                                    var html = '<button type="button" class="btn hbtn_comp" id="btnPublish">배포</button>';
                                    $(html).prependTo($('#dvRight'));
                                }
                                else 
                                {
                                    var html = '<button type="button" class="btn hbtn_comp" id="btnPublish" style="width:60px">예약중</button>';
                                    $(html).prependTo($('#dvRight'));
                                }
                            }

                            if(value.service_event_use_yn == 'Y') 
                            {
                                
                            }
                            else 
                            {
                                
                            }

                            if(value.event_content_type_idx == 1) 
                            {
                                $('#dlBy').show();
                                $('#step_box').show();
                                $('#btnAdd').show();
                                $('#btnAddFiles').show()

                                $('#form1 > .header > h1').text('기사수정');

                                stepData();
                            }
                            else 
                            {
                                $('#dlBy').show();
                                $('#step_box').show();
                                $('#btnAdd').show();
                                $('#btnAddFiles').show()

                                $('#form1 > .header > h1').text('기사수정');

                                stepData();
                            }

                        });
                    }
                },
                error: ajaxError,
                complete: function () {

                }
            });
        }

        function stepData() 
        {
            $.ajax({
                type: "POST",
                url: url_path + "/Step/GetAll",
                data: stringFormat("event_idx={0}&user_idx={1}", g_event_idx, g_user_idx),
                dataType: "json",
                cache: false,
                success: function (response) {
                    if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}
                        
                    if(response.response_data_count > 0)
                    {
                        $('.art_page').empty();
                        $.each(response.response_data, function(i, value){

                            var step_num = i + 1;
                            var step_idx = value.step_idx;

                            createStep(step_num, step_idx);
                            loadFileUpload_step(step_num);

                            g_step_count = step_num;

                            var step_img_url = imgUrl(value, "step", "step_thumb");
                            var step_file_idx = 0;

                            if(value.step_image_count > 0) 
                            {
                                step_file_idx = value.step_images[0].image_idx;    
                            }
                            
                            if(step_img_url != "")
                            {
                                $('#step_box').find(stringFormat('#imgStep_{0}', step_num)).attr('src', step_img_url);
                            }

                            if(step_file_idx == "")
                                step_file_idx = 0;

                            $('#step_box').find(stringFormat('#txtStepName_{0}', step_num)).val(value.step_name);
                            $('#step_box').find(stringFormat('#txtStepDesc_{0}', step_num)).html(value.step_desc);
                            $('#step_box').find(stringFormat('#txtPickName_{0}', step_num)).val(value.step_pic_name);
                            $('#step_box').find(stringFormat('#txtStepWriterName_{0}', step_num)).val(value.step_writer_name);
                            $('#step_box').find(stringFormat('#txtStepVName_{0}', step_num)).val(value.step_v_name);

                            if(value.step_v_id.length == 11)  
                            {
                                $('#step_box').find(stringFormat('#txtStepVId_{0}', step_num)).val("https://www.youtube.com/watch?v=" + value.step_v_id);    
                            }
                            
                            $('#step_box').find(stringFormat('#txtStepUrl_{0}', step_num)).val(value.step_url);
                            $('#step_box').find(stringFormat('#hdFileIdx_{0}', step_num)).val(step_file_idx);

                            if(value.product_data_count > 0) 
                            {
                                $.each(value.product_data, function(item_i, item_value) {

                                    var product_img_url = imgUrl(item_value, "product", "product");

                                    var str_script = stringFormat('<li class="swiper-slide"><img src="{0}" alt="{1}" /><button type="button" class="btn btn_pd_del" id="btnDeleteProduct_{2}" product_idx="{2}"><span class="blind">상품삭제</span></button></li>', 
                                        product_img_url, item_value.product_name, item_value.product_idx);

                                    $(stringFormat('#rel_wrap{0} > ul', step_num)).append(str_script);

                                });
                            }

                            var dl_name         = $('#step_box').find(stringFormat('#dlName_{0}', step_num));
                            var dl_desc         = $('#step_box').find(stringFormat('#dlDesc_{0}', step_num));
                            var dl_vid          = $('#step_box').find(stringFormat('#dlVId_{0}', step_num));
                            var dl_vname        = $('#step_box').find(stringFormat('#dlVName_{0}', step_num));

                            //arr_swip[step_num - 1].onResize();

                        });
                    }
                    else
                    {
                        if(response.response_code == '1000')
                            $('.art_page').empty();
                    }

                },
                error: ajaxError,
                complete: function () {

                }
            });
        }

        function loadEditor() 
        {
            $.ajax({
                    type: "POST",
                    url: 'http://mnb.joins.com/service' + "/Content/GetEditorUser",
                    data: null,
                    dataType: "json",
                    cache: false,
                    success: function (response) {
                        if(Number(response.response_code) >= 3000) if(Number(response.response_code) == 3000) {alert(response.response_message); return;}else {location.href = '/logout';return;}

                        $('#sel_editor').empty().append('<option selected="selected" value="0">::: 에디터 선택 :::</option>');

                        if(response.response_data_count > 0)
                        {
                            $.each(response.response_data, function(i, value){

                                $('#sel_editor').append($('<option>', { 
                                    value: value.user_idx,
                                    text : value.user_name 
                                }));

                            });
                        }
                    },
                    error: ajaxError,
                    complete: function () {

                    }
                });
        }

        function ytVidId(url) 
        {
            var p = /^(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?(?=.*v=((\w|-){11}))(?:\S+)?$/;
            return (url.match(p)) ? RegExp.$1 : false;
        }

        function validateURL(value) 
        {
            return /^(https?|ftp):\/\/(((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:)*@)?(((\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5])\.(\d|[1-9]\d|1\d\d|2[0-4]\d|25[0-5]))|((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.?)(:\d*)?)(\/((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)+(\/(([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)*)*)?)?(\?((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|[\uE000-\uF8FF]|\/|\?)*)?(\#((([a-z]|\d|-|\.|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(%[\da-f]{2})|[!\$&'\(\)\*\+,;=]|:|@)|\/|\?)*)?$/i.test(value);
        }

        function viewPrductList(resonse_data) 
        {
            page_count = resonse_data.response_option.page_count;

            $.each(resonse_data.response_data, function (i, value) {

                var liStr = stringFormat(
                    '<li>\
						<input type="checkbox" class="chk" id="ckbProduct_{0}" product_idx={0} />\
						<label for="ckbProduct_{0}">\
							<span class="pd_img"><img src="{1}" alt="{2}" /></span>\
							<em>{3}</em>\
							<strong>{2}</strong>\
						</label>\
					</li>'
                    , value.product_idx
                    , imgUrl(value, 'product', 'product')
                    , value.product_name
                    , value.brand_name
                    );

                $("#ulProduct").append(liStr);

            });

        }

        function viewShopPrductList(resonse_data) 
        {
            page_count = resonse_data.response_option.page_count;

            $.each(resonse_data.response_data, function (i, value) {

                var liStr = stringFormat(
                    '<li>\
						<input type="checkbox" class="chk" id="ckbShopProduct_{0}" product_idx={0} product_name="{3}" product_url="{1}" />\
						<label for="ckbShopProduct_{0}">\
							<span class="pd_img"><img src="{1}" alt="{2}" /></span>\
							<em>{3}</em>\
							<strong>{2}</strong>\
						</label>\
					</li>'
                    , value.product_idx
                    , imgUrl(value, 'product', 'product')
                    , value.product_name
                    , value.brand_name
                    );

                $("#ulShopProduct").append(liStr);

            });

        }

        function moreProduct(page, isScroll) {

            if (is_get_data)
                return;

            if($('#txtProductName').val() == "") 
            {
                alert('검색어를 입력하세요.');
                return;
            }

            is_get_data = true;

            url = url_path + "/Product/SearchLists";

            var spinner = new Spinner(g_spin_common).spin();

            //if (page > 1) 
            if (page > 0) 
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
                data: stringFormat("page={0}&page_rows={1}&brand_idx={2}&product_type_idx={3}&product_name={4}", 
                    page, 20, $('#ddlBrand option:selected').val(), $('#ddlProductType option:selected').val(), $('#txtProductName').val() ),
                dataType: "json",
                cache: false,
                success: function (response) {
                    if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                    page_count = response.response_option.page_count;

                    if (isScroll)
                        pageLoaded = page;

                    if(page == 1)
                    {
                        $("#ulProduct").empty();
                    }

                    viewPrductList(response);

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

        
        function moreShopProduct(page, isScroll) {

            if (is_get_data)
                return;

            if($('#txtShopProductName').val() == "") 
            {
                alert('판매상품 검색어를 입력하세요.');
                return;
            }

            is_get_data = true;

            url = url_path + "/Shop/Product/SearchProductForPaing";

            var spinner = new Spinner(g_spin_common).spin();

            //if (page > 1) 
            if (page > 0) 
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
                data: stringFormat("page={0}&page_rows={1}&brand_idx={2}&product_name={3}", 
                    page, 20, $('#ddlShopBrand option:selected').val(), $('#txtShopProductName').val() ),
                dataType: "json",
                cache: false,
                success: function (response) {
                    if (Number(response.response_code) >= 3000) if (Number(response.response_code) == 3000) { alert(response.response_message); return; } else { location.href = '/logout'; return; }

                    page_count = response.response_option.page_count;

                    if (isScroll)
                        pageLoaded = page;

                    if(page == 1)
                    {
                        $("#ulShopProduct").empty();
                    }

                    viewShopPrductList(response);

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


        function changeImg(obj) {
            var limitSize = 1024 * 2000;
            var limitSize2 = 1024 * 1500;
            var fPath = obj.value;

            if (obj.files && obj.files[0]) {
                var file = obj.files[0];
                var fleng = 0;
                var str = navigator.userAgent.toLowerCase();
                if (-1 != str.indexOf('msie')) {
                    try {
                        var myFSO = new ActiveXObject("Scripting.FileSystemObject");
                        var filePath = file.value;
                        var thefile = myFSO.getFile(filePath);
                        fleng = thefile.size;

                        fPath = filePath;
                    } catch (e) { fleng = file.size; }
                } else {
                    fleng = file.size;
                }

                if (fPath.toLowerCase().indexOf(".gif") > -1) {
                    if (fleng > limitSize) {
                        alert('gif 파일 : 등록 가능한 파일 최대 용량(2mb)를 초과하였습니다.');
                        $(obj).val("");
                        return;
                    }
                } else {
                    if (fleng > limitSize2) {
                        alert('이미지파일 : 등록 가능한 파일 최대 용량(1.5MB)를 초과하였습니다.');
                        $(obj).val("");
                        return;
                    }
                }


            }

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
        <div class="header">
	        <h1></h1>
            <div class="posl">
		        <button type="button" class="btn hbtn_x" id="btnCancel"><span class="blind">취소</span></button>
		        <button type="button" class="btn hbtn_view" id="btnView">보기</button>
	        </div>
	        <div class="posr" id="dvRight">
                <button type="button" class="btn hbtn_del" id="btnEventDelete"><span class="blind">삭세</span></button>
	        </div>
        </div>

        <div class="wrap">

	        <div id="ct" class="ct">

			        <div class="edit">
				        <h2>커버</h2>
				        <div class="wbox">
					        <dl class="fd">
						        <dt>커버</dt>
						        <dd>
							        <div class="art_img_box">
								        <div class="art_img_area">
                                            <span style="display:none;"><input id="fileupload" type="file" name="files[]" accept="image/*" onchange="changeImg(this)"/></span>
									        <button type="button" class="btn_img_up" onclick="checkFile();"><div class="art_img"><img src="/article/Images/btn_img_up1.png" id="imgCover" /></div><span class="blind">사진 올리기</span></button>
								        </div>
                                        <div class="art_source" style="border-bottom:0;display:none" id="dvChangeImg">
									        <label for="ckbOtherImage" style="100px">썸네일만 변경</label>
									        <input type="checkbox" id="ckbOtherImage" style="display:inline;margin:13px 0 0 30px" />
								        </div>
							        </div>
						        </dd>
					        </dl>
					        <dl class="fd">
						        <dt><label for="art_tit0">제목(100)</label></dt>
						        <dd><input type="text" id="txtSupplyName" class="inp_txt" maxlength="100"/></dd>
					        </dl>
					        <dl class="fd">
						        <dt><label for="art_desc0">설명</label></dt>
						        <dd><textarea id="txtSupplyDesc" class="inp_textarea" cols="30" rows="10"></textarea></dd>
					        </dl>
					        <dl class="fd" id="dlCate">
						        <dt><label for="art_cate">카테고리</label></dt>
						        <dd>
							        <select id="art_cate" class="sel">
								        <option value="">카테고리를 선택하세요.</option>
                                        <% 
                                            Biz_Code biz_code = new Biz_Code();
                                            DataTable dt = biz_code.GetCodeList(10,"Y",0);

                                            foreach(DataRow dr in dt.Rows)
                                            {
                                                WebUtility.Write(string.Format("<option value='{0}'>{1}</option>", dr.ItemValue("code_value"), dr.ItemValue("code_name")));
                                            }
                                        %>

							        </select>
						        </dd>
					        </dl>
					        <dl class="fd" id="dlSeries">
						        <dt><label for="art_series">시리즈</label></dt>
						        <dd>
							        <select id="art_series" class="sel">
								        <option value="0">시리즈 없음</option>

                                        <% 
                                            int total_count;
                                            int page_count;

                                            Biz_Channel biz_channel = new Biz_Channel();
                                            DataTable dtChannel = biz_channel.GetLists(WebUtility.GetRequestByInt("channel_idx"),
                                                                                        WebUtility.GetRequestByInt("channel_type_idx"),
                                                                                        WebUtility.GetRequest("use_yn", "Y"),
                                                                                        "name",
                                                                                        DateTime.Now,
                                                                                        WebUtility.GetRequestByInt("page", 1), 
                                                                                        WebUtility.GetRequestByInt("page_rows", 1000),
                                                                                        out total_count,
                                                                                        out page_count);

                                            foreach(DataRow dr in dtChannel.Rows)
                                            {
                                                WebUtility.Write(string.Format("<option value='{0}'>{1}</option>", dr.ItemValue("channel_idx"), dr.ItemValue("channel_name")));
                                            } 
                                        %>

							        </select>
						        </dd>
					        </dl>
					        <dl class="fd" id="dlTag">
						        <dt><label for="art_tag">태그</label></dt>
						        <dd><input type="text" id="txtSupplyTag" class="inp_txt inp_link" placeholder="쉼표(,)로 구분" /></dd>
					        </dl>
                            <dl class="fd" id="dlBy">
						        <dt><label for="art_tag">By</label></dt>
						        <dd><input type="text" id="txtSupplyPicName" class="inp_txt" placeholder="" /></dd>
					        </dl>
                            <dl class="fd" id="dlPrivateViewYn">
						        <dt><label for="ckbPrivateViewYn">비공개</label></dt>
						        <dd><input type="checkbox" id="ckbPrivateViewYn" style="display:inline;margin:12px 0 0 0px" /> (Y:비공개,N:공개)</dd>
					        </dl>
				        </div>

                        <div class="btn_area"><button type="button" class="btn btn_add_page" id="btnSave">커버내용 저장</button></div>

                        <div id="step_box" style="padding-top:0;display:none">
                            <h2 style="margin-top:20px;" id="title_step_content">페이지</h2>
                        </div>

                        <div class="btn_area"><span style="display:none;"><input id="fileupload_multi" type="file" name="files[]" accept="image/*" multiple onchange="changeImg(this)"/></span><button type="button" class="btn btn_add_page" id="btnAddFiles" style="display:none" onclick="checkFileStepMulti();">멀티 이미지로 페이지 추가</button></div>
                        <div class="btn_area"><button type="button" class="btn btn_add_page" id="btnAdd" style="display:none">단일 페이지 추가</button></div>
				        
			        </div>

			        
                    
		        <!-- // edit -->

	        </div>
	        <!-- // ct -->
        </div>

        <div id="loadingDiv"></div>

<script>

    function checkFile() 
    {
        if (g_user_idx == 0) 
        {
            var url = "login?return_url=" + window.location.href;
            location.href = url;
            //$.modalView(url, 660, 500);
            return;
        }

        $('#fileupload').click();
    }

    function checkFileStep(step_num)
    {
        if (g_user_idx == 0)
        {
            var url = "login?return_url=" + window.location.href;
            location.href = url;
            //$.modalView(url, 660, 500);
            return;
        }

        $('#fileupload_' + step_num).click();
    }

    function checkFileStepMulti()
    {
        if (g_user_idx == 0)
        {
            var url = "login?return_url=" + window.location.href;
            location.href = url;
            //$.modalView(url, 660, 500);
            return;
        }

        $('#fileupload_multi').click();
    }

</script>
    </form>
</body>
</html>
