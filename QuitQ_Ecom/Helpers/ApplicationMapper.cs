using AutoMapper;
using QuitQ_Ecom.DTOs;
using QuitQ_Ecom.Models;

namespace QuitQ_Ecom.Helpers
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<SubCategory,SubCategoryDTO>().ReverseMap();
            CreateMap<Account, AccountDTO>().ReverseMap();
            CreateMap<Brand, BrandDTO>().ReverseMap();
            CreateMap<Cart, CartDTO>().ReverseMap();
            
            CreateMap<City, CityDTO>().ReverseMap();
            CreateMap<Gender, GenderDTO>().ReverseMap();
            CreateMap<Image, ImageDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Payment, PaymentDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<ProductDetail, ProductDetailDTO>().ReverseMap();
            CreateMap<State, StateDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();
            CreateMap<Store, StoreDTO>().ReverseMap();
            
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserAddress, UserAddressDTO>().ReverseMap();
            CreateMap<UserStatus, UserStatusDTO>().ReverseMap();
            CreateMap<UserType, UserTypeDTO>().ReverseMap();
            CreateMap<WishList, WishListDTO>().ReverseMap();

            CreateMap<Shipper, ShipperDTO>().ReverseMap();
        }
    }
}
