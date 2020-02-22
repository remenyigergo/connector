using System.Collections.Generic;
using System.Threading.Tasks;
using Server.DataManagement.SQL.Repositories;
using Server.SqlDataManager;
using Standard.Contracts.Exceptions;
using Standard.Core.Dependency;
using Microsoft.Extensions.DependencyInjection;

namespace Server.Services
{
    public class Monitor
    {
        //private readonly IParser _monitorParser;
        //private readonly MonitorRepository _repo = new MonitorRepository();

        private readonly IMonitorRepository _monitorRepository = ServiceDependency.Current.GetService<IMonitorRepository>();
        private SqlStoreCatalogDataAccessManager SqlCatalogDataAccessManager = new SqlStoreCatalogDataAccessManager();

        public async Task<List<string>> GetAllPrograms()
        {
            var result = await _monitorRepository.GetAllPrograms();
            if (result.Count == 0)
                throw new InternalException(613, "No programs were found.");
            return result;
        }

        public async Task InsertProgramFollow(int userId, int programId)
        {
            var s = await _monitorRepository.FollowProgramRequest(userId, programId);
            if (s == 0)
                throw new InternalException(612, "No program was marked as follow");
        }

        public async Task<int?> CheckProgram(string programName)
        {
            var response = await _monitorRepository.CheckProgramRequest(programName);
            if (response == null)
                throw new InternalException(613, "No program was found.");

            return response;
        }

        public async Task<int> InsertProgram(List<string> processes)
        {
            var effectedRows = await _monitorRepository.InsertProgram(processes);
            if (effectedRows == 0)
                throw new InternalException(603, "No field/line was modified.");
            return effectedRows;
        }

        public async Task<bool> CheckInsertedById(int id)
        {
            return await _monitorRepository.CheckInsertedById(id);
        }

        public async Task<bool> UpdateFollowedPrograms(int userId, Dictionary<int, int> programsToUpdate)
        {
            var result = await _monitorRepository.UpdateFollowedPrograms(userId, programsToUpdate);
            if (result == false)
                throw new InternalException(607, "Update failed.");

            return result;
        }

        public async Task<Dictionary<string, int>> RetrieveFollowedProgramsByUser(int userId)
        {
            return await _monitorRepository.RetrieveFollowedProgramsByUser(userId);
        }

        public async Task<bool> UpdateFollowedProgramCategory(int userId, int programId, int? categoryId)
        {
            return await _monitorRepository.UpdateFollowedProgramCategory(userId, programId, categoryId);
        }

        public async Task<bool> IsBookModuleActivated(int userid)
        {
            return await _monitorRepository.IsBookModuleActivated(userid);
        }

        public async Task<int> GetUserIdFromUsername(string username)
        {
            return await _monitorRepository.GetUserIdFromUsername(username);
        }
    }
}