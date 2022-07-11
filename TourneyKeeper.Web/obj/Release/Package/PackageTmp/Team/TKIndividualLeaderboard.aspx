<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKIndividualLeaderboard.aspx.cs" Inherits="TourneyKeeper.Web.IndividualLeaderboard" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#individualLeaderboardli").addClass("active");
    </script>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
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
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" 
                 AllowSorting="true" OnSorting="LeaderboardGridViewSorting" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Placement" HeaderText="#" InsertVisible="false" SortExpression="Placement" />
                    <asp:TemplateField HeaderText="Player" SortExpression="Player">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "PlayerId")) %>' runat="server">
                                <%# DataBinder.Eval(Container.DataItem, "Name") %>
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="GamePath" HeaderText="Path" InsertVisible="false" SortExpression="GamePath" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:TemplateField HeaderText="Army" SortExpression="Army">
                        <ItemTemplate>
                            <%# ShowArmylistLink((int)Eval("Id"), (string)Eval("PrimaryCodex"), (bool)Eval("HasArmylist"))%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Penalty" HeaderText="Penalty" InsertVisible="false" SortExpression="Penalty" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="SecondaryPoints" HeaderText="Sec. Points" InsertVisible="false" SortExpression="SecondaryPoints" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell" />
                    <asp:BoundField DataField="Points" HeaderText="Points" InsertVisible="false" SortExpression="Points" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
