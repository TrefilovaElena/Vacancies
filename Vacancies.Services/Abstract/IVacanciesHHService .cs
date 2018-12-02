
using System.Collections.Generic;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
   public interface IVacanciesHHService
    {
        List<VacanciesViewModel> GetVacanciesViewModel(int amount, SearchVacanciesViewModel searchParameters);
        IdsWrapper SaveVacanciesToDB(List<string> ids);

    }
}
