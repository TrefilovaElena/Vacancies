using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Vacancies.DataModel.Entities;
using Vacancies.ViewModels;

namespace Vacancies.Services
{
    public class ParsingVacanciesService : IParsingVacanciesService
    {
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

        public List<VacanciesViewModel> GetVacanciesViewModel(int amount, SearchVacanciesViewModel searchModel)
        {
            try
            {
                int count = 0;
                List<VacanciesViewModel> listOfVacanciesViewModel = new List<VacanciesViewModel>();

                while (count < amount)
                {
                    RootObject itemsFromApi = GetdataFromApi<RootObject>("https://api.hh.ru/vacancies/" + FormSearchString(searchModel));

                    foreach (var item in itemsFromApi.items)
                    {
                        if (!item.archived)
                        {
                            VacanciesViewModel vacanciesViewModel = new VacanciesViewModel();
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
                                if (item.contacts != null)
                                {
                                    foreach (var phone in item.contacts.phones)
                                    {
                                        vacanciesViewModel.PhoneNumber = $"+{phone.country} ({phone.city}) - {phone.number}";
                                    }
                                }
                            }
                            else
                            {
                                vacanciesViewModel.Contact = ""; vacanciesViewModel.PhoneNumber = "";
                            };

                            if (item.snippet != null)
                                vacanciesViewModel.Description = $"{GetRightStringFromAnyString(item.snippet.responsibility)} {GetRightStringFromAnyString(item.snippet.responsibility)}";
                            else
                                vacanciesViewModel.Description = "";


                            //вызывается только для employment, так как все другие поля есть в основном списке.
                            VacancieRootObject vacancieFromApi = GetdataFromApi<VacancieRootObject>("https://api.hh.ru/vacancies/" + item.id);
                            if (vacancieFromApi.employment != null)
                            {
                                vacanciesViewModel.Employment = GetRightStringFromAnyString(vacancieFromApi.employment.name);
                            }
                            else vacanciesViewModel.Employment = "";
                            //-------------


                            listOfVacanciesViewModel.Add(vacanciesViewModel);
                            count++;
                        }
                        if (count == amount) return listOfVacanciesViewModel;
                    }
                }
                return listOfVacanciesViewModel;

            }
            catch (Exception ex)
            {
                return null;
            }

        }

    }
}



