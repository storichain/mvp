<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="intro.aspx.cs" Inherits="Storichain.WebSite.User.intro" %>
<%@ Register Src="~/Libs/header.ascx" TagPrefix="uc1" TagName="header" %>
<html lang="ko" class=" ">
<head>
    <uc1:header runat="server" ID="header" />
</head>
<body class="mbd">
<div class="intro">
    <div class="logo_intro">
        <div class="sb"><img src="/images/logo_sb.png" alt="S"></div>
        <div class="st"><img src="/images/logo_txt.png" alt="Storichain"></div>
    </div>
</div>

<div class="footer">
    <p class="copy">copyright &copy; 2018 STORICHAIN ALL RIGHTS RESERVED</p>
    <ul class="cp_lk">
        <li><a href="#">Private policy</a></li>
        <li><a href="#">Terms of Platform</a></li>
        <li><a href="#">Advertise</a></li>
        <li><a href="#">Contact</a></li>
    </ul>
</div>
</body>
</html>