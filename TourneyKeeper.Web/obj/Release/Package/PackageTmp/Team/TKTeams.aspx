<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKTeams.aspx.cs" Inherits="TourneyKeeper.Web.TKTeams" MasterPageFile="TKTeam.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#adminli").addClass("active");
        $(document).ready(function () {
            $(":button,:submit,:input").removeAttr("disabled");
        });
    </script>

    <asp:PlaceHolder runat="server" ID="addPlayerPlaceHolder">
        <div class="row-fluid">
            <div class="col-md-12 padding-0">
                <asp:Label runat="server" Text="Team name:"></asp:Label>
                <asp:TextBox runat="server" ID="teamNameTextBox" Width="250" onfocus="this.value = ''"></asp:TextBox>
                <asp:Button disabled="disabled" runat="server" ID="AddTeamButton" OnClick="AddTeamButtonClick" Text="Add Team" CssClass="btn btn-large btn-primary" />
            </div>
        </div>
    </asp:PlaceHolder>
    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="" ID="TeamSize" ClientIDMode="Static" />
            <asp:GridView ID="LeaderboardGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" OnRowCommand="LeaderboardGridViewRowCommand"
                AutoGenerateEditButton="false" GridLines="None">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:HiddenField ID="TeamId" Value='<%#Eval("Id") %>' runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Team">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Name" Text='<%#Eval("Name") %>' Width="250" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentTeam/Update", "Name", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Paid" ItemStyle-HorizontalAlign="Center" SortExpression="Paid">
                        <ItemTemplate>
                            <div class="form-check">
                                <input disabled="disabled" type="checkbox" <%#((bool)Eval("Paid")) ? "checked" : ""%> class="form-check-input" id='<%#"paidCheckBox" + Eval("Id").ToString()%>' onchange='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentPlayer/Update", "Paid", Eval("Id"), "this.checked", "this", Eval("Token"))%>'>
                                <label class="form-check-label" for='<%#"paidCheckBox" + Eval("Id").ToString()%>'></label>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Players">
                        <ItemTemplate>
                            <asp:Label runat="server" PlayerTag="x" ID="Players" Text='<%#Eval("Players") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Non-players">
                        <ItemTemplate>
                            <asp:Label runat="server" NonPlayerTag="x" ID="NonPlayers" Text='<%#Eval("NonPlayers") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="BattlePoint Penalty" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="BattlePointPenalty" Text='<%#Bind("BattlePointPenalty") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentTeam/Update", "BattlePointPenalty", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MatchPoint Penalty" ItemStyle-HorizontalAlign="Right">
                        <ItemTemplate>
                            <asp:TextBox disabled="disabled" runat="server" ID="Penalty" Text='<%#Bind("Penalty") %>' type="number" Width="40" onblur='<%# string.Format("javascript:UpdateField(\"{0}\", \"{1}\", {2}, {3}, {4}, \"{5}\");", "/WebAPI/TournamentTeam/Update", "Penalty", Eval("Id"), "this.value", "this", Eval("Token"))%>'></asp:TextBox>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" InsertVisible="false">
                        <ItemTemplate>
                            <asp:Button disabled="disabled" runat="server" ID="RemoveTeamButton" UseSubmitBehavior="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="RemoveTeam" Text="Remove Team" CssClass="btn btn-sm btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('span[PlayerTag="x"]').each(function () {
                if ($(this).html() != TeamSize.value) {
                    $(this).parent().css('background-color', 'red');
                }
                else {
                    $(this).parent().css('background-color', 'green');
                }
            });
        });
    </script>
</asp:Content>
