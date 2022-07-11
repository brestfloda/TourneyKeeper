<%@ Page ValidateRequest="false" Language="C#" AutoEventWireup="true" CodeBehind="TKArmylists.aspx.cs" Inherits="TourneyKeeper.Web.TKArmylists" MasterPageFile="TKShared.master" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#armylistsli").addClass("active");
        $(document).ready(function () {
            $('.mdb-select').materialSelect();
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mb-0');
            $('div.select-wrapper.mdb-select.colorful-select.dropdown-primary.md-form.tableselect').addClass('mt-0');
            $('input.select-dropdown').addClass('mb-0');
        });
    </script>

    <input type="hidden" id="_canedit" value="<%=CanEdit%>" />
    <div class="row-fluid">
        <div class="col-md-12">
            <div style="margin: 5px 5px; padding-top: 3px">
                <h4>
                    <asp:Label ID="PlayerNameLabel" runat="server"></asp:Label></h4>
            </div>
            <!-- TODO -->
            <asp:GridView runat="server" ID="ArmyListsGridView" AutoGenerateColumns="false" CssClass="table table-condensed table-striped"
                DataKeyNames="Id" OnRowEditing="ArmyListsGridView_RowEditing" OnRowUpdating="ArmyListsGridView_RowUpdating"
                OnRowCancelingEdit="ArmyListsGridView_RowCancelingEdit" OnRowDataBound="ArmyListsGridView_RowDataBound" GridLines="None">
                <Columns>
                    <asp:BoundField DataField="TKTournament.Name" ReadOnly="true" HeaderText="Tournament" />
                    <asp:TemplateField HeaderText="Armylist">
                        <ItemTemplate>
                            <asp:Literal ID="ArmyList" runat="server" Text='<%#Bind("ArmyList") %>'></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="ArmyList" Text='<%#Bind("ArmyList") %>' TextMode="MultiLine" Width="100%" Height="200"></asp:TextBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Primary codex">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("TKCodex.Name") %>'></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="primaryCodexDropDownList" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" ></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Secondary codex">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("TKCodex1.Name") %>'></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="secondaryCodexDropDownList" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" ></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tertiary codex">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("TKCodex2.Name") %>'></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="tertiaryCodexDropDownList" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" ></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quaternary codex">
                        <ItemTemplate>
                            <asp:Literal runat="server" Text='<%#Eval("TKCodex3.Name") %>'></asp:Literal>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList runat="server" ID="quaternaryCodexDropDownList" DataTextField="Name" DataValueField="Id" Width="100%" CssClass="mdb-select colorful-select dropdown-primary md-form tableselect" ></asp:DropDownList>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <script type="text/javascript">
        if (document.getElementById('_canedit').value == 'y') {
        }
        else {
            document.getElementById('Settings').style.display = 'none';
            document.getElementById('SettingsLi').style.display = 'none';
        }
    </script>
</asp:Content>
