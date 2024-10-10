using System;
using YoutubeExplode.Videos.Streams;

namespace YoutubeExplode.Demo.Cli.Tests;

public class StreamInfoExtensionsTests
{
    [Theory]
    [InlineData([new StreamInfoMock(256)])]
    public void TestName(IEnumerable<IStreamInfo> streamInfos, IStreamInfo? result)
    {
        // Given

        // When

        // Then
    }


        
    class StreamInfoMock(Bitrate bitrate) : IStreamInfo
    {
        public string Url => throw new NotImplementedException();

        public Container Container => throw new NotImplementedException();

        public FileSize Size => throw new NotImplementedException();

        public Bitrate Bitrate => bitrate;
    }
}
