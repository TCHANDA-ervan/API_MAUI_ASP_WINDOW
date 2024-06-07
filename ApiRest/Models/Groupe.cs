using System.ComponentModel.DataAnnotations;

namespace ApiRest.Models
{
    public class Groupe
    {
        [Key]
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Formation { get; set; }

    }
}
