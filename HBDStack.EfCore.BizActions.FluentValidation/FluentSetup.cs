using System.Reflection;
using FluentValidation;
using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.EfCore.BizActions.FluentValidation;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class FluentSetup
{
    public static IServiceCollection AddFluentValidator(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        if (assemblies is not { Length: > 0 })
            assemblies = new[] { Assembly.GetCallingAssembly() };

        return services.AddScoped<IModelValidator, FluentValidator>()
            .AddScoped<IValidatorFactory, ServiceProviderValidatorFactory>()
            .AddValidatorsFromAssemblies(assemblies);
    }

    /// <summary>
    /// Scan and Add all the <see cref="IValidator"/> in the assemblies for FluentValidator
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies"></param>
    /// <returns></returns>
    private static IServiceCollection AddValidatorsFromAssemblies(this IServiceCollection services,
        params Assembly[] assemblies)
    {
        var types = assemblies.Distinct().SelectMany(a => a.GetTypes()).Distinct()
            .Where(t => t.IsClass && !t.IsGenericType && typeof(IValidator).IsAssignableFrom(t));

        foreach (var t in types)
        {
            var i = t.GetInterfaces().FirstOrDefault();
            if (i == null) continue;

            services.AddScoped(i, t);
        }

        return services;
    }
}