using System;
using Microsoft.AspNetCore.Mvc;
namespace Webapp.Models
{

    public class SignUpModel
    {
       public string? username{get;set;}


       public string? password{get; set;}
       public string? emailId{get; set;}

       public string? ConfirmPassword{get; set;}

        public string? choice{get; set;}
    }
}