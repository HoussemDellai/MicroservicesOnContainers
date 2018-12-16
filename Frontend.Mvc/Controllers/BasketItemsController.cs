using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Frontend.Mvc;
using Frontend.Mvc.Models;

namespace Frontend.Mvc.Controllers
{
    public class BasketItemsController : Controller
    {
        private readonly FrontendMvcContext _context;

        public BasketItemsController(FrontendMvcContext context)
        {
            _context = context;
        }

        // GET: BasketItems
        public async Task<IActionResult> Index()
        {
            return View(await _context.BasketItem.ToListAsync());
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
