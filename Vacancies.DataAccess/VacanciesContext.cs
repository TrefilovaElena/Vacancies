using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Vacancies.DataModel.Entities;

namespace Vacancies.DataAccess
{
   public class VacanciesContext: DbContext
    {
        public VacanciesContext(DbContextOptions<VacanciesContext> options)
         : base(options) { }


        /*public DbSet<VacanciesSalary> VacanciesSalares { get; set; }
        public DbSet<VacanciesEmployer> VacanciesEmployers { get; set; }
        public DbSet<VacanciesSnippet> VacanciesSnippets { get; set; }

        public DbSet<VacanciesPhone> VacanciesPhones { get; set; }
        public DbSet<VacanciesContacts> VacanciesContacts { get; set; }
        public DbSet<VacanciesItem> VacanciesItems { get; set; }*/



    }
}
