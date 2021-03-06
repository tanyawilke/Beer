﻿using System;
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
        private static HttpClient client = new HttpClient();
        private string baseUrl = "http://api.brewerydb.com/v2";
        private string apiKey = "ee8a1a84bc76fd7d7ae6dd0dc45583e3";

        [HttpGet]
        public async Task<IActionResult> Index(int currentPage = 1, string sort = "ASC", string apiParam="beers")
        {
            Uri geturi = new Uri(baseUrl + "/" + apiParam +"/?key=" + apiKey + "&p=" + currentPage + "&sort=" + sort);

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

        public async Task<IActionResult> GetDetails(string id, string apiParam = "beer")
        {
            Uri geturi = new Uri(baseUrl + "/" + apiParam + "/" + id + "/?key=" + apiKey);
            
            HttpResponseMessage responseGet = await client.GetAsync(geturi);

            BeerModel dat1 = null;
            string response = "";

            // The EnsureSuccessStatusCode method throws an exception if the HTTP response was unsuccessful. 
            // If the Content is not null, this method will also call Dispose to free managed and unmanaged resources.
            responseGet.EnsureSuccessStatusCode();

            if (responseGet.IsSuccessStatusCode)
            {
                response = await responseGet.Content.ReadAsStringAsync();
            }

            // Gotcha discovered for .net core - deserialisation no matter how specified does not work when defined for Generic List
            // When defined as usual, information returned from api cannot be binded to the model.
            dat1 = JsonConvert.DeserializeObject<BeerModel>(response);            

            return View(dat1);
        }

        public async Task<IActionResult> SearchResult(string searchParam, int currentPage = 1, string apiParam = "search")
        {
            Uri geturi = new Uri(baseUrl + "/" + apiParam + "/?key=" + apiKey + "&type=beer&q=" + searchParam + "&p=" + currentPage);

            HttpResponseMessage responseGet = await client.GetAsync(geturi);

            // The EnsureSuccessStatusCode method throws an exception if the HTTP response was unsuccessful. 
            // If the Content is not null, this method will also call Dispose to free managed and unmanaged resources.
            responseGet.EnsureSuccessStatusCode();

            var response = "";

            if (!responseGet.IsSuccessStatusCode)
            {
                RedirectToAction(nameof(Error));          
            }

            response = await responseGet.Content.ReadAsStringAsync();

            // Deserialise the data (include the Newtonsoft JSON Nuget package if you don't already have it) 
            var data = JsonConvert.DeserializeObject<RootModel>(response);

            data.searchParam = searchParam;

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
