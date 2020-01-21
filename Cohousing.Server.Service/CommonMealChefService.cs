using System;
using System.Threading.Tasks;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;
using Cohousing.Server.Model.Repositories;

namespace Cohousing.Server.Service
{
    public class CommonMealChefService : ICommonMealChefService
    {
        private readonly ICommonMealChefRepository _chefRepository;
        private readonly ICommonMealRepository _mealRepository;
        private readonly ITimeProvider _timeprovider;

        public CommonMealChefService(ICommonMealChefRepository chefRepository, ICommonMealRepository mealRepository, ITimeProvider timeprovider)
        {
            _chefRepository = chefRepository;
            _mealRepository = mealRepository;
            _timeprovider = timeprovider;
        }

        public async Task<CommonMealChef> UpdateChef(CommonMealChef chef)
        {
            var status = await _mealRepository.GetStatus(chef.CommonMealId);

            if (status != "OPEN")
                throw new Exception($"The meal with id '{chef.CommonMealId}' is not open for chef changes");

            chef.Timestamp = _timeprovider.Now();
            return await _chefRepository.Update(chef);
        }
    }
}