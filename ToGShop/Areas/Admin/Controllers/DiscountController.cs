﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.ViewModels.DiscountTimerViewModel;
using Core;

namespace ToGShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DiscountController : Controller
    {
        private readonly IDiscountTimerService _discountTimerService;


        public DiscountController(IDiscountTimerService discountTimerService)
        {
            _discountTimerService = discountTimerService;
        }

        public async Task<IActionResult> Index()
        {
            var discountTimer = await _discountTimerService.Get();

            return View(discountTimer);
        }

        public async Task<IActionResult> Update()
        {



            var discountTimer = await _discountTimerService.Get();


            var discountTimerViewModel = new DiscountTimerViewModel()
            {
                DiscountTitle = discountTimer.DiscountTittle,
                DiscountTime = discountTimer.DiscountTime
            };


            return View(discountTimerViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(DiscountTimerViewModel discountTimerViewModel)
        {
            await _discountTimerService.Update(discountTimerViewModel);
            return RedirectToAction(nameof(Index));
        }
    }
}