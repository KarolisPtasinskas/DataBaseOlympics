using DataBaseOlympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Services
{
    public class SortingFilteringService
    {
        private readonly SqlConnection _connection;

        public SortingFilteringService(SqlConnection connection)
        {
            _connection = connection;
        }


        public ParticipantsModel SortAthletesByName(ParticipantsModel participant)
        {
            participant.AllAthletes = participant.AllAthletes.OrderBy(a => a.FirstName).ToList();

            return participant;
        }

        public ParticipantsModel SortAthletesByLastName(ParticipantsModel participant)
        {
            participant.AllAthletes = participant.AllAthletes.OrderBy(a => a.LastName).ToList();

            return participant;
        }

        public ParticipantsModel SortAthletesByCountry(ParticipantsModel participant)
        {
            participant.AllAthletes = participant.AllAthletes.OrderBy(a => a.CountryName).ToList();

            return participant;
        }
    }
}
