using AutoMapper;
using Pr1_ASP.Dtos;
using Pr1_ASP.Models;

namespace Pr1_ASP.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // ===== Entity -> DTO (чтение, с вложенными связанными данными) =====
        CreateMap<Role, RoleDto>();

        CreateMap<Category, CategoryBriefDto>();
        CreateMap<Category, CategoryDto>()
            .ForMember(d => d.ParentCategoryName,
                opt => opt.MapFrom(src => src.ParentCategory != null ? src.ParentCategory.CategoryName : null));

        CreateMap<User, UserBriefDto>();
        CreateMap<User, UserDto>();                 // Role -> RoleDto; PasswordHash в DTO нет

        CreateMap<Product, ProductBriefDto>();
        CreateMap<Product, ProductDto>();           // Category -> CategoryBriefDto

        CreateMap<CartItem, CartItemDto>();         // Product -> ProductBriefDto
        CreateMap<Cart, CartDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(src => src.CartItems));

        CreateMap<OrderItem, OrderItemDto>();
        CreateMap<Order, OrderDto>()
            .ForMember(d => d.Items, opt => opt.MapFrom(src => src.OrderItems));

        CreateMap<Reservation, ReservationDto>()
            .ForMember(d => d.IsActive, opt => opt.MapFrom(src => src.ReleasedAt == null));

        // ===== DTO -> Entity (запись). Навигации (Author/Role/...) не трогаем: приходят только Id =====
        // Create/Update — полный перенос полей. Patch — переносим только непустые (Condition),
        // поэтому ручного "if (dto.X is not null) ..." не нужно.

        // Role: DTO -> Entity
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        CreateMap<PatchRoleDto, Role>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Category: DTO -> Entity
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<PatchCategoryDto, Category>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // User: DTO -> Entity
        CreateMap<CreateUserDto, User>()
            .ForMember(d => d.PasswordHash, opt => opt.Ignore());
        CreateMap<UpdateUserDto, User>()
            .ForMember(d => d.PasswordHash, opt => opt.Ignore());
        CreateMap<PatchUserDto, User>()
            .ForMember(d => d.PasswordHash, opt => opt.Ignore())
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Product: DTO -> Entity
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<PatchProductDto, Product>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Cart: DTO -> Entity
        CreateMap<CreateCartDto, Cart>();
        CreateMap<UpdateCartDto, Cart>();
        CreateMap<PatchCartDto, Cart>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // CartItem: DTO -> Entity
        CreateMap<CreateCartItemDto, CartItem>();
        CreateMap<UpdateCartItemDto, CartItem>();
        CreateMap<PatchCartItemDto, CartItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Order: DTO -> Entity
        CreateMap<CreateOrderDto, Order>();
        CreateMap<UpdateOrderDto, Order>();
        CreateMap<PatchOrderDto, Order>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // OrderItem: DTO -> Entity
        CreateMap<CreateOrderItemDto, OrderItem>();
        CreateMap<UpdateOrderItemDto, OrderItem>();
        CreateMap<PatchOrderItemDto, OrderItem>()
            .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

        // Резервирование: запрос -> запись (ReservedAt проставляет сервис)
        CreateMap<ReserveProductDto, Reservation>();
    }
}
