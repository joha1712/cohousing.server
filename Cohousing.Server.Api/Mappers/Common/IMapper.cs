﻿using System.Collections.Generic;
using System.Collections.Immutable;

namespace Cohousing.Server.Api.Mappers.Common
{
    public interface IMapper<T1, T2>
    {
        T1 Map(T2 item);
        T2 Map(T1 item);

        IImmutableList<T1> MapMany(IEnumerable<T2> items);
        IImmutableList<T2> MapMany(IEnumerable<T1> items);
    }
}