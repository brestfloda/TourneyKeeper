using System.Net.Http;

namespace TourneyKeeper.Web.WebAPI
{
    public class JsonMessage : HttpResponseMessage
    {
        public JsonMessage(bool success, string message)
        {
            Content = new JsonContent(new
            {
                Success = success,
                Message = message
            });
        }
    }
}