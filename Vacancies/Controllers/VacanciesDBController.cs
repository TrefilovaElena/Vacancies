using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Vacancies.Services;
using Vacancies.ViewModels;

namespace Vacancies.Controllers
{
    [Produces("application/json")]
    [Route("api/VacanciesDB")]
    public class VacanciesDBController : Controller
    {

        private IVacanciesDBService _vacanciesDBService;


        public VacanciesDBController(IVacanciesDBService vacanciesDBService)
        {
            _vacanciesDBService = vacanciesDBService;
        }

        // [HttpGet("{searchText}")]
        [HttpGet]
        public IActionResult GetVacanciesFromDB(string searchText)
        {
            try
            {
              SearchVacanciesViewModel searchmodel = new SearchVacanciesViewModel();
               searchmodel.SearchText = searchText;
               List<VacanciesViewModel> model = _vacanciesDBService.GetVacanciesViewModelFromDB(50, searchmodel);
               return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteVacancie(int id)
        {
            try
            {
                _vacanciesDBService.DeleteVacancieFromDB(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}