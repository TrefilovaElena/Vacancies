using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vacancies.Common.Helpers;
using Vacancies.DataAccess;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
   public class VacanciesDBService: IVacanciesDBService
    {
        VacanciesContext db;

        public VacanciesDBService(VacanciesContext context)
        {
            db = context;
        }
        public void DeleteVacancieFromDB(int id)
        {
            try
            {
                var entity = db.Vacancies.FirstOrDefault(x => int.Parse(x.IdHH) == id);
                if (entity != null)
                {
                    db.Vacancies.Remove(entity);
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка удаления записи из БД. {ex.Message}");
            }

        }


        public List<VacanciesViewModel> GetVacanciesViewModelFromDB(int number, SearchVacanciesViewModel searchModel)
        {
            try
            {
           

                var query = db.Vacancies.Take(number);
                        

                if (searchModel != null)
                {if (CommonHelpers.CreateStringFromNull(searchModel.SearchText) !="")
                        query = query.Where(a => a.Description.ToLower().Contains(searchModel.SearchText) || a.Name.ToLower().Contains(searchModel.SearchText)); }

                query = query.OrderByDescending(x => x.Created_at);

                var result = query.Select(y => new VacanciesViewModel
                {
                    Id = y.Id,
                    IdHH = y.IdHH,
                    Name = y.Name,
                    Salary = y.Salary,
                    OrganisationName = y.OrganisationName,
                    Description=CommonHelpers.CreateStringFromNull(y.Description),
                    Contact = CommonHelpers.CreateStringFromNull(y.Contact),
                    PhoneNumber = CommonHelpers.CreateStringFromNull(y.PhoneNumber),
                    Employment = CommonHelpers.CreateStringFromNull(y.Employment)
                });
                 return result.ToList<VacanciesViewModel>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения данных из БД. {ex.Message}");
            }

        }
    }
}
