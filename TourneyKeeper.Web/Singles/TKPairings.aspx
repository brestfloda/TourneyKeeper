<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKPairings.aspx.cs" Inherits="TourneyKeeper.Web.Pairings" MasterPageFile="TKSingles.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#gamesAndPairingsli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('input.select-dropdown').addClass('mb-0');
        });
    </script>
    <div class="row-fluid">
        <div class="col-md-2">
            <asp:DropDownList ID="RoundDropDown" ClientIDMode="Static" runat="server" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AutoPostBack="true" OnSelectedIndexChanged="RoundDropDown_SelectedIndexChanged">
            </asp:DropDownList>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="PairingsGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" OnRowCreated="PairingsGridViewRowCreated"
                AllowSorting="true" OnSorting="PairingsGridViewSorting" OnInit="GridViewInit" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="TableNumber" HeaderText="Table" InsertVisible="false" SortExpression="TableNumber" />
                    <asp:TemplateField HeaderText="Player1" SortExpression="Player1">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%# hideResultsforRound ? false: ((int)Eval("Player1Result")) > ((int)Eval("Player2Result")) %>'>
                                <b>
                                    <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", Eval("Player1Id")) %>' runat="server">
                                        <%# Eval("Player1Name") %>
                                    </a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#hideResultsforRound ? true: ((int)Eval("Player1Result")) <= ((int)Eval("Player2Result")) %>'>
                                <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", Eval("Player1Id")) %>' runat="server">
                                    <%# Eval("Player1Name") %>
                                </a>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div style="text-align: center;">Result</div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <%-- Show results - cannot edit --%>
                                <%# (!hideResultsforRound && !editResults && !((int)Eval("Player1Result") == 0 && (int)Eval("Player2Result") == 0)) || ((((int)Eval("Player1Id")==(int)Eval("PlayerId"))||((int)Eval("Player2Id")==(int)Eval("PlayerId"))) && (bool)Eval("IsCurrentRound") && hideResultsforRound && !editResults) ? String.Format("{0} - {1}", Eval("Player1Result"), Eval("Player2Result")):"" %>
                                <%-- No results - show edit button --%>
                                <asp:HyperLink CssClass="btn btn-sm btn-primary" BackColor="#99bfd8" runat="Server"
                                    Visible='<%#!(((((int)Eval("Player1Id")==(int)Eval("PlayerId"))||((int)Eval("Player2Id")==(int)Eval("PlayerId"))) && (bool)Eval("IsCurrentRound") && hideResultsforRound && !editResults)) &&
                                        (((editResults && hideResultsforRound)) || (editResults && ((int)Eval("Player1Result") == 0 && (int)Eval("Player2Result") == 0)))%>' Text="Results" NavigateUrl='<%# string.Format("/Shared/TKEditGame.aspx?GameId={0}&TournamentId={1}", Eval("Id"), Eval("TournamentId")) %>' />
                                <%-- Show results - allow edit --%>
                                <asp:HyperLink runat="Server"
                                    Visible='<%#((((int)Eval("Player1Id")==(int)Eval("PlayerId"))||((int)Eval("Player2Id")==(int)Eval("PlayerId"))) && (bool)Eval("IsCurrentRound") && hideResultsforRound && editResults) ||
                                        (!hideResultsforRound && editResults && !((int)Eval("Player1Result") == 0 && (int)Eval("Player2Result") == 0))%>' Text='<%#String.Format("{0} - {1}", Eval("Player1Result"), Eval("Player2Result")) %>' NavigateUrl='<%# string.Format("/Shared/TKEditGame.aspx?GameId={0}&TournamentId={1}", Eval("Id"), Eval("TournamentId")) %>' />
                                <%-- Show nothing --%>
                                <asp:Label runat="Server"
                                    Visible='<%#!(((((int)Eval("Player1Id")==(int)Eval("PlayerId"))||((int)Eval("Player2Id")==(int)Eval("PlayerId"))) && (bool)Eval("IsCurrentRound") && hideResultsforRound && !editResults)) && (hideResultsforRound && !editResults)%>' Text="N/A" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Player2" SortExpression="Player2">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%# hideResultsforRound ? false: ((int)Eval("Player2Result")) > ((int)Eval("Player1Result")) %>'>
                                <b>
                                    <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "Player2Id")) %>' runat="server">
                                        <%# DataBinder.Eval(Container.DataItem, "Player2Name") %>
                                    </a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#hideResultsforRound ? true:((int)Eval("Player2Result")) <= ((int)Eval("Player1Result")) %>'>
                                <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "Player2Id")) %>' runat="server">
                                    <%# DataBinder.Eval(Container.DataItem, "Player2Name") %>
                                </a>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell">
                        <ItemTemplate>
                            <asp:HyperLink Visible='<%#!hideResultsforRound%>' NavigateUrl='<%# String.Format("/Shared/TKShowGame.aspx?GameId={0}", DataBinder.Eval(Container.DataItem, "Id")) %>' class="btn btn-sm btn-primary" target="_blank" runat="server">Game</asp:HyperLink>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
