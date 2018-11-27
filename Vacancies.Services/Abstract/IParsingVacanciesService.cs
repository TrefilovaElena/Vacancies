
using System.Collections.Generic;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
   public interface IParsingVacanciesService
    {
        List<VacanciesViewModel> GetVacanciesViewModel(int amount, SearchVacanciesViewModel searchParameters);
        List<VacanciesViewModel> GetVacanciesViewModelFromDB(int amount, SearchVacanciesViewModel searchParameters);
        void SaveVacanciesToDB(List<string> ids);
        void DeleteVacancieFromDB(int id);
    }
}
