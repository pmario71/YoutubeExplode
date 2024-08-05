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

    [Fact]
    public void LoadProfileFromJson()
    {
        // Given
        var sut = ProfileHelper.ResolveProfilePath("Trainings");
    
        // When
    
        // Then
        Assert.NotNull(sut);
    }

    [Fact]
    public void LoadProfileFromJson_profile_not_found()
    {
        // Given
        var sut = ProfileHelper.ResolveProfilePath("unknown");
    
        // When
    
        // Then
        Assert.Null(sut);
    }
}