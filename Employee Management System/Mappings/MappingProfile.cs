﻿using AutoMapper;
using Employee_Management_System.Models.Entities;
using Employee_Management_System.Models.ViewModels;

namespace Employee_Management_System.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.DepartmentName, opt => opt.MapFrom(src => src.Department.Name));

            CreateMap<EmployeeViewModel, Employee>().ReverseMap();

            CreateMap<TaskAssignment, TaskAssignmentViewModel>()
                .ForMember(dest => dest.EmployeeName, opt => opt.MapFrom(src => src.Employee.UserName))
                .ForMember(dest => dest.AssignedTo, opt => opt.MapFrom(src => src.Employee.FullName))
                .ForMember(dest => dest.Designation, opt => opt.MapFrom(src => src.Employee.Designation))
                .ForMember(dest => dest.IsCompleted, opt => opt.MapFrom(src => src.Description == "Completed"));

            CreateMap<TaskAssignmentViewModel, TaskAssignment>();   
        }
    }
}
