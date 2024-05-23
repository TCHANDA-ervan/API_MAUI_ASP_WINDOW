using System.ComponentModel.DataAnnotations;

namespace ApiRest.Models
{
    public class Journee
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateMatin { get; set; }
        public DateTime DateSoir { get; set; }
    }
}
