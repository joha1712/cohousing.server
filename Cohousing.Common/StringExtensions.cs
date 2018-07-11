namespace Cohousing.Common
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
    }
}
