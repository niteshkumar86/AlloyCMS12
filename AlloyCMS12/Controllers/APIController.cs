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

            //var result = query.IncludeType<HomePage, BuildOnYourLotHomePage>(x => new HomePage()
            //{
            //    Price = x.Price,
            //    City = x.City,
            //    ACL = x.ACL,
            //    PlanName = x.PlanName
            //}).GetResult();  //throw error and suggest using GetContentResult()
            //.GetContentResult(); is not a method in ISearch but ITypeSearch 

            var result = query.GetContentResult()
                .Select(s=> new { SearchName = s.SearchName, Price = s.Price, ProductName = s.ProductName })
                .ToList();
            return new JsonResult(result);
        }
    }
}
