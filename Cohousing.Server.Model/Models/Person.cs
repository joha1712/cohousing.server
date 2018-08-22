namespace Cohousing.Server.Model.Models
{
    public class Person
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int AddressId { get; set; }
    }
}