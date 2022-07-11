namespace TourneyKeeper.DTO.App
{
    public class VerifyLoginDTO : BaseDTO
    {
        public string Token { get; set; }

        public override string Describe()
        {
            return $"{Token}";
        }
    }
}