using System.Text;

namespace YoutubeExplode.Demo.Gui.Tests.Services;

public class ConfigurationReaderTests
{
    [Fact]
    public void SingleEntry()
    {
        string input = """
                       Name1;/test/slash/files
                       """;

        var sut = new ConfigurationReader(GetStreamFromString(input));

        Assert.True(sut.Read());

        Assert.NotNull(sut.Entries);
        Assert.Single(sut.Entries!);

        var entry = sut.Entries.First();
        Assert.Equal("Name1", entry.Name);
        Assert.Equal("/test/slash/files", entry.Value);
    }

    [Fact]
    public void Entries_are_trimmed()
    {
        string input = """
                        Name1 ; /test/slash/files 
                       """;

        var sut = new ConfigurationReader(GetStreamFromString(input));

        Assert.True(sut.Read());

        Assert.NotNull(sut.Entries);
        Assert.Single(sut.Entries!);

        var entry = sut.Entries.First();
        Assert.DoesNotContain(entry.Name, c => char.IsWhiteSpace(c));
        Assert.DoesNotContain(entry.Value, c => char.IsWhiteSpace(c));
    }

    [Fact]
    public void MultiLineSupport()
    {
        // Given
        string input = """
                       Name1;/test/slash/files1
                       Name2;/test/slash/files2
                       Name3;/test/slash/files3
                       """;

        var sut = new ConfigurationReader(GetStreamFromString(input));

        Assert.True(sut.Read());

        // When

        // Then
        Assert.NotNull(sut.Entries);
        Assert.Equal(3, sut.Entries.Count);

        var entry = sut.Entries.ElementAt(2);
        Assert.Equal("Name3", entry.Name);
        Assert.Equal("/test/slash/files3", entry.Value);
    }

    [Fact]
    public void Support_empty_file()
    {
        string input = string.Empty;

        var sut = new ConfigurationReader(GetStreamFromString(input));

        Assert.True(sut.Read());

        Assert.NotNull(sut.Entries);
        Assert.Empty(sut.Entries!);
    }

    [Fact]
    public void Support_skipping_header_row()
    {
        string input = """
                       Name1;/test/slash/files
                       """;

        var sut = new ConfigurationReader(GetStreamFromString(input), new ConfigReaderSettings { IgnoreHeader = true });

        Assert.True(sut.Read());

        Assert.NotNull(sut.Entries);
        Assert.Empty(sut.Entries!);
    }


    public static Stream GetStreamFromString(string sampleString, Encoding? encoding = null)
    {
        encoding ??= Encoding.UTF8;
        var stream = new MemoryStream(encoding.GetByteCount(sampleString));
        using var writer = new StreamWriter(stream, encoding, -1, true);
        writer.Write(sampleString);
        writer.Flush();
        stream.Position = 0;
        return stream;
    }
}