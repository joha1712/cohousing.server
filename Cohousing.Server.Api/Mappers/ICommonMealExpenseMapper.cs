using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Api.Mappers
{
    public interface ICommonMealExpenseMapper
    {
        CommonMealExpense Map(CommonMealExpenseViewModel item);
        IImmutableList<CommonMealExpense> MapMany(IImmutableList<CommonMealExpenseViewModel> items);
        Task<CommonMealExpenseViewModel> Map(CommonMealExpense item);
        Task<IImmutableList<CommonMealExpenseViewModel>> MapMany(IImmutableList<CommonMealExpense> item);
        
    }
}