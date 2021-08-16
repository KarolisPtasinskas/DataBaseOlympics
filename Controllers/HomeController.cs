using DataBaseOlympics.Models;
using DataBaseOlympics.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Controllers
{
    public class HomeController : Controller
    {
        private ParticipantsService _participantsService;
        private AthleteDbService _athleteService;

        public HomeController(AthleteDbService athleteService, ParticipantsService participantsService)
        {
            _athleteService = athleteService;
            _participantsService = participantsService;
        }

        public IActionResult Index(ParticipantsModel participant)
        {
            if (participant.AllAthletes == null)
            {
                return View(_participantsService.ShowAllAthletes());
            }

            return View(participant);
        }

        public IActionResult NewAthlete()
        {
            return View(_participantsService.AddParticipant());
        }

        [HttpPost]
        public IActionResult AddAthlete(ParticipantsModel participant)
        {
            _athleteService.AddAthlete(participant);
            return RedirectToAction("Index");
        }

        public IActionResult EditAthlete(int id)
        {
            return View(_participantsService.GetAthlete(id));
        }

        [HttpPost]
        public IActionResult EditAthlete(ParticipantsModel participant)
        {
            _athleteService.EditAthlete(participant);
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAthlete(int id)
        {
            _athleteService.DeleteAthlete(id);
            return RedirectToAction ("Index");
        }

        ////
        //
        //Filtering and Sorting
        //
        ////
        
        public IActionResult SortAthletes(ParticipantsModel participant)
        {
            
            ParticipantsModel newParticipant = _participantsService.SortedAthletes(participant);
            return View("Index", newParticipant);
        }
        

        public IActionResult FilterAthletes(ParticipantsModel participant)
        {
            ParticipantsModel newParticipant = _participantsService.FilterAthletes(participant);
            return View("Index", newParticipant);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
