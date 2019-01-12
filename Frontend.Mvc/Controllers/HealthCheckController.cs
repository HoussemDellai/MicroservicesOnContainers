using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Mvc.Models;
using Microsoft.Extensions.Configuration;
using System;

namespace Frontend.Mvc.Controllers
{
    public class HealthCheckController : Controller
    {
        private readonly IConfiguration _configuration;
        private string _apiGatewayUrl;

        public HealthCheckController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiGatewayUrl = _configuration.GetValue<string>("ApiGatewayUrl");
            _apiGatewayUrl = "http://104.45.20.246";
        }

        // GET: HealthCheck
        public async Task<IActionResult> Index()
        {
            // Kong doesn't natively support composing API calls.
            var jsonCatalogHealth = await GetHealthCheck("mvc-client-catalog");
            var jsonBasketHealth = await GetHealthCheck("mvc-client-basket");

            var healthChecks = new List<HealthCheckStatus>
            {
                new HealthCheckStatus
                {
                    ServiceName = "Products API",
                    Status =  jsonCatalogHealth
                },
                new HealthCheckStatus
                {
                    ServiceName = "Basket API",
                    Status =  jsonBasketHealth
                }
            };

            return View(healthChecks);
        }

        private async Task<string> GetHealthCheck(string host)
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _apiGatewayUrl);

            request.Headers.Add("Host", host);

            var response = await client.SendAsync(request);

            return await response.Content.ReadAsStringAsync();
        }
    }
}
