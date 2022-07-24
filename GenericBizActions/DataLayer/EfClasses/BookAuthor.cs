// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace DataLayer.EfClasses;

public class BookAuthor
{
    private BookAuthor()
    {
    }

    internal BookAuthor(Book book, Author author, byte order)
    {
        Book = book;
        Author = author;
        Order = order;
    }

    public int BookId { get; private set; }
    public int AuthorId { get; private set; }
    public byte Order { get; }

    //-----------------------------
    //Relationships

    public Book Book { get; }
    public Author Author { get; }
}