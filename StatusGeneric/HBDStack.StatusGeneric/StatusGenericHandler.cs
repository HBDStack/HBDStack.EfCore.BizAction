// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

// ReSharper disable MemberCanBeProtected.Global

namespace HBDStack.StatusGeneric;

/// <inheritdoc />
public class StatusGenericHandler : IStatusGenericHandler
{
    /// <summary>
    /// This is the default success message.
    /// </summary>
    public const string DefaultSuccessMessage = "Success";

    private string _successMessage = DefaultSuccessMessage;

    /// <summary>
    /// This creates a StatusGenericHandler, with optional header (see Header property, and CombineStatuses)
    /// </summary>
    /// <param name="header"></param>
    public StatusGenericHandler(string header = "") => Header = header;

    protected List<ErrorGeneric> InternalErrors { get; } = new List<ErrorGeneric>();

    /// <summary>
    /// The header provides a prefix to any errors you add. Useful if you want to have a general prefix to all your errors
    /// e.g. a header if "MyClass" would produce error messages such as "MyClass: This is my error message."
    /// </summary>
    public string Header { get; set; }

    /// <inheritdoc />
    public IReadOnlyList<ErrorGeneric> Errors => InternalErrors.AsReadOnly();

    /// <summary>
    /// This is true if there are no errors 
    /// </summary>
    public bool IsValid => !InternalErrors.Any();

    /// <summary>
    /// This is true if any errors have been added 
    /// </summary>
    public bool HasErrors => InternalErrors.Any();

    /// <inheritdoc />
    public string Message
    {
        get => IsValid
            ? _successMessage
            : $"Failed with {InternalErrors.Count} error" + (InternalErrors.Count == 1 ? "" : "s");
        set => _successMessage = value;
    }

    /// <inheritdoc />
    public IStatusGeneric CombineStatuses(IStatusGeneric status)
    {
        if (!status.IsValid)
        {
            InternalErrors.AddRange(string.IsNullOrEmpty(Header)
                ? status.Errors
                : status.Errors.Select(x => new ErrorGeneric(Header, x)));
        }

        if (IsValid && status.Message != DefaultSuccessMessage)
            Message = status.Message;

        return this;
    }

    /// <inheritdoc />
    public string GetAllErrors(string separator = null)
    {
        separator ??= Environment.NewLine;
        return InternalErrors.Any()
            ? string.Join(separator, Errors)
            : null;
    }

    /// <inheritdoc />
    public virtual IStatusGeneric AddError(string errorMessage, string errorCode,
        Dictionary<string, object> references)
    {
        if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
        InternalErrors.Add(new ErrorGeneric(Header,
            new GenericValidationResult(errorMessage, new[] { errorCode })
            {
                References = references
            }));
        return this;
    }

    public IStatusGeneric AddError(string errorMessage, string errorCode, string[] propertyNames)
    {
        if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
        InternalErrors.Add(new ErrorGeneric(Header,
            new GenericValidationResult(errorCode, errorMessage, propertyNames)));
        return this;
    }

    /// <inheritdoc />
    public virtual IStatusGeneric AddError(string errorMessage, params string[] propertyNames)
    {
        if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
        InternalErrors.Add(new ErrorGeneric(Header, new GenericValidationResult(errorMessage, propertyNames)));
        return this;
    }

    /// <inheritdoc />
    public IStatusGeneric AddError(Exception ex, string errorMessage, params string[] propertyNames)
    {
        if (errorMessage == null) throw new ArgumentNullException(nameof(errorMessage));
        var errorGeneric = new ErrorGeneric(Header, new GenericValidationResult(errorMessage, propertyNames));

        errorGeneric.CopyExceptionToDebugData(ex);
        InternalErrors.Add(errorGeneric);
        return this;
    }

    /// <inheritdoc />
    public void AddValidationResult(ValidationResult validationResult) =>
        AddValidationResult(validationResult.ToGeneric());

    /// <inheritdoc />
    public void AddValidationResults(IEnumerable<ValidationResult> validationResults)
        => AddValidationResults(validationResults.Select(v => v.ToGeneric()));

    /// <inheritdoc />
    public void AddValidationResult(GenericValidationResult validationResult)
        => InternalErrors.Add(new ErrorGeneric(Header, validationResult));

    /// <inheritdoc />
    public void AddValidationResults(IEnumerable<GenericValidationResult> validationResults)
        => InternalErrors.AddRange(validationResults.Select(x => new ErrorGeneric(Header, x)));
}