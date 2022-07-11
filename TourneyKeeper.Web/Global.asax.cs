using System;
using System.Web.Http;
using System.Web.Routing;
using TourneyKeeper.Common.Managers;
using TourneyKeeper.Common.SharedCode;

namespace TourneyKeeper.Web
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            RouteTable.Routes.MapHttpRoute(
                "API Default",
                "WebAPI/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional });
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.Url.Host.Equals("localhost", StringComparison.InvariantCultureIgnoreCase))
                return;

            switch (Request.Url.Scheme)
            {
                case "https":
                    Response.AddHeader("Strict-Transport-Security", "max-age=300");
                    break;
                case "http":
                    var path = "https://" + Request.Url.Host + Request.Url.PathAndQuery;
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", path);
                    break;
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            try
            {
                var ex = Server.GetLastError();
                if (ex.InnerException != null)
                {
                    LogManager.LogError($"Inner: {ex.InnerException.Message} - {ex.InnerException.StackTrace} Exception: {ex.Message} - {ex.StackTrace}");
                }
                else
                {
                    LogManager.LogError($"{ex.Message} - {ex.StackTrace}");
                }
                Response.Redirect("/");
            }
            catch
            {
                //end of the road mofo!
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}