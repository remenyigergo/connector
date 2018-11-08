using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Contracts.Models.Series;
using Contracts.Models.Series.ExtendClasses;
using Contracts.Models.Series.ExtendClasses.Cast;
using Core.NetworkManager;
using Series.Parsers.TMDB.Models;
using Series.Parsers.TMDB.Models.TmdbShowModels;
using Series.Parsers.TMDB.Models.TmdbShowModels.ConvertHelper;
using Series.Parsers.TMDB.Models.TmdbShowModels.SeasonModel;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Series.Parsers.TMDB
{
    public class TmdbParser
    {
        //https://api.themoviedb.org/3/search/tv?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US&page=1&query=fargo  ez egy sorozat keresés
        //https://api.themoviedb.org/3/tv/60622?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US ez mindennel együtt visszaadja de ID kell hozzá
        //https://api.themoviedb.org/3/tv/{tv_id}/season/{season_number}?api_key=<<api_key>>&language=en-US  EZ EGY NEM SIMPLE SEASON


        private const string _endpoint = "https://api.themoviedb.org";
        private const string _lang = "en-US";
        private const string _key = "e9443688992dbb4fa3940ed77a0a8e1d";
        

        public async Task<InternalSeries> ImportTmdbSeries(string title)
        {
            var tmdbQueryShow = await new WebClientManager().Get<TmdbQueryShow>($"{_endpoint}/3/search/tv?api_key={_key}&language={_lang}&page=1&query={title}");
            var tmdbShowForID = tmdbQueryShow.Results[0];
            var tmdbShowSimple = await new WebClientManager().Get<TmdbShow>($"{_endpoint}/3/tv/{tmdbShowForID.Id}?api_key={_key}&language={_lang}");


            var seasons = GetSeasons(tmdbShowSimple, tmdbShowForID.Id);
            

            return new InternalSeries()
            {
                Id = tmdbShowSimple.Id,
                Runtime = tmdbShowSimple.EpisodeRunTime.Select(x=>x.Length.ToString()).ToList(),
                Title = tmdbShowSimple.Name,
                Seasons = await seasons,
                Categories = tmdbShowSimple.Genres.Select(x => x.Name).ToList(),
                Description = tmdbShowSimple.Overview,
                Rating = tmdbShowSimple.VoteAverage,
                CreatedBy = tmdbShowSimple.CreatedBy.Select(x => new InternalCreator() { Name = x.Name }).ToList(),
                EpisodeRunTime = tmdbShowSimple.EpisodeRunTime,
                FirstAirDate = tmdbShowSimple.FirstAirDate,
                Genres = tmdbShowSimple.Genres.Select(x => new InternalGenre() { Name = x.Name }).ToList(),
                LastEpisodeSimpleToAir = InternalConverter.ConvertTmdbEpisodeToInternal(tmdbShowSimple.LastEpisodeToAir),
                Networks = InternalConverter.ConvertTmdbNetworkToInternal(tmdbShowSimple.Networks),
                Popularity = tmdbShowSimple.Popularity,
                ProductionCompanies = InternalConverter.ConvertTmdbProductionCompanyToInternal(tmdbShowSimple.ProductionCompanies),
                Status = tmdbShowSimple.Status,
                Type = tmdbShowSimple.Type,
                VoteCount = tmdbShowSimple.VoteCount,
                OriginalLanguage = tmdbShowSimple.OriginalLanguage,
                //TODO: CAST FELSZEDÉSE
            };

        }

        public async Task<List<InternalSeason>> GetSeasons(TmdbShow tmdbShowSimple, string id)
        {
            List<TmdbSeason> _tmdbSeasons = new List<TmdbSeason>();

            foreach (var season in tmdbShowSimple.Seasons)
            {
                var notSimpleTmdbSeason =
                    await new WebClientManager().Get<TmdbSeason>(
                        $"{_endpoint}/3/tv/{Int32.Parse(id)}/season/{season.SeasonNumber}?api_key={_key}&language={_lang}");
                _tmdbSeasons.Add(notSimpleTmdbSeason);
            }
            return InternalConverter.ConvertTmdbSeasonToInternalSeason(_tmdbSeasons);
        }

        

        
    }
}
