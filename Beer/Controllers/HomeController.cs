using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Beer.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Dynamic;
using Newtonsoft.Json.Linq;
using System.Net;
using Newtonsoft.Json;

namespace Beer.Controllers
{
    public class HomeController : Controller
    {
        static HttpClient client = new HttpClient();

        public async Task<IActionResult> Index(int currentPage = 1, string sort = "ASC")
        {
            Uri geturi = new Uri("http://api.brewerydb.com/v2/beers/?key=ee8a1a84bc76fd7d7ae6dd0dc45583e3&p=" + currentPage + "&sort=" + sort);

            HttpResponseMessage responseGet = await client.GetAsync(geturi);

            // The EnsureSuccessStatusCode method throws an exception if the HTTP response was unsuccessful. 
            // If the Content is not null, this method will also call Dispose to free managed and unmanaged resources.
            responseGet.EnsureSuccessStatusCode();

            var response = "";

            if (responseGet.IsSuccessStatusCode)
            {
                response = await responseGet.Content.ReadAsStringAsync();
                // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it)                
            }

            var data = JsonConvert.DeserializeObject<RootModel>(response);

            return View(data);
        }

        public async Task<IActionResult> GetDetails(string id)
        {
            Uri geturi = new Uri("http://api.brewerydb.com/v2/beer/" + id + "/?key=ee8a1a84bc76fd7d7ae6dd0dc45583e3");
            
            HttpResponseMessage responseGet = await client.GetAsync(geturi);

            // The EnsureSuccessStatusCode method throws an exception if the HTTP response was unsuccessful. 
            // If the Content is not null, this method will also call Dispose to free managed and unmanaged resources.
            responseGet.EnsureSuccessStatusCode();

            var response = "";

            if (responseGet.IsSuccessStatusCode)
            {
                response = await responseGet.Content.ReadAsStringAsync();                               
            }

            // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it) 
            var data = JsonConvert.DeserializeObject<BeerModel>(response);

            return View(data);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
