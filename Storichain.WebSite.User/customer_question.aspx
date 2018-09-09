<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="customer_question.aspx.cs" Inherits="Storichain.WebSite.User.customer_question" %>
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

            $('.btn_r.btn_l.btn_cw').click(function (e) {
                e.preventDefault();

                location.href = 'discover';
            });

            $('.btn_r.btn_l.btn_cp').click(function (e) {
                e.preventDefault();

                var q_sel_val   = $('#qst_cat').val();
                var q_title     = $('#qst_tit').val();
                var q_desc      = $('#qst_ct').val();

                if(q_sel_val == '') {
                    alert('Please select a question item.');
                    return;
                }

                if(q_title == '') {
                    alert('Please enter a question title.');
                    return;
                }

                if(q_desc == '') {
                    alert('Please enter a question content.');
                    return;
                }

                var formData = new FormData();
                formData.append('user_idx', g_user_idx);
                formData.append('question_type_idx', q_sel_val);
                formData.append('title', q_title);
                formData.append('question_content', q_desc);
                
                $.ajax({
                    type: "Post",
                    url: "/Question/Add",
                    data: formData,
                    dataType: "json",
                    cache: false,
                    contentType: false,
                    processData: false,
                    jsonp: false,
                    success: function (response) {
                        if (response.response_code == "1000") {
                            alert("It was saved");
                            location.href = "/discover";
                        }
                        else 
                        {
                            alert("Please enter the correct data.");
                            return;
                        }
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
			<h1 class="ptit"><a href="#">Inquiry</a></h1>
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
							<h3 class="form_tit">The items of the inquiry</h3>
							<div class="inp_box">
								<select id="qst_cat" name="qst_cat" title="문의분야" class="sel">
                                    <option value="" selected="">- Select the item -</option>
                                    <%
                                        Biz_Code biz_code = new Biz_Code();
                                        DataTable dt_code = biz_code.GetCodeList(34);
                                        foreach(DataRow dr_code in dt_code.Rows)
                                        {
                                            WebUtility.Write(string.Format("<option value='{0}'>{1}</option>", dr_code.ItemValue("code_value"), dr_code.ItemValue("code_name")));
                                        }
                                    %>
								</select>
							</div>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Title</h3>
							<div class="inp_box"><input type="email" name="" id="qst_tit" class="inpt" placeholder="Title" /><label for="qst_tit" class="lblt">Title</label></div>
							<p class="error_msg">error_msg</p>
						</div>
						<div class="form_box">
							<h3 class="form_tit">Content</h3>
							<div class="txt_tbox"><textarea name="" id="qst_ct" class="ftxt" cols="30" rows="10" title="Content"></textarea><label for="qst_ct" class="lblt">Content</label></div>
						</div>
						
						<ul class="form_btn form_btn2">
							<li><span><button class="btn_r btn_l btn_cw">Cancel</button></span></li>
							<li><span><button class="btn_r btn_l btn_cp">Save</button></span></li>
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
