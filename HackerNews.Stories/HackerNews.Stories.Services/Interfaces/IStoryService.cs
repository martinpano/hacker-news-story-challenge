using HackerNews.Stories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.Stories.Services.Interfaces
{
    public interface IStoryService
    {
        Task<IEnumerable<Story>> GetAllStories();
    }
}
