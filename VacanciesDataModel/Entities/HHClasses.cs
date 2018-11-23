using System;
using System.Collections.Generic;
using System.Text;

namespace Vacancies.DataModel.Entities
{ // на сайте http://json2csharp.com запустить https://api.hh.ru/vacancies и потом значимые типы сделать Nullable на всякий случай, кроме item.archived
    public class Department
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Area
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Salary
    {
        public int? from { get; set; }
        public int? to { get; set; }
        public string currency { get; set; }
        public bool? gross { get; set; }
    }

    public class Type
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class Metro
    {
        public string station_name { get; set; }
        public string line_name { get; set; }
        public string station_id { get; set; }
        public string line_id { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
    }

    public class Address
    {
        public string city { get; set; }
        public string street { get; set; }
        public string building { get; set; }
        public object description { get; set; }
        public double? lat { get; set; }
        public double? lng { get; set; }
        public string raw { get; set; }
        public Metro metro { get; set; }
        public List<object> metro_stations { get; set; }
        public string id { get; set; }
    }

    public class LogoUrls
    {
        public string __invalid_name__90 { get; set; }
        public string original { get; set; }
        public string __invalid_name__240 { get; set; }
    }

    public class Employer
    {
        public string id { get; set; }
        public string name { get; set; }
        public string url { get; set; }
        public string alternate_url { get; set; }
        public LogoUrls logo_urls { get; set; }
        public string vacancies_url { get; set; }
        public bool? trusted { get; set; }
    }

    public class Snippet
    {
        public string requirement { get; set; }
        public string responsibility { get; set; }
    }

    public class Phone
    {
        public string comment { get; set; }
        public string city { get; set; }
        public string number { get; set; }
        public string country { get; set; }
    }

    public class Contacts
    {
        public string name { get; set; }
        public string email { get; set; }
        public List<Phone> phones { get; set; }
    }

    public class Item
    {
        public string id { get; set; }
        public bool? premium { get; set; }
        public string name { get; set; } 
        public Department department { get; set; }
        public bool? has_test { get; set; }
        public bool? response_letter_required { get; set; }
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

    public class RootObject
    {
        public List<Item> items { get; set; }
        public int found { get; set; }
        public int pages { get; set; }
        public int per_page { get; set; }
        public int page { get; set; }
        public object clusters { get; set; }
        public object arguments { get; set; }
        public string alternate_url { get; set; }
    }
}


