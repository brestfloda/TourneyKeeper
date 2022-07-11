<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKWeightedLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.WeightedLeaderboard" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#statisticsli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                OnRowCreated="LeaderboardGridViewRowCreated" OnInit="LeaderboardGridViewInit" AllowSorting="true" OnSorting="LeaderboardGridViewSorting" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Placement" HeaderText="Position" InsertVisible="false" SortExpression="Placement" />
                    <asp:TemplateField HeaderText="Player" SortExpression="Name">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "PlayerId")) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="PrimaryCodex" HeaderText="Primary codex" InsertVisible="false" SortExpression="PrimaryCodex" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:TemplateField HeaderText="Armylist">
                        <ItemTemplate>
                            <a href='<%# String.Format("javascript:OpenPopup({0});", DataBinder.Eval(Container.DataItem, "Id")) %>' runat="server">Show</a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="TotalPoints" HeaderText="Total points" InsertVisible="false" SortExpression="TotalPoints" />
                    <asp:BoundField DataField="TotalOpponentsPoints" HeaderText="Total opponents points" InsertVisible="false" SortExpression="TotalOpponentsPoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="WeightedScore" HeaderText="Dorner Points" InsertVisible="false" SortExpression="WeightedScore" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
