// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using TestBizLayer.BizDTOs;

namespace TestBizLayer.ActionsTransactional.Concrete;

public class BizTranActionFinalAsync : BizTranActionBase, IBizTranActionFinalAsync
{
    #region Constructors

    public BizTranActionFinalAsync()
        : base(3)
    {
    }

    #endregion Constructors

    #region Methods

    public Task<BizDataGuid> BizActionAsync(BizDataGuid input, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(BizAction(input));
    }

    #endregion Methods
}