<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKShowRanking.aspx.cs" Inherits="TourneyKeeper.Web.TKShowRanking" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
        <li class="nav-item" id="previoustournamentsli">
            <asp:HyperLink ID="previousTournamentsHyperLink" NavigateUrl="/Shared/PreviousTournaments.aspx" class="nav-link" runat="server">Previous Tournaments</asp:HyperLink></li>
        <li class="nav-item" id="newsandlinksli">
            <asp:HyperLink ID="newsAndLinksHyperLink" NavigateUrl="/Shared/NewsAndLinks.aspx" class="nav-link" runat="server">News and Links</asp:HyperLink></li>
        <li class="nav-item active" id="rankingsli">
            <asp:HyperLink ID="rankingsHyperLink" NavigateUrl="/Shared/TKShowRanking.aspx" class="nav-link" runat="server">Rankings</asp:HyperLink></li>
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script>
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
        });
    </script>

    <div class="row">
        <div class="col-md-4">
            <asp:DropDownList ID="GameSystemDropDown" runat="server" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AutoPostBack="true" DataValueField="Id" DataTextField="Name" OnSelectedIndexChanged="GameSystemDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <label class="mdb-main-label">Game system</label>
        </div>
        <div class="col-md-4">
            <asp:DropDownList ID="CountryDropdown" runat="server" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AutoPostBack="true" DataValueField="Name" DataTextField="Name" OnSelectedIndexChanged="CountryDropDown_SelectedIndexChanged">
            </asp:DropDownList>
            <label class="mdb-main-label">Country</label>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12">
            <asp:GridView ID="RankingGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" AllowSorting="True" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Placement" HeaderText="Position" InsertVisible="false" />
                    <asp:TemplateField HeaderText="Player (W/L/D)">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "PlayerId")) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "PlayerName") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Points" HeaderText="Points" InsertVisible="false" />
                    <asp:BoundField DataField="LatestGame" HeaderText="Latest game" InsertVisible="false" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContactPlaceHolder">
    Contact: <a href="mailto:admin@tourneykeeper.net">admin@tourneykeeper.net</a>
</asp:Content>
