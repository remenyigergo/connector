using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Contracts.Models.Series;
using Core.NetworkManager;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels;
using Series.Parsers.Trakt.Models.TraktShowModels.ConvertHelper;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Series.Parsers.Trakt
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

            List<TmdbSeason> _tmdbSeasons = new List<TmdbSeason>();
            int i = 1;
            while (tmdbShowSimple.Seasons[i].Name != tmdbShowSimple.Seasons.Last().Name)
            {
                var notSimpleTmdbSeason = await new WebClientManager().Get<TmdbSeason>($"{_endpoint}/3/tv/{tmdbShowForID.Id}/season/{i}?api_key={_key}&language={_lang}");
                _tmdbSeasons.Add(notSimpleTmdbSeason);
                i++;
            }

            var convertedInternalSeasons = InternalConverter.ConvertTmdbSeasonToInternalSeason(_tmdbSeasons);

            return new InternalSeries()
            {
                Id = tmdbShowSimple.Id,
                Runtime = tmdbShowSimple.Episode_run_time.Select(x=>x.Length.ToString()).ToList(),
                Title = tmdbShowSimple.Name,
                Seasons = convertedInternalSeasons,
                Categories = tmdbShowSimple.Genres.Select(x => x.Name).ToList(),
                Description = tmdbShowSimple.Overview,
                Rating = tmdbShowSimple.Vote_average,
                Created_by = tmdbShowSimple.Created_by.Select(x => new InternalCreator() { Name = x.Name }).ToList(),
                Episode_run_time = tmdbShowSimple.Episode_run_time,
                First_air_date = tmdbShowSimple.First_air_date,
                Genres = tmdbShowSimple.Genres.Select(x => new InternalGenre() { Name = x.Name }).ToList(),
                LastEpisodeSimpleToAir = InternalConverter.ConvertTmdbEpisodeToInternal(tmdbShowSimple.Last_Episode_To_Air), //ITT PROBLÉMA VAN, null érték TODO: JAVÍTANI A LASTEPISODETOAIR-t
                Networks = InternalConverter.ConvertTmdbNetworkToInternal(tmdbShowSimple.Networks),
                Popularity = tmdbShowSimple.Popularity,
                Production_companies = InternalConverter.ConvertTmdbProductionCompanyToInternal(tmdbShowSimple.Production_companies),
                Status = tmdbShowSimple.Status,
                Type = tmdbShowSimple.Type,
                Vote_count = tmdbShowSimple.Vote_count,
                Original_language = tmdbShowSimple.Original_language
                //TODO: CAST FELSZEDÉSE
            };

            return null;
        }

        
    }
}
