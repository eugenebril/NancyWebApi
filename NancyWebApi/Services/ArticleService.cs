using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using NancyWebApi.Models;
using NyTimesApi.Services;

namespace NancyWebApi.Services
{
    public class ArticleService : IArticleService
    {
        private readonly INyTimesService _nyTimesService;
        private const int ShortUrlLength = 7;

        public ArticleService(INyTimesService nyTimesService)
        {
            _nyTimesService = nyTimesService ?? throw new ArgumentNullException(nameof(nyTimesService));
        }

        public async Task<IEnumerable<ArticleView>> GetArticlesBySectionAsync(string section)
        {
            var articleViews = await GetArticleViewsAsync(section);
            return articleViews.ToList();
        }

        public async Task<ArticleView> GetFirstArticleBySectionAsync(string section)
        {
            var articleViews = await GetArticleViewsAsync(section);
            var firstArticleView = articleViews.FirstOrDefault();

            return firstArticleView;
        }

        public async Task<IEnumerable<ArticleView>> GetArticlesBySectionAndDateAsync(string section, DateTime updatedDate)
        {
            var articleViews = await GetArticleViewsAsync(section);
            return articleViews.Where(x => x.Updated.Date == updatedDate.Date).ToList();
        }

        public async Task<ArticleView> GetArticlesByShortUrlAsync(string section, string shortUrl)
        {
            ValidateShortUrl(shortUrl);
            var articleViews = await GetArticleViewsAsync(section);
            return articleViews.FirstOrDefault(x => x.Link.EndsWith($"/{shortUrl}"));
        }

        public async Task<IEnumerable<ArticleGroupByDateView>> GetArticleGroupByDateAsync(string section)
        {
            var articles = await _nyTimesService.GetArticlesBySectionAsync(section);
            var groupedArticles = articles.GroupBy(x => x.UpdatedDate.Date)
                .Select(x => new ArticleGroupByDateView { Date = x.Key.ToString("yyyy-MM-dd"), Total = x.Count() });
            return groupedArticles;
        }

        private async Task<IEnumerable<ArticleView>> GetArticleViewsAsync(string section)
        {
            var articles = await _nyTimesService.GetArticlesBySectionAsync(section);

            return articles.Select(x => new ArticleView
            {
                Heading = x.Title,
                Updated = x.UpdatedDate,
                Link = x.ShortUrl
            });
        }

        private void ValidateShortUrl(string shortUrl)
        {
            if (shortUrl.Length != ShortUrlLength)
            {
                throw new Exception("Incorrect short url format");
            }
        }
    }
}
