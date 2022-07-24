// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizAction.Helpers;
using HBDStack.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.BizAction.Internal.Runners;

internal abstract class ActionServiceBase
{
    #region Constructors

    protected ActionServiceBase(bool requiresSaveChanges, IWrappedBizRunnerConfigAndMappings wrappedConfig)
    {
        RequiresSaveChanges = requiresSaveChanges;
        WrappedConfig = wrappedConfig;
    }

    #endregion Constructors

    #region Properties

    protected IWrappedBizRunnerConfigAndMappings WrappedConfig { get; }

    /// <summary>
    ///     This contains info on whether SaveChanges (with validation) should be called after a successful business logic has
    ///     run
    /// </summary>
    private bool RequiresSaveChanges { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    ///     This handled optional save to database with various validation and/or handlers
    ///     Note: if it did save successfully to the database it alters the message
    /// </summary>
    /// <param name="db"></param>
    /// <param name="bizStatus"></param>
    /// <returns></returns>
    // protected void SaveChangedIfRequiredAndNoErrors(DbContext db, IBizActionStatus bizStatus)
    // {
    //     // if (!(bizStatus is IErrorCodeGenericHandler))
    //     // {
    //     //     throw new WarningException(
    //     //         "IBizActionStatus is not instance of ErrorCodeGenericHandler which lead to invalid used " +
    //     //         "for error handling");
    //     // }
    //
    //     if (!bizStatus.HasErrors && RequiresSaveChanges)
    //     {
    //         bizStatus.CombineStatuses(db.SaveChangesWithOptionalValidation(
    //             bizStatus.ShouldValidateSaveChanges(WrappedConfig.Config), WrappedConfig.Config));
    //         WrappedConfig.Config.UpdateSuccessMessageOnGoodWrite(bizStatus, WrappedConfig.Config);
    //     }
    // }

    /// <summary>
    ///     This handled optional save to database with various validation and/or handlers
    ///     Note: if it did save successfully to the database it alters the message
    /// </summary>
    /// <param name="db"></param>
    /// <param name="bizStatus"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected async Task SaveChangedIfRequiredAndNoErrorsAsync(DbContext db, IBizActionStatus bizStatus,
        CancellationToken cancellationToken = default)
    {
        if (!bizStatus.HasErrors && RequiresSaveChanges)
        {
            //var shouldValidate = bizStatus.ShouldValidateSaveChanges(WrappedConfig.Config);
            bizStatus.CombineStatuses(await db
                .SaveChangesWithExtrasAsync(WrappedConfig.Config, cancellationToken).ConfigureAwait(false));
            WrappedConfig.Config.UpdateSuccessMessageOnGoodWrite(bizStatus, WrappedConfig.Config);
        }
    }

    #endregion Methods
}