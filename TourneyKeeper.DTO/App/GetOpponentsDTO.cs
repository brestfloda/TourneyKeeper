namespace TourneyKeeper.DTO.App
{
    public class GetOpponentsDTO : BaseDTO
    {
        public string PlayerToken { get; set; }

        public override string Describe()
        {
            return $"{PlayerToken}";
        }
    }
}