// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Text;
using System.Text.Json.Serialization;
// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.StatusGeneric;

/// <summary>
/// This holds an error registered in the <see cref="IStatusGeneric"/> Errors collection
/// </summary>
public struct ErrorGeneric
{
    /// <summary>
    /// If there are multiple headers this separator is placed between them, e.g. Update>Author
    /// </summary>
    public const string HeaderSeparator = ">";

    /// <summary>
    /// This ctor will create an ErrorGeneric
    /// </summary>
    /// <param name="header"></param>
    /// <param name="error"></param>
    public ErrorGeneric(string header, GenericValidationResult error) : this()
    {
        Header = header ?? throw new ArgumentNullException(nameof(header));
        ErrorResult = error ?? throw new ArgumentNullException(nameof(error));
    }

    internal ErrorGeneric(string prefix, ErrorGeneric existingError)
    {          
        Header = string.IsNullOrEmpty(prefix)
            ? existingError.Header
            : string.IsNullOrEmpty(existingError.Header) 
                ? prefix
                : prefix + HeaderSeparator + existingError.Header;
            
        ErrorResult = existingError.ErrorResult;
        DebugData = existingError.DebugData;
    }

    /// <summary>
    /// A Header. Can be null
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Header { get;  }

    /// <summary>
    /// This is the error provided
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public GenericValidationResult ErrorResult { get;  }

    /// <summary>
    /// This can be used to contain extra data to help the developer debug the error
    /// For instance, the content of an exception.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public string DebugData { get; private set; }

    /// <summary>
    /// This copies the exception Message, StackTrace and any entries in the Data dictionary into the DebugData string
    /// </summary>
    /// <param name="ex"></param>
    internal void CopyExceptionToDebugData(Exception ex)
    {
        var sb = new StringBuilder();
        sb.AppendLine(ex.Message);
        sb.Append("StackTrace:");
        sb.AppendLine(ex.StackTrace);
            
        foreach (DictionaryEntry entry in ex.Data)
            sb.AppendLine($"Data: {entry.Key}\t{entry.Value}");

        DebugData = sb.ToString();
    }

    /// <summary>
    /// A human-readable error display
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        var start = string.IsNullOrEmpty(Header) ? "" : Header + ": ";
        return start + ErrorResult;
    }

}