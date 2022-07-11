<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKCodexMatrix.aspx.cs" Inherits="TourneyKeeper.Web.TKCodexMatrix" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#statisticsli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="true" CssClass="table table-condensed table-striped" OnRowDataBound="LeaderboardGridView_RowDataBound" GridLines="None">
            </asp:GridView>
        </div>
    </div>
</asp:Content>
