using System.Linq;
using YoutubeExplode.Demo.Cli.Utils;


namespace YoutubeExplode.Demo.Cli.Tests;

public class UrlPatcherTests
{
    [Theory]
    [InlineData("https://www.youtube.com/watch?v=s3b96zhjTCI&list=PLgseqxAjbWl6_lxHAvZpIB0wfwqZx_XOF&index=13", "https://www.youtube.com/watch?v=s3b96zhjTCI")]
    [InlineData("https://www.youtube.com/watch?list=PLgseqxAjbWl6_lxHAvZpIB0wfwqZx_XOF&index=13&v=s3b96zhjTCI", "https://www.youtube.com/watch?v=s3b96zhjTCI")]
    [InlineData("https://www.youtube.com/watch?xx=rrr&index=13&v=s3b96zhjTCI&list=PLgseqxAjbWl6_lxHAvZpIB0wfwqZx_XOF", "https://www.youtube.com/watch?v=s3b96zhjTCI")]
    [InlineData("https://www.youtube.com/watch", "https://www.youtube.com/watch")]
    public void StripUrlParameters(string url, string expected)
    {
        var patchedUrl = UrlPatcher.StripUrlParameters(url);

        Assert.True(expected.AsSpan().SequenceEqual(patchedUrl));
    }

    [Theory]
    [InlineData("01234567;012345", 8)]
    [InlineData("01234567", 8)]
    public void SkipUntil(string url, int expected)
    {
        var patchedUrl = UrlPatcher.FindFirstOccurrance(url, 0, ';');

        Assert.Equal(expected, patchedUrl);
    }
}
