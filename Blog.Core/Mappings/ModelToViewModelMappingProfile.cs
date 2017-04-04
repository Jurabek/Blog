﻿using AutoMapper;
using Blog.Model.Entities;
using Blog.Model.ViewModels;
using System.Linq;

namespace Blog.Core.Mappings
{
    public class ModelToViewModelMappingProfile : Profile
    {
        public ModelToViewModelMappingProfile()
        {
            CreateMap<User, UpdateProfileViewModel>();

            CreateMap<RegiserViewModel, User>()
                .ForMember(u => u.UserName, map => map.MapFrom(vm => vm.Email));

            CreateMap<User, UsersViewModel>()
                .ForMember(vm => vm.Roles, opt => opt.MapFrom(u => u.Roles.Select(ur => ur.Role)))
                .ForMember(vm => vm.Permissions,
                                    opt => opt.MapFrom(u =>
                                            u.Roles.Select(ur => ur.Role)
                                                   .SelectMany(r => r.Permissions)
                                                   .Select(rp => rp.Permission)));


            CreateMap<RoleViewModel, IdentityRole>();
            CreateMap<IdentityRole, RoleViewModel>();
        }
    }
}
