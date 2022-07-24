﻿// // Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// // Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using HBDStack.StatusGeneric;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfClasses;

public class Book
{
    public const int PromotionalTextLength = 200;
    private readonly HashSet<BookAuthor> _authorsLink;

    //-----------------------------------------------
    //relationships

    //Use uninitialised backing fields - this means we can detect if the collection was loaded
    private readonly HashSet<Review> _reviews;

    //-----------------------------------------------
    //ctors

    private Book()
    {
    }

    public Book(string title, string description, DateTime publishedOn,
        string publisher, decimal price, string imageUrl, ICollection<Author> authors)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentNullException(nameof(title));

        Title = title;
        Description = description;
        PublishedOn = publishedOn;
        Publisher = publisher;
        ActualPrice = price;
        OrgPrice = price;
        ImageUrl = imageUrl;
        _reviews =
            new HashSet<Review>(); //We add an empty list on create. I allows reviews to be added when building test data

        if (authors == null || !authors.Any())
            throw new ArgumentException("You must have at least one Author for a book", nameof(authors));
        byte order = 0;
        _authorsLink = new HashSet<BookAuthor>(authors.Select(a => new BookAuthor(this, a, order++)));
    }

    public int BookId { get; private set; }
    public string Title { get; }
    public string Description { get; }
    public DateTime PublishedOn { get; private set; }
    public string Publisher { get; }
    public decimal OrgPrice { get; }
    public decimal ActualPrice { get; private set; }

    [MaxLength(PromotionalTextLength)] public string PromotionalText { get; private set; }

    public string ImageUrl { get; }

    public IEnumerable<Review> Reviews => _reviews?.ToList();
    public IEnumerable<BookAuthor> AuthorsLink => _authorsLink?.ToList();

    public void UpdatePublishedOn(DateTime newDate)
    {
        PublishedOn = newDate;
    }

    public void AddReview(int numStars, string comment, string voterName,
        DbContext context = null)
    {
        if (_reviews != null)
            _reviews.Add(new Review(numStars, comment, voterName));
        else if (context == null)
            throw new ArgumentNullException(nameof(context),
                "You must provide a context if the Reviews collection isn't valid.");
        else if (context.Entry(this).IsKeySet)
            context.Add(new Review(numStars, comment, voterName, BookId));
        else
            throw new InvalidOperationException("Could not add a new review.");
    }

    public void RemoveReview(Review review)
    {
        if (_reviews == null)
            throw new NullReferenceException("You must use .Include(p => p.Reviews) before calling this method.");

        _reviews.Remove(review);
    }

    public IStatusGeneric AddPromotion(decimal newPrice, string promotionalText)
    {
        var status = new StatusGenericHandler();
        if (string.IsNullOrWhiteSpace(promotionalText))
            return status.AddError("You must provide some text to go with the promotion.", nameof(PromotionalText));

        ActualPrice = newPrice;
        PromotionalText = promotionalText;
        return status;
    }

    public void RemovePromotion()
    {
        ActualPrice = OrgPrice;
        PromotionalText = null;
    }
}