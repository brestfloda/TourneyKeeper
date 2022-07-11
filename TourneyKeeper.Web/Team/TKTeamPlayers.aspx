<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeamPlayers.aspx.cs" Inherits="TourneyKeeper.Web.TKTeamPlayers" MasterPageFile="TKTeam.master" %>

<%@ Register Src="~/Controls/Army.ascx" TagPrefix="uc3" TagName="Army" %>
<%@ Register Src="~/Controls/SearchPlayer.ascx" TagPrefix="uc4" TagName="SearchPlayer" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <uc3:Army runat="server" ID="Army" />

    <input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
    <input type="hidden" id="_search" value="<%=FromSubmit%>" />
    <script type="text/javascript">
        $("#adminli").addClass("active");

        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mt-0');
            $('input.select-dropdown').addClass('mb-0');
            $('#addPlayerModal').on('shown.bs.modal', function () {
                $('#playerNameTextbox').focus();
            })

            $(":button,:submit,:input").removeAttr("disabled");
        });

        $(document).keypress(function (e) {
            if ($('#addPlayerModal').is(':visible') && (e.keycode == 13 || e.which == 13)) {
                SearchClick();
            }
        });
    </script>

    <uc4:SearchPlayer runat="server" ID="SearchPlayer" />

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:GridView ID="TournamentPlayersGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped valign-important" OnRowCommand="TournamentPlayersGridViewRowCommand"
                OnRowDataBound="TournamentPlayersGridView_RowDataBound" DataKeyNames="Id" GridLines="None">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="PlayerId" Value='<%#Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Player">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Name" Text='<%#Eval("PlayerName") %>' onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "PlayerName", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team">
                        <ItemTemplate>
                            <asp:DropDownList ID="teamDropDownList" OnDataBound="TeamDropDownListDataBound" DataTextField="Name" DataValueField="Id" ClientIDMode="Static" runat="server" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" AppendDataBoundItems="true">
                                <asp:ListItem Text="None" Value="null"></asp:ListItem>
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Paid" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="form-check">
                                <input disabled="disabled" type="checkbox" <%#((bool)Eval("Paid")) ? "checked" : ""%> class="form-check-input" id='<%#"paidCheckBox" + Eval("Id").ToString()%>' onchange='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Paid", Eval("Id"), "this.checked", "this", Eval("Token"))%>'>
                                <label class="form-check-label" for='<%#"paidCheckBox" + Eval("Id").ToString()%>'></label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Non-player" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="form-check">
                                <input disabled="disabled" type="checkbox" <%#((bool)Eval("NonPlayer")) ? "checked" : ""%> class="form-check-input" id='<%#"nonPlayerCheckBox" + Eval("Id").ToString()%>' onchange='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "NonPlayer", Eval("Id"), "this.checked", "this", Eval("Token"))%>'>
                                <label class="form-check-label" for='<%#"nonPlayerCheckBox" + Eval("Id").ToString()%>'></label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Don't Rank" ItemStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <div class="form-check">
                                <input disabled="disabled" type="checkbox" <%#((bool)Eval("DoNotRank")) ? "checked" : ""%> class="form-check-input" id='<%#"doNotRankCheckBox" + Eval("Id").ToString()%>' onchange='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "DoNotRank", Eval("Id"), "this.checked", "this", Eval("Token"))%>'>
                                <label class="form-check-label" for='<%#"doNotRankCheckBox" + Eval("Id").ToString()%>'></label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Club" SortExpression="Club">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Club" Text='<%#Bind("Club") %>' onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Club", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fairplay" ItemStyle-HorizontalAlign="Right" SortExpression="Fairplay">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="FairPlay" Text='<%#Bind("FairPlay") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "FairPlay", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quiz" ItemStyle-HorizontalAlign="Right" SortExpression="Quiz">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Quiz" Text='<%#Bind("Quiz") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Quiz", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Painting" ItemStyle-HorizontalAlign="Right" SortExpression="Painting">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Painting" Text='<%#Bind("Painting") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Painting", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Penalty" ItemStyle-HorizontalAlign="Right" SortExpression="Penalty">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Penalty" Text='<%#Bind("Penalty") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Penalty", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Army">
                        <ItemTemplate>
                            <a id='<%# $"alink{DataBinder.Eval(Container.DataItem, "Id")}" %>' href='#'><%# string.IsNullOrEmpty( Eval("ArmyList").ToString())?"Blank": Eval("ArmyList").ToString()%></a>
                            <script>$('#alink<%# $"{DataBinder.Eval(Container.DataItem, "Id")}" %>').click(function (e) { { EnterArmy(e, <%# $"{Eval("Id")}" %>,<%# $"{Eval("GameSystemId")}" %>); return false; } });</script>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove player" InsertVisible="false">
                        <ItemTemplate>
                            <asp:Button disabled="disabled" runat="server" ID="RemovePlayerButton" UseSubmitBehavior="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="RemovePlayer" Text="Remove" CssClass="btn btn-sm btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <script type="text/javascript">
</script>
</asp:Content>
