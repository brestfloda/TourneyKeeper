<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamCodexLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.TKTeamCodexLeaderboard" MasterPageFile="TKTeam.master" %>
<%@ Register Src="~/Controls/CodexLeaderboard.ascx" TagPrefix="uc1" TagName="CodexLeaderboard" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <uc1:CodexLeaderboard runat="server" ID="CodexLeaderboard" />
</asp:Content>
