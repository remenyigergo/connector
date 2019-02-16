using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using Server.SqlDataManager;

namespace Server.DataManagement.SQL.Repositories
{
    class MonitorRepository : IMonitorRepository
    {
        private SqlStoreCatalogDataAccessManager SqlCatalogDataAccessManager = new SqlStoreCatalogDataAccessManager();


        public async Task<bool> CheckInsertedById(int id)
        {
            return await SqlCatalogDataAccessManager.CheckInsertedById(id);
        }

        public async Task<int?> CheckProgramRequest(string programName)
        {
            return await SqlCatalogDataAccessManager.CheckProgram(programName);
        }

        public async Task FollowProgramRequest(int userId, int programId)
        {
            await SqlCatalogDataAccessManager.InsertProgramFollow(userId, programId);
        }

        public async Task<int> InsertProgram(List<string> processes)
        {
            return await SqlCatalogDataAccessManager.InsertProgram(processes);
        }

        public async Task<List<string>> GetAllPrograms()
        {
            return await SqlCatalogDataAccessManager.GetAllPrograms();
        }

        public async Task<Dictionary<string, int>> RetrieveFollowedProgramsByUser(int userId)
        {
            return await SqlCatalogDataAccessManager.RetrieveFollowedProgramsByUser(userId);
        }

        public async Task<bool> UpdateFollowedPrograms(int userId, Dictionary<int, int> programsToUpdate)
        {
            if (await SqlCatalogDataAccessManager.UpdateFollowedPrograms(userId, programsToUpdate))
            {
                if (await SqlCatalogDataAccessManager.LastUpdateFollowedPrograms(userId, programsToUpdate) == programsToUpdate.Count)
                    return true;
            }

            return false;
        }

        public async Task<bool> UpdateFollowedProgramCategory(int userId, int programId, int? categoryId)
        {
            return await SqlCatalogDataAccessManager.UpdateFollowedProgramCategory(userId, programId, categoryId);
        }

        public async Task<bool> IsBookModuleActivated(int userid)
        {
            return await SqlCatalogDataAccessManager.IsBookModuleActivated(userid) == 1;
        }

        public async Task<int> GetUserIdFromUsername(string username)
        {
            return await SqlCatalogDataAccessManager.GetUserIdFromUsername(username);
        }

    }
}
