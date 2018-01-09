<%@ Page Title="" Language="C#" MasterPageFile="~/TravelBuddy.master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="TravelBuddy._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cpMainContent" runat="server">
    <div>
        <asp:Panel ID="locationsPanel" runat="server">
            <asp:PlaceHolder ID="err_Placeholder" runat="server" />
            <asp:PlaceHolder ID="msgPlaceholder" runat="server" />

            <%-- Iterate over locations --%>
            <div class="row">
                <asp:PlaceHolder ID="loc_Placeholder" runat="server" />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="cpJQuery" runat="server">
    <asp:PlaceHolder ID="jq_Placeholder" runat="server" />
    <script type="text/javascript">
        $(function () {
            // Character counter for comment box
            var text_max = 200;
            $('h6[id*="CharsLeft_"]').html(text_max + ' characters remaining');
            $('textarea[id*="Comment_"]').keyup(function () {
                var text_length = $(this).val().length;
                var text_remaining = text_max - text_length;

                $('h6[id*="CharsLeft_"]').html(text_remaining + ' characters remaining');
            });

            // Clear modal inputs on dismiss
            $('[data-dismiss=modal]').on('click', function (e) {
                var $t = $(this),
                    target = $t[0].href || $t.data("target") || $t.parents('.modal') || [];
                $(target)
                    .find("input,textarea,select")
                    .val('')
                    .end()
                    .find("input[type=checkbox], input[type=radio]")
                    .prop("checked", "")
                    .end();
                // Reset character count
                var text_max = 200;
                $('h6[id*="CharsLeft_"]').html(text_max + ' characters remaining');
            });

            // Prevent "enter" key in comment section
            $('textarea').keypress(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });
            $('input[type="text"]').keypress(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                }
            });
        });
    </script>
</asp:Content>
