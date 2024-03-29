﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Utilities;
using Business.ViewModels.ProductAdminViewModels;
using Business.ViewModels.ProductCommentViewModels;
using Business.ViewModels.ProductViewModels;
using Core;
using Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace ToGShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,SuperModerator")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductService _productService;
        private readonly IProductImageService _productImageService;
        private readonly ICategoryService _categoryService;
        private readonly IBrandService _brandService;
        private readonly IProductCommentService _productCommentService;

        public ProductController(IProductCommentService productCommentService,IProductImageService productImageService,IUnitOfWork unitOfWork, IProductService productService, ICategoryService categoryService, IBrandService brandService)
        {
            _unitOfWork = unitOfWork;
            _productService = productService;
            _categoryService = categoryService;
            _brandService = brandService;
            _productImageService = productImageService;
            _productCommentService = productCommentService;
        }


        public async Task<ActionResult> Index()
        {

            var productViewModel = new ProductAdminViewModel()
            {
                Products = await _productService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                ProductImages = await _productImageService.GetAllAsync()

            };

            return View(productViewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Index(string productSearch)
        {
            ViewData["SearchedProduct"] = productSearch;

            var productQuery = from p in await _unitOfWork.productRepository.GetAllAsync() select p;

            if (!String.IsNullOrEmpty(productSearch))
            {
                productQuery = productQuery.Where(p => p.Name.Trim().ToLower().Contains(productSearch.Trim().ToLower()) || p.Description.Trim().ToLower().Contains(productSearch.Trim().ToLower()) || p.Information.Trim().ToLower().Contains(productSearch.Trim().ToLower()));
            }


            var productViewModel = new ProductAdminViewModel()
            {
                Products = productQuery.ToList(),
                Brands = await _brandService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                ProductImages = await _productImageService.GetAllAsync()

            };

            return View(productViewModel);

            
        }



        [HttpGet]
        public async Task<ActionResult> Create()
        {
            ViewBag.categories = await _categoryService.GetAllAsync();
            ViewBag.brands = await _brandService.GetAllAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductCreateViewModel productViewModel)
        {

            ViewBag.categories = await _categoryService.GetAllAsync();
            ViewBag.brands = await _brandService.GetAllAsync();

            if (ModelState.IsValid)
            {
                foreach (var item in productViewModel.ImageFiles)
                {

                    if (!item.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("ImageFiles", "Seçdiyiniz fayl şəkil tipində olmalıdır ! ");
                        return View(productViewModel);
                    }

                    if (!item.CheckFileSize(300))
                    {
                        ModelState.AddModelError("ImageFiles", "Seçdiyiniz faylın ölçüsü 300 kb dan çox olmamalıdır !");
                        return View(productViewModel);
                    }

                }

                await _productService.Create(productViewModel);
                return RedirectToAction(nameof(Index));
            }


            return View(productViewModel);



            //await _productService.Create(productViewModel);

        }


        public async Task<ActionResult> Update(int id)
        {
            ViewBag.categories = await _categoryService.GetAllAsync();
            ViewBag.brands = await _brandService.GetAllAsync();

            Product product = await _productService.Get(id);

            if (product == null) return NotFound();


            var productViewModel = new ProductUpdateViewModel()
            {
                Name = product.Name,
                Description = product.Description,
                Information = product.Information,
                Price = product.Price,
                Count = product.Count,
                DiscountPrice = product.DiscountPrice,
                CategoryId = product.CategoryId,
                IsDiscount = product.IsDiscount,
                BrandId = product.BrandId

            };



            return View(productViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Update(int id, ProductUpdateViewModel productViewModel)
        {
            ViewBag.categories = await _categoryService.GetAllAsync();
            ViewBag.brands = await _brandService.GetAllAsync();

            if (ModelState.IsValid)
            {
                if (productViewModel.ImageFiles != null)
                {
                    foreach (var item in productViewModel.ImageFiles)
                    {

                        if (!item.CheckFileType("image/"))
                        {
                            ModelState.AddModelError("ImageFiles", "Seçdiyiniz fayl şəkil tipində olmalıdır ! ");
                            return View(productViewModel);
                        }

                        if (!item.CheckFileSize(300))
                        {
                            ModelState.AddModelError("ImageFiles", "Seçdiyiniz faylın ölçüsü 300 kb dan çox olmamalıdır !");
                            return View(productViewModel);
                        }

                    }

                }
                await _productService.Update(id, productViewModel);
                return RedirectToAction(nameof(Index));
            }


            return View(productViewModel);
        }

        public async Task<ActionResult> Delete(int id)
        {
            await _productService.Remove(id);

            return RedirectToAction(nameof(Index));
        }

        [Authorize]
        public async Task<IActionResult> ProductDetail(int id)
        {

            var productCommentViewModel = new ProductCommentViewModel()
            {
                Product = await _productService.Get(id),
                Comments = await _productCommentService.GetProductId(id),
                ProductImages = await _productImageService.GetAllAsync(),
                Categories = await _categoryService.GetAllAsync(),
                Brands = await _brandService.GetAllAsync()
            };

            return View(productCommentViewModel);

        }

    }
}
