using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRest.Models
{
    public class Eleve
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Telephone { get; set; }
        public string INE { get; set; }
        public string Email { get; set; }

        public string Password { get; set; }
       
        public string Token { get; set; }
        public string Role { get; set; }

        public string Description { get; set; }


        [ForeignKey("Promotion")]
        public int IdPromotion { get; set; }
        public Promotion Promotion { get; set; }
        
        [ForeignKey("Groupe")]
        public int IdGroupe { get; set; }
        public Groupe Groupe { get; set; }
       
    }
}
