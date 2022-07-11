<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GamesOverview.ascx.cs" Inherits="TourneyKeeper.Web.Controls.GamesOverview" %>

<div class="row-fluid">
    <div class="col-md-12 padding-0">
        <asp:HiddenField runat="server" Value="Initial" ID="sortExpressionHidden" />
        <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
        <asp:GridView ID="GamesGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
            AllowSorting="true" OnSorting="GamesGridViewSorting" GridLines="None">
            <Columns>
                <asp:BoundField DataField="Round" HeaderText="Round" InsertVisible="false" SortExpression="Round" />
                <asp:BoundField DataField="TableNumber" HeaderText="Table" InsertVisible="false" SortExpression="TableNumber" />
                <asp:TemplateField HeaderText="Player1" SortExpression="Player1">
                    <ItemTemplate>
                        <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) > ((int)Eval("Player2Result")) %>'>
                            <b>
                                <%# Eval("TKTournamentPlayer.PlayerName") %>
                            </b>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible='<%#((int)Eval("Player1Result")) <= ((int)Eval("Player2Result")) %>'>
                            <%# Eval("TKTournamentPlayer.PlayerName") %>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <div style="text-align: center;">Result</div>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <div style="text-align: center;">
                            <%# String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "Player1Result"), DataBinder.Eval(Container.DataItem, "Player2Result")) %>
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Player2" SortExpression="Player2">
                    <ItemTemplate>
                        <asp:Panel runat="server" Visible='<%#((int)Eval("Player2Result")) > ((int)Eval("Player1Result")) %>'>
                            <b>
                                <%# DataBinder.Eval(Container.DataItem, "TKTournamentPlayer1.PlayerName") %>
                            </b>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible='<%#((int)Eval("Player2Result")) <= ((int)Eval("Player1Result")) %>'>
                            <%# DataBinder.Eval(Container.DataItem, "TKTournamentPlayer1.PlayerName") %>
                        </asp:Panel>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="LastEdited" HeaderText="Last edited" InsertVisible="false" SortExpression="LastEdited" />
                <asp:BoundField DataField="LastEditedBy" HeaderText="Last edited by" InsertVisible="false" SortExpression="LastEditedBy" />
            </Columns>
        </asp:GridView>
    </div>
</div>
