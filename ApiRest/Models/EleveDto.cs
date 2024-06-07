using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApiRest.Models
{
    public class EleveDto
    {
        public required string Nom { get; set; }
        public required string Password { get; set; }
       
    }
}
