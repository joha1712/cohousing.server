using Cohousing.Server.Api.Mappers;
using Cohousing.Server.Api.Mappers.Common;
using Cohousing.Server.Api.ViewModels;

namespace Cohousing.Server.Model.Models
{
    public class PersonGroupMapper : MapperBase<PersonGroup, PersonGroupViewModel>, IPersonGroupMapper
    {
        public override PersonGroup Map(PersonGroupViewModel item)
        {
            return new PersonGroup { 
                Conventional = item.Conventional ?? 0, 
                Vegetarians = item.Vegetarians ?? 0
            };
        }

        public override PersonGroupViewModel Map(PersonGroup item)
        {
            return new PersonGroupViewModel { 
                Conventional = item.Conventional, 
                Vegetarians = item.Vegetarians
            };
        }
    }
}