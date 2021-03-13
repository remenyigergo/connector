using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Series.Parsers.TMDB.Models.TmdbShowModels;
using Series.Parsers.TvMaze.Models;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Contracts.Models.Series.ExtendClasses.Cast;
using Standard.Core.Configuration;
using Standard.Core.NetworkManager;

namespace Series.Parsers.TVMAZE
{
    public class TvMazeParser : ITvMazeParser, IParser
    {
        private readonly IServiceConfiguration _configuration;

        public TvMazeParser(IServiceConfiguration config)
        {
            _configuration = config;
        }

        //LESZEDJÜK AZ APIN KERESZTÜL ÉS ÁTALAKÍTJUK SAJÁT SOROZAT FORMÁTUMRA
        public async Task<InternalSeries> ImportSeries(string title)
        {
            try
            {
                var tvMazeSeries =
                    await new WebClientManager().Get<List<TvMazeSeries>>($"{_configuration.Endpoints.TvMaze}/search/shows?q={title}");
                var firstSeries = tvMazeSeries[0];
                var seasons =
                    await ImportSeasons(firstSeries.Show.Name, int.Parse(firstSeries.Show.Id)); //ÖSSZES SZEZON
                var cast = await GetCast(int.Parse(firstSeries.Show.Id));

                //A végleges belső MongoSeries modelje
                if (firstSeries != null && seasons != null)
                    return new InternalSeries
                    {
                        ExternalIds = new Dictionary<string, string>() { { "TvMaze", firstSeries.Show.Id } },
                        TvMazeId = firstSeries.Show.Id,
                        Runtime = new List<string> {firstSeries.Show.Runtime},
                        Title = firstSeries.Show.Name,
                        Seasons = seasons,
                        Categories = firstSeries.Show.Genres,
                        Description = firstSeries.Show.Summary,
                        Rating = firstSeries.Show.Rating.Average,
                        LastUpdated = firstSeries.Show.Updated,
                        Cast = cast
                    };
            }
            catch
            {
                throw new InternalException(605, "Series not found on TVMAZE.");
            }


            return null;
        }

        public async Task<List<InternalEpisode>> ImportEpisodes(int id)
        {
            var tvMazeEpisodes =
                await new WebClientManager().Get<List<TvMazeEpisode>>($"{_configuration.Endpoints.TvMaze}/shows/{id}/episodes");

            var episodeList = new List<InternalEpisode>();

            if (tvMazeEpisodes != null)
            {
                foreach (var episode in tvMazeEpisodes)
                    episodeList.Add(new InternalEpisode
                    {
                        Title = episode.Name,
                        //Rating = episode.Rating,
                        Description = episode.Summary,
                        Length = episode.Runtime,
                        EpisodeNumber = int.Parse(episode.Number),
                        SeasonNumber = int.Parse(episode.Season)
                    });
                return episodeList;
            }

            return null;
        }


        public async Task<List<InternalSeason>> ImportSeasons(string title, int id)
        {
            var tvMazeSeasons = await new WebClientManager().Get<List<TvMazeSeason>>($"{_configuration.Endpoints.TvMaze}/shows/{id}/seasons");
            var seasons = new Dictionary<int, InternalSeason>();

            if (tvMazeSeasons != null)
            {
                foreach (var season in tvMazeSeasons)
                    seasons.Add(season.SeasonNumber, new InternalSeason
                    {
                        TvMazeId = season.Id,
                        Summary = season.Summary,
                        EpisodesCount = season.EpisodesCount ?? 0,
                        SeasonNumber = season.SeasonNumber,
                        Episodes = new List<InternalEpisode>()
                    });

                await FillEpisodes(id, seasons);
                return seasons.Values.ToList();
            }

            return null;
        }

        private async Task FillEpisodes(int id, Dictionary<int, InternalSeason> seasons)
        {
            var episodeList = await ImportEpisodes(id);
            foreach (InternalEpisode episode in episodeList)
                seasons[episode.SeasonNumber].Episodes.Add(episode);
        }

        public async Task<InternalShowCast> GetCast(int showId)
        {
            List<InternalActor> personList = new List<InternalActor>();
            var tmdbShowCast =
                await new WebClientManager().Get<List<TvMazeShowCast>>($"{_configuration.Endpoints.TvMaze}/shows/{showId}/cast");

            foreach (var character in tmdbShowCast)
                personList.Add(new InternalActor
                {
                    CharacterName = character.Character.Name,
                    RealName = character.Person.Name
                });

            return new InternalShowCast {Persons = personList};
        }


        //EZT A DESKTOP APP MIATT RAKTAM IDE
        public async Task<bool> GetShow(string title)
        {
            try
            {
                var s = await new WebClientManager().Get<List<TvMazeSeries>>($"{_configuration.Endpoints.TvMaze}/search/shows?q={title}");

                if (s.Count > 0)
                {
                    var removedAccent = RemoveAccent(s[0].Show.Name.ToLower());
                    if (title.ToLower().Equals(removedAccent))
                        return true;
                }
            }
            catch (Exception)
            {
                throw new InternalException(605, "Couldn't fetch series!");
            }
            return false;
        }


        public async Task<bool> IsMediaExistInTvMaze(string title)
        {
            var boolean = await new WebClientManager().Get<List<TvMazeSeries>>($"{_configuration.Endpoints.TvMaze}/search/shows?q={title}");
            if (boolean.Count > 0)
                foreach (var series in boolean)
                {
                    var seriesName = RemoveAccent(series.Show.Name.ToLower());

                    if (seriesName == title.ToLower())
                        return true;
                }

            return false;
        }

        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            var filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new string(filtered);
        }
        //        }
        //            }
        //                internalSeason.Episodes = internalSeason.Episodes.OrderBy(e => e.EpisodeNumber).ToList();
        //            {
        //            foreach (var internalSeason in seasonList)
        //            seasonList.Values = seasonList.Values.OrderBy(s => s.SeasonNumber).ToList();
        //        {


        //        private static void SortSeasonsAndEpisodes(ref Dictionary<int, InternalSeason> seasonList)
    }
}