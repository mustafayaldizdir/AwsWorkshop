using AutoMapper;
using AwsWorkshop.Product.Api.Core.Application.Dtos;
using AwsWorkshop.Product.Api.Core.Domain.Entities;
using System.Text.Json;
using System.Web;

namespace AwsWorkshop.Product.Api.Core.Application.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AppProduct, ProductDto>().ReverseMap()
                .ForMember(dest => dest.ProductCode, opt => opt.MapFrom(src => src.ProductCode + "-" + src.ProductId));

         
        }

    }
}
