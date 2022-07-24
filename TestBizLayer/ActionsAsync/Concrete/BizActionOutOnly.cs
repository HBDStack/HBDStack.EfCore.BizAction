// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction;
using TestBizLayer.BizDTOs;

namespace TestBizLayer.ActionsAsync.Concrete;

public class BizActionOutOnlyAsync : BizActionStatus, IBizActionOutOnlyAsync
{
    #region Methods

    public Task<BizDataOut> BizActionAsync(CancellationToken cancellationToken= default)
    {
        Message = "All Ok";
        return Task.FromResult(new BizDataOut("Result"));
    }

    #endregion Methods
}