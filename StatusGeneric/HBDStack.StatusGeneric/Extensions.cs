using System.ComponentModel.DataAnnotations;

namespace HBDStack.StatusGeneric;

public static class Extensions
{
    public static GenericValidationResult ToGeneric(this ValidationResult validationResult) => new GenericValidationResult(validationResult);
}