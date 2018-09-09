
$(document).ready(function () {
    $.extend({
        modalView: function (url, width, height, scroll, defaultIframe) {

            var oldBodyMarginRight = $("body").css("margin-right");

            if(scroll == null)
                scroll = 'no';

            if(defaultIframe == null)
                defaultIframe = true;

            $.modal('<iframe id="popIframe" src="' + url + '" width="100%" height="' + height + '" scrolling="' + scroll + '" frameborder="0" frameborder="0" allowtransparency="true">', {
                containerCss: {
                    backgroundColor: "#fff",
                    borderColor: "#fff",
                    width: width,
                    padding: 0,
                    align: "center",
                    overflow: "hidden"
                },
                overlayCss: { backgroundColor: "black" },
                overlayClose: true,
                onShow: function () {
                    if(defaultIframe == false)
                        parent.$("html,body").css("overflow", "hidden");

                    $(".simplemodal-wrap").css("overflow", "hidden");
                    $("#simplemodal-container").css({'width':width,'height':height,'margin':'auto', 'top':(($(window).height()-height)/2.0) + 'px'});

                    $("html,body").css("overflow", "hidden");
                    $(".simplemodal-close").css("right", "30px");
                    
                },
                onOpen: function (dialog) {
                    dialog.overlay.fadeIn('slow', function () {
                    });
                    dialog.data.hide();
                    dialog.container.fadeIn('slow', function () {
                        dialog.data.slideDown('slow');
                    });
                    
                },
                onClose: function () {
                    if(defaultIframe)
                        $("html,body").css("overflow", "auto");
                    else
                        parent.$("html,body").css("overflow", "auto");

                    $.modal.close();
                }
            });
        },
        modalLayer: function (div, width, height, defaultIframe) {
            if(defaultIframe == null)
                defaultIframe = true;

            div.modal({
                containerCss: {
                    backgroundColor: "#fff",
                    borderColor: "#fff",
                    width: width,
                    padding: 0,
                    align: "center",
                    overflow: "hidden"
                },
                overlayCss: { backgroundColor: "black" },
                overlayClose: true,
                onShow: function () {
                    $("#simplemodal-wrap").css("overflow", "hidden");
                    $("#simplemodal-container").css({'width':width,'height':height,'margin':'auto', 'top':(($(window).height()-height)/2.0) + 'px'});
                    $("html,body").css("overflow", "hidden");
                },
                onOpen: function (dialog) {
                    dialog.overlay.fadeIn('slow', function () {
                    });
                    dialog.data.hide();
                    dialog.container.fadeIn('slow', function () {
                        dialog.data.slideDown('slow');
                    });
                    
                },
                onClose: function () {
                    if(defaultIframe)
                        $("html,body").css("overflow", "auto");

                    $.modal.close();
                    
                }
            });
        },
        requests: function(){
            var vars = [], hash;
            var hashes = window.location.href.slice(window.location.href.indexOf('?') + 1).split('&');
            for(var i = 0; i < hashes.length; i++)
            {
                hash = hashes[i].split('=');
                vars.push(hash[0]);
                vars[hash[0]] = hash[1];
            }
            return vars;
        },
        request: function(name) {
            if($.requests()[name] == null || $.requests()[name] == undefined)
                return '';
            
            return $.requests()[name];
        },
        cookieList: function (cookieName) {

            return {
                add: function (val) {
                    var items = this.items();

                    var index = items.indexOf(val);

                    // Note: Add only unique values.
                    if (index == -1) {
                        items.push(val);
                        $.cookie(cookieName, items.join(','), { expires: 7, path: '/' });
                    }
                },
                remove: function (val) {
                    var items = this.items();

                    var index = items.indexOf(val);

                    if (index != -1) {
                        items.splice(index, 1);
                        $.cookie(cookieName, items.join(','), { expires: 7, path: '/' });
                    }
                },
                indexOf: function (val) {
                    return this.items().indexOf(val);
                },
                clear: function () {
                    $.cookie(cookieName, null, { expires: 7, path: '/' });
                },
                items: function () {
                    var cookie = $.cookie(cookieName);

                    return cookie ? cookie.split(',') : []; ;
                    //return cookie ? eval("([" + cookie + "])") : []; ;
                },
                length: function () {
                    return this.items().length;
                },
                join: function (separator) {
                    return this.items().join(separator);
                }
            };
        },
        
        doQuickList: function () {
            var div = $("#quickList");
            var cookieLinkList = $.cookieList("marketLink");
            var cookieImgList = $.cookieList("marketImg");
            var linkList = cookieLinkList.items();
            var imgList = cookieImgList.items();
            
            div.empty();
            if(linkList.length == 0)
            {
                div.remove();
                $("#quickZero").addClass("click_item_list");
                $("#quickZero").append("<p>최근 본 상품이 없습니다.</p>");
                $("#quickZero").attr("id", "quickList");
                $("#quickList").css("height", "30px");
                $("#quickList > p").show();
                $("#cp_prev").hide();
                $("#cp_next").hide();
                //div.append("<p>최근 본 상품이 없습니다.</p>");
            }
            else
            {
                $("#cp_prev").show();
                $("#cp_next").show();
                $("#quickList").css("height", "190px");
                var j = 0;
                if((linkList.length - 1) > 7)
                {
                    for(var i = 8 ; i < linkList.length ; i++){
                        cookieLinkList.remove(linkList[0]);
                        cookieImgList.remove(imgList[0]);
                    }
                    j = (linkList.length - 1) - 7;
                }

                div.append('<ul class="click_items" id="quickUl"></ul>');
                for(var i = linkList.length - 1 ; i >= j ; i--){

                    var str = stringFormat(
                        '<li>' +
                            '<a href="/products/{0}">' +
                                '<img alt="" src="{1}" width="80" height="80"/>' +
                            '</a>' +
                        '</li>' 
                        , linkList[i], imgList[i]
                        );

                    $("#quickUl").append(str);
                }
                click_items_jcarousel.jcarousel('reload');
            }
            
            //return false;
        }

    });

});

