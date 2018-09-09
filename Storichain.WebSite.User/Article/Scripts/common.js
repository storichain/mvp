$(function() {
	$(".btn_ham").click(function(){ 
		$(".layer_mask").css('display','block');				
		$(".asdie").css('left','0px');					
		$(".asdie").css('height',$(window).height()+'px');
	});
	$(".layer_mask").click(function(){ 
		$(".asdie").css('left','-290px');					
		$(".asdie").css('height',$(window).height()+'px');
		$(".layer_mask").css('display','none');	
	});
});