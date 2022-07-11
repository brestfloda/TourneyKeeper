namespace TourneyKeeper.DTO.App
{
    public class GamesDTO : BaseDTO
    {
        public string Token { get; set; }

        public override string Describe()
        {
            return $"{Token}";
        }
    }
}