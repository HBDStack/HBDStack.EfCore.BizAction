using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.EfCore.BizActions.DefaultValidation;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class DefaultModelValidatorSetup
{
    public static IServiceCollection AddDefaultModelValidator(this IServiceCollection services)
        => services
            .AddScoped<IModelValidator, DefaultModelValidator>();
}