using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TourneyKeeper.DTO.Web;
using TourneyKeeper.Common;
using TourneyKeeper.Common.SharedCode;
using System.Net;

namespace TourneyKeeper.Web
{
    public partial class TKSinglesArmyListsExport : TKWebPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var id = General.GetParam<int>("Id");
            var player = Session["LoggedInUser"] as TKPlayer;
            General.GuardLogin(id, player, this);

            if (!IsPostBack)
            {
                BindData(id);
            }
        }

        private void BindData(int tournamentId)
        {
            using (var context = new TourneyKeeperEntities())
            {
                var data = context.TKTournamentPlayer
                    .Where(g => g.TournamentId == tournamentId && g.Active && !g.NonPlayer)
                    .ToList()
                    .OrderBy(g => g.TeamName)
                    .ThenBy(g => g.PlayerName)
                    .Select(g => new
                    {
                        ArmyList = string.IsNullOrEmpty(g.ArmyList) ? $"{g.PlayerName} - no list entered" : WebUtility.HtmlEncode(g.ArmyList),
                    });

                var sb = new StringBuilder();

                foreach (var d in data)
                {
                    sb.AppendLine($"{d.ArmyList}");
                    sb.AppendLine();
                }

                ExportDataTextBox.Text = sb.ToString();
            }
        }
    }
}