using System;
using AdvertiseApi.Models;
using AutoMapper;

namespace AdvertiseApi.Services
{
    public class AdvertiseModelProfile : Profile
    {
        public AdvertiseModelProfile()
        {
            CreateMap<AdvertiseModelDb, AdvertiseModel>();
        }
    }
}
