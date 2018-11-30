using System.Collections.Generic;
using System.Threading.Tasks;
using Standard.Contracts.Models.Series;
using Series.DataManagement.MongoDB.Models.Series;
using Series.Service.Models;
using Series.DataManagement.MongoDB.SeriesFunctionModels;

namespace Series.DataManagement.MongoDB.Repositories
{
    public interface ISeriesRepository
    {
//        Task<List<MongoSeries>> GetAllSeries();
//        Task<List<MongoSeries>> GetSeriesById(int id);
        Task<List<MongoSeries>> GetSeriesByTitle(string name);
        Task AddInternalSeries(InternalSeries series);
        Task DeleteSeriesById(int id);
        Task<bool> IsSeriesImported(string title);
        Task<bool> IsUpToDate(string title, string updateCode);
        Task<bool> Update(InternalSeries internalSeries);
        Task<bool> IsItSeen(int userid, string tvmazeid, string tmdbid, int seasonNumber, int episodeNumber);
        Task MarkAsSeen(int userid, string tvmazeid, string tmdbid, int seasonNumber, int episodeNumber);
        Task AddSeriesToUser(int userid, int seriesid);
        Task<bool> IsSeriesAddedToUser(int userid, int seriesid);
        Task MarkEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel);
        Task<bool> IsEpisodeStarted(InternalEpisodeStartedModel episodeStartedModel);
        Task<bool> DeleteStartedEpisode(string tvmazeid, string tmdbid, int season, int episode);
        Task<bool> GetShow(EpisodeStarted episodeStarted,string title);
        Task<bool> IsShowExistInMongoDb(string title);
        Task<InternalSeries> GetSeries(string title);
        Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode, string showName);

        //TODO: FELFEJLESZTENI
        Task SetFavoriteSeries(int userid, int tvmazeid, int tmdbid);  //többis lehet
        Task<List<FavoriteSeries>> GetAllFavoritesSeries(int userid);
        Task<bool> IsSeriesFavoriteAlready(int userid, int tvmazeid, int tmdbid);
        Task SetFavoriteEpisodes(int userid, int tvmazeid, int tmdbid, int episode, int season);
        Task<bool> IsEpisodeFavoriteAlready(int userid, int tvmazeid, int tmdbid, int episode, int season);
        Task CommentOnSeries(int userid, int tvmazeid, int tmdbid, string message);
        Task CommentOnEpisode(int userid, int tvmazeid, int tmdbid, int episode, int season, string message);
        Task RateSeries(int userid, int tvmazeid, int tmdbid, int rate);
        Task RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate);
        Task<StartedAndSeenEpisodes> GetLastDaysEpisodes(int days);
    }
}

