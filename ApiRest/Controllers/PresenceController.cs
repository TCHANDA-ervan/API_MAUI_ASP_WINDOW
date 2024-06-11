using ApiRest.Context;
using ApiRest.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PresenceController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;


        public PresenceController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        // GET: api/Presence
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Presence>>> Getpresence()
        {
            if (_authContext.Presences == null)
            {
                return NotFound();
            }
            return await _authContext.Presences.ToListAsync();
        }

        [HttpGet("getpresence"), Authorize]
        public async Task<ActionResult<IEnumerable<Presence>>> Getgroupes()
        {
            if (_authContext.Presences == null)
            {
                return NotFound();
            }
            return await _authContext.Presences.ToListAsync();
        }

        // GET: api/presences/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Presence>> GetPresence(int id)
        {
            if (_authContext.Presences == null)
            {
                return NotFound();
            }
            var presence = await _authContext.Presences.FindAsync(id);

            if (presence == null)
            {
                return NotFound();
            }

            return presence;
        }

        // PUT: api/Presences/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPresence(int id, Presence presence)
        {
            if (id != presence.Id)
            {
                return BadRequest();
            }

            _authContext.Entry(presence).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PresenceExists(id))
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

        // POST: api/Groupes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Groupe>> Postpresence(Presence presence)
        {
            if (_authContext.Presences == null)
            {
                return Problem("Entity set 'ClasseContext.groupe'  is null.");
            }
            _authContext.Presences.Add(presence);
            await _authContext.SaveChangesAsync();

            return CreatedAtAction("Getpresence", new { id = presence.Id }, presence);
        }

        // DELETE: api/presences/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePresence(int id)
        {
            if (_authContext.Presences == null)
            {
                return NotFound();
            }
            var presence = await _authContext.Presences.FindAsync(id);
            if (presence == null)
            {
                return NotFound();
            }

            _authContext.Presences.Remove(presence);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PresenceExists(int id)
        {
            return (_authContext.Presences?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

    
}

