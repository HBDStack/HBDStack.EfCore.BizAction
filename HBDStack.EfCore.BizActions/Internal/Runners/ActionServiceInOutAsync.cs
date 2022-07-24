// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HBDStack.EfCore.BizAction.Internal.Runners;

internal class ActionServiceInOutAsync<TBizInterface, TBizIn, TBizOut> : ActionServiceBase
{
    #region Constructors

    public ActionServiceInOutAsync(bool requiresSaveChanges, IWrappedBizRunnerConfigAndMappings wrappedConfig)
        : base(requiresSaveChanges, wrappedConfig)
    {
    }

    #endregion Constructors

    #region Methods

    public async Task<TOut> RunBizActionDbAndInstanceAsync<TOut>(DbContext db, TBizInterface bizInstance,
        object inputData, CancellationToken cancellationToken= default)
    {
        //var toBizCopier = DtoAccessGenerator.BuildCopier(inputData.GetType(), typeof(TBizIn), true, true, WrappedConfig.Config.TurnOffCaching);
        //var fromBizCopier = DtoAccessGenerator.BuildCopier(typeof(TBizOut), typeof(TOut), false, true, WrappedConfig.Config.TurnOffCaching);
        var bizStatus = (IBizActionStatus) bizInstance;

        //The SetupSecondaryData produced errors
        if (bizStatus.HasErrors) return default;

        //var inData = await toBizCopier.DoCopyToBizAsync<TBizIn>(db, WrappedConfig.ToBizIMapper, inputData).ConfigureAwait(false);

        object result = await ((IGenericActionAsync<TBizIn, TBizOut>) bizInstance)
            .BizActionAsync((TBizIn) inputData, cancellationToken).ConfigureAwait(false);

        //This handles optional call of save changes
        await SaveChangedIfRequiredAndNoErrorsAsync(db, bizStatus, cancellationToken).ConfigureAwait(false);
        if (bizStatus.HasErrors) return default;


        if (typeof(TOut) == typeof(TBizOut))
            return (TOut) result;

        return WrappedConfig.FromBizIMapper.Map<TBizOut, TOut>((TBizOut) result);
    }

    #endregion Methods
}