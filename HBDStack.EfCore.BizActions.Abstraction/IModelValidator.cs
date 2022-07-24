using HBDStack.StatusGeneric;

namespace HBDStack.EfCore.BizActions.Abstraction;

public interface IModelValidator
{
    //Remove Async
    //IStatusGenericHandler Validate(object inputData);

    Task<IStatusGenericHandler> ValidateAsync(object inputData, CancellationToken cancellation = default);
}