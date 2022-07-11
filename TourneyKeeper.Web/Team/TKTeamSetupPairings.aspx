<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamSetupPairings.aspx.cs" Inherits="TourneyKeeper.Web.SetupTeamPairings" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mt-0');
            $('input.select-dropdown').addClass('mb-0');
        });

        function CheckResultCallBack(control, result) {
            if (result.Success) {
                warningsLabel.innerText = "";
            }
            else {
                control.selectedIndex = 0;
                warningsLabel.innerText = result.Message;

                control.animate([{ backgroundColor: getComputedStyle(control).backgroundColor }, { backgroundColor: '#ff0000' }], 2000);

                setTimeout(function () {
                    control.animate([{ backgroundColor: getComputedStyle(control).backgroundColor }, { backgroundColor: '#ffffff' }], 2000);
                }, 2000);
            }
        }
    </script>
    <asp:PlaceHolder runat="server" ID="penaltyPlaceHolder">
        <asp:HiddenField ID="teamMatchIdHidden" runat="server"></asp:HiddenField>

        <div class="row">
            <div class="col-md-2">
                <div class="md-form">
                    <asp:TextBox ID="team1PenaltyTextBox" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                    <label for="team1PenaltyTextBox">Team1 penalty</label>
                </div>
            </div>
            <div class="col-md-2">
                <div class="md-form">
                    <asp:TextBox ID="team2PenaltyTextBox" ClientIDMode="Static" CssClass="form-control" runat="server" autofocus="true"></asp:TextBox>
                    <label for="team2PenaltyTextBox">Team2 penalty</label>
                </div>
            </div>
            <div class="col-md-3">
                <asp:LinkButton ID="LinkButton1" OnClick="PenaltyButtonClick" runat="server" Text="Submit penalty" CssClass="btn btn-primary" />
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:Label runat="server" ForeColor="Red" ID="warningsLabel" ClientIDMode="Static"></asp:Label>
            <asp:GridView runat="server" ID="PairingsGridView" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" GridLines="None"
                AutoGenerateEditButton="false" OnRowDataBound="PairingsGridView_RowDataBound" AllowSorting="true" OnSorting="PairingsGridViewSorting">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="GameId" Value='<%#Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Table" SortExpression="TableNumber">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="TableNumber" CssClass="form-control" Text='<%#Eval("TableNumber") %>' Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/Game/Update", "TableNumber", Eval("Id"), "this.value", "this", Eval("Token"))%>' type="number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 1 player">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="team1PlayersDropDownList" OnDataBound="Team1PlayersDropDownListDataBound" DataTextField="NameAndCodex" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AppendDataBoundItems="true">
                                <asp:ListItem>Select player</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P1 score">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="Player1Result" CssClass="form-control" Text='<%#Eval("Player1Result") %>' Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/Game/Update", "Player1Result", Eval("Id"), "this.value", "this", Eval("Token"))%>' type="number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P1 sec. score">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="Player1SecondaryResult" CssClass="form-control" Text='<%#Eval("Player1SecondaryResult") %>' Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/Game/Update", "Player1SecondaryResult", Eval("Id"), "this.value", "this", Eval("Token"))%>' type="number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 2 player">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="team2PlayersDropDownList" OnDataBound="Team2PlayersDropDownListDataBound" DataTextField="NameAndCodex" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AppendDataBoundItems="true">
                                <asp:ListItem>Select player</asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P2 score">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="Player2Result" CssClass="form-control" Text='<%#Eval("Player2Result") %>' Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/Game/Update", "Player2Result", Eval("Id"), "this.value", "this", Eval("Token"))%>' type="number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P2 sec. score">
                        <ItemTemplate>
                            <asp:TextBox runat="server" ID="Player2SecondaryResult" CssClass="form-control" Text='<%#Eval("Player2SecondaryResult") %>' Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/Game/Update", "Player2SecondaryResult", Eval("Id"), "this.value", "this", Eval("Token"))%>' type="number"></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <asp:GridView runat="server" ID="readOnlyPairingsGridView" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                DataKeyNames="Id" Visible="false" GridLines="None">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="GameId" Value='<%#Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Table" SortExpression="TableNumber">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("TableNumber") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team1 player">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#Eval("TKTournamentPlayer") != null && !string.IsNullOrEmpty(Eval("TKTournamentPlayer.NameAndCodex").ToString())%>'>
                                <asp:Literal runat="server" Text='<%# Eval("TKTournamentPlayer.NameAndCodex") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#Eval("TKTournamentPlayer") == null%>'>
                                <asp:Literal runat="server" Text='<%# Eval("TKTeamMatch.TKTournamentTeam.Name") %>'></asp:Literal>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P1 score">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("Player1Result") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P1 sec. score">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("Player1SecondaryResult") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team2 player">
                        <ItemTemplate>
                            <asp:Panel runat="server" Visible='<%#Eval("TKTournamentPlayer1") != null && !string.IsNullOrEmpty(Eval("TKTournamentPlayer1.NameAndCodex").ToString())%>'>
                                <asp:Literal runat="server" Text='<%# Eval("TKTournamentPlayer1.NameAndCodex") %>'></asp:Literal>
                            </asp:Panel>
                            <asp:Panel runat="server" Visible='<%#Eval("TKTournamentPlayer1") == null%>'>
                                <asp:Literal runat="server" Text='<%# Eval("TKTeamMatch.TKTournamentTeam1.Name") %>'></asp:Literal>
                            </asp:Panel>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P2 score">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("Player2Result") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="P2 sec. score">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("Player2SecondaryResult") %>'></asp:Literal>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row">
        <div class="col-md-4">
            <asp:DropDownList runat="server" ID="swapPlayer1DropDownList" DataTextField="NameAndCodex" DataValueField="Id" CssClass="mdb-select colorful-select dropdown-primary md-form no-select-margin"></asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:DropDownList runat="server" ID="swapPlayer2DropDownList" DataTextField="NameAndCodex" DataValueField="Id" CssClass="mdb-select colorful-select dropdown-primary md-form no-select-margin"></asp:DropDownList>
        </div>
        <div class="col-md-4">
            <asp:LinkButton runat="server" ID="swapButton" OnClick="SwapClick" Text="Swap players" CssClass="btn btn-primary" />
            <asp:Label runat="server" Visible="false" ForeColor="Red" ID="swapErrorLabel" Text="Illegal swap!"></asp:Label>
        </div>
    </div>
</asp:Content>
