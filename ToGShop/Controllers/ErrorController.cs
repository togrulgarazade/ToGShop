﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToGShop.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Problem()
        {
            return View();
        }
    }
}
