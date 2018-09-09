// pay61_secuni_cross.js
// function : Merchant Script & 전자지갑의 Interface
// ⓒ 2006 INICIS.Co.,Ltd. All rights reserved.

var PLUGIN_SVR_NAME = "/wallet61/";
var PLUGIN_CLASSID = "<OBJECT ID=INIpay CLASSID=CLSID:24F6E6A8-852C-45A8-ADD3-C4AB0D6FD231 width=0 height=0 CODEBASE=http://plugin.inicis.com/wallet61/INIwallet61.cab#Version=1,0,0,1 onerror=OnErr()></OBJECT>";
var PLUGIN_ERRMSG = "고객님의 안전한 결제를 위하여 결제용 암호화 프로그램의 설치가 필요합니다.\n\n" +
							"다음 단계에 따라 진행하십시오.\n\n\n" +
							"1. 브라우저(인터넷 익스플로어) 상단 또는 하단의 노란색 알림 표시줄을 마우스로 클릭 하십시오.\n\n" +
							"2. 'ActiveX 컨트롤 설치'를 선택하십시오.\n\n" +
							"3. 보안 경고창이 나타나면 '설치'를 눌러서 진행하십시오.\n";
var INIFLASH_CLASSID = "<script src='http://plugin.inicis.com/inipayFlash/pay61_flash_uni_cross.js' language='javascript'></script>";
var JSINFO_NAME = "61_secuni_npapi";

var ini_browser_name = "IE";
var ini_IsWindows = false;
var ini_IsVistaAfter = false;
var ini_Is64Bit = false;
var ini_IsUseFlash = false;
var ini_IsNewVerOCX = false;

var NPPLUGIN_NANE = "INICIS INIpay Plugin";
var NPPLUGIN_VERSION = "1.0.0.3";
var NPPLUGIN_DESCRIPTION = "x-INIpay Plugin v.1.0.0.3";
var NPPLUGIN_MIMETYPE = "<embed id='INIpay' type='application/x-INIwallet61-INICIS' width=0 height=0></embed>";
var NPPLUGIN_INSTALLURL = "http://plugin.inicis.com/html60/npapi/install/install_plugins.html";

function StartSmartUpdate()
{
	SetEnvironment();
	if( ini_IsUseFlash == true )
	{
		document.write(INIFLASH_CLASSID);
	}
	else
	{
		if( ini_IsMSIE() == false )
		{
			if( ini_CheckPluginVer() == false )
				return false;
		}
		
		document.writeln(PLUGIN_CLASSID);
	}
}

function IsPluginModule()
{
	if( ini_IsUseFlash == true )
		return false;
	
	return true;
}

function MakePayMessage_Flash(payform)
{
	flashPay(payform);
}

function MakePayMessage(payform)
{
	if( ini_IsUseFlash == true )
	{
		MakePayMessage_Flash(payform);
	}
	else
	{
		var plugin_obj = GetPlugin();

		plugin_obj.IFplugin(100, "INIpay", "");  

		if( ini_IsVistaAfter == true )
		{
			if( ini_Is64Bit == true )
			{
				if(plugin_obj.IFplugin(0, PLUGIN_SVR_NAME, "inipay|00") == "ERROR") 
				return false;
			}
			else
			{
  				if(plugin_obj.IFplugin(0, PLUGIN_SVR_NAME, "inipay|10") == "ERROR") 
				return false;
			}
		}
		else
		{
			if(plugin_obj.IFplugin(0, PLUGIN_SVR_NAME, "inipay") == "ERROR") 
			return false;
		}

		if( ini_SetField(plugin_obj, payform) == false ) 
		{
			plugin_obj.IFplugin(1, "", "");
			return false;
		}
  
		if(plugin_obj.IFplugin(4, "", "") == "ERROR")
		{
			ini_GetErrField(plugin_obj, payform);
			plugin_obj.IFplugin(1, "", "");
			return false;
		}
  
		if( ini_GetField(plugin_obj, payform) == false ) 
		{
			plugin_obj.IFplugin(1, "", "");
			return false;
		}
	
		plugin_obj.IFplugin(1, "", "");
  
		return true;
	}	
}		


