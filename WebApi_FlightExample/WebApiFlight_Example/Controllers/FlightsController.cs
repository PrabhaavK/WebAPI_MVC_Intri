using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApiFlight_Example.Models;
 
namespace WebApiFlight_Example.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly AppDbContext _context;
 
        public FlightsController(AppDbContext context)
        {
            _context = context;
        }
 
        private bool FlightExists(int id)
        {
            return _context.flights.Any(e => e.FlightId == id);
        }
 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> GetFlights()
        {
            return await _context.flights.ToListAsync();
        }
 
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> GetFlight(int id)
        {
            var flight = await _context.flights.FindAsync(id);
 
            if (flight == null)
            {
                return NotFound();
            }
            return flight;
        }
 
        [HttpPost]
        public async Task<ActionResult<Flight>> PostFlight(Flight flight)
        {
            _context.flights.Add(flight);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFlight), new { id = flight.FlightId }, flight);
        }
 
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteFlight(int id)
        {
            var flight = await _context.flights.FirstOrDefaultAsync(f => f.FlightId == id);
            if (flight == null)
            {
                return NotFound();
            }
 
            _context.flights.Remove(flight);
            await _context.SaveChangesAsync();
            return NoContent();
        }
 
        [HttpPut("{id}")]
        public async Task<ActionResult> PutFlight(int id, Flight flight)
        {
            if (id != flight.FlightId)
            {
                return BadRequest();
            }
 
            _context.Entry(flight).State = EntityState.Modified;
 
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlightExists(id))
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

        
    }
}