using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cohousing.Server.Api.Mappers.Common
{
    public abstract class MapperBase<T1, T2> : IMapper<T1, T2>
    {
        public abstract T1 Map(T2 item);
        public abstract T2 Map(T1 item);

        public IImmutableList<T1> Map(IEnumerable<T2> items)
        {
            var result = items?.Select(Map);
            return result?.ToImmutableList();
        }

        public IImmutableList<T2> Map(IEnumerable<T1> items)
        {
            var result = items?.Select(Map);
            return result?.ToImmutableList();
        }
    }
}