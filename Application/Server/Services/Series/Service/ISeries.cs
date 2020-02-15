using Series.Dto.RequestDtoModels;
using Series.Dto.RequestDtoModels.SeriesDto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Series.Service
{
    public interface ISeries
    {
        Task ImportSeries(string title);
        Task<bool> MarkAsSeen(int userid, string tvmazeid, string tmdbid, int season, int episode, string showname);
        Task AddSeriesToUser(int userid, int seriesid);
        Task MarkEpisodeStarted(EpisodeDto episodeStartedModel, string showName);
        Task UpdateSeries(string title);
        Task<bool> IsSeriesImported(string title);
        Task CheckSeriesUpdate(SeriesDto internalSeries);
        Task<bool> GetShow(EpisodeStartedDto episodeStarted, string title);
        Task<SeriesDto> GetSeries(string title);
        Task<bool> IsMediaExistInMongoDb(string title);
        Task<bool> IsMediaExistInTvMaze(string title);
        Task<bool> IsMediaExistInTmdb(string title);
        Task<List<SeriesDto>> GetSeriesByTitle(string title);
        Task<bool> IsEpisodeStarted(EpisodeStartedDto internalEpisode);
        Task<bool> UpdateStartedEpisode(EpisodeStartedDto internalEpisode, string showName);
        Task RateEpisode(int userid, int? tvmazeid, int? tmdbid, int episode, int season, int rate);
        Task<StartedAndSeenEpisodesDto> GetLastDays(int days, int userid);
        Task<int> IsShowExist(ImportSeriesDto request);
        Task<List<SeriesDto>> RecommendSeriesFromDb(int userid);
        Task<List<SeriesDto>> RecommendSeriesFromDbByGenre(List<string> genres, string username,
            int userid);
        Task<List<EpisodeDto>> PreviousEpisodeSeen(string showTitle, int seasonNum, int episodeNum,
            int userid);
    }
}
