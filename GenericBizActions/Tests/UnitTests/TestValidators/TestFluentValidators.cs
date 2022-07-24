// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using DataLayer.EfCode;
using HBD.EfCore.BizAction;
using Microsoft.Extensions.DependencyInjection;
using TestBizLayer.DbForTransactions;
using Tests.Biz;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.TestValidators;

public class TestFluentValidators
{
    #region Methods

    [Fact]
    public async Task TestFluentValidator()
    {
        //SETUP
        var service = new ServiceCollection();
        using var option = SqliteInMemory.CreateOptions<TestDbContext>();
            
        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(option))
            .RegisterBizRunner<TestDbContext>();

        service.AddFluentValidator().AddScoped<ITestBizAsync, TestBizAsync>();

        var provider = service.BuildServiceProvider();

        using var scope = provider.CreateScope();
        var sv = scope.ServiceProvider.GetRequiredService<IActionServiceAsync<ITestBizAsync>>();

       await sv.RunBizActionAsync<TestModel>(new TestModel()).ConfigureAwait(false);
        sv.Status.HasErrors.ShouldBeTrue();
    }

    [Fact]
    public async Task TestFluentValidatorNoValidator()
    {
        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddFluentValidator(typeof(EfCoreContext).Assembly).AddScoped<ITestBizAsync, TestBizAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizAsync>>();

       await sv.RunBizActionAsync<TestModel>(new TestModel()).ConfigureAwait(false);
    }

    #endregion Methods
}