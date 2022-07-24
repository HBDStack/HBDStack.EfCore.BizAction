// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction.Configuration;
using HBDStack.EfCore.BizAction.Internal.Runners;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;
using TestBizLayer.ActionsAsync;
using TestBizLayer.ActionsAsync.Concrete;
using TestBizLayer.BizDTOs;
using TestBizLayer.DbForTransactions;
using Tests.DTOs;
using Tests.Helpers;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Internals;

public class TestRunnersAsync
{
    #region Fields

    private readonly DbContext _dbContext = new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>());
    private readonly IGenericBizRunnerConfig _noCachingConfig = new GenericBizRunnerConfig {TurnOffCaching = true};

    #endregion Fields

    //-------------------------------------------------------
    //Async, no mapping, no database access

    #region Methods

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOnlyMappingDatabaseOk(int num, bool hasErrors)
    {
        //SETUP
        var options = SqliteInMemory.CreateOptions<TestDbContext>();
        await using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
        var utData =
            NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, _noCachingConfig);
        //utData.AddDtoMapping<ServiceLayerBizOutDto>();
        var bizInstance = new BizActionInOnlyWriteDbAsync(context);
        var runner =
            new ActionServiceInOnlyAsync<IBizActionInOnlyWriteDbAsync, BizDataIn>(true, utData.WrappedConfig);
        var inDto = new BizDataIn {Num = num};

        //ATTEMPT
        await runner.RunBizActionDbAndInstanceAsync(context, bizInstance, inDto, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
        if (hasErrors)
            context.LogEntries.Any().ShouldBeFalse();
        else
            context.LogEntries.Single().LogText.ShouldEqual(num.ToString());
    }

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOnlyMappingOk(int num, bool hasErrors)
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, _noCachingConfig);
        //utData.AddDtoMapping<ServiceLayerBizOutDto>();
        var bizInstance = new BizActionInOnlyAsync();
        var runner = new ActionServiceInOnlyAsync<IBizActionInOnlyAsync, BizDataIn>(false, utData.WrappedConfig);
        var inDto = new BizDataIn {Num = num};

        //ATTEMPT
        await runner.RunBizActionDbAndInstanceAsync(_dbContext, bizInstance, inDto, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
    }

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOnlyNoDtoOk(int num, bool hasErrors)
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, _noCachingConfig);
        var bizInstance = new BizActionInOnlyAsync();
        var runner = new ActionServiceInOnlyAsync<IBizActionInOnlyAsync, BizDataIn>(false, utData.WrappedConfig);
        var input = new BizDataIn {Num = num};

        //ATTEMPT
        await runner.RunBizActionDbAndInstanceAsync(_dbContext, bizInstance, input, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
    }

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOutMappingDatabaseOk(int num, bool hasErrors)
    {
        //SETUP
        var config = new GenericBizRunnerConfig {TurnOffCaching = true, DoNotValidateSaveChanges = true};
        var options = SqliteInMemory.CreateOptions<TestDbContext>();
        await using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
            
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, config);
        //utData.AddDtoMapping<ServiceLayerBizOutDto>();
        var bizInstance = new BizActionInOutWriteDbAsync(context);
        var runner =
            new ActionServiceInOutAsync<IBizActionInOutWriteDbAsync, BizDataIn, BizDataOut>(true,
                utData.WrappedConfig);
        var inDto = new BizDataIn {Num = num};

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(context, bizInstance, inDto,
            default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
        if (hasErrors)
            context.LogEntries.Any().ShouldBeFalse();
        else
            context.LogEntries.Single().LogText.ShouldEqual(num.ToString());
    }

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOutMappingOk(int num, bool hasErrors)
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, _noCachingConfig);
        //utData.AddDtoMapping<ServiceLayerBizOutDto>();
        var bizInstance = new BizActionInOutAsync();
        var runner =
            new ActionServiceInOutAsync<IBizActionInOutAsync, BizDataIn, BizDataOut>(false, utData.WrappedConfig);
        var inDto = new BizDataIn {Num = num};

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(_dbContext, bizInstance, inDto, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
        if (hasErrors)
            data.ShouldBeNull();
        else
            data.Output.ShouldEqual(inDto.Num.ToString());
    }

    [Theory]
    [InlineData(123, false)]
    [InlineData(-1, true)]
    public async Task TestActionServiceInOutValuesOkAsync(int num, bool hasErrors)
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, _noCachingConfig);
        var bizInstance = new BizActionInOutAsync();
        var runner =
            new ActionServiceInOutAsync<IBizActionInOutAsync, BizDataIn, BizDataOut>(false, utData.WrappedConfig);
        var input = new BizDataIn {Num = num};

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(_dbContext, bizInstance, input, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldEqual(hasErrors);
        if (hasErrors)
            data.ShouldBeNull();
        else
            data.Output.ShouldEqual(num.ToString());
    }

    //-------------------------------------------------------
    //Async, with mapping, no database access
    //-------------------------------------------------------
    //Async, with mapping, with database access
    //---------------------------------------------------------------
    //checking validation

    [Fact]
    public async Task TestActionServiceOutOnlyMappingDatabaseOk()
    {
        //SETUP
        var options = SqliteInMemory.CreateOptions<TestDbContext>();
        await using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

        var utData =
            NonDiBizSetup.SetupDtoMapping<ServiceLayerBizOutDto>(DefaultMapper._mapper, _noCachingConfig);
        var bizInstance = new BizActionOutOnlyWriteDbAsync(context);
        var runner =
            new ActionServiceOutOnlyAsync<IBizActionOutOnlyWriteDbAsync, BizDataOut>(true,
                utData.WrappedConfig);

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(context, bizInstance, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldBeFalse();
        context.LogEntries.Single().LogText.ShouldEqual("BizActionOutOnlyWriteDbAsync");
    }

    [Fact]
    public async Task TestActionServiceOutOnlyMappingOk()
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizOutDto>(DefaultMapper._mapper, _noCachingConfig);
        var bizInstance = new BizActionOutOnlyAsync();
        var runner = new ActionServiceOutOnlyAsync<IBizActionOutOnlyAsync, BizDataOut>(false, utData.WrappedConfig);

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(_dbContext, bizInstance, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldBeFalse();
        data.Output.ShouldEqual("Result");
    }

    [Fact]
    public async Task TestActionServiceOutOnlyNoDtoOk()
    {
        //SETUP
        var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizOutDto>(DefaultMapper._mapper, _noCachingConfig);
        var bizInstance = new BizActionOutOnlyAsync();
        var runner = new ActionServiceOutOnlyAsync<IBizActionOutOnlyAsync, BizDataOut>(false, utData.WrappedConfig);

        //ATTEMPT
        var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(_dbContext, bizInstance, default).ConfigureAwait(false);

        //VERIFY
        bizInstance.HasErrors.ShouldBeFalse();
        data.Output.ShouldEqual("Result");
    }

    // [Theory]
    // [InlineData(123, false, false)]
    // [InlineData(1, false, true)]
    // [InlineData(1, true, false)]
    // public async Task TestValidation(int num, bool hasErrors, bool doNotValidateSaveChanges)
    // {
    //     //SETUP
    //     var config = new GenericBizRunnerConfig
    //         {TurnOffCaching = true, DoNotValidateSaveChanges = doNotValidateSaveChanges};
    //     var options = SqliteInMemory.CreateOptions<TestDbContext>();
    //     await using var context = new TestDbContext(options);
    //     await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    //     var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerBizInDto>(DefaultMapper._mapper, config);
    //     //utData.AddDtoMapping<ServiceLayerBizOutDto>();
    //     var bizInstance = new BizActionInOutWriteDbAsync(context);
    //     var runner =
    //         new ActionServiceInOutAsync<IBizActionInOutWriteDbAsync, BizDataIn, BizDataOut>(true,
    //             utData.WrappedConfig);
    //     var inDto = new BizDataIn {Num = num};
    //
    //     //ATTEMPT
    //     var data = await runner.RunBizActionDbAndInstanceAsync<BizDataOut>(context, bizInstance, inDto,
    //         default).ConfigureAwait(false);
    //
    //     //VERIFY
    //     bizInstance.HasErrors.ShouldEqual(hasErrors);
    //     if (hasErrors)
    //         context.LogEntries.Any().ShouldBeFalse();
    //     else
    //         context.LogEntries.Single().LogText.ShouldEqual(num.ToString());
    // }

    #endregion Methods
}