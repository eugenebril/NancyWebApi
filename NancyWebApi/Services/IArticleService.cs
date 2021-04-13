using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NancyWebApi.Models;

namespace NancyWebApi.Services
{
    public interface IArticleService
    {
        /// <summary>
        /// Get enumeration of ArticleViews by section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleView>> GetArticlesBySectionAsync(string section);
        /// <summary>
        /// Get first or default ArticleView by section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<ArticleView> GetFirstArticleBySectionAsync(string section);
        /// <summary>
        /// Get enumeration of ArticleViews by section and date
        /// </summary>
        /// <param name="section"></param>
        /// <param name="updatedDate"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleView>> GetArticlesBySectionAndDateAsync(string section, DateTime updatedDate);
        /// <summary>
        /// Get first or default ArticleView by shortUrl
        /// </summary>
        /// <param name="section"></param>
        /// <param name="shortUrl"></param>
        /// <returns></returns>
        Task<ArticleView> GetArticlesByShortUrlAsync(string section, string shortUrl);
        /// <summary>
        /// Get enumeration of ArticleGroupByDateView by section
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleGroupByDateView>> GetArticleGroupByDateAsync(string section);
    }
}
