using System.Threading.Tasks;
using Cohousing.Server.Api.Common;
using Cohousing.Server.Api.ViewModels;
using Cohousing.Server.Model;
using Cohousing.Server.Model.Repositories;
using Cohousing.Server.Util;

namespace Cohousing.Server.Api.Mappers
{
    public class CommonMealShoppingInfoMapper : ICommonMealShoppingInfoMapper 
    {
        private readonly ITimeFormatter _timeFormatter;
        private readonly ICommonMealExpenseMapper _commonMealExpenseMapper;
        private readonly ICommonMealRepository _commonMealRepository;

        public CommonMealShoppingInfoMapper(ITimeFormatter timeFormatter, ICommonMealExpenseMapper commonMealExpenseMapper, ICommonMealRepository commonMealRepository)
        {
            _timeFormatter = timeFormatter;
            _commonMealExpenseMapper = commonMealExpenseMapper;
            _commonMealRepository = commonMealRepository;
        }

        public CommonMealShoppingInfo Map(CommonMealShoppingInfoViewModel item)
        {
            return new CommonMealShoppingInfo
            {
                MealId = item.MealId,
                Adults = item.Adults,
                Children = item.Children,
                Vegetarians = item.Vegetarians,
                Budget = item.Children,
                Expenses = _commonMealExpenseMapper.MapMany(item.Expenses)
            };
        }

        public async Task<CommonMealShoppingInfoViewModel> Map(CommonMealShoppingInfo item)
        {
            var meal = await _commonMealRepository.GetById(item.MealId);
            
            return new CommonMealShoppingInfoViewModel
            {
                MealId = item.MealId,
                Date = meal.Date,
                DateName = _timeFormatter.GetDateName(meal.Date),
                DayName = _timeFormatter.GetDayName(meal.Date).ToUpperFirstLetter(),
                Adults = item.Adults,
                Children = item.Children,
                Vegetarians = item.Vegetarians,
                Budget = item.Budget,
                Expenses = await _commonMealExpenseMapper.MapMany(item.Expenses)
            };
        }
    }
}