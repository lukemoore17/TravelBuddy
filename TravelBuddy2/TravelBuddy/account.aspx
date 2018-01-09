<%@ Page Title="" Language="C#" MasterPageFile="~/TravelBuddy.master" AutoEventWireup="true" CodeBehind="account.aspx.cs" Inherits="TravelBuddy.account" %>
<%@ Import Namespace="TravelBuddy.code" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<style type="text/less">

    </style>
    <script src="//cdnjs.cloudflare.com/ajax/libs/less.js/2.7.2/less.min.js"></script>--%>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <asp:Panel runat="server" ID="AccountPanel">
        <asp:PlaceHolder ID="err_Placeholder" runat="server" />

        <h4><%=(string)Session[AppData.FIRSTNAME] %>, welcome to your account page</h4>
        <p>Here you can view and manage your information.</p>
        <br />

        <div class="row">
            <div class="col-md-4">
                <nav class="nav flex-column">
                    <a id="PersonalInfoNav" class="nav-link" href="#">Personal Information</a>
                    <a id="ChangePasswordNav" class="nav-link" href="#">Change Password</a>
                    <a id="ChangeUsernameNav" class="nav-link" href="#">Change Username</a>
                </nav>
            </div>

            <div class="col-md-8">
                <div id="PersonalInfo" class="col-md-6">
                    <h5>Your Personal Information</h5><a id="EditPI" href="#">Edit</a>
                    <br />
                    <asp:UpdatePanel runat="server" ID="PI_UpdatePanel" UpdateMode="Always">
                        <ContentTemplate>
                            <%-- Validation Here --%>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
                                DisplayMode="BulletList" CssClass="alert alert-danger" ValidationGroup="ValidatePI" Visible="true" />
                            <asp:PlaceHolder runat="server" ID="updatePI_ErrPlaceholder" />
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblFirstName" AssociatedControlID="txtFirstName">First Name</asp:Label>
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="form-control" Enabled="false" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="First name required"
                                    ControlToValidate="txtFirstName" Display="None" ValidationGroup="ValidatePI" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Invalid first name text" 
                                    ValidationExpression="^([A-Z]?[a-z]?['-]?[ ]?)+$" Display="None" 
                                    ControlToValidate="txtFirstName" ValidationGroup="ValidatePI" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblLastName" AssociatedControlID="txtLastName">Last Name</asp:Label>
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="form-control" Enabled="false" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Last name Required"
                                    ControlToValidate="txtLastName" Display="None" ValidationGroup="ValidatePI" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid last name text" 
                                    ValidationExpression="^([A-Z]?[a-z]?['-]?[ ]?)+$" Display="None" 
                                    ControlToValidate="txtLastName" ValidationGroup="ValidatePI" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblCountry" AssociatedControlID="CountryListBox"></asp:Label>
                                <asp:DropDownList ID="CountryListBox" runat="server" CssClass="form-control" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Country Required"
                                    ControlToValidate="CountryListBox" Display="None" ValidationGroup="ValidatePI" />
                            </div>
                            <asp:Button runat="server" ID="btnUpdatePI" CssClass="btn btn-outline-success" 
                                Text="Save Changes" OnClick="btnUpdatePI_Click" ValidationGroup="ValidatePI" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="ChangePassword" class="col-md-6" style="display:none;">
                    <h5>Change your password</h5>
                    <br />
                    <asp:UpdatePanel runat="server" ID="NewPW_UpdatePanel">
                        <ContentTemplate>
                            <%-- Validation Here --%>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" 
                                DisplayMode="BulletList" CssClass="alert alert-danger" ValidationGroup="ValidateNewPW" />
                            <asp:PlaceHolder runat="server" ID="updatePW_ErrPlaceholder" />
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblOldPW" AssociatedControlID="txtOldPW">Current Password</asp:Label>
                                <asp:TextBox runat="server" ID="txtOldPW" CssClass="form-control" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Old password is required"
                                    ControlToValidate="txtOldPW" Display="None" ValidationGroup="ValidateNewPW" />
                            </div>
                            <div class="form-group">
                                <asp:Label runat="server" ID="lblNewPW" AssociatedControlID="txtNewPW">New Password</asp:Label>
                                <asp:TextBox runat="server" ID="txtNewPW" CssClass="form-control" placeholder="Enter secure password" TextMode="Password" />
                                <asp:TextBox runat="server" ID="txtNewPWConfirm" CssClass="form-control" placeholder="Confirm new password" TextMode="Password" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="New password is required"
                                    ControlToValidate="txtNewPW" Display="None" ValidationGroup="ValidateNewPW" />
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Confirm new password"
                                    ControlToValidate="txtNewPWConfirm" Display="None" ValidationGroup="ValidateNewPW" />
                                <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="New passwords do not match"
                                    Display="None" ControlToValidate="txtNewPWConfirm" ControlToCompare="txtNewPW" ValidationGroup="ValidateNewPW" />
                                <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="New password not long enough" Display="None"
                                    OnServerValidate="NewPW_ServerValidate" ControlToValidate="txtNewPW"
                                    ValidateEmptyText="True" ValidationGroup="ValidateNewPW" />
                                <small class="form-text text-muted">Passwords must be at least 6 characters</small>
                            </div>
                    
                            <asp:Button runat="server" ID="btnUpdatePW" CssClass="btn btn-outline-success" 
                                Text="Update" OnClick="btnUpdatePW_Click" ValidationGroup="ValidateNwewPW" />
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div id="ChangeUsername" class="col-md-6" style="display:none;">
                    <h5>Change your username</h5><a id="EditUN" href="#">Edit</a>
                    <br />
                    <%-- Validation Here --%>
                </div>
            </div>
        </div>



    </asp:Panel>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="cpJQuery" runat="server">
    <script>
        if (isAjaxPostback == true) {
            //alert("ajax postback");
        }
        else if (isAjaxPostback == false) {
            //alert("not ajax postback");
        }

        $(document).ready(function () {
            $('input[id*="btnUpdatePI"]').hide();
            $('select[id*="CountryListBox"]').hide();

            $('#PersonalInfoNav').click(function () {
                $('#ChangePassword').hide();
                $('#ChangeUsername').hide();
                $('#PersonalInfo').show();
            });
            $('#ChangePasswordNav').click(function () {
                $('#PersonalInfo').hide();
                $('#ChangeUsername').hide();
                $('#ChangePassword').show();
            });
            $('#ChangeUsernameNav').click(function () {
                $('#PersonalInfo').hide();
                $('#ChangePassword').hide();
                $('#ChangeUsername').show();
            });

            $('#EditPI').click(function () {
                $('#PersonalInfo input[type="text"]').prop('disabled', function (i, v) { return !v; });
                $('button[id*="CountryComboBox"]').prop('disabled', function (i, v) { return !v; });

                $('input[id*="btnUpdatePI"]').toggle();
                $('select[id*="CountryListBox"]').toggle();
            });
        });

        // Code for when Ajax postback
        // This re-subscribes j-script and jquery events
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            if (isAjaxPostback == true) {
                //alert("ajax postback");
                $('input[id*="btnUpdatePI"]').hide();
                $('select[id*="CountryListBox"]').hide();
            }
            else if (isAjaxPostback == false) {
                //alert("not ajax postback");
            }
        });
    </script>
</asp:Content>
