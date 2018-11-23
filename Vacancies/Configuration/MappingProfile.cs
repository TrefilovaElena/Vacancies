using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vacancies.DataModel.Entities;

namespace Vacancies
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Salary, VacanciesSalary>();
            CreateMap<VacanciesSalary, Salary>();
            CreateMap<Employer, VacanciesEmployer>();
            CreateMap<VacanciesSalary, Employer>();
            CreateMap<Snippet, VacanciesSnippet>();
            CreateMap<VacanciesSnippet, Snippet>();
            CreateMap<Phone, VacanciesPhone>();
            CreateMap<VacanciesPhone, Phone>();
            CreateMap<Contacts, VacanciesContacts>();
            CreateMap<VacanciesContacts, Contacts>();
            CreateMap<Item, VacanciesItem>();
            CreateMap<VacanciesItem, Item>();

        }
    }
}
