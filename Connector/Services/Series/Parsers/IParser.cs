﻿
using System.Threading.Tasks;

namespace Series.Parsers
{
    public interface IParser
    {
        Task<Contracts.Models.Series.InternalSeries> ImportSeries(string title);
    }
}