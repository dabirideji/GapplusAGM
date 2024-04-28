using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BarcodeGenerator.Models;
using Gapplus.Application.DTO.Company.Request;
using Gapplus.Application.DTO.Company.Response;
using Gapplus.Application.DTO.User.Request;
using Gapplus.Application.DTO.User.Response;
using Gapplus.Domain;
using Gapplus.Domain.Models.Base;

namespace Gapplus.Application.Helpers.AutoMapperProfiles
{
  public class GapplusAutomapperProfile : Profile
  {
    public GapplusAutomapperProfile()
    {
      //USER MAPPINGS
      CreateMap<CreateUserDto, User>();
      CreateMap<UpdateUserDto, User>();
      CreateMap<ReadUserDto, User>().ReverseMap();


      //COMPANY MAPPINGS
      CreateMap<CreateCompanyDto, Company>();
      CreateMap<UpdateCompanyDto, Company>();
      CreateMap<ReadCompanyDto, Company>().ReverseMap();
    }

  }
}