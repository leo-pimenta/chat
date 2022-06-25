namespace Test
{
    public class KeyValueConfigurationExtensionTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AddKeyValueFile_Should_AddAllConfigurationsInConfigFile(bool optional)
        {
            var configuration = CreateConfig(".env", optional);
            configuration["var1"].Should().Be("val1");
            configuration["var2"].Should().Be("val2");
            configuration["VAR_3"].Should().Be("VAL-3");
            configuration["5"].Should().Be("9");
        }

        [Fact]
        public void AddKeyValueFile_Should_IgnoreEmptyLines()
        {
            var configuration = CreateConfig("emptylines.txt");
            configuration["test"].Should().Be("lalala");
            configuration["what"].Should().Be("yep");
            configuration["zap"].Should().Be("zip");
        }

        [Fact]
        public void AddKeyValueFile_Should_IgnoreNonExistentFile_When_OptionalIsTrue()
        {
            var configuration = CreateConfig("void", optional: true);
        }

        [Fact]
        public void AddKeyValueFile_Should_ThrowFileNotFoundException_IfNonExistentFile_When_OptionalIsFalse()
        {
            Assert.Throws<FileNotFoundException>(() => CreateConfig("void", optional: false));
        }

        private IConfiguration CreateConfig(string configFileName, bool optional = false) =>
            WebApplication.CreateBuilder()
                .Configuration
                .AddKeyValueFile(Path.Combine("TestFiles", configFileName), optional)
                .Build();
    }
}