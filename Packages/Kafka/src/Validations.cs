using System;
using System.Diagnostics.CodeAnalysis;

namespace Utils.ValidationExtensions
{
    internal static class Validations
    {
        internal static void ThrowIfNullOrWhitespace([NotNull]this string? s, string message)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException(message);
            }
        }

        internal static void ThrowIfNull([NotNull]this object? obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}