using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using TourneyKeeper.Common;
using TourneyKeeper.Common.Managers;

namespace TourneyKeeper.Web.Shared
{
    /// <summary>
    /// Summary description for Login
    /// </summary>
    public class Login : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            var login = context.Request["usernameTextbox"];
            var password = context.Request["passwordTextbox"];
            bool rememberMe = (context.Request["RememberMeHiddenField"] ?? "") == "true";

            var playerManager = new PlayerManager();
            var player = playerManager.Logon(login, password);
            if (player != null)
            {
                context.Session["LoggedInUser"] = new TKPlayer()
                {
                    Email = player.Email,
                    Country = player.Country,
                    Name = player.Name,
                    Id = player.Id,
                    Token = player.Token,
                    IsAdmin = player.IsAdmin,
                    TournamentIds = playerManager.GetTournamentIds(player.Id),
                    OrganizerForTournamentIds = playerManager.GetOrganizerTournamentIds(player.Id)
                };

                if (rememberMe)
                {
                    var loginCookie = new HttpCookie("TKUser");
                    loginCookie["Email"] = player.Email;
                    loginCookie["Country"] = player.Country;
                    loginCookie["Name"] = player.Name;
                    loginCookie["Id"] = player.Id.ToString();
                    loginCookie["Token"] = player.Token;
                    loginCookie.Expires = DateTime.Now.AddDays(1000);
                    context.Response.Cookies.Add(loginCookie);
                }

                var url = string.IsNullOrEmpty(context.Request["navigationFrom"]) || context.Request["navigationFrom"].Contains("CreateUser.aspx") ?
                    "/" : context.Request["navigationFrom"];

                context.Response.Redirect(url);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}