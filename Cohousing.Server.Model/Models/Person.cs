namespace Cohousing.Server.Model.Models
{
    public class Person
    {
        public int Id { get; set; }
        public bool Active { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CallName { get; set; }
        public int AddressId { get; set; }
        public string Attributes {get; set;}

        public bool IsChild() { return Attributes.Contains("CHILD");}
        public bool IsAdult() { return Attributes.Contains("ADULT");}
        public bool IsVegetarian() { return Attributes.Contains("VEGETARIAN");}
    }
}
