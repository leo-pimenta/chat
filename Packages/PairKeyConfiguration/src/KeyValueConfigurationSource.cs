namespace Microsoft.Extensions.Configuration
{
    internal class KeyValueConfigurationSource : IConfigurationSource
    {
        private readonly string Path;
        private readonly bool Optional;

        public KeyValueConfigurationSource(string path, bool optional)
        {
            this.Path = path;
            this.Optional = optional;
        }

        public IConfigurationProvider Build(IConfigurationBuilder builder) => 
            new KeyValueConfigurationProvider(Path, Optional);
    }
}