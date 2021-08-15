using DataBaseOlympics.Models;
using DataBaseOlympics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Controllers
{
    public class CountriesController : Controller
    {
        private CountryDBService _countryDBService;

        public CountriesController(CountryDBService countryDBService)
        {
            _countryDBService = countryDBService;
        }
        public IActionResult Index()
        {
            return View(_countryDBService.GetAllCountries());
        }

        public IActionResult NewCountry()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCountry(CountryModel country)
        {
            _countryDBService.AddCountry(country);
            return View("Index", _countryDBService.GetAllCountries());
        }
    }
}
