using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Vacancies.Common.Helpers;
using Vacancies.DataAccess;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
    public class VacanciesHHService : IVacanciesHHService
    {
        private IMapper _mapper;
        VacanciesContext db;

        public VacanciesHHService(VacanciesContext context, IMapper mapper)
        {
            db = context;
            _mapper = mapper;
        }

        private T GetdataFromApi<T>(string source)
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Headers.Add("User-Agent: api-test-agent");
                    string HtmlResult = client.DownloadString(source);
                    T rootObject = JsonConvert.DeserializeObject<T>(HtmlResult);
                    return rootObject;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка получения данных с сайта. {ex.Message}");
            }
        }
        private string CreateSearchStringForHH(SearchVacanciesViewModel searchModel)
        {
            string searchstring = "";
            if (searchModel != null)
            {
                if (searchModel.SearchText != null) { searchstring = searchModel.SearchText == "" ? "" : $"&text={searchModel.SearchText}"; }
            }
            return searchstring;
        }


        private Vacancie ConvertToVacancieDB(VacancieFromApiHH vacancieFromApiHH)
        {
            Vacancie vacancie = new Vacancie();
            vacancie.IdHH = vacancieFromApiHH.id;
            vacancie.Name = vacancieFromApiHH.name;
            vacancie.OrganisationName = CommonHelpers.CreateStringFromNull(vacancieFromApiHH.employer.name);
            if (vacancieFromApiHH.contacts != null)
            {
                vacancie.Contact = vacancieFromApiHH.contacts.name;
                if (vacancieFromApiHH.contacts.phones.Count > 0)
                {
                    vacancie.PhoneNumber = $"+{vacancieFromApiHH.contacts.phones[0].country} ({vacancieFromApiHH.contacts.phones[0].city}) - {vacancieFromApiHH.contacts.phones[0].number}";
                }
                else
                { vacancie.PhoneNumber = ""; }
            }
            else
            {
                vacancie.PhoneNumber = "";
                vacancie.Contact = "";
            }

            if (vacancieFromApiHH.salary != null)
                vacancie.Salary = vacancieFromApiHH.salary.to != null ? $"от {vacancieFromApiHH.salary.from} до {vacancieFromApiHH.salary.to}  { vacancieFromApiHH.salary.currency}" : $"{vacancieFromApiHH.salary.from} {vacancieFromApiHH.salary.currency}";
            else
                vacancie.Salary = "";
            vacancie.Description = vacancieFromApiHH.description;
            vacancie.Created_at = vacancieFromApiHH.created_at;
            if (vacancieFromApiHH.employment != null)
                vacancie.Employment = vacancieFromApiHH.employment.name;
            else
                vacancie.Employment = "";
            return vacancie;
        }

        public List<VacanciesViewModel> GetVacanciesViewModel(int number, SearchVacanciesViewModel searchModel)
        {
            try
            {
                int count = 0;
                int page = 1;
                string hrefHHVacancies = "https://api.hh.ru/vacancies/";
                bool endOfSearch = false;
                List<VacanciesViewModel> listOfVacanciesViewModel = new List<VacanciesViewModel>();


                while ((count < number) && !endOfSearch)
                {
                    RootObject itemsFromApi = GetdataFromApi<RootObject>($"{hrefHHVacancies}?page={page}&per_page={number}{CreateSearchStringForHH(searchModel)}");
                    //если ничего не найдено
                    if (itemsFromApi.items.Count == 0)
                    {
                        return listOfVacanciesViewModel;
                    }
                    else
                    {
                        page++;
                        //если на этой странице вакансий меньше чем запрошенный максимум, то значит результатов больше нет и следующую страницу можно не вызывать
                        if (itemsFromApi.items.Count < number)
                        { endOfSearch = true; }
                    };

                    foreach (var item in itemsFromApi.items)
                    {
                        if ((!item.archived) && (!(listOfVacanciesViewModel.Any(x => x.IdHH == item.id)))) //убираются дублирующие записи с одинаковым Id (можно будет и все поля добавить)
                        {
                            VacancieFromApiHH vacancieFromApiHH = GetdataFromApi<VacancieFromApiHH>("https://api.hh.ru/vacancies/" + item.id);
                            Vacancie vacancie = ConvertToVacancieDB(vacancieFromApiHH);
                            listOfVacanciesViewModel.Add(_mapper.Map<Vacancie, VacanciesViewModel>(vacancie));
                            count++;
                        }
                        if (count == number) return listOfVacanciesViewModel;
                    }
                }
                return listOfVacanciesViewModel;

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка формирования списка вакансий. {ex.Message}");
            }

        }

   


        public IdsWrapper SaveVacanciesToDB(List<string> ids)
        {

            try
            {
                List<Vacancie> vacancies = new List<Vacancie>();
                foreach (var item in ids)
                {
                    var entity = db.Vacancies.FirstOrDefault(x => string.Compare(x.IdHH, item) == 0);
                    if (entity == null)
                    {
                        VacancieFromApiHH vacancieFromApiHH = GetdataFromApi<VacancieFromApiHH>("https://api.hh.ru/vacancies/" + item);
                        if (vacancieFromApiHH != null)
                        {                          
                            vacancies.Add(ConvertToVacancieDB(vacancieFromApiHH));
                        }
                    }
                };
                if (vacancies.Count > 0)
                {
                    db.Vacancies.AddRange(vacancies);
                    db.SaveChanges();
                };

                IdsWrapper idsWrapper = new IdsWrapper();
                idsWrapper.Ids = vacancies.Select(x => x.IdHH.ToString()).ToList<string>();

                return idsWrapper;
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения данных в БД. {ex.Message}");
            }

        }


        //это попытка реализовать копирование всех полей, как они представлены в json. 
        //Но так как все равно требуется большая обработка классов, полученных из http://json2csharp.com 
        // я решила закомментарить этот вариант и реализовать загрузку в методе SaveVacanciesToDB

        public List<VacanciesViewModel> GetVacanciesViewModelFromAllDB(int count, SearchVacanciesViewModel searchModel)
        {
            try
            {

                /*  List<VacanciesViewModel> listOfVacanciesViewModel = new List<VacanciesViewModel>();

                  var query = db.VacancieFromApiHHs
                           .Where(a => a.description.ToLower().Contains(searchModel.SearchText) || a.name.ToLower().Contains(searchModel.SearchText))
                           .Take(count)
                           .OrderByDescending(x => x.created_at);

                  var result = query.Select(y => new VacanciesViewModel
                  {
                      Id = y.id,
                      Name = y.name,
                      Salary = (y.salary != null) ? y.salary.to != null ? $"от {y.salary.from} до {y.salary.to}  { y.salary.currency}" : $"{y.salary.from} {y.salary.currency}" : "",
                      OrganisationName = GetRightStringFromAnyString(y.employer.name),
                      Description = y.description,
                      Contact = y.contacts.name,
                      PhoneNumber = (y.contacts != null) ? $"+{y.contacts.phones[0].country} ({y.contacts.phones[0].city}) - {y.contacts.phones[0].number}" : "",
                      Employment = (y.employment != null) ? y.employment.name : ""
                  });
                  */
                return null;// result.ToList<VacanciesViewModel>();

            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка формирования списка данных из HH. {ex.Message}");
            }

        }


        public void SaveVacanciesToDBAllFields(List<string> ids)
        {

            try
            {
                /* List<VacancieFromApiHH> vacancieFromApiHHs = new List<VacancieFromApiHH>();
                 foreach (var item in ids)
                 {
                     var entity = db.VacancieFromApiHHs.FirstOrDefault(x => x.id == item);
                     if (entity == null)
                     {
                         VacancieFromApiHH vacancieFromApiHH = GetdataFromApi<VacancieFromApiHH>("https://api.hh.ru/vacancies/" + item);
                         vacancieFromApiHHs.Add(vacancieFromApiHH);
                     }
                 };
                 if (vacancieFromApiHHs.Count > 0)
                 {
                     db.VacancieFromApiHHs.AddRange(vacancieFromApiHHs);
                     db.SaveChanges();
                 };
                 */
            }
            catch (Exception ex)
            {
                throw new Exception($"Ошибка сохранения данных в БД. {ex.Message}");
            }

        }

    }
}



