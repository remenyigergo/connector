using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;
using Server.SqlDataManager;
using Standard.Contracts.Requests;

namespace Server.Controllers
{

    public class ServerController : ControllerBase
    {
        
        [HttpPost("insert/program")]
        public async Task InsertProcesses([FromBody] List<string> processes)
        {
            await new Monitor().InsertProgram(processes);
        }



        [HttpGet("get/programs/userid={userId}")]
        public async Task<Dictionary<string,int>> RetrieveFollowedProgramsByUser(int userId)
        {
            return await new Monitor().RetrieveFollowedProgramsByUser(userId);
        }

        /// <summary>
        /// Letárolt alkalmazások a "Programs" táblában.
        /// </summary>
        /// <returns></returns>
        [HttpGet("get/programs")]
        public async Task<List<string>> RetrieveAllPrograms()
        {
            return await new Monitor().GetAllPrograms();
        }

        /// <summary>
        /// Program követésére való kérés indítás. "ProgramsFollowed" tábla
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        [HttpPost("follow/{programId}")]
        public async Task FollowProgramRequest([FromBody] int userId, int programId)
        {
            await new Monitor().InsertProgramFollow(userId, programId);
        }

        /// <summary>
        /// Ha már bent van a programok közt amit paraméterben kap, akkor a letárolt ID-ját adja vissza, különben nullt.
        /// "Program" tábla
        /// </summary>
        /// <param name="programName"></param>
        /// <returns></returns>
        [HttpPost("follow/check")]
        public async Task<int?> CheckProgramRequest([FromBody]string programName)
        {
            return await new Monitor().CheckProgram(programName);
        }

        /// <summary>
        /// Ellenőrizzük, hogy a már felvett programok közt benne van-e az adott ID-val renedlkező
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("follow/check/{id}")]
        public async Task<bool> CheckProgramIsInsertedById(int id)
        {
            return await new Monitor().CheckInsertedById(id);
        }

        [HttpPost("update/followed/{userId}")]
        public async Task<bool> UpdateProgramsFollowedRequest(int userId, [FromBody] Dictionary<int, int> programs)
        {
            return await new Monitor().UpdateFollowedPrograms(userId, programs);
        }


        [HttpPost("update/followed/{userId}/{categoryId}")]
        public async Task<bool> UpdateProgramsFollowedRequest(int userId, int programId, int? categoryId)
        {
            return await new Monitor().UpdateFollowedProgramCategory(userId, programId, categoryId);
        }


        [HttpPost("modules/bookModule/activated")]
        public async Task<bool> IsBookModuleActivated([FromBody]int userid)
        {
            return await new Monitor().IsBookModuleActivated(userid);
        }

    }
}
