using System.Collections.Immutable;

namespace Cohousing.Server.Util
{
    public static class LinqExtensions
    {
        public static IImmutableList<T> Append<T>(this IImmutableList<T> a, IImmutableList<T> b)
        {
            if (a == null)
                return b;

            if (b == null)
                return a;

            return a.AddRange(b);
        }
    }
}