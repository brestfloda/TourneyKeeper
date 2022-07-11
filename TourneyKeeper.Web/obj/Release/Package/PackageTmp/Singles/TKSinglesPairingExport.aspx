<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSinglesPairingExport.aspx.cs" Inherits="TourneyKeeper.Web.TKSinglesPairingExport" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");

        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('input.select-dropdown').addClass('mb-0');
        });
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:TextBox ID="ExportDataTextBox" runat="server" CssClass="maxboxtextarea" Width="100%" Height="500" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <div class="form-inline">
                <div class="form-group">
                    <asp:DropDownList ID="RoundDropDown" ClientIDMode="Static" runat="server" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AutoPostBack="true" OnSelectedIndexChanged="RoundDropDown_SelectedIndexChanged">
                    </asp:DropDownList>
                    <button class="btn btn-primary" data-clipboard-action="copy" data-clipboard-target="#ExportDataTextBox">Copy to clipboard</button>
                </div>
            </div>
        </div>
    </div>

    <script src="../Tools/Scripts/clipboard.min.js"></script>

    <script>
        var clipboard = new Clipboard('.btn');

        clipboard.on('success', function (e) {
            console.log(e);
        });

        clipboard.on('error', function (e) {
            console.log(e);
        });

        $(".form").submit(function (event) {
            event.preventDefault();
        });
    </script>
</asp:Content>
