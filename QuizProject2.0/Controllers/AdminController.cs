using QuizProject2._0.Models;
using QuizProject2._0.Models.AccountView;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace QuizProject2._0
    .Controllers
{

    
    public class AdminController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }
       

        }


       
    }
