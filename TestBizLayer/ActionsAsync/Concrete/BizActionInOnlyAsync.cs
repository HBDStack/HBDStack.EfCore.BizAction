// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction;
using TestBizLayer.BizDTOs;

namespace TestBizLayer.ActionsAsync.Concrete;

public class BizActionInOnlyAsync : BizActionStatus, IBizActionInOnlyAsync
{
    #region Methods

    public Task BizActionAsync(BizDataIn inputData, CancellationToken cancellationToken= default)
    {
        if (inputData.Num >= 0)
            Message = "All Ok";
        else
            AddError("Error");

        return Task.CompletedTask;
    }

    #endregion Methods
}