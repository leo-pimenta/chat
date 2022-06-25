using System.Diagnostics.CodeAnalysis;

namespace Utils.ValidationExtensions
{
    public static class Validations
    {
        public static void ThrowIfNullOrWhitespace([NotNull]this string? s, string message)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException(message);
            }
        }

        public static void ThrowIfNull([NotNull]this object? obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}