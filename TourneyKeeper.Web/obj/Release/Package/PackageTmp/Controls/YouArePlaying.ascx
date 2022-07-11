<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="YouArePlaying.ascx.cs" Inherits="TourneyKeeper.Web.Controls.YouArePlaying" %>
<%@ Register Src="~/Controls/Army.ascx" TagPrefix="uc3" TagName="Army" %>

<h4>My tournaments</h4>
<div class="container-fluid padding-0">
    <uc3:Army runat="server" ID="Army" />

    <asp:ListView ID="YouArePlayingListView" runat="server">
        <ItemTemplate>
            <div class="card card-primary mb-4">
                <div class="card-body">
                    <h5 class="card-title"><%#Eval("TournamentName")%>&nbsp;- Round <%#Eval("Round")%></h5>
                    <div class="row">
                        <div class="col-md-12">
                            <p>
                                <asp:Literal ID="armyLiteral" Text='<%#Eval("Army")%>' runat="server"></asp:Literal>
                            </p>
                        </div>
                    </div>
                    <asp:PlaceHolder ID="TeamPlaceHolder" Visible='<%#Eval("IsTeamCurrentlyMatched")%>' runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    You are playing <%#Eval("CurrentOpponentTeamName")%> on table <%#Eval("TeamTable")%> (<a href='<%#Eval("SetupPairingsLink")%>'>Setup pairings</a>)
                                </p>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <asp:PlaceHolder ID="CurrentlyMatchedPlaceHolder" Visible='<%#Eval("IsCurrentlyMatched")%>' runat="server">
                        <div class="row">
                            <div class="col-md-12">
                                <p>
                                    Current opponent is <%#Eval("CurrentOpponentName")%> on table <%#Eval("Table")%> (<a href='<%#Eval("ResultsLink")%>'>Enter results</a>)
                                </p>
                            </div>
                        </div>
                    </asp:PlaceHolder>
                    <div class="row align-bottom">
                        <div class="col-md-12">
                            <p>
                                <a href='<%#Eval("LeaderboardLink")%>'>Leaderboard</a>&nbsp;&nbsp;&nbsp;<a href='<%#Eval("PairingsLink")%>'>Pairings</a>
                            </p>
                        </div>
                    </div>
                </div>
            </div>
        </ItemTemplate>
    </asp:ListView>
</div>
