// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Diagnostics.CodeAnalysis;
using HBD.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.BizAction.Internal.Runners;

internal class ActionServiceOutOnlyAsync<TBizInterface, TBizOut> : ActionServiceBase
{
    #region Constructors

    public ActionServiceOutOnlyAsync(bool requiresSaveChanges, IWrappedBizRunnerConfigAndMappings wrappedConfig)
        : base(requiresSaveChanges, wrappedConfig)
    {
    }

    #endregion Constructors

    #region Methods

    public async Task<TOut> RunBizActionDbAndInstanceAsync<TOut>(DbContext db, [NotNull]TBizInterface bizInstance,
        CancellationToken cancellationToken= default)
    {
        //var fromBizCopier = DtoAccessGenerator.BuildCopier(typeof(TBizOut), typeof(TOut), false, true, WrappedConfig.Config.TurnOffCaching);
        var bizStatus = (IBizActionStatus) bizInstance!;

        object result = await ((IGenericActionOutOnlyAsync<TBizOut>) bizInstance).BizActionAsync(cancellationToken)
            .ConfigureAwait(false);

        //This handles optional call of save changes
        await SaveChangedIfRequiredAndNoErrorsAsync(db, bizStatus, cancellationToken).ConfigureAwait(false);
        if (bizStatus.HasErrors) return default;

        if (typeof(TOut) == typeof(TBizOut))
            return (TOut) result;

        return WrappedConfig.FromBizIMapper.Map<TOut>(result);
    }

    #endregion Methods
}