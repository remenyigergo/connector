using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Social.Services;
using Standard.Contracts;
using Standard.Contracts.Enum;
using Standard.Contracts.Exceptions;
using Standard.Contracts.Models.Social;

namespace Social.Service.Controllers
{
    public class SocialController : ControllerBase
    {
        [HttpGet("messages")]
        public async Task<Result<List<InternalFeed>>> GetFeeds()
        {
            try
            {
                var feeds = await new SocialService().GetAllFeeds();
                return new Result<List<InternalFeed>>
                {
                    Data = feeds,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Feeds fetched successfully."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.FeedsNull)
                    return new Result<List<InternalFeed>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.Message
                    };
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<List<InternalFeed>>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<InternalFeed>>
            {
                Data = null,
                ResultCode = (int) HttpStatusCode.ExpectationFailed,
                ResultMessage = "Feeds not fetched"
            };
        }


        [HttpPost("post")]
        public async Task<Result<bool>> PostMessage([FromBody] InternalFeed msg)
        {
            try
            {
                if (msg == null || msg.PersonName == "" || msg.PostText == "")
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) HttpStatusCode.BadRequest,
                        ResultMessage = "Wrong input."
                    };

                var isItPosted = await new SocialService().Post(msg);
                return new Result<bool>
                {
                    Data = true,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Message posted successfully."
                };
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<bool>
                {
                    Data = false,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error. Probably bad insert."
                };
            }
        }

        [HttpGet("messages/user={userid}")]
        public async Task<Result<List<InternalMessage>>> GetAllMessagesByUserId(int userid) //admin
        {
            try
            {
                if (userid == 0)
                    return new Result<List<InternalMessage>>
                    {
                        Data = null,
                        ResultCode = (int) HttpStatusCode.BadRequest,
                        ResultMessage = "Wrong input"
                    };

                var messages = await new SocialService().GetAllMessagesByUserId(userid);
                return new Result<List<InternalMessage>>
                {
                    Data = messages,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "All messages from user fetched successfully."
                };
            }
            catch (InternalException ex)
            {
                if (ex.ErrorCode == (int) CoreCodes.MessagesNull)
                    return new Result<List<InternalMessage>>
                    {
                        Data = null,
                        ResultCode = ex.ErrorCode,
                        ResultMessage = ex.Message
                    };
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                return new Result<List<InternalMessage>>
                {
                    Data = null,
                    ResultCode = (int) CoreCodes.CommonGenericError,
                    ResultMessage = "Common Generic Error"
                };
            }

            return new Result<List<InternalMessage>>
            {
                Data = null,
                ResultCode = (int) HttpStatusCode.ExpectationFailed,
                ResultMessage = "Messages were not fetched"
            };
        }


        [HttpPost("messages")]
        public async Task<Result<bool>> SendChatMessage([FromBody] InternalMessage msg)
        {
            try
            {
                if (msg.ToId == 0 || msg.FromId == 0)
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) HttpStatusCode.BadRequest,
                        ResultMessage = "Wrong input"
                    };

                var result = await new SocialService().SendMessage(msg);
                return new Result<bool>
                {
                    Data = result,
                    ResultCode = (int) HttpStatusCode.OK,
                    ResultMessage = "Message were sent."
                };
            }
            catch (InternalException ex)
            {
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
                Data = false,
                ResultCode = (int) HttpStatusCode.ExpectationFailed,
                ResultMessage = "Messages were not sent."
            };
        }

        [HttpPost("create/group")]
        public async Task<Result<bool>> CreateGroup([FromBody] List<int> UserIds)
        {
            try
            {
                if (UserIds.Count == 0)
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) HttpStatusCode.BadRequest,
                        ResultMessage = "Wrong input"
                    };

                await new SocialService().CreateGroup(UserIds);
            }
            catch (InternalException ex)
            {
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
                Data = false,
                ResultCode = (int) HttpStatusCode.ExpectationFailed,
                ResultMessage = "Messages were not sent."
            };
        }

        [HttpPost("send/group/{groupId}")]
        public async Task<Result<bool>> SendGroupMessage([FromBody] string message, [FromBody] int userid, int groupId)
        {
            //TODO
            try
            {
                if (groupId == 0)
                    return new Result<bool>
                    {
                        Data = false,
                        ResultCode = (int) HttpStatusCode.BadRequest,
                        ResultMessage = "Wrong input"
                    };

                await new SocialService().SendGroupMessage(groupId, message, DateTime.Now, userid);
            }
            catch (InternalException ex)
            {
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
                Data = false,
                ResultCode = (int) HttpStatusCode.ExpectationFailed,
                ResultMessage = "Messages were not sent."
            };
        }
    }
}