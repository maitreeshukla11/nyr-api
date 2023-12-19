using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nyr_api.Models;

namespace nyr_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NYRItemsController : ControllerBase
    {
        private readonly NYRContext _context;

        public NYRItemsController(NYRContext context)
        {
            _context = context;
        }

        // GET: api/NYRItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NYRItem>>> GetNYRItems()
        {
            return await _context.NYRItems.ToListAsync();
        }

        // GET: api/NYRItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NYRItem>> GetNYRItem(long id)
        {
            var nYRItem = await _context.NYRItems.FindAsync(id);

            if (nYRItem == null)
            {
                return NotFound();
            }

            return nYRItem;
        }

        // PUT: api/NYRItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNYRItem(long id, NYRItem nYRItem)
        {
            if (id != nYRItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(nYRItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NYRItemExists(id))
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

        // POST: api/NYRItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NYRItem>> PostNYRItem(NYRItem nYRItem)
        {
            _context.NYRItems.Add(nYRItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNYRItem", new { id = nYRItem.Id }, nYRItem);
        }

        // DELETE: api/NYRItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNYRItem(long id)
        {
            var nYRItem = await _context.NYRItems.FindAsync(id);
            if (nYRItem == null)
            {
                return NotFound();
            }

            _context.NYRItems.Remove(nYRItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NYRItemExists(long id)
        {
            return _context.NYRItems.Any(e => e.Id == id);
        }
    }
}
