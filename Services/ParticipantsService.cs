using DataBaseOlympics.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Services
{
    public class ParticipantsService
    {
        private readonly SqlConnection _connection;
        private AthleteDbService _athleteService;
        private CountryDBService _countryDBService;
        private SportDBService _sportDBService;

        public ParticipantsService(SqlConnection connection, AthleteDbService athleteService, CountryDBService countryDBService, SportDBService sportDBService)
        {
            _connection = connection;
            _athleteService = athleteService;
            _countryDBService = countryDBService;
            _sportDBService = sportDBService;
        }

        public ParticipantsModel GetAthlete(int id)
        {
            ParticipantsModel participant = new()
            {
                Athlete = _athleteService.GetAthlete(id),
                AllCountries = _countryDBService.GetAllCountries(),
                AllSports = _sportDBService.GetAllSports()
            };

            return participant;
        }

        public ParticipantsModel ShowAllAthletes()
        {
            ParticipantsModel participant = new()
            {
                AllAthletes = _athleteService.GetAllAthletes(),
                AllCountries = _countryDBService.GetAllCountries(),
                AllSports = _sportDBService.GetAllSports()
            };

            return participant;
        }

        public ParticipantsModel AddParticipant()
        {
            ParticipantsModel participant = new()

            {
                Athlete = new AthleteModel(),
                AllCountries = _countryDBService.GetAllCountries(),
                AllSports = _sportDBService.GetAllSports()
            };

            return participant;
        }

        ////
        //
        //Grouping and Sorting
        //
        ////

        public ParticipantsModel SortedAthletes(ParticipantsModel info)
        {
            ParticipantsModel participant = new()
            {
                AllAthletes = _athleteService.GetAllGroupedAthletes(info),
                AllCountries = _countryDBService.GetAllCountries(),
                AllSports = _sportDBService.GetAllSports()
            };

            return participant;
        }

        
        //public ParticipantsModel FilterAthletes(ParticipantsModel info)
        //{
        //    ParticipantsModel participant = new()
        //    {
        //        AllAthletes = _athleteService.GetFilteredAthletes(info),
        //        AllCountries = _countryDBService.GetAllCountries(),
        //        AllSports = _sportDBService.GetAllSports()
        //    };

        //    return participant;
        //}

    }
}
