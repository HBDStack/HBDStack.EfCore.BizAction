// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBD.EfCore.BizAction;
using TestBizLayer.BizDTOs;
using TestBizLayer.DbForTransactions;

namespace TestBizLayer.ActionsAsync.Concrete;

public class BizActionOutOnlyWriteDbAsync : BizActionStatus, IBizActionOutOnlyWriteDbAsync
{
    #region Fields

    private readonly TestDbContext _context;

    #endregion Fields

    #region Constructors

    public BizActionOutOnlyWriteDbAsync(TestDbContext context)
    {
        _context = context;
    }

    #endregion Constructors

    #region Methods

    public Task<BizDataOut> BizActionAsync(CancellationToken cancellationToken= default)
    {
        _context.Add(new LogEntry(GetType().Name));
        Message = "All Ok";
        return Task.FromResult(new BizDataOut("Result"));
    }

    #endregion Methods
}