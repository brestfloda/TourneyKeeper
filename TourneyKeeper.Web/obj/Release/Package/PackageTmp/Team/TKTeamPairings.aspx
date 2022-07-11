<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamPairings.aspx.cs" Inherits="TourneyKeeper.Web.TeamPairings" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#teamPairingsli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mt-0');
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
            <asp:HiddenField ID="RoundHiddenField" ClientIDMode="Static" runat="server" />
            <asp:GridView ID="PairingsGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                OnInit="PairingsGridViewInit" OnRowCreated="PairingsGridViewRowCreated" AllowSorting="true" OnSorting="PairingsGridViewSorting" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="TableNumber" HeaderText="Table" InsertVisible="false" SortExpression="Table" />
                    <asp:TemplateField HeaderText="Team 1" SortExpression="Team1">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Team1Points")) > ((int)Eval("Team2Points")) %>'>
                                <b>
                                    <a href='<%# String.Format("/Team/TKTeam.aspx?TeamId={0}&Id={1}", Eval("Team1Id"), Eval("TournamentId")) %>' runat="server"><%# Eval("Team1Name") %></a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Team1Points")) <= ((int)Eval("Team2Points")) %>'>
                                <a href='<%# String.Format("/Team/TKTeam.aspx?TeamId={0}&Id={1}", Eval("Team1Id"), Eval("TournamentId")) %>' runat="server"><%# Eval("Team1Name") %></a>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Team1Penalty" HeaderText="Penalty" InsertVisible="false" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <div style="text-align: center;">Result</div>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <div style="text-align: center;">
                                <%# Eval("FormattedPoints") %>
                                <%# ((bool)Eval("AllowSetup")) && !string.IsNullOrEmpty(((string)Eval("FormattedPoints")))?"<br/>":"" %>
                                <asp:HyperLink CssClass="btn btn-sm btn-primary" runat="Server" Visible='<%#((bool)Eval("AllowSetup"))%>' Text="Setup" NavigateUrl='<%# String.Format("/Team/TKTeamSetupPairings.aspx?Id={0}&MatchId={1}", Request["Id"], DataBinder.Eval(Container.DataItem, "MatchId")) %>' />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Team2Penalty" HeaderText="Penalty" InsertVisible="false" />
                    <asp:TemplateField HeaderText="Team 2" SortExpression="Team2">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Team2Points")) > ((int)Eval("Team1Points")) %>'>
                                <b>
                                    <a href='<%# String.Format("/Team/TKTeam.aspx?TeamId={0}&Id={1}", Eval("Team2Id"), Eval("TournamentId")) %>' runat="server"><%# Eval("Team2Name") %></a>
                                </b>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#((int)Eval("Team2Points")) <= ((int)Eval("Team1Points")) %>'>
                                <a href='<%# String.Format("/Team/TKTeam.aspx?TeamId={0}&Id={1}", Eval("Team2Id"), Eval("TournamentId")) %>' runat="server"><%# Eval("Team2Name") %></a>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
