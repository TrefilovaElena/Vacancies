using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vacancies.DataModel.Entities
{


    public class VacanciesSalary
    {
        public int? from { get; set; }
        public int? to { get; set; }
        public string currency { get; set; }
        public bool? gross { get; set; }
    }
    
    public class VacanciesEmployer
    {
        [Required]
        public int VacanciesId { get; set; }
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string alternate_url { get; set; }
        public LogoUrls logo_urls { get; set; }
        public string vacancies_url { get; set; }
        public bool trusted { get; set; }
    }

    public class VacanciesSnippet
    {
        [Required]
        public int Id { get; set; }
        public string requirement { get; set; }
        public string responsibility { get; set; }
    }

    public class VacanciesPhone
    {
        [Required]
        public int Id { get; set; }
        public string comment { get; set; }
        public string city { get; set; }
        public string number { get; set; }
        public string country { get; set; }
    }

    public class VacanciesContacts
    {
        [Required]
        public int Id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public List<Phone> phones { get; set; }
    }

    public class VacanciesItem
    {
        [Required]
        public int Id { get; set; }
        public string id { get; set; }
        public bool premium { get; set; }
        public string name { get; set; }
        public Department department { get; set; }
        public bool has_test { get; set; }
        public bool response_letter_required { get; set; }
        public Area area { get; set; }
        public Salary salary { get; set; }
        public Type type { get; set; }
        public Address address { get; set; }
        public object response_url { get; set; }
        public object sort_point_distance { get; set; }
        public Employer employer { get; set; }
        public DateTime published_at { get; set; }
        public DateTime created_at { get; set; }
        public bool archived { get; set; }
        public string apply_alternate_url { get; set; }
        public object insider_interview { get; set; }
        public string url { get; set; }
        public string alternate_url { get; set; }
        public List<object> relations { get; set; }
        public Snippet snippet { get; set; }
        public Contacts contacts { get; set; }
    }

    
}
