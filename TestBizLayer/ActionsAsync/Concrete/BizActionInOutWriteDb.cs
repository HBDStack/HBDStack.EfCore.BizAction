// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction;
using TestBizLayer.BizDTOs;
using TestBizLayer.DbForTransactions;

namespace TestBizLayer.ActionsAsync.Concrete;

public class BizActionInOutWriteDbAsync : BizActionStatus, IBizActionInOutWriteDbAsync
{
    #region Fields

    private readonly TestDbContext _context;

    #endregion Fields

    #region Constructors

    public BizActionInOutWriteDbAsync(TestDbContext context)
    {
        _context = context;
    }

    #endregion Constructors

    #region Methods

    public Task<BizDataOut> BizActionAsync(BizDataIn inputData, CancellationToken cancellationToken= default)
    {
        //Put it here so that if SaveChanges is called it will be in database
        _context.Add(new LogEntry(inputData.Num.ToString()));
        if (inputData.Num >= 0)
            Message = "All Ok";
        else
            AddError("Error");

        return Task.FromResult(new BizDataOut(inputData.Num.ToString()));
    }

    #endregion Methods
}