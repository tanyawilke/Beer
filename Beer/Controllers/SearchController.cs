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
    public class SearchController : Controller
    {
        static HttpClient client = new HttpClient();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SearchModel searchParam)
        {
            Uri geturi = new Uri("http://api.brewerydb.com/v2/search/?key=ee8a1a84bc76fd7d7ae6dd0dc45583e3&type=beer&q=" + searchParam);

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

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
