﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBD.EfCore.BizAction;
using TestBizLayer.DbForTransactions;

namespace TestBizLayer.Actions.Concrete;

public class BizActionCheckSqlErrorHandlerWriteDb : BizActionStatus, IBizActionCheckSqlErrorHandlerWriteDb
{
    private readonly TestDbContext _context;

    public BizActionCheckSqlErrorHandlerWriteDb(TestDbContext context)
    {
        _context = context;
    }

    public void BizAction(string setUnique)
    {
        _context.Add(new UniqueEntity {UniqueString = setUnique});
    }
}