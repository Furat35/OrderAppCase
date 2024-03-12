using AutoMapper;
using Ordering.Application.Models.Dtos.Products;
using Ordering.Domain.Entities;

namespace Ordering.Application.Mappings
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<ProductAddDto, Product>();
            CreateMap<Product, ProductListDto>();
        }
    }
}
