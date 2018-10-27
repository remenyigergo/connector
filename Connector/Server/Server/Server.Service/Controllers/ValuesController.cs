using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{



    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        SqlConnection conn = new SqlConnection();
        private string connString = "Data Source=DESKTOP-9J0UP14;" +
                                    "Initial Catalog=Connector;" +
                                    "Integrated Security=SSPI;";

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}




        // SAJÁT
        // INSERT A TÁBLÁBA PRÓBA

        [HttpPost("{id}/insertsql")]
        public void InsertSql([FromBody] User user)
        {
            Values insertValue = new Values();

            insertValue.InsertIntoSqL(user);
            
        }

        [HttpGet("users")]
        public async Task<User> GetUsers()
        {
            Values insertValue = new Values();

            return await insertValue.GetUsers();

        }

    }
}
