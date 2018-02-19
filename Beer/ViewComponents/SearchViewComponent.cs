using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Beer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;

namespace Beer.ViewComponents
{
    public class Search: ViewComponent
    {
        static HttpClient client = new HttpClient();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IViewComponentResult> InvokeAsync(string searchParam="peach")
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
            var data = JsonConvert.DeserializeObject<RootModel>(response);

            return View(data);
        }
    }
}
