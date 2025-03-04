using System.Net.Http;
using System.Threading.Tasks;

namespace  Dream_Bridge.Services.Adapters
{
    public interface IApiAdapter
    {
        Task<string> GetAsync(string url);
        Task<string> PostAsync(string url, HttpContent content);
    }
}
