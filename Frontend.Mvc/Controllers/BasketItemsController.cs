using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Frontend.Mvc.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;

namespace Frontend.Mvc.Controllers
{
    public class BasketItemsController : Controller
    {
        IConfiguration _configuration;
        private readonly string _apiGatewayUrl;
        private readonly FrontendMvcContext _context;

        public BasketItemsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiGatewayUrl = _configuration.GetValue<string>("ApiGatewayUrl");
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

        // GET: BasketItems/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // GET: BasketItems/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BasketItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ProductId,Name")] BasketItem basketItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(basketItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(basketItem);
        }

        // GET: BasketItems/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }
            return View(basketItem);
        }

        // POST: BasketItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Name")] BasketItem basketItem)
        {
            if (id != basketItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(basketItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BasketItemExists(basketItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(basketItem);
        }

        // GET: BasketItems/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var basketItem = await _context.BasketItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (basketItem == null)
            {
                return NotFound();
            }

            return View(basketItem);
        }

        // POST: BasketItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var basketItem = await _context.BasketItem.FindAsync(id);
            _context.BasketItem.Remove(basketItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItem.Any(e => e.Id == id);
        }
    }
}
