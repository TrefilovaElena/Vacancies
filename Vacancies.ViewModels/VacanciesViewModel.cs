﻿using System;
using Vacancies.DataModel.Entities;

namespace Vacancies.ViewModels
{
    public class VacanciesViewModel
    {
        public int Id { get; set; }
        public string IdHH { get; set; }
        public string Name { get; set; }
        public string Salary  { get; set; }
        public string OrganisationName { get; set; }
        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public string Employment { get; set; }
        public string Description { get; set; }

       
    }
}
