// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using HBD.EfCore.BizAction;
using HBD.EfCore.BizAction.Configuration;
using HBD.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

// ReSharper disable CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;
    
/// <summary>
///     This contains the code to register the GenericBizRunner library and also find and GenericBizRunner DTO and
/// </summary>
public static partial class NetCoreBizRunnerDi
{
    #region Classes

    private class CheckDescriptor : IEqualityComparer<ServiceDescriptor>
    {
        #region Methods

        public bool Equals([NotNull] ServiceDescriptor x, [NotNull] ServiceDescriptor y)
        {
            return x.ServiceType == y.ServiceType
                   && x.ImplementationType == y.ImplementationType
                   && x.Lifetime == y.Lifetime;
        }

        public int GetHashCode(ServiceDescriptor obj) => throw new NotImplementedException();

        #endregion Methods
    }

    #endregion Classes

    #region Methods

    /// <summary>
    ///     This is used to register GenericBizRunner and any GenericBizRunner DTOs with .NET Core DI provider to work with
    ///     multiple DbContexts
    /// </summary>
    /// <param name="services"></param>
    public static void RegisterBizRunnerMultiDbContext(this IServiceCollection services)
        => services.RegisterBizRunnerMultiDbContext(new GenericBizRunnerConfig());

    /// <summary>
    ///     This is used to register GenericBizRunner and any GenericBizRunner DTOs with .NET Core DI provider to work with
    ///     multiple DbContexts
    /// </summary>
    /// <param name="services"></param>
    /// <param name="config"></param>
    public static void RegisterBizRunnerMultiDbContext(this IServiceCollection services, IGenericBizRunnerConfig config)
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        //Remove Sync
        //services.AddTransient(typeof(IActionService<,>), typeof(ActionService<,>));
        services.AddTransient(typeof(IActionServiceAsync<,>), typeof(ActionServiceAsync<,>));

        services.BuildRegisterWrappedConfig(config);
    }

    /// <summary>
    ///     This is the method for registering GenericBizRunner and any GenericBizRunner DTOs with .NET Core DI provider
    /// </summary>
    /// <typeparam name="TDefaultDbContext"></typeparam>
    /// <param name="services"></param>
    public static void RegisterBizRunner<TDefaultDbContext>(this IServiceCollection services)
        where TDefaultDbContext : DbContext =>
        services.RegisterBizRunner<TDefaultDbContext>(new GenericBizRunnerConfig());

    /// <summary>
    ///     This is the method for registering GenericBizRunner and any GenericBizRunner DTOs with .NET Core DI provider with
    ///     config
    /// </summary>
    /// <typeparam name="TDefaultDbContext"></typeparam>
    /// <param name="services"></param>
    /// <param name="config"></param>
    public static void RegisterBizRunner<TDefaultDbContext>(this IServiceCollection services,
        IGenericBizRunnerConfig config)
        where TDefaultDbContext : DbContext
    {
        if (config == null) throw new ArgumentNullException(nameof(config));

        if (services.All(s => s.ServiceType != typeof(DbContext)))
        {
            //TODO: This will cause too many IServiceProvider issue.
            services.AddScoped<DbContext>(sp => sp.GetService<TDefaultDbContext>());
        }

        //Remove Sync
        //services.AddScoped(typeof(IActionService<>), typeof(ActionService<>));
        services.AddScoped(typeof(IActionServiceAsync<>), typeof(ActionServiceAsync<>));

        services.BuildRegisterWrappedConfig(config);
    }

    //---------------------------------------------------------
    //private parts

    private static void BuildRegisterWrappedConfig(this IServiceCollection services, IGenericBizRunnerConfig config)
    {
        //It is possible that the user would use both default DbContext and MultiDbContext, so we only add if not already there
        if (!services.Contains(
                new ServiceDescriptor(typeof(IWrappedBizRunnerConfigAndMappings), config), new CheckDescriptor()))
            //var wrapBuilder = new SetupDtoMappings(config);
            //var wrapperConfig = wrapBuilder.BuildWrappedConfigByScanningForDtos( config);

            //Register the IWrappedBizRunnerConfigAndMappings
            services.AddSingleton<IWrappedBizRunnerConfigAndMappings>(p =>
                new WrappedBizRunnerConfigAndMappings(p.GetRequiredService<IMapper>(), config));
    }

    #endregion Methods
}