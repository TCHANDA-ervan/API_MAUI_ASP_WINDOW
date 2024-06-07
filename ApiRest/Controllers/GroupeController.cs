using ApiRest.Context;
using ApiRest.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupeController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public GroupeController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        // GET: api/Groupes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Groupe>>> Getgroupe()
        {
            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            return await _authContext.Groupes.ToListAsync();
        }

        [HttpGet("getgroupe"),Authorize]
        public async Task<ActionResult<IEnumerable<Groupe>>> Getgroupes()
        {
            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            return await _authContext.Groupes.ToListAsync();
        }

        // GET: api/Groupes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Groupe>> GetGroupe(int id)
        {
            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            var groupe = await _authContext.Groupes.FindAsync(id);

            if (groupe == null)
            {
                return NotFound();
            }

            return groupe;
        }

        // PUT: api/Groupes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupe(int id, Groupe groupe)
        {
            if (id != groupe.Id)
            {
                return BadRequest();
            }

            _authContext.Entry(groupe).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GroupeExists(id))
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
        public async Task<ActionResult<Groupe>> PostGroupe(Groupe groupe)
        {
            if (_authContext.Groupes == null)
            {
                return Problem("Entity set 'ClasseContext.groupe'  is null.");
            }
            _authContext.Groupes.Add(groupe);
            await _authContext.SaveChangesAsync();

            return CreatedAtAction("GetGroupe", new { id = groupe.Id }, groupe);
        }

        // DELETE: api/Groupes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupe(int id)
        {
            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            var groupe = await _authContext.Groupes.FindAsync(id);
            if (groupe == null)
            {
                return NotFound();
            }

            _authContext.Groupes.Remove(groupe);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }

        private bool GroupeExists(int id)
        {
            return (_authContext.Groupes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

