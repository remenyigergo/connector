using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.DataManager.Mongo.DbModels;
using Core.DataManager.Mongo.IRepository;
using Core.DataManager.Mongo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Server.Service.Controllers
{
    [Route("api/[controller]")]    
    public class UserMongoController : Controller
    {

        
        private readonly IUserRepository _userRepository;

        public UserMongoController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet("{id}")]
        public async Task<string> Get()
        {
            var a = await _userRepository.Get();
            return a.ToString();
        }


    }
}