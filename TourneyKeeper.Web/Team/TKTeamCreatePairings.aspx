<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamCreatePairings.aspx.cs" Inherits="TourneyKeeper.Web.CreateTeamPairings" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mt-0');
            $('input.select-dropdown').addClass('mb-0');

            $('#TableGenerationHiddenField').val($('#tableGenerationDropDown').val());
            $('#OptionsHiddenField').val($('#optionsDropDown').val());

            var model = {
                tournamentId: getUrlVars().id
            };

            $.getJSON('/webapi/tournament/GetCurrentRound', model)
                .done(function (res) {
                    if (!res.Success) {
                        alert('Could not get current round from server');
                    }
                    else {
                        var data = JSON.parse(res.Message);
                        for (i = data; i > 0; i--) {
                            var selected = $('#RoundHiddenField').val() == '' && i == data ? true : $('#RoundHiddenField').val() == i ? true : $('#RoundHiddenField').val() == '' && i == data;
                            $('#roundDropDown').append(new Option(i, i, selected, false));
                        }
                        $('#RoundHiddenField').val($('#roundDropDown').val());
                    }
                });

            $(":button,:submit,:input").removeAttr("disabled");
        });
    </script>

    <asp:HiddenField ID="RoundHiddenField" ClientIDMode="Static" runat="server" />
    <asp:HiddenField ID="TableGenerationHiddenField" ClientIDMode="Static" runat="server" />

    <div class="modal" tabindex="-1" role="dialog" id="tableModal">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Table generation info</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <p><b>Unique</b></p>
                    <p>TourneyKeeper attempts to pair at tables not played at before.</p>
                    <p><b>Linear</b></p>
                    <p>Tables are generated from the top, e.g. top pairing is always played on table 1.</p>
                    <p><b>Random</b></p>
                    <p>Tables are completely randomly generated.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <div class="row">
                <div class="col-md-12">
                    <asp:Button disabled="disabled" runat="server" ID="createRandomPairingsButton" OnClick="CreateRandomPairingsClick" Text="Create Random Pairings" CssClass="btn btn-large btn-primary" />
                    <asp:Button disabled="disabled" runat="server" ID="createSwissPairingsButton" OnClick="CreateSwissPairingsClick" Text="Create Swiss Pairings" CssClass="btn btn-large btn-primary" />
                    <asp:Button disabled="disabled" ID="deleteLastRoundButton" runat="server" OnClientClick="return confirm('Do you want to delete?')" Text="Delete last round" OnClick="DeleteLastRoundClick" CssClass="btn btn-large btn-primary"></asp:Button>
                    <button type="button" data-toggle="modal" data-target="#tableModal" class="btn btn-default">Help<i class="fas fa-info ml-1"></i></button>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <asp:Label runat="server" ForeColor="Red" ID="warningsLabel"></asp:Label>
                </div>
            </div>
            <div class="row">
                <div class="col-md-4">
                    <select class="mdb-select colorful-select dropdown-primary md-form" id="roundDropDown" onchange="javascript:RoundHiddenField.value = this.value;$('form')[0].submit();">
                    </select>
                    <label class="mdb-main-label">Round</label>
                </div>
                <div class="col-md-4">
                    <select class="mdb-select colorful-select dropdown-primary md-form" id="tableGenerationDropDown" onchange="javascript:$('#TableGenerationHiddenField').val(this.value);">
                        <option value="Unique" selected>Unique</option>
                        <option value="Linear">Linear</option>
                        <option value="Random">Random</option>
                    </select>
                    <label class="mdb-main-label">Table generation</label>
                </div>
            </div>
        </div>
    </div>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView runat="server" ID="PairingsGridView" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" GridLines="None"
                AutoGenerateEditButton="false" OnRowDataBound="PairingsGridView_RowDataBound" AllowSorting="true" OnSorting="PairingsGridViewSorting">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="MatchId" Value='<%#Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Table" SortExpression="TableNumber">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Name" Text='<%#Eval("TableNumber") %>' Width="70" type="number" CssClass="form-control" min="0" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TeamMatch/Update", "TableNumber", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 1">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="team1DropDownList" OnDataBound="Team1DropDownListDataBound" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" searchable="Search here.."></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 1 Penalty" SortExpression="Team1Penalty">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Team1Penalty" Text='<%#Bind("Team1Penalty") %>' CssClass="form-control" type="number" Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TeamMatch/Update", "Team1Penalty", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 2">
                        <ItemTemplate>
                            <asp:DropDownList runat="server" ID="team2DropDownList" OnDataBound="Team2DropDownListDataBound" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect"></asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team 2 Penalty" SortExpression="Team2Penalty">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Team2Penalty" Text='<%#Bind("Team2Penalty") %>' CssClass="form-control" type="number" Width="70" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TeamMatch/Update", "Team2Penalty", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <div class="form-inline">
                <div class="form-group">
                    <asp:DropDownList runat="server" ID="swapTeam1DropDownList" DataTextField="Name" DataValueField="Id" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:DropDownList runat="server" ID="swapTeam2DropDownList" DataTextField="Name" DataValueField="Id" CssClass="form-control"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Button disabled="disabled" runat="server" ID="swapButton" OnClick="SwapClick" Text="Swap teams" CssClass="btn btn-large btn-primary" />
                    <asp:Label runat="server" Visible="false" ForeColor="Red" ID="swapErrorLabel" Text="Illegal swap!"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
