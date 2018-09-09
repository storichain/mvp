article_idx = 0;
currentScene = null;
currentRole = null;
textShowTime = 300; // 300
textGapTime = 1900;  // 900
scrollTime = 500;   // 500
scenePlay = true;
totalSceneCount = 0;
currentSceneCount = 0;


$(document).ready(function () {
    scenePlay = $.request('autoplay') != 'false';
    //article_idx = $.request('article_dix');
    totalSceneCount = $('.scene').length;

    $('button.kakao, button.facebook, button.story').hide();
    /*=======topbar 애니메이션/끼어들기 ON/OFF효과==========*/
    $('button.btn_back').click(function () {
        location.href = "list";
    });
    $('.animation').on('click', function () {
        //$('.animation').toggleClass('on');
        if (scenePlay) {
            location.href = "StoriTextShow?autoplay=false";
        } else {
            location.href = "StoriTextShow?autoplay=true";

        }
    });
    $('.cut-in-msg').on('click', function () {
        $('.cut-in-msg').toggleClass('on');
    });
    /*=======topbar 제목텍스트 클릭시 full text show==========*/
    $('h2>button').on('click', function () {
        $('.full_tit').toggleClass('show');
    });
    $('.full_tit').on('click', function () {
        $('.full_tit').removeClass('show');
    });
    /*
    $('.cutin_msg').find('.noti > button').on('click', function(){
        $('.cutin_msg').toggleClass('show');
    });
    */
    /*=======끼어들기 메세지 펼치기/접기==========*/
    $('.cutin_msg').find('.noti > button, .foldin').on('click', function () {
        //                    if($('.cutin_msg').hasClass('show')){
        var cutinMsg = $(this).closest('.cutin_msg');
        var msgWrap = cutinMsg.find('.msg_wrap');
        console.log(msgWrap);
        if (cutinMsg.hasClass('show')) {
            msgWrap.slideUp('fast');
            cutinMsg.removeClass('show');
        }
        else {
            msgWrap.slideDown();
            cutinMsg.addClass('show');
            scrollTo(msgWrap);
        }
    });
    /*--======share 버튼 show=====*/
    $('.share').on('click', function () {
        $('.sns').slideDown('fast');
        $('.share').hide('fast');
        $('.close').show('fast');
    });
    $('.close').on('click', function () {
        $('.sns').slideUp('fast');
        $('.share').show('fast');
        $('.close').hide('fast');
    });
    $("button.link").click(function () {
        console.log("link clicked");
        var url = location.href;
        console.log(url);
        $("#thisLocation").val(url);
        console.log($("#thisLocation").val());
        // 클립보드
        var clipboard = new Clipboard('button.link');
        clipboard.on('success', function (e) {
            alert("복사되었습니다.");
            e.clearSelection();
            clipboard.destroy();
        });
        clipboard.on('error', function (e) {
            alert("복사에 실패하였습니다. -" + e.action);
        });
    });

    $('.yes').on('click', function () {
        console.log(currentScene);
        showNextScene(currentScene.next());
        $('.user_react').fadeOut(200);
    });

    $('button.no').on('click', function () {
        location.href = "list";
    });



    if (scenePlay) {
        showScene($('.scene:first-of-type'));
        $('.animation').addClass('on');
    } else {
        $(".scene,.cutin_msg,.role,.type,.type dd,.user_react").show();
        $(".user_react .btn_area").hide();
        $('.animation').removeClass('on');
    }


//    $(document).ready(function () {
//
//        $.ajax({
//            type: "Post",
//            url: "/Comment/Get",
//            data: stringFormat("user_idx={0}", article_idx),
//            dataType: "json",
//            cache: false,
//            success: function (response) {
//                if (response.response_code == 1000) {
//
//                }
//
//            },
//            error: function (e) {
//                alert("error");
//            }
//        });
//        if (scenePlay) {
//            showScene($('.scene:first-of-type'));
//            $('.animation').addClass('on');
//        } else {
//            $(".scene,.cutin_msg,.role,.type,.type dd,.user_react").show();
//            $('.animation').removeClass('on');
//        }
//    });

});

function addComment(comment) {
    var scene = comment.scene_idx;
    var commentText = comment.comment_text;
}

function showAfterScene(scene) {
    var element = scene;
    //element.nextUntil('.scene').show();
    if (totalSceneCount > currentSceneCount) {
        var user_react = $('.user_react')[(currentSceneCount - 1)];
        $(user_react).fadeIn(200);
        scrollTo($(user_react));
    }
}

function showScene(scene) {
    currentScene = scene;
    currentSceneCount++;
    scene.show();
    showRole(scene.children('.role').first());
}

function showNextScene(scene) {
    if (!scene.is('.scene')) {
        showNextScene(scene.next());
    } else {
        showScene(scene);
    }
}



function showRole(role) {
    var dl = role.children().first();
    var text = dl.children().first();
    //console.log("-----");
    //console.log(role);
    //console.log(dl);
    //console.log(text);
    //console.log("-----");
    dl.show();
    showText(text);
    //                
}

function showText(text) {

    if (text.is('dt')) {
        text.fadeIn(textShowTime);
        text = text.next();
    }

    text.fadeIn(textShowTime);
    scrollTo(text);
    setTimeout(function () {
        if (text.next().length != 0) {
            showText(text.next());
        } else if (text.parents('.role').next().length != 0) {
            showRole(text.parents('.role').next());
        } else {
            showAfterScene(currentScene);
        }
    }, textGapTime);

}

function scrollTo(element) {
    $('html, body').animate({
        scrollTop: element.offset().top
    }, scrollTime);
}