(function($){
    $.fn.extend({
        center: function () {
            return this.each(function() {
                var top = ($(window).height() - $(this).outerHeight()) / 2;
                var left = ($(window).width() - $(this).outerWidth()) / 2;
                $(this).css({position:'absolute', margin:0, top: (top > 0 ? top : 0)+'px', left: (left > 0 ? left : 0)+'px'});
            });
        }
    }); 
})(jQuery);

//fgnass.github.com/spin.js#v1.3.3
!function(a,b){"object"==typeof exports?module.exports=b():"function"==typeof define&&define.amd?define(b):a.Spinner=b()}(this,function(){"use strict";function a(a,b){var c,d=document.createElement(a||"div");for(c in b)d[c]=b[c];return d}function b(a){for(var b=1,c=arguments.length;c>b;b++)a.appendChild(arguments[b]);return a}function c(a,b,c,d){var e=["opacity",b,~~(100*a),c,d].join("-"),f=.01+c/d*100,g=Math.max(1-(1-a)/b*(100-f),a),h=j.substring(0,j.indexOf("Animation")).toLowerCase(),i=h&&"-"+h+"-"||"";return l[e]||(m.insertRule("@"+i+"keyframes "+e+"{0%{opacity:"+g+"}"+f+"%{opacity:"+a+"}"+(f+.01)+"%{opacity:1}"+(f+b)%100+"%{opacity:"+a+"}100%{opacity:"+g+"}}",m.cssRules.length),l[e]=1),e}function d(a,b){var c,d,e=a.style;for(b=b.charAt(0).toUpperCase()+b.slice(1),d=0;d<k.length;d++)if(c=k[d]+b,void 0!==e[c])return c;return void 0!==e[b]?b:void 0}function e(a,b){for(var c in b)a.style[d(a,c)||c]=b[c];return a}function f(a){for(var b=1;b<arguments.length;b++){var c=arguments[b];for(var d in c)void 0===a[d]&&(a[d]=c[d])}return a}function g(a,b){return"string"==typeof a?a:a[b%a.length]}function h(a){this.opts=f(a||{},h.defaults,n)}function i(){function c(b,c){return a("<"+b+' xmlns="urn:schemas-microsoft.com:vml" class="spin-vml">',c)}m.addRule(".spin-vml","behavior:url(#default#VML)"),h.prototype.lines=function(a,d){function f(){return e(c("group",{coordsize:k+" "+k,coordorigin:-j+" "+-j}),{width:k,height:k})}function h(a,h,i){b(m,b(e(f(),{rotation:360/d.lines*a+"deg",left:~~h}),b(e(c("roundrect",{arcsize:d.corners}),{width:j,height:d.width,left:d.radius,top:-d.width>>1,filter:i}),c("fill",{color:g(d.color,a),opacity:d.opacity}),c("stroke",{opacity:0}))))}var i,j=d.length+d.width,k=2*j,l=2*-(d.width+d.length)+"px",m=e(f(),{position:"absolute",top:l,left:l});if(d.shadow)for(i=1;i<=d.lines;i++)h(i,-2,"progid:DXImageTransform.Microsoft.Blur(pixelradius=2,makeshadow=1,shadowopacity=.3)");for(i=1;i<=d.lines;i++)h(i);return b(a,m)},h.prototype.opacity=function(a,b,c,d){var e=a.firstChild;d=d.shadow&&d.lines||0,e&&b+d<e.childNodes.length&&(e=e.childNodes[b+d],e=e&&e.firstChild,e=e&&e.firstChild,e&&(e.opacity=c))}}var j,k=["webkit","Moz","ms","O"],l={},m=function(){var c=a("style",{type:"text/css"});return b(document.getElementsByTagName("head")[0],c),c.sheet||c.styleSheet}(),n={lines:12,length:7,width:5,radius:10,rotate:0,corners:1,color:"#000",direction:1,speed:1,trail:100,opacity:.25,fps:20,zIndex:2e9,className:"spinner",top:"50%",left:"50%",position:"absolute"};h.defaults={},f(h.prototype,{spin:function(b){this.stop();{var c=this,d=c.opts,f=c.el=e(a(0,{className:d.className}),{position:d.position,width:0,zIndex:d.zIndex});d.radius+d.length+d.width}if(b&&(b.insertBefore(f,b.firstChild||null),e(f,{left:d.left,top:d.top})),f.setAttribute("role","progressbar"),c.lines(f,c.opts),!j){var g,h=0,i=(d.lines-1)*(1-d.direction)/2,k=d.fps,l=k/d.speed,m=(1-d.opacity)/(l*d.trail/100),n=l/d.lines;!function o(){h++;for(var a=0;a<d.lines;a++)g=Math.max(1-(h+(d.lines-a)*n)%l*m,d.opacity),c.opacity(f,a*d.direction+i,g,d);c.timeout=c.el&&setTimeout(o,~~(1e3/k))}()}return c},stop:function(){var a=this.el;return a&&(clearTimeout(this.timeout),a.parentNode&&a.parentNode.removeChild(a),this.el=void 0),this},lines:function(d,f){function h(b,c){return e(a(),{position:"absolute",width:f.length+f.width+"px",height:f.width+"px",background:b,boxShadow:c,transformOrigin:"left",transform:"rotate("+~~(360/f.lines*k+f.rotate)+"deg) translate("+f.radius+"px,0)",borderRadius:(f.corners*f.width>>1)+"px"})}for(var i,k=0,l=(f.lines-1)*(1-f.direction)/2;k<f.lines;k++)i=e(a(),{position:"absolute",top:1+~(f.width/2)+"px",transform:f.hwaccel?"translate3d(0,0,0)":"",opacity:f.opacity,animation:j&&c(f.opacity,f.trail,l+k*f.direction,f.lines)+" "+1/f.speed+"s linear infinite"}),f.shadow&&b(i,e(h("#000","0 0 4px #000"),{top:"2px"})),b(d,b(i,h(g(f.color,k),"0 0 1px rgba(0,0,0,.1)")));return d},opacity:function(a,b,c){b<a.childNodes.length&&(a.childNodes[b].style.opacity=c)}});var o=e(a("group"),{behavior:"url(#default#VML)"});return!d(o,"transform")&&o.adj?i():j=d(o,"animation"),h});
var g_spin_common = {lines: 13,length: 15,width: 9,radius: 25,corners: 1,rotate: 0,direction: 1,color: '#000',speed: 1,trail: 60,shadow: false,hwaccel: false,className: 'spinner',zIndex: 2e9,top: 'auto',left: 'auto'};

