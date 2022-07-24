﻿// Copyright (c) 2016 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using DataLayer.EfCode;
using DataLayer.QueryObjects;
using Microsoft.EntityFrameworkCore;
using ServiceLayer.BookServices.QueryObjects;

namespace ServiceLayer.BookServices.Concrete;

public class ListBooksService
{
    private readonly EfCoreContext _context;

    public ListBooksService(EfCoreContext context)
    {
        _context = context;
    }

    public IQueryable<BookListDto> SortFilterPage
        (SortFilterPageOptions options)
    {
        var booksQuery = _context.Books
            .AsNoTracking()
            .MapBookToDto()
            .OrderBooksBy(options.OrderByOptions)
            .FilterBooksBy(options.FilterBy,
                options.FilterValue);

        options.SetupRestOfDto(booksQuery);

        return booksQuery.Page(options.PageNum - 1,
            options.PageSize);
    }
}

/*********************************************************
#A This starts by selecting the Books property in the Application's DbContext 
#B Because this is a read-only query I add .AsNoTracking(). It makes the query faster
#C It then uses the Select query object which will pick out/calculate the data it needs
#D It then adds the commands to order the data using the given options
#E Then it adds the commands to filter the data
#F This stage sets up the number of pages and also makes sure PageNum is in the right range
#G Finally it applies the paging commands
    * *****************************************************/