using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nancy;
using Nancy.Responses;
using NancyWebApi.Models;
using NancyWebApi.Services;
using Newtonsoft.Json;

namespace NancyWebApi.Modules
{
    public class MainModule : NancyModule
    {
        private readonly IArticleService _articleService;
        private const string DefaultSection = "home";

        public MainModule(IArticleService articleService)
        {
            _articleService = articleService ?? throw new ArgumentNullException(nameof(articleService));

            Get("/", _ => HelthCheckResponse());

            Get("/list/{section}", async args =>
            {
                try
                {
                    IEnumerable<ArticleView> articleViews = await _articleService.GetArticlesBySectionAsync(args.section) ?? Enumerable.Empty<ArticleView>();

                    return Response.AsJson(articleViews);
                }
                catch (Exception e)
                {
                    return ErrorResponse(e.Message); 
                }
            });

            Get("/list/{section}/first", async args =>
            {
                try
                {
                    ArticleView firstArticleView = await _articleService.GetFirstArticleBySectionAsync(args.section);
                    return (firstArticleView == null) ? NotFoundResponse() : Response.AsJson(firstArticleView);
                }
                catch (Exception e)
                {
                    return ErrorResponse(e.Message);
                }
            });

            Get("/list/{section}/{updatedDate}", async args =>
            {
                try
                {
                    if (!DateTime.TryParse((string)args.updatedDate, out var updateDate))
                    {
                        return ErrorResponse($"Incorrect input parameter {nameof(args.updatedDate)}");
                    }

                    IEnumerable<ArticleView> articleViews = await _articleService.GetArticlesBySectionAndDateAsync(args.section, updateDate);
                    return Response.AsJson(articleViews);
                }
                catch (Exception e)
                {
                    return ErrorResponse(e.Message);
                }
            });

            Get("/article/{shortUrl}", async args =>
            {
                try
                {
                    
                    ArticleView articleView = await _articleService.GetArticlesByShortUrlAsync(DefaultSection, args.shortUrl);
                    return (articleView == null) ? NotFoundResponse() : Response.AsJson(articleView);
                }
                catch (Exception e)
                {
                    return ErrorResponse(e.Message);
                }
            });

            Get("/group/{section}", async args =>
            {
                try
                {
                    IEnumerable<ArticleGroupByDateView> articleGroupByDateViews = await _articleService.GetArticleGroupByDateAsync(args.section);
                    return Response.AsJson(articleGroupByDateViews);
                }
                catch (Exception e)
                {
                    return ErrorResponse(e.Message);
                }
            });
        }

        private Response NotFoundResponse()
        {
            return Response.AsJson($"{HttpStatusCode.NotFound}", HttpStatusCode.NotFound);
        }

        private Response ErrorResponse(string errorMessage)
        {
            return Response.AsJson(new { error = errorMessage }, HttpStatusCode.InternalServerError);
        }

        private Response HelthCheckResponse()
        {
            return Response.AsJson(new { result = true });
        }
    }
}
