using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Mvc.Models;
using Microsoft.Extensions.Configuration;

namespace Frontend.Mvc.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly IConfiguration _configuration;

        public HealthCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // GET: HealthCheck
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();

            var catalogApiUrl = _configuration.GetValue<string>("CatalogApiUrl");
            var basketApiUrl = _configuration.GetValue<string>("BasketApiUrl");

            var jsonProducts = await client.GetStringAsync(catalogApiUrl + "/healthz");
            var jsonBasket = await client.GetStringAsync(basketApiUrl + "/healthz");
            
            var healthChecks = new List<HealthCheckStatus>
            {
                new HealthCheckStatus { ServiceName = "Products API", Status =  jsonProducts },
                new HealthCheckStatus { ServiceName = "Basket API", Status =  jsonBasket }
            };

            return View(healthChecks);
        }
    }
}
