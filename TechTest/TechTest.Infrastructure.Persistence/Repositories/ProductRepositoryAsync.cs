﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TechTest.Application.Interfaces.Repositories;
using TechTest.Domain.Entities;
using TechTest.Infrastructure.Persistence.Contexts;
using TechTest.Infrastructure.Persistence.Repository;

namespace TechTest.Infrastructure.Persistence.Repositories
{
    public class ProductRepositoryAsync : GenericRepositoryAsync<Product>, IProductRepositoryAsync
    {
        private readonly DbSet<Product> _products;

        public ProductRepositoryAsync(IServiceProvider service) : base(service)
        {
            var dbContext = (ApplicationDbContext)service.GetService(typeof(ApplicationDbContext));
            _products = dbContext.Set<Product>();
        }

        public Task<bool> IsUniqueBarcodeAsync(string barcode)
        {
            return _products
                .AllAsync(p => p.Barcode != barcode);
        }
    }
}
