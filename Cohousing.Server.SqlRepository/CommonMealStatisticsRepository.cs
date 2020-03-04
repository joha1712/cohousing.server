using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Repositories;
using Dapper;
// ReSharper disable RedundantAnonymousTypePropertyName

namespace Cohousing.Server.SqlRepository
{
  public class CommonMealStatisticsRepository : ICommonMealStatisticsRepository
    {
        private readonly ISqlRepositoryConnectionFactory _connectionFactory;
        
        public CommonMealStatisticsRepository(ISqlRepositoryConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;            
        }

        public async Task<IImmutableList<CommonMealStatisticOverview>> GetOverviewStatistics(DateTime fromDate, DateTime toDate)
        {
            const string query = " SELECT personid, chefcount, mealcount, adultguestscount, childguestscount, expensessum " +
                " FROM commonmeal_statistics_overview(@DateFrom, @DateTo) ";

            using (var connection = _connectionFactory.New())
            {
                var data = await connection.QueryAsync(query, new { DateFrom = fromDate, DateTo = toDate });
                var result = data.
                    Select(x => new CommonMealStatisticOverview {
                        PersonId = x.personid,
                        ChefCount = x.chefcount,
                        MealCount = x.mealcount,
                        AdultGuestsCount = x.adultguestscount,
                        ChildGuestsCount = x.childguestscount,
                        Cost = new CommonMealStatisticCostSumOverview {
                            ExpensesSum = Convert.ToDecimal(x.expensessum)
                        }                        
                    });
                return result.ToImmutableList();
            }
        }        
    }    
}