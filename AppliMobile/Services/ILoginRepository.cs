using AppliMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppliMobile.Services
{
    public interface ILoginRepository
    {
        Task<Eleve> Login(string email, string password);
    }
}
