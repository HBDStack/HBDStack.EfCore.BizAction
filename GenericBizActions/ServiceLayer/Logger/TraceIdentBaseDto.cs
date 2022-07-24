// =====================================================
// EfCoreExample - Example code to go with book
// Filename: TraceIdentBaseDto.cs
// Date Created: 2016/09/13
// 
// Under the MIT License (MIT)
// 
// Written by Jon P Smith : GitHub JonPSmith, www.thereformedprogrammer.net
// =====================================================

namespace ServiceLayer.Logger;

public class TraceIdentBaseDto
{
    public TraceIdentBaseDto(string traceIdentifier)
    {
        TraceIdentifier = traceIdentifier;
        NumLogs = HttpRequestLog.GetHttpRequestLog(traceIdentifier).RequestLogs.Count;
    }

    public string TraceIdentifier { get; }

    public int NumLogs { get; }
}