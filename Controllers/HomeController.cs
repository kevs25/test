using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Web.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Webapp.Models;
using System;
using System.Data;
using System.Text.Json;



namespace Webapp.Controllers;

public class HomeController : Controller
{
    string? logincheck="logincheck";
    string? usercheck ="usercheck";

    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    
    
        
    public IActionResult Index()
    {
        Console.WriteLine(HttpContext.Session.GetString("log"));
        return View();
    }
    

    public IActionResult Contact()
    {
        return View();
    }
    [HttpGet]
    public IActionResult Login()
    {
            Console.WriteLine("Login Get");

        // var name = HttpContext.Session.GetString("logincheck");

        // _logger.LogInformation("Session Name: {Name}", name);
        
       if(HttpContext.Session.GetString("logincheck")=="ok")
        {
            Console.WriteLine("Login Get Session check");

            // return RedirectToAction("Index","User");
            return Redirect("~/User/Index");

        }
        return View();
    }
    [HttpPost]
    public IActionResult Login(LoginModel login)
    {
            Console.WriteLine("Login Post");

            // HttpContext.Session.GetString(sessionname);
        
            
        int choice = ValidationModel.LoginValidation(login);
        if(choice == 1)
        {   


            HttpContext.Session.SetString(logincheck,"ok");
            HttpContext.Session.SetString(usercheck,login.username.ToString());

            
            // return RedirectToAction("Index","User",login.username);

            return RedirectPreserveMethod("~/User/Index");

        }
        else if(choice == 2)
        {
            ViewBag.Message="incorrect password";
            return View("Login");
        }
        else if(choice == 3)
        {
            ViewBag.Message="User does not exists";
            return View("Login");
        }
        else
            return View();
          
        
    }
    [HttpGet]
    public IActionResult forgotPassword()
    {
        return View();
    }
    [HttpPost]
    public IActionResult forgotPassword(ForgotPasswordModel forgotPasswordModel)
    {
        int choice = ValidationModel.ForgotPassword(forgotPasswordModel);
        if(choice == 1)
            return View("Index");
        else if(choice == 2)
        {
            ViewBag.Message="User does not exist";
            return View("ForgotPassword");
        }
        else if(choice == 3)
        {
            ViewBag.Message="Password Does not Match";
            return View("ForgotPassword");
        }
        else if(choice == 4)
        {
            ViewBag.Message="Wrong Validation.Please include more than 8 charcters atleast one special character, one uppercase, one lowercase, one number,";
            return View();
        }
        else
            return View();
            
    }
    
    [HttpGet]
    public IActionResult SignUp()
    {
        return View();
    }
    [HttpPost]

   
    public IActionResult SignUp(SignUpModel register)
    {
        int choice = ValidationModel.signupValidation(register);
        if(choice == 1)
            return View("Index");
        else if(choice == 2)
        {
            ViewBag.Message="password does not match";
            return View("SignUp");
        }
            
        else if(choice == 3)
        {
            ViewBag.Message="Invalid username or passsword";
            return View("SignUp");
        }
        else if(choice == 4)
        {
            ViewBag.Message="User already Exists. Please use different Email-ID";
            return View("SignUp");
        }
        else
            return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}
