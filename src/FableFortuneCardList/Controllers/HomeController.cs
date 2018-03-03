﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FableFortuneCardList.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "cards");
        }

        public IActionResult Error()
        {
            return View();
        }

        public IActionResult PrivacyAndToS()
        {
            return View();
        }
    }
}
