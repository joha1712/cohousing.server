namespace Cohousing.Server.Model.Models
{
    public class CommonMealGuestRegistration {
        public int Id { get; set;}
        public int RegistrationId { get; set;}
        public PersonGroup Adults { get; set;}
        public PersonGroup Children { get; set;}
    }
}