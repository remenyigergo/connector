using Standard.Core.NetworkManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KeyEventForm.Modules.ApplicationManager.ProcessEqualityComparer
{
    public class RequestManager
    {
        private string _endpoint = "http://localhost:5000";

        /// <summary>
        /// HTTP POST REQUEST - követett programok frissítése
        /// </summary>
        /// <param name="programs"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateRequest(Dictionary<int, int> programs, int userId)
        {
            await new WebClientManager().UpdateProgramsFollowedRequest(_endpoint + "/update/followed/" + userId,
                programs);
        }


       

        /// <summary>
        /// HTTP POST REQUEST - program hozzáadása a serveren lévő programokhoz
        /// </summary>
        /// <param name="programs"></param>
        public async void InsertProgramRequest(List<string> programs)
        {
            await new WebClientManager().InsertPrograms(_endpoint + "/insert/program", programs);
        }


        /// <summary>
        /// HTTP GET REQUEST - serveren lévő programok listájának lekérése
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllPrograms()
        {
            var programs = await new WebClientManager().GetAllPrograms(_endpoint + "/get/programs");
            return programs;
        }

        /// <summary>
        /// HTTP GET REQUEST - követett programok lekérése
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<Dictionary<string, int>> GatherFollowedPrograms(int userId)
        {
            //Web-request to API, to download from DB all the programs that user follows.
            var programs =
                await new WebClientManager().GetFollowedProgramsByUser(_endpoint + "/get/programs/userid=" + userId);
            return programs;
        }
    }
}
