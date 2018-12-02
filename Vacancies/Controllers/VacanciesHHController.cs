using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacancies.Services;
using Vacancies.ViewModels;

namespace Vacancies.Controllers
{
    [Route("api/VacanciesHH")]
    public class VacanciesHHController : Controller
    {

        private IVacanciesHHService _vacanciesHHService;


        public VacanciesHHController(IVacanciesHHService vacanciesHHService)
        {
            _vacanciesHHService = vacanciesHHService;
        }

        [HttpGet]
        public IActionResult GetVacancies(string searchText)
        {
            try
            {
                SearchVacanciesViewModel searchmodel = new SearchVacanciesViewModel();
                searchmodel.SearchText = searchText;
                List<VacanciesViewModel> model= _vacanciesHHService.GetVacanciesViewModel(50, searchmodel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

     
        [HttpPatch]
        public IActionResult SaveVacancies([FromBody] IdsWrapper ids)
        {
            try
            {
               IdsWrapper updatedRecords = _vacanciesHHService.SaveVacanciesToDB(ids.Ids);
               return Ok(updatedRecords);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
       
    }
   
}