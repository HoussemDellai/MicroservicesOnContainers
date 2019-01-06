using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Frontend.Mvc.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Frontend.Mvc.Controllers
{
    public class BasketItemsController : Controller
    {
        private readonly string _apiGatewayUrl;
        private readonly FrontendMvcContext _context;

        public BasketItemsController(IConfiguration configuration)
        {
            _apiGatewayUrl = configuration.GetValue<string>("ApiGatewayUrl");
            _apiGatewayUrl = "http://104.45.20.246";
            Console.WriteLine($"_apiGatewayUrl : {_apiGatewayUrl}");
            _context = null;
        }

        // GET: BasketItems
        public async Task<IActionResult> Checkout()
        {
            var client = new HttpClient();

            var json = await client.GetStringAsync(_apiGatewayUrl + "/api/BasketItems");

            HttpContent content = new StringContent(json);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(_apiGatewayUrl + "/api/BasketItems/checkout", content);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        // GET: BasketItems
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _apiGatewayUrl);

            request.Headers.Add("Host", "mvc-client-basket");

            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            
            var basketItems = JsonConvert.DeserializeObject<List<BasketItem>>(json);

            return View(basketItems);
        }
    }
}
