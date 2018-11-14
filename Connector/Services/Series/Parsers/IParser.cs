
using System.Threading.Tasks;

namespace Series.Parsers
{
    public interface IParser
    {
        Task<Standard.Contracts.Models.Series.InternalSeries> ImportSeriesFromTvMaze(string title);
    }
}