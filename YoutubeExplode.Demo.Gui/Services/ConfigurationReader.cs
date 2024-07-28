using System;
using System.Collections.Generic;
using System.IO;
using YoutubeExplode.Common;

namespace YoutubeExplode.Demo.Gui;

public class ConfigurationReader
{
    private readonly StreamReader _config;
    private readonly ConfigReaderSettings _settings;

    public ConfigurationReader(ConfigReaderSettings? settings = null)
        : this(File.OpenRead("profiles.csv"), settings) { }

    internal ConfigurationReader(Stream config, ConfigReaderSettings? settings = null)
    {
        _config = new StreamReader(config);
        _settings = settings ?? new ConfigReaderSettings();
    }

    public IReadOnlyCollection<ConfigEntry>? Entries { get; private set; }

    public bool Read()
    {
        var items = new List<ConfigEntry>();

        var line = _config.ReadLine();

        if (_settings.IgnoreHeader && line != null)
            line = _config.ReadLine();

        while (line != null)
        {
            if (!TryParse(line, items))
                return false;

            line = _config.ReadLine();
        }
        this.Entries = items;
        return true;
    }

    internal static bool TryParse(string line, ICollection<ConfigEntry> collection)
    {
        var res = new Range[3];
        if (line.AsSpan().Split(res, ';', StringSplitOptions.TrimEntries) == 3)
        {
            var item = new ConfigEntry()
            {
                Name = line[res[0]],
                Resolution = Enum.Parse<Resolution>(line[res[1]], true),
                Path = line[res[2]],
            };
            collection.Add(item);
            return true;
        }
        return false;
    }
}

public class ConfigReaderSettings
{
    public bool IgnoreHeader { get; set; }
}

public record ConfigEntry
{
    public required string Name { get; set; }

    public Resolution Resolution { get; set; }

    public required string Path { get; set; }
}

public enum Resolution
{
    Low,
    Medium,
    High
}
