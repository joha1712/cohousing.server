namespace Cohousing.Server.Service
{
    public interface ICommonMealPriceSettings {
        decimal GetAdultPrice();
        decimal GetChildPrice();
    }
}