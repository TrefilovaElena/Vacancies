using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Vacancies.DataAccess;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
    public class ParsingVacanciesService : IParsingVacanciesService



    {
        private IMapper _mapper;
        VacanciesContext db;

        public ParsingVacanciesService(VacanciesContext context, IMapper mapper)
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
                return default(T);
            }
        }
        private string FormSearchString(SearchVacanciesViewModel searchModel)
        {

            if (searchModel != null)
            {
                string searchstring = "";
                if (searchModel.SearchText != null) { searchstring = searchModel.SearchText == "" ? "" : $"?text={searchModel.SearchText}"; }
                return searchstring;
            }
            else return "";
        }
        private string GetRightStringFromAnyString(string foo)
        {
            return foo != null ? foo : "";
        }

        public List<VacanciesViewModel> GetVacanciesViewModel(int number, SearchVacanciesViewModel searchModel)
        {
            try
            {
                int count = 0;
                List<VacanciesViewModel> listOfVacanciesViewModel = new List<VacanciesViewModel>();


                while (count < number)
                {
                    RootObject itemsFromApi = GetdataFromApi<RootObject>("https://api.hh.ru/vacancies/" + FormSearchString(searchModel));

                    foreach (var item in itemsFromApi.items)
                    {
                        if ((!item.archived) && (!(listOfVacanciesViewModel.Any(x => x.IdHH == item.id)))) //убираются дублирующие записи с одинаковым Id (можно будет и все поля добавить)
                        {
                            
                                VacanciesViewModel vacanciesViewModel = new VacanciesViewModel();
                                vacanciesViewModel.IdHH = item.id;
                                vacanciesViewModel.Name = item.name;
                                if (item.salary != null)
                                {
                                    vacanciesViewModel.Salary = item.salary.to != null ? $"от {item.salary.from} до {item.salary.to}  { item.salary.currency}" : $"{item.salary.from} { item.salary.currency}";
                                }
                                else
                                    vacanciesViewModel.Salary = "";
                                vacanciesViewModel.OrganisationName = item.employer.name;
                                if (item.contacts != null)
                                {
                                    vacanciesViewModel.Contact = item.contacts.name;
                                    foreach (var phone in item.contacts.phones)
                                    {
                                        vacanciesViewModel.PhoneNumber = $"+{phone.country} ({phone.city}) - {phone.number}";
                                    }
                                }
                                else
                                {
                                    vacanciesViewModel.Contact = ""; vacanciesViewModel.PhoneNumber = "";
                                };
                                // может быть достаточно snippet вместо description. На всякий случай, сделала два варианта, 1 закоментарила

                                /*         if (item.snippet != null)
                                             vacanciesViewModel.Description = $"{GetRightStringFromAnyString(item.snippet.responsibility)} {GetRightStringFromAnyString(item.snippet.responsibility)}";
                                         else
                                             vacanciesViewModel.Description = "";
             */

                                //вызывается только для employment, так как все другие поля есть в основном списке.
                                VacancieFromApiHH vacancieFromApiHH = GetdataFromApi<VacancieFromApiHH>("https://api.hh.ru/vacancies/" + item.id);
                                if (vacancieFromApiHH.employment != null)
                                {
                                    vacanciesViewModel.Employment = GetRightStringFromAnyString(vacancieFromApiHH.employment.name);
                                }
                                else vacanciesViewModel.Employment = "";
                                vacanciesViewModel.Description = GetRightStringFromAnyString(vacancieFromApiHH.description);
                                //-------------


                                listOfVacanciesViewModel.Add(vacanciesViewModel);
                                count++;
                        }
                        if (count == number) return listOfVacanciesViewModel;
                    }
                }
                return listOfVacanciesViewModel;

            }
            catch (Exception ex)
            {
                return null;
            }

        }




        public void SaveVacanciesToDB(List<string> ids)
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
                            Vacancie vacancie = new Vacancie();
                            vacancie.IdHH = vacancieFromApiHH.id;
                            vacancie.Name = vacancieFromApiHH.name;
                            vacancie.OrganisationName = GetRightStringFromAnyString(vacancieFromApiHH.employer.name);
                            vacancie.PhoneNumber = (vacancieFromApiHH.contacts != null) ? $"+{vacancieFromApiHH.contacts.phones[0].country} ({vacancieFromApiHH.contacts.phones[0].city}) - {vacancieFromApiHH.contacts.phones[0].number}" : "";
                            vacancie.Salary = (vacancieFromApiHH.salary != null) ? vacancieFromApiHH.salary.to != null ? $"от {vacancieFromApiHH.salary.from} до {vacancieFromApiHH.salary.to}  { vacancieFromApiHH.salary.currency}" : $"{vacancieFromApiHH.salary.from} {vacancieFromApiHH.salary.currency}" : "";
                            vacancie.Description = vacancieFromApiHH.description;
                            vacancie.Created_at = vacancieFromApiHH.created_at;
                            vacancie.Employment = (vacancieFromApiHH.employment != null) ? vacancieFromApiHH.employment.name:"";
                            vacancie.Contact = (vacancieFromApiHH.contacts != null) ? vacancieFromApiHH.contacts.name : "";
                            vacancies.Add(vacancie);
                        }
                    }
                };
                if (vacancies.Count > 0)
                {
                    db.Vacancies.AddRange(vacancies);
                    db.SaveChanges();
                };
            }
            catch (Exception ex)
            {
                return;
            }

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
                return;
            }

        }


        public List<VacanciesViewModel> GetVacanciesViewModelFromDB(int count, SearchVacanciesViewModel searchModel)
        {
            try
            {
                var query = db.Vacancies
                         .Where(a => a.Description.ToLower().Contains(searchModel.SearchText) || a.Name.ToLower().Contains(searchModel.SearchText))
                         .Take(count)
                         .OrderByDescending(x => x.Created_at);

                var result = query.Select(y => new VacanciesViewModel
                {
                    Id = y.Id,
                    IdHH = y.IdHH,
                    Name = y.Name,
                    Salary = y.Salary,
                    OrganisationName = y.OrganisationName,
                    Description = GetRightStringFromAnyString(y.Description),
                    Contact = GetRightStringFromAnyString(y.Contact),
                    PhoneNumber = GetRightStringFromAnyString(y.PhoneNumber),
                    Employment = GetRightStringFromAnyString(y.Employment)
                });
                return result.ToList<VacanciesViewModel>();
            }
            catch (Exception ex)
            {
                return null;
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
                return null;
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
                return;
            }

        }

    }
}



