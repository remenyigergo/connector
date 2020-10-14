using System.Threading.Tasks;

namespace Series.Parsers.TMDB
{
    public interface ITmdbParser : IParser
    {
        Task<bool> IsMediaExistInTmdb(string title);
    }
}
