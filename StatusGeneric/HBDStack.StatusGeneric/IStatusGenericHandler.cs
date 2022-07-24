// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License file in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HBDStack.StatusGeneric;

/// <summary>
/// This is the interface for the full StatusGenericHandler
/// </summary>
public interface IStatusGenericHandler : IStatusGeneric
{
    /// <summary>
    /// This adds one error to the Errors collection
    /// NOTE: This is virtual so that the StatusGenericHandler.Generic can override it. That allows both to return a IStatusGeneric result
    /// </summary>
    /// <param name="errorMessage">The text of the error message</param>
    /// <param name="errorCode">Error code</param>
    /// <param name="references">Optional. Reference data of the error</param>
    IStatusGeneric AddError(string errorMessage, string errorCode, Dictionary<string, object> references);

    /// <summary>
    /// This adds one error to the Errors collection
    /// NOTE: This is virtual so that the StatusGenericHandler.Generic can override it. That allows both to return a IStatusGeneric result
    /// </summary>
    /// <param name="errorMessage">The text of the error message</param>
    /// <param name="errorCode">Error code</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    IStatusGeneric AddError(string errorMessage, string errorCode, string[] propertyNames);

    /// <summary>
    /// This adds one error to the Errors collection
    /// NOTE: This is virtual so that the StatusGenericHandler.Generic can override it. That allows both to return a IStatusGeneric result
    /// </summary>
    /// <param name="errorMessage">The text of the error message</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    IStatusGeneric AddError(string errorMessage, params string[] propertyNames);

    /// <summary>
    /// This adds one error to the Errors collection and saves the exception's data to the DebugData property
    /// </summary>
    /// <param name="ex">The exception that you want to turn into a IStatusGeneric error.</param>
    /// <param name="errorMessage">The user-friendly text for the error message</param>
    /// <param name="propertyNames">optional. A list of property names that this error applies to</param>
    IStatusGeneric AddError(Exception ex, string errorMessage, params string[] propertyNames);

    /// <summary>
    /// This adds one ValidationResult to the Errors collection
    /// </summary>
    /// <param name="validationResult"></param>
    void AddValidationResult(ValidationResult validationResult);

    /// <summary>
    /// This appends a collection of ValidationResults to the Errors collection
    /// </summary>
    /// <param name="validationResults"></param>
    void AddValidationResults(IEnumerable<ValidationResult> validationResults);

    /// <summary>
    /// This adds one GenericValidationResult to the Errors collection
    /// </summary>
    /// <param name="validationResult"></param>
    void AddValidationResult(GenericValidationResult validationResult);

    /// <summary>
    /// This appends a collection of GenericValidationResult to the Errors collection
    /// </summary>
    /// <param name="validationResults"></param>
    void AddValidationResults(IEnumerable<GenericValidationResult> validationResults);
}