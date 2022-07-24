﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.EfClasses;
using DataLayer.EfCode;

namespace BizDbAccess.Orders.Concrete;

public class ChangeDeliverDbAccess : IChangeDeliverDbAccess
{
    private readonly EfCoreContext _context;

    public ChangeDeliverDbAccess(EfCoreContext context)
    {
        _context = context;
    }

    public Order GetOrder(int orderId)
    {
        return _context.Find<Order>(orderId);
    }
}