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

        //SORTING 

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


        //FILTERING 
        
        public List<AthleteModel> FilteredAthletes(ParticipantsModel participant)
        {
            var countryToFilter = "";
            var sportToFilter = participant.FilterByList[1];


            if (participant.FilterByList[0] != null)
            {
                countryToFilter = $"WHERE [dbo].[Athletes].[Country] = {participant.FilterByList[0]}";
            }


            List<AthleteModel> athletes = new();

            _connection.Open();
            var command = new SqlCommand($"SELECT [Id], [FirstName], [LastName], [Country], (SELECT [CountryName] FROM [dbo].[Countries] WHERE [dbo].[Athletes].[Country] = [dbo].[Countries].[id]) FROM [dbo].[Athletes] {countryToFilter}", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                AthleteModel athlete = new()
                {
                    Id = (int)reader.GetValue(0),
                    FirstName = (string)reader.GetValue(1),
                    LastName = (string)reader.GetValue(2),
                    Country = (int)reader.GetValue(3),
                    CountryName = (string)reader.GetValue(4),
                    Sports = new List<int>(),
                    SportsNames = new List<string>()
                };

                athletes.Add(athlete);
            }
            _connection.Close();

            var allAthletes = AddSportsToAthletes(athletes);

            if (sportToFilter == null)
            {
                return allAthletes;
            }

            allAthletes = allAthletes.Where(a => a.SportsNames.Contains($"{sportToFilter}")).ToList();

            return allAthletes;
        }

        private List<AthleteModel> AddSportsToAthletes(List<AthleteModel> allAthletes)
        {
            _connection.Open();
            var command = new SqlCommand("SELECT [Athlete_id], [Sports_id], [SportName] FROM [dbo].[Athlete_Sports] LEFT JOIN [dbo].[Sports] ON [dbo].[Athlete_Sports].[Sports_id] = [dbo].[Sports].[id]", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                foreach (var athlete in allAthletes)
                {
                    if (athlete.Id == (int)reader.GetValue(0))
                    {
                        athlete.Sports.Add((int)reader.GetValue(1));
                        athlete.SportsNames.Add((string)reader.GetValue(2));
                    }
                }

            }

            _connection.Close();
            return allAthletes;
        }


    }
}
