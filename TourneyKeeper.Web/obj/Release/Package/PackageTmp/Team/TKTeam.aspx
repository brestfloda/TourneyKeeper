<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeam.aspx.cs" Inherits="TourneyKeeper.Web.TKTeam" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");
    </script>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="TeamMatchesGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                OnRowDataBound="TeamMatchesGridViewRowDataBound" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="Round" HeaderText="Round" />
                    <asp:TemplateField HeaderText="Matches">
                        <ItemTemplate>
                            <asp:GridView ID="GamesGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" GridLines="None">
                                <Columns>
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
                                    <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <%# ((int)DataBinder.Eval(Container.DataItem, "Player1Result") == 0 && (int)DataBinder.Eval(Container.DataItem, "Player2Result") == 0) && (int)DataBinder.Eval(Container.DataItem, "Player1SecondaryResult") == 0 && (int)DataBinder.Eval(Container.DataItem, "Player2SecondaryResult") == 0 ? "":String.Format("{0} - {1}", DataBinder.Eval(Container.DataItem, "Player1Result"), DataBinder.Eval(Container.DataItem, "Player2Result")) %>
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
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <a href='<%# String.Format("/Shared/TKShowGame.aspx?GameId={0}", DataBinder.Eval(Container.DataItem, "Id")) %>' target="_blank" class="btn btn-sm btn-primary" runat="server">Game
                                            </a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
