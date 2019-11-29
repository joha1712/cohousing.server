namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealRegistrationViewModel
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public string PersonName { get; set; }
        public bool Attending { get; set; }
        public int RegNo { get; set; }
    }
}