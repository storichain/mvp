<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="Storichain.WebSite.User.index" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<%@ Register Src="~/Libs/footer.ascx" TagPrefix="uc1" TagName="footer" %>

<%@ Import Namespace="Storichain" %>
<%@ Import Namespace="Storichain.Models.Biz" %>
<%@ Import Namespace="Storichain.Models" %>
<%@ Import Namespace="System.Data" %>
<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
    <script>
        var g_user_idx = '<%= PageUtility.UserIdx() %>';

        $(function() {
            location.href = "discover";
            return;
        });

    </script>
</head>
<body class="mbd">
<uc1:footer runat="server" ID="footer" />
</body>
</html>


<%--            //doGetMyPost(myPost_json);

//$(window).scroll(function () {

//    var buffer = 100;
//    var storyList_view = $(window);

//    if ($("html").prop('scrollHeight') - storyList_view.scrollTop() <= storyList_view.height() + buffer) 
//    {
//        if (pageLoaded > 0) 
//        {
//            if (page_count > pageLoaded) 
//            {
//                doMoreMyPost(pageLoaded + 1, true);
//            }
//        }
//    }
//});

//$('#cbxCategory').change(function (e) {
//    e.preventDefault();

//    doMoreMyPost(1, false);
//});--%>