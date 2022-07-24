// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using System.Threading.Tasks;
using HBD.EfCore.BizAction.Helpers;
using HBDStack.StatusGeneric;
using Microsoft.EntityFrameworkCore;
using TestBizLayer.DbForTransactions;
using TestSupport.EfHelpers;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Setup;

public class TestBeforeSaveChangesAsync
{
    #region Methods

    // [Fact]
    // public async Task TestBeforeSaveChangesMethodProvidedNoError()
    // {
    //     //SETUP
    //     var options = SqliteInMemory.CreateOptions<TestDbContext>();
    //     options.StopNextDispose();
    //
    //     await using (var context = new TestDbContext(options))
    //     {
    //         await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    //
    //         var config = new GenericBizRunnerConfig
    //         {
    //             BeforeSaveChanges = FailOnBadWord
    //         };
    //
    //         //ATTEMPT
    //         context.Add(new UniqueEntity {UniqueString = "good word"});
    //         var status = await context.SaveChangesWithValidationAsync(config).ConfigureAwait(false);
    //
    //         //VERIFY
    //         status.HasErrors.ShouldBeFalse(status.GetAllErrors());
    //     }
    //
    //     await using (var context = new TestDbContext(options))
    //     {
    //         context.UniqueEntities.Count().ShouldEqual(1);
    //     }
    // }

    // [Fact]
    // public async Task TestBeforeSaveChangesMethodProvidedWithError()
    // {
    //     //SETUP
    //     var options = SqliteInMemory.CreateOptions<TestDbContext>();
    //     options.StopNextDispose();
    //
    //     await using (var context = new TestDbContext(options))
    //     {
    //         await context.Database.EnsureCreatedAsync().ConfigureAwait(false);
    //
    //         var config = new GenericBizRunnerConfig
    //         {
    //             BeforeSaveChanges = FailOnBadWord
    //         };
    //
    //         //ATTEMPT
    //         context.Add(new UniqueEntity {UniqueString = "bad word"});
    //         var status = await context.SaveChangesWithValidationAsync(config).ConfigureAwait(false);
    //
    //         //VERIFY
    //         status.HasErrors.ShouldBeTrue();
    //         status.GetAllErrors().ShouldEqual("The UniqueEntity class contained a bad word.");
    //     }
    //
    //     await using (var context = new TestDbContext(options))
    //     {
    //         context.UniqueEntities.Count().ShouldEqual(0);
    //     }
    // }

    [Fact]
    public async Task TestNoBeforeSaveChangesMethodProvided()
    {
        //SETUP
        var options = SqliteInMemory.CreateOptions<TestDbContext>();
        options.StopNextDispose();

        await using (var context = new TestDbContext(options))
        {
            await context.Database.EnsureCreatedAsync().ConfigureAwait(false);

            //ATTEMPT
            context.Add(new UniqueEntity {UniqueString = "bad word"});
            var status = await context.SaveChangesWithExtrasAsync().ConfigureAwait(false);

            //VERIFY
            status.HasErrors.ShouldBeFalse(status.GetAllErrors());
        }

        await using (var context = new TestDbContext(options)) 
            context.UniqueEntities.Count().ShouldEqual(1);
    }

    //-------------------------------------------------
    //BeforeSaveChanges test setup

    private IStatusGeneric FailOnBadWord(DbContext context)
    {
        var status = new StatusGenericHandler();
        var entriesToCheck = context.ChangeTracker.Entries()
            .Where(e =>
                e.State == EntityState.Added ||
                e.State == EntityState.Modified);
        foreach (var entity in entriesToCheck)
            if (entity.Entity is UniqueEntity normalInstance && normalInstance.UniqueString.Contains("bad"))
                status.AddError($"The {nameof(UniqueEntity)} class contained a bad word.");

        return status;
    }

    #endregion Methods
}