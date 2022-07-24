using FluentValidation.Results;
using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.BizActions.FluentValidation;

public static class ValidateExtensions
{
    public static void AddValidationResult(this IStatusGenericHandler status, ValidationResult result)
    {
        foreach (var error in result.Errors)
            status.AddError(error.ErrorMessage, error.ErrorCode, new[] { error.PropertyName });
    }
}