<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKShowGame.aspx.cs" Inherits="TourneyKeeper.Web.TKShowGame" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="row-fluid">
        <div class="row">
            <div class="col-md-2">
                Name:
            </div>
            <div class="col-md-4">
                <asp:HyperLink ID="Player1Name" runat="server"></asp:HyperLink>
            </div>
            <div class="col-md-2">
                Name
            </div>
            <div class="col-md-4">
                <asp:HyperLink ID="Player2Name" runat="server"></asp:HyperLink>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Points:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1Points" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Points:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2Points" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Sec. Points:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1SecondaryPoints" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Sec. Points:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2SecondaryPoints" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Primary codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1PrimaryCodex" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Primary codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2PrimaryCodex" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Sec. codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1SecondaryCodex" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Secondary codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2SecondaryCodex" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Tert. codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1TertiaryCodex" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Tertiary codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2TertiaryCodex" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Quat. codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player1QuaternaryCodex" runat="server"></asp:Label>
            </div>
            <div class="col-md-2">
                Quaternary codex:
            </div>
            <div class="col-md-4">
                <asp:Label ID="Player2QuaternaryCodex" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <div class="col-md-2">
                Armylist:
            </div>
            <div class="col-md-4">
            </div>
            <div class="col-md-2">
                Armylist:
            </div>
            <div class="col-md-4">
            </div>
        </div>
        <div class="row">
            <div class="col-md-6">
                <asp:Label ID="Player1Armylist" runat="server"></asp:Label>
            </div>
            <div class="col-md-6">
                <asp:Label ID="Player2Armylist" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>
