using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB.DbModels;
using Standard.Core.DataManager.MongoDB.IRepository;
using Standard.Core.DataManager.MongoDB.Repository;
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