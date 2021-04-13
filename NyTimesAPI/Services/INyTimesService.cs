using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NyTimesApi.Models;

namespace NyTimesApi.Services
{
    public interface INyTimesService
    {
        /// <summary>
        /// Get articles by section from NyTimes
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<Article>> GetArticlesBySectionAsync(string section);
    }
}
