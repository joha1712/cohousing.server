namespace Cohousing.Server.Api.ViewModels
{
    public class CommonMealGuestRegistrationViewModel {
        public int Id { get; set;}
        public int RegistrationId { get; set;}
        public PersonGroupViewModel Adults { get; set;}
        public PersonGroupViewModel Children { get; set;}
    }
}