using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Cohousing.Server.Util
{
    public static class StringExtensions
    {
        public static string ToUpperFirstLetter(this string source)
        {
            if (string.IsNullOrEmpty(source))
                return source;

            if (source.Length == 1)
                return source.ToUpperInvariant();

            return source[0].ToString().ToUpperInvariant() + source.Substring(1);
        }

        public static IImmutableList<KeyValuePair<string, string>> AsKeyValuePairs(this string source, string splitOn = ";|")
        {
            KeyValuePair<string, string>? AsKeyValuePair(string s)
            {
                var idx = s?.IndexOf("=", StringComparison.Ordinal);

                if (idx == null || idx.Value <= 0)
                    return null;

                var key = s.Substring(0, idx.Value).Trim();
                var value = s.Substring(Math.Min(s.Length, idx.Value + 1)).Trim();

                return new KeyValuePair<string, string>(key, value);
            }

            if (source == null)
                return new ImmutableArray<KeyValuePair<string, string>>();

            return source
                .Split(splitOn.ToCharArray(), StringSplitOptions.RemoveEmptyEntries)
                .Select(AsKeyValuePair)
                .Where(x => x != null)
                .Select(x => x.Value)
                .ToImmutableList();
        }
        
        public static bool ContainsOrdinalIgnoreCase(this string source, string toCheck, StringComparison comp = StringComparison.OrdinalIgnoreCase)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
        
    }
}
