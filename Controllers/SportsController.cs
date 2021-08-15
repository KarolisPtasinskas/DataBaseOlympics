using DataBaseOlympics.Models;
using DataBaseOlympics.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Controllers
{
    public class SportsController : Controller
    {
        private SportDBService _sportDBService;

        public SportsController(SportDBService sportDBService)
        {
            _sportDBService = sportDBService;
        }
        public IActionResult Index()
        {
            return View(_sportDBService.GetAllSports());
        }

        public IActionResult NewSport()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddSport(SportModel sport)
        {
            _sportDBService.AddSport(sport);
            return View("Index", _sportDBService.GetAllSports());
        }
    }
}
