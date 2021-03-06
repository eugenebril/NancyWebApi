using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using NancyWebApi.Models;
using NancyWebApi.Services;
using Newtonsoft.Json;
using NUnit.Framework;
using NyTimesApi.Models;
using NyTimesApi.Services;

namespace NancyWebApiTests
{
    [TestFixture]
    public class ArticleServiceTests
    {
        private Mock<INyTimesService> _clientService;
        private ArticleService _articleService;
        private readonly List<ArticleView> _articleViews;
        private readonly List<Article> _articles;
        private const string DEFAULT_SECTION = "home";

        [SetUp]
        public void Setup()
        {
            _clientService = new Mock<INyTimesService>();
            _articleService = new ArticleService(_clientService.Object);
        }

        public ArticleServiceTests()
        {
            var data = new List<(string, DateTime, string)>
            {
                ("Heading_1", DateTime.Today.AddDays(-1), "https://nyti.ms/short_1"),
                ("Heading_2", DateTime.Today.AddDays(-1), "https://nyti.ms/short_2"),
                ("Heading_3", DateTime.Today, "https://nyti.ms/short_3"),
                ("Heading_4", DateTime.Today.AddDays(-2), "https://nyti.ms/short_4"),
                ("Heading_5", DateTime.Today.AddDays(-2), "https://nyti.ms/short_5"),
                ("Heading_6", DateTime.Today.AddDays(-3), "https://nyti.ms/short_6"),
                ("Heading_7", DateTime.Today.AddDays(-3), "https://nyti.ms/short_7"),
                ("Heading_8", DateTime.Today, "https://nyti.ms/short_8"),
                ("Heading_9", DateTime.Today, "https://nyti.ms/short_9"),
                ("Heading_10", DateTime.Today, "https://nyti.ms/short_10")
            };

            _articles = new List<Article>();
            _articleViews = new List<ArticleView>();

            foreach (var item in data)
            {
                _articles.Add(new Article { Title = item.Item1, UpdatedDate = item.Item2, ShortUrl = item.Item3 });
                _articleViews.Add(new ArticleView { Heading = item.Item1, Updated = item.Item2, Link = item.Item3 });
            }
        }

        [Test]
        public async Task GetArticleGroupByDateAsync_SectionPassed_ReturnArticleGroupByDateViews()
        {
            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);
            var expectedResult = new List<ArticleGroupByDateView>
            {
                new ArticleGroupByDateView {Date = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd"), Total = 2},
                new ArticleGroupByDateView {Date = DateTime.Today.ToString("yyyy-MM-dd"), Total = 4},
                new ArticleGroupByDateView {Date = DateTime.Today.AddDays(-2).ToString("yyyy-MM-dd"), Total = 2},
                new ArticleGroupByDateView {Date = DateTime.Today.AddDays(-3).ToString("yyyy-MM-dd"), Total = 2}
            };

            var result = await _articleService.GetArticleGroupByDateAsync(DEFAULT_SECTION);

            _clientService.Verify(cs => cs.GetArticlesBySectionAsync(DEFAULT_SECTION));
            Assert.AreEqual(JsonConvert.SerializeObject(result), JsonConvert.SerializeObject(expectedResult));
        }

        [Test]
        public async Task GetArticlesBySectionAndDateAsync_SectionAndDatePassed_ReturnArticleViews()
        {
            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);
            var updatedDate = DateTime.Today.AddDays(1);

            var result = await _articleService.GetArticlesBySectionAndDateAsync(DEFAULT_SECTION, updatedDate);


            _clientService.Verify(cs => cs.GetArticlesBySectionAsync(DEFAULT_SECTION));
            Assert.AreEqual(JsonConvert.SerializeObject(result),
                JsonConvert.SerializeObject(_articleViews.Where(x => x.Updated == updatedDate)));
        }


        [Test]
        public async Task GetArticlesBySectionAsync_SectionPassed_ReturnArticleViews()
        {
            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);
            
            var result = await _articleService.GetArticlesBySectionAsync(DEFAULT_SECTION);
            
            _clientService.Verify(cs => cs.GetArticlesBySectionAsync(DEFAULT_SECTION));
            Assert.AreEqual(JsonConvert.SerializeObject(result), JsonConvert.SerializeObject(_articleViews));
        }

        [Test]
        public void GetArticlesByShortUrlAsync_ShortUrlIsNotValid_ThrowException()
        {
            var shortUrl = "short_4_"; 
            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);

            Assert.ThrowsAsync<Exception>(async () => { await _articleService.GetArticlesByShortUrlAsync(DEFAULT_SECTION, shortUrl); });
        }

        [Test]
        public async Task GetArticlesByShortUrlAsync_ShortUrlIsValid_ReturnArticleView()
        {
            var shortUrl = "short_4"; 

            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);

            var result = await _articleService.GetArticlesByShortUrlAsync(DEFAULT_SECTION, shortUrl);

            _clientService.Verify(cs => cs.GetArticlesBySectionAsync(DEFAULT_SECTION));
            Assert.AreEqual(JsonConvert.SerializeObject(result),
                JsonConvert.SerializeObject(_articleViews.FirstOrDefault(x => x.Link.EndsWith(shortUrl))));
        }

        [Test]
        public async Task GetFirstArticleBySectionAsync_SectionPassed_ReturnArticleView()
        {
            _clientService.Setup(x => x.GetArticlesBySectionAsync(DEFAULT_SECTION)).ReturnsAsync(_articles);
            var firstArticleView = _articleViews.FirstOrDefault();

            var result = await _articleService.GetFirstArticleBySectionAsync(DEFAULT_SECTION);

            _clientService.Verify(cs => cs.GetArticlesBySectionAsync(DEFAULT_SECTION));
            Assert.AreEqual(JsonConvert.SerializeObject(result), JsonConvert.SerializeObject(firstArticleView));
        }
    }
}