using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Series.Service.Models;
using Series.Service.Requests;
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                await new Series().ImportSeries(request.Title);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.AlreadyImported)
                {
                    Response.StatusCode = (int) HttpStatusCode.Conflict;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception e)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var markseen = await new Series().MarkAsSeen(int.Parse(request.UserId), request.TvMazeId,
                    request.TmdbId,
                    int.Parse(request.SeasonNumber), int.Parse(request.EpisodeNumber), request.ShowName);
                if (!markseen)
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.AlreadySeen,
                        ResultMessage = "The series is already seen."
                    };
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }
                //TODO: TVMAZE ÉS TMDB CAST & SHOW CREW importálása a későbbiekben
                await new Series().AddSeriesToUser(int.Parse(request.Userid), int.Parse(request.Seriesid));
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.AlreadyAdded)
                {
                    Response.StatusCode = (int) HttpStatusCode.Conflict;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                await new Series().UpdateSeries(request.Title);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.UpToDate)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.UpdateFailed)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
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
        public async Task<Result<InternalSeries>> GetSeries([FromBody] GetSeriesRequest request)
        {
            try
            {
                var series = await new Series().GetSeries(request.Title);
                if (series != null)
                    return new Result<InternalSeries>
                    {
                        Data = series,
                        ResultCode = (int) HttpStatusCode.OK,
                        ResultMessage = "Series found."
                    };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.SeriesNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.SeriesNotFound;
                    return new Result<InternalSeries>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<InternalSeries>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<InternalSeries>
            {
                Data = null,
                ResultCode = (int) CoreCodes.SeriesNotFound,
                ResultMessage = "Error getting show."
            };
        }

        [HttpPost("exist")]
        public async Task<Result<int>> IsShowExist([FromBody] InternalImportRequest request)
        {
            try
            {
                if (request.Title == null)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<int>
                    {
                        Data = 0,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var existEnumType = await new Series().IsShowExist(request);
                var enumVal = (MediaExistIn) existEnumType;
                return new Result<int>
                {
                    Data = existEnumType,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "It exists in" + enumVal.ToString()
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.EpisodeNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.EpisodeNotFound;
                    return new Result<int>
                    {
                        Data = 0,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.SeriesNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.SeriesNotFound;
                    return new Result<int>
                    {
                        Data = 0,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<int>
                {
                    Data = 0,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<int>
            {
                Data = 0,
                ResultCode = (int) CoreCodes.SeriesNotFound,
                ResultMessage = "Show doesn't exist."
            };
        }

        [HttpPost("updateStartedEpisode/{showName}")]
        public async Task<Result<bool>> UpdateStartedEpisode([FromBody] InternalEpisodeStartedModel internalEpisode,
            string showName)
        {
            try
            {
                //internalEpisode.TvMazeId == 0 && internalEpisode.TmdbId == 0 || 
                if (showName.Length == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var update = await new Series().UpdateStartedEpisode(internalEpisode, showName);
                if (update)
                {
                    Response.StatusCode = (int) HttpStatusCode.OK;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.EpisodeStartedUpdated,
                        ResultMessage = "Update was successful."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.UpToDate)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
                if (ex.ErrorCode == (int) CoreCodes.SeriesNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.SeriesNotFound;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.EpisodeStartedNotUpdated,
                ResultMessage = "Update failed."
            };
        }

        [HttpPost("rate/episode")]
        public async Task<Result<bool>> RateEpisode([FromBody] EpisodeRateRequest episodeRate)
        {
            try
            {
                if (episodeRate.TmdbId == null && episodeRate.TvMazeId == null || episodeRate.UserId == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                await new Series().RateEpisode(episodeRate.UserId, episodeRate.TvMazeId, episodeRate.TmdbId,
                    episodeRate.EpisodeNumber, episodeRate.SeasonNumber, episodeRate.Rate);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.EpisodeNotRated)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<bool>
            {
                Data = true,
                ResultCode = (int) CoreCodes.EpisodeRated,
                ResultMessage = "Episode rated successfully."
            };
        }

        [HttpGet("getdays/{days}")]
        public async Task<Result<InternalStartedAndSeenEpisodes>> GetLastDaysEpisodes(int days, int userid)
        {
            try
            {
                if (days == 0 || userid == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<InternalStartedAndSeenEpisodes>
                    {
                        Data = null,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Not a correct input."
                    };
                }
                var res = await new Series().GetLastDays(days, userid);

                return new Result<InternalStartedAndSeenEpisodes>
                {
                    Data = res,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Last days episodes fetched successfully."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.EpisodeNotRated)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<InternalStartedAndSeenEpisodes>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<InternalStartedAndSeenEpisodes>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<InternalStartedAndSeenEpisodes>
            {
                Data = null,
                ResultCode = (int) CoreCodes.EpisodeStartedAndSeenNotFound,
                ResultMessage = "Started and seen episodes couldn't be fetched."
            };
        }

        [HttpPost("recommend")]
        public async Task<Result<List<InternalSeries>>> RecommendSeriesFromDb([FromBody] int userid)
        {
            try
            {
                if (userid == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<List<InternalSeries>>
                    {
                        Data = null,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Input incorrect."
                    };
                }

                var result = await new Series().RecommendSeriesFromDb(userid);
                return new Result<List<InternalSeries>>
                {
                    Data = result,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Recommend is done."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.RecommendFailed)
                {
                    Response.StatusCode = (int) HttpStatusCode.NotModified;
                    return new Result<List<InternalSeries>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<List<InternalSeries>>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<InternalSeries>>
            {
                Data = null,
                ResultCode = (int) CoreCodes.RecommendFailed,
                ResultMessage = "Recommend failed."
            };
        }

        [HttpPost("recommend/genre")]
        public async Task<Result<List<InternalSeries>>> RecommendSeriesFromDb([FromBody] RecommendByGenreRequest model)
        {
            try
            {
                if (model.userid == 0 || model.username.Length == 0 || model.Genres == null || model.Genres.Count == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<List<InternalSeries>>
                    {
                        Data = null,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Input incorrect."
                    };
                }

                var result =
                    await new Series().RecommendSeriesFromDbByGenre(model.Genres, model.username, model.userid);
                return new Result<List<InternalSeries>>
                {
                    Data = result,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Recommended from DB is successful."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.RecommendFailed)
                {
                    Response.StatusCode = (int) CoreCodes.RecommendFailed;
                    return new Result<List<InternalSeries>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.UserNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.UserNotFound;
                    return new Result<List<InternalSeries>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<List<InternalSeries>>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<InternalSeries>>
            {
                Data = null,
                ResultCode = (int) CoreCodes.RecommendFailed,
                ResultMessage = "Recommend failed."
            };
        }

        [HttpPost("check/seen/previous")]
        public async Task<Result<List<InternalEpisode>>> PreviousEpisodeSeen(
            [FromBody] InternalPreviousEpisodeSeenRequest model)
        {
            try
            {
                if (model.userid == 0 || model.title.Length == 0 || model.episodeNum == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<List<InternalEpisode>>
                    {
                        Data = null,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Input incorrect."
                    };
                }


                var result =
                    await new Series().PreviousEpisodeSeen(model.title, model.seasonNum, model.episodeNum,
                        model.userid);
                return new Result<List<InternalEpisode>>
                {
                    Data = result,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Recommend is a success."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.RecommendFailed)
                {
                    Response.StatusCode = (int) CoreCodes.RecommendFailed;
                    return new Result<List<InternalEpisode>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<List<InternalEpisode>>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<InternalEpisode>>
            {
                Data = null,
                ResultCode = (int) CoreCodes.RecommendFailed,
                ResultMessage = "Recommend failed."
            };
        }
    }
}