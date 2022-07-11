using System;

namespace TourneyKeeper.DTO.App
{
    public class RegisterClientDTO : BaseDTO
    {
        public string PlayerToken { get; set; }
        public string ClientToken { get; set; }

        public override string Describe()
        {
            return $"{PlayerToken}, {ClientToken}";
        }
    }
}