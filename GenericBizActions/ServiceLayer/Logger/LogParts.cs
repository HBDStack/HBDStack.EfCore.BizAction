// =====================================================
// EfCoreExample - Example code to go with book
// Filename: LogParts.cs
// Date Created: 2016/09/11
// 
// Under the MIT License (MIT)
// 
// Written by Jon P Smith : GitHub JonPSmith, www.thereformedprogrammer.net
// =====================================================

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ServiceLayer.Logger;

public class LogParts
{
    private const string EfCoreEventIdStartWith = "Microsoft.EntityFrameworkCore";

    public LogParts(LogLevel logLevel, EventId eventId, string eventString)
    {
        LogLevel = logLevel;
        EventId = eventId;
        EventString = eventString;
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public LogLevel LogLevel { get; }

    public EventId EventId { get; }

    public string EventString { get; }

    public bool IsDb => EventId.Name?.StartsWith(EfCoreEventIdStartWith) ?? false;

    public override string ToString()
    {
        return $"{LogLevel}: {EventString}";
    }
}