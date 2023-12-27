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

        // GET: api/NYREvents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NYREvent>>> GetNYREvents()
        {
            return await _context.NYREvents.ToListAsync();
        }

        // GET: api/NYREvents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NYREvent>> GetNYREvents(long id)
        {
            var nYRItem = await _context.NYREvents.FindAsync(id);

            if (nYRItem == null)
            {
                return NotFound();
            }

            return nYRItem;
        }

        // GET: api/NYREvents/GetByUsername/{username}
        [HttpGet("GetByUsername/{username}")]
        public async Task<ActionResult<IEnumerable<NYREvent>>> GetNYREventsByUsername(string username)
        {
            var NYREvents = await _context.NYREvents
                                        .Where(item => item.Username == username.ToLower())
                                        .ToListAsync();

            return NYREvents;
        }

        // PUT: api/NYRItems/PutCheckIn
        [HttpPut("PutCheckIn")]
        public async Task<IActionResult> PutCheckIn([FromBody] CheckInUpdateRequest request)
        {
            var NYREvent = await _context.NYREvents.FindAsync(request.Id);

            if (NYREvent == null)
            {
                return NotFound();
            }

            NYREvent.CheckIn = request.Value;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NYREventExists(request.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(NYREvent);
        }

        public class CheckInUpdateRequest
        {
            public long Id { get; set; }
            public bool Value { get; set; }
        }


       // POST: api/NYRItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NYRItem>> PostNYRItem(NYRItem nYRItem)
        {

            // Create NYREvents based on the cadence
            switch (nYRItem.Cadence.ToUpper())
            {
                case "DAILY":
                    CreateDailyEvents(nYRItem);
                    break;
                case "WEEKLY":
                    CreateWeeklyEvents(nYRItem);
                    break;
                case "MONTHLY":
                    CreateMonthlyEvents(nYRItem);
                    break;
                default:
                    throw new ArgumentException("Invalid Cadence value");
            }

            await _context.SaveChangesAsync();
            return CreatedAtAction("GetNYREventsByUsername", new { username = nYRItem.Username }, nYRItem);
        }

        private void CreateDailyEvents(NYRItem nYRItem)
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            for (int i = 0; i < 366; i++)
            {
                var newEvent = new NYREvent
                {
                    Username = nYRItem.Username,
                    GoalText = nYRItem.GoalText,
                    StartDate = startDate.AddDays(i),
                    EndDate = startDate.AddDays(i + 1),
                    CheckIn = false
                };
                _context.NYREvents.Add(newEvent);
            }
        }

        private void CreateWeeklyEvents(NYRItem nYRItem)
        {
            DateTime startDate = new DateTime(2023, 12, 31);
            for (int i = 0; i < 52; i++)
            {
                var newEvent = new NYREvent
                {
                    Username = nYRItem.Username,
                    GoalText = nYRItem.GoalText,
                    StartDate = startDate.AddDays(i * 7),
                    EndDate = startDate.AddDays((i + 1) * 7),
                    CheckIn = false
                };
                _context.NYREvents.Add(newEvent);
            }
        }

        private void CreateMonthlyEvents(NYRItem nYRItem)
        {
            DateTime startDate = new DateTime(2024, 1, 1);
            for (int i = 0; i < 12; i++)
            {
                var lastDayOfMonth = new DateTime(startDate.Year, startDate.Month, DateTime.DaysInMonth(startDate.Year, startDate.Month));
                var newEvent = new NYREvent
                {
                    Username = nYRItem.Username,
                    GoalText = nYRItem.GoalText,
                    StartDate = startDate,
                    EndDate = lastDayOfMonth.AddDays(1),
                    CheckIn = false
                };
                _context.NYREvents.Add(newEvent);

                startDate = startDate.AddMonths(1);
            }
        }


        private bool NYREventExists(long id)
        {
            return _context.NYREvents.Any(e => e.Id == id);
        }
    }
}
