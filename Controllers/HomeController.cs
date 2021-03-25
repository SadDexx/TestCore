using Microsoft.AspNetCore.Mvc;
using NETCORE.Models;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using NETCORE.Interface;
using System;

namespace NETCORE.Controllers
{
    public class HomeController : Controller
    {
        IUser usr;
        public HomeController(IUser u)
        {
            usr = u;
        }
        public IActionResult Index()
        {
            //ViewData["Message"] = "Test";
            //return View("Index");
            return View(usr.Alls());
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult GetUser(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            User user = usr.Get(id.Value);
            if (user == null)
                return NotFound();
            return View(user);
        }

        public IActionResult AddUser() => View();

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (ModelState.IsValid)
            {
                usr.Create(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
