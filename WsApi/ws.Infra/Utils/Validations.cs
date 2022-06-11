namespace Utils.ValidationExtensions
{
    public static class Validations
    {
        public static void ThrowIfNullOrWhitespace(this string s, string message)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                throw new ArgumentException(message);
            }
        }

        public static void ThrowIfNull(this object obj, string paramName)
        {
            if (obj == null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}