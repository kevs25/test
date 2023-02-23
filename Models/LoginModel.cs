using System;
using Microsoft.AspNetCore.Mvc;
namespace Webapp.Models
{
    public class LoginModel
    {
        public string? username{get; set;}

        public string? password{get; set;}
    }
}