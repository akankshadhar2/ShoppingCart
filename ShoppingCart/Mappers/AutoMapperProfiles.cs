using AutoMapper;
using ShoppingCart.Models.DTO;
using ShoppingCart.Models;
using ShoppingCart.Models.Domain;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ShoppingCartApplication.Mappers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<UserDTO, User>();
        }
    }
}