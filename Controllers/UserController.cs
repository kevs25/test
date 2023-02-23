using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Web.Mvc;
using Webapp.Models;
using System;
using System.Data;

namespace Webapp.Controllers
{
    public class UserController : Controller
    {
        string username="";

        [HttpGet]
        public IActionResult Index()
        {
            Console.WriteLine(HttpContext.Session.GetString("check"));
            Console.WriteLine("Index Get");
            // if(username == "")
            // {
            //     return RedirectToAction("Login","Home");
            // }

            if(HttpContext.Session.GetString("check") != "true")
            {
                return RedirectToAction("Login","Home");
            }

            
           ViewBag.User = HttpContext.Session.GetString("user");
            return View();
        }
      
       [HttpPost]
        public IActionResult Index(LoginModel login)
        {
            Console.WriteLine("Index Post");

            ViewBag.User = login.username;
            // username = login.username;
            HttpContext.Session.SetString("check","true");
            HttpContext.Session.SetString("user",login.username);

            Console.WriteLine(username);
            return View();
        }
    }
}