//Set Merchant Payment Field
function ini_SetField(plugin_obj, payform)
{
	var nField = payform.elements.length;
  
	for(i = 0; i < nField; i++)
	{
		if(payform.elements[i].name == "mid")
		{
			plugin_obj.IFplugin(2, "mid", payform.mid.value);
		}
		else if(payform.elements[i].name == "nointerest")
		{
			plugin_obj.IFplugin(2, "nointerest", payform.nointerest.value);
		}
		else if(payform.elements[i].name == "quotabase")
		{
			plugin_obj.IFplugin(2, "quotabase", payform.quotabase.value);
		}
		else if(payform.elements[i].name == "price")
		{
			plugin_obj.IFplugin(2, "price", payform.price.value);
		}
		else if(payform.elements[i].name == "currency")
		{
			plugin_obj.IFplugin(2, "currency", payform.currency.value);
		}
		else if(payform.elements[i].name == "buyername")
		{
			plugin_obj.IFplugin(2, "buyername", payform.buyername.value);
		}
		else if(payform.elements[i].name == "goodname")
		{
			plugin_obj.IFplugin(2, "goodname", payform.goodname.value);
		}
		else if(payform.elements[i].name == "acceptmethod")
		{
			plugin_obj.IFplugin(2, "acceptmethod", payform.acceptmethod.value);
		}
		else if(payform.elements[i].name == "gopaymethod")
		{
			if(payform.gopaymethod.value != "")
				plugin_obj.IFplugin(2, "gopaymethod", payform.gopaymethod.value); 
		}
		else if(payform.elements[i].name == "ini_encfield")
		{
			plugin_obj.IFplugin(2, "ini_encfield", payform.ini_encfield.value);
		}
		else if(payform.elements[i].name == "ini_certid")
		{
			plugin_obj.IFplugin(2, "ini_certid", payform.ini_certid.value);
		} 
		else if(payform.elements[i].name == "INIregno")
		{
			plugin_obj.IFplugin(2, "INIregno", payform.INIregno.value);
		}
		else if(payform.elements[i].name == "oid")
		{
			plugin_obj.IFplugin(2, "oid", payform.oid.value);
		}
		else if(payform.elements[i].name == "buyeremail")
		{
			plugin_obj.IFplugin(2, "buyeremail", payform.buyeremail.value);
		}
		else if(payform.elements[i].name == "ini_menuarea_url")
		{
			plugin_obj.IFplugin(2, "menuareaimage_url", payform.ini_menuarea_url.value);
		}
		else if(payform.elements[i].name == "ini_logoimage_url")
		{
			plugin_obj.IFplugin(2, "logoimage_url", payform.ini_logoimage_url.value);
		}
		else if(payform.elements[i].name == "ini_bgskin_url")
		{
			plugin_obj.IFplugin(2, "ini_bgskin_url", payform.ini_bgskin_url.value);
		}    
		else if(payform.elements[i].name == "mall_noint")
		{
			plugin_obj.IFplugin(2, "mall_noint", payform.mall_noint.value);
		}
		else if(payform.elements[i].name == "ini_onket_flag")
		{
			plugin_obj.IFplugin(2, "onket_flag", payform.ini_onket_flag.value);
		}
		else if(payform.elements[i].name == "ini_pin_flag")
		{
			plugin_obj.IFplugin(2, "pin_flag", payform.ini_pin_flag.value);
		}
		else if(payform.elements[i].name == "buyertel")
		{
			plugin_obj.IFplugin(2, "buyertel", payform.buyertel.value);
		}
		else if(payform.elements[i].name == "ini_escrow_dlv")
		{
			plugin_obj.IFplugin(2, "ini_escrow_dlv", payform.ini_escrow_dlv.value);
		}
		else if(payform.elements[i].name == "ansim_cardnumber")
		{
			plugin_obj.IFplugin(2, "ansim_cardnumber", payform.ansim_cardnumber.value);
		}
		else if(payform.elements[i].name == "ansim_expy")
		{
			plugin_obj.IFplugin(2, "ansim_expy", payform.ansim_expy.value);
		}
		else if(payform.elements[i].name == "ansim_expm")
		{
			plugin_obj.IFplugin(2, "ansim_expm", payform.ansim_expm.value);
		}
		else if(payform.elements[i].name == "ansim_quota")
		{
			plugin_obj.IFplugin(2, "ansim_quota", payform.ansim_quota.value);
		}
		else if(payform.elements[i].name == "ini_cardcode")
		{
			plugin_obj.IFplugin(2, "ini_cardcode", payform.ini_cardcode.value);
		}
		else if(payform.elements[i].name == "ini_onlycardcode")
		{
			plugin_obj.IFplugin(2, "ini_onlycardcode", payform.ini_onlycardcode.value);
		}
		else if(payform.elements[i].name == "ini_offer_period")
		{
			plugin_obj.IFplugin(2, "ini_offer_period", payform.ini_offer_period.value);
		}		
		else if(payform.elements[i].name == "ESCROW_LOGO_URL")
		{
			plugin_obj.IFplugin(2, "ESCROW_LOGO_URL", payform.ESCROW_LOGO_URL.value);
		}
		else if(payform.elements[i].name == "KVP_OACERT_INF") 
		{
			plugin_obj.IFplugin(2, "reserved6", payform.KVP_OACERT_INF.value);
		}
		else if(payform.elements[i].name == "ini_nm_comp")
		{
			plugin_obj.IFplugin(2, "ini_nm_comp", payform.ini_nm_comp.value);
		}
	}
	
	plugin_obj.IFplugin(2, "version", payform.version.value);
  	plugin_obj.IFplugin(2, "reqsign", payform.reqsign.value);

	var VISA3D_INF = "12:13:01:14:04:03:34:42:45:51:52:33";
	var ADD_KVP_FLAG = "ISP_CARD_INF=06:11&PUBCERT_FLAG=1001100110011001&PUBCERT_MSG=회원님의 안전한 전자상거래를 위해 공인인증서(금융결제원 발급)를 통한 본인인증이 필요합니다.\n회원님의 공인인증서를 선택하신후 해당 비밀번호를 입력하여 주시기 바랍니다.\n공인인증서가 없으신 경우, 금융결제원에서 발급하는 공인인증서를 신청하신후 이용해 주십시오.\n&PUBCERT_MSG2=공인 인증서를 사용하시겠습니까?&KMPAY=300000&BCPAY=300000&URIPAY=300000&CHOPAY=300000&PUBIMG_URL=http://plugin.inicis.com/wallet00/files/&VISA_MSG=국민이면서 비자일경우 3D화면으로 전환 됩니다.\n";
	var NORMAL_INF = "21:31:35:43";
	var KFTC_BANK_INFO = "04:11:20:23:03:05:07:88:27:31:32:34:35:37:39:81:71";
	var VISA3D_PUBCERT_PRICE = "300000";

	plugin_obj.IFplugin(2, "ADD_KVP_FLAG", ADD_KVP_FLAG);
	plugin_obj.IFplugin(2, "visa3d_inf", VISA3D_INF);
	plugin_obj.IFplugin(2, "visa3d_pubcert_price", VISA3D_PUBCERT_PRICE);
	plugin_obj.IFplugin(2, "NORMAL_INF", NORMAL_INF);
	plugin_obj.IFplugin(2, "reserved3", KFTC_BANK_INFO);
	plugin_obj.IFplugin(2, "plugin_jsinfo", JSINFO_NAME);    
	plugin_obj.IFplugin(2, "ini_browser_name", ini_browser_name);
  
	return true;
}	


