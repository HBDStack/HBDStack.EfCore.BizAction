// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.ComponentModel.DataAnnotations;

namespace DataLayer.EfClasses;

public class Review
{
    public const int NameLength = 100;

    private Review()
    {
    }

    internal Review(int numStars, string comment, string voterName, int bookId = 0)
    {
        NumStars = numStars;
        Comment = comment;
        VoterName = voterName;
        BookId = bookId;
    }

    public int ReviewId { get; private set; }

    [MaxLength(NameLength)] public string VoterName { get; }

    public int NumStars { get; }
    public string Comment { get; }

    //-----------------------------------------
    //Relationships

    public int BookId { get; }
}