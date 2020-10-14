using Standard.Contracts.Requests;
using System.Threading.Tasks;

namespace Standard.Core.NetworkManager
{
    public interface IWebClientManager
    {
        Task<T> Get<T>(string url);
        Task<int> Post<T>(string url, InternalImportRequest body);
        Task<int> GetUserIdFromUsername(string url);
    }
}
