<%@ Page Title="" Language="C#" MasterPageFile="~/TravelBuddy.master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="TravelBuddy._register" %>
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

<%--<style type="text/less">

</style>
<script src="//cdnjs.cloudflare.com/ajax/libs/less.js/2.7.2/less.min.js"></script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <asp:PlaceHolder runat="server" ID="messagePlaceholder" />
    <section id="regeisterSection">
        <div class="container" id="registerFormGroup">
            <div class="row">
                <div class="col-md-6">
                    <h4 class="form-group">Create a TravelBuddy Account</h4>
                    <div role="alert">
                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" DisplayMode="BulletList" CssClass="alert alert-danger" />
                        <asp:PlaceHolder runat="server" ID="registerErrPlaceholder" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblFirstname" AssociatedControlID="txtFirstName">First Name</asp:Label>
                        <asp:TextBox class="form-control" ID="txtFirstName" runat="server" placeholder="Enter first name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="First name required" 
                            ControlToValidate="txtFirstName" Display="None" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Inavlid first name" 
                            ValidationExpression="^([A-Z]?[a-z]?['-]?[ ]?)+$" Display="None" ControlToValidate="txtFirstName"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblLastname" AssociatedControlID="txtLastName">Last Name</asp:Label>
                        <asp:TextBox class="form-control" ID="txtLastName" runat="server" placeholder="Enter last name"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="First name required" 
                            ControlToValidate="txtLastName" Display="None" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Inavlid last name" 
                            ValidationExpression="^([A-Z]?[a-z]?['-]?[ ]?)+$" Display="None" ControlToValidate="txtLastName"></asp:RegularExpressionValidator>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblCountry" AssociatedControlID="CountryListBox">Country</asp:Label>
                        <asp:DropDownList ID="CountryListBox" runat="server" CssClass="form-control" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Country is required" 
                            ControlToValidate="CountryListBox" Display="None" />
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblEmail" AssociatedControlID="txtEmail">Email</asp:Label>
                        <asp:TextBox TextMode="Email" class="form-control" ID="txtEmail" runat="server" placeholder="john.doe@example.com"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Email is required" 
                            ControlToValidate="txtEmail" Display="None" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ErrorMessage="Inavlid email address" 
                            ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="None" ControlToValidate="txtEmail"></asp:RegularExpressionValidator>
                        <asp:TextBox TextMode="Email" class="form-control" ID="txtConfirmEmail" runat="server" placeholder="Confirm email"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Emails do not match" Display="None" 
                            ControlToValidate="txtConfirmEmail" ControlToCompare="txtEmail"></asp:CompareValidator>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblUsername" AssociatedControlID="txtUsername">Username</asp:Label>
                        <asp:TextBox class="form-control" ID="txtUsername" runat="server" placeholder="Choose a username"></asp:TextBox>
                        <small class="form-text text-muted">Letters, numbers, and underscores only (4-20 characters)</small>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Username is required" 
                            ControlToValidate="txtEmail" Display="None" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator5" runat="server" ErrorMessage="Inavlid username" 
                            ValidationExpression="^([\w]?[_-]?)+$" Display="None" ControlToValidate="txtUsername"></asp:RegularExpressionValidator>
                        <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Username too short or long" Display="None" 
                            OnServerValidate="UsernameCustomValidator_ServerValidate" ControlToValidate="txtUsername"
                            ValidateEmptyText="True"></asp:CustomValidator>
                    </div>
                    <div class="form-group">
                        <asp:Label runat="server" ID="lblPassword" AssociatedControlID="txtPassword">Password</asp:Label>
                        <asp:TextBox TextMode="Password" class="form-control" ID="txtPassword" runat="server" placeholder="Enter a secure password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Password is required" 
                            ControlToValidate="txtPassword" Display="None" />
                        <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Password not long enough" Display="None"
                            OnServerValidate="PasswordCustomValidator_ServerValidate" ControlToValidate="txtPassword"
                            ValidateEmptyText="True"></asp:CustomValidator>
                        <asp:TextBox TextMode="Password" class="form-control" ID="txtConfirmPassword" runat="server" placeholder="Confirm password"></asp:TextBox>
                        <asp:CompareValidator ID="CompareValidator2" runat="server" ErrorMessage="Passwords do not match" Display="None" 
                            ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword"></asp:CompareValidator>
                        <small class="form-text text-muted">Passwords must be at least 6 characters</small>
                    </div>
                    <div align="center">
                        <asp:Button runat="server" ID="submit" CssClass="btn btn-primary" Text="Register" OnClick="Register_Click" />
                    </div>
                </div>
            </div>
        </div>
        <br />
        <br />
        <div class="loading" align="center">
            Creating your account...<br />
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
            $('#registerFormGroup').addClass("modal");
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
