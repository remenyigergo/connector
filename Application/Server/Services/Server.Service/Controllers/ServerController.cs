using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using Server.SqlDataManager;
using Standard.Contracts;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Requests;

namespace Server.Controllers
{

    public class ServerController : ControllerBase
    {

        [HttpPost("insert/program")]
        public async Task<Result<bool>> InsertProcesses([FromBody] List<string> processes)
        {
            try
            {
                if (processes.Count == 0)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "Zero input."
                    };
                }
                await new Monitor().InsertProgram(processes);
            }
            catch (InternalException ex)
            {
                
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
                Data = false,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Insert was successful."
            };

        }



        [HttpGet("get/programs/userid={userId}")]
        public async Task<Result<Dictionary<string, int>>> RetrieveFollowedProgramsByUser(int userId)
        {
            try
            {
                if (userId == 0)
                {
                    return new Result<Dictionary<string,int>>()
                    {
                        Data = null,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var result = await new Monitor().RetrieveFollowedProgramsByUser(userId);
                if (result != null)
                {
                    return new Result<Dictionary<string, int>>()
                    {
                        Data = result,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Retrieve was successful."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.ProcessNotFound)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    return new Result<Dictionary<string, int>>()
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<Dictionary<string, int>>()
                {
                    Data = null,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<Dictionary<string, int>>()
            {
                Data = null,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "Retrieve was successful."
            };

        }

        /// <summary>
        /// Letárolt alkalmazások a "Programs" táblában.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/programs")]
        public async Task<Result<List<string>>> RetrieveAllPrograms()
        {
            try
            {
                var data = await new Monitor().GetAllPrograms();
                return new Result<List<string>>()
                {
                    Data = data,
                    ResultCode = (int)CoreCodes.NoError,
                    ResultMessage = "Programs retrieved successfully."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.ProcessNotFound)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    return new Result<List<string>>()
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<List<string>>()
                {
                    Data = null,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<string>>()
            {
                Data = null,
                ResultCode = (int)CoreCodes.NoError,
                ResultMessage = "No programs were found."
            };

        }

        /// <summary>
        /// Program követésére való kérés indítás. "ProgramsFollowed" tábla
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpPost("follow/{programId}")]
        public async Task<Result<bool>> FollowProgramRequest([FromBody] int userId, int programId)
        {
            try
            {
                if (userId == 0 || programId == 0)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                await new Monitor().InsertProgramFollow(userId, programId);
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.FollowEpisodeError)
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
                ResultMessage = "Program marked as follow."
            };
        }

        /// <summary>
        /// Ha már bent van a programok közt amit paraméterben kap, akkor a letárolt ID-ját adja vissza, különben nullt.
        /// "Program" tábla
        /// </summary>
        /// <param name="programName"></param>
        /// <returns></returns>
        [HttpPost("follow/check")]
        public async Task<Result<int?>> CheckProgramRequest([FromBody]string programName)
        {
            try
            {


                var data = await new Monitor().CheckProgram(programName);
                return new Result<int?>()
                {
                    Data = data,
                    ResultCode = (int)CoreCodes.NoError,
                    ResultMessage = "Process is found."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.ProcessNotFound)
                {
                    Response.StatusCode = (int)HttpStatusCode.NotModified;
                    return new Result<int?>()
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.ErrorMessage
                    };
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return new Result<int?>()
                {
                    Data = null,
                    ResultCode = (int)CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<int?>()
            {
                Data = null,
                ResultCode = (int)CoreCodes.ProcessNotFound,
                ResultMessage = "Program was not found."
            };

        }

        /// <summary>
        /// Ellenőrizzük, hogy a már felvett programok közt benne van-e az adott ID-val renedlkező
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("follow/check/{id}")]
        public async Task<Result<bool>> CheckProgramIsInsertedById(int id)
        {
            try
            {
                if (id == 0)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.MalformedRequest,
                        ResultMessage = "All of the fields must be filled up."
                    };
                }

                var result = await new Monitor().CheckInsertedById(id);
                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Process returned successfully."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.ProcessNotFound)
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
                Data = false,
                ResultCode = (int)CoreCodes.ProcessNotFound,
                ResultMessage = "Process not found."
            };
        }

        [HttpPost("update/followed/{userId}")]
        public async Task<Result<bool>> UpdateProgramsFollowedRequest(int userId, [FromBody] Dictionary<int, int> programs)
        {
            try
            {

                var result = await new Monitor().UpdateFollowedPrograms(userId, programs);
                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Update was successful."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.UpdateFailed)
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
                Data = false,
                ResultCode = (int)CoreCodes.UpdateFailed,
                ResultMessage = "Update failed."
            };

        }


        [HttpPost("update/followed/{userId}/{categoryId}")]
        public async Task<Result<bool>> UpdateProgramsFollowedRequest(int userId, [FromBody] int programId, int? categoryId)
        {
            try
            {
                var result = await new Monitor().UpdateFollowedProgramCategory(userId, programId, categoryId);
                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Update was successful."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.UpdateFailed)
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
                Data = false,
                ResultCode = (int)CoreCodes.UpdateFailed,
                ResultMessage = "Update failed."
            };
        }


        [HttpPost("modules/bookModule/activated")]
        public async Task<Result<bool>> IsBookModuleActivated([FromBody]int userid)
        {
            try
            {
                var result = await new Monitor().IsBookModuleActivated(userid);
                if (result)
                {
                    return new Result<bool>()
                    {
                        Data = false,
                        ResultCode = (int)CoreCodes.NoError,
                        ResultMessage = "Module is activated."
                    };
                }
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int)CoreCodes.ModuleNotActivated)
                {
                    Response.StatusCode = (int)HttpStatusCode.NoContent;
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
                Data = false,
                ResultCode = (int)CoreCodes.ModuleNotActivated,
                ResultMessage = "Module not activated."
            };

        }

        [HttpGet("users/get/{username}")]
        public async Task<Result<int>> GetUserIdFromUsername(string username)
        {
            try
            {
                var result = await new Monitor().GetUserIdFromUsername(username);
                return new Result<int>()
                {
                    Data = result,
                    ResultCode = (int) CoreCodes.NoError,
                    ResultMessage = "Userid was fetched successfully."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.ModuleNotActivated)
                {
                    Response.StatusCode = (int) HttpStatusCode.NoContent;
                    return new Result<int>()
                    {
                        Data = 0,
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
                ResultCode = (int)CoreCodes.ModuleNotActivated,
                ResultMessage = "UserId was not found."
            };

        }


    }
}
