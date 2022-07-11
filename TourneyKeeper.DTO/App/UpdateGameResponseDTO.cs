namespace TourneyKeeper.DTO.App
{
    public class UpdateGameResponseDTO : BaseDTO
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public override string Describe()
        {
            return $"{Status}, {Message}";
        }
    }
}