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
        public IActionResult Intervention()
        {
            return View();
        }
    }
}