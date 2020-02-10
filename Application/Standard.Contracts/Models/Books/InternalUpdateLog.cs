using System;

namespace Standard.Contracts.Models.Books
{
    public class InternalUpdateLog
    {
        public int PageNumber { get; set; }
        public int HoursRead { get; set; }
        public int MinutesRead { get; set; }
        public DateTime UpDateTime { get; set; }
    }
}