using System;
using System.Collections.Generic;
using System.Text;

namespace Vacancies.DataModel.Entities
{ // на сайте http://json2csharp.com запустить https://api.hh.ru/vacancies/id какой нибудь вакансии и потом значимые типы сделать Nullable на всякий случай
    public class BillingType
    {
        public string id { get; set; }
        public string name { get; set; }
    }



    public class Site
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Experience
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Schedule
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Employment
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Specialization
    {
        public string id { get; set; }
        public string name { get; set; }
        public string profarea_id { get; set; }
        public string profarea_name { get; set; }
    }




    public class VacancieRootObject
    {
        public string id { get; set; }
        public bool? premium { get; set; }
        public BillingType billing_type { get; set; }
        public List<object> relations { get; set; }
        public string name { get; set; }
        public object insider_interview { get; set; }
        public bool? response_letter_required { get; set; }
        public Area area { get; set; }
        public Salary salary { get; set; }
        public Type type { get; set; }
        public object address { get; set; }
        public bool? allow_messages { get; set; }
        public Site site { get; set; }
        public Experience experience { get; set; }
        public Schedule schedule { get; set; }
        public Employment employment { get; set; }
        public object department { get; set; }
        public object contacts { get; set; }
        public string description { get; set; }
        public object branded_description { get; set; }
        public List<object> key_skills { get; set; }
        public bool? accept_handicapped { get; set; }
        public bool? accept_kids { get; set; }
        public bool? archived { get; set; }
        public object response_url { get; set; }
        public List<Specialization> specializations { get; set; }
        public object code { get; set; }
        public bool? hidden { get; set; }
        public bool? quick_responses_allowed { get; set; }
        public List<object> driver_license_types { get; set; }
        public bool? accept_incomplete_resumes { get; set; }
        public Employer employer { get; set; }
        public DateTime published_at { get; set; }
        public DateTime created_at { get; set; }
        public object negotiations_url { get; set; }
        public object suitable_resumes_url { get; set; }
        public string apply_alternate_url { get; set; }
        public bool? has_test { get; set; }
        public object test { get; set; }
        public string alternate_url { get; set; }
    }
}
