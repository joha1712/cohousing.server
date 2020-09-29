namespace Cohousing.Server.Service.Interfaces
{
    public interface ICommonMealPriceSettings {
        decimal GetAdultPrice();
        decimal GetChildPrice();
    }
}