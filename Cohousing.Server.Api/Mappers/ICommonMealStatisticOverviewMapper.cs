using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Cohousing.Server.Model;

namespace Cohousing.Server.Api.ViewModels
{
    public interface ICommonMealStatisticOverviewMapper
    {   
        Task<CommonMealStatisticOverviewViewModel> Map(CommonMealStatisticOverview item);        
        Task<IImmutableList<CommonMealStatisticOverviewViewModel>> MapMany(IEnumerable<CommonMealStatisticOverview> items);
        
    }
}