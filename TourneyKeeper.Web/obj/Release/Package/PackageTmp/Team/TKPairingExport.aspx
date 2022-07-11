<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKPairingExport.aspx.cs" Inherits="TourneyKeeper.Web.TKPairingExport" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <!-- 1. Define some markup -->
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:TextBox ID="ExportDataTextBox" runat="server" CssClass="maxboxtextarea" Width="100%" Height="500" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <div class="form-inline">
                <div class="form-group">
                    <asp:Label ID="StatusLabel" runat="server" Text="Round: "></asp:Label>
                </div>
                <div class="form-group">
                    <asp:DropDownList ID="RoundDropDown" runat="server" CssClass="selectpicker" AutoPostBack="true" OnSelectedIndexChanged="RoundDropDown_SelectedIndexChanged">
                    </asp:DropDownList>
                    <button class="btn btn-primary" data-clipboard-action="copy" data-clipboard-target="#ExportDataTextBox">Copy to clipboard</button>
                </div>
            </div>
        </div>
    </div>

    <!-- 2. Include library -->
    <script src="../Tools/Scripts/clipboard.min.js"></script>

    <!-- 3. Instantiate clipboard -->
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
