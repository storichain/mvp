<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="trade_form.aspx.cs" Inherits="Storichain.WebSite.User.trade_form" %>
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

            $('.btn_r.btn_l.btn_cp').click(function (e) {
                e.preventDefault();

                var buyer_name      = $('#buyer_name').val();
                var story_asset     = $('#story_asset').val();
                var ask             = $('#ask').is(':checked');
                var contract        = $('#contract').is(':checked');
                var deposit         = $('#deposit').val();
                var bprice          = $('#bprice').val();
                var tg_country      = $('#tg_country').val();
                var period          = $('#period').val();
                var scc_agree       = $('#scc_agree').is(':checked');

                console.log(buyer_name + ' buyer_name');
                console.log(story_asset  + ' story_asset');
                console.log(ask + ' ask');
                console.log(contract + ' contract');
                console.log(deposit + ' deposit');
                console.log(bprice + ' bprice');
                console.log(tg_country + ' tg_country');
                console.log(period + ' period');
                console.log(scc_agree + ' scc_agree');
                
            });

        });
    </script>
</head>
<body class="sbd">
<div class="header mhd">
	<div class="mhd_lg">
		<div class="hd_wrap">
			<h1 class="ptit"><a href="#">Deal &amp; Buy</a></h1>
			<a href="javascript:history.back();" class="btn btn_back_w"><span class="blind">Back</span></a>
			<a href="discover" class="btn btn_go_home"><span class="blind">Home</span></a>
		</div>
	</div>
</div>

<div class="container">
	<div class="content">
		<div class="ct_wrap">
			<h2 class="form_htit">Request For Contract on Use of IP<br />Story Asset [Worriors of the Dawn]</h2>
			<form action="">
				<fieldset>
					<div class="form_wrap">
						<div class="form_box">
							<h3 class="form_tit">Buyer's Name</h3>
							<div class="inp_box"><input type="email" name="" id="buyer_name" class="inpt" placeholder="Real Full Name" /><label for="buyer_name" class="lblt">Buyer's Name</label></div>
							<p class="error_msg">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Suggestion of Buying this Story Asset</h3>
							<div class="txt_tbox"><textarea name="" id="story_asset" class="ftxt" cols="30" rows="10" placeholder="Type suggestion ment here to storyteller(master writer of this story)"></textarea><label for="story_asset" class="lblt">Suggestion of Buying this Story Asset</label></div>
						</div>
						<div class="form_check">
							<span class="form_check_box">
								<input type="checkbox" id="ask" name="" class="ckbx" value="">
								<label for="ask" class="lblck">Asking Only</label>
							</span>
							<span class="form_check_box">
								<input type="checkbox" id="contract" name="" class="ckbx" value="">
								<label for="contract" class="lblck">Contract Now</label>
							</span>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Deposit</h3>
							<div class="inp_box"><input type="number" name="" id="deposit" class="inpt inpt_r" placeholder="0" /><label for="deposit" class="lblt">Deposit</label></div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Buying Price</h3>
							<div class="inp_box"><input type="number" name="" id="bprice" class="inpt inpt_r" placeholder="0" /><label for="bprice" class="lblt">Buying Price</label></div>
						</div>
						<div class="form_fwrap">
							<div class="form_flt form_fl">
								<div class="form_box">
									<h3 class="form_tit">Target Country</h3>
									<div class="inp_box">
										<select id="tg_country" name="tg_country" title="Target Country" class="sel">
											<option value="0">USA</option>
											<option value="1">KOREA</option>
										</select>
									</div>
								</div>
							</div>
							<div class="form_flt form_fr">
								<div class="form_box">
									<h3 class="form_tit">Period of using IP</h3>
									<div class="inp_box">
										<select id="period" name="period" title="Period of using IP" class="sel">
											<option value="1">For 1 years</option>
											<option value="2">For 2 years</option>
											<option value="3">For 3 years</option>
										</select>
									</div>
								</div>
							</div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Smart Contract Conditions</h3>
							<p class="form_p">본 스마트컨트랙트는 작가와의 판권계약을 위한 계약으로 하기 조건을 충족 시 즉시 이행됨을 알려드립니다.</p>
							<p class="form_p">스마트 컨트랙트 내용을 숙지하였으며 이에 동의합니까?</p>
						</div>
						<div class="form_check">
							<div class="form_check_box">
								<input type="checkbox" id="scc_agree" name="" class="ckbx" value="">
								<label for="scc_agree" class="lblck">I Agree</label>
							</div>
						</div>
						<ul class="form_btn">
							<li><span><button class="btn_r btn_l btn_cp">SEND</button></span></li>
						</ul>
					</div>
				</fieldset>
			</form>
		</div>
	</div>
</div>

<uc1:footer runat="server" ID="footer" />

</body>
</html>
