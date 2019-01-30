using System;
using System.Collections.Generic;
using System.Text;

namespace Standard.Contracts.Models.Books
{
    public class InternalBook
    {
        //public string Id;
        public int BookId;
        public string Title;
        public string Writer;
        public int Pages;
        public int PublicationYear;
        public int OverallRating;
        public string Sample;
        public System.Enum Genre;

    }
}
