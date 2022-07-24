﻿// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using DataLayer.Dtos;
using DataLayer.EfClasses;
using DataLayer.EfCode;

namespace BizDbAccess.Orders.Concrete;

public class PlaceOrderDbAccess : IPlaceOrderDbAccess
{
    private readonly EfCoreContext _context;

    public PlaceOrderDbAccess(EfCoreContext context)
    {
        _context = context;
    }

    public OrderBooksDto BuildBooksDto(int bookId, byte numBooks)
    {
        return new OrderBooksDto(bookId, _context.Find<Book>(bookId), numBooks);
    }

    public void Add(Order newOrder)
    {
        _context.Orders.Add(newOrder);
    }
}