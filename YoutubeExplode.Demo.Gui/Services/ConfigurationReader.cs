using System;
using System.Collections.Generic;
using System.IO;

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
        var res = new Range[2];

        var line = _config.ReadLine();

        if (_settings.IgnoreHeader && line != null)
            line = _config.ReadLine();

        while (line != null)
        {
            ReadOnlySpan<char> lineSpan = line.AsSpan();
            if (lineSpan.Split(res, ';', StringSplitOptions.TrimEntries) == 2)
            {
                var item = new ConfigEntry() { Name = line[res[0]], Value = line[res[1]], };
                items.Add(item);
            }
            else
            {
                return false;
            }
            line = _config.ReadLine();
        }
        this.Entries = items;
        return true;
    }
}

public class ConfigReaderSettings
{
    public bool IgnoreHeader { get; set; }
}

public record ConfigEntry
{
    public required string Name { get; set; }
    public required string Value { get; set; }
}
