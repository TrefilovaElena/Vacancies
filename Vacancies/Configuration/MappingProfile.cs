using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            /* CreateMap<BillingType, Db_BillingType>();
             CreateMap<Db_BillingType, BillingType>();
             CreateMap<Site, Db_Site>();
             CreateMap<Db_Site, Site>();
             CreateMap<Experience, Db_Experience>();
             CreateMap<Db_Experience, Experience>();
             CreateMap<Schedule, Db_Schedule>();
             CreateMap<Db_Schedule, Schedule>();
             CreateMap<Employment, Db_Employment>();
             CreateMap<Db_Employment, Employment>();
             CreateMap<Specialization, Db_Specialization>();
             CreateMap<Db_Specialization, Specialization>();
             CreateMap<LogoUrls, Db_LogoUrls>();
             CreateMap<Db_LogoUrls, LogoUrls>();
             CreateMap<DataModel.Entities.Type, Db_Type>();
             CreateMap<Db_Type, DataModel.Entities.Type>();
             CreateMap<Salary, Db_Salary>();
             CreateMap<Db_Salary, Salary>();
             CreateMap<Area, Db_Area>();
             CreateMap<Db_Area, Area>();
             CreateMap<Employer, Db_Employer>();
             CreateMap<Db_Employer, Employer>();

             CreateMap<Item, Db_Item>();
             CreateMap<Db_Item, Item>();
             */
            CreateMap<VacanciesViewModel, Vacancie>();
            CreateMap<Vacancie, VacanciesViewModel>();

        }
    }
}
