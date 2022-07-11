<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.TKLeaderboard" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#leaderboardli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" OnRowCreated="LeaderboardGridViewRowCreated"
                AllowSorting="true" OnSorting="LeaderboardGridViewSorting" OnInit="LeaderboardGridViewInit" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="#">
                        <ItemTemplate>
                            <asp:Label Visible='<%# hideResultsforRound%>' Text="N/A" runat="server"></asp:Label>
                            <asp:Label Visible='<%# !hideResultsforRound%>' Text='<%# Eval("Placement")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Player" SortExpression="Player">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "PlayerId")) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Club" HeaderText="Club/Team" InsertVisible="false" SortExpression="Club" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Seed" HeaderText="Seed" InsertVisible="false" SortExpression="Seed" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:TemplateField HeaderText="Gamepath" SortExpression="Gamepath">
                        <ItemTemplate>
                            <asp:Label Visible='<%# hideResultsforRound%>' Text="N/A" runat="server"></asp:Label>
                            <asp:Label Visible='<%# !hideResultsforRound%>' Text='<%# Eval("Gamepath")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Army" SortExpression="Army">
                        <ItemTemplate>
                            <%# ShowArmylistLink((int)Eval("Id"), (string)Eval("PrimaryCodex"), (bool)Eval("HasArmylist"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Quiz" HeaderText="Quiz" InsertVisible="false" SortExpression="Quiz" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Painting" HeaderText="Painting" InsertVisible="false" SortExpression="Painting" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Fairplay" HeaderText="Fairplay" InsertVisible="false" SortExpression="Fairplay" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Penalty" HeaderText="Penalty" InsertVisible="false" SortExpression="Penalty" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="BattlePoints" HeaderText="Battle" InsertVisible="false" SortExpression="BattlePoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="SecondaryPoints" HeaderText="Sec. Points" InsertVisible="false" SortExpression="SecondaryPoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:TemplateField HeaderText="Points" SortExpression="Points">
                        <ItemTemplate>
                            <asp:Label Visible='<%# hideResultsforRound%>' Text="N/A" runat="server"></asp:Label>
                            <asp:Label Visible='<%# !hideResultsforRound%>' Text='<%# Eval("Points")%>' runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
