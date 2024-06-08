using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace WindowsApp.Models
{
    public class Promotion
    {
       
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Niveau { get; set; }
        public string Annee { get; set; }
    }
}
