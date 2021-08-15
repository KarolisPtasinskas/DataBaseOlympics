using DataBaseOlympics.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DataBaseOlympics.Services
{
    public class AthleteDbService
    {
        private readonly SqlConnection _connection;

        public AthleteDbService(SqlConnection connection)
        {
            _connection = connection;
        }

        public AthleteModel GetAthlete(int getId)
        {
            AthleteModel athlete = new();

            _connection.Open();
            var command = new SqlCommand($"SELECT [dbo].[Athletes].[Id], [FirstName], [LastName], [Country],[CountryName] FROM[Olympics].[dbo].[Athletes] LEFT JOIN[dbo].[Countries] ON[dbo].[Athletes].[Country] = [dbo].[Countries].[id] WHERE[dbo].[Athletes].[Id] = '{getId}'", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athlete.Id = (int)reader.GetValue(0);
                athlete.FirstName = (string)reader.GetValue(1);
                athlete.LastName = (string)reader.GetValue(2);
                athlete.Country = (int)reader.GetValue(3);
                athlete.CountryName = (string)reader.GetValue(4);
                athlete.Sports = new List<int>();
                athlete.SportsNames = new List<string>();
            }

            _connection.Close();

            AthleteModel completeAthlete = AddSportsToAthlete(athlete);

            return completeAthlete;
        }

        public AthleteModel AddSportsToAthlete(AthleteModel athlete)
        {
            _connection.Open();
            var command = new SqlCommand($"SELECT [Sports_id], [SportName] FROM [dbo].[Athlete_Sports] LEFT JOIN [dbo].[Sports] ON [dbo].[Athlete_Sports].[Sports_id] = [dbo].[Sports].[id] WHERE [dbo].[Athlete_Sports].[Athlete_id] = '{athlete.Id}'", _connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                athlete.Sports.Add((int)reader.GetValue(0));
                athlete.SportsNames.Add((string)reader.GetValue(1));
            }

            return athlete;
        }


        public List<AthleteModel> GetAllAthletes()
        {
            List<AthleteModel> athletes = new();

            _connection.Open();
            var command = new SqlCommand("SELECT [Id], [FirstName], [LastName], [Country], (SELECT [CountryName] FROM [dbo].[Countries] WHERE [dbo].[Athletes].[Country] = [dbo].[Countries].[id]) FROM [dbo].[Athletes]", _connection);
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

            return allAthletes;
        }

        public List<AthleteModel> AddSportsToAthletes(List<AthleteModel> allAthletes)
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

        public void AddAthlete(ParticipantsModel participant)
        {
            _connection.Open();
            var command = new SqlCommand($"INSERT INTO dbo.Athletes (FirstName, LastName, Country) values ('{participant.Athlete.FirstName}', '{participant.Athlete.LastName}', '{participant.Athlete.Country}')", _connection);
            var reader = command.ExecuteReader();

            _connection.Close();

            foreach (var sport in participant.Athlete.Sports)
            {
                _connection.Open();
                var command2 = new SqlCommand($"INSERT INTO [dbo].[Athlete_Sports] ([Athlete_id], [Sports_id]) VALUES ((SELECT [Id] FROM [dbo].[Athletes] WHERE [FirstName] = '{participant.Athlete.FirstName}' AND [LastName] = '{participant.Athlete.LastName}' AND [Country] = {participant.Athlete.Country}), {sport})", _connection);
                var reader2 = command2.ExecuteReader();
                _connection.Close();
            }
            
        }

        
        public void EditAthlete(ParticipantsModel participant)
        {
            _connection.Open();
            var command = new SqlCommand($"UPDATE [dbo].[Athletes] SET [FirstName] = '{participant.Athlete.FirstName}' ,[LastName] = '{participant.Athlete.LastName}' ,[Country] = {participant.Athlete.Country} WHERE[dbo].[Athletes].[Id] = {participant.Athlete.Id}", _connection);
            var reader = command.ExecuteReader();
            _connection.Close();

            _connection.Open();
            var command2 = new SqlCommand($"DELETE FROM [dbo].[Athlete_Sports] WHERE [Athlete_id] = {participant.Athlete.Id}", _connection);
            var reader2 = command2.ExecuteReader();
            _connection.Close();

            foreach (var sport in participant.Athlete.Sports)
            {
                _connection.Open();
                var command3 = new SqlCommand($"INSERT INTO [dbo].[Athlete_Sports] ([Athlete_id], [Sports_id]) VALUES ({participant.Athlete.Id}, {sport})", _connection);
                var reader3 = command3.ExecuteReader();
                _connection.Close();
            }

        }

        
        public void DeleteAthlete(int id)
        {
            _connection.Open();
            var command = new SqlCommand($"DELETE FROM [dbo].[Athlete_Sports] WHERE [Athlete_id] = {id}", _connection);
            var reader = command.ExecuteReader();
            _connection.Close();

            _connection.Open();
            var command2 = new SqlCommand($"DELETE FROM [Olympics].[dbo].[Athletes] WHERE [Id] = {id}", _connection);
            var reader2 = command2.ExecuteReader();
            _connection.Close();
        }

        ////
        //
        //Grouping and Sorting
        //
        ////

        public List<AthleteModel> GetAllGroupedAthletes(ParticipantsModel participant)
        {
            List<AthleteModel> athletes = new();

            _connection.Open();
            var command = new SqlCommand($"SELECT [Id], [FirstName], [LastName], [Country], (SELECT [CountryName] FROM [dbo].[Countries] WHERE ath.Country = [dbo].[Countries].[id]) as 'CountryName' FROM [dbo].[Athletes] as ath ORDER BY [{participant.OrderBy}]", _connection);
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

            return allAthletes;
        }


        
        //public List<AthleteModel> GetFilteredAthletes(ParticipantsModel participant)
        //{


        //    var allAthletes = AddSportsToAthletes(athletes);

        //    return allAthletes;
        //}
    }
}
