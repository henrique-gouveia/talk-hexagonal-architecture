using AutoMapper;
using Store4Dev.Application.Commands;
using Store4Dev.Application.ViewModels;
using Store4Dev.Domain.Entities;

namespace Store4Dev.Application.AutoMapper
{
    public class StoreProfile : Profile
    {
        public StoreProfile()
        {
            CreateMap<Product, ProductViewModel>()
                .ForMember(x => x.BrandName, opt => opt.MapFrom(p => p.Brand.Name));

            CreateMap<CreateProductCommand, Product>()
                .ConvertUsing(p => new(
                    Brand.New(p.BrandId, p.BrandName),
                    p.Name,
                    p.CostPrice,
                    p.SalePrice,
                    p.CurrentStock,
                    p.MinStock,
                    true));
        }
    }
}
