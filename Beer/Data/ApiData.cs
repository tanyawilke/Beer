using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Beer.Data
{
    public class ApiData
    {
        private static HttpClient client = new HttpClient();
        private string baseUrl = "http://api.brewerydb.com/v2";
        private string apiKey = "ee8a1a84bc76fd7d7ae6dd0dc45583e3";

        // public  ApiData(string apiParam)
    }
}
