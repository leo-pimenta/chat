namespace Microsoft.Extensions.Configuration
{
    public static class KeyValueConfigurationExtension
    {
        public static IConfigurationBuilder AddKeyValueFile(this IConfigurationBuilder builder, 
            string path, bool optional = false) => builder.Add(new KeyValueConfigurationSource(path, optional));
    }
}