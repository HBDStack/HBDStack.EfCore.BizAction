// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;
using HBDStack.EfCore.BizAction;
using TestBizLayer.DbForTransactions;

namespace TestBizLayer.ActionsAsync.Concrete;

public class BizActionCheckSqlErrorHandlerWriteDbAsync : BizActionStatus, IBizActionCheckSqlErrorHandlerWriteDbAsync
{
    #region Fields

    private readonly TestDbContext _context;

    #endregion Fields

    #region Constructors

    public BizActionCheckSqlErrorHandlerWriteDbAsync(TestDbContext context)
    {
        _context = context;
    }

    #endregion Constructors

    #region Methods

    public Task BizActionAsync(string setUnique, CancellationToken cancellationToken= default)
    {
        _context.Add(new UniqueEntity {UniqueString = setUnique});
        return Task.CompletedTask;
    }

    #endregion Methods
}