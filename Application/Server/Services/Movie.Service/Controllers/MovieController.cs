using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Movie.DataManagement.MongoDB.Models;
using Movie.Service.Requests;
using Standard.Contracts;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Movie;
using Standard.Contracts.Requests;
using Standard.Contracts.Requests.Movie;

namespace Movie.Service.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        [HttpPost("import")]
        public async Task<Result<bool>> Import([FromBody] InternalMovie movie)
        {
            try
            {
                if (string.IsNullOrEmpty(movie.Title))
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                await new MovieService().Import(movie);

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

        [HttpPost("exist")]
        public async Task<Result<int>> IsTheShowExist([FromBody] InternalImportRequest request)
        {
            try
            {
                if (request.Title.Length == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<int>()
                    {
                        Data = -1000,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                var existEnumType = await new MovieService().IsMovieExist(request);
                var enumVal = (MediaExistIn)existEnumType;
                return new Result<int>()
                {
                    Data = existEnumType,
                    ResultCode = (int)HttpStatusCode.OK,
                    ResultMessage = "It exists in" + enumVal.ToString()
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int)CoreCodes.MovieNotFound;
                    return new Result<int>()
                    {
                        Data = (int)MediaExistIn.NONE,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<int>()
                {
                    Data = 0,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<int>()
            {
                Data = 0,
                ResultCode = (int)CoreCodes.MovieNotFound,
                ResultMessage = "Movie doesn't exist."
            };
        }

        [HttpPost("update")]
        public async Task<Result<bool>> UpdateStartedMovie([FromBody] InternalStartedMovieUpdateRequest requestModel)
        {
            try
            {
                if (requestModel.Title.Length == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                var result = await new MovieService().UpdateStartedMovie(requestModel);
                
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int)CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int)CoreCodes.AlreadySeen)
                {
                    Response.StatusCode = (int)CoreCodes.AlreadySeen;
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
                ResultMessage = "Update was successful."
            };


        }
    }
}
