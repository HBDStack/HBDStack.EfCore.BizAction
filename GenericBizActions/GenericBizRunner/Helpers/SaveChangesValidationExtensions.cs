// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.StatusGeneric;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.BizAction.Helpers;

/// <summary>
///     This static class contains the extension methods for saving data with validation
/// </summary>
public static class SaveChangesValidationExtensions
{

    /// <summary>
    ///     This SaveChanges, with a boolean to decide whether to validate or not
    /// </summary>
    /// <param name="context"></param>
    /// <param name="shouldValidate"></param>
    /// <param name="config"></param>
    /// <returns></returns>
    // public static IStatusGeneric SaveChangesWithOptionalValidation(this DbContext context,
    //     bool shouldValidate, IGenericBizRunnerConfig config)
    // {
    //     return shouldValidate
    //         ? context.SaveChangesWithValidation(config)
    //         : context.SaveChangesWithExtras(config);
    // }

    /// <summary>
    ///     This SaveChangesAsync, with a boolean to decide whether to validate or not
    /// </summary>
    /// <param name="context"></param>
    /// <param name="shouldValidate"></param>
    /// <param name="config"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    // public static async Task<IStatusGeneric> SaveChangesWithOptionalValidationAsync(this DbContext context,
    //     bool shouldValidate, IGenericBizRunnerConfig config, CancellationToken cancellationToken = default)
    // {
    //     return shouldValidate
    //         ? await context.SaveChangesWithValidationAsync(config, cancellationToken).ConfigureAwait(false)
    //         : await context.SaveChangesWithExtrasAsync(config, false, cancellationToken).ConfigureAwait(false);
    // }

    //see https://blogs.msdn.microsoft.com/dotnet/2016/09/29/implementing-seeding-custom-conventions-and-interceptors-in-ef-core-1-0/
    //for why I call DetectChanges before ChangeTracker, and why I then turn ChangeTracker.AutoDetectChangesEnabled off/on around SaveChanges

    /// <summary>
    ///     This will validate any entity classes that will be added or updated
    ///     If the validation does not produce any errors then SaveChanges will be called
    /// </summary>
    /// <param name="context"></param>
    /// <param name="config"></param>
    /// <returns>List of errors, empty if there were no errors</returns>
    // public static IStatusGeneric SaveChangesWithValidation(this DbContext context,
    //     IGenericBizRunnerConfig config = null)
    // {
    //     var status = context.ExecuteValidation();
    //     return status.HasErrors
    //         ? status
    //         : context.SaveChangesWithExtras(config, true);
    // }

    /// <summary>
    ///     This will validate any entity classes that will be added or updated
    ///     If the validation does not produce any errors then SaveChangesAsync will be called
    /// </summary>
    /// <param name="context"></param>
    /// <param name="config"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>List of errors, empty if there were no errors</returns>
    // public static async Task<IStatusGeneric> SaveChangesWithValidationAsync(this DbContext context,
    //     IGenericBizRunnerConfig config = null, CancellationToken cancellationToken = default)
    // {
    //     // var status = context.ExecuteValidation();
    //     // return status.HasErrors
    //     //     ? status
    //     //     : await context.SaveChangesWithExtrasAsync(config, true, cancellationToken).ConfigureAwait(false);
    //     return await context.SaveChangesWithExtrasAsync(config, true, cancellationToken).ConfigureAwait(false);
    // }

    //see https://blogs.msdn.microsoft.com/dotnet/2016/09/29/implementing-seeding-custom-conventions-and-interceptors-in-ef-core-1-0/
    //for why I call DetectChanges before ChangeTracker, and why I then turn ChangeTracker.AutoDetectChangesEnabled off/on around SaveChanges
    //-------------------------------------------------------------------
    //private methods

    // private static IStatusGeneric ExecuteValidation(this DbContext context)
    // {
    //     var status = new StatusGenericHandler();
    //     foreach (var entry in
    //              context.ChangeTracker.Entries()
    //                  .Where(e =>
    //                      e.State is EntityState.Added or EntityState.Modified))
    //     {
    //         var entity = entry.Entity;
    //         var valProvider = new ValidationDbContextServiceProvider(context);
    //         var valContext = new ValidationContext(entity, valProvider, null);
    //         var entityErrors = new List<ValidationResult>();
    //         if (!Validator.TryValidateObject(
    //                 entity, valContext, entityErrors, true))
    //             status.AddValidationResults(entityErrors);
    //     }
    //
    //     return status;
    // }

    // private static IStatusGeneric SaveChangesWithExtras(this DbContext context,
    //     IGenericBizRunnerConfig config, bool turnOffChangeTracker = false)
    // {
    //     // var status = config?.BeforeSaveChanges != null
    //     //     ? config.BeforeSaveChanges(context)
    //     //     : new StatusGenericHandler();
    //     var status = new StatusGenericHandler();
    //     
    //     if (status.HasErrors)
    //         return status;
    //
    //     if (turnOffChangeTracker)
    //         context.ChangeTracker.AutoDetectChangesEnabled = false;
    //     try
    //     {
    //         context.SaveChanges();
    //     }
    //     catch (Exception e)
    //     {
    //         var exStatus = config?.SaveChangesExceptionHandler(e);
    //         if (exStatus == null) throw; //error wasn't handled, so rethrow
    //         status.CombineStatuses(exStatus);
    //     }
    //     finally
    //     {
    //         context.ChangeTracker.AutoDetectChangesEnabled = true;
    //     }
    //
    //     return status;
    // }
    public static async Task<IStatusGeneric> SaveChangesWithExtrasAsync(this DbContext context, IGenericBizRunnerConfig config = null, CancellationToken cancellationToken = default)
        => await context.SaveChangesWithExtrasAsync(config, false, cancellationToken).ConfigureAwait(false);

    public static async Task<IStatusGeneric> SaveChangesWithExtrasAsync(this DbContext context, IGenericBizRunnerConfig config, bool turnOffChangeTracker, CancellationToken cancellationToken = default)
    {
        // var status = config?.BeforeSaveChanges != null
        //     ? config.BeforeSaveChanges(context)
        //     : new StatusGenericHandler();

        var status = new StatusGenericHandler();

        if (status.HasErrors)
            return status;

        if (turnOffChangeTracker)
            context.ChangeTracker.AutoDetectChangesEnabled = false;
        try
        {
            await context.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            var exStatus = config?.SaveChangesExceptionHandler(e);
            if (exStatus == null) throw; //error wasn't handled, so rethrow
            status.CombineStatuses(exStatus);
        }
        finally
        {
            context.ChangeTracker.AutoDetectChangesEnabled = true;
        }

        return status;
    }
}