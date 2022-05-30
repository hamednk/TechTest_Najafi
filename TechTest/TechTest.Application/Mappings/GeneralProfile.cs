using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using TechTest.Application.Features.Products.Commands.CreateProduct;
using TechTest.Application.Features.Products.Queries.GetAllProducts;
using TechTest.Domain.Entities;

namespace TechTest.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            CreateMap<Product, GetAllProductsViewModel>().ReverseMap();
            CreateMap<CreateProductCommand, Product>();
            CreateMap<GetAllProductsQuery, GetAllProductsParameter>();
        }
    }
}
