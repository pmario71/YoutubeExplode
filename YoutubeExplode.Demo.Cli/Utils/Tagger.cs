using System;
using AngleSharp.Dom;

namespace YoutubeExplode.Demo.Cli.Utils;

public static class Tagger
{
    /// <summary>
    /// Write source url into comment tag of meta data.
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="url"></param>
    public static void SetUrlToCommentTag(string fileName, ReadOnlySpan<char> url)
    {
        using TagLib.File videoFile = TagLib.File.Create(fileName);

        var tag = videoFile.GetTag(TagLib.TagTypes.Apple, true);
        tag.Comment = $"source: {url}";

        videoFile.Save();
    }

    /// <summary>
    /// Read comment tag from meta data
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string ReadCommentTag(string fileName)
    {
        TagLib.File videoFile = TagLib.File.Create(fileName);

        var tag = (TagLib.Mpeg4.AppleTag)videoFile.GetTag(TagLib.TagTypes.Apple, false);
        return tag.Comment;
    }
}
