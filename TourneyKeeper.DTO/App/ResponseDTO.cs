using System;

namespace TourneyKeeper.DTO.App
{
    public class ResponseDTO : BaseDTO
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public override string Describe()
        {
            return $"{Success}, {Message}";
        }
    }
}