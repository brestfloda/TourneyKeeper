namespace TourneyKeeper.DTO.App
{
    public class GamesResponseDTO : BaseDTO
    {
        public GameDTO[] Games { get; set; }

        public override string Describe()
        {
            return $"";
        }
    }
}