namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealChefViewModel
    {
        public int Id { get; set; }
        public int? PersonId { get; set; }
        public int CommonMealId { get; set; }
        public string PersonName { get; set; }
    }
}