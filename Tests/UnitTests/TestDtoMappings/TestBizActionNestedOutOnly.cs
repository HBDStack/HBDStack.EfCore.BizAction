// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizAction.Configuration;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;
using TestBizLayer.DbForTransactions;
using TestSupport.EfHelpers;

namespace Tests.UnitTests.TestDtoMappings;

public class TestBizActionNestedOutOnly
{
    private readonly IGenericBizRunnerConfig _config = new GenericBizRunnerConfig {TurnOffCaching = true};

    //This action does not access the database, but the ActionService checks that the dbContext isn't null
    private readonly DbContext _emptyDbContext = new TestDbContext(SqliteInMemory.CreateOptions<TestDbContext>());

    // [Fact]
    // public void TestActionServiceNestedOutUsingGenericActionFromBizDtoOk()
    // {
    //     //SETUP 
    //     var utData = NonDiBizSetup.SetupDtoMapping<ServiceLayerNestedOutDto>(DefaultMapper._mapper, _config);
    //     //utData.AddDtoMapping<ServiceLayerNestedOutChildDto>();
    //     var wrappedConfig = utData.WrappedConfig;
    //     var bizInstance = new BizActionNestedOutOnlyAsync();
    //     var runner = new ActionServiceAsync<IBizActionNestedOutOnlyAsync>(_emptyDbContext, bizInstance, wrappedConfig);
    //
    //     //ATTEMPT
    //     var data = runner.RunBizActionAsync<NestedBizDataOut>();
    //
    //     //VERIFY
    //     bizInstance.HasErrors.ShouldBeFalse();
    //     data.Output.ShouldEqual("Test");
    //     //data.ChildData.ChildInt.ShouldEqual(123);
    //     //data.ChildData.ChildString.ShouldEqual("Nested");
    // }
}