/*! Lazy Load 1.9.3 - MIT license - Copyright 2010-2013 Mika Tuupola */
!function(a,b,c,d){var e=a(b);a.fn.lazyload=function(f){function g(){var b=0;i.each(function(){var c=a(this);if(!j.skip_invisible||c.is(":visible"))if(a.abovethetop(this,j)||a.leftofbegin(this,j));else if(a.belowthefold(this,j)||a.rightoffold(this,j)){if(++b>j.failure_limit)return!1}else c.trigger("appear"),b=0})}var h,i=this,j={threshold:0,failure_limit:0,event:"scroll",effect:"show",container:b,data_attribute:"original",skip_invisible:!0,appear:null,load:null,placeholder:"data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsQAAA7EAZUrDhsAAAANSURBVBhXYzh8+PB/AAffA0nNPuCLAAAAAElFTkSuQmCC"};return f&&(d!==f.failurelimit&&(f.failure_limit=f.failurelimit,delete f.failurelimit),d!==f.effectspeed&&(f.effect_speed=f.effectspeed,delete f.effectspeed),a.extend(j,f)),h=j.container===d||j.container===b?e:a(j.container),0===j.event.indexOf("scroll")&&h.bind(j.event,function(){return g()}),this.each(function(){var b=this,c=a(b);b.loaded=!1,(c.attr("src")===d||c.attr("src")===!1)&&c.is("img")&&c.attr("src",j.placeholder),c.one("appear",function(){if(!this.loaded){if(j.appear){var d=i.length;j.appear.call(b,d,j)}a("<img />").bind("load",function(){var d=c.attr("data-"+j.data_attribute);c.hide(),c.is("img")?c.attr("src",d):c.css("background-image","url('"+d+"')"),c[j.effect](j.effect_speed),b.loaded=!0;var e=a.grep(i,function(a){return!a.loaded});if(i=a(e),j.load){var f=i.length;j.load.call(b,f,j)}}).attr("src",c.attr("data-"+j.data_attribute))}}),0!==j.event.indexOf("scroll")&&c.bind(j.event,function(){b.loaded||c.trigger("appear")})}),e.bind("resize",function(){g()}),/(?:iphone|ipod|ipad).*os 5/gi.test(navigator.appVersion)&&e.bind("pageshow",function(b){b.originalEvent&&b.originalEvent.persisted&&i.each(function(){a(this).trigger("appear")})}),a(c).ready(function(){g()}),this},a.belowthefold=function(c,f){var g;return g=f.container===d||f.container===b?(b.innerHeight?b.innerHeight:e.height())+e.scrollTop():a(f.container).offset().top+a(f.container).height(),g<=a(c).offset().top-f.threshold},a.rightoffold=function(c,f){var g;return g=f.container===d||f.container===b?e.width()+e.scrollLeft():a(f.container).offset().left+a(f.container).width(),g<=a(c).offset().left-f.threshold},a.abovethetop=function(c,f){var g;return g=f.container===d||f.container===b?e.scrollTop():a(f.container).offset().top,g>=a(c).offset().top+f.threshold+a(c).height()},a.leftofbegin=function(c,f){var g;return g=f.container===d||f.container===b?e.scrollLeft():a(f.container).offset().left,g>=a(c).offset().left+f.threshold+a(c).width()},a.inviewport=function(b,c){return!(a.rightoffold(b,c)||a.leftofbegin(b,c)||a.belowthefold(b,c)||a.abovethetop(b,c))},a.extend(a.expr[":"],{"below-the-fold":function(b){return a.belowthefold(b,{threshold:0})},"above-the-top":function(b){return!a.belowthefold(b,{threshold:0})},"right-of-screen":function(b){return a.rightoffold(b,{threshold:0})},"left-of-screen":function(b){return!a.rightoffold(b,{threshold:0})},"in-viewport":function(b){return a.inviewport(b,{threshold:0})},"above-the-fold":function(b){return!a.belowthefold(b,{threshold:0})},"right-of-fold":function(b){return a.rightoffold(b,{threshold:0})},"left-of-fold":function(b){return!a.rightoffold(b,{threshold:0})}})}(jQuery,window,document);


