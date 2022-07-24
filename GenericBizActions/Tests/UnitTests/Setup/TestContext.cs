// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction.Helpers;
using TestBizLayer.DbForTransactions;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Setup;

public class TestContext
{
    [Fact]
    public void TestAddLogEntryOk()
    {
        //SETUP
        var options = SqliteInMemory.CreateOptions<TestDbContext>();
        using var context = new TestDbContext(options);
        context.Database.EnsureCreated();

        //ATTEMPT
        context.Add(new LogEntry("Hello"));
        context.SaveChanges();

        //VERIFY
        context.LogEntries.Count().ShouldEqual(1);
    }

    // [Fact]
    // public async Task TestAddLogEntryValidationError()
    // {
    //     //SETUP
    //     var options = SqliteInMemory.CreateOptions<TestDbContext>();
    //     await using var context = new TestDbContext(options);
    //     await context.Database.EnsureCreatedAsync();
    //
    //     //ATTEMPT
    //     context.Add(new LogEntry("!"));
    //     var errors =await context.SaveChangesWithExtrasAsync();
    //
    //     //VERIFY
    //     context.LogEntries.Count().ShouldEqual(0);
    //     errors.HasErrors.ShouldBeTrue();
    // }

    [Fact]
    public async Task TestAddLogEntryValidationOk()
    {
        //SETUP
        using var options = SqliteInMemory.CreateOptions<TestDbContext>();
        await using var context = new TestDbContext(options);
        await context.Database.EnsureCreatedAsync();

        //ATTEMPT
        context.Add(new LogEntry("Hello"));
        var errors =await context.SaveChangesWithExtrasAsync();

        //VERIFY
        context.LogEntries.Count().ShouldEqual(1);
        errors.HasErrors.ShouldBeFalse();
    }
}