<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKITCExport.aspx.cs" Inherits="TourneyKeeper.Web.TKITCExport" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <asp:TextBox ID="ExportDataTextBox" runat="server" CssClass="maxboxtextarea" Width="100%" Height="500" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
    <button class="btn" data-clipboard-action="copy" data-clipboard-target="#ExportDataTextBox">Copy to clipboard</button>

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