function addBlackList(user_idx1, from_user_idx) 
{
    var url = stringFormat("/pages/popup/BlacklistAdd?user_idx={0}&seller_idx={1}", user_idx1, from_user_idx);
    $.modalView(url, 790, 569, true);
}


//spinner height
var spinnerHeight = function (scrollTop, windowWidth, windowHeight) {
    
    var left = Math.floor(windowWidth/2);
    var top = Math.floor((scrollTop + windowHeight/2) - 20);

    return {'left': left + 'px', 'top': top + 'px'};
};

//string.Format
var stringFormat = function (str, col) {

    col = typeof col === 'object' ? col : Array.prototype.slice.call(arguments, 1);

    return str.replace(/\{\{|\}\}|\{(\w+)\}/g, function (m, n) {
        if (m == "{{") { return "{"; }
        if (m == "}}") { return "}"; }

        return col[n];
    });
};

var imgUrl = function (item_data, column_name, key, default_image_path) {
    
    if (!item_data.hasOwnProperty(column_name + '_image_count'))
        return '/Images/etc/product_blank.jpg';
    if (!item_data.hasOwnProperty(column_name + '_images'))
        return '/Images/etc/product_blank.jpg';

    if(Number(item_data[column_name + '_image_count']) > 0)
    {
        var img_data = item_data[column_name + '_images'];

        if(key == null)
            return img_data[0].image_url;
        
        for(i = 0; i < img_data.length; i++) 
        {
            var item_img_data = img_data[i];

            if(item_img_data.image_key == key)
                return item_img_data.image_url;
        }

        return img_data[0].image_url;
    }

    if(default_image_path != null)
        return default_image_path;

    if(column_name == "user")
        return '/Images/etc/user_blank_s.jpg';
    else if(column_name == "comment")
        return '';
    else if(column_name == "product" || column_name == "product_small" || column_name == "product_thumb")
        return '/Images/etc/product_blank.jpg';
    
    return '';
    
    //return "";
};

