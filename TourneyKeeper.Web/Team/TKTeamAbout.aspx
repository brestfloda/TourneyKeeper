<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamAbout.aspx.cs" Inherits="TourneyKeeper.Web.TKTeamAbout" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#aboutli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:Literal ID="descriptionLiteral" runat="server"></asp:Literal>
        </div>
    </div>
</asp:Content>
