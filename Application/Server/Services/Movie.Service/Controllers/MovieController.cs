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
using Standard.Contracts.Models.Movie.ExtendClasses;
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                await new MovieService().Import(movie);
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

        [HttpPost("exist")]
        public async Task<Result<int>> IsTheShowExist([FromBody] InternalImportRequest request)
        {
            try
            {
                if (request.Title.Length == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<int>()
                    {
                        Data = -1000,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                var existEnumType = await new MovieService().IsMovieExist(request);
                var enumVal = (MediaExistIn) existEnumType;
                return new Result<int>()
                {
                    Data = existEnumType,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "It exists in" + enumVal.ToString()
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<int>()
                    {
                        Data = (int) MediaExistIn.NONE,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<int>()
                {
                    Data = 0,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<int>()
            {
                Data = 0,
                ResultCode = (int) CoreCodes.MovieNotFound,
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
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "The title cannot be empty."
                    };
                }

                var result = await new MovieService().UpdateStartedMovie(requestModel);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.AlreadySeen)
                {
                    Response.StatusCode = (int) CoreCodes.AlreadySeen;
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
                ResultMessage = "Update was successful."
            };
        }

        [HttpPost("seen")]
        public async Task<Result<bool>> IsMovieSeen([FromBody] InternalMovieSeenRequest requestModel)
        {
            try
            {
                if (requestModel.UserId == 0 || requestModel.Title == "")
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Wrong input."
                    };
                }

                var result = await new MovieService().IsMovieSeen(requestModel);

                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = true,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Movie is seen."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.AlreadySeen)
                {
                    Response.StatusCode = (int) CoreCodes.AlreadySeen;
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
                Data = false,
                ResultCode = (int) CoreCodes.MovieNotSeen,
                ResultMessage = "Movie is not seen!"
            };
        }

        [HttpPost("started")]
        public async Task<Result<bool>> IsMovieStarted([FromBody] InternalMovieSeenRequest requestModel)
        {
            try
            {
                if (requestModel.UserId == 0 || requestModel.Title == "")
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Wrong input."
                    };
                }

                var result = await new MovieService().IsMovieStarted(requestModel);
                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = true,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Movie is started."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }

                if (ex.ErrorCode == (int) CoreCodes.AlreadyStarted)
                {
                    Response.StatusCode = (int) CoreCodes.AlreadyStarted;

                    return new Result<bool>()
                    {
                        Data = true,
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
                Data = false,
                ResultCode = (int) CoreCodes.MovieNotStarted,
                ResultMessage = "Movie is not started."
            };
        }

        [HttpPost("mark")]
        public async Task<Result<bool>> MarkMovieSeen([FromBody] InternalMovieSeenRequest requestModel)
        {
            try
            {
                if (requestModel.Title == "" || requestModel.UserId == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Bad input requests."
                    };
                }

                await new MovieService().MarkAsSeenMovie(requestModel.Title, requestModel.UserId);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
                ResultMessage = "Movie is marked."
            };
        }

        [HttpPost("rate")]
        public async Task<Result<bool>> RateMovie([FromBody] InternalMovieRateRequest requestModel)
        {
            try
            {
                if (requestModel.UserId == 0 || requestModel.Rating == 0)
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Bad input requests."
                    };
                }

                await new MovieService().RateMovie(requestModel);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }


            return new Result<bool>()
            {
                Data = true,
                ResultCode = (int) CoreCodes.NoError,
                ResultMessage = "Movie rated successfully."
            };
        }

        [HttpGet("get/{title}")]
        public async Task<Result<InternalMovie>> GetMovie(string title)
        {
            try
            {
                if (title == "")
                {
                    Response.StatusCode = (int) HttpStatusCode.BadRequest;
                    return new Result<InternalMovie>
                    {
                        Data = null,
                        ResultCode = (int) CoreCodes.MalformedRequest,
                        ResultMessage = "Empty title in request."
                    };
                }

                var movie = await new MovieService().GetMovie(title);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int) CoreCodes.MovieNotFound;
                    return new Result<InternalMovie>()
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }

            return new Result<InternalMovie>()
            {
                Data = null,
                ResultCode = (int) CoreCodes.CommonGenericError,
                ResultMessage = "Error"
            };
        }

        [HttpPost("recommend")]
        public async Task<Result<List<InternalMovie>>> Recommend([FromBody] RecommendByGenreRequest model)
        {
            try
            {
                if (model.Genres.Count == 0 || model.UserId == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<List<InternalMovie>>
                    {
                        Data = null,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "Empty title in request."
                    };
                }

                await new MovieService().Recommend(model.Genres, model.UserId);
            }
            catch (InternalException ex)
            {
            }


            return new Result<List<InternalMovie>>()
            {
                Data = null,
                ResultCode = (int)CoreCodes.CommonGenericError,
                ResultMessage = "Error"
            };
        }

        [HttpPost("get/days/{days}")]
        public async Task<Result<List<InternalMovie>>> GetLastDays([FromBody] int userid, int days)
        {
            try
            {
                if (userid == 0 || days == 0)
                {
                    Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    return new Result<List<InternalMovie>>
                    {
                        Data = null,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "Input error."
                    };
                }

                await new MovieService().GetLastDays(days, userid);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.MovieNotFound)
                {
                    Response.StatusCode = (int)CoreCodes.MovieNotFound;
                    return new Result<List<InternalMovie>>()
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }

            return new Result<List<InternalMovie>>
            {
                Data = null,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Movies returned. No error."
            };
        }
    }
}