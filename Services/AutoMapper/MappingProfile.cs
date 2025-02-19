﻿using AutoMapper;
using RSAHyundai.Data.Entities;
using RSAHyundai.DTOs.Projects;
using RSAHyundai.DTOs.Tags;
using RSAHyundai.DTOs.Tasks;
using RSAHyundai.DTOs.Users;
using RSAHyundai.Encryption;
using RSAHyundai.Data.Entities;
using System;
using System.Linq;

namespace RSAHyundai.AutoMapper
{
    public class MappingProfile : Profile
    {
        private readonly IPasswordHasher _pwHasher;
        public MappingProfile(IPasswordHasher pwHasher)
        {
            _pwHasher = pwHasher;

            // User
            CreateMap<UserRegisterDTO, UserModel>().ForMember(dest => dest.Password, opt => opt.MapFrom(src => _pwHasher.Hash(src.Password)));
            CreateMap<UserDTO, UserModel>();
            CreateMap<UserModel, UserDetailsDTO>();

            //Project
            CreateMap<ProjectDTO, ProjectModel>()
                .ForMember(dest => dest.LastUpdated, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<ProjectModel, ProjectDetailsDTO>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.ProjectTags.Select(c => c.Tag)))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(src => src.Tasks));

            //Tags
            CreateMap<TagDTO, TagModel>();
            CreateMap<TagModel, TagDetailsDTO>()
                .ForMember(dest => dest.AssociatedProjectsCount, opt => opt.MapFrom(src => src != null ? src.ProjectTags.Count : 0));


            //Tasks
            CreateMap<TaskModel, TaskDetailsDTO>();
        }
    }
}
