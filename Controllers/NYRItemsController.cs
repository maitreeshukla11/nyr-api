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

        // GET: api/NYRItems/GetByUsername/{username}
        [HttpGet("GetByUsername/{username}")]
        public async Task<ActionResult<IEnumerable<NYRItem>>> GetNYRItemsByUsername(string username)
        {
            var nYRItems = await _context.NYRItems
                                        .Where(item => item.Username == username.ToLower())
                                        .ToListAsync();

            return nYRItems;
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

                // PUT: api/NYRItems/PutCheckIn
        [HttpPut("PutCheckIn")]
        public async Task<IActionResult> PutCheckIn([FromBody] CheckInUpdateRequest request)
        {
            var nYRItem = await _context.NYRItems.FindAsync(request.Id);

            if (nYRItem == null)
            {
                return NotFound();
            }

            // Calculate index based on Cadence and Date
            int index = CalculateCheckInIndex(nYRItem.Cadence, request.Date);

            // Update the CheckIn array at the calculated index
            if (index >= 0 && index < nYRItem.CheckIn.Length)
            {
                nYRItem.CheckIn[index] = request.Value;
            }
            else
            {
                return BadRequest("Invalid index calculation");
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NYRItemExists(request.Id))
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

        private int CalculateCheckInIndex(string cadence, DateTime date)
        {
            switch (cadence.ToUpper())
            {
                case "DAILY":
                    return (date - new DateTime(date.Year, 1, 1)).Days;
                case "WEEKLY":
                    return (date - new DateTime(date.Year, 1, 1)).Days / 7;
                case "MONTHLY":
                    return (date.Month - 1);
                default:
                    throw new ArgumentException("Invalid Cadence value");
            }
        }

        public class CheckInUpdateRequest
        {
            public long Id { get; set; }
            public DateTime Date { get; set; }
            public bool Value { get; set; }
        }


       // POST: api/NYRItems
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NYRItem>> PostNYRItem(NYRItem nYRItem)
        {
            // Calculate CheckIn array based on Cadence
            nYRItem.CheckIn = CalculateCheckInArray(nYRItem.Cadence);

            nYRItem.Username = nYRItem.Username.ToLower();

            _context.NYRItems.Add(nYRItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNYRItem", new { id = nYRItem.Id }, nYRItem);
        }

        // Helper method to calculate CheckIn array based on Cadence
        private bool[] CalculateCheckInArray(string cadence)
        {
            switch (cadence.ToUpper())
            {
                case "DAILY":
                    return new bool[365];
                case "WEEKLY":
                    return new bool[52];
                case "MONTHLY":
                    return new bool[12];
                default:
                    throw new ArgumentException("Invalid Cadence value");
            }
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
