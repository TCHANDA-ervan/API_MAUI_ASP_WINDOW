using ApiRest.Models;
using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static Eleve eleve = new Eleve();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<Eleve> Register(EleveDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            eleve.Nom = request.Nom;
            eleve.Password = passwordHash;

            return Ok(eleve);
        }

        [HttpPost("login")]
        public ActionResult<Eleve> Login(EleveDto request)
        {
            if (eleve.Nom != request.Nom)
            {
                return BadRequest("Élève non trouvé.");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, eleve.Password))
            {
                return BadRequest("Mot de passe incorrect.");
            }

            string token = CreateToken(eleve);


            return Ok(token);
        }
        private string CreateToken(Eleve eleve)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, eleve.Nom),
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
    }
    
}

