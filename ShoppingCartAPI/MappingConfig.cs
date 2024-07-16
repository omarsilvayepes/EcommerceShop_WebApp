using AutoMapper;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Models.Dtos;

namespace ShoppingCartAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mapperConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<CartHeader, CartHeaderDto>().ReverseMap(); //map also CartHeaderDto --> CartHeader
                config.CreateMap<CartDetails, CartDetailsDto>().ReverseMap(); //map also CartDetailsDto --> CartDetails
            }) ;
            return mapperConfig;
        }
    }
}
