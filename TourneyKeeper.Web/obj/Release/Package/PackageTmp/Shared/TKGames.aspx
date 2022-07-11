<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKGames.aspx.cs" Inherits="TourneyKeeper.Web.TKGames" MasterPageFile="TKShared.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#gamesli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="span12" style="margin: 6px; padding-top: 6px; padding-bottom: 1px">
            <div style="margin: 5px 5px; padding-top: 3px">
                <h4>
                    <asp:Label ID="PlayerNameLabel" runat="server"></asp:Label></h4>
            </div>
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="GamesGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                AllowSorting="true" OnSorting="GamesGridViewSorting" OnRowCreated="GamesGridViewRowCreated" OnInit="GamesGridViewInit" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Tournament" SortExpression="TournamentName">
                        <ItemTemplate>
                            <a href='<%# (bool)Eval("IsTeamTournament")?String.Format("/Team/TKTeamLeaderboard.aspx?id={0}", Eval("TournamentId")): String.Format("/Singles/TKLeaderboard.aspx?id={0}", Eval("TournamentId")) %>' runat="server">
                                <%# Eval("TournamentName") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TournamentDate" HeaderText="Date" DataFormatString="{0:D}" InsertVisible="false" SortExpression="TournamentDate" />
                    <asp:BoundField DataField="TableNumber" HeaderText="Table" InsertVisible="false" SortExpression="TableNumber" />
                    <asp:TemplateField HeaderText="Opponent" SortExpression="Opponent">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", Eval("OpponentPlayerId")) %>' runat="server">
                                <%# Eval("OpponentName") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Points" HeaderText="Points" InsertVisible="false" SortExpression="Points" />
                    <asp:BoundField DataField="Ranked" HeaderText="Ranked?" InsertVisible="false" SortExpression="Ranked" />
                    <asp:BoundField DataField="ELO" HeaderText="ELO" DataFormatString="{0:f2}" InsertVisible="false" SortExpression="ELO" />
                    <asp:TemplateField HeaderText="Show game">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKShowGame.aspx?GameId={0}", Eval("GameId")) %>' runat="server">Game
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Result">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKEditGame.aspx?GameId={0}&TournamentId={1}", Eval("GameId"), Eval("TournamentId")) %>' runat="server">Result
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
