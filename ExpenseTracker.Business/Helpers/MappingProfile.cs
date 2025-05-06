using AutoMapper;
using ExpenseTracker.Business.Dtos.Category;
using ExpenseTracker.Business.Dtos.Expense;
using ExpenseTracker.Business.Dtos.PaymentMethod;
using ExpenseTracker.Business.Dtos.User;
using ExpenseTracker.Data.Entities;

namespace ExpenseTracker.Business.Helpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<RegisterUserRequestDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<User, UserResponseDto>()
                .ForMember(dest => dest.FullName, opt =>
                    opt.MapFrom(src => $"{src.FirstName} {src.MiddleName} {src.LastName}".Replace("  ", " ")))
                .ForMember(dest => dest.Role, opt =>
                    opt.MapFrom(src => src.Role.ToString()));

            CreateMap<UpdateUserRequestDto, User>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.Role, opt => opt.Ignore())
    .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
    .ForMember(dest => dest.IsActive, opt => opt.Ignore());

            CreateMap<CreateExpenseRequestDto, Expense>()
    .ForMember(dest => dest.Id, opt => opt.Ignore())
    .ForMember(dest => dest.Status, opt => opt.Ignore())
    .ForMember(dest => dest.UserId, opt => opt.Ignore())
    .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

            CreateMap<Expense, ExpenseResponseDto>()
    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
    .ForMember(dest => dest.PaymentMethodName, opt => opt.MapFrom(src => src.PaymentMethod != null ? src.PaymentMethod.Name : ""))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.UserFullName,
           opt => opt.MapFrom(src => src.User.FirstName + " " + src.User.LastName));


            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Category, CategoryResponseDto>();

            CreateMap<CreatePaymentMethodDto, PaymentMethod>();
            CreateMap<UpdatePaymentMethodDto, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodResponseDto>();

        }
    }
}
