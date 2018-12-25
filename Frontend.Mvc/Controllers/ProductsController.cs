using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Frontend.Mvc;
using Frontend.Mvc.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace Frontend.Mvc.Controllers
{
    public class ProductsController : Controller
    {
        private IConfiguration _configuration;
        private readonly FrontendMvcContext _context;

        public ProductsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _context = null;
        }
        //public ProductsController(FrontendMvcContext context)
        //{
        //    _context = context;
        //}

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var catalogApiUrl = _configuration.GetValue<string>("CatalogApiUrl");

            var client = new HttpClient();

            var json = await client.GetStringAsync(catalogApiUrl + "/api/Products");

            var products = JsonConvert.DeserializeObject<List<Product>>(json);

            return View(products);
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Price")] Product product)
        {
            if (!ModelState.IsValid)
                return View(product);

            var catalogApiUrl = _configuration.GetValue<string>("CatalogApiUrl");

            var client = new HttpClient();

            var productJson = JsonConvert.SerializeObject(product);

            HttpContent content = new StringContent(productJson);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(catalogApiUrl + "/api/Products", content);

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

            var catalogApiUrl = _configuration.GetValue<string>("BasketApiUrl");

            var client = new HttpClient();

            var basketItem = new BasketItem
            {
                ProductId = product.Id,
                Name = product.Name
            };

            var basketItemJson = JsonConvert.SerializeObject(basketItem);

            HttpContent content = new StringContent(basketItemJson);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var response = await client.PostAsync(catalogApiUrl + "/api/BasketItems", content);

            if (!response.IsSuccessStatusCode)
                return View(product);

            return RedirectToAction(nameof(Index));
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
