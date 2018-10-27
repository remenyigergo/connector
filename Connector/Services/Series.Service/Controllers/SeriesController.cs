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
        public async Task<Result<bool>> ImportSeries([FromBody]ImportRequest request)
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

        [HttpPost("update")]
        public async Task UpdateSeries([FromBody]ImportRequest request)
        {
            await new Series().UpdateSeries(request.Title);
        }

    }
}
