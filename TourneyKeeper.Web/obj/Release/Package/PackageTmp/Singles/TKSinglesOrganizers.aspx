<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TKSinglesOrganizers.aspx.cs" Inherits="TourneyKeeper.Web.TKSinglesOrganizers" MasterPageFile="TKSingles.master" %>
<%@ Register Src="~/Controls/SearchOrganizer.ascx" TagPrefix="uc4" TagName="SearchOrganizer" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <input type="hidden" id="_ispostback" value="<%=Page.IsPostBack.ToString()%>" />
    <input type="hidden" id="_search" value="<%=FromSubmit%>" />
    <script type="text/javascript">
        $("#adminli").addClass("active");

        $(document).ready(function () {
            $('#addPlayerModal').on('shown.bs.modal', function () {
                $('#playerNameTextbox').focus();
            })
        });

        $(document).keypress(function (e) {
            if ($('#addPlayerModal').is(':visible') && (e.keycode == 13 || e.which == 13)) {
                SearchClick();
            }
        });
    </script>

    <uc4:SearchOrganizer runat="server" ID="SearchOrganizer" />

    <div class="row-fluid">
        <div class="col-md-12 padding-0">
            <asp:HiddenField runat="server" Value="Initial" ID="sortExpressionHidden" />
            <asp:HiddenField runat="server" Value="Ascending" ID="sortDirectionHidden" />
            <asp:GridView ID="OrganizerGridView" runat="server" AutoGenerateColumns="false" CssClass="table table-condensed table-striped" OnRowCommand="OrganizerGridViewRowCommand"
                DataKeyNames="Id" GridLines="None">
                <Columns>
                    <asp:TemplateField HeaderText="Organizer" SortExpression="OrganizerName">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="Organizer" Text='<%#Bind("OrganizerName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Remove" InsertVisible="false">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" ID="RemoveOrganizerButton" UseSubmitBehavior="false" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Id") %>' CommandName="RemoveOrganizer" Text="Remove Organizer" CssClass="btn btn-sm btn-primary" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>
