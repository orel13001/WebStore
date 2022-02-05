using AutoMapper;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;

namespace WebStore.Infrastructure.Automapper
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeEditViewModel>()
                .ForMember(user => user.Patronymic, opt => opt.MapFrom(model => model.Patronomic))
                .ReverseMap();
        }
    }
}