var imgResize = function (item_data, column_name, key, oriWidth, oriHeight) {

    if (!item_data.hasOwnProperty(column_name + '_image_count'))
        return '';
    if (!item_data.hasOwnProperty(column_name + '_images'))
        return '';

    if (Number(item_data[column_name + '_image_count']) > 0) {
        var img_data = item_data[column_name + '_images'];

        if (key == null) {
            if (oriWidth != null && oriHeight != null) {

                if (((oriWidth / oriHeight) < img_data[0].image_width / img_data[0].image_height)) {
                    return (img_data[0].image_width > img_data[0].image_height) ? 'width:auto;height:100%' : '';
                } else {
                    return (img_data[0].image_width > img_data[0].image_height) ? 'width:100%;height:auto' : '';
                }

            } else {
                return (img_data[0].image_width > img_data[0].image_height)?'width:auto;height:100%':'';
            }
        }

        for (i = 0; i < img_data.length; i++) {
            var item_img_data = img_data[i];

            if (item_img_data.image_key == key) {

                if (oriWidth != null && oriHeight != null) {

                    if (((oriWidth / oriHeight) < item_img_data.image_width / item_img_data.image_height)) {
                        return (item_img_data.image_width > item_img_data.image_height) ? 'width:auto;height:100%' : '';
                    } else {
                        return (item_img_data.image_width > item_img_data.image_height) ? 'width:100%;height:auto' : '';
                    }

                } else {
                    return (item_img_data.image_width > item_img_data.image_height) ? 'width:auto;height:100%' : '';
                }
                
            }
        }

        if (oriWidth != null && oriHeight != null) {

            if (((oriWidth / oriHeight) < img_data[0].image_width / img_data[0].image_height)) {
                return (img_data[0].image_width > img_data[0].image_height) ? 'width:auto;height:100%' : '';
            } else {
                return (img_data[0].image_width > img_data[0].image_height) ? 'width:100%;height:auto' : '';
            }

        } else {
            return (img_data[0].image_width > img_data[0].image_height) ? 'width:auto;height:100%' : '';;
        }

    }

    if (column_name == "user")
        return '';
    else if (column_name == "comment")
        return '';
    else if (column_name == "product" || column_name == "product_small" || column_name == "product_thumb")
        return '';

    return '';

};

var imgIdx = function (item_data, column_name) {
    
    if (!item_data.hasOwnProperty(column_name + '_image_count'))
        return 0;
    if (!item_data.hasOwnProperty(column_name + '_images'))
        return 0;

    if(Number(item_data[column_name + '_image_count']) > 0)
    {
        var img_data = item_data[column_name + '_images'];
        return img_data[0].image_idx;
    }

    return 0;
};

