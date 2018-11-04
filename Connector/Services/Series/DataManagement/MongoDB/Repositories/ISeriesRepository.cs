using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts.Models.Series;
using Series.DataManagement.MongoDB.Models.Series;
using Series.Service.Models;
using Season = Series.DataManagement.MongoDB.Models.Series.Season;

namespace Series.DataManagement.MongoDB.Repositories
{
    public interface ISeriesRepository
    {
//        Task<List<MongoSeries>> GetAllSeries();
//        Task<List<MongoSeries>> GetSeriesById(int id);
        Task<List<MongoSeries>> GetSeriesByTitle(string name);
        Task AddInternalSeries(InternalSeries series);
        Task AddEpisodes(List<InternalEpisode> episodes);
        Task AddSeason(Season season, int seriesId);
        Task DeleteSeriesById(int id);
        Task<bool> IsSeriesImported(string title);
        Task<bool> IsUpToDate(string title, string updateCode);
        Task<bool> Update(InternalSeries internalSeries);

        Task MarkAsSeen(int userid, int seriesid, int seasonNumber, int episodeNumber);
        Task AddSeriesToUser(int userid, int seriesid);
        Task<bool> IsSeriesAddedToUser(int userid, int seriesid);
        Task MarkEpisodeStarted(EpisodeStartedModel episodeStartedModel);
        Task<bool> IsEpisodeStarted(EpisodeStartedModel episodeStartedModel);
    }
}
