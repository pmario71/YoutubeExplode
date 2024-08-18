using System.Net.Http.Headers;
using System.Web;

namespace YoutubeExplode.Demo.Cli.Utils;

public class UrlPatcher
{
    /// <summary>
    /// Remove additional url parameters that do not help
    /// </summary>
    /// <remarks>Might not be necessary, if url is properly put in high commas</remarks>
    /// <param name="orgUrl"></param>
    /// <returns></returns>
    public static ReadOnlySpan<char> StripUrlParameters(ReadOnlySpan<char> orgUrl)
    {
        int endBase = FindFirstOccurrance(orgUrl, 0, '?');
        int startQuery = endBase + 1;

        if (startQuery < orgUrl.Length)
        {
            while (orgUrl[startQuery] != 'v')
            {
                startQuery = FindFirstOccurrance(orgUrl, startQuery, '&');
                startQuery++;
            }
        }
        else
        {
            return orgUrl.Slice(0, startQuery - 1);
        }

        int endQuery = FindFirstOccurrance(orgUrl, startQuery, '&');

        if (endBase != startQuery - 1)
        {
            return $"{orgUrl[0..(endBase + 1)]}{orgUrl[startQuery..endQuery]}";
        }

        return orgUrl.Slice(0, endQuery);
    }

    /// <summary>
    /// Find first occurrance of <see cref="character"/> and returns its position or length of text.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="start"></param>
    /// <param name="character"></param>
    /// <returns></returns>
    public static int FindFirstOccurrance(ReadOnlySpan<char> text, int start, char character)
    {
        for (int i = start; i < text.Length; i++)
        {
            if (text[i] == character)
                return i;
        }
        return text.Length;
    }
}
