<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.TeamLeaderboard" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#leaderboardli").addClass("active");
    </script>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:PlaceHolder runat="server" Visible="false" ID="signupPlaceHolder">
                <h1 id="signup">
                    <p style="text-align: center">
                        <asp:HyperLink runat="server" ID="signupLink">Sign up!</asp:HyperLink>
                    </p>
                </h1>
            </asp:PlaceHolder>
            <asp:PlaceHolder runat="server" Visible="false" ID="signoutPlaceHolder">
                <h1 id="signout">
                    <p style="text-align: center">
                        <asp:HyperLink runat="server" ID="signoutLink">Sign out</asp:HyperLink>
                    </p>
                </h1>
            </asp:PlaceHolder>
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" OnRowCreated="LeaderboardGridViewRowCreated"
                AllowSorting="true" OnSorting="LeaderboardGridViewSorting" OnInit="LeaderboardGridViewInit" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Placement" HeaderText="#" InsertVisible="false" SortExpression="Placement" />
                    <asp:TemplateField HeaderText="Team" InsertVisible="false" SortExpression="Name">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Team/TKTeam.aspx?TeamId={0}&Id={1}", Eval("Id"), Eval("TournamentId")) %>' runat="server"><%# Eval("Name") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="SecondaryPoints" HeaderText="Secondary Points" InsertVisible="false" SortExpression="SecondaryPoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="BattlePointPenalty" HeaderText="BP Penalty" InsertVisible="false" SortExpression="BattlePointPenalty" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Penalty" HeaderText="Penalty" InsertVisible="false" SortExpression="Penalty" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="BattlePoints" HeaderText="BattlePoints" InsertVisible="false" SortExpression="BattlePoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Points" HeaderText="Points" InsertVisible="false" SortExpression="Points" />
                    <asp:BoundField DataField="Players" HtmlEncode="false" HeaderText="Players" InsertVisible="false" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
