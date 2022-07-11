<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Army.ascx.cs" Inherits="TourneyKeeper.Web.Controls.Army" %>

<script>
    $(document).ready(function () {
        $('input.select-dropdown').addClass('mb-0');
    });

    function EnterArmy(e, tpid, gamesystemId) {
        $('#armyModal').modal({
            'backdrop': 'static'
        });

        $('#aId').val(e.currentTarget.id);
        $('#tournamentPlayerId').val(tpid);

        var primaryDropdown = $("#PrimaryCodexDropDownList");
        var secondaryDropdown = $("#SecondaryCodexDropDownList");
        var tertiaryDropdown = $("#TertiaryCodexDropDownList");
        var quaternaryDropdown = $("#QuaternaryCodexDropDownList");

        var codexmodel = {
            gamesystemId: gamesystemId
        };

        if ($("#PrimaryCodexDropDownList option").length < 1) {
            $.getJSON('/webapi/codex/GetCodices', codexmodel)
                .done(function (res) {
                    if (!res.Success) {
                        alert('Could not get codices from server');
                    }
                    else {
                        primaryDropdown.append($("<option />").text("Select your primary codex"));
                        secondaryDropdown.append($("<option />").text("Select your secondary codex"));
                        tertiaryDropdown.append($("<option />").text("Select your tertiary codex"));
                        quaternaryDropdown.append($("<option />").text("Select your quaternary codex"));
                        var data = JSON.parse(res.Message);
                        $.each(data, function (item, obj) {
                            primaryDropdown.append($("<option />").val(obj.Id).text(obj.Name));
                            secondaryDropdown.append($("<option />").val(obj.Id).text(obj.Name));
                            tertiaryDropdown.append($("<option />").val(obj.Id).text(obj.Name));
                            quaternaryDropdown.append($("<option />").val(obj.Id).text(obj.Name));
                        });

                        FillArmy(tpid);
                    }
                });
        }
        else {
            $('#PrimaryCodexDropDownList').val(-1);
            $('#SecondaryCodexDropDownList').val(-1);
            $('#TertiaryCodexDropDownList').val(-1);
            $('#QuaternaryCodexDropDownList').val(-1);
            $('#ArmyListTextBox').val("");

            FillArmy(tpid);
        }
    }

    function FillArmy(tpid) {
        var armymodel = {
            tournamentPlayerId: tpid
        };

        $.getJSON('/webapi/tournamentplayer/GetPlayerArmyDetails', armymodel)
            .done(function (res) {
                if (!res.Success) {
                    alert('Could not get player from server');
                }
                else {
                    var data = JSON.parse(res.Message);
                    $('#PrimaryCodexDropDownList').val(data.PrimaryId);
                    if ($('#PrimaryCodexDropDownList option:selected').text() === '') {
                        $("#PrimaryCodexDropDownList").val($("#PrimaryCodexDropDownList option:first").val());
                    }
                    $('#SecondaryCodexDropDownList').val(data.SecondaryId);
                    if ($('#SecondaryCodexDropDownList option:selected').text() === '') {
                        $("#SecondaryCodexDropDownList").val($("#SecondaryCodexDropDownList option:first").val());
                    }
                    $('#TertiaryCodexDropDownList').val(data.TertiaryId);
                    if ($('#TertiaryCodexDropDownList option:selected').text() === '') {
                        $("#TertiaryCodexDropDownList").val($("#TertiaryCodexDropDownList option:first").val());
                    }
                    $('#QuaternaryCodexDropDownList').val(data.QuaternaryId);
                    if ($('#QuaternaryCodexDropDownList option:selected').text() === '') {
                        $("#QuaternaryCodexDropDownList").val($("#QuaternaryCodexDropDownList option:first").val());
                    }
                    $('#ArmyListTextBox').val(data.Army);
                }
            });
    }

    function SubmitArmy() {
        var primaryValue = $("#PrimaryCodexDropDownList").val();
        var secondaryValue = $("#SecondaryCodexDropDownList").val();
        var tertiaryValue = $("#TertiaryCodexDropDownList").val();
        var quaternaryValue = $("#QuaternaryCodexDropDownList").val();
        var tpid = $('#tournamentPlayerId').val();
        var army = $('#ArmyListTextBox').val();

        var armymodel = {
            TournamentPlayerId: tpid,
            PrimaryId: primaryValue,
            SecondaryId: secondaryValue,
            TertiaryId: tertiaryValue,
            QuaternaryId: quaternaryValue,
            Army: army
        };

        $.post('/webapi/tournamentplayer/SetPlayerArmyDetails', armymodel)
            .done(function (res) {
                if (!res.Success) {
                    alert("Could not set army");
                }
                else {
                    var tmp = $("#PrimaryCodexDropDownList option:selected").text();
                    if (!tmp) {
                        tmp = 'blank';
                    }
                    $('#' + $('#aId').val()).text(tmp);
                    CloseModal('#armyModal');
                }
            });
    }
</script>

<div class="modal fade" id="armyModal" tabindex="-1" role="dialog" aria-labelledby="modalLabel" aria-hidden="true">
    <div class="modal-dialog" role="document">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="modalLabel">Enter your army</h5>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <asp:HiddenField ID="aId" ClientIDMode="Static" runat="server" />
                <asp:HiddenField ID="tournamentPlayerId" ClientIDMode="Static" runat="server" />
                <div class="row-fluid">
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DropDownList ID="PrimaryCodexDropDownList" ClientIDMode="Static" CssClass="mdb-select colorful-select dropdown-primary md-form" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DropDownList ID="SecondaryCodexDropDownList" ClientIDMode="Static" CssClass="mdb-select colorful-select dropdown-primary md-form" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DropDownList ID="TertiaryCodexDropDownList" ClientIDMode="Static" CssClass="mdb-select colorful-select dropdown-primary md-form" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:DropDownList ID="QuaternaryCodexDropDownList" ClientIDMode="Static" CssClass="mdb-select colorful-select dropdown-primary md-form" runat="server" DataValueField="Id" DataTextField="Name"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-md-12">
                            <asp:TextBox runat="server" ClientIDMode="Static" TextMode="MultiLine" ID="ArmyListTextBox" CssClass="form-control" Height="200"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>
            <div class="modal-footer">
                <a href="javascript:SubmitArmy();" class="btn btn-primary">Ok</a>
                <a href="javascript:CloseModal('#armyModal');" class="btn btn-primary">Close</a>
            </div>
        </div>
    </div>
</div>
