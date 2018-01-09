<%@ Page Title="" Language="C#" MasterPageFile="~/TravelBuddy.master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="TravelBuddy._login" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        /*border: 5px solid #67CFF5;*/
        width: 200px;
        height: 150px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="messagePlaceholder" />
    <section id="loginSection">
        <div id="loginFormGroup" class="container">
            <div class="row">
                <div class="col-md-4">
                    <h4 class="form-group">Please Log In</h4>
                    <div role="alert">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" CssClass="alert alert-danger" />
                        <asp:PlaceHolder runat="server" ID="loginErrPlaceholder" />
                    </div>
                    <div class="form-group">
                        <asp:TextBox class="form-control" ID="txtUsername" runat="server" placeholder="Enter your username"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Username required"
                            ControlToValidate="txtUsername" Display="None" />
                    </div>
                    <div class="form-group">
                        <asp:TextBox class="form-control" ID="txtPassword" runat="server" placeholder="Enter your password" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password required"
                            ControlToValidate="txtPassword" Display="None" />
                    </div>
                    <div align="center">
                        <asp:Button runat="server" ID="submit" CssClass="btn btn-primary" Text="Log In" OnClick="Login_Click" />
                    </div>
                    <br />
                    <div class="form-group" style="text-align:center">
                        <a href="./register.aspx">Not registered? Create an account here</a>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="loading" align="center">
            Logging you in...<br />
            <br />
            <img src="img/loader.gif" alt="" />
        </div>
    </section>
    <br />
    <br />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJQuery" runat="server">
<script type="text/javascript">
    function ShowProgress() {
        setTimeout(function () {
            //var modal = $('<div />');
            $('#loginFormGroup').addClass("modal");
            //$('body').append(modal);
            var loading = $(".loading");
            loading.show();
            //var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
            var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
            loading.css({ left: left });
        }, 200);
    }

    $(document).ready(function () {
        $('input[type="submit"]').on("click", function () {
            if (Page_ClientValidate()) {
                ShowProgress();
            }
        });
    });
    
</script>
</asp:Content>
