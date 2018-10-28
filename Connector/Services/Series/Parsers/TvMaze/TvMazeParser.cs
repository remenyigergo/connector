using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts.Models.Series;
using Core.NetworkManager;
using Series.Parsers.TvMaze.Models;

namespace Series.Parsers.TvMaze
{
    public class TvMazeParser : IParser
    {
        private const string _endpoint = "http://api.tvmaze.com";

        //LESZEDJÜK AZ APIN KERESZTÜL ÉS ÁTALAKÍTJUK SAJÁT SOROZAT FORMÁTUMRA
        public async Task<Contracts.Models.Series.InternalSeries> ImportSeriesFromTvMaze(string title)
        {
            var tvMazeSeries = await new WebClientManager().Get<List<TvMazeSeries>>($"{_endpoint}/search/shows?q={title}");
            var firstSeries = tvMazeSeries[0];
            var seasons = await ImportSeasons(firstSeries.Show.Name, Int32.Parse(firstSeries.Show.Id));    //ÖSSZES SZEZON
            
            //A végleges belső MongoSeries modelje
            if (firstSeries != null && seasons !=null)
            {
                return new InternalSeries()
                {                    
                    SeriesId = firstSeries.Show.Id,
                    Runtime = firstSeries.Show.Runtime,
                    Title = firstSeries.Show.Name,
                    Seasons = seasons,
                    Categories = firstSeries.Show.Genres,
                    Description = firstSeries.Show.Summary,
                    Rating = firstSeries.Show.Rating.Average,
                    LastUpdated = firstSeries.Show.Updated
                };
            }

            return null;
        }

        public async Task<List<InternalEpisode>> ImportEpisodes(int id)
        {
            var tvMazeEpisodes = await new WebClientManager().Get<List<TvMazeEpisode>>($"{_endpoint}/shows/{id}/episodes");

            var episodeList = new List<InternalEpisode>();

            if (tvMazeEpisodes != null)
            {
                foreach (var episode in tvMazeEpisodes)
                {
                    episodeList.Add(new InternalEpisode()
                    {
                        Title = episode.Name,
                        //Rating = episode.Rating,
                        Description = episode.Summary,
                        Length = episode.Runtime,
                        Episode = int.Parse(episode.Number),
                        Season = int.Parse(episode.Season)
                    });
                }
                return episodeList;
            }

            return null;
        }


        public async Task<List<InternalSeason>> ImportSeasons(string title, int id)
        {
            var tvMazeSeasons = await new WebClientManager().Get<List<TvMazeSeason>>($"{_endpoint}/shows/{id}/seasons");
            var seasons = new Dictionary<int, InternalSeason>();

            if (tvMazeSeasons != null)
            {
                foreach (var season in tvMazeSeasons)
                {
                    seasons.Add(season.SeasonNumber, new InternalSeason()
                    {
                        Id = season.Id,
                        Summary = season.Summary,
                        EpisodesCount = season.EpisodesCount ?? 0,
                        SeasonNumber = season.SeasonNumber,
                        Episodes = new List<InternalEpisode>()
                    });
                }

                await FillEpisodes(id, seasons);                
                return seasons.Values.ToList();
            }

            return null;
        }

        private async Task FillEpisodes(int id, Dictionary<int, InternalSeason> seasons)
        {
            var episodeList = await ImportEpisodes(id);
            foreach (InternalEpisode episode in episodeList)
            {
                seasons[episode.Season].Episodes.Add(episode);
            }
        }

        
//        private static void SortSeasonsAndEpisodes(ref Dictionary<int, InternalSeason> seasonList)
//        {
//            seasonList.Values = seasonList.Values.OrderBy(s => s.SeasonNumber).ToList();
//            foreach (var internalSeason in seasonList)
//            {
//                internalSeason.Episodes = internalSeason.Episodes.OrderBy(e => e.Episode).ToList();
//            }
//        }
    }
}