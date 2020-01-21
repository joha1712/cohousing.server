using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Cohousing.Server.Model.Common;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealChefFactory
    {
        private readonly ITimeProvider _timeProvider;

        public CommonMealChefFactory(ITimeProvider timeProvider) {
            _timeProvider = timeProvider;
        }

        public IImmutableList<CommonMealChef> CreateMany(int numChefs)
        {
            var result = new List<CommonMealChef>();

            for (int i = 0; i < numChefs; i++)
            {
                result.Add(Create());
            }

            return result.ToImmutableList();
        }

        public CommonMealChef Create()
        {
            return new CommonMealChef
            {
                Timestamp = _timeProvider.Now(),
                PersonId = null
            };
        }
    }
}