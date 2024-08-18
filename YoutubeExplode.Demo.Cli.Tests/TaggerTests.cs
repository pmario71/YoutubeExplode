using YoutubeExplode.Demo.Cli.Utils;

namespace YoutubeExplode.Demo.Cli.Tests;

public class TaggerTests
{
    [Fact(Skip = "Integration Test only")]
    public void TestName()
    {
        var comment = Tagger.ReadCommentTag("/Users/pmario/Documents/Guitar/Neu.nosync/Delta Blues.mp4");

        Assert.NotEmpty(comment);
    }
}