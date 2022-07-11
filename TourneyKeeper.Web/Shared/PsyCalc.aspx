<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PsyCalc.aspx.cs" Inherits="TourneyKeeper.Web.PsyCalc" MasterPageFile="TKShared.master" ValidateRequest="false" %>

<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="BodyContent">
    <script type="text/javascript">
        $("#createtournamentli").addClass("active");

        $(document).ready(function () {
            $('.mdb-select').material_select();
        });
    </script>

    <asp:HiddenField ID="numberOfDiceDropDownHiddenField" ClientIDMode="Static" Value="8" runat="server" />
    <asp:HiddenField ID="minimumSuccessesDropDownHiddenField" ClientIDMode="Static" Value="3" runat="server" />
    <asp:HiddenField ID="minimumScoreDropDownHiddenField" ClientIDMode="Static" Value="4" runat="server" />

    <div class="row-fluid">
        <div class="card card-primary mb-2">
            <div class="card-body">
                <div class="row">
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="numberOfDiceDropDownList" onchange="javascript:$('#numberOfDiceDropDownHiddenField').val(this.value);">
                            <asp:Literal ID="numberOfDiceDropDownListLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Number of dice</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="minimumSuccessesDropDownList" onchange="javascript:$('#minimumSuccessesDropDownHiddenField').val(this.value);">
                            <asp:Literal ID="minimumSuccessesDropDownListLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Minimum successes</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <select class="mdb-select md-form colorful-select dropdown-primary" id="minimumScoreDropDownList" onchange="javascript:$('#minimumScoreDropDownHiddenField').val(this.value);">
                            <asp:Literal ID="minimumScoreDropDownListLiteral" runat="server"></asp:Literal>
                        </select>
                        <label class="mdb-main-label">Minimum score</label>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        Probability of cast: 
                            <asp:Literal ID="chanceOfSuccessTextBox" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        Probability of perils: 
                            <asp:Literal ID="chanceOfPerilsTextBox" runat="server"></asp:Literal>
                    </div>
                </div>
                <div class="row">
                    <div class="col-md-6">
                        <div class="col-md-12">
                            <asp:LinkButton runat="server" CssClass="btn btn-large btn-primary" OnClick="CalcClick" Text="Calculate"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
