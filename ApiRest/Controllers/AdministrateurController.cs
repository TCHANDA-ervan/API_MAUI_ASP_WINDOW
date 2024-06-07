using ApiRest.Context;
using ApiRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrateurController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public AdministrateurController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        // GET: api/Administrateur/5
        [HttpGet("{id}"),Authorize]
        public async Task<ActionResult<Adminitrateur>> GetAdministrateur(int id)
        {
            if (_authContext.Adminitrateurs == null)
            {
                return NotFound("Entity set 'AppDbContext.Administrateurs' is null.");
            }

            var administrateur = await _authContext.Adminitrateurs.FindAsync(id);

            if (administrateur == null)
            {
                return NotFound();
            }

            return administrateur;
        }

        // POST: api/Administrateur
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Adminitrateur>> PostAdministrateur(Adminitrateur administrateur)
        {
            if (_authContext.Adminitrateurs == null)
            {
                return Problem("Entity set 'AppDbContext.Administrateurs' is null.");
            }

            _authContext.Adminitrateurs.Add(administrateur);
            await _authContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAdministrateur), new { id = administrateur.Id }, administrateur);
        }

         // GET: api/Promotions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Adminitrateur>>> GetAdministrateur()
        {
            if (_authContext.Adminitrateurs == null)
            {
                return NotFound();
            }
            return await _authContext.Adminitrateurs.ToListAsync();
        }
    }
}
