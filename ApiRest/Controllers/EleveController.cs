using ApiRest.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Text;
using ApiRest.Models;
using Microsoft.EntityFrameworkCore;
using ApiRest.Helpers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EleveController : ControllerBase
    {
        private readonly AppDbContext _authContext;
        private readonly IConfiguration _configuration;

        public EleveController(AppDbContext context, IConfiguration configuration)
        {
            _authContext = context;
            _configuration = configuration;
        }

        [HttpPost("authenticate"),Authorize]
        public async Task<IActionResult> Authenticate([FromBody] Eleve userObj)
        {
            if (userObj == null)
                return BadRequest();

            var eleve = await _authContext.Eleves
                .FirstOrDefaultAsync(x => x.Email == userObj.Email);

            if (eleve == null)
                return NotFound(new { Message = "User not found!" });

            if (!BCrypt.Net.BCrypt.Verify(userObj.Password, eleve.Password))
            {
                return BadRequest(new { Message = "Password is Incorrect" });
            }

            // Uncomment and adjust if you want to return tokens
            // eleve.Token = CreateJwt(eleve);
            // var newAccessToken = eleve.Token;
            // var newRefreshToken = CreateRefreshToken();
            // eleve.RefreshToken = newRefreshToken;
            // eleve.RefreshTokenExpiryTime = DateTime.Now.AddDays(5);
            // await _authContext.SaveChangesAsync();

            // return Ok(new TokenApiDto()
            // {
            //     AccessToken = newAccessToken,
            //     RefreshToken = newRefreshToken
            // });

            return Ok(new
            {
                Message = "Login Success!"
            });
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterEleve([FromBody] Eleve userObj)
        {
            if (userObj == null)
                return BadRequest();

            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exists" });

            if (await CheckUsernameExistAsync(userObj.INE))
                return BadRequest(new { Message = "INE Already Exists" });

            // Uncomment and adjust if you want to check password strength
            // var passMessage = CheckPasswordStrength(userObj.Password);
            // if (!string.IsNullOrEmpty(passMessage))
            //     return BadRequest(new { Message = passMessage });

            userObj.Password = BCrypt.Net.BCrypt.HashPassword(userObj.Password);
            userObj.Role = "Etudiant";
            userObj.Token = "";
            await _authContext.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            var token = CreateToken(userObj);

            return Ok(new
            {
                Message = "Eleve Registered!",
                Token = token
            });
        }

        [HttpGet("{email}/{password}")]
        public async Task<ActionResult> Login(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                return BadRequest("Email and Password are required");
            }

            var eleve = await _authContext.Eleves.FirstOrDefaultAsync(x => x.Email.ToLower() == email.ToLower());

            if (eleve == null)
            {
                // Optionnel: Log l'erreur pour un suivi côté serveur
                // Log.Warning("Tentative de connexion avec un e-mail incorrect: {Email}", email);
                return BadRequest(new { Message = "Email or Password is Incorrect" }); // Ne précise pas si c'est l'email ou le mot de passe
            }

            if (!BCrypt.Net.BCrypt.Verify(password, eleve.Password))
            {
                // Optionnel: Log l'erreur pour un suivi côté serveur
                // Log.Warning("Tentative de connexion avec un mot de passe incorrect pour l'e-mail: {Email}", email);
                return BadRequest(new { Message = "Email or Password is Incorrect" }); // Ne précise pas si c'est l'email ou le mot de passe
            }

            var token = CreateToken(eleve);

            return Ok(new
            {
                Message = "Login Successful!",
                Token = token
            });
        }

        private string CreateToken(Eleve eleve)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, eleve.Nom),
            new Claim("Prenom", eleve.Prenom),
            new Claim("Telephone", eleve.Telephone),
            new Claim("INE", eleve.INE),
            new Claim(ClaimTypes.Email, eleve.Email),
            new Claim(ClaimTypes.Role, eleve.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private async Task<bool> CheckEmailExistAsync(string email)
        {
            return await _authContext.Eleves.AnyAsync(e => e.Email == email);
        }

        private async Task<bool> CheckUsernameExistAsync(string ine)
        {
            return await _authContext.Eleves.AnyAsync(e => e.INE == ine);
        }

        // GET: api/Eleves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Eleve>>> Geteleve()
        {


            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            return await _authContext.Eleves.ToListAsync();
        }

        [HttpPost("register2"),Authorize]
        public async Task<IActionResult> Register1Eleve([FromBody] Eleve userObj)
        {
            if (userObj == null)
                return BadRequest();

            if (await CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exists" });

            if (await CheckUsernameExistAsync(userObj.INE))
                return BadRequest(new { Message = "INE Already Exists" });

            // Uncomment and adjust if you want to check password strength
            // var passMessage = CheckPasswordStrength(userObj.Password);
            // if (!string.IsNullOrEmpty(passMessage))
            //     return BadRequest(new { Message = passMessage });

            userObj.Password = BCrypt.Net.BCrypt.HashPassword(userObj.Password);
            userObj.Role = "Etudiant";
            userObj.Token = "";
            await _authContext.AddAsync(userObj);
            await _authContext.SaveChangesAsync();

            var token = CreateToken(userObj);

            return Ok(new
            {
                Message = "Eleve Registered!",
                Token = token
            });
        }
        [HttpGet("geteleves"),Authorize]
        public async Task<ActionResult<IEnumerable<Eleve>>> Geteleves()
        {


            if (_authContext.Groupes == null)
            {
                return NotFound();
            }
            return await _authContext.Eleves.ToListAsync();
        }

        // GET: api/Eleves/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Eleve>> GetEleve(int id)
        {
            if (_authContext.Eleves == null)
            {
                return NotFound();
            }
            var eleve = await _authContext.Eleves.FindAsync(id);

            if (eleve == null)
            {
                return NotFound();
            }

            return eleve;
        }

        // PUT: api/Eleves/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEleve(int id, Eleve eleve)
        {
            if (id != eleve.Id)
            {
                return BadRequest();
            }

            _authContext.Entry(eleve).State = EntityState.Modified;

            try
            {
                await _authContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EleveExists(id))
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


        // DELETE: api/Eleves/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEleve(int id)
        {
            if (_authContext.Eleves == null)
            {
                return NotFound();
            }
            var eleve = await _authContext.Eleves.FindAsync(id);
            if (eleve == null)
            {
                return NotFound();
            }

            _authContext.Eleves.Remove(eleve);
            await _authContext.SaveChangesAsync();

            return NoContent();
        }

        private bool EleveExists(int id)
        {
            return (_authContext.Eleves?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }

}



