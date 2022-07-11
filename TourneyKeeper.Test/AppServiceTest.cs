using Microsoft.VisualStudio.TestTools.UnitTesting;
using TourneyKeeper.DTO.App;
using TourneyKeeper.Web.WebAPI;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class AppServiceTest
    {
        [TestMethod]
        public void CanLogin()
        {
            var dto = new LoginDTO { Login = "Brestfloda", Password = "" };

            var controller = new LoginController();
            var response = controller.Login(dto);
        }
    }
}
