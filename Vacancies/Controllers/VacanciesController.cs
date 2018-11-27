﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vacancies.Services;
using Vacancies.ViewModels;

namespace Vacancies.Controllers
{
    [Route("api/Vacancies")]
    public class VacanciesController : Controller
    {

        private IParsingVacanciesService _parsingVacanciesService;


        public VacanciesController(IParsingVacanciesService parsingVacanciesService)
        {
            _parsingVacanciesService = parsingVacanciesService;
        }

        [HttpPost("/api/vacancies")]
        public IActionResult GetVacancies([FromBody] SearchVacanciesViewModel searchmodel)
        {
            try
            {
                List<VacanciesViewModel> model= _parsingVacanciesService.GetVacanciesViewModel(50, searchmodel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("/api/vacanciesDB")]
        public IActionResult GetVacanciesFromDB([FromBody] SearchVacanciesViewModel searchmodel)
        {
            try
            {
                List<VacanciesViewModel> model = _parsingVacanciesService.GetVacanciesViewModelFromDB(50, searchmodel);
                return Ok(model);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/api/save")]
        public IActionResult SaveVacancies([FromBody] IdsWrapper ids)
        {
            try
            {
                 _parsingVacanciesService.SaveVacanciesToDB(ids.Ids);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("/api/delete/{id}")]
        public IActionResult DeleteVacancie(int id)
        {
            try
            {
                _parsingVacanciesService.DeleteVacancieFromDB(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
    public class IdsWrapper
    {

       public List<string> Ids { get; set; }
    }
}