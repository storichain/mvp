<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logout.aspx.cs" Inherits="Storichain.WebSite.User.logout" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
    <script>
    $(document).ready(function() {
        
        $.ajax({type: 'Post',
                url: url_path + '/User/Logout',
                dataType: 'json',
                async: false,
                cache: false,
                success: function(data) {
                    location.href = 'discover';
                }
        });
    });
</script>
</head>
<body>
</body>
</html>








