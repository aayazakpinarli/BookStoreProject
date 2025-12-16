namespace CORE.APP.Extensions
{
    // Provides extension methods for string validation and default value handling
    public static class StringExtensions
    {
        public static string HasNotAny(this string value, string defaultValue)
        {
            return HasNotAny(value) ? defaultValue : value;
        }

        public static bool HasNotAny(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        public static bool HasAny(this string value)
        {
            return !HasNotAny(value);
        }
    }
}