//Get PayMessage made
function ini_GetField(plugin_obj, payform)
{
	var nField = payform.elements.length;

	if((payform.paymethod.value = plugin_obj.IFplugin(3, "paymethod", "")) == "ERROR")
	{
		return false;
	}
	if(payform.paymethod.value == "") 
	{
		return false;
	}
	if((payform.sessionkey.value = plugin_obj.IFplugin(3, "sessionkey", "")) == "ERROR") 
	{
		return false;
	}
	if(payform.sessionkey.value == "") 
	{
		return false;
	}
	if((payform.encrypted.value = plugin_obj.IFplugin(3, "encrypted", "")) == "ERROR") 
	{
		return false;
	}
	if(payform.encrypted.value == "") 
	{
		return false;
	}
	if((payform.uid.value = plugin_obj.IFplugin(3, "uid", "")) == "ERROR") 
	{
		return false;
	}
	if(payform.paymethod.value == "DirectBank")
	{
		if((payform.rbankcode.value = plugin_obj.IFplugin(3, "realbankcode", "")) == "ERROR") 
		{
			alert("error bankcode");
			return false;
		}
	}

	for(i = 0; i < nField; i++)
	{
		if(payform.elements[i].name == "cardcode")
		{
			payform.cardcode.value = plugin_obj.IFplugin(3, "cardcode", "");
		} 
		else if(payform.elements[i].name == "cardquota")
		{
			payform.cardquota.value = plugin_obj.IFplugin(3, "cardquota", "");
		}  	
		else if(payform.elements[i].name == "quotainterest")
		{
			payform.quotainterest.value = plugin_obj.IFplugin(3, "quotainterest", "");
		}
		else if(payform.elements[i].name == "buyeremail")
		{
			payform.buyeremail.value = plugin_obj.IFplugin(3, "buyeremail", "");
		}
		else
		{
			if(payform.paymethod.value == "VCard")
			{
				if(payform.elements[i].name == "ispcardcode")
					payform.ispcardcode.value = plugin_obj.IFplugin(3,"vcard_cardcode",""); 
				else if(payform.elements[i].name == "kvp_card_prefix")
					payform.kvp_card_prefix.value = plugin_obj.IFplugin(3,"vcard_prefix","");   	    
			}
		}  	
	}
	return true;
}

