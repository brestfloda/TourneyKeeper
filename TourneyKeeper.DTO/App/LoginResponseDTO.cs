using System;

namespace TourneyKeeper.DTO.App
{
    public class LoginResponseDTO : BaseDTO
    {
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public override string Describe()
        {
            return $"{Token}, {Name}, {Email}";
        }
    }
}