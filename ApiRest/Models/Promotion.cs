using System.ComponentModel.DataAnnotations;

namespace ApiRest.Models
{
    public class Promotion
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Niveau { get; set; }
        public string Annee { get; set; }
    }
}
