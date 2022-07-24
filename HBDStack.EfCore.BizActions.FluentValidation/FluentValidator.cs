using FluentValidation;
using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.BizActions.FluentValidation;

public class FluentValidator : IModelValidator
{
    private readonly IValidatorFactory _validatorFactory;
    public FluentValidator(IValidatorFactory validatorFactory) => _validatorFactory = validatorFactory;


    // public IStatusGenericHandler Validate(object inputData)
    // {
    //     if (inputData is null)
    //         throw new ArgumentNullException(nameof(inputData));
    //
    //     var state = new StatusGenericHandler();
    //
    //     var validator = _validatorFactory.GetValidator(inputData.GetType());
    //     if (validator == null)
    //         return state;
    //     var context = new ValidationContext<object>(inputData);
    //     var result = validator.Validate(context);
    //
    //     if (result is { IsValid: false })
    //         state.AddValidationResult(result);
    //
    //     return state;
    // }

    public async Task<IStatusGenericHandler> ValidateAsync(object inputData,
        CancellationToken cancellation = default)
    {
        if (inputData is null)
            throw new ArgumentNullException(nameof(inputData));

        var state = new StatusGenericHandler();

        var validator = _validatorFactory.GetValidator(inputData.GetType());
        if (validator == null)
            return state;

        var result = await validator.ValidateAsync(new ValidationContext<object>(inputData), cancellation)
            .ConfigureAwait(false);

        if (result is { IsValid: false })
            state.AddValidationResult(result);

        return state;
    }
}