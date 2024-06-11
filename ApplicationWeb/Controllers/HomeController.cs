using ApplicationWeb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ApiRest.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace ApplicationWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        string baseURL = "https://localhost:7070/";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;

        }


        public IActionResult Connexion()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            //appel de l'api 
            List<Eleve> eleves = new List<Eleve>();
            List<Promotion> promotions = new List<Promotion>();
            List<Groupe> groupes = new List<Groupe>();
            List<Presence> presences = new List<Presence>();
            // List<Journee> journees = new List<Journee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("api/Eleve");
                HttpResponseMessage getDataPromotion = await client.GetAsync("api/Promotion");
                HttpResponseMessage getDataGroupe = await client.GetAsync("api/Groupe");
                HttpResponseMessage getDataPresence = await client.GetAsync("api/Presence");
                // HttpResponseMessage getDataJournee = await client.GetAsync("api/Journees");

                if (getData.IsSuccessStatusCode)
                {
                    string results = await getData.Content.ReadAsStringAsync();
                    eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

                    string resultgroup = await getDataPromotion.Content.ReadAsStringAsync();
                    promotions = JsonConvert.DeserializeObject<List<Promotion>>(resultgroup);

                    string resultpresen = await getDataPresence.Content.ReadAsStringAsync();
                    presences = JsonConvert.DeserializeObject<List<Presence>>(resultpresen);

                    //  string resultjourn = await getDataJournee.Content.ReadAsStringAsync();
                    //  journees = JsonConvert.DeserializeObject<List<Journee>>(resultjourn);

                    string resultsprom = await getDataGroupe.Content.ReadAsStringAsync();
                    groupes = JsonConvert.DeserializeObject<List<Groupe>>(resultsprom);
                }
                else
                {
                    Console.WriteLine("erreur calling web API");
                }

                ViewData["Eleve"] = eleves;
                ViewData["Promotion"] = promotions;
                ViewData["Groupe"] = groupes;
                ViewData["Presence"] = presences;
                //  ViewData["Journee"] = journees;
            }
            return View();
        }


        public IActionResult Privacy()
        {
            return View();


        }
        public async Task<IActionResult> ERIS()
        {
            //appel de l'api 
            List<Eleve> eleves = new List<Eleve>();
            List<Promotion> promotions = new List<Promotion>();
            List<Groupe> groupes = new List<Groupe>();
            List<Presence> presences = new List<Presence>();
            // List<Journee> journees = new List<Journee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("api/Eleve");
                HttpResponseMessage getDataPromotion = await client.GetAsync("api/Promotion");
                HttpResponseMessage getDataGroupe = await client.GetAsync("api/Groupe");
                HttpResponseMessage getDataPresence = await client.GetAsync("api/Presence");
                // HttpResponseMessage getDataJournee = await client.GetAsync("api/Journees");

                if (getData.IsSuccessStatusCode)
                {
                    string results = await getData.Content.ReadAsStringAsync();
                    eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

                    string resultgroup = await getDataPromotion.Content.ReadAsStringAsync();
                    promotions = JsonConvert.DeserializeObject<List<Promotion>>(resultgroup);

                    string resultpresen = await getDataPresence.Content.ReadAsStringAsync();
                    presences = JsonConvert.DeserializeObject<List<Presence>>(resultpresen);

                    //  string resultjourn = await getDataJournee.Content.ReadAsStringAsync();
                    //  journees = JsonConvert.DeserializeObject<List<Journee>>(resultjourn);

                    string resultsprom = await getDataGroupe.Content.ReadAsStringAsync();
                    groupes = JsonConvert.DeserializeObject<List<Groupe>>(resultsprom);
                }
                else
                {
                    Console.WriteLine("erreur calling web API");
                }

                ViewData["Eleve"] = eleves;
                ViewData["Promotion"] = promotions;
                ViewData["Groupe"] = groupes;
                ViewData["Presence"] = presences;
                //  ViewData["Journee"] = journees;
            }
            return View();
        }

        public async Task<IActionResult> Nouveau()
        {
            //appel de l'api 
            List<Eleve> eleves = new List<Eleve>();
            List<Promotion> promotions = new List<Promotion>();
            List<Groupe> groupes = new List<Groupe>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear(); 
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("api/Eleve");
                HttpResponseMessage getDataPromotion = await client.GetAsync("api/Promotion");
                HttpResponseMessage getDataGroupe = await client.GetAsync("api/Groupe");


                if (getData.IsSuccessStatusCode)
                {
                    string results = await getData.Content.ReadAsStringAsync();
                    eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

                    string resultgroup = await getDataPromotion.Content.ReadAsStringAsync();
                    promotions = JsonConvert.DeserializeObject<List<Promotion>>(resultgroup);



                    string resultsprom = await getDataGroupe.Content.ReadAsStringAsync();
                    groupes = JsonConvert.DeserializeObject<List<Groupe>>(resultsprom);
                }
                else
                {
                    Console.WriteLine("erreur calling web API");
                }

                ViewData["Eleve"] = eleves;
                ViewData["Promotion"] = promotions;
                ViewData["Groupe"] = groupes;

                //  ViewData["Journee"] = journees;
            }
            return View();
        }

        public async Task<IActionResult> ING()
        {
            //appel de l'api 
            List<Eleve> eleves = new List<Eleve>();
            List<Promotion> promotions = new List<Promotion>();
            List<Groupe> groupes = new List<Groupe>();
            List<Presence> presences = new List<Presence>();
            // List<Journee> journees = new List<Journee>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseURL);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await client.GetAsync("api/Eleve");
                HttpResponseMessage getDataPromotion = await client.GetAsync("api/Promotion");
                HttpResponseMessage getDataGroupe = await client.GetAsync("api/Groupe");
                HttpResponseMessage getDataPresence = await client.GetAsync("api/Presence");
                // HttpResponseMessage getDataJournee = await client.GetAsync("api/Journees");

                if (getData.IsSuccessStatusCode)
                {
                    string results = await getData.Content.ReadAsStringAsync();
                    eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

                    string resultgroup = await getDataPromotion.Content.ReadAsStringAsync();
                    promotions = JsonConvert.DeserializeObject<List<Promotion>>(resultgroup);

                    string resultpresen = await getDataPresence.Content.ReadAsStringAsync();
                    presences = JsonConvert.DeserializeObject<List<Presence>>(resultpresen);

                    //  string resultjourn = await getDataJournee.Content.ReadAsStringAsync();
                    //  journees = JsonConvert.DeserializeObject<List<Journee>>(resultjourn);

                    string resultsprom = await getDataGroupe.Content.ReadAsStringAsync();
                    groupes = JsonConvert.DeserializeObject<List<Groupe>>(resultsprom);
                }
                else
                {
                    Console.WriteLine("erreur calling web API");
                }

                ViewData["Eleve"] = eleves;
                ViewData["Promotion"] = promotions;
                ViewData["Groupe"] = groupes;
                ViewData["Presence"] = presences;
                //  ViewData["Journee"] = journees;
            }
            return View();
        }

        public async Task<IActionResult> Accueil()
        {
            //appel de l'api 
            //List<Eleve> Eleves = new List<Eleve>();
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(baseURL);
            //    client.DefaultRequestHeaders.Accept.Clear();
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage getData = await client.GetAsync("api/Eleves");

            //    if (getData.IsSuccessStatusCode)
            //    {
            //        string results = getData.Content.ReadAsStringAsync().Result;
            //        Eleves = JsonConvert.DeserializeObject<List<Eleve>>(results);

            //    }
            //    else
            //    {
            //        Console.WriteLine("erreur calling web API");
            //    }

            //    ViewData["Eleves"] = Eleves;

            //}

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