//Get PayMessage made
function ini_GetErrField(plugin_obj, payform)
{
	var nField = payform.elements.length;
	if((payform.paymethod.value = plugin_obj.IFplugin(3, "paymethod", "")) == "HPP") 
	{
		for(i = 0; i < nField; i++)
		{
			if(payform.elements[i].name == "INI_errmsg")
				payform.INI_errmsg.value = plugin_obj.IFplugin(3,"hpp_errmsg","");   	 		
		}
	}

	if( ini_IsFirefox() )
	{
		var strvalue = plugin_obj.IFplugin(3,"firefox_changeflag","");   
		if( strvalue == "true")
		{
			if( confirm("firefox 브라우저로 결제를 하시려면 이니페이 플러그인 설치가 필요합니다.\n\n\n[확인] 버튼을 누르시면 설치 프로그램을 다운로드합니다. \n이니페이 플러그인 설치 프로그램을 [저장]하시고 반드시 [실행]해 주십시오.\n설치시, firefox 브라우저는 자동종료됩니다.\n\n\n설치를 원하시지 않는 경우, [취소]버튼을 클릭해 주십시오."))
			{		
				document.location.href = "http://plugin.inicis.com/repair/INIpayInstall.exe";
			}
		}
	}
}

function ini_GetUserInfo()
{
	var strAgent = navigator.userAgent.toLowerCase();

	//== windows 여부 체크
	if (strAgent.indexOf("windows") != -1)
		ini_IsWindows = true;

	//== windows Vista이후 여부 체크
	if(strAgent.indexOf("windows nt 6") > -1)
	{
		ini_IsVistaAfter = true;
		ini_IsNewVerOCX = true;
				
		//== Share해결용 OCX 설치여부
		if(strAgent.indexOf("windows nt 6.1") > -1 || strAgent.indexOf("windows nt 6.0") > -1)  //-- Vista,Window7(IE10이하)은 예전 OCX 설치
			ini_IsNewVerOCX = false;
	}

	//== 64비트 체크
	var strAppVersion = window.navigator.appVersion.toLowerCase();
	if( strAppVersion.indexOf("win64") != -1 || strAppVersion.indexOf("wow64") != -1 ||  strAgent.indexOf("wow64") != -1 )
	{
		ini_Is64Bit = true;
	}
		
	//== 브라우저 정보 체크
	if (strAgent.indexOf("msie") != -1)
		ini_browser_name = "IE";
	else if (strAgent.indexOf("opera") != -1)
		ini_browser_name = "opera";
	else if (strAgent.indexOf("firefox") != -1)
		ini_browser_name = "firefox";
	else if (strAgent.indexOf("chrome") != -1)
		ini_browser_name = "chrome";
	else if (strAgent.indexOf("safari") != -1)
		ini_browser_name = "safari";
	else if (strAgent.indexOf("rv:1") != -1 && strAgent.indexOf("trident") != -1 )
		ini_browser_name = "IE";
	else
		ini_browser_name = "NONE";
		
	//== IE11 이후버전인지 체크 
	if (strAgent.indexOf("msie") != -1 || strAgent.indexOf("rv:1") != -1 )
	{
		var nIdx = strAgent.indexOf('trident/', 0);
		if ( nIdx >= 0 )
		{
			var strTriVer = strAgent.substring(nIdx + 8);
		
			var nTriVer = parseInt(strTriVer);
		
			if(  nTriVer >= 7 )
				ini_IsNewVerOCX = true;
		}
	}

}

function ini_IsMSIE()
{
	var strAgent = navigator.userAgent.toLowerCase();
	if (strAgent.indexOf("msie") != -1)
		return true;
	else if(strAgent.indexOf("rv:1") != -1 && strAgent.indexOf("trident") != -1 )
		return true;
				
	return false;
}

function ini_IsFirefox()
{
	var strAgent = navigator.userAgent.toLowerCase();
	if (strAgent.indexOf("firefox") != -1)
		return true;
	return false;
}

function ini_IsInstalledPlugin()
{
	if( ini_IsUseFlash == true )
		return true;

	var plugin_obj = GetPlugin();

	if( plugin_obj == null )
		return false;

	if( ini_IsMSIE() == true )
	{
		if( plugin_obj.object == null)
			return false;
	}
	return true;
} 

function GetPlugin()
{
	return document.getElementById("INIpay");
}

function ini_GetInstallFile()
{
	return NPPLUGIN_INSTALLURL;
}

