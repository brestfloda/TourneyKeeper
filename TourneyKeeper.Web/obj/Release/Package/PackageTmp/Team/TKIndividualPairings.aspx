<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKIndividualPairings.aspx.cs" Inherits="TourneyKeeper.Web.IndividualPairings" MasterPageFile="TKTeam.master" %>

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
                AllowSorting="true" OnSorting="PairingsGridViewSorting" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="TableNumber" HeaderText="Table" InsertVisible="false" SortExpression="TableNumber" />
                    <asp:TemplateField HeaderText="Player1" SortExpression="Player1">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) > ((int)Eval("Player2Result")) && int.Parse( Eval("Player1Id").ToString())> 0  %>'>
                                <b>
                                    <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", Eval("Player1Id")) %>' runat="server">
                                        <%# Eval("Player1Name") %>
                                    </a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) > ((int)Eval("Player2Result")) && int.Parse( Eval("Player1Id").ToString())== 0  %>'>
                                <%# Eval("Player1Name") %>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) <= ((int)Eval("Player2Result")) && int.Parse( Eval("Player1Id").ToString())> 0%>'>
                                <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", Eval("Player1Id")) %>' runat="server">
                                    <%# Eval("Player1Name") %>
                                </a>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) <= ((int)Eval("Player2Result")) && int.Parse( Eval("Player1Id").ToString())== 0%>'>
                                <%# Eval("Player1Name") %>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div style="text-align: center;">Result</div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <%# !(bool)DataBinder.Eval(Container.DataItem, "AllowEdit") && !((int)DataBinder.Eval(Container.DataItem, "Player1Result") == 0 && (int)DataBinder.Eval(Container.DataItem, "Player2Result") == 0)? String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "Player1Result"), DataBinder.Eval(Container.DataItem, "Player2Result")):"" %>
                                <asp:HyperLink CssClass="btn btn-sm btn-primary" BackColor="#99bfd8" runat="Server" Visible='<%#(bool)DataBinder.Eval(Container.DataItem, "AllowEdit") && ((int)DataBinder.Eval(Container.DataItem, "Player1Result") == 0 && (int)DataBinder.Eval(Container.DataItem, "Player2Result") == 0)%>' Text="Results" NavigateUrl='<%# string.Format("/Shared/TKEditGame.aspx?GameId={0}&TournamentId={1}", DataBinder.Eval(Container.DataItem, "Id"), DataBinder.Eval(Container.DataItem, "TournamentId")) %>' />
                                <asp:HyperLink runat="Server" Visible='<%#(bool)DataBinder.Eval(Container.DataItem, "AllowEdit") && !((int)DataBinder.Eval(Container.DataItem, "Player1Result") == 0 && (int)DataBinder.Eval(Container.DataItem, "Player2Result") == 0)%>' Text='<%#String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "Player1Result"), DataBinder.Eval(Container.DataItem, "Player2Result")) %>' NavigateUrl='<%# string.Format("/Shared/TKEditGame.aspx?GameId={0}&TournamentId={1}", DataBinder.Eval(Container.DataItem, "Id"), DataBinder.Eval(Container.DataItem, "TournamentId")) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Player2" SortExpression="Player2">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player2Result")) > ((int)Eval("Player1Result")) && int.Parse( Eval("Player2Id").ToString())> 0 %>'>
                                <b>
                                    <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "Player2Id")) %>' runat="server">
                                        <%# Eval("Player2Name") %>
                                    </a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) > ((int)Eval("Player2Result")) && int.Parse( Eval("Player2Id").ToString())== 0  %>'>
                                <%# Eval("Player2Name") %>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player2Result")) <= ((int)Eval("Player1Result"))&&int.Parse( Eval("Player2Id").ToString())> 0 %>'>
                                <a href='<%# String.Format("/Shared/TKGames.aspx?PlayerId={0}", DataBinder.Eval(Container.DataItem, "Player2Id")) %>' runat="server">
                                    <%# Eval("Player2Name") %>
                                </a>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) <= ((int)Eval("Player2Result")) && int.Parse( Eval("Player2Id").ToString())== 0%>'>
                                <%# Eval("Player2Name") %>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" ItemStyle-CssClass="d-none d-sm-table-cell" HeaderStyle-CssClass="d-none d-sm-table-cell">
                        <ItemTemplate>
                            <a href='<%# String.Format("/Shared/TKShowGame.aspx?GameId={0}", DataBinder.Eval(Container.DataItem, "Id")) %>' target="_blank" class="btn btn-sm btn-primary" runat="server">Game
                            </a>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
