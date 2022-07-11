using System;
using System.Configuration;
using System.Linq;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web
{
    public partial class TKSettings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int id = int.Parse(Request["PlayerId"]);

            if (!IsPostBack)
            {
                using (var context = new TourneyKeeperEntities())
                {
                    var loggedInPlayer = Session["LoggedInUser"] as TKPlayer;
                    if(loggedInPlayer == null)
                    {
                        Response.Redirect("/");
                    }

                    if(id != loggedInPlayer.Id)
                    {
                        Response.Redirect("/");
                    }

                    var player = context.TKPlayer.Single(p => p.Id == id);

                    Country.Countries.ForEach(c => countryDropdown.Items.Add(new ListItem(c)));
                    countryDropdown.SelectedValue = player.Country;

                    emailTextBox.Text = player.Email;
                    nameTextBox.Text = player.Name;
                    idLabel.Text = player.Id.ToString();
                }
            }
        }

        protected void UpdateClick(object sender, EventArgs e)
        {
            var playerManager = new PlayerManager();

            using (var context = new TourneyKeeperEntities())
            {
                var id = int.Parse(Request["PlayerId"]);
                var player = context.TKPlayer.Single(p => p.Id == id);
                player.Email = emailTextBox.Text;
                player.Name = nameTextBox.Text;
                player.Country = countryDropdown.SelectedValue;

                if (!string.IsNullOrEmpty(password1TextBox.Text) && password1TextBox.Text == password2TextBox.Text)
                {
                    player.Password = playerManager.EncodePassword(password1TextBox.Text);
                }

                var tournamentPlayers = context.TKTournamentPlayer.Where(tp => tp.PlayerId == id);
                foreach (var tp in tournamentPlayers)
                {
                    tp.PlayerName = nameTextBox.Text;
                }

                context.SaveChanges();
            }
        }
    }
}