function ini_CheckVersionUpdate(strDescription, strNewVer) 
{
	var index = strDescription.indexOf('v.', 0);
	if (index < 0) return true;
	
	var strOldVer = strDescription.substring(index + 2, strDescription.length);

	if (strOldVer.length <= 0) return true;

	var arrayOldVersion = strOldVer.split('.');
	var old_v1 = parseInt(arrayOldVersion[0], 10);
	var old_v2 = parseInt(arrayOldVersion[1], 10);
	var old_v3 = parseInt(arrayOldVersion[2], 10);
	var old_v4 = parseInt(arrayOldVersion[3], 10);

	var arrayNewVersion = strNewVer.split('.');
	var new_v1 = parseInt(arrayNewVersion[0], 10);
	var new_v2 = parseInt(arrayNewVersion[1], 10);
	var new_v3 = parseInt(arrayNewVersion[2], 10);
	var new_v4 = parseInt(arrayNewVersion[3], 10);

	if (old_v1 > new_v1) return false;
	if (old_v1 < new_v1) return true;
	if (old_v2 > new_v2) return false;
	if (old_v2 < new_v2) return true;
	if (old_v3 > new_v3) return false;
	if (old_v3 < new_v3) return true;
	if (old_v4 > new_v4) return false;
	if (old_v4 < new_v4) return true;

	return false;
}

function ini_CheckPluginVer()
{
	var instinfo = 0;
	for (var i = 0; i < navigator.plugins.length; i++)
	{
		if (navigator.plugins[i].name == NPPLUGIN_NANE )
		{
			if (navigator.plugins[i].description == NPPLUGIN_DESCRIPTION )
			{
				instinfo = 1;	
			}
			else
			{
				if( ini_CheckVersionUpdate(navigator.plugins[i].description, NPPLUGIN_VERSION) == false )
					instinfo = 1;	
				else 
					instinfo = 2;
			}
			break;
		}		
	}

	if (instinfo != 1)
	{
		if (instinfo == 0)
		{
			alert("Internet Exploer 이외의 환경에서 결제를 하시려면 \n\nNON-IE용 플러그인을 설치하셔야 합니다.\n\n\n확인 버튼을 누르시면, \n\n플러그인 설치 프로그램 다운로드창이 실행됩니다.\n\n\nINIpay 플러그인 설치후에는 \n\n반드시 브라우저를 재시작 하셔야 합니다.");
		}
		else if (instinfo == 2)
		{
			alert("INIpay 플러그인이 업데이트 되었습니다.\n\n\n확인 버튼을 누르시면, \n\n플러그인 설치 프로그램 다운로드창이 실행됩니다.\n\n\nINIpay 플러그인 설치후에는 \n\n반드시 브라우저를 재시작 하셔야 합니다.");
		}
	
		document.location.href = ini_GetInstallFile();
		return false;
	}
	
	return true;
}

function SetEnvironment()        
{
	//== PC환경 
	ini_GetUserInfo();

	//== windows가 아닌 경우
	if( ini_IsWindows == false )
	{
		ini_IsUseFlash = true;
		return;
	}

	//== Use OCX Plugin
	if( ini_IsMSIE() == true ) 
	{
		if( ini_IsVistaAfter == true )
		{
			if( ini_IsNewVerOCX == true )
				PLUGIN_CLASSID = "<OBJECT ID=INIpay CLASSID=CLSID:24F6E6A8-852C-45A8-ADD3-C4AB0D6FD231 width=0 height=0 CODEBASE=http://plugin.inicis.com/wallet61/INIwallet61_win8.cab#Version=1,0,0,3 onerror=OnErr()></OBJECT>";
			else PLUGIN_CLASSID = "<OBJECT ID=INIpay CLASSID=CLSID:24F6E6A8-852C-45A8-ADD3-C4AB0D6FD231 width=0 height=0 CODEBASE=http://plugin.inicis.com/wallet61/INIwallet61_vista.cab#Version=1,0,0,1 onerror=OnErr()></OBJECT>";
	
		}
		else
		{
			if( navigator.userAgent.indexOf("Windows NT 5.1") <= -1 && navigator.userAgent.indexOf("Windows NT 5.2") <= -1 )
				PLUGIN_ERRMSG = "[INIpay전자지갑]이 설치되지 않았습니다.\n\n브라우저에서 [새로고침]버튼을 클릭하신 후 [보안경고]창이 나타나면 [예]버튼을 클릭하세요.";
		}
		
		return;
	}

	//== Use NPPLUGIN
	PLUGIN_CLASSID = NPPLUGIN_MIMETYPE;

}

function OnErr()
{
	alert(PLUGIN_ERRMSG);
}