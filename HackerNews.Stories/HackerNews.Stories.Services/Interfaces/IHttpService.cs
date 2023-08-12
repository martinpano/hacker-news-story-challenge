using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Stories.Services.Interfaces
{
    public interface IHttpService
    {
        Task<TResponse> GetData<TResponse>(string url) where TResponse : class;
    }
}
