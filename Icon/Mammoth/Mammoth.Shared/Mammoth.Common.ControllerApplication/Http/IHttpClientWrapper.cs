using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Common.ControllerApplication.Http
{
    public interface IHttpClientWrapper
    {
        Task<HttpResponseMessage> PostAsJsonAsync<T>(string uri, T value);
        Task<HttpResponseMessage> PutAsJsonAsync<T>(string uri, T value);
        Task<HttpResponseMessage> DeleteAsync(string uri, IEnumerable<int> ids);
    }
}
