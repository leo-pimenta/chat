using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Extensions.Configuration
{
    internal class KeyValueConfigurationProvider : ConfigurationProvider
    {
        private readonly string Path;
        private readonly bool Optional;

        public KeyValueConfigurationProvider(string path, bool optional)
        {
            this.Path = System.IO.Path.GetFullPath(path);
            this.Optional = optional;
        }

        public override void Load()
        {
            foreach (var config in ReadConfigurationsFromFile())
            {
                Data.Add(config);
            }
        }

        private IEnumerable<KeyValuePair<string, string>> ReadConfigurationsFromFile()
        {
            if (Optional && !File.Exists(Path))
            {
                return new List<KeyValuePair<string, string>>();
            }

            return File.ReadAllLines(Path).Where(IsNotEmpty).Select(SplitIntoKeyValuePairObject);
        }

        private bool IsNotEmpty(string s) => !string.IsNullOrWhiteSpace(s);

        private KeyValuePair<string, string> SplitIntoKeyValuePairObject(string keyValueString)
        {
            var arr = keyValueString.Split('=');
            return new KeyValuePair<string, string>(arr[0], arr[1]);
        }
    }
}