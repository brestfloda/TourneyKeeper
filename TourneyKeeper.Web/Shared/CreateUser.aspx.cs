using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web
{
    public partial class CreateUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            createdOk.Value = "false";

            if (!Page.IsPostBack)
            {
                Country.Countries.ForEach(c => countryDropdown.Items.Add(new ListItem(c)));
                countryDropdown.SelectedValue = "Denmark";
            }
            else
            {
                SignUp();
            }
        }

        private void SignUp()
        {
            signupWarning.Text = "";

            if (string.IsNullOrEmpty(nameTextBox.Text))
            {
                signupWarning.Text = "Name must be filled";
            }

            if (string.IsNullOrEmpty(emailTextBox.Text))
            {
                signupWarning.Text = "E-mail must be filled";
            }

            if (string.IsNullOrEmpty(usernameRegisterTextBox.Text))
            {
                signupWarning.Text += "Username must be filled";
            }

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                signupWarning.Text += "Password must be filled";
            }

            if (!string.IsNullOrEmpty(signupWarning.Text))
            {
                return;
            }

            PlayerManager playerManager = new PlayerManager();

            using (var context = new TourneyKeeperEntities())
            {
                if (context.TKPlayer.Any(p => p.Username.Equals(usernameRegisterTextBox.Text)))
                {
                    signupWarning.Text = "Username exists, pick another";
                    return;
                }

                if (context.TKPlayer.Any(p => p.Email.Equals(emailTextBox.Text)))
                {
                    signupWarning.Text = "Email is already registrered, reset password instead";
                    return;
                }

                var player = new TKPlayer
                {
                    Name = nameTextBox.Text,
                    Country = countryDropdown.SelectedValue,
                    Email = emailTextBox.Text,
                    Password = playerManager.EncodePassword(passwordTextBox.Text),
                    Username = usernameRegisterTextBox.Text
                };
                context.TKPlayer.Add(player);
                context.SaveChanges();
            }

            countryDropdown.SelectedValue = "Denmark";
            nameTextBox.Text = "";
            emailTextBox.Text = "";
            usernameRegisterTextBox.Text = "";
            passwordTextBox.Text = "";
            signupWarning.Text = "";
            createdOk.Value = "true";
        }
    }
}