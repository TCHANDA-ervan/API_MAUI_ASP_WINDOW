
namespace webapi.Models
{
    public class Adminitrateur
    {
       
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }
        public string Nom { get; set; }
        public string Token { get; set; }
        public string Role { get; set; }
    }
}
