using FluentValidation;

namespace HBDStack.EfCore.BizActions.FluentValidation;

internal sealed class ServiceProviderValidatorFactory : ValidatorFactoryBase
{
    private readonly IServiceProvider _serviceProvider;
    public ServiceProviderValidatorFactory(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;
    public override IValidator CreateInstance(Type validatorType) =>
        (IValidator)_serviceProvider.GetService(validatorType)!;
}