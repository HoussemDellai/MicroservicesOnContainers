using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Mvc.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Frontend.Mvc.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiGatewayUrl;


        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiGatewayUrl = _configuration.GetValue<string>("ApiGatewayUrl");
            _apiGatewayUrl = "http://104.45.20.246";
            Console.WriteLine($"_apiGatewayUrl : {_apiGatewayUrl}");
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var client = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Get, _apiGatewayUrl);

            request.Headers.Add("Host", "mvc-client-catalog");
            
            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Product product)
        {
            if (!ModelState.IsValid)
                return View(product);
            
            var client = new HttpClient();

            var productJson = JsonConvert.SerializeObject(product);

            HttpContent content = new StringContent(productJson);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _apiGatewayUrl);

            request.Headers.Add("Host", "mvc-client-catalog");

            request.Content = content;

            var response = await client.SendAsync(request);

            var json = await response.Content.ReadAsStringAsync();
            
            if (!response.IsSuccessStatusCode)
                return View(product);

            return RedirectToAction(nameof(Index));
        }
        
        // GET: Products/AddToBasket/5
        public async Task<IActionResult> AddToBasket(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var catalogApiUrl = _configuration.GetValue<string>("CatalogApiUrl");

            var client = new HttpClient();

            var json = await client.GetStringAsync(catalogApiUrl + "/api/Products/" + id);

            var product = JsonConvert.DeserializeObject<Product>(json);

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> AddToBasket(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
                return View(product);
            
            var client = new HttpClient();

            var basketItem = new BasketItem
            {
                ProductId = product.Id,
                Name = product.Name
            };

            var basketItemJson = JsonConvert.SerializeObject(basketItem);

            HttpContent content = new StringContent(basketItemJson);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, _apiGatewayUrl);

            request.Headers.Add("Host", "mvc-client-basket");

            request.Content = content;

            var response = await client.SendAsync(request);
            
            if (!response.IsSuccessStatusCode)
                return View(product);

            return RedirectToAction(nameof(Index));
        }
    }
}
