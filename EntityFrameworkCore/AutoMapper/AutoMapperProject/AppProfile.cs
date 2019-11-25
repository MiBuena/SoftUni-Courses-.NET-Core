using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace AutoMapperProject
{
    public class AppProfile : Profile
    {
        public AppProfile()
        {
            CreateMap<Source, Destination>();
        }
    }
}
