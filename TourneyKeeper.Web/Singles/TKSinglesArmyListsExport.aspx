<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSinglesArmyListsExport.aspx.cs" Inherits="TourneyKeeper.Web.TKSinglesArmyListsExport" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:TextBox ID="ExportDataTextBox" runat="server" CssClass="maxboxtextarea" Width="100%" Height="500" TextMode="MultiLine" ClientIDMode="Static"></asp:TextBox>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <div class="form-inline">
                <div class="form-group">
                    <button class="btn btn-primary" data-clipboard-action="copy" data-clipboard-target="#ExportDataTextBox">Copy to clipboard</button>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript" src="../Tools/Scripts/clipboard.min.js"></script>

    <script type="text/javascript">
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
