// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading.Tasks;
using HBD.EfCore.BizAction;
using Microsoft.Extensions.DependencyInjection;
using TestBizLayer.DbForTransactions;
using Tests.Biz;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.TestValidators;

public class TestDataAnnotationsValidators
{
    #region Methods

    [Fact]
    public async Task TestDataAnnotationsValidator()
    {
        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddAutoMapper(typeof(TestDbContext).Assembly);
        service.AddDefaultModelValidator().AddScoped<ITestBizAsync, TestBizAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizAsync>>();

        await sv.RunBizActionAsync<TestModel>(new TestModel()).ConfigureAwait(false);
        sv.Status.HasErrors.ShouldBeTrue();
    }

    #endregion Methods
}