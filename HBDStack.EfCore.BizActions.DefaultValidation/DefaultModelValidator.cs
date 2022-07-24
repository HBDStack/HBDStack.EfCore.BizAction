using System.ComponentModel.DataAnnotations;
using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.BizActions.DefaultValidation;

public class DefaultModelValidator : IModelValidator
{
    private static IStatusGenericHandler Validate(object inputData)
    {
        if (inputData is null)
            throw new ArgumentNullException(nameof(inputData));

        var state = new StatusGenericHandler();
        var errors = new List<ValidationResult>();
        Validator.TryValidateObject(inputData, new ValidationContext(inputData), errors, true);
        state.AddValidationResults(errors);

        return state;
    }

    public Task<IStatusGenericHandler> ValidateAsync(object inputData, CancellationToken cancellation = default) =>
        Task.FromResult(Validate(inputData));
}