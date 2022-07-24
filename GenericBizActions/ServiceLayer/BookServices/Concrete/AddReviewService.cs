﻿// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using DataLayer.EfClasses;
using DataLayer.EfCode;

namespace ServiceLayer.BookServices.Concrete;

public class AddReviewService : IAddReviewService
{
    private readonly EfCoreContext _context;

    public AddReviewService(EfCoreContext context)
    {
        _context = context;
    }

    public AddReviewDto GetOriginal(int id)
    {
        var dto = _context.Books
            .Select(p => new AddReviewDto
            {
                BookId = p.BookId,
                Title = p.Title
            })
            .SingleOrDefault(k => k.BookId == id);
        if (dto == null)
            throw new InvalidOperationException($"Could not find the book with Id of {id}.");
        return dto;
    }

    public Book AddReviewToBook(AddReviewDto dto)
    {
        var book = _context.Find<Book>(dto.BookId);
        if (book == null)
            throw new InvalidOperationException($"Could not find the book with Id of {dto.BookId}.");
        book.AddReview(dto.NumStars, dto.Comment, dto.VoterName, _context);
        _context.SaveChanges();
        return book;
    }
}