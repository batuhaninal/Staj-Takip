using AutoMapper;
using StajTakip.Entities.Concrete;
using StajTakip.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StajTakip.Business.Utilities.AutoMapper.Profiles
{
    public class InternshipsBookProfile : Profile
    {
        public InternshipsBookProfile()
        {
            CreateMap<InternshipsBookPageListDto, InternshipsBook>().ReverseMap();
            CreateMap<InternshipsBookPageAddDto, InternshipsBook>().ReverseMap();
            CreateMap<InternshipsBookPageUpdateDto, InternshipsBook>().ReverseMap();
        }
    }
}
