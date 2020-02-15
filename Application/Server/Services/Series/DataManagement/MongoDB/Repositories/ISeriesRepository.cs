using System.Collections.Generic;
using System.Threading.Tasks;
using Series.DataManagement.MongoDB.Models.Series;
using Series.DataManagement.MongoDB.SeriesFunctionModels;
using Standard.Contracts.Models.Series;

namespace Series.DataManagement.MongoDB.Repositories
{
    public interface ISeriesRepository
    {
        Task<List<InternalSeries>> GetSeriesByTitle(string name);
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
        Task<bool> GetShow(InternalEpisodeStartedModel episodeStarted, string title);
        Task<bool> IsMediaExistInMongoDb(string title);
        Task<InternalSeries> GetSeries(string title);
        Task<bool> UpdateStartedEpisode(InternalEpisodeStartedModel internalEpisode);

        //TODO: FELFEJLESZTENI
        Task SetFavoriteSeries(int userid, int tvmazeid, int tmdbid); //többis lehet
        Task<List<InternalFavoriteSeries>> GetAllFavoritesSeries(int userid);
        Task<bool> IsSeriesFavoriteAlready(int userid, int tvmazeid, int tmdbid);

        Task SetFavoriteEpisodes(int userid, int tvmazeid, int tmdbid, int episode,
            int season); //egy epizód csak 1x, viszont több kedvenc epizód is lehet

        Task<bool> IsEpisodeFavoriteAlready(int userid, int tvmazeid, int tmdbid, int episode, int season);

        Task CommentOnSeries(int userid, int tvmazeid, int tmdbid, string message); //akármennyi komment lehetséges

        Task CommentOnEpisode(int userid, int tvmazeid, int tmdbid, int episode, int season,
            string message); //akármennyi lehetséges

        Task RateSeries(int userid, int tvmazeid, int tmdbid, int rate); //folyamatos frissítéssel akárhányszor

        Task<bool> RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season,
            int rate); //folyamatos frissítéssel akárhányszor

        Task<InternalStartedAndSeenEpisodes> GetLastDaysEpisodes(int days, int userid);
        Task<List<InternalSeries>> RecommendSeries(int userid);

        Task<List<Standard.Contracts.Models.Series.InternalSeries>> RecommendSeries(
            List<Standard.Contracts.Models.Series.ExtendClasses.InternalSeriesGenre> genre, string username, int userid);

        Task<InternalReturnSeriesEpisodeModel>
            GetSeriesByStartedEpisode(string show, int seasonnum, int episodenum, int userid);

        Task<List<InternalEpisodeSeen>> PreviousEpisodeSeen(int seasonnum, int episodenum, int tvmazeid, int tmbdid,
            int userid);

        //Task<List<InternalEpisode>> GetNotSeenEpisodes(int seasonNum, List<int> notSeenEpisodeIds, int tvmazeid, int tmbdid);
    }
}