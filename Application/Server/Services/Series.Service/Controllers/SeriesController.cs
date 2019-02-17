using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Series.Service.Models;
using Series.Service.Requests;
using Series.Service.Response;
using Standard.Contracts;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Series;
using Standard.Contracts.Models.Series.ExtendClasses;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Series;

namespace Series.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SeriesController : ControllerBase
    {
        [HttpPost("import")]
        public async Task<Result<bool>> ImportSeries([FromBody] ImportRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Title))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                await new Series().ImportSeries(request.Title);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.AlreadyImported)
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Successfully imported."
            };
        }

        [HttpPost("mark")]
        public async Task<Result<bool>> MarkAsSeen([FromBody] MarkRequest request)
        {
            try
            {
                //ebben a vizsgálatban még a többi paramétert is le kell checkolni, userid, stb
                if (string.IsNullOrEmpty(request.SeasonNumber) || string.IsNullOrEmpty(request.EpisodeNumber))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var markseen = await new Series().MarkAsSeen(Int32.Parse(request.UserId), request.TvMazeId, request.TmdbId,
                    Int32.Parse(request.SeasonNumber), Int32.Parse(request.EpisodeNumber), request.ShowName);
                if (!markseen)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.AlreadySeen,
                        ResultMessage = "The series is already seen."
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Successfully imported."
            };
        }

        [HttpPost("add")]
        public async Task<Result<bool>> AddSeries([FromBody] AddedSeriesRequest request)
        {
            try
            {
                if (request.Seriesid == null || request.Userid == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }
                //TODO: TVMAZE ÉS TMDB CAST & SHOW CREW importálása a későbbiekben
                await new Series().AddSeriesToUser(Int32.Parse(request.Userid), Int32.Parse(request.Seriesid));
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.AlreadyAdded)
                {
                    Response.StatusCode = (int)HttpStatusCode.Conflict;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Successfully imported."
            };
        }

        //[HttpPost("started")]
        //public async Task<Result<bool>> MarkEpisodeStarted([FromBody] InternalEpisodeStartedModel request)
        //{
        //    try
        //    {
        //        await new Series().MarkEpisodeStarted(request);
        //    }
        //    catch (InternalException ex)
        //    {
        //        return new Result<bool>()
        //        {
        //            Data = false,
        //            ResultCode = ex.ErrorCode,
        //            ResultMessage = ex.ErrorMessage
        //        };
        //    }

        //    catch (Exception ex)
        //    {
        //        Response.StatusCode = (int) HttpStatusCode.InternalServerError;
        //        return new Result<bool>()
        //        {
        //            Data = true,
        //            ResultCode = (int) CoreCodes.CommonGenericError,
        //            ResultMessage = "Common Generic Error"
        //        };
        //    }


        //    return new Result<bool>()
        //    {
        //        Data = true,
        //        ResultCode = (int) CoreCodes.NoError,
        //        ResultMessage = "Successfully marked."
        //    };
        //}

        [HttpPost("update")]
        public async Task<Result<bool>> UpdateSeries([FromBody] ImportRequest request)
        {
            try
            {
                if (request.Title == null)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                await new Series().UpdateSeries(request.Title);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.UpToDate)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int)CoreCodes.SeriesNotUpdated)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Successfully updated."
            };

        }

        //[HttpPost("getshow")]
        //public async Task<Result<bool>> GetShow([FromBody] GetShowRequest request)
        //{
        //    try
        //    {

        //        await new Series().GetShow(request.Episode, request.Title);
        //    }
        //    catch ()
        //    {
                
        //    }

            
        //}

        [HttpPost("getseries")]
        public async Task<InternalSeries> GetSeries([FromBody] GetSeriesRequest request)
        {
            return await new Series().GetSeries(request.Title);
        }

        [HttpPost("exist")]
        public async Task<int> IsShowExist([FromBody] InternalImportRequest request) //DB : 1, TVMAZE: 2, TMDB: 3, Egyik sem: -1, Request hiba: -2
        {
            return await new Series().IsShowExist(request);
        }

        [HttpPost("updateStartedEpisode/{showName}")]
        public async Task<bool> UpdateStartedEpisode([FromBody]InternalEpisodeStartedModel internalEpisode, string showName)
        {
            return await new Series().UpdateStartedEpisode(internalEpisode, showName);
        }

        [HttpPost("rate/episode")]
        public async Task RateEpisode([FromBody] EpisodeRateRequest episodeRate)
        {
            await new Series().RateEpisode(episodeRate.UserId, episodeRate.TvMazeId, episodeRate.TmdbId, episodeRate.EpisodeNumber, episodeRate.SeasonNumber, episodeRate.Rate);
        }

        [HttpGet("getdays/{days}")]
        public async Task<List<InternalStartedAndSeenEpisodes>> GetLastDaysEpisodes(int days, int userid)
        {
            return await new Series().GetLastDays(days, userid);
        }

        [HttpPost("recommend")]
        public async Task<List<InternalSeries>> RecommendSeriesFromDb([FromBody]int userid)
        {
            return await new Series().RecommendSeriesFromDb(userid);
        }

        [HttpPost("recommend/genre")]
        public async Task<List<InternalSeries>> RecommendSeriesFromDb([FromBody]RecommendByGenreRequest model)
        {
            return await new Series().RecommendSeriesFromDbByGenre(model.Genres, model.username, model.userid);
        }

        [HttpPost("check/seen/previous")]
        public async Task<List<InternalEpisode>> PreviousEpisodeSeen([FromBody] InternalPreviousEpisodeSeenRequest model)
        {
            return await new Series().PreviousEpisodeSeen(model.title, model.seasonNum, model.episodeNum, model.userid);
        }
    }
}