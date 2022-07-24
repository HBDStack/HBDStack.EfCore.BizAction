// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using TestBizLayer.DbForTransactions;
using Tests.Biz;
using TestSupport.EfHelpers;
using Xunit;

namespace Tests.UnitTests.TestValidators;

public class TestModelValidators
{
    #region Methods

    [Fact]
    public async Task TestModelValidator()
    {
        var moq = new Mock<HBDStack.EfCore.BizActions.Abstraction.IModelValidator>();
        moq.Setup(s => s.ValidateAsync(It.IsAny<object>(),It.IsAny<CancellationToken>())).Verifiable();

        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddSingleton(moq.Object)
            .AddScoped<ITestBizAsync, TestBizAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizAsync>>();

        var rs = await sv.RunBizActionAsync<TestModel>(new TestModel()).ConfigureAwait(false);

        moq.Verify(s => s.ValidateAsync(It.IsAny<object>(),It.IsAny<CancellationToken>()));
    }

    [Fact]
    public void TestModelValidatorAsync()
    {
        var moq = new Mock<HBDStack.EfCore.BizActions.Abstraction.IModelValidator>();
        moq.Setup(s => s.ValidateAsync(It.IsAny<object>(), default)).Verifiable();

        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddSingleton(moq.Object).AddScoped<ITestBizAsync, TestBizAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizAsync>>();

        var rs = sv.RunBizActionAsync<TestModel>(new TestModel());

        moq.Verify(s => s.ValidateAsync(It.IsAny<object>(), default));
    }

    [Fact]
    public async Task TestModelValidatorInOnly()
    {
        var moq = new Mock<HBDStack.EfCore.BizActions.Abstraction.IModelValidator>();
        moq.Setup(s => s.ValidateAsync(It.IsAny<object>(),It.IsAny<CancellationToken>())).Verifiable();

        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddSingleton(moq.Object).AddScoped<ITestBizInOnlyAsync, TestBizInOnlyAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizInOnlyAsync>>();

        await sv.RunBizActionAsync(new TestModel()).ConfigureAwait(false);

        moq.Verify(s => s.ValidateAsync(It.IsAny<object>(), It.IsAny<CancellationToken>()));
    }

    [Fact]
    public async Task TestModelValidatorInOnlyAsync()
    {
        var moq = new Mock<HBDStack.EfCore.BizActions.Abstraction.IModelValidator>();
        moq.Setup(s => s.ValidateAsync(It.IsAny<object>(), default)).Verifiable();

        //SETUP
        var service = new ServiceCollection();

        //ATTEMPT
        service
            .AddAutoMapper(typeof(TestDbContext).Assembly)
            .AddSingleton(new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>()))
            .RegisterBizRunner<TestDbContext>();

        service.AddSingleton(moq.Object).AddScoped<ITestBizInOnlyAsync, TestBizInOnlyAsync>();

        var provider = service.BuildServiceProvider();
        var sv = provider.GetRequiredService<IActionServiceAsync<ITestBizInOnlyAsync>>();

        await sv.RunBizActionAsync(new TestModel()).ConfigureAwait(false);

        moq.Verify(s => s.ValidateAsync(It.IsAny<object>(), default));
    }

    #endregion Methods
}