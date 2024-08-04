using Microsoft.Extensions.Configuration;
using YoutubeExplode.Demo.Cli.Utils;

namespace YoutubeExplode.Demo.Cli.Tests;

public class ProfileHelperTests
{
    [Theory]
    [InlineData("fdas", false, null)]
    [InlineData("{fdas", false, null)]
    [InlineData("fdas}", false, null)]
    [InlineData("{fdas}", true, "fdas")]
    [InlineData(" {fdas}", true, "fdas")]
    [InlineData(" {fdas} ", true, "fdas")]
    public void TryGetProfile(string input, bool isProfile, string expected)
    {
        var profile = ProfileHelper.TryGetProfile(input, out var profileName);
        
        Assert.Equal(isProfile, profile);
        Assert.Equal(expected, profileName);
    }

    [Theory]
    [InlineData("Profile1", "value1")]
    [InlineData("fdas", null)]
    public void ResolveProfilePath(string input, string expectedProfilePath)
    {
        var inmem = new Dictionary<string, string?>
        {
            {"Key1", "Value1"},
            {"Profiles:Profile1", "value1"},
            {"Profiles:Profile2", "value2"}
        };

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inmem)
            .Build();

        var profilePath = ProfileHelper.ResolveProfilePath(configuration, input);
        
        Assert.Equal(expectedProfilePath, profilePath);
    }
}