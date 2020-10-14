using System.Threading.Tasks;

namespace Series.Parsers.TVMAZE
{
    public interface ITvMazeParser : IParser
    {
        Task<bool> IsMediaExistInTvMaze(string title);
    }
}
