using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Standard.Core.DataManager.MongoDB;
using Standard.Core.DataManager.SQL;
using Server.Models;
using Server.SqlDataManager;
using System.Diagnostics;
using Server.DataManagement.SQL.Repositories;
using Standard.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Services
{
    public class Monitor
    {
        private SqlStoreCatalogDataAccessManager SqlCatalogDataAccessManager = new SqlStoreCatalogDataAccessManager();

        //private readonly IParser _monitorParser;
        private readonly MonitorRepository _repo = new MonitorRepository();

        public Monitor()
        {
        }

        public async Task<List<string>> GetAllPrograms()
        {
            return await _repo.GetAllPrograms();
        }

        public async Task InsertProgramFollow(int userId, int programId)
        {
            await _repo.FollowProgramRequest(userId, programId);
        }

        public async Task<int?> CheckProgram(string programName)
        {
            return await  _repo.CheckProgramRequest(programName);
        }

        public async Task<int> InsertProgram(List<string> processes)
        {
            return await _repo.InsertProgram(processes);
        }

        public async Task<bool> CheckInsertedById(int id)
        {
            return await _repo.CheckInsertedById(id);
        }

        public async Task<bool> UpdateFollowedPrograms(int userId, Dictionary<int, int> programsToUpdate)
        {
            return await _repo.UpdateFollowedPrograms(userId,programsToUpdate);
        }

        public async Task<Dictionary<string,int>> RetrieveFollowedProgramsByUser(int userId)
        {
            return await _repo.RetrieveFollowedProgramsByUser(userId);
        }

        public async Task<bool> UpdateFollowedProgramCategory(int userId, int programId, int? categoryId)
        {
            return await _repo.UpdateFollowedProgramCategory(userId, programId, categoryId);
        }
    }
}