namespace TourneyKeeper.DTO.App
{
    public class LoginDTO : BaseDTO
    {
        public string Login { get; set; }
        public string Password { get; set; }

        public override string Describe()
        {
            return $"{Login}, {Password}";
        }
    }
}