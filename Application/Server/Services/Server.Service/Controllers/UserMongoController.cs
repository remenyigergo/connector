using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Standard.Core.DataManager.MongoDB.IRepository;

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