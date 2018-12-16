using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Basket.Api.Models;

namespace Basket.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketItemsController : ControllerBase
    {
        private readonly BasketContext _context;

        public BasketItemsController(BasketContext context)
        {
            _context = context;
        }

        // GET: api/BasketItems
        [HttpGet]
        public IEnumerable<BasketItem> GetBasketItem()
        {
            return _context.BasketItem;
        }

        // GET: api/BasketItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBasketItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basketItem = await _context.BasketItem.FindAsync(id);

            if (basketItem == null)
            {
                return NotFound();
            }

            return Ok(basketItem);
        }

        // PUT: api/BasketItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBasketItem([FromRoute] int id, [FromBody] BasketItem basketItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != basketItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(basketItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BasketItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/BasketItems
        [HttpPost]
        public async Task<IActionResult> PostBasketItem([FromBody] BasketItem basketItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.BasketItem.Add(basketItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBasketItem", new { id = basketItem.Id }, basketItem);
        }

        // DELETE: api/BasketItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBasketItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var basketItem = await _context.BasketItem.FindAsync(id);
            if (basketItem == null)
            {
                return NotFound();
            }

            _context.BasketItem.Remove(basketItem);
            await _context.SaveChangesAsync();

            return Ok(basketItem);
        }

        private bool BasketItemExists(int id)
        {
            return _context.BasketItem.Any(e => e.Id == id);
        }
    }
}