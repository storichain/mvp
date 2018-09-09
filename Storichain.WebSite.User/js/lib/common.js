$(function() {
    $('.btn_memu').on('click',function(){
       $('.aside').addClass('aside_show');
       $('.black_scrn').addClass('active');
    });
    $('.btn_close_aside').on('click',function(){
       $('.aside').removeClass('aside_show');
        $('.black_scrn').removeClass('active');
    });
});