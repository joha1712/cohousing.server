using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using Cohousing.Server.Model.Models;

namespace Cohousing.Server.Model.Factories
{
    public class CommonMealChefFactory
    {
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
                Timestamp = DateTime.Now,
                PersonId = null
            };
        }
    }
}