using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;

// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HBDStack.StatusGeneric;

public class GenericValidationResult
{
    public GenericValidationResult(ValidationResult validationResult) : this(validationResult.ErrorMessage, validationResult.MemberNames.ToArray())
    {
    }

    public GenericValidationResult(string errorCode, string errorMessage) : this(errorCode, errorMessage, null)
    {
    }

    public GenericValidationResult(string errorMessage, string[] memberNames) : this(null, errorMessage, memberNames)
    {
    }

    public GenericValidationResult(string errorCode, string errorMessage, [AllowNull] string[] memberNames)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        MemberNames = memberNames?.Any() == true ? memberNames : null;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ErrorCode { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IDictionary<string, object> References { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IEnumerable<string> MemberNames { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string ErrorMessage { get; set; }

    public override string ToString() => ErrorMessage ?? base.ToString()!;
}