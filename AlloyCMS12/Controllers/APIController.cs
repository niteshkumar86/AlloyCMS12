using AlloyCMS12.Models.Pages;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework;
using EPiServer.Find.Framework.Statistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AlloyCMS12.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class APIController : ControllerBase
    {
        private readonly IClient _client;
        public APIController(IClient client) {
            _client = client;
        }
        [HttpGet("search")]
        public JsonResult GetProductArticles(int priceMin = 0, int priceMax = 0, string name = "")
        {
            var query =
            _client.Search<ProductPage>()
            .CurrentlyPublished()
            .ExcludeDeleted()            
            .Filter(x => !x.StopPublish.Exists());


            // Determine the ordering function based on sortBy
            //query = input.SortBy?.ToLower() switch
            //{
            //    "price" => input.SortOrder?.ToLower() switch
            //    {
            //        "desc" => query.OrderByDescending(o => o.Price),
            //        _ => query.OrderBy(o => o.Price, EPiServer.Find.Api.SortMissing.Last),
            //    },
            //    "name" => input.SortOrder?.ToLower() switch
            //    {
            //        "desc" => query.OrderByDescending(o => o.PlanName),
            //        _ => query.OrderBy(o => o.PlanName)
            //    },
            //    "sqft" => input.SortOrder?.ToLower() switch
            //    {
            //        "desc" => query.OrderByDescending(o => o.SquareFeet),
            //        _ => query.OrderBy(o => o.SquareFeet, EPiServer.Find.Api.SortMissing.Last),
            //    },
            //    "moveindate" => input.SortOrder?.ToLower() switch
            //    {
            //        "desc" => query.OrderByDescending(o => o.MoveInDate),
            //        _ => query.OrderBy(o => o.MoveInDate),
            //    },
            //    _ => query
            //};
            
            if (priceMin != 0 && priceMax != 0)
            {
                query = query.Filter(x => x.Price.InRange(priceMin, priceMax));
            }
            
            if (name != "")
            {
                query = query.Filter(x => x.SearchName.Match(name));
            }
            

            query = query.Skip(0)
            .Take(100)
            .Track();

            //var multiQueryResult = query.IncludeType<ProductPage, ArticlePage>(x => new ProductPage()
            //{
            //    Price = x.Price,                
            //    SearchName = x.SearchName,
            //    ProductName = x.ArticleName
            //}).GetResult();  
            //EPiServer.Core.EPiServerException: 'Please use GetContentResult extension method for getting results with content types'
            //.GetContentResult(); is not a method in ISearch but ITypeSearch


            var multiQuery = query
                .Select(s => new SearchResultModel { SearchName = s.SearchName, Price = s.Price, ProductName = s.ProductName, IsProductPage = true })
                .IncludeType<SearchResultModel, ArticlePage>(x => new SearchResultModel()
            {
                Price = x.Price,
                SearchName = x.SearchName,
                ProductName = x.ArticleName,
                IsArticlePage = true
            }).IncludeType<SearchResultModel, NewsPage>(x => new SearchResultModel()
            {
                Price = x.JustNumericValue, //do not filter by price value
                SearchName = x.Name,
                ProductName = x.Name
            });

            var multiQueryResult = multiQuery.GetResult().ToList();
            return new JsonResult(multiQueryResult);
            //Unable to cast object of type 'AlloyCMS12.Models.Pages.ProductPage' to type 'EPiServer.Find.Cms.ContentInLanguageReference'.




            //var result = query.GetContentResult()
            //    .Select(s=> new SearchResultModel { SearchName = s.SearchName, Price = s.Price, ProductName = s.ProductName })
            //    .ToList();
            //return new JsonResult(result);
        }
    }

    public class SearchResultModel
    {
        public string SearchName { get; set; }
        public int Price { get; set; }
        public string ProductName { get; set; }

        public bool IsProductPage { get; set; }
        public bool IsArticlePage { get; set;}
    }
}
