using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Vacancies.DataModel.Entities
{
    public class Vacancie
    {
        public int Id { get; set; }
        public string IdHH { get; set; }
        public string Name { get; set; }
        public string Salary { get; set; }
        public string OrganisationName { get; set; }
        public string Contact { get; set; }
        public string PhoneNumber { get; set; }
        public string Employment { get; set; }
        public string Description { get; set; }
        public DateTime Created_at { get; set; }
    }
}
