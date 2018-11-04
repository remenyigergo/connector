using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Contracts;
using Contracts.Enum;
using Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Series.Service.Models;
using Contracts.Models.Series;

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
                    return new Result<bool>()
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
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<bool>()
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
                if (string.IsNullOrEmpty(request.SeasonNumber) || string.IsNullOrEmpty(request.EpisodeNumber) ||
                    string.IsNullOrEmpty(request.SeriesId) || string.IsNullOrEmpty(request.UserId))
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                await new Series().MarkAsSeen(Int32.Parse(request.UserId), Int32.Parse(request.SeriesId),
                    Int32.Parse(request.SeasonNumber), Int32.Parse(request.EpisodeNumber));
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
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }
                //TODO: TVMAZE ÉS TMDB CAST & SHOW CREW importálása a későbbiekben, ELŐTTE KIJAVÍTANI AZ ÖSSZEFÉSÜLÉST
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

        [HttpPost("started")]
        public async Task<Result<bool>> MarkEpisodeStarted([FromBody] EpisodeStartedModel request)
        {
            try
            {
                await new Series().MarkEpisodeStarted(request);
            }
            catch (InternalException ex)
            {
                return new Result<bool>()
                {
                    Data = false,
                    ResultCode = ex.ErrorCode,
                    ResultMessage = ex.ErrorMessage
                };
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = true,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Successfully marked."
            };

        }

        [HttpPost("update")]
        public async Task UpdateSeries([FromBody] ImportRequest request)
        {
            await new Series().UpdateSeries(request.Title);
        }
    }
}