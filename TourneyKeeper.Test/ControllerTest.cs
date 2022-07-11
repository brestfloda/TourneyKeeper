using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using TourneyKeeper.DTO.App;
using TourneyKeeper.Web.WebAPI;

namespace TourneyKeeper.Test
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void GetPlayerArmyDetails()
        {
            var controller = new TournamentPlayerController();
            var response = controller.GetPlayerArmyDetails(43126);
            var tmp = response.Content.ReadAsStringAsync().Result;
            var responseContent = JsonConvert.DeserializeObject<ResponseDTO>(tmp);
            var message = JsonConvert.DeserializeObject<DTO.Web.PlayerArmyDetailsDTO>(responseContent.Message);
        }

        [TestMethod]
        public void PlayerSearch()
        {
            var controller = new PlayerController();
            var response = controller.Search(new DTO.Web.PlayerSearchDTO { TournamentId = 34, SearchString = "Niels" });
            var tmp = response.Content.ReadAsStringAsync().Result;
            var responseContent = JsonConvert.DeserializeObject<ResponseDTO>(tmp);
            var message = JsonConvert.DeserializeObject<List<DTO.Web.PlayerSearchResultDTO>>(responseContent.Message);
        }

        [TestMethod]
        public void GetOpponents()
        {
            var controller = new TournamentPlayerController();
            var response = controller.GetOpponents(new GetOpponentsDTO { PlayerToken = "328d7bd7-21be-467c-91f3-c6c164584027" });
            var tmp = response.Content.ReadAsStringAsync().Result;
            var responseContent = JsonConvert.DeserializeObject<ResponseDTO>(tmp);
            var message = JsonConvert.DeserializeObject<GetOpponentsResponseDTO>(responseContent.Message);
        }

        [TestMethod]
        public void GetGames()
        {
            var controller = new GameController();
            var response = controller.GetGames(new GamesDTO { Token = "328d7bd7-21be-467c-91f3-c6c164584027" });
            var tmp = response.Content.ReadAsStringAsync().Result;
            var responseContent = JsonConvert.DeserializeObject<ResponseDTO>(tmp);
            var message = JsonConvert.DeserializeObject<GamesResponseDTO>(responseContent.Message);

            var controller2 = new TournamentPlayerController();
            var response2 = controller2.GetOpponents(new GetOpponentsDTO { PlayerToken = "328d7bd7-21be-467c-91f3-c6c164584027" });
            var tmp2 = response2.Content.ReadAsStringAsync().Result;
            var responseContent2 = JsonConvert.DeserializeObject<ResponseDTO>(tmp2);
            var message2 = JsonConvert.DeserializeObject<GetOpponentsResponseDTO>(responseContent2.Message);
        }
    }
}
