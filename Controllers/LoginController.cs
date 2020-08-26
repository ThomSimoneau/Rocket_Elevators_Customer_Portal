using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;


namespace Rocket_Elevators_Customer_Portal
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        static async Task RunAsync()
        {
        
            // Update port # in the following line.
            Program.client.BaseAddress = new Uri("http://localhost:5001/");
            Program.client.DefaultRequestHeaders.Accept.Clear();
            Program.client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


        }
    }
}