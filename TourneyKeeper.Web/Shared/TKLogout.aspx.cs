using System;

namespace TourneyKeeper.Web
{
    public partial class TKLogout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["LoggedInUser"] = null;
            var cookie = Request.Cookies["TKUser"];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }

            Response.Redirect("/default.aspx");
        }
    }
}