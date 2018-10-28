using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Contracts.Models.Series;
using Core.NetworkManager;
using Series.Parsers.Trakt.Models;
using Series.Parsers.Trakt.Models.TraktShowModels.SeasonModel;

namespace Series.Parsers.Trakt
{
    public class TraktParser
    {
        //https://api.themoviedb.org/3/search/tv?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US&page=1&query=fargo  ez egy sorozat keresés
        //https://api.themoviedb.org/3/tv/60622?api_key=e9443688992dbb4fa3940ed77a0a8e1d&language=en-US ez mindennel együtt visszaadja de ID kell hozzá
        //https://api.themoviedb.org/3/tv/{tv_id}/season/{season_number}?api_key=<<api_key>>&language=en-US  EZ EGY NEM SIMPLE SEASON


        private const string _endpoint = "https://api.themoviedb.org";
        private const string _lang = "en-US";
        private const string _key = "e9443688992dbb4fa3940ed77a0a8e1d";

        public async Task<InternalSeason> ImportTraktSeries(string title)
        {
            var traktQueryShow = await new WebClientManager().Get<TraktQueryShow>($"{_endpoint}/3/search/tv?api_key={_key}&language={_lang}&page=1&query={title}");
            var traktShowForID = traktQueryShow.Results[0];
            var traktShowSimple = await new WebClientManager().Get<TraktShow>($"{_endpoint}/3/tv/{traktShowForID.Id}?api_key={_key}&language={_lang}");

            List<TraktSeason> Seasons = new List<TraktSeason>();
            for (int i = 1; i <= traktShowSimple.Seasons.Count; i++)
            {
                var notSimpleTraktSeason = await new WebClientManager().Get<TraktSeason>($"{_endpoint}/3/tv/{traktShowForID.Id}/season/{i}?api_key={_key}&language={_lang}");
                Seasons.Add(notSimpleTraktSeason);
            }

            return null;
        }

    }
}
