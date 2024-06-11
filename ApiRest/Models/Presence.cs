using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiRest.Models
{
    public class Presence
    {
        [Key]
        public int Id { get; set; }
        public string statut { get; set; }
        public DateTime Heurescan { get; set; }

        [ForeignKey("Eleve")]
        public int IdEleve { get; set; }
      //  public Eleve Eleve { get; set; }

    }
}