var uploadImgUrl = function (item_data, key, default_image_path) {

    if(item_data == null)
        return '/Images/etc/product_blank.jpg';
    
    if(item_data.length > 0)
    {
        if(key == null)
            return item_data[0].image_url;
        
        for(i = 0; i < item_data.length; i++) 
        {
            var item_img_data = item_data[i];

            if(item_img_data.image_key == key)
                return item_img_data.image_url;
        }

        return item_data[0].image_url;
    }

    if(default_image_path != null)
        return default_image_path;

    return '/Images/etc/product_blank.jpg';
};

var validate = function (evt) {
        var theEvent = evt || window.event;
        var key = theEvent.keyCode || theEvent.which;
        key = String.fromCharCode( key );
        var regex = /^[\d\-\b]*$/g;
        if( !regex.test(key) ) {
        theEvent.returnValue = false;
        if(theEvent.preventDefault) theEvent.preventDefault();
        }
    }


// 클릭수 콤마
var clickCountFormat = function (n) {

	if (n >= 1000) {
		var mok = (n / 1000);
		var na = n.substring(1, 1);

		return mok.toString() + '.' + na + 'k';
	}
	else {
		return priceFormat(n);
	}
	
};

// 숫자 콤마
var priceFormat = function (n) {
    var reg = /(^[+-]?\d+)(\d{3})/;
    n += '';
    while (reg.test(n)) {
        n = n.replace(reg, '$1' + ',' + '$2');
    }
    return n;
};

// 말줄임
var stringShort = function (str, cnt) {
    return str.substring(0, cnt) + "...";
};

// UUID 만들기
var stringUUID = function () {
    var d = new Date().getTime();
    var str = 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function(c) {
        var r = (d + Math.random()*16)%16 | 0;
        d = Math.floor(d/16);
        return (c=='x' ? r : (r&0x7|0x8)).toString(16);
    });
    return str;
};

var dateHyphen = function (date, spliter) 
{
    date = date.trim();

    if(date.length >= 8) 
    {
        return date.substring(0, 4) + spliter + date.substring(4, 6) + spliter + date.substring(6, 8);
    }

    return date;
}

String.prototype.trim = function () { return this.replace(/^\s*/ ,"").replace(/\s*$/ ,""); }

String.prototype.replaceEnterAndSpace = function () {

	var re = /\r/g;
	var value = this.replace(re,"");
	re = /\n/g;
	value = value.replace(re, "<br/>");
	re = /\s/g;
	return value.replace(re, " ");
}

String.prototype.replaceEnterToBr = function()
{
	var re = /\r/g;
	var value = this.replace(re, "");
	re = /\n/g;
	return value.replace(re, "<br/>");
}


String.prototype.replaceBrToEnter = function () {
	var re = /<br\>/g;
	var value = this.replace(re, "");
	re = /<br>/g;
	return value.replace(re, "");
}


String.prototype.replaceBlankTo = function (val) {
	var re = /\s/g;
	return this.replace(re, val);
}


var nowDate = function () {

    var now = new Date();
    var year = now.getFullYear();
    var mon = (now.getMonth() + 1) > 9 ? '' + (now.getMonth() + 1) : '0' + (now.getMonth() + 1);
    var day = now.getDate() > 9 ? '' + now.getDate() : '0' + now.getDate();

    var today = year + '-' + mon + '-' + day;

    return today;
}

/** 
* string String::cutByte(int len)
* 글자를 앞에서부터 원하는 바이트만큼 잘라 리턴합니다.
* 한글의 경우 2바이트로 계산하며, 글자 중간에서 잘리지 않습니다.
*/
String.prototype.cutByte = function (len, ellipseCount) {
    var str = this;
    var count = 0;

    for (var i = 0; i < str.length; i++) {
        if (escape(str.charAt(i)).length >= 4)
            count += 2;
        else
            if (escape(str.charAt(i)) != "%0D")
                count++;


        if (count > len) {
            if (escape(str.charAt(i)) == "%0A")
                i--;
            break;
            return str.substring(0, i);
        }
    }

    var ellipse = "";
    if (ellipseCount) {
        for (var i = 0; i < ellipseCount ; i++) {
            ellipse += " .";
        }
    }
    else {
        ellipse = "...";
    }
        

    return str.substring(0, i) + ellipse;
}

