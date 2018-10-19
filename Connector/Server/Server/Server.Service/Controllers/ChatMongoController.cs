using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Core.DataManager.Mongo.Models;
using Core.DataManager.Mongo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Core.WireProtocol.Messages;

namespace Server.Service.Controllers
{
    [Route("api/[controller]")]    
    public class ChatMongoController : Controller
    {
        private readonly IChatRepository _chatRepository;
        
        public ChatMongoController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        [HttpGet("messages")]
        public void GetMessages()
        {
            _chatRepository.GetAllMessages();
            
        }

        [HttpGet("messages/{id}")]
        public async Task<List<Chat>> GetMessagesByUserId(int id)
        {
            return await _chatRepository.GetAllMessagesByUserId(id);
        }

        [HttpPost("messages")]
        public async Task PostChatMessage([FromBody] Chat msg)
        {
            await _chatRepository.PostMessage(msg);
        }

    }
}