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
    public class PromotionController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public PromotionController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        // GET: api/Promotions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Promotion>>> Getpromotion()
        {
            if (_authContext.Promotions == null)
            {
                return NotFound();
            }
            return await _authContext.Promotions.ToListAsync();
        }

        [HttpGet("getpromotion"),Authorize]
        public async Task<ActionResult<IEnumerable<Promotion>>> Getpromotions()
        {
            if (_authContext.Promotions == null)
            {
                return NotFound();
            }
            return await _authContext.Promotions.ToListAsync();
        }

        // GET: api/Promotions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Promotion>> GetPromotion(int id)
        {
            if (_authContext.Promotions == null)
            {
                return NotFound();
            }
            var promotion = await _authContext.Promotions.FindAsync(id);

            if (promotion == null)
            {
                return NotFound();
            }

            return promotion;
        }

        // PUT: api/Promotions/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPromotion(int id, Promotion promotion)
        {
            if (id != promotion.Id)
            {
                return BadRequest();
            }

            _authContext.Entry(promotion).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromotionExists(id))
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

        // POST: api/Promotions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Promotion>> PostPromotion(Promotion promotion)
        {
            if (_authContext.Promotions == null)
            {
                return Problem("Entity set 'ClasseContext.promotion'  is null.");
            }
            _authContext.Promotions.Add(promotion);
            await _authContext.SaveChangesAsync();

            return CreatedAtAction("GetPromotion", new { id = promotion.Id }, promotion);
        }

        // DELETE: api/Promotions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePromotion(int id)
        {
            if (_authContext.Promotions == null)
            {
                return NotFound();
            }
            var promotion = await _authContext.Promotions.FindAsync(id);
            if (promotion == null)
            {
                return NotFound();
            }

            _authContext.Promotions.Remove(promotion);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }

        private bool PromotionExists(int id)
        {
            return (_authContext.Promotions?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

