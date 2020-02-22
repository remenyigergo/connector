using Standard.Contracts.Models.Series;
using System.Threading.Tasks;

namespace Series.Parsers
{
    public interface IParser
    {
        Task<InternalSeries> ImportSeries(string title);
    }
}