<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="NewsAndLinks.aspx.cs" Inherits="TourneyKeeper.Web.Shared.NewsAndLinks" MasterPageFile="~/TKSite.master" %>

<asp:Content ID="TitleContent" runat="server" ContentPlaceHolderID="TitleContentPlaceHolder">
    TourneyKeeper
</asp:Content>

<asp:Content ID="NavContent" runat="server" ContentPlaceHolderID="NavigationContent">
    <ul class="nav navbar-nav">
        <li class="nav-item" id="previoustournamentsli">
            <asp:HyperLink ID="previousTournamentsHyperLink" NavigateUrl="/Shared/PreviousTournaments.aspx" class="nav-link" runat="server">Previous Tournaments</asp:HyperLink></li>
        <li class="nav-item active" id="newsandlinksli">
            <asp:HyperLink ID="newsAndLinksHyperLink" NavigateUrl="/Shared/NewsAndLinks.aspx" class="nav-link" runat="server">News and Links</asp:HyperLink></li>
        <li class="nav-item" id="rankingsli">
            <asp:HyperLink ID="rankingsHyperLink" NavigateUrl="/Shared/TKShowRanking.aspx" class="nav-link" runat="server">Rankings</asp:HyperLink></li>
    </ul>
</asp:Content>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:LinqDataSource ID="FrontPageLinksDataSource" runat="server" ContextTypeName="TourneyKeeper.Common.TourneyKeeperDataContext"
        TableName="TKFrontPageLinks" OrderBy="Id Descending" OnContextCreating="TournamentsDataSource_ContextCreating">
    </asp:LinqDataSource>
    <div class="row">
        <div class="col-md-6">
            <h4>Updates</h4>
            <div>
                <div>
                    <asp:ListView DataSourceID="FrontPageLinksDataSource" runat="server">
                        <LayoutTemplate>
                            <table runat="server" id="table1">
                                <tr runat="server" id="itemPlaceholder"></tr>
                            </table>
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="3">
                            </asp:DataPager>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server">
                                <td runat="server">
                                    <div>
                                        <a href='<%#Eval("Link")%>' runat="server"><%#Eval("Headline")%></a>
                                        <br />
                                        <asp:Label ID="NameLabel" runat="server" Text='<%#Eval("Content")%>' />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
        </div>
        <div class="col-md-6">
            <h4>Links</h4>
            <div>
                <div>
                    <a href="/Shared/HowToSignup.aspx">How to sign up</a>
                </div>
                <div>
                    <a href="/pdf/SettingUpPairings.pdf">How to setup pairings</a>
                </div>
                <div>
                    <a href="/pdf/EnteringScores.pdf">How to enter scores</a>
                </div>
                <div>
                    <a href="/Shared/TKShowRanking.aspx">Rankings</a>
                </div>
                <div>
                    <a href="/pdf/TourneyKeeper.pdf">TourneyKeeper manual</a>
                </div>
                <div>
                    <a href="/Shared/PsyCalc.aspx">PsyCalc</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="ContactPlaceHolder">
    Contact: <a href="mailto:admin@tourneykeeper.net">admin@tourneykeeper.net</a>
</asp:Content>
