namespace TourneyKeeper.DTO.App
{
    public class EntityUpdateDTO : BaseDTO
    {
        public int Id { get; set; }
        public object Value { get; set; }
        public string FieldToUpdate { get; set; }
        public string Token { get; set; }

        public override string Describe()
        {
            return $"{Id}, {Value}, {FieldToUpdate}, {Token}";
        }
    }
}