using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public partial class TKArmylists : System.Web.UI.Page
    {
        public string CanEdit = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request["PlayerId"]);

            if (Session["LoggedInUser"] != null && ((TKPlayer)Session["LoggedInUser"]).Id == id)
            {
                ArmyListsGridView.AutoGenerateEditButton = true;
                CanEdit = "y";
            }
            else
            {
                ArmyListsGridView.AutoGenerateEditButton = false;
                CanEdit = "";
            }

            if (!IsPostBack)
            {
                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    var armyListData = CanEdit.Equals("y") ?
                        context.TKTournamentPlayer
                        .Where(p => p.PlayerId == id && p.TKTournament.Active)
                        .OrderByDescending(p => p.Id)
                        .ToList()
                        .Select(p => new TKTournamentPlayer
                        {
                            Id = p.Id,
                            ArmyList = WebUtility.HtmlEncode(p.ArmyList),
                            TKCodex = p.TKCodex,
                            TKCodex1 = p.TKCodex1,
                            TKCodex2 = p.TKCodex2,
                            TKCodex3 = p.TKCodex3,
                            TKTournament = p.TKTournament
                        })
                        :
                        context.TKTournamentPlayer
                        .Where(p => p.PlayerId == id && p.TKTournament.Active)
                        .ToList()
                        .Where(p => p.TKTournament.ShowListsDate.HasValue ? p.TKTournament.ShowListsDate < DateTime.Now : p.TKTournament.TournamentDate < DateTime.Now || General.IsAdminOrOrganizer(Session["LoggedInUser"] as TKPlayer, p.TournamentId))
                        .OrderByDescending(p => p.Id)
                        .ToList();

                    ArmyListsGridView.DataSource = armyListData;
                    ArmyListsGridView.DataBind();

                    var data = context.TKPlayer.SingleOrDefault(g => g.Id == id);

                    PlayerNameLabel.Text = data == null ? "" : string.Format("{0} ({1}/{2}/{3})", data.Name, data.Wins, data.Losses, data.Draws);
                }
            }
        }

        protected void ArmyListsGridView_RowEditing(object sender, GridViewEditEventArgs e)
        {
            ArmyListsGridView.EditIndex = e.NewEditIndex;

            using (var context = new TourneyKeeperEntities())
            {
#if DEBUG
                context.Database.Log = x => Debug.WriteLine(x);
#endif
                var playerId = ((TKPlayer)Session["LoggedInUser"]).Id;
                var armyListData = context.TKTournamentPlayer
                    .Where(p => p.PlayerId == playerId)
                    .OrderByDescending(p => p.Id)
                    .ToList()
                    .Select(p => new TKTournamentPlayer
                    {
                        Id = p.Id,
                        ArmyList = WebUtility.HtmlEncode(p.ArmyList),
                        TKCodex = p.TKCodex,
                        TKCodex1 = p.TKCodex1,
                        TKCodex2 = p.TKCodex2,
                        TKCodex3 = p.TKCodex3,
                        PrimaryCodexId = p.PrimaryCodexId,
                        SecondaryCodexId = p.SecondaryCodexId,
                        TertiaryCodexId = p.TertiaryCodexId,
                        QuaternaryCodexId = p.QuaternaryCodexId,
                        TKTournament = p.TKTournament
                    });

                ArmyListsGridView.DataSource = armyListData;
                ArmyListsGridView.DataBind();
            }
        }

        protected void ArmyListsGridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var pid = int.Parse(e.Keys["Id"].ToString());
                var tournamentPlayer = context.TKTournamentPlayer.Single(p => p.Id == pid);
                tournamentPlayer.ArmyList = WebUtility.HtmlEncode(((TextBox)ArmyListsGridView.Rows[e.RowIndex].FindControl("ArmyList")).Text);
                tournamentPlayer.PrimaryCodexId = int.Parse(((DropDownList)ArmyListsGridView.Rows[e.RowIndex].FindControl("primaryCodexDropDownList")).SelectedValue);
                tournamentPlayer.SecondaryCodexId = int.Parse(((DropDownList)ArmyListsGridView.Rows[e.RowIndex].FindControl("secondaryCodexDropDownList")).SelectedValue);
                tournamentPlayer.TertiaryCodexId = int.Parse(((DropDownList)ArmyListsGridView.Rows[e.RowIndex].FindControl("tertiaryCodexDropDownList")).SelectedValue);
                tournamentPlayer.QuaternaryCodexId = int.Parse(((DropDownList)ArmyListsGridView.Rows[e.RowIndex].FindControl("quaternaryCodexDropDownList")).SelectedValue);

                context.SaveChanges();

                var playerid = ((TKPlayer)Session["LoggedInUser"]).Id;
                var armyListData = context.TKTournamentPlayer
                    .Where(p => p.PlayerId == playerid)
                    .OrderByDescending(p => p.Id)
                    .ToList()
                    .Select(p => new TKTournamentPlayer
                    {
                        Id = p.Id,
                        ArmyList = WebUtility.HtmlEncode(p.ArmyList),
                        TKCodex = p.TKCodex,
                        TKCodex1 = p.TKCodex1,
                        TKCodex2 = p.TKCodex2,
                        TKCodex3 = p.TKCodex3,
                        PrimaryCodexId = p.PrimaryCodexId,
                        SecondaryCodexId = p.SecondaryCodexId,
                        TertiaryCodexId = p.TertiaryCodexId,
                        QuaternaryCodexId = p.QuaternaryCodexId,
                        TKTournament = p.TKTournament
                    });

                ArmyListsGridView.EditIndex = -1;
                ArmyListsGridView.DataSource = armyListData;
                ArmyListsGridView.DataBind();
            }
        }

        protected void ArmyListsGridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var playerId = ((TKPlayer)Session["LoggedInUser"]).Id;
                var armyListData = context.TKTournamentPlayer
                    .Where(p => p.PlayerId == playerId)
                    .OrderByDescending(p => p.Id)
                    .ToList()
                    .Select(p => new TKTournamentPlayer
                    {
                        Id = p.Id,
                        ArmyList = WebUtility.HtmlEncode(p.ArmyList),
                        TKCodex = p.TKCodex,
                        TKCodex1 = p.TKCodex1,
                        TKCodex2 = p.TKCodex2,
                        TKCodex3 = p.TKCodex3,
                        PrimaryCodexId = p.PrimaryCodexId,
                        SecondaryCodexId = p.SecondaryCodexId,
                        TertiaryCodexId = p.TertiaryCodexId,
                        QuaternaryCodexId = p.QuaternaryCodexId,
                        TKTournament = p.TKTournament
                    });

                ArmyListsGridView.EditIndex = -1;
                ArmyListsGridView.DataSource = armyListData;
                ArmyListsGridView.DataBind();
            }
        }

        protected void ArmyListsGridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && ArmyListsGridView.EditIndex == e.Row.RowIndex)
            {
                using (var context = new TourneyKeeperEntities())
                {
#if DEBUG
                    context.Database.Log = x => Debug.WriteLine(x);
#endif
                    TKTournamentPlayer player = e.Row.DataItem as TKTournamentPlayer;

                    var codices = context.TKCodex.Where(c => c.Active.Value && c.GameSystemId == player.TKTournament.GameSystemId).OrderBy(o => o.Name).ToList();
                    var primaryCodex = (DropDownList)e.Row.FindControl("primaryCodexDropDownList");
                    primaryCodex.DataSource = codices;
                    primaryCodex.DataBind();
                    var secondaryCodex = (DropDownList)e.Row.FindControl("secondaryCodexDropDownList");
                    secondaryCodex.DataSource = codices;
                    secondaryCodex.DataBind();
                    var tertiaryCodex = (DropDownList)e.Row.FindControl("tertiaryCodexDropDownList");
                    tertiaryCodex.DataSource = codices;
                    tertiaryCodex.DataBind();
                    var quaternaryCodex = (DropDownList)e.Row.FindControl("quaternaryCodexDropDownList");
                    quaternaryCodex.DataSource = codices;
                    quaternaryCodex.DataBind();
                    if (player.TKCodex != null)
                    {
                        primaryCodex.SelectedValue = player.PrimaryCodexId.ToString();
                    }
                    if (player.TKCodex1 != null)
                    {
                        secondaryCodex.SelectedValue = player.SecondaryCodexId.ToString();
                    }
                    if (player.TKCodex2 != null)
                    {
                        tertiaryCodex.SelectedValue = player.TertiaryCodexId.ToString();
                    }
                    if (player.TKCodex3 != null)
                    {
                        quaternaryCodex.SelectedValue = player.QuaternaryCodexId.ToString();
                    }
                }
            }
        }
    }
}