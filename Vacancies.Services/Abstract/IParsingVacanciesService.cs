
using System.Collections.Generic;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
   public interface IParsingVacanciesService
    {
        List<VacanciesViewModel> GetVacanciesViewModel(int amount, SearchVacanciesViewModel searchParameters);
    }
}
