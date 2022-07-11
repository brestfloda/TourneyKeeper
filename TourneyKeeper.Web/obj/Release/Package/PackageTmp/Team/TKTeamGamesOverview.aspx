<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamGamesOverview.aspx.cs" Inherits="TourneyKeeper.Web.TKTeamGamesOverview" MasterPageFile="TKTeam.master" %>

<%@ Register Src="~/Controls/GamesOverview.ascx" TagPrefix="uc4" TagName="GamesOverview" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <uc4:GamesOverview runat="server" ID="GamesOverview" />
</asp:Content>
