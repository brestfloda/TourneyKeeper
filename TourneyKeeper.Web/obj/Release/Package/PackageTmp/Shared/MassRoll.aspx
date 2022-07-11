<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MassRoll.aspx.cs" Inherits="TourneyKeeper.Web.MassRoll" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent" style="background-color: white; background: white;">
    <div class="row-fluid">
        <div class="row">
            <div class="col-md-3">
                Number of attacks
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="attacks" runat="server" TextMode="Number" min="1">10</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                BS
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="bs" runat="server" TextMode="Number" min="2" max="6">3</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Damage
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="damage" runat="server" TextMode="Number" min="1" max="20">1</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Reroll to hit
            </div>
            <div class="col-md-9">
                <asp:DropDownList runat="server" ID="rerolltohit">
                    <asp:ListItem Text="Full reroll" Value="FullReroll"></asp:ListItem>
                    <asp:ListItem Text="Reroll 1s" Value="Reroll1s"></asp:ListItem>
                    <asp:ListItem Text="No reroll" Value="NoReroll" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Hit modifier
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="hitmodifier" runat="server" TextMode="Number" min="-3" max="3">0</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                To wound
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="towound" runat="server" TextMode="Number" min="2" max="6">4</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Reroll to wound
            </div>
            <div class="col-md-9">
                <asp:DropDownList runat="server" ID="rerolltowound">
                    <asp:ListItem Text="Full reroll" Value="FullReroll"></asp:ListItem>
                    <asp:ListItem Text="Reroll 1s" Value="Reroll1s"></asp:ListItem>
                    <asp:ListItem Text="No reroll" Value="NoReroll" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Save
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="save" runat="server" TextMode="Number" min="0" max="6">5</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Reroll save
            </div>
            <div class="col-md-9">
                <asp:DropDownList runat="server" ID="rerollsave">
                    <asp:ListItem Text="Full reroll" Value="FullReroll"></asp:ListItem>
                    <asp:ListItem Text="Reroll 1s" Value="Reroll1s"></asp:ListItem>
                    <asp:ListItem Text="No reroll" Value="NoReroll" Selected="True"></asp:ListItem>
                </asp:DropDownList>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Feel no pain
            </div>
            <div class="col-md-9">
                <asp:TextBox ID="feelnopain" runat="server" TextMode="Number" min="0" max="6">5</asp:TextBox>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Hits
            </div>
            <div class="col-md-9">
                <asp:Literal ID="hits" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Wounding hits
            </div>
            <div class="col-md-9">
                <asp:Literal ID="woundinghits" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Killed single wound models after saves
            </div>
            <div class="col-md-9">
                <asp:Literal ID="aftersaves" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-3">
                Killed single wound models after feel no pain
            </div>
            <div class="col-md-9">
                <asp:Literal ID="afterfeelnopain" runat="server"></asp:Literal>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:LinkButton runat="server" CssClass="btn btn-large btn-primary" OnClick="CalcClick" Text="Calculate"></asp:LinkButton>
            </div>
        </div>
    </div>
</asp:Content>
