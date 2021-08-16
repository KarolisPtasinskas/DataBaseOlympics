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
        private SortingFilteringService _sortingFilteringService;

        public ParticipantsService(SqlConnection connection, AthleteDbService athleteService, CountryDBService countryDBService, SportDBService sportDBService, SortingFilteringService sortingFilteringService)
        {
            _connection = connection;
            _athleteService = athleteService;
            _countryDBService = countryDBService;
            _sportDBService = sportDBService;
            _sortingFilteringService = sortingFilteringService;
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
        //filtering and Sorting
        //
        ////

        public ParticipantsModel SortedAthletes(ParticipantsModel participant)
        {
            switch (participant.OrderBy)
            {
                case "FirstName":
                    participant = _sortingFilteringService.SortAthletesByName(participant);
                    break;

                case "LastName":
                    participant = _sortingFilteringService.SortAthletesByLastName(participant);
                    break;

                case "Country":
                    participant = _sortingFilteringService.SortAthletesByCountry(participant);
                    break;
            }

            return participant;
        }


        public ParticipantsModel FilterAthletes(ParticipantsModel sendParticipant)
        {
            ParticipantsModel participant = new()

            {
                AllAthletes = _sortingFilteringService.FilteredAthletes(sendParticipant),
                AllCountries = _countryDBService.GetAllCountries(),
                AllSports = _sportDBService.GetAllSports()
            };

            return participant;
        }

    }
}
