using AppliMobile.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AppliMobile.Services
{
      public class LoginService : ILoginRepository
      {
           public async Task<Eleve> Login(string email, string password)
           {
               //throw new NotImplementedException();
               //var eleve = new List<Eleve>();
               var client = new HttpClient();

               string url = "http://10.0.2.2:5012/api/Eleve/" + email + "/" +password;
               client.BaseAddress = new Uri(url);
               HttpResponseMessage response= await client.GetAsync(client.BaseAddress);
               if(response.IsSuccessStatusCode)
               {
                   string content = response.Content.ReadAsStringAsync().Result;
                   Eleve eleve = JsonConvert.DeserializeObject<Eleve>(content);
                   return await Task.FromResult(eleve);

               }

                   return null;
           }
      }
}
  