/* bool String::byte(void)
/* 해당스트링의 바이트단위 길이를 리턴합니다. (기존의 length 속성은 2바이트 문자를 한글자로 간주합니다)
*/
String.prototype.byte = function () {
    var str = this;
    var length = 0;
    for (var i = 0; i < str.length; i++) {
        if (escape(str.charAt(i)).length >= 4)
            length += 2;
        else if (escape(str.charAt(i)) == "%A7")
            length += 2;
        else
            if (escape(str.charAt(i)) != "%0D")
                length++;
    }
    return length;
}










function ajaxError(request, type, errorThrown) {
    var message = "There was an error with the AJAX request.\n";
    switch (type) {
        case 'timeout':
            message += "The request timed out.";
            break;
        case 'notmodified':
            message += "The request was not modified but was not retrieved from the cache.";
            break;
        case 'parseerror':
            message += "XML/Json format is bad.";
            break;
        default:
            message += "HTTP Error (" + request.status + " " + request.statusText + ").";
    }
    message += "\n";
    //alert(message);
}


function campPaging(total_row_count, page_count, currentPage, eventName, pageRows) {

    var lastPage = 0;
    if (! (pageRows == null)) 
    {
        lastPage = parseInt((total_row_count/pageRows)) + 1;
    }
    
	var startCnt = 1;
	var pageCnt = page_count;

	var prevPage = currentPage * 1 - 1;
	var nextPage = currentPage * 1 + 1;

	if (page_count >= 5) {
		pageCnt = "5";

        // 처음 3개 까진..
		if (currentPage > 3) {
			
            if(lastPage != 0)
            {
                if((lastPage - currentPage) == 1)
                {
                    startCnt = currentPage * 1 - 3;
                    pageCnt = currentPage * 1 + 1;
                }
                else if((lastPage - currentPage) == 0)
                {
                    startCnt = currentPage * 1 - 4;
                    pageCnt = currentPage;
                }
                else
                {
                    startCnt = currentPage * 1 - 2;
			        pageCnt = currentPage * 1 + 2;
                }
            }
            else
            {
                startCnt = currentPage * 1 - 2;
			    pageCnt = currentPage * 1 + 2;
            }

		}
		else 
        {
		    if (currentPage == page_count) 
            {
		        startCnt = currentPage * 1 - 2;
		        pageCnt = currentPage;
		    }
		    else if(currentPage == 1)
            {
                pageCnt = currentPage * 1 + 4;
		    }
            else if(currentPage == 2)
            {
                pageCnt = currentPage * 1 + 3;
		    }else{
                pageCnt = currentPage * 1 + 2;
            }
		}

        // 맨 끝으로..
		
	}

	if (currentPage == page_count) {
		//prevPage = currentPage;
		nextPage = currentPage;
	}

	if (currentPage == 1) {
		prevPage = 1;
	}

	if (!eventName) {
	    eventName = "doPaging";
	}

	var pages = [];

	//alert(startCnt);
	//alert(pageCnt);
    
	for (var k = startCnt; k <= pageCnt; k++) {
            
        if (currentPage == k) {
			pages.push(stringFormat('<span class="blind">현재페이지</span><strong>{0}</strong>', k));
		}
		else 
        {
		    pages.push(stringFormat('<a href="javascript:void(0);" onclick="{1}({0})">{0}</a>', k, eventName));
		}            
            
	}

	//alert(page_count);
	var tag = '<a href="javascript:void(0);" class="pbtn pbtn_prev" onclick="{4}({0})"><span>이전</span></a>'
			+ '{3}'
			+ '<a href="javascript:void(0);" class="pbtn pbtn_next" onclick="{4}({1})"><span>다음</span></a>';

	return stringFormat(tag, prevPage, nextPage, page_count, pages.join(''), eventName);
}



var g_spin_common = { lines: 13, length: 15, width: 9, radius: 25, corners: 1, rotate: 0, direction: 1, color: '#000', speed: 1, trail: 60, shadow: false, hwaccel: false, className: 'spinner', zIndex: 2e9, top: 'auto', left: 'auto' };

