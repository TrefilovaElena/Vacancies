using System;
using System.Collections.Generic;
using System.Text;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
    public interface IVacanciesDBService
    {
        List<VacanciesViewModel> GetVacanciesViewModelFromDB(int amount, SearchVacanciesViewModel searchParameters);
        void DeleteVacancieFromDB(int id);
    }
}
