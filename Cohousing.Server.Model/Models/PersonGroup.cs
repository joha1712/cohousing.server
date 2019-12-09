namespace Cohousing.Server.Model.Models
{
    public class PersonGroup {
        public int Total => Conventional + Vegetarians;
        
        public int Conventional { get; set;}
        public int Vegetarians { get; set;}
    }
}