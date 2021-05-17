using AutoMapper;
using Becomex.Robo.Application.Models;
using Bexomex.Robo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Becomex.Robo.Application
{
    public class DomainMappingProfile : Profile
    {
        public DomainMappingProfile()
        {
            DomainToDto();
            DtoToDomain();
        }

        public void DomainToDto()
        {
            CreateMap<Arm, ArmModel>().ReverseMap();
            CreateMap<Elbow, ElbowModel>().ReverseMap();
            CreateMap<Fist, FistModel>().ReverseMap();
            CreateMap<Head, HeadModel>().ReverseMap();
            CreateMap<Robot, RobotModel>().ReverseMap();
        }
        public void DtoToDomain()
        {
            
        }
    }
}
