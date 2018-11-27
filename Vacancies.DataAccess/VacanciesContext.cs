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
         : base(options) { Database.EnsureCreated(); }
/*        public DbSet<VacancieFromApiHH> VacancieFromApiHHs { get; set; }
         public DbSet<BillingType> BillingTypes { get; set; }
         public DbSet<Site> Sites { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Employment> Employments { get; set; }
      //  public DbSet<Specialization> Specializations { get; set; }
      //  public DbSet<LogoUrls> LogoUrls { get; set; }
        public DbSet<DataModel.Entities.Type> Types { get; set; }
        public DbSet<Salary> Salares { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<Phone> Phones { get; set; }
        //   public DbSet<Contacts> Contacts { get; set; } */

        public DbSet<Vacancie> Vacancies { get; set; }

    }
}
