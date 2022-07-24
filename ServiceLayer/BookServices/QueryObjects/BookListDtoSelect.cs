﻿// Copyright (c) 2016 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Linq;
using DataLayer.EfClasses;

namespace ServiceLayer.BookServices.QueryObjects;

public static class BookListDtoSelect
{
    public static IQueryable<BookListDto>
        MapBookToDto(this IQueryable<Book> books)
    {
        return books.Select(p => new BookListDto
        {
            BookId = p.BookId,
            Title = p.Title,
            PublishedOn = p.PublishedOn,
            ActualPrice = p.ActualPrice,
            OrgPrice = p.OrgPrice,
            PromotionalText = p.PromotionalText,
            AuthorsOrdered = string.Join(", ",
                p.AuthorsLink
                    .OrderBy(q => q.Order)
                    .Select(q => q.Author.Name)),
            ReviewsCount = p.Reviews.Count(),
            ReviewsAverageVotes =
                p.Reviews.Select(y =>
                    (double?) y.NumStars).Average()
        });
    }
}