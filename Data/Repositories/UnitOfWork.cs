﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Core;
using Data.DAL;
using Data.Repositories.Implementations;
using Core.Interfaces;

namespace Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IProductRepository _productRepository;
        private ICategoryRepository _categoryRepository;
        private IBrandRepository _brandRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public IProductRepository productRepository => 
            _productRepository = _productRepository ?? new ProductRepository(_context);

        public ICategoryRepository categoryRepository =>
            _categoryRepository = _categoryRepository ?? new CategoryRepository(_context);

        public IBrandRepository brandRepository => 
            _brandRepository = _brandRepository ?? new BrandRepository(_context);


        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
