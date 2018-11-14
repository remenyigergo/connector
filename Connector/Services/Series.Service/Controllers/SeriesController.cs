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
using Standard.Contracts.Requests;

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
                //TODO: TVMAZE ÉS TMDB CAST & SHOW CREW importálása a későbbiekben
                await new Series().AddSeriesToUser(Int32.Parse(request.Userid), Int32.Parse(request.Seriesid));
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.AlreadyAdded)
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

            catch (Exception ex)
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
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>()
                {
                    Data = true,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
                ResultMessage = "Successfully marked."
            };
        }

        [HttpPost("update")]
        public async Task UpdateSeries([FromBody] ImportRequest request)
        {
            await new Series().UpdateSeries(request.Title);
        }

        [HttpPost("getshow")]
        public async Task<bool> GetShow([FromBody] GetShowRequest request)
        {
            return await new Series().GetShow(request.Episode, request.Title);
        }

        [HttpPost("getseries")]
        public async Task<InternalSeries> GetSeries([FromBody] GetSeriesRequest request)
        {
            return await new Series().GetSeries(request.Title);
        }

        [HttpPost("exist")]
        public async Task<int>
            IsShowExist(
                [FromBody] InternalImportRequest request) //DB : 1, TVMAZE: 2, TMDB: 3, Egyik sem: -1, Request hiba: -2
        {
            if (request != null)
            {
                if (await new Series().IsShowExistInMongoDb(request.Title))
                {
                    return 1;
                }
                request.Title = RemoveAccent(request.Title);
                var tvmazexist = await new Series().IsShowExistInTvMaze(request.Title);
                var tmdbexist = await new Series().IsShowExistInTmdb(request.Title);
                if (tvmazexist)
                {
                    if (tmdbexist)
                    {
                        return 3;
                    }
                    return 2;
                }
                return -1;
            }
            return -2;
        }

        public string RemoveAccent(string text)
        {
            var decomposed = text.Normalize(NormalizationForm.FormD);

            char[] filtered = decomposed
                .Where(c => char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            return new String(filtered);
        }
    }
}