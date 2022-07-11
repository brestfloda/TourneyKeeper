<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PreviousTournaments.aspx.cs" Inherits="TourneyKeeper.Web.Shared.PreviousTournaments" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
        <li class="nav-item active" id="previoustournamentsli">
            <asp:HyperLink ID="previousTournamentsHyperLink" NavigateUrl="/Shared/PreviousTournaments.aspx" class="nav-link" runat="server">Previous Tournaments</asp:HyperLink></li>
        <li class="nav-item" id="newsandlinksli">
            <asp:HyperLink ID="newsAndLinksHyperLink" NavigateUrl="/Shared/NewsAndLinks.aspx" class="nav-link" runat="server">News and Links</asp:HyperLink></li>
        <li class="nav-item" id="rankingsli">
            <asp:HyperLink ID="rankingsHyperLink" NavigateUrl="/Shared/TKShowRanking.aspx" class="nav-link" runat="server">Rankings</asp:HyperLink></li>
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <script>
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form').addClass('mt-0');
            $('input.select-dropdown').addClass('mb-0');
        });
    </script>

    <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
    <asp:GridView ID="FinishedTournamentsGridView" runat="server" OnDataBound="CurrentTournamentsGridView_DataBound"
        AutoGenerateColumns="False" CssClass="table table-condensed table-striped" AllowSorting="True"
        UseAccessibleHeader="true" OnSorting="FinishedTournamentsGridViewSorting" GridLines="None">
        <Columns>
            <asp:TemplateField HeaderText="Tournament" SortExpression="Name">
                <ItemTemplate>
                    <asp:HyperLink ID="TournamentButton" runat="Server" NavigateUrl='<%# Eval("TournamentTypeName").ToString() == "Team" ?Eval("Id", "/Team/TKTeamLeaderboard.aspx?Id={0}"):  Eval("Id", "/Singles/TKLeaderboard.aspx?Id={0}") %>'>
                                            <%# Eval("Name") %>
                    </asp:HyperLink>
                    <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (!((DateTime?)Eval("OnlineSignupStart")).HasValue || ((DateTime?)Eval("OnlineSignupStart")).Value < DateTime.Now )) ? " - signup open" : ""%>
                    <%# ((bool)Eval("OnlineSignup") && (DateTime)Eval("TournamentDate") > DateTime.Now && (((DateTime?)Eval("OnlineSignupStart")).HasValue && ((DateTime?)Eval("OnlineSignupStart")).Value > DateTime.Now )) ? " - signup opens soon" : ""%>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Country" SortExpression="Country">
                <HeaderTemplate>
                    <asp:LinkButton ID="countryHeading" runat="server" CommandName="Sort" CommandArgument="Country">Country</asp:LinkButton>
                    <br />
                    <asp:DropDownList ID="ddCountry" ForeColor="Black" Width="60" CssClass="mdb-select colorful-select dropdown-primary md-form"
                        AutoPostBack="true"
                        runat="server" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Country") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="TournamentDate" HeaderText="Date" InsertVisible="false" DataFormatString="{0:D}" SortExpression="TournamentDate" />
            <asp:BoundField DataField="TournamentTypeName" HeaderText="Type" InsertVisible="false" SortExpression="TournamentType" />
            <asp:TemplateField HeaderText="Game system" SortExpression="GameSystem">
                <HeaderTemplate>
                    <asp:LinkButton ID="gameSystemHeading" runat="server" CommandName="Sort" CommandArgument="GameSystem">Gamesystem</asp:LinkButton>
                    <br />
                    <asp:DropDownList ID="ddGameSystem" ForeColor="Black" Width="60" CssClass="mdb-select colorful-select dropdown-primary md-form"
                        AutoPostBack="true"
                        DataTextField="Name" DataValueField="Id"
                        runat="server" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("TKGameSystem.Name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContactPlaceHolder">
    Contact: <a href="mailto:admin@tourneykeeper.net">admin@tourneykeeper.net</a>
</asp:Content>
