using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Server.DataManagement.SQL.Repositories
{
    public interface IMonitorRepository
    {

        Task<int> InsertProgram(List<string> processes);

        Task<Dictionary<string,int>> RetrieveFollowedProgramsByUser(int userId);

        Task<List<string>> GetAllPrograms();

        Task FollowProgramRequest(int userId, int programId);

        Task<int?> CheckProgramRequest(string programName);

        Task<bool> CheckInsertedById(int id);

    }
}
